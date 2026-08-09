# System: Housing

## Classification

SharedService

## Summary

Housing framework, placement, ownership, remodeling, and house-related item surfaces.

## Source Files

Matched source files: 94.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Items/Decorations/HouseSign.cs | GumpUI |  | Yes | OnResponse |
| Data/Scripts/Items/Doors/HouseDoors.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/AdvertiserVendor.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/BaseHouse.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Items/Houses/CircusTentsEast.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/CircusTentsSouth.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/ComponentVerification.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Deeds.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/GypsyCampAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/HouseFoundation.cs | CommandSurface | EventSink;Initialize;Timer.DelayCall | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/HousePlacement.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/HousePlacementTool.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/Houses.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/HouseSign.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Items/Houses/HouseTeleporter.cs | StartupWiring | CustomTimerSubclass | Yes |  |
| Data/Scripts/Items/Houses/InteriorDecorator.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/MagicalRope.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Mailbox.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/MovingCrate.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/NewHouses.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/PlayersHouseTeleporter.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/PlayersZTeleporter.cs | StartupWiring | Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/PreviewHouse.cs | StartupWiring | CustomTimerSubclass;Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/SmallTents.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/TavernTable.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Houses/TentsEast.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/TentsSouth.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Decorate.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Items/Houses/Monopoly/RUOVersion.cs | CommandSurface |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Error Reporting/Errors.cs | StartupWiring | EventSink;Initialize;OnLogin | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Error Reporting/Gumps/ErrorsGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Error Reporting/Gumps/ErrorsNotifyGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/BackgroundPlus.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/ButtonPlus.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/GumpPlusLight.cs | StartupWiring | Timer.DelayCall | No | OnResponse;SendGump |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/HTML.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/HtmlPlus.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/Gumps Plus Light/InfoGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/TownHouse Gumps/ContractConfirmGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/TownHouse Gumps/ContractSetupGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/TownHouse Gumps/TownHouseConfirmGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/TownHouse Gumps/TownHouseSetupGump.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Gumps/TownHouse Gumps/TownHousesGump.cs | StartupWiring | Initialize | No | SendGump |
| Data/Scripts/Items/Houses/Monopoly/Items/RentalContract.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Items/RentalContractCopy.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Items/RentalLicense.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Items/SignHammer.cs | StartupWiring | Initialize | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Items/TownHouse.cs | StartupWiring | OnSpeech;Timer.DelayCall | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Items/TownHouseSign.cs | StartupWiring | Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Items/Houses/Monopoly/Misc/CommandInfo.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Monopoly/Misc/DecoreItemInfo.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Monopoly/Misc/General.cs | StartupWiring | EventSink;OnLogin;OnSpeech;WorldSave | No |  |
| Data/Scripts/Items/Houses/Monopoly/Misc/GumpResponse.cs | PacketNetwork | Initialize;PacketHandlers.Register;Timer.DelayCall | No | OnResponse;SendGump |
| Data/Scripts/Items/Houses/Remodeling/ContextMenuEntries.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Items/Houses/Remodeling/LawnGate.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/LawnItem.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Items/Houses/Remodeling/LawnRegistry.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Remodeling/LawnStair.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/LawnSystem.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Items/Houses/Remodeling/LawnTarget.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Items/Houses/Remodeling/LawnTools.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/Remodeling.cs | StartupWiring | WorldSave | No |  |
| Data/Scripts/Items/Houses/Remodeling/ShantyDoor.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/ShantyItem.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Items/Houses/Remodeling/ShantyRegistry.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Houses/Remodeling/ShantyStair.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/ShantySystem.cs | StartupWiring | Timer.DelayCall | No |  |
| Data/Scripts/Items/Houses/Remodeling/ShantyTarget.cs | GumpUI |  | No | SendGump |
| Data/Scripts/Items/Houses/Remodeling/ShantyTools.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Houses/Remodeling/Gumps/ConfirmRemoveGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Items/Houses/Remodeling/Gumps/LawnGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Items/Houses/Remodeling/Gumps/LawnSecurityGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Items/Houses/Remodeling/Gumps/ShantyGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Items/Houses/Remodeling/Gumps/ShantySecurityGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Items/Special/MiniHouses.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Special/Heritage Items/HouseLadder.cs | GumpUI |  | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Special/Holiday/GingerBreadHouseDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Special/House Raffle/HouseRaffleDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Special/House Raffle/HouseRaffleRegion.cs | ItemContent |  | No |  |
| Data/Scripts/Items/Special/House Raffle/HouseRaffleStone.cs | StartupWiring | Initialize;Timer.DelayCall | Yes | SendGump |
| Data/Scripts/Items/Special/House Raffle/Gumps/HouseRaffleManagementGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Items/Special/Veteran Rewards/ContestMiniHouse.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Fishing/Sea Artifacts/LightHouse.cs | Persistence |  | Yes |  |
| Data/Scripts/Mobiles/Base/StoreSalesList.cs | MobileContent |  | No |  |
| Data/Scripts/Mobiles/Civilized/Citizens/HouseVisitor.cs | Persistence |  | Yes |  |
| Data/Scripts/System/Commands/Commands/Commands.cs | StartupWiring | Initialize | No | SendGump |
| Data/Scripts/System/Gumps/ConfirmHouseResize.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/System/Gumps/HouseDemolishGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/System/Gumps/HouseGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/System/Gumps/HouseGumpAOS.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/System/Gumps/HouseTransferGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/System/Gumps/ViewHousesGump.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/System/Regions/HouseRegion.cs | StartupWiring | EventSink;Initialize;OnLogin;OnSpeech;RegionOverride | No | SendGump |
| Data/Scripts/System/Regions/UnderHouseRegion.cs | RegionPolicy | RegionOverride | No |  |

## Data Files

*.cfg;Data/System/Components/doors.txt;Data/System/Components/floors.txt;Data/System/Components/misc.txt;Data/System/Components/roof.txt;Data/System/Components/stairs.txt;Data/System/Components/teleprts.txt;Data/System/Components/walls.txt;dsd_exceptions.txt

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Items/Houses/HouseFoundation.cs; Line=974; LikelySystem=Items:Houses; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Items/Houses/HouseFoundation.cs:974 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Items/Houses/Monopoly/Decorate.cs; Line=14; LikelySystem=Items:Houses; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Items/Houses/Monopoly/Decorate.cs:14 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=OnCommand; File=Data/Scripts/Items/Houses/Monopoly/RUOVersion.cs; Line=29; LikelySystem=Items:Houses; RegistrationLine=Server.Commands.CommandSystem.Register(com, acc, new CommandEventHandler(OnCommand));}.Command) | Data/Scripts/Items/Houses/Monopoly/RUOVersion.cs:29 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/System/Gumps/ViewHousesGump.cs; Line=17; LikelySystem=System:Gumps; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/System/Gumps/ViewHousesGump.cs:17 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;Timer.DelayCall;EventSink;Initialize;OnLogin;EventSink;Initialize;OnLogin;OnSpeech;RegionOverride;EventSink;Initialize;Timer.DelayCall;EventSink;OnLogin;OnSpeech;WorldSave;Initialize;Initialize;PacketHandlers.Register;Timer.DelayCall;Initialize;Timer.DelayCall;OnSpeech;Timer.DelayCall;RegionOverride;Timer.DelayCall;WorldSave

## Serialized State

Serialized marker files: 49. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

Regions; items; vendors; player ownership; gumps

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Housing_System.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
