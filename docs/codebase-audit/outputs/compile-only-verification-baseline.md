# Compile-Only Verification Baseline

Generated: 2026-06-06T14:24:35.6662555-05:00

## Summary

The server now has a safe runtime script compile verification path:

```powershell
.\ConficturaServer.exe -compileonly -nocache
```

The command compiles runtime-visible scripts and exits before script `Configure`, `Region.Load`, `World.Load`, script `Initialize`, timers, `MessagePump`, `NetState`, or listener binding.

## Source Changes

| File | Change |
| --- | --- |
| `Data/System/Source/Main.cs` | Added `-compileonly` argument parsing, argument display, compile-failure exit code `1`, and early success return after `ScriptCompiler.Compile`. |
| `Data/System/Source/ScriptCompiler.cs` | Skips exact `bin` and `obj` directories while recursively gathering script `.cs` files. |

## Verification Results

| Check | Result | Evidence |
| --- | --- | --- |
| `git diff --check` | Passed | No whitespace errors; expected `core.autocrlf=true` warnings only. |
| `Server.csproj` Debug/x86 build | Passed | MSBuild reported `Server -> D:\ConficturaUO\ConficturaServer.exe`. |
| First `-compileonly -nocache` run | Failed as expected before discovery fix | Runtime compiler included `Data/Scripts/obj/Debug/.NETFramework,Version=v4.8.AssemblyAttributes.cs`, causing duplicate `TargetFrameworkAttribute`. |
| Generated-folder filter | Applied | `ScriptCompiler.GetScripts` now skips exact `bin` and `obj` directories. |
| Second `-compileonly -nocache` run | Passed | Output ended with `Scripts: Compile-only verification completed successfully.` and did not contain `Listening:`. |
| Generated artifacts restored | Passed | Tracked server artifacts were restored; `Data/Data.bin` and `Data/Data.hash` were restored from backup. |

## Compile-Only Output

```text
System Initializing...
Running on .NET Framework Version 4.0.30319
Core: Running with arguments: -nocache -compileonly
Optimizing for 24 processors
Processing. Please wait...
Scripts: Compile-only verification completed successfully.
Exiting...done
```

## Runtime Compile Blocker Fixed

The first compile-only run proved that the runtime script compiler was recursing into `Data/Scripts/obj`. That directory contains Visual Studio generated source files, not shard scripts. The generated `.NETFramework,Version=v4.8.AssemblyAttributes.cs` file caused:

```text
CS0579: Duplicate 'global::System.Runtime.Versioning.TargetFrameworkAttribute' attribute
```

Skipping exact `bin` and `obj` directories aligns runtime script discovery with the audit inventory policy and prevents project output files from becoming runtime scripts.

## Next Runtime-Risk Batch

With compile-only verification available, proceed to `POST-BATCH-A`: review the 17 P0 packet handler rows before broader serializer or reorganization work.
