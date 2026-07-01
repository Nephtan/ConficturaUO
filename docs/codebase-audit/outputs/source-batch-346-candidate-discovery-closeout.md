# SOURCE-BATCH-346 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-346` continued the clean Shadowlords artifact sibling queue and re-confirmed `CandleOfLove` as the next narrow non-gated target.

## Recommended Target

- Candidate: `SB346-CAND-001`
- Batch: `SOURCE-BATCH-346`
- System: `Quests:Shadowlords / CandleOfLove`
- Source file: `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs`
- Behavior: add stale/null/mobile/source-item guards before reading `from.Backpack` or sending the existing candle messages.

## Fence Evidence

- `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs` has exact-file POST-BATCH-Y gate hits: `0`.
- `Data/Scripts/Quests/Shadowlords/CandleOfLove.cs` has exact-file active overlay rows: `0`.
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization gate is crossed.

## Result

Proceed with `SOURCE-BATCH-346 CandleOfLove Guard Repair`, then continue with `BellOfCourage` if exact-file preflight remains clean.
