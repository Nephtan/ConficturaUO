# SOURCE-BATCH-443 Candidate Discovery Closeout

`SOURCE-BATCH-443` ran fresh non-gated candidate discovery after `SOURCE-BATCH-442`.

## Result

Recommended implementation target: `SB443-CAND-001` / `Lockpick` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Trades/Thieving/LockPick.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `Lockpick.OnDoubleClick` dereferenced `from` before stale/null guards; `InternalTarget.OnTarget` dereferenced source lockpick, mobile, and target item state before full stale-state guards; `InternalTimer.OnTick` dereferenced delayed mobile/source lockpick/target state after a timer delay.

## Skips

- `CommunicationCrystals.cs` was skipped because exact-file backlog overlay rows exist for save compatibility/runtime-hook review and the file includes speech broadcasting/linking behavior.
- `PlasmaGrenade.cs` was skipped as combat damage, harmful-region, targeting, and delayed explosion timer behavior.
- `ThermalDetonator.cs` was skipped as combat damage, harmful-region, targeting, and delayed explosion timer behavior.
- `BaseSuit.cs` was skipped as access-policy-adjacent suit behavior.

## Decision

Proceed with `SOURCE-BATCH-443 Lockpick Guard Repair`. Keep `SOURCE-BATCH-444+` pending fresh discovery after the source batch commits.
