# SOURCE-BATCH-402 ThrowingWeapon Guard Repair Closeout

## Result

`SOURCE-BATCH-402` implemented `SB402-CAND-001`, a non-gated guard repair for `ThrowingWeapon` ammo cycling.

## Source Change

- File: `Data/Scripts/Items/Weapons/Marksman/ThrowingWeapon.cs`
- `ThrowingWeapon.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source throwing weapon item is deleted.
- Missing backpacks now use the existing backpack-use failure message before ammo cycling.

## Preserved Behavior

- ThrowingWeapon randomized setup, ammo labels, stack behavior, OnMoveOver pickup behavior, backpack-use message, ammo cycling order, jester-only card/tomato branches, item ID/name changes, final ammo-change message, `InvalidateProperties` call, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved ammo cycling behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-402` source commit: pending. `SOURCE-BATCH-403+` should run fresh candidate discovery after `SOURCE-BATCH-402`.
