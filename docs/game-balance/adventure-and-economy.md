# Adventure and economy

[Start here](README.md) · [Systems at a glance](systems-at-a-glance.md) · [Connections](connections-and-balance.md) · [Coverage](coverage-and-evidence.md)

Review date: 2026-09-27. Source baseline: `main` at `6568058a`.

## What this review establishes

This is a read-only source review with a documentation output. It describes executable paths and checked-in settings. It does not establish the deployed revision, saved item properties, actual spawn placement, player participation, trade prices, exploitation, or income per hour. No source, configuration, project, or save files were changed. No server was started and no gameplay was simulated.

The useful mental model is **adventure and production create goods; skills and guilds improve their yield; trade turns goods into gold; gold can buy permanent equipment improvement; stronger characters repeat profitable content faster**. Houses, collections, decoration, boats, and cities also turn wealth into convenience, ownership, and social goals. These are different rewards and need different balance targets.

The code already contains deliberate limits: resource banks, harvesting skills, recipe gates, cooldowns, equipment caps, upgrade costs, gold scaling, party reward rules, and city upkeep. Their existence does not establish that their combined results are balanced.

## At a glance

| System | Player loop | What the player keeps | Main limit or uncertainty | Evidence |
|---|---|---|---|---|
| Monster loot | Defeat creatures, identify/use/sell drops | Gold, randomized equipment, artifacts, materials | Creature-specific loot; damage rights; Luck; throughput | E01–E03 |
| NPC trade | Sell eligible items or buy supplies/services | Gold or goods | Price tables, Mercantile/Begging/guild rules; sellability | E04 |
| Player vendors | Offer goods to other players | Transferred gold; goods move to buyer | Demand, stock and house access; regular vendor charge is 1 gold | E05 |
| Shoppes | Own a specialist shop, fulfill generated orders, cash out | Created gold; trade skill progress | Resources, tools, order difficulty, timers and 500,000 stored-gold cap | E06 |
| Harvesting | Visit resource patches, use tool, repeat until stopped | Ore, logs, fish and related resources; skills | Resource-bank depletion/respawn, skill, special geography, tool and inventory state | E07 |
| Crafting | Convert resources into recipe output | Equipment, supplies, furnishings; skill | Recipe minima, success and exceptional chances, tool/resource use | E08 |
| Runic crafting | Spend limited-use special tools on equipment | Random magic properties | Material-specific property counts/intensities | E09 |
| Guild enhancement | Qualify for guild tool; pay to raise chosen properties | Permanent chosen equipment properties | Eligible item and property caps; gold; guild and skill gates | E10 |
| Bulk orders | Make specified quantities/quality/material, complete deed | Gold, tools, runics, crafting Power Scrolls | Deed types, completion requirements, reward points | E11 |
| Crops and homestead | Plant/maintain/harvest, prepare food/drink | Produce, ingredients, consumables, household goods | Space, recipe/material and skill requirements; growth and picking timers | E12–E13 |
| Hunger/thirst | Carry and consume supplies during activity | Restored food/drink meters | Periodic decay, Camping avoidance, racial/safe-area exemptions | E14 |
| Houses and storage | Acquire property, store goods, establish production and trade | Durable space, convenience, collection/display | Current settings remove house decay, house-count and floor-item decay pressures | E15 |
| Player government | Recruit citizens, pay upkeep, manage public services and taxes | City facilities, territory, shared funds | Population, space, weekly upkeep, election timing, saved state | E16–E17 |
| Bulletin/fishing quests | Accept generated job, complete, return | Gold, reputation and activity progression | Board availability, qualifying world targets, cooldowns | E18 |
| Artifact searches | Buy information, select artifact, follow clues | Chance at chosen artifact | Paid clue reliability, travel/search, completion cooldown | E19 |
| Museum/antiquities | Obtain antiques, sell to collectors | Gold; collection choices remain | Base antique value, Mercantile/Begging/guild multipliers | E20 |
| Treasure maps | Decode, navigate, excavate, defeat threats, open chest | Gold, relics, artifacts, occasional skill consumables | Map and skills, guardians/locks, chest lifetime and reward tier | E21 |
| Sailing/fishing/piracy | Sail, fish, salvage, find wrecks, gather cargo/bounties | Food, resources, valuables, Seafaring progression | Boat/geography/skill, encounters, reward bonuses | E22–E23 |
| Champions | Kill waves and boss, qualify for rewards | Power Scrolls, skull, gold; possible artifact | Spawn settings, wave timer, damage rights, reward sharing | E24 |
| Monster nests | Destroy nest, activate remains near players | Individual gold/check reward to nearby players | Nest properties and encounter placement; nearby population affects total payout | E25 |
| Random encounters | Explore eligible areas and trigger configured encounters | Loot and progression from selected creatures | Character level, XML pools, region and timer logic | E26 |
| Goliaths/bespoke bosses | Fight individually configured opponents | Creature-specific rewards | Concrete profile and spawn availability; no single universal boss schedule | E27 |
| Invasions/events | Staff starts configured waves/encounters | Creature/event rewards | Administrator command and saved/generated event state | E28 |
| Voting/referrals | Request voting page; qualify referral relationship | Voting: no material reward in traced path; referral: stat consumable | Voting cooldown; referral account/playtime checks and existing stat caps | E29–E30 |

## Money: creation, destruction, and transfer

These are different operations even when all show the player a gold amount:

- **Gold creation:** monster/chest/quest/cargo/antique/shoppes payouts and NPC purchases of player goods. New gold enters the economy.
- **Gold destruction:** NPC purchases by players and services that consume payment; guild enhancement; player-vendor upkeep; city maintenance. Whether a purchase is a permanent sink also depends on later refunds/resale.
- **Gold transfer:** player-to-player sales, deposits/withdrawals, player-city taxes and service charges credited to a city treasury. These do not remove gold merely because one player pays it.
- **Item destruction/conversion:** crafting ingredients, tool uses, food/drink, turn-in items and failed enhancement. This affects goods scarcity independently of gold.

There is no demonstrated single universal gold-rate switch. `GetGoldCutRate()` clamps the configured percentage to 5–100; checked-in value is **25**. It affects the paths that call it, including loot dice, cargo base values, museum book values, shoppes order generation, BOD gold and pirate bounties. Direct `new Gold(...)` paths can bypass it. Champion ground gold and nest rewards are two concrete examples. The setting's comment is a summary, not a complete call graph. [E02, E06, E11, E20, E23–E25]

**A balance implication:** reducing the percentage alone could make some activities relatively more profitable while leaving their direct rewards intact. Compare complete reward bundles per active hour and per elapsed hour before interpreting this setting as a global economy control.

### NPC trade and shoppes

`GenericSell.GetSellPriceFor` starts from the vendor's type table, applies item quality/material adjustments, halves the result, then multiplies by `1 + 0.03 × barter`, capped at barter 100. With an adjusted pre-halving value of 100, the final examples are 50 at barter 0, 125 at 50, and 200 at 100. `BaseVendor.OnSellItems` normally supplies Mercantile; matching NPC guild can raise it to 100, while an active Begging pose can substitute Begging when the guild benefit is not in use. Different item/service branches have separate rules. [E04]

This couples economic skills directly to adventuring income: the same loot can be worth substantially more to a specialist. It may encourage a merchant character, trade with other players, or training economic skills on a combat character. The source alone does not say which behavior players choose.

**Checked nonissue:** generic resale pricing uses `1.90 × GetSellPriceFor(item, 0)`, which looks suspicious beside the maximum barter multiplier. However, `GenericSell.IsResellable` returns `false`, preventing the ordinary generic sold-item buyback path. This formula pair alone does not prove an arbitrage exploit. Stock buy/sell price parity still requires an item-by-item table review. [E04]

Shoppes are a distinct NPC-order economy, not ordinary player vendors. `Customers.FillOrder` adds the quoted order value to stored shop gold and caps storage at **500,000**. `BaseShoppe` calls customer generation on a **two-hour** customer timer and a **60-second** quick timer after construction/load. Order fulfillment depends on tool/resources and the shop's trade difficulty. [E06]

The cash-out code uses integer arithmetic:

`cash = storedGold + storedGold × floor((floor(Mercantile) + guildBonus) / 100)`

`guildBonus` is 25 for Merchants Guild members. Thus 99 effective barter gives no bonus and 100 gives +100%; a guild member reaches that step at Mercantile 75. This is demonstrated arithmetic in this path, not a claim that the step was intended. Compare it with the smooth NPC/cargo/antique formulas when reviewing economic skill identity. [E06]

## How wealth becomes lasting power

### Crafting and runics

Crafting has more than one outcome axis: success, exceptional quality, material bonuses, random magic from runics, and later enhancement. A nominal item type is therefore not a complete description of its power.

For ordinary recipes, all required skills must meet their recipe minima. Success then uses the main skill:

`minimumChance + (skill − minimumSkill) / (maximumSkill − minimumSkill) × (1 − minimumChance)`

Exceptional chance depends on the craft system's mode: ordinary success chance minus 0.60; half success chance minus 0.10; or success chance minus a skill-dependent 0.60–0.45. Resource consumption, failure consumption, and recipe/tool conditions must also be included in expected cost per useful item. [E08]

Runics use resource-specific ranges for number and intensity of properties. For example dull copper has 1–2 properties, gold 3–4, and agapite 4; intensity ranges also change with `Core.ML`. A material's armor/weapon benefits are separate from its runic ranges. A high skill alone therefore does not describe the final gear distribution. [E09]

Bulk orders connect production to future production power: reward groups include harvesting tools, runic hammers and Blacksmith Power Scrolls. Gold is computed by amount, quality/material/type and the gold percentage. This can form a production loop: resources → orders → better tools or higher skill ceiling → better output. A complete profitability model still needs actual deed acquisition, mix, combination/completion time and market values. [E11]

### Guild enhancement is a direct gold-to-stat channel

Guild crafting exposes chosen permanent property upgrades. The smithing example requires Blacksmiths Guild membership, at least **90 Blacksmith**, and proximity to a suitable guildmaster, owned shoppe, or the explicitly coded guild area. Target must be in the player's possession, eligible metal weapon/armor, and not `ILevelable` legendary gear. Related guild tools serve other equipment families. [E10]

The shared upgrade cost is:

`(currentPropertyValue + 1) × propertyCost × 100`

The base 100 becomes 50 if the player crafted that item. The attribute-count cost multiplier is disabled. Example: Defense Chance Increase has maximum 15, increment 1, propertyCost 10. Raising it from 0 to 15 costs `1000 × (1 + ... + 15) = 120,000` gold, or **60,000** for the qualifying original crafter. These are per-item costs before considering existing properties. No random failure roll appears in `BeginUpgrade`. [E10]

The process has an attribute-count gate of 10 and per-property maxima, but the exact gate uses `CurrentAttributeCount > 10` or `MaxedAttributes >= 10`. Treat the precise allowed combination boundary as needing a controlled test; do not replace the implementation with the phrase “hard cap of ten attributes.” Combat caps and item eligibility can make a purchased property partly or entirely redundant, so measure effective build benefit rather than nominal property totals. [E10]

**The feedback loop:** more gold permits better gear; better gear may increase income per hour, which funds further gear. This preserves lasting progress, but its slope determines whether older wealth overwhelms current challenge. Source establishes the conversion path; comparative runs are needed to establish the slope.

## Gathering, farming, and time

### Harvesting is spatial and renewable

At checked-in resource multiplier 1:

| Resource | Shared patch geometry | Stored resource units | Refill delay | Ordinary consumption/yield basis |
|---|---|---:|---|---:|
| Ore/stone | 8×8 tiles | 10–34 | 10–20 minutes | 1 per harvest |
| Logs | 4×3 tiles | 20–45 | 20–30 minutes | 5 per harvest |

Both definitions use a 1.6-second effect delay. Isles of Dread uses a separate larger per-harvest expression. Additional mutations, skills, equipment and map-specific substitutions can change the actual output. Rare material veins also have skill gates and fallback resources. These are bank resources, not guaranteed successful player yields. [E07]

`HarvestLoopController` supports repeated harvesting, cancels on relevant movement/logout/disconnect/death/aggression conditions, and registers player command `StopHarvest`. Checked-in `AllowMacroResources=true` disables the intermittent resource CAPTCHA path. Automation is therefore a first-class workload assumption, not merely a hypothetical external macro. [E07]

Distinguish **active effort** (choosing routes, surviving, moving between banks, managing load) from **waiting** (effects, loops and refill). If a balance model counts every elapsed harvest minute as equally demanding play, it may overvalue unattended-compatible repetition and undervalue exploration or dangerous encounters.

### Homestead is several production systems

The package includes crops/wild crops and trees, grilling/baking/boiling, brewing, juicing, winecrafting, cheese, and household support items. Trades also has separate cooking, gardening, apiculture and crafting. These are overlapping ingredient/production families, not evidence of one uniform farm model. [E12–E13]

Wheat is a traced example, not a universal crop guarantee. Planting checks soil suitability and neighboring crop density. Picking has a three-second delay; `floor(Cooking / 20)` sets a random pick bound, doubled for the sower, with a yield cap. Regrowth waits 600 seconds, then increments every 15 seconds until capacity. At Cooking 20–39 a non-owner's bound is 1, so `Random(1)` returns 0; owner/non-owner and integer thresholds materially affect results. Other crop classes need comparison before assuming identical behavior. [E12]

The traced wine base restores Thirst up to 20, consumes the beverage, may return an empty container, applies poison if present, and adds 5 BAC up to a maximum 60. This base path is food/drink utility and intoxication, not demonstrated permanent combat progression. Special food/drink subclasses require their own effect inventory. [E13]

### Food is maintenance with exceptions

`FoodDecayTimer` runs every five minutes over connected clients. Normal hunger/thirst decreases by one, with a Camping avoidance check against a random 1–200. Racial exemptions and staff immunity exist. With checked-in `Belly=true`, specified public/safe/start/house regions skip decay. [E14]

At low hunger transitions, source removes portions of mana/stamina; at hunger 1 it can remove 5% of max health, 10% stamina and 20% mana when current values exceed those deductions. The implementation gates the switch behind hunger at least 1 and contains no case 0. Consequently the introductory comments saying “starvation can kill” are not sufficient evidence of continual lethal starvation. The thirst path similarly needs to be read from its implementation. Treat this as supply friction whose actual inconvenience depends strongly on safe locations and race. [E14]

## Property and government

Checked-in settings have house decay disabled, house limit `-1` (unlimited per the accessor), and `HouseStorage=true`. `HouseRegion.OnDecay` returns false with HouseStorage enabled, even before the usual locked/secure check. Formal container/lockdown/security limits still exist; this does not make every storage container unlimited or every dropped item secure. It removes normal floor-item decay inside that region. These rules make ownership and accumulation unusually persistent. [E15]

Player government adds a social expenditure loop with six population thresholds: **6, 12, 18, 24, 30, 36** citizens. Defaults are a seven-day city update and fourteen-day voting period, adjusted if the staff testing mode is enabled. Starting treasury is 150,000. The source documents membership by characters, including multiple characters per account; membership and population code need separate acceptance checks before treating thresholds as unique human counts. [E16]

Actual maintenance code charges:

`5000 × cityLevel + 2 × citizens + 3 × decor + 100 × add-ons`

plus 1,000 each for enabled guards, registration, bank, tavern, healer, moongate, stable, market, and per garden/park. A level-1 city with six citizens and no extras is therefore **5,012 per update**. The nearby comments list several older, higher prices and should not be used as current rules. Insufficient treasury funds lead to disband/delete behavior in this path. [E17]

Property tax withdraws from member bank funds and credits city treasury; city vendor income tax is also credited to treasury. Treasury withdrawals allow funds back into player hands. Thus taxes are transfers; maintenance is the gold sink. A rebalance should compare the service value and collective burden, not just count every tax as currency removal. Saved cities, elected officers, taxable membership, actual facilities and testing mode remain unverified. [E17]

## Adventure and reward pacing

### Ordinary loot and Luck

`LootPack` uses the highest credited damage-right holder's Luck. For AOS, the chance value is `floor(Luck^(1/1.8) × 100)`, compared with random 0–9999. Without SE, Luck above 1,200 is capped in this calculation. Honor perfection can add Luck in the killer-specific path. [E01]

| Luck | Chance of passing this Luck check |
|---:|---:|
| 0 | 0% |
| 100 | 12.91% |
| 500 | 31.58% |
| 1,000 | 46.41% |
| 2,000 | 68.21%, only if the SE cap does not apply |

This is **not a universal artifact probability or a direct percentage increase in total loot**. In pack generation it gives a retry opportunity on the first missed entry, and property mutation has further Luck checks. Treasure chests use separate Luck functions. Top-damage selection also means party equipment/Luck interactions are not described by averaging the party's Luck. [E01, E21]

### Repeatable jobs and artifact searches

Bulletin and fishing boards read a common configured cooldown of **60 minutes**, though they keep their own quest completion-time fields. One ordinary kill-job branch pays `floor((target Fame / 5) × QuestRewardModifier / 100)`; the checked-in modifier is **150%**. Completion creates gold and awards fame based on fee/100. This is a concrete branch, not every quest's formula. Generated jobs use available world targets, so source presence does not establish a populated job board in a deployed save. [E18]

Sages accept 5,000–10,000 gold in 1,000 increments for artifact search books. `LegendLore = paid/1000 − 4`; the real-clue probability becomes `LegendLore × 10 + 10`, giving **20% at 5,000** and **70% at 10,000**. Buying another replaces owned search books/pages in the traced vendor path. The configured completion cooldown is **8,640 minutes, six days**. This combines currency, discovery and uncertainty; the probability is paid information reliability, not a full run's success rate. [E19]

### Antiquities and cargo reward economic preparation

Antique sale total is base value plus Mercantile/400 of base, active Begging/400 of base, and +25% for Merchants Guild. At 100 Mercantile and 100 active Begging with the guild, total is **1.75× base**, with the applicable Begging karma effect. [E20]

Cargo total is base value plus Seafaring/300, Mercantile/300, active Begging/300, +25% for Fishermen's Guild, and +25% if turned in from a recognized port. With all three skills at 100 and both bonuses it approaches **2.5× base**, with per-term integer truncation. Cargo sales also call Seafaring skill checks based on gold/100. Finding the same cargo can therefore have different economic and training value for different characters. [E23]

### Treasure maps have their own reward staircase

`TreasureMapChest` adds four to the supplied level and caps it at ten. It fills once above effective level 0, again above 3, again above 7, and once more if its separate Luck check succeeds. It adds extra cash based on effective level and one relic per effective level. Chest lifetime is three hours. [E21]

Artifact condition is `RandomMinMax(0,100) < effectiveLevel × 17 + floor(min(Luck,2000) × .005)`. For nonnegative Luck, effective level 6+ makes this artifact condition certain; actual artifact construction and chest acquisition remain separate steps. At effective level 5 and zero Luck it passes 85 of 101 possible integers, about 84.16%. This reward gate is independent of ordinary `LootPack` Luck. [E21]

The 105 Power Scroll branch tests `0.02 + (level / 200)` where level is integer, so the added term is zero across effective levels 0–10. It is a 2% branch for levels above one. If that roll fails, a separate 7.5% Alacrity roll follows. These exact boundaries deserve comparison against paid artifact searches and champion rewards before setting any universal artifact/scroll scarcity target. [E21]

### Champions and nests share rewards differently

Current champion code allows Power Scrolls/skulls on every non-null, non-Internal map; an older Lodor-only rule is not present in this baseline. Champions normally award **six total** noncraft Power Scrolls round-robin among credited damage-right holders, with 110/115/120 quality probabilities **60%/35%/5%**. One skull goes to a random eligible recipient or the corpse when none qualify. Spawn artifact eligibility is a separate path and must not be conflated with boss scroll rights. [E24]

Champion ground gold schedules a pile on every fitting tile in a radius-12 disk, subject to `NoKillAwards`/`NoGoodies`. There are at most **441** integer tiles. Each fitting tile receives 500–1,000 gold without the gold percentage applied. If all tiles fit, the expected total is **330,750**, with possible total range 220,500–441,000. This is a geometric/theoretical payout, not a measured collection amount or typical live kill. Terrain and collection/losses matter. [E24]

Monster-nest remains instead reward **every PlayerMobile within 20 tiles** independently when activated, then delete. That method does not check damage contribution, party membership or alive state. Its reward tier uses `Random(5,20) × lootLevel`, meaning a 5–24 integer roll times the tier. `Random(7000,10000)` in its top branch means **7,000–16,999**, because the second parameter is a count. The total reward increases with nearby player-character count. This differs fundamentally from dividing a fixed champion scroll pool. Actual nest spawn rates, live tiers and nearby-account behavior are unknown. [E25]

### Oceans, encounters and events

Fishing uses Seafaring plus pole skill. Its explicit training helper permits gains below 50 anywhere, and at/above 50 when on a boat. Wreck/ruin salvage, cargo, pirate bounties and dedicated fishing quests make the sea a distinct acquisition and travel branch. Pirate bounty base values are 1,000–3,000 before the 25% gold factor. Boats, grappling/boarding and navigation classes are present; their full cost/risk/time model was not reconstructed here. [E22–E23]

Random encounters have an initialization/timer engine, XML definitions and a helper that routes player difficulty through `CharacterLevelService.GetEncounterLevel`; spawned mobiles use a legacy level calculation. A listed XML creature is a candidate, not proof it is active in every region or spawned at a meaningful frequency. [E26]

The custom Atlas boss calls `MobileBalanceCatalog.DropLoot`. Goliaths and similar named opponents therefore need profile/loot/spawn analysis, not a single inferred “endgame boss” reward rate. The invasion interface registers `invasion` for Administrators and creates event spawners/waypoints. An installed invasion package is not proof of an automatic active event schedule. [E27–E28]

## Community incentives

The traced voting path validates cooldown, launches the configured browser URL and records time. Default cooldown is 24 hours. `VoteStone` delegates to base and adds no material reward in its overrides. External hooks or other rewards were not exhaustively ruled out. Avoid calling voting a power faucet without an actual payout path. [E29]

Referral login processing awards a stat ball to the referred player and queues rewards for the referrer. Both account playtimes must reach **48 hours**; IP uniqueness is enabled; age/recent-login checks use static date thresholds initialized at process start. The reward defaults to +10 raw stat training, limited by the character's total StatCap and 125 in the chosen stat. It is not a stat-cap increase. The static dates merit restart-sensitive test cases if referral timing is later reviewed. [E30]

## Balance questions supported by these paths

These are hypotheses or decisions for later investigation, not approved changes.

| Question | Why the source raises it | Minimum evidence to resolve it |
|---|---|---|
| Do direct payouts dominate scaled faucets? | Champion/nest gold bypasses 25% scale; other loops use it | Created gold by source, encounter completions, active/elapsed minutes, net consumable cost |
| Does repeatable income outgrow relevant spending? | Gold upgrades end at per-property caps; vendor upkeep is small; persistent housing/storage | Gold created/destroyed by source and account cohort; upgrade saturation; net wealth distribution |
| Does income scale too strongly with prior wealth? | Gold buys permanent properties; power may shorten kills and unlock rewards | Same content at several realistic wealth/build tiers; damage, survival, time and net rewards |
| Are economic skills compulsory or a useful specialty? | Same goods receive large Mercantile/Begging/guild bonuses | Sale-value share, skill opportunity cost, cross-character transfer, trade participation |
| Are rewards keyed to challenge or to repetition? | Resource loops, long cooldowns and multiple random stages have different effort profiles | Active inputs, elapsed wait, failure/recovery cost, uncertainty and discovery per activity |
| Do group rules reward cooperation fairly? | Fixed six champion scrolls versus independent nest reward per nearby character | Contribution/account counts, solo/group completion time, reward per person and per event |
| Do step formulas create hidden mandatory breakpoints? | Shoppes integer division; treasure effective-level jump/artifact threshold | Values immediately below/at/above thresholds in a safe fixture; player understanding |
| Is discovery a sustainable reward? | Named artifact choice/search competes with other artifact channels | Time-to-target artifact, duplicates, collection completion, effective item desirability |
| Do production professions retain a market? | Loot, runics, guild upgrades and craft output can substitute for each other | Acquisition cost per useful item; trade volumes; consumable demand; repair/loss rates |
| Does accumulated property crowd out social goals? | Unlimited/non-decaying houses and persistent floor storage | Actual property occupancy, inactive ownership, travel convenience, city usage and upkeep burden |

Preserving the game's identity does not require every repeated action to increase combat power forever. A later design can preserve earned permanence while distinguishing combat ceilings, build choices, rare discoveries, collections, convenience, craft specialization, social infrastructure and prestige. The first decision should be which activities are meant to reward each of these; numeric changes should follow that decision and measured baselines.

## Coverage and explicit gaps

**Mechanisms traced:** ordinary LootPack Luck; global gold scaling examples; generic NPC sale pricing and reseller exclusion; player-vendor upkeep; shoppes order/cash-out; resource banks and harvesting loops; common crafting chance; representative runic ranges; smithing guild enhancement/shared costs; BOD reward path; representative crop and wine; hunger implementation; housing decay/storage settings and region consumer; government timers/maintenance/tax transfers; ordinary quest branch/cooldowns; artifact search pricing/reliability; antique/cargo multipliers; treasure chest bundles; champion/nest payout; voting/referral behavior.

**Family present, sampled only:** all specialized recipes, materials, consumables, apiculture, ornamental gardening, animal husbandry, all bulk-order professions, fishing catches/SOS/salvage, boat mechanics/piracy, museum collections, full artifact catalog, generated encounter pools, named bosses and invasion wave distributions.

**Inventory only / not fully traced here:** gambling/casino economy; all special tokens; XML quest token uses and staff-defined rewards; every quest family (Assassin, Bards Tale, Codex, Epic, Frankenstein, Golems, Hoard, Jester, Magic Pools, Major, Pagan, Prisoners, Robots, Runes, Serpents, Shadowlords, Summon, Thief, Underworld); full NPC buy/sell table parity; all stock resource mutations; complete property purchase/refund/rental paths; city elections/war/alliance effects; every rare material and unique boss gate.

**Unavailable:** production save/placement/spawn state, effective deployed settings, player economic telemetry, measured activities, item distributions and market prices. No conclusion here establishes live abundance, player abuse, inflation, trivial combat, or actual unavailability. No compile/startup build was attempted because this is documentation from static review and a startup would not establish economic balance.

## Source register

- **E01:** `Data/Scripts/Items/Containers/LootPack.cs:15–80` (Luck calculation/credited killer); `:90–114` (first missed-entry retry); `:2406–2450` (mutation Luck). These are local LootPack semantics, not all reward code.
- **E02:** `Data/Scripts/System/Misc/Settings.cs:129–135` (settings file load), `:189–197` (artifact delay/gold/harvesting settings), `:675–701` (gold clamp and CAPTCHA flag), `:1118–1135` (resource multiplier/sales); `Info/settings.xml:45–59`, `:113–115`, `:237–243`; `Data/Scripts/Items/Containers/LootPack.cs:3468–3478` (dice scaling).
- **E03:** `Data/Scripts/Items/Containers/Loot.cs:1294` onward (artifact type roster); `Data/Scripts/Items/Magical/ArtifactBuilder.cs:14`; `Data/Scripts/Items/Magical/Artifacts/Arty_Setup.cs:9–91` (artifact point setup) and `:150–164` (enchantment eligibility helpers). Catalog members were not individually audited.
- **E04:** `Data/Scripts/Mobiles/Base/GenericSell.cs:20–297` (type/quality/material pricing, half value, barter multiplier, resale price); `:322–330` (sellability and disabled resale); `Data/Scripts/Mobiles/Base/BaseVendor.cs:2950–3101` (sale gates and barter selection), `:2713–2728` (conditional resale purchase path), `:3107–3115` (gold delivery).
- **E05:** `Data/Scripts/Mobiles/Base/PlayerVendor.cs:617–625` (1-gold charges), `:1146–1177` (collection), `:1423–1455` (pay path).
- **E06:** `Data/Scripts/Trades/Shoppes/BaseShoppe.cs:429–473` (skill progress), `:489–513` (cash-out), `:812–843` (timers); `Data/Scripts/Trades/Shoppes/Customers.cs:86–88` (gold scale), `:230` (difficulty helper), `:266–356` (fulfillment and gold cap).
- **E07:** `Data/Scripts/Trades/Harvest/Mining.cs:47–76`, `:87–184`; `Data/Scripts/Trades/Harvest/Lumberjacking.cs:45–114`; `Data/Scripts/Trades/Harvest/HarvestSystem.cs:126–162`, `:253–283`, `:332–357`, `:819–877`; `Data/Scripts/Trades/Harvest/HarvestLoopController.cs:36–62`, `:109–139` (hooks, command, loop continuation).
- **E08:** `Data/Scripts/Trades/Core/CraftItem.cs:678–865` (resource consumption), `:947–980` (exceptional modes), `:1024–1076` (success chance), `:4337–4350` (failure consumption); `Data/Scripts/Trades/Core/Enhance.cs:123–222`, `:298–308` (separate material-enhancement risk).
- **E09:** `Data/Scripts/System/Misc/ResourceInfo.cs:275–293`, `:363–393`; `Data/Scripts/Items/Trades/Tools/BaseRunicTool.cs:471–490`, `:952–971`.
- **E10:** `Data/Scripts/Trades/Guild/GuildHammer.cs:50–98`, `:139–183`; `Data/Scripts/Trades/Guild/EnhancementStoneProcess.cs:10–76`, `:80–118`, `:178–207`; `Data/Scripts/Trades/Guild/AttributeHandler.cs:63–108` (DCI/HCI definitions).
- **E11:** `Data/Scripts/Trades/Bulk Orders/Rewards.cs:149–198`, `:301–315`, `:399–426`, `:538–573`, `:579` onward; `Data/Scripts/Trades/Bulk Orders/SmallSmithBOD.cs:28–58` (reward delegation).
- **E12:** `Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Vhaerun's CRL Crops/Crops/WheatCrop.cs:37–72`, `:274–397`; sibling crops/wild crops/trees are an inventory, not this sample's proven behavior.
- **E13:** `Data/Scripts/Custom/Vhaerun's CRL Homestead System [2.0]/Dracana's Winecrafting [2.0]/Items/Food/BaseCraftWine.cs:231–288`; package `DefGrilling.cs`, `DefBoiling.cs`, `DefBaking.cs`, `DefBrewing.cs`, `DefJuicing.cs`, `DefWinecrafting.cs`; `Data/Scripts/Trades/Crafting/DefCooking.cs`, `Trades/Gardening`, `Trades/Apiculture` (family inventory).
- **E14:** `Data/Scripts/System/Misc/FoodDecay.cs:54–123`, `:159–225`, `:241–397`; `Data/Scripts/System/Misc/Settings.cs:1300–1303`; `Info/settings.xml:321–323`.
- **E15:** `Info/settings.xml:145–155`, `:253–255`; `Data/Scripts/System/Misc/Settings.cs:289–293`, `:391–393`, `:941–956`, `:1162–1165`; `Data/Scripts/System/Regions/HouseRegion.cs:423–432`; `Data/Scripts/Items/Houses/BaseHouse.cs:37`, `:304–314`, `:947–965`.
- **E16:** `Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs:28–47`, `:49–115`; `Data/Scripts/Custom/Government System/GovernmentTestingMode.cs:14–47`.
- **E17:** `Data/Scripts/Custom/Government System/Items/Stones/CityManagementStone.cs:724–852`, `:1628–1697`; `Data/Scripts/Custom/Government System/Prompts/CityTreasuryWithdrawPrompt.cs:39–68`; `Data/Scripts/Custom/Government System/Gumps/PCMoongateTollGump.cs:36` onward; `Data/Scripts/Custom/Government System/Gumps/CityResFeeGump.cs:54` and `CityCorpseFeeGump.cs:60` (treasury credits).
- **E18:** `Data/Scripts/Quests/Standard/StandardQuestFunctions.cs:163–185`, `:401–426`, `:584–603`; `StandardQuestBoard.cs:105–106`; `Data/Scripts/Quests/Fishing/FishingQuestFunctions.cs:184–206`, `:679–684`, `:820–829`; `FishingQuestBoard.cs:105–106`.
- **E19:** `Data/Scripts/Mobiles/Civilized/Vendors/Sage.cs:87–148`; `Data/Scripts/Quests/Search/SearchBook.cs:39`; `SearchPage.cs:125–167`, `:383–405`; `SearchBase.cs:1193`; `Info/settings.xml:49–51`.
- **E20:** `Data/Scripts/Quests/Museum/Museum.cs:223–259`, `:323–324`; `MuseumBook.cs:524`, `:691`.
- **E21:** `Data/Scripts/Items/Trades/Maps/TreasureMap.cs:167`, `:260–328`, `:561–577`; `Data/Scripts/Items/Containers/TreasureMapChest.cs:67–169`; `Data/Scripts/System/Misc/Players.cs:1254–1261` (artifact Luck bonus).
- **E22:** `Data/Scripts/Trades/Harvest/Fishing.cs:146–180`, `:276–283`, `:465–620`, `:635–641`; `Data/Scripts/Trades/Harvest/HarvestSystem.cs:738–761` (wreck/crash/ruin branches). `Data/Scripts/Items/Boats/BaseBoat.cs`, `GrapplingHook.cs`, `Galleons.cs`, `Vessels.cs` are inventory pointers.
- **E23:** `Data/Scripts/Items/Boats/Cargo.cs:724`, `:2667–2672`, `:2877–2935`; `Data/Scripts/Items/Boats/PirateBounty.cs:43–46`.
- **E24:** `Data/Scripts/Custom/Champions/System/Mobiles/BaseChampion.cs:47–56`, `:80–155`, `:226–280`, `:291–317`; `Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs:74–75`, `:154–194`, `:242–244`; `Spawns/RWChamp.xml` (checked-in placement definitions, not live proof); `Data/Scripts/Items/Misc/Gold.cs:16–26` (inclusive gold constructor range).
- **E25:** `Data/Scripts/Custom/PvE/MonsterNests/MonsterNest.cs:84–98`, `:140–166`; `MonsterNestLoot.cs:25–82`; `Data/System/Source/Utility.cs:780–804` (RandomMinMax versus Random semantics).
- **E26:** `Data/Scripts/Custom/PvE/RandomEncounters/EncounterEngine.cs:99–173`; `Helpers.cs:115–122`; `RandomEncounters.xml`; `Records.cs:108–110`, `:164`.
- **E27:** `Data/Scripts/Custom/Mobiles/Goliaths/Atlas.cs:41–44`; `Data/Scripts/Custom/Mobiles/Goliaths/TwoFace.cs`; `CyclopsIntruder.cs`; `Data/Scripts/Mobiles/Goliaths` (individual classes/profile-dependent inventory).
- **E28:** `Data/Scripts/Custom/Invasion System/Gump/InvasionGump.cs:12–23`; `Data/Scripts/Custom/Invasion System/Sosaria/StartStopBritTrammel.cs:35` onward (spawner/waypoint creation).
- **E29:** `Data/Scripts/Custom/Voting/VoteCommand.cs:18–20`; `VoteConfig.cs:35`, `:43–56`; `VoteEvents.cs:27–61`; `VoteItem.cs:87–153`; `VoteStone.cs:66–79`.
- **E30:** `Data/Scripts/Custom/TellAFriend/TellAFriend.cs:37–54`, `:69–118`, `:123–138`, `:382–391`; `Rewards/StatBall.cs:32`, `:191–242`.

## Review record

Published from the [dated source review](../codebase-audit/outputs/subagent-findings/2026-09-27-gameplay-economy-world.md). The [verification record](VERIFICATION.md) applies to the complete documentation package. Regenerate this chapter with `python docs/game-balance/tools/publish_chapters.py` after changing its review record.
