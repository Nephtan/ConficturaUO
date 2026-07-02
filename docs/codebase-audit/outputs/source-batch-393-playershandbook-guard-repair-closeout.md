# SOURCE-BATCH-393 PlayersHandbook Guard Repair Closeout

## Result

`SOURCE-BATCH-393` implemented `SB393-CAND-001`, a non-gated guard repair for `PlayersHandbook`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/DandD/PlayersHandbook.cs`
- Added an early return in `PlayersHandbook.OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Added early returns in `BookTarget.OnTarget` for null/deleted mobiles and stale/deleted source books.
- Added deleted targeted-mobile handling using the existing invalid-target message.

## Preserved Behavior

PlayersHandbook item ID, name, weight, Dungeons & Dragons property label, target range, people classification rules, target eligibility messages, `StatsGump` construction, `DruidismGump` construction, sound `0x55` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/DandD/PlayersHandbook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/DandD/PlayersHandbook.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved PlayersHandbook lookup behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-393` source commit is pending. `SOURCE-BATCH-394+` should run fresh candidate discovery after `SOURCE-BATCH-393`.
