# SOURCE-BATCH-394 MonsterManual Guard Repair Closeout

## Result

`SOURCE-BATCH-394` implemented `SB394-CAND-001`, a non-gated guard repair for `MonsterManual`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/DandD/MonsterManual.cs`
- Added an early return in `MonsterManual.OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Added early returns in `BookTarget.OnTarget` for null/deleted mobiles and stale/deleted source books.
- Added deleted targeted-mobile handling using the existing invalid-target message.

## Preserved Behavior

MonsterManual item ID, name, weight, Dungeons & Dragons property label, target range, target eligibility messages, `PlayersHandbook.IsPeople` delegation, `DruidismGump` construction, sound `0x55` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/DandD/MonsterManual.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/MonsterManual.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved MonsterManual lookup behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-394` committed as `aacd93c9`. `SOURCE-BATCH-395+` should run fresh candidate discovery after `SOURCE-BATCH-394`.
