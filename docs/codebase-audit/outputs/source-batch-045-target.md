# SOURCE-BATCH-045 StatuetteDyeTub Guard Repair Target

## Target

- Batch: SOURCE-BATCH-045
- Candidate: SB040-CAND-006
- Behavior: add stale/null/mobile/deleted-item guards to `StatuetteDyeTub.OnDoubleClick` before `RewardSystem.CheckIsUsableBy` or `base.OnDoubleClick`.
- System: Items:Misc / Dyes / StatuetteDyeTub
- File: `Data/Scripts/Items/Misc/Dyes/StatuetteDyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- `AllowDyables = false`
- `AllowStatuettes = true`
- TargetMessage `1049777`
- FailMessage `1049778`
- LabelNumber `1049741`
- `CustomHuePicker.LeatherDyeTub`
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- namespace/type/file layout
- project/config/data files
