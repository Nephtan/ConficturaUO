# SOURCE-BATCH-465 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-465` used the source-batch controller and executive decision intake to select the next narrow non-gated source repair after `SOURCE-BATCH-464`.

## Recommended Target

- Candidate: `SB465-CAND-001`
- Target: `BagOfTricks`
- File: `Data/Scripts/Magic/Jester/BagOfTricks.cs`
- Reason: `BagOfTricks.OnDoubleClick(Mobile from)` dereferenced `from.Backpack` before stale/null mobile, deleted source item, or missing-backpack guards.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Candidate Notes

- Completed book/note candidates were excluded using the source-batch intake register.
- Government, staff/access, economy/trades, housing, region/map, serializer layout, project/config/data, XML/config/data, and reorganization candidates remain excluded by executive policy.
- `BagOfTricks` is selected because the edit is a one-method guard-only fix around an existing gump-open path.
- `SOURCE-BATCH-466+` should run fresh discovery after `SOURCE-BATCH-465` is committed.
