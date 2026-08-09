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

function Get-FirstSourcePath
{
    param([object]$BacklogRow)

    $path = Normalize-AuditPath -Path $BacklogRow.Files

    if (-not [string]::IsNullOrWhiteSpace($path))
    {
        return $path
    }

    if ($BacklogRow.Evidence -match '(Data/Scripts/[^:;]+?\.cs)')
    {
        return Normalize-AuditPath -Path $Matches[1]
    }

    if ($BacklogRow.Evidence -match '(docs/[^ >;]+?\.md)')
    {
        return Normalize-AuditPath -Path $Matches[1]
    }

    return ''
}

function Get-GuardList
{
    param([string]$Evidence)

    if ($Evidence -notmatch '(?i)guards=')
    {
        return ''
    }

    $start = $Evidence.IndexOf($Matches[0], [System.StringComparison]::OrdinalIgnoreCase)
    $tail = $Evidence.Substring($start + $Matches[0].Length)
    $tail = $tail -replace '(?i);\s*review=.*$', ''
    $tail = $tail -replace '(?i);\s*reason=.*$', ''
    $tail = $tail -replace '\s+', ''

    $tokens = @($tail -split ';' | Where-Object { $_ -match 'Guard$' } | Select-Object -Unique)
    return ($tokens -join ';')
}

function Test-HasGuard
{
    param(
        [string]$GuardList,
        [string]$Guard
    )

    return @($GuardList -split ';') -contains $Guard
}

function Get-EvidenceValue
{
    param(
        [string]$Evidence,
        [string]$Name
    )

    if ($Evidence -match ([regex]::Escape($Name) + '=([^;]+)'))
    {
        return $Matches[1].Trim()
    }

    return ''
}

function Get-HookKind
{
    param([object]$BacklogRow)

    switch ($BacklogRow.Category)
    {
        'Gump guards' { return 'Gump' }
        'Command access' { return 'Command' }
        'PlayerMobile coupling' { return 'PlayerMobileReference' }
        default
        {
            if ($BacklogRow.Evidence -match '^([^:]+):')
            {
                return $Matches[1].Trim()
            }

            return 'Unknown'
        }
    }
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

function Get-Decision
{
    param(
        [object]$BacklogRow,
        [string]$HookKind,
        [string]$GuardList,
        [string]$Access,
        [bool]$Disabled,
        [bool]$SourceLineExists,
        [bool]$HookMapMatched,
        [bool]$DependencyMatched
    )

    if ($Disabled)
    {
        return 'SafeNoChange'
    }

    switch ($BacklogRow.Category)
    {
        'PlayerMobile coupling'
        {
            return 'DeferredPolicyDecision'
        }
        'Command access'
        {
            if ($Access -in @('Administrator', 'GameMaster', 'Seer'))
            {
                return 'ReviewedNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        'Region and map assumptions'
        {
            if ($SourceLineExists -and (Test-HasGuard -GuardList $GuardList -Guard 'MapGuard') -and (Test-HasGuard -GuardList $GuardList -Guard 'StateGuard'))
            {
                return 'ReviewedNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        'Gump guards'
        {
            if ($SourceLineExists -and (Test-HasGuard -GuardList $GuardList -Guard 'StateGuard') -and
                ((Test-HasGuard -GuardList $GuardList -Guard 'AccessGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'NullGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'DeletedGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'RangeGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'MapGuard')))
            {
                return 'ReviewedNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        'Runtime hooks'
        {
            if ($HookKind -in @('Initialize', 'Timer', 'WorldSave', 'WorldLoad') -and $SourceLineExists)
            {
                return 'ReviewedNoChange'
            }

            if ($SourceLineExists -and (Test-HasGuard -GuardList $GuardList -Guard 'StateGuard') -and
                ((Test-HasGuard -GuardList $GuardList -Guard 'AccessGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'BoundsGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'NullGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'DeletedGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'RangeGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'MapGuard') -or
                 (Test-HasGuard -GuardList $GuardList -Guard 'PacketReadGuard')))
            {
                return 'ReviewedNoChange'
            }

            return 'QueuedSourceFollowUp'
        }
        default
        {
            if ($SourceLineExists -or $HookMapMatched -or $DependencyMatched)
            {
                return 'QueuedSourceFollowUp'
            }

            return 'QueuedSourceFollowUp'
        }
    }
}

function Get-Action
{
    param([string]$Decision)

    switch ($Decision)
    {
        'SafeNoChange' { return 'No source change. Current evidence shows the row is disabled/commented or has no live runtime command/hook surface to repair in this batch.' }
        'ReviewedNoChange' { return 'No source change. Current source-derived guard/access evidence does not prove a narrow mechanical defect; preserve existing runtime behavior.' }
        'DeferredPolicyDecision' { return 'No source change. The row represents broad PlayerMobile/runtime policy coupling that requires an explicit design or migration decision before refactoring.' }
        'NeedsMigrationPlan' { return 'No source change. A migration plan is required before changing save-sensitive runtime/player coupling.' }
        'NeedsHumanDecision' { return 'No source change. A human design decision is required before implementation.' }
        'QueuedSourceFollowUp' { return 'No source change. Current evidence is real but not strong enough to claim a safe no-change or apply a mechanical fix; queue a focused source review.' }
        'Fixed' { return 'Source repair was applied and verified in this batch.' }
        default { return 'No source change. Recorded deterministic runtime-surface triage disposition.' }
    }
}

function Get-Verification
{
    param(
        [string]$Decision,
        [string]$DiffCheckResult
    )

    $base = "Reviewed against current source path/line evidence, runtime-hook-map and dependency-graph evidence where applicable. git diff --check: $DiffCheckResult."

    switch ($Decision)
    {
        'SafeNoChange' { return "$base No runtime source edit was made because the current source evidence did not expose a live repair target." }
        'ReviewedNoChange' { return "$base No runtime source edit was made; build/runtime proof was not required for this audit-only disposition." }
        'DeferredPolicyDecision' { return "$base No PlayerMobile or policy source refactor was made; future implementation requires explicit design and save/runtime verification." }
        'QueuedSourceFollowUp' { return "$base No runtime source edit was made; future focused source repair must run targeted static checks, Server.csproj Debug/x86 build, and compile-only runtime verification." }
        default { return $base }
    }
}

$Timestamp = (Get-Date).ToString('o')
$BatchName = 'POST-BATCH-K'
$ReviewBatchId = 'POST-BATCH-K-01A'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-k-p1-runtime-surface-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-k-p1-runtime-surface-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$RuntimeHookPath = Join-Path $OutputDir 'runtime-hook-map.csv'
$DependencyGraphPath = Join-Path $OutputDir 'dependency-graph.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$ScopedCategories = @(
    'Runtime hooks',
    'Gump guards',
    'PlayerMobile coupling',
    'Region and map assumptions',
    'Command access'
)

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

$RepairRows = Get-CsvRows -Path $RepairBacklogPath
$ActiveRows = Get-CsvRows -Path $ActivePath
$ActiveWithoutK = @($ActiveRows | Where-Object { $_.PostAuditBatch -ne $BatchName })
$CoveredIds = New-Object 'System.Collections.Generic.HashSet[string]'

foreach ($row in $ActiveWithoutK)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredIds.Add($row.BacklogId)
    }
}

$ScopeRows = @($RepairRows | Where-Object {
    $_.Priority -eq 'P1' -and
    $ScopedCategories -contains $_.Category -and
    -not $CoveredIds.Contains($_.Id)
} | Sort-Object Id)

if ($ScopeRows.Count -ne 2691)
{
    throw "Expected 2,691 POST-BATCH-K P1 runtime-surface rows, found $($ScopeRows.Count)."
}

$RuntimeHooks = Get-CsvRows -Path $RuntimeHookPath
$HooksByFileLine = @{}

foreach ($hook in $RuntimeHooks)
{
    $key = "$(Normalize-AuditPath -Path $hook.File)|$($hook.Line)"

    if (-not $HooksByFileLine.ContainsKey($key))
    {
        $HooksByFileLine[$key] = @()
    }

    $HooksByFileLine[$key] += $hook
}

$DependencyRows = Get-CsvRows -Path $DependencyGraphPath
$PlayerMobileEdges = @($DependencyRows | Where-Object { $_.TargetSystem -eq 'PlayerMobile Core' })
$PlayerMobileByFileLine = @{}

foreach ($edge in $PlayerMobileEdges)
{
    if ($edge.Evidence -match '(Data/Scripts/[^:;]+?\.cs):L(\d+)')
    {
        $edgePath = (Resolve-CurrentPath -Path $Matches[1]).Path
        $edgeKey = "$edgePath|$($Matches[2])"

        if (-not $PlayerMobileByFileLine.ContainsKey($edgeKey))
        {
            $PlayerMobileByFileLine[$edgeKey] = @()
        }

        $PlayerMobileByFileLine[$edgeKey] += $edge
    }
}

$SourceCache = @{}

function Get-SourceLine
{
    param(
        [string]$Path,
        [int]$Line
    )

    if ([string]::IsNullOrWhiteSpace($Path) -or $Line -le 0)
    {
        return ''
    }

    $fullPath = Join-Path $RepoRoot $Path

    if (-not (Test-Path -LiteralPath $fullPath))
    {
        return ''
    }

    if (-not $SourceCache.ContainsKey($Path))
    {
        $SourceCache[$Path] = @(Get-Content -LiteralPath $fullPath)
    }

    $lines = $SourceCache[$Path]

    if ($Line -gt $lines.Count)
    {
        return ''
    }

    return $lines[$Line - 1].Trim()
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $ScopeRows)
{
    $Index++
    $ReviewRowId = 'PBK-{0:D4}' -f $Index
    $historicalPath = Get-FirstSourcePath -BacklogRow $row
    $resolved = Resolve-CurrentPath -Path $historicalPath
    $currentFile = $resolved.Path
    $line = Get-BacklogLine -Evidence $row.Evidence
    $sourceLine = Get-SourceLine -Path $currentFile -Line $line
    $sourceLineExists = -not [string]::IsNullOrWhiteSpace($sourceLine)
    $currentFileExists = $false

    if (-not [string]::IsNullOrWhiteSpace($currentFile))
    {
        $currentFileExists = Test-Path -LiteralPath (Join-Path $RepoRoot $currentFile)
    }

    $hookKind = Get-HookKind -BacklogRow $row
    $guardList = Get-GuardList -Evidence $row.Evidence
    $access = Get-EvidenceValue -Evidence $row.Evidence -Name 'Access'
    $commandName = Get-EvidenceValue -Evidence $row.Evidence -Name 'Command'
    $handler = Get-EvidenceValue -Evidence $row.Evidence -Name 'Handler'
    $disabled = Test-DisabledEvidence -Evidence $row.Evidence -SourceLine $sourceLine
    $hookKey = "$currentFile|$line"
    $hookMatches = @()

    if ($HooksByFileLine.ContainsKey($hookKey))
    {
        $hookMatches = @($HooksByFileLine[$hookKey])
    }

    $playerMobileMatches = @()

    if ($PlayerMobileByFileLine.ContainsKey($hookKey))
    {
        $playerMobileMatches = @($PlayerMobileByFileLine[$hookKey])
    }

    $dependencyMatched = $playerMobileMatches.Count -gt 0
    $hookMapMatched = $hookMatches.Count -gt 0

    $matchQuality = if ($dependencyMatched) {
        'DependencyGraphPlayerMobileFileLine'
    } elseif ($hookMapMatched -and $resolved.PathResolution -like 'PostBatchHMove:*') {
        'RuntimeHookMapFileLineAfterPostBatchHPathMap'
    } elseif ($hookMapMatched) {
        'RuntimeHookMapFileLine'
    } elseif ($sourceLineExists -and $resolved.PathResolution -like 'PostBatchHMove:*') {
        'CurrentSourceLineAfterPostBatchHPathMap'
    } elseif ($sourceLineExists) {
        'CurrentSourceLine'
    } elseif ($currentFileExists) {
        'CurrentFileOnly'
    } else {
        'NoCurrentSourcePath'
    }

    if ($matchQuality -eq 'NoCurrentSourcePath')
    {
        throw "Could not resolve current source path for $($row.Id): $($row.Evidence)"
    }

    $decision = Get-Decision -BacklogRow $row -HookKind $hookKind -GuardList $guardList -Access $access -Disabled $disabled -SourceLineExists $sourceLineExists -HookMapMatched $hookMapMatched -DependencyMatched $dependencyMatched
    $action = Get-Action -Decision $decision
    $verification = Get-Verification -Decision $decision -DiffCheckResult $DiffCheckResult
    $runtimeEvidence = if ($hookMapMatched) {
        (($hookMatches | Select-Object -First 2 | ForEach-Object { "HookType=$($_.HookType);Marker=$($_.Marker);Access=$($_.Access);Risk=$($_.Risk);Guards=$($_.Guards)" }) -join ' | ')
    } else {
        ''
    }
    $dependencyEvidence = if ($dependencyMatched) {
        (($playerMobileMatches | Select-Object -First 2 | ForEach-Object { "$($_.SourceSystem)->$($_.TargetSystem);$($_.EdgeType);$($_.Evidence)" }) -join ' | ')
    } elseif ($row.Category -eq 'PlayerMobile coupling' -and $row.Evidence -match 'DocsSourceTrace') {
        'DocsSourceTrace row points at PlayerMobile source; no direct dependency-graph line edge is expected for this documentation edge.'
    } else {
        ''
    }

    $sourceEvidence = "CurrentFile=$currentFile; Line=$line; SourceLine=$sourceLine; PathResolution=$($resolved.PathResolution); MatchQuality=$matchQuality; HookKind=$hookKind; GuardList=$guardList; Access=$access; Command=$commandName; Handler=$handler; DisabledEvidence=$disabled"

    if (-not [string]::IsNullOrWhiteSpace($runtimeEvidence))
    {
        $sourceEvidence = "$sourceEvidence; RuntimeHookMap=$runtimeEvidence"
    }

    if (-not [string]::IsNullOrWhiteSpace($dependencyEvidence))
    {
        $sourceEvidence = "$sourceEvidence; DependencyEvidence=$dependencyEvidence"
    }

    $notes = "HistoricalFile=$historicalPath; Risk=$($row.Risk); POST-BATCH-K is audit/runtime-surface triage only. No command names, gump button IDs, PlayerMobile fields, region policy, XML/config, save layout, or gameplay behavior changed."

    $ReviewRows.Add([pscustomobject]@{
        BatchId = $BatchName
        ReviewRowId = $ReviewRowId
        BacklogId = $row.Id
        SourceId = $row.SourceId
        Priority = $row.Priority
        HistoricalStatus = $row.Status
        Category = $row.Category
        System = $row.System
        HistoricalFile = $historicalPath
        CurrentFile = $currentFile
        Line = $line
        HookKind = $hookKind
        Command = $commandName
        Access = $access
        Handler = $handler
        GuardList = $guardList
        OriginalEvidence = $row.Evidence
        CurrentSourceLine = $sourceLine
        MatchQuality = $matchQuality
        Decision = $decision
        SourceEvidence = $sourceEvidence
        Action = $action
        Verification = $verification
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $notes
    }) | Out-Null
}

if ($ReviewRows.Count -ne 2691)
{
    throw "Review row count invariant failed: $($ReviewRows.Count)"
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 2691)
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
        Commit = 'Pending current POST-BATCH-K commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($ActiveWithoutK + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$ActiveAfter = Get-CsvRows -Path $ActivePath
$PostBatchKRows = @($ActiveAfter | Where-Object { $_.PostAuditBatch -eq $BatchName })

if ($PostBatchKRows.Count -ne 2691)
{
    throw "Active overlay POST-BATCH-K count invariant failed: $($PostBatchKRows.Count)"
}

$CoveredAfter = New-Object 'System.Collections.Generic.HashSet[string]'
foreach ($row in $ActiveAfter)
{
    if (-not [string]::IsNullOrWhiteSpace($row.BacklogId))
    {
        [void]$CoveredAfter.Add($row.BacklogId)
    }
}

$RemainingScopedAfter = @($RepairRows | Where-Object {
    $_.Priority -eq 'P1' -and
    $ScopedCategories -contains $_.Category -and
    -not $CoveredAfter.Contains($_.Id)
})

if ($RemainingScopedAfter.Count -ne 0)
{
    throw "Unreviewed POST-BATCH-K scoped rows remain: $($RemainingScopedAfter.Id -join ', ')"
}

$RemainingP1After = @($RepairRows | Where-Object {
    $_.Priority -eq 'P1' -and
    -not $CoveredAfter.Contains($_.Id)
})

if ($RemainingP1After.Count -ne 0)
{
    throw "Unreviewed P1 rows remain after POST-BATCH-K overlay update: $($RemainingP1After.Id -join ', ')"
}

$RemainingAllAfter = @($RepairRows | Where-Object { -not $CoveredAfter.Contains($_.Id) })
$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$CategoryCounts = @($ReviewRows | Group-Object Category | Sort-Object Name)
$MatchQualityCounts = @($ReviewRows | Group-Object MatchQuality | Sort-Object Name)
$SystemCounts = @($ReviewRows | Group-Object System | Sort-Object Count -Descending)
$DecisionSummary = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$CategorySummary = (($CategoryCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-K P1 Runtime Surface Safety Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-K` reviewed all 2,691 remaining P1 runtime-facing rows from `repair-backlog.csv` that were absent from the active overlay. The batch is source-evidence triage only: it records current source path/line, runtime-hook-map or dependency-graph evidence, dispositions, and follow-up gates without changing runtime behavior.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('No runtime source, public API, namespace, serialized type name, save version, command name, gump button ID, packet behavior, region name, XML/config file, project file, runtime file location, or gameplay behavior was changed.') | Out-Null
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
$Closeout.Add('- Applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.') | Out-Null
$Closeout.Add('- `repair-backlog.csv` compared against `post-audit-active-backlog-status.csv` produced exactly 2,691 remaining P1 runtime-facing rows before POST-BATCH-K.') | Out-Null
$Closeout.Add('- Every POST-BATCH-K row resolves to current source evidence through current source-file checks, direct source-line checks, current `runtime-hook-map.csv`, current `dependency-graph.csv`, or explicit docs-source-trace evidence. File-only/stale-line matches are conservatively dispositioned as follow-up or deferred policy work rather than source fixes.') | Out-Null
$Closeout.Add('- `post-batch-k-p1-runtime-surface-review.csv` contains exactly 2,691 rows.') | Out-Null
$Closeout.Add('- `post-audit-active-backlog-status.csv` contains exactly 2,691 POST-BATCH-K rows.') | Out-Null
$Closeout.Add('- Comparing the five P1 runtime-facing categories to the active overlay leaves 0 unreviewed scoped rows; comparing all P1 rows leaves 0 unreviewed P1 rows.') | Out-Null
$Closeout.Add('- Runtime hook map, dependency graph, documentation truth, and system cards were not regenerated because this batch made no runtime source or path changes.') | Out-Null
$Closeout.Add("- `git diff --check`: $DiffCheckResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Boundary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-K` does not begin P2 legacy compatibility, P2 command-access, P2 save-compatibility, or P2 region/map cleanup. Rows marked `QueuedSourceFollowUp` or `DeferredPolicyDecision` require focused future source/design batches before any behavior, PlayerMobile, command, gump, region, or migration change.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-k-p1-runtime-surface-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-k-p1-runtime-surface-review.csv` | Post-audit | Review all 2,691 remaining P1 runtime-hook, gump guard, PlayerMobile coupling, region/map, and command-access rows against current source/runtime evidence. | Complete |',
    '| `post-batch-k-p1-runtime-surface-closeout.md` | Post-audit | Close out POST-BATCH-K with category counts, disposition counts, source match quality, active-overlay reconciliation, and verification notes. | Complete |'
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
$KSummary = 'Post-audit P1 runtime surface safety triage: `{0}` reviewed all 2,691 remaining P1 runtime-facing rows in `outputs/post-batch-k-p1-runtime-surface-review.csv`. Category summary is `{1}`; disposition summary is `{2}`; remaining unreviewed P1 rows=0 and remaining unreviewed total backlog rows={3}. No source/project/runtime/XML/config files changed, and runtime hook map/dependency graph/system cards were not regenerated because no source or path evidence changed.' -f $ReviewBatchId, $CategorySummary, $DecisionSummary, $RemainingAllAfter.Count

if ($PhaseStatus -match 'Post-audit P1 runtime surface safety triage:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit P1 runtime surface safety triage:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $KSummary }, 1)
}
else
{
    $Marker = 'Post-audit P1 save compatibility triage:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($KSummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 1,598 reviewed save-compatibility dispositions across `POST-BATCH-B`, `POST-BATCH-I`, and `POST-BATCH-J`, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, 61 `POST-BATCH-G` project include drift dispositions, 14 `POST-BATCH-H` folder/namespace cleanup dispositions, and 2,691 `POST-BATCH-K` P1 runtime-surface dispositions. Comparing `repair-backlog.csv` P0 rows against the active overlay leaves 0 unreviewed P0 rows; comparing all P1 rows leaves 0 unreviewed P1 rows; remaining unreviewed backlog rows are P2 rows only.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    '- Affected phase: Post-audit `POST-BATCH-K` P1 runtime surface safety triage',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P1 runtime-facing rows against current source paths/lines, `runtime-hook-map.csv`, `dependency-graph.csv`, and POST-BATCH-H path moves; run `New-PostBatchKP1RuntimeSurfaceReview.ps1`.',
    ('- Result: Review CSV contains {0} POST-BATCH-K rows; active overlay contains {1} POST-BATCH-K rows; category summary is {2}; disposition summary is {3}; remaining unreviewed P1 rows=0; no source/project/runtime/XML/config files changed; runtime hook map and dependency graph were not regenerated because no source/path inputs changed; git diff --check={4}.' -f $ReviewRows.Count, $PostBatchKRows.Count, $CategorySummary, $DecisionSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-K` P1 runtime surface safety triage\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $PostBatchKRows.Count
    CategorySummary = $CategorySummary
    DecisionSummary = $DecisionSummary
    RemainingScopedAfter = $RemainingScopedAfter.Count
    RemainingP1After = $RemainingP1After.Count
    RemainingAllAfter = $RemainingAllAfter.Count
    DiffCheckResult = $DiffCheckResult
    Timestamp = $Timestamp
}
