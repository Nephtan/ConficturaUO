# SOURCE-BATCH-466 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-466` used the source-batch controller and executive decision intake to select the next narrow non-gated source repair after `SOURCE-BATCH-465`.

## Recommended Target

- Candidate: `SB466-CAND-001`
- Target: `ResearchBag`
- File: `Data/Scripts/Magic/Research/ResearchBag.cs`
- Reason: `ResearchBag.OnDoubleClick(Mobile from)` dereferenced `from.Backpack` before stale/null mobile, deleted source item, or missing-backpack guards.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Candidate Notes

- Completed source-batch files were excluded using the intake register.
- Government, staff/access, economy/trades, housing, region/map, serializer layout, project/config/data, XML/config/data, and reorganization candidates remain excluded by executive policy.
- `ResearchBag` is selected because the edit is a one-method guard-only fix around an existing research gump-open path.
- `SOURCE-BATCH-467+` should run fresh discovery after `SOURCE-BATCH-466` is committed.
