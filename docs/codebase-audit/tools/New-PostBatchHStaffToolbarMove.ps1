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
$ReviewBatchId = 'POST-BATCH-H-05A'
$ReviewRowId = 'PBH-005'
$BacklogId = 'RB-06812'
$OldDir = 'Data/Scripts/Custom/Staff Toolbar [2.0]'
$TargetDir = 'Data/Scripts/Custom/ThirdParty/Staff Toolbar [2.0]'
$OldProjectPrefix = 'Custom\Staff Toolbar [2.0]\'
$TargetProjectPrefix = 'Custom\ThirdParty\Staff Toolbar [2.0]\'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-staff-toolbar-move-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-staff-toolbar-move-closeout.md'
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

if ($ProjectTargetRows.Count -ne 2 -or $ProjectOldRows.Count -ne 0)
{
    throw "Unexpected Staff Toolbar project truth rows. TargetRows=$($ProjectTargetRows.Count) OldRows=$($ProjectOldRows.Count)"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath (Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj')
$TargetProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($TargetProjectPrefix))).Count
$OldProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($OldProjectPrefix))).Count

if ($TargetProjectRefs -ne 1 -or $OldProjectRefs -ne 0)
{
    throw "Unexpected Scripts.csproj Staff Toolbar references. Target=$TargetProjectRefs Old=$OldProjectRefs"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeTargetRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($TargetDir + '/') })
$RuntimeOldRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeInventory.Count -ne 6581 -or $RuntimeTargetRows.Count -ne 1 -or $RuntimeOldRows.Count -ne 0)
{
    throw "Unexpected runtime script inventory rows. Total=$($RuntimeInventory.Count) Target=$($RuntimeTargetRows.Count) Old=$($RuntimeOldRows.Count)"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 1)
{
    throw "Expected one Staff Toolbar serialization row after move, found $($SerializationRows.Count)."
}

$SerializedRow = $SerializationRows[0]

if ($SerializedRow.Class -ne 'Joeku.ToolbarInfos' -or $SerializedRow.TypeName -ne 'ToolbarInfos' -or $SerializedRow.VersionHandling -ne 'ReadVersionOnly')
{
    throw "Unexpected Staff Toolbar serialized type evidence: Class=$($SerializedRow.Class) Type=$($SerializedRow.TypeName) VersionHandling=$($SerializedRow.VersionHandling)"
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 26 -or $HookSummary -ne 'Event=1; Gump=23; Initialize=1; Login=1')
{
    throw "Unexpected Staff Toolbar hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath (Join-Path $RepoRoot $TargetDir) -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFileCount = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' }).Count

if ($WorkspaceFiles.Count -ne 1 -or $RuntimeFileCount -ne 1)
{
    throw "Unexpected Staff Toolbar workspace file counts. Files=$($WorkspaceFiles.Count) RuntimeFiles=$RuntimeFileCount"
}

Assert-NoText -Paths @(
    'Data/Scripts/Scripts.csproj',
    'docs/wiki/Staff_Toolbar.md',
    'docs/wiki/SystemAudit.md',
    'docs/codebase-audit/tools/New-SystemCards.ps1',
    'docs/codebase-audit/outputs/system-cards/staff-toolbar.md',
    'docs/codebase-audit/outputs/project-truth-register.csv',
    'docs/codebase-audit/outputs/runtime-script-compile-inventory.csv'
) -Patterns @(
    'Data/Scripts/Custom/Staff Toolbar [2.0]',
    'Custom\Staff Toolbar [2.0]\',
    '../Data/Scripts/Custom/Staff%20Toolbar%20%5B2.0%5D'
)

$SourceEvidence = "Phase 12 proposal target=$TargetDir; source review of Toolbar.cs lines 28-39 keeps the [Toolbar command registered at AccessLevel.Counselor and EventSink.Login subscription unchanged; lines 44-53 send the toolbar on login only when mobile AccessLevel is Counselor or higher; lines 56-60 preserve the command close-and-send flow; lines 129-171 preserve Joeku.ToolbarInfos serialization/version-read behavior without namespace, type, or save-version changes; ScriptsProjectTruth after move has $ProjectIncludes includes, $ScriptSources script sources, 0 missing compile targets, 0 unincluded sources, and 0 project cleanup backlog groups; RuntimeScriptCompileTruth inventory has $($RuntimeTargetRows.Count) target row, 0 old-path rows, and $($RuntimeInventory.Count) total runtime-visible script rows; runtime-hook-map.csv has $($HookRows.Count) Staff Toolbar hook rows ($HookSummary); serialization-register.csv has one Staff Toolbar serialized row, $($SerializedRow.Class), with $($SerializedRow.FieldAlignment)."
$Action = "Moved the Staff Toolbar workspace from $OldDir to $TargetDir; updated the single Scripts.csproj Compile Include row; updated canonical Staff Toolbar documentation, SystemAudit source trace, and the system-card generator path pattern; regenerated runtime script compile inventory plus Phase 1, 2, 3, 4, 5, 6, 7, 8, and 9 path-sensitive audit outputs."
$Verification = "Project truth passed with $ProjectIncludes includes, $ScriptSources sources, 0 missing, 0 unincluded, and 0 cleanup backlog groups. Runtime script compile inventory contains $($RuntimeTargetRows.Count) moved Staff Toolbar row and 0 old-path rows. Runtime hook map contains $($HookRows.Count) Staff Toolbar hook rows ($HookSummary). Serialization register contains one Staff Toolbar row for $($SerializedRow.Class), version handling $($SerializedRow.VersionHandling), and preserved legacy field evidence $($SerializedRow.FieldAlignment). git diff --check: $DiffCheckResult. ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult. Server.csproj Debug/x86: $ServerBuildResult. ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult."
$Notes = "No namespace, class, command name, public API, serialized type name, save version, XML/config, staff workflow, access policy, gump behavior, or gameplay behavior changed. The known legacy serializer evidence for $($SerializedRow.Class) was preserved and this batch did not attempt a save-format repair. Historical Phase 12 and Phase 13 old-path rows remain proposal/rollback evidence, not current source location."

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
    Rollback = "Move $TargetDir back to $OldDir, restore the previous Scripts.csproj include path, rerun project truth and path-sensitive audit generators, and revert Staff Toolbar documentation/source-trace paths."
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
    Commit = 'Pending current POST-BATCH-H-05A commit'
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
    '# POST-BATCH-H Staff Toolbar Move Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("`POST-BATCH-H-05A` executed the approved Phase 12 Staff Toolbar containment move from `{0}` to `{1}`. The batch moved the single-script workspace, preserved the `Joeku` namespace and serialized `ToolbarInfos` type, updated one `Scripts.csproj` compile include, updated current source-trace docs, and regenerated path-sensitive audit outputs." -f $OldDir, $TargetDir),
    '',
    'The moved script remains under `Data/Scripts`, so live runtime script compile visibility is preserved. The access/workflow gate was source-reviewed: `[Toolbar` remains Counselor-gated, the login hook still sends the toolbar only to Counselor-or-higher mobiles, and no gump command execution behavior changed.',
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
    ('| Serialized row | `{0}` / `{1}` |' -f $SerializedRow.Class, $SerializedRow.TypeName),
    ('| Serialization evidence | `{0}`, `{1}` |' -f $SerializedRow.VersionHandling, $SerializedRow.FieldAlignment),
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
    ("- runtime-script-compile-inventory.csv: regenerated with $($RuntimeInventory.Count) runtime-visible script rows, $($RuntimeTargetRows.Count) Staff Toolbar target row, and 0 old-path rows."),
    ("- runtime-hook-map.csv: regenerated with $($HookRows.Count) Staff Toolbar hook rows ($HookSummary)."),
    ("- serialization-register.csv: regenerated with one Staff Toolbar serialized row, $($SerializedRow.Class), preserving $($SerializedRow.VersionHandling) and $($SerializedRow.FieldAlignment)."),
    '- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.',
    "- git diff --check: $DiffCheckResult",
    "- ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult",
    "- Server.csproj Debug/x86: $ServerBuildResult",
    "- .\ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult",
    '',
    '## Rollback',
    '',
    ("Move `{0}` back to `{1}`, restore the previous `Scripts.csproj` include path, rerun project truth and path-sensitive audit generators, and revert Staff Toolbar documentation/source-trace paths." -f $TargetDir, $OldDir)
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-staff-toolbar-move-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-staff-toolbar-move-review.csv` | Post-audit | Review and disposition `RB-06812` for the POST-BATCH-H-05A Staff Toolbar reorganization batch. | Complete |',
    '| `post-batch-h-staff-toolbar-move-closeout.md` | Post-audit | Close out the Staff Toolbar move with project truth, runtime visibility, access/workflow, serialization, verification, and rollback evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` executed the Staff Toolbar Phase 12 containment move from `{1}` to `{2}`, moved 1 runtime-visible `.cs` file, updated 1 `Scripts.csproj` compile include and current source-trace docs, preserved serialized type `{3}` with `{4}`, preserved Counselor-only staff workflow/access behavior, regenerated runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 path-sensitive outputs, and added an active overlay `Fixed` disposition for `{5}`. Verification: git diff --check={6}; solution Debug/Any CPU={7}; Server Debug/x86={8}; compile-only={9}.' -f $ReviewBatchId, $OldDir, $TargetDir, $SerializedRow.Class, $SerializedRow.VersionHandling, $BacklogId, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult

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
    ('- Affected phase: Post-audit `{0}` Staff Toolbar reorganization batch' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: move Staff Toolbar workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHStaffToolbarMove.ps1`.',
    ('- Result: Review CSV and active overlay contain one POST-BATCH-H row for {0}; project truth has {1} includes, {2} sources, {3} missing targets, {4} unlisted sources, and {5} cleanup backlog rows; runtime script compile inventory has {6} target row and {7} old-path rows; serialization register has one Staff Toolbar row for {8} with {9}; runtime hook map has {10} Staff Toolbar hook rows ({11}). Final verification: git diff --check={12}; solution={13}; server build={14}; compile-only={15}.' -f $BacklogId, $ProjectIncludes, $ScriptSources, $MissingCompileTargets, $UnincludedSources, $ProjectCleanupBacklog, $RuntimeTargetRows.Count, $RuntimeOldRows.Count, $SerializedRow.Class, $SerializedRow.VersionHandling, $HookRows.Count, $HookSummary, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-05A` Staff Toolbar reorganization batch\r?\n.*?(?=^### |\z)'
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
    SerializedType = $SerializedRow.Class
    MissingCompileTargets = $MissingCompileTargets
    UnincludedSources = $UnincludedSources
}
