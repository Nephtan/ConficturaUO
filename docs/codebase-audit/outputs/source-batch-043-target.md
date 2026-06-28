# SOURCE-BATCH-043 FurnitureDyeTub Guard Repair Target

## Target

- Batch: SOURCE-BATCH-043
- Candidate: SB040-CAND-004
- Behavior: add stale/null/mobile/deleted-item guards to `FurnitureDyeTub.OnDoubleClick` before `RewardSystem.CheckIsUsableBy` or `base.OnDoubleClick`.
- System: Items:Misc / Dyes / FurnitureDyeTub
- File: `Data/Scripts/Items/Misc/Dyes/FurnitureDyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- `AllowDyables = false`
- `AllowFurniture = true`
- TargetMessage `501019`
- FailMessage `501021`
- LabelNumber `1041246`
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- LootType blessed fallback
- namespace/type/file layout
- project/config/data files
