# SOURCE-BATCH-428 SoulLantern Guard Repair

## Target

- Candidate: `SB428-CAND-001`
- System: `Magic:Death Knight / SoulLantern`
- File: `Data/Scripts/Magic/Death Knight/SoulLantern.cs`
- Behavior: add a stale/null mobile and deleted source-lantern guard to `SoulLantern.OnDoubleClick(Mobile from)` before existing equipped-layer lookup and equip/unequip behavior.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Owner comparison, equipped-layer detection, backpack-use localized message `1042001`, item ID transitions `0xA18` and `0xA15`, sounds `0x4BB` and `0x47`, base `OnRemoved`/`OnEquip` calls, `TrappedSouls` and `owner` fields, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
