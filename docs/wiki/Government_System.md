# Government System

## Overview
The Government System is a player-city package centered on `CityDeed`, `CityManagementStone`, `CityVotingStone`, `PlayerCityRegion`, the civic-structure deeds, and supporting gumps/prompts. A placed city hall creates the city region, the management stone, the voting stone, and the initial mayor/citizen state. Weekly city updates handle membership validation, mayor succession, level resizing, taxes, maintenance, and market vendor refreshes.

## Core Components
- `PlayerGovernmentSystem.cs`: global configuration, city placement checks, map caps, mayor/citizen helper checks, and upgrade/version-file verification.
- `Items/Deeds/CityDeed.cs`: builds city halls and civic structures, creates stones/regions, and links the city hall to the mayor.
- `Items/Stones/CityManagementStone.cs`: the main city controller, update timer, treasury/tax state, war/alliance state, lockdown state, and versioned persistence.
- `Items/Stones/CityVotingStone.cs`: two-candidate election stone with a 14-day timer and vote tally logic.
- `Regions/PlayerCityRegion.cs`: housing permission, mayor-only speech commands, ban attack rules, entry/exit messaging, and recall/gate blocking for players.
- `Commands/*.cs`: `GovHelp`, `AdminAdd`, `FindCities`, `GovTesting`, and `UpgradeCitySystem`.

## Founding A City
- A mayor places a city hall deed with the standard RunUO housing placement check.
- City hall placement is blocked if the map is disabled for cities, the facet has reached its map cap, or another guarded region or player city is found in the placement scan.
- Underworld city placement is disabled by default through `PlayerGovernmentSystem.EnableUnderworld = false`.
- New cities start with:
  - Treasury: `150000`
  - Update timer: first update in `24` hours
  - Election timer: first election in `14` days
  - Default name: `<mayor name>'s City`
  - One citizen: the founder
  - One vote stone linked to the management stone
- The founder must qualify as mayor. If `NeedsForensics` is enabled, the current code requires at least `35.0` Forensics to found or run for mayor.

## City Levels And Bounds
City level is recalculated on each city update from the number of current citizens:

| Level | Citizens Required | Rank Title | Region Size | Lockdowns |
| --- | --- | --- | --- | --- |
| 1 | 6 | outpost | 50x50 | 100 |
| 2 | 12 | village | 76x76 | 200 |
| 3 | 18 | township | 100x100 | 300 |
| 4 | 24 | city | 150x150 | 400 |
| 5 | 30 | metropolis | 176x176 | 500 |
| 6 | 36 | empire | 200x200 | 600 |

Notes:
- Cities disband on update if they fall below 6 citizens or lose their mayor with no valid replacement path.
- Membership is not automatic from house ownership. A player must still be added to the city, but house ownership inside the city is required to remain a member.
- All characters on the same account can qualify through house ownership, and each joined character counts toward population.

## Membership, Mayors, And Elections
- The mayor manages the city through `CityManagementGump`; citizens get a reduced `CityCitizenGump`.
- Citizens can be added only if they are not already a citizen or mayor of another city, are not banned from the target city, and still qualify through house ownership.
- Assistant mayor selection exists in the management gump.
- Election rules in code:
  - Elections run every `14` days.
  - Only two runners can register per election.
  - Each voter can vote once.
  - Ties keep the current mayor in office.
  - If fewer than two runners exist, the current mayor stays in office.
- Succession rules in code:
  - If the mayor is invalid but the assistant mayor is still valid, the assistant mayor immediately becomes mayor.
  - If neither a valid mayor nor a valid assistant mayor exists, a random citizen becomes mayor and the vote stone is restarted for a forced election in `1` day.

## Treasury, Taxes, And Maintenance
- Citizens and the mayor can deposit treasury gold from their bank account.
- Only the mayor can withdraw treasury funds.
  - `1` to `4999` gold is withdrawn as loose gold.
  - `5000` to `1000000` gold is withdrawn as a bank check.
  - Citizens are notified whenever the mayor withdraws.
- Tax configuration:
  - Income tax: `0` to `100`, applied as a percentage to `CityPlayerVendor.TaxRate`.
  - Property tax: `0` to `10000`, collected from each citizen bank account on city update.
  - Travel tax: `0` to `10000`, charged on civic moongate travel to non-citizens.
- Unpaid property tax marks the player with back taxes instead of immediately ejecting them.

### Actual Maintenance Formula
The compiled maintenance calculation is:
- `5000 * city level`
- `2 * citizen count`
- `3 * locked-down item count`
- `100 * addon count`
- `1000` each for:
  - guards
  - registration
  - bank
  - tavern
  - healer
  - moongate
  - stable
  - market
- `1000 * garden count`
- `1000 * park count`

If the treasury cannot pay that cost during `DoUpdate()`, the city disbands and locked-down city items are made movable again.

## Civic Structures And Unlocks
All civic structure deeds must be in the mayor's backpack, must be placed by the mayor while standing inside their own city, and usually require the city to be at or above a specific level.

Representative unlocks:
- Bank: level 2, optional `40.0` Forensics
- Moongate: level 3, optional `55.0` Forensics
- Market: level 3, optional `60.0` Forensics
- Healer: level 4, optional `60.0` Forensics
- Small park: level 4, optional `70.0` Forensics
- Stable: level 5, optional `80.0` Forensics
- Tavern: level 6, optional `95.0` Forensics

Other notes:
- Registration is toggled in the city management gump, but public city-gate listing only happens when the city is both registered and already has a civic moongate.
- Market placement creates a `CityLandLord` via `PremiumSpawner` and can seed random vendors on update.
- Healer structures enable resurrection and corpse-retrieval fees through the city healer stone.

## Region Rules
`PlayerCityRegion` applies these city-specific behaviors:
- Only the mayor can use the speech keywords:
  - `I wish to lock this down`
  - `I wish to release this`
  - `I ban thee`
- Banned players cannot place houses in the city.
- City citizens may attack banned players inside the city.
- After a banned player leaves the city, they remain attackable by that city's citizens for 5 minutes.
- Player recall and gate travel spells are blocked inside player cities.

## Moongates, Banking, And Healer Services
- `PCMoongateGump` lists only cities with both `HasMoongate == true` and `IsRegistered == true`.
- Travel tax can be charged from either the origin city or the destination city when the traveler is not a citizen of that city.
- `CityBankStone` supports banker speech keywords such as withdraw, balance, bank, and check.
- `CityResurrectionStone` supports:
  - resurrection fee: `0` to `500`
  - corpse retrieval fee: `0` to `1000`
  - up to 3 corpse retrievals per player per update cycle, reset when the city update clears ghost usage

## Commands
| Command | Access | Behavior |
| --- | --- | --- |
| `GovHelp` | Player | Opens the legacy in-game help gump. |
| `AdminAdd` | GameMaster | Opens the item creation gump for government-system pieces. |
| `FindCities` | GameMaster | Opens a finder gump for existing cities. |
| `GovTesting [on <multiplier>|off]` | GameMaster | Enables or disables accelerated timer math for the system. |
| `UpgradeCitySystem` | Administrator | Runs the versioned city-upgrade pass and writes `Data/GovernmentVersion.xml`. |
| `CityUpdate` | Administrator | Forces `DoUpdate()` on a targeted city management stone. |

## Persistence
- `CityManagementStone` serializes at version `6`.
- `CityVotingStone` serializes at version `0`.
- `CityDeed` serializes at version `2`.
- `CityResurrectionStone` serializes at version `1`.

## Code-Verified Rework Notes
- `PlayerGovernmentSystem.ForensicsRequirement` is declared as a configurable value, but `CheckIfCanBeMayor()` still hard-codes `35.0` Forensics. Mayor eligibility is therefore not actually driven by the configuration field.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0018.
- Backlog rows: RB-06697.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs (CurrentFile)
- Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs (CurrentFile)
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs (CurrentFile)
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs (CurrentFile)
- Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs (CurrentFile)
- Data/Scripts/Custom/Government System/Commands/GovHelp.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=2; Gump=7; Initialize=3; Region=3; Speech=2; Timer=2.
- Data/Scripts/Custom/Government System/Commands/GovHelp.cs:L14 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Government System/Commands/GovHelp.cs:L16 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/Government System/Commands/GovHelp.cs:L27 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L59 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L105 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L463 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L465 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L527 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L564 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:L2112 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs:L94 Gump SendGump access=Internal
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs:L302 Timer CustomTimerSubclass access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 3.
- Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs:Server.Items.CityDeed version=2 serialize=L3619 deserialize=L3632 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:Server.Items.CityManagementStone version=6 serialize=L1918 deserialize=L2010 alignment=CountMatchNeedsTypeReview:UnknownWrites=27
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs:Server.Items.CityVotingStone version=0 serialize=L261 deserialize=L276 alignment=CountMatchNeedsTypeReview:UnknownWrites=5

### Project And Runtime Coverage

- Data/Scripts/Custom/Government System/Commands/GovHelp.cs=Keep
- Data/Scripts/Custom/Government System/Commands/GovHelp.cs=Keep
- Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs=Keep
- Data/Scripts/Custom/Government System/Items/Deeds/CityDeed.cs=Keep
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs=Keep
- Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs=Keep
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs=Keep
- Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs=Keep
- Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs=Keep
- Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs=Keep
- Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs=Keep
- Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
