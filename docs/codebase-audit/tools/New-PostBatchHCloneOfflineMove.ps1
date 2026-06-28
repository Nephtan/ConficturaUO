param(
    [string]$RepoRoot,
    [string]$OutputDir,
    [string]$DiffCheckResult = 'Not run yet',
    [string]$SolutionBuildResult = 'Not run yet',
    [string]$ServerBuildResult = 'Not run yet',
    [string]$CompileOnlyResult = 'Not run yet'
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

function Assert-NoText
{
    param(
        [string[]]$Paths,
        [string[]]$Patterns
    )

    foreach ($path in $Paths)
    {
        $fullPath = Join-Path $RepoRoot $path

        if (-not (Test-Path -LiteralPath $fullPath))
        {
            throw "Required text path missing: $path"
        }

        $text = Get-Content -Raw -LiteralPath $fullPath

        foreach ($pattern in $Patterns)
        {
            if ($text.Contains($pattern))
            {
                throw "Stale text '$pattern' found in $path"
            }
        }
    }
}

$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-H-07A'
$ReviewRowId = 'PBH-007'
$BacklogId = 'RB-06815'
$OldDir = 'Data/Scripts/Custom/CloneOfflinePlayerCharacters'
$TargetDir = 'Data/Scripts/Custom/PvE/CloneOfflinePlayerCharacters'
$OldProjectPrefix = 'Custom\CloneOfflinePlayerCharacters\'
$TargetProjectPrefix = 'Custom\PvE\CloneOfflinePlayerCharacters\'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-clone-offline-move-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-clone-offline-move-closeout.md'
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

if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $TargetDir) -PathType Container))
{
    throw "Moved target directory missing: $TargetDir"
}

if (Test-Path -LiteralPath (Join-Path $RepoRoot $OldDir))
{
    throw "Old source directory still exists after move: $OldDir"
}

$ProjectTruth = Get-CsvRows -Path (Join-Path $OutputDir 'project-truth-register.csv')
$ProjectIncludes = @($ProjectTruth | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ScriptSources = @($ProjectTruth | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count
$ProjectCleanupBacklog = @(Get-CsvRows -Path (Join-Path $OutputDir 'project-cleanup-backlog.csv')).Count
$ProjectTargetRows = @($ProjectTruth | Where-Object { $_.Path.Contains($TargetDir + '/') -or $_.IncludePath.Contains($TargetProjectPrefix) })
$ProjectOldRows = @($ProjectTruth | Where-Object { $_.Path.Contains($OldDir + '/') -or $_.IncludePath.Contains($OldProjectPrefix) })

if ($ProjectIncludes -ne 6581 -or $ScriptSources -ne 6581 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0 -or $ProjectCleanupBacklog -ne 0)
{
    throw "Unexpected project truth counts. Includes=$ProjectIncludes Sources=$ScriptSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources Cleanup=$ProjectCleanupBacklog"
}

if ($ProjectTargetRows.Count -ne 14 -or $ProjectOldRows.Count -ne 0)
{
    throw "Unexpected Clone Offline project truth rows. TargetRows=$($ProjectTargetRows.Count) OldRows=$($ProjectOldRows.Count)"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath (Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj')
$TargetProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($TargetProjectPrefix))).Count
$OldProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($OldProjectPrefix))).Count

if ($TargetProjectRefs -ne 7 -or $OldProjectRefs -ne 0)
{
    throw "Unexpected Scripts.csproj Clone Offline references. Target=$TargetProjectRefs Old=$OldProjectRefs"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeTargetRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($TargetDir + '/') })
$RuntimeOldRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeInventory.Count -ne 6581 -or $RuntimeTargetRows.Count -ne 7 -or $RuntimeOldRows.Count -ne 0)
{
    throw "Unexpected runtime script inventory rows. Total=$($RuntimeInventory.Count) Target=$($RuntimeTargetRows.Count) Old=$($RuntimeOldRows.Count)"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 4)
{
    throw "Expected four Clone Offline serialization rows after move, found $($SerializationRows.Count)."
}

$ExpectedSerializedClasses = @(
    'Server.Custom.Confictura.CloneOfflinePlayerCharacters.BackpackClone',
    'Server.Custom.Confictura.CloneOfflinePlayerCharacters.CharacterClone',
    'Server.Custom.Confictura.CloneOfflinePlayerCharacters.EtherealMountClone',
    'Server.Custom.Confictura.CloneOfflinePlayerCharacters.MountClone'
)

foreach ($className in $ExpectedSerializedClasses)
{
    $classRows = @($SerializationRows | Where-Object { $_.Class -eq $className -and $_.Namespace -eq 'Server.Custom.Confictura.CloneOfflinePlayerCharacters' -and $_.CurrentVersion -eq '0' -and $_.VersionHandling -eq 'WriteVersionOnly' })

    if ($classRows.Count -ne 1)
    {
        throw "Unexpected serialization evidence for $className. Rows=$($classRows.Count)"
    }
}

$SerializedSummary = (($SerializationRows | Sort-Object TypeName | ForEach-Object { '{0}:v{1}:{2}:{3}' -f $_.TypeName, $_.CurrentVersion, $_.VersionHandling, $_.FieldAlignment }) -join '; ')

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 8 -or $HookSummary -ne 'Command=1; Event=2; Initialize=2; Login=1; Logout=1; Timer=1')
{
    throw "Unexpected Clone Offline hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath (Join-Path $RepoRoot $TargetDir) -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFileCount = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' }).Count

if ($WorkspaceFiles.Count -ne 7 -or $RuntimeFileCount -ne 7)
{
    throw "Unexpected Clone Offline workspace file counts. Files=$($WorkspaceFiles.Count) RuntimeFiles=$RuntimeFileCount"
}

Assert-NoText -Paths @(
    'Data/Scripts/Scripts.csproj',
    'docs/wiki/AI_OVERHAUL_AUDIT.md',
    'docs/wiki/Clone_Offline_Player_Characters.md',
    'docs/wiki/SystemAudit.md',
    'docs/codebase-audit/tools/New-SystemCards.ps1',
    'docs/codebase-audit/outputs/system-cards/clone-offline-player-characters.md',
    'docs/codebase-audit/outputs/project-truth-register.csv',
    'docs/codebase-audit/outputs/runtime-script-compile-inventory.csv',
    'docs/codebase-audit/outputs/runtime-hook-map.csv',
    'docs/codebase-audit/outputs/serialization-register.csv'
) -Patterns @(
    'Data/Scripts/Custom/CloneOfflinePlayerCharacters',
    'Custom\CloneOfflinePlayerCharacters\'
)

$SourceEvidence = "Phase 12 proposal target=$TargetDir; source review found no package-local file path constants under the Clone Offline source tree; CloneOfflinePlayerCharacters.cs lines 19-22 keep Initialize, EventSink.Logout, and EventSink.Login registration; CheckClonesCommand.cs lines 17-23 keep the [CheckClones administrator command registration; CharacterClone.cs lines 27-39 preserve BaseCreature constructor AI settings and lines 95-98 preserve ForcedAI returning OmniAI; serialization-register.csv has four preserved namespace/type rows ($SerializedSummary); prior POST-BATCH-B-16A serializer triage classified the Clone Offline serializer rows as SafeNoChange, and prior POST-BATCH-E-04A fixed the login/logout hook guards; ScriptsProjectTruth after move has $ProjectIncludes includes, $ScriptSources script sources, 0 missing compile targets, 0 unincluded sources, and 0 project cleanup backlog groups; RuntimeScriptCompileTruth inventory has $($RuntimeTargetRows.Count) target rows, 0 old-path rows, and $($RuntimeInventory.Count) total runtime-visible script rows; runtime-hook-map.csv has $($HookRows.Count) Clone Offline hook rows ($HookSummary)."
$Action = "Moved the Clone Offline Player Characters workspace from $OldDir to $TargetDir; updated 7 Scripts.csproj Compile Include rows; updated current Clone Offline, AI Overhaul, SystemAudit, and audit-tool source traces; removed stale docs claims that Scripts.csproj listed only the entry-point file; regenerated runtime script compile inventory plus Phase 1, 2, 3, 4, 5, 6, 7, 8, and 9 path-sensitive audit outputs."
$Verification = "Project truth passed with $ProjectIncludes includes, $ScriptSources sources, 0 missing, 0 unincluded, and 0 cleanup backlog groups. Runtime script compile inventory contains $($RuntimeTargetRows.Count) moved Clone Offline rows and 0 old-path rows. Runtime hook map contains $($HookRows.Count) Clone Offline hook rows ($HookSummary). Serialization register contains four Clone Offline rows preserving namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters, version 0, and WriteVersionOnly handling ($SerializedSummary). git diff --check: $DiffCheckResult. ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult. Server.csproj Debug/x86: $ServerBuildResult. ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult."
$Notes = "No namespace, class, command name, public API, serialized type name, save version, XML/config schema, AI selection, clone lifecycle rule, reward/balance rule, or gameplay behavior changed. Historical Phase 12 and Phase 13 old-path rows remain proposal/rollback evidence, not current source location."

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
    TargetFiles = ($WorkspaceFiles -join ';')
    OriginalEvidence = $BacklogRow.Evidence
    Risk = $BacklogRow.Risk
    RecommendedFix = $BacklogRow.RecommendedFix
    Decision = 'ExecutedMoveNoNamespaceChange'
    SourceEvidence = $SourceEvidence
    Action = $Action
    Verification = $Verification
    Rollback = "Move $TargetDir back to $OldDir, restore the previous Scripts.csproj compile paths, rerun project truth and path-sensitive audit generators, and revert Clone Offline documentation/source-trace paths."
    ReviewedBatchId = $ReviewBatchId
    ReviewedAt = $Timestamp
    Notes = $Notes
}

@($ReviewRow) | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((Get-CsvRows -Path $ReviewPath).Count -ne 1)
{
    throw 'Review CSV row count invariant failed.'
}

$ActiveRows = @(Get-CsvRows -Path $ActivePath | Where-Object { $_.BacklogId -ne $BacklogId })
$OverlayRow = [pscustomobject]@{
    BacklogId = $ReviewRow.BacklogId
    SourceId = $ReviewRow.SourceId
    Category = $ReviewRow.Category
    System = $ReviewRow.System
    Files = $ReviewRow.TargetFiles
    HistoricalStatus = $ReviewRow.HistoricalStatus
    ActiveStatus = 'Fixed'
    PostAuditBatch = 'POST-BATCH-H'
    ReviewRowId = $ReviewRow.ReviewRowId
    ReviewStatus = $ReviewRow.Decision
    Action = $ReviewRow.Action
    ReviewArtifact = $ReviewRel
    SourceEvidence = $ReviewRow.SourceEvidence
    Commit = 'Pending current POST-BATCH-H-07A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($ActiveRows + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8
$OverlayCount = @((Get-CsvRows -Path $ActivePath) | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-H' -and $_.BacklogId -eq $BacklogId }).Count

if ($OverlayCount -ne 1)
{
    throw "Active overlay POST-BATCH-H $BacklogId invariant failed: $OverlayCount"
}

$Closeout = @(
    '# POST-BATCH-H Clone Offline Player Characters Move Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-07A executed the approved Phase 12 Clone Offline Player Characters containment move from `{0}` to `{1}`. The batch moved all seven runtime-visible `.cs` files, preserved the `Server.Custom.Confictura.CloneOfflinePlayerCharacters` namespace and all serialized type names, updated seven `Scripts.csproj` compile references, updated current source-trace docs, and regenerated path-sensitive audit outputs." -f $OldDir, $TargetDir),
    '',
    'The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. No package-local path constant, XML/config file, command name, hook registration, AI selection, serialization version, or gameplay rule changed.',
    '',
    '## Scope',
    '',
    '| Evidence | Value |',
    '| --- | --- |',
    ('| Backlog row | `{0}` |' -f $BacklogId),
    ('| Original path | `{0}` |' -f $OldDir),
    ('| Target path | `{0}` |' -f $TargetDir),
    ('| Workspace files moved | `{0}` |' -f $WorkspaceFiles.Count),
    ('| Runtime-visible files moved | `{0}` |' -f $RuntimeFileCount),
    '| Namespace/type/API changes | `None` |',
    ('| Serialized rows | `{0}` |' -f $SerializationRows.Count),
    ('| Serialization evidence | `{0}` |' -f $SerializedSummary),
    ('| Runtime hook rows | `{0}` ({1}) |' -f $HookRows.Count, $HookSummary),
    ('| Runtime script inventory target rows | `{0}` |' -f $RuntimeTargetRows.Count),
    '',
    '## Project Truth Result',
    '',
    '| Evidence | Count |',
    '| --- | ---: |',
    ("| Scripts.csproj compile includes | $ProjectIncludes |"),
    ("| Script source files | $ScriptSources |"),
    ("| Missing compile targets | $MissingCompileTargets |"),
    ("| Unincluded source files | $UnincludedSources |"),
    ("| Project cleanup backlog rows | $ProjectCleanupBacklog |"),
    '',
    '## Verification',
    '',
    ("- New-ProjectTruthRegister.ps1: passed with $ProjectIncludes includes, $ScriptSources sources, $MissingCompileTargets missing compile targets, $UnincludedSources unincluded sources, and $ProjectCleanupBacklog cleanup backlog rows."),
    ("- runtime-script-compile-inventory.csv: regenerated with $($RuntimeInventory.Count) runtime-visible script rows, $($RuntimeTargetRows.Count) Clone Offline target rows, and 0 old-path rows."),
    ("- runtime-hook-map.csv: regenerated with $($HookRows.Count) Clone Offline hook rows ($HookSummary)."),
    ("- serialization-register.csv: regenerated with $($SerializationRows.Count) Clone Offline rows preserving namespace/type names and version 0 WriteVersionOnly handling ($SerializedSummary)."),
    '- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.',
    "- git diff --check: $DiffCheckResult",
    "- ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult",
    "- Server.csproj Debug/x86: $ServerBuildResult",
    "- .\ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult",
    '',
    '## Rollback',
    '',
    ("Move `{0}` back to `{1}`, restore the previous `Scripts.csproj` compile paths, rerun project truth and path-sensitive audit generators, and revert Clone Offline documentation/source-trace paths." -f $TargetDir, $OldDir)
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-clone-offline-move-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-clone-offline-move-review.csv` | Post-audit | Review and disposition `RB-06815` for the POST-BATCH-H-07A Clone Offline Player Characters reorganization batch. | Complete |',
    '| `post-batch-h-clone-offline-move-closeout.md` | Post-audit | Close out the Clone Offline Player Characters move with project truth, runtime visibility, serialization, hook, verification, and rollback evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` executed the Clone Offline Player Characters Phase 12 containment move from `{1}` to `{2}`, moved 7 runtime-visible `.cs` files, updated 7 `Scripts.csproj` compile references and current source-trace docs, preserved the `Server.Custom.Confictura.CloneOfflinePlayerCharacters` namespace and four serialized version `0` clone types, regenerated runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 path-sensitive outputs, and added an active overlay `Fixed` disposition for `{3}`. Verification: git diff --check={4}; solution Debug/Any CPU={5}; Server Debug/x86={6}; compile-only={7}.' -f $ReviewBatchId, $OldDir, $TargetDir, $BacklogId, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult

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
    ('- Affected phase: Post-audit `{0}` Clone Offline Player Characters reorganization batch' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: move Clone Offline Player Characters workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCloneOfflineMove.ps1`.',
    ('- Result: Review CSV and active overlay contain one POST-BATCH-H row for {0}; project truth has {1} includes, {2} sources, {3} missing targets, {4} unlisted sources, and {5} cleanup backlog rows; runtime script compile inventory has {6} target rows and {7} old-path rows; serialization register has four Clone Offline rows ({8}); runtime hook map has {9} Clone Offline hook rows ({10}). Final verification: git diff --check={11}; solution={12}; server build={13}; compile-only={14}.' -f $BacklogId, $ProjectIncludes, $ScriptSources, $MissingCompileTargets, $UnincludedSources, $ProjectCleanupBacklog, $RuntimeTargetRows.Count, $RuntimeOldRows.Count, $SerializedSummary, $HookRows.Count, $HookSummary, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-07A` Clone Offline Player Characters reorganization batch\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n", $Utf8NoBom)
Add-Content -LiteralPath $RunLogPath -Value ($RunLogEntry -join "`n") -Encoding UTF8

[pscustomobject]@{
    ReviewPath = $ReviewPath
    CloseoutPath = $CloseoutPath
    BacklogId = $BacklogId
    ActiveStatus = 'Fixed'
    WorkspaceFiles = $WorkspaceFiles.Count
    RuntimeFiles = $RuntimeFileCount
    ProjectTargetRows = $ProjectTargetRows.Count
    RuntimeTargetRows = $RuntimeTargetRows.Count
    RuntimeOldRows = $RuntimeOldRows.Count
    HookRows = $HookRows.Count
    HookSummary = $HookSummary
    SerializationRows = $SerializationRows.Count
    MissingCompileTargets = $MissingCompileTargets
    UnincludedSources = $UnincludedSources
}
