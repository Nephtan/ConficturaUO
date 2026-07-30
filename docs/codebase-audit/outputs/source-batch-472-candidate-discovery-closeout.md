# SOURCE-BATCH-472 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-472` ran fresh non-gated discovery after `SOURCE-BATCH-471` and selected one clean guard repair candidate.

## Recommended Target

- Candidate: `SB472-CAND-001`
- Batch: `SOURCE-BATCH-472`
- System: `Magic:Magery / Spellbook`
- File: `Data/Scripts/Magic/Magery/Spellbook.cs`
- Behavior: add stale/null mobile and deleted source spellbook guards to `Spellbook.OnDoubleClick(Mobile from)` before reading `from.Backpack` or displaying the book.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Intake register completed rows for this file: `0`

## Candidate Notes

The scan also surfaced trades, taxidermy, gardening, quest, and command-adjacent candidates. Those were not selected for this batch because they are more likely to cross corpse flow, economy/trades, region, quest, staff/access, or command-policy boundaries. `Spellbook.OnDoubleClick` is still central magic infrastructure, but the selected edit is a single local guard before the existing parent/backpack and `DisplayTo` flow.

## Result

Proceed with `SOURCE-BATCH-472 Spellbook Guard Repair`. `SOURCE-BATCH-473+` should run fresh candidate discovery after `SOURCE-BATCH-472` commits.
