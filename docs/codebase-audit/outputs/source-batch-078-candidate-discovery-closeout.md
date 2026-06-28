# SOURCE-BATCH-078 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-078+` discovery found three zero-gate, zero-overlay guard candidates after `SOURCE-BATCH-077` exhausted its MysticalPearl queue.

The recommended target is `SB078-CAND-001` / `SOURCE-BATCH-078 CrystallineJar Guard Repair`.

## Candidate Evidence

- `Data/Scripts/Items/Potions/Bottles/CrystallineJar.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Potions/Special/BottleOfAcid.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Trades/Misc/RepairDeed.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.

## Recommendation

Select `CrystallineJar` first because it contains a direct null-target ordering defect in `ThrowTarget.OnTarget` and unguarded source-jar/mobile state across scoop and throw paths. The repair is local, guard-only, and does not require policy, serializer, project, config, data, or reorganization approval.

`BottleOfAcid` and `RepairDeed` remain viable later candidates but are lower priority because their behavior is adjacent to lock/trap/security and craft/region workflows.

## Verification

- `source-batch-078-candidate-discovery.csv` imports successfully.
- All three candidates have populated gate, overlay, risk, verification, source evidence, and unchanged-behavior fields.
- The recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
- No source/project/XML/config/data file was changed by discovery alone.
