# SOURCE-BATCH-473 BaseMagicObject Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-473`
- Candidate: `SB473-CAND-001`
- Behavior: add stale/null mobile, deleted source magic object, and stale timer-callback guards to `BaseMagicObject` interaction paths.
- System: `Magic:Misc / BaseMagicObject`
- File: `Data/Scripts/Magic/Misc/BaseMagicObject.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `BaseMagicObject` item identity
- weapon abilities and stat overrides
- `MagicObjectEffect` and `Charges` properties
- charge decrement and out-of-charges message `1019073`
- `SpellChanneling` reset behavior
- `BeginAction`/`EndAction` behavior for valid mobiles
- `GetUseDelay` duration
- `Timer.DelayCall` scheduling
- equipped-item eligibility
- localized message `502641`
- `OnMagicObjectUse` target assignment
- `DoMagicObjectTarget` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-473 BaseMagicObject Guard Repair`
