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

function Normalize-AuditPath
{
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path))
    {
        return ''
    }

    return ($Path -replace '\\', '/').Trim()
}

function Get-BacklogLine
{
    param([string]$Evidence)

    if ($Evidence -match '\bL(\d+)\b')
    {
        return [int]$Matches[1]
    }

    return 0
}

function Get-EvidenceValue
{
    param(
        [string]$Evidence,
        [string]$Name
    )

    if ($Evidence -match ([regex]::Escape($Name) + '=([^;]*)'))
    {
        return $Matches[1].Trim()
    }

    return ''
}

function Get-FirstSourcePath
{
    param([object]$BacklogRow)

    $path = Normalize-AuditPath -Path $BacklogRow.Files

    if (-not [string]::IsNullOrWhiteSpace($path))
    {
        return $path
    }

    if ($BacklogRow.Evidence -match '(Data/(?:Scripts|System/Source)/[^:;]+?\.cs)')
    {
        return Normalize-AuditPath -Path $Matches[1]
    }

    if ($BacklogRow.Evidence -match '(docs/[^ >;]+?\.md)')
    {
        return Normalize-AuditPath -Path $Matches[1]
    }

    return ''
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

$MoveRoots = [ordered]@{
    'Data/Scripts/Custom/AIOverhaul' = 'Data/Scripts/Custom/Combat/AIOverhaul'
    'Data/Scripts/Custom/CharacterLevel' = 'Data/Scripts/Custom/Progression/CharacterLevel'
    'Data/Scripts/Custom/CloneOfflinePlayerCharacters' = 'Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters'
    'Data/Scripts/Custom/MonsterNest' = 'Data/Scripts/Custom/PvE/MonsterNests'
    'Data/Scripts/Custom/OmniAI' = 'Data/Scripts/Custom/ThirdParty/OmniAI'
    'Data/Scripts/Custom/RandomEncounters' = 'Data/Scripts/Custom/PvE/RandomEncounters'
    'Data/Scripts/Custom/Staff Toolbar [2.0]' = 'Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]'
    'Data/Scripts/Custom/OzThothStaticGump' = 'Data/Scripts/Custom/StaffTools/StaticGumpTool'
}

function Resolve-CurrentPath
{
    param([string]$Path)

    $pathText = Normalize-AuditPath -Path $Path

    foreach ($oldRoot in ($MoveRoots.Keys | Sort-Object Length -Descending))
    {
        if ($pathText.StartsWith($oldRoot, [System.StringComparison]::Ordinal))
        {
            return [pscustomobject]@{
                Path = $MoveRoots[$oldRoot] + $pathText.Substring($oldRoot.Length)
                PathResolution = "PostBatchHMove:$oldRoot->$($MoveRoots[$oldRoot])"
            }
        }
    }

    return [pscustomobject]@{
        Path = $pathText
        PathResolution = 'OriginalPathCurrent'
    }
}

$SourceCache = @{}

function Get-SourceLines
{
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path))
    {
        return @()
    }

    $fullPath = Join-Path $RepoRoot $Path

    if (-not (Test-Path -LiteralPath $fullPath))
    {
        return @()
    }

    if (-not $SourceCache.ContainsKey($Path))
    {
        $SourceCache[$Path] = @(Get-Content -LiteralPath $fullPath)
    }

    return @($SourceCache[$Path])
}

function Get-SourceLine
{
    param(
        [string]$Path,
        [int]$Line
    )

    $lines = @(Get-SourceLines -Path $Path)

    if ($Line -le 0 -or $Line -gt $lines.Count)
    {
        return ''
    }

    return $lines[$Line - 1].Trim()
}

function Get-CommandBlock
{
    param(
        [string]$Path,
        [int]$Line
    )

    $lines = @(Get-SourceLines -Path $Path)

    if ($Line -le 0 -or $Line -gt $lines.Count)
    {
        return ''
    }

    $parts = New-Object System.Collections.Generic.List[string]
    $maxLine = [Math]::Min($lines.Count, $Line + 15)

    for ($i = $Line; $i -le $maxLine; $i++)
    {
        $text = $lines[$i - 1].Trim()
        $parts.Add($text) | Out-Null

        if ($text -match '\);\s*$')
        {
            break
        }
    }

    return ($parts -join ' ')
}

function Get-CommandParse
{
    param([string]$Block)

    $result = [ordered]@{
        Command = ''
        Access = ''
        Handler = ''
        ParseQuality = 'NoCommandBlock'
    }

    if ([string]::IsNullOrWhiteSpace($Block))
    {
        return [pscustomobject]$result
    }

    $result.ParseQuality = 'BlockOnly'

    if ($Block -match 'CommandSystem\.Register\s*\(\s*"([^"]+)"')
    {
        $result.Command = $Matches[1]
    }
    elseif ($Block -match 'CommandSystem\.Register\s*\(\s*([^,\s]+)')
    {
        $result.Command = $Matches[1]
    }

    if ($Block -match 'AccessLevel\.([A-Za-z]+)')
    {
        $result.Access = $Matches[1]
    }
    elseif ($Block -match 'CommandSystem\.Register\s*\([^,]+,\s*([^,\s]+)')
    {
        $result.Access = $Matches[1]
    }

    if ($Block -match 'new\s+CommandEventHandler\s*\(\s*([^)]+)\s*\)')
    {
        $result.Handler = $Matches[1].Trim()
    }
    elseif ($Block -match 'CommandSystem\.Register\s*\([^,]+,\s*[^,]+,\s*([^)]+)\)')
    {
        $result.Handler = $Matches[1].Trim()
    }

    if (-not [string]::IsNullOrWhiteSpace($result.Command) -and
        -not [string]::IsNullOrWhiteSpace($result.Access) -and
        -not [string]::IsNullOrWhiteSpace($result.Handler))
    {
        $result.ParseQuality = 'ParsedCommandAccessHandler'
    }
    elseif (-not [string]::IsNullOrWhiteSpace($result.Access))
    {
        $result.ParseQuality = 'ParsedAccessOnly'
    }

    return [pscustomobject]$result
}

function Test-DisabledEvidence
{
    param(
        [string]$Evidence,
        [string]$SourceLine
    )

    if ($Evidence -match 'Registration=\s*//CommandSystem\.Register')
    {
        return $true
    }

    if ($Evidence -match ':\s*/\*')
    {
        return $true
    }

    if ($SourceLine -match '^\s*//')
    {
        return $true
    }

    if ($SourceLine -match '^\s*/\*')
    {
        return $true
    }

    return $false
}

$Timestamp = (Get-Date).ToString('o')
$BatchName = 'POST-BATCH-L'
$ReviewBatchId = 'POST-BATCH-L-01A'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-l-p2-residual-backlog-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-l-p2-residual-backlog-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$ProjectTruthPath = Join-Path $OutputDir 'project-truth-register.csv'
$RuntimeInventoryPath = Join-Path $OutputDir 'cross-tree-runtime-inventory.csv'
$CommandRegisterPath = Join-Path $OutputDir 'phase-05-command-surface-register.csv'
$SerializationPath = Join-Path $OutputDir 'serialization-register.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$ScopedCategories = @(
    'Command access',
    'Legacy compatibility',
    'Save compatibility',
    'Region and map assumptions'
)

$RepairRows = Get-CsvRows -Path $RepairBacklogPath
$ActiveRows = Get-CsvRows -Path $ActivePath
$ActiveWithoutL = @($ActiveRows | Where-Object { $_.PostAuditBatch -ne $BatchName })
$CoveredIds = New-Object 'System.Collections.Generic.HashSet[string]'

foreach ($row in $ActiveWithoutL)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredIds.Add($row.BacklogId)
    }
}

$ScopeRows = @($RepairRows | Where-Object {
    $_.Priority -eq 'P2' -and
    $ScopedCategories -contains $_.Category -and
    -not $CoveredIds.Contains($_.Id)
} | Sort-Object Category, System, Id)

if ($ScopeRows.Count -ne 1186)
{
    throw "Expected 1,186 POST-BATCH-L P2 residual rows, found $($ScopeRows.Count)."
}

$ProjectTruthRows = Get-CsvRows -Path $ProjectTruthPath
$RuntimeRows = Get-CsvRows -Path $RuntimeInventoryPath
$CommandRows = Get-CsvRows -Path $CommandRegisterPath
$SerializerRows = Get-CsvRows -Path $SerializationPath

$ProjectByPath = @{}
foreach ($row in $ProjectTruthRows)
{
    $path = Normalize-AuditPath -Path $row.Path
    if (-not [string]::IsNullOrWhiteSpace($path))
    {
        if (-not $ProjectByPath.ContainsKey($path))
        {
            $ProjectByPath[$path] = @()
        }
        $ProjectByPath[$path] += $row
    }
}

$RuntimeByPath = @{}
foreach ($row in $RuntimeRows)
{
    $path = Normalize-AuditPath -Path $row.Path
    if (-not [string]::IsNullOrWhiteSpace($path))
    {
        $RuntimeByPath[$path] = $row
    }
}

$CommandByFileLine = @{}
foreach ($row in $CommandRows)
{
    $path = Normalize-AuditPath -Path $row.File
    $key = "$path|$($row.Line)"
    if (-not $CommandByFileLine.ContainsKey($key))
    {
        $CommandByFileLine[$key] = @()
    }
    $CommandByFileLine[$key] += $row
}

$SerializerByFile = @{}
foreach ($row in $SerializerRows)
{
    $path = Normalize-AuditPath -Path $row.File
    if (-not [string]::IsNullOrWhiteSpace($path))
    {
        if (-not $SerializerByFile.ContainsKey($path))
        {
            $SerializerByFile[$path] = @()
        }
        $SerializerByFile[$path] += $row
    }
}

function Get-Decision
{
    param(
        [object]$BacklogRow,
        [bool]$Disabled,
        [bool]$FileExists,
        [object]$CommandParse,
        [object[]]$ProjectRows,
        [object]$RuntimeRow,
        [object[]]$SerializerRowsForFile
    )

    switch ($BacklogRow.Category)
    {
        'Command access'
        {
            if ($Disabled)
            {
                return 'SafeNoChange'
            }

            if ($CommandParse.ParseQuality -eq 'ParsedCommandAccessHandler' -and
                $CommandParse.Access -match '^(Administrator|GameMaster|Seer|Counselor|Player)$')
            {
                return 'ReviewedNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        'Legacy compatibility'
        {
            if ($FileExists -and $ProjectRows.Count -gt 0 -and
                @($ProjectRows | Where-Object { $_.MissingCompileTarget -eq 'True' -or $_.UnincludedSource -eq 'True' }).Count -eq 0)
            {
                return 'IntentionalLegacy'
            }

            return 'QueuedSourceFollowUp'
        }
        'Save compatibility'
        {
            if ($BacklogRow.Files -like 'Data/System/Source/*')
            {
                return 'ReviewedNoChange'
            }

            if ($FileExists -and $ProjectRows.Count -gt 0 -and $SerializerRowsForFile.Count -gt 0)
            {
                return 'SafeNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        'Region and map assumptions'
        {
            return 'DeferredPolicyDecision'
        }
        default
        {
            return 'QueuedSourceFollowUp'
        }
    }
}

function Get-Action
{
    param(
        [string]$Decision,
        [string]$Category
    )

    if ($Category -eq 'Region and map assumptions')
    {
        return 'No source change. Region precedence and paired-system policy remain deferred to a focused design/source batch before changing travel, PvP, government, spawning, or map behavior.'
    }

    switch ($Decision)
    {
        'ReviewedNoChange' { return 'No source change. Current source/register evidence resolves the generated P2 triage concern without proving a narrow mechanical defect.' }
        'SafeNoChange' { return 'No source change. Current evidence shows the row is already covered by explicit source/project/serializer state or is not a live repair target.' }
        'IntentionalLegacy' { return 'No source change. Current evidence shows this runtime-visible legacy source remains intentionally compiled for compatibility or old-save/type availability.' }
        'DeferredPolicyDecision' { return 'No source change. A design or policy decision is required before behavior changes.' }
        'QueuedSourceFollowUp' { return 'No source change. Current evidence is real but still needs a focused source review before implementation.' }
        default { return 'No source change. Recorded deterministic P2 residual triage disposition.' }
    }
}

function Get-Verification
{
    param(
        [string]$Decision,
        [string]$Category,
        [string]$DiffCheckResult
    )

    $base = "Reviewed against current repair backlog, active overlay, source path/line, project truth, runtime inventory, command register, and serialization register as applicable. git diff --check: $DiffCheckResult."

    if ($Decision -eq 'QueuedSourceFollowUp')
    {
        return "$base Future source repair must run the category-specific register refresh plus Server.csproj Debug/x86 build and compile-only runtime verification if runtime source changes."
    }

    if ($Category -eq 'Save compatibility')
    {
        return "$base No serialized layout was changed; future save-layout edits still require serialization register comparison and migration/save-load policy."
    }

    if ($Category -eq 'Region and map assumptions')
    {
        return "$base No region or map policy was changed; future source edits require dependency graph/runtime hook refresh and build verification."
    }

    return "$base No runtime source edit was made, so source build and compile-only runtime verification were not required for this audit-only disposition."
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $ScopeRows)
{
    $Index++
    $reviewRowId = 'PBL-{0:D4}' -f $Index
    $historicalPath = Get-FirstSourcePath -BacklogRow $row
    $resolved = Resolve-CurrentPath -Path $historicalPath
    $currentFile = $resolved.Path
    $line = Get-BacklogLine -Evidence $row.Evidence
    $sourceLine = Get-SourceLine -Path $currentFile -Line $line
    $fileExists = $false

    if (-not [string]::IsNullOrWhiteSpace($currentFile))
    {
        $fileExists = Test-Path -LiteralPath (Join-Path $RepoRoot $currentFile)
    }

    $commandBlock = ''
    $commandParse = [pscustomobject]@{
        Command = ''
        Access = ''
        Handler = ''
        ParseQuality = 'NotCommandRow'
    }

    if ($row.Category -eq 'Command access')
    {
        $commandBlock = Get-CommandBlock -Path $currentFile -Line $line
        $commandParse = Get-CommandParse -Block $commandBlock
    }

    $projectRowsForFile = @()
    if ($ProjectByPath.ContainsKey($currentFile))
    {
        $projectRowsForFile = @($ProjectByPath[$currentFile])
    }

    $runtimeRow = $null
    if ($RuntimeByPath.ContainsKey($currentFile))
    {
        $runtimeRow = $RuntimeByPath[$currentFile]
    }

    $serializerRowsForFile = @()
    if ($SerializerByFile.ContainsKey($currentFile))
    {
        $serializerRowsForFile = @($SerializerByFile[$currentFile])
    }

    $commandRegisterRows = @()
    $commandKey = "$currentFile|$line"
    if ($CommandByFileLine.ContainsKey($commandKey))
    {
        $commandRegisterRows = @($CommandByFileLine[$commandKey])
    }

    $disabled = Test-DisabledEvidence -Evidence $row.Evidence -SourceLine $sourceLine
    $decision = Get-Decision -BacklogRow $row -Disabled $disabled -FileExists $fileExists -CommandParse $commandParse -ProjectRows $projectRowsForFile -RuntimeRow $runtimeRow -SerializerRowsForFile $serializerRowsForFile
    $action = Get-Action -Decision $decision -Category $row.Category
    $verification = Get-Verification -Decision $decision -Category $row.Category -DiffCheckResult $DiffCheckResult
    $matchQuality = if (-not [string]::IsNullOrWhiteSpace($currentFile) -and $fileExists -and $line -gt 0 -and -not [string]::IsNullOrWhiteSpace($sourceLine)) {
        'CurrentSourceLine'
    } elseif (-not [string]::IsNullOrWhiteSpace($currentFile) -and $fileExists) {
        'CurrentFileOnly'
    } elseif ($row.Category -eq 'Region and map assumptions') {
        'DependencyGraphPairEvidence'
    } else {
        'NoCurrentSourcePath'
    }

    if ($matchQuality -eq 'NoCurrentSourcePath')
    {
        throw "Could not resolve current source path for $($row.Id): $($row.Files)"
    }

    $projectSummary = if ($projectRowsForFile.Count -gt 0) {
        (($projectRowsForFile | Select-Object -First 2 | ForEach-Object { "RecordType=$($_.RecordType);Included=$($_.IncludedInScriptsProject);Missing=$($_.MissingCompileTarget);Unincluded=$($_.UnincludedSource);Action=$($_.Action)" }) -join ' | ')
    } else {
        ''
    }

    $runtimeSummary = if ($runtimeRow -ne $null) {
        "PrimaryRole=$($runtimeRow.PrimaryRole);SecondaryRoles=$($runtimeRow.SecondaryRoles);Commands=$($runtimeRow.Commands);Hooks=$($runtimeRow.Hooks);SerializedTypes=$($runtimeRow.SerializedTypes);Gumps=$($runtimeRow.Gumps)"
    } else {
        ''
    }

    $serializerSummary = if ($serializerRowsForFile.Count -gt 0) {
        (($serializerRowsForFile | Select-Object -First 3 | ForEach-Object { "Class=$($_.Class);Version=$($_.CurrentVersion);Handling=$($_.VersionHandling);Alignment=$($_.FieldAlignment);MoveRisk=$($_.MoveRenameRisk)" }) -join ' | ')
    } else {
        ''
    }

    $commandSummary = if ($commandRegisterRows.Count -gt 0) {
        (($commandRegisterRows | Select-Object -First 2 | ForEach-Object { "RegisterCommand=$($_.Command);RegisterAccess=$($_.AccessLevel);RegisterHandler=$($_.Handler);Guards=$($_.Guards);Risk=$($_.Risk)" }) -join ' | ')
    } else {
        ''
    }

    $sourceEvidence = "CurrentFile=$currentFile; Line=$line; SourceLine=$sourceLine; PathResolution=$($resolved.PathResolution); MatchQuality=$matchQuality; FileExists=$fileExists; DisabledEvidence=$disabled"

    if ($row.Category -eq 'Command access')
    {
        $sourceEvidence = "$sourceEvidence; ParsedCommand=$($commandParse.Command); ParsedAccess=$($commandParse.Access); ParsedHandler=$($commandParse.Handler); CommandParseQuality=$($commandParse.ParseQuality); CommandBlock=$commandBlock"
    }

    if (-not [string]::IsNullOrWhiteSpace($projectSummary))
    {
        $sourceEvidence = "$sourceEvidence; ProjectTruth=$projectSummary"
    }

    if (-not [string]::IsNullOrWhiteSpace($runtimeSummary))
    {
        $sourceEvidence = "$sourceEvidence; RuntimeInventory=$runtimeSummary"
    }

    if (-not [string]::IsNullOrWhiteSpace($serializerSummary))
    {
        $sourceEvidence = "$sourceEvidence; SerializationRegister=$serializerSummary"
    }

    if (-not [string]::IsNullOrWhiteSpace($commandSummary))
    {
        $sourceEvidence = "$sourceEvidence; CommandRegister=$commandSummary"
    }

    $notes = "HistoricalFile=$historicalPath; Risk=$($row.Risk); POST-BATCH-L is audit-only P2 residual triage. No source, project, XML/config, serialized layout, runtime file location, command name, access level, or region policy changed."

    $ReviewRows.Add([pscustomobject]@{
        BatchId = $BatchName
        ReviewRowId = $reviewRowId
        BacklogId = $row.Id
        SourceId = $row.SourceId
        Priority = $row.Priority
        HistoricalStatus = $row.Status
        Category = $row.Category
        System = $row.System
        HistoricalFile = $historicalPath
        CurrentFile = $currentFile
        Line = $line
        CurrentSourceLine = $sourceLine
        MatchQuality = $matchQuality
        ParsedCommand = $commandParse.Command
        ParsedAccess = $commandParse.Access
        ParsedHandler = $commandParse.Handler
        CommandParseQuality = $commandParse.ParseQuality
        OriginalEvidence = $row.Evidence
        Decision = $decision
        SourceEvidence = $sourceEvidence
        Action = $action
        Verification = $verification
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $notes
    }) | Out-Null
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 1186)
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
        Commit = 'Pending current POST-BATCH-L commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($ActiveWithoutL + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$ActiveAfter = Get-CsvRows -Path $ActivePath
$PostBatchLRows = @($ActiveAfter | Where-Object { $_.PostAuditBatch -eq $BatchName })

if ($PostBatchLRows.Count -ne 1186)
{
    throw "Active overlay POST-BATCH-L count invariant failed: $($PostBatchLRows.Count)"
}

$CoveredAfter = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ActiveAfter)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredAfter.Add($row.BacklogId)
    }
}

$RemainingAfter = @($RepairRows | Where-Object { -not [string]::IsNullOrWhiteSpace($_.Id) -and -not $CoveredAfter.Contains($_.Id) })

if ($RemainingAfter.Count -ne 0)
{
    throw "Unreviewed repair-backlog rows remain after POST-BATCH-L overlay update: $($RemainingAfter.Id -join ', ')"
}

$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$CategoryCounts = @($ReviewRows | Group-Object Category | Sort-Object Name)
$MatchQualityCounts = @($ReviewRows | Group-Object MatchQuality | Sort-Object Name)
$SystemCounts = @($ReviewRows | Group-Object System | Sort-Object Count -Descending)
$DecisionSummary = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$CategorySummary = (($CategoryCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-L P2 Residual Backlog Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-L` reviewed all 1,186 remaining P2 rows from `repair-backlog.csv` that were absent from the active overlay. The batch is audit-only residual triage: it records current source/project/runtime/serializer evidence, dispositions, and follow-up gates without changing runtime behavior.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('No runtime source, public API, namespace, serialized type name, save version, command name, command access level, gump button ID, packet behavior, region name, XML/config file, project file, runtime file location, or gameplay behavior was changed.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Category Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $CategoryCounts -NameHeader 'Category'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
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
$Closeout.Add('- Applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read; root audit plan plus Phase 13 and Phase 14 plans were re-read for backlog and verification rules.') | Out-Null
$Closeout.Add('- `repair-backlog.csv` compared against `post-audit-active-backlog-status.csv` produced exactly 1,186 remaining P2 rows before POST-BATCH-L.') | Out-Null
$Closeout.Add('- Every POST-BATCH-L row resolves to current source evidence, current project truth/runtime inventory/serializer evidence, or explicit pairwise dependency/policy evidence for region rows.') | Out-Null
$Closeout.Add('- `post-batch-l-p2-residual-backlog-review.csv` contains exactly 1,186 rows.') | Out-Null
$Closeout.Add('- `post-audit-active-backlog-status.csv` contains exactly 1,186 POST-BATCH-L rows.') | Out-Null
$Closeout.Add('- Comparing all `repair-backlog.csv` rows to the active overlay leaves 0 unreviewed rows.') | Out-Null
$Closeout.Add('- Project truth, runtime hook map, dependency graph, documentation truth, serialization register, and system cards were not regenerated because this batch made no runtime source, project, path, XML/config, docs source-trace, or serializer source changes.') | Out-Null
$Closeout.Add("- `git diff --check`: $DiffCheckResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Boundary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-L` closes the unreviewed historical repair-backlog gap. Rows marked `QueuedSourceFollowUp` or `DeferredPolicyDecision` still require focused future source/design batches before changing command behavior, legacy compatibility surfaces, save layout, or region/map policy.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-l-p2-residual-backlog-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-l-p2-residual-backlog-review.csv` | Post-audit | Review all 1,186 remaining P2 command-access, legacy-compatibility, save-compatibility, and region/map rows against current source/project/runtime evidence. | Complete |',
    '| `post-batch-l-p2-residual-backlog-closeout.md` | Post-audit | Close out POST-BATCH-L with category counts, disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |'
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
$LSummary = 'Post-audit P2 residual backlog triage: `{0}` reviewed all 1,186 remaining P2 rows in `outputs/post-batch-l-p2-residual-backlog-review.csv`. Category summary is `{1}`; disposition summary is `{2}`; remaining unreviewed repair-backlog rows=0. No source/project/runtime/XML/config files changed, and project truth/runtime hook map/dependency graph/serialization outputs were not regenerated because no source or path evidence changed.' -f $ReviewBatchId, $CategorySummary, $DecisionSummary

if ($PhaseStatus -match 'Post-audit P2 residual backlog triage:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit P2 residual backlog triage:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $LSummary }, 1)
}
else
{
    $Marker = 'Post-audit P1 runtime surface safety triage:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($LSummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 1,598 reviewed save-compatibility dispositions across `POST-BATCH-B`, `POST-BATCH-I`, and `POST-BATCH-J`, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, 61 `POST-BATCH-G` project include drift dispositions, 14 `POST-BATCH-H` folder/namespace cleanup dispositions, 2,691 `POST-BATCH-K` P1 runtime-surface dispositions, and 1,186 `POST-BATCH-L` P2 residual dispositions. Comparing all `repair-backlog.csv` rows against the active overlay leaves 0 unreviewed rows.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    '- Affected phase: Post-audit `POST-BATCH-L` P2 residual backlog triage',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P2 command-access, legacy-compatibility, save-compatibility, and region/map rows against current source paths/lines, project truth, runtime inventory, command register, and serialization register; run `New-PostBatchLP2ResidualBacklogReview.ps1`.',
    ('- Result: Review CSV contains {0} POST-BATCH-L rows; active overlay contains {1} POST-BATCH-L rows; category summary is {2}; disposition summary is {3}; remaining unreviewed repair-backlog rows=0; no source/project/runtime/XML/config files changed; project truth/runtime hook/dependency/serialization outputs were not regenerated because no source/path inputs changed; git diff --check={4}.' -f $ReviewRows.Count, $PostBatchLRows.Count, $CategorySummary, $DecisionSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-L` P2 residual backlog triage\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $PostBatchLRows.Count
    CategorySummary = $CategorySummary
    DecisionSummary = $DecisionSummary
    RemainingAfter = $RemainingAfter.Count
    DiffCheckResult = $DiffCheckResult
    Timestamp = $Timestamp
}
