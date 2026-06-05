# System: Offline Skill Training

## Classification

GameplayLayer

## Summary

Offline progression system for skill training.

## Source Files

Matched source files: 7.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Offline Skill Training/Items/AdvancedStudyBooks.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Items/LegendaryStudyBooks.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Items/RandomStudyBooks.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Items/StandardStudyBooks.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Items/StudyBook.cs | StartupWiring | EventSink;Initialize;OnLogin;OnLogout;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs | PlayerProgression |  | No |  |
| Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs | Persistence |  | Yes |  |

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

EventSink;Initialize;OnLogin;OnLogout;Timer.DelayCall

## Serialized State

Serialized marker files: 6. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

PlayerMobile; skill system; persistence

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/offline-skill-training.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
