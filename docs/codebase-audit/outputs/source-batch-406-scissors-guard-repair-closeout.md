# SOURCE-BATCH-406 Scissors Guard Repair Closeout

## Result

`SOURCE-BATCH-406` implemented `SB406-CAND-001`, a non-gated guard repair for `Scissors` targeting interactions.

## Source Change

- File: `Data/Scripts/Items/Trades/Tailor Items/Scissors.cs`
- `Scissors.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source scissors item is deleted.
- `Scissors.InternalTarget.OnTarget(Mobile from, object targeted)` and `OnNonlocalTarget(Mobile from, object targeted)` now return immediately when `from` is null/deleted or the source scissors reference is null/deleted.

## Preserved Behavior

- Scissors item ID, weight, target prompt message, target range, `TargetFlags.None`, self-scissor localized messages, running-with-scissors localized message, movable/nonmovable `IScissorable` dispatch, invalid-target message, success sound `0x248`, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved Scissors targeting behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-406` source commit: `df625beb` (`fix: guard Scissors interactions`). `SOURCE-BATCH-407+` should run fresh candidate discovery after `SOURCE-BATCH-406`.
