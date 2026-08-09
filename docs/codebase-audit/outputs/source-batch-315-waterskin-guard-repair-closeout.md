# SOURCE-BATCH-315 Waterskin Guard Repair Closeout

## Summary

`SOURCE-BATCH-315` implemented `SB315-CAND-001` as a focused, non-gated Waterskin drinking guard repair.

## Source Change

- File: `Data/Scripts/Items/Food/Waterskin.cs`
- Added null/deleted mobile and deleted source drink guards before water checks, backpack checks, fill/drink behavior, and thirst mutation in waterskin double-click paths.
- Added null/deleted mobile guard to `CheckWater`.
- Added null/deleted mobile, null/deleted drink, and missing-backpack guards to `OnDrink`.
- Added null/deleted mobile guard to `DrinkBenefits`.

## Preserved Behavior

- Water target IDs
- `CheckWater` range/LOS behavior
- Fill/drink backpack messages
- Waterskin/canteen item ID/name/weight transitions
- Thirst increments and messages
- `BloodDrinker`/`BrainEater` restrictions
- Dirty waterskin conversion
- `DrinkBenefits` stamina/poison behavior
- Sound/animation/gump behavior
- Consume semantics
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Food/Waterskin.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Food/Waterskin.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and water target, fill/drink, thirst, dirty waterskin, drink benefit, consume, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Food/Waterskin.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-316+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-315` is committed.
