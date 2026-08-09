param(
    [string]$RepoRoot,
    [string]$OutputDir
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

$ReviewPath = Join-Path $OutputDir 'post-batch-g-project-include-drift-review.csv'
$CloseoutPath = Join-Path $OutputDir 'post-batch-g-project-include-drift-closeout.md'
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$Phase02SummaryPath = Join-Path $OutputDir 'phase-02-summary.md'
$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-G-01A'
$ArtifactRel = 'docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv'

$Backlog = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'repair-backlog.csv'))
$Rows = @($Backlog | Where-Object { $_.Category -eq 'Project include drift' } | Sort-Object Id)

if ($Rows.Count -ne 61)
{
    throw "Expected 61 POST-BATCH-G project include drift rows, found $($Rows.Count)."
}

$MissingCompileTargets = Get-CsvCount -Path (Join-Path $OutputDir 'missing-compile-targets.csv')
$UnincludedSources = Get-CsvCount -Path (Join-Path $OutputDir 'unincluded-source-files.csv')
$ProjectCleanupBacklog = Get-CsvCount -Path (Join-Path $OutputDir 'project-cleanup-backlog.csv')
$PhaseMissingCompileTargets = Get-CsvCount -Path (Join-Path $OutputDir 'phase-02-missing-compile-targets-classified.csv')
$PhaseUnincludedSources = Get-CsvCount -Path (Join-Path $OutputDir 'phase-02-unincluded-source-classified.csv')
$PhaseProjectCleanupBacklog = Get-CsvCount -Path (Join-Path $OutputDir 'phase-02-project-cleanup-backlog.csv')
$ProjectTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'project-truth-register.csv'))
$ScriptCompileIncludes = @($ProjectTruth | Where-Object { $_.RecordType -eq 'ProjectInclude' }).Count
$ScriptSources = @($ProjectTruth | Where-Object { $_.RecordType -eq 'SourceFile' }).Count

if ($MissingCompileTargets -ne 0 -or $UnincludedSources -ne 0 -or $ProjectCleanupBacklog -ne 0 -or
    $PhaseMissingCompileTargets -ne 0 -or $PhaseUnincludedSources -ne 0 -or $PhaseProjectCleanupBacklog -ne 0)
{
    throw "Project truth drift outputs are not all zero. Missing=$MissingCompileTargets Unincluded=$UnincludedSources Backlog=$ProjectCleanupBacklog PhaseMissing=$PhaseMissingCompileTargets PhaseUnincluded=$PhaseUnincludedSources PhaseBacklog=$PhaseProjectCleanupBacklog"
}

if ($ScriptCompileIncludes -ne 6581 -or $ScriptSources -ne 6581)
{
    throw "Unexpected project truth source/include counts. Includes=$ScriptCompileIncludes Sources=$ScriptSources"
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $Rows)
{
    $Index++
    $ReviewRowId = 'PBG-{0:D3}' -f $Index

    $ReviewRows.Add([pscustomobject]@{
        BatchId = 'POST-BATCH-G'
        ReviewRowId = $ReviewRowId
        BacklogId = $row.Id
        SourceId = $row.SourceId
        Priority = $row.Priority
        HistoricalStatus = $row.Status
        Category = $row.Category
        System = $row.System
        Files = $row.Files
        OriginalEvidence = $row.Evidence
        Risk = $row.Risk
        RecommendedFix = $row.RecommendedFix
        Decision = 'FixedProjectTruthDrift'
        SourceEvidence = "ScriptsProjectTruth after POST-BATCH-G: project-truth-register.csv has $ScriptCompileIncludes Scripts.csproj compile includes, $ScriptSources script source files, 0 missing compile targets, 0 unincluded source files, and 0 project cleanup backlog groups."
        Action = 'Reconciled the historical project include drift row to the already completed ScriptsProjectTruth and IDEProjectHygiene repairs; no source, project, runtime, namespace, serialization, XML, config, or file-location change was made in POST-BATCH-G.'
        Verification = 'Passed: New-ProjectTruthRegister.ps1 regenerated Phase 2 project truth; missing-compile-targets.csv, unincluded-source-files.csv, project-cleanup-backlog.csv, and phase-scoped equivalents are zero rows.'
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = 'This is IDE/project-hygiene reconciliation, not live runtime script compile proof. Direct Scripts.csproj Debug/AnyCPU keeps the known standalone Server.csproj platform limitation; maintained ConficturaUO.sln Debug/Any CPU previously passed with warnings after IDEProjectHygiene repair.'
    }) | Out-Null
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((@(Import-Csv -LiteralPath $ReviewPath)).Count -ne 61)
{
    throw 'Review CSV row count invariant failed.'
}

$Active = @(Import-Csv -LiteralPath $ActivePath | Where-Object { $_.PostAuditBatch -ne 'POST-BATCH-G' })
$OverlayRows = foreach ($review in $ReviewRows)
{
    [pscustomobject]@{
        BacklogId = $review.BacklogId
        SourceId = $review.SourceId
        Category = $review.Category
        System = $review.System
        Files = $review.Files
        HistoricalStatus = $review.HistoricalStatus
        ActiveStatus = 'Fixed'
        PostAuditBatch = 'POST-BATCH-G'
        ReviewRowId = $review.ReviewRowId
        ReviewStatus = $review.Decision
        Action = $review.Action
        ReviewArtifact = $ArtifactRel
        SourceEvidence = $review.SourceEvidence
        Commit = 'Pending current POST-BATCH-G commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($Active + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8
$OverlayCount = @((Import-Csv -LiteralPath $ActivePath) | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-G' }).Count

if ($OverlayCount -ne 61)
{
    throw "Active overlay POST-BATCH-G count invariant failed: $OverlayCount"
}

$PriorityCounts = @($ReviewRows | Group-Object Priority | Sort-Object Name)
$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$StatusCounts = @($OverlayRows | Group-Object ActiveStatus | Sort-Object Name)

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-G Project Include Drift Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-G` reviewed all 61 historical `Project include drift` rows from `repair-backlog.csv` and reconciled them to the already completed `ScriptsProjectTruth` and `IDEProjectHygiene` repairs. Current project truth has no remaining missing `Scripts.csproj` targets, no unlisted active script sources, and no project cleanup backlog groups.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('The batch is audit/project-hygiene bookkeeping only. It does not edit `Data/Scripts/Scripts.csproj`, runtime source, XML/config files, namespaces, serialized types, save versions, runtime file locations, or gameplay behavior, and it does not start `POST-BATCH-H` reorganization.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Scope Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-CountTable -Lines $Closeout -Groups $PriorityCounts -NameHeader 'Priority'
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
$Closeout.Add("| Evidence | Count |") | Out-Null
$Closeout.Add('| --- | ---: |') | Out-Null
$Closeout.Add("| Scripts.csproj compile includes | $ScriptCompileIncludes |") | Out-Null
$Closeout.Add("| Script source files | $ScriptSources |") | Out-Null
$Closeout.Add(('| `missing-compile-targets.csv` rows | {0} |' -f $MissingCompileTargets)) | Out-Null
$Closeout.Add(('| `unincluded-source-files.csv` rows | {0} |' -f $UnincludedSources)) | Out-Null
$Closeout.Add(('| `project-cleanup-backlog.csv` rows | {0} |' -f $ProjectCleanupBacklog)) | Out-Null
$Closeout.Add(('| `phase-02-missing-compile-targets-classified.csv` rows | {0} |' -f $PhaseMissingCompileTargets)) | Out-Null
$Closeout.Add(('| `phase-02-unincluded-source-classified.csv` rows | {0} |' -f $PhaseUnincludedSources)) | Out-Null
$Closeout.Add(('| `phase-02-project-cleanup-backlog.csv` rows | {0} |' -f $PhaseProjectCleanupBacklog)) | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- `git status --short` was clean before the batch.') | Out-Null
$Closeout.Add('- `rg --files -g AGENTS.md` was rerun; applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.') | Out-Null
$Closeout.Add('- `New-ProjectTruthRegister.ps1` regenerated Phase 2 project truth and reported 6,581 compile includes, 6,581 script source files, 0 missing compile targets, 0 unincluded source files, and 0 project cleanup backlog groups.') | Out-Null
$Closeout.Add('- The maintained `ConficturaUO.sln` Debug/`Any CPU` build previously passed with warnings after the `IDEProjectHygiene` repair. This closeout does not claim live runtime script compile proof.') | Out-Null
$Closeout.Add('- Direct `Data/Scripts/Scripts.csproj` Debug/`AnyCPU` still has the known standalone project-reference limitation because it passes `Debug|AnyCPU` to `Server.csproj`, which has no matching standalone output path outside solution mapping.') | Out-Null
$Closeout.Add('- Source/runtime/config/project verification was not run because this batch made no `.cs`, `.xml`, `.cfg`, `.csproj`, `Data/`, or runtime behavior changes.') | Out-Null
$Closeout.Add('- `git diff --check` remains required before staging and commit.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$NewEntries = @(
    '| `post-batch-g-project-include-drift-review.csv` | Post-audit | Review all 61 POST-BATCH-G historical project include drift rows and reconcile them to current zero-drift ScriptsProjectTruth evidence. | Complete |',
    '| `post-batch-g-project-include-drift-closeout.md` | Post-audit | Close out POST-BATCH-G with project truth counts, overlay status, known direct Scripts.csproj limitation, and verification notes. | Complete |'
)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-g-project-include-drift-(review\.csv|closeout\.md)' })
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
$GSummary = 'Post-audit project include drift closeout: `POST-BATCH-G-01A` reviewed all 61 historical project include drift rows in `outputs/post-batch-g-project-include-drift-review.csv` and reconciled them to current zero-drift `ScriptsProjectTruth` evidence. Current project truth reports 6,581 script source files, 6,581 `Scripts.csproj` compile includes, 0 missing compile targets, 0 active unlisted sources, and 0 project cleanup backlog groups. The direct `Data/Scripts/Scripts.csproj` Debug/`AnyCPU` limitation remains a known standalone project-reference issue, while the maintained `ConficturaUO.sln` Debug/`Any CPU` project-hygiene build previously passed with warnings. No source/project/runtime/config files changed in `POST-BATCH-G`.'

if ($PhaseStatus -match 'Post-audit project include drift closeout:')
{
    $PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit project include drift closeout:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $GSummary }, 1)
}
else
{
    $Marker = 'Post-audit latest runtime-risk review batch:'
    $PhaseStatus = $PhaseStatus -replace [regex]::Escape($Marker), ($GSummary + "`r`n`r`n" + $Marker)
}

$OverlaySummary = 'Post-audit active backlog overlay: `outputs/post-audit-active-backlog-status.csv` preserves historical `repair-backlog.csv` while recording 17 packet-handler dispositions, 285 reviewed save-compatibility dispositions across `POST-BATCH-B-34A` and prior `POST-BATCH-B` subbatches, 24 active `POST-BATCH-C-01A` runtime-hook/`PlayerMobile` coupling dispositions after `RB-01883` was superseded by `POST-BATCH-E-94A`, 406 `POST-BATCH-D` pooled enumerable fixes, 2 `POST-BATCH-D` false positives, 276 `POST-BATCH-E` runtime-hook/gump-guard dispositions, 540 `POST-BATCH-F` documentation/balance dispositions, and 61 `POST-BATCH-G` project include drift dispositions. The `POST-BATCH-E` review CSV has 292 reviewed rows through `POST-BATCH-E-100A`; `RB-01877` through `RB-01882`, `RB-01884` through `RB-01891`, and `RB-01892` through `RB-01893` keep their earlier `POST-BATCH-C-01A` active overlay dispositions to preserve unique BacklogIds. Comparing `repair-backlog.csv` against the `POST-BATCH-E` review CSV found no remaining unreviewed `Runtime hooks`, `Gump guards`, `Command access`, or `Regions` rows.'
$PhaseStatus = [regex]::Replace($PhaseStatus, 'Post-audit active backlog overlay:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $OverlaySummary }, 1)
[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseStatus, $Utf8NoBom)

$Phase02Summary = Get-Content -Raw -LiteralPath $Phase02SummaryPath
$Phase02Summary = [regex]::Replace($Phase02Summary, 'Generated: .*', "Generated: $Timestamp", 1)
$Phase02Summary = $Phase02Summary -replace 'Build verification is recorded separately in `phase-02-build-verification.md` after running the narrow Scripts build command\.', 'Build verification is recorded separately in `phase-02-build-verification.md`. Post-audit `IDEProjectHygiene` verification records the maintained `ConficturaUO.sln` Debug/`Any CPU` build passing with warnings after `Scripts.csproj` reference and portable PDB repairs. `POST-BATCH-G` is a reconciliation closeout only and does not claim live runtime script compile proof.'
$Phase02Summary = $Phase02Summary -replace 'Project repair is deferred to focused batches; no `Scripts\.csproj` changes are made by Phase 2\.', 'Post-audit project repair has reconciled `ScriptsProjectTruth`; post-audit Visual Studio reference and Debug PDB repairs are recorded as `IDEProjectHygiene`, not live runtime script compile proof. `POST-BATCH-G` records all historical project include drift rows as active-overlay fixed against current zero-drift project truth evidence.'
[System.IO.File]::WriteAllText($Phase02SummaryPath, $Phase02Summary, $Utf8NoBom)

$RunLog = New-Object System.Collections.Generic.List[string]
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Post-audit `POST-BATCH-G` project include drift closeout') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `git status --short`; `rg --files -g AGENTS.md`; read root/doc audit instructions, root audit plan, Phase 2 plan, Phase 13 plan, repair backlog, active overlay, and current project truth outputs.') | Out-Null
$RunLog.Add('- Result: Initial worktree was clean; applicable instructions and inputs were present; `repair-backlog.csv` contains 61 historical `Project include drift` rows.') | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv`') | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Phase 2 project truth verification for `POST-BATCH-G`') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`') | Out-Null
$RunLog.Add(('- Result: Succeeded. Counts are {0} `Scripts.csproj` compile includes, {1} script source files, 0 missing compile targets, 0 unincluded source files, 0 project cleanup backlog groups, and zero rows in phase-scoped missing/unincluded/project cleanup outputs.' -f $ScriptCompileIncludes, $ScriptSources)) | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/project-truth-register.csv`; `docs/codebase-audit/outputs/missing-compile-targets.csv`; `docs/codebase-audit/outputs/unincluded-source-files.csv`; `docs/codebase-audit/outputs/project-cleanup-backlog.csv`; `docs/codebase-audit/outputs/phase-02-summary.md`') | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Post-audit `POST-BATCH-G` audit state update') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `.\docs\codebase-audit\tools\New-PostBatchGReview.ps1`') | Out-Null
$RunLog.Add("- Result: Review CSV and active overlay each contain 61 POST-BATCH-G rows; all are `FixedProjectTruthDrift` / active `Fixed` reconciliations. No source, project, runtime, XML, config, namespace, serialization, save-version, runtime-location, or gameplay files changed.") | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv`; `docs/codebase-audit/outputs/post-batch-g-project-include-drift-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`') | Out-Null
Add-Content -LiteralPath $RunLogPath -Value ($RunLog -join "`n") -Encoding UTF8

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $OverlayCount
    ScriptCompileIncludes = $ScriptCompileIncludes
    ScriptSources = $ScriptSources
    MissingCompileTargets = $MissingCompileTargets
    UnincludedSources = $UnincludedSources
    ProjectCleanupBacklog = $ProjectCleanupBacklog
    Timestamp = $Timestamp
}
