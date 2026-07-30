# SOURCE-BATCH-075 DwarvenForge Guard Repair Closeout

## Summary

`SOURCE-BATCH-075` completed fresh candidate discovery, selected `SB075-CAND-001`, and implemented the `DwarvenForge` guard repair.

`Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs` now guards null/deleted mobiles and deleted source forges before backpack, range, movable, or forge toggle state is evaluated.

## Preservation

The batch preserved:

- Secure-in-home failure message.
- Range failure message.
- Movable-item rejection.
- `ItemID` toggle between `0x544A` and `0x544B`.
- `LightType.Empty` / `LightType.Circle225` behavior.
- Sound `0x208`.
- `OnDragLift` reset behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Blacksmith Items/DwarvenForge.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-075-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-forge guards and preserved toggle behavior evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-075` is closed. The `source-batch-075-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-076+` requires fresh candidate discovery.
