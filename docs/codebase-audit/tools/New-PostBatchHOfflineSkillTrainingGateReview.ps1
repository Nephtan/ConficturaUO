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
$ReviewBatchId = 'POST-BATCH-H-13A'
$ReviewRowId = 'PBH-013'
$BacklogId = 'RB-06803'
$OldDir = 'Data/Scripts/Custom/Offline Skill Training'
$TargetDir = 'Data/Scripts/Custom/Progression/OfflineSkillTraining'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-offline-skill-training-gate-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-offline-skill-training-gate-closeout.md'
$ReviewPath = Join-Path $RepoRoot $ReviewRel
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$ScriptsProjectPath = Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj'

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
    throw "Offline Skill Training source directory missing: $OldDir"
}

if (Test-Path -LiteralPath $TargetPath)
{
    throw "Offline Skill Training target already exists; gate review expected no move yet: $TargetDir"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath $SourcePath -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFiles = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' })
$NestedAgentRows = @($WorkspaceFiles | Where-Object { $_ -like '*/AGENTS.md' })

if ($WorkspaceFiles.Count -ne 8 -or $RuntimeFiles.Count -ne 7 -or $NestedAgentRows.Count -ne 0)
{
    throw "Unexpected Offline Skill Training workspace shape. Files=$($WorkspaceFiles.Count) Runtime=$($RuntimeFiles.Count) NestedAgents=$($NestedAgentRows.Count)"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeRows.Count -ne 7)
{
    throw "Expected 7 Offline Skill Training runtime inventory rows, found $($RuntimeRows.Count)."
}

$ProjectTruth = Get-CsvRows -Path (Join-Path $OutputDir 'project-truth-register.csv')
$ProjectRows = @($ProjectTruth | Where-Object { $_.Path.Contains($OldDir + '/') -or $_.IncludePath.Contains('Custom\Offline Skill Training\') })
$ProjectIncludes = @($ProjectRows | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ProjectSources = @($ProjectRows | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count

if ($ProjectIncludes -ne 7 -or $ProjectSources -ne 7 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0)
{
    throw "Unexpected Offline Skill Training project truth evidence. Includes=$ProjectIncludes Sources=$ProjectSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath $ScriptsProjectPath
$ScriptsCompileRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('Compile Include="Custom\Offline Skill Training\'))).Count
$ScriptsContentRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('Content Include="Custom\Offline Skill Training\'))).Count
$ScriptsNoneRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('None Include="Custom\Offline Skill Training\'))).Count

if ($ScriptsCompileRows -ne 7 -or $ScriptsContentRows -ne 1 -or $ScriptsNoneRows -ne 0)
{
    throw "Unexpected Scripts.csproj package rows. Compile=$ScriptsCompileRows Content=$ScriptsContentRows None=$ScriptsNoneRows"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 170)
{
    throw "Expected 170 Offline Skill Training serialization rows, found $($SerializationRows.Count)."
}

$SerializationSummary = (($SerializationRows | Group-Object VersionHandling,FieldAlignment,MoveRenameRisk | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$NeedsSourceReview = @($SerializationRows | Where-Object { $_.ReviewStatus -eq 'NeedsSourceReview' }).Count
$UnknownUntilSaveTest = @($SerializationRows | Where-Object { $_.MoveRenameRisk -eq 'UnknownUntilSaveTest' }).Count
$NamespaceOrTypeRenameDanger = @($SerializationRows | Where-Object { $_.MoveRenameRisk -eq 'NamespaceOrTypeRenameDanger' }).Count

if ($NeedsSourceReview -ne 166 -or $UnknownUntilSaveTest -ne 166 -or $NamespaceOrTypeRenameDanger -ne 4)
{
    throw "Unexpected Offline Skill Training serializer gate evidence. NeedsSourceReview=$NeedsSourceReview UnknownUntilSaveTest=$UnknownUntilSaveTest NamespaceOrTypeRenameDanger=$NamespaceOrTypeRenameDanger"
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 8 -or $HookSummary -ne 'Event=4; Initialize=1; Login=1; Logout=1; Timer=1')
{
    throw "Unexpected Offline Skill Training hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$ActiveRowsAll = Get-CsvRows -Path $ActivePath
$RelatedGateRows = @(
    @{ BacklogId = 'RB-05603'; Expected = 'DeferredBalanceDecision'; Label = 'Character Level <-> Offline Skill Training balance/policy' },
    @{ BacklogId = 'RB-05608'; Expected = 'DeferredBalanceDecision'; Label = 'Random Encounters <-> Offline Skill Training balance/policy' },
    @{ BacklogId = 'RB-06737'; Expected = 'QueuedSourceFollowUp'; Label = 'Offline Skill Training canonical documentation source trace' }
)

$GateEvidence = New-Object System.Collections.Generic.List[string]

foreach ($required in $RelatedGateRows)
{
    $row = @($ActiveRowsAll | Where-Object { $_.BacklogId -eq $required.BacklogId })

    if ($row.Count -ne 1)
    {
        throw "Expected one related gate row $($required.BacklogId), found $($row.Count)."
    }

    if ($row[0].ActiveStatus -ne $required.Expected)
    {
        throw "Unexpected related gate row status for $($required.BacklogId): $($row[0].ActiveStatus), expected $($required.Expected)."
    }

    $GateEvidence.Add(('{0}={1} ({2}, {3})' -f $required.BacklogId, $row[0].ActiveStatus, $required.Label, $row[0].ReviewRowId)) | Out-Null
}

$RuntimeHookDispositionRows = @($ActiveRowsAll | Where-Object { $_.BacklogId -in @('RB-01712', 'RB-01713', 'RB-01714', 'RB-01715') })
$RuntimeHookDispositionText = (($RuntimeHookDispositionRows | Sort-Object BacklogId | ForEach-Object { '{0}={1} ({2})' -f $_.BacklogId, $_.ActiveStatus, $_.ReviewRowId }) -join '; ')
$GateEvidenceText = ($GateEvidence -join '; ')
$RemainingBlockers = (($RelatedGateRows | ForEach-Object { '{0}:{1}' -f $_.BacklogId, $_.Label }) -join '; ')

$SourceEvidence = "Backlog gate requires serializer review and project truth repair before moving $OldDir. Project truth is repaired, but the serializer gate is not cleared: source path exists with $($RuntimeFiles.Count) runtime-visible C# files and $($WorkspaceFiles.Count) total package files; target path $TargetDir does not exist; RuntimeScriptCompileTruth inventory has $($RuntimeRows.Count) old-path rows; ScriptsProjectTruth has $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, and 0 unincluded sources; Scripts.csproj has $ScriptsCompileRows Compile rows, $ScriptsContentRows Content row, and $ScriptsNoneRows None rows under the current package path; runtime-hook-map.csv has $($HookRows.Count) Offline Skill Training rows ($HookSummary); serialization-register.csv has $($SerializationRows.Count) Offline Skill Training rows, with $NeedsSourceReview NeedsSourceReview rows, $UnknownUntilSaveTest UnknownUntilSaveTest rows, and $NamespaceOrTypeRenameDanger NamespaceOrTypeRenameDanger rows ($SerializationSummary). Gate evidence is not clear for execution because serializer source review remains unresolved and related balance/doc rows remain deferred or queued: $GateEvidenceText. Previously reviewed hook rows are not the remaining blocker: $RuntimeHookDispositionText."
$Action = "Did not move Offline Skill Training. Recorded DeferredMoveGate for $BacklogId because the Phase 12/repair-backlog gate allows the move only after serializer review and project truth repair; project truth is clean, but current serialization evidence still has $NeedsSourceReview rows requiring source review, $UnknownUntilSaveTest UnknownUntilSaveTest rows, and $NamespaceOrTypeRenameDanger namespace/type rename danger rows. Preserved current source path, project paths, namespaces, serialized type names, save versions, commands, hooks, XML/config contents, public APIs, and gameplay behavior. Next safe action is focused serializer source review for the study-book item hierarchy, then rerun this move gate."
$Verification = "Audit-only serializer gate review. Confirmed source path exists, target path absent, $($RuntimeFiles.Count) runtime-visible C# files, $($WorkspaceFiles.Count) total package files, no nested AGENTS.md scopes, $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, 0 unincluded sources, $ScriptsCompileRows Scripts.csproj Compile rows, $ScriptsContentRows Content row, $($SerializationRows.Count) serialization rows, and $($HookRows.Count) runtime hook rows ($HookSummary). No source, project, XML/config, namespace, save, or runtime behavior changed. git diff --check: $DiffCheckResult."
$Notes = "Deferred move gate. Project truth repair is satisfied, but serializer review is not: $NeedsSourceReview serializer rows require source review, $UnknownUntilSaveTest rows remain UnknownUntilSaveTest, and $NamespaceOrTypeRenameDanger rows are namespace/type rename danger rows. Related unresolved rows: $RemainingBlockers. This disposition does not reject the Phase 12 target; it prevents a silent move while save-review and related documentation/balance evidence remain unresolved."

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
    Rollback = 'No file move was performed. Rollback is removing this gate-disposition artifact and active overlay row if later serializer review authorizes a move batch.'
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
    Commit = 'Pending current POST-BATCH-H-13A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($ActiveRows + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$Closeout = @(
    '# POST-BATCH-H Offline Skill Training Gate Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-13A did not move `{0}` to `{1}`. The row was classified `DeferredMoveGate` because the backlog gate requires serializer review plus project truth repair, and only the project-truth side is currently satisfied." -f $OldDir, $TargetDir),
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
    ('| Nested AGENTS.md scopes | `{0}` |' -f $NestedAgentRows.Count),
    ('| RuntimeScriptCompileTruth rows | `{0}` old-path rows, `{1}` total runtime-visible rows |' -f $RuntimeRows.Count, $RuntimeInventory.Count),
    ('| ScriptsProjectTruth rows | `{0}` includes, `{1}` sources, `0` missing, `0` unincluded |' -f $ProjectIncludes, $ProjectSources),
    ('| Scripts.csproj package rows | `{0}` Compile, `{1}` Content, `{2}` None |' -f $ScriptsCompileRows, $ScriptsContentRows, $ScriptsNoneRows),
    ('| Runtime hook rows | `{0}` ({1}) |' -f $HookRows.Count, $HookSummary),
    ('| Serialization rows | `{0}` total, `{1}` NeedsSourceReview, `{2}` UnknownUntilSaveTest, `{3}` NamespaceOrTypeRenameDanger |' -f $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $NamespaceOrTypeRenameDanger),
    '',
    '## Gate Decision',
    '',
    ('- Related balance/doc rows: {0}' -f $GateEvidenceText),
    ('- Previously reviewed runtime hook rows: {0}' -f $RuntimeHookDispositionText),
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
    'Complete focused serializer source review for the study-book item hierarchy, explicitly preserve namespace/type/save-version behavior, then rerun this move gate. Continue POST-BATCH-H with the next eligible folder cleanup row while this row remains dispositioned.'
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-offline-skill-training-gate-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-offline-skill-training-gate-review.csv` | Post-audit | Review and DeferredMoveGate disposition `RB-06803` for the POST-BATCH-H-13A Offline Skill Training serializer gate. | Complete |',
    '| `post-batch-h-offline-skill-training-gate-closeout.md` | Post-audit | Close out the Offline Skill Training move gate with project truth, runtime hook, serialization, balance/doc blocker, and next-action evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` classified Offline Skill Training `{1}` as `DeferredMoveGate` rather than moving to `{2}`. Project truth is clean ({3} include rows, {4} source rows, 0 missing, 0 unincluded), but serializer review is not cleared ({5} rows, {6} NeedsSourceReview, {7} UnknownUntilSaveTest, {8} NamespaceOrTypeRenameDanger); related balance/doc rows remain deferred or queued ({9}). Verification: git diff --check={10}; no source/project/runtime files changed.' -f $ReviewBatchId, $OldDir, $TargetDir, $ProjectIncludes, $ProjectSources, $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $NamespaceOrTypeRenameDanger, $RemainingBlockers, $DiffCheckResult

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
    ('- Affected phase: Post-audit `{0}` Offline Skill Training reorganization gate review' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: review Offline Skill Training folder move gate, source/project/serialization/hook evidence, and related balance/doc rows; run `New-PostBatchHOfflineSkillTrainingGateReview.ps1`.',
    ('- Result: Classified {0} as DeferredMoveGate. Source path exists with {1} runtime-visible C# files and {2} total files; target path absent; project truth has {3} includes, {4} sources, {5} missing targets, and {6} unincluded sources; Scripts.csproj has {7} Compile rows and {8} Content row under the current package path; serialization register has {9} rows ({10} NeedsSourceReview, {11} UnknownUntilSaveTest, {12} NamespaceOrTypeRenameDanger); runtime hook map has {13} rows ({14}); remaining blockers are {15}. Verification: git diff --check={16}.' -f $BacklogId, $RuntimeFiles.Count, $WorkspaceFiles.Count, $ProjectIncludes, $ProjectSources, $MissingCompileTargets, $UnincludedSources, $ScriptsCompileRows, $ScriptsContentRows, $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $NamespaceOrTypeRenameDanger, $HookRows.Count, $HookSummary, $RemainingBlockers, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-13A` Offline Skill Training reorganization gate review\r?\n.*?(?=^### |\z)'
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
