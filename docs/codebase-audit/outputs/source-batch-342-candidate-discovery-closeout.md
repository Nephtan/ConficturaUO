# SOURCE-BATCH-342 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-342` ran fresh candidate discovery after `SOURCE-BATCH-341` and identified a small set of quest artifact `OnDoubleClick` guard repairs with zero exact-file POST-BATCH-Y gate hits and zero exact-file active overlay rows.

## Recommended Target

- Candidate: `SB342-CAND-001`
- Batch: `SOURCE-BATCH-342`
- System: `Quests:Serpents / LanternOfDiscipline`
- Source file: `Data/Scripts/Quests/Serpents/LanternOfDiscipline.cs`
- Behavior: add stale/null/mobile/source-item guards before reading `from.Backpack` or sending the existing lantern messages.

## Deferred Sibling Candidates

- `SB342-CAND-002` / `SOURCE-BATCH-343` / `OrbOfLogic`
- `SB342-CAND-003` / `SOURCE-BATCH-344` / `ScalesOfEthicality`
- `SB342-CAND-004` / `SOURCE-BATCH-345` / `BookOfTruth`
- `SB342-CAND-005` / `SOURCE-BATCH-346` / `CandleOfLove`
- `SB342-CAND-006` / `SOURCE-BATCH-347` / `BellOfCourage`
- `SB342-CAND-007` / `SOURCE-BATCH-348` / `ShardOfHatred`
- `SB342-CAND-008` / `SOURCE-BATCH-349` / `ShardOfFalsehood`
- `SB342-CAND-009` / `SOURCE-BATCH-350` / `ShardOfCowardice`

## Fence Evidence

- Every listed candidate has exact-file POST-BATCH-Y gate hits: `0`.
- Every listed candidate has exact-file active overlay rows: `0`.
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization gate is crossed.

## Result

Proceed with `SOURCE-BATCH-342 LanternOfDiscipline Guard Repair`, then continue with the deferred sibling candidates one item per source batch if their preflight checks remain clean.
