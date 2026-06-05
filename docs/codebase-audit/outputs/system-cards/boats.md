# System: Boats

## Classification

GameplayLayer

## Summary

Boat item and travel system.

## Source Files

Matched source files: 33.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateShip.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BaseBoat.cs | StartupWiring | CustomTimerSubclass;EventSink;Initialize;OnSpeech;WorldSave | Yes | SendGump |
| Data/Scripts/Items/Boats/BaseBoatDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BaseDockedBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BoatBuild.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BoatDoor.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BoatModel.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/BoatStain.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Cargo.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Boats/CarpetBuild.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Boats/DockingLantern.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Galleons.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/GrapplingHook.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Hold.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/LargeBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/LargeDragonBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/MediumBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/MediumDragonBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/PirateBounty.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Boats/Plank.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Items/Boats/RenameBoatPrompt.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Items/Boats/SmallBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/SmallDragonBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Strandedness.cs | StartupWiring | EventSink;Initialize | No |  |
| Data/Scripts/Items/Boats/TillerMan.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Items/Boats/TinyBoat.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Vessels.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Boats/Gumps/ConfirmDryDockGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Items/Boats/Gumps/FindboatGump.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Items/Boats/Gumps/TillerManGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Mobiles/Humanoids/Sailors/BoatPirates.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Humanoids/Sailors/BoatSailors.cs | Persistence |  | Yes |  |

## Data Files

No XML/config/text/json references were found in Phase 1 string-reference markers.

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs; Line=19; LikelySystem=Custom:BoatNavigationTotem; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:19 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs; Line=24; LikelySystem=Custom:BoatNavigationTotem; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:24 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs; Line=29; LikelySystem=Custom:BoatNavigationTotem; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:29 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Items/Boats/Gumps/FindboatGump.cs; Line=17; LikelySystem=Items:Boats; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Items/Boats/Gumps/FindboatGump.cs:17 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;EventSink;Initialize;OnSpeech;WorldSave;EventSink;Initialize;Initialize

## Serialized State

Serialized marker files: 27. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Items; maps; movement; shipwright docs and vendors

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Boat_Core_Mechanics.md;docs/wiki/boat-navigation-control.md;docs/wiki/NPC_Shipwright_Sailing_Guide.md;docs/wiki/Shipwright_Sailing_Tutorial.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
