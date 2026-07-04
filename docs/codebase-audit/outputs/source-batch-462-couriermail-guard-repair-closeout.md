# SOURCE-BATCH-462 CourierMail Guard Repair Closeout

## Summary

`SOURCE-BATCH-462` implemented `SB462-CAND-001`, a non-gated guard repair for opening the `CourierMail` gump.

## Source Change

File changed: `Data/Scripts/Quests/Epic/CourierMail.cs`

Guarded interaction:

- `CourierMail.OnDoubleClick(Mobile e)`

Added guard coverage:

- null mobile
- deleted mobile
- deleted source mail
- missing backpack before `IsChildOf(e.Backpack)`

## Preserved Behavior

- courier mail contents
- `SearchGump` construction
- sound `0x249`
- existing backpack-use failure message
- `Serial` constructor
- owner/dungeon `Serialize` and `Deserialize` layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Epic/CourierMail.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Quests/Epic/CourierMail.cs`: `0`

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved mail-open behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan
- forbidden-surface diff scan
- `git diff --check`
- `Data/System/Source/Server.csproj` Debug/x86 build
- `.\ConficturaServer.exe -compileonly -nocache`
- generated root artifacts restored with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`

## Result

`SOURCE-BATCH-462` source commit pending. `SOURCE-BATCH-463+` should run fresh candidate discovery after `SOURCE-BATCH-462`.
