# SOURCE-BATCH-040 Candidate Discovery Closeout

## Summary

SOURCE-BATCH-040 candidate discovery ran after the `source-batch-037-candidate-discovery.csv` implementation queue was exhausted by MusicBox.

The discovery pass selected six small zero-gate, zero-overlay reward dye tub wrapper guard candidates:

- `SB040-CAND-001` / `SOURCE-BATCH-040` / RewardBlackDyeTub Guard Repair
- `SB040-CAND-002` / `SOURCE-BATCH-041` / SpecialDyeTub Guard Repair
- `SB040-CAND-003` / `SOURCE-BATCH-042` / LeatherDyeTub Guard Repair
- `SB040-CAND-004` / `SOURCE-BATCH-043` / FurnitureDyeTub Guard Repair
- `SB040-CAND-005` / `SOURCE-BATCH-044` / RunebookDyeTub Guard Repair
- `SB040-CAND-006` / `SOURCE-BATCH-045` / StatuetteDyeTub Guard Repair

`SB040-CAND-001` is the recommended next source target.

## Selection Rules

- Candidate files must have POST-BATCH-Y gate hits=0.
- Candidate files must have active overlay rows=0.
- Candidate behavior must be a narrow guard-only source repair.
- Candidate behavior must preserve `RewardSystem.CheckIsUsableBy` policy and `base.OnDoubleClick` behavior.
- Candidate behavior must not cross staff/access, command policy, balance/economy, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gates.

## Exclusions

- Hair/beard dye files were not selected because active overlay rows match those files.
- Economy/balance-adjacent game items and broader artifact/combat/travel surfaces remain excluded.

## Verification

- `source-batch-040-candidate-discovery.csv` imports with `Import-Csv`.
- Each candidate has nonblank behavior, system, file, gate result, overlay result, risk, verification, and unchanged-behavior fields.
- Recommended candidate has POST-BATCH-Y gate hits=0 and active overlay rows=0.
- No source/project/XML/config/data files changed in this discovery batch.
- `git diff --check` passed with expected LF-to-CRLF warnings only.

## Result

`SOURCE-BATCH-040+` is ready to implement RewardBlackDyeTub Guard Repair if preflight remains clean.
