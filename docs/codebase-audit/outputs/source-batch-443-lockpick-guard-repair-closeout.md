# SOURCE-BATCH-443 Lockpick Guard Repair Closeout

## Summary

`SOURCE-BATCH-443` implemented `SB443-CAND-001`, a non-gated guard repair for Lockpick interaction paths.

## Source Change

File changed: `Data/Scripts/Items/Trades/Thieving/LockPick.cs`

`Lockpick.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source lockpick is `Deleted`

`InternalTarget.OnTarget(Mobile from, object targeted)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source lockpick is `null`
- the source lockpick is `Deleted`
- the targeted item is already `Deleted`

`InternalTimer.OnTick()` now returns safely when:

- the delayed mobile is `null`
- the delayed mobile is `Deleted`
- the delayed `ILockpickable` state is `null`
- the delayed source lockpick is `null`
- the delayed source lockpick is `Deleted`
- the delayed lockpickable target is not an `Item`
- the delayed target item is `Deleted`

The guards run before the existing prompt, target assignment, lock type checks, lockpicking skill checks, delayed timer work, sounds, messages, lockpick consumption, and lock success/failure behavior.

## Preserved Behavior

- lockpick item IDs
- stack/weight behavior
- target prompt localized message `502068`
- target range
- spaceship/key-card rules
- dungeon-door rules
- lockpicking skill thresholds
- sounds `0x54B`, `0x241`, `0x549`, `0x3A4`, and `0x4A`
- success/failure messages
- lockpick `Consume()` behavior
- `TreasureMapChest` failure side effects
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Thieving/LockPick.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Trades/Thieving/LockPick.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `LockPick.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-443` source commit: `7136d0ce`. `SOURCE-BATCH-444+` should run fresh candidate discovery after `SOURCE-BATCH-443`.
