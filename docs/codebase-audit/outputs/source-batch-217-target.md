# SOURCE-BATCH-217 RewardCake Guard Repair

## Target

- Candidate: `SB217-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `RewardCake.OnDoubleClick(Mobile from)`.
- System: `Items:Special / RewardCake`
- File: `Data/Scripts/Items/Special/RewardCake.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source cakes before checking range or sending the localized overhead reach message.

## Must Stay Unchanged

- Out-of-range localized overhead message `1019045`.
- Valid in-range no-op behavior.
- Cake label, blessed loot behavior, randomized hue, stackability, and weight.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted `OnDoubleClick` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
