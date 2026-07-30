# SOURCE-BATCH-482 Puke Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-482`
- Candidate: `SB482-CAND-001`
- System: `Items:Misc / Puke`
- File: `Data/Scripts/Items/Misc/Puke.cs`
- Behavior: add stale/null mobile and deleted source item guards to the item name label interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `Puke.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- `Puke` item identity
- random item IDs `0xF3B` and `0xF3C`
- `Name` value
- `Hue` value
- `Movable=false` behavior
- `ItemRemovalTimer` setup and delete behavior
- `Deserialize` delete behavior
- valid `OnSingleClick` name label behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
