# SOURCE-BATCH-435 Fukiya Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-435`
- Candidate: `SB435-CAND-001`
- System: `Items:Trades / Ninjitsu / Fukiya`
- File: `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs`

## Behavior

Add an early stale/null guard to `Fukiya.OnDoubleClick(Mobile from)` before existing Ninjitsu weapon dispatch behavior.

Allowed source change:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

`NinjaWeapon.AttemptShoot((PlayerMobile)from, this)` dispatch, `PlayerMobile` cast behavior, ammo type `FukiyaDarts`, range `0` to `6`, damage `4` to `6`, localized message IDs `1063325` through `1063330`, `AttackAnimation` behavior, uses/poison properties, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Fence Result

Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs`: `0`.

Exact-file active overlay rows for `Data/Scripts/Items/Trades/Ninjitsu/Fukiya.cs`: `0`.

No gated approval is crossed.
