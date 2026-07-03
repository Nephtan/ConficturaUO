# SOURCE-BATCH-457 LearnReagentsBook Guard Repair Closeout

## Summary

`SOURCE-BATCH-457` implemented `SB457-CAND-001`, a non-gated guard repair for opening the `LearnReagentsBook` informational gump.

## Source Change

File changed: `Data/Scripts/Items/Books/LearnReagents.cs`

Guarded interaction:

- `LearnReagentsBook.OnDoubleClick(Mobile e)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source book
- missing backpack before `IsChildOf(e.Backpack)`

## Preserved Behavior

- book item ID, name, weight, and property text
- reagents guide text
- `LearnReagentsGump` layout
- sound `0x249`
- `Server.Gumps.MyLibrary.readBook(this, e)` behavior
- existing backpack-use failure message
- `LearnReagentsGump.OnResponse` stale response guards
- `Serial` constructor
- `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Books/LearnReagents.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Books/LearnReagents.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved book-read behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-457` source commit pending. `SOURCE-BATCH-458+` should run fresh candidate discovery after `SOURCE-BATCH-457`.
