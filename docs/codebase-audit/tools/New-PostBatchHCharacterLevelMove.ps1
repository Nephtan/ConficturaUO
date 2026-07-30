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

function Get-CsvCount
{
    param(
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path))
    {
        throw "Required CSV missing: $Path"
    }

    if ((Get-Item -LiteralPath $Path).Length -eq 0)
    {
        return 0
    }

    return @((Import-Csv -LiteralPath $Path)).Count
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

$ReviewPath = Join-Path $OutputDir 'post-batch-h-character-level-move-review.csv'
$CloseoutPath = Join-Path $OutputDir 'post-batch-h-character-level-move-closeout.md'
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-H-01A'
$ArtifactRel = 'docs/codebase-audit/outputs/post-batch-h-character-level-move-review.csv'
$OldDir = 'Data/Scripts/Custom/CharacterLevel'
$TargetDir = 'Data/Scripts/Custom/Progression/CharacterLevel'
$MovedFiles = @('CharacterLevelCommands.cs', 'CharacterLevelService.cs')
$TargetFiles = @($MovedFiles | ForEach-Object { "$TargetDir/$_" })
$OldFiles = @($MovedFiles | ForEach-Object { "$OldDir/$_" })

$Backlog = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'repair-backlog.csv'))
$Row = @($Backlog | Where-Object { $_.Id -eq 'RB-06802' })

if ($Row.Count -ne 1)
{
    throw "Expected one RB-06802 backlog row, found $($Row.Count)."
}

$Row = $Row[0]

foreach ($path in $TargetFiles)
{
    if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $path) -PathType Leaf))
    {
        throw "Moved target file missing: $path"
    }
}

foreach ($path in $OldFiles)
{
    if (Test-Path -LiteralPath (Join-Path $RepoRoot $path) -PathType Leaf)
    {
        throw "Old source file still exists after move: $path"
    }
}

$ProjectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$ScriptCompileIncludes = @($ProjectTruth | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ScriptSources = @($ProjectTruth | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count
$ProjectCleanupBacklog = Get-CsvCount -Path (Join-Path $OutputDir 'project-cleanup-backlog.csv')
$ProjectTargetRows = @($ProjectTruth | Where-Object { $_.Path -like "$TargetDir/*" -or $_.IncludePath -like 'Custom\Progression\CharacterLevel\*' })
$ProjectOldRows = @($ProjectTruth | Where-Object { $_.Path -like "$OldDir/*" -or $_.IncludePath -like 'Custom\CharacterLevel\*' })

if ($ScriptCompileIncludes -ne 6581 -or $ScriptSources -ne 6581 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0 -or $ProjectCleanupBacklog -ne 0)
{
    throw "Unexpected project truth counts. Includes=$ScriptCompileIncludes Sources=$ScriptSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources Cleanup=$ProjectCleanupBacklog"
}

if ($ProjectTargetRows.Count -ne 4 -or $ProjectOldRows.Count -ne 0)
{
    throw "Unexpected Character Level project truth rows. TargetRows=$($ProjectTargetRows.Count) OldRows=$($ProjectOldRows.Count)"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath (Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj')

foreach ($file in $MovedFiles)
{
    $include = "Custom\Progression\CharacterLevel\$file"
    $oldInclude = "Custom\CharacterLevel\$file"

    if ($ScriptsProjectText -notlike "*$include*")
    {
        throw "Scripts.csproj missing moved include: $include"
    }

    if ($ScriptsProjectText -like "*$oldInclude*")
    {
        throw "Scripts.csproj still contains old include: $oldInclude"
    }
}

$Serialization = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'serialization-register.csv'))
$CharacterSerializationRows = @($Serialization | Where-Object { $_.File -like "$TargetDir/*" -or $_.File -like "$OldDir/*" -or $_.Class -like '*CharacterLevel*' -or $_.TypeName -like '*CharacterLevel*' })

if ($CharacterSerializationRows.Count -ne 0)
{
    throw "Character Level serialization rows were found after move: $($CharacterSerializationRows.Count)"
}

$HookRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File -like "$TargetDir/*" })

if ($HookRows.Count -lt 2)
{
    throw "Expected moved Character Level command hook rows in runtime-hook-map.csv, found $($HookRows.Count)."
}

$RuntimeInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-script-compile-inventory.csv'))
$RuntimeTargetRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath -like "$TargetDir/*" })
$RuntimeOldRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath -like "$OldDir/*" })

if ($RuntimeInventory.Count -ne 6581 -or $RuntimeTargetRows.Count -ne 2 -or $RuntimeOldRows.Count -ne 0)
{
    throw "Unexpected runtime script inventory rows. Total=$($RuntimeInventory.Count) TargetRows=$($RuntimeTargetRows.Count) OldRows=$($RuntimeOldRows.Count)"
}

$ReviewRow = [pscustomobject]@{
    BatchId = 'POST-BATCH-H'
    ReviewRowId = 'PBH-001'
    BacklogId = $Row.Id
    SourceId = $Row.SourceId
    Priority = $Row.Priority
    HistoricalStatus = $Row.Status
    Category = $Row.Category
    System = $Row.System
    OriginalFiles = $Row.Files
    TargetFiles = ($TargetFiles -join ';')
    OriginalEvidence = $Row.Evidence
    Risk = $Row.Risk
    RecommendedFix = $Row.RecommendedFix
    Decision = 'ExecutedMoveNoNamespaceChange'
    SourceEvidence = "Phase 12 proposal target=$TargetDir; Phase 6/serialization evidence has 0 Character Level serialized rows; ScriptsProjectTruth after move has $ScriptCompileIncludes includes, $ScriptSources script sources, 0 missing compile targets, 0 unincluded sources, and 0 project cleanup backlog groups; runtime-script-compile-inventory.csv has $($RuntimeTargetRows.Count) target rows, 0 old-path rows, and $($RuntimeInventory.Count) total runtime-visible script rows."
    Action = "Moved only $($OldFiles -join ';') to $TargetDir; updated two Scripts.csproj Compile Include rows; updated Character Level documentation/source-trace paths; regenerated runtime script compile inventory plus Phase 1, 2, 3, 4, 5, 6, 7, 8, and 9 path-sensitive audit outputs."
    Verification = "Project truth passed with $ScriptCompileIncludes includes, $ScriptSources sources, 0 missing, 0 unincluded, and 0 cleanup backlog groups. Runtime script compile inventory contains $($RuntimeTargetRows.Count) moved Character Level rows and 0 old-path rows. Runtime hook map contains $($HookRows.Count) moved Character Level hook rows. Serialization register contains 0 Character Level rows. git diff --check: $DiffCheckResult. ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult. Server.csproj Debug/x86: $ServerBuildResult. ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult."
    Rollback = "Move $TargetDir back to $OldDir, restore the previous Scripts.csproj includes, rerun project truth and path-sensitive audit generators, and revert documentation source-trace paths."
    ReviewedBatchId = $ReviewBatchId
    ReviewedAt = $Timestamp
    Notes = 'No namespace, class, command name, public API, serialized type, save version, XML/config, or gameplay behavior changed. Historical Phase 12 and Phase 13 old-path rows remain proposal/rollback evidence, not current source location.'
}

@($ReviewRow) | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((@(Import-Csv -LiteralPath $ReviewPath)).Count -ne 1)
{
    throw 'Review CSV row count invariant failed.'
}

$Active = @(Import-Csv -LiteralPath $ActivePath | Where-Object { $_.BacklogId -ne 'RB-06802' })
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
    ReviewArtifact = $ArtifactRel
    SourceEvidence = $ReviewRow.SourceEvidence
    Commit = 'Pending current POST-BATCH-H-01A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($Active + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8
$OverlayCount = @((Import-Csv -LiteralPath $ActivePath) | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-H' -and $_.BacklogId -eq 'RB-06802' }).Count

if ($OverlayCount -ne 1)
{
    throw "Active overlay POST-BATCH-H RB-06802 invariant failed: $OverlayCount"
}

$DecisionCounts = @(@($ReviewRow) | Group-Object Decision | Sort-Object Name)
$StatusCounts = @(@($OverlayRow) | Group-Object ActiveStatus | Sort-Object Name)

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-H Character Level Move Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add(('`POST-BATCH-H-01A` executed the approved Phase 12 Character Level pilot move from `{0}` to `{1}`. The batch moved exactly two runtime-visible script files, preserved namespaces/classes/commands/APIs, updated two `Scripts.csproj` include rows, updated current documentation source-trace paths, and regenerated path-sensitive audit outputs.' -f $OldDir, $TargetDir)) | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('The moved files remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The serialization register still reports zero Character Level serialized rows, so this move did not introduce a save-format migration.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Scope') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('| Evidence | Value |') | Out-Null
$Closeout.Add('| --- | --- |') | Out-Null
$Closeout.Add(('| Backlog row | `{0}` |' -f $Row.Id)) | Out-Null
$Closeout.Add(('| Original path | `{0}` |' -f $OldDir)) | Out-Null
$Closeout.Add(('| Target path | `{0}` |' -f $TargetDir)) | Out-Null
$Closeout.Add(('| Files moved | `{0}` |' -f ($MovedFiles -join '`, `'))) | Out-Null
$Closeout.Add("| Namespace/type/command changes | `None` |") | Out-Null
$Closeout.Add(('| Serialized Character Level rows | `{0}` |' -f $CharacterSerializationRows.Count)) | Out-Null
$Closeout.Add(('| Runtime script inventory target rows | `{0}` |' -f $RuntimeTargetRows.Count)) | Out-Null
$Closeout.Add(('| Moved hook rows | `{0}` |' -f $HookRows.Count)) | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Active Overlay Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $StatusCounts -NameHeader 'Active status'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Project Truth Result') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('| Evidence | Count |') | Out-Null
$Closeout.Add('| --- | ---: |') | Out-Null
$Closeout.Add("| Scripts.csproj compile includes | $ScriptCompileIncludes |") | Out-Null
$Closeout.Add("| Script source files | $ScriptSources |") | Out-Null
$Closeout.Add("| Missing compile targets | $MissingCompileTargets |") | Out-Null
$Closeout.Add("| Unincluded source files | $UnincludedSources |") | Out-Null
$Closeout.Add("| Project cleanup backlog rows | $ProjectCleanupBacklog |") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("- New-ProjectTruthRegister.ps1: passed with $ScriptCompileIncludes includes, $ScriptSources sources, $MissingCompileTargets missing compile targets, $UnincludedSources unincluded sources, and $ProjectCleanupBacklog cleanup backlog rows.") | Out-Null
$Closeout.Add("- runtime-script-compile-inventory.csv: regenerated with $($RuntimeInventory.Count) runtime-visible script rows, $($RuntimeTargetRows.Count) Character Level target rows, and 0 old-path rows.") | Out-Null
$Closeout.Add('- `Invoke-CodebaseAuditInventory.ps1`: passed after adding a zero-row export guard to the Phase 1 inventory helper.') | Out-Null
$Closeout.Add('- Regenerated cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.') | Out-Null
$Closeout.Add("- git diff --check: $DiffCheckResult") | Out-Null
$Closeout.Add("- ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult") | Out-Null
$Closeout.Add("- Server.csproj Debug/x86: $ServerBuildResult") | Out-Null
$Closeout.Add("- .\ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Rollback') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add(('Move `{0}` back to `{1}`, restore the previous two `Scripts.csproj` include paths, rerun project truth and the path-sensitive audit generators, and revert Character Level documentation source traces.' -f $TargetDir, $OldDir)) | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$NewEntries = @(
    '| `post-batch-h-character-level-move-review.csv` | Post-audit | Review and disposition `RB-06802` for the POST-BATCH-H-01A Character Level reorganization pilot. | Complete |',
    '| `post-batch-h-character-level-move-closeout.md` | Post-audit | Close out the Character Level move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |'
)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-character-level-move-(review\.csv|closeout\.md)' })
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
$HSummary = 'Post-audit reorganization pilot: `{0}` executed the Character Level Phase 12 move from `{1}` to `{2}`, moved exactly two runtime-visible `.cs` files, updated two `Scripts.csproj` includes and current source-trace docs, regenerated runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 path-sensitive outputs, and added an active overlay `Fixed` disposition for `RB-06802`. Verification: git diff --check={3}; solution Debug/Any CPU={4}; Server Debug/x86={5}; compile-only={6}.' -f $ReviewBatchId, $OldDir, $TargetDir, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult

if ($PhaseStatus -match 'Post-audit reorganization pilot:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit reorganization pilot:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $HSummary }, 1)
}
elseif ($PhaseStatus -match '\r?\nScope:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, '(\r?\nScope:)', "`n$HSummary`n`$1", 1)
}
else
{
    $PhaseStatus = $PhaseStatus.TrimEnd() + "`n`n$HSummary`n"
}

[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$RunLog = New-Object System.Collections.Generic.List[string]
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add(('- Affected phase: Post-audit `{0}` Character Level reorganization pilot' -f $ReviewBatchId)) | Out-Null
$RunLog.Add(('- Cwd: `{0}`' -f $RepoRoot)) | Out-Null
$RunLog.Add('- Command: move Character Level files, update `Scripts.csproj`, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCharacterLevelMove.ps1`.') | Out-Null
$RunLog.Add("- Result: Review CSV and active overlay contain one POST-BATCH-H row for `RB-06802`; project truth has $ScriptCompileIncludes includes, $ScriptSources sources, $MissingCompileTargets missing targets, $UnincludedSources unlisted sources, and $ProjectCleanupBacklog cleanup backlog rows; runtime script compile inventory has $($RuntimeTargetRows.Count) target rows and $($RuntimeOldRows.Count) old-path rows; serialization register has $($CharacterSerializationRows.Count) Character Level rows; runtime hook map has $($HookRows.Count) moved Character Level hook rows. Final verification: git diff --check=$DiffCheckResult; solution=$SolutionBuildResult; server build=$ServerBuildResult; compile-only=$CompileOnlyResult.") | Out-Null
$RunLog.Add(('- Output path: `{0}`; `docs/codebase-audit/outputs/post-batch-h-character-level-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ArtifactRel)) | Out-Null
$RunLog.Add('') | Out-Null
Add-Content -LiteralPath $RunLogPath -Value ($RunLog -join "`n") -Encoding UTF8

[pscustomobject]@{
    ReviewPath = $ReviewPath
    CloseoutPath = $CloseoutPath
    OverlayCount = $OverlayCount
    ScriptCompileIncludes = $ScriptCompileIncludes
    ScriptSources = $ScriptSources
    MissingCompileTargets = $MissingCompileTargets
    UnincludedSources = $UnincludedSources
    ProjectCleanupBacklog = $ProjectCleanupBacklog
    RuntimeScriptInventoryRows = $RuntimeInventory.Count
    RuntimeTargetRows = $RuntimeTargetRows.Count
    RuntimeOldRows = $RuntimeOldRows.Count
    CharacterSerializationRows = $CharacterSerializationRows.Count
    CharacterHookRows = $HookRows.Count
}
