# SOURCE-BATCH-417 Candidate Discovery Closeout

## Result

Fresh discovery for `SOURCE-BATCH-417+` selected one clean non-gated source target:

- `SB417-CAND-001` / `SOURCE-BATCH-417` / TarotCards guard repair.

## Evidence

- Executive decision `EXEC-0002` allows sequential non-gated repairs with one commit after each completed batch.
- `SOURCE-BATCH-417+` was `CandidateDiscoveryRequired` before this batch opened.
- Exact-file POST-BATCH-Y gate hits for the nine `Data/Scripts/Items/Special/Rares/TarotCards/*.cs` files: `0`.
- Exact-file active overlay rows for the nine `Data/Scripts/Items/Special/Rares/TarotCards/*.cs` files: `0`, using the `Files`, `ActiveStatus`, and `ReviewStatus` columns from `post-audit-active-backlog-status.csv`.
- BankCheck was skipped as money/economy-adjacent; Mailbox was skipped as housing-adjacent; EssenceOrb was skipped because its owner/morph behavior is broader than this decorative guard repair; potion production, resurrection, command-driven prank, reward, government, XMLSpawner, economy, region-policy, travel, and broader framework candidates remain excluded.

## Recommendation

Open `SOURCE-BATCH-417` for the TarotCards rare-decoration set and keep the edit guard-only: reject stale/null mobiles, deleted source tarot items, stale gump response state, and invalid static gump-send state before existing range checks, gump sends, sounds, random text generation, and card layout behavior.
