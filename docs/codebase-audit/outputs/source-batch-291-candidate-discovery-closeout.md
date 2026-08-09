# SOURCE-BATCH-291 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-291+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB291-CAND-001`
- Batch: `SOURCE-BATCH-291`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs`
- Behavior: add a stale/null/mobile/source-item guard to `UncutCloth.OnSingleClick(Mobile from)` before localized folded-cloth label packet send.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch history: `SOURCE-BATCH-106` covered `Dye` and `Scissor` guards for this exact file; this target covers the remaining `OnSingleClick` surface.

## Result

`SB291-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change localized label values, amount text, dye/scissor behavior, `ICommodity` behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
