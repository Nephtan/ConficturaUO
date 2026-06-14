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
$ReviewBatchId = 'POST-BATCH-H-12A'
$ReviewRowId = 'PBH-012'
$BacklogId = 'RB-06807'
$OldDir = 'Data/Scripts/Custom/Government System'
$TargetDir = 'Data/Scripts/Custom/Government/GovernmentSystem'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-government-gate-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-government-gate-closeout.md'
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
    throw "Government source directory missing: $OldDir"
}

if (Test-Path -LiteralPath $TargetPath)
{
    throw "Government target already exists; gate review expected no move yet: $TargetDir"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath $SourcePath -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFiles = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' })
$NestedAgentRows = @($WorkspaceFiles | Where-Object { $_ -like '*/AGENTS.md' })

if ($WorkspaceFiles.Count -ne 208 -or $RuntimeFiles.Count -ne 207 -or $NestedAgentRows.Count -ne 1)
{
    throw "Unexpected Government workspace shape. Files=$($WorkspaceFiles.Count) Runtime=$($RuntimeFiles.Count) NestedAgents=$($NestedAgentRows.Count)"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeRows.Count -ne 207)
{
    throw "Expected 207 Government runtime inventory rows, found $($RuntimeRows.Count)."
}

$ProjectTruth = Get-CsvRows -Path (Join-Path $OutputDir 'project-truth-register.csv')
$ProjectRows = @($ProjectTruth | Where-Object { $_.Path.Contains($OldDir + '/') -or $_.IncludePath.Contains('Custom\Government System\') })
$ProjectIncludes = @($ProjectRows | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ProjectSources = @($ProjectRows | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count

if ($ProjectIncludes -ne 207 -or $ProjectSources -ne 207 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0)
{
    throw "Unexpected Government project truth evidence. Includes=$ProjectIncludes Sources=$ProjectSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 144)
{
    throw "Expected 144 Government serialization rows, found $($SerializationRows.Count)."
}

$SerializationSummary = (($SerializationRows | Group-Object VersionHandling,FieldAlignment,MoveRenameRisk | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$NeedsSourceReview = @($SerializationRows | Where-Object { $_.ReviewStatus -eq 'NeedsSourceReview' }).Count
$UnknownUntilSaveTest = @($SerializationRows | Where-Object { $_.MoveRenameRisk -eq 'UnknownUntilSaveTest' }).Count
$NoVersionFound = @($SerializationRows | Where-Object { $_.VersionHandling -eq 'NoVersionFound' }).Count

if ($NeedsSourceReview -ne 144 -or $UnknownUntilSaveTest -ne 144 -or $NoVersionFound -ne 1)
{
    throw "Unexpected Government serializer gate evidence. NeedsSourceReview=$NeedsSourceReview UnknownUntilSaveTest=$UnknownUntilSaveTest NoVersionFound=$NoVersionFound"
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 227 -or $HookSummary -ne 'Command=8; Gump=190; Initialize=9; Region=5; Speech=10; Timer=4; WorldSave=1')
{
    throw "Unexpected Government hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$ActiveRowsAll = Get-CsvRows -Path $ActivePath
$RequiredGateRows = @(
    @{ BacklogId = 'RB-05609'; Expected = 'DeferredBalanceDecision'; Label = 'PvP Consent <-> Government balance/policy' },
    @{ BacklogId = 'RB-05611'; Expected = 'DeferredBalanceDecision'; Label = 'Government <-> Homestead balance/policy' },
    @{ BacklogId = 'RB-05612'; Expected = 'DeferredBalanceDecision'; Label = 'Government <-> Vendor Core balance/policy' },
    @{ BacklogId = 'RB-05632'; Expected = 'NeedsHumanDecision'; Label = 'Government <-> Invasion staff workflow ownership' },
    @{ BacklogId = 'RB-06806'; Expected = 'DeferredMoveGate'; Label = 'PvP Consent move gate remains deferred' },
    @{ BacklogId = 'RB-06808'; Expected = 'DeferredMoveGate'; Label = 'Invasion move gate remains deferred' }
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

$QueuedFollowupCount = @($ActiveRowsAll | Where-Object {
    ($_.System -like '*Government*' -or $_.Files -like '*Government*' -or $_.SourceEvidence -like '*Government*') -and
    $_.ActiveStatus -eq 'QueuedSourceFollowUp'
}).Count
$GateEvidenceText = ($GateEvidence -join '; ')
$RemainingBlockers = (($RequiredGateRows | ForEach-Object { '{0}:{1}' -f $_.BacklogId, $_.Label }) -join '; ')

$SourceEvidence = "Backlog gate explicitly requires approval before moving $OldDir because serialized file volume plus gump/hook volume are high. Current source/project evidence is stable for review only: source path exists with $($RuntimeFiles.Count) runtime-visible C# files and $($WorkspaceFiles.Count) total package files; target path $TargetDir does not exist; RuntimeScriptCompileTruth inventory has $($RuntimeRows.Count) old-path rows; ScriptsProjectTruth has $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, and 0 unincluded sources; runtime-hook-map.csv has $($HookRows.Count) Government rows ($HookSummary); serialization-register.csv has $($SerializationRows.Count) Government rows, with $NeedsSourceReview NeedsSourceReview rows, $UnknownUntilSaveTest UnknownUntilSaveTest rows, and $NoVersionFound NoVersionFound row ($SerializationSummary). Gate evidence is not clear for execution because explicit approval is absent and related policy/move rows remain deferred or human-gated: $GateEvidenceText. Additional Government source/schema/doc follow-up rows still queued: $QueuedFollowupCount."
$Action = "Did not move Government. Recorded NeedsHumanDecision for $BacklogId because the Phase 12 approval gate explicitly requires human package/save approval and current audit state still contains serializer, gump, hook, policy, balance, region, and schema follow-up risk. Preserved current source path, project paths, namespaces, serialized type names, save versions, commands, hooks, XML/config contents, public APIs, and gameplay behavior. Next safe action is explicit human approval with a move plan that covers serializer source review, world-save/region/gump hooks, policy dependencies, project includes, docs/source traces, verification, and rollback."
$Verification = "Audit-only explicit approval gate review. Confirmed source path exists, target path absent, $($RuntimeFiles.Count) runtime-visible C# files, $($WorkspaceFiles.Count) total package files, one nested Gumps AGENTS.md scope, $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, 0 unincluded sources, $($SerializationRows.Count) serialization rows, and $($HookRows.Count) runtime hook rows ($HookSummary). No source, project, XML/config, namespace, save, or runtime behavior changed. git diff --check: $DiffCheckResult."
$Notes = "Needs human decision before any move. Explicit approval blockers: $RemainingBlockers. Serializer/package evidence: $NeedsSourceReview serializer rows require source review and $UnknownUntilSaveTest rows remain UnknownUntilSaveTest. This disposition does not reject the Phase 12 target; it prevents a silent high-risk Government package move while approval and save/runtime gates remain unresolved."

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
    Decision = 'NeedsHumanDecision'
    SourceEvidence = $SourceEvidence
    Action = $Action
    Verification = $Verification
    Rollback = 'No file move was performed. Rollback is removing this gate-disposition artifact and active overlay row if later explicit human approval authorizes a move batch.'
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
    ActiveStatus = 'NeedsHumanDecision'
    PostAuditBatch = 'POST-BATCH-H'
    ReviewRowId = $ReviewRow.ReviewRowId
    ReviewStatus = $ReviewRow.Decision
    Action = $ReviewRow.Action
    ReviewArtifact = $ReviewRel
    SourceEvidence = $ReviewRow.SourceEvidence
    Commit = 'Pending current POST-BATCH-H-12A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($ActiveRows + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$Closeout = @(
    '# POST-BATCH-H Government Gate Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-12A did not move `{0}` to `{1}`. The row was classified `NeedsHumanDecision` because the Phase 12 gate explicitly requires approval and current audit state still contains serializer, gump, hook, region, policy, balance, and XML/config follow-up risk." -f $OldDir, $TargetDir),
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
    ('| Serialization rows | `{0}` total, `{1}` NeedsSourceReview, `{2}` UnknownUntilSaveTest, `{3}` NoVersionFound |' -f $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $NoVersionFound),
    '',
    '## Gate Decision',
    '',
    ('- Current policy/move gate rows: {0}' -f $GateEvidenceText),
    ('- Additional Government source/schema/doc follow-up rows queued: `{0}`' -f $QueuedFollowupCount),
    '- Decision: `NeedsHumanDecision`.',
    '',
    '## Verification',
    '',
    '- No path-sensitive outputs were regenerated because no move was executed.',
    '- RuntimeScriptCompileTruth and ScriptsProjectTruth remain unchanged and explicitly separate.',
    "- git diff --check: $DiffCheckResult",
    '',
    '## Next Safe Action',
    '',
    'Get explicit human package/save approval and a focused move plan covering serializer source review, world-save/region/gump hooks, policy dependencies, project includes, docs/source traces, verification, and rollback. Continue POST-BATCH-H with the next eligible folder cleanup row while this row remains dispositioned.'
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-government-gate-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-government-gate-review.csv` | Post-audit | Review and NeedsHumanDecision disposition `RB-06807` for the POST-BATCH-H-12A Government reorganization gate. | Complete |',
    '| `post-batch-h-government-gate-closeout.md` | Post-audit | Close out the Government move gate with project truth, runtime hook, serialization, explicit approval blocker, and next-action evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` classified Government `{1}` as `NeedsHumanDecision` rather than moving to `{2}`. The package has {3} runtime-visible `.cs` files, {4} project include rows, {5} serializer rows, and {6} runtime hook rows; the Phase 12 gate explicitly requires human package/save approval because serializer, gump, hook, region, policy, balance, and XML/config risks remain unresolved ({7}). Verification: git diff --check={8}; no source/project/runtime files changed.' -f $ReviewBatchId, $OldDir, $TargetDir, $RuntimeFiles.Count, $ProjectIncludes, $SerializationRows.Count, $HookRows.Count, $RemainingBlockers, $DiffCheckResult

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
    ('- Affected phase: Post-audit `{0}` Government reorganization gate review' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: review Government folder move gate, source/project/serialization/hook evidence, and related explicit approval rows; run `New-PostBatchHGovernmentGateReview.ps1`.',
    ('- Result: Classified {0} as NeedsHumanDecision. Source path exists with {1} runtime-visible C# files and {2} total files; target path absent; project truth has {3} includes, {4} sources, {5} missing targets, and {6} unincluded sources; serialization register has {7} rows ({8} NeedsSourceReview, {9} UnknownUntilSaveTest); runtime hook map has {10} rows ({11}); remaining blockers are {12}; queued Government source/schema/doc follow-ups={13}. Verification: git diff --check={14}.' -f $BacklogId, $RuntimeFiles.Count, $WorkspaceFiles.Count, $ProjectIncludes, $ProjectSources, $MissingCompileTargets, $UnincludedSources, $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $HookRows.Count, $HookSummary, $RemainingBlockers, $QueuedFollowupCount, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-12A` Government reorganization gate review\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    ReviewPath = $ReviewPath
    CloseoutPath = $CloseoutPath
    BacklogId = $BacklogId
    ActiveStatus = 'NeedsHumanDecision'
    RuntimeFiles = $RuntimeFiles.Count
    ProjectIncludes = $ProjectIncludes
    SerializationRows = $SerializationRows.Count
    HookRows = $HookRows.Count
    HookSummary = $HookSummary
    RemainingBlockers = $RemainingBlockers
}
