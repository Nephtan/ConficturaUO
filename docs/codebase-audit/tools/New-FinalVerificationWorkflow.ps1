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

function Invoke-GitCommand
{
    param([string[]]$Arguments)

    Push-Location $RepoRoot
    try
    {
        $output = @(& git @Arguments 2>&1)
        $exitCode = $LASTEXITCODE
    }
    finally
    {
        Pop-Location
    }

    return [pscustomobject]@{
        Command = 'git ' + ($Arguments -join ' ')
        ExitCode = $exitCode
        Output = @($output | ForEach-Object { $_.ToString() })
    }
}

function Get-OutputRowCount
{
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path))
    {
        return ''
    }

    if ([System.IO.Path]::GetExtension($Path) -ieq '.csv')
    {
        return (Import-Csv -LiteralPath $Path | Measure-Object).Count
    }

    return ''
}

function Escape-MarkdownCell
{
    param([string]$Value)

    if ($null -eq $Value)
    {
        return ''
    }

    return (($Value -replace '\|', '\|') -replace "`r?`n", ' ')
}

function Write-AuditMarkdown
{
    param(
        [string]$FileName,
        [string[]]$Lines
    )

    $path = Join-Path $OutputDir $FileName
    Set-Content -LiteralPath $path -Value $Lines -Encoding UTF8
    return $path
}

$phaseStatusPath = Join-Path $RepoRoot 'docs\codebase-audit\PHASE_STATUS.md'
$runLogPath = Join-Path $RepoRoot 'docs\codebase-audit\RUN_LOG.md'
$rootPlanPath = Join-Path $RepoRoot 'CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md'
$phase14PlanPath = Join-Path $RepoRoot 'docs\codebase-audit-phases\phase-14-verification-and-commit-workflow.md'

$requiredInputs = @(
    'project-truth-register.csv',
    'cross-tree-runtime-inventory.csv',
    'system-owner-map.csv',
    'runtime-hook-map.csv',
    'serialization-register.csv',
    'documentation-truth-table.csv',
    'dependency-graph.csv',
    'synergy-conflict-matrix.csv',
    'risk-track-findings.csv',
    'comment-target-register.csv',
    'reorganization-design.csv',
    'repair-backlog.csv',
    'accepted-risk-register.csv',
    'deferred-work-register.csv',
    'verification-matrix.csv'
)

$inputRows = foreach ($fileName in $requiredInputs)
{
    $path = Join-Path $OutputDir $fileName
    [pscustomobject]@{
        Input = $fileName
        Present = if (Test-Path -LiteralPath $path) { 'Yes' } else { 'No' }
        RowCount = Get-OutputRowCount -Path $path
        Purpose = 'Prior phase output required by Phase 14 final verification.'
    }
}

$controlRows = foreach ($path in @($phaseStatusPath, $runLogPath, $rootPlanPath, $phase14PlanPath))
{
    [pscustomobject]@{
        Input = (($path.Substring($RepoRoot.Length).TrimStart('\', '/')) -replace '\\', '/')
        Present = if (Test-Path -LiteralPath $path) { 'Yes' } else { 'No' }
        RowCount = ''
        Purpose = 'Controlling phase-runner state, instructions, or plan evidence.'
    }
}

$allInputRows = @($inputRows) + @($controlRows)
Export-AuditCsv -Rows $allInputRows -FileName 'phase-14-required-inputs.csv' | Out-Null

$phaseLines = @(Get-Content -LiteralPath $phaseStatusPath | Where-Object { $_ -match '^\| Phase [0-9]' })
$phaseRows = foreach ($line in $phaseLines)
{
    $cells = @($line.Trim().Trim('|') -split '\|' | ForEach-Object { $_.Trim() })

    if ($cells.Count -ge 8)
    {
        [pscustomobject]@{
            Phase = $cells[0]
            Status = $cells[1]
            InputsPresent = $cells[2]
            Outputs = $cells[3]
            ExitCriteria = $cells[4]
            Verification = $cells[5]
            Blockers = $cells[6]
            LastCommit = $cells[7]
        }
    }
}

Export-AuditCsv -Rows @($phaseRows) -FileName 'phase-14-phase-status-snapshot.csv' | Out-Null

$statusResult = Invoke-GitCommand -Arguments @('status', '--short')
$headResult = Invoke-GitCommand -Arguments @('rev-parse', '--short', 'HEAD')
$logResult = Invoke-GitCommand -Arguments @('log', '--format=%h%x09%ad%x09%s', '--date=iso-strict', '-n', '60')

$commitRows = foreach ($line in $logResult.Output)
{
    $parts = @($line -split "`t", 3)
    if ($parts.Count -eq 3)
    {
        [pscustomobject]@{
            Commit = $parts[0]
            Date = $parts[1]
            Subject = $parts[2]
        }
    }
}

Export-AuditCsv -Rows @($commitRows) -FileName 'phase-14-commit-history.csv' | Out-Null

$msBuildCandidates = @(
    'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe'
)

$msBuildPath = @($msBuildCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1)
$buildAvailability = if ($msBuildPath.Count -gt 0) { $msBuildPath[0] } else { 'Unavailable on PATH and common Visual Studio 2022 paths were not found.' }

$changeRows = @(
    [pscustomobject]@{
        Batch = 'Phase14FinalVerification'
        ChangeType = 'DocumentationOnly;GeneratedAuditData;AuditMetadata'
        Files = 'docs/codebase-audit/PHASE_STATUS.md;docs/codebase-audit/RUN_LOG.md;docs/codebase-audit/outputs/*phase-14*;docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1'
        VerificationRequired = 'git status --short; docs truth inventory rerun; trailing whitespace scan; git diff --check; staged diff check; focused commit'
        BuildRequired = 'No'
        Reason = 'Phase 14 changes do not touch C# source, project files, serializers, runtime hooks, or file moves.'
    },
    [pscustomobject]@{
        Batch = 'PriorPhase11SourceComments'
        ChangeType = 'SourceCodeCommentOnly'
        Files = 'Data/Scripts/Mobiles/Base/PlayerMobile.cs;Data/Scripts/Custom/RandomEncounters/Helpers.cs'
        VerificationRequired = 'Static comment presence checks; git diff checks; MSBuild attempted and failed on known Phase 2 Scripts.csproj missing compile targets.'
        BuildRequired = 'AttemptedEarlier'
        Reason = 'Only two explanatory comments were added; no behavior, serialization writes/reads, hooks, or project includes changed.'
    }
)

Export-AuditCsv -Rows $changeRows -FileName 'phase-14-change-classification.csv' | Out-Null

$verificationRows = @(
    [pscustomobject]@{
        Command = 'git status --short'
        AppliesTo = 'Git hygiene'
        ExpectedResult = 'Clean before Phase 14 edits; only intended Phase 14 files pending before commit; clean after commit.'
        RequiredBy = 'Phase 14.7'
    },
    [pscustomobject]@{
        Command = '.\docs\codebase-audit\tools\New-DocumentationTruthAudit.ps1'
        AppliesTo = 'Documentation truth inventory'
        ExpectedResult = 'Regenerates Phase 7 documentation truth outputs without source/project changes.'
        RequiredBy = 'Root plan Phase 14 documentation-only workflow'
    },
    [pscustomobject]@{
        Command = 'Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern ''[ \t]+$'''
        AppliesTo = 'Whitespace check'
        ExpectedResult = 'No trailing whitespace in audit files.'
        RequiredBy = 'Documentation-only verification'
    },
    [pscustomobject]@{
        Command = 'git diff --check'
        AppliesTo = 'Whitespace and patch sanity'
        ExpectedResult = 'No whitespace errors; known LF-to-CRLF warnings may appear due checkout settings.'
        RequiredBy = 'Phase 14.2'
    },
    [pscustomobject]@{
        Command = 'git diff --cached --check'
        AppliesTo = 'Staged patch sanity'
        ExpectedResult = 'No whitespace errors in staged Phase 14 batch.'
        RequiredBy = 'Phase 14.7'
    }
)

Export-AuditCsv -Rows $verificationRows -FileName 'phase-14-verification-plan.csv' | Out-Null

$statusLines = @(
    '# Phase 14 Worktree Status',
    '',
    '## Generation-Time Status',
    '',
    "| Field | Value |",
    "| --- | --- |",
    "| Command | ``$($statusResult.Command)`` |",
    "| Exit code | ``$($statusResult.ExitCode)`` |",
    "| HEAD | ``$($headResult.Output -join '; ')`` |",
    '',
    '```text'
) + @(
    if ($statusResult.Output.Count -gt 0)
    {
        $statusResult.Output
    }
    else
    {
        '<clean>'
    }
) + @(
    '```',
    '',
    'Phase 14 generation-time status is expected to change after this script writes final verification outputs. The run log records the pre-edit clean state and the staged/committed final state.'
)

Write-AuditMarkdown -FileName 'phase-14-worktree-status.md' -Lines $statusLines | Out-Null

$inputsMissing = @($allInputRows | Where-Object { $_.Present -ne 'Yes' })
$phaseNotClosed = @($phaseRows | Where-Object { $_.Phase -ne 'Phase 14: Verification And Commit Workflow' -and $_.Status -notin @('Committed', 'Complete', 'Intentional', 'Blocked') })

$openInputIssueLines = if ($inputsMissing.Count -eq 0)
{
    @('No required Phase 14 inputs are missing.')
}
else
{
    @($inputsMissing | ForEach-Object { '- Missing: `{0}`' -f $_.Input })
}

$priorPhaseClosureLines = if ($phaseNotClosed.Count -eq 0)
{
    @('All prior phases are closed as `Committed`, `Complete`, `Intentional`, or `Blocked` in the status snapshot.')
}
else
{
    @($phaseNotClosed | ForEach-Object { '- Not closed: {0} is `{1}`' -f $_.Phase, $_.Status })
}

$notesLines = @(
    '# Phase 14 Verification Notes',
    '',
    '## Required Inputs',
    '',
    '| Input | Present | Row Count |',
    '| --- | --- | --- |'
) + @($allInputRows | ForEach-Object {
    "| $(Escape-MarkdownCell $_.Input) | $($_.Present) | $($_.RowCount) |"
}) + @(
    '',
    '## Required Outputs',
    '',
    '- `phase-14-required-inputs.csv`',
    '- `phase-14-phase-status-snapshot.csv`',
    '- `phase-14-change-classification.csv`',
    '- `phase-14-verification-plan.csv`',
    '- `phase-14-commit-history.csv`',
    '- `phase-14-worktree-status.md`',
    '- `phase-14-verification-notes.md`',
    '- `phase-14-final-status-report.md`',
    '- `phase-14-summary.md`',
    '',
    '## Change Classification',
    '',
    'Phase 14 is documentation-only plus generated audit data and audit metadata. It does not move files, change namespaces, edit project files, edit serializer order, or alter runtime hooks.',
    '',
    '## Build Verification',
    '',
    "MSBuild availability: $buildAvailability.",
    '',
    'A new build is not required for Phase 14 because no C# source or project files are changed in this phase. The last source-touching phase, Phase 11, attempted the maintained solution build with Visual Studio MSBuild and failed on the known Phase 2 `Scripts.csproj` missing compile targets rather than on the comment-only edits.',
    '',
    '## Open Input Issues',
    ''
) + $openInputIssueLines + @(
    '',
    '## Prior Phase Closure',
    ''
) + $priorPhaseClosureLines

Write-AuditMarkdown -FileName 'phase-14-verification-notes.md' -Lines $notesLines | Out-Null

$finalReportLines = @(
    '# Phase 14 Final Status Report',
    '',
    '| Field | Value |',
    '| --- | --- |',
    "| Generation HEAD | ``$($headResult.Output -join '; ')`` |",
    "| Phase 14 content commit | Pending commit at generation time |",
    "| Phase 14 metadata commit | Pending commit at generation time |",
    "| Worktree status at generation | ``$(if ($statusResult.Output.Count -eq 0) { 'Clean' } else { 'See phase-14-worktree-status.md' })`` |",
    "| Build verification | Not rerun for Phase 14 docs-only/generated-data batch; prior source-comment build attempt is recorded as blocked by known Phase 2 project include drift. |",
    '',
    '## Files Changed By Phase 14',
    '',
    '- `docs/codebase-audit/PHASE_STATUS.md`',
    '- `docs/codebase-audit/RUN_LOG.md`',
    '- `docs/codebase-audit/outputs/README.md`',
    '- `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`',
    '- Phase 14 generated outputs under `docs/codebase-audit/outputs/`',
    '',
    '## Verification Performed',
    '',
    'The run log records exact commands and results. The intended final verification set is listed in `phase-14-verification-plan.csv` and summarized in `phase-14-verification-notes.md`.',
    '',
    '## Verification Not Performed',
    '',
    'No new MSBuild run is required for Phase 14 because it is documentation/generated audit data only. The final audit report must not claim build success while the known Phase 2 `Scripts.csproj` missing compile targets remain unresolved.',
    '',
    '## Unrelated Pre-existing Changes',
    '',
    'None were present before Phase 14 edits; `git status --short` was clean at phase start.'
)

Write-AuditMarkdown -FileName 'phase-14-final-status-report.md' -Lines $finalReportLines | Out-Null

$summaryLines = @(
    '# Phase 14 Summary',
    '',
    '## Required Inputs And Outputs',
    '',
    'Phase 14 required backlog items, changed files, applicable instructions, relevant phase outputs, and build tool availability. Required outputs are verification notes, a clean or explained worktree, focused commits, and a final status report.',
    '',
    '## Generated Outputs',
    '',
    '| Output | Purpose |',
    '| --- | --- |',
    '| `phase-14-required-inputs.csv` | Confirms prior phase outputs and control files are present. |',
    '| `phase-14-phase-status-snapshot.csv` | Captures phase closure state before final commit. |',
    '| `phase-14-change-classification.csv` | Classifies Phase 14 risk and verification level. |',
    '| `phase-14-verification-plan.csv` | Lists required final verification commands. |',
    '| `phase-14-commit-history.csv` | Captures recent focused audit commits. |',
    '| `phase-14-worktree-status.md` | Records generation-time git status. |',
    '| `phase-14-verification-notes.md` | Records input, output, build, and closure notes. |',
    '| `phase-14-final-status-report.md` | Closeout report for reviewers. |',
    '',
    '## Exit Criteria',
    '',
    'Phase 14 is ready for completion after the generated outputs are checked, documentation-only verification passes, the staged file set is reviewed, and the focused commit is created.'
)

Write-AuditMarkdown -FileName 'phase-14-summary.md' -Lines $summaryLines | Out-Null

Write-Host "Generated Phase 14 outputs in $OutputDir"
Write-Host "Required inputs present: $(@($allInputRows | Where-Object { $_.Present -eq 'Yes' }).Count)/$($allInputRows.Count)"
Write-Host "Prior phases not closed: $($phaseNotClosed.Count)"
