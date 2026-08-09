# SOURCE-BATCH-292 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-292+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB292-CAND-001`
- Batch: `SOURCE-BATCH-292`
- Source file: `Data/Scripts/Items/Trades/Fishing/Misc/ShipwreckedItem.cs`
- Behavior: add stale/null/mobile/source-item/dye-tub guards to `ShipwreckedItem.OnSingleClick(Mobile from)` and `ShipwreckedItem.Dye(Mobile from, DyeTub sender)` before label sends, hue assignment, or failure-message sends.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch history: no prior source-batch record for this exact file was found in the current controller history.

## Result

`SB292-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change shipwreck label values, property labels, eligible armor dye range, hue assignment, ineligible-item failure messaging, `IShipwreckedItem` behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
