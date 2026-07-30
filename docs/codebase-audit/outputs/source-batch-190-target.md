# SOURCE-BATCH-190 HalloweenPack Guard Repair

## Target

- Candidate: `SB189-CAND-002`
- Behavior: add stale/null/mobile/source-package/backpack guards to `HalloweenPack.OnDoubleClick(Mobile from)`.
- System: `Items:Gifts / Halloween / HalloweenPack`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenPack.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately when `from == null`, `from.Deleted`, or the Halloween pack item is deleted. Treat missing backpack or out-of-backpack source pack as the existing backpack-use failure message.

## Must Stay Unchanged

- `HalloweenGift` creation.
- Existing backpack-use failure message.
- Existing private overhead message text.
- Existing `Delete()` consumption semantics.
- `AddNameProperties` label.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted guard/backpack/reward/message/delete scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
