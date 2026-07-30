# SOURCE-BATCH-474 BaseMagicStaff Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-474`
- Candidate: `SB474-CAND-001`
- Behavior: add stale/null mobile, deleted source magic staff, and stale timer-callback guards to `BaseMagicStaff` interaction paths.
- System: `Items:Wands / BaseMagicStaff`
- File: `Data/Scripts/Items/Wands/BaseMagicStaff.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `BaseMagicStaff` item identity
- weapon stat overrides
- `MagicStaffEffect` and `Charges` properties
- charge decrement and out-of-charges message `1019073`
- `SpellChanneling` reset behavior for valid use/equip/deserialize paths
- `BeginAction`/`EndAction` behavior for valid mobiles
- `GetUseDelay` duration
- `Timer.DelayCall` scheduling
- equipped-item eligibility
- localized message `502641`
- `OnMagicStaffUse` target assignment
- `DoMagicStaffTarget` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-474 BaseMagicStaff Guard Repair`
