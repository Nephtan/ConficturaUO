# SOURCE-BATCH-446 CustomHuePickerGump Guard Repair Closeout

## Summary

`SOURCE-BATCH-446` implemented `SB446-CAND-001`, a non-gated guard repair for custom dye hue-picker gump responses.

## Source Change

File changed: `Data/Scripts/Items/Misc/Dyes/CustomHuePicker.cs`

`CustomHuePickerGump.OnResponse(NetState sender, RelayInfo info)` now returns safely when:

- `sender == null`
- `info == null`
- the stored mobile is `null`
- the stored mobile is deleted
- the hue-picker definition is `null`
- the callback is `null`
- the hue groups array is `null`
- the hue groups array is empty
- the switch list is `null`
- the selected hue group is `null`
- the selected hue list is `null`

The guard runs before the existing OK/default callback paths dereference response, group, hue, or callback state.

## Preserved Behavior

- `CustomHuePicker.SpecialDyeTub` and `CustomHuePicker.LeatherDyeTub` definitions
- `CustomHueGroup` hue/name data
- gump layout
- button IDs `1` and `2`
- OK/default callback behavior for valid responses
- selected-hue index math
- caller callback/state semantics
- dye tub behavior
- serialization absence
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Dyes/CustomHuePicker.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Misc/Dyes/CustomHuePicker.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved callback behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `CustomHuePicker.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-446` source commit: `5198e2bb`. `SOURCE-BATCH-447+` should run fresh candidate discovery after `SOURCE-BATCH-446`.
