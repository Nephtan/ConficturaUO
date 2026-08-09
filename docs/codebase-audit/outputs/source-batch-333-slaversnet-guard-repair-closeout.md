# SOURCE-BATCH-333 SlaversNet Guard Repair Closeout

## Summary

`SOURCE-BATCH-333` implemented `SB333-CAND-001` as a focused, non-gated `SlaversNet` guard repair.

## Source Change

- File: `Data/Scripts/Items/Special/SlaversNet.cs`
- Added null/deleted mobile and deleted source net guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted source nets, missing backpacks, source nets outside the backpack, and deleted target mobiles before creature capture checks, capture mutation, sound behavior, or source net deletion.

## Preserved Behavior

- Target range `6`
- Localized backpack message `1060640`
- Target prompt
- Invalid-target and capture failure messages
- `BaseCreature` eligibility
- Paragon, tamable, controlled, and follower-slot checks
- `Utility.RandomMinMax(50, 200)` capture odds
- `Utility.RandomBool()` torn-net branch
- `ControlSlots` mutation
- `MinTameSkill` clamp
- `SetControlMaster`, `ControlTarget`, `IsBonded`, and `ControlOrder` behavior
- Invisible-pet visibility fixes
- Sound `0x059`
- Source net `Delete` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/SlaversNet.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Special/SlaversNet.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and `BaseCreature` eligibility, capture/follower checks, capture odds, control mutation, visibility fixes, sound, delete, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Special/SlaversNet.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-334+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-333` is committed.
