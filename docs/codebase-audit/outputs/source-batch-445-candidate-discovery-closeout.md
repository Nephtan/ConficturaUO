# SOURCE-BATCH-445 Candidate Discovery Closeout

`SOURCE-BATCH-445` ran fresh non-gated candidate discovery after `SOURCE-BATCH-444`.

## Result

Recommended implementation target: `SB445-CAND-001` / `MagicStaffTarget` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Wands/MagicStaffTarget.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `MagicStaffTarget.OnTarget` dereferenced `m_Item` and dispatched `from` before stale/null guards.
- Override evidence: `rg` found no `DoMagicStaffTarget` overrides beyond `BaseMagicStaff`.

## Skips

- `FoeRequiemSong.cs` was skipped as spell/combat target behavior.
- `DruidPouch.cs` was skipped as broader magic gump/pouch workflow.
- potion target handlers were skipped as combat/poison policy behavior.
- remaining Government, Invasion, Homestead, StaffTools, economy/reward, housing/addon, pet, region/map, serializer, and reorganization surfaces remain outside this runner.

## Decision

Proceed with `SOURCE-BATCH-445 MagicStaffTarget Guard Repair`. Keep `SOURCE-BATCH-446+` pending fresh discovery after the source batch commits.
