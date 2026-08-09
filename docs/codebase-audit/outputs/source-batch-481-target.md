# SOURCE-BATCH-481 LightOfTheWinterSolstice Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-481`
- Candidate: `SB481-CAND-001`
- System: `Items:Gifts / Christmas / LightOfTheWinterSolstice`
- File: `Data/Scripts/Items/Gifts/Holiday/Christmas/Christmas Gifts/LightOfTheWinterSolstice.cs`
- Behavior: add stale/null mobile and deleted source item guards to the winter-solstice gift label interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `LightOfTheWinterSolstice.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- `LightOfTheWinterSolstice` item identity
- item ID `0x236E`
- flipable IDs
- staff-name source array
- `Dipper` command property and persistence
- `Weight`, `LootType`, `Light`, and `Hue` metadata
- base `OnSingleClick` behavior for valid mobiles
- localized labels `1070881` and `1070880`
- `GetProperties` labels
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
