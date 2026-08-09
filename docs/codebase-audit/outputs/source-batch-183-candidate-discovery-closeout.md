# SOURCE-BATCH-183 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-183+ discovery found four zero-gate, zero-overlay Jedi/Syth datacron and spellbook guard candidates after the SOURCE-BATCH-179 candidate queue was exhausted.

The discovery output is docs/codebase-audit/outputs/source-batch-183-candidate-discovery.csv.

## Recommended Target

- Candidate: SB183-CAND-001
- Proposed batch: SOURCE-BATCH-183
- Target: SythDatacrons
- File: Data/Scripts/Magic/Syth/SythDatacrons.cs
- Behavior: add stale/null/mobile/source-token guards to all SythDatacron OnDoubleClick message paths.

## Candidate Queue

- SythDatacrons
- JediDatacrons
- SythSpellbook
- JediSpellbook

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for each candidate file: 0
- Exact-file active overlay rows for each candidate file: 0
- Previously completed source-batch register matches for each candidate file: 0
- Gated approval crossed: No

## Exclusions

Discovery did not select staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization work. Spellbook OnDragDrop transformation behavior remains outside the initial guard target and must stay unchanged.

## Next Step

Implement SOURCE-BATCH-183 / SB183-CAND-001 / SythDatacrons Guard Repair if exact-file preflight remains zero-gate and zero-overlay.
