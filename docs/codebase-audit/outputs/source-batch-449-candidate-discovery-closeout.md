# SOURCE-BATCH-449 Candidate Discovery Closeout

`SOURCE-BATCH-449` ran fresh non-gated candidate discovery after `SOURCE-BATCH-448`.

## Result

Recommended implementation target: `SB449-CAND-001` / `Mistletoe` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Gifts/Holiday/Christmas/Christmas Gifts/Mistletoe.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: Mistletoe add-on, deed, dye, placement, single-click label, and gump response paths dereferenced mobile/source/gump state before complete stale/null guards.
- Completed-file evidence: `Wreath.cs` was excluded because `SOURCE-BATCH-448` already committed its guard repair.

## Skips

- `Wreath.cs` was skipped as already completed.
- `NameChangeDeed.cs` was skipped because its `OnDoubleClick` is a no-op.
- `SmallTent.cs`, `CommodityDeed.cs`, `BaseSuit.cs`, addon, pet, potion, fishing, and spell target families were skipped as policy-sensitive or too broad.
- remaining Government, Invasion, Homestead, StaffTools, economy/reward tuning, region/map, serializer, project/config/data, XML/config/data, and reorganization surfaces remain outside this runner.

## Decision

Proceed with `SOURCE-BATCH-449 Mistletoe Guard Repair`. Keep `SOURCE-BATCH-450+` pending fresh discovery after the source batch commits.
