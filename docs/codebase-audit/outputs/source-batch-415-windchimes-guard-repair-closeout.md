# SOURCE-BATCH-415 WindChimes Guard Repair Closeout

## Result

`SOURCE-BATCH-415` implemented `SB415-CAND-001`, a non-gated guard repair for `WindChimes` owner-toggle interactions.

## Source Change

- File: `Data/Scripts/Items/Misc/WindChimes.cs`
- `BaseWindChimes.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source wind chimes item is deleted.
- `OnOffGump.OnResponse(NetState sender, RelayInfo info)` now returns safely for stale response state, null/deleted mobiles, or deleted source wind chimes before toggling `TurnedOn`.

## Preserved Behavior

- Owner-only use rule, `OnOffGump` layout, OK/cancel button behavior, locked-down reminder, cancel message, `TurnedOn` toggle, movement sound behavior, sound table, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB415-CAND-001` row.
- Targeted source scan: passed; mobile/source-chime/gump-response guards are present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Misc/WindChimes.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Misc/WindChimes.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-415` source commit: pending. `SOURCE-BATCH-416+` should run fresh candidate discovery after `SOURCE-BATCH-415`.
