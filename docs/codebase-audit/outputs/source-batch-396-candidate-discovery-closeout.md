# SOURCE-BATCH-396 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-396` ran fresh non-gated candidate discovery after `SOURCE-BATCH-395` and selected one recommended zero-gate, zero-overlay target.

## Recommended Candidate

- Candidate: `SB396-CAND-001`
- Batch: `SOURCE-BATCH-396`
- System: `Items:Misc / Games / Tarot Poker`
- File: `Data/Scripts/Items/Misc/Games/tarotpoker.cs`
- Behavior: add stale/null mobile and deleted source-item guard before existing range, gump, card text, and betting-instruction behavior.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Exclusions

Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded. Betting instruction text is preserved; no economy or reward tuning is included.

## Next Step

Implement `SOURCE-BATCH-396` as a guard-only source batch, verify it, commit it, and leave `SOURCE-BATCH-397+` pending fresh discovery.
