# SOURCE-BATCH-398 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-398+` selected one clean non-gated source target:

- `SB398-CAND-001` / `SOURCE-BATCH-398` / BaseHat cowl hood guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-398+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Clothing/Hats.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Clothing/Hats.cs`: `0`.
- Broad framework, housing, economy, travel, and active-overlay candidates were skipped in favor of this narrow cowl-hood color matching guard.

## Recommendation

Open `SOURCE-BATCH-398` for `Data/Scripts/Items/Clothing/Hats.cs` and keep the edit guard-only: reject stale/null mobiles, deleted source hats, and deleted target items before existing color matching behavior.
