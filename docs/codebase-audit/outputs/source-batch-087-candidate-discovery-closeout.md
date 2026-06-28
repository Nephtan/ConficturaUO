# SOURCE-BATCH-087 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-087+` discovery ran after `SOURCE-BATCH-086` exhausted `source-batch-084-candidate-discovery.csv`.

The recommended target is `SB087-CAND-001` / `SOURCE-BATCH-087 HairDyePotion Guard Repair`.

## Candidate Evidence

- `Data/Scripts/Items/Potions/Special/HairDyePotion.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/HairDyeBottle.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/GenderPotion.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/NecroSkinPotion.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/HairOilPotion.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.

## Recommendation

Select `HairDyePotion` first because it is a one-file guard-only repair that protects `Drink(Mobile from)` before race, backpack, hue, effect, sound, and consume behavior. The related appearance potion/item candidates remain queued for following batches.

## Verification

- `source-batch-087-candidate-discovery.csv` imports successfully.
- Every recorded candidate has populated behavior, system, files, gate result, overlay result, risk, verification, source evidence, and unchanged-behavior fields.
- The recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
- No project/XML/config/data file was changed by discovery.
