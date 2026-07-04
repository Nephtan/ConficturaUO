# SOURCE-BATCH-471 Dyes Guard Repair Closeout

## Summary

`SOURCE-BATCH-471` implemented `SB471-CAND-001`, a non-gated guard repair for dye tub interaction and hue callback paths.

## Source Change

File changed: `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`

Guarded interactions:

- `Dyes.OnDoubleClick(Mobile from)`
- `InternalTarget.OnTarget(Mobile from, object targeted)`
- `InternalPicker.OnResponse(int hue)`
- `SetTubHue(Mobile from, object state, int hue)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source dyes
- deleted target dye tub
- null or deleted callback dye tub

## Preserved Behavior

- `Dyes` item identity
- target range
- localized prompt `500856`
- invalid-target localized message `500857`
- `DyeTub.Redyable` behavior
- `BlackDyeTub` rejection localized message `1010092`
- may-not-redye message
- `HuePicker` behavior
- `CustomHuePickerGump` behavior
- `DyedHue` assignment for valid dye tubs
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import
- targeted source scan for new guards and preserved dye tub behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-471` source commit: pending. `SOURCE-BATCH-472+` should run fresh candidate discovery after `SOURCE-BATCH-471`.
