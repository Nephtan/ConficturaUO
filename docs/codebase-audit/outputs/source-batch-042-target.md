# SOURCE-BATCH-042 LeatherDyeTub Guard Repair Target

## Target

- Batch: SOURCE-BATCH-042
- Candidate: SB040-CAND-003
- Behavior: add stale/null/mobile/deleted-item guards to `LeatherDyeTub.OnDoubleClick` before `RewardSystem.CheckIsUsableBy` or `base.OnDoubleClick`.
- System: Items:Misc / Dyes / LeatherDyeTub
- File: `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- `AllowDyables = false`
- `AllowLeather = true`
- TargetMessage `1042416`
- FailMessage `1042418`
- LabelNumber `1041284`
- `CustomHuePicker.LeatherDyeTub`
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- namespace/type/file layout
- project/config/data files

## Ready Goal Command

```text
/goal SOURCE-BATCH-042 LeatherDyeTub Guard Repair

Implement the SB040-CAND-003 guard-only repair in Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs.

Add stale/null/mobile/deleted-item guards to LeatherDyeTub.OnDoubleClick before RewardSystem.CheckIsUsableBy or base.OnDoubleClick.

Preserve reward usability checks, leather dye targeting, hue picker, messages, serialization, namespace/type/file layout, and project/config/data files.
```
