# SOURCE-BATCH-209 AppleBobbingBarrel Guard Repair

## Target

- Candidate: `SB206-CAND-004`
- Behavior: add stale/null/mobile/source-item and timer callback guards to `AppleBobbingBarrel`.
- System: `Items:Gifts / Holiday / Halloween / AppleBobbingBarrel`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Decorations/AppleBobbingBarrel.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted barrels before starting bobbing. In the delayed callback, return for null/deleted mobiles, release `CantWalk`, and then return if the source barrel was deleted before reward/messaging.

## Must Stay Unchanged

- Mounted rejection message.
- Bobbing start message.
- `CantWalk` true/false transition for valid bobbing.
- Direction, animation IDs, sound `37`, delay duration `6` seconds, apple chance `.30`, apple reward, success/failure messages, and public overhead messages.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted double-click/timer guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
