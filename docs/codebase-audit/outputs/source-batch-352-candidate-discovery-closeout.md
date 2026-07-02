# SOURCE-BATCH-352 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-352` ran fresh non-gated candidate discovery after `SOURCE-BATCH-351` closed. The recommended target is `SB352-CAND-001`, a guard-only repair for `SpecialSeaweed`.

## Recommended Target

- Batch: `SOURCE-BATCH-352`
- Candidate: `SB352-CAND-001`
- File: `Data/Scripts/Items/Trades/Fishing/SpecialSeaweed.cs`
- Behavior: add stale/null mobile, deleted source item, and missing-backpack guards before `SpecialSeaweed.OnDoubleClick` reads `from.Backpack` or attempts bottle consumption.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: do not add a new source-item backpack-location rule. Preserve Seafaring skill checks, bottle requirement, potion mapping, messages, sound, and seaweed consumption semantics.

## Skipped Candidate Notes

- `NewPlayerTicket.cs` was not selected because the apparent guard surface crosses target/gump reward handling.
- `BarkeepContract.cs` remains excluded because it crosses vendor/housing/GM placement behavior.
- Fishing-net targets were not selected in this pass because target-water behavior is broader than the one-method `SpecialSeaweed` guard.
- Moonstone and house teleporter candidates remain excluded because they cross travel/region or housing behavior.

## Next Step

Implement `SOURCE-BATCH-352 SpecialSeaweed Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-353+` should run fresh discovery after `SOURCE-BATCH-352` commits.
