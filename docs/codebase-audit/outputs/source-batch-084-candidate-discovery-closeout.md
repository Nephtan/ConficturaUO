# SOURCE-BATCH-084 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-084+` discovery ran after `SOURCE-BATCH-083` exhausted `source-batch-082-candidate-discovery.csv`.

The recommended target is `SB084-CAND-001` / `SOURCE-BATCH-084 PotionOfWisdom Guard Repair`.

## Candidate Evidence

- `Data/Scripts/Items/Potions/Special/PotionOfWisdom.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/PotionOfMight.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/PotionOfDexterity.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.

## Recommendation

Select `PotionOfWisdom` first because it is a one-file guard-only repair that covers a stale/null backpack dereference before stat-cap and consume behavior. The related stat potions remain queued for the following batches.

## Exclusions

This discovery pass left housing/vendor, boats, broad base classes, food/hunger/race-consumption, gifts, command, and region/map-adjacent candidates unselected because they are more policy-sensitive than the stat potion guard-only cluster.

## Verification

- `source-batch-084-candidate-discovery.csv` imports successfully.
- Every recorded candidate has populated behavior, system, files, gate result, overlay result, risk, verification, source evidence, and unchanged-behavior fields.
- The recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
- No project/XML/config/data file was changed by discovery.
