# SOURCE-BATCH-280 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-280+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB280-CAND-001`
- Batch: `SOURCE-BATCH-280`
- Source file: `Data/Scripts/Items/Magical/SavageTalisman.cs`
- Behavior: add stale/null/mobile/source-item guards to `SavageTalisman.OnEquip(Mobile from)` and `OnDoubleClick(Mobile from)` before owner-message handling or worn-slot messaging.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch hits for this exact file: `0`

## Result

`SB280-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change owner restrictions, talisman skill bonuses, messages, `ItemOwner` serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
