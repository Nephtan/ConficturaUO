# SOURCE-BATCH-390 ChocolateMonster Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-390`
- Candidate: `SB390-CAND-001`
- System: `Items:Gifts / Holiday / Halloween / ChocolateMonster`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenBag.cs`

## Intended Source Change

Add a local guard to `ChocolateMonster.OnDoubleClick(Mobile m)` so stale/null interaction state cannot dereference an invalid mobile or delete a stale candy item before the existing consumption behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `m == null || m.Deleted || Deleted`.

## Must Stay Unchanged

ChocolateMonster randomized candy names, item IDs, hues, weight, sound `Utility.Random(0x3A, 3)`, human unmounted animation, phrase choices, `PrivateOverheadMessage` behavior, `Stam`, `Hits`, `Mana`, `Thirst`, and `Hunger` assignments, source `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
