# SOURCE-BATCH-309 LargeWaxPot Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-309`
- Candidate: `SB309-CAND-001`
- Behavior: add stale/null/mobile/source-pot/target item/backpack guards to LargeWaxPot interaction paths.
- System: `Trades:Apiculture / LargeWaxPot`
- File: `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs`

## Allowed Source Change

Add guard-only checks to:

- `apiLargeWaxPot.OnSingleClick(Mobile from)`
- `apiLargeWaxPot.OnDoubleClick(Mobile from)`
- `apiLargeWaxPot.BeginAdd(Mobile from)`
- `apiLargeWaxPot.EndAdd(Mobile from, object o)`
- `AddPureWaxTarget.OnTarget(Mobile from, object targeted)`

The guards may return early for null/deleted mobiles, deleted source pots, missing backpacks, deleted target items, or stale target state before dereferences, target assignment, wax mutation, wax return, or target handoff.

## Must Stay Unchanged

- Durability label display for valid state
- Use counts and `MeltedBeeswax`
- `MaxWax`
- `BeeHiveHelper.Find` heat-source rule
- Beeswax amount mutation and delete behavior
- `PlaceInBackpack` wax return behavior
- Overhead messages
- `ItemID` changes
- Sound `43`
- Target range `18`
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-309 LargeWaxPot Guard Repair
```
