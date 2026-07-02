# SOURCE-BATCH-391 LiarsDice Guard Repair Closeout

## Result

`SOURCE-BATCH-391` implemented `SB391-CAND-001`, a non-gated guard repair for `LiarsDice`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs`
- Added an early return in `LiarsDice.OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

LiarsDice item ID, name, weight, hue, `GOLD_PER_GAME`, `GAME_BALANCE_MIN`, `GAME_BALANCE_MAX`, `GAME_PLAYER_TO_ACT_SECONDS`, `GAME_MAX_PLAYERS`, `DiceState` initialization, range check, `Banker.GetBalance` threshold, `from.Frozen = true`, `ds.ShowNewGameGump(from)`, insufficient-bank message, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved LiarsDice game-entry behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-391` commit hash pending. `SOURCE-BATCH-392+` should run fresh candidate discovery after `SOURCE-BATCH-391`.
