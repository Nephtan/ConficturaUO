# SOURCE-BATCH-444 DoorSwitch Guard Repair Closeout

## Summary

`SOURCE-BATCH-444` implemented `SB444-CAND-001`, a non-gated guard repair for DoorSwitch interaction paths.

## Source Change

File changed: `Data/Scripts/Items/Doors/DoorSwitch.cs`

`DoorSwitch.OnDoubleClick(Mobile m)` now returns safely when:

- `m == null`
- `m.Deleted`
- the source switch is `Deleted`

`DoorSwitch.switchit()` and `AddDoor.switchit()` now skip stale controlled-door list entries when:

- the list entry is not a `BaseDoor`
- the door is already `Deleted`

`AddDoor.OnTarget(Mobile from, object targ)` now returns safely when:

- `from == null`
- `from.Deleted`
- the controlled-door list is `null`

It also treats null/deleted target item state as the existing invalid-door path using the existing message `That is not a door`.

The guards run before the existing target prompt, add/remove logic, switch targeting, door open/close toggles, and add/remove messages.

## Preserved Behavior

- `DoorSwitch` item ID `0x108F`
- name `Door locker`
- hue `38`
- blessed loot type
- `Movable` behavior
- target prompt text
- target range `15`
- add/remove messages
- controlled-door list semantics
- open/close toggle semantics
- serialized door list
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Doors/DoorSwitch.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Doors/DoorSwitch.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `DoorSwitch.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-444` source commit: `b3e72fcd`. `SOURCE-BATCH-445+` should run fresh candidate discovery after `SOURCE-BATCH-444`.
