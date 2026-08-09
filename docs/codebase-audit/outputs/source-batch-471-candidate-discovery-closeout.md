# SOURCE-BATCH-471 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-471` ran fresh non-gated candidate discovery after `SOURCE-BATCH-470` and selected one clean guard repair candidate.

## Recommended Target

- Candidate: `SB471-CAND-001`
- Batch: `SOURCE-BATCH-471`
- System: `Items:Trades / Tailor Items / Dyes`
- File: `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`
- Behavior: add stale/null mobile, deleted source dyes, stale target dye tub, and stale callback dye tub guards to `Dyes` interaction paths.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`
- Intake register completed rows for `Data/Scripts/Items/Trades/Tailor Items/Dyes.cs`: `0`
- No staff/access, command policy, balance/economy, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Candidate Notes

`Dyes.cs` was selected because its double-click, target, hue-picker callback, and custom hue callback paths could dereference stale/null mobile or dye tub state. Existing dye tub eligibility, hue picker behavior, custom hue picker behavior, and messages remain unchanged.

Skipped discovery hit:

- `Data/Scripts/Items/Magical/Artifacts/Artifact_RodOfResurrection.cs`: resurrection, pet resurrection, and henchman target behavior is broader gameplay behavior and should be handled only as a separate explicit target.

## Result

Proceed with `SOURCE-BATCH-471 Dyes Guard Repair`.
