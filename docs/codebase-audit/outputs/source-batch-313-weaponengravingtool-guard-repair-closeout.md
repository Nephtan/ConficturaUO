# SOURCE-BATCH-313 WeaponEngravingTool Guard Repair Closeout

## Summary

`SOURCE-BATCH-313` implemented `SB313-CAND-001` as a focused, non-gated WeaponEngravingTool interaction and gump response guard repair.

## Source Change

- File: `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs`
- Added null/deleted mobile and deleted tool guards before reward checks, skill checks, backpack diamond lookup, target assignment, recharge logic, and helper lookup.
- Added null/deleted mobile, deleted tool, and null/deleted target weapon guards before engraving gump creation.
- Added null `NetState`, null `RelayInfo`, null/deleted mobile, deleted tool, and deleted target guards before engraving mutation or response messaging.
- Added null `NetState`, null `RelayInfo`, null/deleted mobile, and deleted engraver guards before confirm-gump recharge.

## Preserved Behavior

- `RewardSystem.CheckIsUsableBy` behavior
- Target prompt
- `BaseWeapon` eligibility
- `InternalGump` layout/text flow
- 64-character truncation
- Blank engraving removal
- Localized messages
- `UsesRemaining` decrement
- Blue diamond and 100000 gold recharge rules
- Guildmaster recharge flow
- `IsRewardItem` persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and reward check, prompt, gump text flow, truncation, blank removal, charge decrement, blue diamond recharge, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization change, and the only gump-adjacent changes are named response guards.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Special/Veteran Rewards/WeaponEngravingTool.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-314+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-313` is committed.
