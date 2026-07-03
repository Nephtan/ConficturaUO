# SOURCE-BATCH-417 TarotCards Guard Repair Closeout

## Result

`SOURCE-BATCH-417` implemented `SB417-CAND-001`, a non-gated guard repair for the TarotCards rare-decoration interaction path.

## Source Change

- Files: the nine `Data/Scripts/Items/Special/Rares/TarotCards/*.cs` files.
- All nine TarotCards `OnDoubleClick(Mobile from)` entry points now return immediately when `from` is null, `from` is deleted, or the source tarot item is deleted.
- `TarotCardsGump.OnResponse(NetState state, RelayInfo info)` now returns safely for null `NetState`, null `RelayInfo`, null/deleted mobiles, or deleted source cards before stale gump refresh behavior can dereference invalid state.
- `TarotCardsGump.SendGump(Mobile from, Item cards)` now returns safely for null/deleted mobiles or null/deleted source cards before opening the gump or reading `cards.Name`.

## Preserved Behavior

- Tarot item IDs, names, movable/stackable flags, range check, localized reach message, `TarotCardsGump` layout, random tarot text generation, card image selection, refresh/cancel button behavior, gypsy sound, base sound `0x5BB`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB417-CAND-001` row.
- Targeted source scan: passed; nine `OnDoubleClick` mobile/source guards are present, nine existing range checks remain present, nine existing `TarotCardsGump.SendGump(from, this)` calls remain present, `OnResponse` state/mobile/source-card guards are present, static `SendGump` mobile/source-card guard is present, random tarot generation remains present, gypsy sound behavior remains present, and base sound `0x5BB` remains present.
- Exact-file POST-BATCH-Y scan: passed; the nine TarotCards files have `0` gate hits.
- Exact-file active overlay scan: passed; the nine TarotCards files have `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-417` source commit: pending. `SOURCE-BATCH-418+` should run fresh candidate discovery after `SOURCE-BATCH-417`.
