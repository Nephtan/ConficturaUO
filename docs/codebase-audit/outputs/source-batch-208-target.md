# SOURCE-BATCH-208 SnowPile Guard Repair

## Target

- Candidate: `SB206-CAND-003`
- Behavior: add stale/null/mobile/source-item and target guards to `SnowPile` snowball interaction paths.
- System: `Items:Gifts / Holiday / Christmas / SnowPile`
- File: `Data/Scripts/Items/Gifts/Holiday/Christmas/Christmas Gifts/SnowPile.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source snow before reading backpack, mounted, action, target, or timer state. Treat deleted target objects as the existing invalid-target snowball outcome.

## Must Stay Unchanged

- Backpack-use message `1042010`.
- Mounted rejection message `1010097`.
- Snow packing message `1005575`.
- Cooldown message `1005574` and `BeginAction`/`EndAction` timing.
- Target range `10`, self-target rejection, valid reciprocal snow holder rule, snowball hit/miss messages, sound `0x145`, animation, moving-effect hue `0x480`, and serialization.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted double-click/target/timer guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
