# System: Clone Offline Player Characters

## Classification

GameplayLayer

## Summary

Offline character clone system with mobile/item clone behavior and command surfaces.

## Source Files

Matched source files: 13.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/BackpackClone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/CharacterClone.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/CheckClonesCommand.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/CloneOfflinePlayerCharacters.cs | StartupWiring | EventSink;Initialize;OnLogin;OnLogout | No |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/CloneThings.cs | Unknown |  | No |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/EtherealMountClone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/CloneOfflinePlayerCharacters/MountClone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Lord BlackThorn Clone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/NPC Control/CloneCommands.cs | CommandSurface | Initialize | Yes |  |
| Data/Scripts/Custom/NPC Control/StaffCommands/CloneMe.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Magic/Ninjitsu/MirrorImage.cs | Persistence |  | Yes |  |
| Data/Scripts/Magic/Research/Spells/Conjuration/ResearchClone.cs | CombatPolicy |  | No |  |
| Data/Scripts/Magic/Syth/Spells/Clone.cs | CombatPolicy |  | No |  |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/CloneOfflinePlayerCharacters/CheckClonesCommand.cs; Line=19; LikelySystem=Custom:CloneOfflinePlayerCharacters; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/CloneOfflinePlayerCharacters/CheckClonesCommand.cs:19 |
| Command $(Escape-MarkdownCell @{Command=Clone; AccessLevel=; Handler=Clone_OnCommand; File=Data/Scripts/Custom/NPC Control/CloneCommands.cs; Line=21; LikelySystem=Custom:NPC Control; RegistrationLine=CommandSystem.Register("Clone", accessLevel, new CommandEventHandler(Clone_OnCommand));}.Command) | Data/Scripts/Custom/NPC Control/CloneCommands.cs:21 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/NPC Control/CloneCommands.cs; Line=22; LikelySystem=Custom:NPC Control; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/NPC Control/CloneCommands.cs:22 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/NPC Control/StaffCommands/CloneMe.cs; Line=17; LikelySystem=Custom:NPC Control; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/NPC Control/StaffCommands/CloneMe.cs:17 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;EventSink;Initialize;OnLogin;OnLogout;Initialize

## Serialized State

Serialized marker files: 7. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

PlayerMobile; items; mobiles; timers

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Clone_Offline_Player_Characters.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
