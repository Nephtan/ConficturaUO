# System: Vendor Core

## Classification

SharedService

## Summary

Vendor framework, shop buy/sell info, and economy surface.

## Source Files

Matched source files: 111.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Government System/Items/Misc/CityVendorDismissTimer.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Custom/Government System/Items/Misc/MoveVendorTarget.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/LandLord.cs | StartupWiring | Initialize;OnSpeech | Yes |  |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/MallToken.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/SBLandLord.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Vendor/CityContractOfEmployment.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityLandLord.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Government System/Vendor/CityManager.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs | CommandSurface | Initialize;OnSpeech | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityRentedVendor.cs | StartupWiring | CustomTimerSubclass | Yes | SendGump |
| Data/Scripts/Custom/Government System/Vendor/SBCityLandLord.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Vendor/SBCityManager.cs | Economy |  | No |  |
| Data/Scripts/Custom/Mobiles/Vendors/Gort.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Mobiles/Vendors/Siddo.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Mobiles/Vendors/SirGideon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Offline Skill Training/Vendors/SBStudyBookbinder.cs | PlayerProgression |  | No |  |
| Data/Scripts/Custom/Offline Skill Training/Vendors/StudyBookbinder.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/XMLSpawner/XmlMobiles/TalkingBaseVendor.cs | StartupWiring | Initialize | Yes |  |
| Data/Scripts/Items/Deeds/VendorRentalContract.cs | StartupWiring | CustomTimerSubclass | Yes | SendGump |
| Data/Scripts/Items/Houses/AdvertiserVendor.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/HouseSign.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Items/Misc/PlayerVendorDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Base/BaseVendor.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Base/Behavior.cs | StartupWiring | CustomTimerSubclass;EventSink;OnSpeech;Timer.DelayCall;WorldLoad | Yes | SendGump |
| Data/Scripts/Mobiles/Base/PlayerVendor.cs | StartupWiring | CustomTimerSubclass;OnSpeech;Timer.DelayCall | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Base/RentedVendor.cs | StartupWiring | CustomTimerSubclass | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Base/StoreSalesList.cs | MobileContent |  | No |  |
| Data/Scripts/Mobiles/Base/VendorInventory.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Actor.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Alchemist.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/AnimalTrainer.cs | GumpUI | OnSpeech | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Architect.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Armorer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Artist.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Baker.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Banker.cs | GumpUI | OnSpeech | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Bard.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Barkeeper.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Beekeeper.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Blacksmith.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Bowyer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Butcher.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Carpenter.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Cobbler.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Cook.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/CustomHairstylist.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Druid.cs | GumpUI | OnMovement | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/DruidTree.cs | GumpUI | OnMovement | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Elementalist.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Enchanter.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/EtherealDealer.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Farmer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Fighter.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Fisherman.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Furtrader.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Glassblower.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/GolemCrafter.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/GypsyAnimalTrainer.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/GypsyBanker.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/GypsyLady.cs | GumpUI | OnMovement | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/GypsyMaiden.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/HairStylist.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Herbalist.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/HolyMage.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/InnKeeper.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/IronWorker.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Jester.cs | GumpUI | OnMovement | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Jeweler.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/KeeperOfChivalry.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/KungFu.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/LeatherWorker.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Lumberjack.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Mage.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Mapmaker.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Miller.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Miner.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Minter.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Monk.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/NecroMage.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Necromancer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Provisioner.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Rancher.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Ranger.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/RealEstateBroker.cs | Persistence | OnMovement;OnSpeech | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Sage.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Scribe.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Shepherd.cs | GumpUI | OnMovement;OnSpeech | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/StoneCrafter.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Tailor.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Tanner.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/TavernKeeper.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Thief.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Tinker.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Undertaker.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Vagabond.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/VarietyDealer.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Veterinarian.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Waiter.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Civilized/Vendors/Weaponsmith.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Weaver.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Mobiles/Civilized/Vendors/Witches.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/System/Gumps/PlayerVendorGumps.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/System/Gumps/ReclaimVendorGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/System/Gumps/VendorInventoryGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/System/Gumps/VendorRentalGumps.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/System/Obsolete/Obsolete.cs | PacketNetwork | CustomTimerSubclass;EventSink;Initialize;OnMovement;OnSpeech;PacketHandlers.Register;RegionOverride;Timer.DelayCall;WorldSave | Yes | OnResponse;SendGump |
| Data/System/Source/BaseVendor.cs | StartupWiring |  | No |  |
| Data/System/Source/Interfaces.cs | StartupWiring |  | No |  |
| Data/System/Source/Network/Packets.cs | StartupWiring |  | No |  |

## Data Files

myrunuodb_{0}_{1}.txt;myrunuodb_{0}.txt

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs; Line=68; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs:68 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs; Line=73; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs:73 |
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

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;EventSink;Initialize;OnMovement;OnSpeech;PacketHandlers.Register;RegionOverride;Timer.DelayCall;WorldSave;CustomTimerSubclass;EventSink;OnSpeech;Timer.DelayCall;WorldLoad;CustomTimerSubclass;OnSpeech;Timer.DelayCall;CustomTimerSubclass;Timer.DelayCall;Initialize;Initialize;OnSpeech;OnMovement;OnMovement;OnSpeech;OnSpeech

## Serialized State

Serialized marker files: 97. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Economy; crafting; Government; NPC mobiles; banking

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Shoppes_Vendors.md;docs/wiki/Vendor_Core_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
