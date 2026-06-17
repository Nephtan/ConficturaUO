# Codebase Audit Run Log

Initialized: 2026-06-05T16:15:59.8020730-05:00

Last updated: 2026-06-16T11:22:52.2093583-05:00

Branch: `SAR`

Current HEAD: `7daa0753 docs: add dependency graph audit`

Scope: Deterministic phase runner log for the Confictura codebase audit and reorganization program. No source files, project files, serialized types, or runtime hooks have been changed by the audit batches so far.

## Worktree Baseline

`git status --short` returned no output before Phase 0 edits, so there were no pre-existing user-owned, audit-related, generated, or unknown changes to classify for this batch.

| Path | Status | Classification | Action |
| --- | --- | --- | --- |
| None | Clean | Not applicable | Proceed with Phase 0 audit-state edits only |

## Command Log

### 2026-06-05T16:15:59.8020730-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 0 edits.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:15:59.8020730-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git branch --show-current`
- Result: Current branch is `SAR`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:23:24.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git log --oneline -5`
- Result: Recent commits recorded: `86659bba`, `b52f9c54`, `721c8591`, `bdd893f5`, `5b5936fe`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:15:59.8020730-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Root and nested instruction files located. No nested instruction file applies to `docs/codebase-audit/`, so root `AGENTS.md` governs this batch.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:28:30.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath AGENTS.md`
- Result: Root instructions read from disk; audit phase-runner, save-compatibility, project-file, source-root, documentation, and git rules confirmed.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:15:59.8020730-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `rg --files CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md docs/codebase-audit-phases`
- Result: Found root audit plan and detailed phase files `phase-00` through `phase-14`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:20:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Root audit and reorganization plan read.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:20:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-00-baseline-and-guardrails.md`
- Result: Phase 0 detailed plan read; required inputs, outputs, subphases, and exit criteria recorded in the Phase 0 output.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:20:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-01-reproducible-inventory-scripts.md` through `docs/codebase-audit-phases/phase-14-verification-and-commit-workflow.md`
- Result: Phase plans 1 through 14 read before starting Phase 0 edits, preserving the requested phase order and dependency chain.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:23:24.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `where.exe msbuild`
- Result: No `msbuild` found on `PATH`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'; if (Test-Path -LiteralPath $vswhere) { & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' } else { 'vswhere not found: ' + $vswhere }`
- Result: Visual Studio MSBuild found at `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath ConficturaUO.sln -TotalCount 70`
- Result: Solution includes `Server`, `Scripts`, and `Solution Items`; `Scripts` declares a project dependency on `Server`; solution configurations and project mappings recorded.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath Data/System/Source/Server.csproj -TotalCount 55`
- Result: `Server.csproj` targets .NET Framework v4.8, builds executable `ConficturaServer`, uses x86 platform target, and writes Debug output to the repository root.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath Data/Scripts/Scripts.csproj -TotalCount 45`
- Result: `Scripts.csproj` targets .NET Framework v4.8, builds library `ClassLibrary`, defaults to AnyCPU, and writes Debug output to `Data/Scripts/`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:23:24.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `Select-String -LiteralPath Data/System/Source/Server.csproj,Data/Scripts/Scripts.csproj -Pattern '<TargetFrameworkVersion>', '<PlatformTarget>', '<OutputPath>', '<AssemblyName>', '<ProjectReference'`
- Result: Confirmed `Scripts.csproj` contains a `ProjectReference` to `..\System\Source\Server.csproj`.
- Output path: `docs/codebase-audit/outputs/phase-00-baseline.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --untracked-files=all`
- Result: No output before Phase 0 edits; ignored output file policy checked separately.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:26:20.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git check-ignore -v docs/codebase-audit/outputs/README.md`
- Result: `docs/codebase-audit/outputs/` is ignored by `.git/info/exclude`; phase outputs must be force-staged when they are intended audit artifacts.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:37:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 0 documentation batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:37:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were `docs/codebase-audit/PHASE_STATUS.md`, `docs/codebase-audit/RUN_LOG.md`, `docs/codebase-audit/outputs/README.md`, and `docs/codebase-audit/outputs/phase-00-baseline.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:37:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: complete audit phase 0 baseline"`
- Result: Created commit `87d75e24 docs: complete audit phase 0 baseline`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:38:00.0000000-05:00

- Affected phase: Phase 0
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 0 commit"`
- Result: Created commit `37828cd7 docs: record audit phase 0 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:38:20.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 1 edits.
- Output path: `docs/codebase-audit/outputs/phase-01-summary.md`

### 2026-06-05T16:38:20.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Instruction files rechecked before Phase 1 edits. No deeper instruction file applies to `docs/codebase-audit/tools/` or `docs/codebase-audit/outputs/`.
- Output path: `docs/codebase-audit/outputs/phase-01-summary.md`

### 2026-06-05T16:38:20.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Root audit plan re-read before Phase 1 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:38:20.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-01-reproducible-inventory-scripts.md`
- Result: Phase 1 plan re-read; required inputs, outputs, and exit criteria used for the generator and summary.
- Output path: `docs/codebase-audit/outputs/phase-01-summary.md`

### 2026-06-05T16:38:36.9628575-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\Invoke-CodebaseAuditInventory.ps1`
- Result: Completed successfully. Generated 17 outputs; counted 6,700 audited source files, 6,571 script compile includes, 82 missing compile targets, 92 unincluded script sources, 6,604 runtime marker hits, 5,342 serializer-marker files, and 141 markdown docs.
- Output path: `docs/codebase-audit/outputs/phase-01-summary.md`

### 2026-06-05T16:40:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-01-summary.md`
- Result: Summary reviewed; generated outputs and reproduction command recorded.
- Output path: `docs/codebase-audit/outputs/phase-01-summary.md`

### 2026-06-05T16:40:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-01-missing-compile-targets.csv -TotalCount 8`
- Result: Spot check confirmed missing compile target rows use decoded paths and literal existence results.
- Output path: `docs/codebase-audit/outputs/phase-01-missing-compile-targets.csv`

### 2026-06-05T16:40:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-01-unincluded-source-files.csv -TotalCount 8`
- Result: Spot check confirmed unincluded source rows are real source files under `Data/Scripts`.
- Output path: `docs/codebase-audit/outputs/phase-01-unincluded-source-files.csv`

### 2026-06-05T16:44:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 1 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:44:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 1 generator, Phase 1 generated outputs, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:44:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add reproducible audit inventories"`
- Result: Created commit `78341d10 docs: add reproducible audit inventories`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:45:00.0000000-05:00

- Affected phase: Phase 1
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 1 commit"`
- Result: Created commit `8b11bfc9 docs: record audit phase 1 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:45:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 2 edits.
- Output path: `docs/codebase-audit/outputs/phase-02-summary.md`

### 2026-06-05T16:45:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Instruction files rechecked before Phase 2 edits. No deeper instruction file applies to `docs/codebase-audit/tools/` or `docs/codebase-audit/outputs/`.
- Output path: `docs/codebase-audit/outputs/phase-02-summary.md`

### 2026-06-05T16:45:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-02-build-and-project-truth.md`
- Result: Phase 2 plan re-read; required inputs, outputs, and exit criteria used for project truth output.
- Output path: `docs/codebase-audit/outputs/phase-02-summary.md`

### 2026-06-05T16:45:26.0995929-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`
- Result: Completed successfully. Generated 13,152 project truth rows; classified 82 missing compile targets and 92 unincluded script source files; created 61 project cleanup backlog groups.
- Output path: `docs/codebase-audit/outputs/phase-02-summary.md`

### 2026-06-05T16:46:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal`
- Result: Failed before script compilation because `Server.csproj` has no `Debug|AnyCPU` `OutputPath` when built through the project reference.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-05T16:46:30.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal`
- Result: Failed during `Scripts.csproj` compilation with `CS2001` missing source-file errors, matching the 82 missing compile targets in `phase-02-missing-compile-targets-classified.csv`.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-05T16:47:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`
- Result: Restored tracked server build artifacts generated by the failed solution build.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-05T16:50:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 2 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:50:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 2 project truth generator, Phase 2 generated outputs, canonical project truth outputs, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:50:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add project truth register"`
- Result: Created commit `138d5959 docs: add project truth register`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:51:00.0000000-05:00

- Affected phase: Phase 2
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 2 commit"`
- Result: Created commit `8c47bb6c docs: record audit phase 2 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:52:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 3 edits.
- Output path: `docs/codebase-audit/outputs/phase-03-summary.md`

### 2026-06-05T16:52:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Instruction files rechecked before Phase 3 edits. No deeper instruction file applies to `docs/codebase-audit/tools/` or `docs/codebase-audit/outputs/`.
- Output path: `docs/codebase-audit/outputs/phase-03-summary.md`

### 2026-06-05T16:52:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-03-cross-tree-runtime-inventory.md`
- Result: Phase 3 plan re-read; required inputs, outputs, and exit criteria used for runtime inventory output.
- Output path: `docs/codebase-audit/outputs/phase-03-summary.md`

### 2026-06-05T16:53:39.2955094-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-CrossTreeRuntimeInventory.ps1`
- Result: Completed successfully. Generated 6,700 runtime inventory rows, 8 root summary rows, 147 explicit unknown follow-up rows, and 8 high-risk root summary rows.
- Output path: `docs/codebase-audit/outputs/phase-03-summary.md`

### 2026-06-05T16:54:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-03-summary.md`
- Result: Summary reviewed; root counts and generated output list recorded.
- Output path: `docs/codebase-audit/outputs/phase-03-summary.md`

### 2026-06-05T16:54:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-03-unknown-owner-list.csv -TotalCount 12`
- Result: Unknown follow-up rows spot-checked; unknowns have root, provisional system, evidence, and follow-up.
- Output path: `docs/codebase-audit/outputs/phase-03-unknown-owner-list.csv`

### 2026-06-05T16:56:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 3 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:56:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 3 runtime inventory generator, Phase 3 generated outputs, canonical runtime inventory, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T16:56:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add cross-tree runtime inventory"`
- Result: Created commit `851e0fa1 docs: add cross-tree runtime inventory`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:57:00.0000000-05:00

- Affected phase: Phase 3
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 3 commit"`
- Result: Created commit `56db567f docs: record audit phase 3 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T16:58:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 4 edits.
- Output path: `docs/codebase-audit/outputs/phase-04-summary.md`

### 2026-06-05T16:58:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-04-system-cards.md`
- Result: Phase 4 plan re-read; required inputs, outputs, and exit criteria used for card generation.
- Output path: `docs/codebase-audit/outputs/phase-04-summary.md`

### 2026-06-05T16:59:45.2981517-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-SystemCards.ps1`
- Result: Completed successfully after fixing card-line array construction. Generated 27 system cards, 2,629 owner-map rows, 27 card backlog rows, and 27 priority rows.
- Output path: `docs/codebase-audit/outputs/phase-04-summary.md`

### 2026-06-05T17:00:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-04-summary.md`
- Result: Summary reviewed; verification status counts show 23 `NeedsSerializationReview`, 3 `NeedsRuntimeReview`, and 1 `PartiallyVerified`.
- Output path: `docs/codebase-audit/outputs/phase-04-summary.md`

### 2026-06-05T17:00:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/system-cards/xmlspawner.md -TotalCount 80`
- Result: Sample high-risk card reviewed; card lists classification, summary, source files, runtime hooks, serialized marker presence, and follow-up status.
- Output path: `docs/codebase-audit/outputs/system-cards/xmlspawner.md`

### 2026-06-05T17:02:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 4 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:02:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 4 system card generator, generated cards, owner maps, backlog, priority list, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:02:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add high-risk system cards"`
- Result: Created commit `9d0383eb docs: add high-risk system cards`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:03:00.0000000-05:00

- Affected phase: Phase 4
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 4 commit"`
- Result: Created commit `c81b6440 docs: record audit phase 4 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Resume-time status showed `?? docs/codebase-audit/tools/New-RuntimeHookMap.ps1`; classified as audit-owned Phase 5 work in progress. Phase 5 generated outputs are hidden by the local ignore and will be force-staged with the focused batch.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Rechecked instruction scopes before Phase 5 metadata edits; nested `AGENTS.md` files are under source/gump trees, while Phase 5 edits are limited to `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-05-runtime-hook-map.md`
- Result: Phase 5 plan re-read; required inputs, required outputs, subphase gates, and exit criteria used for hook map generation.
- Output path: `docs/codebase-audit/outputs/phase-05-summary.md`

### 2026-06-05T17:07:19.2703856-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-RuntimeHookMap.ps1`
- Result: Completed successfully. Generated 6,604 hook rows, 2,979 global/high-risk rows, 499 command rows, 17 packet rows, 4,066 gump rows, and 1,043 timer/world-hook rows.
- Output path: `docs/codebase-audit/outputs/phase-05-summary.md`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/runtime-hook-map.csv | Measure-Object).Count`
- Result: Returned `6604`, matching the Phase 5 summary.
- Output path: `docs/codebase-audit/outputs/runtime-hook-map.csv`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-05-packet-handler-register.csv | Measure-Object).Count`
- Result: Returned `17`, matching the Phase 5 summary and treating packet handlers as critical network entry points.
- Output path: `docs/codebase-audit/outputs/phase-05-packet-handler-register.csv`

### 2026-06-05T17:07:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-05-summary.md`
- Result: Summary reviewed; outputs, hook type counts, risk counts, and exit criteria are present. Guard fields are conservative marker scans and `NeedsSourceReview` rows are deferred to manual review phases.
- Output path: `docs/codebase-audit/outputs/phase-05-summary.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RuntimeHookMap.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit status/log/index files, untracked Phase 5 generator, and ignored Phase 5 outputs; no source, project, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RuntimeHookMap.ps1 docs/codebase-audit/outputs/runtime-hook-map.csv docs/codebase-audit/outputs/phase-05-runtime-hook-map.csv docs/codebase-audit/outputs/phase-05-global-hook-risk-list.csv docs/codebase-audit/outputs/phase-05-command-surface-register.csv docs/codebase-audit/outputs/phase-05-packet-handler-register.csv docs/codebase-audit/outputs/phase-05-gump-response-risk-register.csv docs/codebase-audit/outputs/phase-05-timer-world-hook-register.csv docs/codebase-audit/outputs/phase-05-summary.md`
- Result: Staged only the Phase 5 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 5 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:08:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 5 generator, runtime hook map outputs, focused registers, summary, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:10:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add runtime hook map"`
- Result: Created commit `c5bba2fa docs: add runtime hook map`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:10:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 5 content commit and before Phase 5 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:11:00.0000000-05:00

- Affected phase: Phase 5
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 5 commit"`
- Result: Created commit `f7e6e254 docs: record audit phase 5 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:12:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 6 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:12:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Rechecked instruction scopes before Phase 6 edits; Phase 6 edits are limited to `docs/codebase-audit/`, so only root instructions apply.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:12:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Root audit plan re-read; Phase 6 is required before source moves or serializer edits in high-risk systems.
- Output path: `docs/codebase-audit/outputs/phase-06-summary.md`

### 2026-06-05T17:12:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-06-serialization-and-save-compatibility.md`
- Result: Phase 6 plan re-read; required inputs, outputs, subphase gates, and exit criteria used for serializer register generation.
- Output path: `docs/codebase-audit/outputs/phase-06-summary.md`

### 2026-06-05T17:13:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-01-serialization-marker-inventory.csv -TotalCount 5`; `Get-Content -LiteralPath docs/codebase-audit/outputs/cross-tree-runtime-inventory.csv -TotalCount 3`; `Get-Content -LiteralPath docs/codebase-audit/outputs/system-owner-map.csv -TotalCount 3`; `Get-Content -LiteralPath docs/codebase-audit/outputs/project-truth-register.csv -TotalCount 3`
- Result: Prior output schemas inspected before writing the Phase 6 generator.
- Output path: `docs/codebase-audit/tools/New-SerializationRegister.ps1`

### 2026-06-05T17:23:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-SerializationRegister.ps1`
- Result: Initial generator attempts exposed PowerShell generic-list conversion, string-formatting, and `$Matches` collision bugs. The tool was corrected and rerun until outputs generated successfully.
- Output path: `docs/codebase-audit/tools/New-SerializationRegister.ps1`

### 2026-06-05T17:25:38.2804381-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-SerializationRegister.ps1`
- Result: Completed successfully. Generated 9,158 serializer rows, 3,953 high-risk rows, 9,158 move-risk rows, 3,105 comment-target rows, and 1,625 save-compatibility backlog rows.
- Output path: `docs/codebase-audit/outputs/phase-06-summary.md`

### 2026-06-05T17:25:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-06-summary.md`
- Result: Summary reviewed; generated outputs, version handling counts, field alignment counts, move/rename risk counts, and exit criteria are present.
- Output path: `docs/codebase-audit/outputs/phase-06-summary.md`

### 2026-06-05T17:25:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/serialization-register.csv | Where-Object { $_.TypeName -eq 'PlayerMobile' } | Select-Object -First 1 | Format-List File,Class,HasSerialConstructor,CurrentVersion,VersionHandling,MoveRenameRisk,ReviewStatus,ReviewReasons`
- Result: `PlayerMobile` spot-check shows detected Serial constructor, current version `37`, `SwitchGotoCase` handling, `DoNotMove` risk, and `NeedsSourceReview` for manual save-map review.
- Output path: `docs/codebase-audit/outputs/serialization-register.csv`

### 2026-06-05T17:25:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-06-save-compatibility-repair-backlog.csv | Group-Object Category | Sort-Object Name | Format-Table -AutoSize`
- Result: Backlog category counts reviewed: 54 base-call order, 113 missing Serial constructor, 68 missing serializer pair, 27 serialized project-truth, 1,113 version-handling, and 250 write/read alignment items.
- Output path: `docs/codebase-audit/outputs/phase-06-save-compatibility-repair-backlog.csv`

### 2026-06-05T17:26:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:26:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-SerializationRegister.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:26:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/serialization-register.csv | Measure-Object).Count`
- Result: Returned `9158`, matching the Phase 6 summary.
- Output path: `docs/codebase-audit/outputs/serialization-register.csv`

### 2026-06-05T17:26:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-06-save-compatibility-repair-backlog.csv | Measure-Object).Count`
- Result: Returned `1625`, matching the Phase 6 summary.
- Output path: `docs/codebase-audit/outputs/phase-06-save-compatibility-repair-backlog.csv`

### 2026-06-05T17:26:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 6 generator, and ignored Phase 6 outputs; no source, project, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:27:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-SerializationRegister.ps1 docs/codebase-audit/outputs/serialization-register.csv docs/codebase-audit/outputs/phase-06-serialization-register.csv docs/codebase-audit/outputs/phase-06-high-risk-serializer-list.csv docs/codebase-audit/outputs/phase-06-move-rename-risk-list.csv docs/codebase-audit/outputs/phase-06-serializer-comment-target-list.csv docs/codebase-audit/outputs/phase-06-save-compatibility-repair-backlog.csv docs/codebase-audit/outputs/phase-06-summary.md`
- Result: Staged only Phase 6 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:27:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 6 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:27:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 6 serializer generator, serializer registers, high-risk list, move/rename risk list, comment target list, save-compatibility backlog, summary, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:28:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add serialization save compatibility register"`
- Result: Created commit `63be46b2 docs: add serialization save compatibility register`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:28:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 6 content commit and before Phase 6 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:29:00.0000000-05:00

- Affected phase: Phase 6
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 6 commit"`
- Result: Created commit `50d95eb4 docs: record audit phase 6 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:30:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 7 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:30:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Rechecked instruction scopes before Phase 7 edits; Phase 7 edits are limited to `docs/codebase-audit/`, so only root instructions apply.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:30:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Root audit plan re-read; documentation is treated as evidence until source-traced.
- Output path: `docs/codebase-audit/outputs/phase-07-summary.md`

### 2026-06-05T17:30:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-07-documentation-truth-audit.md`
- Result: Phase 7 plan re-read; required inputs, outputs, subphase gates, and exit criteria used for documentation truth generation.
- Output path: `docs/codebase-audit/outputs/phase-07-summary.md`

### 2026-06-05T17:31:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Test-Path -LiteralPath docs/wiki/INDEX.md`; `Test-Path -LiteralPath docs/SystemAudit.md`; `rg -n "backlog|Backlog|Needs Rework|TODO|Source Trace" docs -g "*.md"`
- Result: Wiki index and SystemAudit exist; wiki backlog located at `docs/wiki/wikibacklog.md`; Source Trace appears only in `docs/wiki/PvP_Consent_System.md`.
- Output path: `docs/codebase-audit/outputs/phase-07-summary.md`

### 2026-06-05T17:35:57.1061209-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-DocumentationTruthAudit.ps1`
- Result: Completed successfully after tightening alias classification and source-path extraction. Generated 122 documentation rows, 105 canonical rows, 2 alias rows, 115 backlog rows, and 8 coverage rows.
- Output path: `docs/codebase-audit/outputs/phase-07-summary.md`

### 2026-06-05T17:36:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/documentation-truth-table.csv | Where-Object { $_.DocPath -eq 'docs/wiki/PvP_Consent_System.md' } | Format-List DocPath,CanonicalPage,SourceTracePresent,RuntimeHooksCovered,SerializationCovered,CoverageStatus`
- Result: PvP Consent spot-check is canonical, has source trace, maps runtime hooks and serialized classes from Phase 5/6 registers, and is `SourceTracePresent`.
- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`

### 2026-06-05T17:36:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/documentation-truth-table.csv | Where-Object { $_.DocPath -eq 'docs/wiki/Access_Level_Stone.md' } | Format-List DocPath,VerifiedSourceFiles,MissingSourceFiles,StaleClaims,MissingClaims`
- Result: Access Level Stone spot-check confirms the source-path extractor no longer reports a false partial missing path; page remains backlogged for missing Source Trace.
- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`

### 2026-06-05T17:36:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/documentation-truth-table.csv | Group-Object CoverageStatus | Sort-Object Name | Format-Table -AutoSize`
- Result: Coverage status counts reviewed: 1 source-traced page, 6 existing-backlog pages, 102 pages needing generated backlog, and 13 support docs.
- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`

### 2026-06-05T17:37:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:37:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-DocumentationTruthAudit.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:37:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/documentation-truth-table.csv | Measure-Object).Count`
- Result: Returned `122`, matching the Phase 7 summary.
- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`

### 2026-06-05T17:37:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-07-stale-claim-backlog.csv | Measure-Object).Count`
- Result: Returned `115`, matching the Phase 7 summary.
- Output path: `docs/codebase-audit/outputs/phase-07-stale-claim-backlog.csv`

### 2026-06-05T17:37:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 7 generator, and ignored Phase 7 outputs; no wiki pages, source files, project files, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:38:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-DocumentationTruthAudit.ps1 docs/codebase-audit/outputs/documentation-truth-table.csv docs/codebase-audit/outputs/phase-07-documentation-truth-table.csv docs/codebase-audit/outputs/phase-07-canonical-page-map.csv docs/codebase-audit/outputs/phase-07-alias-legacy-slug-map.csv docs/codebase-audit/outputs/phase-07-stale-claim-backlog.csv docs/codebase-audit/outputs/phase-07-source-trace-coverage-report.csv docs/codebase-audit/outputs/phase-07-summary.md`
- Result: Staged only Phase 7 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:38:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 7 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:38:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 7 documentation truth generator, truth table, canonical map, alias map, stale-claim backlog, source-trace coverage report, summary, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:39:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add documentation truth audit"`
- Result: Created commit `151fe5a9 docs: add documentation truth audit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:39:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 7 content commit and before Phase 7 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:40:00.0000000-05:00

- Affected phase: Phase 7
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 7 commit"`
- Result: Created commit `f3f481ec docs: record audit phase 7 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:41:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 8 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:41:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Rechecked instruction scopes before Phase 8 edits; Phase 8 edits are limited to `docs/codebase-audit/`, so only root instructions apply.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:41:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Root audit plan re-read; dependency graph edges must distinguish source-verified, docs-only, speculative, and conflict relationships.
- Output path: `docs/codebase-audit/outputs/phase-08-summary.md`

### 2026-06-05T17:41:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -Raw -LiteralPath docs/codebase-audit-phases/phase-08-dependency-graph.md`
- Result: Phase 8 plan re-read; required inputs, outputs, subphase gates, and exit criteria used for dependency graph generation.
- Output path: `docs/codebase-audit/outputs/phase-08-summary.md`

### 2026-06-05T17:42:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-01-config-reference-inventory.csv -TotalCount 5`; `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-04-system-card-index.csv -TotalCount 5`; `Get-Content -LiteralPath docs/codebase-audit/outputs/system-owner-map.csv -TotalCount 5`; `Get-Content -LiteralPath docs/codebase-audit/outputs/runtime-hook-map.csv -TotalCount 5`; `Get-Content -LiteralPath docs/codebase-audit/outputs/serialization-register.csv -TotalCount 3`
- Result: Input schemas inspected before writing the Phase 8 generator.
- Output path: `docs/codebase-audit/tools/New-DependencyGraph.ps1`

### 2026-06-05T17:47:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-DependencyGraph.ps1`
- Result: Initial run timed out during direct source-reference scanning. The generator was optimized to precheck file content before line-level marker scans.
- Output path: `docs/codebase-audit/tools/New-DependencyGraph.ps1`

### 2026-06-05T17:51:45.7240107-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-DependencyGraph.ps1`
- Result: Completed successfully. Generated 30,063 dependency edges, 29,933 hard edges, 130 soft/speculative edges, 1,248 conflict/manual-review edges, and 27 standalone proof rows.
- Output path: `docs/codebase-audit/outputs/phase-08-summary.md`

### 2026-06-05T17:52:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/dependency-graph.csv | Where-Object { ($_.SourceSystem -eq 'Random Encounters' -and $_.TargetSystem -eq 'Character Level') -or ($_.SourceSystem -eq 'Character Level' -and $_.TargetSystem -eq 'Random Encounters') }`
- Result: Required graph-area spot-check found `Random Encounters -> Character Level` direct reference edge from `Data/Scripts/Custom/RandomEncounters/Helpers.cs`.
- Output path: `docs/codebase-audit/outputs/dependency-graph.csv`

### 2026-06-05T17:52:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-08-standalone-proof-list.csv`
- Result: Standalone proof list reviewed; all 27 system-card systems have generated edge evidence and are marked `NotStandalone`.
- Output path: `docs/codebase-audit/outputs/phase-08-standalone-proof-list.csv`

### 2026-06-05T17:53:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-08-summary.md -Tail 12`
- Result: Summary reviewed; docs-only/speculative edges are separated from hard edges and no system-card system lacks generated incoming/outgoing evidence.
- Output path: `docs/codebase-audit/outputs/phase-08-summary.md`

### 2026-06-05T17:54:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:54:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-DependencyGraph.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:54:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/dependency-graph.csv | Measure-Object).Count`
- Result: Returned `30063`, matching the Phase 8 summary.
- Output path: `docs/codebase-audit/outputs/dependency-graph.csv`

### 2026-06-05T17:54:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-08-standalone-proof-list.csv | Measure-Object).Count`
- Result: Returned `27`, matching the Phase 8 summary.
- Output path: `docs/codebase-audit/outputs/phase-08-standalone-proof-list.csv`

### 2026-06-05T17:54:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 8 generator, and ignored Phase 8 outputs; no source, project, wiki page, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:55:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-DependencyGraph.ps1 docs/codebase-audit/outputs/dependency-graph.csv docs/codebase-audit/outputs/phase-08-dependency-graph.csv docs/codebase-audit/outputs/phase-08-hard-dependency-list.csv docs/codebase-audit/outputs/phase-08-soft-dependency-list.csv docs/codebase-audit/outputs/phase-08-conflict-edge-list.csv docs/codebase-audit/outputs/phase-08-standalone-proof-list.csv docs/codebase-audit/outputs/phase-08-summary.md`
- Result: Staged only Phase 8 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:55:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 8 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:55:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 8 dependency graph generator, canonical graph, phase graph, hard/soft/conflict/standalone outputs, summary, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T17:56:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add dependency graph audit"`
- Result: Created commit `7daa0753 docs: add dependency graph audit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T17:56:00.0000000-05:00

- Affected phase: Phase 8
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 8 content commit and before Phase 8 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:01:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 9 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:01:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `8e716592`, the Phase 8 metadata commit.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:01:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Re-read instruction scopes before Phase 9 edits; only root instructions apply under `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:02:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-09-synergy-and-conflict-matrix.md`
- Result: Confirmed required inputs, outputs, labels, domain buckets, pairwise review rules, balance review, player-objective review, checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-09-summary.md`

### 2026-06-05T18:07:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-SynergyConflictMatrix.ps1`
- Result: Generated 351 matrix rows, 26 balance-risk rows, 158 documentation-risk rows, 32 staff-dependency rows, 22 preservation rows, 27 domain rows, and 27 player-objective rows.
- Output path: `docs/codebase-audit/outputs/phase-09-summary.md`

### 2026-06-05T18:07:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv | Where-Object { ($_.SystemA -eq 'Character Level' -and $_.SystemB -eq 'Random Encounters') -or ($_.SystemA -eq 'Random Encounters' -and $_.SystemB -eq 'Character Level') } | Format-List`
- Result: Spot check returned labels `DependsOn;DocRisk;Supports`, hard `DirectReference` evidence from Random Encounters to Character Level, and preservation recommendation.
- Output path: `docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv`

### 2026-06-05T18:07:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv | Where-Object { ($_.SystemA -eq 'PvP Consent' -and $_.SystemB -eq 'Government') -or ($_.SystemA -eq 'Government' -and $_.SystemB -eq 'PvP Consent') } | Format-List`
- Result: Spot check returned labels `BalanceRisk;Conflicts;DocRisk;Overrides` and the consent-versus-government policy recommendation.
- Output path: `docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv`

### 2026-06-05T18:07:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv | Where-Object { ($_.SystemA -eq 'Spell Framework' -and $_.SystemB -eq 'Magic Schools') -or ($_.SystemA -eq 'Magic Schools' -and $_.SystemB -eq 'Spell Framework') } | Format-List`
- Result: Spot check returned labels `BalanceRisk;DependsOn;DocRisk;Supports` and hard direct-reference evidence from magic school files to spell framework symbols.
- Output path: `docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-SynergyConflictMatrix.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/synergy-conflict-matrix.csv | Measure-Object).Count`
- Result: Returned `351`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/synergy-conflict-matrix.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-balance-risk-list.csv | Measure-Object).Count`
- Result: Returned `26`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/phase-09-balance-risk-list.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-documentation-risk-list.csv | Measure-Object).Count`
- Result: Returned `158`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/phase-09-documentation-risk-list.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-staff-dependency-list.csv | Measure-Object).Count`
- Result: Returned `32`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/phase-09-staff-dependency-list.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-preservation-notes.csv | Measure-Object).Count`
- Result: Returned `22`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/phase-09-preservation-notes.csv`

### 2026-06-05T18:08:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-09-player-objective-review.csv | Measure-Object).Count`
- Result: Returned `27`, matching the Phase 9 summary.
- Output path: `docs/codebase-audit/outputs/phase-09-player-objective-review.csv`

### 2026-06-05T18:09:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 9 generator, and ignored Phase 9 outputs; no source, project, wiki page, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:09:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-SynergyConflictMatrix.ps1 docs/codebase-audit/outputs/synergy-conflict-matrix.csv docs/codebase-audit/outputs/phase-09-synergy-conflict-matrix.csv docs/codebase-audit/outputs/phase-09-domain-buckets.csv docs/codebase-audit/outputs/phase-09-balance-risk-list.csv docs/codebase-audit/outputs/phase-09-documentation-risk-list.csv docs/codebase-audit/outputs/phase-09-staff-dependency-list.csv docs/codebase-audit/outputs/phase-09-preservation-notes.csv docs/codebase-audit/outputs/phase-09-player-objective-review.csv docs/codebase-audit/outputs/phase-09-summary.md`
- Result: Staged only Phase 9 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:09:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 9 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:09:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 9 synergy/conflict generator, canonical matrix, phase matrix, domain, balance, documentation, staff, preservation, player-objective, summary outputs, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:10:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add synergy conflict matrix"`
- Result: Created commit `c24f0037 docs: add synergy conflict matrix`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:10:00.0000000-05:00

- Affected phase: Phase 9
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 9 content commit and before Phase 9 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:11:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 10 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:11:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `143b1305`, the Phase 9 metadata commit.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:11:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Re-read instruction scopes before Phase 10 edits; only root instructions apply under `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:11:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-10-risk-specific-code-review-tracks.md`
- Result: Confirmed required inputs, outputs, finding template, 14 review tracks, review checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-10-summary.md`

### 2026-06-05T18:18:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-RiskSpecificReviewTracks.ps1`
- Result: Generated 6,801 findings, 14 non-issue records, 6,801 repair follow-ups, 3 audit-stage accepted-risk notes, 3,738 candidate comment targets, 652 pooled-enumerable source-scan rows, and 15 coverage rows.
- Output path: `docs/codebase-audit/outputs/phase-10-summary.md`

### 2026-06-05T18:18:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-track-coverage.csv | Format-Table -AutoSize`
- Result: Spot check showed every named Phase 10 track has `Complete` coverage; the overall accepted-risk row records audit-stage caveats.
- Output path: `docs/codebase-audit/outputs/phase-10-track-coverage.csv`

### 2026-06-05T18:18:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/risk-track-findings.csv | Group-Object Severity | Sort-Object Name | Select-Object Name,Count | Format-Table -AutoSize`
- Result: Severity counts were P0=375, P1=4,699, P2=1,429, P3=298.
- Output path: `docs/codebase-audit/outputs/risk-track-findings.csv`

### 2026-06-05T18:18:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/risk-track-findings.csv | Group-Object Track | Sort-Object Name | Select-Object Name,Count | Format-Table -AutoSize`
- Result: Track counts matched the Phase 10 summary, including build drift 61, serialization 1,625, global hooks 1,548, packets 17, gumps 938, commands 499, pooled enumerables 408, regions/maps 112, PlayerMobile coupling 394, economy 26, staff tooling 124, legacy 659, XML/config 232, and docs contradictions 158.
- Output path: `docs/codebase-audit/outputs/risk-track-findings.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RiskSpecificReviewTracks.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/risk-track-findings.csv | Measure-Object).Count`
- Result: Returned `6801`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/risk-track-findings.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-repair-backlog-items.csv | Measure-Object).Count`
- Result: Returned `6801`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/phase-10-repair-backlog-items.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-non-issue-records.csv | Measure-Object).Count`
- Result: Returned `14`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/phase-10-non-issue-records.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-accepted-risk-notes.csv | Measure-Object).Count`
- Result: Returned `3`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/phase-10-accepted-risk-notes.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-comment-target-additions.csv | Measure-Object).Count`
- Result: Returned `3738`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/phase-10-comment-target-additions.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-pooled-enumerable-review.csv | Measure-Object).Count`
- Result: Returned `652`, matching the Phase 10 summary.
- Output path: `docs/codebase-audit/outputs/phase-10-pooled-enumerable-review.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-10-track-coverage.csv | Where-Object { $_.Status -ne 'Complete' } | Measure-Object).Count`
- Result: Returned `0`; all Phase 10 coverage rows are complete.
- Output path: `docs/codebase-audit/outputs/phase-10-track-coverage.csv`

### 2026-06-05T18:19:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 10 generator, and ignored Phase 10 outputs; no source, project, wiki page, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:20:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RiskSpecificReviewTracks.ps1 docs/codebase-audit/outputs/risk-track-findings.csv docs/codebase-audit/outputs/phase-10-risk-track-findings.csv docs/codebase-audit/outputs/phase-10-non-issue-records.csv docs/codebase-audit/outputs/phase-10-repair-backlog-items.csv docs/codebase-audit/outputs/phase-10-accepted-risk-notes.csv docs/codebase-audit/outputs/phase-10-comment-target-additions.csv docs/codebase-audit/outputs/phase-10-pooled-enumerable-review.csv docs/codebase-audit/outputs/phase-10-track-coverage.csv docs/codebase-audit/outputs/phase-10-summary.md`
- Result: Staged only Phase 10 audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:20:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 10 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:20:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 10 risk-track generator, canonical findings, phase findings, non-issues, repair backlog items, accepted-risk notes, comment target additions, pooled-enumerable review, track coverage, summary, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:21:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add risk review tracks"`
- Result: Created commit `f87abe4d docs: add risk review tracks`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:21:00.0000000-05:00

- Affected phase: Phase 10
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 10 content commit and before Phase 10 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:22:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 11 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:22:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `cfdac941`, the Phase 10 metadata commit.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:22:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Re-read instruction scopes before Phase 11 edits; root instructions apply to the edited source files, with no more-specific `AGENTS.md` in their directories.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:22:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-11-inline-code-documentation.md`
- Result: Confirmed required inputs, outputs, comment target table, seven subphases, review checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-11-summary.md`

### 2026-06-05T18:24:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `apply_patch`
- Result: Added two source comments: one warning that `PlayerMobile.Deserialize` version fall-through mirrors `Serialize`, and one explaining why Random Encounters uses `CharacterLevelService` for players while preserving legacy mobile level math.
- Output path: `Data/Scripts/Mobiles/Base/PlayerMobile.cs`; `Data/Scripts/Custom/RandomEncounters/Helpers.cs`

### 2026-06-05T18:26:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-CommentTargetRegister.ps1`
- Result: Generated 3,739 reviewed comment targets, 2 approved/applied targets, 3,737 rejected or deferred targets, and 2 source comment edit records.
- Output path: `docs/codebase-audit/outputs/phase-11-summary.md`

### 2026-06-05T18:26:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-11-approved-comment-targets.csv | Format-List`
- Result: Spot check showed approved applied targets for `Data/Scripts/Mobiles/Base/PlayerMobile.cs` line 4544 and `Data/Scripts/Custom/RandomEncounters/Helpers.cs` line 117.
- Output path: `docs/codebase-audit/outputs/phase-11-approved-comment-targets.csv`

### 2026-06-05T18:26:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/comment-target-register.csv | Group-Object Decision | Sort-Object Count -Descending | Select-Object Count,Name | Format-Table -AutoSize`
- Result: Decision counts were `ApprovedApplied=2`, `DeferredGenericSerializationDraft=3104`, `DeferredRepairNeeded=408`, `DeferredGenericHookDraft=206`, `DeferredNeedsSourceReview=16`, and `RejectedExistingComment=3`.
- Output path: `docs/codebase-audit/outputs/comment-target-register.csv`

### 2026-06-05T18:27:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:27:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- Data/Scripts/Mobiles/Base/PlayerMobile.cs Data/Scripts/Custom/RandomEncounters/Helpers.cs docs/codebase-audit/tools/New-CommentTargetRegister.ps1 docs/codebase-audit/outputs/README.md docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched source files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:27:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/comment-target-register.csv | Measure-Object).Count`
- Result: Returned `3739`, matching the Phase 11 summary.
- Output path: `docs/codebase-audit/outputs/comment-target-register.csv`

### 2026-06-05T18:27:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-11-approved-comment-targets.csv | Measure-Object).Count`
- Result: Returned `2`, matching the Phase 11 summary.
- Output path: `docs/codebase-audit/outputs/phase-11-approved-comment-targets.csv`

### 2026-06-05T18:27:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-11-source-comment-edits.csv | Measure-Object).Count`
- Result: Returned `2`, matching the Phase 11 summary.
- Output path: `docs/codebase-audit/outputs/phase-11-source-comment-edits.csv`

### 2026-06-05T18:28:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal`
- Result: Failed because `msbuild` is not on PATH in this shell.
- Output path: `docs/codebase-audit/outputs/phase-11-verification-notes.md`

### 2026-06-05T18:28:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal`
- Result: Failed with the known Phase 2 `Scripts.csproj` missing compile target errors after building `ConficturaServer.exe`; this does not prove the comment-only source edits compile until project truth drift is repaired.
- Output path: `docs/codebase-audit/outputs/phase-11-verification-notes.md`

### 2026-06-05T18:28:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`
- Result: Restored generated build artifacts touched by the MSBuild verification attempt.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:29:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Remaining changes were the two approved source-comment edits, Phase 11 audit metadata, generator, and generated outputs; generated build artifacts were restored.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:29:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- Data/Scripts/Mobiles/Base/PlayerMobile.cs Data/Scripts/Custom/RandomEncounters/Helpers.cs docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-CommentTargetRegister.ps1 docs/codebase-audit/outputs/comment-target-register.csv docs/codebase-audit/outputs/phase-11-reviewed-comment-targets.csv docs/codebase-audit/outputs/phase-11-approved-comment-targets.csv docs/codebase-audit/outputs/phase-11-rejected-comment-list.csv docs/codebase-audit/outputs/phase-11-source-comment-edits.csv docs/codebase-audit/outputs/phase-11-verification-notes.md docs/codebase-audit/outputs/phase-11-summary.md`
- Result: Staged only Phase 11 source comments, audit metadata, generator, and generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:29:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 11 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:29:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the two approved source-comment edits, Phase 11 comment-target generator, canonical comment target register, reviewed/approved/rejected/source-edit outputs, summary, verification notes, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:30:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: apply inline audit comments"`
- Result: Created commit `7d9ad0c2 docs: apply inline audit comments`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:30:00.0000000-05:00

- Affected phase: Phase 11
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 11 content commit and before Phase 11 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:31:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 12 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:31:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `aa14f132`, the Phase 11 metadata commit.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:31:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Re-read instruction scopes before Phase 12 edits; only root instructions apply under `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:31:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-12-reorganization-design.md`
- Result: Confirmed required inputs, outputs, organization principles, target layout, existing root preservation, move proposal table fields, third-party containment, namespace plan, documentation move plan, checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-12-summary.md`

### 2026-06-05T18:34:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-ReorganizationDesign.ps1`
- Result: Generated 16 target layout rows, 14 design-only move proposals, 9 keep-in-place decisions, third-party containment, save-compatibility, project-update, namespace, documentation move, design, and summary outputs.
- Output path: `docs/codebase-audit/outputs/phase-12-summary.md`

### 2026-06-05T18:34:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-move-proposal-table.csv | Where-Object { $_.System -in @('XMLSpawner','Homestead','Government','PlayerMobile Core','Random Encounters','PvP Consent') } | Format-List`
- Result: Spot checks showed high-risk move proposals are `DesignOnlyNotExecuted`, forbid namespace/type rename without migration, and include project/docs/verification/rollback plans.
- Output path: `docs/codebase-audit/outputs/phase-12-move-proposal-table.csv`

### 2026-06-05T18:35:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-move-proposal-table.csv | ForEach-Object { $path = $_.CurrentPath -replace '/', '\\'; [pscustomobject]@{ System=$_.System; CurrentPath=$_.CurrentPath; Exists=(Test-Path -LiteralPath $path) } } | Format-Table -AutoSize`
- Result: All 14 move proposal current paths exist after correcting `Monster Nests` to `Data/Scripts/Custom/MonsterNest`.
- Output path: `docs/codebase-audit/outputs/phase-12-move-proposal-table.csv`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-ReorganizationDesign.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-move-proposal-table.csv | Measure-Object).Count`
- Result: Returned `14`, matching the Phase 12 summary.
- Output path: `docs/codebase-audit/outputs/phase-12-move-proposal-table.csv`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-target-layout-proposal.csv | Measure-Object).Count`
- Result: Returned `16`, matching the Phase 12 summary.
- Output path: `docs/codebase-audit/outputs/phase-12-target-layout-proposal.csv`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-keep-in-place-decisions.csv | Measure-Object).Count`
- Result: Returned `9`, matching the Phase 12 summary.
- Output path: `docs/codebase-audit/outputs/phase-12-keep-in-place-decisions.csv`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-12-move-proposal-table.csv | Where-Object { $_.Phase12Status -ne 'DesignOnlyNotExecuted' } | Measure-Object).Count`
- Result: Returned `0`; no Phase 12 move proposal was executed.
- Output path: `docs/codebase-audit/outputs/phase-12-move-proposal-table.csv`

### 2026-06-05T18:36:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 12 generator, and ignored Phase 12 outputs; no source, project, or serialized files were moved.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:37:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-ReorganizationDesign.ps1 docs/codebase-audit/outputs/reorganization-design.csv docs/codebase-audit/outputs/reorganization-design.md docs/codebase-audit/outputs/phase-12-target-layout-proposal.csv docs/codebase-audit/outputs/phase-12-move-proposal-table.csv docs/codebase-audit/outputs/phase-12-keep-in-place-decisions.csv docs/codebase-audit/outputs/phase-12-third-party-containment-plan.csv docs/codebase-audit/outputs/phase-12-save-compatibility-notes.csv docs/codebase-audit/outputs/phase-12-project-update-plan.csv docs/codebase-audit/outputs/phase-12-namespace-plan.csv docs/codebase-audit/outputs/phase-12-documentation-move-plan.csv docs/codebase-audit/outputs/phase-12-summary.md`
- Result: Staged only Phase 12 audit metadata, generator, and generated design outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:37:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: No output; staged Phase 12 batch passed whitespace verification.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:37:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Staged files were the Phase 12 design generator, canonical design outputs, layout, move, keep, third-party, save, project, namespace, documentation, and summary outputs, `PHASE_STATUS.md`, `RUN_LOG.md`, and `outputs/README.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:38:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: design reorganization plan"`
- Result: Created commit `6fc58b06 docs: design reorganization plan`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:38:00.0000000-05:00

- Affected phase: Phase 12
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean after Phase 12 content commit and before Phase 12 metadata update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:39:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: No output; worktree clean before Phase 13 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:39:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `f181e068`, the Phase 12 metadata commit.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:39:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Re-read instruction scopes before Phase 13 edits; only root instructions apply under `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:39:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-13-repair-backlog.md`
- Result: Confirmed required inputs, outputs, backlog item template, priority rules, category buckets, ready/blocked criteria, batch planning, verification matrix, checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-13-summary.md`

### 2026-06-05T18:41:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-RepairBacklog.ps1`
- Result: Generated 6,815 backlog rows, 3 accepted-risk rows, 21 deferred-work rows, 7 batch-plan rows, 10 verification-matrix rows, and Phase 13 summary.
- Output path: `docs/codebase-audit/outputs/phase-13-summary.md`

### 2026-06-05T18:41:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/repair-backlog.csv | Group-Object Priority | Sort-Object Name | Select-Object Name,Count | Format-Table -AutoSize`
- Result: Priority counts were P0=375, P1=4,699, P2=1,429, P3=298, P4=14.
- Output path: `docs/codebase-audit/outputs/repair-backlog.csv`

### 2026-06-05T18:41:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/repair-backlog.csv | Group-Object Category | Sort-Object Count -Descending | Select-Object Name,Count -First 20 | Format-Table -AutoSize`
- Result: Category counts matched the Phase 13 summary, with largest buckets Save compatibility=1,625, Runtime hooks=1,548, Gump guards=938, Legacy compatibility=659, and Command access=499.
- Output path: `docs/codebase-audit/outputs/repair-backlog.csv`

### 2026-06-05T18:41:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/accepted-risk-register.csv | Format-List`
- Result: Accepted-risk register contains 3 audit-stage-only risks carried from Phase 10, each with rationale and review trigger.
- Output path: `docs/codebase-audit/outputs/accepted-risk-register.csv`

### 2026-06-05T18:41:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/verification-matrix.csv | Format-Table -AutoSize`
- Result: Verification matrix covers docs, project includes, save compatibility, runtime hooks, packet handlers, gumps, pooled enumerables, reorganization, inline comments, and economy/reward loops.
- Output path: `docs/codebase-audit/outputs/verification-matrix.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No output; no trailing whitespace matches found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RepairBacklog.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/repair-backlog.csv | Measure-Object).Count`
- Result: Returned `6815`, matching the Phase 13 summary.
- Output path: `docs/codebase-audit/outputs/repair-backlog.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/risk-track-findings.csv | Measure-Object).Count`
- Result: Returned `6801`; Phase 13 backlog includes every Phase 10 finding plus 14 Phase 12 organization deferrals.
- Output path: `docs/codebase-audit/outputs/risk-track-findings.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/repair-backlog.csv | Where-Object { [string]::IsNullOrWhiteSpace($_.Evidence) -or [string]::IsNullOrWhiteSpace($_.RecommendedFix) -or [string]::IsNullOrWhiteSpace($_.Verification) } | Measure-Object).Count`
- Result: Returned `0`; every backlog row has evidence, recommendation, and verification fields.
- Output path: `docs/codebase-audit/outputs/repair-backlog.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/accepted-risk-register.csv | Measure-Object).Count`
- Result: Returned `3`, matching the Phase 13 summary.
- Output path: `docs/codebase-audit/outputs/accepted-risk-register.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/deferred-work-register.csv | Measure-Object).Count`
- Result: Returned `21`, matching the Phase 13 summary.
- Output path: `docs/codebase-audit/outputs/deferred-work-register.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/verification-matrix.csv | Measure-Object).Count`
- Result: Returned `10`, matching the Phase 13 summary.
- Output path: `docs/codebase-audit/outputs/verification-matrix.csv`

### 2026-06-05T18:42:00.0000000-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed modified audit metadata/index files, untracked Phase 13 generator, and ignored Phase 13 outputs; no source, project, or serialized files changed.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:45:47.2460327-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No trailing whitespace found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:45:47.2460327-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RepairBacklog.ps1`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:45:47.2460327-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Confirmed only Phase 13 audit metadata, generator, and ignored generated outputs were pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:45:47.2460327-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/RUN_LOG.md -Tail 60`
- Result: Reviewed recent Phase 13 run-log entries before staging.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:46:23.3562711-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/tools/New-RepairBacklog.ps1 docs/codebase-audit/outputs/repair-backlog.csv docs/codebase-audit/outputs/phase-13-repair-backlog.csv docs/codebase-audit/outputs/accepted-risk-register.csv docs/codebase-audit/outputs/phase-13-accepted-risk-register.csv docs/codebase-audit/outputs/deferred-work-register.csv docs/codebase-audit/outputs/phase-13-deferred-work-register.csv docs/codebase-audit/outputs/phase-13-batch-plan.csv docs/codebase-audit/outputs/verification-matrix.csv docs/codebase-audit/outputs/phase-13-verification-matrix.csv docs/codebase-audit/outputs/phase-13-summary.md`
- Result: Staged Phase 13 metadata, generator, and ignored generated outputs; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:46:23.3562711-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:46:23.3562711-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged files are limited to Phase 13 audit metadata, outputs, and `New-RepairBacklog.ps1`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:46:23.3562711-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only the staged Phase 13 batch was pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors before commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed final staged files were limited to Phase 13 audit metadata, outputs, and `New-RepairBacklog.ps1`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only the staged Phase 13 content batch was pending before commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add repair backlog"`
- Result: Created commit `5c65b476 docs: add repair backlog`.
- Output path: `docs/codebase-audit/outputs/phase-13-summary.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean after the Phase 13 content commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `5c65b476`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git add -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md`
- Result: Staged Phase 13 metadata record updates; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed metadata staged files were limited to `PHASE_STATUS.md` and `RUN_LOG.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only Phase 13 metadata files were staged.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:47:05.4144799-05:00

- Affected phase: Phase 13
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 13 commit"`
- Result: Created commit `f13d86de docs: record audit phase 13 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean before Phase 14 edits.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Rechecked instruction scopes; root `AGENTS.md` applies to Phase 14 audit docs and generated outputs.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- Result: Re-read the root audit plan and Phase 14 documentation-only verification workflow.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit-phases/phase-14-verification-and-commit-workflow.md`
- Result: Re-read detailed Phase 14 required inputs, required outputs, subphases, checklist, and exit criteria.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/tools/New-RepairBacklog.ps1 -TotalCount 80`
- Result: Reviewed existing audit generator style before adding the Phase 14 generator.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit/tools -File | Select-Object -ExpandProperty Name`
- Result: Confirmed existing audit tool names before adding `New-FinalVerificationWorkflow.ps1`.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/README.md -Tail 80`
- Result: Reviewed output index before adding Phase 14 rows.
- Output path: `docs/codebase-audit/outputs/README.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/tools/New-DocumentationTruthAudit.ps1 -TotalCount 120`
- Result: Reviewed docs truth generator before using it as Phase 14 verification.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Select-String -LiteralPath docs/codebase-audit/tools/New-DocumentationTruthAudit.ps1 -Pattern 'Get-ChildItem|\.md' -Context 0,2`
- Result: Confirmed docs truth generation scans docs markdown while excluding `docs/codebase-audit/`.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-FinalVerificationWorkflow.ps1`
- Result: Initial run failed with a PowerShell parser error in markdown array construction; generator was corrected before rerun.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `$p='docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1'; $i=0; Get-Content -LiteralPath $p | ForEach-Object { $i++; if ($i -ge 300 -and $i -le 340) { '{0}:{1}: {2}' -f $p,$i,$_ } }`
- Result: Inspected the parser-error region and confirmed the inline `if` array expression needed to be split into variables.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-FinalVerificationWorkflow.ps1`
- Result: Second run failed on an interpolation edge case in a generated markdown string; generator was corrected with `-f` formatting.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `$p='docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1'; $i=0; Get-Content -LiteralPath $p | ForEach-Object { $i++; if ($i -ge 286 -and $i -le 306) { '{0}:{1}: {2}' -f $p,$i,$_ } }`
- Result: Inspected the interpolation-error region and confirmed the dynamic markdown strings needed safe formatting.
- Output path: `docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-FinalVerificationWorkflow.ps1`
- Result: Generator completed; all 19 required inputs were present, but a snapshot parser bug counted the markdown table header as an unclosed phase.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-14-phase-status-snapshot.csv | Where-Object { $_.Status -notin @('Committed','Complete','Intentional','Blocked') } | Format-Table Phase,Status,LastCommit -AutoSize`
- Result: Showed the snapshot parser included the markdown header row and Phase 14 `NotStarted` state at generation time.
- Output path: `docs/codebase-audit/outputs/phase-14-phase-status-snapshot.csv`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-14-verification-notes.md -Tail 30`
- Result: Confirmed the generated notes showed the parser artifact and an over-escaped MSBuild availability variable.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed only the Phase 14 generator and ignored Phase 14 generated outputs pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-FinalVerificationWorkflow.ps1`
- Result: Generator completed after parser fixes; all 19 required inputs were present and prior phases not closed returned `0`.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-DocumentationTruthAudit.ps1`
- Result: Documentation truth inventory rerun completed with 122 documentation rows, 105 canonical rows, 2 alias rows, 115 backlog rows, and 8 coverage rows.
- Output path: `docs/codebase-audit/outputs/phase-07-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed the Phase 14 generator, ignored Phase 14 outputs, and one modified generated Phase 7 summary from the docs truth rerun.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --stat -- docs/codebase-audit/outputs/documentation-truth-table.csv docs/codebase-audit/outputs/phase-07-documentation-truth-table.csv docs/codebase-audit/outputs/phase-07-summary.md`
- Result: Only `phase-07-summary.md` changed, with one insertion and one deletion.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-14-summary.md`
- Result: Reviewed generated Phase 14 summary and required output list.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:52:46.4583396-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff -- docs/codebase-audit/outputs/phase-07-summary.md`
- Result: Confirmed docs truth rerun changed only the generated timestamp in `phase-07-summary.md`.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-FinalVerificationWorkflow.ps1`
- Result: Regenerated Phase 14 outputs after updating `PHASE_STATUS.md`; all 19 required inputs were present and prior phases not closed returned `0`.
- Output path: `docs/codebase-audit/outputs/phase-14-summary.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No trailing whitespace found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored=matching docs/codebase-audit`
- Result: Showed only audit metadata, output index, generated Phase 7 summary timestamp, Phase 14 generator, and ignored Phase 14 outputs pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs/codebase-audit/outputs/phase-14-phase-status-snapshot.csv | Where-Object { $_.Status -notin @('Committed','Complete','Intentional','Blocked') } | Measure-Object | Select-Object -ExpandProperty Count`
- Result: Returned `0`; Phase 14 snapshot has no unclosed rows.
- Output path: `docs/codebase-audit/outputs/phase-14-phase-status-snapshot.csv`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --name-only`
- Result: Tracked diffs were limited to audit metadata/index files and regenerated `phase-07-summary.md`; Phase 14 generated outputs are ignored until force-staged.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --stat`
- Result: Tracked diff summary showed only `PHASE_STATUS.md`, `RUN_LOG.md`, `outputs/README.md`, and generated `phase-07-summary.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-14-verification-notes.md -Tail 25`
- Result: Reviewed verification notes: MSBuild path is recorded, no Phase 14 build is required, and all prior phases are closed in the snapshot.
- Output path: `docs/codebase-audit/outputs/phase-14-verification-notes.md`

### 2026-06-05T18:55:06.9477763-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-14-final-status-report.md`
- Result: Reviewed generated final status report before staging.
- Output path: `docs/codebase-audit/outputs/phase-14-final-status-report.md`

### 2026-06-05T18:55:55.5806209-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/outputs/phase-07-summary.md docs/codebase-audit/tools/New-FinalVerificationWorkflow.ps1 docs/codebase-audit/outputs/phase-14-required-inputs.csv docs/codebase-audit/outputs/phase-14-phase-status-snapshot.csv docs/codebase-audit/outputs/phase-14-change-classification.csv docs/codebase-audit/outputs/phase-14-verification-plan.csv docs/codebase-audit/outputs/phase-14-commit-history.csv docs/codebase-audit/outputs/phase-14-worktree-status.md docs/codebase-audit/outputs/phase-14-verification-notes.md docs/codebase-audit/outputs/phase-14-final-status-report.md docs/codebase-audit/outputs/phase-14-summary.md`
- Result: Staged Phase 14 metadata, generator, generated outputs, and docs truth timestamp output; Git reported expected LF-to-CRLF checkout warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:55.5806209-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:55.5806209-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged files are limited to Phase 14 audit metadata, generated outputs, generator, output index, and regenerated Phase 7 summary timestamp.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:55.5806209-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only the staged Phase 14 batch was pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:55:55.5806209-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --stat`
- Result: Reviewed staged diff size and confirmed 14 intended files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git add -- docs/codebase-audit/RUN_LOG.md`
- Result: Restaged the run log after recording Phase 14 staging checks; Git reported expected LF-to-CRLF checkout warning.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors before the Phase 14 content commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed final staged files were limited to Phase 14 audit metadata, generated outputs, generator, output index, and regenerated Phase 7 summary timestamp.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only the staged Phase 14 content batch was pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: add final audit verification workflow"`
- Result: Created commit `cbf808f6 docs: add final audit verification workflow`.
- Output path: `docs/codebase-audit/outputs/phase-14-final-status-report.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean after the Phase 14 content commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Returned `cbf808f6`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T18:56:41.4271983-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git log --oneline -n 6`
- Result: Confirmed recent audit closeout commits, with `cbf808f6 docs: add final audit verification workflow` at HEAD.
- Output path: `docs/codebase-audit/outputs/phase-14-commit-history.csv`

### 2026-06-05T18:57:57.1364682-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath docs/codebase-audit -Recurse -File | Select-String -Pattern '[ \t]+$'`
- Result: No trailing whitespace found in audit files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:57:57.1364682-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: No whitespace errors; Git reported expected LF-to-CRLF checkout warnings for touched text files.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:57:57.1364682-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Only final Phase 14 status-record files were modified.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:57:57.1364682-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --name-only`
- Result: Confirmed final metadata diffs are limited to `PHASE_STATUS.md`, `RUN_LOG.md`, `outputs/README.md`, and `phase-14-final-status-report.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:58:38.3044189-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git add -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/outputs/phase-14-final-status-report.md`
- Result: Failed because `docs/codebase-audit/outputs/` is ignored; rerun with `-f` was required for the intended generated final report.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:58:38.3044189-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/outputs/README.md docs/codebase-audit/outputs/phase-14-final-status-report.md`
- Result: Staged final Phase 14 status-record files, including ignored generated final report.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:58:38.3044189-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:58:38.3044189-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged metadata files are limited to `PHASE_STATUS.md`, `RUN_LOG.md`, `outputs/README.md`, and `phase-14-final-status-report.md`.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T18:58:38.3044189-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only the staged final metadata batch was pending.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T19:00:02.2765540-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git commit -m "docs: record audit phase 14 commit"`
- Result: Created commit `aeb6e947 docs: record audit phase 14 commit`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-05T19:00:02.2765540-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean after the Phase 14 status-record commit.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-05T19:00:02.2765540-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `Get-Content -LiteralPath docs/codebase-audit/outputs/phase-14-final-status-report.md -TotalCount 20`
- Result: Found stale self-referential text saying the metadata commit was pending after `aeb6e947` had already been created.
- Output path: `docs/codebase-audit/outputs/phase-14-final-status-report.md`

### 2026-06-05T19:00:02.2765540-05:00

- Affected phase: Phase 14
- Cwd: `D:\ConficturaUO`
- Command: `git log --oneline -n 4`
- Result: Confirmed `aeb6e947 docs: record audit phase 14 commit` followed `cbf808f6 docs: add final audit verification workflow`.
- Output path: `docs/codebase-audit/outputs/phase-14-final-status-report.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean before applying the live build context documentation update.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed root `AGENTS.md` applies and no existing `docs/codebase-audit/AGENTS.md` scope existed before this batch; nested gump/system instruction files were unrelated to the edited docs/audit paths.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Phase 14 housekeeping
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Confirmed current audit closeout HEAD was `4120e1be`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Phase 14 housekeeping
- Cwd: `D:\ConficturaUO`
- Command: `git log --pretty=format:"%h|%cI|%s" -n 8`
- Result: Confirmed final Phase 14 commits `cbf808f6`, `aeb6e947`, and `4120e1be` for the refreshed commit history.
- Output path: `docs/codebase-audit/outputs/phase-14-commit-history.csv`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `Select-String` source checks in `Data/System/Source/Main.cs` and `Data/System/Source/ScriptCompiler.cs`
- Result: Confirmed startup loops on `ScriptCompiler.Compile`, script initialization happens after compile, script discovery starts at `Data/Scripts`, discovery recurses directories, and CodeDOM compiles gathered `.cs` files.
- Output path: `docs/codebase-audit/outputs/live-build-and-runtime-script-compile-model.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `manual documentation edits`
- Result: Updated root `AGENTS.md`, added `docs/codebase-audit/AGENTS.md`, added live runtime compile model output, reclassified Phase 2 build verification as IDE/project hygiene, refreshed Phase 14 closeout metadata, and corrected the stale runtime-inventory output row.
- Output path: `AGENTS.md`; `docs/codebase-audit/AGENTS.md`; `docs/codebase-audit/outputs/live-build-and-runtime-script-compile-model.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed the new `docs/codebase-audit/AGENTS.md` scope is discoverable alongside the root and nested instruction files.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors for tracked docs/instruction edits; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git add -- AGENTS.md docs/codebase-audit/PHASE_STATUS.md docs/codebase-audit/RUN_LOG.md docs/codebase-audit/AGENTS.md`
- Result: Staged root and audit-scope instruction/status/log files; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git add -f -- docs/codebase-audit/outputs/README.md docs/codebase-audit/outputs/phase-02-build-verification.md docs/codebase-audit/outputs/phase-14-commit-history.csv docs/codebase-audit/outputs/phase-14-final-status-report.md docs/codebase-audit/outputs/live-build-and-runtime-script-compile-model.md`
- Result: Force-staged intended ignored audit output files.
- Output path: `docs/codebase-audit/outputs/README.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T13:58:21.5320530-05:00

- Affected phase: Post-audit live build context update
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged files were limited to root instructions, audit-scope instructions, audit status/log files, and intended audit outputs.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:05:54.8534809-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`
- Result: Passed. MSBuild reported `Server -> D:\ConficturaUO\ConficturaServer.exe`.
- Output path: `docs/codebase-audit/outputs/source-build-and-runtime-compile-baseline.md`

### 2026-06-06T14:05:54.8534809-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed the source build updated tracked generated artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
- Output path: `docs/codebase-audit/outputs/source-build-and-runtime-compile-baseline.md`

### 2026-06-06T14:05:54.8534809-05:00

- Affected phase: Post-audit runtime script inventory
- Cwd: `D:\ConficturaUO`
- Command: `Get-ChildItem -LiteralPath Data\Scripts -Recurse -File -Filter *.cs` with `bin`/`obj` exclusion, exported to CSV.
- Result: Generated `runtime-script-compile-inventory.csv` with 6,581 runtime-visible script source rows.
- Output path: `docs/codebase-audit/outputs/runtime-script-compile-inventory.csv`

### 2026-06-06T14:05:54.8534809-05:00

- Affected phase: Post-audit runtime startup smoke
- Cwd: `D:\ConficturaUO`
- Command: `.\ConficturaServer.exe -service -nocache`
- Result: Not run. Startup smoke was recorded as unavailable because this checkout contains a `Saves` tree and no safe no-load/no-listen startup flag was source-verified.
- Output path: `docs/codebase-audit/outputs/source-build-and-runtime-compile-baseline.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`
- Result: Restored tracked generated server artifacts produced by the source build.
- Output path: `docs/codebase-audit/outputs/source-build-and-runtime-compile-baseline.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed tracked generated server artifacts were restored; remaining visible changes were audit docs/output index updates.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit runtime script inventory
- Cwd: `D:\ConficturaUO`
- Command: `(Import-Csv -LiteralPath docs/codebase-audit/outputs/runtime-script-compile-inventory.csv).Count`
- Result: Confirmed the generated runtime script inventory contains 6,581 rows.
- Output path: `docs/codebase-audit/outputs/runtime-script-compile-inventory.csv`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors for tracked baseline docs/index edits; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored docs/codebase-audit/outputs/runtime-script-compile-inventory.csv docs/codebase-audit/outputs/source-build-and-runtime-compile-baseline.md`
- Result: Confirmed the new baseline output files are ignored by the output directory policy and require force-staging.
- Output path: `docs/codebase-audit/outputs/README.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:06:44.1367867-05:00

- Affected phase: Post-audit source build baseline
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged files were limited to `RUN_LOG.md`, output index, runtime script inventory, and source-build/runtime-compile baseline note.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Worktree was clean before implementing the post-audit next-step plan.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed root `AGENTS.md` and `docs/codebase-audit/AGENTS.md` apply to this documentation batch.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `git rev-parse --short HEAD`
- Result: Confirmed starting post-audit live-runtime baseline HEAD was `9dce70de`.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `manual documentation edits`
- Result: Added post-audit current-state notes, added runtime-first batch plan, updated output index, and recorded `9dce70de` as the post-audit live-runtime baseline HEAD.
- Output path: `docs/codebase-audit/outputs/post-audit-next-steps.md`; `docs/codebase-audit/outputs/post-audit-batch-plan.csv`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors for the post-audit next-step documentation batch; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:18:55.2116400-05:00

- Affected phase: Post-audit next-step plan
- Cwd: `D:\ConficturaUO`
- Command: `git status --short --ignored docs/codebase-audit/outputs/post-audit-next-steps.md docs/codebase-audit/outputs/post-audit-batch-plan.csv`
- Result: Confirmed new post-audit output files are ignored by the output directory policy and require force-staging.
- Output path: `docs/codebase-audit/outputs/README.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `manual source edits`
- Result: Added `-compileonly` argument parsing and early return after successful script compilation in `Main.cs`.
- Output path: `Data/System/Source/Main.cs`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `Copy-Item` backup of `Data/Data*.bin` and `Data/Data.hash`
- Result: Backed up `Data.bin` and `Data.hash` to `%TEMP%\confictura-compileonly-backup-20260606T141855`.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`
- Result: Passed. MSBuild reported `Server -> D:\ConficturaUO\ConficturaServer.exe`.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `.\ConficturaServer.exe -compileonly -nocache`
- Result: First run failed with duplicate `TargetFrameworkAttribute` from generated `Data/Scripts/obj/Debug/.NETFramework,Version=v4.8.AssemblyAttributes.cs`; no `Listening:` output was emitted.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `manual source edits`
- Result: Updated `ScriptCompiler.GetScripts` to skip exact `bin` and `obj` directories during runtime script discovery.
- Output path: `Data/System/Source/ScriptCompiler.cs`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`
- Result: Passed after the generated-folder runtime compiler fix.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `.\ConficturaServer.exe -compileonly -nocache`
- Result: Passed with exit code 0. Output included `Scripts: Compile-only verification completed successfully.` and no `Listening:` lines.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `Copy-Item` restore of backed-up `Data/Data.bin` and `Data/Data.hash`
- Result: Restored script cache artifacts from the pre-verification backup.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`
- Result: Restored tracked generated server artifacts produced by the source builds.
- Output path: `docs/codebase-audit/outputs/compile-only-verification-baseline.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Confirmed only `Data/System/Source/Main.cs` and `Data/System/Source/ScriptCompiler.cs` remained modified before recording verification outputs.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors for source and audit-output edits; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `rg -n -F -- "-compileonly" Data/System/Source/Main.cs docs/codebase-audit/outputs/compile-only-verification-baseline.md docs/codebase-audit/outputs/post-audit-next-steps.md`
- Result: Confirmed `-compileonly` is present in argument parsing, argument display, and audit verification outputs.
- Output path: `Data/System/Source/Main.cs`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `rg -n -F "Project output folders" Data/System/Source/ScriptCompiler.cs`
- Result: Confirmed the runtime compiler documents why generated project output folders are skipped.
- Output path: `Data/System/Source/ScriptCompiler.cs`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --check`
- Result: Passed with no whitespace errors.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:24:35.6662555-05:00

- Affected phase: Post-audit compile-only verification
- Cwd: `D:\ConficturaUO`
- Command: `git diff --cached --name-only`
- Result: Confirmed staged files were limited to `Main.cs`, `ScriptCompiler.cs`, run log, output index, compile-only verification output, and updated post-audit baseline notes.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean at the start of the batch.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed applicable root and audit-scoped instruction files before edits; packet-handler source files in this batch are governed by the root `AGENTS.md`, and audit outputs are governed by `docs/codebase-audit/AGENTS.md`.
- Output path: `docs/codebase-audit/outputs/post-batch-a-packet-handler-review.csv`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `phase-05-packet-handler-register.csv` rows against packet-handler source files.
- Result: Reviewed all 17 P0 packet-handler rows. Source-confirmed fixes were limited to XMLSpawner `UseReq`, XMLSpawner book content edit guards, and Monopoly gump-response fallback reader position.
- Output path: `docs/codebase-audit/outputs/post-batch-a-packet-handler-review.csv`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv -LiteralPath docs\codebase-audit\outputs\post-batch-a-packet-handler-review.csv | Measure-Object`
- Result: Parsed 17 review rows, matching the Phase 5 packet-handler register count.
- Output path: `docs/codebase-audit/outputs/post-batch-a-packet-handler-review.csv`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `& .\docs\codebase-audit\tools\New-RuntimeHookMap.ps1`
- Result: Passed; generated 6,604 hook rows and 17 packet rows. The only generated file content churn was a Phase 5 summary timestamp, which was restored and not committed.
- Output path: `docs/codebase-audit/outputs/runtime-hook-map.csv`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`
- Result: Passed; MSBuild produced `D:\ConficturaUO\ConficturaServer.exe`.
- Output path: `Data/System/Source/Server.csproj`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: `.\ConficturaServer.exe -compileonly -nocache`
- Result: Passed with exit code 0. Output included `Scripts: Compile-only verification completed successfully.` and no `Listening:` lines.
- Output path: `docs/codebase-audit/outputs/post-audit-next-steps.md`

### 2026-06-06T14:50:28.7959583-05:00

- Affected phase: Post-audit `POST-BATCH-A` packet-handler review
- Cwd: `D:\ConficturaUO`
- Command: Restore generated artifacts: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`; restore `Data\Data.bin` and `Data\Data.hash` from temporary backups.
- Result: Generated executable/config/PDB and script cache files restored; `git status --short` no longer listed generated build/runtime artifacts.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit active backlog reconciliation and `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean at the start of the reconciliation and initial save-triage batch.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit active backlog reconciliation and `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed applicable root and audit-scoped instruction files before editing audit artifacts.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit active backlog reconciliation
- Cwd: `D:\ConficturaUO`
- Command: Generate `post-audit-active-backlog-status.csv` from `repair-backlog.csv` and `post-batch-a-packet-handler-review.csv`.
- Result: Wrote 17 active packet-handler disposition rows: 3 `Fixed` and 14 `ReviewedNoChange`; `repair-backlog.csv` remains historical Phase 13 generated evidence.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of the 19 `ServerCore` critical save-compatibility rows from `phase-06-save-compatibility-repair-backlog.csv`.
- Result: Classified 19 high-blast-radius rows without source edits: 7 `FalsePositive`, 6 `IntentionalLegacy`, and 6 `SafeNoChange`.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Generate `post-batch-b-save-compatibility-triage.csv` from `phase-06-save-compatibility-repair-backlog.csv`, `serialization-register.csv`, `repair-backlog.csv`, and reviewed source evidence.
- Result: Wrote 304 scoped P0 critical save-compatibility rows: 19 `Reviewed` with decisions and 285 `Queued` for later source review; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit active backlog reconciliation and `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-audit-active-backlog-status.csv` and `post-batch-b-save-compatibility-triage.csv`.
- Result: Parsed 17 active backlog overlay rows with 3 `Fixed` and 14 `ReviewedNoChange`; parsed 304 save-triage rows with 19 `Reviewed`, 285 `Queued`, 0 reviewed rows missing decisions, 0 queued rows carrying decisions, and 0 missing backlog IDs.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T16:42:02.3829332-05:00

- Affected phase: Post-audit active backlog reconciliation and `POST-BATCH-B` save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors for tracked audit artifact edits; Git emitted expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T20:37:00.9767061-05:00

- Affected phase: Post-audit `POST-BATCH-B-02A` XMLSpawner central persistence save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean at the start of the review-only subbatch.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T20:37:00.9767061-05:00

- Affected phase: Post-audit `POST-BATCH-B-02A` XMLSpawner central persistence save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`
- Result: Confirmed applicable root and audit-scoped instruction files before editing audit artifacts.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T20:37:00.9767061-05:00

- Affected phase: Post-audit `POST-BATCH-B-02A` XMLSpawner central persistence save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of 10 queued XMLSpawner central persistence rows in `post-batch-b-save-compatibility-triage.csv`.
- Result: Reviewed `BaseXmlSpawner.cs`, `XmlAttachment.cs`, `XmlSpawner2.cs`, and `XmlQuestPoints.cs`; classified 6 rows as `FalsePositive` and 4 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T20:37:00.9767061-05:00

- Affected phase: Post-audit `POST-BATCH-B-02A` XMLSpawner central persistence save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv` and append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 29 reviewed rows, 275 queued rows, 0 reviewed rows without decisions, and 0 queued rows carrying decisions; active backlog overlay now has 27 rows, including 10 save-compatibility rows for `POST-BATCH-B-02A`.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T20:37:00.9767061-05:00

- Affected phase: Post-audit `POST-BATCH-B-02A` XMLSpawner central persistence save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git diff --name-only`; `git diff --check`
- Result: Changed-file scope was limited to six audit artifacts under `docs/codebase-audit`; whitespace check passed with expected `core.autocrlf=true` line-ending warnings. No source files changed, so no source build or compile-only smoke was run for this review-only batch.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean at the start of the review-only subbatch.
- Output path: `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `rg --files -g AGENTS.md`; read `docs/codebase-audit/AGENTS.md`
- Result: Confirmed applicable root and audit-scoped instruction files before editing audit artifacts.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of 20 queued XMLSpawner serializer rows in `post-batch-b-save-compatibility-triage.csv`.
- Result: Reviewed XMLSpawner attachment, item, mobile, challenge, region, and quest serializers; classified 15 rows as `SafeNoChange` and 5 rows as `IntentionalLegacy`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 49 reviewed rows, 255 queued rows, 0 reviewed rows missing decisions/evidence/actions, and 0 queued rows carrying decisions; active backlog overlay now has 47 rows, including 20 save-compatibility rows for `POST-BATCH-B-02B`.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`.
- Result: Parsed 304 save-triage rows with 49 `Reviewed`, 255 `Queued`, 20 `POST-BATCH-B-02B` rows, 15 `SafeNoChange`, and 5 `IntentionalLegacy`; parsed 47 active backlog overlay rows with 17 packet rows, 10 `POST-BATCH-B-02A` rows, and 20 `POST-BATCH-B-02B` rows.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T21:22:59.8315240-05:00

- Affected phase: Post-audit `POST-BATCH-B-02B` XMLSpawner remaining serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git diff --name-only`; `git diff --check`
- Result: Changed-file scope was limited to five audit artifacts before this run-log update; whitespace check passed with expected `core.autocrlf=true` line-ending warnings. No source files changed, so no source build or compile-only smoke was run for this review-only batch.
- Output path: `docs/codebase-audit/RUN_LOG.md`

### 2026-06-06T22:07:09.5100825-05:00

- Affected phase: Post-audit `POST-BATCH-B-02C` `System:Obsolete` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; read root and audit-scoped `AGENTS.md` instructions.
- Result: Worktree was clean before editing; applicable audit instruction scope was confirmed before updating audit artifacts.
- Output path: `docs/codebase-audit/AGENTS.md`

### 2026-06-06T22:07:09.5100825-05:00

- Affected phase: Post-audit `POST-BATCH-B-02C` `System:Obsolete` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of 28 queued `System:Obsolete` serializer rows in `post-batch-b-save-compatibility-triage.csv`.
- Result: Reviewed obsolete pigment, faction, ethics, town, election, voter, candidate, order, reaction, reflector, and persistence serializers in `Data/Scripts/System/Obsolete/Obsolete.cs`; classified 12 rows as `FalsePositive`, 11 rows as `SafeNoChange`, and 5 rows as `IntentionalLegacy`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:07:09.5100825-05:00

- Affected phase: Post-audit `POST-BATCH-B-02C` `System:Obsolete` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 77 reviewed rows, 227 queued rows, 0 reviewed rows missing decisions/evidence/actions, and 0 queued rows carrying decisions; active backlog overlay now has 75 rows, including 28 save-compatibility rows for `POST-BATCH-B-02C`.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:07:09.5100825-05:00

- Affected phase: Post-audit `POST-BATCH-B-02C` `System:Obsolete` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 77 `Reviewed`, 227 `Queued`, 28 `POST-BATCH-B-02C` rows, 12 `FalsePositive`, 5 `IntentionalLegacy`, and 11 `SafeNoChange`; active overlay has one row for each 58 active `POST-BATCH-B-02*` save disposition and 75 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:11:27.3221361-05:00

- Affected phase: Post-audit `POST-BATCH-B-03A` `Custom:Mobiles/Civilized` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Civilized` serializer rows.
- Result: Reviewed `GraveRobber`, `Malachi`, and `RangerOfTheBritainMilitia`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:11:27.3221361-05:00

- Affected phase: Post-audit `POST-BATCH-B-03A` `Custom:Mobiles/Civilized` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 80 reviewed rows, 224 queued rows, and 3 `POST-BATCH-B-03A` rows, all `SafeNoChange`; active backlog overlay now has 78 rows, including 61 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:11:27.3221361-05:00

- Affected phase: Post-audit `POST-BATCH-B-03A` `Custom:Mobiles/Civilized` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 80 `Reviewed`, 224 `Queued`, 3 `POST-BATCH-B-03A` rows, and 3 `SafeNoChange`; active overlay has one row for each 61 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 78 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:14:54.4566522-05:00

- Affected phase: Post-audit `POST-BATCH-B-03B` `Custom:Mobiles/Constructs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 5 queued `Custom:Mobiles/Constructs` serializer rows.
- Result: Reviewed `CursedAdventurer`, `DiosMonster`, `FinalExodus`, `ManaGolem`, and `TombWarden`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 5 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:14:54.4566522-05:00

- Affected phase: Post-audit `POST-BATCH-B-03B` `Custom:Mobiles/Constructs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 85 reviewed rows, 219 queued rows, and 5 `POST-BATCH-B-03B` rows, all `SafeNoChange`; active backlog overlay now has 83 rows, including 66 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:14:54.4566522-05:00

- Affected phase: Post-audit `POST-BATCH-B-03B` `Custom:Mobiles/Constructs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 85 `Reviewed`, 219 `Queued`, 5 `POST-BATCH-B-03B` rows, and 5 `SafeNoChange`; active overlay has one row for each 66 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 83 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:17:42.8375164-05:00

- Affected phase: Post-audit `POST-BATCH-B-03C` `Custom:Mobiles/Cultists` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Cultists` serializer rows.
- Result: Reviewed `DarkCultist`, `DarkCultistApprentice`, and `DarkDragonCultist`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:17:42.8375164-05:00

- Affected phase: Post-audit `POST-BATCH-B-03C` `Custom:Mobiles/Cultists` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 88 reviewed rows, 216 queued rows, and 3 `POST-BATCH-B-03C` rows, all `SafeNoChange`; active backlog overlay now has 86 rows, including 69 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:17:42.8375164-05:00

- Affected phase: Post-audit `POST-BATCH-B-03C` `Custom:Mobiles/Cultists` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 88 `Reviewed`, 216 `Queued`, 3 `POST-BATCH-B-03C` rows, and 3 `SafeNoChange`; active overlay has one row for each 69 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 86 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:20:46.5157459-05:00

- Affected phase: Post-audit `POST-BATCH-B-03D` `Custom:Mobiles/Daemons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 4 queued `Custom:Mobiles/Daemons` serializer rows.
- Result: Reviewed `FallenAngel`, `GuardianOfTheUnderworld`, `PlagueGod`, and `TombPest`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 4 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:20:46.5157459-05:00

- Affected phase: Post-audit `POST-BATCH-B-03D` `Custom:Mobiles/Daemons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 92 reviewed rows, 212 queued rows, and 4 `POST-BATCH-B-03D` rows, all `SafeNoChange`; active backlog overlay now has 90 rows, including 73 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:20:46.5157459-05:00

- Affected phase: Post-audit `POST-BATCH-B-03D` `Custom:Mobiles/Daemons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 92 `Reviewed`, 212 `Queued`, 4 `POST-BATCH-B-03D` rows, and 4 `SafeNoChange`; active overlay has one row for each 73 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 90 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:24:25.5078831-05:00

- Affected phase: Post-audit `POST-BATCH-B-03E` `Custom:Mobiles/Dragons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 4 queued `Custom:Mobiles/Dragons` serializer rows.
- Result: Reviewed `DarkDragonAvatar`, `DragonHydra`, `DragonIllusion`, and `Wyrmling`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 4 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:24:25.5078831-05:00

- Affected phase: Post-audit `POST-BATCH-B-03E` `Custom:Mobiles/Dragons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 96 reviewed rows, 208 queued rows, and 4 `POST-BATCH-B-03E` rows, all `SafeNoChange`; active backlog overlay now has 94 rows, including 77 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:24:25.5078831-05:00

- Affected phase: Post-audit `POST-BATCH-B-03E` `Custom:Mobiles/Dragons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 96 `Reviewed`, 208 `Queued`, 4 `POST-BATCH-B-03E` rows, and 4 `SafeNoChange`; active overlay has one row for each 77 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 94 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:28:51.5387682-05:00

- Affected phase: Post-audit `POST-BATCH-B-03F` `Custom:Mobiles/Elementals` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 7 queued `Custom:Mobiles/Elementals` serializer rows.
- Result: Reviewed `FireIllusion`, `FireKing`, `FireServant`, `FireSpirit`, `Geode`, `GrumpyEarthSpirit`, and `SkySpirit`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 7 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:28:51.5387682-05:00

- Affected phase: Post-audit `POST-BATCH-B-03F` `Custom:Mobiles/Elementals` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 103 reviewed rows, 201 queued rows, and 7 `POST-BATCH-B-03F` rows, all `SafeNoChange`; active backlog overlay now has 101 rows, including 84 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:28:51.5387682-05:00

- Affected phase: Post-audit `POST-BATCH-B-03F` `Custom:Mobiles/Elementals` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 103 `Reviewed`, 201 `Queued`, 7 `POST-BATCH-B-03F` rows, and 7 `SafeNoChange`; active overlay has one row for each 84 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 101 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:32:41.2393702-05:00

- Affected phase: Post-audit `POST-BATCH-B-03G` `Custom:Mobiles/Goliaths` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Goliaths` serializer rows.
- Result: Reviewed `Atlas`, `CyclopsIntruder`, and `TwoFace`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:32:41.2393702-05:00

- Affected phase: Post-audit `POST-BATCH-B-03G` `Custom:Mobiles/Goliaths` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 106 reviewed rows, 198 queued rows, and 3 `POST-BATCH-B-03G` rows, all `SafeNoChange`; active backlog overlay now has 104 rows, including 87 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:32:41.2393702-05:00

- Affected phase: Post-audit `POST-BATCH-B-03G` `Custom:Mobiles/Goliaths` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 106 `Reviewed`, 198 `Queued`, 3 `POST-BATCH-B-03G` rows, and 3 `SafeNoChange`; active overlay has one row for each 87 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 104 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:36:58.6200354-05:00

- Affected phase: Post-audit `POST-BATCH-B-03H` `Custom:Mobiles/Kobolds` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Kobolds` serializer rows.
- Result: Reviewed `KoboldChief`, `KoboldFootSoldier`, and `KoboldRingleader`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:36:58.6200354-05:00

- Affected phase: Post-audit `POST-BATCH-B-03H` `Custom:Mobiles/Kobolds` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 109 reviewed rows, 195 queued rows, and 3 `POST-BATCH-B-03H` rows, all `SafeNoChange`; active backlog overlay now has 107 rows, including 90 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:36:58.6200354-05:00

- Affected phase: Post-audit `POST-BATCH-B-03H` `Custom:Mobiles/Kobolds` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 109 `Reviewed`, 195 `Queued`, 3 `POST-BATCH-B-03H` rows, and 3 `SafeNoChange`; active overlay has one row for each 90 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 107 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:40:45.4901685-05:00

- Affected phase: Post-audit `POST-BATCH-B-03I` `Custom:Mobiles/Minotaurs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Minotaurs` serializer rows.
- Result: Reviewed `MazeMinotaur`, `MazeMinotaurEast`, and `MazeMinotaurSouth`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:40:45.4901685-05:00

- Affected phase: Post-audit `POST-BATCH-B-03I` `Custom:Mobiles/Minotaurs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 112 reviewed rows, 192 queued rows, and 3 `POST-BATCH-B-03I` rows, all `SafeNoChange`; active backlog overlay now has 110 rows, including 93 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:40:45.4901685-05:00

- Affected phase: Post-audit `POST-BATCH-B-03I` `Custom:Mobiles/Minotaurs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 112 `Reviewed`, 192 `Queued`, 3 `POST-BATCH-B-03I` rows, and 3 `SafeNoChange`; active overlay has one row for each 93 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 110 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:44:00.7275262-05:00

- Affected phase: Post-audit `POST-BATCH-B-03J` `Custom:Mobiles/Mystical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 4 queued `Custom:Mobiles/Mystical` serializer rows.
- Result: Reviewed `Archon`, `EternalGuardian`, `Lovecraftian`, and `TrueVirtue`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 4 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:44:00.7275262-05:00

- Affected phase: Post-audit `POST-BATCH-B-03J` `Custom:Mobiles/Mystical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 116 reviewed rows, 188 queued rows, and 4 `POST-BATCH-B-03J` rows, all `SafeNoChange`; active backlog overlay now has 114 rows, including 97 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:44:00.7275262-05:00

- Affected phase: Post-audit `POST-BATCH-B-03J` `Custom:Mobiles/Mystical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 116 `Reviewed`, 188 `Queued`, 4 `POST-BATCH-B-03J` rows, and 4 `SafeNoChange`; active overlay has one row for each 97 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 114 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:47:13.9837366-05:00

- Affected phase: Post-audit `POST-BATCH-B-03K` `Custom:Mobiles/Pirates` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Pirates` serializer rows.
- Result: Reviewed `CaptainSwag`, `Pirate`, and `Scallywag`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:47:13.9837366-05:00

- Affected phase: Post-audit `POST-BATCH-B-03K` `Custom:Mobiles/Pirates` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 119 reviewed rows, 185 queued rows, and 3 `POST-BATCH-B-03K` rows, all `SafeNoChange`; active backlog overlay now has 117 rows, including 100 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:47:13.9837366-05:00

- Affected phase: Post-audit `POST-BATCH-B-03K` `Custom:Mobiles/Pirates` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 119 `Reviewed`, 185 `Queued`, 3 `POST-BATCH-B-03K` rows, and 3 `SafeNoChange`; active overlay has one row for each 100 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 117 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:50:21.3318360-05:00

- Affected phase: Post-audit `POST-BATCH-B-03L` `Custom:Mobiles/Serpents` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 2 queued `Custom:Mobiles/Serpents` serializer rows.
- Result: Reviewed `FireAngel` and `ImmortalGenie`; both call base serialize/deserialize, write version 0, read version 0, and have no custom serialized fields; classified both rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:50:21.3318360-05:00

- Affected phase: Post-audit `POST-BATCH-B-03L` `Custom:Mobiles/Serpents` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 121 reviewed rows, 183 queued rows, and 2 `POST-BATCH-B-03L` rows, all `SafeNoChange`; active backlog overlay now has 119 rows, including 102 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:50:21.3318360-05:00

- Affected phase: Post-audit `POST-BATCH-B-03L` `Custom:Mobiles/Serpents` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 121 `Reviewed`, 183 `Queued`, 2 `POST-BATCH-B-03L` rows, and 2 `SafeNoChange`; active overlay has one row for each 102 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 119 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:53:28.9794767-05:00

- Affected phase: Post-audit `POST-BATCH-B-03M` `Custom:Mobiles/Spiders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; source review of 3 queued `Custom:Mobiles/Spiders` serializer rows.
- Result: Reviewed `SpiderIllusion`, `Spiderling`, and `SpiderQueen`; each calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 3 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T22:53:28.9794767-05:00

- Affected phase: Post-audit `POST-BATCH-B-03M` `Custom:Mobiles/Spiders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 124 reviewed rows, 180 queued rows, and 3 `POST-BATCH-B-03M` rows, all `SafeNoChange`; active backlog overlay now has 122 rows, including 105 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T22:53:28.9794767-05:00

- Affected phase: Post-audit `POST-BATCH-B-03M` `Custom:Mobiles/Spiders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 124 `Reviewed`, 180 `Queued`, 3 `POST-BATCH-B-03M` rows, and 3 `SafeNoChange`; active overlay has one row for each 105 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 122 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:00:23.0394448-05:00

- Affected phase: Post-audit `POST-BATCH-B-03N` `Custom:Mobiles/Titans` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of 5 queued `Custom:Mobiles/Titans` serializer rows in `FalseEarthGod.cs`, `FalseFireGod.cs`, `FalseSeaGod.cs`, `FalseWindGod.cs`, and `Herculian.cs`.
- Result: Each reviewed serializer calls base serialize/deserialize, writes version 0, reads version 0, and has no custom serialized fields; classified all 5 rows as `SafeNoChange`; no source or serialized-layout edits were approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:00:23.0394448-05:00

- Affected phase: Post-audit `POST-BATCH-B-03N` `Custom:Mobiles/Titans` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 129 reviewed rows, 175 queued rows, and 5 `POST-BATCH-B-03N` rows, all `SafeNoChange`; active backlog overlay now has 127 rows, including 110 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-06T23:02:18.6468100-05:00

- Affected phase: Post-audit `POST-BATCH-B-03N` `Custom:Mobiles/Titans` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 129 `Reviewed`, 175 `Queued`, 5 `POST-BATCH-B-03N` rows, and 5 `SafeNoChange`; active overlay has one row for each 110 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 127 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:06:40.4235492-05:00

- Affected phase: Post-audit `POST-BATCH-B-03O` `Custom:Mobiles/Undead` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`; source scan confirmed `Serialize`, `base.Serialize`, version write, `Deserialize`, `base.Deserialize`, and version read ordering for 19 queued `Custom:Mobiles/Undead` serializer rows.
- Result: Reviewed 19 queued `Custom:Mobiles/Undead` serializer rows. Source evidence showed version-only mobile serializers with aligned base calls and version write/read; classified all 19 rows as `SafeNoChange`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:06:40.4235492-05:00

- Affected phase: Post-audit `POST-BATCH-B-03O` `Custom:Mobiles/Undead` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 148 reviewed rows, 156 queued rows, and 19 `POST-BATCH-B-03O` rows, all `SafeNoChange`; active backlog overlay now has 146 rows, including 129 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:07:18.2987897-05:00

- Affected phase: Post-audit `POST-BATCH-B-03O` `Custom:Mobiles/Undead` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 148 `Reviewed`, 156 `Queued`, 19 `POST-BATCH-B-03O` rows, and 19 `SafeNoChange`; active overlay has one row for each 129 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 146 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:09:30.0545430-05:00

- Affected phase: Post-audit `POST-BATCH-B-03P` `Custom:Mobiles/Vendors` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source scan confirmed `Serialize`, `base.Serialize`, version write, `Deserialize`, `base.Deserialize`, and version read ordering for 3 queued `Custom:Mobiles/Vendors` serializer rows.
- Result: Reviewed 3 queued `Custom:Mobiles/Vendors` serializer rows. Source evidence showed version-only mobile serializers with aligned base calls and version write/read; classified all 3 rows as `SafeNoChange`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:09:30.0545430-05:00

- Affected phase: Post-audit `POST-BATCH-B-03P` `Custom:Mobiles/Vendors` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 151 reviewed rows, 153 queued rows, and 3 `POST-BATCH-B-03P` rows, all `SafeNoChange`; active backlog overlay now has 149 rows, including 132 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:11:10.3142791-05:00

- Affected phase: Post-audit `POST-BATCH-B-03P` `Custom:Mobiles/Vendors` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 151 `Reviewed`, 153 `Queued`, 3 `POST-BATCH-B-03P` rows, and 3 `SafeNoChange`; `Custom:Mobiles` has 0 queued rows; active overlay has one row for each 132 active `POST-BATCH-B-02*` and `POST-BATCH-B-03*` save disposition and 149 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:13:48.6090525-05:00

- Affected phase: Post-audit `POST-BATCH-B-04A` Homestead resource serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `WineGrapes.cs:33-103` and `Hops.cs:33-82`.
- Result: Current version 1 writes version plus variety enum and reads the same shape; version 0 branches consume old info-id shapes and map them to current varieties. Classified both rows as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:13:48.6090525-05:00

- Affected phase: Post-audit `POST-BATCH-B-04A` Homestead resource serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 153 reviewed rows, 151 queued rows, and 2 `POST-BATCH-B-04A` rows, all `IntentionalLegacy`; active backlog overlay now has 151 rows, including 134 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:14:29.5013819-05:00

- Affected phase: Post-audit `POST-BATCH-B-04A` Homestead resource serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 153 `Reviewed`, 151 `Queued`, 2 `POST-BATCH-B-04A` rows, and 2 `IntentionalLegacy`; active overlay has one row for each 134 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, and `POST-BATCH-B-04*` save disposition and 151 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:17:11.3106522-05:00

- Affected phase: Post-audit `POST-BATCH-B-04B` Homestead craft beverage and juice serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseCraftWine.cs`, `BaseCraftAle.cs`, `BaseCraftCider.cs`, `BaseCraftMead.cs`, and `BaseCraftJuice.cs` serializer blocks.
- Result: Current version 2 writes flags and optional crafter/quality/variety values, then writes legacy poisoner/poison/fill data; deserialization reads version 2 and falls through to version 1 and 0 branches. Classified all 5 rows as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:17:11.3106522-05:00

- Affected phase: Post-audit `POST-BATCH-B-04B` Homestead craft beverage and juice serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 158 reviewed rows, 146 queued rows, and 5 `POST-BATCH-B-04B` rows, all `IntentionalLegacy`; active backlog overlay now has 156 rows, including 139 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:17:48.9700678-05:00

- Affected phase: Post-audit `POST-BATCH-B-04B` Homestead craft beverage and juice serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 158 `Reviewed`, 146 `Queued`, 5 `POST-BATCH-B-04B` rows, and 5 `IntentionalLegacy`; active overlay has one row for each 139 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, and `POST-BATCH-B-04*` save disposition and 156 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:20:03.8668234-05:00

- Affected phase: Post-audit `POST-BATCH-B-04C` Homestead crop serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseCrop.cs:135-180` plus 10 wild crop plant serializer wrappers.
- Result: `BaseCrop` writes and reads version, age/harvest fields, booleans, and ID list values in matching order; plant wrappers write and read only local version after base serialization. Classified all 11 rows as `SafeNoChange`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:20:03.8668234-05:00

- Affected phase: Post-audit `POST-BATCH-B-04C` Homestead crop serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 169 reviewed rows, 135 queued rows, and 11 `POST-BATCH-B-04C` rows, all `SafeNoChange`; active backlog overlay now has 167 rows, including 150 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:20:42.2690101-05:00

- Affected phase: Post-audit `POST-BATCH-B-04C` Homestead crop serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 169 `Reviewed`, 135 `Queued`, 11 `POST-BATCH-B-04C` rows, and 11 `SafeNoChange`; Homestead has 0 queued rows; active overlay has one row for each 150 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, and `POST-BATCH-B-04*` save disposition and 167 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:23:14.4900504-05:00

- Affected phase: Post-audit `POST-BATCH-B-05A` `System:Misc` helper serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `AOS.cs:1393-1433`, `ShardPoller.cs:388-419`, and `TextDefinition.cs:31-75`.
- Result: Reviewed 6 helper serializer rows. Source shows reader constructors or type-token read paths paired with helper `Serialize` methods; missing base override warnings are extractor noise. Classified all 6 rows as `FalsePositive`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:23:14.4900504-05:00

- Affected phase: Post-audit `POST-BATCH-B-05A` `System:Misc` helper serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 175 reviewed rows, 129 queued rows, and 6 `POST-BATCH-B-05A` rows, all `FalsePositive`; active backlog overlay now has 173 rows, including 156 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:23:50.3984841-05:00

- Affected phase: Post-audit `POST-BATCH-B-05A` `System:Misc` helper serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 175 `Reviewed`, 129 `Queued`, 6 `POST-BATCH-B-05A` rows, and 6 `FalsePositive`; active overlay has one row for each 156 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, and `POST-BATCH-B-05*` save disposition and 173 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:26:49.3474871-05:00

- Affected phase: Post-audit `POST-BATCH-B-05B` `System:Misc/Guilds.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Guilds.cs:205-236`, `Guilds.cs:578-616`, and `Guilds.cs:1335-1508`.
- Result: Helper rows for `AllianceInfo` and `WarDeclaration` use reader constructors and no applicable base serializer override; one duplicate `Guild` row was generated for the same source block. Main `Guild` serializer writes version 5 and falls through versions 4 through 0 on read. Classified 5 rows as `FalsePositive` and 1 row as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:26:49.3474871-05:00

- Affected phase: Post-audit `POST-BATCH-B-05B` `System:Misc/Guilds.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 181 reviewed rows, 123 queued rows, and 6 `POST-BATCH-B-05B` rows; active backlog overlay now has 179 rows, including 162 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:27:28.9912127-05:00

- Affected phase: Post-audit `POST-BATCH-B-05B` `System:Misc/Guilds.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 181 `Reviewed`, 123 `Queued`, 6 `POST-BATCH-B-05B` rows, 5 `FalsePositive`, and 1 `IntentionalLegacy`; active overlay has one row for each 162 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, and `POST-BATCH-B-05*` save disposition and 179 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:29:44.4932595-05:00

- Affected phase: Post-audit `POST-BATCH-B-05C` `System:Misc/Spawning.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Spawning.cs:4792-5163` and `Spawning.cs:6245-6375`.
- Result: `PremiumSpawner` and `Spawner` write version 4 and deserialize through versions 4, 3/2, 1, and 0 with legacy default handling. Classified both rows as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:29:44.4932595-05:00

- Affected phase: Post-audit `POST-BATCH-B-05C` `System:Misc/Spawning.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 183 reviewed rows, 121 queued rows, and 2 `POST-BATCH-B-05C` rows, all `IntentionalLegacy`; active backlog overlay now has 181 rows, including 164 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:31:47.1559311-05:00

- Affected phase: Post-audit `POST-BATCH-B-05C` `System:Misc/Spawning.cs` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 183 `Reviewed`, 121 `Queued`, 2 `POST-BATCH-B-05C` rows, and 2 `IntentionalLegacy`; `System:Misc` has 0 queued rows; active overlay has one row for each 164 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, and `POST-BATCH-B-05*` save disposition and 181 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:35:12.9936236-05:00

- Affected phase: Post-audit `POST-BATCH-B-06A` `Items:Trades` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RecallRune.cs:18-64`, `Runebook.cs:546-584`, `Arrow.cs:47-78`, `Bolt.cs:47-78`, `Feather.cs:38-69`, `Shaft.cs:38-69`, sharpening stone serializers, and `DisguisePersistance.cs:40-79`.
- Result: Reviewed 13 `Items:Trades` rows. Classified 7 rows as `SafeNoChange`, 5 sharpening stone rows as `IntentionalLegacy`, and 1 `RunebookEntry` base-call row as `FalsePositive`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:35:12.9936236-05:00

- Affected phase: Post-audit `POST-BATCH-B-06A` `Items:Trades` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 196 reviewed rows, 108 queued rows, and 13 `POST-BATCH-B-06A` rows; active backlog overlay now has 194 rows, including 177 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:39:56.0271054-05:00

- Affected phase: Post-audit `POST-BATCH-B-06A` `Items:Trades` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 196 `Reviewed`, 108 `Queued`, 13 `POST-BATCH-B-06A` rows, 1 `FalsePositive`, 5 `IntentionalLegacy`, and 7 `SafeNoChange`; `Items:Trades` has 0 queued rows; active overlay has one row for each 177 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, `POST-BATCH-B-05*`, and `POST-BATCH-B-06*` save disposition and 194 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:43:13.1315103-05:00

- Affected phase: Post-audit `POST-BATCH-B-07A` `Items:Misc` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `AcidSlime.cs:103-108`, `Corpse.cs:700-900`, `Head.cs:125-182`, `EffectController.cs:295-384`, `Firebomb.cs:241-249`, `Gold.cs:60-94`, `PlayerBulletinBoards.cs:126-185`, `PlayerBulletinBoards.cs:371-395`, `PoolOfAcid.cs:102-110`, `PowerGenerator.cs:603-635`, and `SerpentPillar.cs:105-126`.
- Result: Reviewed 12 `Items:Misc` rows. Classified 5 rows as `SafeNoChange`, 3 rows as `IntentionalLegacy`, 1 row as `FalsePositive`, 2 rows as `NeedsHumanDecision`, and 1 row as `NeedsMigrationPlan`. Human decision is required for transient `Item` subclasses that omit base serialization: `AcidSlime`, `FirebombField`, and `PoolOfAcid`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-06T23:43:13.1315103-05:00

- Affected phase: Post-audit `POST-BATCH-B-07A` `Items:Misc` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 208 reviewed rows, 96 queued rows, and 12 `POST-BATCH-B-07A` rows; active backlog overlay now has 206 rows, including 189 active save-compatibility dispositions; `outputs/README.md` marks the save-triage artifact `Blocked` pending transient-item save-policy guidance.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-06T23:46:06.5696089-05:00

- Affected phase: Post-audit `POST-BATCH-B-07A` `Items:Misc` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 208 `Reviewed`, 96 `Queued`, 12 `POST-BATCH-B-07A` rows, 1 `FalsePositive`, 3 `IntentionalLegacy`, 5 `SafeNoChange`, 2 `NeedsHumanDecision`, and 1 `NeedsMigrationPlan`; `Items:Misc` has 0 queued rows; active overlay has one row for each 189 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, `POST-BATCH-B-05*`, `POST-BATCH-B-06*`, and `POST-BATCH-B-07*` save disposition and 206 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T11:53:50.3593009-05:00

- Affected phase: Post-audit `POST-BATCH-B-07B` transient item save-policy decision
- Cwd: `D:\ConficturaUO`
- Command: Apply human decision to `SERIAL-0964`, `SERIAL-0973`, and `SERIAL-1006` in `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`.
- Result: `AcidSlime`, `FirebombField`, and `PoolOfAcid` are accepted as transient no-payload hazard-effect exceptions that should not survive world save/load. Reclassified all 3 rows from `NeedsMigrationPlan`/`NeedsHumanDecision` to `SafeNoChange`; no source files changed and no serialized-layout edit is approved.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T11:53:50.3593009-05:00

- Affected phase: Post-audit `POST-BATCH-B-07B` transient item save-policy decision
- Cwd: `D:\ConficturaUO`
- Command: Update `PHASE_STATUS.md`, `outputs/README.md`, and `outputs/post-audit-next-steps.md` after transient item policy acceptance.
- Result: Save-compatibility triage remains at 304 total rows, 208 reviewed rows, and 96 queued rows; reviewed decision counts are now 38 `FalsePositive`, 34 `IntentionalLegacy`, and 136 `SafeNoChange`; `outputs/README.md` marks `post-batch-b-save-compatibility-triage.csv` as `InProgress`.
- Output path: `docs/codebase-audit/outputs/post-audit-next-steps.md`

### 2026-06-07T11:56:32.4855090-05:00

- Affected phase: Post-audit `POST-BATCH-B-07B` transient item save-policy decision
- Cwd: `D:\ConficturaUO`
- Command: `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 208 `Reviewed`, 96 `Queued`, 3 `POST-BATCH-B-07B` rows, and 3 `SafeNoChange`; there are 0 `NeedsHumanDecision` rows and 0 `NeedsMigrationPlan` rows; active overlay has one row for each 189 active `POST-BATCH-B-02*`, `POST-BATCH-B-03*`, `POST-BATCH-B-04*`, `POST-BATCH-B-05*`, `POST-BATCH-B-06*`, and `POST-BATCH-B-07*` save disposition and 206 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:04:24.6776910-05:00

- Affected phase: Post-audit `POST-BATCH-B-08A` `Items:Houses` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseHouse.cs:2921-3162`, `BaseHouse.cs:4084-4094`, `HouseFoundation.cs:853-933`, `HouseFoundation.cs:2077-2128`, `TownHouse.cs:304-340`, `LawnGate.cs:140-173`, `LawnItem.cs:194-244`, `ShantyDoor.cs:182-215`, and `ShantyItem.cs:194-244`.
- Result: Reviewed 12 `Items:Houses` rows. Classified 9 rows as `SafeNoChange`, 2 helper base-call rows as `FalsePositive`, and 1 `TownHouse` row as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:04:24.6776910-05:00

- Affected phase: Post-audit `POST-BATCH-B-08A` `Items:Houses` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 220 reviewed rows, 84 queued rows, and 12 `POST-BATCH-B-08A` rows; active backlog overlay now has 218 rows, including 201 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:08:00.9791145-05:00

- Affected phase: Post-audit `POST-BATCH-B-08A` `Items:Houses` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 220 `Reviewed`, 84 `Queued`, 12 `POST-BATCH-B-08A` rows, 2 `FalsePositive`, 1 `IntentionalLegacy`, and 9 `SafeNoChange`; there are 0 `NeedsHumanDecision` rows and 0 `NeedsMigrationPlan` rows; active overlay has one row for each 201 non-`ServerCore` active save disposition and 218 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:10:42.8938133-05:00

- Affected phase: Post-audit `POST-BATCH-B-09A` `Trades:Bulk Orders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BOBFilter.cs:51-84`, `BOBLargeEntry.cs:103-144`, `BOBLargeSubEntry.cs:37-68`, `BOBSmallEntry.cs:100-144`, `BulkOrderBook.cs:193-276`, and `LargeBulkEntry.cs:180-200`.
- Result: Reviewed 11 `Trades:Bulk Orders` rows. Classified 6 rows as `SafeNoChange` and 5 helper base-call rows as `FalsePositive`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:10:42.8938133-05:00

- Affected phase: Post-audit `POST-BATCH-B-09A` `Trades:Bulk Orders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 231 reviewed rows, 73 queued rows, and 11 `POST-BATCH-B-09A` rows; active backlog overlay now has 229 rows, including 212 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:13:26.6689317-05:00

- Affected phase: Post-audit `POST-BATCH-B-09A` `Trades:Bulk Orders` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 231 `Reviewed`, 73 `Queued`, 11 `POST-BATCH-B-09A` rows, 5 `FalsePositive`, and 6 `SafeNoChange`; there are 0 `NeedsHumanDecision` rows and 0 `NeedsMigrationPlan` rows; active overlay has one row for each 212 non-`ServerCore` active save disposition and 229 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:17:37.7351987-05:00

- Affected phase: Post-audit `POST-BATCH-B-10A` `Items:Magical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Artifact_LuckyNecklace.cs:20-30`, `GiftLeatherGloves.cs:77-117`, `GiftCloaks.cs:122-165`, `GiftOuterTorso.cs:188-222`, `GiftShoes.cs:16-44`, `GiftShoes.cs:212-246`, `LevelLeatherGloves.cs:77-117`, `LevelCloaks.cs:122-165`, `LevelOuterTorso.cs:188-222`, `LevelShoes.cs:16-44`, and `LevelShoes.cs:212-246`.
- Result: Reviewed 11 `Items:Magical` rows. Classified 9 rows as `SafeNoChange` and 2 shoe-base resource-compatibility rows as `IntentionalLegacy`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:17:37.7351987-05:00

- Affected phase: Post-audit `POST-BATCH-B-10A` `Items:Magical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 242 reviewed rows, 62 queued rows, and 11 `POST-BATCH-B-10A` rows; active backlog overlay now has 240 rows, including 223 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:19:56.3608597-05:00

- Affected phase: Post-audit `POST-BATCH-B-10A` `Items:Magical` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 242 `Reviewed`, 62 `Queued`, 11 `POST-BATCH-B-10A` rows, 2 `IntentionalLegacy`, and 9 `SafeNoChange`; there are 0 `NeedsHumanDecision` rows and 0 `NeedsMigrationPlan` rows; active overlay has one row for each 223 non-`ServerCore` active save disposition and 240 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:22:18.7253198-05:00

- Affected phase: Post-audit `POST-BATCH-B-11A` `Items:Special` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `HouseRaffleStone.cs:48-69`, `HouseRaffleStone.cs:613-676`, `ScrollofAlacrity.cs:189-202`, `ScrollofTranscendence.cs:173-186`, `SpecialScroll.cs:341-371`, `MiningCart.cs:306-350`, and `TreeStump.cs:157-188`.
- Result: Reviewed 8 `Items:Special` rows. Classified 4 rows as `SafeNoChange`, 3 special-scroll compatibility rows as `IntentionalLegacy`, and 1 helper base-call row as `FalsePositive`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:22:18.7253198-05:00

- Affected phase: Post-audit `POST-BATCH-B-11A` `Items:Special` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 250 reviewed rows, 54 queued rows, and 8 `POST-BATCH-B-11A` rows; active backlog overlay now has 248 rows, including 231 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:24:41.6474179-05:00

- Affected phase: Post-audit `POST-BATCH-B-11A` `Items:Special` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; `Import-Csv` verification of `post-batch-b-save-compatibility-triage.csv` and `post-audit-active-backlog-status.csv`; `git diff --name-only -- Data`; `git diff --check`.
- Result: Parsed 304 save-triage rows with 250 `Reviewed`, 54 `Queued`, 8 `POST-BATCH-B-11A` rows, 1 `FalsePositive`, 3 `IntentionalLegacy`, and 4 `SafeNoChange`; there are 0 `NeedsHumanDecision` rows and 0 `NeedsMigrationPlan` rows; active overlay has one row for each 231 non-`ServerCore` active save disposition and 248 rows total; no `Data` files changed; whitespace check passed with expected `core.autocrlf=true` line-ending warnings.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:32:48.7538883-05:00

- Affected phase: Post-audit `POST-BATCH-B-12A` `Mobiles:Base` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseCreature.cs:6497-6625`, `BaseCreature.cs:6655-6755`, `BaseVendor.cs:3126-3190`, `PlayerBarkeeper.cs:108-128`, `PlayerMobile.cs:4539-4935`, `PlayerMobile.cs:5004-5221`, and `VendorInventory.cs:99-140`.
- Result: Reviewed 7 `Mobiles:Base` rows. Classified 3 rows as `SafeNoChange`, 2 high-blast-radius mobile rows as `IntentionalLegacy`, and 2 helper base-call rows as `FalsePositive`. No source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:32:48.7538883-05:00

- Affected phase: Post-audit `POST-BATCH-B-12A` `Mobiles:Base` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 257 reviewed rows, 47 queued rows, and 7 `POST-BATCH-B-12A` rows; active backlog overlay now has 255 rows, including 238 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:36:52.7279021-05:00

- Affected phase: Post-audit `POST-BATCH-B-13A` `Custom:Book Publisher [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `PublishedBook.cs:193-226`.
- Result: Reviewed 1 `Custom:Book Publisher [2.0]` row. Classified the row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:36:52.7279021-05:00

- Affected phase: Post-audit `POST-BATCH-B-13A` `Custom:Book Publisher [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 258 reviewed rows, 46 queued rows, and 1 `POST-BATCH-B-13A` row; active backlog overlay now has 256 rows, including 239 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:40:02.8210795-05:00

- Affected phase: Post-audit `POST-BATCH-B-14A` `Custom:CEO's GM Hiding Stone [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `gmhidingstone.cs:748-780`.
- Result: Reviewed 1 `Custom:CEO's GM Hiding Stone [2.0]` row. Classified the row as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:40:02.8210795-05:00

- Affected phase: Post-audit `POST-BATCH-B-14A` `Custom:CEO's GM Hiding Stone [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 259 reviewed rows, 45 queued rows, and 1 `POST-BATCH-B-14A` row; active backlog overlay now has 257 rows, including 240 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:42:46.0693251-05:00

- Affected phase: Post-audit `POST-BATCH-B-15A` `Custom:Champions` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `PlagueBeastLord.cs:276-309` and `ChampionSpawn.cs:1144-1275`.
- Result: Reviewed 2 `Custom:Champions` rows. Classified 1 row as `SafeNoChange` and 1 row as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:42:46.0693251-05:00

- Affected phase: Post-audit `POST-BATCH-B-15A` `Custom:Champions` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 261 reviewed rows, 43 queued rows, and 2 `POST-BATCH-B-15A` rows; active backlog overlay now has 259 rows, including 242 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:45:34.7412525-05:00

- Affected phase: Post-audit `POST-BATCH-B-16A` `Custom:CloneOfflinePlayerCharacters` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BackpackClone.cs:46-55`, `CharacterClone.cs:272-288`, `EtherealMountClone.cs:36-45`, and `MountClone.cs:45-54`.
- Result: Reviewed 4 `Custom:CloneOfflinePlayerCharacters` rows. Classified all 4 rows as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:45:34.7412525-05:00

- Affected phase: Post-audit `POST-BATCH-B-16A` `Custom:CloneOfflinePlayerCharacters` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 265 reviewed rows, 39 queued rows, and 4 `POST-BATCH-B-16A` rows; active backlog overlay now has 263 rows, including 246 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:49:12.5066183-05:00

- Affected phase: Post-audit `POST-BATCH-B-17A` `Custom:Government System` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `CityResurrectionStone.cs:131-137`, `CityResurrectionStone.cs:237-242`, `CityResurrectionStone.cs:278-320`, `LandLord.cs:148-165`, `MallToken.cs:103-122`, `ResourceBox.cs:210-248`, and `CityLandLord.cs:262-290`.
- Result: Reviewed 5 `Custom:Government System` rows. Classified 4 rows as `SafeNoChange` and 1 row as `ConfirmedIssue`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:49:12.5066183-05:00

- Affected phase: Post-audit `POST-BATCH-B-17A` `Custom:Government System` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 270 reviewed rows, 34 queued rows, and 5 `POST-BATCH-B-17A` rows; active backlog overlay now has 268 rows, including 251 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:51:58.8396348-05:00

- Affected phase: Post-audit `POST-BATCH-B-18A` `Custom:PandorasGiftBox` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `PandorasGiftBox.cs:75-86`.
- Result: Reviewed 1 `Custom:PandorasGiftBox` row. Classified the row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:51:58.8396348-05:00

- Affected phase: Post-audit `POST-BATCH-B-18A` `Custom:PandorasGiftBox` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 271 reviewed rows, 33 queued rows, and 1 `POST-BATCH-B-18A` row; active backlog overlay now has 269 rows, including 252 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T12:54:29.1482173-05:00

- Affected phase: Post-audit `POST-BATCH-B-19A` `Custom:Skill Stone` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `SkillStone.cs:168-199`.
- Result: Reviewed 1 `Custom:Skill Stone` row. Classified the row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T12:54:29.1482173-05:00

- Affected phase: Post-audit `POST-BATCH-B-19A` `Custom:Skill Stone` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 272 reviewed rows, 32 queued rows, and 1 `POST-BATCH-B-19A` row; active backlog overlay now has 270 rows, including 253 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:00:54.3537424-05:00

- Affected phase: Post-audit `POST-BATCH-B-20A` `Custom:Staff Toolbar [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Toolbar.cs:126-162`, `Toolbar.cs:164-240`, and `Toolbar.cs:313-329`.
- Result: Reviewed 1 `Custom:Staff Toolbar [2.0]` row. Classified the row as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:00:54.3537424-05:00

- Affected phase: Post-audit `POST-BATCH-B-20A` `Custom:Staff Toolbar [2.0]` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 273 reviewed rows, 31 queued rows, and 1 `POST-BATCH-B-20A` row; active backlog overlay now has 271 rows, including 254 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:04:45.9390281-05:00

- Affected phase: Post-audit `POST-BATCH-B-21A` `Custom:Voting` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `VoteItem.cs:167-203`, `VoteSite.cs:113-149`, and `VoteStone.cs:109-143`.
- Result: Reviewed 4 `Custom:Voting` rows. Classified 3 rows as `SafeNoChange` and 1 row as `FalsePositive`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:04:45.9390281-05:00

- Affected phase: Post-audit `POST-BATCH-B-21A` `Custom:Voting` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 277 reviewed rows, 27 queued rows, and 4 `POST-BATCH-B-21A` rows; active backlog overlay now has 275 rows, including 258 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:08:13.8438738-05:00

- Affected phase: Post-audit `POST-BATCH-B-22A` `Items:Armor` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseArmor.cs:1318-1430`, `BaseArmor.cs:1432-1725`, `LeatherGloves.cs:77-115`, and `LeatherNinjaMitts.cs:76-103`.
- Result: Reviewed 3 `Items:Armor` rows. Classified 2 rows as `IntentionalLegacy` and 1 row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:08:13.8438738-05:00

- Affected phase: Post-audit `POST-BATCH-B-22A` `Items:Armor` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 280 reviewed rows, 24 queued rows, and 3 `POST-BATCH-B-22A` rows; active backlog overlay now has 278 rows, including 261 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:11:10.2426654-05:00

- Affected phase: Post-audit `POST-BATCH-B-23A` `Items:Boats` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseDockedBoat.cs:71-102`.
- Result: Reviewed 1 `Items:Boats` row. Classified the row as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:11:10.2426654-05:00

- Affected phase: Post-audit `POST-BATCH-B-23A` `Items:Boats` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 281 reviewed rows, 23 queued rows, and 1 `POST-BATCH-B-23A` row; active backlog overlay now has 279 rows, including 262 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:14:26.7946669-05:00

- Affected phase: Post-audit `POST-BATCH-B-24A` `Items:Books` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseBook.cs:13-49`, `BaseBook.cs:175-295`, `PowerScroll.cs:370-380`, and `SpecialScroll.cs:13-20,341-373`.
- Result: Reviewed 4 `Items:Books` rows. Classified 2 rows as `IntentionalLegacy` and 2 rows as `FalsePositive`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:14:26.7946669-05:00

- Affected phase: Post-audit `POST-BATCH-B-24A` `Items:Books` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 285 reviewed rows, 19 queued rows, and 4 `POST-BATCH-B-24A` rows; active backlog overlay now has 283 rows, including 266 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:17:03.7614640-05:00

- Affected phase: Post-audit `POST-BATCH-B-25A` `Items:Clothing` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseClothing.cs:1009-1155`, `Cloaks.cs:123-160`, `OuterTorso.cs:585-622`, and `Shoes.cs:25-50,259-296`.
- Result: Reviewed 5 `Items:Clothing` rows. Classified 3 rows as `SafeNoChange` and 2 rows as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:17:03.7614640-05:00

- Affected phase: Post-audit `POST-BATCH-B-25A` `Items:Clothing` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 290 reviewed rows, 14 queued rows, and 5 `POST-BATCH-B-25A` rows; active backlog overlay now has 288 rows, including 271 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-07T13:24:13.0985293-05:00

- Affected phase: Post-audit `POST-BATCH-B-26A` `Items:Containers` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `FillableContainers.cs:247-296`.
- Result: Reviewed 1 `Items:Containers` row. Classified the row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:24:13.0985293-05:00

- Affected phase: Post-audit `POST-BATCH-B-26A` `Items:Containers` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility row to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 291 reviewed rows, 13 queued rows, and 1 `POST-BATCH-B-26A` row; active backlog overlay now has 289 rows, including 272 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-07T13:29:11.7318672-05:00

- Affected phase: Post-audit `POST-BATCH-B-27A` `Items:Deeds` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `DragonBardingDeed.cs:119-151`.
- Result: Reviewed 1 `Items:Deeds` row. Classified the row as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:29:11.7318672-05:00

- Affected phase: Post-audit `POST-BATCH-B-27A` `Items:Deeds` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 292 reviewed rows, 12 queued rows, and 1 `POST-BATCH-B-27A` row; active backlog overlay now has 290 rows, including 273 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-07T13:33:13.4111031-05:00

- Affected phase: Post-audit `POST-BATCH-B-28A` `Items:Food` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Beverage.cs:581-653,1419-1466` and `Food.cs:264-324`.
- Result: Reviewed 4 `Items:Food` rows. Classified 1 row as `FalsePositive`, 2 rows as `IntentionalLegacy`, and 1 row as `SafeNoChange`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:33:13.4111031-05:00

- Affected phase: Post-audit `POST-BATCH-B-28A` `Items:Food` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 296 reviewed rows, 8 queued rows, and 4 `POST-BATCH-B-28A` rows; active backlog overlay now has 294 rows, including 277 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-07T13:35:28.9172195-05:00

- Affected phase: Post-audit `POST-BATCH-B-29A` `Items:Weapons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BaseWeapon.cs:3738-4180` and `BaseRanged.cs:445-481`.
- Result: Reviewed 2 `Items:Weapons` rows. Classified both rows as `IntentionalLegacy`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:35:28.9172195-05:00

- Affected phase: Post-audit `POST-BATCH-B-29A` `Items:Weapons` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 298 reviewed rows, 6 queued rows, and 2 `POST-BATCH-B-29A` rows; active backlog overlay now has 296 rows, including 279 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-07T13:37:53.0802751-05:00

- Affected phase: Post-audit `POST-BATCH-B-30A` `Magic:Druidism` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `BlendWithForrestSpell.cs:123-147` and `GraspingRootsSpell.cs:116-127`.
- Result: Reviewed 2 `Magic:Druidism` rows. Classified both rows as `NeedsHumanDecision` because current effect serializers write payload that current deserializers do not consume; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-07T13:37:53.0802751-05:00

- Affected phase: Post-audit `POST-BATCH-B-30A` `Magic:Druidism` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append reviewed save-compatibility rows to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts with exact blocker evidence and next safe action.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 300 reviewed rows, 4 queued rows, and 2 `POST-BATCH-B-30A` rows; active backlog overlay now has 298 rows, including 281 active save-compatibility dispositions. Human decision required for `SERIAL-1298` and `SERIAL-1300` before source edits or further POST-BATCH-B review.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T01:42:04.9233477-05:00

- Affected phase: Post-audit `POST-BATCH-B-30B` `Magic:Druidism` transient-effect serializer fix
- Cwd: `D:\ConficturaUO`
- Command: Apply source fix to `BlendWithForrestSpell.cs` and `GraspingRootsSpell.cs` after human decision to consume existing duration payloads and clean up transient timed-effect items on load.
- Result: `BlendWithForrestSpell.InternalItem.Deserialize` now consumes the version 1 duration before owner/squelch reads and cleanup; `GraspingRootsSpell.InternalItem.Deserialize` now consumes the version 1 duration before deleting the transient helper item. Writer order, version values, namespaces, type names, and file locations were unchanged.
- Output path: `Data/Scripts/Magic/Druidism/Effects/BlendWithForrestSpell.cs`; `Data/Scripts/Magic/Druidism/Effects/GraspingRootsSpell.cs`

### 2026-06-08T01:42:04.9233477-05:00

- Affected phase: Post-audit `POST-BATCH-B-30B` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: `docs/codebase-audit/tools/New-SerializationRegister.ps1`; `MSBuild.exe Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; update triage, active overlay, status, README, and next-step artifacts.
- Result: Serialization outputs regenerated and now show the Druidism duration `ReadTimeSpan` calls; first MSBuild attempt was blocked by sandbox access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`, then escalated Visual Studio MSBuild passed; compile-only script verification passed and did not print `Listening:`; generated executable artifacts were restored; `post-batch-b-save-compatibility-triage.csv` remains 304 total rows, 300 reviewed rows, and 4 queued rows.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/outputs/serialization-register.csv`

### 2026-06-08T01:45:58.1124396-05:00

- Affected phase: Post-audit `POST-BATCH-B-31A` `Mobiles:Animals` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Data/Scripts/Mobiles/Animals/Mounts/Ethereals.cs:338-400`.
- Result: Reviewed 1 `Mobiles:Animals` row. Classified `SERIAL-1319` as `IntentionalLegacy` because the current version 3 payload is aligned and the extra version 1 `ReadInt` intentionally consumes an old payload; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-08T01:45:58.1124396-05:00

- Affected phase: Post-audit `POST-BATCH-B-31A` audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append `SERIAL-1319` to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 301 reviewed rows, 3 queued rows, and 1 `POST-BATCH-B-31A` row; active backlog overlay now has 299 rows, including 282 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T01:48:26.4410726-05:00

- Affected phase: Post-audit `POST-BATCH-B-32A` `Quests:Summon` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Data/Scripts/Quests/Summon/SummonPrison.cs:486-540`.
- Result: Reviewed 1 `Quests:Summon` row. Classified `SERIAL-1356` as `ConfirmedIssue` because `Serialize` writes `PrisonerFullNameUsed` and `PrisonerClothColorUsed` before `PrisonerSerial`, while `Deserialize` reads `PrisonerSerial` before those two integers; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-08T01:48:26.4410726-05:00

- Affected phase: Post-audit `POST-BATCH-B-32A` audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append `SERIAL-1356` to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 302 reviewed rows, 2 queued rows, and 1 `POST-BATCH-B-32A` row; active backlog overlay now has 300 rows, including 283 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T01:51:02.7822737-05:00

- Affected phase: Post-audit `POST-BATCH-B-33A` `System:Regions` serializer save-compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: Source review of `Data/Scripts/System/Regions/Spawning/SpawnPersistence.cs:39-70` and `Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:277-361`.
- Result: Reviewed 2 `System:Regions` rows. Classified `SERIAL-1494` and `SERIAL-1496` as `FalsePositive` because `SpawnEntry` is a helper serializer whose payload is consumed by `Deserialize(reader, version)` or `Remove(reader, version)` through `SpawnPersistence`; no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`

### 2026-06-08T01:51:02.7822737-05:00

- Affected phase: Post-audit `POST-BATCH-B-33A` audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Update `post-batch-b-save-compatibility-triage.csv`, append `SERIAL-1494` and `SERIAL-1496` to `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-b-save-compatibility-triage.csv` now has 304 total rows, 304 reviewed rows, 0 queued rows, and 2 `POST-BATCH-B-33A` rows; active backlog overlay now has 302 rows, including 285 active save-compatibility dispositions.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T01:54:18.0000000-05:00

- Affected phase: Post-audit `POST-BATCH-B` save compatibility closeout
- Cwd: `D:\ConficturaUO`
- Command: Review final triage and active overlay counts after all 304 P0 critical save-compatibility rows were reviewed.
- Result: `POST-BATCH-B` review is complete with 304 reviewed rows and 0 queued rows. Active overlay has 2 unresolved save `ConfirmedIssue` rows: `SERIAL-0032` and `SERIAL-1356`. `SERIAL-1356` requires a human save-policy decision before source repair, so `POST-BATCH-C` is blocked until the remaining save issues are fixed or explicitly deferred.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-closeout.md`

### 2026-06-08T14:34:52.8340561-05:00

- Affected phase: Post-audit `POST-BATCH-B-34A` focused save serializer source repair
- Cwd: `D:\ConficturaUO`
- Command: Apply source fixes to `Data/Scripts/Quests/Summon/SummonPrison.cs` and `Data/Scripts/Custom/Government System/Items/Stones/CityResurrectionStone.cs`.
- Result: `SummonPrison.Deserialize` now reads `PrisonerFullNameUsed`, `PrisonerClothColorUsed`, then `PrisonerSerial`, matching current writer order; `CityResurrectionStone.Serialize` now writes zero ghost entries when `m_ghosts` is null. Writer order for persisted fields, version numbers, serialized type names, namespaces, and file locations were unchanged.
- Output path: `Data/Scripts/Quests/Summon/SummonPrison.cs`; `Data/Scripts/Custom/Government System/Items/Stones/CityResurrectionStone.cs`

### 2026-06-08T14:34:52.8340561-05:00

- Affected phase: Post-audit `POST-BATCH-B-34A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-SerializationRegister.ps1`; `MSBuild.exe Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated executable artifacts; update triage, active overlay, status, README, next-step, and closeout artifacts.
- Result: Serialization outputs regenerated; `SummonPrison` no longer reports the prior Mobile-versus-Int type mismatch; Visual Studio MSBuild Debug/x86 server build passed; compile-only runtime script verification passed and did not print `Listening:`; generated executable artifacts were restored; active save `ConfirmedIssue` count is now 0 and `POST-BATCH-C` is unblocked but not started.
- Output path: `docs/codebase-audit/outputs/post-batch-b-save-compatibility-triage.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/outputs/post-batch-b-save-compatibility-closeout.md`

### 2026-06-08T15:41:13.9877742-05:00

- Affected phase: Post-audit `POST-BATCH-C-01A` runtime-hook and `PlayerMobile` coupling review
- Cwd: `D:\ConficturaUO`
- Command: Source/register review of the 17 P0 runtime-hook rows `RB-01877` through `RB-01893`, `SkillStone.cs:168-199`, `PlayerMobile.cs:4539-4937`, `PlayerMobile.cs:5004-5221`, `post-batch-a-packet-handler-review.csv`, `serialization-register.csv`, `dependency-graph.csv`, and `comment-target-register.csv`.
- Result: Reviewed 25 rows with no source edits. Runtime-hook rows reconcile to the earlier packet-handler review and fixes; `SkillStone` assigned-player serialization is source-aligned through Mobile object-reference persistence; `PlayerMobile` version 37 source review found the current fall-through mirrors `Serialize`, so the generated count mismatch remains extractor noise rather than a confirmed save-stream mismatch.
- Output path: `docs/codebase-audit/outputs/post-batch-c-runtime-hooks-player-mobile-review.csv`

### 2026-06-08T15:41:13.9877742-05:00

- Affected phase: Post-audit `POST-BATCH-C-01A` audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Create `post-batch-c-runtime-hooks-player-mobile-review.csv`, append/update 25 rows in `post-audit-active-backlog-status.csv`, and update status/readme/next-step artifacts.
- Result: `post-batch-c-runtime-hooks-player-mobile-review.csv` has 25 reviewed rows: 3 prior `Fixed`, 21 `ReviewedNoChange`, and 1 `SafeNoChange`; active backlog overlay now has 327 rows. No `Data` source file changed, and no row requires a migration plan or human decision from this review-only batch.
- Output path: `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T16:38:18.0434300-05:00

- Affected phase: Post-audit `POST-BATCH-D-01A` `Custom:Champions` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04689` through `RB-04696` in `Barracoon.cs`, `Ilhenir.cs`, `Rikktor.cs`, `Semidar.cs`, `Serado.cs`, `InterredGrizzle .cs`, and `PlagueBeastLord.cs`.
- Result: Replaced 8 direct `foreach` range scans with local `IPooledEnumerable` variables and `try/finally Free`, preserving loop filtering, break behavior, target collection, damage/message side effects, serialization, namespaces, type names, save versions, and file locations.
- Output path: `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Barracoon.cs`; `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Ilhenir.cs`; `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Rikktor.cs`; `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Semidar.cs`; `Data/Scripts/Custom/Champions/Mobiles/Champs-UO/Serado.cs`; `Data/Scripts/Custom/Champions/Mobiles/Minions/InterredGrizzle .cs`; `Data/Scripts/Custom/Champions/Mobiles/Minions/PlagueBeastLord.cs`

### 2026-06-08T16:38:18.0434300-05:00

- Affected phase: Post-audit `POST-BATCH-D-01A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Custom:Champions` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct `foreach` range scans in `Custom:Champions`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 335 unique rows including 8 `POST-BATCH-D-01A` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:34:27.2807478-05:00

- Affected phase: Post-audit `POST-BATCH-E-01A` Boats runtime-hook/gump-guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-02281`, `RB-02284`, `RB-02282`, `RB-02283`, `RB-03325`, `RB-03328`, `RB-03326`, and `RB-03327` across `Data/Scripts/Items/Boats/BaseBoat.cs`, `CarpetBuild.cs`, `Gumps/ConfirmDryDockGump.cs`, `PirateBounty.cs`, and `RenameBoatPrompt.cs`.
- Result: Added null/deleted response guards to passive boat gump/prompt handlers, added stale-state validation to `ConfirmDryDockGump`, delegated dry-dock completion through the responding mobile, and moved the `DockedBoat` null check before `boat.Hue` in `BaseBoat.EndDryDock`. Serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Items/Boats/BaseBoat.cs`; `Data/Scripts/Items/Boats/CarpetBuild.cs`; `Data/Scripts/Items/Boats/Gumps/ConfirmDryDockGump.cs`; `Data/Scripts/Items/Boats/PirateBounty.cs`; `Data/Scripts/Items/Boats/RenameBoatPrompt.cs`

### 2026-06-09T08:34:27.2807478-05:00

- Affected phase: Post-audit `POST-BATCH-E-01A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Boats hook/gump guard `rg` scans; failed PATH `msbuild` lookup; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-e-hooks-gumps-commands-regions-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scans confirmed the intended null/deleted/stale-state guards and existing Boats event/speech registrations; PATH `msbuild` was unavailable, so Visual Studio Community 2022 MSBuild was used and `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 18 `POST-BATCH-E-01A` Boats dispositions, with 8 `Fixed` and 10 `ReviewedNoChange`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:39:45.8011683-05:00

- Affected phase: Post-audit `POST-BATCH-E-02A` Bulk Orders Dragon shape-stone runtime-hook/gump-guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01894`, `RB-01895`, `RB-02208`, `RB-02544`, `RB-03252`, and `RB-03499` in `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/DragonShapeChangeStone-body.cs`.
- Result: Added null/deleted Mobile and Backpack guards to `DragonShapeShiftStone.OnSpeech`, added null/deleted/stale user and shift-stone guards to `BodyValueGump.OnResponse`, and guarded the context-menu gump send in `ChangeBodyValue.OnClick`. Serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/DragonShapeChangeStone-body.cs`

### 2026-06-09T08:39:45.8011683-05:00

- Affected phase: Post-audit `POST-BATCH-E-02A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Dragon shape-stone guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-02A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the intended speech/gump/context-menu guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 24 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:43:49.7334198-05:00

- Affected phase: Post-audit `POST-BATCH-E-02B` Bulk Orders BOB filter gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-02441`, `RB-03214`, `RB-03215`, `RB-03216`, `RB-03217`, `RB-03218`, `RB-03485`, `RB-04169`, `RB-04170`, `RB-04171`, `RB-04172`, and `RB-04173` in `Data/Scripts/Trades/Bulk Orders/Books/Gumps/BOBFilterGump.cs`.
- Result: Added NetState, PlayerMobile, stale responder, deleted book, and null active filter guards before `BOBFilterGump.OnResponse` mutates filter state or resends `BOBGump`/`BOBFilterGump`. Serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Trades/Bulk Orders/Books/Gumps/BOBFilterGump.cs`

### 2026-06-09T08:43:49.7334198-05:00

- Affected phase: Post-audit `POST-BATCH-E-02B` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted BOB filter guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-02B` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the intended response-entry guards before filter mutation and SendGump branches; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 36 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:49:56.5077033-05:00

- Affected phase: Post-audit `POST-BATCH-E-02C` Bulk Orders large/small BOD gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-02442`, `RB-03486`, `RB-02443`, `RB-03219`, `RB-03487`, `RB-04174`, `RB-02444`, `RB-03488`, `RB-02445`, `RB-03220`, `RB-03489`, and `RB-04175` across `Data/Scripts/Trades/Bulk Orders/Gumps/LargeBODAcceptGump.cs`, `LargeBODGump.cs`, `SmallBODAcceptGump.cs`, and `SmallBODGump.cs`.
- Result: Added null `NetState`, null/deleted or stale Mobile, deleted deed, missing backpack, and child-of-backpack guards before accept, cancel, combine, or gump resend branches. Serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Trades/Bulk Orders/Gumps`

### 2026-06-09T08:49:56.5077033-05:00

- Affected phase: Post-audit `POST-BATCH-E-02C` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted large/small BOD gump guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-02C` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the intended response-entry guards before accept, cancel, combine, and SendGump branches; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 48 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:53:45.8104115-05:00

- Affected phase: Post-audit `POST-BATCH-E-03A` Champions movement-hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01997` and `RB-01998` in `Data/Scripts/Custom/Champions/Mobiles/Minions/PlagueBeastLord.cs`.
- Result: Added null/deleted mover and deleted backpack guards before `PlagueBeastLord.OnMovement` calls `IsAccessibleTo` or `SendRemovePacket`. Valid-mover behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/Champions/Mobiles/Minions/PlagueBeastLord.cs`

### 2026-06-09T08:53:45.8104115-05:00

- Affected phase: Post-audit `POST-BATCH-E-03A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PlagueBeastLord movement-hook guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-03A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed mover/backpack guards before `SendRemovePacket`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 50 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T08:59:47.3063697-05:00

- Affected phase: Post-audit `POST-BATCH-E-04A` Clone Offline Player Characters login/logout hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01709` and `RB-01710` in `Data/Scripts/Custom/CloneOfflinePlayerCharacters/CloneOfflinePlayerCharacters.cs`.
- Result: Added null event args, null Mobile, and deleted Mobile guards before `OnLogout` clone creation and `OnLogin` clone deletion. Valid login/logout behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/CloneOfflinePlayerCharacters/CloneOfflinePlayerCharacters.cs`

### 2026-06-09T08:59:47.3063697-05:00

- Affected phase: Post-audit `POST-BATCH-E-04A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Clone Offline Player Characters login/logout hook guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-04A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed event registrations and null/deleted Mobile guards in `OnLogout` and `OnLogin`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 52 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:04:23.2376728-05:00

- Affected phase: Post-audit `POST-BATCH-E-05A` Crafting Core repair target/CraftGump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-03221` and `RB-04176` in `Data/Scripts/Trades/Core/Repair.cs`.
- Result: Added null/deleted targeter, missing craft-system, null target, and stale/deleted tool guards before repair processing and `CraftGump` resend. Valid repair target behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Trades/Core/Repair.cs`

### 2026-06-09T09:04:23.2376728-05:00

- Affected phase: Post-audit `POST-BATCH-E-05A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Repair.cs` guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-05A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed `OnTarget` entry guards and tool guards before `CraftGump` resend; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 54 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:08:45.1217761-05:00

- Affected phase: Post-audit `POST-BATCH-E-06A` Custom:AnimalSystem Faery shape-stone speech-hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01896` and `RB-01897` in `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FaeryShapeChangeStone.cs`.
- Result: Added null speech event, null/deleted Mobile, missing Backpack, and deleted Backpack guards before backpack checks and keyword handling. Valid backpack speech behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FaeryShapeChangeStone.cs`

### 2026-06-09T09:08:45.1217761-05:00

- Affected phase: Post-audit `POST-BATCH-E-06A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Faery shape-stone speech guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-06A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed speech-event/Mobile/Backpack guards before keyword handling and `base.OnSpeech`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 56 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:13:27.3774610-05:00

- Affected phase: Post-audit `POST-BATCH-E-07A` Custom:AnimalSystem Felinus shape-stone speech/gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01898`, `RB-01899`, `RB-02209`, `RB-02545`, `RB-03253`, and `RB-03500` in `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FelinusShapeChangeStone.cs`.
- Result: Added speech event/Mobile/Backpack guards before keyword handling, NetState/Mobile/stale-user/deleted-stone guards before hue response handling, and stored Mobile/stone guards before context-menu gump sends. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/FelinusShapeChangeStone.cs`

### 2026-06-09T09:13:27.3774610-05:00

- Affected phase: Post-audit `POST-BATCH-E-07A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Felinus shape-stone guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-07A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed speech, gump response, and context-menu send guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 62 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:18:35.1821639-05:00

- Affected phase: Post-audit `POST-BATCH-E-08A` Custom:AnimalSystem Rat shape-stone speech/gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01900`, `RB-01901`, `RB-02210`, `RB-02546`, `RB-03254`, and `RB-03501` in `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/RatShapeChangeStone.cs`.
- Result: Added speech event/Mobile/Backpack guards before keyword handling, NetState/Mobile/stale-user/deleted-stone guards before hue response handling, and stored Mobile/stone guards before context-menu gump sends. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/RatShapeChangeStone.cs`

### 2026-06-09T09:18:35.1821639-05:00

- Affected phase: Post-audit `POST-BATCH-E-08A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Rat shape-stone guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-08A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed speech, gump response, and context-menu send guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 68 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:24:43.3180757-05:00

- Affected phase: Post-audit `POST-BATCH-E-09A` Custom:AnimalSystem Wolven shape-stone speech/gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01902`, `RB-01903`, `RB-02211`, `RB-02547`, `RB-03255`, and `RB-03502` in `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/WolvenShapeChangeStone.cs`.
- Result: Added speech event/Mobile/Backpack guards before keyword handling, NetState/Mobile/stale-user/deleted-stone guards before hue response handling, and stored Mobile/stone guards before context-menu gump sends. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/AnimalSystem/ShapeShiftStones/WolvenShapeChangeStone.cs`

### 2026-06-09T09:24:43.3180757-05:00

- Affected phase: Post-audit `POST-BATCH-E-09A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Wolven shape-stone guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-09A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed speech, gump response, and context-menu send guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 74 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:33:31.0477793-05:00

- Affected phase: Post-audit `POST-BATCH-E-10A` Custom:Book Publisher `[2.0]` speech-hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01904`, `RB-01905`, and `RB-01906` in `Data/Scripts/Custom/Book Publisher [2.0]/Publisher.cs`.
- Result: Added null/deleted speaker guards before speech range routing, and null event/Mobile/deleted-Mobile guards before criminal checks and base speech handling. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/Book Publisher [2.0]/Publisher.cs`

### 2026-06-09T09:33:31.0477793-05:00

- Affected phase: Post-audit `POST-BATCH-E-10A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Book Publisher speech-hook guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-10A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed `HandlesOnSpeech` and `OnSpeech` null/deleted guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 77 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:39:06.6359982-05:00

- Affected phase: Post-audit `POST-BATCH-E-11A` Custom:NPC Control speech-event guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01711` in `Data/Scripts/Custom/NPC Control/StaffCommands/HearAll.cs`.
- Result: Added a deleted-speaker guard before formatting and broadcasting captured speech. Listener pruning, access checks, valid speech behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/NPC Control/StaffCommands/HearAll.cs`

### 2026-06-09T09:39:06.6359982-05:00

- Affected phase: Post-audit `POST-BATCH-E-11A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted NPC Control HearAll hook guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-11A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed EventSink.Speech registration, deleted-speaker guard, listener pruning guards, and command deleted-mobile guard; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 78 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:43:15.6368631-05:00

- Affected phase: Post-audit `POST-BATCH-E-12A` Custom:OrbRemoteServer command-hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01687`, `RB-01688`, and `RB-01689` in `Data/Scripts/Custom/OrbRemoteServer/UOArchitect/ClientCommands.cs`.
- Result: Added null/deleted command-user guards before the nudge commands move the caller, added null/deleted command-user guards before assigning the multi-remove target, and added a stale stored-mobile guard before target callback messaging and target reassignment. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/OrbRemoteServer/UOArchitect/ClientCommands.cs`

### 2026-06-09T09:43:15.6368631-05:00

- Affected phase: Post-audit `POST-BATCH-E-12A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted OrbRemoteServer ClientCommands guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-12A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed command registrations, command mobile guards, stale target-owner guard, and preserved movement/target sends; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 81 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:48:06.7426692-05:00

- Affected phase: Post-audit `POST-BATCH-E-13A` Custom:Voting command-hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01690` and `RB-01691` in `Data/Scripts/Custom/Voting/VoteCommand.cs`.
- Result: Added null command-event guards before Mobile/deleted checks and gump/vote actions in the Voting command handlers. The adjacent player `Vote` command received the same guard in the touched entry-point surface. Valid behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/Voting/VoteCommand.cs`

### 2026-06-09T09:48:06.7426692-05:00

- Affected phase: Post-audit `POST-BATCH-E-13A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Voting command guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-13A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Vote, VoteConfig, and VoteInstance registrations and command event/mobile/deleted guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 83 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:52:22.4314972-05:00

- Affected phase: Post-audit `POST-BATCH-E-14A` XMLSpawner command-register false-positive review
- Cwd: `D:\ConficturaUO`
- Command: Targeted source review of commented command-register rows for `RB-01692` through `RB-01698` in `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs` and `Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs`; append `POST-BATCH-E-14A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Classified seven marker-derived rows as `FalsePositive` because the matched `XmlSet`, `TagList`, `ItemAtt`, `MobAtt`, `DelAtt`, `TrigAtt`, and `AddAtt` `CommandSystem.Register` rows are inactive line comments. Active `TargetCommands` registrations remain for `XmlSet`, `AddAtt`, and `DelAtt`. No source files changed; source build and compile-only runtime script verification were not required for this review-only false-positive batch. Active backlog overlay now includes 90 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T09:57:13.2751853-05:00

- Affected phase: Post-audit `POST-BATCH-E-15A` System:Commands Crash command guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01699` in `Data/Scripts/System/Commands/CrashCommand.cs`.
- Result: Made pre-crash diagnostic logging tolerate null command events and null/deleted mobiles while preserving the intentional crash exception for every invocation. Valid Administrator crash-test behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/System/Commands/CrashCommand.cs`

### 2026-06-09T09:57:13.2751853-05:00

- Affected phase: Post-audit `POST-BATCH-E-15A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted CrashCommand guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-15A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Administrator command registration, null/deleted-safe diagnostic logging, and preserved intentional `InvalidOperationException` throw; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 91 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:04:00.0876526-05:00

- Affected phase: Post-audit `POST-BATCH-E-16A` System:Misc AutoRestart command guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01700` through `RB-01707` in `Data/Scripts/System/Misc/AutoRestart.cs`.
- Result: Added shared `GetCommandMobile` guard for null command events and null/deleted command users across `Restart`, `AR-On`, `AR-Off`, `AR-When`, `AR-Time`, `AR-Text`, `AR-Save`, and `AR-Load`; guarded the load target callback against null/deleted targeters; preserved valid restart scheduling, AutoRestart setting changes, backpack save behavior, load-target behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files.
- Output path: `Data/Scripts/System/Misc/AutoRestart.cs`

### 2026-06-09T10:04:00.0876526-05:00

- Affected phase: Post-audit `POST-BATCH-E-16A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AutoRestart command guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-16A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed command registrations, shared `GetCommandMobile` guard, guarded AutoRestart command handlers, and preserved restart scheduling/save/load target behavior; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 99 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:10:22.1373846-05:00

- Affected phase: Post-audit `POST-BATCH-E-17A` System:Misc world command guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01708` in `Data/Scripts/System/Misc/World.cs`.
- Result: Added null `CommandEventArgs`, null mobile, and deleted-mobile guard before `WhereWorld_OnCommand` reads the command user's map/location and sends the world-location message. Valid Administrator world-command behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/World.cs`

### 2026-06-09T10:10:22.1373846-05:00

- Affected phase: Post-audit `POST-BATCH-E-17A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted World command guard `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-17A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Administrator world command registration, null/deleted guard before `Worlds.GetMyWorld`, and preserved valid world-location response behavior; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 100 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:14:22.4498695-05:00

- Affected phase: Post-audit `POST-BATCH-E-18A` Offline Skill Training login/logout hook review and repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01712` through `RB-01715` in `Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs`.
- Result: Classified the constructor `EventSink.Login` and `EventSink.Logout` rows as false positives because both are line-commented inactive code. Added null event args, null mobile, deleted-mobile, and missing-backpack guards to active `OnLogin` and `OnLogout` before scanning the user's backpack for `StudyBook` instances. Valid login/logout training behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs`

### 2026-06-09T10:14:22.4498695-05:00

- Affected phase: Post-audit `POST-BATCH-E-18A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Offline Skill Training StudyBook hook `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-18A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed constructor `EventSink` rows are commented inactive, active `Initialize` registrations remain, and login/logout handlers now guard null/deleted/no-backpack mobiles; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 104 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:19:11.1303372-05:00

- Affected phase: Post-audit `POST-BATCH-E-19A` OmniAI stun-request invocation guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01716` in `Data/Scripts/Custom/OmniAI/OmniAI Shared.cs`.
- Result: Added null/deleted `m_Mobile` guard at the start of `UseWeaponStrike` before weapon skill checks, weapon ability assignment, or `EventSink.InvokeStunRequest`. Valid live-mobile weapon-strike behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Custom/OmniAI/OmniAI Shared.cs`

### 2026-06-09T10:19:11.1303372-05:00

- Affected phase: Post-audit `POST-BATCH-E-19A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted OmniAI `UseWeaponStrike` `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-19A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed null/deleted `m_Mobile` guard precedes `WeaponAbility.SetCurrentAbility` and `EventSink.InvokeStunRequest`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 105 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:24:09.8274394-05:00

- Affected phase: Post-audit `POST-BATCH-E-20A` Custom:PandorasGiftBox character-created/login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01717` and `RB-01718` in `Data/Scripts/Custom/PandorasGiftBox/PandorasGiftBox.cs`.
- Result: Added null event-args guards to `EventSink_CharacterCreated` and `EventSink_Login` before passing event mobiles to `TryGift`; `TryGift` now rejects deleted/no-account mobiles before account/backpack gift checks. Valid character-created/login gift behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Custom/PandorasGiftBox/PandorasGiftBox.cs`

### 2026-06-09T10:24:09.8274394-05:00

- Affected phase: Post-audit `POST-BATCH-E-20A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PandorasGiftBox hook `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-20A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed CharacterCreated/Login registrations, null event guards, and deleted/no-account `TryGift` guards before account/backpack access; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 107 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:28:18.0581904-05:00

- Affected phase: Post-audit `POST-BATCH-E-21A` Staff Toolbar login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01719` in `Data/Scripts/Custom/Staff Toolbar [2.0]/Toolbar.cs`.
- Result: Added null `LoginEventArgs`, null mobile, deleted-mobile, and missing-account guard before access-level checks, toolbar gump close, and `SendToolbar`/`ReadInfo` account access. Valid staff login toolbar behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Custom/Staff Toolbar [2.0]/Toolbar.cs`

### 2026-06-09T10:28:18.0581904-05:00

- Affected phase: Post-audit `POST-BATCH-E-21A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Staff Toolbar hook `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-21A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed login registration and null/deleted/no-account guard before `SendToolbar`/`ReadInfo` path; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 108 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:32:12.5581442-05:00

- Affected phase: Post-audit `POST-BATCH-E-22A` Custom:TellAFriend login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01720` in `Data/Scripts/Custom/TellAFriend/TellAFriend.cs`.
- Result: Added null login event, null mobile, deleted-mobile, and missing-account guard before referral tag checks and account reward logic. Valid login referral behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Custom/TellAFriend/TellAFriend.cs`

### 2026-06-09T10:32:12.5581442-05:00

- Affected phase: Post-audit `POST-BATCH-E-22A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted TellAFriend hook `rg` scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-22A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed login registration and null/deleted/no-account guard before `ToldAFriend`/`GotAFriend`/account reward logic; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 109 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:35:40.1743598-05:00

- Affected phase: Post-audit `POST-BATCH-E-23A` Custom:Voting config persistence hook review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01721` and `RB-01722` in `Data/Scripts/Custom/Voting/VoteConfig.cs`; update `post-batch-e-hooks-gumps-commands-regions-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Classified both rows `ReviewedNoChange`. `EventSink_WorldSave` serializes singleton-backed `VoteConfig.Instance` without dereferencing event args; `EventSink_ServerStarted` deserializes singleton-backed `VoteConfig.Instance` and has no event args. No source files changed; source build and compile-only runtime script verification were not required for this review-only batch. Active backlog overlay now includes 111 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:43:30.1202232-05:00

- Affected phase: Post-audit `POST-BATCH-E-24A` XMLSpawner runtime hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01723` through `RB-01728`; patch `Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs` and `Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs`.
- Result: Classified the commented QuestGumpRequest subscription as `FalsePositive`, classified XMLSpawner attachment WorldLoad/WorldSave persistence hooks as `ReviewedNoChange`, and added null event-args plus null/deleted mobile guards to XMLSpawner attachment speech/movement and points/challenge speech handlers. Valid XMLSpawner runtime behavior, attachment persistence, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Custom/XMLSpawner/XmlAttach/XmlAttach.cs`; `Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs`

### 2026-06-09T10:43:30.1202232-05:00

- Affected phase: Post-audit `POST-BATCH-E-24A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted XMLSpawner hook source review; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-24A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted review confirmed the QuestGumpRequest row is inactive, WorldLoad/WorldSave hooks intentionally avoid event-arg dereference, and the speech/movement hooks guard null event args plus null/deleted mobiles; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 117 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:51:31.2071582-05:00

- Affected phase: Post-audit `POST-BATCH-E-25A` Items:Doors door macro hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01731` in `Data/Scripts/Items/Doors/BaseDoor.cs`.
- Result: Added null `OpenDoorMacroEventArgs` and null/deleted mobile guards before direction, map, sector, LOS, and door `OnDoubleClick` processing. Valid door macro behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Doors/BaseDoor.cs`

### 2026-06-09T10:51:31.2071582-05:00

- Affected phase: Post-audit `POST-BATCH-E-25A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted BaseDoor hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-25A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed OpenDoorMacroUsed registration and null/deleted guard before map/sector access; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 118 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T10:56:13.8370017-05:00

- Affected phase: Post-audit `POST-BATCH-E-26A` Items:Food beverage login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01732` in `Data/Scripts/Items/Food/Beverage.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before drunk heave timer checks read BAC, map, or timer table state. Valid beverage BAC timer behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Food/Beverage.cs`

### 2026-06-09T10:56:13.8370017-05:00

- Affected phase: Post-audit `POST-BATCH-E-26A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Beverage login hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-26A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed EventSink.Login registration and null/deleted guard before BAC/map/timer access; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 119 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:01:12.4683505-05:00

- Affected phase: Post-audit `POST-BATCH-E-27A` Items:Gifts holiday speech hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01733` and `RB-01734` in `Data/Scripts/Items/Gifts/Holiday/Christmas/Commands/Merry Christmas.cs` and `Data/Scripts/Items/Gifts/Holiday/Halloween/TrickOrTreat.cs`.
- Result: Added null speech event, null/deleted speaker, missing backpack, null speech text, and null/deleted nearby-mobile guards before Christmas and Halloween gift-bag/vendor processing. Valid holiday gift behavior, pooled enumerable ownership, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Items/Gifts/Holiday/Christmas/Commands/Merry Christmas.cs`; `Data/Scripts/Items/Gifts/Holiday/Halloween/TrickOrTreat.cs`

### 2026-06-09T11:01:12.4683505-05:00

- Affected phase: Post-audit `POST-BATCH-E-27A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Items:Gifts speech hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-27A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Christmas and Halloween speech registrations and null/deleted/backpack/speech/mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 121 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:07:59.4473900-05:00

- Affected phase: Post-audit `POST-BATCH-E-28A` Housing hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01735`, `RB-01736`, `RB-01737`, `RB-01738`, `RB-01739`, `RB-01740`, `RB-01945`, and `RB-02173` across `Data/Scripts/Items/Houses/HouseFoundation.cs`, `Data/Scripts/Items/Houses/Monopoly/Gumps/Error Reporting/Errors.cs`, and `Data/Scripts/Items/Houses/Monopoly/Misc/General.cs`.
- Result: Added null/deleted event/mobile guards for house customization speech and Monopoly error login; added invalid TownHouse/TownHouseSign guard coverage for Monopoly save/start/login/speech paths including duplicate WorldSave and speech-delegation rows. Valid housing behavior, Monopoly error notification behavior, pooled enumerable ownership, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/Items/Houses/HouseFoundation.cs`; `Data/Scripts/Items/Houses/Monopoly/Gumps/Error Reporting/Errors.cs`; `Data/Scripts/Items/Houses/Monopoly/Misc/General.cs`

### 2026-06-09T11:07:59.4473900-05:00

- Affected phase: Post-audit `POST-BATCH-E-28A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Housing hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-28A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed HouseFoundation, Monopoly Errors, and Monopoly General registrations plus null/deleted/list-entry guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 129 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T16:42:46.5194703-05:00

- Affected phase: Post-audit `POST-BATCH-D-02A` `Custom:ClearDeckCommand` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04697` in `Data/Scripts/Custom/ClearDeckCommand/ClearDeckCommand.cs`.
- Result: Replaced the direct `foreach` over `playerBoat.GetItemsInRange(18)` with a local `IPooledEnumerable` and `try/finally Free`, preserving corpse filtering, boat matching, deletion behavior, serialization, namespaces, type names, save versions, and file location.
- Output path: `Data/Scripts/Custom/ClearDeckCommand/ClearDeckCommand.cs`

### 2026-06-08T16:42:46.5194703-05:00

- Affected phase: Post-audit `POST-BATCH-D-02A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `ClearDeckCommand` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scan in `ClearDeckCommand`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 336 unique rows including 9 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T16:50:54.6607029-05:00

- Affected phase: Post-audit `POST-BATCH-D-03A` `Custom:Government System` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04698` through `RB-04708` in `Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs` and `Data/Scripts/Custom/Government System/Vendor/CityLandLord.cs`.
- Result: Replaced 11 direct `foreach` range scans over spawner `GetMobilesInRange(0)` with local `IPooledEnumerable` variables and `try/finally Free`, preserving landlord lookup, initialization, vendor freeze/direction behavior, serialization, namespaces, type names, save versions, and file locations.
- Output path: `Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs`; `Data/Scripts/Custom/Government System/Vendor/CityLandLord.cs`

### 2026-06-08T16:50:54.6607029-05:00

- Affected phase: Post-audit `POST-BATCH-D-03A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Custom:Government System` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Custom:Government System`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 347 unique rows including 20 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T16:54:57.9847310-05:00

- Affected phase: Post-audit `POST-BATCH-D-04A` `Custom:Invasion System` `BaseSpecialCreature.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04709` through `RB-04711` in `Data/Scripts/Custom/Invasion System/Add Ins/BaseSpecialCreature.cs`.
- Result: Replaced 3 direct `foreach` range scans over `GetMobilesInRange` with local `IPooledEnumerable` variables and `try/finally Free`, preserving target collection, filtering, damage behavior, serialization, namespaces, type names, save versions, and file location.
- Output path: `Data/Scripts/Custom/Invasion System/Add Ins/BaseSpecialCreature.cs`

### 2026-06-08T16:54:57.9847310-05:00

- Affected phase: Post-audit `POST-BATCH-D-04A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `BaseSpecialCreature.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `BaseSpecialCreature.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 350 unique rows including 23 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T16:57:40.7346174-05:00

- Affected phase: Post-audit `POST-BATCH-D-05A` `Custom:Invasion System` `Lord BlackThorn Clone.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04712` in `Data/Scripts/Custom/Invasion System/Add Ins/Lord BlackThorn Clone.cs`.
- Result: Replaced the direct `foreach` range scan over `this.GetMobilesInRange(10)` with a local `IPooledEnumerable` and `try/finally Free`, preserving `RunicGolemInvader` count behavior, serialization, namespaces, type names, save versions, and file location.
- Output path: `Data/Scripts/Custom/Invasion System/Add Ins/Lord BlackThorn Clone.cs`

### 2026-06-08T16:57:40.7346174-05:00

- Affected phase: Post-audit `POST-BATCH-D-05A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Lord BlackThorn Clone.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Lord BlackThorn Clone.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 351 unique rows including 24 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:01:35.0112916-05:00

- Affected phase: Post-audit `POST-BATCH-D-06A` `Custom:Invasion System` `MobileFeatures.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04713` through `RB-04722` in `Data/Scripts/Custom/Invasion System/Add Ins/MobileFeatures.cs`.
- Result: Replaced 10 direct `foreach` range scans over `GetMobilesInRange` with local `IPooledEnumerable` variables and `try/finally Free`, preserving target collection, reveal/drain side effects, damage behavior, serialization, namespaces, type names, save versions, and file location.
- Output path: `Data/Scripts/Custom/Invasion System/Add Ins/MobileFeatures.cs`

### 2026-06-08T17:01:35.0112916-05:00

- Affected phase: Post-audit `POST-BATCH-D-06A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `MobileFeatures.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `MobileFeatures.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 361 unique rows including 34 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:05:09.8925191-05:00

- Affected phase: Post-audit `POST-BATCH-D-07A` `Custom:Invasion System` `PirateCaptain.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04723` and `RB-04724` in `Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateCaptain.cs`.
- Result: Replaced direct `foreach` range scans over `this.GetItemsInRange(200)` and `this.GetMobilesInRange(10)` with local `IPooledEnumerable` variables and `try/finally Free`; the touched nested pirate crew scans now also free through `finally`. Enemy-boat selection, pirate count behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateCaptain.cs`

### 2026-06-08T17:05:09.8925191-05:00

- Affected phase: Post-audit `POST-BATCH-D-07A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `PirateCaptain.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `PirateCaptain.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 363 unique rows including 36 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:09:23.4456433-05:00

- Affected phase: Post-audit `POST-BATCH-D-08A` `Custom:Invasion System` `SubChamps` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04725` through `RB-04733` in `Data/Scripts/Custom/Invasion System/Add Ins/SubChamps`.
- Result: Replaced 9 direct `foreach` range scans over `this.GetMobilesInRange(10)` with local `IPooledEnumerable` variables and `try/finally Free`, preserving helper-count thresholds, spawn behavior, serialization, namespaces, type names, save versions, and file locations.
- Output path: `Data/Scripts/Custom/Invasion System/Add Ins/SubChamps`

### 2026-06-08T17:09:23.4456433-05:00

- Affected phase: Post-audit `POST-BATCH-D-08A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `SubChamps` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `SubChamps`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 372 unique rows including 45 `POST-BATCH-D` fixed rows. `Custom:Invasion System` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:13:09.9375736-05:00

- Affected phase: Post-audit `POST-BATCH-D-09A` `Custom:OmniAI` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04734` through `RB-04737` in `Data/Scripts/Custom/OmniAI/OmniAI Core.cs` and `Data/Scripts/Custom/OmniAI/OmniAI Knightship.cs`.
- Result: Replaced 4 direct `foreach` range scans with local `IPooledEnumerable` variables and `try/finally Free`, preserving target filtering, corpse selection, cast detection, serialization, namespaces, type names, save versions, and file locations.
- Output path: `Data/Scripts/Custom/OmniAI/OmniAI Core.cs`; `Data/Scripts/Custom/OmniAI/OmniAI Knightship.cs`

### 2026-06-08T17:13:09.9375736-05:00

- Affected phase: Post-audit `POST-BATCH-D-09A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Custom:OmniAI` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Custom:OmniAI`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 376 unique rows including 49 `POST-BATCH-D` fixed rows. `Custom:OmniAI` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:16:10.5554764-05:00

- Affected phase: Post-audit `POST-BATCH-D-10A` `Custom:RandomEncounters` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04738` through `RB-04740` in `Data/Scripts/Custom/RandomEncounters/Timers.cs`.
- Result: Wrapped the assigned `GetClientsInRange` enumerable in `RandomEncounterCleanupTimer.MaybeRemove` with `try/finally Free`, preserving cleanup grace behavior, staff-player filtering, serialization, namespaces, type names, save versions, and file location.
- Output path: `Data/Scripts/Custom/RandomEncounters/Timers.cs`

### 2026-06-08T17:16:10.5554764-05:00

- Affected phase: Post-audit `POST-BATCH-D-10A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Timers.cs` ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted ownership check found the assigned enumerable paired with `finally`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 379 unique rows including 52 `POST-BATCH-D` fixed rows. `Custom:RandomEncounters` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:19:53.0225261-05:00

- Affected phase: Post-audit `POST-BATCH-D-11A` `Custom:XMLSpawner` `BaseXmlSpawner.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04741` through `RB-04743` in `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs`.
- Result: Replaced item/mobile `PLAYERSINRANGE` direct range scans with local `IPooledEnumerable` variables and `try/finally Free`; the sibling split-line `refobject` mobile scan in the same touched branch was also fixed. Player counting, keyword behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs`

### 2026-06-08T17:19:53.0225261-05:00

- Affected phase: Post-audit `POST-BATCH-D-11A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `BaseXmlSpawner.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `BaseXmlSpawner.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 382 unique rows including 55 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:24:41.7122501-05:00

- Affected phase: Post-audit `POST-BATCH-D-12A` `Custom:XMLSpawner` `XmlSpawner2.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04744` and `RB-04745` in `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs`.
- Result: Replaced `NearbyPlayerCount` and `OnTick` proximity direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Player counting, proximity trigger checks, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs`

### 2026-06-08T17:24:41.7122501-05:00

- Affected phase: Post-audit `POST-BATCH-D-12A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `XmlSpawner2.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `XmlSpawner2.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 384 unique rows including 57 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:28:14.2259511-05:00

- Affected phase: Post-audit `POST-BATCH-D-13A` `Custom:XMLSpawner` `XmlPoints.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04746` through `RB-04748` in `Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs`.
- Result: Replaced duel availability and duel return pet direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Occupied-duel-area early return, controlled pet collection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Custom/XMLSpawner/XmlAttachments/XmlPoints.cs`

### 2026-06-08T17:28:14.2259511-05:00

- Affected phase: Post-audit `POST-BATCH-D-13A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `XmlPoints.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `XmlPoints.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 387 unique rows including 60 `POST-BATCH-D` fixed rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:32:22.7946878-05:00

- Affected phase: Post-audit `POST-BATCH-D-14A` `Custom:XMLSpawner` PvP pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04749`, `RB-04750`, and `RB-04752` in XMLSpawner PvP challenge files; source review of `RB-04751` in `LastManStandingGauntlet.cs`.
- Result: Replaced live arena and hill direct range scans with local `IPooledEnumerable` variables and `try/finally Free`; classified `RB-04751` as `FalsePositive` because the reported loop is inside a disabled block comment. Arena clearing, hill scoring, non-participant collection, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/BaseChallengeGame.cs`; `Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/KingOfTheHillGauntlet.cs`; `Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGames/TeamKotHGauntlet.cs`

### 2026-06-08T17:32:22.7946878-05:00

- Affected phase: Post-audit `POST-BATCH-D-14A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Raw XMLSpawner PvP direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Raw PvP scan returned only `LastManStandingGauntlet.cs:350`, which is inside a disabled block comment; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 391 unique rows including 63 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Custom:XMLSpawner` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:37:35.5304910-05:00

- Affected phase: Post-audit `POST-BATCH-D-15A` `Items:Boats` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04753` through `RB-04759` in `Data/Scripts/Items/Boats`.
- Result: Replaced boat cleanup, ship proximity, enemy-ship lookup, shipwright/caster, docking lantern, and plank close direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Crew cleanup detection, obstacle counting, shipwright/caster counting, dock access checks, blocked-close early return, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Boats/BaseBoat.cs`; `Data/Scripts/Items/Boats/BoatBuild.cs`; `Data/Scripts/Items/Boats/CarpetBuild.cs`; `Data/Scripts/Items/Boats/DockingLantern.cs`; `Data/Scripts/Items/Boats/Plank.cs`

### 2026-06-08T17:37:35.5304910-05:00

- Affected phase: Post-audit `POST-BATCH-D-15A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Boats` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Boats`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 398 unique rows including 70 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Boats` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:44:48.3151266-05:00

- Affected phase: Post-audit `POST-BATCH-D-16A` `Items:Books` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04760` through `RB-04763` in `Data/Scripts/Items/Books/MerchantsBook.cs`.
- Result: Replaced merchant proximity, near-book validation, successful-purchase vendor speech, and insufficient-gold vendor speech direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Vendor eligibility detection, purchase validation, vendor speech behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Books/MerchantsBook.cs`

### 2026-06-08T17:44:48.3151266-05:00

- Affected phase: Post-audit `POST-BATCH-D-16A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Books` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Books`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 402 unique rows including 74 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Books` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:48:47.1288968-05:00

- Affected phase: Post-audit `POST-BATCH-D-17A` `Items:Containers` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04764` in `Data/Scripts/Items/Containers/FillableContainers.cs`.
- Result: Replaced the fillable container nearest-vendor direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Nearest vendor content selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Containers/FillableContainers.cs`

### 2026-06-08T17:48:47.1288968-05:00

- Affected phase: Post-audit `POST-BATCH-D-17A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Containers` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Containers`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 403 unique rows including 75 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Containers` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:52:09.6020300-05:00

- Affected phase: Post-audit `POST-BATCH-D-18A` `Items:Doors` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04765` through `RB-04768` in `Data/Scripts/Items/Doors`.
- Result: Replaced nearby door lock/unlock/open and near-gate direct item range scans with local `IPooledEnumerable` variables and `try/finally Free`. Door collection, gate detection early return, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Doors/BaseDoor.cs`; `Data/Scripts/Items/Doors/DoorOpener.cs`; `Data/Scripts/Items/Doors/GateMoon.cs`

### 2026-06-08T17:52:09.6020300-05:00

- Affected phase: Post-audit `POST-BATCH-D-18A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Doors` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Doors`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 407 unique rows including 79 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Doors` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T17:57:34.2698412-05:00

- Affected phase: Post-audit `POST-BATCH-D-19A` `Items:Explorers` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04769` through `RB-04773` in `Data/Scripts/Items/Explorers`.
- Result: Replaced bedroll, bedrolled-out, campfire, enemy-nearby, and camp-nearby direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Bedroll owner checks, rest collection, hostile-creature checks, active-camp early returns, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Explorers/Bedroll.cs`; `Data/Scripts/Items/Explorers/BedrolledOut.cs`; `Data/Scripts/Items/Explorers/Campfire.cs`; `Data/Scripts/Items/Explorers/Kindling.cs`

### 2026-06-08T17:57:34.2698412-05:00

- Affected phase: Post-audit `POST-BATCH-D-19A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Explorers` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Explorers`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 412 unique rows including 84 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Explorers` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:01:31.0667430-05:00

- Affected phase: Post-audit `POST-BATCH-D-20A` `Items:Gifts` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04774` and `RB-04775` in `Data/Scripts/Items/Gifts`.
- Result: Replaced Christmas and Halloween holiday speech-handler vendor direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Gift/treat selection, vendor responses, early returns, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Gifts/Holiday/Christmas/Commands/Merry Christmas.cs`; `Data/Scripts/Items/Gifts/Holiday/Halloween/TrickOrTreat.cs`

### 2026-06-08T18:01:31.0667430-05:00

- Affected phase: Post-audit `POST-BATCH-D-20A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Gifts` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Gifts`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 414 unique rows including 86 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive. `Items:Gifts` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:04:37.8036735-05:00

- Affected phase: Post-audit `POST-BATCH-D-21A` `Items:Houses` `TavernTable.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04776` through `RB-04779` in `Data/Scripts/Items/Houses/TavernTable.cs`.
- Result: Replaced patron removal, patron counting, lawn visitor detection, and shanty visitor detection direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Patron collection/deletion, count accumulation, visitor early returns, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Houses/TavernTable.cs`

### 2026-06-08T18:04:37.8036735-05:00

- Affected phase: Post-audit `POST-BATCH-D-21A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `TavernTable.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `TavernTable.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 418 unique rows including 90 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:08:15.0506421-05:00

- Affected phase: Post-audit `POST-BATCH-D-22A` `Items:Houses` `TownHouse.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04780` in `Data/Scripts/Items/Houses/Monopoly/Items/TownHouse.cs`.
- Result: Replaced the delete-time sign item direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Item visibility restoration, house cleanup, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Houses/Monopoly/Items/TownHouse.cs`

### 2026-06-08T18:08:15.0506421-05:00

- Affected phase: Post-audit `POST-BATCH-D-22A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `TownHouse.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `TownHouse.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 419 unique rows including 91 `POST-BATCH-D` fixed rows and 1 `POST-BATCH-D` false positive.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:11:13.8707965-05:00

- Affected phase: Post-audit `POST-BATCH-D-23A` `Items:Houses` `TownHouseSign.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04781` through `RB-04784`; source review of `RB-04785` in `Data/Scripts/Items/Houses/Monopoly/Items/TownHouseSign.cs`.
- Result: Replaced sign-hiding, item-bounds, convert-door, and unconvert-door direct range scans with local `IPooledEnumerable` variables and `try/finally Free`; classified `RB-04785` as `FalsePositive` because the reported loop is inside a disabled line-comment block. Sign visibility changes, converted item collection, door linking/relinking, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Houses/Monopoly/Items/TownHouseSign.cs`

### 2026-06-08T18:11:13.8707965-05:00

- Affected phase: Post-audit `POST-BATCH-D-23A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Raw `TownHouseSign.cs` range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Raw `TownHouseSign.cs` scan returned only the disabled comment hit at line 1412; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 424 unique rows including 95 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:14:22.5520682-05:00

- Affected phase: Post-audit `POST-BATCH-D-24A` `Items:Houses` `GumpResponse.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04786` in `Data/Scripts/Items/Houses/Monopoly/Misc/GumpResponse.cs`.
- Result: Replaced the nearby town-house direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Gump response validation flow, owner-house lookup, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Houses/Monopoly/Misc/GumpResponse.cs`

### 2026-06-08T18:14:22.5520682-05:00

- Affected phase: Post-audit `POST-BATCH-D-24A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `GumpResponse.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `GumpResponse.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 425 unique rows including 96 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:18:01.3699109-05:00

- Affected phase: Post-audit `POST-BATCH-D-25A` `Items:Houses` `Remodeling` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04787` through `RB-04792` in `Data/Scripts/Items/Houses/Remodeling`.
- Result: Replaced or completed pooled enumerable ownership for lawn/shanty house lookup and visitor cleanup scans with `try/finally Free`. Owner-house lookup early returns, visitor collection/deletion, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Houses/Remodeling/LawnGate.cs`; `Data/Scripts/Items/Houses/Remodeling/LawnItem.cs`; `Data/Scripts/Items/Houses/Remodeling/LawnSystem.cs`; `Data/Scripts/Items/Houses/Remodeling/ShantyDoor.cs`; `Data/Scripts/Items/Houses/Remodeling/ShantyItem.cs`; `Data/Scripts/Items/Houses/Remodeling/ShantySystem.cs`

### 2026-06-08T18:18:01.3699109-05:00

- Affected phase: Post-audit `POST-BATCH-D-25A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items/Houses/Remodeling` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Items/Houses/Remodeling`; explicit pooled-variable check showed matching `eable.Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 431 unique rows including 102 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Houses` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:21:22.2049742-05:00

- Affected phase: Post-audit `POST-BATCH-D-26A` `Items:Misc` `AcidSlime.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04793` in `Data/Scripts/Items/Misc/AcidSlime.cs`.
- Result: Replaced the damage target direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Transient hazard behavior, damage filtering, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Misc/AcidSlime.cs`

### 2026-06-08T18:21:22.2049742-05:00

- Affected phase: Post-audit `POST-BATCH-D-26A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `AcidSlime.cs` direct range-scan `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `AcidSlime.cs`; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 432 unique rows including 103 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:34:25.5065432-05:00

- Affected phase: Post-audit `POST-BATCH-D-27A` `Items:Misc` `MagicForges.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04794` through `RB-04801` in `Data/Scripts/Items/Misc/MagicForges.cs`.
- Result: Replaced serpent reward, dark core validation/enchantment, Golden Ranger, poison, cold, energy, and fire forge direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Reward, validation, item morphing, forge effect behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Misc/MagicForges.cs`

### 2026-06-08T18:34:25.5065432-05:00

- Affected phase: Post-audit `POST-BATCH-D-27A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `MagicForges.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `MagicForges.cs`; explicit pooled-variable check showed matching `Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 440 unique rows including 111 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:38:09.4106780-05:00

- Affected phase: Post-audit `POST-BATCH-D-28A` remaining `Items:Misc` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04802` through `RB-04805` in `Data/Scripts/Items/Misc/MorphItem.cs`, `PoolOfAcid.cs`, `WarningItem.cs`, and `Games/BaseBoard.cs`.
- Result: Replaced morph item refresh, acid pool damage collection, warning-neighbor propagation, and game-board client fan-out direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Movement trigger, transient hazard, warning propagation, packet release flow, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Misc/MorphItem.cs`; `Data/Scripts/Items/Misc/PoolOfAcid.cs`; `Data/Scripts/Items/Misc/WarningItem.cs`; `Data/Scripts/Items/Misc/Games/BaseBoard.cs`

### 2026-06-08T18:38:09.4106780-05:00

- Affected phase: Post-audit `POST-BATCH-D-28A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted four-file `Items:Misc` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the four touched `Items:Misc` files; explicit pooled-variable check showed matching `Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 444 unique rows including 115 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Misc` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:42:18.0141056-05:00

- Affected phase: Post-audit `POST-BATCH-D-29A` `Items:Potions` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04806` through `RB-04810` in `Data/Scripts/Items/Potions`.
- Result: Replaced alchemist counting, monster splatter counting, conflagration target collection, confusion blast effect, and frostbite target collection direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Potion targeting, effect application, transient potion behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Potions/Special/CanopicJar.cs`; `Data/Scripts/Items/Potions/Special/MonsterSplatter.cs`; `Data/Scripts/Items/Potions/Standard/Conflagration Potions/BaseConflagrationPotion.cs`; `Data/Scripts/Items/Potions/Standard/Confusion Blast Potions/BaseConfusionBlastPotion.cs`; `Data/Scripts/Items/Potions/Standard/Frostbite Potions/BaseFrostbitePotion.cs`

### 2026-06-08T18:42:18.0141056-05:00

- Affected phase: Post-audit `POST-BATCH-D-29A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Items:Potions` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the five touched `Items:Potions` files; explicit pooled-variable check showed matching `Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 449 unique rows including 120 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Potions` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:45:14.1137550-05:00

- Affected phase: Post-audit `POST-BATCH-D-30A` `Items:Special` `DemonPrison.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04811` in `Data/Scripts/Items/Special/DemonPrison.cs`.
- Result: Replaced the monster-splatter counting direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Melee-hit splatter behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Special/DemonPrison.cs`

### 2026-06-08T18:45:14.1137550-05:00

- Affected phase: Post-audit `POST-BATCH-D-30A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `DemonPrison.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `DemonPrison.cs`; explicit pooled-variable check showed a matching `Free` call; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 450 unique rows including 121 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Special` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:48:03.4942119-05:00

- Affected phase: Post-audit `POST-BATCH-D-31A` `Items:Technology` `Landmine.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04812` in `Data/Scripts/Items/Technology/Landmine.cs`.
- Result: Replaced the nearby landmine counting direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Landmine placement limits, harmful-region check, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Technology/Landmine.cs`

### 2026-06-08T18:48:03.4942119-05:00

- Affected phase: Post-audit `POST-BATCH-D-31A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Landmine.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Landmine.cs`; explicit pooled-variable check showed a matching `Free` call; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 451 unique rows including 122 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Technology` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:50:41.5106055-05:00

- Affected phase: Post-audit `POST-BATCH-D-32A` `Items:Trades` `RubyPickaxe.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04813` in `Data/Scripts/Items/Trades/Blacksmith Items/RubyPickaxe.cs`.
- Result: Replaced the nearby hydra detection direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Mining skill, hydra block, charge behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Trades/Blacksmith Items/RubyPickaxe.cs`

### 2026-06-08T18:50:41.5106055-05:00

- Affected phase: Post-audit `POST-BATCH-D-32A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `RubyPickaxe.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `RubyPickaxe.cs`; explicit pooled-variable check showed a matching `Free` call; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 452 unique rows including 123 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Trades` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T18:54:48.5479798-05:00

- Affected phase: Post-audit `POST-BATCH-D-33A` `Items:Traps` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04814` through `RB-04817` in `Data/Scripts/Items/Traps/FlameSpurtTrap.cs`, `SpikeTrap.cs`, `StoneFaceTrap.cs`, and `TrapKit.cs`.
- Result: Replaced flame spurt refresh, spike trap movement damage, stone face damage, and trap kit nearby trap counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Gem/jewel early return, player detection, damage application, nearby trap limit behavior, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Items/Traps/FlameSpurtTrap.cs`; `Data/Scripts/Items/Traps/SpikeTrap.cs`; `Data/Scripts/Items/Traps/StoneFaceTrap.cs`; `Data/Scripts/Items/Traps/TrapKit.cs`

### 2026-06-08T18:54:48.5479798-05:00

- Affected phase: Post-audit `POST-BATCH-D-33A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted four-file `Items:Traps` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the four touched trap files; explicit pooled-variable check showed matching `Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 456 unique rows including 127 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Traps` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:00:15.2280067-05:00

- Affected phase: Post-audit `POST-BATCH-D-34A` `Items:Weapons` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04818` through `RB-04820` in `Data/Scripts/Items/Weapons/BaseWeapon.cs`.
- Result: Replaced pack-instinct counting, mirror-image diversion, and area-attack target collection direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Pack bonus thresholds, mirror-image break behavior, area-target collection, empty-list return, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Items/Weapons/BaseWeapon.cs`

### 2026-06-08T19:00:15.2280067-05:00

- Affected phase: Post-audit `POST-BATCH-D-34A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `BaseWeapon.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `BaseWeapon.cs`; explicit pooled-variable check showed matching `Free` calls; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 459 unique rows including 130 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Items:Weapons` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:04:47.1924696-05:00

- Affected phase: Post-audit `POST-BATCH-D-35A` `Magic:Bard` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04821` through `RB-04831` in `Data/Scripts/Magic/Bard/Spells`.
- Result: Replaced 11 Bard song nearby target-collection direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Beneficial target filters, Magic Finale summoned/control-slot filters, song effects, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Bard/Spells/ArmysPaeonSong.cs`; `Data/Scripts/Magic/Bard/Spells/EnchantingEtudeSong.cs`; `Data/Scripts/Magic/Bard/Spells/EnergyCarolSong.cs`; `Data/Scripts/Magic/Bard/Spells/FireCarolSong.cs`; `Data/Scripts/Magic/Bard/Spells/IceCarolSong.cs`; `Data/Scripts/Magic/Bard/Spells/KnightsMinneSong.cs`; `Data/Scripts/Magic/Bard/Spells/MagesBalladSong.cs`; `Data/Scripts/Magic/Bard/Spells/MagicFinaleSong.cs`; `Data/Scripts/Magic/Bard/Spells/PoisonCarolSong.cs`; `Data/Scripts/Magic/Bard/Spells/SheepfoeMamboSong.cs`; `Data/Scripts/Magic/Bard/Spells/SinewyEtudeSong.cs`

### 2026-06-08T19:04:47.1924696-05:00

- Affected phase: Post-audit `POST-BATCH-D-35A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Bard/Spells` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Magic/Bard/Spells`; explicit pooled-variable check showed matching `Free` calls across the 11 touched spell files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 470 unique rows including 141 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Bard` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:10:10.9806326-05:00

- Affected phase: Post-audit `POST-BATCH-D-36A` `Magic:Bushido` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04832` in `Data/Scripts/Magic/Bushido/MomentumStrike.cs`.
- Result: Replaced the momentum target collection direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Target filtering, mana checks, damage transfer behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Magic/Bushido/MomentumStrike.cs`

### 2026-06-08T19:10:10.9806326-05:00

- Affected phase: Post-audit `POST-BATCH-D-36A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `MomentumStrike.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `MomentumStrike.cs`; explicit pooled-variable check showed a matching `Free` call; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 471 unique rows including 142 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Bushido` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:13:33.1988002-05:00

- Affected phase: Post-audit `POST-BATCH-D-37A` `Magic:Druidism` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04833` in `Data/Scripts/Magic/Druidism/Effects/TreefellowSpell.cs`.
- Result: Replaced the summoned treefellow nearby vortex cleanup direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Core.SE/Summoned guard, vortex filtering, dispel cleanup, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Magic/Druidism/Effects/TreefellowSpell.cs`

### 2026-06-08T19:13:33.1988002-05:00

- Affected phase: Post-audit `POST-BATCH-D-37A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `TreefellowSpell.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `TreefellowSpell.cs`; explicit pooled-variable check showed a matching `Free` call; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 472 unique rows including 143 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Druidism` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:18:16.3025306-05:00

- Affected phase: Post-audit `POST-BATCH-D-38A` `Magic:Elementalism` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04834` through `RB-04843` in `Data/Scripts/Magic/Elementalism`.
- Result: Replaced elemental mobile cleanup, elemental field collision, and Elemental Apocalypse target-collection direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Summoned guards, vortex filters, field target queueing, apocalypse harmful target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendAir.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendEarth.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendFire.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendWater.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritAir.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritEarth.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritFire.cs`; `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritWater.cs`; `Data/Scripts/Magic/Elementalism/Sphere 4/Elemental_Field.cs`; `Data/Scripts/Magic/Elementalism/Sphere 8/Elemental_Apocalypse.cs`

### 2026-06-08T19:18:16.3025306-05:00

- Affected phase: Post-audit `POST-BATCH-D-38A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Elementalism` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched `Magic:Elementalism` files; explicit pooled-variable check showed matching `Free` calls across the 10 touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 482 unique rows including 153 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Elementalism` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:24:31.4575885-05:00

- Affected phase: Post-audit `POST-BATCH-D-39A` `Magic:Jester` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04844` through `RB-04848` in `Data/Scripts/Magic/Jester`.
- Result: Replaced jester proximity, prank explosion, splatter-count, and Hilarity target direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Prank point checks, explosion target filters, splatter creation gates, Hilarity creature/non-creature filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Jester/BagOfTricks.cs`; `Data/Scripts/Magic/Jester/SummonedPrank.cs`; `Data/Scripts/Magic/Jester/Spells/FlowerPower.cs`; `Data/Scripts/Magic/Jester/Spells/Hilarity.cs`; `Data/Scripts/Magic/Jester/Spells/SeltzerBottle.cs`

### 2026-06-08T19:24:31.4575885-05:00

- Affected phase: Post-audit `POST-BATCH-D-39A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Jester` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Magic/Jester`; explicit pooled-variable check showed matching `Free` calls across the five touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 487 unique rows including 158 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Jester` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:30:25.3414101-05:00

- Affected phase: Post-audit `POST-BATCH-D-40A` `Magic:Knight` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04849` through `RB-04851` in `Data/Scripts/Magic/Knight`.
- Result: Replaced Dispel Evil, Holy Light, and Noble Sacrifice direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Harmful/beneficial target filters, Dispel Evil controlled-creature handling, Noble Sacrifice range TODO, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Knight/DispelEvil.cs`; `Data/Scripts/Magic/Knight/HolyLight.cs`; `Data/Scripts/Magic/Knight/NobleSacrifice.cs`

### 2026-06-08T19:30:25.3414101-05:00

- Affected phase: Post-audit `POST-BATCH-D-40A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Knight` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Magic/Knight`; explicit pooled-variable check showed matching `Free` calls across the three touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 490 unique rows including 161 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Knight` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:35:10.9398937-05:00

- Affected phase: Post-audit `POST-BATCH-D-41A` `Magic:Magery` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04852` through `RB-04854` in `Data/Scripts/Magic/Magery`.
- Result: Replaced Magic Trap, Fire Field, and Earthquake direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Trap counting, fire field target queueing, earthquake harmful target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Magery/Magery 2nd/MagicTrap.cs`; `Data/Scripts/Magic/Magery/Magery 4th/FireField.cs`; `Data/Scripts/Magic/Magery/Magery 8th/Earthquake.cs`

### 2026-06-08T19:35:10.9398937-05:00

- Affected phase: Post-audit `POST-BATCH-D-41A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Magery` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched `Magic:Magery` files; explicit pooled-variable check showed matching `Free` calls across the three touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 493 unique rows including 164 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Magery` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:40:46.3102526-05:00

- Affected phase: Post-audit `POST-BATCH-D-42A` `Magic:Misc` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04855` and `RB-04856` in `Data/Scripts/Magic/Misc`.
- Result: Replaced Summon Dragon and Summon Snakes direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Summoned guards, vortex filters, cleanup thresholds, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Misc/SummonDragonSpell.cs`; `Data/Scripts/Magic/Misc/SummonSnakesSpell.cs`

### 2026-06-08T19:40:46.3102526-05:00

- Affected phase: Post-audit `POST-BATCH-D-42A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Misc` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched `Magic:Misc` files; explicit pooled-variable check showed matching `Free` calls across the two touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 495 unique rows including 166 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Misc` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:44:46.7075578-05:00

- Affected phase: Post-audit `POST-BATCH-D-43A` `Magic:Necromancy` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04857` and `RB-04858` in `Data/Scripts/Magic/Necromancy`.
- Result: Replaced Poison Strike and Wither direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Harmful target filters, splash damage collection, wither target collection, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Necromancy/PoisonStrike.cs`; `Data/Scripts/Magic/Necromancy/Wither.cs`

### 2026-06-08T19:44:46.7075578-05:00

- Affected phase: Post-audit `POST-BATCH-D-43A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Necromancy` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched `Magic:Necromancy` files; explicit pooled-variable check showed matching `Free` calls across the two touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 497 unique rows including 168 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Necromancy` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:49:22.3307872-05:00

- Affected phase: Post-audit `POST-BATCH-D-44A` `Magic:Research` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04859` through `RB-04867` in `Data/Scripts/Magic/Research/Spells`.
- Result: Replaced Death, Enchanting, Sorcery, and Wizardry research spell direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Corpse selection, nearby hazard counts, beneficial target collection, field target queueing, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Magic/Research/Spells/Death/ResearchDeathSpeak.cs`; `Data/Scripts/Magic/Research/Spells/Death/ResearchOpenGround.cs`; `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchMassMight.cs`; `Data/Scripts/Magic/Research/Spells/Sorcery/ResearchConflagration.cs`; `Data/Scripts/Magic/Research/Spells/Sorcery/ResearchCreateFire.cs`; `Data/Scripts/Magic/Research/Spells/Sorcery/ResearchEndureCold.cs`; `Data/Scripts/Magic/Research/Spells/Sorcery/ResearchEndureHeat.cs`; `Data/Scripts/Magic/Research/Spells/Sorcery/ResearchRingofFire.cs`; `Data/Scripts/Magic/Research/Spells/Wizardry/ResearchFrostField.cs`

### 2026-06-08T19:49:22.3307872-05:00

- Affected phase: Post-audit `POST-BATCH-D-44A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Magic/Research/Spells` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Magic/Research/Spells`; explicit pooled-variable check showed matching `Free` calls across the nine touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 506 unique rows including 177 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Magic:Research` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:54:20.6458254-05:00

- Affected phase: Post-audit `POST-BATCH-D-45A` `Mobiles:Animals` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04868` and `RB-04869` in `Data/Scripts/Mobiles/Animals`.
- Result: Replaced Infected and Stirge direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Splatter counting, drain target filters, serialization, namespaces, type names, save versions, and file locations were preserved.
- Output path: `Data/Scripts/Mobiles/Animals/Misc/Infected.cs`; `Data/Scripts/Mobiles/Animals/Rodents/Stirge.cs`

### 2026-06-08T19:54:20.6458254-05:00

- Affected phase: Post-audit `POST-BATCH-D-45A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Animals` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched `Mobiles:Animals` files; explicit pooled-variable check showed matching `Free` calls across the two touched files; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 508 unique rows including 179 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Animals` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T19:58:58.4143400-05:00

- Affected phase: Post-audit `POST-BATCH-D-46A` `Mobiles:Base` `BaseCreature.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04870` through `RB-04874` in `Data/Scripts/Mobiles/Base/BaseCreature.cs`.
- Result: Replaced breath splash, team-size, boat-link, rummage, and pet-teleport direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target filters, early break behavior, boat-link keep/delete behavior, pet selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Base/BaseCreature.cs`

### 2026-06-08T19:58:58.4143400-05:00

- Affected phase: Post-audit `POST-BATCH-D-46A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `BaseCreature.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `BaseCreature.cs`; explicit pooled-variable check showed matching `Free` calls across the five touched loops; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 513 unique rows including 184 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `BaseCreature.cs` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:02:40.1354010-05:00

- Affected phase: Post-audit `POST-BATCH-D-47A` `Mobiles:Base` `BaseVendor.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04875` and `RB-04876` in `Data/Scripts/Mobiles/Base/BaseVendor.cs`.
- Result: Replaced vendor action and player-sight direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Vendor action object handling, visible player counting, enemy detection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Base/BaseVendor.cs`

### 2026-06-08T20:02:40.1354010-05:00

- Affected phase: Post-audit `POST-BATCH-D-47A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `BaseVendor.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `BaseVendor.cs`; explicit pooled-variable check showed matching `Free` calls across the two touched loops; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 515 unique rows including 186 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `BaseVendor.cs` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:10:26.5074828-05:00

- Affected phase: Post-audit `POST-BATCH-D-48A` `Mobiles:Base` `Behavior.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04877` through `RB-04885` in `Data/Scripts/Mobiles/Base/Behavior.cs`.
- Result: Replaced AI summon-count, cleanup, proximity, friend-guard, aggressor, marching-order, searching, need-finding, and dispel direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target filters, continue paths, combatant scoring, hidden-player detection math, need priority, dispel priority, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Base/Behavior.cs`

### 2026-06-08T20:10:26.5074828-05:00

- Affected phase: Post-audit `POST-BATCH-D-48A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Behavior.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Behavior.cs`; explicit pooled-variable check showed matching `Free` calls across the nine touched loops; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 524 unique rows including 195 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Behavior.cs` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:15:02.7910996-05:00

- Affected phase: Post-audit `POST-BATCH-D-49A` `Mobiles:Base` `PlayerMobile.cs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04886` and `RB-04887` in `Data/Scripts/Mobiles/Base/PlayerMobile.cs`.
- Result: Replaced staff-message client and enemy notoriety direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Packet acquisition/release, staff access filters, enemy notoriety update packets, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Base/PlayerMobile.cs`

### 2026-06-08T20:15:02.7910996-05:00

- Affected phase: Post-audit `POST-BATCH-D-49A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `PlayerMobile.cs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `PlayerMobile.cs`; explicit pooled-variable check showed matching `Free` calls across the two touched loops; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 526 unique rows including 197 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `PlayerMobile.cs` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:24:55.1993612-05:00

- Affected phase: Post-audit `POST-BATCH-D-50A` `Mobiles:Civilized` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04888` through `RB-04910` across `Data/Scripts/Mobiles/Civilized`.
- Result: Replaced tradesman, training, working-spot, familiar, and pack-beast direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Work actions, cleanup lists, familiar filters, pack-beast eligibility checks, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Civilized`

### 2026-06-08T20:24:55.1993612-05:00

- Affected phase: Post-audit `POST-BATCH-D-50A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Civilized` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Civilized`; explicit pooled-variable check showed matching `Free` calls across the 23 touched loops; CSV consistency parse reported 222 review rows, 220 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 549 unique rows including 220 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Civilized` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:30:55.7470141-05:00

- Affected phase: Post-audit `POST-BATCH-D-51A` `Mobiles:Constructs` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04911` through `RB-04913` in `Data/Scripts/Mobiles/Constructs/WaxSculpture.cs`, `Data/Scripts/Mobiles/Constructs/Alien/Mutant.cs`, and `Data/Scripts/Mobiles/Constructs/Golems/IronCobra.cs`.
- Result: Replaced WaxSculpture stamina-drain targeting, Mutant toxic blood counting, and IronCobra stone-effect targeting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Constructs`

### 2026-06-08T20:30:55.7470141-05:00

- Affected phase: Post-audit `POST-BATCH-D-51A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Constructs` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Constructs`; explicit pooled-variable check showed matching `Free` calls across the three touched loops; CSV consistency parse reported 225 review rows, 223 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 552 unique rows including 223 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Constructs` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:35:44.6047175-05:00

- Affected phase: Post-audit `POST-BATCH-D-52A` `Mobiles:Daemons` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04914` through `RB-04919` in `Data/Scripts/Mobiles/Daemons/Balron.cs`, `BloodDemigod.cs`, `BloodDemon.cs`, `Daemon.cs`, and `Succubus.cs`.
- Result: Replaced monster-splatter counting, demon-gate hiding, and drain-life target direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Splatter selection, gate visibility handling, drain-life target filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Daemons`

### 2026-06-08T20:35:44.6047175-05:00

- Affected phase: Post-audit `POST-BATCH-D-52A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Daemons` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Daemons`; explicit pooled-variable check showed matching `Free` calls across the six touched loops; CSV consistency parse reported 231 review rows, 229 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 558 unique rows including 229 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Daemons` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:40:08.4278926-05:00

- Affected phase: Post-audit `POST-BATCH-D-53A` `Mobiles:Dragons` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04920` through `RB-04925` across `Data/Scripts/Mobiles/Dragons`.
- Result: Replaced dragon monster-splatter counting and VampiricDragon drain-life target direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Splatter selection, drain-life target filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Dragons`

### 2026-06-08T20:40:08.4278926-05:00

- Affected phase: Post-audit `POST-BATCH-D-53A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Dragons` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Dragons`; explicit pooled-variable check showed matching `Free` calls across the six touched loops; CSV consistency parse reported 237 review rows, 235 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 564 unique rows including 235 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Dragons` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:44:53.7189983-05:00

- Affected phase: Post-audit `POST-BATCH-D-54A` `Mobiles:Elementals` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04926` through `RB-04933` across `Data/Scripts/Mobiles/Elementals`.
- Result: Replaced MudMan mud choke, StormCloud storm hit, and elemental monster-splatter direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target filters, splatter selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Elementals`

### 2026-06-08T20:44:53.7189983-05:00

- Affected phase: Post-audit `POST-BATCH-D-54A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Elementals` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Elementals`; explicit pooled-variable check showed matching `Free` calls across the eight touched loops; CSV consistency parse reported 245 review rows, 243 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 572 unique rows including 243 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Elementals` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:49:02.4111547-05:00

- Affected phase: Post-audit `POST-BATCH-D-55A` `Mobiles:Hellish` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04934` in `Data/Scripts/Mobiles/Hellish/ShadowFiend.cs`.
- Result: Replaced the hidden-player reveal direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Reveal filters, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Hellish/ShadowFiend.cs`

### 2026-06-08T20:49:02.4111547-05:00

- Affected phase: Post-audit `POST-BATCH-D-55A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Hellish` direct range-scan `rg`; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Hellish`; explicit pooled-variable check showed a matching `Free` call; CSV consistency parse reported 246 review rows, 244 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 573 unique rows including 244 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Hellish` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T20:55:22.6323043-05:00

- Affected phase: Post-audit `POST-BATCH-D-56A` `Mobiles:Humanoids` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04935` through `RB-04946` across `Data/Scripts/Mobiles/Humanoids`.
- Result: Replaced stone, spawn-count, provoke, sailor, savage dance, and splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Qualified and unqualified `Get*InRange` calls were handled; target filters, list-building behavior, splatter selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Humanoids`

### 2026-06-08T20:55:22.6323043-05:00

- Affected phase: Post-audit `POST-BATCH-D-56A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Humanoids` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Humanoids`; explicit pooled-variable check showed matching `Free` calls across the 12 touched loops; CSV consistency parse reported 258 review rows, 256 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 585 unique rows including 256 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Humanoids` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:04:32.6661737-05:00

- Affected phase: Post-audit `POST-BATCH-D-57A` `Mobiles:Insects` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04947` through `RB-04966` across `Data/Scripts/Mobiles/Insects`.
- Result: Replaced insect, alien, and antaur splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Splatter selection, before-death effect gating, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Insects`

### 2026-06-08T21:04:32.6661737-05:00

- Affected phase: Post-audit `POST-BATCH-D-57A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Insects` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Insects`; explicit pooled-variable check showed matching `Free` calls across the 20 touched loops; CSV consistency parse reported 278 review rows, 276 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 605 unique rows including 276 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Insects` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:08:37.3029522-05:00

- Affected phase: Post-audit `POST-BATCH-D-58A` `Mobiles:Mystical` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04967` through `RB-04975` across `Data/Scripts/Mobiles/Mystical`.
- Result: Replaced sphinx stone-target, dryad peace/undress, and satyr provoke direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Petrification target collection, dryad target filters, provoke filters and first-target break behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Mystical`

### 2026-06-08T21:08:37.3029522-05:00

- Affected phase: Post-audit `POST-BATCH-D-58A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Mystical` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Mystical`; explicit pooled-variable check showed matching `Free` calls across the nine touched loops; CSV consistency parse reported 287 review rows, 285 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 614 unique rows including 285 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Mystical` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:13:14.0024412-05:00

- Affected phase: Post-audit `POST-BATCH-D-59A` `Mobiles:Plants` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04976` through `RB-04981` across `Data/Scripts/Mobiles/Plants`.
- Result: Replaced drain-life target, bogling collection, and plant splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target collection, bogling deletion order, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Plants`

### 2026-06-08T21:13:14.0024412-05:00

- Affected phase: Post-audit `POST-BATCH-D-59A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Plants` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Plants`; explicit pooled-variable check showed matching `Free` calls across the six touched loops; CSV consistency parse reported 293 review rows, 291 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 620 unique rows including 291 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Plants` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:17:41.4935079-05:00

- Affected phase: Post-audit `POST-BATCH-D-60A` `Mobiles:Reptilian` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04982` through `RB-04988` across `Data/Scripts/Mobiles/Reptilian`.
- Result: Replaced stone, poison, teleport, shock, drain-life, and splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target collection, first valid teleport target selection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Reptilian`

### 2026-06-08T21:17:41.4935079-05:00

- Affected phase: Post-audit `POST-BATCH-D-60A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Reptilian` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Reptilian`; explicit pooled-variable check showed matching `Free` calls across the seven touched loops; CSV consistency parse reported 300 review rows, 298 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 627 unique rows including 298 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Reptilian` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:21:30.2015762-05:00

- Affected phase: Post-audit `POST-BATCH-D-61A` `Mobiles:Slimes` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04989` through `RB-04993` across `Data/Scripts/Mobiles/Slimes`.
- Result: Replaced drain-life, oil-burn, and splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target collection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Slimes`

### 2026-06-08T21:21:30.2015762-05:00

- Affected phase: Post-audit `POST-BATCH-D-61A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Slimes` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Slimes`; explicit pooled-variable check showed matching `Free` calls across the five touched loops; CSV consistency parse reported 305 review rows, 303 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 632 unique rows including 303 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Slimes` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:25:54.4630498-05:00

- Affected phase: Post-audit `POST-BATCH-D-62A` `Mobiles:Summoned` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-04994` through `RB-05000` across `Data/Scripts/Mobiles/Summoned`.
- Result: Replaced random target acquisition and summoned cleanup direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Random filters, target list damage scaling, cleanup list trimming behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Summoned`

### 2026-06-08T21:25:54.4630498-05:00

- Affected phase: Post-audit `POST-BATCH-D-62A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Summoned` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Summoned`; explicit pooled-variable check showed matching `Free` calls across the seven touched loops; CSV consistency parse reported 312 review rows, 310 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 639 unique rows including 310 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Summoned` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:30:31.4849277-05:00

- Affected phase: Post-audit `POST-BATCH-D-63A` `Mobiles:Undead` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05001` through `RB-05012` across `Data/Scripts/Mobiles/Undead`.
- Result: Replaced spawn-count, drain-life, and splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Spawn thresholds, drain-life target collection, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Undead`

### 2026-06-08T21:30:31.4849277-05:00

- Affected phase: Post-audit `POST-BATCH-D-63A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Undead` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Undead`; explicit pooled-variable check showed matching `Free` calls across the 12 touched loops; CSV consistency parse reported 324 review rows, 322 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 651 unique rows including 322 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Undead` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:41:33.8587906-05:00

- Affected phase: Post-audit `POST-BATCH-D-64A` `Mobiles:Unique` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05013` through `RB-05028` across `Data/Scripts/Mobiles/Unique`.
- Result: Replaced death-gate, spawn-count, and splatter-counting direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Quest item checks, reward effects, Titan kill-permission fallback checks, spawn thresholds, splatter-counting behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Unique`

### 2026-06-08T21:41:33.8587906-05:00

- Affected phase: Post-audit `POST-BATCH-D-64A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Unique` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Unique`; explicit pooled-variable check showed matching `Free` calls across the 16 touched loops; CSV consistency parse reported 340 review rows, 338 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 667 unique rows including 338 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Unique` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:47:31.4004585-05:00

- Affected phase: Post-audit `POST-BATCH-D-65A` `Mobiles:Unusual` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05029` through `RB-05039` across `Data/Scripts/Mobiles/Unusual`.
- Result: Replaced drain-life, petrification, teleport-target, splatter-counting, nearby-attacker, and gold-carrier direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Target filters, first valid teleport target selection, splatter-counting behavior, nearby-attacker detection, gold-carrier selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Mobiles/Unusual`

### 2026-06-08T21:47:31.4004585-05:00

- Affected phase: Post-audit `POST-BATCH-D-65A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Mobiles/Unusual` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `Mobiles/Unusual`; explicit pooled-variable check showed matching `Free` calls across the 11 touched loops; CSV consistency parse reported 351 review rows, 349 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 678 unique rows including 349 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Mobiles:Unusual` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T21:57:00.4564829-05:00

- Affected phase: Post-audit `POST-BATCH-D-66A` quest pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05040` through `RB-05050` across `Data/Scripts/Quests`.
- Result: Replaced power-coil, hoard, nearby-monster, Balinor blocker, PremiumSpawner, town-detection, and rune-gate direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Proximity checks, target collection, location override, town detection, rune gate collection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Quests`

### 2026-06-08T21:57:00.4564829-05:00

- Affected phase: Post-audit `POST-BATCH-D-66A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted quest direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched quest files; explicit pooled-variable check showed matching `Free` calls across the 11 touched loops; CSV consistency parse reported 362 review rows, 360 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 689 unique rows including 360 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. Quest queued pooled-enumerable rows through `RB-05050` are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:03:27.7088717-05:00

- Affected phase: Post-audit `POST-BATCH-D-67A` `System:Commands` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05051` through `RB-05054` across `Data/Scripts/System/Commands`.
- Result: Replaced banker-spawner, client broadcast, corpse search, and moon gate search direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Banker detection, packet visibility checks, nearest-corpse selection, moon gate selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/System/Commands`

### 2026-06-08T22:03:27.7088717-05:00

- Affected phase: Post-audit `POST-BATCH-D-67A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `System:Commands` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched command files; explicit pooled-variable check showed matching `Free` calls across the four touched loops; CSV consistency parse reported 366 review rows, 364 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 693 unique rows including 364 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `System:Commands` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:08:49.5055724-05:00

- Affected phase: Post-audit `POST-BATCH-D-68A` `System:Misc` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05055` through `RB-05061` across `Data/Scripts/System/Misc`.
- Result: Replaced death healer/shrine, party staff-message, PremiumSpawner activation, boat detection, and boat-town proximity direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Healer/shrine selection, staff listener filtering, packet send behavior, spawner activation, boat detection, town proximity early return, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/System/Misc`

### 2026-06-08T22:08:49.5055724-05:00

- Affected phase: Post-audit `POST-BATCH-D-68A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `System:Misc` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; CSV consistency parse; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched misc files; explicit pooled-variable check showed matching `Free` calls across the seven touched loops; CSV consistency parse reported 373 review rows, 371 fixed rows, 2 false positives, no missing reviewed fields, and no duplicate backlog IDs; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 700 unique rows including 371 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `System:Misc` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:19:47.7829353-05:00

- Affected phase: Post-audit `POST-BATCH-D-69A` `System:Obsolete` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05062` through `RB-05074` across `Data/Scripts/System/Obsolete/Obsolete.cs` and `Data/Scripts/System/Obsolete/Sphinx.cs`.
- Result: Replaced faction trap, Holy Bless/Curse, Ethics ankh, faction existence, glowing-goo splatter, dispel-target, TurnStone, and SalesBook direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Placement rejection, target filtering, early return/break behavior, dispel prioritization, target list behavior, vendor speech gating, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/System/Obsolete/Obsolete.cs`; `Data/Scripts/System/Obsolete/Sphinx.cs`

### 2026-06-08T22:19:47.7829353-05:00

- Affected phase: Post-audit `POST-BATCH-D-69A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `System:Obsolete` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; `git diff --check`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched obsolete files; explicit pooled-variable check showed matching `Free` calls across the 13 touched loops; `git diff --check` reported only expected line-ending warnings; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 713 unique rows including 384 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `System:Obsolete` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:26:22.1572402-05:00

- Affected phase: Post-audit `POST-BATCH-D-70A` `System:Regions` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05075` in `Data/Scripts/System/Regions/GuardedRegion.cs`.
- Result: Replaced the direct guard-candidate neighbor range scan with a local `IPooledEnumerable` variable and `try/finally Free`. Guard-candidate timing, nearest fake guard-caller selection, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/System/Regions/GuardedRegion.cs`

### 2026-06-08T22:26:22.1572402-05:00

- Affected phase: Post-audit `POST-BATCH-D-70A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `System:Regions` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; `git diff --check`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `GuardedRegion.cs`; explicit pooled-variable check showed matching `Free` calls; `git diff --check` reported only expected line-ending warnings; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 714 unique rows including 385 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `System:Regions` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:30:50.7895053-05:00

- Affected phase: Post-audit `POST-BATCH-D-71A` `System:Skills` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05076` through `RB-05085` across `Data/Scripts/System/Skills`.
- Result: Replaced hiding, peacemaking, spiritualism, stealing, tracking, and weapon ability direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Combat detection, area peace filters, corpse first-match behavior, thief witness notices, tracking filters, weapon ability target snapshots, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/System/Skills`

### 2026-06-08T22:30:50.7895053-05:00

- Affected phase: Post-audit `POST-BATCH-D-71A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `System:Skills` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; `git diff --check`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched skill files; explicit pooled-variable check showed matching `Free` calls across the 10 touched loops; `git diff --check` reported only expected line-ending warnings; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 724 unique rows including 395 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `System:Skills` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:36:10.3289029-05:00

- Affected phase: Post-audit `POST-BATCH-D-72A` `Trades:Guild` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05086` through `RB-05095` across `Data/Scripts/Trades/Guild`.
- Result: Replaced guildmaster and shoppe authorization direct range scans with local `IPooledEnumerable` variables and `try/finally Free`. Guildmaster count, owned-shoppe count, map fallback authorization, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Trades/Guild`

### 2026-06-08T22:36:10.3289029-05:00

- Affected phase: Post-audit `POST-BATCH-D-72A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Trades:Guild` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; `git diff --check`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in the touched guild files; explicit pooled-variable check showed matching `Free` calls across the 10 touched loops; `git diff --check` reported only expected line-ending warnings; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 734 unique rows including 405 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `Trades:Guild` queued pooled-enumerable rows are complete.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-08T22:40:58.5435096-05:00

- Affected phase: Post-audit `POST-BATCH-D-73A` `Trades:Harvest` pooled enumerable ownership repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-05096` in `Data/Scripts/Trades/Harvest/HarvestSystem.cs`.
- Result: Replaced the harvest drop-at-feet item direct range scan with a local `IPooledEnumerable` variable and `try/finally Free`. At-feet item snapshot, stack merge behavior, serialization, namespaces, type names, save versions, and file location were preserved.
- Output path: `Data/Scripts/Trades/Harvest/HarvestSystem.cs`

### 2026-06-08T22:40:58.5435096-05:00

- Affected phase: Post-audit `POST-BATCH-D-73A` verification, audit artifact update, and `POST-BATCH-D` closeout
- Cwd: `D:\ConficturaUO`
- Command: Targeted `Trades:Harvest` direct range-scan `rg` covering qualified and unqualified `Get*InRange` calls; explicit pooled-variable ownership `rg`; `git diff --check`; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; update `post-batch-d-pooled-enumerable-review.csv`, active overlay, status, README, and next-step artifacts.
- Result: Targeted scan found no remaining direct range scans in `HarvestSystem.cs`; explicit pooled-variable check showed a matching `Free` call; `git diff --check` reported only expected line-ending warnings; `Server.csproj` Debug/x86 build passed; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now has 735 unique rows including 406 `POST-BATCH-D` fixed rows and 2 `POST-BATCH-D` false positives. `POST-BATCH-D` is complete with 408 reviewed pooled-enumerable rows and no remaining queued pooled-enumerable backlog rows.
- Output path: `docs/codebase-audit/outputs/post-batch-d-pooled-enumerable-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:21:06.8359454-05:00

- Affected phase: Post-audit `POST-BATCH-E-29A` System:Chat hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01758`, `RB-01759`, `RB-01760`, `RB-01761`, `RB-01762`, `RB-01763`, `RB-01977`, `RB-02175`, and `RB-02198` across `Data/Scripts/System/Chat/General/General.cs` and `Data/Scripts/System/Chat/Gumps/Error Reporting/Errors.cs`.
- Result: Kept `General.OnLoad` unchanged after source review; added stale data/message guards to `General.OnSave`; added null/deleted event/mobile/speech and stale data guards to chat speech, login, and character-created hooks; added null/deleted login guard to chat error notification. Valid chat behavior, error notification behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/System/Chat/General/General.cs`; `Data/Scripts/System/Chat/Gumps/Error Reporting/Errors.cs`

### 2026-06-09T11:21:06.8359454-05:00

- Affected phase: Post-audit `POST-BATCH-E-29A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted System:Chat hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-29A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed chat General and Errors registrations plus null/deleted/list-entry guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 138 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:26:48.6563353-05:00

- Affected phase: Post-audit `POST-BATCH-E-30A` System:Chat gump guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-02390`, `RB-03108`, `RB-03434`, and `RB-04063` across `Data/Scripts/System/Chat/Gumps/3.0 Skin/SendMessageGump.cs` and `Data/Scripts/System/Chat/Gumps/3.0 Skin/ProfileGump.cs`.
- Result: Added stale gump/owner validation to `SendMessageGump.InternalPicker.OnResponse` before message-color mutation and gump rebuild; added stale owner/target validation to `ProfileGump.Client` before profile rebuild, target `NetState` read, or `ClientGump` dispatch. Valid chat gump behavior, serialization, public APIs, namespaces, type names, save versions, file locations, and project files were preserved.
- Output path: `Data/Scripts/System/Chat/Gumps/3.0 Skin/SendMessageGump.cs`; `Data/Scripts/System/Chat/Gumps/3.0 Skin/ProfileGump.cs`

### 2026-06-09T11:26:48.6563353-05:00

- Affected phase: Post-audit `POST-BATCH-E-30A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted System:Chat gump guard scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-30A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed SendMessageGump hue-picker and ProfileGump client callback stale-state guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 142 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:34:19.3992586-05:00

- Affected phase: Post-audit `POST-BATCH-E-31A` Items:Magical SoulOrb PlayerDeath hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01741` in `Data/Scripts/Items/Magical/SoulOrb.cs`.
- Result: Added a null `PlayerDeathEventArgs` guard before `SoulOrb.EventSink_Death` reads `e.Mobile`. Valid soul-orb resurrection behavior, delayed resurrection scheduling, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Magical/SoulOrb.cs`

### 2026-06-09T11:34:19.3992586-05:00

- Affected phase: Post-audit `POST-BATCH-E-31A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted SoulOrb PlayerDeath hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-31A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed PlayerDeath registration plus null `PlayerDeathEventArgs` guard before `e.Mobile`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 143 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:38:53.1223165-05:00

- Affected phase: Post-audit `POST-BATCH-E-32A` Items:Misc LiarsDice disconnect/crash hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01742` and `RB-01743` in `Data/Scripts/Items/Misc/Games/LiarsDice/DiceChannel.cs`.
- Result: Added null disconnect args, null/deleted mobile, stale player-entry, and list-count bounds guards to `DiceState.EventSink_Disconnected`; added list-count bounds and stale/null/deleted player-entry guards before crash-time balance deposits in `DiceState.EventSink_ServerCrashed`. Valid LiarsDice player removal, balance return, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Misc/Games/LiarsDice/DiceChannel.cs`

### 2026-06-09T11:38:53.1223165-05:00

- Affected phase: Post-audit `POST-BATCH-E-32A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted LiarsDice disconnect/crash hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-32A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Disconnected/Crashed registrations plus null/deleted/list bounds guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 145 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:42:09.0095311-05:00

- Affected phase: Post-audit `POST-BATCH-E-33A` Items:Potions AutoResPotion PlayerDeath hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01744` in `Data/Scripts/Items/Potions/Special/AutoResPotion.cs`.
- Result: Added a null `PlayerDeathEventArgs` guard before `AutoResPotion.EventSink_Death` reads `e.Mobile`. Valid auto-resurrection potion behavior, delayed resurrection scheduling, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Potions/Special/AutoResPotion.cs`

### 2026-06-09T11:42:09.0095311-05:00

- Affected phase: Post-audit `POST-BATCH-E-33A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AutoResPotion PlayerDeath hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-33A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed PlayerDeath registration plus null `PlayerDeathEventArgs` guard before `e.Mobile`; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 146 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:45:34.9680825-05:00

- Affected phase: Post-audit `POST-BATCH-E-34A` Items:Weapons Fists request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01745` and `RB-01746` in `Data/Scripts/Items/Weapons/Hands/Fists.cs`.
- Result: Added null request args and null/deleted mobile guards to `Fists.EventSink_DisarmRequest` and `Fists.EventSink_StunRequest` before skill, free-hand, disruptive-action, and ready-toggle logic. Valid disarm/stun request behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Items/Weapons/Hands/Fists.cs`

### 2026-06-09T11:45:34.9680825-05:00

- Affected phase: Post-audit `POST-BATCH-E-34A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Fists disarm/stun request hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-34A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed DisarmRequest/StunRequest registrations plus null/deleted request guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 148 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:49:00.1804200-05:00

- Affected phase: Post-audit `POST-BATCH-E-35A` Spell Framework Spellbook request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01747` and `RB-01748` in `Data/Scripts/Magic/Magery/Spellbook.cs`.
- Result: Added null request args and null/deleted mobile guards to `Spellbook.EventSink_OpenSpellbookRequest` and `Spellbook.EventSink_CastSpellRequest` before design-context checks, spellbook lookup/display, special-move selection, or spell casting. Valid spellbook open/cast behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Magic/Magery/Spellbook.cs`

### 2026-06-09T11:49:00.1804200-05:00

- Affected phase: Post-audit `POST-BATCH-E-35A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Spellbook open/cast request hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-35A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed OpenSpellbookRequest/CastSpellRequest registrations plus null/deleted request guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 150 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T11:52:53.8033110-05:00

- Affected phase: Post-audit `POST-BATCH-E-36A` Spell Framework AnimalForm login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01749` in `Data/Scripts/Magic/Ninjitsu/AnimalForm.cs`.
- Result: Added null login args and null/deleted mobile guards before animal-form context lookup and speed-control packet send. Valid animal-form speed boost restoration behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Magic/Ninjitsu/AnimalForm.cs`

### 2026-06-09T11:52:53.8033110-05:00

- Affected phase: Post-audit `POST-BATCH-E-36A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AnimalForm login hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-36A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Login registration plus null/deleted login guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 151 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T12:00:38.2203110-05:00

- Affected phase: Post-audit `POST-BATCH-E-37A` Spell Framework ResearchEnchant logout hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01750` in `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs`.
- Result: Added null logout args and null/deleted mobile guards before `ResearchEnchant.OnLogout` ends effects for the logging-out mobile. The intentional `EndEffects(null)` all-cleanup path remains unchanged for timer/deserialization cleanup. Valid ResearchEnchant logout cleanup behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchEnchant.cs`

### 2026-06-09T12:00:38.2203110-05:00

- Affected phase: Post-audit `POST-BATCH-E-37A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ResearchEnchant logout hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-37A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Logout registration plus null/deleted logout guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 152 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:14:14.0725486-05:00

- Affected phase: Post-audit `POST-BATCH-E-38A` Spell Framework ResearchSneak logout hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01751` in `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSneak.cs`.
- Result: Added null logout args and null/deleted mobile guards before `ResearchSneak.OnLogout` removes sneak effects for the logging-out mobile. Valid ResearchSneak logout cleanup behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Magic/Research/Spells/Enchanting/ResearchSneak.cs`

### 2026-06-09T19:14:14.0725486-05:00

- Affected phase: Post-audit `POST-BATCH-E-38A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ResearchSneak logout hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-38A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Logout registration plus null/deleted logout guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 153 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:17:48.2720365-05:00

- Affected phase: Post-audit `POST-BATCH-E-39A` Vendor Core combat request invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01752` and `RB-01753` in `Data/Scripts/Mobiles/Base/Behavior.cs`, `Data/Scripts/Items/Weapons/Hands/Fists.cs`, and `Data/System/Source/EventSink.cs`.
- Result: Classified both rows `ReviewedNoChange`. `Behavior.cs` creates `StunRequestEventArgs` and `DisarmRequestEventArgs` from owned `BaseAI.m_Mobile` inside action methods that already depend on the AI owner, and the active `Fists` subscribers guard null event args plus null/deleted mobiles. No source files changed; source build and compile-only verification were not required for this review-only batch.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:21:50.9852241-05:00

- Affected phase: Post-audit `POST-BATCH-E-40A` PvP Consent PlayerMobile lifecycle hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01754` through `RB-01757` in `Data/Scripts/Mobiles/Base/PlayerMobile.cs`.
- Result: Added null event args and null/deleted mobile guards to `OnLogin`, `OnLogout`, `EventSink_Connected`, and `EventSink_Disconnected` before lifecycle cleanup/update work. Valid player lifecycle behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/Mobiles/Base/PlayerMobile.cs`

### 2026-06-09T19:21:50.9852241-05:00

- Affected phase: Post-audit `POST-BATCH-E-40A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PlayerMobile lifecycle hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-40A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed lifecycle EventSink registrations plus null/deleted event-mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 159 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:30:31.1738917-05:00

- Affected phase: Post-audit `POST-BATCH-E-41A` System:Commands command logging hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01764` in `Data/Scripts/System/Commands/Logging.cs`.
- Result: Added null `CommandEventArgs` and null/deleted mobile guards before `CommandLogging.EventSink_Command` formats command access/account details and writes command logs. Valid command logging behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Commands/Logging.cs`

### 2026-06-09T19:30:31.1738917-05:00

- Affected phase: Post-audit `POST-BATCH-E-41A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Logging command hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-41A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the command EventSink registration plus null/deleted command-mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 160 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:34:14.4161032-05:00

- Affected phase: Post-audit `POST-BATCH-E-42A` System:Commands visibility-list login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01765` in `Data/Scripts/System/Commands/VisibilityList.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before `VisibilityList.OnLogin` clears a `PlayerMobile` visibility list on login. Valid login visibility-list reset behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Commands/VisibilityList.cs`

### 2026-06-09T19:34:14.4161032-05:00

- Affected phase: Post-audit `POST-BATCH-E-42A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted VisibilityList login hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-42A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null/deleted login-mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 161 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:38:51.1881605-05:00

- Affected phase: Post-audit `POST-BATCH-E-43A` System:Commands AFK logout/speech/death hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01766`, `RB-01767`, `RB-01768`, and duplicate speech-method marker `RB-01978` in `Data/Scripts/System/Commands/Player/Afk.cs`.
- Result: Added null event args and null event-mobile guards before `AFK.OnLogout`, `AFK.OnSpeech`, and `AFK.OnDeath` perform AFK table lookups and wake-up cleanup. Valid AFK wake-up behavior, including logout/death cleanup for valid mobiles, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Commands/Player/Afk.cs`

### 2026-06-09T19:38:51.1881605-05:00

- Affected phase: Post-audit `POST-BATCH-E-43A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AFK hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-43A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Logout/Speech/PlayerDeath registrations plus null event and null mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 165 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:42:42.4534284-05:00

- Affected phase: Post-audit `POST-BATCH-E-44A` System:Commands auto-sheathe logout hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01769` in `Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs`.
- Result: Added null `LogoutEventArgs` and null event-mobile guards before `AutoSheatheWeapon.OnPlayerLogout` clears cached weapon state. Valid logout cache cleanup behavior, including cleanup for valid mobile objects without a deleted-mobile early return, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Commands/Player/AutoSheatheWeapon.cs`

### 2026-06-09T19:42:42.4534284-05:00

- Affected phase: Post-audit `POST-BATCH-E-44A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AutoSheathe logout hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-44A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Logout registration plus null event and null mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 166 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:46:29.0986166-05:00

- Affected phase: Post-audit `POST-BATCH-E-45A` System:Gumps report-murderer death hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01770` in `Data/Scripts/System/Gumps/ReportMurderer.cs`.
- Result: Added null `PlayerDeathEventArgs` and null/deleted victim mobile guards before `ReportMurdererGump.EventSink_PlayerDeath` reads aggressor state, awards fame/karma, registers quest kills, or schedules murder-report gumps. Valid murder-report behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Gumps/ReportMurderer.cs`

### 2026-06-09T19:46:29.0986166-05:00

- Affected phase: Post-audit `POST-BATCH-E-45A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ReportMurderer death hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-45A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the PlayerDeath registration plus null/deleted victim guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 167 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:49:55.6040746-05:00

- Affected phase: Post-audit `POST-BATCH-E-46A` System:Help help-request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01771` in `Data/Scripts/System/Help/Gumps/HelpGump.cs`.
- Result: Added null `HelpRequestEventArgs`, null/deleted mobile, and null `NetState` guards before `HelpGump.EventSink_HelpRequest` reads open gumps, checks page eligibility, or sends the help menu/gump. Valid help-request behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Help/Gumps/HelpGump.cs`

### 2026-06-09T19:49:55.6040746-05:00

- Affected phase: Post-audit `POST-BATCH-E-46A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted HelpGump help-request hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-46A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the HelpRequest registration plus null/deleted mobile and null `NetState` guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 168 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T19:56:07.2914086-05:00

- Affected phase: Post-audit `POST-BATCH-E-47A` System:Misc account event-hook guard repair/review
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01772` through `RB-01780`, plus duplicate WorldSave/WorldLoad rows `RB-02177` and `RB-02199`, in `Data/Scripts/System/Misc/Accounts.cs`.
- Result: Added null event args and invalid state guards to socket connect, delete request, account login, game login, connected, disconnected, and login handlers. Disconnect cleanup intentionally has no deleted-mobile early return so valid mobile timer/session cleanup still runs. `Accounts.Load` and `Accounts.Save` were classified `ReviewedNoChange` because WorldLoad has no args and `Save(WorldSaveEventArgs e)` does not consume `e`.
- Output path: `Data/Scripts/System/Misc/Accounts.cs`

### 2026-06-09T19:56:07.2914086-05:00

- Affected phase: Post-audit `POST-BATCH-E-47A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Accounts hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-47A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the SocketConnect/DeleteRequest/AccountLogin/GameLogin/Connected/Disconnected/Login registrations plus null args/state/socket/mobile guard coverage; WorldLoad/WorldSave rows were reviewed with no source change; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 179 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:02:18.7424669-05:00

- Affected phase: Post-audit `POST-BATCH-E-48A` System:Misc animate-request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01781` in `Data/Scripts/System/Misc/Animations.cs`.
- Result: Added null `AnimateRequestEventArgs`, null/deleted mobile, and null action-text guards before bow/salute action selection and animation eligibility checks. Valid bow/salute animation behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Animations.cs`

### 2026-06-09T20:02:18.7424669-05:00

- Affected phase: Post-audit `POST-BATCH-E-48A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Animations hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-48A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the AnimateRequest registration plus null args, null/deleted mobile, and null action guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 180 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:05:43.1008267-05:00

- Affected phase: Post-audit `POST-BATCH-E-49A` System:Misc aggressive-action hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01782` in `Data/Scripts/System/Misc/AttackMessage.cs`.
- Result: Added null `AggressiveActionEventArgs` and null/deleted aggressor/aggressed mobile guards before player checks, recent aggression scans, and overhead attack-message dispatch. Valid attack notification behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/AttackMessage.cs`

### 2026-06-09T20:05:43.1008267-05:00

- Affected phase: Post-audit `POST-BATCH-E-49A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted AttackMessage hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-49A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the AggressiveAction registration plus null args and null/deleted mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 181 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:09:27.7323573-05:00

- Affected phase: Post-audit `POST-BATCH-E-50A` System:Misc broadcast hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01783` through `RB-01786` in `Data/Scripts/System/Misc/Broadcast.cs`.
- Result: Added null/deleted login mobile and `PlayerMobile` guards before login migration/region work; added null/deleted logout and death mobile guards before logging/ghost walking; added null disconnect args/mobile guards while preserving save-on-disconnect behavior for valid mobile objects. Valid broadcast hook behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Broadcast.cs`

### 2026-06-09T20:09:27.7323573-05:00

- Affected phase: Post-audit `POST-BATCH-E-50A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Broadcast hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-50A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login/Logout/Disconnected/PlayerDeath registrations plus null/deleted event-mobile guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 185 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:12:01.2537794-05:00

- Affected phase: Post-audit `POST-BATCH-E-51A` System:Misc crash/shutdown broadcast hook review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01787` and `RB-01788` in `Data/Scripts/System/Misc/Broadcasts.cs` and `Data/System/Source/EventSink.cs`.
- Result: Classified both rows `ReviewedNoChange`. `EventSink_Crashed` and `EventSink_Shutdown` do not dereference event args and already wrap `World.Broadcast` in `try/catch`; adding null-args returns would not improve state safety. No source files changed; source build and compile-only verification were not required for this review-only batch. Active backlog overlay now includes 187 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:15:08.2899745-05:00

- Affected phase: Post-audit `POST-BATCH-E-52A` System:Misc buff-icon client-version hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01789` in `Data/Scripts/System/Misc/BuffIcons.cs`.
- Result: Added null `ClientVersionReceivedArgs`, null `NetState`, null/deleted mobile, and non-`PlayerMobile` guards before scheduling `PlayerMobile.ResendBuffs`. Valid buff resend behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/BuffIcons.cs`

### 2026-06-09T20:15:08.2899745-05:00

- Affected phase: Post-audit `POST-BATCH-E-52A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted BuffIcons hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-52A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the ClientVersionReceived registration plus null args/state/mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 188 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:18:14.2044315-05:00

- Affected phase: Post-audit `POST-BATCH-E-53A` System:Misc character-created hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01790` in `Data/Scripts/System/Misc/CharacterCreation.cs`.
- Result: Added null `CharacterCreatedEventArgs`, null `NetState`, and non-`Account` account guards before profession validation, player mobile creation, access-level assignment, and new-character logging. Valid character creation behavior, XML points attachment, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/CharacterCreation.cs`

### 2026-06-09T20:18:14.2044315-05:00

- Affected phase: Post-audit `POST-BATCH-E-53A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted CharacterCreation hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-53A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the CharacterCreated registration plus null args/state/account guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 189 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:21:35.7153818-05:00

- Affected phase: Post-audit `POST-BATCH-E-54A` System:Misc client-version verification hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01791` in `Data/Scripts/System/Misc/ClientVerification.cs`.
- Result: Added null `ClientVersionReceivedArgs`, null `NetState`, null `ClientVersion`, and null/deleted mobile guards before access-level checks, client policy checks, warning/kick messaging, delayed disconnect scheduling, or warning gump dispatch. Valid client version enforcement behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/ClientVerification.cs`

### 2026-06-09T20:21:35.7153818-05:00

- Affected phase: Post-audit `POST-BATCH-E-54A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ClientVerification hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-54A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the ClientVersionReceived registration plus null args/state/version/mobile guards; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 190 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:27:21.6056244-05:00

- Affected phase: Post-audit `POST-BATCH-E-55A` System:Misc console server-start/speech hook review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01792` and source review/patch of `RB-01793` in `Data/Scripts/System/Misc/Console.cs` and `Data/System/Source/EventSink.cs`.
- Result: Classified `RB-01792` `ReviewedNoChange` because `ServerStartedEventHandler` has no event args and `EventSink_ServerStarted` does not dereference caller state. Added null `SpeechEventArgs`, null/deleted mobile, and null-map-safe region lookup guards in `OnSpeech` before console speech logging. Valid console startup/speech behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Console.cs`

### 2026-06-09T20:27:21.6056244-05:00

- Affected phase: Post-audit `POST-BATCH-E-55A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Console/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-55A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the ServerStarted no-args delegate and Speech registration plus null args/mobile/map guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 192 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:31:10.7542023-05:00

- Affected phase: Post-audit `POST-BATCH-E-56A` System:Misc crash-guard crashed-event hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01794` in `Data/Scripts/System/Misc/CrashGuard.cs`.
- Result: Added null `CrashedEventArgs` guard before crash report generation, backup, service close flag assignment, or restart logic. Valid crash-report, backup, service-close, and restart behavior for real crash events, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/CrashGuard.cs`

### 2026-06-09T20:31:10.7542023-05:00

- Affected phase: Post-audit `POST-BATCH-E-56A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted CrashGuard/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-56A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Crashed registration plus null args guard coverage before `e.Close`/restart/report use; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 193 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:34:30.6232667-05:00

- Affected phase: Post-audit `POST-BATCH-E-57A` System:Misc death auto-resurrection hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01795` in `Data/Scripts/System/Misc/Death.cs`.
- Result: Added null `PlayerDeathEventArgs` and null/deleted mobile guards before scheduling delayed resurrection prompt work; added delayed-callback stale mobile and missing-backpack guards before closing/sending resurrection gumps. Valid auto-resurrection prompt behavior, soul-orb skip behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Death.cs`

### 2026-06-09T20:34:30.6232667-05:00

- Affected phase: Post-audit `POST-BATCH-E-57A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Death/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-57A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the PlayerDeath registration plus null args/mobile/deleted guard coverage and delayed callback stale-state coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 194 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:37:28.6064547-05:00

- Affected phase: Post-audit `POST-BATCH-E-58A` System:Misc fast-walk hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01796` in `Data/Scripts/System/Misc/Fastwalk.cs`.
- Result: Added null `FastWalkEventArgs`, null `NetState`, and null/deleted mobile guards before setting `Blocked` or logging the mobile name. Valid fast-walk blocking/logging behavior for enabled detection, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Fastwalk.cs`

### 2026-06-09T20:37:28.6064547-05:00

- Affected phase: Post-audit `POST-BATCH-E-58A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Fastwalk/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-58A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the FastWalk registration plus null args/state/mobile guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 195 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:40:59.3468350-05:00

- Affected phase: Post-audit `POST-BATCH-E-59A` System:Misc guild create/gump request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01797` and `RB-01798` in `Data/Scripts/System/Misc/Guilds.cs`.
- Result: Added null `CreateGuildEventArgs` guard before deserialized guild construction; added disabled-system, null `GuildGumpRequestArgs`, null/deleted mobile, and non-`PlayerMobile` guards before guild create/info gump dispatch. Valid guild deserialization construction and guild gump request behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Guilds.cs`

### 2026-06-09T20:40:59.3468350-05:00

- Affected phase: Post-audit `POST-BATCH-E-59A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Guilds/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-59A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the CreateGuild and GuildGumpRequest registrations plus null args/mobile/deleted/non-player guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 197 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:44:02.3509005-05:00

- Affected phase: Post-audit `POST-BATCH-E-60A` System:Misc keyword speech hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01799` in `Data/Scripts/System/Misc/Keywords.cs`.
- Result: Added null `SpeechEventArgs`, null/deleted mobile, and null keyword-array guards before keyword iteration and guild/murder/young keyword handling. Valid keyword command behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Keywords.cs`

### 2026-06-09T20:44:02.3509005-05:00

- Affected phase: Post-audit `POST-BATCH-E-60A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Keywords/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-60A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Speech registration plus null args/mobile/keyword guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 198 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:46:49.4707927-05:00

- Affected phase: Post-audit `POST-BATCH-E-61A` System:Misc light-cycle login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01800` in `Data/Scripts/System/Misc/LightCycle.cs`.
- Result: Added null `LoginEventArgs`, null/deleted mobile, and null map guards before login light-level checks. Valid login light-level refresh behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/LightCycle.cs`

### 2026-06-09T20:46:49.4707927-05:00

- Affected phase: Post-audit `POST-BATCH-E-61A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted LightCycle/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-61A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null args/mobile/deleted/map guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 199 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:49:48.6341767-05:00

- Affected phase: Post-audit `POST-BATCH-E-62A` System:Misc login-stats hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01801` in `Data/Scripts/System/Misc/LoginStats.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before login messaging; replaced the direct `PlayerMobile` cast with a safe cast before back-tax and unique-name logic. Valid login help messaging, government back-tax handling, and unique-name prompt behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/LoginStats.cs`

### 2026-06-09T20:49:48.6341767-05:00

- Affected phase: Post-audit `POST-BATCH-E-62A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted LoginStats/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-62A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null args/mobile/deleted and safe `PlayerMobile` guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 200 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:53:09.4645394-05:00

- Affected phase: Post-audit `POST-BATCH-E-63A` System:Misc MOTD login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01802` in `Data/Scripts/System/Misc/MOTD.cs`.
- Result: Added null `LoginEventArgs`, null/deleted mobile, and non-`PlayerMobile` guards before MOTD/start-region/quickbar/reagent-bar login gump dispatch; added stale/non-player guards in `CheckLogin` and `SendGump` before MOTD state and gump work. Valid MOTD display, start-region welcome, quickbar, reagent-bar, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/MOTD.cs`

### 2026-06-09T20:53:09.4645394-05:00

- Affected phase: Post-audit `POST-BATCH-E-63A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted MOTD/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-63A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null args/mobile/deleted/non-player guard coverage and helper guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 201 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T20:56:11.6049807-05:00

- Affected phase: Post-audit `POST-BATCH-E-64A` System:Misc paperdoll request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01803` in `Data/Scripts/System/Misc/Paperdoll.cs`.
- Result: Added null `PaperdollRequestEventArgs`, null/deleted beholder and beheld mobile, null item-list, and null/deleted item-entry guards before paperdoll and OPL packet sends. Valid paperdoll display and OPL update behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Paperdoll.cs`

### 2026-06-09T20:56:11.6049807-05:00

- Affected phase: Post-audit `POST-BATCH-E-64A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Paperdoll/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-64A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the PaperdollRequest registration plus null args/mobile/deleted/list/item guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 202 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:00:05.8680548-05:00

- Affected phase: Post-audit `POST-BATCH-E-65A` System:Misc party logout/login/death hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01804`, `RB-01805`, and `RB-01806` in `Data/Scripts/System/Misc/Party.cs`.
- Result: Added null event-args/mobile guards before party logout/login/death hook work, preserving logout cleanup for valid deleted-mobile objects; added deleted killer fallback for death messages; added stale owner and null/deleted party-member guards in the delayed rejoin timer before status packet sends. Valid party cleanup, rejoin notification, and death-message behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Party.cs`

### 2026-06-09T21:00:05.8680548-05:00

- Affected phase: Post-audit `POST-BATCH-E-65A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Party/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-65A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Logout/Login/PlayerDeath registrations plus null args/mobile/deleted and delayed rejoin stale-state guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 205 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:04:07.8825719-05:00

- Affected phase: Post-audit `POST-BATCH-E-66A` System:Misc quest gump request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01807` in `Data/Scripts/System/Misc/Players.cs`.
- Result: Added null `QuestGumpRequestArgs` and null/deleted mobile guards before closing and sending the status/quest gump. Valid quest/status gump display behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Players.cs`

### 2026-06-09T21:04:07.8825719-05:00

- Affected phase: Post-audit `POST-BATCH-E-66A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Players/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-66A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the QuestGumpRequest registration plus null args/mobile/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 206 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:10:47.4099298-05:00

- Affected phase: Post-audit `POST-BATCH-E-67A` System:Misc login music hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01808` in `Data/Scripts/System/Misc/PlayMusicOnLogin.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before choosing login music or sending `PlayMusic` packets. Valid login music behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/PlayMusicOnLogin.cs`

### 2026-06-09T21:10:47.4099298-05:00

- Affected phase: Post-audit `POST-BATCH-E-67A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PlayMusicOnLogin/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-67A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null args/mobile/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 207 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:14:40.6199562-05:00

- Affected phase: Post-audit `POST-BATCH-E-68A` System:Misc profanity speech hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01809` in `Data/Scripts/System/Misc/ProfanityProtection.cs`.
- Result: Added null `SpeechEventArgs` and null/deleted mobile guards before access checks, profanity validation, or configured profanity actions. Valid disabled-by-default registration, profanity blocking/action behavior, null-speech validation behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/ProfanityProtection.cs`

### 2026-06-09T21:14:40.6199562-05:00

- Affected phase: Post-audit `POST-BATCH-E-68A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ProfanityProtection/NameVerification/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-68A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Speech registration plus null args/mobile/deleted guard coverage and existing null-speech validation behavior; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 208 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:18:02.2304241-05:00

- Affected phase: Post-audit `POST-BATCH-E-69A` System:Misc profile request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01810` and `RB-01811` in `Data/Scripts/System/Misc/Profile.cs`.
- Result: Added null event-args and null/deleted beholder/beheld guards before profile display and profile edit request work. Valid profile display/edit behavior, including existing beholder-profile update semantics, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/Profile.cs`

### 2026-06-09T21:18:02.2304241-05:00

- Affected phase: Post-audit `POST-BATCH-E-69A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Profile/EventSink/PacketHandlers hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-69A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed ProfileRequest and ChangeProfileRequest registrations plus null args/beholder/beheld/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 210 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:21:28.3329980-05:00

- Affected phase: Post-audit `POST-BATCH-E-70A` System:Misc rename request hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01812` in `Data/Scripts/System/Misc/RenameRequests.cs`.
- Result: Added null `RenameRequestEventArgs`, null/deleted source and target mobile, and null requested-name guards before visibility, range, rename-permission, trim, and name-validation work. Valid pet rename permission, validation, profanity rejection, localized message, rename behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/RenameRequests.cs`

### 2026-06-09T21:21:28.3329980-05:00

- Affected phase: Post-audit `POST-BATCH-E-70A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted RenameRequests/NameVerification/EventSink/PacketHandlers hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-70A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the RenameRequest registration plus null args/mobile/deleted/name guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 211 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:24:52.8097966-05:00

- Affected phase: Post-audit `POST-BATCH-E-71A` System:Misc server-list hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01813` in `Data/Scripts/System/Misc/ServerList.cs`.
- Result: Added null `ServerListEventArgs` guard and null `NetState`/socket rejection before endpoint selection and server-list address publication. Valid server-list address detection, endpoint selection, catch-all rejection behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/ServerList.cs`

### 2026-06-09T21:24:52.8097966-05:00

- Affected phase: Post-audit `POST-BATCH-E-71A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ServerList/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-71A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the ServerList registration plus null args/state/socket guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 212 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:28:08.9758778-05:00

- Affected phase: Post-audit `POST-BATCH-E-72A` System:Misc shard poller login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01814` in `Data/Scripts/System/Misc/ShardPoller.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before delayed poll gump scheduling; delayed callback now safely casts the mobile state and skips null/deleted mobiles before `NetState` and poll gump work. Valid active poll login prompt, queued poll, vote-check, poll deactivation behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/ShardPoller.cs`

### 2026-06-09T21:28:08.9758778-05:00

- Affected phase: Post-audit `POST-BATCH-E-72A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted ShardPoller/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-72A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Login registration plus null args/mobile/deleted and delayed callback stale-state guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 213 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:31:18.3525350-05:00

- Affected phase: Post-audit `POST-BATCH-E-73A` System:Misc socket connect hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01815` in `Data/Scripts/System/Misc/SocketOptions.cs`.
- Result: Added null `SocketConnectEventArgs` return and null-socket rejection before applying TCP NoDelay. Valid send-queue coalescing, listener endpoint setup, disallowed-connection skip, TCP NoDelay behavior for valid sockets, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/SocketOptions.cs`

### 2026-06-09T21:31:18.3525350-05:00

- Affected phase: Post-audit `POST-BATCH-E-73A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted SocketOptions/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-73A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the SocketConnect registration plus null args/socket guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 214 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:34:06.4662643-05:00

- Affected phase: Post-audit `POST-BATCH-E-74A` System:Misc weight-overloading movement hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01816` in `Data/Scripts/System/Misc/WeightOverloading.cs`.
- Result: Added null `MovementEventArgs` and null/deleted mobile guards before alive/access/player, overload fatigue, blocked movement, step-count, and `DeathStrike` step handling. Valid overload fatigue, movement blocking, low-stamina drain, player step counting, non-player `DeathStrike` step behavior, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Misc/WeightOverloading.cs`

### 2026-06-09T21:34:06.4662643-05:00

- Affected phase: Post-audit `POST-BATCH-E-74A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted WeightOverloading/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-74A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed the Movement registration plus null args/mobile/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 215 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:41:02.6720047-05:00

- Affected phase: Post-audit `POST-BATCH-E-75A` Regions Obsolete runtime-hook group
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01817` through `RB-01823` in `Data/Scripts/System/Obsolete/Obsolete.cs`.
- Result: Added null event-args and null/deleted mobile guards to `Ethic.EventSink_Speech`, `Faction.EventSink_Login`, `Faction.EventSink_Logout`, `Keywords.EventSink_Speech`, and `RewardSystem.EventSink_Login`; `Keywords.EventSink_Speech` also guards null keyword arrays. Reviewed `ChatSystem.EventSink_ChatRequest` and `Reflector.EventSink_WorldSave` with no source change because the handlers do not consume their event args.
- Output path: `Data/Scripts/System/Obsolete/Obsolete.cs`

### 2026-06-09T21:41:02.6720047-05:00

- Affected phase: Post-audit `POST-BATCH-E-75A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Obsolete/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-75A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed ChatRequest, Ethics speech, Faction login/logout/speech, WorldSave, and VeteranRewards login registrations plus intended guard coverage and no-change decisions; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 222 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:47:36.0858469-05:00

- Affected phase: Post-audit `POST-BATCH-E-76A` Housing house-region login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01824` in `Data/Scripts/System/Regions/HouseRegion.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before house lookup, friend/public checks, and ban-location relocation. Valid private-house login relocation behavior for valid mobiles, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Regions/HouseRegion.cs`

### 2026-06-09T21:47:36.0858469-05:00

- Affected phase: Post-audit `POST-BATCH-E-76A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted HouseRegion/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-76A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Login registration plus null args/mobile/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 223 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:51:03.0556461-05:00

- Affected phase: Post-audit `POST-BATCH-E-77A` System:Skills custom weapon abilities login hook guard repair
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01825` in `Data/Scripts/System/Skills/Weapon Abilities/CustomWeaponAbilities.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before auto-open weapon-bar settings lookup and special-attack gump refresh. Valid auto-open weapon-bar behavior for valid mobiles, serialization, public APIs, namespaces, type names, save versions, file location, and project files were preserved.
- Output path: `Data/Scripts/System/Skills/Weapon Abilities/CustomWeaponAbilities.cs`

### 2026-06-09T21:51:03.0556461-05:00

- Affected phase: Post-audit `POST-BATCH-E-77A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted CustomWeaponAbilities/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-77A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed Login registration plus null args/mobile/deleted guard coverage; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 224 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:53:41.1299469-05:00

- Affected phase: Post-audit `POST-BATCH-E-78A` Trades:Apiculture world-save hook review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01826` in `Data/Scripts/Trades/Apiculture/beehivehelper.cs`.
- Result: Reviewed `BeeHiveHelper.EventSink_WorldSave` with no source change because the handler does not consume `WorldSaveEventArgs` and delegates to the existing hive update pass.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T21:53:41.1299469-05:00

- Affected phase: Post-audit `POST-BATCH-E-78A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted BeeHiveHelper/EventSink source review; append `POST-BATCH-E-78A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed WorldSave registration and no event-args dereference; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 225 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T21:57:03.3766063-05:00

- Affected phase: Post-audit `POST-BATCH-E-79A` Gardening plant-system runtime-hook group
- Cwd: `D:\ConficturaUO`
- Command: Source review and patch of `RB-01827` through `RB-01829` in `Data/Scripts/Trades/Gardening/PlantSystem.cs`.
- Result: Added null `LoginEventArgs` and null/deleted mobile guards before backpack and bank plant growth checks. Reviewed `PlantSystem.EventSink_WorldLoad` and `PlantSystem.EventSink_WorldSave` with no source change because they only delegate to `GrowAll`.
- Output path: `Data/Scripts/Trades/Gardening/PlantSystem.cs`

### 2026-06-09T21:57:03.3766063-05:00

- Affected phase: Post-audit `POST-BATCH-E-79A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PlantSystem/EventSink hook scan; Visual Studio MSBuild `Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-79A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted scan confirmed WorldLoad, WorldSave, and Login registrations plus login null args/mobile/deleted guard coverage and no-change world-hook decisions; `Server.csproj` Debug/x86 build passed with Visual Studio Community 2022 MSBuild; compile-only runtime script verification exited 0 and printed no `Listening:` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay now includes 228 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:00:27.0127690-05:00

- Affected phase: Post-audit `POST-BATCH-E-80A` ServerCore command event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01830` in `Data/System/Source/Commands.cs`.
- Result: Reviewed `CommandSystem.Handle` with no source change because it constructs a non-null `CommandEventArgs` immediately before `EventSink.InvokeCommand(e)` on the valid command path after command lookup, access checks, and handler dispatch. Broad null-caller command framework behavior was not changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:00:27.0127690-05:00

- Affected phase: Post-audit `POST-BATCH-E-80A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Commands/EventSink source review; append `POST-BATCH-E-80A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed `InvokeCommand` receives a just-constructed `CommandEventArgs` on the valid command path; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 229 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:03:09.2051568-05:00

- Affected phase: Post-audit `POST-BATCH-E-81A` Homestead `EventSink.cs` marker false positive
- Cwd: `D:\ConficturaUO`
- Command: Source and runtime-hook-map review of `RB-01831` in `Data/System/Source/EventSink.cs`.
- Result: Classified the row as `FalsePositive` because the generated marker matched the file banner text `EventSink.cs` at line 2, not executable runtime hook code. EventSink world-load and world-save rows remain separately represented in the runtime hook map.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:03:09.2051568-05:00

- Affected phase: Post-audit `POST-BATCH-E-81A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted EventSink/runtime-hook-map source review; append `POST-BATCH-E-81A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted review confirmed the row is extractor noise from the file banner; no source files changed and source build/compile-only verification were not required; active backlog overlay now includes 230 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:05:40.9400617-05:00

- Affected phase: Post-audit `POST-BATCH-E-82A` ServerCore `Main.cs` event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01832` through `RB-01834` in `Data/System/Source/Main.cs`.
- Result: Reviewed crash, shutdown, and server-start event invocations with no source change. Crash dispatch constructs `CrashedEventArgs` immediately before `EventSink.InvokeCrashed`; shutdown dispatch passes a newly constructed `ShutdownEventArgs`; server-start dispatch uses a no-args delegate.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:05:40.9400617-05:00

- Affected phase: Post-audit `POST-BATCH-E-82A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Main/EventSink source review; append `POST-BATCH-E-82A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed crash, shutdown, and server-start event invocations use constructed args or no-args delegates; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 233 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:11:02.2898563-05:00

- Affected phase: Post-audit `POST-BATCH-E-83A` ServerCore `Mobile.cs` event invocation group review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01835` through `RB-01844` in `Data/System/Source/Mobile.cs`.
- Result: Reviewed Mobile hunger-change, logout, aggressive-action, movement, fast-walk, player-death, speech, connected/disconnected, and paperdoll event invocations with no source change. Event args are constructed or retrieved at the callsite from the current mobile/state, and pooled args are freed on the existing paths; broader core caller contracts were not changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:11:02.2898563-05:00

- Affected phase: Post-audit `POST-BATCH-E-83A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Mobile/EventSink source review; append `POST-BATCH-E-83A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed the listed Mobile event invocations construct event args at the callsite from current instance/state or use pooled args immediately before dispatch; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 243 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:18:19.8098499-05:00

- Affected phase: Post-audit `POST-BATCH-E-84A` ServerCore `World.cs` event invocation group review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01845` through `RB-01847` in `Data/System/Source/World.cs`.
- Result: Reviewed create-guild, world-load, and world-save EventSink invocations with no source change. `World.Load` constructs/reuses `CreateGuildEventArgs` and updates the current guild id before dispatch; `World.Load` invokes a no-args `WorldLoad` delegate after successful deserialization; `World.Save` constructs `WorldSaveEventArgs` immediately before dispatch and keeps the existing save-event failure wrapping.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:18:19.8098499-05:00

- Affected phase: Post-audit `POST-BATCH-E-84A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted World/EventSink/Guilds source review; append `POST-BATCH-E-84A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed the listed World event invocations use constructed or no-args event dispatch and current subscribers do not require source changes; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 246 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:21:23.4444560-05:00

- Affected phase: Post-audit `POST-BATCH-E-85A` ServerCore `Listener.cs` socket-connect invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01848` in `Data/System/Source/Network/Listener.cs`.
- Result: Reviewed the socket-connect EventSink invocation with no source change. The accept path calls `VerifySocket` only for non-null accepted sockets, `VerifySocket` constructs `SocketConnectEventArgs` inside the existing try/catch before dispatch, and the current socket subscribers already guard null args plus invalid socket state.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:21:23.4444560-05:00

- Affected phase: Post-audit `POST-BATCH-E-85A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted Listener/EventSink/Accounts/SocketOptions source review; append `POST-BATCH-E-85A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed `SocketConnectEventArgs` is constructed at the callsite and subscriber guard coverage is already present; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 247 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:25:56.6260090-05:00

- Affected phase: Post-audit `POST-BATCH-E-86A` ServerCore encoded `PacketHandlers.cs` event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01849` through `RB-01851` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed set-ability, guild-gump, and quest-gump encoded event invocations with no source change. The handlers are registered as in-game encoded packet handlers, so `EncodedCommand` rejects null `state.Mobile` before dispatch; no `SetAbility` subscriber is registered, and current guild/quest subscribers already guard null/deleted mobiles.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:25:56.6260090-05:00

- Affected phase: Post-audit `POST-BATCH-E-86A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/EventSink/Guilds/Players source review; append `POST-BATCH-E-86A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed the encoded in-game dispatch guard, constructed event args, missing SetAbility subscribers, and downstream null/deleted mobile guard coverage for guild and quest gump requests; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 250 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:30:28.7607484-05:00

- Affected phase: Post-audit `POST-BATCH-E-87A` ServerCore `PacketHandlers.cs` rename, chat, and delete-character event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01852` through `RB-01854` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed rename, chat, and delete-character event invocations with no source change. Rename and chat are in-game packet handlers covered by the MessagePump null/deleted mobile guard before dispatch; rename also has subscriber-level guard coverage, chat is an obsolete no-op subscriber, and delete-character is covered by the Accounts subscriber's event/state/account/index validation.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:30:28.7607484-05:00

- Affected phase: Post-audit `POST-BATCH-E-87A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/MessagePump/EventSink/RenameRequests/Obsolete/Accounts source review; append `POST-BATCH-E-87A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed dispatcher or subscriber guard coverage for rename, chat, and delete request invocations; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 253 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:34:22.9656315-05:00

- Affected phase: Post-audit `POST-BATCH-E-88A` ServerCore `TextCommand` event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01855` through `RB-01860` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed animate, open-spellbook, cast-from-book, open-door, cast-from-macro, and virtue macro `TextCommand` event invocations with no source change. The standard in-game packet dispatcher rejects null/deleted mobiles before `TextCommand`; current animate/spellbook/cast/open-door subscribers already guard invalid args/mobile state, and `VirtueMacroRequest` has no current subscriber.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:34:22.9656315-05:00

- Affected phase: Post-audit `POST-BATCH-E-88A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/MessagePump/EventSink/Animations/Spellbook/BaseDoor/Obsolete source review; append `POST-BATCH-E-88A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed TextCommand dispatcher guard coverage, subscriber guard coverage for animate/spellbook/cast/open-door requests, and no current VirtueMacroRequest subscriber; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 259 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:39:11.8850291-05:00

- Affected phase: Post-audit `POST-BATCH-E-89A` ServerCore profile, help, virtue, and cast request invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01861` through `RB-01866` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed profile display/edit, help request, virtue gump/item request, and extended cast request invocations with no source change. Dispatcher guards cover null/deleted mobiles before in-game standard or extended handler dispatch; profile/help/cast subscribers already guard invalid args/mobile state; and `VirtueGumpRequest`/`VirtueItemRequest` have no current subscribers.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:39:11.8850291-05:00

- Affected phase: Post-audit `POST-BATCH-E-89A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/MessagePump/ExtendedCommand/EventSink/Profile/HelpGump/Spellbook/Obsolete source review; append `POST-BATCH-E-89A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed dispatcher guard coverage, subscriber guard coverage for profile/help/cast requests, and no current VirtueGumpRequest or VirtueItemRequest subscriber; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 265 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:42:55.9802192-05:00

- Affected phase: Post-audit `POST-BATCH-E-90A` ServerCore stun, disarm, and client-version invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01867` through `RB-01870` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed stun, disarm, and client-version invocations with no source change, and classified the commented `ClientType` client-version invocation row as a false positive. Extended dispatcher guards cover stun/disarm null/deleted mobiles, Fists subscribers guard invalid event/mobile state, and client-version subscribers guard invalid event/state/version/mobile state before doing work.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:42:55.9802192-05:00

- Affected phase: Post-audit `POST-BATCH-E-90A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/ExtendedCommand/EventSink/Fists/BuffIcons/ClientVerification source review; append `POST-BATCH-E-90A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed guard coverage for stun, disarm, and client-version dispatches and confirmed line 2056 is commented code; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 269 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:47:41.9419923-05:00

- Affected phase: Post-audit `POST-BATCH-E-91A` ServerCore login and account event invocation review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01871` through `RB-01876` in `Data/System/Source/Network/PacketHandlers.cs`.
- Result: Reviewed login, character-created, game-login, account-login, and server-list event invocations with no source change. PacketHandlers constructs event args from validated login/account/character payload state, and current CharacterCreation, Accounts, and ServerList subscribers guard invalid event/state/account/socket inputs before doing work.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:47:41.9419923-05:00

- Affected phase: Post-audit `POST-BATCH-E-91A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlers/EventSink/CharacterCreation/Accounts/ServerList source review; append `POST-BATCH-E-91A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv` and the active overlay; update status, README, and next-step artifacts.
- Result: Targeted source review confirmed constructed event args and caller/subscriber guard coverage for login, character-created, game-login, account-login, and server-list invocations; source build and compile-only verification were not required because no source files changed; active backlog overlay now includes 275 `POST-BATCH-E` dispositions.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:52:16.0339296-05:00

- Affected phase: Post-audit `POST-BATCH-E-92A` XMLSpawner packet override review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01877` through `RB-01879` in `Data/Scripts/Custom/XMLSpawner/PacketHandlerOverrides.cs` and `Data/Scripts/Custom/XMLSpawner/XmlTextEntryBook.cs`.
- Result: Reviewed XMLSpawner content-change and use-request packet overrides with no source change, and classified the commented XmlTextEntryBook registration row as a false positive. The live overrides are in-game packet registrations protected by MessagePump null/deleted mobile guards; target/page/attachment validation remains in the XMLSpawner handlers.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:52:16.0339296-05:00

- Affected phase: Post-audit `POST-BATCH-E-92A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted PacketHandlerOverrides/MessagePump/XmlTextEntryBook/XmlAttach source review; append `POST-BATCH-E-92A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay rows for `RB-01877` through `RB-01879`.
- Result: Targeted source review confirmed XMLSpawner content-change and use-request override guard coverage, and classified the commented XmlTextEntryBook registration row as non-executable; source build and compile-only verification were not required because no source files changed; active backlog overlay remains at 275 `POST-BATCH-E` dispositions with no duplicate BacklogIds, while the `POST-BATCH-E` review CSV records 278 reviewed rows through `POST-BATCH-E-92A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T22:58:44.8330579-05:00

- Affected phase: Post-audit `POST-BATCH-E-93A` BaseBook packet handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01880` through `RB-01882` in `Data/Scripts/Items/Books/BaseBook.cs`, `Data/System/Source/Network/MessagePump.cs`, and targeted direct-call/duplicate-registration `rg` checks.
- Result: Reviewed BaseBook header/content packet handlers with no source change. The handlers are registered as in-game packets and rely on `MessagePump` null/deleted mobile guards before dispatch; handler-local writable/range/accessibility and title/author/page/line bounds checks remain intact. Targeted `rg` found no direct handler calls and no duplicate `0xD4`, `0x66`, or `0x93` BaseBook packet registrations.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T22:58:44.8330579-05:00

- Affected phase: Post-audit `POST-BATCH-E-93A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Targeted BaseBook/MessagePump source review; append `POST-BATCH-E-93A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay rows for `RB-01880` through `RB-01882`.
- Result: Source build and compile-only verification were not required because no source files changed; active backlog overlay remains at 275 `POST-BATCH-E` dispositions with no duplicate BacklogIds, while the `POST-BATCH-E` review CSV records 281 reviewed rows through `POST-BATCH-E-93A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T23:04:31.0536900-05:00

- Affected phase: Post-audit `POST-BATCH-E-94A` Monopoly gump response packet repair
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01883` in `Data/Scripts/Items/Houses/Monopoly/Misc/GumpResponse.cs`, comparison with core `PacketHandlers.DisplayGumpResponse`, and source patch to restore switch/text count and oversized text-entry bounds behavior without changing server public APIs.
- Result: Patched `DisplayGumpResponse` to reject switch counts above the matched gump's check/radio entry count, reject text counts above the matched gump's text-entry count, and disconnect oversized text-entry payloads before relay allocation or `OnResponse`, while preserving the existing PacketReader position restore and successor/core fallback behavior.
- Output path: `Data/Scripts/Items/Houses/Monopoly/Misc/GumpResponse.cs`

### 2026-06-09T23:04:31.0536900-05:00

- Affected phase: Post-audit `POST-BATCH-E-94A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal`; `.\ConficturaServer.exe -compileonly -nocache`; restore generated root executable artifacts; append `POST-BATCH-E-94A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update the existing active overlay row for `RB-01883`; update status, README, and next-step artifacts.
- Result: `Server.csproj` Debug/x86 build passed; compile-only runtime script verification completed successfully and printed no `Listening` output; generated `ConficturaServer.exe`, `.config`, and `.pdb` were restored; active backlog overlay remains unique and now has 276 `POST-BATCH-E` dispositions; the `POST-BATCH-E` review CSV records 282 reviewed rows through `POST-BATCH-E-94A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-09T23:08:59.3836669-05:00

- Affected phase: Post-audit `POST-BATCH-E-95A` already-covered packet handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01884` through `RB-01886` in `Data/Scripts/Items/Misc/BulletinBoards.cs`, `Data/Scripts/Items/Misc/Games/Mahjong/MahjongPacketHandlers.cs`, `Data/Scripts/Items/Trades/Maps/MapItem.cs`, `Data/System/Source/Network/MessagePump.cs`, and targeted duplicate-registration `rg` checks.
- Result: Reviewed bulletin board, mahjong, and map command packet handlers with no source change. Existing MessagePump null/deleted mobile dispatch guards plus local board/range/poster, mahjong player/dealer, and map edit validation gates remain intact. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:08:59.3836669-05:00

- Affected phase: Post-audit `POST-BATCH-E-95A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-95A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay rows for `RB-01884` through `RB-01886`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 285 reviewed rows through `POST-BATCH-E-95A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:13:29.2218208-05:00

- Affected phase: Post-audit `POST-BATCH-E-96A` Chat3Guild packet handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01887` and `RB-01888` in `Data/Scripts/System/Chat/General/Chat3Guild.cs`, `Data/System/Source/Network/MessagePump.cs`, `Data/Scripts/System/Chat/Channels/Channel.cs`, and `Data/Scripts/System/Chat/Channels/Guild.cs`.
- Result: Reviewed ASCII and Unicode guild chat packet handlers with no source change. Existing MessagePump null/deleted mobile dispatch guards, Chat3Guild text/type/encoded-keyword bounds, and Channel/Guild chat eligibility checks remain intact. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:13:29.2218208-05:00

- Affected phase: Post-audit `POST-BATCH-E-96A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-96A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay rows for `RB-01887` and `RB-01888`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 287 reviewed rows through `POST-BATCH-E-96A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:16:05.7673555-05:00

- Affected phase: Post-audit `POST-BATCH-E-97A` HardwareInfo packet handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01889` in `Data/Scripts/System/Misc/HardwareInfo.cs`.
- Result: Reviewed the account hardware packet handler with no source change. The fixed-length 0xD9 handler intentionally accepts pre-ingame account state, writes hardware data only when `state.Account` is an `Account`, and staff display remains command-gated at `AccessLevel.GameMaster`; broader retention/privacy policy remains outside this batch. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:16:05.7673555-05:00

- Affected phase: Post-audit `POST-BATCH-E-97A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-97A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay row for `RB-01889`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 288 reviewed rows through `POST-BATCH-E-97A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:18:07.9376833-05:00

- Affected phase: Post-audit `POST-BATCH-E-98A` ProtocolExtensions packet dispatcher review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01890` in `Data/Scripts/System/Misc/ProtocolExtensions.cs` and first-packet handling in `Data/System/Source/Network/MessagePump.cs`.
- Result: Reviewed the bundled 0xF0 packet dispatcher with no source change. The outer packet remains non-ingame by design, subhandler lookup is bounded, and `DecodeBundledPacket` explicitly rejects null or deleted mobiles before invoking ingame subhandlers. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:18:07.9376833-05:00

- Affected phase: Post-audit `POST-BATCH-E-98A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-98A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay row for `RB-01890`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 289 reviewed rows through `POST-BATCH-E-98A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:20:14.7548323-05:00

- Affected phase: Post-audit `POST-BATCH-E-99A` RemoteAdmin packet handler review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01891` in `Data/Scripts/System/Misc/Remote.cs`.
- Result: Reviewed the 0xF1 remote-admin packet handler with no source change. The handler gates non-login admin commands through authenticated NetState membership, validates account/IP/password/access/ban state before authentication, and the previously identified remote-admin design/security-policy surface remains deferred rather than changed in this packet-guard batch. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:20:14.7548323-05:00

- Affected phase: Post-audit `POST-BATCH-E-99A` verification and audit artifact update
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-99A` row to `post-batch-e-hooks-gumps-commands-regions-review.csv`; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay row for `RB-01891`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 290 reviewed rows through `POST-BATCH-E-99A`.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:22:28.4745902-05:00

- Affected phase: Post-audit `POST-BATCH-E-100A` obsolete legacy chat packet review
- Cwd: `D:\ConficturaUO`
- Command: Source review of `RB-01892` and `RB-01893` in `Data/Scripts/System/Obsolete/Obsolete.cs` and dispatch guards in `Data/System/Source/Network/MessagePump.cs`.
- Result: Reviewed obsolete open-chat and chat-action packet handlers with no source change. Both handlers register as in-game packets protected by MessagePump null/deleted mobile guards; the open-chat path validates bounded nickname/account tag state before adding chat users, and the chat-action path requires an existing `ChatUser`, known action handler, conference state, and moderator status where applicable before callback dispatch. Broader legacy chat policy remains deferred. Source build and compile-only verification were not required because no source files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-09T23:22:28.4745902-05:00

- Affected phase: Post-audit `POST-BATCH-E-100A` verification, audit artifact update, and handoff check
- Cwd: `D:\ConficturaUO`
- Command: Append `POST-BATCH-E-100A` rows to `post-batch-e-hooks-gumps-commands-regions-review.csv`; compare `repair-backlog.csv` against the `POST-BATCH-E` review CSV for unreviewed `Runtime hooks`, `Gump guards`, `Command access`, and `Regions` rows; update status, README, and next-step artifacts while preserving the existing `POST-BATCH-C-01A` active overlay rows for `RB-01892` and `RB-01893`.
- Result: Active backlog overlay remains unique with 276 `POST-BATCH-E` dispositions, while the `POST-BATCH-E` review CSV records 292 reviewed rows through `POST-BATCH-E-100A`; no unreviewed `Runtime hooks`, `Gump guards`, `Command access`, or `Regions` repair-backlog rows remain, so the next step is a human decision on deferred policy reviews or the next post-audit batch track.
- Output path: `docs/codebase-audit/outputs/post-batch-e-hooks-gumps-commands-regions-review.csv`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean worktree before edits.
- Output path: Working tree status only.

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: Update `Data/Scripts/Scripts.csproj` by replacing 82 stale `MovedGumpFolder` compile includes with their one same-filename active source replacement from `phase-02-unincluded-source-classified.csv`, then add 10 active sources missing from the project.
- Result: Replaced 82 exact include paths and added 10 active compile includes without editing runtime source files.
- Output path: `Data/Scripts/Scripts.csproj`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`
- Result: Initial rerun completed against stale Phase 1 include inventory and still reported the pre-repair counts: 6,571 script compile includes, 6,581 script source files, 82 missing compile targets, and 92 unincluded source files. The project truth tool consumes `phase-01-project-includes.csv`, so the include inventory had to be refreshed before the final project truth rerun.
- Output path: `docs/codebase-audit/outputs/project-truth-register.csv`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\Invoke-CodebaseAuditInventory.ps1`
- Result: Failed during a later null-export step, but refreshed the include-dependent Phase 1 CSVs used by project truth. The refreshed `phase-01-project-includes.csv` contains 6,581 compile includes, `phase-01-missing-compile-targets.csv` has 0 rows, and `phase-01-unincluded-source-files.csv` has 0 rows.
- Output path: `docs/codebase-audit/outputs/phase-01-project-includes.csv`; `docs/codebase-audit/outputs/phase-01-missing-compile-targets.csv`; `docs/codebase-audit/outputs/phase-01-unincluded-source-files.csv`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`
- Result: Succeeded after the include inventory refresh. Project truth now reports 13,162 rows, 6,581 script compile includes, 6,581 script source files, 0 missing compile targets, 0 unincluded source files, and 0 project cleanup backlog groups.
- Output path: `docs/codebase-audit/outputs/project-truth-register.csv`; `docs/codebase-audit/outputs/phase-02-missing-compile-targets-classified.csv`; `docs/codebase-audit/outputs/phase-02-unincluded-source-classified.csv`; `docs/codebase-audit/outputs/phase-02-project-cleanup-backlog.csv`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors.
- Output path: Working tree diff check only.

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"`
- Result: Failed because `msbuild` was not on `PATH` in this shell. The same build was rerun through the Visual Studio 2022 MSBuild executable.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"`
- Result: Sandboxed run failed with `MSB4184` because access to `C:\Users\nepht\AppData\Local\Microsoft SDKs` was denied. The same command was rerun outside the sandbox with approval.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"` and errors-only rerun with `/v:minimal /clp:ErrorsOnly`
- Result: Out-of-sandbox build reached `Scripts.csproj` compilation and failed with 93 existing dependency/reference errors. The failure no longer includes stale missing source file errors; representative remaining surfaces are missing `OrbServerSDK`, missing `UOArchitectInterface`, and missing `System.Web`/`System.Drawing` reference namespaces. This is recorded as `IDEProjectHygiene` follow-up, not live runtime proof and not project truth drift.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-13T13:14:33.2498564-05:00

- Affected phase: Post-audit `ScriptsProjectTruth` / `IDEProjectHygiene` repair
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`
- Result: Succeeded. Tracked MSBuild-generated server artifacts were restored before staging.
- Output path: Working tree status only.

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`
- Result: Clean worktree before the Visual Studio project reference repair.
- Output path: Working tree status only.

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: Inspect `Data/System/CFG/Assemblies.cfg`, `Data/System/Source/ScriptCompiler.cs`, checked-in root DLLs, and `Data/Scripts/Scripts.csproj`.
- Result: Runtime script compilation already references `System.Web.dll`, `System.Drawing.dll`, `System.Windows.Forms.dll`, `OrbServerSDK.dll`, `System.Runtime.Remoting.dll`, and `UOArchitectInterface.dll`. `OrbServerSDK.dll` and `UOArchitectInterface.dll` exist at the repository root, and `Scripts.csproj` lacked those Visual Studio project references.
- Output path: `Data/System/CFG/Assemblies.cfg`; `Data/System/Source/ScriptCompiler.cs`; `Data/Scripts/Scripts.csproj`

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: Update `Data/Scripts/Scripts.csproj` references and Debug `DebugType`.
- Result: Added project references for the runtime assembly list and changed Debug PDB output from native `full` PDBs to `portable` PDBs after MSBuild proved portable PDBs avoid `CS0041` for `Server.Misc.Farms.PlantGardens()`. No runtime source files changed.
- Output path: `Data/Scripts/Scripts.csproj`

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`
- Result: Succeeded. Counts remain 13,162 project truth rows, 6,581 script compile includes, 6,581 script source files, 0 missing compile targets, 0 unincluded source files, and 0 project cleanup backlog groups.
- Output path: `docs/codebase-audit/outputs/project-truth-register.csv`; `docs/codebase-audit/outputs/phase-02-missing-compile-targets-classified.csv`; `docs/codebase-audit/outputs/phase-02-unincluded-source-classified.csv`

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`
- Result: Passed with no whitespace errors; Git emitted the expected CRLF warning for `Data/Scripts/Scripts.csproj`.
- Output path: Working tree diff check only.

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal /nologo`
- Result: Passed with warnings. The maintained solution build no longer fails on missing `OrbServerSDK`, `UOArchitectInterface`, `System.Web`, or `System.Drawing` references, and no longer fails on the native PDB metadata limit.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\Data\Scripts\Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal /nologo`
- Result: Failed before script compilation on the known direct project-reference limitation: `Server.csproj` has no standalone `Debug|AnyCPU` output path outside solution mapping.
- Output path: `docs/codebase-audit/outputs/phase-02-build-verification.md`

### 2026-06-13T19:16:08.3913927-05:00

- Affected phase: Post-audit `IDEProjectHygiene` reference repair
- Cwd: `D:\ConficturaUO`
- Command: `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`; remove exact untracked MSBuild outputs from `Data/Scripts`.
- Result: Succeeded. Tracked generated server artifacts were restored and untracked build outputs created by MSBuild were removed before staging.
- Output path: Working tree status only.

### 2026-06-13T19:48:57.0377992-05:00

- Affected phase: Post-audit `POST-BATCH-F` documentation and balance review
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; read root/doc audit instructions, batch plan, repair backlog, active overlay, documentation truth, dependency graph, synergy/conflict, and relevant phase plans.
- Result: Initial worktree was clean; applicable instructions and inputs were present; POST-BATCH-F filter matched 540 rows.
- Output path: `docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv`

### 2026-06-13T19:48:57.0377992-05:00

- Affected phase: Post-audit `POST-BATCH-F` documentation/source-trace corrections
- Cwd: `D:\ConficturaUO`
- Command: edit wiki documentation source traces and index entries for objective missing-path/source-trace evidence.
- Result: Replaced wildcard/multi-path source references with exact existing source paths in nine wiki pages and added `AI_OVERHAUL_AUDIT.md` to the technical index; no source/config/project files changed.
- Output path: `docs/wiki/AI_OVERHAUL_AUDIT.md`; `docs/wiki/Apiculture_Beekeeping.md`; `docs/wiki/Gardening_System.md`; `docs/wiki/In_Game_Command_List.md`; `docs/wiki/Player_Mobile_Core.md`; `docs/wiki/Runic_Tools_Crafting.md`; `docs/wiki/Shoppes_Vendors.md`; `docs/wiki/Standard_Crafting.md`; `docs/wiki/wikibacklog.md`; `docs/wiki/INDEX.md`

### 2026-06-13T19:48:57.0377992-05:00

- Affected phase: Phase 7/8/9 regenerated evidence for `POST-BATCH-F`
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-DocumentationTruthAudit.ps1`; `.\docs\codebase-audit\tools\New-DependencyGraph.ps1`; `.\docs\codebase-audit\tools\New-SynergyConflictMatrix.ps1`
- Result: Documentation truth regenerated with 106 canonical pages, 97 canonical pages still missing Source Trace, and one remaining missing-source row isolated to legacy `SystemAudit.md`; dependency graph regenerated with 30,211 edges; synergy/conflict matrix regenerated with 351 rows, 26 balance-risk rows, 141 documentation-risk rows, and 32 staff-dependency rows.
- Output path: `docs/codebase-audit/outputs/documentation-truth-table.csv`; `docs/codebase-audit/outputs/dependency-graph.csv`; `docs/codebase-audit/outputs/synergy-conflict-matrix.csv`

### 2026-06-13T19:48:57.0377992-05:00

- Affected phase: Post-audit `POST-BATCH-F` audit state update
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-PostBatchFReview.ps1`
- Result: Review CSV and active overlay each contain 540 POST-BATCH-F rows; decision counts are DeferredBalanceDecision=26, DeferredLegacyAuditTableCleanup=1, DeferredSourceTraceReview=130, DeferredSupportDocReview=2, FixedDocumentationTrace=20, FixedHistoricalPathReference=5, NeedsHumanDecision=32, QueuedSchemaDocumentation=232, QueuedSourceFollowUp=92.
- Output path: `docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv`; `docs/codebase-audit/outputs/post-batch-f-documentation-balance-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-13T19:52:04.2147226-05:00

- Affected phase: Post-audit `POST-BATCH-F` final verification
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`; source/config/project diff check; POST-BATCH-F review/overlay invariant check; edited canonical documentation source-trace check.
- Result: Passed. `git diff --check` reported no whitespace errors; no `.cs`, `.xml`, `.cfg`, `.csproj`, or `Data/` paths changed; final invariant checks found 540 review rows, 540 POST-BATCH-F active overlay rows, no blank decisions, no blank verification fields, and no blank overlay actions; all eight edited canonical docs are marked `SourceTracePresent=Yes`.
- Output path: `docs/codebase-audit/outputs/post-batch-f-documentation-balance-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/outputs/documentation-truth-table.csv`

### 2026-06-13T20:17:10.5956233-05:00

- Affected phase: Post-audit `POST-BATCH-G` project include drift closeout
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; read root/doc audit instructions, root audit plan, Phase 2 plan, Phase 13 plan, repair backlog, active overlay, and current project truth outputs.
- Result: Initial worktree was clean; applicable instructions and inputs were present; `repair-backlog.csv` contains 61 historical `Project include drift` rows.
- Output path: `docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv`

### 2026-06-13T20:17:10.5956233-05:00

- Affected phase: Phase 2 project truth verification for `POST-BATCH-G`
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1`
- Result: Succeeded. Counts are 6581 `Scripts.csproj` compile includes, 6581 script source files, 0 missing compile targets, 0 unincluded source files, 0 project cleanup backlog groups, and zero rows in phase-scoped missing/unincluded/project cleanup outputs.
- Output path: `docs/codebase-audit/outputs/project-truth-register.csv`; `docs/codebase-audit/outputs/missing-compile-targets.csv`; `docs/codebase-audit/outputs/unincluded-source-files.csv`; `docs/codebase-audit/outputs/project-cleanup-backlog.csv`; `docs/codebase-audit/outputs/phase-02-summary.md`

### 2026-06-13T20:17:10.5956233-05:00

- Affected phase: Post-audit `POST-BATCH-G` audit state update
- Cwd: `D:\ConficturaUO`
- Command: `.\docs\codebase-audit\tools\New-PostBatchGReview.ps1`
- Result: Review CSV and active overlay each contain 61 POST-BATCH-G rows; all are FixedProjectTruthDrift / active Fixed reconciliations. No source, project, runtime, XML, config, namespace, serialization, save-version, runtime-location, or gameplay files changed.
- Output path: `docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv`; `docs/codebase-audit/outputs/post-batch-g-project-include-drift-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`

### 2026-06-13T20:17:50.3016854-05:00

- Affected phase: Post-audit `POST-BATCH-G` final verification
- Cwd: `D:\ConficturaUO`
- Command: `git diff --check`; source/config/project diff check; project-drift zero-row check; POST-BATCH-G review/overlay invariant check.
- Result: Passed. `git diff --check` reported no whitespace errors; no `.cs`, `.xml`, `.cfg`, `.csproj`, or `Data/` paths changed; six project-drift output files remain zero rows; final invariant checks found 61 review rows, 61 POST-BATCH-G active overlay rows, 61 `FixedProjectTruthDrift` decisions, 61 active `Fixed` rows, no blank decisions, no blank verification fields, and no blank overlay actions.
- Output path: `docs/codebase-audit/outputs/post-batch-g-project-include-drift-review.csv`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/outputs/project-truth-register.csv`
### 2026-06-13T20:46:30.2646799-05:00

- Affected phase: Post-audit `POST-BATCH-H-01A` Character Level reorganization pilot
- Cwd: `D:\ConficturaUO`
- Command: move Character Level files, update `Scripts.csproj`, regenerate Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCharacterLevelMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06802; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; serialization register has 0 Character Level rows; runtime hook map has 3 moved Character Level hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files.; solution=Passed with existing warnings; Scripts built Data/Scripts/ClassLibrary.dll and generated script-project artifacts were removed before staging.; server build=Passed; Server built D:/ConficturaUO/ConficturaServer.exe and tracked root build artifacts were restored before staging.; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done..
- Output path: `docs/codebase-audit/outputs/post-batch-h-character-level-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-character-level-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-13T20:49:08.9470513-05:00

- Affected phase: Post-audit `POST-BATCH-H-01A` Character Level reorganization pilot
- Cwd: `D:\ConficturaUO`
- Command: move Character Level files, update `Scripts.csproj`, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCharacterLevelMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06802; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 2 target rows and 0 old-path rows; serialization register has 0 Character Level rows; runtime hook map has 3 moved Character Level hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed with existing warnings; Scripts built Data/Scripts/ClassLibrary.dll and generated script-project artifacts were removed before staging; server build=Passed; Server built D:/ConficturaUO/ConficturaServer.exe and tracked root build artifacts were restored before staging; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-character-level-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-character-level-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T20:49:49.0363375-05:00

- Affected phase: Post-audit `POST-BATCH-H-01A` Character Level reorganization pilot
- Cwd: `D:\ConficturaUO`
- Command: move Character Level files, update `Scripts.csproj`, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCharacterLevelMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06802; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 2 target rows and 0 old-path rows; serialization register has 0 Character Level rows; runtime hook map has 3 moved Character Level hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed with existing warnings; Scripts built Data/Scripts/ClassLibrary.dll and generated script-project artifacts were removed before staging; server build=Passed; Server built D:/ConficturaUO/ConficturaServer.exe and tracked root build artifacts were restored before staging; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-character-level-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-character-level-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T22:29:56.1292092-05:00

- Affected phase: Post-audit `POST-BATCH-H-02A` AI Overhaul reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move AI Overhaul workspace, update `Scripts.csproj`, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHAIOverhaulMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06809; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 1 target row and 0 old-path rows; serialization register has 0 AI Overhaul rows; runtime hook map has 0 AI Overhaul hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\Combat\AIOverhaul\AITacticalTargeting.cs; 53 warnings and 0 errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-ai-overhaul-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-ai-overhaul-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T22:44:53.5818869-05:00

- Affected phase: Post-audit `POST-BATCH-H-03A` Static Gump Tool reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move Static Gump Tool workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHStaticGumpToolMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06811; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 21 target rows and 0 old-path rows; serialization register has 0 Static Gump Tool rows; runtime hook map has 198 Static Gump Tool hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\StaffTools\StaticGumpTool scripts; 53 warnings and 0 errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-static-gump-tool-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-static-gump-tool-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T22:56:37.1593591-05:00

- Affected phase: Post-audit `POST-BATCH-H-04A` OmniAI reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move OmniAI workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHOmniAIMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06810; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 8 target rows and 0 old-path rows; serialization register has 1 OmniAI row for Server.Mobiles.AITester version 0; runtime hook map has 2 OmniAI hook rows. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed with existing warnings; ConficturaUO.sln Debug/Any CPU built Server and Scripts, including Custom\ThirdParty\OmniAI scripts; 53 warnings and 0 errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-omniai-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-omniai-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T23:10:16.1908299-05:00

- Affected phase: Post-audit `POST-BATCH-H-05A` Staff Toolbar reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move Staff Toolbar workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHStaffToolbarMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06812; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 1 target row and 0 old-path rows; serialization register has one Staff Toolbar row for Joeku.ToolbarInfos with ReadVersionOnly; runtime hook map has 26 Staff Toolbar hook rows (Event=1; Gump=23; Initialize=1; Login=1). Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed after sandbox SDK lookup denial rerun outside sandbox; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings; 53 warnings and 0 errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-staff-toolbar-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-staff-toolbar-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T23:22:41.7451596-05:00

- Affected phase: Post-audit `POST-BATCH-H-06A` Random Encounters reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move Random Encounters workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHRandomEncountersMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06804; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 11 target rows and 0 old-path rows; serialization register has two Random Encounters rows, including Server.Engines.XmlSpawner2.XmlDateCount version 0; runtime hook map has 8 Random Encounters hook rows (Command=1; Initialize=3; Timer=4); EncounterEngine.cs points at the moved RandomEncounters.xml path. Final verification: git diff --check=Passed with no whitespace errors; Git emitted expected LF-to-CRLF working-copy warnings for edited text files; solution=Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings; 53 warnings and 0 errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe with 0 warnings and 0 errors; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-random-encounters-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-random-encounters-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T23:38:41.8781692-05:00

- Affected phase: Post-audit `POST-BATCH-H-07A` Clone Offline Player Characters reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move Clone Offline Player Characters workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHCloneOfflineMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06815; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 7 target rows and 0 old-path rows; serialization register has four Clone Offline rows (BackpackClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; CharacterClone:v0:WriteVersionOnly:CountMismatch:Writes=1;Reads=2; EtherealMountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1; MountClone:v0:WriteVersionOnly:CountMismatch:Writes=0;Reads=1); runtime hook map has 8 Clone Offline hook rows (Command=1; Event=2; Initialize=2; Login=1; Logout=1; Timer=1). Final verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors; solution=Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings and no errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-clone-offline-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-clone-offline-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T23:43:09.9206912-05:00

- Affected phase: Post-audit `POST-BATCH-H-08A` PvP Consent reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review PvP Consent folder move gate, source/project/serialization/hook evidence, and related policy rows; run `New-PostBatchHPvPConsentGateReview.ps1`.
- Result: Classified RB-06806 as DeferredMoveGate. Source path exists with 5 runtime-visible C# files and 7 total files; target path absent; project truth has 5 includes, 5 sources, 0 missing targets, and 0 unincluded sources; serialization register has 5 rows; runtime hook map has 13 rows (Command=1; Gump=6; Initialize=1; Movement=1; Speech=1; Timer=3); remaining blockers are RB-05609:PvP Consent <-> Government balance/policy; RB-05610:PvP Consent <-> XMLSpawner/XMLPoints balance/policy; RB-05631:XMLSpawner staff tooling policy decision. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-pvp-consent-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-pvp-consent-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-13T23:53:11.6554902-05:00

- Affected phase: Post-audit `POST-BATCH-H-09A` Monster Nests reorganization batch
- Cwd: `D:\ConficturaUO`
- Command: move Monster Nests workspace, update `Scripts.csproj`, update path-sensitive docs/tool evidence, regenerate runtime script compile inventory plus Phase 1/2/3/4/5/6/7/8/9 outputs, and run `New-PostBatchHMonsterNestsMove.ps1`.
- Result: Review CSV and active overlay contain one POST-BATCH-H row for RB-06805; project truth has 6581 includes, 6581 sources, 0 missing targets, 0 unlisted sources, and 0 cleanup backlog rows; runtime script compile inventory has 6 target rows and 0 old-path rows; serialization register has six Monster Nests rows (Server.Custom.Confictura.LizardmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Custom.Confictura.RatmanNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.MonsterNestLoot:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Items.UndeadNest:v0:SingleVersionOnly:AlignedByCountAndKnownTypes; Server.Mobiles.MonsterNestEntity:v0:SingleVersionOnly:AlignedByCountAndKnownTypes); runtime hook map has 4 Monster Nests hook rows (Timer=4). Final verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors; solution=Passed; ConficturaUO.sln Debug/Any CPU built Server and Scripts with existing warnings and no errors; server build=Passed; Server.csproj Debug/x86 built ConficturaServer.exe; compile-only=Passed; compile-only output reported Scripts: Compile-only verification completed successfully and Exiting...done.
- Output path: `docs/codebase-audit/outputs/post-batch-h-monster-nests-move-review.csv`; `docs/codebase-audit/outputs/post-batch-h-monster-nests-move-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:02:00.3519907-05:00

- Affected phase: Post-audit `POST-BATCH-H-10A` Invasion reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review Invasion folder move gate, source/project/serialization/hook evidence, and related staff workflow rows; run `New-PostBatchHInvasionGateReview.ps1`.
- Result: Classified RB-06808 as DeferredMoveGate. Source path exists with 100 runtime-visible C# files and 102 total files; target path absent; project truth has 100 includes, 100 sources, 0 missing targets, and 0 unincluded sources; serialization register has 76 rows; runtime hook map has 172 rows (Command=1; Gump=153; Initialize=1; Movement=3; Timer=14); remaining blockers are RB-05605:Random Encounters <-> Invasion balance/policy; RB-05620:Invasion <-> Champions balance/policy; RB-05630:Invasion staff workflow ownership; RB-05632:Government <-> Invasion staff workflow ownership; RB-05634:XMLSpawner <-> Invasion staff workflow ownership; RB-05645:Invasion staff tooling policy; serializer caution rows are Server.Mobiles.BaseSpecialCreature:NoVersionFound:AlignedByCountAndKnownTypes:NamespaceOrTypeRenameDanger; Server.Items.GargoyleCityInvasionStone:SingleVersionOnly:AlignedByCountAndKnownTypes:UnknownUntilSaveTest. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-invasion-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-invasion-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:07:50.9723914-05:00

- Affected phase: Post-audit `POST-BATCH-H-11A` XMLSpawner reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review XMLSpawner folder move gate, source/project/serialization/hook evidence, and related explicit approval rows; run `New-PostBatchHXMLSpawnerGateReview.ps1`.
- Result: Classified RB-06813 as NeedsHumanDecision. Source path exists with 133 runtime-visible C# files and 249 total files; target path absent; project truth has 133 includes, 133 sources, 0 missing targets, and 0 unincluded sources; Scripts.csproj has 92 Content and 19 None package rows; serialization register has 100 rows (100 NeedsSourceReview, 100 UnknownUntilSaveTest); runtime hook map has 589 rows (Command=75; Event=6; Gump=368; Initialize=23; Movement=27; Packet=3; Region=4; Speech=20; Timer=61; WorldLoad=1; WorldSave=1); remaining blockers are RB-05604:Random Encounters <-> XMLSpawner balance/policy; RB-05610:PvP Consent <-> XMLSpawner balance/policy; RB-05618:XMLSpawner <-> Champions balance/policy; RB-05619:XMLSpawner <-> Monster Nests balance/policy; RB-05629:XMLSpawner staff tooling policy; RB-05631:XMLSpawner staff tooling policy; RB-05634:XMLSpawner <-> Invasion staff workflow ownership; RB-05635:XMLSpawner staff tooling policy; RB-05636:XMLSpawner staff tooling policy; RB-05637:XMLSpawner staff tooling policy; RB-05638:XMLSpawner staff tooling policy; RB-05639:XMLSpawner staff tooling policy; RB-05640:XMLSpawner staff tooling policy; RB-05641:XMLSpawner staff tooling policy; RB-05642:XMLSpawner staff tooling policy; RB-05643:XMLSpawner staff tooling policy; RB-05644:XMLSpawner staff tooling policy; queued XMLSpawner source/schema follow-ups=78. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-xmlspawner-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-xmlspawner-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:11:26.6430965-05:00

- Affected phase: Post-audit `POST-BATCH-H-12A` Government reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review Government folder move gate, source/project/serialization/hook evidence, and related explicit approval rows; run `New-PostBatchHGovernmentGateReview.ps1`.
- Result: Classified RB-06807 as NeedsHumanDecision. Source path exists with 207 runtime-visible C# files and 208 total files; target path absent; project truth has 207 includes, 207 sources, 0 missing targets, and 0 unincluded sources; serialization register has 144 rows (144 NeedsSourceReview, 144 UnknownUntilSaveTest); runtime hook map has 227 rows (Command=8; Gump=190; Initialize=9; Region=5; Speech=10; Timer=4; WorldSave=1); remaining blockers are RB-05609:PvP Consent <-> Government balance/policy; RB-05611:Government <-> Homestead balance/policy; RB-05612:Government <-> Vendor Core balance/policy; RB-05632:Government <-> Invasion staff workflow ownership; RB-06806:PvP Consent move gate remains deferred; RB-06808:Invasion move gate remains deferred; queued Government source/schema/doc follow-ups=8. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-government-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-government-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:18:20.2467255-05:00

- Affected phase: Post-audit `POST-BATCH-H-13A` Offline Skill Training reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review Offline Skill Training folder move gate, source/project/serialization/hook evidence, and related balance/doc rows; run `New-PostBatchHOfflineSkillTrainingGateReview.ps1`.
- Result: Classified RB-06803 as DeferredMoveGate. Source path exists with 7 runtime-visible C# files and 8 total files; target path absent; project truth has 7 includes, 7 sources, 0 missing targets, and 0 unincluded sources; Scripts.csproj has 7 Compile rows and 1 Content row under the current package path; serialization register has 170 rows (166 NeedsSourceReview, 166 UnknownUntilSaveTest, 4 NamespaceOrTypeRenameDanger); runtime hook map has 8 rows (Event=4; Initialize=1; Login=1; Logout=1; Timer=1); remaining blockers are RB-05603:Character Level <-> Offline Skill Training balance/policy; RB-05608:Random Encounters <-> Offline Skill Training balance/policy; RB-06737:Offline Skill Training canonical documentation source trace. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-offline-skill-training-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-offline-skill-training-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:22:56.2977509-05:00

- Affected phase: Post-audit `POST-BATCH-H-14A` Homestead reorganization gate review
- Cwd: `D:\ConficturaUO`
- Command: review Homestead folder move gate, source/project/serialization/hook/nested-AGENTS evidence, URI-escaped Scripts.csproj paths, and related explicit approval rows; run `New-PostBatchHHomesteadGateReview.ps1`.
- Result: Classified RB-06814 as NeedsHumanDecision. Source path exists with 388 runtime-visible C# files and 402 total files; target path absent; nested AGENTS scopes=3; project truth has 388 includes, 388 sources, 0 missing targets, and 0 unincluded sources; Scripts.csproj has 388 URI-escaped Compile rows, 5 Content rows, and 6 None rows; serialization register has 707 rows (707 NeedsSourceReview, 707 UnknownUntilSaveTest); runtime hook map has 219 rows (Command=3; Gump=83; Initialize=7; Timer=126); remaining blockers are RB-05611:Government <-> Homestead balance/policy; RB-05613:Homestead <-> Crafting Core balance/policy; RB-05614:Homestead <-> Harvest System balance/policy; RB-05615:Homestead <-> Bulk Orders balance/policy; RB-05616:Homestead <-> Gardening balance/policy; RB-05617:Homestead <-> Vendor Core balance/policy; RB-05633:Homestead <-> Static Gump Tool staff tooling policy; RB-06690:Farmable Crops source trace; RB-06705:Homestead canonical source trace; RB-06735:Offline Skill Training/Homestead doc source trace; RB-06807:Government move gate remains human-gated. Verification: git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-homestead-gate-review.csv`; `docs/codebase-audit/outputs/post-batch-h-homestead-gate-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`

### 2026-06-14T00:27:21.1227216-05:00

- Affected phase: Post-audit `POST-BATCH-H` final folder/namespace cleanup reconciliation
- Cwd: `D:\ConficturaUO`
- Command: verify all folder cleanup backlog rows are represented in the active overlay; reconcile POST-BATCH-H commit hashes; write final closeout.
- Result: POST-BATCH-H has 14 active overlay rows for 14 repair-backlog folder cleanup rows; status summary is DeferredMoveGate=3; Fixed=8; NeedsHumanDecision=3; commit placeholders remaining=0; final verification git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-h-folder-namespace-cleanup-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T11:58:03.6355377-05:00

- Affected phase: Post-audit `POST-BATCH-I` ServerCore P0 save compatibility residual review
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; review residual P0 rows from `repair-backlog.csv`, current source snippets, `serialization-register.csv`, and `post-batch-b-save-compatibility-triage.csv`; run `New-PostBatchIServerCoreSaveCompatReview.ps1`.
- Result: Review CSV contains 19 POST-BATCH-I rows; active overlay contains 19 POST-BATCH-I rows; disposition summary is FalsePositive=7; IntentionalLegacy=6; SafeNoChange=6; remaining unreviewed P0 backlog rows=0; no source/project/runtime/XML/config files changed; serialization register was not regenerated because no source serializer logic or classification output changed; git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-i-servercore-save-compat-review.csv`; `docs/codebase-audit/outputs/post-batch-i-servercore-save-compat-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T12:11:35.4567553-05:00

- Affected phase: Post-audit `POST-BATCH-J` P1 save compatibility triage
- Cwd: `D:\ConficturaUO`
- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P1 save rows against `serialization-register.csv`; run `New-PostBatchJP1SaveCompatReview.ps1`.
- Result: Review CSV contains 1294 POST-BATCH-J rows; active overlay contains 1294 POST-BATCH-J rows; disposition summary is FalsePositive=146; IntentionalLegacy=667; NeedsMigrationPlan=73; QueuedSourceFollowUp=100; SafeNoChange=308; remaining unreviewed P1 save-compatibility rows=0; no source/project/runtime/XML/config files changed; serialization register was not regenerated because no runtime serializer source inputs changed; git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-j-p1-save-compat-review.csv`; `docs/codebase-audit/outputs/post-batch-j-p1-save-compat-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T12:29:33.8414388-05:00

- Affected phase: Post-audit `POST-BATCH-K` P1 runtime surface safety triage
- Cwd: `D:\ConficturaUO`
- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P1 runtime-facing rows against current source paths/lines, `runtime-hook-map.csv`, `dependency-graph.csv`, and POST-BATCH-H path moves; run `New-PostBatchKP1RuntimeSurfaceReview.ps1`.
- Result: Review CSV contains 2691 POST-BATCH-K rows; active overlay contains 2691 POST-BATCH-K rows; category summary is Command access=22; Gump guards=908; PlayerMobile coupling=386; Region and map assumptions=89; Runtime hooks=1286; disposition summary is DeferredPolicyDecision=383; QueuedSourceFollowUp=342; ReviewedNoChange=1949; SafeNoChange=17; remaining unreviewed P1 rows=0; no source/project/runtime/XML/config files changed; runtime hook map and dependency graph were not regenerated because no source/path inputs changed; git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-k-p1-runtime-surface-review.csv`; `docs/codebase-audit/outputs/post-batch-k-p1-runtime-surface-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T12:50:27.3935918-05:00

- Affected phase: Post-audit `POST-BATCH-L` P2 residual backlog triage
- Cwd: `D:\ConficturaUO`
- Command: compare `repair-backlog.csv` against `post-audit-active-backlog-status.csv`; resolve remaining P2 command-access, legacy-compatibility, save-compatibility, and region/map rows against current source paths/lines, project truth, runtime inventory, command register, and serialization register; run `New-PostBatchLP2ResidualBacklogReview.ps1`.
- Result: Review CSV contains 1186 POST-BATCH-L rows; active overlay contains 1186 POST-BATCH-L rows; category summary is Command access=477; Legacy compatibility=659; Region and map assumptions=23; Save compatibility=27; disposition summary is DeferredPolicyDecision=23; IntentionalLegacy=659; QueuedSourceFollowUp=174; ReviewedNoChange=326; SafeNoChange=4; remaining unreviewed repair-backlog rows=0; no source/project/runtime/XML/config files changed; project truth/runtime hook/dependency/serialization outputs were not regenerated because no source/path inputs changed; git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-l-p2-residual-backlog-review.csv`; `docs/codebase-audit/outputs/post-batch-l-p2-residual-backlog-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T13:15:22.7769319-05:00

- Affected phase: Post-audit `POST-BATCH-M` command access source review
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-l-p2-residual-backlog-review.csv` to `Decision=QueuedSourceFollowUp`; verify 174 P2 `Command access` rows; source-review command registrations, helper access variables, wrappers, parser line offsets, and policy cases; run `New-PostBatchMCommandAccessSourceReview.ps1`.
- Result: Review CSV contains 174 POST-BATCH-M rows; active overlay contains 174 POST-BATCH-M rows; active POST-BATCH-L `QueuedSourceFollowUp` rows=0; disposition summary is FalsePositive=2; IntentionalLegacy=11; NeedsHumanDecision=2; ReviewedNoChange=159; system summary is System:Commands=80; XMLSpawner=53; Regions=13; Spell Framework=10; System:Misc=7; Custom:NPC Control=4; Clone Offline Player Characters=3; Items:Doors=2; Housing=1; System:Chat=1; review-class summary is PlayerSpellbarCommandSurface=67; XMLSpawnerStaffCommandSurface=32; XMLPointsCommandSurface=19; PassthroughHelperResolved=12; IntentionalLegacyCommandSurface=11; PlayerSpellCommandSurface=10; DirectCommandAccessResolved=8; AccessVariableResolved=7; CompatibilityWrapperResolved=2; GenericCommandFramework=2; ParserLineOffsetFalsePositive=2; ConfigDrivenCommandRehashPolicy=1; PlayerConsoleDumpPolicy=1; remaining unreviewed repair-backlog rows=0; no source/project/runtime/XML/config files changed; source build and runtime compile were not run because this is audit-only; git diff --check=Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors.
- Output path: `docs/codebase-audit/outputs/post-batch-m-command-access-source-review.csv`; `docs/codebase-audit/outputs/post-batch-m-command-access-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T13:37:35.8427499-05:00

- Affected phase: Post-audit `POST-BATCH-N` source-readiness queue triage
- Cwd: `D:\ConficturaUO`
- Command: `git status --short`; `rg --files -g AGENTS.md`; read root and audit-local instructions, the root audit plan, detailed phase plans, and relevant POST-BATCH-F/J/K/M closeouts; classify every active `QueuedSourceFollowUp` row from `post-audit-active-backlog-status.csv`; write `post-batch-n-source-readiness-queue.csv`; update the active overlay, `PHASE_STATUS.md`, `outputs/README.md`, and this log.
- Result: Input active overlay contained exactly 899 `QueuedSourceFollowUp` rows. POST-BATCH-N queue contains 899 rows with readiness summary ReadyForSourceBatch=453; SchemaDocsOnly=232; DocsOnly=133; NeedsMigrationPlan=77; DeferredPolicyDecision=4. Lane summary is POST-BATCH-N-GUMP-GUARD-SOURCE-BATCH=235; POST-BATCH-N-RUNTIME-HOOK-SOURCE-BATCH=103; POST-BATCH-N-STAFF-COMMAND-METADATA=92; POST-BATCH-N-SAVE-CONSTRUCTOR-PERSISTENCE-REVIEW=23; POST-BATCH-N-SAVE-MIGRATION-PLAN=77; POST-BATCH-N-SCHEMA-DOCUMENTATION=232; POST-BATCH-N-DOCS-SOURCE-TRACE=133; POST-BATCH-N-REGION-POLICY-DESIGN=4. All 899 rows have `FileProofStatus=CurrentPathConfirmed`; Static Gump Tool and Random Encounters stale paths were resolved through POST-BATCH-H move evidence. Active overlay now contains 899 POST-BATCH-N rows and 0 active `QueuedSourceFollowUp` rows. Changed-file review found no `Data/`, project, source, XML/config, or runtime behavior files changed. `git diff --check` passed with expected LF-to-CRLF working-copy warnings and no whitespace errors. Source build and runtime compile were not run because this batch is audit-output only.
- Output path: `docs/codebase-audit/outputs/post-batch-n-source-readiness-queue.csv`; `docs/codebase-audit/outputs/post-batch-n-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T14:40:06.0684849-05:00

- Affected phase: Post-audit `POST-BATCH-O` gump guard source repair
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` to `ReadyForSourceBatch` + `POST-BATCH-N-GUMP-GUARD-SOURCE-BATCH`; review 235 rows; edit confirmed gump/prompt/hue-picker response guard defects; generate POST-BATCH-O review/closeout; update active overlay and audit ledgers; run targeted static guard scans, `git diff --check`, `msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86`, and `.\ConficturaServer.exe -compileonly -nocache`.
- Result: Review CSV contains 235 POST-BATCH-O rows; active overlay contains 235 POST-BATCH-O rows; disposition summary is Fixed=84; SafeNoChange=140; FalsePositive=7; ReviewedNoChange=4; active POST-BATCH-N gump guard source-batch unreviewed rows=0. Source fixes added response-time null/deleted/stale-state, text-entry, hue-picker, and index guards while preserving public APIs, namespaces, type names, file locations, serialization layout, project/XML/config files, command names, and button IDs. Verification: targeted invariants passed; queued OnResponse guard scan reports only the `IMenu` interface signature; rename prompt guards=25; `git diff --check` passed with expected LF-to-CRLF warnings only; initial sandboxed MSBuild attempt failed on user-profile SDK access, escalated rerun passed with 0 warnings and 0 errors; `.\ConficturaServer.exe -compileonly -nocache` passed with compile-only verification completed successfully.
- Output path: `docs/codebase-audit/outputs/post-batch-o-gump-guard-source-review.csv`; `docs/codebase-audit/outputs/post-batch-o-gump-guard-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-14T15:28:15.5263451-05:00

- Affected phase: Post-audit `POST-BATCH-P` runtime hook source repair
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` for `ReadyForSourceBatch` + `POST-BATCH-N-RUNTIME-HOOK-SOURCE-BATCH`; review all 103 rows against source; apply confirmed runtime-hook guard fixes; generate review/closeout outputs; replace active overlay rows; run targeted scans, `git diff --check`, Visual Studio MSBuild, and compile-only script verification.
- Result: Review CSV rows=103; disposition summary FalsePositive=8; Fixed=80; SafeNoChange=15; review class summary AmbientMovementGuardFixed=10; EmptyRuntimeHookFalsePositive=3; ExistingEventGuardSafeNoChange=1; ExistingMovementGuardSafeNoChange=1; FrameworkCallbackReviewedNoChange=3; LegacyHookGuardFixed=2; MovementHookGuardFixed=31; NoSideEffectCallbackFalsePositive=2; PriorGuardOrExistingGuardSafeNoChange=12; RegionHookGuardFixed=4; SendGumpCallbackGuardFixed=17; ServerBootstrapReviewedNoChange=1; SpeechHookGuardFixed=8; SpellCallbackGuardFixed=1; StalePlayerStateGuardFixed=4; TargetCallbackGuardFixed=3; active overlay `POST-BATCH-P` rows=103; remaining runtime-hook source-batch rows=0; active `QueuedSourceFollowUp` rows=0. Final `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors after sandbox escalation; final `.\ConficturaServer.exe -compileonly -nocache` passed.
- Output path: `docs/codebase-audit/outputs/post-batch-p-runtime-hook-source-review.csv`; `docs/codebase-audit/outputs/post-batch-p-runtime-hook-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`.

### 2026-06-14T16:17:42.2744356-05:00

- Affected phase: Post-audit `POST-BATCH-Q` staff command metadata source repair
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` for `ReadyForSourceBatch` + `POST-BATCH-N-STAFF-COMMAND-METADATA`; review all 92 rows against current source registrations and command handlers; apply confirmed command parser/mobile guard fixes; generate review/closeout outputs; replace active overlay rows; prepare targeted scans, `git diff --check`, Visual Studio MSBuild, and compile-only script verification.
- Result: Review CSV rows=92; disposition summary Fixed=86; SafeNoChange=5; ReviewedNoChange=1; system summary XMLSpawner=68; Static Gump Tool=22; Custom:ChangeSeason=1; Custom:Character Swap=1; active overlay `POST-BATCH-Q` rows=92; remaining staff-command metadata source-batch rows=0; remaining active source-readiness rows=23; active `QueuedSourceFollowUp` rows=0. Verification passed: targeted command/access/metadata scans; git diff --check with expected LF-to-CRLF warnings only; initial sandboxed Visual Studio MSBuild hit user-profile SDK access denial; escalated Data/System/Source/Server.csproj Debug|x86 build passed with 0 warnings and 0 errors; .\ConficturaServer.exe -compileonly -nocache passed with compile-only verification completed successfully.
- Output path: `docs/codebase-audit/outputs/post-batch-q-staff-command-metadata-source-review.csv`; `docs/codebase-audit/outputs/post-batch-q-staff-command-metadata-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`.

### 2026-06-14T20:07:03.0000000-05:00

- Affected phase: Post-audit `POST-BATCH-R` save constructor persistence source review
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` for `ReadyForSourceBatch` + `POST-BATCH-N-SAVE-CONSTRUCTOR-PERSISTENCE-REVIEW`; confirm 23 active POST-BATCH-N rows; review each Custom:XMLSpawner attachment class for local `ASerial` constructor, `base(serial)`, serializer version read/write, and writer/reader field alignment; generate review/closeout outputs; replace active overlay rows.
- Result: Review CSV rows=23; disposition summary FalsePositive=23; review class summary XMLSpawnerASerialConstructorFalsePositive=23; active overlay `POST-BATCH-R` rows=23; remaining save-constructor persistence source-batch rows=0; remaining active ReadyForSourceBatch rows=0; active QueuedSourceFollowUp rows=0. Verification passed: targeted ASerial constructor/base(serial) scan; targeted serializer read/write alignment scan; git diff --check with expected LF-to-CRLF warnings only; no C# source changed, so Server.csproj build and compile-only script verification were not required.
- Output path: `docs/codebase-audit/outputs/post-batch-r-save-constructor-persistence-review.csv`; `docs/codebase-audit/outputs/post-batch-r-save-constructor-persistence-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`.

### 2026-06-14T20:33:44.4399529-05:00

- Affected phase: Post-audit `POST-BATCH-S` schema documentation queue
- Cwd: `D:\ConficturaUO`
- Command: process the 232 active `SchemaDocsOnly` rows in `POST-BATCH-N-SCHEMA-DOCUMENTATION`; resolve each row to current source evidence; generate POST-BATCH-S review/closeout; update active overlay, phase status, run log, and outputs README; run docs/path/source-trace checks and `git diff --check`.
- Result: Generated review CSV rows=232 with decision summary Documented=226; FalsePositive=6 and review-class summary PathConstantOrSchemaReference=130; XmlDocumentSchemaSurface=40; GeneratedOrWrittenDataSurface=38; DirectoryGlobSchemaSurface=10; DiagnosticStringNotSchemaPath=6; TextConfigReadSurface=6; ExistenceGateSchemaSurface=2. Active overlay POST-BATCH-S rows=232; remaining active POST-BATCH-N schema documentation rows=0; remaining active SchemaDocsOnly rows=0; remaining active ReadyForSourceBatch rows=0; active QueuedSourceFollowUp rows=0. Verification passed: row/count/source-trace check confirmed all 232 review rows resolve to current source evidence, changed-file scope is limited to docs/codebase-audit artifacts, and `git diff --check` passed with expected LF-to-CRLF working-copy warnings only. No source/project/XML/config/data behavior files changed, so source build and compile-only runtime verification were not required.
- Output path: `docs/codebase-audit/outputs/post-batch-s-schema-documentation-review.csv`; `docs/codebase-audit/outputs/post-batch-s-schema-documentation-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`
### 2026-06-14T21:09:11.0049244-05:00

- Affected phase: Post-audit POST-BATCH-T docs source trace queue
- Cwd: D:\ConficturaUO
- Command: process 133 active DocsOnly rows in POST-BATCH-N-DOCS-SOURCE-TRACE; add/refresh canonical source traces, retire alias pages to canonical routing, generate review/closeout outputs, and replace active overlay rows.
- Result: Generated review CSV rows=133; decision summary Documented=126; ReviewedNoChange=7; review class summary AliasRetiredToCanonical=4; CanonicalSourceTraceAdded=122; SupportArtifactReviewedNoChange=6; UnknownDocReviewedNoChange=1; changed docs=99; remaining active POST-BATCH-N docs-source-trace rows=0; remaining active ReadyForSourceBatch rows=0; active QueuedSourceFollowUp rows=0. Verification passed: custom row/source-trace/path/scope check confirmed all 133 rows are reviewed, all changed wiki docs have Source Trace headings or alias rationale, no Missing path markers remain, and changed scope is docs-only; git diff --check passed with expected LF-to-CRLF warnings only. No source/project/XML/config/data behavior changed, so source build and compile-only runtime verification were not required.
- Output path: docs/codebase-audit/outputs/post-batch-t-docs-source-trace-review.csv; docs/codebase-audit/outputs/post-batch-t-docs-source-trace-closeout.md; docs/codebase-audit/outputs/post-audit-active-backlog-status.csv

### 2026-06-14T22:49:23.8796880-05:00

- Affected phase: Post-audit `POST-BATCH-U` save migration plan queue
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` for `NeedsMigrationPlan` + `POST-BATCH-N-SAVE-MIGRATION-PLAN`; confirm 77 active POST-BATCH-N rows; review each serializer against `serialization-register.csv`, linked prior review evidence, and current source writer/reader blocks; generate review/closeout outputs; replace active overlay rows.
- Result: Generated review CSV rows=77; decision summary FalsePositive=56; ReviewedNoChange=21; review class summary ScannerAmbiguityFalsePositive=56; HelperSerializerReviewedNoChange=19; VersionGatedSerializerReviewedNoChange=2. No source migration was approved because current source review found no active serializer layout defect: regular serializers were scanner ambiguity false positives and helper serializers were valid helper persistence contracts. Active overlay POST-BATCH-U rows=77; remaining active POST-BATCH-N save migration-plan rows=0; remaining active POST-BATCH-N region policy-design rows=4. Verification passed: row/count reconciliation returned `review=77 queue=77 missing=0 extra=0`; evidence completeness returned `bad=0`; active overlay reconciliation returned `post_batch_u=77 remaining_post_batch_n_migration=0 missing_review_overlay=0`; changed-file scope is limited to audit artifacts; no source/project/XML/config/data behavior files changed; `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Output path: `docs/codebase-audit/outputs/post-batch-u-save-migration-plan-review.csv`; `docs/codebase-audit/outputs/post-batch-u-save-migration-plan-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-15T09:53:19.6191586-05:00

- Affected phase: Post-audit `POST-BATCH-V` region policy design queue
- Cwd: `D:\ConficturaUO`
- Command: filter `post-batch-n-source-readiness-queue.csv` for `DeferredPolicyDecision` + `POST-BATCH-N-REGION-POLICY-DESIGN`; confirm 4 active POST-BATCH-N rows; review HouseRegion, Obsolete.cs, and PirateRegion behavior against current source, runtime-hook-map, POST-BATCH-K evidence, and helper region logging/music source; generate review/closeout outputs; replace active overlay rows.
- Result: Generated review CSV rows=4; decision summary PolicyResolvedNoChange=3; FalsePositive=1; review class summary HouseRegionPolicyResolvedNoChange=1; ObsoleteQuestGumpFalsePositive=1; PirateRegionPolicyResolvedNoChange=2. HouseRegion owner vendor-notice behavior and PirateRegion entry/exit behavior were policy-resolved as current intentional guarded behavior; the Obsolete.cs row was classified as a false positive on quest gump UI. Active overlay POST-BATCH-V rows=4; remaining active POST-BATCH-N region policy-design rows=0. Verification passed: row/count reconciliation returned `review=4 queue=4 missing=0 extra=0`; evidence completeness returned `bad=0`; active overlay reconciliation returned `post_batch_v=4 remaining_post_batch_n_region_policy=0 missing_review_overlay=0`; changed-file scope is limited to audit artifacts; no source/project/XML/config/data behavior files changed; `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Output path: `docs/codebase-audit/outputs/post-batch-v-region-policy-design-review.csv`; `docs/codebase-audit/outputs/post-batch-v-region-policy-design-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-15T10:29:12.9182888-05:00

- Affected phase: Post-audit `POST-BATCH-W` historical save migration plan queue
- Cwd: `D:\ConficturaUO`
- Command: filter `post-audit-active-backlog-status.csv` for active historical `POST-BATCH-J` rows with `ActiveStatus=NeedsMigrationPlan` and `Category=Save compatibility`; confirm 73 rows; join each row to `post-batch-j-p1-save-compat-review.csv` and `serialization-register.csv`; source-review helper serializers, XMLSpawner attachment persistence, version-gated legacy serializers, scanner ambiguity rows, and the legacy `HouseFoundation` base-order serializer; generate review/closeout outputs; replace active overlay rows.
- Result: Generated review CSV rows=73; decision summary FalsePositive=11; MigrationPlanReady=1; ReviewedNoChange=61; review class summary HelperSerializerReviewedNoChange=52; LegacyBaseOrderMigrationPlanReady=1; ScannerAmbiguityFalsePositive=11; VersionGatedSerializerReviewedNoChange=3; XMLSpawnerAttachmentReviewedNoChange=6. No serializer source edit was approved in POST-BATCH-W. The only migration-plan row is `Server.Multis.HouseFoundation`, whose subclass-before-base order is self-consistent but migration-sensitive and must not be normalized without explicit save conversion or dual-loader approval. Active overlay POST-BATCH-W rows=73; remaining active historical POST-BATCH-J `NeedsMigrationPlan` rows=0; remaining active `QueuedSourceFollowUp` rows=0; remaining active `ReadyForSourceBatch` rows=0. Verification passed: review/count reconciliation returned `review_count=73`; evidence completeness returned `bad_evidence=0`; active overlay reconciliation returned `remaining_post_batch_j_needs_migration=0` and `overlay_post_batch_w=73`; changed-file scope is limited to audit artifacts; no source/project/XML/config/data behavior files changed; `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Output path: `docs/codebase-audit/outputs/post-batch-w-historical-save-migration-plan-review.csv`; `docs/codebase-audit/outputs/post-batch-w-historical-save-migration-plan-closeout.md`; `docs/codebase-audit/outputs/post-audit-active-backlog-status.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-15T11:25:23.6607652-05:00

- Affected phase: Post-audit POST-BATCH-X PlayerMobile coupling policy decision queue
- Cwd: D:\ConficturaUO
- Command: filter post-audit-active-backlog-status.csv for historical POST-BATCH-K rows with Category=PlayerMobile coupling and ActiveStatus=DeferredPolicyDecision; confirm 383 rows; classify each against POST-BATCH-K evidence, current source exact/context scans, dependency graph, runtime hook map, serialization register, and system card index; generate review/closeout outputs; replace active overlay rows.
- Result: Generated review CSV rows=383; decision summary FalsePositive=2; PolicyResolvedNoChange=369; ReviewedNoChange=12; coupling kind summary PlayerTypeCheckOrCast=257; PvPRegionCombatPolicyCoupling=91; PlayerMobilePropertyStateAccess=15; StoredPlayerMobileFieldOrParameter=12; PlayerMobileCorePolicySurface=6; DocsOnlyOrStaleEvidence=2; active overlay POST-BATCH-X rows=383; remaining raw POST-BATCH-K PlayerMobile coupling DeferredPolicyDecision rows=0; evidence completeness bad_evidence=0; future source-batch rows opened=0. Final git diff --check passed with expected LF-to-CRLF warnings only.
- Output path: docs/codebase-audit/outputs/post-batch-x-playermobile-coupling-policy-review.csv; docs/codebase-audit/outputs/post-batch-x-playermobile-coupling-policy-closeout.md; docs/codebase-audit/outputs/post-audit-active-backlog-status.csv; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T11:56:52.1526354-05:00

- Affected phase: Post-audit POST-BATCH-Y source-change readiness gates
- Cwd: D:\ConficturaUO
- Command: filter post-audit-active-backlog-status.csv for NeedsHumanDecision, DeferredBalanceDecision, DeferredPolicyDecision, DeferredMoveGate, and MigrationPlanReady; confirm 90 rows; classify each as AcceptedFence, BlocksOnlyThisDomain, or BlocksSourceWork; generate residual gate register and closeout; replace active overlay statuses.
- Result: gate register rows=90; gate scope summary AcceptedFence=83; BlocksOnlyThisDomain=7; decision summary BlockedForReorganizationOnly=6; BlockedForSerializerMigrationOnly=1; PreserveCurrentBalancePolicy=26; PreserveCurrentCommandAccessPolicy=2; PreserveCurrentRegionPolicy=23; PreserveCurrentStaffWorkflow=32; POST-BATCH-Y overlay rows=90; old residual gate statuses remaining=0; completeness bad_rows=0. Final git diff --check passed with expected LF-to-CRLF warnings only.
- Output path: docs/codebase-audit/outputs/post-batch-y-source-change-gate-register.csv; docs/codebase-audit/outputs/post-batch-y-source-change-readiness-closeout.md; docs/codebase-audit/outputs/post-audit-active-backlog-status.csv; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T12:30:09.7126988-05:00

- Affected phase: Post-audit POST-BATCH-Z first source-change selection
- Cwd: D:\ConficturaUO
- Command: reconcile phase-13-batch-plan.csv, repair-backlog.csv, post-audit-active-backlog-status.csv, and POST-BATCH-Y gate artifacts; confirm no unresolved pre-source statuses; confirm POST-BATCH-Y gate counts; select the first allowed source-change boundary.
- Result: active overlay unresolved pre-source statuses=0; POST-BATCH-Y gate summary AcceptedFence=83; BlocksOnlyThisDomain=7; BlocksSourceWork=0; Phase 13 BATCH-001 Project include drift active rows Fixed=61 and not selected as live source-code repair; Phase 13 BATCH-002 Packet handlers active rows Fixed=3 and ReviewedNoChange=14 and not reopened; selected SOURCE-BATCH-001 Non-Gated Focused Source Change as the first allowed boundary with excluded gate areas and verification requirements recorded; `git diff --check` passed with expected LF-to-CRLF warnings only. No source/project/XML/config/data behavior changed.
- Output path: docs/codebase-audit/outputs/post-batch-z-first-source-change-selection.csv; docs/codebase-audit/outputs/post-batch-z-first-source-change-selection-closeout.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T12:40:26.6480047-05:00

- Affected phase: Post-audit POST-BATCH-AA source batch roadmap
- Cwd: D:\ConficturaUO
- Command: use post-audit-active-backlog-status.csv as source of truth; reconcile POST-BATCH-Y and POST-BATCH-Z fences; lay out remaining logical source-change batches in sequential order with goal templates.
- Result: active overlay unresolved pre-source statuses=0; POST-BATCH-Y gate summary AcceptedFence=83; BlocksOnlyThisDomain=7; BlocksSourceWork=0; roadmap rows=7; only SOURCE-BATCH-001 is selected as the immediate source-change boundary; all gated categories are conditional only; ready goal templates recorded for non-gated, staff/access, balance, region/map, HouseFoundation migration, and reorganization batches; `git diff --check` passed with expected LF-to-CRLF warnings only. No source/project/XML/config/data behavior changed.
- Output path: docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap.csv; docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap-closeout.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T12:47:55.6250644-05:00

- Affected phase: SOURCE-BATCH-CONTROLLER roadmap execution status
- Cwd: D:\ConficturaUO
- Command: process POST-BATCH-AA roadmap in order; confirm no concrete non-gated source target exists in the thread; record gated roadmap batches as blocked pending explicit approval.
- Result: active overlay unresolved pre-source statuses=0; roadmap rows=7; immediate executable rows=1; selected immediate boundary rows=1; gated rows=5; POST-BATCH-Y gate summary AcceptedFence=83; BlocksOnlyThisDomain=7; BlocksSourceWork=0. SOURCE-BATCH-001 and SOURCE-BATCH-002+ recorded as PendingConcreteSourceTarget; five gated batches recorded as BlockedPendingApproval; `git diff --check` passed with expected LF-to-CRLF warnings only. No source/project/XML/config/data behavior changed.
- Output path: docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T15:45:07.1199481-05:00

- Affected phase: SOURCE-BATCH-001 intake
- Cwd: D:\ConficturaUO
- Command: record interview-selected SOURCE-BATCH-001 target; verify OilCloth POST-BATCH-Y gate hits; update intake register and controller status.
- Result: selected target is OilCloth guard repair in Data/Scripts/Items/Misc/OilCloth.cs; behavior is stale/null/backpack guards for OilCloth interaction paths; must preserve poison-charge reduction, firebomb conversion, oil cloth consumption semantics, messages, targeting flow, serialization, region/map behavior, economy/reward tuning, staff/access behavior, and folder/namespace/package layout; OilCloth.cs POST-BATCH-Y gate hits=0; SOURCE-BATCH-001 intake status=ReadyForSourceBatch; SOURCE-BATCH-002+ remains PendingConcreteSourceTarget; staff/access, balance, region/map, HouseFoundation serializer migration, and reorganization remain BlockedPendingApproval; `git diff --check` passed with expected LF-to-CRLF warnings only. No source/project/XML/config/data behavior changed.
- Output path: docs/codebase-audit/outputs/source-batch-001-target.md; docs/codebase-audit/outputs/source-batch-intake-register.csv; docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T16:44:08.0818812-05:00

- Affected phase: SOURCE-BATCH-001 OilCloth Guard Repair
- Cwd: D:\ConficturaUO
- Command: implement SOURCE-BATCH-001 in `Data/Scripts/Items/Misc/OilCloth.cs`; add stale/null/backpack guards to OilCloth `OnDoubleClick` and `OnTarget`; verify OilCloth POST-BATCH-Y gate hits; run targeted source and serializer scans; build `Data/System/Source/Server.csproj` Debug/x86; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch closeout and controller artifacts.
- Result: OilCloth now guards null/deleted mobiles, deleted or out-of-backpack cloth state, missing backpacks, null target objects, and deleted targeted items before continuing existing interaction behavior. Poison-charge reduction, firebomb conversion, oil cloth consumption semantics, localized messages, targeting flow, serialization layout/versioning, region/map behavior, economy/reward tuning, staff/access behavior, and folder/namespace/package layout were preserved. OilCloth.cs POST-BATCH-Y gate hits=0; no gated approval crossed. Targeted source scan found the new guards and unchanged serializer methods; serializer diff scan returned no serialization changes. Initial sandboxed MSBuild attempt failed on denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`; approved rerun passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warnings only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: Data/Scripts/Items/Misc/OilCloth.cs; docs/codebase-audit/outputs/source-batch-001-oilcloth-guard-repair-closeout.md; docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/outputs/source-batch-intake-register.csv; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/RUN_LOG.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T18:45:22.3593239-05:00

- Affected phase: SOURCE-BATCH-002 OilCloth Dye Scissor Guard Repair
- Cwd: D:\ConficturaUO
- Command: implement SOURCE-BATCH-002 in `Data/Scripts/Items/Misc/OilCloth.cs`; add stale/null guards to OilCloth `Dye` and `Scissor`; verify OilCloth POST-BATCH-Y gate hits and active overlay matches; run targeted source and serializer scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilCloth `Dye` now returns `false` for deleted OilCloth state, null/deleted mobiles, or null/deleted dye tubs before reading `sender.DyedHue`. OilCloth `Scissor` now returns `false` for deleted OilCloth state, null/deleted mobiles, null/deleted scissors, or mobiles that cannot see the OilCloth before calling `ScissorHelper`. Existing dye hue assignment, scissor bandage conversion, SOURCE-BATCH-001 `OnDoubleClick`/`OnTarget` guards, poison cleaning, firebomb conversion, oil cloth consumption semantics, localized messages, targeting flow, serialization layout/versioning, region/map behavior, economy/reward tuning, staff/access behavior, folder/namespace/type layout, and project files were preserved. OilCloth.cs POST-BATCH-Y gate hits=0; active overlay OilCloth matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warnings only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: Data/Scripts/Items/Misc/OilCloth.cs; docs/codebase-audit/outputs/source-batch-002-target.md; docs/codebase-audit/outputs/source-batch-002-oilcloth-dye-scissor-guard-repair-closeout.md; docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/outputs/source-batch-intake-register.csv; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/RUN_LOG.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T19:36:50.2500868-05:00

- Affected phase: SOURCE-BATCH-003 Firebomb Interaction Guard Repair
- Cwd: D:\ConficturaUO
- Command: implement SOURCE-BATCH-003 in `Data/Scripts/Items/Misc/Firebomb.cs`; add stale/null/backpack guards to Firebomb `OnDoubleClick` and `OnFirebombTarget`; verify Firebomb POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Firebomb `OnDoubleClick` now returns before dereferencing interaction state when the Firebomb is deleted, the mobile is null/deleted, the backpack is missing, or the Firebomb is no longer in the backpack. Firebomb `OnFirebombTarget` now returns before target processing when the Firebomb is deleted, the mobile is null/deleted, the backpack is missing, the Firebomb is no longer in the backpack, or the Firebomb map is null/internal. Firebomb and FirebombField serialization, FirebombField transient no-payload behavior, timer scheduling/callbacks, damage values, range, effects, messages, targeting flow, item internalization, region/map behavior, economy/reward tuning, staff/access behavior, command access, folder/namespace/type layout, project files, and XML/config/data files were preserved. Firebomb.cs POST-BATCH-Y gate hits=0; active overlay Firebomb matches=3 and all are SafeNoChange or IntentionalLegacy serializer rows; no gated approval crossed. Targeted source scan found the new guards and unchanged valid paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no timer, hook, command, gump, packet, region, startup, project, XML/config/data, or reorganization diff hits. Initial sandboxed MSBuild attempt failed on denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`; approved rerun passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: Data/Scripts/Items/Misc/Firebomb.cs; docs/codebase-audit/outputs/source-batch-003-target.md; docs/codebase-audit/outputs/source-batch-003-firebomb-interaction-guard-repair-closeout.md; docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/outputs/source-batch-intake-register.csv; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/RUN_LOG.md; docs/codebase-audit/outputs/README.md

### 2026-06-15T19:41:56.9271291-05:00

- Affected phase: SOURCE-BATCH-RUNNER closeout
- Cwd: D:\ConficturaUO
- Command: after committing SOURCE-BATCH-003, read the current source-batch controller status, POST-BATCH-Y gate register, and active overlay residual statuses; record the SOURCE-BATCH-003 commit hash and determine whether another approved source-safe batch can open.
- Result: SOURCE-BATCH-003 was committed as `daca74d1 fix: guard Firebomb interactions`. Current controller rows=9; committed non-gated source batches=3; `SOURCE-BATCH-004+` remains `PendingConcreteSourceTarget`; no concrete approved source-safe target is available after SOURCE-BATCH-003. POST-BATCH-Y gate scope remains AcceptedFence=83 and BlocksOnlyThisDomain=7, with staff/access, command-access, balance/economy, region/map, HouseFoundation serializer migration, broader serializer/migration, project/XML/config/data, and reorganization lanes still requiring explicit approval before source edits.
- Output path: docs/codebase-audit/outputs/source-batch-003-firebomb-interaction-guard-repair-closeout.md; docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv; docs/codebase-audit/outputs/source-batch-controller-closeout.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/RUN_LOG.md

### 2026-06-15T20:19:18.7694914-05:00

- Affected phase: Source change executive decision intake
- Cwd: D:\ConficturaUO
- Command: generate `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv` from `post-batch-y-source-change-gate-register.csv`, `post-batch-aa-source-batch-roadmap.csv`, and `source-batch-controller-roadmap-status.csv`; translate remaining gate rows into executive plain-English decision rows with recommended defaults, risks, automation guidance, required approval specifics, and blank answer columns.
- Result: Generated 94 CSV rows: 90 POST-BATCH-Y gate rows, one `SOURCE-BATCH-004+` next-target row, and three global control rows for automation policy, reorganization policy, and serialization policy. Validation passed: `rows=94`, `gate_rows=90`, `blank_executive_decision=94`, `blank_executive_details=94`, `missing_gate_ids=0`, `extra_gate_ids=0`. No source/project/XML/config/data behavior files changed.
- Output path: docs/codebase-audit/outputs/source-change-executive-decision-intake.csv; docs/codebase-audit/outputs/README.md; docs/codebase-audit/PHASE_STATUS.md; docs/codebase-audit/RUN_LOG.md

### 2026-06-16T09:32:30.6099096-05:00

- Affected phase: SOURCE-DECISION-INTAKE executive decision recording
- Cwd: `D:\ConficturaUO`
- Command: re-read applicable `AGENTS.md` files; import `source-change-executive-decision-intake.csv`, `post-batch-y-source-change-gate-register.csv`, `source-batch-controller-roadmap-status.csv`, and `source-batch-controller-closeout.md`; populate blank `ExecutiveDecision` and `ExecutiveDecisionDetails` cells according to the executive policy plan; preserve `EXEC-0001`; re-import and reconcile counts, gate IDs, category decisions, changed-file scope, and `git diff --check`.
- Result: Updated 94 CSV rows with no blank `ExecutiveDecision` or `ExecutiveDecisionDetails` values. Validation passed: rows=94; GateRow=90; GlobalControl=3; NextTarget=1; missing POST-BATCH-Y gate IDs=0; extra POST-BATCH-Y gate IDs=0. Decision counts matched the requested plan: Non-gated source work / Ask for candidate discovery list=1; Automation policy / Sequential runner only for non-gated rows=1; Reorganization policy / File moves only=1; Serialization policy / Preserve by default=1; Staff tooling / Preserve current workflow=32; Command access / Preserve current access=2; Economy and reward loops / Preserve current tuning=26; Region and map assumptions / Preserve current policy=23; Folder and namespace cleanup / Approve file moves only=6; Save compatibility / Preserve current order=1. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-change-executive-decision-intake.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T10:10:56.0324952-05:00

- Affected phase: SOURCE-BATCH-004 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: use `source-change-executive-decision-intake.csv` as executive source of truth; confirm `EXEC-0001=Ask for candidate discovery list`; confirm `SOURCE-BATCH-004+` remains `PendingConcreteSourceTarget`; inspect narrow Items:Misc interaction surfaces; check candidate files against POST-BATCH-Y gate register and active overlay; generate discovery CSV and closeout; validate candidate fields and changed-file scope.
- Result: Generated 5 candidate rows for `SOURCE-BATCH-004`. Recommended next target is `SB004-CAND-001` / `SOURCE-BATCH-004 ArcaneGem Interaction Guard Repair`. Validation passed: candidate rows=5; recommended target count=1; required candidate fields blank=0; nonzero POST-BATCH-Y gate candidate count=0; recommended gate hits=0. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-004-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-004-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T11:15:37.0000000-05:00

- Affected phase: SOURCE-BATCH-004 ArcaneGem Interaction Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-004 in `Data/Scripts/Items/Misc/ArcaneGem.cs`; add stale/null/backpack guards to ArcaneGem `OnDoubleClick` and `OnTarget`; verify ArcaneGem POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: ArcaneGem now guards null/deleted mobiles, deleted or out-of-backpack ArcaneGem state, missing backpacks, and deleted/out-of-backpack target items before reading backpack, target item resource/loot state, tailoring skill state, or consuming the gem. Arcane charge math, `DefaultArcaneHue`, tailoring thresholds, item eligibility, resource restrictions, blessed-item behavior, messages, gem amount/delete semantics, serialization layout/versioning, region/map behavior, economy/reward tuning, staff/access behavior, folder/namespace/type layout, project files, and XML/config/data files were preserved. ArcaneGem.cs POST-BATCH-Y gate hits=0; active overlay ArcaneGem matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. Initial unqualified `msbuild` was unavailable on PATH; Visual Studio MSBuild sandboxed run failed on denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`; approved rerun passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Misc/ArcaneGem.cs`; `docs/codebase-audit/outputs/source-batch-004-target.md`; `docs/codebase-audit/outputs/source-batch-004-arcanegem-interaction-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T11:22:52.2093583-05:00

- Affected phase: SOURCE-BATCH-005 PowerCrystal Target Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-005 in `Data/Scripts/Items/Misc/PowerCrystal.cs`; add stale/null/backpack guards to PowerCrystal `OnDoubleClick` and `PowerTarget.OnTarget`; verify PowerCrystal POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: PowerCrystal now guards null/deleted mobiles, deleted or out-of-backpack PowerCrystal state, missing backpacks, null/deleted/out-of-backpack source crystal state, and deleted/out-of-backpack target golem porter items before reading backpack, target charge state, or consuming the crystal. Golem porter target eligibility, charge cap, charge increment behavior, messages, sound/revealing behavior, `InvalidateProperties`, crystal consumption, serialization layout/versioning, region/map behavior, economy/reward tuning, staff/access behavior, folder/namespace/type layout, project files, and XML/config/data files were preserved. PowerCrystal.cs POST-BATCH-Y gate hits=0; active overlay PowerCrystal matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Misc/PowerCrystal.cs`; `docs/codebase-audit/outputs/source-batch-005-target.md`; `docs/codebase-audit/outputs/source-batch-005-powercrystal-target-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T11:29:34.9621783-05:00

- Affected phase: SOURCE-BATCH-006 ClockworkAssembly Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-006 in `Data/Scripts/Items/Misc/ClockworkAssembly.cs`; add stale/null/backpack guards to ClockworkAssembly `OnDoubleClick`; verify ClockworkAssembly POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: ClockworkAssembly now guards null/deleted mobiles, deleted or out-of-backpack assembly state, and missing backpacks before reading backpack, skill, follower, resource, or golem construction state. Tinkering threshold, follower-slot requirement, scalar calculation, resource list and quantities, consume-order result messages, golem creation/control behavior, assembly deletion behavior, serialization layout/versioning, region/map behavior, economy/reward tuning, staff/access behavior, folder/namespace/type layout, project files, and XML/config/data files were preserved. ClockworkAssembly.cs POST-BATCH-Y gate hits=0; active overlay ClockworkAssembly matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Misc/ClockworkAssembly.cs`; `docs/codebase-audit/outputs/source-batch-006-target.md`; `docs/codebase-audit/outputs/source-batch-006-clockworkassembly-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T12:36:40.8277251-05:00

- Affected phase: SOURCE-BATCH-007 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: use `source-change-executive-decision-intake.csv` as executive source of truth; confirm `EXEC-0001=Ask for candidate discovery list`; confirm `EXEC-0002=Sequential runner only for non-gated rows`; confirm `SOURCE-BATCH-007+` remains `PendingConcreteSourceTarget`; inspect narrow item interaction surfaces; check candidate files against POST-BATCH-Y gate register and active overlay; generate discovery CSV and closeout; validate candidate fields and changed-file scope.
- Result: Generated 5 candidate rows for `SOURCE-BATCH-007`. Recommended next target is `SB007-CAND-001` / `SOURCE-BATCH-007 UnusualDyes Target Guard Repair`. Validation passed: candidate rows=5; recommended target count=1; required candidate fields blank=0; nonzero POST-BATCH-Y gate candidate count=0; nonzero active overlay candidate count=0; recommended gate hits=0; recommended active overlay rows=0. Origami and KeyRing remain excluded from the recommended path because active save-compat overlay rows exist. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-007-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-007-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T14:53:11.7853163-05:00

- Affected phase: SOURCE-BATCH-007 UnusualDyes Target Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-007 in `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`; add stale/null/backpack guards to `UnusualDyes.OnDoubleClick` and `DyeTarget.OnTarget`; verify UnusualDyes POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: `OnDoubleClick` now returns for null/deleted mobiles and treats deleted dye state, a missing backpack, or the dye item outside the backpack as the existing backpack-use failure. `DyeTarget.OnTarget` now returns for null/deleted mobiles, treats null/deleted/out-of-backpack source dye state as the existing backpack-use failure, treats null/deleted target items as the existing invalid-target failure, and reads `m_Dye.DyeColor` only after source dye/mobile/backpack guards. `DyeColor` persistence, randomized jar names and hues, `DyeTub.Redyable`, `BlackDyeTub` rejection, `MagicPigment` hue assignment, `RevealingAction`, sound `0x23E`, empty `Jar` return, dye `Consume()` semantics, localized messages, target eligibility except stale/deleted/null safety, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved. UnusualDyes.cs POST-BATCH-Y gate hits=0; active overlay UnusualDyes matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`; `docs/codebase-audit/outputs/source-batch-007-target.md`; `docs/codebase-audit/outputs/source-batch-007-unusualdyes-target-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:08:44.8478282-05:00

- Affected phase: SOURCE-BATCH-008 VelocityDeed Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-008 in `Data/Scripts/Items/Magical/VelocityDeed.cs`; add stale/null/backpack guards to `VelocityDeed.OnDoubleClick` and `VelocityTargetx.OnTarget`; verify VelocityDeed POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: `OnDoubleClick` now returns for null/deleted mobiles and treats deleted deed state, a missing backpack, or the deed outside the backpack as the existing backpack-use failure. `VelocityTargetx.OnTarget` now returns for null/deleted mobiles, treats null/deleted/out-of-rooted-source deed state as the existing cannot-add-velocity failure, and avoids mutation or deed deletion when the target is not a live `BaseRanged`. `BaseRanged`-only target eligibility, `+10` Velocity increment, existing success/failure messages, deed `RootParent` ownership rule, deed `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved. VelocityDeed.cs POST-BATCH-Y gate hits=0; active overlay VelocityDeed matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Magical/VelocityDeed.cs`; `docs/codebase-audit/outputs/source-batch-008-target.md`; `docs/codebase-audit/outputs/source-batch-008-velocitydeed-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:14:26.8607304-05:00

- Affected phase: SOURCE-BATCH-009 WeaponRenamingTool Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-009 in `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`; add stale/null guards to `WeaponRenamingTool.OnDoubleClick`, `Find(Mobile from)`, `TargetWeapon.OnTarget`, and `InternalGump.OnResponse`; verify WeaponRenamingTool POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: `OnDoubleClick`, `Find`, `TargetWeapon.OnTarget`, and `InternalGump.OnResponse` now guard stale/null mobile, `NetState`, relay info, tool, target weapon, and backpack state before dereferences or rename mutations. `RewardSystem.CheckIsUsableBy` behavior, `BaseWeapon` target eligibility, `InternalGump` text entry flow, 64-character truncation, blank rename removal, localized messages, tool `Delete()` behavior on nonblank rename, `IsRewardItem` serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved. WeaponRenamingTool.cs POST-BATCH-Y gate hits=0; active overlay WeaponRenamingTool matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits beyond the named gump-response guard repair. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Magical/WeaponRenamingTool.cs`; `docs/codebase-audit/outputs/source-batch-009-target.md`; `docs/codebase-audit/outputs/source-batch-009-weaponrenamingtool-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:18:35.5080479-05:00

- Affected phase: SOURCE-BATCH-010 Scales Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-010 in `Data/Scripts/Items/Misc/Scales.cs`; add stale/null guards to `Scales.OnDoubleClick` and `InternalTarget.OnTarget`; verify Scales POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: `OnDoubleClick` now returns for null/deleted mobiles or deleted scales state; `InternalTarget.OnTarget` now returns for stale mobile/source scales state and treats null/deleted target item state as the existing cannot-weigh failure. Target range, self-weigh rejection, `RootParent` awkward-location rule, movable-object rule, weight formatting, messages, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved. Scales.cs POST-BATCH-Y gate hits=0; active overlay Scales matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Scales.cs`; `docs/codebase-audit/outputs/source-batch-010-target.md`; `docs/codebase-audit/outputs/source-batch-010-scales-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:22:39.1548451-05:00

- Affected phase: SOURCE-BATCH-011 MagicScissors Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-011 in `Data/Scripts/Items/Magical/MagicScissors.cs`; add stale/null/backpack guards to `MagicScissors.OnDoubleClick` and `WearTarget.OnTarget`; verify MagicScissors POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: `OnDoubleClick` now returns for null/deleted mobiles and treats deleted scissors state, a missing backpack, or scissors outside the backpack as the existing backpack-use failure. `WearTarget.OnTarget` now returns for null/deleted mobiles, treats null/deleted/out-of-backpack source scissors state as the existing backpack-use failure, and avoids target ownership/artifact/transform reads when the target is null, deleted, or not an item. Backpack-use message, target range, ownership rule, `MyServerSettings.AlterArtifact` policy, every item ID/layer/name transform, sound `0x248`, success/failure messages, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state were preserved. MagicScissors.cs POST-BATCH-Y gate hits=0; active overlay MagicScissors matches=0; no gated approval crossed. Targeted source scan found the new guards and unchanged success paths; serializer diff scan returned no serialization changes; forbidden-surface diff scan returned no command, hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization diff hits. `Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors. Compile-only runtime verification passed with `Scripts: Compile-only verification completed successfully.` `git diff --check` passed with expected LF-to-CRLF warning only. Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.
- Output path: `Data/Scripts/Items/Magical/MagicScissors.cs`; `docs/codebase-audit/outputs/source-batch-011-target.md`; `docs/codebase-audit/outputs/source-batch-011-magicscissors-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:29:12.8232273-05:00

- Affected phase: SOURCE-BATCH-012 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-011 exhausted `source-batch-007-candidate-discovery.csv`, scan narrow item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-heavy, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-012 candidate discovery CSV and closeout.
- Result: Generated 5 candidate rows for `SOURCE-BATCH-012+`: BalancingDeed, HydraTooth, MagicHammer, BookofDead, and MagicPigment. Recommended next target is `SB012-CAND-001` / `SOURCE-BATCH-012 BalancingDeed Guard Repair`. Validation passed: candidate rows=5; nonzero POST-BATCH-Y gate candidate count=0; nonzero active overlay candidate count=0; required candidate fields are populated. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-012-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-012-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:32:00.1198582-05:00

- Affected phase: SOURCE-BATCH-012 BalancingDeed Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-012 in `Data/Scripts/Items/Magical/BalancingDeed.cs`; add stale/null/backpack guards to `BalancingDeed.OnDoubleClick` and `BalancingTarget.OnTarget`; verify BalancingDeed POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: BalancingDeed now guards null/deleted mobiles, deleted or out-of-backpack deed state, missing backpacks, null/deleted/out-of-rooted source deed state, and non-live `BaseRanged` targets before dereferences, mutation, or deed deletion. `BaseRanged` eligibility, `Balanced` flag behavior, `RootParent` ownership rule, messages, deed `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. BalancingDeed.cs POST-BATCH-Y gate hits=0; active overlay BalancingDeed matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/BalancingDeed.cs`; `docs/codebase-audit/outputs/source-batch-012-target.md`; `docs/codebase-audit/outputs/source-batch-012-balancingdeed-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:37:56.3208151-05:00

- Affected phase: SOURCE-BATCH-013 HydraTooth Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-013 in `Data/Scripts/Items/Magical/HydraTooth.cs`; add stale/null/backpack guards to `HydraTooth.OnDoubleClick`; verify HydraTooth POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: HydraTooth now guards null/deleted mobiles, deleted or out-of-backpack tooth state, and missing backpacks before design-context, backpack, spell construction, or cast state is evaluated. `Multis.DesignContext.Check(from)`, backpack message `1042001`, `SummonSkeletonSpell(from, this)` construction, `spell.Cast()`, item id/name/amount/stacking, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. HydraTooth.cs POST-BATCH-Y gate hits=0; active overlay HydraTooth matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/HydraTooth.cs`; `docs/codebase-audit/outputs/source-batch-013-target.md`; `docs/codebase-audit/outputs/source-batch-013-hydratooth-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:43:10.0827741-05:00

- Affected phase: SOURCE-BATCH-014 MagicHammer Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-014 in `Data/Scripts/Items/Magical/MagicHammer.cs`; add stale/null/backpack guards to `MagicHammer.OnDoubleClick` and `WearTarget.OnTarget`; verify MagicHammer POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: MagicHammer now guards null/deleted mobiles, deleted or out-of-backpack hammer state, missing backpacks, null/deleted/out-of-backpack source hammer state, and deleted target item state before ownership, artifact, transform, or weapon replacement state is evaluated. Backpack-use message, target range, ownership rule, `MyServerSettings.AlterArtifact` policy, item ID/name transforms, weapon replacement/deletion behavior, sounds, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. MagicHammer.cs POST-BATCH-Y gate hits=0; active overlay MagicHammer matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/MagicHammer.cs`; `docs/codebase-audit/outputs/source-batch-014-target.md`; `docs/codebase-audit/outputs/source-batch-014-magichammer-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:47:37.0216166-05:00

- Affected phase: SOURCE-BATCH-015 BookofDead Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-015 in `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`; add stale/null/backpack guards to `BookofDead.OnDoubleClick`; verify BookofDead POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: BookofDead now guards null/deleted mobiles, deleted or out-of-backpack book state, and missing backpacks before skill, follower, resource, or corpse creation state is evaluated. Backpack message `1042001`, Necromancy threshold, Spiritualism math, follower requirement, resource types and quantities, `ConsumeTotal` result messages, corpse creation/control behavior, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. BookofDead.cs POST-BATCH-Y gate hits=0; active overlay BookofDead matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`; `docs/codebase-audit/outputs/source-batch-015-target.md`; `docs/codebase-audit/outputs/source-batch-015-bookofdead-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:52:04.9029004-05:00

- Affected phase: SOURCE-BATCH-016 MagicPigment Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-016 in `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`; add stale/null/backpack guards to `MagicPigment.OnDoubleClick` and `DyeTarget.OnTarget`; verify MagicPigment POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: MagicPigment now guards null/deleted mobiles, deleted or out-of-backpack pigment state, missing backpacks, null/deleted/out-of-backpack source pigment state, and deleted target item state before target ownership/backpack or hue mutation state is evaluated. Randomized pigment names, backpack-use message, target range, backpack-as-target rule, in-backpack paint rule, stackable/item ID exclusions, hue assignment/reset, `RevealingAction`, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. MagicPigment.cs POST-BATCH-Y gate hits=0; active overlay MagicPigment matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`; `docs/codebase-audit/outputs/source-batch-016-target.md`; `docs/codebase-audit/outputs/source-batch-016-magicpigment-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T18:59:28.2920934-05:00

- Affected phase: SOURCE-BATCH-017 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-016 exhausted `source-batch-012-candidate-discovery.csv`, scan narrow item interaction surfaces for zero-gate, zero-overlay guard repair candidates; normalize slash/backslash paths when checking POST-BATCH-Y gates and active overlay rows; exclude active-overlay, travel/region, combat/progression, policy-heavy, staff/access, balance/economy, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-017 candidate discovery CSV and closeout.
- Result: Generated 5 candidate rows for `SOURCE-BATCH-017+`: PromotionalToken, MagicalDyes, AllDyeTubsArmor, AllDyeTubsWeapon, and AllDyeTubsFurniture. Recommended next target is `SB017-CAND-001` / `SOURCE-BATCH-017 PromotionalToken Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-017-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-017-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:03:28.5856904-05:00

- Affected phase: SOURCE-BATCH-017 PromotionalToken Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-017 in `Data/Scripts/Items/Misc/PromotionalToken.cs`; add stale/null/backpack/gump-response guards to `PromotionalToken.OnDoubleClick` and `PromotionalTokenGump.OnResponse`; verify PromotionalToken POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: PromotionalToken now guards null/deleted mobiles, deleted or out-of-backpack token state, missing backpacks, null `NetState`, null `RelayInfo`, stale token state, and missing bank boxes before reward creation, bank delivery, or token deletion. Pack message `1062334`, gump layout/buttons/text, ButtonID `1`, `CreateItemFor`, `BankBox.AddItem`, receive message, token `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. PromotionalToken.cs POST-BATCH-Y gate hits=0; active overlay PromotionalToken matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/PromotionalToken.cs`; `docs/codebase-audit/outputs/source-batch-017-target.md`; `docs/codebase-audit/outputs/source-batch-017-promotionaltoken-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:08:27.3028568-05:00

- Affected phase: SOURCE-BATCH-018 MagicalDyes Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-018 in `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`; add stale/null/backpack guards to `MagicalDyes.OnDoubleClick` and `DyeTarget.OnTarget`; verify MagicalDyes POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: MagicalDyes now guards null/deleted mobiles, deleted or out-of-backpack dye state, missing backpacks, null/deleted/out-of-backpack source dye state, and deleted target item state before target backpack or hue mutation state is evaluated. Randomized dye names/hues, backpack-use message, target range, backpack-as-target rule, in-backpack dye rule, stackable/item ID exclusions, hue assignment/reset, `RevealingAction`, sound `0x23E`, Bottle return, `Consume()` semantics, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. MagicalDyes.cs POST-BATCH-Y gate hits=0; active overlay MagicalDyes matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`; `docs/codebase-audit/outputs/source-batch-018-target.md`; `docs/codebase-audit/outputs/source-batch-018-magicaldyes-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:13:19.7195952-05:00

- Affected phase: SOURCE-BATCH-019 AllDyeTubsArmor Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-019 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs`; add stale/null/source-tub/target guards to `AllDyeTubsArmor.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsArmorTarget.OnTarget`; verify AllDyeTubsArmor POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsArmor now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, armor/shield eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsArmor.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsArmor matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsArmor.cs`; `docs/codebase-audit/outputs/source-batch-019-target.md`; `docs/codebase-audit/outputs/source-batch-019-alldyetubsarmor-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:19:55.8367442-05:00

- Affected phase: SOURCE-BATCH-020 AllDyeTubsWeapon Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-020 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs`; add stale/null/source-tub/target guards to `AllDyeTubsWeapon.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsWeaponTarget.OnTarget`; verify AllDyeTubsWeapon POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsWeapon now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, weapon eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsWeapon.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsWeapon matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsWeapon.cs`; `docs/codebase-audit/outputs/source-batch-020-target.md`; `docs/codebase-audit/outputs/source-batch-020-alldyetubsweapon-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:24:10.9554142-05:00

- Affected phase: SOURCE-BATCH-021 AllDyeTubsFurniture Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-021 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs`; add stale/null/source-tub/target guards to `AllDyeTubsFurniture.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsFurnitureTarget.OnTarget`; verify AllDyeTubsFurniture POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsFurniture now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, `FurnitureAttribute.Check(item)` eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsFurniture.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsFurniture matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsFurniture.cs`; `docs/codebase-audit/outputs/source-batch-021-target.md`; `docs/codebase-audit/outputs/source-batch-021-alldyetubsfurniture-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:28:10.6700531-05:00

- Affected phase: SOURCE-BATCH-022 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-021 exhausted `source-batch-017-candidate-discovery.csv`, scan same-family dye-tub item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-heavy, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-022 candidate discovery CSV and closeout.
- Result: Generated 3 candidate rows for `SOURCE-BATCH-022+`: AllDyeTubsBookRune, AllDyeTubsBookSpell, and AllDyeTubsMountEthereal. Recommended next target is `SB022-CAND-001` / `SOURCE-BATCH-022 AllDyeTubsBookRune Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-022-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-022-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:31:49.9389645-05:00

- Affected phase: SOURCE-BATCH-022 AllDyeTubsBookRune Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-022 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookRune.cs`; add stale/null/source-tub/target guards to `AllDyeTubsBookRune.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsBookRuneTarget.OnTarget`; verify AllDyeTubsBookRune POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsBookRune now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, `Runebook` eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsBookRune.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsBookRune matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookRune.cs`; `docs/codebase-audit/outputs/source-batch-022-target.md`; `docs/codebase-audit/outputs/source-batch-022-alldyetubsbookrune-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:35:56.9822424-05:00

- Affected phase: SOURCE-BATCH-023 AllDyeTubsBookSpell Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-023 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`; add stale/null/source-tub/target guards to `AllDyeTubsBookSpell.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsBookSpellTarget.OnTarget`; verify AllDyeTubsBookSpell POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsBookSpell now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, `Spellbook` eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsBookSpell.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsBookSpell matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsBookSpell.cs`; `docs/codebase-audit/outputs/source-batch-023-target.md`; `docs/codebase-audit/outputs/source-batch-023-alldyetubsbookspell-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T19:40:06.2516530-05:00

- Affected phase: SOURCE-BATCH-024 AllDyeTubsMountEthereal Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-024 in `Data/Scripts/Items/Misc/Dyes/AllDyeTubsMountEthereal.cs`; add stale/null/source-tub/target guards to `AllDyeTubsMountEthereal.OnDoubleClick`, `DoPack`, `DoOut`, and `AllDyeTubsMountEtherealTarget.OnTarget`; verify AllDyeTubsMountEthereal POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: AllDyeTubsMountEthereal now guards null/deleted mobiles, deleted source tubs, null backpacks before backpack containment checks, null/deleted target context, and deleted target items before target or charge state is evaluated. `AllowPack`, world-use behavior, target range, `EtherealMount` eligibility, current 100 gold cost behavior and ordering, charged tub deletion/charge decrement behavior, hue assignment, sound `0x23F`, messages including `Select an ethereal to dye`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. AllDyeTubsMountEthereal.cs POST-BATCH-Y gate hits=0; active overlay AllDyeTubsMountEthereal matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/AllDyeTubsMountEthereal.cs`; `docs/codebase-audit/outputs/source-batch-024-target.md`; `docs/codebase-audit/outputs/source-batch-024-alldyetubsmountethereal-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:02:00-05:00

- Affected phase: SOURCE-BATCH-025 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-024 exhausted `source-batch-022-candidate-discovery.csv`, scan magical item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, travel/world-adjacent, policy-heavy, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-025 candidate discovery CSV and closeout.
- Result: Generated 3 candidate rows for `SOURCE-BATCH-025+`: LuckyHorseShoes, SlayerDeed, and ArtifactManual. Recommended next target is `SB025-CAND-001` / `SOURCE-BATCH-025 LuckyHorseShoes Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. Moonstone was excluded because the interaction is travel/world adjacent under preserved region/map policy; MagicCandle, MagicTorch, and MagicLantern were excluded because active overlay rows still match those files. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-025-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-025-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:06:00-05:00

- Affected phase: SOURCE-BATCH-025 LuckyHorseShoes Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-025 in `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`; add stale/null/backpack/source-deed/target guards to `LuckyHorseShoes.OnDoubleClick` and `LuckTarget.OnTarget`; verify LuckyHorseShoes POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: LuckyHorseShoes now guards null/deleted mobiles, deleted source deeds, null backpacks before backpack containment checks, out-of-backpack source deeds, and deleted target items before luck mutation or deed deletion state is evaluated. Target eligibility, RootParent ownership rule, `+100` Luck increment, cap `1000`, messages, target range, deed `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. LuckyHorseShoes.cs POST-BATCH-Y gate hits=0; active overlay LuckyHorseShoes matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`; `docs/codebase-audit/outputs/source-batch-025-target.md`; `docs/codebase-audit/outputs/source-batch-025-luckyhorseshoes-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:10:00-05:00

- Affected phase: SOURCE-BATCH-026 SlayerDeed Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-026 in `Data/Scripts/Items/Magical/SlayerDeed.cs`; add stale/null/backpack/source-deed/target guards to `SlayerDeed.OnDoubleClick` and `SlayerTarget.OnTarget`; verify SlayerDeed POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: SlayerDeed now guards null/deleted mobiles, deleted source deeds, null backpacks before backpack containment checks, out-of-backpack source deeds, and deleted target items before `SlayerType`, slayer mapping, slayer mutation, or deed deletion state is evaluated. Target eligibility, `HolyManSpellbook` exclusion, `GetDeedSlayer` mapping, `Slayer` then `Slayer2` assignment order, messages, target range, deed `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. SlayerDeed.cs POST-BATCH-Y gate hits=0; active overlay SlayerDeed matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/SlayerDeed.cs`; `docs/codebase-audit/outputs/source-batch-026-target.md`; `docs/codebase-audit/outputs/source-batch-026-slayerdeed-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:14:00-05:00

- Affected phase: SOURCE-BATCH-027 ArtifactManual Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-027 in `Data/Scripts/Items/Magical/ArtifactManual.cs`; add stale/null/backpack/source-manual/target guards to `ArtifactManual.OnDoubleClick`, `BookTarget.OnTarget`, and `LookupTheItem`; verify ArtifactManual POST-BATCH-Y gate hits and active overlay matches; run targeted source, item-conversion behavior, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: ArtifactManual now guards null/deleted mobiles, deleted source manuals, null backpacks before backpack containment checks, out-of-backpack source manuals, and deleted target items before lookup, charge decrement, item conversion, item transfer, or deletion state is evaluated. Charges behavior, item-identification outcomes, item transfer and deletion semantics, messages, target range, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. ArtifactManual.cs POST-BATCH-Y gate hits=0; active overlay ArtifactManual matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/ArtifactManual.cs`; `docs/codebase-audit/outputs/source-batch-027-target.md`; `docs/codebase-audit/outputs/source-batch-027-artifactmanual-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:04:51-05:00

- Affected phase: SOURCE-BATCH-028 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-027 exhausted `source-batch-025-candidate-discovery.csv`, scan item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, progression/reward, travel/world, combat/PvP, economy-adjacent, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-028 candidate discovery CSV and closeout.
- Result: Generated 3 candidate rows for `SOURCE-BATCH-028+`: DyeTub, Key, and PuzzleCube. Recommended next target is `SB028-CAND-001` / `SOURCE-BATCH-028 DyeTub Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. Gift/God leveling weapons, Moonstone, Bola, HueStone, and active-overlay candidates were excluded from the automated queue because they are policy-adjacent, higher risk, or overlay-conflicting. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-028-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-028-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:09:30-05:00

- Affected phase: SOURCE-BATCH-028 DyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-028 in `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`; add stale/null/mobile/source-tub/target guards to `DyeTub.OnDoubleClick` and `DyeTub.InternalTarget.OnTarget`; verify DyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: DyeTub now guards null/deleted mobiles, deleted source tub state, null/deleted source target state, deleted target items, and null backpacks before furniture backpack containment checks. Dye eligibility, furniture locked-down/co-owner behavior, hue assignment, sound `0x23E`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. DyeTub.cs POST-BATCH-Y gate hits=0; active overlay DyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/DyeTub.cs`; `docs/codebase-audit/outputs/source-batch-028-target.md`; `docs/codebase-audit/outputs/source-batch-028-dyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:14:30-05:00

- Affected phase: SOURCE-BATCH-029 Key Interaction Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-029 in `Data/Scripts/Items/Misc/Key.cs`; add stale/null/mobile/source-key/backpack/target guards to `Key.OnDoubleClick`, `RenamePrompt.OnResponse`, `UnlockTarget.OnTarget`, and `CopyTarget.OnTarget`; verify Key POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Key interactions now guard null/deleted mobiles, null/deleted source keys, null backpacks before backpack containment checks, deleted target items before unlock state, and deleted target keys before copy state. Key matching, `UseLocks`, rename flow, unlock behavior, copy behavior, tinkering skill check, 10 percent key destruction chance, messages, `CheckLOS = false`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Key.cs POST-BATCH-Y gate hits=0; active overlay Key matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Key.cs`; `docs/codebase-audit/outputs/source-batch-029-target.md`; `docs/codebase-audit/outputs/source-batch-029-key-interaction-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:18:00-05:00

- Affected phase: SOURCE-BATCH-030 PuzzleCube Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-030 in `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`; add stale/null/mobile/backpack guards to `PuzzleCube.OnDoubleClick`; verify PuzzleCube POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: PuzzleCube now guards null/deleted mobiles, deleted cube state, null backpacks, and out-of-backpack cube state before puzzle state is evaluated. Solved/scrambled item IDs `0x202A` and `0x202B`, random success check, sound `0x04B`, messages, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. PuzzleCube.cs POST-BATCH-Y gate hits=0; active overlay PuzzleCube matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`; `docs/codebase-audit/outputs/source-batch-030-target.md`; `docs/codebase-audit/outputs/source-batch-030-puzzlecube-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:23:00-05:00

- Affected phase: SOURCE-BATCH-031 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-030 exhausted `source-batch-028-candidate-discovery.csv`, scan simple item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-adjacent, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-031 candidate discovery CSV and closeout.
- Result: Generated 6 candidate rows for `SOURCE-BATCH-031+`: Dice4, Dice6, Dice8, Dice10, Dice12, and Dice20. Recommended next target is `SB031-CAND-001` / `SOURCE-BATCH-031 Dice4 Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. `Dices.cs` was excluded because active overlay rows still match that file. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-031-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-031-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:25:00-05:00

- Affected phase: SOURCE-BATCH-031 Dice4 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-031 in `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`; add stale/null/mobile/deleted-item guards to `Dice4.OnDoubleClick` and `Dice4.OnTelekinesis`; verify Dice4 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice4 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x301C`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 4)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice4.cs POST-BATCH-Y gate hits=0; active overlay Dice4 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`; `docs/codebase-audit/outputs/source-batch-031-target.md`; `docs/codebase-audit/outputs/source-batch-031-dice4-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:28:00-05:00

- Affected phase: SOURCE-BATCH-032 Dice6 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-032 in `Data/Scripts/Items/Misc/Games/DandD/Dice6.cs`; add stale/null/mobile/deleted-item guards to `Dice6.OnDoubleClick` and `Dice6.OnTelekinesis`; verify Dice6 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice6 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x3018`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 6)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice6.cs POST-BATCH-Y gate hits=0; active overlay Dice6 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice6.cs`; `docs/codebase-audit/outputs/source-batch-032-target.md`; `docs/codebase-audit/outputs/source-batch-032-dice6-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:31:00-05:00

- Affected phase: SOURCE-BATCH-033 Dice8 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-033 in `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs`; add stale/null/mobile/deleted-item guards to `Dice8.OnDoubleClick` and `Dice8.OnTelekinesis`; verify Dice8 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice8 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x3019`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 8)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice8.cs POST-BATCH-Y gate hits=0; active overlay Dice8 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice8.cs`; `docs/codebase-audit/outputs/source-batch-033-target.md`; `docs/codebase-audit/outputs/source-batch-033-dice8-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:34:00-05:00

- Affected phase: SOURCE-BATCH-034 Dice10 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-034 in `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs`; add stale/null/mobile/deleted-item guards to `Dice10.OnDoubleClick` and `Dice10.OnTelekinesis`; verify Dice10 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice10 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x301B`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 10)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice10.cs POST-BATCH-Y gate hits=0; active overlay Dice10 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice10.cs`; `docs/codebase-audit/outputs/source-batch-034-target.md`; `docs/codebase-audit/outputs/source-batch-034-dice10-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:39:00-05:00

- Affected phase: SOURCE-BATCH-035 Dice12 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-035 in `Data/Scripts/Items/Misc/Games/DandD/Dice12.cs`; add stale/null/mobile/deleted-item guards to `Dice12.OnDoubleClick` and `Dice12.OnTelekinesis`; verify Dice12 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice12 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x301D`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 12)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice12.cs POST-BATCH-Y gate hits=0; active overlay Dice12 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice12.cs`; `docs/codebase-audit/outputs/source-batch-035-target.md`; `docs/codebase-audit/outputs/source-batch-035-dice12-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:44:00-05:00

- Affected phase: SOURCE-BATCH-036 Dice20 Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-036 in `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs`; add stale/null/mobile/deleted-item guards to `Dice20.OnDoubleClick` and `Dice20.OnTelekinesis`; verify Dice20 POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: Dice20 now guards null/deleted mobiles and deleted dice before range checks, telekinesis effects, sound, or roll-message state is evaluated. ItemID `0x301A`, name/weight, AddNameProperties labels, range `2`, telekinesis effects and sound `0x1F5`, roll overhead message, `Utility.Random(1, 20)`, sound `0x34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. Dice20.cs POST-BATCH-Y gate hits=0; active overlay Dice20 matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Games/DandD/Dice20.cs`; `docs/codebase-audit/outputs/source-batch-036-target.md`; `docs/codebase-audit/outputs/source-batch-036-dice20-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:50:00-05:00

- Affected phase: SOURCE-BATCH-037 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-036 exhausted `source-batch-031-candidate-discovery.csv`, scan simple item interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-adjacent, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-037 candidate discovery CSV and closeout.
- Result: Generated 3 candidate rows for `SOURCE-BATCH-037+`: EverlastingBottle, EverlastingLoaf, and MusicBox. Recommended next target is `SB037-CAND-001` / `SOURCE-BATCH-037 EverlastingBottle Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. D&D handbook/guide files were excluded because active overlay rows match those files; economy/balance-adjacent and broader policy-adjacent candidates remain excluded. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-037-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-037-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:55:00-05:00

- Affected phase: SOURCE-BATCH-037 EverlastingBottle Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-037 in `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`; add stale/null/mobile/deleted-item guards to `EverlastingBottle.OnDoubleClick`; verify EverlastingBottle POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: EverlastingBottle now guards null/deleted mobiles and deleted bottles before thirst assignment, message sending, or sound playback. ItemID `0x2827`, Hue `0x849`, Name `Everlasting Bottle`, `Artefact` label, `from.Thirst = 20`, refill message, `Utility.RandomList(0x30, 0x2D6)`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. EverlastingBottle.cs POST-BATCH-Y gate hits=0; active overlay EverlastingBottle matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`; `docs/codebase-audit/outputs/source-batch-037-target.md`; `docs/codebase-audit/outputs/source-batch-037-everlastingbottle-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T20:59:00-05:00

- Affected phase: SOURCE-BATCH-038 EverlastingLoaf Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-038 in `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingLoaf.cs`; add stale/null/mobile/deleted-item guards to `EverlastingLoaf.OnDoubleClick`; verify EverlastingLoaf POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: EverlastingLoaf now guards null/deleted mobiles and deleted loaves before hunger assignment, message sending, sound playback, or animation. ItemID `0x136F`, Hue `0`, Name `Everlasting Loaf`, `Artefact` label, `from.Hunger = 20`, bite/reform message, `Utility.Random(0x3A, 3)`, human/not-mounted animation `34`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. EverlastingLoaf.cs POST-BATCH-Y gate hits=0; active overlay EverlastingLoaf matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingLoaf.cs`; `docs/codebase-audit/outputs/source-batch-038-target.md`; `docs/codebase-audit/outputs/source-batch-038-everlastingloaf-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:04:00-05:00

- Affected phase: SOURCE-BATCH-039 MusicBox Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-039 in `Data/Scripts/Items/Misc/MusicBox.cs`; add stale/null/mobile/deleted-item guards to `MusicBox.OnDoubleClick`; verify MusicBox POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: MusicBox now guards null/deleted mobiles and deleted music boxes before music packets/messages are sent or `Mplay` state advances. Name `Lute of Many Songs`, `Mplay` property, 67 music sends, 67 message sends, `Mplay` increments, `Mplay = 1`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. MusicBox.cs POST-BATCH-Y gate hits=0; active overlay MusicBox matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/MusicBox.cs`; `docs/codebase-audit/outputs/source-batch-039-target.md`; `docs/codebase-audit/outputs/source-batch-039-musicbox-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:10:00-05:00

- Affected phase: SOURCE-BATCH-040 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-039 exhausted `source-batch-037-candidate-discovery.csv`, scan reward dye tub wrapper interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-adjacent, staff/access, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-040 candidate discovery CSV and closeout.
- Result: Generated 6 candidate rows for `SOURCE-BATCH-040+`: RewardBlackDyeTub, SpecialDyeTub, LeatherDyeTub, FurnitureDyeTub, RunebookDyeTub, and StatuetteDyeTub. Recommended next target is `SB040-CAND-001` / `SOURCE-BATCH-040 RewardBlackDyeTub Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. Hair/beard dye files were excluded because active overlay rows match those files. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-040-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-040-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:15:00-05:00

- Affected phase: SOURCE-BATCH-040 RewardBlackDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-040 in `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`; add stale/null/mobile/deleted-item guards to `RewardBlackDyeTub.OnDoubleClick`; verify RewardBlackDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: RewardBlackDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. LabelNumber `1006008`, Hue/DyedHue `0x0001`, `Redyable = false`, blessed loot, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, namespace/type/file layout, and project/config/data files were preserved. RewardBlackDyeTub.cs POST-BATCH-Y gate hits=0; active overlay RewardBlackDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-040-target.md`; `docs/codebase-audit/outputs/source-batch-040-rewardblackdyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:19:00-05:00

- Affected phase: SOURCE-BATCH-041 SpecialDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-041 in `Data/Scripts/Items/Misc/Dyes/SpecialDyeTub.cs`; add stale/null/mobile/deleted-item guards to `SpecialDyeTub.OnDoubleClick`; verify SpecialDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: SpecialDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. `CustomHuePicker.SpecialDyeTub`, LabelNumber `1041285`, blessed loot, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, namespace/type/file layout, and project/config/data files were preserved. SpecialDyeTub.cs POST-BATCH-Y gate hits=0; active overlay SpecialDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/SpecialDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-041-target.md`; `docs/codebase-audit/outputs/source-batch-041-specialdyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:23:00-05:00

- Affected phase: SOURCE-BATCH-042 LeatherDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-042 in `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`; add stale/null/mobile/deleted-item guards to `LeatherDyeTub.OnDoubleClick`; verify LeatherDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: LeatherDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. `AllowDyables = false`, `AllowLeather = true`, TargetMessage `1042416`, FailMessage `1042418`, LabelNumber `1041284`, `CustomHuePicker.LeatherDyeTub`, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, namespace/type/file layout, and project/config/data files were preserved. LeatherDyeTub.cs POST-BATCH-Y gate hits=0; active overlay LeatherDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-042-target.md`; `docs/codebase-audit/outputs/source-batch-042-leatherdyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:27:00-05:00

- Affected phase: SOURCE-BATCH-043 FurnitureDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-043 in `Data/Scripts/Items/Misc/Dyes/FurnitureDyeTub.cs`; add stale/null/mobile/deleted-item guards to `FurnitureDyeTub.OnDoubleClick`; verify FurnitureDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: FurnitureDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. `AllowDyables = false`, `AllowFurniture = true`, TargetMessage `501019`, FailMessage `501021`, LabelNumber `1041246`, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, LootType blessed fallback, namespace/type/file layout, and project/config/data files were preserved. FurnitureDyeTub.cs POST-BATCH-Y gate hits=0; active overlay FurnitureDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/FurnitureDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-043-target.md`; `docs/codebase-audit/outputs/source-batch-043-furnituredyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:31:00-05:00

- Affected phase: SOURCE-BATCH-044 RunebookDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-044 in `Data/Scripts/Items/Misc/Dyes/RunebookDyeTub.cs`; add stale/null/mobile/deleted-item guards to `RunebookDyeTub.OnDoubleClick`; verify RunebookDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: RunebookDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. `AllowDyables = false`, `AllowRunebooks = true`, TargetMessage `1049774`, FailMessage `1049775`, LabelNumber `1049740`, `CustomHuePicker.LeatherDyeTub`, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, namespace/type/file layout, and project/config/data files were preserved. RunebookDyeTub.cs POST-BATCH-Y gate hits=0; active overlay RunebookDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/RunebookDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-044-target.md`; `docs/codebase-audit/outputs/source-batch-044-runebookdyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:32:00-05:00

- Affected phase: SOURCE-BATCH-045 StatuetteDyeTub Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-045 in `Data/Scripts/Items/Misc/Dyes/StatuetteDyeTub.cs`; add stale/null/mobile/deleted-item guards to `StatuetteDyeTub.OnDoubleClick`; verify StatuetteDyeTub POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: StatuetteDyeTub now guards null/deleted mobiles and deleted dye tubs before reward usability checks or base DyeTub behavior. `AllowDyables = false`, `AllowStatuettes = true`, TargetMessage `1049777`, FailMessage `1049778`, LabelNumber `1049741`, `CustomHuePicker.LeatherDyeTub`, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` serialization, namespace/type/file layout, and project/config/data files were preserved. StatuetteDyeTub.cs POST-BATCH-Y gate hits=0; active overlay StatuetteDyeTub matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Misc/Dyes/StatuetteDyeTub.cs`; `docs/codebase-audit/outputs/source-batch-045-target.md`; `docs/codebase-audit/outputs/source-batch-045-statuettedyetub-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:36:00-05:00

- Affected phase: SOURCE-BATCH-046 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-045 exhausted `source-batch-040-candidate-discovery.csv`, scan oil-material interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude active-overlay, policy-adjacent, staff/access, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-046 candidate discovery CSV and closeout.
- Result: Generated 3 candidate rows for `SOURCE-BATCH-046+`: OilMetal, OilLeather, and OilWood. Recommended next target is `SB046-CAND-001` / `SOURCE-BATCH-046 OilMetal Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. `EssenceOrb` was excluded because active overlay rows match that file. HueStone remains unselected because its target flow includes charge/gold policy. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-046-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-046-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:40:00-05:00

- Affected phase: SOURCE-BATCH-046 OilMetal Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-046 in `Data/Scripts/Items/Potions/Oils/OilMetal.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilMetal.OnDoubleClick` and `OilTarget.OnTarget`; verify OilMetal POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilMetal now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilMetal.cs POST-BATCH-Y gate hits=0; active overlay OilMetal matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilMetal.cs`; `docs/codebase-audit/outputs/source-batch-046-target.md`; `docs/codebase-audit/outputs/source-batch-046-oilmetal-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:44:00-05:00

- Affected phase: SOURCE-BATCH-047 OilLeather Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-047 in `Data/Scripts/Items/Potions/Oils/OilLeather.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilLeather.OnDoubleClick` and `OilTarget.OnTarget`; verify OilLeather POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilLeather now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Tailoring skill threshold `90`, leather item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilLeather.cs POST-BATCH-Y gate hits=0; active overlay OilLeather matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilLeather.cs`; `docs/codebase-audit/outputs/source-batch-047-target.md`; `docs/codebase-audit/outputs/source-batch-047-oilleather-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T21:48:00-05:00

- Affected phase: SOURCE-BATCH-048 OilWood Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-048 in `Data/Scripts/Items/Potions/Oils/OilWood.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilWood.OnDoubleClick` and `OilTarget.OnTarget`; verify OilWood POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilWood now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Carpentry or bowcraft skill threshold `90`, wood item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilWood.cs POST-BATCH-Y gate hits=0; active overlay OilWood matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilWood.cs`; `docs/codebase-audit/outputs/source-batch-048-target.md`; `docs/codebase-audit/outputs/source-batch-048-oilwood-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T22:10:21-05:00

- Affected phase: SOURCE-BATCH-049 candidate discovery
- Cwd: `D:\ConficturaUO`
- Command: after SOURCE-BATCH-048 exhausted `source-batch-046-candidate-discovery.csv`, scan gem-specific oil interaction surfaces for zero-gate, zero-overlay guard repair candidates; exclude completed representative oil files, active-overlay, policy-adjacent, staff/access, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, and reorganization surfaces; generate SOURCE-BATCH-049 candidate discovery CSV and closeout.
- Result: Generated 15 candidate rows for `SOURCE-BATCH-049+`: OilAmethyst, OilCaddellite, OilEmerald, OilGarnet, OilIce, OilJade, OilMarble, OilOnyx, OilQuartz, OilRuby, OilSapphire, OilSilver, OilSpinel, OilStarRuby, and OilTopaz. Recommended next target is `SB049-CAND-001` / `SOURCE-BATCH-049 OilAmethyst Guard Repair`. Validation passed: selected candidate files have POST-BATCH-Y gate hits=0, active overlay rows=0, and required candidate fields populated. No source/project/XML/config/data behavior files changed.
- Output path: `docs/codebase-audit/outputs/source-batch-049-candidate-discovery.csv`; `docs/codebase-audit/outputs/source-batch-049-candidate-discovery-closeout.md`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T22:16:04-05:00

- Affected phase: SOURCE-BATCH-049 OilAmethyst Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-049 in `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilAmethyst.OnDoubleClick` and `OilTarget.OnTarget`; verify OilAmethyst POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilAmethyst now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilAmethyst.cs POST-BATCH-Y gate hits=0; active overlay OilAmethyst matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build after sandbox escalation for SDK-cache access, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`; `docs/codebase-audit/outputs/source-batch-049-target.md`; `docs/codebase-audit/outputs/source-batch-049-oilamethyst-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T22:19:53-05:00

- Affected phase: SOURCE-BATCH-050 OilCaddellite Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-050 in `Data/Scripts/Items/Potions/Oils/OilCaddellite.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilCaddellite.OnDoubleClick` and `OilTarget.OnTarget`; verify OilCaddellite POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilCaddellite now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilCaddellite.cs POST-BATCH-Y gate hits=0; active overlay OilCaddellite matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilCaddellite.cs`; `docs/codebase-audit/outputs/source-batch-050-target.md`; `docs/codebase-audit/outputs/source-batch-050-oilcaddellite-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`

### 2026-06-16T22:23:48-05:00

- Affected phase: SOURCE-BATCH-051 OilEmerald Guard Repair
- Cwd: `D:\ConficturaUO`
- Command: implement SOURCE-BATCH-051 in `Data/Scripts/Items/Potions/Oils/OilEmerald.cs`; add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilEmerald.OnDoubleClick` and `OilTarget.OnTarget`; verify OilEmerald POST-BATCH-Y gate hits and active overlay matches; run targeted source, serializer, and forbidden-surface scans; build `Data/System/Source/Server.csproj` Debug/x86 with Visual Studio MSBuild; run `.\ConficturaServer.exe -compileonly -nocache`; restore generated root build artifacts; update source-batch target, closeout, intake, and controller artifacts.
- Result: OilEmerald now guards null/deleted mobiles, deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state before target, morph, consume, or deletion behavior is evaluated. Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, and project/config/data files were preserved. OilEmerald.cs POST-BATCH-Y gate hits=0; active overlay OilEmerald matches=0; no gated approval crossed. Targeted source scan, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, and `git diff --check` passed; generated root build artifacts were restored before staging.
- Output path: `Data/Scripts/Items/Potions/Oils/OilEmerald.cs`; `docs/codebase-audit/outputs/source-batch-051-target.md`; `docs/codebase-audit/outputs/source-batch-051-oilemerald-guard-repair-closeout.md`; `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`; `docs/codebase-audit/outputs/source-batch-controller-closeout.md`; `docs/codebase-audit/outputs/source-batch-intake-register.csv`; `docs/codebase-audit/PHASE_STATUS.md`; `docs/codebase-audit/RUN_LOG.md`; `docs/codebase-audit/outputs/README.md`
