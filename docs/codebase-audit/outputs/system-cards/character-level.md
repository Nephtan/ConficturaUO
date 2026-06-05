# System: Character Level

## Classification

GameplayLayer

## Summary

Player progression layer and level-aware behavior. Marker evidence is provisional until dependency and serialization phases deepen the review.

## Source Files

Matched source files: 2.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs | PlayerProgression |  | No |  |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs; Line=14; LikelySystem=Custom:CharacterLevel; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs:14 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs; Line=20; LikelySystem=Custom:CharacterLevel; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs:20 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

Initialize

## Serialized State

No serialization marker rows were found in matched files.

## Dependencies

PlayerMobile; Random Encounters; combat display; documentation source traces

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Character_Level_Recon_Report.md

## Verification Status

NeedsRuntimeReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
