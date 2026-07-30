# SOURCE-BATCH-042 LeatherDyeTub Guard Repair Closeout

## Summary

SOURCE-BATCH-042 implemented the guard-only LeatherDyeTub repair in `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`.

`OnDoubleClick` now returns before reward usability checks or base DyeTub behavior when the interacting mobile is null/deleted or the dye tub item is deleted.

## Scope Preserved

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

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/LeatherDyeTub.cs`: 0
- Gated approval crossed: no

## Verification

- Targeted source scan confirmed the new `OnDoubleClick` guard and preserved behavior markers.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-042 is complete. `SOURCE-BATCH-043+` remains pending the next clean target from `source-batch-040-candidate-discovery.csv`: FurnitureDyeTub Guard Repair.
