# SOURCE-BATCH-413 ColoringBook Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-413`
- Candidate: `SB413-CAND-001`
- System: `Items:Magical Artifacts / ColoringBook`
- File: `Data/Scripts/Items/Magical/Artifacts/Minor/ColoringBook.cs`
- Behavior: add stale/null mobile, deleted source book, backpack, target item, and gump response guard coverage around `ColoringBook.OnDoubleClick`, `ColorTarget.OnTarget`, and `ColoringBookGump.OnResponse`.

## Fence

- POST-BATCH-Y exact-file gate hits: `0`.
- Active overlay rows: `0`.
- No staff/access, command policy, balance/economy tuning, region/map policy, serializer migration, project/config/data, XML/config/data, or reorganization approval is required for this guard-only target.

## Must Stay Unchanged

- Prismatic color list.
- Page navigation.
- `MagicColor` and `MagicPage` state.
- Item hue application.
- Target range.
- Backpack-use message.
- Invalid-target messages.
- Sound `0x55`.
- Sound `0x1FA`.
- `RevealingAction`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project/config/data files.
- Staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.
