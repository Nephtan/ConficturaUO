# SOURCE-BATCH-411 SackOfHolding Guard Repair Closeout

## Result

`SOURCE-BATCH-411` implemented `SB411-CAND-001`, a non-gated guard repair for `SackOfHolding` container interactions.

## Source Change

- File: `Data/Scripts/Items/Containers/SackOfHolding.cs`
- `BagGump.OnResponse(NetState state, RelayInfo info)` now returns when response state is stale, including null state, null mobile, or deleted mobile.
- `BagMenu.OnClick()` now returns when the stored mobile or source sack is null/deleted.
- `GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)` now returns for null/deleted mobiles or deleted source sacks before adding sack-specific menu entries.
- `SackOfHolding.OnDoubleClick(Mobile from)` now returns for null/deleted mobiles or deleted source sacks before owner/open/information-gump behavior.

## Preserved Behavior

- SackOfHolding randomized item setup, owner assignment, open behavior, BagGump text/layout/sound, context menu label, container rejection rules, weight reduction behavior, MaxItems behavior, SackOwner serialization, deserialize normalization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB411-CAND-001` row.
- Targeted source scan: passed; gump response, context menu, mobile, and source-sack guards are present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Containers/SackOfHolding.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Containers/SackOfHolding.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-411` source commit: `e0651a81`. `SOURCE-BATCH-412+` should run fresh candidate discovery after `SOURCE-BATCH-411`.
