# SOURCE-BATCH-469 BasePoleArm Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-469`
- Candidate: `SB469-CAND-001`
- Behavior: add stale/null mobile and deleted source weapon guards to `BasePoleArm.OnDoubleClick(Mobile from)`.
- System: `Items:Weapons / PoleArms / BasePoleArm`
- File: `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- polearm combat stats
- `HarvestSystem` property behavior
- `HarvestLoopController.CancelForNewTarget(from)` dispatch
- `HarvestSystem.BeginHarvesting(from, this)` dispatch
- backpack/equipped eligibility
- localized backpack-use failure `1042001`
- uses remaining fields/properties
- context-menu harvest entries
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-469 BasePoleArm Guard Repair`
