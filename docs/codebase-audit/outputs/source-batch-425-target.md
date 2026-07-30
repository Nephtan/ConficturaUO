# SOURCE-BATCH-425 SpecialHairDye Guard Repair

## Target

- Candidate: `SB425-CAND-001`
- System: `Items:Misc / SpecialHairDye`
- File: `Data/Scripts/Items/Misc/SpecialHairDye.cs`
- Behavior: add stale/null mobile, deleted source dye, and stale gump response guards to `SpecialHairDye.OnDoubleClick(Mobile from)` and `SpecialHairDyeGump.OnResponse(NetState from, RelayInfo info)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Range requirement, `SpecialHairDyeGump` layout, hue entry table, selected hue calculation, no-hair rejection, backpack requirement, hair hue assignment, dye `Delete` behavior, localized messages `501199`, `501200`, `502623`, and `1042010`, sound `0x4E`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
