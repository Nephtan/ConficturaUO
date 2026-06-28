# SOURCE-BATCH-112 CurseItem Guard Repair Closeout

## Summary

Implemented `SB111-CAND-002` from `source-batch-111-candidate-discovery.csv`.

`Data/Scripts/Items/Traps/CurseItem.cs` now guards stale/null mobile and deleted source item state before sending the existing cursed-item message.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source items.

## Preserved Behavior

- Valid-state message remains `This item is cursed and cannot be used.`
- Constructor item ID `0x9A8`, lock settings, name, and weight are unchanged.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Traps/CurseItem.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Traps/CurseItem.cs`: `0`.

## Verification

- Targeted source scan confirmed the new mobile/source guard and preserved message.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-112 is complete and ready to commit. `SOURCE-BATCH-113+` remains pending the next concrete non-gated source target from `source-batch-111-candidate-discovery.csv`.
