# SOURCE-BATCH-037 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-037 candidate discovery ran after the `source-batch-031-candidate-discovery.csv` implementation queue was exhausted by Dice20.

The discovery pass selected three small zero-gate, zero-overlay guard candidates:

- `SB037-CAND-001` / `SOURCE-BATCH-037` / EverlastingBottle Guard Repair
- `SB037-CAND-002` / `SOURCE-BATCH-038` / EverlastingLoaf Guard Repair
- `SB037-CAND-003` / `SOURCE-BATCH-039` / MusicBox Guard Repair

`SB037-CAND-001` is the recommended next source target.

## Selection Rules

- Candidate files must have POST-BATCH-Y gate hits=0.
- Candidate files must have active overlay rows=0.
- Candidate behavior must be a narrow guard-only source repair.
- Candidate behavior must not cross staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gates.

## Exclusions

- D&D handbook/guide files were not selected because active overlay rows match those files.
- `LiarsDice` was not selected because it is game-balance/economy-adjacent.
- `GemOfSeeing`, spell artifacts, and combat/progression artifacts were not selected because their interactions are broader policy-adjacent surfaces.
- Already guarded or recently completed candidates were not selected.

## Verification

- `source-batch-037-candidate-discovery.csv` imports with `Import-Csv`.
- Each candidate has nonblank behavior, system, file, gate result, overlay result, risk, verification, and unchanged-behavior fields.
- Recommended candidate has POST-BATCH-Y gate hits=0 and active overlay rows=0.
- No source/project/XML/config/data files changed in this discovery batch.
- `git diff --check` passed with expected LF-to-CRLF warnings only.

## Result

`SOURCE-BATCH-037+` is ready to implement EverlastingBottle Guard Repair if preflight remains clean.
