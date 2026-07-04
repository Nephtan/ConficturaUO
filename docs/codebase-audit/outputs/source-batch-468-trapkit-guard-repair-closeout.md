# SOURCE-BATCH-468 TrapKit Guard Repair Closeout

## Summary

`SOURCE-BATCH-468` implemented `SB468-CAND-001`, a non-gated guard repair for placing traps with `TrapKit`.

## Source Change

File changed: `Data/Scripts/Items/Traps/TrapKit.cs`

Guarded interaction:

- `TrapKit.OnDoubleClick(Mobile from)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source trap kit
- missing backpack before `IsChildOf(from.Backpack)`

## Preserved Behavior

- `TrapKit` item identity
- metal and charge properties
- existing backpack-use failure message
- nearby trap limit
- `IPooledEnumerable.Free()` behavior
- `Region.AllowHarmful(from, from)` policy check
- `RemoveTrap` skill gate
- charge consumption and worn-out deletion
- metal-based power bonuses
- sound `0x55`
- `SetTrap` creation
- map/hue/location placement
- messages
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Traps/TrapKit.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Traps/TrapKit.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Traps/TrapKit.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved trap placement behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-468` source commit: `87368509`. `SOURCE-BATCH-469+` should run fresh candidate discovery after `SOURCE-BATCH-468`.
