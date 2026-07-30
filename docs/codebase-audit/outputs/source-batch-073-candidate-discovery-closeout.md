# SOURCE-BATCH-073 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-073+` discovery ran after the decoration-deed queue was exhausted and identified two clean non-gated specialized learning-book guard candidates.

The recommended next target is `SB073-CAND-001` / `SOURCE-BATCH-073` / `MasonryBook Guard Repair`.

## Candidate Results

- `SB073-CAND-001` / `SOURCE-BATCH-073` / `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs`: selected as recommended next candidate.
- `SB073-CAND-002` / `SOURCE-BATCH-074` / `Data/Scripts/Items/Trades/Specialized/StoneMiningBook.cs`: selected as follow-up candidate.

## Fence Evidence

Each selected candidate has:

- POST-BATCH-Y gate hits: `0`
- Active overlay rows: `0`
- Gate crossed: none

## Exclusions

Discovery skipped previously completed source files and avoided staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization boundaries.

TaxidermyKit was left unselected for this narrow pass because it has a broader target flow than the specialized learning-book pattern. Firebomb was excluded because active overlay rows still match that file.

## Verification

- `source-batch-073-candidate-discovery.csv` imports with `Import-Csv`.
- Every selected candidate has nonblank behavior, system, file, gate evidence, overlay evidence, risk, verification, and unchanged-behavior fields.

## Result

Discovery is ready for sequential implementation beginning with `SOURCE-BATCH-073` / `MasonryBook Guard Repair`.
