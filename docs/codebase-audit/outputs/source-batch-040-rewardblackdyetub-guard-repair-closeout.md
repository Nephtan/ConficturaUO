# SOURCE-BATCH-040 RewardBlackDyeTub Guard Repair Closeout

## Summary

SOURCE-BATCH-040 implemented the guard-only RewardBlackDyeTub repair in `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`.

`OnDoubleClick` now returns before reward usability checks or base DyeTub behavior when the interacting mobile is null/deleted or the dye tub item is deleted.

## Scope Preserved

- LabelNumber `1006008`
- Hue and DyedHue `0x0001`
- `Redyable = false`
- blessed loot
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`: 0
- Gated approval crossed: no

## Verification

- Targeted source scan confirmed the new `OnDoubleClick` guard.
- Behavior preservation scan confirmed LabelNumber `1006008`, Hue/DyedHue `0x0001`, `Redyable = false`, blessed loot, `RewardSystem.CheckIsUsableBy`, `base.OnDoubleClick(from)`, `IsRewardItem` write, and `IsRewardItem` read remain present.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-040 is complete. `SOURCE-BATCH-041+` remains pending the next clean target from `source-batch-040-candidate-discovery.csv`: SpecialDyeTub Guard Repair.
