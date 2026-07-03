# SOURCE-BATCH-444 DoorSwitch Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-444`
- Candidate: `SB444-CAND-001`
- System: `Items:Doors / DoorSwitch`
- Source file: `Data/Scripts/Items/Doors/DoorSwitch.cs`
- Behavior: add stale/null/mobile/source-switch/deleted-door/list guards to `DoorSwitch.OnDoubleClick(Mobile m)`, `DoorSwitch.switchit()`, `AddDoor.switchit()`, and `AddDoor.OnTarget(Mobile from, object targ)` before dereferencing mobile, controlled-door list entries, or target item state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard DoorSwitch interactions`.
