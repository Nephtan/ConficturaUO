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

$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-H-10A'
$ReviewRowId = 'PBH-010'
$BacklogId = 'RB-06808'
$OldDir = 'Data/Scripts/Custom/Invasion System'
$TargetDir = 'Data/Scripts/Custom/Events/InvasionSystem'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-invasion-gate-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-invasion-gate-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'

$BacklogRows = Get-CsvRows -Path (Join-Path $OutputDir 'repair-backlog.csv')
$BacklogMatches = @($BacklogRows | Where-Object { $_.Id -eq $BacklogId })

if ($BacklogMatches.Count -ne 1)
{
    throw "Expected one $BacklogId backlog row, found $($BacklogMatches.Count)."
}

$BacklogRow = $BacklogMatches[0]
$SourcePath = Join-Path $RepoRoot $OldDir
$TargetPath = Join-Path $RepoRoot $TargetDir

if (-not (Test-Path -LiteralPath $SourcePath -PathType Container))
{
    throw "Invasion source directory missing: $OldDir"
}

if (Test-Path -LiteralPath $TargetPath)
{
    throw "Invasion target already exists; gate review expected no move yet: $TargetDir"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath $SourcePath -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFiles = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' })
$NestedAgentRows = @($WorkspaceFiles | Where-Object { $_ -like '*/AGENTS.md' })

if ($WorkspaceFiles.Count -ne 102 -or $RuntimeFiles.Count -ne 100 -or $NestedAgentRows.Count -ne 1)
{
    throw "Unexpected Invasion workspace shape. Files=$($WorkspaceFiles.Count) Runtime=$($RuntimeFiles.Count) NestedAgents=$($NestedAgentRows.Count)"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeRows.Count -ne 100)
{
    throw "Expected 100 Invasion runtime inventory rows, found $($RuntimeRows.Count)."
}

$ProjectTruth = Get-CsvRows -Path (Join-Path $OutputDir 'project-truth-register.csv')
$ProjectRows = @($ProjectTruth | Where-Object { $_.Path.Contains($OldDir + '/') -or $_.IncludePath.Contains('Custom\Invasion System\') })
$ProjectIncludes = @($ProjectRows | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ProjectSources = @($ProjectRows | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count

if ($ProjectIncludes -ne 100 -or $ProjectSources -ne 100 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0)
{
    throw "Unexpected Invasion project truth evidence. Includes=$ProjectIncludes Sources=$ProjectSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 76)
{
    throw "Expected 76 Invasion serialization rows, found $($SerializationRows.Count)."
}

$SerializationSummary = (($SerializationRows | Group-Object VersionHandling,FieldAlignment,MoveRenameRisk | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$SerializerGateRows = @($SerializationRows | Where-Object { $_.ReviewStatus -eq 'NeedsSourceReview' -or $_.VersionHandling -eq 'NoVersionFound' -or $_.MoveRenameRisk -eq 'UnknownUntilSaveTest' })
$SerializerGateEvidence = (($SerializerGateRows | Sort-Object File,Class | ForEach-Object { '{0}:{1}:{2}:{3}' -f $_.Class, $_.VersionHandling, $_.FieldAlignment, $_.MoveRenameRisk }) -join '; ')

if ($SerializerGateRows.Count -lt 2)
{
    throw "Expected Invasion serializer gate evidence to include at least two caution rows, found $($SerializerGateRows.Count)."
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 172 -or $HookSummary -ne 'Command=1; Gump=153; Initialize=1; Movement=3; Timer=14')
{
    throw "Unexpected Invasion hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$ActiveRowsAll = Get-CsvRows -Path $ActivePath
$RequiredGateRows = @(
    @{ BacklogId = 'RB-05605'; Expected = 'DeferredBalanceDecision'; Label = 'Random Encounters <-> Invasion balance/policy' },
    @{ BacklogId = 'RB-05620'; Expected = 'DeferredBalanceDecision'; Label = 'Invasion <-> Champions balance/policy' },
    @{ BacklogId = 'RB-05630'; Expected = 'NeedsHumanDecision'; Label = 'Invasion staff workflow ownership' },
    @{ BacklogId = 'RB-05632'; Expected = 'NeedsHumanDecision'; Label = 'Government <-> Invasion staff workflow ownership' },
    @{ BacklogId = 'RB-05634'; Expected = 'NeedsHumanDecision'; Label = 'XMLSpawner <-> Invasion staff workflow ownership' },
    @{ BacklogId = 'RB-05645'; Expected = 'NeedsHumanDecision'; Label = 'Invasion staff tooling policy' }
)

$GateEvidence = New-Object System.Collections.Generic.List[string]

foreach ($required in $RequiredGateRows)
{
    $row = @($ActiveRowsAll | Where-Object { $_.BacklogId -eq $required.BacklogId })

    if ($row.Count -ne 1)
    {
        throw "Expected one gate row $($required.BacklogId), found $($row.Count)."
    }

    if ($row[0].ActiveStatus -ne $required.Expected)
    {
        throw "Unexpected gate row status for $($required.BacklogId): $($row[0].ActiveStatus), expected $($required.Expected)."
    }

    $GateEvidence.Add(('{0}={1} ({2}, {3})' -f $required.BacklogId, $row[0].ActiveStatus, $required.Label, $row[0].ReviewRowId)) | Out-Null
}

$GateEvidenceText = ($GateEvidence -join '; ')
$RemainingBlockers = (($RequiredGateRows | ForEach-Object { '{0}:{1}' -f $_.BacklogId, $_.Label }) -join '; ')

$SourceEvidence = "Backlog gate requires staff workflow docs and serializer review before moving $OldDir. Current source/project evidence is stable for a possible future file-location-only move: source path exists with $($RuntimeFiles.Count) runtime-visible C# files and $($WorkspaceFiles.Count) total package files; target path $TargetDir does not exist; RuntimeScriptCompileTruth inventory has $($RuntimeRows.Count) old-path rows; ScriptsProjectTruth has $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, and 0 unincluded sources; runtime-hook-map.csv has $($HookRows.Count) Invasion rows ($HookSummary); serialization-register.csv has $($SerializationRows.Count) Invasion rows ($SerializationSummary). Gate evidence is not clear for execution because serializer caution rows remain ($SerializerGateEvidence) and related staff/balance rows remain deferred or human-gated: $GateEvidenceText."
$Action = "Did not move Invasion. Recorded DeferredMoveGate for $BacklogId because the Phase 12 approval gate requires staff workflow docs plus serializer review, and current audit state still contains serializer caution rows plus deferred/human-gated staff and balance decisions. Preserved current source path, project paths, namespaces, serialized type names, save versions, commands, hooks, XML/config contents, public APIs, and gameplay behavior. Next safe action is a human staff/workflow policy decision and focused serializer source review before rerunning this move gate."
$Verification = "Audit-only gate review. Confirmed source path exists, target path absent, $($RuntimeFiles.Count) runtime-visible C# files, $($WorkspaceFiles.Count) total package files, one nested Gumps AGENTS.md scope, $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, 0 unincluded sources, $($SerializationRows.Count) serialization rows, and $($HookRows.Count) runtime hook rows ($HookSummary). No source, project, XML/config, namespace, save, or runtime behavior changed. git diff --check: $DiffCheckResult."
$Notes = "Deferred gate blockers: $RemainingBlockers. Serializer caution evidence: $SerializerGateEvidence. This disposition does not reject the Phase 12 target; it prevents a silent high-risk package move while staff workflow and serializer-review gates remain unresolved."

$ReviewRow = [pscustomobject]@{
    BatchId = 'POST-BATCH-H'
    ReviewRowId = $ReviewRowId
    BacklogId = $BacklogRow.Id
    SourceId = $BacklogRow.SourceId
    Priority = $BacklogRow.Priority
    HistoricalStatus = $BacklogRow.Status
    Category = $BacklogRow.Category
    System = $BacklogRow.System
    OriginalFiles = $BacklogRow.Files
    TargetFiles = $TargetDir
    OriginalEvidence = $BacklogRow.Evidence
    Risk = $BacklogRow.Risk
    RecommendedFix = $BacklogRow.RecommendedFix
    Decision = 'DeferredMoveGate'
    SourceEvidence = $SourceEvidence
    Action = $Action
    Verification = $Verification
    Rollback = 'No file move was performed. Rollback is removing this gate-disposition artifact and active overlay row if a later staff/workflow and serializer decision authorizes a move batch.'
    ReviewedBatchId = $ReviewBatchId
    ReviewedAt = $Timestamp
    Notes = $Notes
}

@($ReviewRow) | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 1)
{
    throw 'Review CSV row count invariant failed.'
}

$ActiveRows = @($ActiveRowsAll | Where-Object { $_.BacklogId -ne $BacklogId })
$OverlayRow = [pscustomobject]@{
    BacklogId = $ReviewRow.BacklogId
    SourceId = $ReviewRow.SourceId
    Category = $ReviewRow.Category
    System = $ReviewRow.System
    Files = $ReviewRow.OriginalFiles
    HistoricalStatus = $ReviewRow.HistoricalStatus
    ActiveStatus = 'DeferredMoveGate'
    PostAuditBatch = 'POST-BATCH-H'
    ReviewRowId = $ReviewRow.ReviewRowId
    ReviewStatus = $ReviewRow.Decision
    Action = $ReviewRow.Action
    ReviewArtifact = $ReviewRel
    SourceEvidence = $ReviewRow.SourceEvidence
    Commit = 'Pending current POST-BATCH-H-10A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($ActiveRows + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$Closeout = @(
    '# POST-BATCH-H Invasion Gate Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-10A did not move `{0}` to `{1}`. The row was classified `DeferredMoveGate` because its Phase 12 gate requires staff workflow documentation plus serializer review, and current audit state still contains human-gated staff workflow rows plus serializer caution evidence." -f $OldDir, $TargetDir),
    '',
    'No source, project, XML/config, namespace, serialization, command, hook, public API, or gameplay behavior changed.',
    '',
    '## Evidence',
    '',
    '| Evidence | Value |',
    '| --- | --- |',
    ('| Backlog row | `{0}` |' -f $BacklogId),
    ('| Current source path | `{0}` |' -f $OldDir),
    ('| Proposed target path | `{0}` |' -f $TargetDir),
    ('| Runtime-visible C# files | `{0}` |' -f $RuntimeFiles.Count),
    ('| Total package files | `{0}` |' -f $WorkspaceFiles.Count),
    ('| Nested AGENTS.md scopes | `{0}` |' -f ($NestedAgentRows -join '; ')),
    ('| RuntimeScriptCompileTruth rows | `{0}` old-path rows, `{1}` total runtime-visible rows |' -f $RuntimeRows.Count, $RuntimeInventory.Count),
    ('| ScriptsProjectTruth rows | `{0}` includes, `{1}` sources, `0` missing, `0` unincluded |' -f $ProjectIncludes, $ProjectSources),
    ('| Runtime hook rows | `{0}` ({1}) |' -f $HookRows.Count, $HookSummary),
    ('| Serialization rows | `{0}` ({1}) |' -f $SerializationRows.Count, $SerializationSummary),
    '',
    '## Gate Decision',
    '',
    ('- Current staff/balance gate rows: {0}' -f $GateEvidenceText),
    ('- Serializer caution rows: {0}' -f $SerializerGateEvidence),
    '- Decision: `DeferredMoveGate`.',
    '',
    '## Verification',
    '',
    '- No path-sensitive outputs were regenerated because no move was executed.',
    '- RuntimeScriptCompileTruth and ScriptsProjectTruth remain unchanged and explicitly separate.',
    "- git diff --check: $DiffCheckResult",
    '',
    '## Next Safe Action',
    '',
    'Resolve the staff workflow ownership decisions and complete focused serializer source review, then rerun the Invasion move gate before any file move. Continue POST-BATCH-H with the next eligible folder cleanup row while this row remains dispositioned.'
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-invasion-gate-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-invasion-gate-review.csv` | Post-audit | Review and DeferredMoveGate disposition `RB-06808` for the POST-BATCH-H-10A Invasion reorganization gate. | Complete |',
    '| `post-batch-h-invasion-gate-closeout.md` | Post-audit | Close out the Invasion move gate with project truth, runtime hook, serialization, staff workflow blocker, and next-action evidence. | Complete |'
)
$InitialIndex = [Array]::IndexOf($Readme, '## Initial State')

if ($InitialIndex -lt 0)
{
    throw 'Could not find README Initial State heading.'
}

$Before = if ($InitialIndex -gt 0) { $Readme[0..($InitialIndex - 1)] } else { @() }
$After = $Readme[$InitialIndex..($Readme.Count - 1)]
Write-Utf8File -Path $ReadmePath -Lines @($Before + $NewEntries + '' + $After)

$PhaseStatus = Get-Content -Raw -LiteralPath $PhaseStatusPath
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Last updated: .*', "Last updated: $Timestamp", 1)
$StatusLine = 'Post-audit reorganization runner: `{0}` classified Invasion `{1}` as `DeferredMoveGate` rather than moving to `{2}`. The package has {3} runtime-visible `.cs` files, {4} project include rows, {5} serializer rows, and {6} runtime hook rows, but the Phase 12 move gate remains blocked by staff workflow and serializer review evidence ({7}). Verification: git diff --check={8}; no source/project/runtime files changed.' -f $ReviewBatchId, $OldDir, $TargetDir, $RuntimeFiles.Count, $ProjectIncludes, $SerializationRows.Count, $HookRows.Count, $RemainingBlockers, $DiffCheckResult

if ($PhaseStatus -match 'Post-audit reorganization runner:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit reorganization runner:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $StatusLine }, 1)
}
elseif ($PhaseStatus -match '\r?\nScope:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, '(\r?\nScope:)', "`n$StatusLine`n`$1", 1)
}
else
{
    $PhaseStatus = $PhaseStatus.TrimEnd() + "`n`n$StatusLine`n"
}

[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLogEntry = @(
    "### $Timestamp",
    '',
    ('- Affected phase: Post-audit `{0}` Invasion reorganization gate review' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: review Invasion folder move gate, source/project/serialization/hook evidence, and related staff workflow rows; run `New-PostBatchHInvasionGateReview.ps1`.',
    ('- Result: Classified {0} as DeferredMoveGate. Source path exists with {1} runtime-visible C# files and {2} total files; target path absent; project truth has {3} includes, {4} sources, {5} missing targets, and {6} unincluded sources; serialization register has {7} rows; runtime hook map has {8} rows ({9}); remaining blockers are {10}; serializer caution rows are {11}. Verification: git diff --check={12}.' -f $BacklogId, $RuntimeFiles.Count, $WorkspaceFiles.Count, $ProjectIncludes, $ProjectSources, $MissingCompileTargets, $UnincludedSources, $SerializationRows.Count, $HookRows.Count, $HookSummary, $RemainingBlockers, $SerializerGateEvidence, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-10A` Invasion reorganization gate review\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewPath = $ReviewPath
    CloseoutPath = $CloseoutPath
    BacklogId = $BacklogId
    ActiveStatus = 'DeferredMoveGate'
    RuntimeFiles = $RuntimeFiles.Count
    ProjectIncludes = $ProjectIncludes
    SerializationRows = $SerializationRows.Count
    HookRows = $HookRows.Count
    HookSummary = $HookSummary
    RemainingBlockers = $RemainingBlockers
}
