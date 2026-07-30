# SOURCE-BATCH-030 Closeout: PuzzleCube Guard Repair

Reviewed at: 2026-06-16T20:18:00-05:00

## Summary

`SOURCE-BATCH-030` implemented `SB028-CAND-003`, a non-gated `PuzzleCube` guard repair in `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`.

The batch adds stale/null safety before mobile, backpack, or puzzle state is evaluated. It preserves the existing puzzle behavior, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before dereferencing backpack or puzzle state.
- `OnDoubleClick(Mobile from)` now treats deleted cubes, missing backpacks, or cubes outside the backpack as the existing backpack-use failure.

## Preserved Behavior

- Solved and scrambled item IDs `0x202A` and `0x202B`.
- Random success check.
- Sound `0x04B`.
- Existing messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, deleted cube, and backpack guards.
- Targeted source scan confirmed item IDs `0x202A` and `0x202B`, `Utility.RandomMinMax(50, 150)`, sound `0x04B`, puzzle messages, `writer.Write`, and `reader.ReadInt` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only the expected line-ending warning was reported by Git for the edited source file.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-030` is complete.
- No gated behavior changed.
- The `source-batch-028-candidate-discovery.csv` implementation queue is exhausted.
- `SOURCE-BATCH-031+` remains pending candidate discovery.
