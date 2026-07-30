# SOURCE-BATCH-347 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-347` continued the clean Shadowlords artifact sibling queue and re-confirmed `BellOfCourage` as the next narrow non-gated target.

## Recommended Target

- Candidate: `SB347-CAND-001`
- Batch: `SOURCE-BATCH-347`
- System: `Quests:Shadowlords / BellOfCourage`
- Source file: `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs`
- Behavior: add stale/null/mobile/source-item guards before reading `from.Backpack`, playing sound, or sending the existing bell messages.

## Fence Evidence

- `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs` has exact-file POST-BATCH-Y gate hits: `0`.
- `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs` has exact-file active overlay rows: `0`.
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization gate is crossed.

## Result

Proceed with `SOURCE-BATCH-347 BellOfCourage Guard Repair`, then continue with `ShardOfHatred` if exact-file preflight remains clean.
