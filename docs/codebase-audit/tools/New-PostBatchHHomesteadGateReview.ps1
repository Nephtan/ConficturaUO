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
$ReviewBatchId = 'POST-BATCH-H-14A'
$ReviewRowId = 'PBH-014'
$BacklogId = 'RB-06814'
$OldDir = 'Data/Scripts/Custom/Vhaerun''s CRL Homestead System [2.0]'
$TargetDir = 'Data/Scripts/Custom/ThirdParty/Vhaerun''s CRL Homestead System [2.0]'
$EscapedProjectPrefix = 'Custom\Vhaerun%27s CRL Homestead System [2.0]\'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-homestead-gate-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-homestead-gate-closeout.md'
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
    throw "Homestead source directory missing: $OldDir"
}

if (Test-Path -LiteralPath $TargetPath)
{
    throw "Homestead target already exists; gate review expected no move yet: $TargetDir"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath $SourcePath -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFiles = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' })
$NestedAgentRows = @($WorkspaceFiles | Where-Object { $_ -like '*/AGENTS.md' })

if ($WorkspaceFiles.Count -ne 402 -or $RuntimeFiles.Count -ne 388 -or $NestedAgentRows.Count -ne 3)
{
    throw "Unexpected Homestead workspace shape. Files=$($WorkspaceFiles.Count) Runtime=$($RuntimeFiles.Count) NestedAgents=$($NestedAgentRows.Count)"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeRows.Count -ne 388)
{
    throw "Expected 388 Homestead runtime inventory rows, found $($RuntimeRows.Count)."
}

$ProjectTruth = Get-CsvRows -Path (Join-Path $OutputDir 'project-truth-register.csv')
$ProjectRows = @($ProjectTruth | Where-Object { $_.Path.Contains($OldDir + '/') -or $_.IncludePath.Contains('Custom\Vhaerun''s CRL Homestead System [2.0]\') })
$ProjectIncludes = @($ProjectRows | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ProjectSources = @($ProjectRows | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count

if ($ProjectIncludes -ne 388 -or $ProjectSources -ne 388 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0)
{
    throw "Unexpected Homestead project truth evidence. Includes=$ProjectIncludes Sources=$ProjectSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath $ScriptsProjectPath
$ScriptsCompileRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('Compile Include="' + $EscapedProjectPrefix))).Count
$ScriptsContentRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('Content Include="' + $EscapedProjectPrefix))).Count
$ScriptsNoneRows = ([regex]::Matches($ScriptsProjectText, [regex]::Escape('None Include="' + $EscapedProjectPrefix))).Count

if ($ScriptsCompileRows -ne 388 -or $ScriptsContentRows -ne 5 -or $ScriptsNoneRows -ne 6)
{
    throw "Unexpected Scripts.csproj Homestead package rows. Compile=$ScriptsCompileRows Content=$ScriptsContentRows None=$ScriptsNoneRows"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 707)
{
    throw "Expected 707 Homestead serialization rows, found $($SerializationRows.Count)."
}

$SerializationSummary = (($SerializationRows | Group-Object VersionHandling,FieldAlignment,MoveRenameRisk | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$NeedsSourceReview = @($SerializationRows | Where-Object { $_.ReviewStatus -eq 'NeedsSourceReview' }).Count
$UnknownUntilSaveTest = @($SerializationRows | Where-Object { $_.MoveRenameRisk -eq 'UnknownUntilSaveTest' }).Count
$NoVersionFound = @($SerializationRows | Where-Object { $_.VersionHandling -eq 'NoVersionFound' }).Count

if ($NeedsSourceReview -ne 707 -or $UnknownUntilSaveTest -ne 707 -or $NoVersionFound -ne 0)
{
    throw "Unexpected Homestead serializer gate evidence. NeedsSourceReview=$NeedsSourceReview UnknownUntilSaveTest=$UnknownUntilSaveTest NoVersionFound=$NoVersionFound"
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 219 -or $HookSummary -ne 'Command=3; Gump=83; Initialize=7; Timer=126')
{
    throw "Unexpected Homestead hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$ActiveRowsAll = Get-CsvRows -Path $ActivePath
$RequiredGateRows = @(
    @{ BacklogId = 'RB-00011'; Expected = 'Fixed'; Label = 'Homestead project missing-target drift repaired' },
    @{ BacklogId = 'RB-00042'; Expected = 'Fixed'; Label = 'Homestead project unincluded-source drift repaired' },
    @{ BacklogId = 'RB-05611'; Expected = 'DeferredBalanceDecision'; Label = 'Government <-> Homestead balance/policy' },
    @{ BacklogId = 'RB-05613'; Expected = 'DeferredBalanceDecision'; Label = 'Homestead <-> Crafting Core balance/policy' },
    @{ BacklogId = 'RB-05614'; Expected = 'DeferredBalanceDecision'; Label = 'Homestead <-> Harvest System balance/policy' },
    @{ BacklogId = 'RB-05615'; Expected = 'DeferredBalanceDecision'; Label = 'Homestead <-> Bulk Orders balance/policy' },
    @{ BacklogId = 'RB-05616'; Expected = 'DeferredBalanceDecision'; Label = 'Homestead <-> Gardening balance/policy' },
    @{ BacklogId = 'RB-05617'; Expected = 'DeferredBalanceDecision'; Label = 'Homestead <-> Vendor Core balance/policy' },
    @{ BacklogId = 'RB-05633'; Expected = 'NeedsHumanDecision'; Label = 'Homestead <-> Static Gump Tool staff tooling policy' },
    @{ BacklogId = 'RB-06690'; Expected = 'QueuedSourceFollowUp'; Label = 'Farmable Crops source trace' },
    @{ BacklogId = 'RB-06705'; Expected = 'QueuedSourceFollowUp'; Label = 'Homestead canonical source trace' },
    @{ BacklogId = 'RB-06735'; Expected = 'QueuedSourceFollowUp'; Label = 'Offline Skill Training/Homestead doc source trace' },
    @{ BacklogId = 'RB-06807'; Expected = 'NeedsHumanDecision'; Label = 'Government move gate remains human-gated' }
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
$RemainingBlockers = (($RequiredGateRows | Where-Object { $_.Expected -ne 'Fixed' } | ForEach-Object { '{0}:{1}' -f $_.BacklogId, $_.Label }) -join '; ')
$NestedAgentText = ($NestedAgentRows -join '; ')

$SourceEvidence = "Backlog gate explicitly requires approval before moving $OldDir because serializer volume and package substructure are high. Current source/project evidence is stable for review only: source path exists with $($RuntimeFiles.Count) runtime-visible C# files and $($WorkspaceFiles.Count) total package files; target path $TargetDir does not exist; nested package AGENTS scopes are $NestedAgentText; RuntimeScriptCompileTruth inventory has $($RuntimeRows.Count) old-path rows; ScriptsProjectTruth has $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, and 0 unincluded sources; Scripts.csproj has $ScriptsCompileRows URI-escaped Compile rows, $ScriptsContentRows URI-escaped Content rows, and $ScriptsNoneRows URI-escaped None rows under the current package path; runtime-hook-map.csv has $($HookRows.Count) Homestead rows ($HookSummary); serialization-register.csv has $($SerializationRows.Count) Homestead rows, with $NeedsSourceReview NeedsSourceReview rows and $UnknownUntilSaveTest UnknownUntilSaveTest rows ($SerializationSummary). Project drift rows are fixed, but gate evidence is not clear for execution because explicit approval is absent and related balance/staff/doc/move rows remain deferred, queued, or human-gated: $GateEvidenceText."
$Action = "Did not move Homestead. Recorded NeedsHumanDecision for $BacklogId because the Phase 12/repair-backlog gate explicitly requires approval for the high-volume imported package with 707 serialization rows, 219 hook rows, three nested Gump AGENTS scopes, and URI-escaped project paths. Preserved current source path, project paths, namespaces, serialized type names, save versions, commands, hooks, XML/config contents, public APIs, and gameplay behavior. Next safe action is explicit human package/save approval with a move plan that covers serializer source review, package substructure, escaped Scripts.csproj paths, docs/source traces, verification, and rollback."
$Verification = "Audit-only explicit approval gate review. Confirmed source path exists, target path absent, $($RuntimeFiles.Count) runtime-visible C# files, $($WorkspaceFiles.Count) total package files, $($NestedAgentRows.Count) nested AGENTS.md scopes, $ProjectIncludes project include rows, $ProjectSources source rows, 0 missing compile targets, 0 unincluded sources, $ScriptsCompileRows URI-escaped Scripts.csproj Compile rows, $ScriptsContentRows Content rows, $ScriptsNoneRows None rows, $($SerializationRows.Count) serialization rows, and $($HookRows.Count) runtime hook rows ($HookSummary). No source, project, XML/config, namespace, save, or runtime behavior changed. git diff --check: $DiffCheckResult."
$Notes = "Needs human decision before any move. Project truth repair is satisfied, but explicit approval and save/package gates are not: $NeedsSourceReview serializer rows require source review and $UnknownUntilSaveTest rows remain UnknownUntilSaveTest. Remaining blockers: $RemainingBlockers. This disposition does not reject the Phase 12 target; it prevents a silent high-risk imported package move while approval, save-review, staff tooling, balance, documentation, and package-substructure gates remain unresolved."

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
    Commit = 'Pending current POST-BATCH-H-14A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($ActiveRows + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$Closeout = @(
    '# POST-BATCH-H Homestead Gate Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-14A did not move `{0}` to `{1}`. The row was classified `NeedsHumanDecision` because the backlog gate explicitly requires approval for a high-volume imported package with significant serializer, hook, Gump, project-path, documentation, and balance-policy surface." -f $OldDir, $TargetDir),
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
    ('| Nested AGENTS.md scopes | `{0}` |' -f $NestedAgentText),
    ('| RuntimeScriptCompileTruth rows | `{0}` old-path rows, `{1}` total runtime-visible rows |' -f $RuntimeRows.Count, $RuntimeInventory.Count),
    ('| ScriptsProjectTruth rows | `{0}` includes, `{1}` sources, `0` missing, `0` unincluded |' -f $ProjectIncludes, $ProjectSources),
    ('| Scripts.csproj package rows | `{0}` URI-escaped Compile, `{1}` URI-escaped Content, `{2}` URI-escaped None |' -f $ScriptsCompileRows, $ScriptsContentRows, $ScriptsNoneRows),
    ('| Runtime hook rows | `{0}` ({1}) |' -f $HookRows.Count, $HookSummary),
    ('| Serialization rows | `{0}` total, `{1}` NeedsSourceReview, `{2}` UnknownUntilSaveTest |' -f $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest),
    '',
    '## Gate Decision',
    '',
    ('- Current gate rows: {0}' -f $GateEvidenceText),
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
    'Get explicit human package/save approval and a focused move plan covering serializer source review, nested package structure, URI-escaped project paths, docs/source traces, balance/staff policy dependencies, verification, and rollback. This completes the POST-BATCH-H row review sequence once the active overlay and commit hashes are reconciled.'
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-homestead-gate-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-homestead-gate-review.csv` | Post-audit | Review and NeedsHumanDecision disposition `RB-06814` for the POST-BATCH-H-14A Homestead explicit approval gate. | Complete |',
    '| `post-batch-h-homestead-gate-closeout.md` | Post-audit | Close out the Homestead move gate with project truth, runtime hook, serialization, nested AGENTS, explicit approval blocker, and next-action evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` classified Homestead `{1}` as `NeedsHumanDecision` rather than moving to `{2}`. Project truth is clean ({3} include rows, {4} source rows, 0 missing, 0 unincluded), but explicit approval and save/package gates are not cleared ({5} serializer rows, {6} NeedsSourceReview, {7} UnknownUntilSaveTest, {8} hook rows, {9} nested AGENTS scopes; {10}). Verification: git diff --check={11}; no source/project/runtime files changed.' -f $ReviewBatchId, $OldDir, $TargetDir, $ProjectIncludes, $ProjectSources, $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $HookRows.Count, $NestedAgentRows.Count, $RemainingBlockers, $DiffCheckResult

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
    ('- Affected phase: Post-audit `{0}` Homestead reorganization gate review' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: review Homestead folder move gate, source/project/serialization/hook/nested-AGENTS evidence, URI-escaped Scripts.csproj paths, and related explicit approval rows; run `New-PostBatchHHomesteadGateReview.ps1`.',
    ('- Result: Classified {0} as NeedsHumanDecision. Source path exists with {1} runtime-visible C# files and {2} total files; target path absent; nested AGENTS scopes={3}; project truth has {4} includes, {5} sources, {6} missing targets, and {7} unincluded sources; Scripts.csproj has {8} URI-escaped Compile rows, {9} Content rows, and {10} None rows; serialization register has {11} rows ({12} NeedsSourceReview, {13} UnknownUntilSaveTest); runtime hook map has {14} rows ({15}); remaining blockers are {16}. Verification: git diff --check={17}.' -f $BacklogId, $RuntimeFiles.Count, $WorkspaceFiles.Count, $NestedAgentRows.Count, $ProjectIncludes, $ProjectSources, $MissingCompileTargets, $UnincludedSources, $ScriptsCompileRows, $ScriptsContentRows, $ScriptsNoneRows, $SerializationRows.Count, $NeedsSourceReview, $UnknownUntilSaveTest, $HookRows.Count, $HookSummary, $RemainingBlockers, $DiffCheckResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-14A` Homestead reorganization gate review\r?\n.*?(?=^### |\z)'
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
