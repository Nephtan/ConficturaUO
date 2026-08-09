# SOURCE-BATCH-473 BaseMagicObject Guard Repair Closeout

## Summary

`SOURCE-BATCH-473` implemented `SB473-CAND-001`, a non-gated guard repair for base magic object use and delayed lock-release paths.

## Source Change

File changed: `Data/Scripts/Magic/Misc/BaseMagicObject.cs`

Guarded interactions:

- `BaseMagicObject.OnDoubleClick(Mobile from)`
- `BaseMagicObject.ReleaseMagicObjectLock_Callback(object state)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source magic object
- null or deleted timer callback mobile

## Preserved Behavior

- `BaseMagicObject` item identity
- weapon abilities and stat overrides
- `MagicObjectEffect` and `Charges` properties
- charge decrement and out-of-charges message `1019073`
- `SpellChanneling` reset behavior
- `BeginAction`/`EndAction` behavior for valid mobiles
- `GetUseDelay` duration
- `Timer.DelayCall` scheduling
- equipped-item eligibility
- localized message `502641`
- `OnMagicObjectUse` target assignment
- `DoMagicObjectTarget` behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Magic/Misc/BaseMagicObject.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Magic/Misc/BaseMagicObject.cs`: `0`
- Intake register completed rows for `Data/Scripts/Magic/Misc/BaseMagicObject.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import
- targeted source scan for new guards and preserved magic object behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-473` source commit: `1dcf4fa5`. `SOURCE-BATCH-474+` should run fresh candidate discovery after `SOURCE-BATCH-473`.
