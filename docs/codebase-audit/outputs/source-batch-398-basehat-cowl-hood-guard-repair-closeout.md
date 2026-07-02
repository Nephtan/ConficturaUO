# SOURCE-BATCH-398 BaseHat Cowl Hood Guard Repair Closeout

## Result

`SOURCE-BATCH-398` implemented `SB398-CAND-001`, a non-gated guard repair for `BaseHat` cowl hood color matching.

## Source Change

- File: `Data/Scripts/Items/Clothing/Hats.cs`
- `BaseHat.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source hat is deleted.
- `HatTarget.OnTarget(Mobile from, object targeted)` now returns immediately for stale/null mobile or source-hat callback state.
- Deleted target items now use the existing invalid-target message path without applying hue changes.

## Preserved Behavior

- Cowl-hood-only activation, equipped-hat requirement, target assignment, target range, eligible layer list, distinct-color requirement, hue assignment, existing messages, inherited serialization behavior, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved cowl hood color matching behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-398` source commit: `183bf23c` (`fix: guard BaseHat cowl hood interactions`). `SOURCE-BATCH-399+` should run fresh candidate discovery after `SOURCE-BATCH-398`.
