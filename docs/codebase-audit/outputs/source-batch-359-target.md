# SOURCE-BATCH-359 Museums Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-359`
- Candidate: `SB359-CAND-001`
- System: `Quests:Museum / Museums`
- File: `Data/Scripts/Quests/Museum/Museum.cs`

## Intended Source Change

Add a local guard to `Museums.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, report antique value, play sounds, or mutate display item state.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Item metadata, `AddNameProperties` behavior, `DiscoverName`, `DiscoverOwner`, `ThisDescription`, `ThisValue`, in-backpack `AntiqueTotalValue` reporting, out-of-backpack `ItemID`/`Light` toggles, sound IDs, museum sale/value helpers, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
