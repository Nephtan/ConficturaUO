# Player Government System Guide

This page documents the in-game `GovHelpGump` guide and the government mechanics it references. It is a guide-layer document: the full city object model, civic structure catalog, and region behavior remain covered by the broader Government System page.

## Verified Source Files

| Script | RunUO Type | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/Government System/Commands/GovHelp.cs` | Command | Registers the player `[GovHelp` command and opens `GovHelpGump`. |
| `Data/Scripts/Custom/Government System/Gumps/GovHelpGump.cs` | Gump | Static 11-page player help Gump for city founding, taxes, elections, treasury, growth, war, allegiances, and city speech commands. |
| `Data/Scripts/Custom/Government System/Gumps/CityWarningGump.cs` | Gump | Sent after successful city hall placement; repeats the startup requirements and points players to `[GovHelp`. |
| `Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs` | Static service | Holds the guide-visible timers, city level thresholds, titles, city range offsets, and city relationship helpers. |
| `Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs` | Item | Persistent city controller, update timer, treasury, taxes, maintenance, citizens, mayor state, war/alliance lists, and city level updates. |
| `Data/Scripts/Custom/Government System/Items/Stones/CityVotingStone.cs` | Item | Persistent election controller, runner slots, vote counts, voter list, and election timer. |
| `Data/Scripts/Custom/Government System/Regions/PlayerCityRegion.cs` | Region | City speech keyword handling, housing permission, banned-player attack state, and entry/exit messages. |

## Entry Points

| Entry | Access | Parameters | Behavior |
| --- | --- | --- | --- |
| `[GovHelp` | `AccessLevel.Player` | None | Sends a new `GovHelpGump` to `e.Mobile`. |
| `[CityUpdate` | `AccessLevel.Administrator` | Target a `CityManagementStone` | Forces `DoUpdate()` on the targeted city stone and restarts its update timer. |
| `[FindCities` | `AccessLevel.GameMaster` | None | Opens `FindCitiesGump` for city lookup. |
| `[AdminAdd` | `AccessLevel.GameMaster` | None | Opens `AdminAddGump` for staff city membership maintenance. |
| `[GovTesting [on <multiplier>|off]` | `AccessLevel.GameMaster` | Optional mode and multiplier | Reads or changes government testing mode; adjusted timers are divided by the configured multiplier. |
| `[UpgradeCitySystem` | `AccessLevel.Administrator` | None | Runs the city-system upgrade routine if `FileVersion` is older than `SystemVersion`. |
| `[MoveVendor` / `[MoveBox` | `AccessLevel.Player` | None | Moves a city vendor or resource box when the caller is the mayor and is in the city/market region. |

## Guide Gump Layout

`GovHelpGump` is constructed at `(50, 50)`, uses `AddPage(0)` for the menu, and uses `GumpButtonType.Page` buttons to switch to pages 1 through 11. It does not override `OnResponse`; all navigation is client-side page switching inside the same Gump packet.

| Page | Menu Label | Mechanics Referenced |
| --- | --- | --- |
| 1 | Starting Off | City hall deed purchase, city placement spacing, first citizen requirement, first-day failure condition, treasury funding, and weekly city updates. |
| 2 | Elections | City voting timer, two-runner limit, one vote per election, tie handling, null elections, and mayor succession. |
| 3 | Taxes | Income, property, and travel taxes; mayor withdrawal visibility; property-tax back-tax handling. |
| 4 | Treasury | Shared city gold balance, deposits, mayor withdrawals, and maintenance funding. |
| 5 | City Growth | Citizen thresholds, level titles, city region growth, lockdown growth, registration, moongates, vendors, and housing membership expectations. |
| 6 | Maintenance | Maintenance report and recurring upkeep costs for city features. |
| 7 | Waring | War department workflow and enemy notoriety behavior. The menu label and source text spell this as "Waring". |
| 8 | Allegiances | Alliance proposal workflow and allied-city notoriety behavior. |
| 9 | City Commands | Mayor speech commands: `I wish to lock this down`, `I wish to release this`, and `I ban thee`. |
| 10 | Misc. | Location and traffic advice only; no unique C# mechanics. |
| 11 | Credits | Static author/version labels: concept by RoninGT/Paige, coded by Avelyn, built on RunUO v2.2, version text `2.23`. |

## Configured Values Used By The Guide

| Setting | Current Code Value | Effect |
| --- | --- | --- |
| `SystemVersion` | `2.3` | Current government system version string. |
| `StartUpdate` | `24` hours, adjusted by `GovernmentTestingMode` | Initial city update/deadline after founding. |
| `CityUpdate` | `7` days, adjusted by `GovernmentTestingMode` | Recurring city update interval after the first update. |
| `VoteUpdate` | `14` days, adjusted by `GovernmentTestingMode` | Election interval for `CityVotingStone`. |
| `TreasuryAmount` | `150000` | Starting `CityManagementStone.CityTreasury`. |
| `CityRangeOffset` | `130` | City hall placement spacing message uses `CityRangeOffset * 2`, currently `260` tiles. |
| `MaxCitizensPerCity` | `100` | Hard cap checked before city join/sponsor requests. |
| `MaxBannedPerCity` | `50` | Hard cap for mayor city bans; `0` disables city banning. |
| `NeedsForensics` | `false` | If enabled, mayor eligibility requires Forensics. |
| `ForensicsRequirement` | `35.0` | Configured requirement value, but `CheckIfCanBeMayor()` currently hardcodes `35.0`. |

## Founding Flow

City hall deeds are bought from `CityManager` stock. The traced stock sells eight city hall deed variants, each for `250000` gold: Asian, Necro, Stone, FieldStone, Plaster, Wood, Marble, and Sandstone.

Double-clicking a `CityDeed` assigns a `CityPlacementTarget`. For city hall deed types, the target path checks housing permission, blocks Underworld placement unless `EnableUnderworld` is enabled, checks mayor eligibility, then calls `PlayerGovernmentSystem.PlaceCityHall()`.

Successful city hall placement calls `CityDeed.FinishPlacement()`. The city hall branch creates the city region, `CityManagementStone`, `CityVotingStone`, initial city structures, deletion list, war/alliance lists, citizen list, and default mayor state. The founding `PlayerMobile` is added to the citizens list, assigned to `PlayerMobile.City`, given `CityTitle = "Mayor"`, and has `ShowCityTitle = true`.

After placement, `PlaceCityHall()` sends `CityWarningGump`, which tells the founder to reach the `Level1` citizen requirement within 24 hours, add treasury funds, enable housing if desired, run for office, and use `[GovHelp`.

## City Growth

City level is recalculated on `CityManagementStone.DoUpdate()` from the current citizen count. If the count is lower than `Level1` or the mayor is missing, the city is marked for disbanding.

| Level | Citizen Count Condition | Title | Region Offset | Approx. Region Size | Max Lockdowns |
| --- | --- | --- | --- | --- | --- |
| 1 | `6` to `11` | `outpost` | `25` | `50x50` | `100` |
| 2 | `12` to `17` | `village` | `38` | `76x76` | `200` |
| 3 | `18` to `23` | `township` | `50` | `100x100` | `300` |
| 4 | `24` to `29` | `city` | `75` | `150x150` | `400` |
| 5 | `30` to `35` | `metropolis` | `88` | `176x176` | `500` |
| 6 | `36+` | `empire` | `100` | `200x200` | `600` |

When the level changes, the stone rebuilds its rectangle around `Center`, updates `MaxDecore`, calls `UpdateCityStructures(level)`, refreshes the `PlayerCityRegion`, checks city vendors, and verifies addons/lockdowns still sit inside the city.

## Elections

`CityVotingStone` starts an election timer at `DateTime.Now + PlayerGovernmentSystem.VoteUpdate`. Citizens, the mayor, and Game Masters can open `VotingStoneGump` when within 2 tiles.

The Gump supports two runner slots. Button `3` runs for office if `CheckIfCanBeMayor(from)` passes. Buttons `1` and `2` vote for runner one or runner two; the current `Mobile` can only appear once in `Voters`.

When the timer ticks, `TallyVotes()` keeps the current mayor if either runner slot is empty, if either runner is no longer in the city, or if vote counts tie. Otherwise it assigns the winning `PlayerMobile` as `CityManagementStone.Mayor`, sets the winner title to `Mayor`, and demotes the other runner title to `Citizen`.

If `NeedsForensics` is enabled, voting for a runner performs a Forensics skill check on that runner and may add `0.1` base Forensics while the runner is below `100.0`.

## Taxes And Treasury

| Tax / Treasury Action | Code Path | Limits / Behavior |
| --- | --- | --- |
| Income tax | `CityTaxesIncomePrompt` | Accepts `0` through `100`. Updates `CityManagementStone.IncomeTax` and every current `CityPlayerVendor.TaxRate`. |
| Property tax | `CityTaxesPropertyPrompt` | Mayor Gump only opens this at city level `3+`. Accepts `0` through `10000`. Collected from each citizen bank account during city update. Failed payment sets `PlayerMobile.OwesBackTaxes` and increases `BackTaxesAmount`. |
| Travel tax | `CityTaxesTravelPrompt` | Mayor Gump only opens this at city level `3+`. Accepts `0` through `10000`. Moongate toll Gumps withdraw incoming/outgoing taxes from the traveler's bank account and add them to the relevant city treasuries. |
| Treasury deposit | `CityTreasuryDepoistPrompt` | Mayor or citizen enters a positive integer. The amount is withdrawn through `Banker.Withdraw()` and added to `CityTreasury`. |
| Treasury withdrawal | `CityTreasuryWithdrawPrompt` | Mayor enters a positive integer lower than the current treasury. `1..4999` creates gold in the backpack; `5000..1000000` creates a `BankCheck`. All citizens are notified. |

Income tax collection happens after maintenance during `DoUpdate()`. For each `CityPlayerVendor`, the city adds `vend.IncomeTax` to `CityTreasury`, notifies the vendor owner, and resets `vend.IncomeTax` to `0`.

## Maintenance Formula

`MaintenanceReportGump` and `CityManagementStone.CalculateMaintenance()` use the same visible cost formula.

| Cost Component | Formula |
| --- | --- |
| City hall | `5000 * Level` |
| Citizens | `Citizens.Count * 2` |
| Locked-down decor | `CurrentDecore * 3` |
| Addons | `AddOns.Count * 100` |
| Guarded city | `1000` if enabled |
| Registered city | `1000` if enabled |
| Bank | `1000` if present |
| Tavern | `1000` if present |
| Healer | `1000` if present |
| Moongate | `1000` if present |
| Stable | `1000` if present |
| Market | `1000` if present |
| Gardens | `Gardens.Count * 1000` |
| Parks | `Parks.Count * 1000` |

If `CityTreasury` is at least the calculated cost, the cost is subtracted. If the treasury cannot cover the cost, citizens are notified, locked-down items are made movable, and the city stone deletes itself.

## War, Allegiance, And City Speech

War and allegiance actions are driven from `CityManagementGump` and related list Gumps. War declarations add the target city to the caller's `WarsDeclared` and add the caller city to the target's `WarsInvited`. Accepting a war invitation moves the relationship into both cities' `Waring` lists. Allegiances use parallel `AllegiancesDeclared`, `AllegiancesInvited`, and `Allegiances` lists.

`PlayerGovernmentSystem.CheckAtWarWith()` returns true when both mobiles are `PlayerMobile` instances with cities and the caller city has the target city in `Waring`. `CheckCityAlly()` returns true when the target is in the caller's city citizens list or the target city is in `Allegiances`.

The notoriety system returns `Enemy` for banned or war targets and `Ally` for allied city targets. The harmful-action path also allows attacks against banned, war, and allied city targets, matching the guide's "friendly fire" note for allegiances.

Mayor speech inside `PlayerCityRegion` is keyword-driven:

| Speech | Required Speaker | Target / Effect |
| --- | --- | --- |
| `I wish to lock this down` | Mayor | Assigns `CityLockDownTarget`. Target item must be inside the caller's city and movable; successful lock sets `Movable = false`, adds to `isLockedDown`, and increments `CurrentDecore`. |
| `I wish to release this` | Mayor | Assigns `CityReleaseTarget`. If the item is in `isLockedDown`, it sets `Movable = true`, removes the item from the list, and decrements `CurrentDecore`. |
| `I ban thee` | Mayor | Assigns `CityBanTarget`. The target cannot be the mayor, staff, a house owner in the city, or exceed the banned-list cap. Banned players remain able to enter but are attackable by city citizens in and shortly after leaving the region. |

## Serialization Notes

`GovHelpGump`, `CityWarningGump`, and `GovHelpCommand` do not serialize any state.

`CityManagementStone` serializes version `6`. The stream writes addon list, market flag, region rectangles, voting stone, assistant mayor, resurrection stone, fees, update time, mayor, city rectangles, flags, level, city name, deletion list, citizens, taxes, treasury, rules, URL, center, moongate location, sponsored citizens, lockdowns, decor counts, registration flag, banned list, war lists, allegiance lists, feature flags, gardens, parks, and vendors. Deserialization rebuilds the update timer, ensures the stone is in `PlayerGovernmentSystem.AllCityStones`, and calls `UpdateRegion()`.

`CityVotingStone` serializes version `0`: election end time, two runner mobiles, vote counts, voter mobile list, and linked `CityManagementStone`. Deserialization restarts the election timer.

`CityDeed` serializes version `2`: civic structure type, offset, and multi ID.

## Known Issues

- `GovHelpGump` uses `GumpButtonType.Page` navigation and has no `OnResponse()` branch, no cancellation handling, and no `CloseGump()` call. That is low-risk for a static help Gump, but it allows duplicate guide Gumps to stack and prevents server-side response validation.
- `[GovHelp` directly dereferences `e.Mobile` without a null/deleted guard before sending the Gump.
- The City Growth guide says a city can span up to `250x250`, but current code reaches level 6 with `L6CLOffset = 100`, i.e. the configured table describes `200x200`.
- The Taxes guide presents the tax range as `0` to `10000`, but income tax is capped at `100`; only property and travel taxes use the `10000` cap.
- The maintenance comment in `CalculateMaintenance()` is stale: it claims `City Hall = 10000 x Level` and lists varied feature costs, while the live code and report use `5000 * Level` and `1000` for each feature flag.
- `CityManagementGump` reads `info.GetTextEntry(1).Text` for city rules without checking whether text entry `1` exists.
- `CityTreasuryWithdrawPrompt` rejects withdrawing the exact full treasury amount, adds gold/checks directly to the backpack without hold/drop validation, and notifies citizens after the item creation path.
- The hardcoded help text contains several spelling/encoding artifacts, including `Waring`, `Verison`, and a mojibake character in the Taxes page.
