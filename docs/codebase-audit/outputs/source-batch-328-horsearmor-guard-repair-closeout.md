# SOURCE-BATCH-328 HorseArmor Guard Repair Closeout

## Summary

`SOURCE-BATCH-328` implemented `SB328-CAND-001` as a focused, non-gated `HorseArmor` guard repair.

## Source Change

- File: `Data/Scripts/Items/Armor/HorseArmor.cs`
- Added null/deleted mobile and deleted source armor guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted source armor, missing backpacks, source armor outside the backpack, and deleted target mobiles before mount checks, stat mutation, or source armor consumption.

## Preserved Behavior

- Horse and `ZebraRiding` eligibility
- `ControlMaster` ownership rule
- `BaseMount` requirement
- Target range `8`
- Localized backpack message `1060640`
- Horse-only failure message
- Material hue mapping
- Stat, resistance, and skill mutation
- Sound `0x0AA`
- Source armor `Consume()` behavior
- `ArmorMaterial` persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Armor/HorseArmor.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Armor/HorseArmor.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and horse eligibility, ownership, mount, stat mutation, consume, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Armor/HorseArmor.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-329+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-328` is committed.
