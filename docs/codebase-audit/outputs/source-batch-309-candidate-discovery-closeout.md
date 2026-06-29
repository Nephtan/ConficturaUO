# SOURCE-BATCH-309 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-309+` selected the queued sibling LargeWaxPot guard repair from `SOURCE-BATCH-308` discovery.

## Recommended Target

- Candidate: `SB309-CAND-001`
- Batch: `SOURCE-BATCH-309`
- Source file: `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs`
- Behavior: add stale/null/mobile/source-pot/target item/backpack guards to `apiLargeWaxPot.OnSingleClick`, `OnDoubleClick`, `BeginAdd`, `EndAdd`, and `AddPureWaxTarget.OnTarget` before durability label display, backpack checks, target assignment, wax amount mutation, heat-source checks, wax return, or target handoff.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- None. `SOURCE-BATCH-310+` requires fresh candidate discovery after `SOURCE-BATCH-309` is committed.

## Result

`SB309-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change LargeWaxPot durability labels, uses, `MaxWax`, beeswax amount math, heat-source rule, overhead messages, sound, `ItemID` changes, `PlaceInBackpack` behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
