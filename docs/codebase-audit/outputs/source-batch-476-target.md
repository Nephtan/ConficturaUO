# SOURCE-BATCH-476 BaseStatueDeed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-476`
- Candidate: `SB476-CAND-001`
- Behavior: add stale/null mobile and deleted source statue deed guards to `BaseStatueDeed.OnDoubleClick(Mobile from)`.
- System: `Trades:Stone / BaseStatueDeed`
- File: `Data/Scripts/Trades/Stone/BaseStatueDeed.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- `BaseStatueDeed` item identity
- statue ID/color/material/maker/name fields
- valid deed hue sync from `Hue` into `StatueColor`
- `BaseAddonDeed.OnDoubleClick` placement behavior
- `BaseStatueAddon` component construction
- `Statues` helper behavior
- `GetProperties` and `AddNameProperties` display behavior
- serialization layout/versioning for addon and deed
- namespace/type/file layout
- project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state

## Ready Goal

`/goal SOURCE-BATCH-476 BaseStatueDeed Guard Repair`
