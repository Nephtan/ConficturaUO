# SOURCE-BATCH-465 BagOfTricks Guard Repair Closeout

## Summary

`SOURCE-BATCH-465` implemented `SB465-CAND-001`, a non-gated guard repair for opening the `BagOfTricks` gump.

## Source Change

File changed: `Data/Scripts/Magic/Jester/BagOfTricks.cs`

Guarded interaction:

- `BagOfTricks.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source bag
- missing backpack before `IsChildOf(from.Backpack)`

## Preserved Behavior

- BagOfTricks item identity
- `PrankPoints` storage and accounting
- existing backpack-use failure message
- `BagOfTricksGump` construction
- sound `0x48`
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Magic/Jester/BagOfTricks.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Magic/Jester/BagOfTricks.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved gump-open behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-465` source commit pending. `SOURCE-BATCH-466+` should run fresh candidate discovery after `SOURCE-BATCH-465`.
