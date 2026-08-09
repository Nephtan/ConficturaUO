# SOURCE-BATCH-332 EmbalmingFluid Guard Repair Closeout

## Summary

`SOURCE-BATCH-332` implemented `SB332-CAND-001` as a focused, non-gated `EmbalmingFluid` guard repair.

## Source Change

- File: `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs`
- Added null/deleted mobile and deleted source fluid guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted source fluids, missing backpacks, source fluids outside the backpack, and deleted target items before porter charge reads, charge mutation, bottle return, sound/reveal behavior, or source fluid consumption.

## Preserved Behavior

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Fluid-in-pack failure message
- Invalid-target message
- `FrankenPorterItem` eligibility
- `+5` or `+1` charge increment rule
- Charge cap `100`
- Success and already-full messages
- `RevealingAction`
- Sound `0x23E`
- Empty `Bottle` return
- `InvalidateProperties`
- Source fluid `Consume` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and `FrankenPorterItem` eligibility, charge increment/cap, success/full messages, bottle return, reveal, sound, consume, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Quests/Frankenstein/EmbalmingFluid.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-333+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-332` is committed.
