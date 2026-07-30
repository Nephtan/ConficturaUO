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

$RepoRoot = $RepoRoot.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
$OutputDir = [System.IO.Path]::GetFullPath($OutputDir)

function Export-AuditCsv
{
    param(
        [object[]]$Rows,
        [string]$FileName
    )

    $path = Join-Path $OutputDir $FileName
    $Rows | Export-Csv -LiteralPath $path -NoTypeInformation -Encoding UTF8
    return $path
}

function Convert-ToRepoPath
{
    param([string]$Path)

    $fullPath = [System.IO.Path]::GetFullPath($Path)

    if ($fullPath.StartsWith($RepoRoot, [System.StringComparison]::OrdinalIgnoreCase))
    {
        return ($fullPath.Substring($RepoRoot.Length).TrimStart('\', '/') -replace '\\', '/')
    }

    return ($Path -replace '\\', '/')
}

function Get-RequiredInput
{
    param([string]$FileName)

    $path = Join-Path $OutputDir $FileName
    if (-not (Test-Path -LiteralPath $path))
    {
        throw "Required Phase 13 input is missing: $FileName"
    }

    return $path
}

function Trim-Text
{
    param(
        [string]$Value,
        [int]$MaxLength = 320
    )

    if ($null -eq $Value)
    {
        return ''
    }

    $oneLine = ($Value -replace "`r?`n", ' ').Trim()
    if ($oneLine.Length -le $MaxLength)
    {
        return $oneLine
    }

    return ($oneLine.Substring(0, $MaxLength - 3) + '...')
}

function Get-Category
{
    param([string]$Track)

    switch -Regex ($Track)
    {
        'Build Inclusion' { return 'Project include drift' }
        'Serialization' { return 'Save compatibility' }
        'Global Hooks' { return 'Runtime hooks' }
        'Packet' { return 'Packet handlers' }
        'Gump' { return 'Gump guards' }
        'Command' { return 'Command access' }
        'Pooled' { return 'Pooled enumerables' }
        'Region' { return 'Region and map assumptions' }
        'PlayerMobile' { return 'PlayerMobile coupling' }
        'Economy' { return 'Economy and reward loops' }
        'Staff' { return 'Staff tooling' }
        'Legacy' { return 'Legacy compatibility' }
        'XML' { return 'XML/config schemas' }
        'Documentation' { return 'Documentation contradictions' }
        default { return 'General audit finding' }
    }
}

function Get-Status
{
    param(
        [string]$Priority,
        [string]$File,
        [string]$Evidence,
        [string]$RecommendedFix,
        [string]$Verification
    )

    if ([string]::IsNullOrWhiteSpace($Evidence) -or [string]::IsNullOrWhiteSpace($RecommendedFix) -or [string]::IsNullOrWhiteSpace($Verification))
    {
        return 'Open'
    }

    if ($Priority -eq 'P0' -or $Priority -eq 'P1')
    {
        return 'Ready'
    }

    if ([string]::IsNullOrWhiteSpace($File))
    {
        return 'Open'
    }

    return 'Ready'
}

$projectTruthPath = Get-RequiredInput 'project-truth-register.csv'
$systemCardsPath = Get-RequiredInput 'phase-04-system-card-index.csv'
$runtimeHookPath = Get-RequiredInput 'runtime-hook-map.csv'
$serializationPath = Get-RequiredInput 'serialization-register.csv'
$documentationPath = Get-RequiredInput 'documentation-truth-table.csv'
$dependencyPath = Get-RequiredInput 'dependency-graph.csv'
$synergyPath = Get-RequiredInput 'synergy-conflict-matrix.csv'
$findingPath = Get-RequiredInput 'risk-track-findings.csv'
$phase10BacklogPath = Get-RequiredInput 'phase-10-repair-backlog-items.csv'
$phase10AcceptedPath = Get-RequiredInput 'phase-10-accepted-risk-notes.csv'
$phase11RejectedPath = Get-RequiredInput 'phase-11-rejected-comment-list.csv'
$phase12MovesPath = Get-RequiredInput 'phase-12-move-proposal-table.csv'

$projectTruth = @(Import-Csv -LiteralPath $projectTruthPath)
$systemCards = @(Import-Csv -LiteralPath $systemCardsPath)
$runtimeHooks = @(Import-Csv -LiteralPath $runtimeHookPath)
$serializers = @(Import-Csv -LiteralPath $serializationPath)
$documentation = @(Import-Csv -LiteralPath $documentationPath)
$dependencies = @(Import-Csv -LiteralPath $dependencyPath)
$synergy = @(Import-Csv -LiteralPath $synergyPath)
$findings = @(Import-Csv -LiteralPath $findingPath)
$phase10Backlog = @(Import-Csv -LiteralPath $phase10BacklogPath)
$phase10Accepted = @(Import-Csv -LiteralPath $phase10AcceptedPath)
$phase11Rejected = @(Import-Csv -LiteralPath $phase11RejectedPath)
$phase12Moves = @(Import-Csv -LiteralPath $phase12MovesPath)

$backlogRows = New-Object System.Collections.ArrayList

foreach ($item in $phase10Backlog)
{
    $priority = $item.Priority
    if ([string]::IsNullOrWhiteSpace($priority))
    {
        $priority = 'P3'
    }

    [void]$backlogRows.Add([pscustomobject]@{
        Id = 'RB-{0:00000}' -f ($backlogRows.Count + 1)
        SourceId = $item.SourceFindingId
        Priority = $priority
        Status = Get-Status $priority $item.File $item.Evidence $item.Recommendation $item.Verification
        System = $item.System
        Category = Get-Category $item.Track
        Files = $item.File
        Evidence = Trim-Text $item.Evidence 700
        Risk = $item.Risk
        RecommendedFix = Trim-Text $item.Recommendation 500
        Verification = Trim-Text $item.Verification 400
        Notes = Trim-Text $item.Notes 400
        Source = 'Phase 10 risk-specific review'
    })
}

foreach ($move in $phase12Moves)
{
    [void]$backlogRows.Add([pscustomobject]@{
        Id = 'RB-{0:00000}' -f ($backlogRows.Count + 1)
        SourceId = ''
        Priority = 'P4'
        Status = 'Deferred'
        System = $move.System
        Category = 'Folder and namespace cleanup'
        Files = $move.CurrentPath
        Evidence = ("Move proposal to {0}; save risk={1}; serialized types={2}; risk findings={3}" -f $move.TargetPath, $move.SaveRisk, $move.SerializedTypeCount, $move.RiskFindingCount)
        Risk = 'Organization;Build;Docs;Save'
        RecommendedFix = 'Execute only as a focused future move batch after required gates are met.'
        Verification = $move.Verification
        Notes = ("Approval gate: {0}; rollback: {1}" -f $move.ApprovalGate, $move.Rollback)
        Source = 'Phase 12 reorganization design'
    })
}

$acceptedRows = New-Object System.Collections.ArrayList
foreach ($risk in $phase10Accepted)
{
    [void]$acceptedRows.Add([pscustomobject]@{
        Id = 'AR-{0:0000}' -f ($acceptedRows.Count + 1)
        SourceId = $risk.AcceptedRiskId
        Scope = $risk.System
        Risk = $risk.Risk
        Rationale = $risk.Rationale
        Evidence = $risk.Evidence
        Status = $risk.Status
        ReviewTrigger = $risk.NextReview
    })
}

$deferredRows = New-Object System.Collections.ArrayList
foreach ($group in @($phase11Rejected | Group-Object Decision, CommentType))
{
    $first = $group.Group | Select-Object -First 1
    [void]$deferredRows.Add([pscustomobject]@{
        Id = 'DEF-{0:0000}' -f ($deferredRows.Count + 1)
        Source = 'Phase 11 comment review'
        Category = 'Inline comments'
        System = 'Multiple'
        Files = ''
        Count = $group.Count
        Reason = $first.DecisionReason
        NextAction = 'Review representative targets during the matching repair batch; add comments only with source-specific reasons.'
        Verification = 'Rerun comment target register and source diff check after comments are applied.'
        Status = 'Deferred'
    })
}

foreach ($move in $phase12Moves)
{
    [void]$deferredRows.Add([pscustomobject]@{
        Id = 'DEF-{0:0000}' -f ($deferredRows.Count + 1)
        Source = 'Phase 12 reorganization design'
        Category = 'Folder and namespace cleanup'
        System = $move.System
        Files = $move.CurrentPath
        Count = 1
        Reason = $move.ApprovalGate
        NextAction = 'Open a focused move batch only after project truth, save compatibility, docs, and build gates are satisfied.'
        Verification = $move.Verification
        Status = 'Deferred'
    })
}

$batchRows = @(
    [pscustomobject]@{ BatchId = 'BATCH-001'; Priority = 'P0'; Category = 'Project include drift'; Scope = 'Scripts.csproj missing compile targets and unlisted active sources'; BacklogFilter = 'Category=Project include drift'; Verification = 'Run New-ProjectTruthRegister.ps1; run Scripts.csproj or maintained solution build.'; Notes = 'Unblocks reliable builds and later move verification.' },
    [pscustomobject]@{ BatchId = 'BATCH-002'; Priority = 'P0'; Category = 'Packet handlers'; Scope = 'Critical packet handler rows'; BacklogFilter = 'Category=Packet handlers'; Verification = 'Runtime hook map; manual packet id/length/access review; build.'; Notes = 'Network entry points first after build drift.' },
    [pscustomobject]@{ BatchId = 'BATCH-003'; Priority = 'P0-P1'; Category = 'Save compatibility'; Scope = 'Serialization order/versioning and PlayerMobile coupling'; BacklogFilter = 'Category in Save compatibility,PlayerMobile coupling'; Verification = 'Serialization register diff; migration plan if needed; save-load test where available; build.'; Notes = 'No namespace/type rename without approval.' },
    [pscustomobject]@{ BatchId = 'BATCH-004'; Priority = 'P1'; Category = 'Pooled enumerables'; Scope = 'Range scans and IPooledEnumerable ownership'; BacklogFilter = 'Category=Pooled enumerables'; Verification = 'Source review; pooled enumerable scan; build.'; Notes = 'Prefer repairs over comments for suspected leaks.' },
    [pscustomobject]@{ BatchId = 'BATCH-005'; Priority = 'P1-P2'; Category = 'Runtime hooks and gumps'; Scope = 'Global hooks, gump guards, command access, region/map assumptions'; BacklogFilter = 'Category in Runtime hooks,Gump guards,Command access,Region and map assumptions'; Verification = 'Runtime hook map; gump review; command access review; build.'; Notes = 'Group by system to keep commits small.' },
    [pscustomobject]@{ BatchId = 'BATCH-006'; Priority = 'P2-P3'; Category = 'Docs and balance'; Scope = 'Documentation contradictions, economy/reward loops, staff tooling, XML/config schemas'; BacklogFilter = 'Category in Documentation contradictions,Economy and reward loops,Staff tooling,XML/config schemas'; Verification = 'Docs truth audit; dependency graph; source trace checks; targeted source review.'; Notes = 'Separate player-facing claims from balance tuning.' },
    [pscustomobject]@{ BatchId = 'BATCH-007'; Priority = 'P4'; Category = 'Folder and namespace cleanup'; Scope = 'Phase 12 move proposals'; BacklogFilter = 'Category=Folder and namespace cleanup'; Verification = 'Project truth; serialization register; dependency graph; docs truth; build; rollback check.'; Notes = 'Only after earlier risk batches and explicit approvals.' }
)

$verificationRows = @(
    [pscustomobject]@{ Category = 'Documentation contradictions'; RequiredVerification = 'Run New-DocumentationTruthAudit.ps1; check source traces and aliases; run git diff --check.'; BuildRequired = 'No unless source/project files changed.' },
    [pscustomobject]@{ Category = 'Project include drift'; RequiredVerification = 'Run New-ProjectTruthRegister.ps1; compare missing/unincluded deltas; run maintained solution or Scripts.csproj build.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Save compatibility'; RequiredVerification = 'Run New-SerializationRegister.ps1 before/after; compare write/read order and version gates; run build; perform save-load test where available.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Runtime hooks'; RequiredVerification = 'Run New-RuntimeHookMap.ps1; review access/null/deleted/range/map guards; run build.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Packet handlers'; RequiredVerification = 'Review packet ids/lengths/NetState/access; run New-RuntimeHookMap.ps1; build.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Gump guards'; RequiredVerification = 'Review OnResponse button/text/null/deleted/stale-list guards; run runtime hook/gump scan; build.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Pooled enumerables'; RequiredVerification = 'Run Phase 10 pooled enumerable scan; confirm Free on all paths; build.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Folder and namespace cleanup'; RequiredVerification = 'Project truth, docs truth, dependency graph, serialization register, and build after every move; rollback path checked.'; BuildRequired = 'Yes.' },
    [pscustomobject]@{ Category = 'Inline comments'; RequiredVerification = 'Run comment target register; git diff --check; build if source comments changed and project truth allows.'; BuildRequired = 'Yes for source comments.' },
    [pscustomobject]@{ Category = 'Economy and reward loops'; RequiredVerification = 'Review source reward/resource paths; rerun synergy matrix; build if source changes.'; BuildRequired = 'Only if source changes.' }
)

$canonicalBacklogPath = Export-AuditCsv @($backlogRows) 'repair-backlog.csv'
$phaseBacklogPath = Export-AuditCsv @($backlogRows) 'phase-13-repair-backlog.csv'
$acceptedPath = Export-AuditCsv @($acceptedRows) 'accepted-risk-register.csv'
$phaseAcceptedPath = Export-AuditCsv @($acceptedRows) 'phase-13-accepted-risk-register.csv'
$deferredPath = Export-AuditCsv @($deferredRows) 'deferred-work-register.csv'
$phaseDeferredPath = Export-AuditCsv @($deferredRows) 'phase-13-deferred-work-register.csv'
$batchPath = Export-AuditCsv @($batchRows) 'phase-13-batch-plan.csv'
$verificationPath = Export-AuditCsv @($verificationRows) 'verification-matrix.csv'
$phaseVerificationPath = Export-AuditCsv @($verificationRows) 'phase-13-verification-matrix.csv'

$priorityCounts = @{}
foreach ($row in $backlogRows)
{
    if (-not $priorityCounts.ContainsKey($row.Priority))
    {
        $priorityCounts[$row.Priority] = 0
    }
    $priorityCounts[$row.Priority]++
}

$statusCounts = @{}
foreach ($row in $backlogRows)
{
    if (-not $statusCounts.ContainsKey($row.Status))
    {
        $statusCounts[$row.Status] = 0
    }
    $statusCounts[$row.Status]++
}

$categoryCounts = @{}
foreach ($row in $backlogRows)
{
    if (-not $categoryCounts.ContainsKey($row.Category))
    {
        $categoryCounts[$row.Category] = 0
    }
    $categoryCounts[$row.Category]++
}

$summaryPath = Join-Path $OutputDir 'phase-13-summary.md'
$summary = New-Object System.Collections.ArrayList
[void]$summary.Add('# Phase 13 Repair Backlog Summary')
[void]$summary.Add('')
[void]$summary.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$summary.Add('')
[void]$summary.Add('## Required Inputs')
[void]$summary.Add('')
[void]$summary.Add('| Input | Status |')
[void]$summary.Add('| --- | --- |')
[void]$summary.Add(('| Project Truth Register | Present: `project-truth-register.csv` with {0} rows |' -f $projectTruth.Count))
[void]$summary.Add(('| System Cards | Present: `phase-04-system-card-index.csv` with {0} rows |' -f $systemCards.Count))
[void]$summary.Add(('| Runtime Hook Map | Present: `runtime-hook-map.csv` with {0} rows |' -f $runtimeHooks.Count))
[void]$summary.Add(('| Serialization Register | Present: `serialization-register.csv` with {0} rows |' -f $serializers.Count))
[void]$summary.Add(('| Documentation Truth Table | Present: `documentation-truth-table.csv` with {0} rows |' -f $documentation.Count))
[void]$summary.Add(('| Dependency Graph | Present: `dependency-graph.csv` with {0} rows |' -f $dependencies.Count))
[void]$summary.Add(('| Synergy and Conflict Matrix | Present: `synergy-conflict-matrix.csv` with {0} rows |' -f $synergy.Count))
[void]$summary.Add(('| Risk-Specific Findings | Present: `risk-track-findings.csv` with {0} rows |' -f $findings.Count))
[void]$summary.Add('')
[void]$summary.Add('## Generated Outputs')
[void]$summary.Add('')
[void]$summary.Add('| Output | Rows | Purpose |')
[void]$summary.Add('| --- | ---: | --- |')
[void]$summary.Add(('| `repair-backlog.csv` | {0} | Canonical prioritized repair backlog. |' -f $backlogRows.Count))
[void]$summary.Add(('| `phase-13-repair-backlog.csv` | {0} | Phase-scoped backlog copy. |' -f $backlogRows.Count))
[void]$summary.Add(('| `accepted-risk-register.csv` | {0} | Canonical accepted-risk register. |' -f $acceptedRows.Count))
[void]$summary.Add(('| `deferred-work-register.csv` | {0} | Canonical deferred work register. |' -f $deferredRows.Count))
[void]$summary.Add(('| `phase-13-batch-plan.csv` | {0} | Small-batch implementation plan. |' -f $batchRows.Count))
[void]$summary.Add(('| `verification-matrix.csv` | {0} | Canonical category verification matrix. |' -f $verificationRows.Count))
[void]$summary.Add('')
[void]$summary.Add('## Priority Counts')
[void]$summary.Add('')
[void]$summary.Add('| Priority | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($priority in @($priorityCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $priority, $priorityCounts[$priority]))
}
[void]$summary.Add('')
[void]$summary.Add('## Status Counts')
[void]$summary.Add('')
[void]$summary.Add('| Status | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($status in @($statusCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $status, $statusCounts[$status]))
}
[void]$summary.Add('')
[void]$summary.Add('## Category Counts')
[void]$summary.Add('')
[void]$summary.Add('| Category | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($category in @($categoryCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $category, $categoryCounts[$category]))
}
[void]$summary.Add('')
[void]$summary.Add('## Exit Criteria')
[void]$summary.Add('')
[void]$summary.Add('- Every Phase 10 finding has a repair backlog item.')
[void]$summary.Add('- Phase 12 move proposals are deferred organization backlog items with verification and rollback requirements.')
[void]$summary.Add('- Accepted risks and deferred work are explicit durable registers.')
[void]$summary.Add('- Batch plan and verification matrix let future repair batches proceed without rediscovery.')

[System.IO.File]::WriteAllLines($summaryPath, [string[]]$summary, (New-Object System.Text.UTF8Encoding($false)))

[pscustomobject]@{
    RepairBacklog = Convert-ToRepoPath $canonicalBacklogPath
    PhaseRepairBacklog = Convert-ToRepoPath $phaseBacklogPath
    AcceptedRiskRegister = Convert-ToRepoPath $acceptedPath
    PhaseAcceptedRiskRegister = Convert-ToRepoPath $phaseAcceptedPath
    DeferredWorkRegister = Convert-ToRepoPath $deferredPath
    PhaseDeferredWorkRegister = Convert-ToRepoPath $phaseDeferredPath
    BatchPlan = Convert-ToRepoPath $batchPath
    VerificationMatrix = Convert-ToRepoPath $verificationPath
    PhaseVerificationMatrix = Convert-ToRepoPath $phaseVerificationPath
    Summary = Convert-ToRepoPath $summaryPath
    BacklogRows = $backlogRows.Count
    AcceptedRiskRows = $acceptedRows.Count
    DeferredRows = $deferredRows.Count
    BatchRows = $batchRows.Count
    VerificationRows = $verificationRows.Count
}
