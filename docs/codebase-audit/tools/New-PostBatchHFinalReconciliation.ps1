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

function Escape-MarkdownCell
{
    param([string]$Value)

    if ($null -eq $Value)
    {
        return ''
    }

    return ($Value -replace '\|', '\|')
}

$Timestamp = (Get-Date).ToString('o')
$ActivePath = Join-Path $OutputDir 'post-audit-active-backlog-status.csv'
$RepairBacklogPath = Join-Path $OutputDir 'repair-backlog.csv'
$ReadmePath = Join-Path $OutputDir 'README.md'
$PhaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$RunLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$CloseoutRel = 'docs/codebase-audit/outputs/post-batch-h-folder-namespace-cleanup-closeout.md'
$CloseoutPath = Join-Path $RepoRoot $CloseoutRel

$CommitByReviewRow = @{
    'PBH-001' = '623c30d0 refactor: move character level scripts'
    'PBH-002' = '0afb8d25 refactor: move ai overhaul workspace'
    'PBH-003' = '16af7967 refactor: move static gump tool workspace'
    'PBH-004' = 'c106986e refactor: move omniai workspace'
    'PBH-005' = '4aae66e9 refactor: move staff toolbar workspace'
    'PBH-006' = '6f4df4cc refactor: move random encounters workspace'
    'PBH-007' = '50163cd6 refactor: move clone offline workspace; a0f741e0 docs: add clone offline h review artifacts'
    'PBH-008' = '64837be8 docs: defer pvp consent move gate'
    'PBH-009' = '87a2b18b refactor: move monster nests workspace'
    'PBH-010' = '0524b971 docs: defer invasion move gate'
    'PBH-011' = 'a027079f docs: record xmlspawner move gate'
    'PBH-012' = '6bc0fc15 docs: record government move gate'
    'PBH-013' = '4272dc55 docs: defer offline skill training move gate'
    'PBH-014' = '90c08523 docs: record homestead move gate'
}

$ActiveRows = Get-CsvRows -Path $ActivePath
$RepairRows = Get-CsvRows -Path $RepairBacklogPath
$HRepairRows = @($RepairRows | Where-Object { $_.Category -eq 'Folder and namespace cleanup' })
$HRows = @($ActiveRows | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-H' })

if ($HRepairRows.Count -ne 14)
{
    throw "Expected 14 folder cleanup backlog rows, found $($HRepairRows.Count)."
}

if ($HRows.Count -ne 14)
{
    throw "Expected 14 active POST-BATCH-H rows, found $($HRows.Count)."
}

foreach ($row in $HRows)
{
    if (-not $CommitByReviewRow.ContainsKey($row.ReviewRowId))
    {
        throw "No commit mapping for $($row.ReviewRowId) / $($row.BacklogId)."
    }

    $row.Commit = $CommitByReviewRow[$row.ReviewRowId]
    $row.UpdatedAt = $Timestamp

    if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $row.ReviewArtifact)))
    {
        throw "Missing review artifact for $($row.ReviewRowId): $($row.ReviewArtifact)"
    }
}

$MissingBacklogRows = @($HRepairRows | Where-Object {
    $id = $_.Id
    -not @($HRows | Where-Object { $_.BacklogId -eq $id }).Count
})

if ($MissingBacklogRows.Count -ne 0)
{
    throw "Unreviewed folder cleanup backlog rows remain: $($MissingBacklogRows.Id -join ', ')"
}

$InvalidStatuses = @($HRows | Where-Object { $_.ActiveStatus -notin @('Fixed', 'DeferredMoveGate', 'NeedsHumanDecision', 'Blocked') })

if ($InvalidStatuses.Count -ne 0)
{
    throw "Unexpected POST-BATCH-H status values: $($InvalidStatuses.ActiveStatus -join ', ')"
}

@($ActiveRows) | Export-Csv -LiteralPath $ActivePath -NoTypeInformation -Encoding UTF8

$HRows = @($ActiveRows | Where-Object { $_.PostAuditBatch -eq 'POST-BATCH-H' } | Sort-Object ReviewRowId)
$StatusSummary = (($HRows | Group-Object ActiveStatus | Sort-Object Name | ForEach-Object { '{0}={1}' -f $_.Name, $_.Count }) -join '; ')
$MoveCount = @($HRows | Where-Object { $_.ActiveStatus -eq 'Fixed' }).Count
$DeferredCount = @($HRows | Where-Object { $_.ActiveStatus -eq 'DeferredMoveGate' }).Count
$HumanCount = @($HRows | Where-Object { $_.ActiveStatus -eq 'NeedsHumanDecision' }).Count
$PendingCommitRows = @($HRows | Where-Object { $_.Commit -like 'Pending current *' })

if ($PendingCommitRows.Count -ne 0)
{
    throw "POST-BATCH-H rows still have pending commit placeholders: $($PendingCommitRows.ReviewRowId -join ', ')"
}

$Table = New-Object System.Collections.Generic.List[string]
$Table.Add('| Review Row | Backlog ID | System | Disposition | Commit | Artifact |') | Out-Null
$Table.Add('| --- | --- | --- | --- | --- | --- |') | Out-Null

foreach ($row in $HRows)
{
    $Table.Add(('| `{0}` | `{1}` | {2} | `{3}` | {4} | `{5}` |' -f $row.ReviewRowId, $row.BacklogId, (Escape-MarkdownCell $row.System), $row.ActiveStatus, (Escape-MarkdownCell $row.Commit), $row.ReviewArtifact)) | Out-Null
}

$Closeout = @(
    '# POST-BATCH-H Folder And Namespace Cleanup Closeout',
    '',
    "Generated: $Timestamp",
    '',
    '## Summary',
    '',
    'POST-BATCH-H processed all 14 `Folder and namespace cleanup` rows from `repair-backlog.csv`. Eight rows were executed as file-location-only moves, three rows were deferred behind move gates, and three rows were recorded as requiring human decisions. No POST-BATCH-I work was started.',
    '',
    'Runtime script compile truth and Visual Studio project truth remain explicitly separate in the per-row artifacts. Source/project/runtime builds were run only for move batches that changed runtime-visible scripts or `Scripts.csproj`; gate-only batches were verified with audit evidence and `git diff --check`.',
    '',
    '## Disposition Summary',
    '',
    ('- Total POST-BATCH-H rows: `{0}`' -f $HRows.Count),
    ('- Status summary: `{0}`' -f $StatusSummary),
    ('- Executed moves: `{0}`' -f $MoveCount),
    ('- Deferred move gates: `{0}`' -f $DeferredCount),
    ('- Human decision gates: `{0}`' -f $HumanCount),
    '',
    '## Row Results',
    ''
) + $Table + @(
    '',
    '## Final Verification',
    '',
    ('- `repair-backlog.csv` folder cleanup rows: `{0}`' -f $HRepairRows.Count),
    ('- `post-audit-active-backlog-status.csv` POST-BATCH-H rows: `{0}`' -f $HRows.Count),
    '- No folder cleanup backlog row is missing from the active overlay.',
    '- No POST-BATCH-H commit field remains as a pending placeholder.',
    "- git diff --check: $DiffCheckResult",
    '',
    '## Next Batch',
    '',
    'POST-BATCH-H is closed. The next logical batch is POST-BATCH-I only after a new goal explicitly starts it; this closeout does not begin that work.'
)

Write-Utf8File -Path $CloseoutPath -Lines $Closeout

$Readme = @(Get-Content -LiteralPath $ReadmePath)
$Readme = @($Readme | Where-Object { $_ -notmatch 'post-batch-h-folder-namespace-cleanup-closeout\.md' })
$NewEntries = @(
    '| `post-batch-h-folder-namespace-cleanup-closeout.md` | Post-audit | Final closeout for POST-BATCH-H, including all row dispositions, commit hash reconciliation, verification, and next-batch boundary. | Complete |'
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
$StatusLine = 'Post-audit reorganization runner: POST-BATCH-H is closed with all 14 folder cleanup rows reviewed and committed ({0}; executed moves={1}; deferred move gates={2}; human decision gates={3}). Commit hashes were reconciled in `post-audit-active-backlog-status.csv`; final closeout is `{4}`. Verification: git diff --check={5}; no POST-BATCH-I work started.' -f $StatusSummary, $MoveCount, $DeferredCount, $HumanCount, $CloseoutRel, $DiffCheckResult

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
    '- Affected phase: Post-audit `POST-BATCH-H` final folder/namespace cleanup reconciliation',
    ('- Cwd: `{0}`' -f $RepoRoot),
    '- Command: verify all folder cleanup backlog rows are represented in the active overlay; reconcile POST-BATCH-H commit hashes; write final closeout.',
    ('- Result: POST-BATCH-H has {0} active overlay rows for {1} repair-backlog folder cleanup rows; status summary is {2}; commit placeholders remaining=0; final verification git diff --check={3}.' -f $HRows.Count, $HRepairRows.Count, $StatusSummary, $DiffCheckResult),
    ('- Output path: `{0}`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`' -f $CloseoutRel)
)

$RunLogText = Get-Content -Raw -LiteralPath $RunLogPath
$RunLogPattern = '(?ms)^### [^\r\n]+\r?\n\r?\n- Affected phase: Post-audit `POST-BATCH-H` final folder/namespace cleanup reconciliation\r?\n.*?(?=^### |\z)'
$RunLogText = [regex]::Replace($RunLogText, $RunLogPattern, '')
[System.IO.File]::WriteAllText($RunLogPath, $RunLogText.TrimEnd() + "`n`n" + ($RunLogEntry -join "`n") + "`n", $Utf8NoBom)

[pscustomobject]@{
    CloseoutPath = $CloseoutPath
    HRows = $HRows.Count
    FolderCleanupBacklogRows = $HRepairRows.Count
    StatusSummary = $StatusSummary
    PendingCommitRows = $PendingCommitRows.Count
}
