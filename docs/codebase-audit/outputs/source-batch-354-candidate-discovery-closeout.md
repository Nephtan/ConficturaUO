# SOURCE-BATCH-354 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-354` ran fresh non-gated candidate discovery after `SOURCE-BATCH-353` closed. The recommended target is `SB354-CAND-001`, a guard-only repair for `MuseumBook`.

## Recommended Target

- Batch: `SOURCE-BATCH-354`
- Candidate: `SB354-CAND-001`
- File: `Data/Scripts/Quests/Museum/MuseumBook.cs`
- Behavior: add stale/null mobile and deleted source item guards before `MuseumBook.OnDoubleClick` reads `from.Backpack`.
- Fence result: POST-BATCH-Y exact-file gate hits=0; exact-file active overlay rows=0.
- Boundary: preserve `ArtOwner` assignment/check behavior, ownership denial message, `MuseumBookGump` flow, antique inventory helper behavior, and serialization.

## Skipped Candidate Notes

- `NewPlayerTicket.cs` was not selected because the apparent guard surface crosses target/gump reward handling.
- `HoardPile.cs` was not selected because it crosses randomized reward/economy and region-sensitive loot behavior.
- Porter/robot item candidates were not selected in this pass because they cross follower, summon, and map-placement behavior.
- Travel, housing, vendor, potion-base, and fishing-net target candidates remain excluded unless a later focused goal selects them explicitly.

## Next Step

Implement `SOURCE-BATCH-354 MuseumBook Guard Repair`, then run the standard source-batch verification and commit cycle. `SOURCE-BATCH-355+` should run fresh discovery after `SOURCE-BATCH-354` commits.
