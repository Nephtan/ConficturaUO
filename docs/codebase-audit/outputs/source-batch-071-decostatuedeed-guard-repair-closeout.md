# SOURCE-BATCH-071 DecoStatueDeed Guard Repair Closeout

## Summary

`SOURCE-BATCH-071` completed fresh candidate discovery, selected `SB071-CAND-001`, and implemented the `DecoStatueDeed` guard repair.

`Data/Scripts/Items/Decorations/DecoIngotDeed.cs` now guards null/deleted mobiles, deleted source deeds, and missing backpacks before backpack, random decoration generation, or deed deletion behavior is evaluated.

## Preservation

The batch preserved:

- Backpack message `1042001`.
- Existing `Utility.RandomMinMax` calls and all generated decoration item types.
- `AddToBackpack` behavior and deed `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Decorations/DecoIngotDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Decorations/DecoIngotDeed.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-071-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-deed/backpack guards and preserved decoration generation evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-071` is closed. `SOURCE-BATCH-072+` remains pending the next concrete non-gated source target from `source-batch-071-candidate-discovery.csv`: `SB071-CAND-002` / `MonsterStatueDeed Guard Repair`.
