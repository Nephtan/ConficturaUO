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
$DefaultEncoding = [System.Text.Encoding]::Default

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
    param([string]$Path)

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

        $bytes = [System.IO.File]::ReadAllBytes($fullPath)
        $text = $DefaultEncoding.GetString($bytes)

        foreach ($pattern in $Patterns)
        {
            if ($text.Contains($pattern))
            {
                throw "Stale text '$pattern' found in $path"
            }
        }
    }
}

$ReviewPath = Join-Path $OutputDir 'post-batch-h-static-gump-tool-move-review.csv'
$CloseoutPath = Join-Path $OutputDir 'post-batch-h-static-gump-tool-move-closeout.md'
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-H-03A'
$ReviewRowId = 'PBH-003'
$BacklogId = 'RB-06811'
$ArtifactRel = 'docs/codebase-audit/outputs/post-batch-h-static-gump-tool-move-review.csv'
$OldDir = 'Data/Scripts/Custom/OzThothStaticGump'
$TargetDir = 'Data/Scripts/Custom/StaffTools/StaticGumpTool'
$OldProjectPrefix = 'Custom\OzThothStaticGump\'
$TargetProjectPrefix = 'Custom\StaffTools\StaticGumpTool\'

$Backlog = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'repair-backlog.csv'))
$Row = @($Backlog | Where-Object { $_.Id -eq $BacklogId })

if ($Row.Count -ne 1)
{
    throw "Expected one $BacklogId backlog row, found $($Row.Count)."
}

$Row = $Row[0]

if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $TargetDir) -PathType Container))
{
    throw "Moved target directory missing: $TargetDir"
}

if (Test-Path -LiteralPath (Join-Path $RepoRoot $OldDir))
{
    throw "Old source directory still exists after move: $OldDir"
}

$ProjectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$ScriptCompileIncludes = @($ProjectTruth | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ScriptSources = @($ProjectTruth | Where-Object { $_.RecordType -eq 'SourceFile' }).Count
$MissingCompileTargets = @($ProjectTruth | Where-Object { $_.MissingCompileTarget -eq 'True' }).Count
$UnincludedSources = @($ProjectTruth | Where-Object { $_.UnincludedSource -eq 'True' }).Count
$ProjectCleanupBacklog = Get-CsvCount -Path (Join-Path $OutputDir 'project-cleanup-backlog.csv')
$ProjectTargetRows = @($ProjectTruth | Where-Object { $_.Path -like "$TargetDir/*" -or $_.IncludePath -like "$TargetProjectPrefix*" })
$ProjectOldRows = @($ProjectTruth | Where-Object { $_.Path -like "$OldDir/*" -or $_.IncludePath -like "$OldProjectPrefix*" })

if ($ScriptCompileIncludes -ne 6581 -or $ScriptSources -ne 6581 -or $MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0 -or $ProjectCleanupBacklog -ne 0)
{
    throw "Unexpected project truth counts. Includes=$ScriptCompileIncludes Sources=$ScriptSources Missing=$MissingCompileTargets Unincluded=$UnincludedSources Cleanup=$ProjectCleanupBacklog"
}

if ($ProjectTargetRows.Count -ne 42 -or $ProjectOldRows.Count -ne 0)
{
    throw "Unexpected Static Gump Tool project truth rows. TargetRows=$($ProjectTargetRows.Count) OldRows=$($ProjectOldRows.Count)"
}

$ScriptsProjectText = Get-Content -Raw -LiteralPath (Join-Path $RepoRoot 'Data\Scripts\Scripts.csproj')
$TargetProjectReferences = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($TargetProjectPrefix))).Count
$OldProjectReferences = ([regex]::Matches($ScriptsProjectText, [regex]::Escape($OldProjectPrefix))).Count

if ($TargetProjectReferences -ne 22 -or $OldProjectReferences -ne 0)
{
    throw "Unexpected Scripts.csproj Static Gump Tool path references. Target=$TargetProjectReferences Old=$OldProjectReferences"
}

$Serialization = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'serialization-register.csv'))
$StaticSerializationRows = @($Serialization | Where-Object { $_.File -like "$TargetDir/*" -or $_.File -like "$OldDir/*" })

if ($StaticSerializationRows.Count -ne 0)
{
    throw "Static Gump Tool serialization rows were found after move: $($StaticSerializationRows.Count)"
}

$HookRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-hook-map.csv') | Where-Object { $_.File -like "$TargetDir/*" -or $_.File -like "$OldDir/*" })
$CommandRows = @($HookRows | Where-Object { $_.HookType -eq 'Command' })
$GumpRows = @($HookRows | Where-Object { $_.HookType -eq 'Gump' })
$InitializeRows = @($HookRows | Where-Object { $_.HookType -eq 'Initialize' })

if ($HookRows.Count -ne 198 -or $CommandRows.Count -ne 23 -or $GumpRows.Count -ne 153 -or $InitializeRows.Count -ne 22)
{
    throw "Unexpected Static Gump Tool runtime hook rows. Total=$($HookRows.Count) Command=$($CommandRows.Count) Gump=$($GumpRows.Count) Initialize=$($InitializeRows.Count)"
}

$RuntimeInventory = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'runtime-script-compile-inventory.csv'))
$RuntimeTargetRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath -like "$TargetDir/*" })
$RuntimeOldRows = @($RuntimeInventory | Where-Object { $_.RuntimeScriptPath -like "$OldDir/*" })

if ($RuntimeInventory.Count -ne 6581 -or $RuntimeTargetRows.Count -ne 21 -or $RuntimeOldRows.Count -ne 0)
{
    throw "Unexpected runtime script inventory rows. Total=$($RuntimeInventory.Count) TargetRows=$($RuntimeTargetRows.Count) OldRows=$($RuntimeOldRows.Count)"
}

Assert-NoText -Paths @(
    'Data/Scripts/Scripts.csproj',
    'Data/Scripts/Custom/StaffTools/StaticGumpTool/AGENTS.md',
    'docs/wiki/Static_Gump_Tool.md',
    'docs/wiki/SystemAudit.md',
    'docs/codebase-audit/tools/New-SystemCards.ps1',
    'docs/codebase-audit/outputs/system-cards/static-gump-tool.md',
    'docs/codebase-audit/outputs/project-truth-register.csv',
    'docs/codebase-audit/outputs/runtime-script-compile-inventory.csv'
) -Patterns @(
    'Data/Scripts/Custom/OzThothStaticGump',
    'Custom\OzThothStaticGump'
)

$WorkspaceFiles = @(Get-ChildItem -LiteralPath (Join-Path $RepoRoot $TargetDir) -Recurse -File | ForEach-Object {
    $_.FullName.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/'
} | Sort-Object)
$RuntimeFileCount = @($WorkspaceFiles | Where-Object { $_ -like '*.cs' }).Count

if ($WorkspaceFiles.Count -ne 23 -or $RuntimeFileCount -ne 21)
{
    throw "Unexpected Static Gump Tool workspace file counts. Files=$($WorkspaceFiles.Count) RuntimeFiles=$RuntimeFileCount"
}

$ReviewRow = [pscustomobject]@{
    BatchId = 'POST-BATCH-H'
    ReviewRowId = $ReviewRowId
    BacklogId = $Row.Id
    SourceId = $Row.SourceId
    Priority = $Row.Priority
    HistoricalStatus = $Row.Status
    Category = $Row.Category
    System = $Row.System
    OriginalFiles = $Row.Files
    TargetFiles = ($WorkspaceFiles -join ';')
    OriginalEvidence = $Row.Evidence
    Risk = $Row.Risk
    RecommendedFix = $Row.RecommendedFix
    Decision = 'ExecutedMoveNoNamespaceChange'
    SourceEvidence = "Phase 12 proposal target=$TargetDir; Phase 6/serialization evidence has 0 Static Gump Tool serialized rows; ScriptsProjectTruth after move has $ScriptCompileIncludes includes, $ScriptSources script sources, 0 missing compile targets, 0 unincluded sources, and 0 project cleanup backlog groups; runtime-script-compile-inventory.csv has $($RuntimeTargetRows.Count) target rows, 0 old-path rows, and $($RuntimeInventory.Count) total runtime-visible script rows; runtime-hook-map.csv has $($HookRows.Count) Static Gump Tool hook rows ($($CommandRows.Count) Command, $($GumpRows.Count) Gump, $($InitializeRows.Count) Initialize)."
    Action = "Moved the Static Gump Tool workspace from $OldDir to $TargetDir; updated 21 Scripts.csproj Compile Include rows and one Content Include row; updated the moved folder AGENTS.md placement guidance, current source-trace docs, SystemAudit link, and the system-card generator path pattern; regenerated runtime script compile inventory plus Phase 1, 2, 3, 4, 5, 6, 7, 8, and 9 path-sensitive audit outputs."
    Verification = "Project truth passed with $ScriptCompileIncludes includes, $ScriptSources sources, 0 missing, 0 unincluded, and 0 cleanup backlog groups. Runtime script compile inventory contains $($RuntimeTargetRows.Count) moved Static Gump Tool rows and 0 old-path rows. Runtime hook map contains $($HookRows.Count) Static Gump Tool hook rows. Serialization register contains $($StaticSerializationRows.Count) Static Gump Tool rows. git diff --check: $DiffCheckResult. ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult. Server.csproj Debug/x86: $ServerBuildResult. ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult."
    Rollback = "Move $TargetDir back to $OldDir, restore the previous Scripts.csproj include/content paths, rerun project truth and path-sensitive audit generators, and revert Static Gump Tool documentation/source-trace paths."
    ReviewedBatchId = $ReviewBatchId
    ReviewedAt = $Timestamp
    Notes = 'No namespace, class, command name, public API, serialized type, save version, XML/config, gump button ID, staff command behavior, or gameplay behavior changed. The package-local AGENTS.md was preserved with byte-compatible path updates because it is not UTF-8 encoded. Historical Phase 12 and Phase 13 old-path rows remain proposal/rollback evidence, not current source location.'
}

@($ReviewRow) | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((@(Import-Csv -LiteralPath $ReviewPath)).Count -ne 1)
{
    throw 'Review CSV row count invariant failed.'
}

$Active = @(Import-Csv -LiteralPath $ActivePath | Where-Object { $_.BacklogId -ne $BacklogId })
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
    Commit = 'Pending current POST-BATCH-H-03A commit'
    UpdatedAt = $Timestamp
    Notes = $ReviewRow.Notes
}

@($Active + $OverlayRow) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8
$OverlayCount = @((Import-Csv -LiteralPath $ActivePath) | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-H' -and $_.BacklogId -eq $BacklogId }).Count

if ($OverlayCount -ne 1)
{
    throw "Active overlay POST-BATCH-H $BacklogId invariant failed: $OverlayCount"
}

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-H Static Gump Tool Move Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add(('`POST-BATCH-H-03A` executed the approved Phase 12 Static Gump Tool move from `{0}` to `{1}`. The batch moved the complete 23-file workspace, including 21 runtime-visible scripts, preserved all namespaces/classes/APIs, updated 21 `Scripts.csproj` compile include rows plus one content include row, updated current source-trace docs and package-local placement guidance, and regenerated path-sensitive audit outputs.' -f $OldDir, $TargetDir)) | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('The moved scripts remain under `Data/Scripts`, so live runtime script compile visibility is preserved. The serialization register reports zero Static Gump Tool serialized rows, so this move did not introduce a save-format migration.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Scope') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('| Evidence | Value |') | Out-Null
$Closeout.Add('| --- | --- |') | Out-Null
$Closeout.Add(('| Backlog row | `{0}` |' -f $BacklogId)) | Out-Null
$Closeout.Add(('| Original path | `{0}` |' -f $OldDir)) | Out-Null
$Closeout.Add(('| Target path | `{0}` |' -f $TargetDir)) | Out-Null
$Closeout.Add(('| Workspace files moved | `{0}` |' -f $WorkspaceFiles.Count)) | Out-Null
$Closeout.Add(('| Runtime-visible files moved | `{0}` |' -f $RuntimeFileCount)) | Out-Null
$Closeout.Add('| Namespace/type/API changes | `None` |') | Out-Null
$Closeout.Add(('| Serialized Static Gump Tool rows | `{0}` |' -f $StaticSerializationRows.Count)) | Out-Null
$Closeout.Add(('| Runtime hook rows | `{0}` |' -f $HookRows.Count)) | Out-Null
$Closeout.Add(('| Runtime script inventory target rows | `{0}` |' -f $RuntimeTargetRows.Count)) | Out-Null
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
$Closeout.Add("- runtime-script-compile-inventory.csv: regenerated with $($RuntimeInventory.Count) runtime-visible script rows, $($RuntimeTargetRows.Count) Static Gump Tool target rows, and 0 old-path rows.") | Out-Null
$Closeout.Add("- runtime-hook-map.csv: regenerated with $($HookRows.Count) Static Gump Tool hook rows ($($CommandRows.Count) Command, $($GumpRows.Count) Gump, $($InitializeRows.Count) Initialize).") | Out-Null
$Closeout.Add('- Regenerated Phase 1 inventory, cross-tree runtime inventory, system cards, runtime hook map, serialization register, documentation truth, dependency graph, and synergy/conflict matrix.') | Out-Null
$Closeout.Add("- git diff --check: $DiffCheckResult") | Out-Null
$Closeout.Add("- ConficturaUO.sln Debug/Any CPU: $SolutionBuildResult") | Out-Null
$Closeout.Add("- Server.csproj Debug/x86: $ServerBuildResult") | Out-Null
$Closeout.Add("- .\ConficturaServer.exe -compileonly -nocache: $CompileOnlyResult") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Rollback') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add(('Move `{0}` back to `{1}`, restore the previous `Scripts.csproj` include/content paths, rerun project truth and path-sensitive audit generators, and revert Static Gump Tool documentation/source-trace paths.' -f $TargetDir, $OldDir)) | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$NewEntries = @(
    '| `post-batch-h-static-gump-tool-move-review.csv` | Post-audit | Review and disposition `RB-06811` for the POST-BATCH-H-03A Static Gump Tool reorganization batch. | Complete |',
    '| `post-batch-h-static-gump-tool-move-closeout.md` | Post-audit | Close out the Static Gump Tool move with project truth, runtime visibility, serialization, verification, and rollback evidence. | Complete |'
)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-static-gump-tool-move-(review\.csv|closeout\.md)' })
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
$HSummary = 'Post-audit reorganization runner: `{0}` executed the Static Gump Tool Phase 12 move from `{1}` to `{2}`, moved 21 runtime-visible `.cs` files plus two package docs/rules files, updated 21 `Scripts.csproj` compile includes and one content include, updated current source-trace docs and package-local placement guidance, regenerated runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 path-sensitive outputs, and added an active overlay `Fixed` disposition for `{3}`. Verification: git diff --check={4}; solution Debug/Any CPU={5}; Server Debug/x86={6}; compile-only={7}.' -f $ReviewBatchId, $OldDir, $TargetDir, $BacklogId, $DiffCheckResult, $SolutionBuildResult, $ServerBuildResult, $CompileOnlyResult

if ($PhaseStatus -match 'Post-audit reorganization runner:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit reorganization runner:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $HSummary }, 1)
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
$RunLog.Add(('- Affected phase: Post-audit `{0}` Static Gump Tool reorganization batch' -f $ReviewBatchId)) | Out-Null
$RunLog.Add(('- Cwd: `{0}`' -f $RepoRoot)) | Out-Null
$RunLog.Add('- Command: move Static Gump Tool workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHStaticGumpToolMove.ps1`.') | Out-Null
$RunLog.Add("- Result: Review CSV and active overlay contain one POST-BATCH-H row for $BacklogId; project truth has $ScriptCompileIncludes includes, $ScriptSources sources, $MissingCompileTargets missing targets, $UnincludedSources unlisted sources, and $ProjectCleanupBacklog cleanup backlog rows; runtime script compile inventory has $($RuntimeTargetRows.Count) target rows and $($RuntimeOldRows.Count) old-path rows; serialization register has $($StaticSerializationRows.Count) Static Gump Tool rows; runtime hook map has $($HookRows.Count) Static Gump Tool hook rows. Final verification: git diff --check=$DiffCheckResult; solution=$SolutionBuildResult; server build=$ServerBuildResult; compile-only=$CompileOnlyResult.") | Out-Null
$RunLog.Add(('- Output path: `{0}`; `docs/codebase-audit/outputs/post-batch-h-static-gump-tool-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`' -f $ArtifactRel)) | Out-Null
$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H-03A` Static Gump Tool reorganization batch\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n", $Utf8NoBom)
Add-Content -LiteralPath $RunLogPath -Value ($RunLog -join "`n") -Encoding UTF8

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
    SerializationRows = $StaticSerializationRows.Count
    MissingCompileTargets = $MissingCompileTargets
    UnincludedSources = $UnincludedSources
}
