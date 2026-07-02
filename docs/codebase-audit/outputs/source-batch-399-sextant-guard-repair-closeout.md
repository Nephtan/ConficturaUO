# SOURCE-BATCH-399 Sextant Guard Repair Closeout

## Result

`SOURCE-BATCH-399` implemented `SB399-CAND-001`, a non-gated guard repair for `Sextant` interaction handling.

## Source Change

- File: `Data/Scripts/Items/Trades/Fishing/Misc/Sextant.cs`
- `Sextant.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source sextant is deleted.

## Preserved Behavior

- Sextant and MagicSextant inherited use behavior, coordinate formatting, world and region message decisions, MagicSextant Underworld behavior, existing messages, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved sextant coordinate/message behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-399` source commit: pending. `SOURCE-BATCH-400+` should run fresh candidate discovery after `SOURCE-BATCH-399`.
