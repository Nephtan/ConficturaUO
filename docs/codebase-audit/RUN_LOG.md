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
