# Phase 2 Build Verification

Generated: 2026-06-05T16:48:00.0000000-05:00

## Commands

| Command | Result | Notes |
| --- | --- | --- |
| `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal` | Failed before script compilation | The direct project build passes `Platform=AnyCPU` to the referenced `Server.csproj`, which has no `Debug|AnyCPU` output path. |
| `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal` | Failed during `Scripts.csproj` compilation | The solution maps `Server` to `Debug|x86` and `Scripts` to `Debug|Any CPU`; compilation then fails on missing `Compile Include` targets. |
| `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb` | Succeeded | The failed solution build updated tracked server build artifacts before script compilation failed; those generated artifacts were restored. |

## Direct Project Build Failure

The direct `Scripts.csproj` build failed before reaching script source compilation:

```text
The BaseOutputPath/OutputPath property is not set for project 'Server.csproj'.
Configuration='Debug' Platform='AnyCPU'.
```

Original interpretation: for this checkout and MSBuild version, the solution configuration is the valid way to build both Visual Studio projects because it maps `Server` to x86 and `Scripts` to Any CPU.

Post-audit live workflow interpretation: this is an IDE/project-hygiene failure for the optional scripts project path. It is not evidence that the live server startup compiler failed.

## Maintained Solution Build Failure

The maintained solution build reached `Scripts.csproj` compilation and failed with `CS2001` missing source-file errors. This matches the Phase 2 project truth register:

| Evidence | Count |
| --- | ---: |
| `phase-02-missing-compile-targets-classified.csv` | 82 |
| `phase-02-unincluded-source-classified.csv` | 92 |

Representative errors:

```text
CSC : error CS2001: Source file 'D:\ConficturaUO\Data\Scripts\System\Commands\Adddoorgump.cs' could not be found.
CSC : error CS2001: Source file 'D:\ConficturaUO\Data\Scripts\Custom\Government System\Resourcebox\ResourceBoxGump.cs' could not be found.
CSC : error CS2001: Source file 'D:\ConficturaUO\Data\Scripts\Custom\XMLSpawner\XmlQuest\QuestLogGump.cs' could not be found.
CSC : error CS2001: Source file 'D:\ConficturaUO\Data\Scripts\Custom\PvPConsent\PKNONPKGUMP.cs' could not be found.
CSC : error CS2001: Source file 'D:\ConficturaUO\Data\Scripts\Trades\Core\CraftGump.cs' could not be found.
```

## Verification Status

Original Phase 2 Visual Studio project-hygiene verification did not pass. The original failure was explained and backlogged by Phase 2 project truth outputs.

## Post-Audit ScriptsProjectTruth Repair

Generated: 2026-06-13T13:14:33.2498564-05:00

| Command | Result | Notes |
| --- | --- | --- |
| `.\docs\codebase-audit\tools\Invoke-CodebaseAuditInventory.ps1` | Failed after refreshing include-dependent CSVs | The tool hit an existing null-export failure near the end, but refreshed `phase-01-project-includes.csv`, `phase-01-missing-compile-targets.csv`, and `phase-01-unincluded-source-files.csv` from the repaired `Scripts.csproj`. |
| `.\docs\codebase-audit\tools\New-ProjectTruthRegister.ps1` | Succeeded | Regenerated `project-truth-register.csv`, `phase-02-missing-compile-targets-classified.csv`, `phase-02-unincluded-source-classified.csv`, and cleanup backlog outputs. Counts: 6,581 script compile includes, 6,581 script source files, 0 missing compile targets, 0 unincluded script sources, 0 backlog groups. |
| `git diff --check` | Succeeded | No whitespace errors in the project/audit batch. |
| `msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"` | Unavailable on PATH | The shell did not have `msbuild` on `PATH`; Visual Studio MSBuild was then run by absolute path. |
| `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"` | Failed in sandbox; rerun outside sandbox | The sandboxed attempt could not read `C:\Users\nepht\AppData\Local\Microsoft SDKs`; the approved out-of-sandbox rerun reached script compilation. |
| `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU" /v:minimal /clp:ErrorsOnly` | Failed during `Scripts.csproj` compilation | The repair cleared missing compile targets. The remaining errors are existing dependency/reference surfaces in already-included project areas, including missing `OrbServerSDK`/`UOArchitectInterface` types and missing `System.Web`/`System.Drawing` reference namespaces. This remains an `IDEProjectHygiene` follow-up and is not live runtime script compile proof. |
| `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb` | Succeeded | MSBuild-updated tracked server build artifacts were restored before staging. |

Post-repair project truth status:

| Evidence | Count |
| --- | ---: |
| `phase-02-missing-compile-targets-classified.csv` | 0 |
| `phase-02-unincluded-source-classified.csv` | 0 |
| `phase-02-project-cleanup-backlog.csv` | 0 |

Visual Studio solution build verification still does not pass, but it no longer fails because of stale `Scripts.csproj` paths. The remaining failure should be tracked as project dependency/reference cleanup, not as `ScriptsProjectTruth` drift.

This file does not record live runtime script compile verification. Live runtime verification now requires a `Server.csproj` build followed by a time-boxed startup compile smoke with the built executable, as described in `live-build-and-runtime-script-compile-model.md`.
