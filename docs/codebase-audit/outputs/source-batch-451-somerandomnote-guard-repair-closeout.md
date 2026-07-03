# SOURCE-BATCH-451 SomeRandomNote Guard Repair Closeout

## Summary

`SOURCE-BATCH-451` implemented `SB451-CAND-001`, a non-gated guard repair for reading existing random notes and closing their clue gump.

## Source Change

File changed: `Data/Scripts/Quests/SomeRandomNote.cs`

Guarded interactions:

- `SomeRandomNote.OnDoubleClick(Mobile e)`
- `ClueGump.OnResponse(NetState state, RelayInfo info)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source note
- missing backpack before `IsChildOf(e.Backpack)`
- null `NetState`
- null/deleted gump-response mobile

## Preserved Behavior

- random note names and item IDs
- generated `ScrollMessage` and `ScrollTrue` content
- `ClueGump` layout and text rendering
- sound `0x249`
- existing backpack-use failure message
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/SomeRandomNote.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Quests/SomeRandomNote.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved note-read behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-451` source commit: `72d36986`. `SOURCE-BATCH-452+` should run fresh candidate discovery after `SOURCE-BATCH-451`.
