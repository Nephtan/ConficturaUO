# SOURCE-BATCH-188 CarvedPumpkins Guard Repair

## Target

- Candidate: `SB188-CAND-001`
- Behavior: add stale/null/mobile/source-light guards to all twenty `CarvedPumpkin` `OnDoubleClick(Mobile from)` methods before reading `from.InRange` or delegating to `base.OnDoubleClick(from)`.
- System: `Items:Gifts / Halloween / CarvedPumpkins`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/CarvedPumpkins.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Add an early return to each carved pumpkin double-click path when `from == null`, `from.Deleted`, or the pumpkin item is `Deleted`.

## Must Stay Unchanged

- Lit and unlit item IDs.
- Secure-level properties and context-menu behavior.
- Existing out-of-range localized reach message `1019045`.
- Existing valid-state `base.OnDoubleClick(from)` behavior.
- Serialization layout/versioning and `SecureLevel` read/write order.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted guard/range/message/base behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
