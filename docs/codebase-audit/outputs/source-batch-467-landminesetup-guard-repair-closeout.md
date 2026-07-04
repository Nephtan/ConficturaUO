# SOURCE-BATCH-467 LandmineSetup Guard Repair Closeout

## Summary

`SOURCE-BATCH-467` implemented `SB467-CAND-001`, a non-gated guard repair for setting up landmines.

## Source Change

File changed: `Data/Scripts/Items/Technology/Landmine.cs`

Guarded interaction:

- `LandmineSetup.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source setup item
- missing backpack before `IsChildOf(from.Backpack)`

## Preserved Behavior

- `LandmineSetup` item identity
- existing backpack-use failure message
- nearby landmine limit
- `IPooledEnumerable.Free()` behavior
- `Region.AllowHarmful(from, from)` policy check
- `RemoveTrap` power math
- sound `0x42`
- `Landmine` owner/power construction
- map/location placement
- setup item `Delete()` semantics
- messages
- `Serial` constructors
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Technology/Landmine.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Technology/Landmine.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved landmine setup behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-467` source commit: `46d02cfb`. `SOURCE-BATCH-468+` should run fresh candidate discovery after `SOURCE-BATCH-467`.
