# SOURCE-BATCH-440 Candidate Discovery Closeout

`SOURCE-BATCH-440` ran fresh non-gated candidate discovery after `SOURCE-BATCH-439`.

## Result

Recommended implementation target: `SB440-CAND-001` / `EssenceOrb` guard repair.

## Evidence

- Expected source file: `Data/Scripts/Items/Misc/Dyes/Essence/EssenceOrb.cs`
- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Existing source evidence: `EssenceOrb.OnDoubleClick` read `from` and source item state before stale/null guards, then mutated owner, morph, player appearance, sound, particles, and messages.

## Skips

- `BarbaricSatchel.cs` was skipped as broader owner/gump/equipment-transform behavior.

## Decision

Proceed with `SOURCE-BATCH-440 EssenceOrb Guard Repair`. Keep `SOURCE-BATCH-441+` pending fresh discovery after the source batch commits.
