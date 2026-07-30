# SOURCE-BATCH-340 RuneStoneGate Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-340`
- Candidate: `SB340-CAND-001`
- System: `Quests:Underworld / RuneStoneGate`
- Source file: `Data/Scripts/Quests/Underworld/RuneStoneGate.cs`
- Behavior: add stale/null/mobile/source-item guard to `RuneStoneGate.OnDoubleClick(Mobile from)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted, or the rune stone gate item is deleted.

## Must Stay Unchanged

- Item ID `0x21B9`, `Movable = false`, and `Name = "runic doorway"`.
- Message text: `This large stone door is covered in strange runes.`
- No travel, teleport, region, map, access, quest, or reward behavior changes.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-340 RuneStoneGate Guard Repair`
