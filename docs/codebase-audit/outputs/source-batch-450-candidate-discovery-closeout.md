# SOURCE-BATCH-450 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-450` ran fresh non-gated discovery after `SOURCE-BATCH-449` and selected `SB450-CAND-001`, the `MountedTrophyHead` backpack-flip guard repair.

## Recommended Target

- Candidate: `SB450-CAND-001`
- Batch: `SOURCE-BATCH-450`
- File: `Data/Scripts/Trades/Taxidermy/MountedTrophyHead.cs`
- Gate result: exact-file POST-BATCH-Y gate hits `0`
- Active overlay result: exact-file active overlay rows `0`

## Skipped Candidates

- `TrophyBase`, `StuffingBasket`, and `MountingBase` were skipped because they touch corpse trophy generation, corpse visited state, monster-to-item reward mappings, and region/corpse owner data.
- Bulk order, shoppes, guild trade, harvest, gardening, and apiculture candidates were skipped as economy, reward, crafting, vendor, harvest, or progression adjacent.
- XMLSpawner, UOArchitect, Invasion, and other custom gump/tooling candidates were skipped as staff/access or policy-sensitive.

## Result

The selected candidate is narrow enough for one source batch because it only guards an already-created trophy item's backpack-only art flip path.
