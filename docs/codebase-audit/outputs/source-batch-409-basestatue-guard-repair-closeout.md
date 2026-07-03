# SOURCE-BATCH-409 BaseStatue Guard Repair Closeout

## Result

`SOURCE-BATCH-409` implemented `SB409-CAND-001`, a non-gated guard repair for `BaseStatue` rename interactions.

## Source Change

- File: `Data/Scripts/Trades/Stone/BaseStatue.cs`
- `BaseStatue.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source statue is deleted.
- `RenamePrompt.OnResponse(Mobile from, string text)` now returns when response state is stale, including null/deleted mobile, null/deleted source statue, or null text.

## Preserved Behavior

- BaseStatue crafter/resource properties, material display, color mapping, rename prompt text, prompt assignment, name assignment, confirmation message, `IsNotGraveStone` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one `SB409-CAND-001` row.
- Targeted source scan: passed; mobile/source-statue/prompt-response guards are present and preserved behavior evidence remains present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Trades/Stone/BaseStatue.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Trades/Stone/BaseStatue.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-409` source commit: pending. `SOURCE-BATCH-410+` should run fresh candidate discovery after `SOURCE-BATCH-409`.
