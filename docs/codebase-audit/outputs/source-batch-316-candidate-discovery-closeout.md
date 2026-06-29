# SOURCE-BATCH-316 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-316+` selected a clean `DDRelicCoins` guard repair.

## Recommended Target

- Candidate: `SB316-CAND-001`
- Batch: `SOURCE-BATCH-316`
- Source file: `Data/Scripts/Items/Relics/DDRelicCoins.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `DDRelicCoins.OnDoubleClick` before backpack checks, failure messages, or `RelicFlipID` item-id mutation.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- A documentation-truth backlog row mentions relic source evidence, but no active backlog row targets `Data/Scripts/Items/Relics/DDRelicCoins.cs` as its `Files` value.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `NewPlayerTicket` remained excluded because prior discovery tied it to reward selection.
- Housing/addon placement, boats, spell pouches, and broader quest/reward flows remained excluded from this batch unless a later discovery proves a narrower non-policy repair.

## Result

`SB316-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change relic identification guidance, backpack-use failure messages, `RelicFlipID1`/`RelicFlipID2` toggle behavior, relic value data, constructor/name randomization, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
