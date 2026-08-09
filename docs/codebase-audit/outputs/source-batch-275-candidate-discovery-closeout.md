# SOURCE-BATCH-275 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-275+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB275-CAND-001`
- Batch: `SOURCE-BATCH-275`
- Source file: `Data/Scripts/Items/Technology/MaterialLiquifier.cs`
- Behavior: add stale/null/mobile/item/backpack/gump guards to `MaterialLiquifier.OnDoubleClick`, `OnDragDrop`, `GetColor`, and `MaterialLiquifierGump.OnResponse` before backpack checks, dropped-item handling, bottle lookup, material dye creation, charge consumption, or gump sound response.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file audit rows are `Documented`, `FalsePositive`, and `ReviewedNoChange` and remain nonblocking because this batch does not change documentation policy, serializer behavior, source layout, material/reward tuning, or gated behavior.
- Prior source-batch hits for this exact file: `0`

## Result

`SB275-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change material matching, dye colors, bottle consumption, dropped-item destruction, charge consumption, gump layout, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
