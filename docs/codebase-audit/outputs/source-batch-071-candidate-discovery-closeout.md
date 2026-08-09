# SOURCE-BATCH-071 Candidate Discovery Closeout

## Summary

`SOURCE-BATCH-071+` discovery ran after the skeleton-key queue was exhausted and identified two clean non-gated decoration deed guard candidates.

The recommended next target is `SB071-CAND-001` / `SOURCE-BATCH-071` / `DecoStatueDeed Guard Repair`.

## Candidate Results

- `SB071-CAND-001` / `SOURCE-BATCH-071` / `Data/Scripts/Items/Decorations/DecoIngotDeed.cs`: selected as recommended next candidate.
- `SB071-CAND-002` / `SOURCE-BATCH-072` / `Data/Scripts/Items/Decorations/MonsterStatueDeed.cs`: selected as follow-up candidate.

## Fence Evidence

Each selected candidate has:

- POST-BATCH-Y gate hits: `0`
- Active overlay rows: `0`
- Gate crossed: none

## Exclusions

Discovery skipped previously completed source files and avoided staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, and reorganization boundaries.

Higher-risk zero-overlay candidates such as `HorseArmor.cs`, housing tools, boat tools, travel/world-adjacent items, and broader lock/security systems were not selected for this narrow decoration-deed slice.

## Verification

- `source-batch-071-candidate-discovery.csv` imports with `Import-Csv`.
- Every selected candidate has nonblank behavior, system, file, gate evidence, overlay evidence, risk, verification, and unchanged-behavior fields.

## Result

Discovery is ready for sequential implementation beginning with `SOURCE-BATCH-071` / `DecoStatueDeed Guard Repair`.
