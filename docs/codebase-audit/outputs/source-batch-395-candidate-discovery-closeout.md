# SOURCE-BATCH-395 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-395` ran fresh non-gated candidate discovery after `SOURCE-BATCH-394` and selected one recommended zero-gate, zero-overlay target.

## Recommended Candidate

- Candidate: `SB395-CAND-001`
- Batch: `SOURCE-BATCH-395`
- System: `Items:Misc / Games / Tarot`
- File: `Data/Scripts/Items/Misc/Games/Tarot.cs`
- Behavior: add stale/null mobile and deleted source-item guards before existing Tarot fortune or decorative toggle behavior.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Exclusions

Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded.

## Next Step

Implement `SOURCE-BATCH-395` as a guard-only source batch, verify it, commit it, and leave `SOURCE-BATCH-396+` pending fresh discovery.
