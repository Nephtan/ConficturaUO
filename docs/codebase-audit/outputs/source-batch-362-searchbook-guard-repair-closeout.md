# SOURCE-BATCH-362 SearchBook Guard Repair Closeout

## Result

`SOURCE-BATCH-362` implemented `SB362-CAND-001`, a non-gated guard repair for `SearchBook`.

## Source Change

- File: `Data/Scripts/Quests/Search/SearchBook.cs`
- Added an early return when `from == null || from.Deleted || Deleted`.
- Added missing-backpack handling through the existing backpack-use failure message.

## Preserved Behavior

Owner assignment, `LegendLore`, `AddNameProperties` text, backpack-use message, owner rejection message, sound `0x55`, `CloseGump`/`SendGump` behavior, `SearchBookGump` content and pagination, artifact encyclopedia text, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Search/SearchBook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Search/SearchBook.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed with one recommended row and no missing required fields.
- Targeted source scan: passed for stale/null mobile/source-book guard, missing-backpack guard, existing backpack-use message, owner rejection message, sound `0x55`, gump close/send behavior, and serialization read/write method presence.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed with no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed with no command, event hook, packet handler, timer, gump policy, region, serializer, namespace, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0` warnings and `0` errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: completed with `git restore -- ConficturaServer.exe ConficturaServer.exe.config ConficturaServer.pdb`.

## Commit

`SOURCE-BATCH-362` committed as `afd987f0` (`fix: guard SearchBook interactions`). `SOURCE-BATCH-363+` should run fresh candidate discovery.
