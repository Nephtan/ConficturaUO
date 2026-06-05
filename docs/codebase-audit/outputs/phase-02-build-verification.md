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

Interpretation: for this checkout and MSBuild version, the maintained solution configuration is the valid way to build both projects because it maps `Server` to x86 and `Scripts` to Any CPU.

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

Build verification did not pass. The failure is explained and backlogged by Phase 2 project truth outputs. No `Scripts.csproj` repair was made in this phase; repairs must be performed in focused project-file batches with project truth regeneration and MSBuild verification.
