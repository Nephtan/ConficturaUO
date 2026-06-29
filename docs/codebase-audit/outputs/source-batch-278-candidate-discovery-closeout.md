# SOURCE-BATCH-278 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-278+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB278-CAND-001`
- Batch: `SOURCE-BATCH-278`
- Source file: `Data/Scripts/Items/Relics/DDRelicBook.cs`
- Behavior: add a stale/null/mobile/source-item guard to `DDRelicBook.OnDoubleClick(Mobile from)` before sending the relic-book message.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch hits for this exact file: `0`

## Result

`SB278-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change relic valuation, book generation, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
