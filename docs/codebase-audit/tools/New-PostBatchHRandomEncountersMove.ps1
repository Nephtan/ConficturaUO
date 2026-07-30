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
$ReviewBatchId = 'POST-BATCH-H-06A'
$ReviewRowId = 'PBH-006'
$BacklogId = 'RB-06804'
$OldDir = 'Data/Scripts/Custom/RandomEncounters'
$TargetDir = 'Data/Scripts/Custom/PvE/RandomEncounters'
$OldProjectPrefix = 'Custom\RandomEncounters\'
$TargetProjectPrefix = 'Custom\PvE\RandomEncounters\'
$ReviewRel = 'docs/codebase-audit/outputs/post-batch-h-random-encounters-move-review.csv'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-random-encounters-move-closeout.md'
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

if ($ProjectTargetRows.Count -ne 22 -or $ProjectOldRows.Count -ne 0)
{
    throw "Unexpected Random Encounters project truth rows. TargetRows=$($ProjectTargetRows.Count) OldRows=$($ProjectOldRows.Count)"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath (Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj')
$TargetProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($TargetProjectPrefix))).Count
$OldProjectRefs = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($OldProjectPrefix))).Count

if ($TargetProjectRefs -ne 17 -or $OldProjectRefs -ne 0)
{
    throw "Unexpected Scripts.csproj Random Encounters references. Target=$TargetProjectRefs Old=$OldProjectRefs"
}

$RuntimeInventory = Get-CsvRows -Path (Join-Path $OutputDir 'runtime-script-compile-inventory.csv')
$RuntimeTargetRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($TargetDir + '/') })
$RuntimeOldRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath.Contains($OldDir + '/') })

if ($RuntimeInventory.Count -ne 6581 -or $RuntimeTargetRows.Count -ne 11 -or $RuntimeOldRows.Count -ne 0)
{
    throw "Unexpected runtime script inventory rows. Total=$($RuntimeInventory.Count) Target=$($RuntimeTargetRows.Count) Old=$($RuntimeOldRows.Count)"
}

$SerializationRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'serialization-register.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })

if ($SerializationRows.Count -ne 2)
{
    throw "Expected two Random Encounters serialization rows after move, found $($SerializationRows.Count)."
}

$XmlDateCountRows = @($SerializationRows | Where-Object { $_.Class -eq 'Server.Engines.XmlSpawner2.XmlDateCount' -and $_.TypeName -eq 'XmlDateCount' })
$ImportRows = @($SerializationRows | Where-Object { $_.File.Contains($TargetDir + '/Import.cs') -and $_.Class -eq 'Unknown' -and $_.VersionHandling -eq 'NoVersionFound' })

if ($XmlDateCountRows.Count -ne 1 -or $ImportRows.Count -ne 1)
{
    throw "Unexpected Random Encounters serialization evidence. XmlDateCountRows=$($XmlDateCountRows.Count) ImportRows=$($ImportRows.Count)"
}

$SerializedRow = $XmlDateCountRows[0]

if ($SerializedRow.CurrentVersion -ne '0' -or $SerializedRow.VersionHandling -ne 'SingleVersionOnly')
{
    throw "Unexpected XmlDateCount version evidence: Version=$($SerializedRow.CurrentVersion) VersionHandling=$($SerializedRow.VersionHandling)"
}

$HookRows = @(Get-CsvRows -Path (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File.Contains($TargetDir + '/') -or $_.File.Contains($OldDir + '/') })
$HookSummary = (($HookRows | Group-Object HookType | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')

if ($HookRows.Count -ne 8 -or $HookSummary -ne 'Command=1; Initialize=3; Timer=4')
{
    throw "Unexpected Random Encounters hook rows. Total=$($HookRows.Count) Summary=$HookSummary"
}

$WorkspaceFiles = @(Get-ChildItem -LiteralPath (Join-Path $RepoRoot $TargetDir) -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFileCount = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' }).Count

if ($WorkspaceFiles.Count -ne 17 -or $RuntimeFileCount -ne 11)
{
    throw "Unexpected Random Encounters workspace file counts. Files=$($WorkspaceFiles.Count) RuntimeFiles=$RuntimeFileCount"
}

Assert-NoText -Paths @(
    'Data/Scripts/Scripts.csproj',
    'Data/Scripts/Custom/PvE/RandomEncounters/EncounterEngine.cs',
    'Data/Scripts/Custom/PvE/RandomEncounters/ReadMe.txt',
    'docs/wiki/AI_OVERHAUL_AUDIT.md',
    'docs/wiki/Confictura_Introduction.md',
    'docs/wiki/Random_Encounter_Engine.md',
    'docs/wiki/Search_System.md',
    'docs/wiki/SystemAudit.md',
    'docs/codebase-audit/tools/New-SystemCards.ps1',
    'docs/codebase-audit/tools/New-CommentTargetRegister.ps1',
    'docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1',
    'docs/codebase-audit/outputs/system-cards/random-encounters.md',
    'docs/codebase-audit/outputs/project-truth-register.csv',
    'docs/codebase-audit/outputs/runtime-script-compile-inventory.csv'
) -Patterns @(
    'Data/Scripts/Custom/RandomEncounters',
    'Custom\RandomEncounters\',
    './Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml'
)

$EnginePath = Join-Path $RepoRoot 'Data\Scripts\Custom\PvE\RandomEncounters\EncounterEngine.cs'
$EngineText = Get-Content -Raw -LiteralPath $EnginePath

if (-not $EngineText.Contains('./Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml') -or $EngineText.Contains('./Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml'))
{
    throw 'EncounterEngine.cs XML path constant was not updated to the moved package path.'
}

$SourceEvidence = "Phase 12 proposal target=$TargetDir; source review found EncounterEngine.cs lines 46-47 hardcoded the RandomEncounters.xml path, so this file-location-only move updated that constant to the moved package path while preserving the XML contents; Commands.cs lines 21-27 keep the [rand command registered at AccessLevel.Administrator; Helpers.cs lines 30-33 keep road tile initialization and lines 117-122 keep the CharacterLevelService encounter-level bridge; XmlDateCount.cs lines 29-42 preserve Server.Engines.XmlSpawner2.XmlDateCount serialization version 0; ScriptsProjectTruth after move has $ProjectIncludes includes, $ScriptSources script sources, 0 missing compile targets, 0 unincluded sources, and 0 project cleanup backlog groups; RuntimeScriptCompileTruth inventory has $($RuntimeTargetRows.Count) target rows, 0 old-path rows, and $($RuntimeInventory.Count) total runtime-visible script rows; runtime-hook-map.csv has $($HookRows.Count) Random Encounters hook rows ($HookSummary); serialization-register.csv has two Random Encounters rows: $($SerializedRow.Class) version $($SerializedRow.CurrentVersion) plus the Import.cs NoVersionFound review row."
$Action = "Moved the Random Encounters workspace from $OldDir to $TargetDir; updated 11 Scripts.csproj Compile Include rows, 3 Content rows, and 3 None rows; updated EncounterEngine.cs to load RandomEncounters.xml from the moved package path; updated current Random Encounters, Character Level, Search, AI Overhaul, SystemAudit, and audit-tool source traces; regenerated runtime script compile inventory plus Phase 1, 2, 3, 4, 5, 6, 7, 8, and 9 path-sensitive audit outputs."
$Verification = "Project truth passed with $ProjectIncludes includes, $ScriptSources sources, 0 missing, 0 unincluded, and 0 cleanup backlog groups. Runtime script compile inventory contains $($RuntimeTargetRows.Count) moved Random Encounters rows and 0 old-path rows. Runtime hook map contains $($HookRows.Count) Random Encounters hook rows ($HookSummary). Serialization register contains two Random Encounters rows: $($SerializedRow.Class) version $($SerializedRow.CurrentVersion) with $($SerializedRow.FieldAlignment), and Import.cs with NoVersionFound review evidence. EncounterEngine.cs points at the moved RandomEncounters.xml path. git diff --check: $DiffCheckResult. ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult. Server.csproj Debug/x86: $ServerBuildResult. ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult."
$Notes = "No namespace, class, command name, public API, serialized type name, save version, XML/config schema, encounter table contents, timer cadence, command access, reward/balance rule, or gameplay behavior changed. The package-local XML path constant changed only to preserve the same checked-in RandomEncounters.xml load after the file-location move. Historical Phase 12 and Phase 13 old-path rows remain proposal/rollback evidence, not current source location."

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
    Rollback = "Move $TargetDir back to $OldDir, restore the previous Scripts.csproj compile/content/none paths, rerun project truth and path-sensitive audit generators, and revert Random Encounters documentation/source-trace paths."
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
    Commit = 'Pending current POST-BATCH-H-06A commit'
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
    '# POST-BATCH-H Random Encounters Move Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    ("POST-BATCH-H-06A executed the approved Phase 12 Random Encounters containment move from `{0}` to `{1}`. The batch moved the complete 17-file package, preserved namespaces and serialized type names, updated 17 `Scripts.csproj` compile/content/none path references, updated the package-local XML load path constant, updated current source-trace docs, and regenerated path-sensitive audit outputs." -f $OldDir, $TargetDir),
    '',
    'The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. `RandomEncounters.xml` moved with the package and its contents were not rewritten; `EncounterEngine.cs` now points at the moved checked-in XML path so runtime loading remains aligned with the new file location.',
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
    ('| Serialized rows | `2` (`{0}` plus `Import.cs` review row) |' -f $SerializedRow.Class),
    ('| XmlDateCount serialization evidence | `{0}`, `{1}` |' -f $SerializedRow.VersionHandling, $SerializedRow.FieldAlignment),
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
    ("- runtime-script-compile-inventory.csv: regenerated with $($RuntimeInventory.Count) runtime-visible script rows, $($RuntimeTargetRows.Count) Random Encounters target rows, and 0 old-path rows."),
    ("- runtime-hook-map.csv: regenerated with $($HookRows.Count) Random Encounters hook rows ($HookSummary)."),
    ("- serialization-register.csv: regenerated with two Random Encounters rows: $($SerializedRow.Class) version $($SerializedRow.CurrentVersion), preserving $($SerializedRow.VersionHandling) and $($SerializedRow.FieldAlignment), plus the Import.cs NoVersionFound review row."),
    '- EncounterEngine.cs XML path: updated to `./Data/Scripts/Custom/PvE/RandomEncounters/RandomEncounters.xml` so the moved package continues loading its checked-in config file.',
    '- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.',
    "- git diff --check: $DiffCheckResult",
    "- ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult",
    "- Server.csproj Debug/x86: $ServerBuildResult",
    "- .\ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult",
    '',
    '## Rollback',
    '',
    ("Move `{0}` back to `{1}`, restore the previous `Scripts.csproj` compile/content/none paths, rerun project truth and path-sensitive audit generators, and revert Random Encounters documentation/source-trace paths." -f $TargetDir, $OldDir)
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-random-encounters-move-(review\.csv|closeout\.md)' })
$NewEntries = @(
    '| `post-batch-h-random-encounters-move-review.csv` | Post-audit | Review and disposition `RB-06804` for the POST-BATCH-H-06A Random Encounters reorganization batch. | Complete |',
    '| `post-batch-h-random-encounters-move-closeout.md` | Post-audit | Close out the Random Encounters move with project truth, runtime visibility, XML path, serialization, verification, and rollback evidence. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: `{0}` executed the Random Encounters Phase 12 containment move from `{1}` to `{2}`, moved 11 runtime-visible `.cs` files plus package data/docs, updated 17 `Scripts.csproj` compile/content/none path references and current source-trace docs, updated the package-local RandomEncounters.xml load path, preserved serialized type `{3}` version `{4}`, regenerated runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 path-sensitive outputs, and added an active overlay `Fixed` disposition for `{5}`. Verification: git diff --check={6}; solution Debug/Any CPU={7}; Server Debug/x86={8}; compile-only={9}.' -f $ReviewBatchId, $OldDir, $TargetDir, $SerializedRow.Class, $SerializedRow.CurrentVersion, $BacklogId, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult

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
    ('- Affected phase: Post-audit `{0}` Random Encounters reorganization batch' -f $ReviewBatchId),
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: move Random Encounters workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHRandomEncountersMove.ps1`.',
    ('- Result: Review CSV and active overlay contain one POST-BATCH-H row for {0}; project truth has {1} includes, {2} sources, {3} missing targets, {4} unlisted sources, and {5} cleanup backlog rows; runtime script compile inventory has {6} target rows and {7} old-path rows; serialization register has two Random Encounters rows, including {8} version {9}; runtime hook map has {10} Random Encounters hook rows ({11}); EncounterEngine.cs points at the moved RandomEncounters.xml path. Final verification: git diff --check={12}; solution={13}; server build={14}; compile-only={15}.' -f $BacklogId, $ProjectIncludes, $ScriptSources, $MissingCompileTargets, $UnincludedSources, $ProjectCleanupBacklog, $RuntimeTargetRows.Count, $RuntimeOldRows.Count, $SerializedRow.Class, $SerializedRow.CurrentVersion, $HookRows.Count, $HookSummary, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult),
    ('- Output path: `{0}`; `{1}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ReviewRel, $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-06A` Random Encounters reorganization batch\r?\n.*?(?=^### |\z)'
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
