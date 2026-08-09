# SOURCE-BATCH-427 RuneOfVirtue Guard Repair

## Target

- Candidate: `SB427-CAND-001`
- System: `Items:Magical / RuneOfVirtue`
- File: `Data/Scripts/Items/Magical/RuneOfVirtue.cs`
- Behavior: add a stale/null mobile and deleted source-rune guard to `RuneOfVirtue.OnDoubleClick(Mobile from)` before existing backpack access and rune cycling behavior.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Backpack-use localized message `1060640`, `RuneLook` name/item ID cycle, morality side hue behavior, owner/side fields, `OnEquip` and `MoralityCheck` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
