# Codebase Audit Run Log

Initialized: 2026-06-05T16:15:59.8020730-05:00

Last updated: 2026-06-05T16:45:00.0000000-05:00

Branch: `SAR`

Current HEAD: `78341d10 docs: add reproducible audit inventories`

Scope: Phase 0 baseline and guardrails. No source files, project files, serialized types, or runtime hooks were changed.

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
