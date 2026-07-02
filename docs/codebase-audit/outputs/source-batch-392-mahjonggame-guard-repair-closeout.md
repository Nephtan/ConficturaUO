# SOURCE-BATCH-392 MahjongGame Guard Repair Closeout

## Result

`SOURCE-BATCH-392` implemented `SB392-CAND-001`, a non-gated guard repair for `MahjongGame`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/Mahjong/MahjongGame.cs`
- Added an early return in `MahjongGame.OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

MahjongGame item ID, name, weight, `MahjongPlayers` initialization, context menu behavior, `m_Players.CheckPlayers()`, `m_Players.Join(from)`, reset/recreate-game behavior, pending-player behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/Mahjong/MahjongGame.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/Mahjong/MahjongGame.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved Mahjong game-entry behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-392` committed as `4059e74b`. `SOURCE-BATCH-393+` should run fresh candidate discovery after `SOURCE-BATCH-392`.
