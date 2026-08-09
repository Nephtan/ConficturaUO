# SOURCE-BATCH-401 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-401+` selected one clean non-gated source target:

- `SB401-CAND-001` / `SOURCE-BATCH-401` / BaseSword guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-401+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Weapons/Swords/BaseSword.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Weapons/Swords/BaseSword.cs`: `0`.
- Broader framework, housing, economy, travel, and active-overlay candidates were skipped in favor of this narrow bladed-item target guard.

## Recommendation

Open `SOURCE-BATCH-401` for `Data/Scripts/Items/Weapons/Swords/BaseSword.cs` and keep the edit guard-only: reject stale/null mobiles and deleted source swords before existing prompt and target assignment behavior.
