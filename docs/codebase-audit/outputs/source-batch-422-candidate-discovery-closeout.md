# SOURCE-BATCH-422 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-422+` selected one clean non-gated source target:

- `SB422-CAND-001` / `SOURCE-BATCH-422` / BaseImprisonedMobile guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-422+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Items/Special/BaseImprisonedMobile.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Items/Special/BaseImprisonedMobile.cs`: `0`, using the `Files`, `ActiveStatus`, and `ReviewStatus` columns from `post-audit-active-backlog-status.csv`.
- Broader gump framework, pet/control policy, reward, economy, region-policy, travel, staff/access, command, project/config/data, XML/config/data, serializer layout, and reorganization candidates remain excluded.

## Recommendation

Open `SOURCE-BATCH-422` for `Data/Scripts/Items/Special/BaseImprisonedMobile.cs` and keep the edit guard-only: reject stale/null mobiles and deleted source crystals before the existing backpack check, confirmation gump send, or backpack-use failure message.
