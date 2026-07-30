# SOURCE-BATCH-329 GrapplingHook Guard Repair Closeout

## Summary

`SOURCE-BATCH-329` implemented `SB329-CAND-001` as a focused, non-gated `GrapplingHook` guard repair.

## Source Change

- File: `Data/Scripts/Items/Boats/GrapplingHook.cs`
- Added null/deleted mobile and deleted source hook guards before backpack checks or target assignment.
- Added target callback guards for null/deleted mobiles, null mobile maps, and deleted target creatures before pirate ship lookup or teleport behavior.

## Preserved Behavior

- Target range `20`
- Localized backpack message `1060640`
- Target prompt
- Invalid-target message
- `BaseCreature` target eligibility
- `BaseBoat.GetPirateShip` lookup
- Valid `loc.X`/`loc.Y` rule
- `DoTeleport` call
- Sound randomization
- `BaseCreature.TeleportPets`
- `MoveToWorld`
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Boats/GrapplingHook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Boats/GrapplingHook.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and pirate ship lookup, teleport call, pet teleport, move-to-world, invalid-target message, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Boats/GrapplingHook.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-330+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-329` is committed.
