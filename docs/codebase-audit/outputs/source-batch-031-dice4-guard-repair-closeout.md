# SOURCE-BATCH-031 Closeout: Dice4 Guard Repair

Reviewed at: 2026-06-16T20:25:00-05:00

## Summary

`SOURCE-BATCH-031` implemented `SB031-CAND-001`, a non-gated `Dice4` guard repair in `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`.

The batch adds stale/null safety before mobile, deleted dice, range, telekinesis, sound, or roll-message state is evaluated. It preserves dice behavior, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles or deleted dice before range checks and rolling.
- `OnTelekinesis(Mobile from)` now returns for null/deleted mobiles or deleted dice before particle effects, sound, and rolling.

## Preserved Behavior

- ItemID `0x301C`.
- Name and weight.
- `AddNameProperties` labels.
- Range `2`.
- Telekinesis effects and sound `0x1F5`.
- Roll overhead message.
- `Utility.Random(1, 4)`.
- Sound `0x34`.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/Dice4.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile/deleted-item guards.
- Targeted source scan confirmed `AddNameProperties`, telekinesis effects, sound `0x1F5`, `Utility.Random(1, 4)`, sound `0x34`, `writer.Write`, and `reader.ReadInt` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only the expected line-ending warning was reported by Git for the edited source file.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-031` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-032+` remains pending the next concrete non-gated source target from `source-batch-031-candidate-discovery.csv`.
