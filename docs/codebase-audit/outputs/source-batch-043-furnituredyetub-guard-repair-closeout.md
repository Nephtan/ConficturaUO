# SOURCE-BATCH-043 FurnitureDyeTub Guard Repair Closeout

## Summary

SOURCE-BATCH-043 implemented the guard-only FurnitureDyeTub repair in `Data/Scripts/Items/Misc/Dyes/FurnitureDyeTub.cs`.

`OnDoubleClick` now returns before reward usability checks or base DyeTub behavior when the interacting mobile is null/deleted or the dye tub item is deleted.

## Scope Preserved

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

SOURCE-BATCH-043 is complete. `SOURCE-BATCH-044+` remains pending RunebookDyeTub Guard Repair.
