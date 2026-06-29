# SOURCE-BATCH-264 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-264+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB264-CAND-001`
- Batch: `SOURCE-BATCH-264`
- Source file: `Data/Scripts/Items/Construction/Addons/DartBoard.cs`
- Behavior: add stale/null/mobile/source-component guards to `DartBoard.OnDoubleClick(Mobile from)` and `DartBoard.Throw(Mobile from)` before direction, range, LOS, weapon, animation, effect, sound, scoring, or reach-message paths.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file save-compat rows are `IntentionalLegacy` and remain nonblocking because this batch does not edit serialization.
- Prior source-batch intake hits for this exact file: `0`

## Result

`SB264-CAND-001` is ready for a focused non-gated source batch. Gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded.
