# System: Government

## Classification

GameplayLayer

## Summary

Persistent civic gameplay system for cities, elections, taxes, treasuries, bans, wars, vendors, and civic structures.

## Source Files

Matched source files: 217.

| File | Primary Role | Runtime Hooks | Serialized | Gumps |
| --- | --- | --- | --- | --- |
| Data/Scripts/Custom/Government System/CityEnums.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/CityUpgradeSystem.cs | CommandSurface | Initialize;WorldSave | No |  |
| Data/Scripts/Custom/Government System/GovernmentTestingMode.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/InternalChatMsg.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs | StartupWiring | Initialize | No | SendGump |
| Data/Scripts/Custom/Government System/Commands/AdminAdd.cs | CommandSurface | Initialize | No | SendGump |
| Data/Scripts/Custom/Government System/Commands/FindCities.cs | CommandSurface | Initialize | No | SendGump |
| Data/Scripts/Custom/Government System/Commands/GovernmentTestingCommand.cs | CommandSurface | Initialize | No |  |
| Data/Scripts/Custom/Government System/Commands/GovHelp.cs | CommandSurface | Initialize | No | SendGump |
| Data/Scripts/Custom/Government System/Gumps/AcceptJoinGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/AdminAddGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/AssistMayorGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/CancelAllegiancesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ChangeCitizenTitleGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/CityCitizenGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/CityCorpseFeeGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/CityManagementGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/CityMarketGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/CityResFeeGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/CityWarningGump.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Gumps/ConfirmDisbandCityGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ConfirmLeaveCityGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/DeclareCityAllegiancesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/DeclareCityWarGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/DeclarePeaceGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/DestoryStructureGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/FindCitiesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/GovHelpGump.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Gumps/MaintenanceReportGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/NoCitiesGump.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Gumps/PCMoongateGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/PCMoongateToll2Gump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/PCMoongateTollGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/RemoveCityBanGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/RemoveMemberGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ResourceBoxGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/SponsorGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Gumps/ViewCityAllegiancesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewCityAtWarGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewCityBansGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewCityMembersGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewDeclaredAllegiancesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewDeclaredWarsGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewInvitedAllegiancesGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewInvitedWarsGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/ViewSponsoredGump.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Gumps/VotingStoneGump.cs | GumpUI |  | No | OnResponse |
| Data/Scripts/Custom/Government System/Items/Civic Decore/CityHedge.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Decore/CityLampPost.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Decore/CityTrashBarrel.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/AsianCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/FieldstoneCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/MarbleCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/NecroCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/PlasterCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/SandstoneCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/StoneCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Banks/WoodCityBankAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/AsianCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/FieldStoneCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/MarbleCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/NecroCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/PlasterCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/SandstoneCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/StoneCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/City Halls/WoodCityHallAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/AsianCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/FieldstoneCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/MarbleCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/NecroCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/PlasterCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/SandstoneCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/StoneCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Healers/WoodCityHealerAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/AsianMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/FieldstoneMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/MarbleMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/NecroMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/PlasterMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/SandstoneMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/StoneMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Markets/WoodMarketAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/AsianCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/MarbleCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/NecroCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/SandstoneCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/StoneCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Moongates/WoodCityMoongateAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/LargeCityGardenAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/LargeCityParkAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/MedCityGardenAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/MedCityParkAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/SmallCityGardenAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Parks & Gardens/SmallCityParkAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/AsianCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/FieldstoneCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/MarbleCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/NecroCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/PlasterCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/SandstoneCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/StoneCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Stable/WoodCityStableAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/AsianCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/FieldstoneCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/MarbleCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/NecroCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/PlasterCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/SandstoneCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/StoneCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Civic Structures/Stock Buildings/Taverns/WoodCityTavernAddon.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/AsianCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/FieldstoneCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/MarbleCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/NecroCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/PlasterCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/SandstoneCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/StoneCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Banks/WoodCityBankDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/AsainCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/FieldStoneCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/MarbleCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/NecroCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/PlasterCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/SandstoneCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/StoneCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/City Halls/WoodCityHallDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/AsianCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/FieldstoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/MarbleCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/NecroCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/PlasterCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/SandstoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/StoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Healers/WoodCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/AsianCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/FieldstoneCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/MarbleCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/NecroCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/PlasterCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/SandstoneCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/StoneCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Markets/WoodCityMarketDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/AsianCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/MarbleCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/NecroCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/SandstoneCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/StoneCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Moongates/WoodCityMoongateDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/LargeCityGardenDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/LargeCityParkDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/MediumCityGardenDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/MediumCityParkDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/SmallCityGardenDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Parks & Gardens/SmallCityParkDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/AsainCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/FieldstoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/MarbleCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/NecroCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/PlasterCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/SandstoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/StoneCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Stable/WoodCityHealerDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/AsianCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/FieldstoneCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/MarbleCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/NecroCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/PlasterCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/SandstoneCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/StoneCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Deeds/Stock Deeds/Taverns/WoodCityTavernDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Misc/CityMallToken.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Misc/CityVendorDismissTimer.cs | StartupWiring | CustomTimerSubclass | No |  |
| Data/Scripts/Custom/Government System/Items/Misc/MoveVendorTarget.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Items/Signs/CivicSign.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Government System/Items/Signs/RecCivicSign.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Items/Stones/CityBankStone.cs | Persistence | OnSpeech | Yes |  |
| Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs | CommandSurface | CustomTimerSubclass;Initialize | Yes | SendGump |
| Data/Scripts/Custom/Government System/Items/Stones/CityResurrectionStone.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Government System/Items/Stones/CityStableStone.cs | GumpUI | OnSpeech | Yes | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs | StartupWiring | CustomTimerSubclass | Yes | SendGump |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/LandLord.cs | StartupWiring | Initialize;OnSpeech | Yes |  |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/MallToken.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/NPC Vendor Mall 2.1/SBLandLord.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Prompts/CitizenTitleChangePrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityCorpseFeePrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityNamePrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityResFeePrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityTaxesIncomePrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityTaxesPropertyPrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityTaxesTravelPrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityTreasuryDepoistPrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityTreasuryWithdrawPrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Prompts/CityURLPrompt.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Custom/Government System/Regions/CityMarketRegion.cs | RegionPolicy | RegionOverride | No |  |
| Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs | RegionPolicy | OnSpeech;RegionOverride | No |  |
| Data/Scripts/Custom/Government System/Resourcebox/CityResourceBox.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Resourcebox/CityResourceBoxDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Resourcebox/ResourceBox.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Government System/Resourcebox/StorageTypes.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Vendor/CityContractOfEmployment.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityLandLord.cs | GumpUI |  | Yes | SendGump |
| Data/Scripts/Custom/Government System/Vendor/CityManager.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs | CommandSurface | Initialize;OnSpeech | Yes |  |
| Data/Scripts/Custom/Government System/Vendor/CityRentedVendor.cs | StartupWiring | CustomTimerSubclass | Yes | SendGump |
| Data/Scripts/Custom/Government System/Vendor/SBCityLandLord.cs | Economy |  | No |  |
| Data/Scripts/Custom/Government System/Vendor/SBCityManager.cs | Economy |  | No |  |
| Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs | CommandSurface | Initialize | No | OnResponse;SendGump |
| Data/Scripts/Custom/Invasion System/Stones/GargoyleCityInvasionStone.cs | Persistence |  | Yes |  |
| Data/Scripts/Custom/Invasion System/Underworld/StartstoGargoyleCityIlshenar.cs | GumpUI |  | No | OnResponse;SendGump |
| Data/Scripts/Items/Books/DynamicBook.cs | StartupWiring | Timer.DelayCall | Yes | OnResponse;SendGump |
| Data/Scripts/Items/Magical/VelocityDeed.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Magical/Artifacts/Artifact_ArmsOfToxicity.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_ArmsOfToxicity.cs | Persistence |  | Yes |  |
| Data/Scripts/Items/Trades/Maps/CityMap.cs | Persistence |  | Yes |  |
| Data/System/Source/Network/Packets.cs | StartupWiring |  | No |  |
| Data/System/Source/Network/SendQueue.cs | StartupWiring |  | No |  |

## Data Files

Data/GovernmentVersion.xml

## Player Entry Points

| Entry | Evidence |
| --- | --- |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/CityUpgradeSystem.cs; Line=17; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/CityUpgradeSystem.cs:17 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Commands/AdminAdd.cs; Line=16; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Commands/AdminAdd.cs:16 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Commands/FindCities.cs; Line=16; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Commands/FindCities.cs:16 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Commands/GovernmentTestingCommand.cs; Line=16; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Commands/GovernmentTestingCommand.cs:16 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Commands/GovHelp.cs; Line=16; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Commands/GovHelp.cs:16 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs; Line=465; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:465 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs; Line=68; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs:68 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs; Line=73; LikelySystem=Custom:Government System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Government System/Vendor/CityPlayerVendor.cs:73 |
| Command $(Escape-MarkdownCell @{Command=; AccessLevel=; Handler=; File=Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs; Line=14; LikelySystem=Custom:Invasion System; RegistrationLine=CommandSystem.Register(}.Command) | Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs:14 |

## Staff Entry Points

| Entry | Evidence |
| --- | --- |
| None found in Phase 1 command markers | Review staff gumps and props in later phases. |

## Runtime Hooks

CustomTimerSubclass;CustomTimerSubclass;Initialize;Initialize;Initialize;OnSpeech;Initialize;WorldSave;OnSpeech;OnSpeech;RegionOverride;RegionOverride;Timer.DelayCall

## Serialized State

Serialized marker files: 148. See phase-01-serialization-marker-inventory.csv and Phase 6 for write/read maps.

## Dependencies

PlayerMobile; Regions; Housing; Vendors; Banking; city gumps

## Dependents

Not fully verified in Phase 4. Dependency and dependent edges are deferred to Phase 8.

## Synergies

Deferred to Phase 9 synergy and conflict matrix.

## Conflicts And Risks

- Verification status is $verificationStatus; this card is generated from marker inventories and requires deeper Phase 5/6 review.
- Project truth issues for matched files are tracked in project-truth-register.csv and project-cleanup-backlog.csv.

## Documentation

docs/wiki/Government_System.md;docs/wiki/Player_Government_System_Guide.md

## Verification Status

NeedsSerializationReview

## Follow-Up Work

- Review runtime hooks in Phase 5.
- Review serialized state in Phase 6 when serialization markers are present.
- Verify documentation source traces in Phase 7.
- Convert findings into Phase 13 repair backlog items where needed.
