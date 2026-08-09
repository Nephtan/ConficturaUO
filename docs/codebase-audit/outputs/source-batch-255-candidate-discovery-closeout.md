# SOURCE-BATCH-255 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-255+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB255-CAND-001`
- Batch: `SOURCE-BATCH-255`
- Source file: `Data/Scripts/Items/Magical/Gifts/Jewels/MagicLantern.cs`
- Behavior: add stale/null/mobile/source-item guard to `GiftLantern.OnDoubleClick(Mobile from)` before layer lookup, backpack checking, equip, and unequip paths.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Prior source-batch intake hits for this exact file: `0`

## Result

`SB255-CAND-001` is ready for a focused non-gated source batch. Gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded.
