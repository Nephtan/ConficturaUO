# SOURCE-BATCH-389 HalloweenMaiden Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-389`
- Candidate: `SB389-CAND-001`
- System: `Items:Gifts / Holiday / Halloween / HalloweenMaiden`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenMaiden.cs`

## Intended Source Change

Add a local guard to `HalloweenMaiden.OnDoubleClick(Mobile from)` so stale/null interaction state cannot mutate deleted furniture or proceed with an invalid mobile before the existing toggle behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

HalloweenMaiden item IDs `0x124B` and `0x1249`, `Name = "Iron Maiden"`, `Weight = 1.0`, `[Furniture]` classification, valid item-ID toggle behavior, old `Weight == 4.0` normalization in `Deserialize`, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
