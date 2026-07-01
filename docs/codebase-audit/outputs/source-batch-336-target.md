# SOURCE-BATCH-336 HolidayFoods Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-336`
- Candidate: `SB336-CAND-001`
- System: `Items:Special / HolidayFoods`
- Source file: `Data/Scripts/Items/Special/Holiday/HolidayFoods.cs`
- Behavior: add stale/null/mobile/source-food guards to `CandyCane.OnDoubleClick(Mobile from)` and `GingerBreadCookie.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `CandyCane.OnDoubleClick` when `from` is null or deleted, or the candy cane is deleted.
- Return immediately from `GingerBreadCookie.OnDoubleClick` when `from` is null or deleted, or the cookie is deleted.

## Must Stay Unchanged

- CandyCane item IDs, `Stackable=false`, and `LootType=Blessed`.
- GingerBreadCookie item IDs, `Stackable=false`, and `LootType=Blessed`.
- Backpack-or-range eligibility: `IsChildOf(from.Backpack) || from.InRange(this, 1)`.
- CandyCane sound `0x3a + Utility.Random(3)`, animation, toothache dictionary/timer behavior, message `1077387`, and source `Delete()` behavior.
- GingerBreadCookie random message table, `base.OnDoubleClick(from)` consumption path, and `SendLocalizedMessageTo(from, result)` behavior.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-336 HolidayFoods Guard Repair`
