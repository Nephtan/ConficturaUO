# SOURCE-BATCH-475 BaseAxe Guard Repair Closeout

## Summary

`SOURCE-BATCH-475` implemented `SB475-CAND-001`, a non-gated guard repair for base axe harvest double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Weapons/Axes/BaseAxe.cs`

Guarded interaction:

- `BaseAxe.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source axe

## Preserved Behavior

- `BaseAxe` item identity
- weapon sounds, skill, type, and animation
- `HarvestSystem` property behavior
- uses remaining fields/properties
- durability scaling
- LOS/range/accessibility checks
- localized messages `1019045`, `1061637`, and `1010018`
- `HarvestLoopController.CancelForNewTarget` dispatch
- `HarvestSystem.BeginHarvesting` dispatch
- context-menu harvest entries
- combat `OnHit` behavior
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Weapons/Axes/BaseAxe.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Weapons/Axes/BaseAxe.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Weapons/Axes/BaseAxe.cs`: `0`

## Verification

Passed before source commit:

- candidate CSV import
- targeted source scan for new guard and preserved axe/harvest behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-475` source commit: pending. `SOURCE-BATCH-476+` should run fresh candidate discovery after `SOURCE-BATCH-475`.
