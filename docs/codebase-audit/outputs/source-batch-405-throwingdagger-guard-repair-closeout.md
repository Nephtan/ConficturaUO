# SOURCE-BATCH-405 ThrowingDagger Guard Repair Closeout

## Result

`SOURCE-BATCH-405` implemented `SB405-CAND-001`, a non-gated guard repair for `ThrowingDagger` harmful throw interactions.

## Source Change

- File: `Data/Scripts/Items/Weapons/Knives/ThrowingDagger.cs`
- `ThrowingDagger.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source dagger item is deleted.
- `ThrowingDagger.InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately when `from` is null/deleted, the source dagger reference is null/deleted, or the targeted mobile is deleted.

## Preserved Behavior

- ThrowingDagger default name, item ID, weight, layer, held-weapon requirement, target range, `TargetFlags.Harmful`, `HarmfulCheck`, direction and animation behavior, hit chance, damage calculation, dagger `MoveToWorld` behavior, moving effects, miss message, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved held-weapon and harmful throw behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-405` source commit: `adb027dd` (`fix: guard ThrowingDagger interactions`). `SOURCE-BATCH-406+` should run fresh candidate discovery after `SOURCE-BATCH-405`.
