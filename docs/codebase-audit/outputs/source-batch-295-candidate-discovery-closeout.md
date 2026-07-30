# SOURCE-BATCH-295 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-295+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB295-CAND-001`
- Batch: `SOURCE-BATCH-295`
- Source file: `Data/Scripts/Items/Potions/Special/EvilSkull.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `EvilSkull.OnDoubleClick(Mobile from)` before backpack checks, mana restore, sound, karma award, or item deletion.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch history: no prior source-batch record for this exact file was found in the current controller history.

## Result

`SB295-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change the backpack-use message, mana restoration amount, sound, karma award, crumble messages, item deletion, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
