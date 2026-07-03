# SOURCE-BATCH-461 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-461` ran fresh non-gated discovery after `SOURCE-BATCH-460` and selected `SB461-CAND-001`, the `SearchPage` read-path guard repair.

## Recommended Target

- Candidate: `SB461-CAND-001`
- Batch: `SOURCE-BATCH-461`
- File: `Data/Scripts/Quests/Search/SearchPage.cs`
- Gate result: exact-file POST-BATCH-Y gate hits `0`
- Active overlay result: exact-file active overlay rows `0`

## Skipped Or Deferred Candidates

- `CourierMail.cs`, `BookWitchBrewing.cs`, and `BookDruidBrewing.cs` remain clean candidates for later one-item batches if exact-file preflight still passes.
- `ScrollClue.cs`, `GygaxStatue.cs`, and `DDRelicTablet.cs` were deferred for separate review because their nearby interaction logic is broader than a simple read-only page opener.
- Policy-sensitive housing, boat, travel, trade/craft, staff/admin, XMLSpawner, magic/combat behavior, serializer layout, project/config/data, XML/config/data, and reorganization candidates remain excluded under current executive fences.

## Result

The selected candidate is narrow enough for one source batch because it only guards opening an existing search page gump.
