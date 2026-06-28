# Live Build And Runtime Script Compile Model

Generated: 2026-06-06T13:58:21.5320530-05:00

## Summary

The live Confictura workflow builds the server source project first. The resulting server executable compiles shard scripts at startup, then initializes scripts and starts the server.

This means `Data/Scripts/Scripts.csproj` is Visual Studio project hygiene, not the authoritative runtime script compile list.

## Source Evidence

| Evidence | Interpretation |
| --- | --- |
| `Data/System/Source/Main.cs:575` | Startup loops on `ScriptCompiler.Compile(m_Debug, m_Cache)` before proceeding. |
| `Data/System/Source/Main.cs:595` | Script `Initialize` methods are invoked after script compile, region load, and world load. |
| `Data/System/Source/ScriptCompiler.cs:149` | C# script compilation begins from `GetScripts("*.cs")`. |
| `Data/System/Source/ScriptCompiler.cs:233` | CodeDOM compiles the gathered script files with `CompileAssemblyFromFile`. |
| `Data/System/Source/ScriptCompiler.cs:591` | Runtime script discovery starts at `Path.Combine(Core.BaseDirectory, "Data/Scripts")`. |
| `Data/System/Source/ScriptCompiler.cs:598-601` | Script discovery recurses through child directories and adds matching files from each directory. |

## Truth Model

| Truth model | Authority | What it proves | What it does not prove |
| --- | --- | --- | --- |
| `RuntimeScriptCompileTruth` | Built server executable plus `ScriptCompiler.GetScripts("*.cs")` over `Data/Scripts` | Which `.cs` files the live server attempts to compile at startup. | Visual Studio project membership. |
| `ScriptsProjectTruth` | `Data/Scripts/Scripts.csproj` and solution/project builds | Visual Studio project health, source-list drift, and IDE/solution build hygiene. | Live runtime script inclusion or exclusion. |
| `SourceBuildTruth` | `Data/System/Source/Server.csproj` build | The server executable source compiles. | Runtime scripts compile successfully. |

## Phase 2 Reclassification

The Phase 2 project truth outputs remain valid, but their operational meaning is narrower than originally recorded:

- `phase-02-missing-compile-targets-classified.csv` identifies stale `Scripts.csproj` includes.
- `phase-02-unincluded-source-classified.csv` identifies `.cs` files under `Data/Scripts` that are absent from `Scripts.csproj`.
- These are IDE/project hygiene findings unless a runtime startup compile reports matching C# compiler errors.

The old Phase 2 solution failure should not be treated as the first live-server blocker. It proves the optional Visual Studio scripts project is out of sync with the filesystem.

## Verification Baseline To Establish Next

1. Build `Data/System/Source/Server.csproj` using the narrowest available Debug/x86 or Release/x86 command.
2. Run a time-boxed local startup smoke with the built executable and `-service -nocache`.
3. Record whether runtime script compile succeeds, reports compiler errors, fails during script initialization, reaches listening state, or cannot be run safely in the local environment.
4. Treat startup compiler errors as live-runtime blockers before prioritizing `Scripts.csproj` drift.

## Reorganization Impact

Any `.cs` file moved into or within `Data/Scripts` remains runtime-visible if it keeps a `.cs` extension. Reorganization planning must preserve namespace/type compatibility, save compatibility, and runtime compile visibility. `Scripts.csproj` updates are still useful when maintaining Visual Studio project hygiene, but they are not required for the live server to discover scripts.
