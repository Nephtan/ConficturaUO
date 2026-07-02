# SOURCE-BATCH-396 Tarot Poker Guard Repair Closeout

## Result

`SOURCE-BATCH-396` implemented `SB396-CAND-001`, a non-gated guard repair for `tarotpoker`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/tarotpoker.cs`
- Added an early return in `tarotpoker.OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

tarotpoker item IDs, `Flipable` behavior, name, weight, `Stackable = false`, `IsNoisy` command property behavior, range 4 rule, card randomization, `TarotGump` image IDs, public overhead message text including betting instructions, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/tarotpoker.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/tarotpoker.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved Tarot Poker behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-396` source commit is pending. `SOURCE-BATCH-397+` should run fresh candidate discovery after `SOURCE-BATCH-396`.
