# SOURCE-BATCH-435 Fukiya Guard Repair Closeout

## Result

`SOURCE-BATCH-435` implemented `SB435-CAND-001`, a non-gated guard repair for Fukiya double-click interaction.

## Source Change

- File: `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs`
- `Fukiya.OnDoubleClick(Mobile from)` now returns immediately when `from` is null, `from` is deleted, or the source fukiya is deleted before existing Ninjitsu weapon dispatch behavior.

## Preserved Behavior

`NinjaWeapon.AttemptShoot((PlayerMobile)from, this)` dispatch, `PlayerMobile` cast behavior, ammo type `FukiyaDarts`, range `0` to `6`, damage `4` to `6`, localized message IDs `1063325` through `1063330`, `AttackAnimation` behavior, uses/poison properties, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Verification

- Candidate CSV import: passed; one recommended `SB435-CAND-001` row.
- Targeted source scan: passed; OnDoubleClick guard is present; Ninjitsu weapon dispatch, message IDs, ammo/range/damage, attack animation, uses/poison properties, and serialization remain present.
- Exact-file POST-BATCH-Y scan: passed; `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs` has `0` gate hits.
- Exact-file active overlay scan: passed; `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs` has `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-435` source commit: pending. `SOURCE-BATCH-436+` should run fresh candidate discovery after `SOURCE-BATCH-435`.
