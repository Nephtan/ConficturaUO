# SOURCE-BATCH-475 BaseAxe Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-475`
- Candidate: `SB475-CAND-001`
- Behavior: add stale/null mobile and deleted source axe guards to `BaseAxe.OnDoubleClick(Mobile from)`.
- System: `Items:Weapons / Axes / BaseAxe`
- File: `Data/Scripts/Items/Weapons/Axes/BaseAxe.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `BaseAxe` item identity
- weapon sounds, skill, type, and animation
- `HarvestSystem` property behavior
- uses remaining fields/properties
- durability scaling
- LOS/range/accessibility checks
- localized messages `1019045`, `1061637`, and `1010018`
- `HarvestLoopController.CancelForNewTarget` dispatch
- `HarvestSystem.BeginHarvesting` dispatch
- context-menu harvest entries
- combat `OnHit` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-475 BaseAxe Guard Repair`
