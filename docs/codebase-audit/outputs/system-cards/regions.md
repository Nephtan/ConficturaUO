# System: Regions

## Classification

SharedService

## Summary

Region policy framework affecting combat, travel, housing, and gameplay rules.

## Source Files

Matched source files: 56.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawnRegion.cs | WorldState |  | No |  |
| Data/Scripts/Custom/Government System/Regions/CityMarketRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs | RegionPolicy | OnSpeech;RegionOverride | No |  |
| Data/Scripts/Custom/RandomEncounters/Definitions.cs | WorldState |  | No |  |
| Data/Scripts/Custom/RandomEncounters/Records.cs | WorldState |  | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeGameRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/Custom/XMLSpawner/XmlPoints PvP/ChallengeRegionStone.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/BaseHouse.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Items/Special/House Raffle/HouseRaffleRegion.cs | ItemContent |  | No |  |
| Data/Scripts/System/Commands/Implementors/RegionCommandImplementor.cs | WorldState |  | No |  |
| Data/Scripts/System/Misc/Reporting.cs | StartupWiring | Initialize;Timer.DelayCall | No |  |
| Data/Scripts/System/Misc/Spawning.cs | CommandSurface | CustomTimerSubclass;Initialize | Yes | OnResponse;SendGump |
| Data/Scripts/System/Obsolete/Obsolete.cs | PacketNetwork | CustomTimerSubclass;EventSink;Initialize;OnMovement;OnSpeech;PacketHandlers.Register;RegionOverride;Timer.DelayCall;WorldSave | Yes | OnResponse;SendGump |
| Data/Scripts/System/Regions/BardDungeonRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/BardTownRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/BargeDeadRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/BaseRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/CaveRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/CrashRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/DawnRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/DeadRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/DungeonHomeRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/DungeonRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/GargoyleRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/GuardedRegion.cs | CommandSurface | CustomTimerSubclass;Initialize;OnSpeech;RegionOverride | No |  |
| Data/Scripts/System/Regions/HouseRegion.cs | StartupWiring | EventSink;Initialize;OnLogin;OnSpeech;RegionOverride | No | SendGump |
| Data/Scripts/System/Regions/Jail.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/LunaRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/MazeRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/MoonCore.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/NecromancerRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/NoHousingRegion.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/OutDoorBadRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/OutDoorRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/PirateRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/PrisonArea.cs | RegionPolicy | RegionOverride | No | SendGump |
| Data/Scripts/System/Regions/ProtectedRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/PublicRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/RegionMusic.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/SafeRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/SavageRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/SeaSpawnRegion.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/SkyHomeRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/StartRegion.cs | RegionPolicy | RegionOverride | No | SendGump |
| Data/Scripts/System/Regions/TownRegion.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/UmbraRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/UnderHouseRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/VillageRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/WantedRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/System/Regions/Spawning/SpawnDefinition.cs | WorldState |  | No |  |
| Data/Scripts/System/Regions/Spawning/SpawnEntry.cs | CommandSurface | Initialize;Timer.DelayCall | Yes |  |
| Data/Scripts/System/Regions/Spawning/SpawnPersistence.cs | Persistence |  | Yes |  |
| Data/Scripts/System/Skills/Weapon Abilities/WeaponArmorCalls.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/System/Source/Region.cs | StartupWiring | OnSpeech | No |  |
| Data/System/Source/Sector.cs | StartupWiring |  | No |  |
| Data/System/Source/Gumps/GumpAlphaRegion.cs | StartupWiring |  | No |  |

## Data Files

Could not find root element 'ServerRegions' in Regions.xml;Data/System/XML/Regions.xml;Invalid region type '{0}' in regions.xml;myrunuodb_{0}_{1}.txt;myrunuodb_{0}.txt;reportHistory.xml;staffHistory.xml

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=26; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:26 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=31; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:31 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=36; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:36 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=41; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:41 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=46; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:46 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=5253; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/System/Misc/Spawning.cs:5253 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=5286; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:5286 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=5291; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:5291 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=6464; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:6464 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=7516; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:7516 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=7521; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Misc/Spawning.cs:7521 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Misc/Spawning.cs; Line=7530; LikelySystem=System:Misc; RegistrationLine=CommandSystem.Register(command, access, handler);}.Command) | Data/Scripts/System/Misc/Spawning.cs:7530 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=10823; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:10823 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=10828; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:10828 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=10833; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:10833 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=10838; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:10838 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=10843; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:10843 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=15432; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:15432 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=22867; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:22867 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=22873; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:22873 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=22878; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:22878 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=23612; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:23612 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Obsolete/Obsolete.cs; Line=32252; LikelySystem=System:Obsolete; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Obsolete/Obsolete.cs:32252 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/GuardedRegion.cs; Line=32; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/GuardedRegion.cs:32 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/GuardedRegion.cs; Line=37; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/GuardedRegion.cs:37 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/GuardedRegion.cs; Line=42; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/GuardedRegion.cs:42 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=378; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:378 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=383; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:383 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=388; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:388 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=393; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:393 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=398; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:398 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=403; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:403 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=408; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:408 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Regions/Spawning/SpawnEntry.cs; Line=413; LikelySystem=System:Regions; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Regions/Spawning/SpawnEntry.cs:413 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;EventSink;Initialize;OnMovement;OnSpeech;PacketHandlers.Register;RegionOverride;Timer.DelayCall;WorldSave;CustomTimerSubclass;Initialize;CustomTimerSubclass;Initialize;OnSpeech;RegionOverride;CustomTimerSubclass;Timer.DelayCall;EventSink;Initialize;OnLogin;OnSpeech;RegionOverride;Initialize;Timer.DelayCall;OnSpeech;OnSpeech;RegionOverride;RegionOverride;Timer.DelayCall

## Serialized State

Serialized marker files: 6. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Maps; Notoriety; PvP Consent; Government; housing

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/codebase-audit/outputs/system-cards/regions.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
