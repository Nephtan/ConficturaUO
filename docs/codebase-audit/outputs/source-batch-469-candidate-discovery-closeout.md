# SOURCE-BATCH-469 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-469` ran fresh non-gated candidate discovery after `SOURCE-BATCH-468` and selected one clean guard repair candidate.

## Recommended Target

- Candidate: `SB469-CAND-001`
- Batch: `SOURCE-BATCH-469`
- System: `Items:Weapons / PoleArms / BasePoleArm`
- File: `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`
- Behavior: add stale/null mobile and deleted source weapon guards to `BasePoleArm.OnDoubleClick(Mobile from)`.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Candidate Notes

`BasePoleArm` was selected because its double-click harvest dispatch path could run before validating the mobile or source weapon state. The existing harvest system, pack/equipped eligibility, combat properties, and uses-remaining persistence remain unchanged.

Skipped discovery hits:

- `Data/Scripts/Items/Misc/Waypoint.cs`: command/staff workflow and waypoint sequencing behavior.
- `Data/Scripts/Quests/Underworld/SkullOfBaron Almric.cs`: map/underworld gate and travel behavior.
- `Data/Scripts/Quests/Thief/HollowStump.cs` and `Data/Scripts/Quests/Thief/HayCrate.cs`: thief quest reward/progression behavior.
- `Data/Scripts/Trades/Shoppes/MerchantCrate.cs`: merchant/economy cash-out behavior.
- Taxidermy support files: large target/corpse trophy flows better handled as a separate explicit candidate.

## Result

Proceed with `SOURCE-BATCH-469 BasePoleArm Guard Repair`.
