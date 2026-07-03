# SOURCE-BATCH-444 Candidate Discovery Closeout

`SOURCE-BATCH-444` ran fresh non-gated candidate discovery after `SOURCE-BATCH-443`.

## Result

Recommended implementation target: `SB444-CAND-001` / `DoorSwitch` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Doors/DoorSwitch.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `DoorSwitch.OnDoubleClick` dereferenced mobile before stale/null guards; `switchit` loops dereferenced controlled-door list entries as `BaseDoor`; `AddDoor.OnTarget` dereferenced mobile, controlled-door list, and target item state before full stale-state guards.

## Skips

- `AnimalCages.cs` was skipped as pet/follower ownership and animal price behavior.
- `NameChangeDeed.cs` was skipped because the current double-click path is a no-op comment.
- `BaseSuit.cs` was skipped as access-policy-adjacent suit behavior.
- `PowerScrollBuy.cs` and `BankCheck.cs` remain skipped as economy/reward-adjacent behavior.

## Decision

Proceed with `SOURCE-BATCH-444 DoorSwitch Guard Repair`. Keep `SOURCE-BATCH-445+` pending fresh discovery after the source batch commits.
