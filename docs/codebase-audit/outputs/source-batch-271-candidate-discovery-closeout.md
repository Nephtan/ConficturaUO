# SOURCE-BATCH-271 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-271+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB271-CAND-001`
- Batch: `SOURCE-BATCH-271`
- Source file: `Data/Scripts/Items/Technology/SpaceDyes.cs`
- Behavior: add stale/null/mobile/source-dye/target-item guards to `SpaceDyes.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, hue mutation, sound/revealing action, bottle return, or dye consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file audit row is `Documented` and remains nonblocking because this batch does not change documentation policy, serializer behavior, source layout, or policy behavior.
- Prior source-batch intake hits for this exact file: `0`

## Result

`SB271-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change dye eligibility, color behavior, source dye consumption, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
