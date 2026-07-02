# SOURCE-BATCH-400 BaseKnife Guard Repair Closeout

## Result

`SOURCE-BATCH-400` implemented `SB400-CAND-001`, a non-gated guard repair for `BaseKnife` interaction handling.

## Source Change

- File: `Data/Scripts/Items/Weapons/Knives/BaseKnife.cs`
- `BaseKnife.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source knife is deleted.

## Preserved Behavior

- BaseKnife combat defaults, weapon skill/type/animation, hit and miss sounds, localized prompt `1010018`, `BladedItemTarget` assignment, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved prompt/target assignment behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-400` source commit: `c92a7ac1` (`fix: guard BaseKnife interactions`). `SOURCE-BATCH-401+` should run fresh candidate discovery after `SOURCE-BATCH-400`.
