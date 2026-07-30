# SOURCE-BATCH-321 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-321+` selected a clean `DDRelicGrave` guard repair.

## Recommended Target

- Candidate: `SB321-CAND-001`
- Batch: `SOURCE-BATCH-321`
- Source file: `Data/Scripts/Items/Relics/DDRelicGrave.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `DDRelicGrave.OnDoubleClick` before backpack checks, failure messages, or `RelicFlipID` item-id mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Other relic flip classes remain available only for later one-file discovery and verification.
- Housing/addon placement, boats, spell pouches, and broader quest/reward flows remained excluded from this batch unless a later discovery proves a narrower non-policy repair.

## Result

`SB321-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change relic identification guidance, backpack-use failure messages, `RelicFlipID1`/`RelicFlipID2` toggle behavior, grave description properties, relic value data, constructor/name randomization, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
