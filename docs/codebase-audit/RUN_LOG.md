# Codebase Audit Run Log

Initialized: 2026-06-05T16:15:59.8020730-05:00

Last updated: 2026-06-05T17:57:00.0000000-05:00

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
