# SOURCE-BATCH-309 LargeWaxPot Guard Repair Closeout

## Summary

`SOURCE-BATCH-309` implemented `SB309-CAND-001` as a focused, non-gated LargeWaxPot interaction guard repair.

## Source Change

- File: `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs`
- Added null/deleted mobile and deleted source pot guards before durability label display, backpack checks, target assignment, wax amount mutation, wax return, or target handoff.
- Added missing backpack and deleted target item guards in `EndAdd`.
- Added null/deleted mobile, null/deleted source pot, missing backpack, and out-of-backpack source pot guards in `AddPureWaxTarget.OnTarget`.

## Preserved Behavior

- Durability label display for valid state
- Use counts and `MeltedBeeswax`
- `MaxWax`
- `BeeHiveHelper.Find` heat-source rule
- Beeswax amount mutation/delete behavior
- `PlaceInBackpack` wax return behavior
- Overhead messages
- `ItemID` changes
- Sound `43`
- Target range `18`
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and LargeWaxPot success behavior remains present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-310+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-309` is committed.
