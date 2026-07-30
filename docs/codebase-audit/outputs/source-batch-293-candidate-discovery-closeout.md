# SOURCE-BATCH-293 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-293+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB293-CAND-001`
- Batch: `SOURCE-BATCH-293`
- Source file: `Data/Scripts/Items/Special/Rares/Containers/BaseWaterContainer.cs`
- Behavior: add stale/null/mobile/source-container/drop-item guards to `BaseWaterContainer.OnDoubleClick`, `BaseWaterContainer.OnSingleClick`, and `BaseWaterContainer.OnDragDropInto` before forwarding to base container behavior.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch history: no prior source-batch record for this exact file was found in the current controller history.

## Result

`SB293-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change quantity semantics, item ID switching, movable state, empty-container forwarding behavior, the current `OnSingleClick` call to `base.OnDoubleClick`, drag/drop rejection when full, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
