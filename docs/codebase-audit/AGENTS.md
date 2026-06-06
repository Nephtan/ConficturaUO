# Agent Instructions For Codebase Audit Artifacts

This file applies to `docs/codebase-audit/` and its descendants.

## Audit Truth Models

Audit outputs must distinguish runtime script compile truth from Visual Studio project truth.

- **RuntimeScriptCompileTruth:** The live server executable compiles scripts at startup through `Data/System/Source/ScriptCompiler.cs`, recursively gathering `.cs` files under `Data/Scripts`.
- **ScriptsProjectTruth:** `Data/Scripts/Scripts.csproj` records the Visual Studio project source list. It is useful for solution builds and IDE hygiene, but it does not define what scripts the live server compiles.

Do not describe a missing `Scripts.csproj` include as a live runtime absence unless a server startup script compile proves that runtime behavior.

## Phase 2 Interpretation

Phase 2 project truth findings remain valid project-hygiene evidence.

- Missing compile targets in `Scripts.csproj` mean the Visual Studio scripts project references files that are absent at those paths.
- Unlisted source files mean `.cs` files exist under `Data/Scripts` but are absent from `Scripts.csproj`.
- Both categories are IDE/project hygiene findings unless the server startup compiler reports a matching runtime compile failure.

Keep `Scripts.csproj` drift in repair backlog outputs, but do not make it the first live-server blocker by default.

## Required Verification Records

Future audit repair batches must record:

- source build result for `Data/System/Source/Server.csproj`, or why it was unavailable;
- runtime script compile/startup smoke result, or why it was unsafe or unavailable;
- optional `Scripts.csproj` or solution build result only when the batch intentionally maintains Visual Studio project hygiene.

When verification is unavailable, record the exact command attempted, tool or safety blocker, static checks completed, and the next safe action.

## Output Rules

- Prefer explicit labels such as `RuntimeScriptCompileTruth`, `ScriptsProjectTruth`, `LiveRuntime`, `IDEProjectHygiene`, `Blocked`, and `Unavailable`.
- Keep generated or reviewed audit outputs source-traced to code, project files, command output, or checked-in data.
- Do not approve reorganization from audit output tables alone; moves still require save compatibility review, runtime compile visibility review, docs updates, verification, and rollback notes.
