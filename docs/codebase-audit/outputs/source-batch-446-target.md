# SOURCE-BATCH-446 CustomHuePickerGump Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-446`
- Candidate: `SB446-CAND-001`
- System: `Items:Misc / Dyes / CustomHuePickerGump`
- Source file: `Data/Scripts/Items/Misc/Dyes/CustomHuePicker.cs`
- Behavior: add stale response, mobile, definition, callback, switch-list, hue-group, and hue-list guards to `CustomHuePickerGump.OnResponse(NetState sender, RelayInfo info)` before invoking the existing hue callback.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard CustomHuePicker interactions`.
