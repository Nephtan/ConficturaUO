# SOURCE-BATCH-272 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-272+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB272-CAND-001`
- Batch: `SOURCE-BATCH-272`
- Source file: `Data/Scripts/Items/Technology/Chainsaw.cs`
- Behavior: add stale/null/mobile/source-tool/backpack guards to `Chainsaw.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, log range checks, skill/resource checks, board conversion, or charge consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file audit row is `Documented` and remains nonblocking because this batch does not change documentation policy, serializer behavior, source layout, economy/crafting tuning, or policy behavior.
- Prior source-batch hits for this exact file: `0`

## Result

`SB272-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change log eligibility, skill thresholds, board conversion, charge consumption, broken-tool replacement, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
