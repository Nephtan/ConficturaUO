# SOURCE-BATCH-330 BoatStain Guard Repair Closeout

## Summary

`SOURCE-BATCH-330` implemented `SB330-CAND-001` as a focused, non-gated `BoatStain` guard repair.

## Source Change

- File: `Data/Scripts/Items/Boats/BoatStain.cs`
- Added null/deleted mobile and deleted source stain guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null/deleted source stains, missing backpacks, source stains outside the backpack, and deleted target items before boat deed eligibility checks or hue mutation.

## Preserved Behavior

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Docked-ship-in-pack message
- Invalid-target message
- `BaseBoatDeed` and `BaseDockedBoat` eligibility
- Hue `0x5BE` assignment
- `RevealingAction`
- Sound `0x23E`
- Non-consuming source stain behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Boats/BoatStain.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Boats/BoatStain.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and boat deed eligibility, hue assignment, reveal, sound, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Boats/BoatStain.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-331+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-330` is committed.
