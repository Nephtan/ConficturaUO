# SOURCE-BATCH-353 ObeliskTip Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-353`
- Candidate: `SB353-CAND-001`
- System: `Quests:Pagan / ObeliskTip`
- File: `Data/Scripts/Quests/Pagan/ObeliskTip.cs`

## Intended Source Change

Add local guards to `ObeliskTip.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, `from.Backpack`, or a deleted source item before the existing owner and gump flow runs.

Allowed changes:

- Return immediately when `from == null || from.Deleted`.
- Use the existing localized backpack-use message when the source item is deleted, the mobile has no backpack, or the item is outside the backpack.

## Must Stay Unchanged

Item metadata, owner properties, the existing localized backpack-use message, owner mismatch message, account lookup/return/delete behavior, `ObeliskGump` type and constructor arguments, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
