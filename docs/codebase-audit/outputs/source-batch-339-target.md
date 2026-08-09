# SOURCE-BATCH-339 BottleOil Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-339`
- Candidate: `SB339-CAND-001`
- System: `Quests:Golems / BottleOil`
- Source file: `Data/Scripts/Quests/Golems/BottleOil.cs`
- Behavior: add stale/null/mobile/source-item guard to `BottleOil.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted, or the bottle oil item is deleted.

## Must Stay Unchanged

- Item ID `0xF0E`, `Weight = 0.01`, `Stackable = true`, `Amount`, `Hue = 0x497`, and `Name = "technomancer oil"`.
- Message text: `I don't think you want to drink that!`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-339 BottleOil Guard Repair`
