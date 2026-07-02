# SOURCE-BATCH-395 Tarot Guard Repair Closeout

## Result

`SOURCE-BATCH-395` implemented `SB395-CAND-001`, a non-gated guard repair for `Tarot.cs`.

## Source Change

- File: `Data/Scripts/Items/Misc/Games/Tarot.cs`
- Added early returns in `TarotDeck.OnDoubleClick` and `DecoTarotDeck.OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

TarotDeck and DecoTarotDeck item IDs, `Flipable` behavior, fortune randomization, fortune text/image mappings, `PublicOverheadMessage` content, `TarotGump` construction, open/closed item-ID toggles, `OnAdded` normalization, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Games/Tarot.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Games/Tarot.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved Tarot behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-395` source commit is pending. `SOURCE-BATCH-396+` should run fresh candidate discovery after `SOURCE-BATCH-395`.
