# SOURCE-BATCH-179 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-179+ discovery found four zero-gate, zero-overlay magic token/book guard candidates after the Bard scroll queue was exhausted.

The discovery output is docs/codebase-audit/outputs/source-batch-179-candidate-discovery.csv.

## Recommended Target

- Candidate: SB179-CAND-001
- Proposed batch: SOURCE-BATCH-179
- Target: DeathSkulls
- File: Data/Scripts/Magic/Death Knight/DeathSkulls.cs
- Behavior: add stale/null/mobile/source-token guards to all DeathKnightSkull OnDoubleClick message paths.

## Candidate Queue

- DeathSkulls
- HolySymbols
- DeathKnightSpellbook
- HolyManSpellbook

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for each candidate file: 0
- Exact-file active overlay rows for each candidate file: 0
- Previously completed source-batch register matches for each candidate file: 0
- Gated approval crossed: No

## Exclusions

Discovery excluded overlay-conflicting ForgetfulGem, SoulLantern, and HolySymbol files. It did not select staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization work.

## Next Step

Implement SOURCE-BATCH-179 / SB179-CAND-001 / DeathSkulls Guard Repair if exact-file preflight remains zero-gate and zero-overlay.