# SOURCE-BATCH-441 Candidate Discovery Closeout

`SOURCE-BATCH-441` ran fresh non-gated candidate discovery after `SOURCE-BATCH-440`.

## Result

Recommended implementation target: `SB441-CAND-001` / `TapestryOfSosaria` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Special/TapestryOfSosaria.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `TapestryOfSosaria.OnDoubleClick` read `from` before stale/null guards, then performed range check and display gump send.

## Skips

- `StableStone.cs` was skipped as broader pet-stabling, bank-fee, context-menu, speech, and gump behavior.
- `RoseOfTrinsic.cs` was skipped as timed petal spawning, stat-mod consumable, and consumption behavior.

## Decision

Proceed with `SOURCE-BATCH-441 TapestryOfSosaria Guard Repair`. Keep `SOURCE-BATCH-442+` pending fresh discovery after the source batch commits.
