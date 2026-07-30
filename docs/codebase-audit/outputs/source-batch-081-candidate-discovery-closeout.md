# SOURCE-BATCH-081 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-081+` discovery found one zero-gate, zero-overlay explorer item guard candidate after `SOURCE-BATCH-080` exhausted `source-batch-078-candidate-discovery.csv`.

The recommended target is `SB081-CAND-001` / `SOURCE-BATCH-081 ArrowsAndBolts Guard Repair`.

## Candidate Evidence

- `Data/Scripts/Items/Explorers/ArrowsAndBolts.cs`: POST-BATCH-Y gate hits `0`; active overlay rows `0`.

## Recommendation

Select `ArrowsAndBolts` because it has four repeated bundle-splitting interactions that dereference `from.Backpack` before stale/null guards. The repair is local, guard-only, and preserves quantities, messages, item creation, bundle deletion, serialization, and layout.

Candidates with active overlay rows or policy-heavy behavior, including `TitleChangeDeed`, `MagicCandle`, `MagicTorch`, `MagicLantern`, boats, houses, world travel, and region/map-adjacent files, were excluded from this discovery pass.

## Verification

- `source-batch-081-candidate-discovery.csv` imports successfully.
- The recommended candidate has populated gate, overlay, risk, verification, source evidence, and unchanged-behavior fields.
- The recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
- No source/project/XML/config/data file was changed by discovery alone.
