# SOURCE-BATCH-474 BaseMagicStaff Guard Repair Closeout

## Summary

`SOURCE-BATCH-474` implemented `SB474-CAND-001`, a non-gated guard repair for base magic staff use and delayed lock-release paths.

## Source Change

File changed: `Data/Scripts/Items/Wands/BaseMagicStaff.cs`

Guarded interactions:

- `BaseMagicStaff.OnDoubleClick(Mobile from)`
- `BaseMagicStaff.ReleaseMagicStaffLock_Callback(object state)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source magic staff
- null or deleted timer callback mobile

## Preserved Behavior

- `BaseMagicStaff` item identity
- weapon stat overrides
- `MagicStaffEffect` and `Charges` properties
- charge decrement and out-of-charges message `1019073`
- `SpellChanneling` reset behavior for valid use/equip/deserialize paths
- `BeginAction`/`EndAction` behavior for valid mobiles
- `GetUseDelay` duration
- `Timer.DelayCall` scheduling
- equipped-item eligibility
- localized message `502641`
- `OnMagicStaffUse` target assignment
- `DoMagicStaffTarget` behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Wands/BaseMagicStaff.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Wands/BaseMagicStaff.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Wands/BaseMagicStaff.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import
- targeted source scan for new guards and preserved magic staff behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-474` source commit: pending. `SOURCE-BATCH-475+` should run fresh candidate discovery after `SOURCE-BATCH-474`.
