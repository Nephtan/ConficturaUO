# SOURCE-BATCH-045 StatuetteDyeTub Guard Repair Closeout

## Summary

SOURCE-BATCH-045 implemented the guard-only StatuetteDyeTub repair in `Data/Scripts/Items/Misc/Dyes/StatuetteDyeTub.cs`.

`OnDoubleClick` now returns before reward usability checks or base DyeTub behavior when the interacting mobile is null/deleted or the dye tub item is deleted.

## Scope Preserved

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

SOURCE-BATCH-045 is complete. The `source-batch-040-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-046+` remains pending candidate discovery for the next clean non-gated target.
