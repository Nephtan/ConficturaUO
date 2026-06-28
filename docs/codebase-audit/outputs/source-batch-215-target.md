# SOURCE-BATCH-215 GiftThrowingGloves Guard Repair

## Target

- Candidate: `SB214-CAND-002`
- Behavior: add stale/null/mobile/source-item guard to `GiftThrowingGloves.OnDoubleClick(Mobile from)`.
- System: `Items:Magical / Gifts / Weapons / GiftThrowingGloves`
- File: `Data/Scripts/Items/Magical/Gifts/Weapons/GiftThrowingGloves.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source gloves before backpack checks or cycling `GloveType`.

## Must Stay Unchanged

- Backpack-use failure message.
- `Stones` / `Axes` / `Knives` / `Darts` / `Stars` cycle.
- Success message text and hue.
- `InvalidateProperties()` call.
- Weapon stats, abilities, range, resource, hue, layer, and metadata.
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
