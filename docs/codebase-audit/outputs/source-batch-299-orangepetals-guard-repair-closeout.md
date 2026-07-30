# SOURCE-BATCH-299 OrangePetals Guard Repair Closeout

## Summary

`SOURCE-BATCH-299` implemented `SB299-CAND-001` in `Data/Scripts/Trades/Gardening/MiscItems/OrangePetals.cs`.

`OrangePetals.CheckItemUse(Mobile from, Item item)` and `OrangePetals.OnDoubleClick(Mobile from)` now return safely for null/deleted mobiles or deleted orange petals. `OnDoubleClick` also applies the existing RootParent use requirement before context checks, timer creation, or item consumption. The context helpers now return safely for null mobile/context state, and `OrangePetalsTimer.OnTick()` sends the wear-off message only when its stored mobile is still valid.

## Preserved Behavior

- RootParent use requirement and backpack-use message `1042038`.
- Already-under-effect message `1061904`.
- Success message `1061905`.
- Sound `0x3B`.
- Five-minute effect timer duration.
- Wear-off message text.
- `AddContext`, `RemoveContext`, and `UnderEffect` behavior for valid mobiles.
- Orange petals `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-orange-petals/context guards and preserved RootParent checks, backpack-use message, effect-active message, success message, sound, five-minute timer duration, wear-off message, `Consume()` behavior, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, packet handler, region, startup, project, XML/config/data, or reorganization changes; the only timer/message diff context was the existing wear-off message guarded by a mobile validity check.
- Passed: changed-file scan found only `Data/Scripts/Trades/Gardening/MiscItems/OrangePetals.cs` as a tracked source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
