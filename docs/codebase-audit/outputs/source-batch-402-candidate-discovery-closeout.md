# SOURCE-BATCH-402 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-402+` selected one clean non-gated source target:

- `SB402-CAND-001` / `SOURCE-BATCH-402` / ThrowingWeapon guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-402+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Weapons/Marksman/ThrowingWeapon.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Weapons/Marksman/ThrowingWeapon.cs`: `0`.
- Broader framework, housing, economy, travel, and active-overlay candidates were skipped in favor of this narrow ammo-cycling guard.

## Recommendation

Open `SOURCE-BATCH-402` for `Data/Scripts/Items/Weapons/Marksman/ThrowingWeapon.cs` and keep the edit guard-only: reject stale/null mobiles, deleted source items, and missing backpacks before existing ammo cycling behavior.
