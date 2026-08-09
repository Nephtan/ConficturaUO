# SOURCE-BATCH-407 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-407+` selected one clean non-gated source target:

- `SB407-CAND-001` / `SOURCE-BATCH-407` / NewPlayerTicket guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-407+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Deeds/NewPlayerTicket.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Deeds/NewPlayerTicket.cs`: `0`.
- Housing, travel, economy tuning, region-policy, and broader reward-design candidates were skipped in favor of this narrow ticket interaction guard. The selected edit preserves reward choices and only guards invalid interaction state.

## Recommendation

Open `SOURCE-BATCH-407` for `Data/Scripts/Items/Deeds/NewPlayerTicket.cs` and keep the edit guard-only: reject stale/null mobiles, deleted source tickets, missing backpacks, deleted target tickets, and stale gump responses before existing ticket pairing and reward behavior.
