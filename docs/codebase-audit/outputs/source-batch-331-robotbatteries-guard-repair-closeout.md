# SOURCE-BATCH-331 RobotBatteries Guard Repair Closeout

## Summary

`SOURCE-BATCH-331` implemented `SB331-CAND-001` as a focused, non-gated `RobotBatteries` guard repair.

## Source Change

- File: `Data/Scripts/Quests/Robots/RobotBatteries.cs`
- Added null/deleted mobile and deleted source battery guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted source batteries, missing backpacks, source batteries outside the backpack, and deleted target items before robot charge reads, charge mutation, sound/reveal behavior, or source battery deletion.

## Preserved Behavior

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Robot-in-pack failure message
- Invalid-target message
- `RobotItem` eligibility
- `+1` charge increment
- Charge cap `100`
- Success and already-full messages
- `RevealingAction`
- Sound `0x559`
- `InvalidateProperties`
- Source battery `Delete` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Robots/RobotBatteries.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Robots/RobotBatteries.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and `RobotItem` eligibility, charge increment/cap, success/full messages, reveal, sound, delete, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Quests/Robots/RobotBatteries.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-332+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-331` is committed.
