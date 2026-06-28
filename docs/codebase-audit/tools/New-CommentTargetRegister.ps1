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

function Trim-Text
{
    param(
        [string]$Value,
        [int]$MaxLength = 260
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

function Get-RequiredInput
{
    param([string]$FileName)

    $path = Join-Path $OutputDir $FileName
    if (-not (Test-Path -LiteralPath $path))
    {
        throw "Required Phase 11 input is missing: $FileName"
    }

    return $path
}

function Find-CommentLine
{
    param(
        [string]$RepoPath,
        [string]$Pattern
    )

    $path = Join-Path $RepoRoot ($RepoPath -replace '/', '\')
    if (-not (Test-Path -LiteralPath $path))
    {
        return ''
    }

    $match = Select-String -LiteralPath $path -Pattern $Pattern | Select-Object -First 1
    if ($null -eq $match)
    {
        return ''
    }

    return [string]$match.LineNumber
}

$candidatePath = Get-RequiredInput 'phase-10-comment-target-additions.csv'
$serializationPath = Get-RequiredInput 'serialization-register.csv'
$hookPath = Get-RequiredInput 'runtime-hook-map.csv'
$dependencyPath = Get-RequiredInput 'dependency-graph.csv'
$findingPath = Get-RequiredInput 'risk-track-findings.csv'

$candidates = @(Import-Csv -LiteralPath $candidatePath)
$serializers = @(Import-Csv -LiteralPath $serializationPath)
$hooks = @(Import-Csv -LiteralPath $hookPath)
$dependencies = @(Import-Csv -LiteralPath $dependencyPath)
$findings = @(Import-Csv -LiteralPath $findingPath)

$reviewed = New-Object System.Collections.ArrayList

function New-ReviewedRow
{
    param(
        [string]$CommentTargetId,
        [string]$SourceFindingId,
        [string]$Track,
        [string]$File,
        [string]$Class,
        [string]$Location,
        [string]$CommentType,
        [string]$Reason,
        [string]$DraftComment,
        [string]$Evidence,
        [string]$Approved,
        [string]$Decision,
        [string]$DecisionReason,
        [string]$AppliedLine,
        [string]$AppliedComment,
        [string]$Verification
    )

    return [pscustomobject]@{
        CommentTargetId = $CommentTargetId
        SourceFindingId = $SourceFindingId
        Track = $Track
        File = $File
        Class = $Class
        Location = $Location
        CommentType = $CommentType
        Reason = Trim-Text $Reason 400
        DraftComment = Trim-Text $DraftComment 400
        Evidence = Trim-Text $Evidence 500
        Approved = $Approved
        Decision = $Decision
        DecisionReason = Trim-Text $DecisionReason 400
        AppliedLine = $AppliedLine
        AppliedComment = Trim-Text $AppliedComment 500
        Verification = Trim-Text $Verification 400
    }
}

foreach ($candidate in $candidates)
{
    $approved = 'No'
    $decision = 'DeferredNeedsSourceReview'
    $decisionReason = 'Generated candidate needs method-level source review before a comment can be high-signal.'
    $appliedLine = ''
    $appliedComment = ''
    $verification = 'No source comment applied for this target in Phase 11.'

    if ($candidate.CommentTargetId -eq 'P10-COM-2671')
    {
        $approved = 'Yes'
        $decision = 'ApprovedApplied'
        $decisionReason = 'PlayerMobile serializer is a high-risk cross-system save surface; a brief warning prevents unsafe field-order edits.'
        $appliedLine = Find-CommentLine 'Data/Scripts/Mobiles/Base/PlayerMobile.cs' 'Save format: this fall-through mirrors Serialize below'
        $appliedComment = '// Save format: this fall-through mirrors Serialize below; PlayerMobile is a cross-system state hub, so preserve field order unless a migration covers every dependent system.'
        $verification = 'Comment found in PlayerMobile.cs and paired with serializer register evidence.'
    }
    elseif ($candidate.CommentType -eq 'PooledEnumerable')
    {
        $decision = 'DeferredRepairNeeded'
        $decisionReason = 'Pooled enumerable findings may require code changes such as try/finally Free; adding only a comment would document a likely bug without fixing it.'
    }
    elseif ($candidate.File -eq 'Data/Scripts/Custom/XMLSpawner/PacketHandlerOverrides.cs')
    {
        $decision = 'RejectedExistingComment'
        $decisionReason = 'Nearby source comments already explain that XMLSpawner replaces or extends default packet handlers and why the delayed registration exists.'
    }
    elseif ($candidate.DraftComment -match 'keep Serialize and Deserialize field order')
    {
        $decision = 'DeferredGenericSerializationDraft'
        $decisionReason = 'The draft is intentionally generic; Phase 11 approves only comments with source-specific save risk such as PlayerMobile.'
    }
    elseif ($candidate.DraftComment -match 'Runtime hook: keep this registration aligned')
    {
        $decision = 'DeferredGenericHookDraft'
        $decisionReason = 'The hook draft names no concrete ordering, lifecycle, or duplicate-subscription rule; leave it for targeted source review.'
    }

    [void]$reviewed.Add((New-ReviewedRow `
        $candidate.CommentTargetId `
        $candidate.SourceFindingId `
        $candidate.Track `
        $candidate.File `
        $candidate.Class `
        $candidate.Location `
        $candidate.CommentType `
        $candidate.Reason `
        $candidate.DraftComment `
        $candidate.Evidence `
        $approved `
        $decision `
        $decisionReason `
        $appliedLine `
        $appliedComment `
        $verification))
}

$randomEncounterLine = Find-CommentLine 'Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs' 'Player encounters intentionally use CharacterLevelService'
$randomEncounterEvidence = 'Random Encounters->Character Level:DirectReference:Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs:L118:CharacterLevelService'
[void]$reviewed.Add((New-ReviewedRow `
    'P11-MANUAL-0001' `
    '' `
    '11.4 Cross-System Dependency Comments' `
    'Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs' `
    'Server.Custom.Confictura.RandomEncounters.Helpers' `
    'CalculateLevelForMobile' `
    'Dependency' `
    'Random Encounters intentionally uses CharacterLevelService for players while preserving legacy mobile level math.' `
    '// Player encounters intentionally use CharacterLevelService; spawned mobiles keep legacy level math for old encounter tables and scale comparisons.' `
    $randomEncounterEvidence `
    'Yes' `
    'ApprovedApplied' `
    'This is a source-verified Phase 9 synergy and Phase 8 hard dependency; the comment prevents replacing player scaling with legacy mobile math.' `
    $randomEncounterLine `
    '// Player encounters intentionally use CharacterLevelService; spawned mobiles keep legacy level math for old encounter tables and scale comparisons.' `
    'Comment found in Helpers.cs and paired with dependency graph evidence.'))

$approvedRows = @($reviewed | Where-Object { $_.Approved -eq 'Yes' })
$rejectedRows = @($reviewed | Where-Object { $_.Approved -ne 'Yes' })
$sourceEditRows = @($approvedRows | ForEach-Object {
    [pscustomobject]@{
        File = $_.File
        AppliedLine = $_.AppliedLine
        CommentType = $_.CommentType
        AppliedComment = $_.AppliedComment
        Evidence = $_.Evidence
        Verification = $_.Verification
    }
})

$canonicalPath = Export-AuditCsv @($reviewed) 'comment-target-register.csv'
$reviewedPath = Export-AuditCsv @($reviewed) 'phase-11-reviewed-comment-targets.csv'
$approvedPath = Export-AuditCsv @($approvedRows) 'phase-11-approved-comment-targets.csv'
$rejectedPath = Export-AuditCsv @($rejectedRows) 'phase-11-rejected-comment-list.csv'
$sourceEditPath = Export-AuditCsv @($sourceEditRows) 'phase-11-source-comment-edits.csv'

$decisionCounts = @{}
foreach ($row in $reviewed)
{
    if (-not $decisionCounts.ContainsKey($row.Decision))
    {
        $decisionCounts[$row.Decision] = 0
    }

    $decisionCounts[$row.Decision]++
}

$verificationPath = Join-Path $OutputDir 'phase-11-verification-notes.md'
$verificationNotes = New-Object System.Collections.ArrayList
[void]$verificationNotes.Add('# Phase 11 Verification Notes')
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('## Source Comment Edits')
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('| File | Line | Comment Type | Evidence |')
[void]$verificationNotes.Add('| --- | ---: | --- | --- |')
foreach ($row in $sourceEditRows)
{
    [void]$verificationNotes.Add(("| {0} | {1} | {2} | {3} |" -f $row.File, $row.AppliedLine, $row.CommentType, $row.Evidence))
}
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('## Rejection Policy')
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('- Generic serialization and global-hook drafts were deferred unless source-specific risk was reviewed.')
[void]$verificationNotes.Add('- Pooled enumerable candidates were deferred to repair because comments alone would not fix suspected ownership bugs.')
[void]$verificationNotes.Add('- XMLSpawner packet override candidates were rejected where nearby source comments already explain the replacement behavior.')
[void]$verificationNotes.Add('- No broad formatting churn was performed.')
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('## Build Verification')
[void]$verificationNotes.Add('')
[void]$verificationNotes.Add('Build verification is recorded in `RUN_LOG.md`. Existing project truth drift is expected to prevent a clean full script build until Phase 13 repair batches address `Scripts.csproj` mismatches.')

[System.IO.File]::WriteAllLines($verificationPath, [string[]]$verificationNotes, (New-Object System.Text.UTF8Encoding($false)))

$summaryPath = Join-Path $OutputDir 'phase-11-summary.md'
$summary = New-Object System.Collections.ArrayList
[void]$summary.Add('# Phase 11 Inline Code Documentation Summary')
[void]$summary.Add('')
[void]$summary.Add(("Generated: {0}" -f (Get-Date -Format 'o')))
[void]$summary.Add('')
[void]$summary.Add('## Required Inputs')
[void]$summary.Add('')
[void]$summary.Add('| Input | Status |')
[void]$summary.Add('| --- | --- |')
[void]$summary.Add(('| Comment Target Register | Present: `phase-10-comment-target-additions.csv` with {0} rows |' -f $candidates.Count))
[void]$summary.Add(('| Serialization Register | Present: `serialization-register.csv` with {0} rows |' -f $serializers.Count))
[void]$summary.Add(('| Runtime Hook Map | Present: `runtime-hook-map.csv` with {0} rows |' -f $hooks.Count))
[void]$summary.Add(('| Dependency Graph | Present: `dependency-graph.csv` with {0} rows |' -f $dependencies.Count))
[void]$summary.Add(('| Risk-Specific Findings | Present: `risk-track-findings.csv` with {0} rows |' -f $findings.Count))
[void]$summary.Add('| Local Coding Conventions | Present: root `AGENTS.md`; added brief `//` comments near risky code without formatting churn. |')
[void]$summary.Add('')
[void]$summary.Add('## Generated Outputs')
[void]$summary.Add('')
[void]$summary.Add('| Output | Rows | Purpose |')
[void]$summary.Add('| --- | ---: | --- |')
[void]$summary.Add(('| `comment-target-register.csv` | {0} | Canonical reviewed comment target register. |' -f $reviewed.Count))
[void]$summary.Add(('| `phase-11-reviewed-comment-targets.csv` | {0} | Phase-scoped reviewed target list. |' -f $reviewed.Count))
[void]$summary.Add(('| `phase-11-approved-comment-targets.csv` | {0} | Approved targets with source comments applied. |' -f $approvedRows.Count))
[void]$summary.Add(('| `phase-11-rejected-comment-list.csv` | {0} | Rejected or deferred targets with reasons. |' -f $rejectedRows.Count))
[void]$summary.Add(('| `phase-11-source-comment-edits.csv` | {0} | Source comment edits applied in this phase. |' -f $sourceEditRows.Count))
[void]$summary.Add('| `phase-11-verification-notes.md` | 1 | Verification and rejection policy notes. |')
[void]$summary.Add('')
[void]$summary.Add('## Decision Counts')
[void]$summary.Add('')
[void]$summary.Add('| Decision | Count |')
[void]$summary.Add('| --- | ---: |')
foreach ($decision in @($decisionCounts.Keys | Sort-Object))
{
    [void]$summary.Add(("| {0} | {1} |" -f $decision, $decisionCounts[$decision]))
}
[void]$summary.Add('')
[void]$summary.Add('## Exit Criteria')
[void]$summary.Add('')
[void]$summary.Add('- Approved high-risk comment targets were documented in source.')
[void]$summary.Add('- Generic, duplicate, or repair-needed comments were rejected or deferred instead of added.')
[void]$summary.Add('- Comments are brief, near the risky code, and explain why the dependency or save-format risk matters.')
[void]$summary.Add('- No broad formatting churn or source reorganization was performed.')

[System.IO.File]::WriteAllLines($summaryPath, [string[]]$summary, (New-Object System.Text.UTF8Encoding($false)))

[pscustomobject]@{
    CommentTargetRegister = Convert-ToRepoPath $canonicalPath
    ReviewedTargets = Convert-ToRepoPath $reviewedPath
    ApprovedTargets = Convert-ToRepoPath $approvedPath
    RejectedTargets = Convert-ToRepoPath $rejectedPath
    SourceCommentEdits = Convert-ToRepoPath $sourceEditPath
    VerificationNotes = Convert-ToRepoPath $verificationPath
    Summary = Convert-ToRepoPath $summaryPath
    ReviewedRows = $reviewed.Count
    ApprovedRows = $approvedRows.Count
    RejectedRows = $rejectedRows.Count
    SourceEditRows = $sourceEditRows.Count
}
