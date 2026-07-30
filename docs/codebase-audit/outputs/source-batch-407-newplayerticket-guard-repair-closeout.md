# SOURCE-BATCH-407 NewPlayerTicket Guard Repair Closeout

## Result

`SOURCE-BATCH-407` implemented `SB407-CAND-001`, a non-gated guard repair for `NewPlayerTicket` pairing interactions.

## Source Change

- File: `Data/Scripts/Items/Deeds/NewPlayerTicket.cs`
- `NewPlayerTicket.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source ticket is deleted.
- Missing backpacks now use the existing backpack-use failure message before target assignment.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately when `from` is null/deleted or the source ticket is null/deleted, and deleted target tickets use the existing invalid-ticket message.
- `InternalGump.OnResponse(NetState sender, RelayInfo info)` now returns when response state is stale, including null sender, null/deleted response mobile, or null/deleted source ticket.

## Preserved Behavior

- NewPlayerTicket label/properties, blessed loot type, owner persistence, owner-only use rule, backpack-use message, target range, pairing messages, gump layout, reward choices, reward messages, `AddToBackpack` behavior, ticket `Delete` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB407-CAND-001` row.
- Targeted source scan: passed; mobile, source-ticket, backpack, deleted-target-ticket, and gump-response guards are present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Deeds/NewPlayerTicket.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Deeds/NewPlayerTicket.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild; initial sandbox run was denied access to the local SDK cache, rerun with approved filesystem access succeeded with `0` warnings and `0` errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-407` source commit: `880a4142`. `SOURCE-BATCH-408+` should run fresh candidate discovery after `SOURCE-BATCH-407`.
