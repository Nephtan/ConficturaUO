# SOURCE-BATCH-143 DecoWyrmsHeart Guard Repair Closeout

## Summary

Implemented `SB119-CAND-025` from `source-batch-119-candidate-discovery.csv`.

`Data/Scripts/Items/Special/Rares/PaganReagents/DecoWyrmsHeart.cs` now guards stale/null mobile and deleted source item state before sending the existing collectible message and delegating to `base.OnDragLift(from)`.

## Source Changes

- `DecoWyrmsHeart.OnDragLift(Mobile from)` now returns `false` for null/deleted mobiles or deleted source items.

## Preserved Behavior

- Valid-state collectible message remains unchanged.
- Valid-state return remains `base.OnDragLift(from)`.
- Constructor item ID `0xF91`, `Movable`, `Stackable`, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/Rares/PaganReagents/DecoWyrmsHeart.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Special/Rares/PaganReagents/DecoWyrmsHeart.cs`: `0`.

## Verification

- Targeted source scan confirmed the new mobile/source guard, invalid-state `false` return, preserved collectible message, and preserved `base.OnDragLift(from)` return.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-143 is complete and ready to commit. The `source-batch-119-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-144+` requires fresh candidate discovery.
