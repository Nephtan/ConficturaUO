# SOURCE-BATCH-044 RunebookDyeTub Guard Repair Target

## Target

- Batch: SOURCE-BATCH-044
- Candidate: SB040-CAND-005
- Behavior: add stale/null/mobile/deleted-item guards to `RunebookDyeTub.OnDoubleClick` before `RewardSystem.CheckIsUsableBy` or `base.OnDoubleClick`.
- System: Items:Misc / Dyes / RunebookDyeTub
- File: `Data/Scripts/Items/Misc/Dyes/RunebookDyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- `AllowDyables = false`
- `AllowRunebooks = true`
- TargetMessage `1049774`
- FailMessage `1049775`
- LabelNumber `1049740`
- `CustomHuePicker.LeatherDyeTub`
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- namespace/type/file layout
- project/config/data files
