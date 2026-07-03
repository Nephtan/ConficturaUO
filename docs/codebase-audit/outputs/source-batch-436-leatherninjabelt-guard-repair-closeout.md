# SOURCE-BATCH-436 LeatherNinjaBelt Guard Repair Closeout

## Summary

`SOURCE-BATCH-436` implemented `SB436-CAND-001`, a non-gated guard repair for LeatherNinjaBelt double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Trades/Ninjitsu/LeatherNinjaBelt.cs`

`LeatherNinjaBelt.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source belt is `Deleted`

The guard runs before the existing `NinjaWeapon.AttemptShoot((PlayerMobile)from, this)` dispatch.

## Preserved Behavior

- `NinjaWeapon.AttemptShoot((PlayerMobile)from, this)` dispatch
- PlayerMobile cast behavior
- ammo type `Shuriken`
- range `2` to `10`
- damage `3` to `5`
- localized message IDs `1063297` through `1063302`
- `AttackAnimation` behavior
- uses/poison properties
- context menu load/unload behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Ninjitsu/LeatherNinjaBelt.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Trades/Ninjitsu/LeatherNinjaBelt.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `LeatherNinjaBelt.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-436` source commit: pending. `SOURCE-BATCH-437+` should run fresh candidate discovery after `SOURCE-BATCH-436`.
