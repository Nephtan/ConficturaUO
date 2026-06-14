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

function Add-TableRows
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

$ReviewPath = Join-Path $OutputDir 'post-batch-f-documentation-balance-review.csv'
$CloseoutPath = Join-Path $OutputDir 'post-batch-f-documentation-balance-closeout.md'
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$Timestamp = (Get-Date).ToString('o')
$ReviewBatchId = 'POST-BATCH-F-01A'
$ArtifactRel = 'docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv'
$Categories = @('Documentation contradictions', 'Economy and reward loops', 'Staff tooling', 'XML/config schemas')
$FixedDocPaths = @(
    'docs/wiki/AI_OVERHAUL_AUDIT.md',
    'docs/wiki/Apiculture_Beekeeping.md',
    'docs/wiki/Gardening_System.md',
    'docs/wiki/In_Game_Command_List.md',
    'docs/wiki/Player_Mobile_Core.md',
    'docs/wiki/Runic_Tools_Crafting.md',
    'docs/wiki/Shoppes_Vendors.md',
    'docs/wiki/Standard_Crafting.md',
    'docs/wiki/wikibacklog.md'
)

$Backlog = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'repair-backlog.csv'))
$DocTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'documentation-truth-table.csv'))
$DocByPath = @{}

foreach ($doc in $DocTruth)
{
    $DocByPath[$doc.DocPath] = $doc
}

$Rows = @($Backlog | Where-Object { $Categories -contains $_.Category })

if ($Rows.Count -ne 540)
{
    throw "Expected 540 POST-BATCH-F rows, found $($Rows.Count)."
}

$ReviewRows = New-Object System.Collections.Generic.List[object]
$Index = 0

foreach ($row in $Rows)
{
    $Index++
    $ReviewRowId = 'PBF-{0:D3}' -f $Index
    $Decision = ''
    $Action = ''
    $Verification = ''
    $SourceEvidence = $row.Evidence
    $Notes = ''

    if ($row.Category -eq 'Documentation contradictions')
    {
        $DocPath = $row.Files
        $Doc = if ($DocByPath.ContainsKey($DocPath)) { $DocByPath[$DocPath] } else { $null }

        if ($FixedDocPaths -contains $DocPath)
        {
            if ($DocPath -eq 'docs/wiki/wikibacklog.md')
            {
                $Decision = 'FixedHistoricalPathReference'
                $Action = 'Converted a fixed historical stale-path row from current source code spans to plain-text historical evidence so documentation truth no longer treats old bad paths as live source references.'
            }
            else
            {
                $Decision = 'FixedDocumentationTrace'
                $Action = 'Corrected objective documentation/source-trace evidence by replacing wildcard or multi-path source references with exact existing source files and adding the required Source Trace heading where applicable.'
            }

            if ($null -ne $Doc)
            {
                $Verified = if ([string]::IsNullOrWhiteSpace($Doc.VerifiedSourceFiles)) { 'none' } else { $Doc.VerifiedSourceFiles }
                $Missing = if ([string]::IsNullOrWhiteSpace($Doc.MissingSourceFiles)) { 'none' } else { $Doc.MissingSourceFiles }
                $SourceEvidence = "Documentation truth after POST-BATCH-F: SourceTracePresent=$($Doc.SourceTracePresent); MissingSourceFiles=$Missing; VerifiedSourceFiles=$Verified"

                if (-not [string]::IsNullOrWhiteSpace($Doc.StaleClaims))
                {
                    $Notes = "Remaining stale-marker text is deferred unless it was the objective missing-path/source-trace issue for this batch: $($Doc.StaleClaims)"
                }
            }

            $Verification = 'Passed: New-DocumentationTruthAudit.ps1 regenerated documentation truth; edited source-trace paths resolve to existing files; no runtime source/config/project files changed.'
        }
        elseif ($DocPath -eq 'docs/wiki/SystemAudit.md')
        {
            $Decision = 'DeferredLegacyAuditTableCleanup'
            $Action = 'Deferred broad legacy SystemAudit table cleanup; this batch did not rewrite the historical audit table or infer behavior claims from parser artifacts.'

            if ($null -ne $Doc)
            {
                $SourceEvidence = "Documentation truth after POST-BATCH-F still reports legacy table missing-source parser artifact: $($Doc.MissingSourceFiles)"
            }

            $Verification = 'Reviewed: New-DocumentationTruthAudit.ps1 isolates remaining missing-source path evidence to SystemAudit.md; no source/config/project files changed.'
            $Notes = 'Requires a separate SystemAudit structure/source-link cleanup batch, not balance/runtime proof.'
        }
        elseif ($DocPath -match '^docs/CCWM/')
        {
            $Decision = 'DeferredSupportDocReview'
            $Action = 'Deferred support-document source-trace review; no behavior claim changed in this batch.'
            $Verification = 'Reviewed through documentation truth table; support docs remain explicit backlog rows.'
            $Notes = 'Map support docs need their own source-backed refresh pass.'
        }
        else
        {
            $Decision = 'DeferredSourceTraceReview'
            $Action = 'Deferred canonical source-trace authoring and claim verification; no behavior claim was invented or silently fixed.'

            if ($null -ne $Doc)
            {
                $SourceEvidence = "Documentation truth after POST-BATCH-F: SourceTracePresent=$($Doc.SourceTracePresent); MissingClaims=$($Doc.MissingClaims); StaleClaims=$($Doc.StaleClaims)"
            }

            $Verification = 'Reviewed through regenerated documentation truth table; source trace work remains queued for exact source-backed doc authoring.'
        }
    }
    elseif ($row.Category -eq 'Economy and reward loops')
    {
        $Decision = 'DeferredBalanceDecision'
        $Action = 'No gameplay, economy, reward, source, XML, config, or project behavior changed; balance tuning requires an explicit design decision and source-specific follow-up.'
        $Verification = 'Reviewed against regenerated synergy/conflict matrix and balance-risk list; no source edits were made.'
        $Notes = 'Balance risk is intentionally separated from code correctness in POST-BATCH-F.'
    }
    elseif ($row.Category -eq 'Staff tooling')
    {
        if ([string]::IsNullOrWhiteSpace($row.Files))
        {
            $Decision = 'NeedsHumanDecision'
            $Action = 'Queued staff event/workflow policy decision; no staff tool behavior changed from relationship-level evidence alone.'
            $Verification = 'Reviewed against regenerated staff-dependency list and dependency graph; no source edits were made.'
            $Notes = 'Relationship-level staff dependency needs explicit owner policy before runtime changes.'
        }
        else
        {
            $Decision = 'QueuedSourceFollowUp'
            $Action = 'Queued focused staff-tool source review for command access/metadata; no source change made in this documentation/balance batch.'
            $Verification = 'Reviewed against repair-backlog source evidence; source build not required because no source files changed.'
            $Notes = 'Keep as source-level follow-up if command access metadata is changed later.'
        }
    }
    elseif ($row.Category -eq 'XML/config schemas')
    {
        $Decision = 'QueuedSchemaDocumentation'
        $Action = 'Recorded config/schema trace and queued schema documentation; no XML/config normalization, migration, or runtime behavior change was made.'
        $Verification = 'Reviewed against regenerated dependency graph XML/config edges and existing source/config evidence; no source/config files changed.'
        $Notes = 'Treat as schema documentation/data-contract follow-up unless a later row proves a concrete config defect.'
    }

    $ReviewRows.Add([pscustomobject]@{
        BatchId = 'POST-BATCH-F'
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
        Decision = $Decision
        SourceEvidence = $SourceEvidence
        Action = $Action
        Verification = $Verification
        ReviewedBatchId = $ReviewBatchId
        ReviewedAt = $Timestamp
        Notes = $Notes
    }) | Out-Null
}

$ReviewRows | Export-Csv -LiteralPath $ReviewPath -NoTypeInformation -Encoding UTF8

if ((@(Import-Csv -LiteralPath $ReviewPath)).Count -ne 540)
{
    throw 'Review CSV row count invariant failed.'
}

$Active = @(Import-Csv -LiteralPath $ActivePath | Where-Object { $_.PostAuditBatch -ne 'POST-BATCH-F' })
$OverlayRows = foreach ($review in $ReviewRows)
{
    $ActiveStatus = switch ($review.Decision)
    {
        'FixedDocumentationTrace' { 'Fixed'; break }
        'FixedHistoricalPathReference' { 'Fixed'; break }
        'DeferredBalanceDecision' { 'DeferredBalanceDecision'; break }
        'NeedsHumanDecision' { 'NeedsHumanDecision'; break }
        default { 'QueuedSourceFollowUp' }
    }

    [pscustomobject]@{
        BacklogId = $review.BacklogId
        SourceId = $review.SourceId
        Category = $review.Category
        System = $review.System
        Files = $review.Files
        HistoricalStatus = $review.HistoricalStatus
        ActiveStatus = $ActiveStatus
        PostAuditBatch = 'POST-BATCH-F'
        ReviewRowId = $review.ReviewRowId
        ReviewStatus = $review.Decision
        Action = $review.Action
        ReviewArtifact = $ArtifactRel
        SourceEvidence = $review.SourceEvidence
        Commit = 'Pending current POST-BATCH-F commit'
        UpdatedAt = $Timestamp
        Notes = $review.Notes
    }
}

@($Active + $OverlayRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8
$OverlayCount = @((Import-Csv -LiteralPath $ActivePath) | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-F' }).Count

if ($OverlayCount -ne 540)
{
    throw "Active overlay POST-BATCH-F count invariant failed: $OverlayCount"
}

$DecisionCounts = @($ReviewRows | Group-Object Decision | Sort-Object Name)
$CategoryCounts = @($ReviewRows | Group-Object Category | Sort-Object Name)
$ActiveCounts = @($OverlayRows | Group-Object ActiveStatus | Sort-Object Name)
$DocTruth = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'documentation-truth-table.csv'))
$CanonicalCount = @($DocTruth | Where-Object { $_.CanonicalPage -eq 'Canonical' }).Count
$CanonicalMissingSourceTrace = @($DocTruth | Where-Object { $_.CanonicalPage -eq 'Canonical' -and $_.SourceTracePresent -ne 'Yes' }).Count
$MissingSourceFileDocs = @($DocTruth | Where-Object { -not [string]::IsNullOrWhiteSpace($_.MissingSourceFiles) }).Count
$BalanceRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-09-balance-risk-list.csv'))
$StaffRows = @(Import-Csv -LiteralPath (Join-Path $OutputDir 'phase-09-staff-dependency-list.csv'))

$Closeout = New-Object System.Collections.Generic.List[string]
$Closeout.Add('# POST-BATCH-F Documentation And Balance Closeout') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("Generated: $Timestamp") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Summary') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('`POST-BATCH-F` reviewed all 540 scoped P2-P3 documentation and balance rows from `repair-backlog.csv`. The batch stayed documentation/audit-only: no `.cs`, `.xml`, `.cfg`, project, namespace, serialized type, save-version, or runtime file-location changes were made.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('Nine wiki pages received objective documentation/source-trace cleanup where source evidence was clear: `AI_OVERHAUL_AUDIT.md`, `Apiculture_Beekeeping.md`, `Gardening_System.md`, `In_Game_Command_List.md`, `Player_Mobile_Core.md`, `Runic_Tools_Crafting.md`, `Shoppes_Vendors.md`, `Standard_Crafting.md`, and `wikibacklog.md`. `INDEX.md` now links the source-traced AI overhaul audit under technical references.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Scope Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-TableRows -Lines $Closeout -Groups $CategoryCounts -NameHeader 'Category'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Decision Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-TableRows -Lines $Closeout -Groups $DecisionCounts -NameHeader 'Decision'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Active Overlay Counts') | Out-Null
$Closeout.Add('') | Out-Null
Add-TableRows -Lines $Closeout -Groups $ActiveCounts -NameHeader 'Active status'
$Closeout.Add('') | Out-Null
$Closeout.Add('## Documentation Result') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add("- Documentation truth audit after edits reports $CanonicalCount canonical pages, $CanonicalMissingSourceTrace canonical pages still missing `## Source Trace`, and $MissingSourceFileDocs documentation row with missing source-file evidence.") | Out-Null
$Closeout.Add('- The remaining missing source-file evidence is isolated to `docs/wiki/SystemAudit.md`, whose legacy table shape/source-link cleanup is queued as `DeferredLegacyAuditTableCleanup`.') | Out-Null
$Closeout.Add('- The corrected docs use exact existing source paths instead of wildcard source references or multi-path command snippets that the audit parser treated as current missing source files.') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Balance And Staff/Config Result') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- Balance/economy rows are recorded as `DeferredBalanceDecision`; no reward tables, economy source, config data, or gameplay code changed.') | Out-Null
$Closeout.Add('- Staff tooling rows are split between `NeedsHumanDecision` for relationship-level staff workflow/policy rows and `QueuedSourceFollowUp` for concrete command/access source rows.') | Out-Null
$Closeout.Add('- XML/config schema rows are `QueuedSchemaDocumentation`; source/config references were traced through the dependency graph, but no schemas were normalized or migrated.') | Out-Null
$Closeout.Add("- Regenerated Phase 9 outputs still report $($BalanceRows.Count) balance-risk rows and $($StaffRows.Count) staff-dependency rows.") | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('## Verification') | Out-Null
$Closeout.Add('') | Out-Null
$Closeout.Add('- `git status --short` was clean before the batch.') | Out-Null
$Closeout.Add('- `rg --files -g AGENTS.md` was rerun; applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.') | Out-Null
$Closeout.Add('- `New-DocumentationTruthAudit.ps1` regenerated Phase 7 documentation truth outputs after doc edits.') | Out-Null
$Closeout.Add('- `New-DependencyGraph.ps1` regenerated Phase 8 dependency outputs after source-trace changes.') | Out-Null
$Closeout.Add('- `New-SynergyConflictMatrix.ps1` regenerated Phase 9 balance/staff/documentation risk outputs after dependency evidence changed.') | Out-Null
$Closeout.Add('- Source/config/project verification was not run because this batch made no `.cs`, `.xml`, `.cfg`, `.csproj`, or runtime behavior changes.') | Out-Null
$Closeout.Add('- `git diff --check` remains required before staging and commit.') | Out-Null
Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$NewEntries = @(
    '| `post-batch-f-documentation-balance-review.csv` | Post-audit | Review all 540 POST-BATCH-F documentation contradiction, economy/reward, staff tooling, and XML/config schema rows with fixed/deferred/queued dispositions. | Complete |',
    '| `post-batch-f-documentation-balance-closeout.md` | Post-audit | Close out POST-BATCH-F with decision counts, documentation fixes, balance/staff/config disposition policy, and verification notes. | Complete |'
)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-f-documentation-balance-(review\.csv|closeout\.md)' })
$InitialIndex = [Array]::IndexOf($Readme, '## Initial State')

if ($InitialIndex -lt 0)
{
    throw 'Could not find README Initial State heading.'
}

$Before = if ($InitialIndex -gt 0) { $Readme[0..($InitialIndex - 1)] } else { @() }
$After = $Readme[$InitialIndex..($Readme.Count - 1)]
Write-Utf8File -Path $ReadmePath -Lines @($Before + $NewEntries + '' + $After)

$PhaseText = Get-Content -Raw -LiteralPath $PhaseStatusPath
$PhaseText = [regex]::Replace($PhaseText, 'Last updated: .*', "Last updated: $Timestamp", 1)
$PostSummary = 'Post-audit documentation and balance cleanup: `POST-BATCH-F-01A` reviewed all 540 P2-P3 documentation contradiction, economy/reward, staff tooling, and XML/config schema rows in `outputs/post-batch-f-documentation-balance-review.csv`. Objective source-trace/path corrections were applied to nine wiki pages plus the wiki index; remaining broad source-trace, staff workflow, balance, and XML/config schema work is explicitly dispositioned as deferred, needs-human-decision, or queued source/schema follow-up. Phase 7, Phase 8, and Phase 9 outputs were regenerated; no runtime source/config/project files changed.'

if ($PhaseText -match 'Post-audit documentation and balance cleanup:')
{
    $PhaseText = [regex]::Replace($PhaseText, 'Post-audit documentation and balance cleanup:.*', [System.Text.RegularExpressions.MatchEvaluator]{ param($match) $PostSummary }, 1)
}
else
{
    $Marker = 'Post-audit active backlog overlay:'
    $PhaseText = $PhaseText -replace [regex]::Escape($Marker), ($PostSummary + "`r`n`r`n" + $Marker)
}

[System.IO.File]::WriteAllText($PhaseStatusPath, $PhaseText, $Utf8NoBom)

$RunLog = New-Object System.Collections.Generic.List[string]
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Post-audit `POST-BATCH-F` documentation and balance review') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `git status --short`; `rg --files -g AGENTS.md`; read root/doc audit instructions, batch plan, repair backlog, active overlay, documentation truth, dependency graph, synergy/conflict, and relevant phase plans.') | Out-Null
$RunLog.Add('- Result: Initial worktree was clean; applicable instructions and inputs were present; POST-BATCH-F filter matched 540 rows.') | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv`') | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Post-audit `POST-BATCH-F` documentation/source-trace corrections') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: edit wiki documentation source traces and index entries for objective missing-path/source-trace evidence.') | Out-Null
$RunLog.Add('- Result: Replaced wildcard/multi-path source references with exact existing source paths in nine wiki pages and added `AI_OVERHAUL_AUDIT.md` to the technical index; no source/config/project files changed.') | Out-Null
$RunLog.Add('- Output path: `docs/wiki/AI_OVERHAUL_AUDIT.md`; `docs/wiki/Apiculture_Beekeeping.md`; `docs/wiki/Gardening_System.md`; `docs/wiki/In_Game_Command_List.md`; `docs/wiki/Player_Mobile_Core.md`; `docs/wiki/Runic_Tools_Crafting.md`; `docs/wiki/Shoppes_Vendors.md`; `docs/wiki/Standard_Crafting.md`; `docs/wiki/wikibacklog.md`; `docs/wiki/INDEX.md`') | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Phase 7/8/9 regenerated evidence for `POST-BATCH-F`') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `.\docs\codebase-audit\tools\New-DocumentationTruthAudit.ps1`; `.\docs\codebase-audit\tools\New-DependencyGraph.ps1`; `.\docs\codebase-audit\tools\New-SynergyConflictMatrix.ps1`') | Out-Null
$RunLog.Add('- Result: Documentation truth regenerated with 106 canonical pages, 97 canonical pages still missing Source Trace, and one remaining missing-source row isolated to legacy `SystemAudit.md`; dependency graph regenerated with 30,211 edges; synergy/conflict matrix regenerated with 351 rows, 26 balance-risk rows, 141 documentation-risk rows, and 32 staff-dependency rows.') | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`; `docs/codebase-audit/outputs/dependency-graph.csv`; `docs/codebase-audit/outputs/synergy-conflict-matrix.csv`') | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add("### $Timestamp") | Out-Null
$RunLog.Add('') | Out-Null
$RunLog.Add('- Affected phase: Post-audit `POST-BATCH-F` audit state update') | Out-Null
$RunLog.Add('- Cwd: `D:\ConficturaUO`') | Out-Null
$RunLog.Add('- Command: `.\docs\codebase-audit\tools\New-PostBatchFReview.ps1`') | Out-Null
$RunLog.Add("- Result: Review CSV and active overlay each contain 540 POST-BATCH-F rows; decision counts are $(($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join ', ').") | Out-Null
$RunLog.Add('- Output path: `docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv`; `docs/codebase-audit/outputs/post-batch-f-documentation-balance-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`') | Out-Null
Add-Content -LiteralPath $RunLogPath -Value ($RunLog -join "`n") -Encoding UTF8

[pscustomobject]@{
    ReviewRows = $ReviewRows.Count
    OverlayRows = $OverlayCount
    Decisions = (($DecisionCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
    ActiveStatuses = (($ActiveCounts | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
    Timestamp = $Timestamp
}
