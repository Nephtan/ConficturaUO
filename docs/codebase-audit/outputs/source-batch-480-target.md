# SOURCE-BATCH-480 RunicSewingKit Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-480`
- Candidate: `SB480-CAND-001`
- System: `Items:Trades / Tools / RunicSewingKit`
- File: `Data/Scripts/Items/Trades/Tools/RunicSewingKit.cs`
- Behavior: add stale/null mobile and deleted source item guards to the runic sewing kit label interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `RunicSewingKit.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- `RunicSewingKit` item identity
- item ID `0x4C81`
- `Name` value
- weight
- hue/resource behavior
- `CraftSystem` binding to `DefTailoring.CraftSystem`
- `AddNameProperty` label behavior
- valid `OnSingleClick` localized label behavior
- uses behavior inherited from `BaseRunicTool`/`BaseTool`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
