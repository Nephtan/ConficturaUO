# SOURCE-BATCH-342 LanternOfDiscipline Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-342`
- Candidate: `SB342-CAND-001`
- System: `Quests:Serpents / LanternOfDiscipline`
- Source file: `Data/Scripts/Quests/Serpents/LanternOfDiscipline.cs`
- Behavior: add stale/null/mobile/source-item guards to `LanternOfDiscipline.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-use failure message when the lantern item is deleted, the mobile has no backpack, or the lantern is not in the backpack.

## Must Stay Unchanged

- Item ID `0x4101`, `Name = "Lantern of Discipline"`, `Weight = 1.0`, and `Light = LightType.Circle150`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `The lantern glows with a disciplined light.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-342 LanternOfDiscipline Guard Repair`
