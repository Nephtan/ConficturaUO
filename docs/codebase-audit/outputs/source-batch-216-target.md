# SOURCE-BATCH-216 GiftShepherdsCrook Guard Repair

## Target

- Candidate: `SB214-CAND-003`
- Behavior: add stale/null/mobile/source-item and target-callback guards to `GiftShepherdsCrook` herding interaction paths.
- System: `Items:Magical / Gifts / Weapons / GiftShepherdsCrook`
- File: `Data/Scripts/Items/Magical/Gifts/Weapons/Staves/GiftShepherdsCrook.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source crooks before assigning the herding target. Return immediately from herding target callbacks when the mobile is null/deleted or the selected creature is null/deleted.

## Must Stay Unchanged

- Initial herding prompt `502464`.
- Target ranges.
- Animal and tame checks.
- Localized messages `502467`, `502475`, `502468`, `502472`, and `502479`.
- `CheckTargetSkill` behavior.
- Target-location assignment.
- Weapon stats and abilities.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted `OnDoubleClick` and target-callback guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
