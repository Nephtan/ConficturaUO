# SOURCE-BATCH-270 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-270+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB270-CAND-001`
- Batch: `SOURCE-BATCH-270`
- Source file: `Data/Scripts/Items/Technology/PlasmaTorch.cs`
- Behavior: add stale/null/mobile/source-tool guards to `PlasmaTorch.OnDoubleClick(Mobile from)` and `UnlockTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, lock/trap/door checks, messages, sound, revealing action, or source torch consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file audit row is `Documented` and remains nonblocking because this batch does not change documentation policy, serializer behavior, source layout, or policy behavior.
- Prior source-batch intake hits for this exact file: `0`

## Result

`SB270-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change lock/trap/door behavior, source torch consumption, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
