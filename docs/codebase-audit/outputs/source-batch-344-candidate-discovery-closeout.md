# SOURCE-BATCH-344 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-344` continued the clean quest artifact sibling queue from `SOURCE-BATCH-343` and re-confirmed `ScalesOfEthicality` as the next narrow non-gated target.

## Recommended Target

- Candidate: `SB344-CAND-001`
- Batch: `SOURCE-BATCH-344`
- System: `Quests:Serpents / ScalesOfEthicality`
- Source file: `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs`
- Behavior: add stale/null/mobile/source-item guards before reading `from.Backpack` or sending the existing scales messages.

## Fence Evidence

- `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs` has exact-file POST-BATCH-Y gate hits: `0`.
- `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs` has exact-file active overlay rows: `0`.
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization gate is crossed.

## Result

Proceed with `SOURCE-BATCH-344 ScalesOfEthicality Guard Repair`, then continue with `BookOfTruth` if exact-file preflight remains clean.
