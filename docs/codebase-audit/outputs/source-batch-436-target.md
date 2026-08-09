# SOURCE-BATCH-436 LeatherNinjaBelt Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-436`
- Candidate: `SB436-CAND-001`
- System: `Items:Trades / Ninjitsu / LeatherNinjaBelt`
- Source file: `Data/Scripts/Items/Trades/Ninjitsu/LeatherNinjaBelt.cs`
- Behavior: add a stale/null/mobile/source-item guard to `LeatherNinjaBelt.OnDoubleClick(Mobile from)` before existing Ninjitsu weapon dispatch.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard LeatherNinjaBelt interactions`.
