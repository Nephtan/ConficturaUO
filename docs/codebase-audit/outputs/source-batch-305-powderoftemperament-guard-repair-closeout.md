# SOURCE-BATCH-305 PowderOfTemperament Guard Repair Closeout

## Summary

`SOURCE-BATCH-305` implemented `SB305-CAND-001` from `source-batch-305-candidate-discovery.csv`.

The batch adds stale/null/mobile/source-powder/target-item/backpack guards to `Data/Scripts/Items/Special/Bulk Order Rewards/Blacksmithy/PowderOfTemperament.cs` without crossing POST-BATCH-Y gates or changing powder durability behavior.

## Source Changes

- `OnSingleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the powder is deleted before durability label display.
- `OnDoubleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the powder is deleted before backpack checks or target assignment.
- `OnDoubleClick(Mobile from)` now treats a missing backpack as the existing pack-use failure path.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately for invalid mobiles and handles null/deleted/used-up source powder state before target inspection.
- Deleted target items and missing backpacks use the existing pack-use failure message path before durability mutation.

## Preserved Behavior

- Durability label display.
- Target assignment.
- Pack-use message `1042001`.
- Used-up message `1049086`.
- `CanFortify` rejection message `1049083`.
- Success message `1049084`.
- Cannot-improve message `1049085`.
- `Core.AOS ? 255 : wearable.InitMaxHits` calculation.
- Bonus cap of `10`.
- Durability scaling/unscaling and 255 caps.
- `UsesRemaining` decrement and source powder delete semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/Bulk Order Rewards/Blacksmithy/PowderOfTemperament.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Special/Bulk Order Rewards/Blacksmithy/PowderOfTemperament.cs`: `0`.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval was crossed.

## Verification

- Candidate CSV import: passed; `SB305-CAND-001` is recommended with gate hits `0` and active overlay rows `0`.
- Targeted source scan: passed; mobile, powder, target, deleted item, backpack, label, `CanFortify`, bonus cap, use decrement/delete, and serializer-preservation evidence found.
- POST-BATCH-Y exact-file gate scan: passed with `0` hits.
- Active overlay exact-file scan: passed with `0` rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Changed-file scan: passed; no project/config/data files changed.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-306+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-305` is committed. `JarsOfWax` remains a sibling candidate from this discovery, but it still requires exact-file gate/overlay preflight before source edits.
