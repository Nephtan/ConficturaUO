# SOURCE-BATCH-488 WheatSheaf Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-488`
- Candidate: `SB488-CAND-001`
- System: `Items:Food / Cooking / WheatSheaf`
- File: `Data/Scripts/Items/Food/Cooking.cs`
- Behavior: add stale/null mobile, deleted source wheat sheaf, and deleted target item guards to the flour-mill target flow.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add early returns in:

- `WheatSheaf.OnDoubleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `WheatSheaf.OnTarget(Mobile from, object obj)` when `from == null`, `from.Deleted`, `Deleted`, or the resolved target item is deleted

## Must Stay Unchanged

- WheatSheaf item identity
- constructors and metadata
- `Movable` gating
- target range/flags
- `AddonComponent` to addon handling
- `IFlourMill` target eligibility
- `MaxFlour`/`CurFlour` needs calculation
- `mill.CurFlour` increment
- `Consume(needs)` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
