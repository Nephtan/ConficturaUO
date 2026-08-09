# SOURCE-BATCH-397 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-397+` selected one clean non-gated source target:

- `SB397-CAND-001` / `SOURCE-BATCH-397` / FireworksWand guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-397+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Weapons/Maces/FireworksWand.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Weapons/Maces/FireworksWand.cs`: `0`.
- Previously considered small candidates with active overlay rows or policy-heavy behavior were skipped, including `Guillotine.cs`, Halloween grave stones, dart boards, and housing decoration/redeed items.

## Recommendation

Open `SOURCE-BATCH-397` for `Data/Scripts/Items/Weapons/Maces/FireworksWand.cs` and keep the edit guard-only: reject stale/null mobiles or deleted source wands before existing firework launch behavior.
