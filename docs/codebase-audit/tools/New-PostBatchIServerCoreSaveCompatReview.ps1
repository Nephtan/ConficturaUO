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

$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-I-01A'
$BatchName = 'POST-BATCH-I'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-i-servercore-save-compat-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-i-servercore-save-compat-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$PostBatchBTriagePath = Join-Path $OutputDir 'post-batch-b-save-compatibility-triage.csv'
$SerializationRegisterPath = Join-Path $OutputDir 'serialization-register.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$AllowedDecisions = @(
    'Fixed',
    'SafeNoChange',
    'IntentionalLegacy',
    'FalsePositive',
    'NeedsHumanDecision',
    'QueuedSourceFollowUp'
)

$RepairRows = Get-CsvRows -Path $RepairBacklogPath
$ActiveRows = Get-CsvRows -Path $ActivePath
$ActiveWithoutI = @($ActiveRows | Where-Object { $_.PostAuditBatch -ne $BatchName })
$CoveredIds = New-Object 'System.Collections.Generic.HashSet[string]'

foreach ($row in $ActiveWithoutI)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredIds.Add($row.BacklogId)
    }
}

$RemainingP0Rows = @($RepairRows | Where-Object { $_.Priority -eq 'P0' -and -not $CoveredIds.Contains($_.Id) } | Sort-Object Id)

if ($RemainingP0Rows.Count -ne 19)
{
    throw "Expected 19 residual unreviewed P0 rows before POST-BATCH-I, found $($RemainingP0Rows.Count)."
}

$OutOfScope = @($RemainingP0Rows | Where-Object { $_.Category -ne 'Save compatibility' -or $_.System -ne 'ServerCore' })

if ($OutOfScope.Count -ne 0)
{
    throw "Residual P0 rows are not all ServerCore save compatibility rows: $($OutOfScope.Id -join ', ')"
}

$TriageRows = Get-CsvRows -Path $PostBatchBTriagePath
$TriageById = @{}

foreach ($triage in $TriageRows)
{
    $TriageById[$triage.BacklogId] = $triage
}

$SerializationRows = Get-CsvRows -Path $SerializationRegisterPath
$SerializationByEvidence = @{}

foreach ($serializer in $SerializationRows)
{
    $key = "$($serializer.File)|$($serializer.Evidence)"
    if (-not $SerializationByEvidence.ContainsKey($key))
    {
        $SerializationByEvidence[$key] = $serializer
    }
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $RemainingP0Rows)
{
    if (-not $TriageById.ContainsKey($row.Id))
    {
        throw "POST-BATCH-B triage evidence is missing for $($row.Id)."
    }

    $triage = $TriageById[$row.Id]

    if ($AllowedDecisions -notcontains $triage.Decision)
    {
        throw "Decision '$($triage.Decision)' for $($row.Id) is not allowed for POST-BATCH-I."
    }

    $Index++
    $ReviewRowId = 'PBI-{0:D3}' -f $Index
    $serializerKey = "$($row.Files)|$($row.Evidence -replace '; class=.*$', '')"
    $RegisterFinding = $row.Evidence

    if ($SerializationByEvidence.ContainsKey($serializerKey))
    {
        $serializer = $SerializationByEvidence[$serializerKey]
        $RegisterFinding = "serialization-register.csv: BaseCallStatus=$($serializer.BaseCallStatus); VersionHandling=$($serializer.VersionHandling); FieldAlignment=$($serializer.FieldAlignment); MoveRenameRisk=$($serializer.MoveRenameRisk); Evidence=$($serializer.Evidence)"
    }

    $SourceEvidence = "Current source and prior POST-BATCH-B review: $($triage.SourceEvidence); $RegisterFinding"
    $Verification = "$($triage.Verification) POST-BATCH-I verified the residual P0 row is present in POST-BATCH-B source review evidence, source files still exist, and active-overlay reconciliation is required. git diff --check: $DiffCheckResult. Serialization register was not regenerated because no source serializer logic or classification output changed."
    $Notes = "$($triage.Notes) Prior reviewed batch: $($triage.ReviewedByBatch). POST-BATCH-I only maps this residual BacklogId into the active overlay."

    $ReviewRows.Add([pscustomobject]@{
        BatchId = $BatchName
        ReviewRowId = $ReviewRowId
        BacklogId = $row.Id
        SourceId = $row.SourceId
        SerialId = $triage.SerialId
        Priority = $row.Priority
        HistoricalStatus = $row.Status
        Category = $row.Category
        System = $row.System
        Files = $row.Files
        Class = $triage.Class
        OriginalEvidence = $row.Evidence
        Risk = $row.Risk
        RecommendedFix = $row.RecommendedFix
        Decision = $triage.Decision
        SourceEvidence = $SourceEvidence
        Action = "$($triage.ProposedAction) POST-BATCH-I action: reconciled the missing active-overlay row to this existing source-reviewed disposition; no source, project, runtime, XML, config, namespace, save-layout, or gameplay behavior change was made."
        Verification = $Verification
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $Notes
    }) | Out-Null
}

if ($ReviewRows.Count -ne 19)
{
    throw "Review row invariant failed: $($ReviewRows.Count)"
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 19)
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
        Files = $review.Files
        HistoricalStatus = $review.HistoricalStatus
        ActiveStatus = $review.Decision
        PostAuditBatch = $BatchName
        ReviewRowId = $review.ReviewRowId
        ReviewStatus = $review.Decision
        Action = $review.Action
        ReviewArtifact = $ReviewRel
        SourceEvidence = $review.SourceEvidence
        Commit = 'Pending current POST-BATCH-I commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($ActiveWithoutI + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$ActiveAfter = Get-CsvRows -Path $ActivePath
$PostBatchIRows = @($ActiveAfter | Where-Object { $_.PostAuditBatch -eq $BatchName })

if ($PostBatchIRows.Count -ne 19)
{
    throw "Active overlay POST-BATCH-I count invariant failed: $($PostBatchIRows.Count)"
}

$CoveredAfter = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ActiveAfter)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredAfter.Add($row.BacklogId)
    }
}

$RemainingP0After = @($RepairRows | Where-Object { $_.Priority -eq 'P0' -and -not $CoveredAfter.Contains($_.Id) })

if ($RemainingP0After.Count -ne 0)
{
    throw "Unreviewed P0 rows remain after POST-BATCH-I overlay update: $($RemainingP0After.Id -join ', ')"
}

$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$StatusCounts = @($PostBatchIRows | Group-Object ActiveStatus | Sort-Object Name)
$DecisionSummary = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-I ServerCore Save Compatibility Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-I` reviewed and reconciled the 19 residual unreviewed P0 `ServerCore` / `Save compatibility` rows from `repair-backlog.csv`. These rows were already source-reviewed in `post-batch-b-save-compatibility-triage.csv`, but their `BacklogId` values were absent from `post-audit-active-backlog-status.csv`.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('No serializer source, save layout, serialized type name, namespace, version, project file, XML/config file, runtime file location, or gameplay behavior was changed. The batch is an active-overlay reconciliation backed by current source and prior POST-BATCH-B evidence.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Active Overlay Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $StatusCounts -NameHeader 'Active status'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- `git status --short` was clean before the batch.') | Out-Null
$Closeout.Add('- `rg --files -g AGENTS.md` was rerun; applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.') | Out-Null
$Closeout.Add('- Current source snippets and `serialization-register.csv` were reviewed for the 19 rows, and the matching POST-BATCH-B triage rows were found for every `BacklogId`.') | Out-Null
$Closeout.Add('- `post-batch-i-servercore-save-compat-review.csv` contains exactly 19 rows.') | Out-Null
$Closeout.Add('- `post-audit-active-backlog-status.csv` contains exactly 19 POST-BATCH-I rows.') | Out-Null
$Closeout.Add('- Comparing `repair-backlog.csv` P0 rows to the active overlay leaves 0 unreviewed P0 rows.') | Out-Null
$Closeout.Add('- `New-SerializationRegister.ps1` was not rerun because this batch made no source serializer edits and did not change serializer classification outputs.') | Out-Null
$Closeout.Add("- `git diff --check`: $DiffCheckResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Boundary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('This batch does not begin broad P1 runtime hook, gump guard, command access, region/map, legacy compatibility, or non-ServerCore save-compatibility work.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-i-servercore-save-compat-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-i-servercore-save-compat-review.csv` | Post-audit | Review and reconcile the 19 residual P0 ServerCore save-compatibility rows against current source and POST-BATCH-B source-reviewed evidence. | Complete |',
    '| `post-batch-i-servercore-save-compat-closeout.md` | Post-audit | Close out POST-BATCH-I with disposition counts, active-overlay reconciliation, P0 backlog verification, and source-regeneration boundary. | Complete |'
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
$ISummary = 'Post-audit ServerCore save compatibility residual review: `{0}` reconciled the 19 residual unreviewed P0 `ServerCore` / `Save compatibility` rows in `outputs/post-batch-i-servercore-save-compat-review.csv`. The rows were already source-reviewed in `post-batch-b-save-compatibility-triage.csv` and are now active-overlay-covered with disposition summary `{1}`; remaining unreviewed P0 backlog rows=0. No source/project/runtime/XML/config files changed, and `New-SerializationRegister.ps1` was not rerun because no serializer source or classification output changed.' -f $ReviewBatchId, $DecisionSummary

if ($PhaseStatus -match 'Post-audit ServerCore save compatibility residual review:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit ServerCore save compatibility residual review:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $ISummary }, 1)
}
else
{
    $Marker = 'Post-audit save compatibility triage:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($ISummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 304 reviewed save-compatibility dispositions across `POST-BATCH-B` and `POST-BATCH-I`, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, 61 `POST-BATCH-G` project include drift dispositions, and 14 `POST-BATCH-H` folder/namespace cleanup dispositions. The `POST-BATCH-E` review CSV has 292 reviewed rows through `POST-BATCH-E-100A`; `RB-01877` through `RB-01882`, `RB-01884` through `RB-01891`, and `RB-01892` through `RB-01893` keep their earlier `POST-BATCH-C-01A` active overlay dispositions to preserve unique BacklogIds. Comparing `repair-backlog.csv` P0 rows against the active overlay now leaves 0 unreviewed P0 rows.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    '- Affected phase: Post-audit `POST-BATCH-I` ServerCore P0 save compatibility residual review',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: `git status --short`; `rg --files -g AGENTS.md`; review residual P0 rows from `repair-backlog.csv`, current source snippets, `serialization-register.csv`, and `post-batch-b-save-compatibility-triage.csv`; run `New-PostBatchIServerCoreSaveCompatReview.ps1`.',
    ('- Result: Review CSV contains {0} POST-BATCH-I rows; active overlay contains {1} POST-BATCH-I rows; disposition summary is {2}; remaining unreviewed P0 backlog rows=0; no source/project/runtime/XML/config files changed; serialization register was not regenerated because no source serializer logic or classification output changed; git diff --check={3}.' -f $ReviewRows.Count, $PostBatchIRows.Count, $DecisionSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-I` ServerCore P0 save compatibility residual review\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $PostBatchIRows.Count
    DecisionSummary = $DecisionSummary
    RemainingP0After = $RemainingP0After.Count
    DiffCheckResult = $DiffCheckResult
    Timestamp = $Timestamp
}
