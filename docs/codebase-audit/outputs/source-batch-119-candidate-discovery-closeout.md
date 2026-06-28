# SOURCE-BATCH-119 Candidate Discovery Closeout

## Summary

Ran fresh candidate discovery after the `source-batch-111-candidate-discovery.csv` trap/junk queue was exhausted at SOURCE-BATCH-118.

Discovery selected 25 clean Pagan reagent decorative rare item candidates. Each candidate is a one-file `OnDragLift(Mobile from)` guard repair with zero POST-BATCH-Y gate hits and zero active overlay rows.

## Recommended Target

- `SOURCE-BATCH-119` / `SB119-CAND-001` / `DecoBlackmoor`
- File: `Data/Scripts/Items/Special/Rares/PaganReagents/DecoBlackmoor.cs`
- Behavior: add stale/null/mobile/source-item guards before the existing collectible message and `base.OnDragLift(from)` delegation.

## Queue

The remaining queue runs from `SOURCE-BATCH-120` through `SOURCE-BATCH-143` for the other Pagan reagent decorative rare files recorded in `source-batch-119-candidate-discovery.csv`.

## Exclusions

- Tarot card rare files were not selected because `post-audit-active-backlog-status.csv` contains reviewed overlay rows for those files.
- Staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded.

## Verification

- Candidate discovery CSV imports with `Import-Csv`.
- Every candidate has a proposed batch id, behavior, system, source file, non-gated rationale, gate evidence, overlay evidence, risk, verification plan, unchanged-behavior constraints, source evidence, and suggested goal shape.
- Recommended candidate has zero POST-BATCH-Y gate hits and zero active overlay rows.
