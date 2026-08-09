# SOURCE-BATCH-294 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-294+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB294-CAND-001`
- Batch: `SOURCE-BATCH-294`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Minor/PandorasBox.cs`
- Behavior: add stale/null/mobile/source-item guards to `PandorasBox.ConsumeCharge(Mobile from)` and `PandorasBox.OnDoubleClick(Mobile from)` before charge decrement, replacement item creation, backpack add, or bank-box access.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch history: no prior source-batch record for this exact file was found in the current controller history.

## Result

`SB294-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change charge semantics, last-charge replacement behavior, bank box opening, artifact labels, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
