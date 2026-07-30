# SOURCE-BATCH-223 HugeWaterTub Guard Repair

## Target

- Candidate: `SB223-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `HugeWaterTub.OnDoubleClick(Mobile from)`.
- System: `Items:Special / Rares / Containers / HugeWaterTub`
- File: `Data/Scripts/Items/Special/Rares/Containers/HugeWaterTub.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source water tubs before thirst reads, thirst mutation, drink messages, animation, or sound playback.

## Must Stay Unchanged

- Thirst threshold and mutation behavior.
- Drink and quenched messages.
- Human/unmounted animation rule.
- Sound playback.
- Furniture metadata.
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
