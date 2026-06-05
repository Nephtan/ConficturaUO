# System: Monster Nests

## Classification

GameplayLayer

## Summary

Monster nest PvE system with world-state and reward pressure.

## Source Files

Matched source files: 6.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/MonsterNest/MonsterNest.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/MonsterNest/MonsterNestEntity.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/MonsterNest/MonsterNestLoot.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/MonsterNest/Types/LizardmanNest.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/MonsterNest/Types/RatmanNest.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/MonsterNest/Types/UndeadNest.cs | Persistence |  | Yes |  |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review items, NPCs, speech, regions, and gumps in later phases. |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass

## Serialized State

Serialized marker files: 6. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Mobiles; items; regions; player progression

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Monster_Nest_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
