# Source Build And Runtime Compile Baseline

Generated: 2026-06-06T14:05:54.8534809-05:00

## Summary

The first live-workflow verification baseline was run after updating the audit instructions to distinguish source builds, runtime script compile truth, and Visual Studio project hygiene.

| Check | Result | Notes |
| --- | --- | --- |
| Source build | Passed | `Server.csproj` Debug/x86 compiled and produced `D:\ConficturaUO\ConficturaServer.exe`. |
| Runtime script inventory | Complete | `runtime-script-compile-inventory.csv` lists 6,581 `.cs` files under `Data/Scripts`, excluding `bin` and `obj`. |
| Startup script compile smoke | Superseded | Full startup remains unsafe in this checkout, but `compile-only-verification-baseline.md` now records safe runtime script compile verification. |
| Visual Studio scripts project build | Not run | Optional IDE/project hygiene check; not required for live runtime proof. |

## Commands And Results

| Command | Result |
| --- | --- |
| `& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86 /v:minimal` | Passed. MSBuild reported `Server -> D:\ConficturaUO\ConficturaServer.exe`. |
| Runtime script inventory generation from `Data/Scripts/**/*.cs`, excluding `bin` and `obj` | Passed. Generated 6,581 rows in `runtime-script-compile-inventory.csv`. |

## Startup Smoke Blocker

The planned executable startup smoke should use the built server executable with `-service -nocache` so script compile failures exit instead of waiting for console input. It was not run in this batch because the current workspace contains a `Saves` directory and the startup path proceeds from script compilation into world load and network listening. No no-load/no-listen flag was source-verified before this batch.

Next safe action: use `.\ConficturaServer.exe -compileonly -nocache` for routine runtime script compile verification. Run full startup smoke only in an isolated local copy or after the operator confirms this checkout and port configuration are safe for a short-lived server start.

## Generated Artifact Handling

The source build updated tracked generated artifacts:

- `ConficturaServer.exe`
- `ConficturaServer.exe.config`
- `ConficturaServer.pdb`

These artifacts are build outputs and should be restored after recording verification so this documentation-only baseline commit does not include generated binaries.
