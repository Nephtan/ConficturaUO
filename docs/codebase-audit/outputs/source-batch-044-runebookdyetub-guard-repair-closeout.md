# SOURCE-BATCH-044 RunebookDyeTub Guard Repair Closeout

## Summary

SOURCE-BATCH-044 implemented the guard-only RunebookDyeTub repair in `Data/Scripts/Items/Misc/Dyes/RunebookDyeTub.cs`.

`OnDoubleClick` now returns before reward usability checks or base DyeTub behavior when the interacting mobile is null/deleted or the dye tub item is deleted.

## Scope Preserved

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

## Verification

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Targeted source scan confirmed the new guard and preserved behavior markers.
- Serializer diff scan found no serialization changes.
- Forbidden-surface diff scan found no forbidden-surface changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts were restored before staging.

## Result

SOURCE-BATCH-044 is complete. `SOURCE-BATCH-045+` remains pending StatuetteDyeTub Guard Repair.
