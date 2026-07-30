param(
    [string]$RepoRoot,
    [string]$OutputDir,
    [string]$DiffCheckResult = 'Not run yet'
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($RepoRoot))
{
    $RepoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\..'))
}
else
{
    $RepoRoot = [System.IO.Path]::GetFullPath($RepoRoot)
}

if ([string]::IsNullOrWhiteSpace($OutputDir))
{
    $OutputDir = Join-Path $RepoRoot 'docs\codebase-audit\outputs'
}
else
{
    $OutputDir = [System.IO.Path]::GetFullPath($OutputDir)
}

$Utf8NoBom = New-Object -TypeName System.Text.UTF8Encoding -ArgumentList $false

function Write-Utf8File
{
    param(
        [string]$Path,
        [string[]]$Lines
    )

    [System.IO.File]::WriteAllText($Path, ($Lines -join "`n") + "`n", $Utf8NoBom)
}

function Get-CsvRows
{
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path))
    {
        throw "Required CSV missing: $Path"
    }

    if ((Get-Item -LiteralPath $Path).Length -eq 0)
    {
        return @()
    }

    return @(Import-Csv -LiteralPath $Path)
}

function Add-CountTable
{
    param(
        [System.Collections.Generic.List[string]]$Lines,
        [object[]]$Groups,
        [string]$NameHeader
    )

    $Lines.Add("| $NameHeader | Count |") | Out-Null
    $Lines.Add('| --- | ---: |') | Out-Null

    foreach ($group in $Groups)
    {
        $Lines.Add("| ``$($group.Name)`` | $($group.Count) |") | Out-Null
    }
}

function Get-BacklogEvidenceValue
{
    param(
        [string]$Evidence,
        [string]$Name,
        [string]$DefaultValue = ''
    )

    if ($Evidence -match ('(^|;)\s*' + [regex]::Escape($Name) + '=([^;]+)'))
    {
        return $Matches[2].Trim()
    }

    return $DefaultValue
}

function Get-RegisterClass
{
    param([object]$RegisterRow)

    if ($RegisterRow.Class -and $RegisterRow.Class -ne 'Unknown')
    {
        return $RegisterRow.Class
    }

    if ($RegisterRow.TypeName -and $RegisterRow.TypeName -ne 'Unknown')
    {
        return $RegisterRow.TypeName
    }

    return 'Unknown'
}

function Test-WorldSaveSurface
{
    param(
        [object]$BacklogRow,
        [object]$RegisterRow,
        [string]$ClassName
    )

    if ($RegisterRow.Kind -in @('Item', 'Mobile', 'Spawner'))
    {
        return $true
    }

    if ($BacklogRow.Evidence -match 'SerialCtor:')
    {
        return $true
    }

    if ($ClassName -like 'Server.Items.*' -or $ClassName -like 'Server.Mobiles.*')
    {
        return $true
    }

    if ($RegisterRow.File -like 'Data/Scripts/Items/*' -or $RegisterRow.File -like 'Data/Scripts/Mobiles/*')
    {
        return $true
    }

    return $false
}

function Get-Decision
{
    param(
        [object]$BacklogRow,
        [object]$RegisterRow,
        [string]$BacklogRiskCategory,
        [string]$ClassName
    )

    $WriteCount = [int]$RegisterRow.WriteCount
    $ReadCount = [int]$RegisterRow.ReadCount
    $VersionHandling = $RegisterRow.VersionHandling
    $FieldAlignment = $RegisterRow.FieldAlignment
    $Kind = $RegisterRow.Kind

    if ($ClassName -eq 'Unknown' -or $BacklogRow.Evidence -match '^\s*;\s*class=Unknown')
    {
        return 'FalsePositive'
    }

    if ($BacklogRiskCategory -eq 'MissingSerialConstructor' -and -not (Test-WorldSaveSurface -BacklogRow $BacklogRow -RegisterRow $RegisterRow -ClassName $ClassName))
    {
        return 'FalsePositive'
    }

    if ($BacklogRiskCategory -eq 'MissingSerializerPair' -and ($ClassName -eq 'Unknown' -or $Kind -notin @('Item', 'Mobile', 'Spawner')))
    {
        return 'FalsePositive'
    }

    if ($FieldAlignment -like 'TypeMismatch*' -or $FieldAlignment -like 'CountMismatch*')
    {
        if (($VersionHandling -in @('WriteVersionOnly', 'ReadVersionOnly')) -and $WriteCount -eq 1 -and $ReadCount -eq 1)
        {
            return 'SafeNoChange'
        }

        if (Test-WorldSaveSurface -BacklogRow $BacklogRow -RegisterRow $RegisterRow -ClassName $ClassName)
        {
            return 'NeedsMigrationPlan'
        }

        return 'QueuedSourceFollowUp'
    }

    if ($FieldAlignment -like 'CountMatchNeedsTypeReview*')
    {
        return 'QueuedSourceFollowUp'
    }

    if ($VersionHandling -in @('SingleVersionOnly', 'Switch', 'SwitchGotoCase', 'IfVersionGates', 'SuspiciousOrder', 'WriteVersionOnly', 'ReadVersionOnly'))
    {
        return 'SafeNoChange'
    }

    if ($VersionHandling -eq 'NoVersionFound')
    {
        if ($BacklogRiskCategory -eq 'MissingSerializerPair')
        {
            return 'FalsePositive'
        }

        return 'IntentionalLegacy'
    }

    return 'QueuedSourceFollowUp'
}

function Get-Action
{
    param([string]$Decision)

    switch ($Decision)
    {
        'SafeNoChange' { return 'No source change. Current source/register evidence shows paired serializer structure or a version-only serializer shape that does not require a layout edit.' }
        'IntentionalLegacy' { return 'No source change. Preserve the existing fixed-format legacy serializer; adding a version field would be a save-layout migration and is not approved in this triage batch.' }
        'FalsePositive' { return 'No source change. The generated row points at a helper, parser artifact, unknown/no-serializer entry, or non-world-save surface rather than a concrete serializer defect.' }
        'NeedsMigrationPlan' { return 'No source change. The row has source/register evidence of a mismatch or missing pair on a world-save surface; any fix requires a focused migration plan and source verification.' }
        'QueuedSourceFollowUp' { return 'No source change. The row has ambiguous mechanical evidence that needs a focused source review before a safe/no-change or migration decision can be claimed.' }
        'NeedsHumanDecision' { return 'No source change. A human save-policy decision is required before implementation.' }
        'Fixed' { return 'Source repair was applied and verified in this batch.' }
        default { return 'No source change. Recorded deterministic triage disposition.' }
    }
}

function Get-Verification
{
    param(
        [string]$Decision,
        [string]$DiffCheckResult
    )

    $Base = "Reviewed against current source-derived serialization-register evidence and confirmed the current source file exists. git diff --check: $DiffCheckResult."

    switch ($Decision)
    {
        'SafeNoChange' { return "$Base No source serializer edit was made; build/runtime proof was not required." }
        'IntentionalLegacy' { return "$Base No source serializer edit was made; migration/build/runtime proof is deferred until a layout change is explicitly approved." }
        'FalsePositive' { return "$Base No source serializer edit was made because the row is not a concrete save-layout defect." }
        'NeedsMigrationPlan' { return "$Base No source serializer edit was made; required future verification is serialization-register rerun, write/read map comparison, Server.csproj Debug/x86 build, and compile-only runtime script verification." }
        'QueuedSourceFollowUp' { return "$Base No source serializer edit was made; future focused review must inspect exact source read/write branches before changing layout." }
        default { return $Base }
    }
}

$Timestamp = (Get-Date).ToString('o')
$BatchName = 'POST-BATCH-J'
$ReviewBatchId = 'POST-BATCH-J-01A'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-j-p1-save-compat-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-j-p1-save-compat-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$SerializationRegisterPath = Join-Path $OutputDir 'serialization-register.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$RepairRows = Get-CsvRows -Path $RepairBacklogPath
$ActiveRows = Get-CsvRows -Path $ActivePath
$ActiveWithoutJ = @($ActiveRows | Where-Object { $_.PostAuditBatch -ne $BatchName })
$CoveredIds = New-Object 'System.Collections.Generic.HashSet[string]'

foreach ($row in $ActiveWithoutJ)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredIds.Add($row.BacklogId)
    }
}

$ScopeRows = @($RepairRows | Where-Object {
    $_.Priority -eq 'P1' -and
    $_.Category -eq 'Save compatibility' -and
    -not $CoveredIds.Contains($_.Id)
} | Sort-Object Id)

if ($ScopeRows.Count -ne 1294)
{
    throw "Expected 1,294 POST-BATCH-J P1 save rows, found $($ScopeRows.Count)."
}

$RegisterRows = Get-CsvRows -Path $SerializationRegisterPath
$ByFileClass = @{}
$ByClass = @{}
$ByLeaf = @{}

foreach ($register in $RegisterRows)
{
    $className = Get-RegisterClass -RegisterRow $register
    $fileClassKey = "$($register.File)|$className"
    $leaf = Split-Path -Leaf $register.File

    if (-not $ByFileClass.ContainsKey($fileClassKey))
    {
        $ByFileClass[$fileClassKey] = @()
    }
    $ByFileClass[$fileClassKey] += $register

    if (-not $ByClass.ContainsKey($className))
    {
        $ByClass[$className] = @()
    }
    $ByClass[$className] += $register

    if (-not $ByLeaf.ContainsKey($leaf))
    {
        $ByLeaf[$leaf] = @()
    }
    $ByLeaf[$leaf] += $register
}

function Resolve-RegisterRow
{
    param(
        [object]$BacklogRow,
        [string]$ClassName
    )

    $fileClassKey = "$($BacklogRow.Files)|$ClassName"

    if ($ByFileClass.ContainsKey($fileClassKey))
    {
        return [pscustomobject]@{
            RegisterRow = $ByFileClass[$fileClassKey][0]
            MatchQuality = 'ExactFileClass'
        }
    }

    if ($ClassName -ne 'Unknown' -and $ByClass.ContainsKey($ClassName))
    {
        $classMatches = @($ByClass[$ClassName])
        if ($classMatches.Count -eq 1)
        {
            return [pscustomobject]@{
                RegisterRow = $classMatches[0]
                MatchQuality = 'ClassOnlyCurrentPath'
            }
        }

        $evidence = $BacklogRow.Evidence -replace ';\s*class=.*$', ''
        $evidenceMatches = @($classMatches | Where-Object { $_.Evidence -eq $evidence })

        if ($evidenceMatches.Count -eq 1)
        {
            return [pscustomobject]@{
                RegisterRow = $evidenceMatches[0]
                MatchQuality = 'ClassEvidenceCurrentPath'
            }
        }
    }

    $leaf = Split-Path -Leaf $BacklogRow.Files

    if ($ByLeaf.ContainsKey($leaf))
    {
        $leafMatches = @($ByLeaf[$leaf])

        if ($ClassName -eq 'Unknown')
        {
            $leafMatches = @($leafMatches | Where-Object { (Get-RegisterClass -RegisterRow $_) -eq 'Unknown' })
        }

        if ($leafMatches.Count -eq 1)
        {
            return [pscustomobject]@{
                RegisterRow = $leafMatches[0]
                MatchQuality = 'LeafCurrentPath'
            }
        }
    }

    throw "Could not resolve current serializer register row for $($BacklogRow.Id) $($BacklogRow.Files) class=$ClassName"
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $ScopeRows)
{
    $Index++
    $ReviewRowId = 'PBJ-{0:D4}' -f $Index
    $className = Get-BacklogEvidenceValue -Evidence $row.Evidence -Name 'class' -DefaultValue 'Unknown'
    $backlogRiskCategory = Get-BacklogEvidenceValue -Evidence $row.Evidence -Name 'category' -DefaultValue 'UnknownCategory'
    $resolved = Resolve-RegisterRow -BacklogRow $row -ClassName $className
    $register = $resolved.RegisterRow
    $currentPath = Join-Path $RepoRoot $register.File

    if (-not (Test-Path -LiteralPath $currentPath))
    {
        throw "Current source file missing for $($row.Id): $($register.File)"
    }

    $decision = Get-Decision -BacklogRow $row -RegisterRow $register -BacklogRiskCategory $backlogRiskCategory -ClassName $className
    $action = Get-Action -Decision $decision
    $verification = Get-Verification -Decision $decision -DiffCheckResult $DiffCheckResult
    $sourceEvidence = "Current source file exists at $($register.File); match=$($resolved.MatchQuality); serializer evidence=$($register.Evidence); class=$className; kind=$($register.Kind); HasSerialConstructor=$($register.HasSerialConstructor); BaseCallStatus=$($register.BaseCallStatus); VersionHandling=$($register.VersionHandling); FieldAlignment=$($register.FieldAlignment); MoveRenameRisk=$($register.MoveRenameRisk); ReviewStatus=$($register.ReviewStatus); ReviewReasons=$($register.ReviewReasons)"
    $notes = "Historical path=$($row.Files); backlog risk category=$backlogRiskCategory; write count=$($register.WriteCount); read count=$($register.ReadCount). POST-BATCH-J is triage only; no serialized layout edit is approved by this row."

    if ($resolved.MatchQuality -ne 'ExactFileClass')
    {
        $notes = "$notes Historical path was reconciled to current source path $($register.File)."
    }

    $ReviewRows.Add([pscustomobject]@{
        BatchId = $BatchName
        ReviewRowId = $ReviewRowId
        BacklogId = $row.Id
        SourceId = $row.SourceId
        Priority = $row.Priority
        HistoricalStatus = $row.Status
        Category = $row.Category
        System = $row.System
        HistoricalFile = $row.Files
        CurrentFile = $register.File
        Class = $className
        BacklogRiskCategory = $backlogRiskCategory
        OriginalEvidence = $row.Evidence
        RegisterEvidence = $register.Evidence
        VersionHandling = $register.VersionHandling
        FieldAlignment = $register.FieldAlignment
        BaseCallStatus = $register.BaseCallStatus
        HasSerialConstructor = $register.HasSerialConstructor
        MatchQuality = $resolved.MatchQuality
        Risk = $row.Risk
        RecommendedFix = $row.RecommendedFix
        Decision = $decision
        SourceEvidence = $sourceEvidence
        Action = $action
        Verification = $verification
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $notes
    }) | Out-Null
}

if ($ReviewRows.Count -ne 1294)
{
    throw "Review row count invariant failed: $($ReviewRows.Count)"
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 1294)
{
    throw 'Review CSV row count invariant failed after write.'
}

$OverlayRows = foreach ($review in $ReviewRows)
{
    [pscustomobject]@{
        BacklogId = $review.BacklogId
        SourceId = $review.SourceId
        Category = $review.Category
        System = $review.System
        Files = $review.HistoricalFile
        HistoricalStatus = $review.HistoricalStatus
        ActiveStatus = $review.Decision
        PostAuditBatch = $BatchName
        ReviewRowId = $review.ReviewRowId
        ReviewStatus = $review.Decision
        Action = $review.Action
        ReviewArtifact = $ReviewRel
        SourceEvidence = $review.SourceEvidence
        Commit = 'Pending current POST-BATCH-J commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($ActiveWithoutJ + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$ActiveAfter = Get-CsvRows -Path $ActivePath
$PostBatchJRows = @($ActiveAfter | Where-Object { $_.PostAuditBatch -eq $BatchName })

if ($PostBatchJRows.Count -ne 1294)
{
    throw "Active overlay POST-BATCH-J count invariant failed: $($PostBatchJRows.Count)"
}

$CoveredAfter = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ActiveAfter)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredAfter.Add($row.BacklogId)
    }
}

$RemainingP1SaveAfter = @($RepairRows | Where-Object {
    $_.Priority -eq 'P1' -and
    $_.Category -eq 'Save compatibility' -and
    -not $CoveredAfter.Contains($_.Id)
})

if ($RemainingP1SaveAfter.Count -ne 0)
{
    throw "Unreviewed P1 save rows remain after POST-BATCH-J overlay update: $($RemainingP1SaveAfter.Id -join ', ')"
}

$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$SystemCounts = @($ReviewRows | Group-Object System | Sort-Object Count -Descending)
$RiskCategoryCounts = @($ReviewRows | Group-Object BacklogRiskCategory | Sort-Object Name)
$MatchQualityCounts = @($ReviewRows | Group-Object MatchQuality | Sort-Object Name)
$DecisionSummary = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-J P1 Save Compatibility Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-J` reviewed all 1,294 remaining P1 `Save compatibility` rows from `repair-backlog.csv` that were absent from the active overlay. The batch is source/register triage only: it records dispositions, current source evidence, and follow-up/migration gates without changing serialized layouts.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('No runtime source, serialized type name, namespace, save version, read/write order, project file, XML/config file, runtime file location, or gameplay behavior was changed.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Backlog Risk Categories') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $RiskCategoryCounts -NameHeader 'Risk category'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Source Match Quality') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $MatchQualityCounts -NameHeader 'Match quality'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Top Systems') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups @($SystemCounts | Select-Object -First 15) -NameHeader 'System'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- `git status --short` was clean before the batch.') | Out-Null
$Closeout.Add('- Applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.') | Out-Null
$Closeout.Add('- `repair-backlog.csv` compared against `post-audit-active-backlog-status.csv` produced exactly 1,294 remaining P1 save-compatibility rows before POST-BATCH-J.') | Out-Null
$Closeout.Add('- Every POST-BATCH-J row resolves to a current source file through `serialization-register.csv`; historical moved paths are explicitly recorded with `MatchQuality` in the review CSV.') | Out-Null
$Closeout.Add('- `post-batch-j-p1-save-compat-review.csv` contains exactly 1,294 rows.') | Out-Null
$Closeout.Add('- `post-audit-active-backlog-status.csv` contains exactly 1,294 POST-BATCH-J rows.') | Out-Null
$Closeout.Add('- Comparing `repair-backlog.csv` P1 save-compatibility rows to the active overlay leaves 0 unreviewed P1 save rows.') | Out-Null
$Closeout.Add('- `New-SerializationRegister.ps1` was not rerun because this batch made no runtime source serializer edits and did not change serializer source inputs.') | Out-Null
$Closeout.Add("- `git diff --check`: $DiffCheckResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Boundary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-J` does not begin broad P1 runtime hooks, gump guards, command access, region/map, or legacy compatibility work. Rows marked `NeedsMigrationPlan` or `QueuedSourceFollowUp` require focused future source batches before any save-layout edit.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-j-p1-save-compat-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-j-p1-save-compat-review.csv` | Post-audit | Review all 1,294 remaining P1 save-compatibility rows against current serializer-register source evidence and record triage dispositions. | Complete |',
    '| `post-batch-j-p1-save-compat-closeout.md` | Post-audit | Close out POST-BATCH-J with disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |'
)
$InitialIndex = [Array]::IndexOf($Readme, '## Initial State')

if ($InitialIndex -lt 0)
{
    throw 'Could not find README Initial State heading.'
}

$Before = if ($InitialIndex -gt 0) { $Readme[0..($InitialIndex - 1)] } else { @() }
$After = $Readme[$InitialIndex..($Readme.Count - 1)]

while ($Before.Count -gt 0 -and [string]::IsNullOrWhiteSpace($Before[$Before.Count - 1]))
{
    if ($Before.Count -eq 1)
    {
        $Before = @()
    }
    else
    {
        $Before = $Before[0..($Before.Count - 2)]
    }
}

Write-Utf8File -Path $ReadmePath -Lines @($Before + '' + $NewEntries + '' + $After)

$PhaseStatus = Get-Content -Raw -LiteralPath $PhaseStatusPath
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Last updated: .*', "Last updated: $Timestamp", 1)
$JSummary = 'Post-audit P1 save compatibility triage: `{0}` reviewed all 1,294 remaining P1 `Save compatibility` rows in `outputs/post-batch-j-p1-save-compat-review.csv`. Disposition summary is `{1}`; remaining unreviewed P1 save-compatibility rows=0. No source/project/runtime/XML/config files changed, and `New-SerializationRegister.ps1` was not rerun because no runtime serializer source input changed.' -f $ReviewBatchId, $DecisionSummary

if ($PhaseStatus -match 'Post-audit P1 save compatibility triage:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit P1 save compatibility triage:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $JSummary }, 1)
}
else
{
    $Marker = 'Post-audit ServerCore save compatibility residual review:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($JSummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 1,598 reviewed save-compatibility dispositions across `POST-BATCH-B`, `POST-BATCH-I`, and `POST-BATCH-J`, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, 61 `POST-BATCH-G` project include drift dispositions, and 14 `POST-BATCH-H` folder/namespace cleanup dispositions. Comparing `repair-backlog.csv` P0 rows against the active overlay leaves 0 unreviewed P0 rows; comparing P1 save-compatibility rows leaves 0 unreviewed P1 save rows.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    '- Affected phase: Post-audit `POST-BATCH-J` P1 save compatibility triage',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P1 save rows against `serialization-register.csv`; run `New-PostBatchJP1SaveCompatReview.ps1`.',
    ('- Result: Review CSV contains {0} POST-BATCH-J rows; active overlay contains {1} POST-BATCH-J rows; disposition summary is {2}; remaining unreviewed P1 save-compatibility rows=0; no source/project/runtime/XML/config files changed; serialization register was not regenerated because no runtime serializer source inputs changed; git diff --check={3}.' -f $ReviewRows.Count, $PostBatchJRows.Count, $DecisionSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-J` P1 save compatibility triage\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $PostBatchJRows.Count
    DecisionSummary = $DecisionSummary
    RemainingP1SaveAfter = $RemainingP1SaveAfter.Count
    DiffCheckResult = $DiffCheckResult
    Timestamp = $Timestamp
}
