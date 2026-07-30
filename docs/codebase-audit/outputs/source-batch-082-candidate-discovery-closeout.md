# SOURCE-BATCH-082 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-082+` discovery ran after `SOURCE-BATCH-081` exhausted its `ArrowsAndBolts` candidate queue.

The recommended target is `SB082-CAND-001` / `SOURCE-BATCH-082 ClothingBlessDeed Guard Repair`.

## Candidate Evidence

- `Data/Scripts/Items/Deeds/ClothingBlessDeed.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.
- `Data/Scripts/Items/Deeds/HairRestylingDeed.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.

## Recommendation

Select `ClothingBlessDeed` first because its double-click and target callback paths dereference mobile, backpack, source deed, and target state before full stale/null coverage. The repair is local, guard-only, and preserves blessing eligibility, messages, `LootType.Blessed` assignment, deed deletion, serialization, and layout.

`HairRestylingDeed` remains the next clean candidate for `SOURCE-BATCH-083`.

## Exclusions

- `Data/Scripts/Items/Deeds/DragonBardingDeed.cs` was excluded because active overlay rows match that file.
- `NewPlayerTicket` and `BarkeepContract` were left unselected because their source paths are tied to reward selection, housing/vendor placement, or staff/access behavior; those are not the best next non-gated guard-only targets under the current executive decisions.
- Food/race-consumption candidates were left unselected because their behavior is closer to hunger/race/balance tuning, even where a narrow guard-only edit might be possible.

## Verification

- `source-batch-082-candidate-discovery.csv` imports successfully.
- Every recorded candidate has populated behavior, system, files, gate result, overlay result, risk, verification, source evidence, and unchanged-behavior fields.
- The recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
- No project/XML/config/data file was changed by discovery.
