# System: Invasion

## Classification

StaffEventTool

## Summary

Staff/event PvE pressure system with gumps, spawns, rewards, and event lifecycle risks.

## Source Files

Matched source files: 100.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Invasion System/Add Ins/BaseSpecialCreature.cs | StartupWiring | CustomTimerSubclass;OnMovement;Timer.DelayCall | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Lord BlackThorn Clone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/MobileFeatures.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Custom/Invasion System/Add Ins/TrashPiles.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Assembly Analyzer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Assembly Repair Kit.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Assembly Upgrade Kit.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Daemon Power Core.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Dragon Power Core.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Dragons Blood.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Gargoyle Power Core.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Overseer Power Core.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Runic Clockwork Assembly.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Runic Golem Power Core.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/BaseAssembly.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/IronDragon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/MechGargoyle.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/MetalDaemon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/Overseer.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/ASSEMBLY/runicGolem.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Bags/IronDragonBag.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Bags/MechanicalGargoyleAssemblyBag.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Bags/MetalDaemonBag.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Bags/OverSeerAssemblyBag.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Metal Assemblies/Bags/RunicGolemAssemblyBag.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/galleonAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateCaptain.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateCrew.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Chasing Pirates/PirateShip.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Morgan/Morgan2.cs | StartupWiring | CustomTimerSubclass;OnMovement | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Morgan/Pirate.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Morgan/PirateCaptain.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Morgan/pirateevil.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Pirate Set/Pirate 2.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Pirate Set/Pirate First Mate.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Pirate Set/Pirate.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Pirate Set/PirateCabinboy.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/Pirates/Pirate Set/PirateCaptain.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/BaseSubChampion.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/lizardmanmage.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/LordBlackThornBot.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/LordOrcalis.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/OgreChieftan.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/OrcKing.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/PirateLeader.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/SpiderQueen.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/Ssslither.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/TheSavageGeneral.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Add Ins/SubChamps/WidowQueen.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartStopBritFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopBuccaneersDenFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopCoveFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopMaginciaFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopMinocFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopNujelmFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopSkaraBraeFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopTrinsicFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopVesperFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Lodor/StartstopYewFelucca.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/SerpentIsland/StartstopLunaMalas.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/SerpentIsland/StartstopUmbraMalas.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartStopBritTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopBuccaneersDenTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopCoveTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopMaginciaTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopMinocTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopNujelmTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopSkaraBraeTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopTrinsicTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopVesperTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Sosaria/StartstopYewTrammel.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Stones/Ancient CitadelInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/BritInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/BuccaneersDenInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/CoveInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/GargoyleCityInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/LakeShireMiregInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/LunaInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/MaginciaInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/MinocInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/MistasInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/MontorInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/NujelmInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/RegVolonInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/SavageCampInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/SkaraBraeInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/TerortSkitasInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/TrinsicInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/UmbraInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/VesperInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Stones/YewInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Underworld/StartstoGargoyleCityIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopAncientCitadelIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopLakeShireMiregIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopMistasIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopMontorIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopRegValomIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopSavageCampIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Underworld/StartstopTerortSkitasIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |

## Data Files

Data/The Pirate Captain Invader/Speech.txt

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs; Line=14; LikelySystem=Custom:Invasion System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs:14 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;OnMovement;CustomTimerSubclass;OnMovement;Timer.DelayCall;Initialize

## Serialized State

Serialized marker files: 68. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Mobiles; items; staff commands; world timers

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Invasion_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
