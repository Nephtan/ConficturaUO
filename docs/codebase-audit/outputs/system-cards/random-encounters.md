# System: Random Encounters

## Classification

GameplayLayer

## Summary

Automated PvE encounter layer driven by source and data markers.

## Source Files

Matched source files: 11.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/RandomEncounters/Commands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/RandomEncounters/Definitions.cs | WorldState |  | No |  |
| Data/Scripts/Custom/RandomEncounters/EncounterEngine.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/RandomEncounters/Helpers.cs | StartupWiring | Initialize | No |  |
| Data/Scripts/Custom/RandomEncounters/Import.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/RandomEncounters/Records.cs | WorldState |  | No |  |
| Data/Scripts/Custom/RandomEncounters/RedBlackTree.cs | WorldState |  | No |  |
| Data/Scripts/Custom/RandomEncounters/SpawnFinder.cs | WorldState |  | No |  |
| Data/Scripts/Custom/RandomEncounters/Timers.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Custom/RandomEncounters/XmlDateCount.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/RandomEncounters/XmlLinePreservingDoc.cs | WorldState |  | No |  |

## Data Files

./Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml;Data/Import/Premiums/RandomEncounters.xml

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/RandomEncounters/Commands.cs; Line=23; LikelySystem=Custom:RandomEncounters; RegistrationLine=Server.Commands.CommandSystem.Register(}.Command) | Data/Scripts/Custom/RandomEncounters/Commands.cs:23 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;Initialize

## Serialized State

Serialized marker files: 2. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Character Level; XML/config encounter tables; timers; cleanup attachments

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Random_Encounter_Engine.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
