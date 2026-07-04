# SOURCE-BATCH-469 BasePoleArm Guard Repair Closeout

## Summary

`SOURCE-BATCH-469` implemented `SB469-CAND-001`, a non-gated guard repair for polearm harvest dispatch.

## Source Change

File changed: `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`

Guarded interaction:

- `BasePoleArm.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source weapon

## Preserved Behavior

- polearm combat stats
- `HarvestSystem` property behavior
- `HarvestLoopController.CancelForNewTarget(from)` dispatch
- `HarvestSystem.BeginHarvesting(from, this)` dispatch
- backpack/equipped eligibility
- localized backpack-use failure `1042001`
- uses remaining fields/properties
- context-menu harvest entries
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Weapons/PoleArms/BasePoleArm.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved harvest dispatch behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-469` source commit: `7f638b6b`. `SOURCE-BATCH-470+` should run fresh candidate discovery after `SOURCE-BATCH-469`.
