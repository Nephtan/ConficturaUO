# SOURCE-BATCH-344 ScalesOfEthicality Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-344`
- Candidate: `SB344-CAND-001`
- System: `Quests:Serpents / ScalesOfEthicality`
- Source file: `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs`
- Behavior: add stale/null/mobile/source-item guards to `ScalesOfEthicality.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-use failure message when the scales item is deleted, the mobile has no backpack, or the scales are not in the backpack.

## Must Stay Unchanged

- Item ID `0x573A`, `Name = "Scales of Ethicality"`, `Weight = 1.0`, and `Hue = 0x4AB`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You scale seems to weigh the ethics of the situation.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-344 ScalesOfEthicality Guard Repair`
