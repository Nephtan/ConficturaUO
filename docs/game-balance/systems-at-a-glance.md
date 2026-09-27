# Systems at a glance

[Start here](README.md) · [Character](character-and-progression.md) · [Combat](combat-and-builds.md) · [Adventure and economy](adventure-and-economy.md) · [World](world-and-exploration.md)

Read each row left to right: **what you do → what you get → what limits it**. These are systems and activity families, not a list of every item or spell. Follow the chapter links for exact rules, exceptions, and source evidence.

## Build a character

| System | What you do | What you gain | Main limit / connection |
| --- | --- | --- | --- |
| Origins and fate | Choose a starting life and its obligations. | Different starting resources, skill capacity, and costs. | Default, savage, fugitive, and alien progression have materially different ceilings and burdens. |
| Skills | Practice eligible actions and use training paths. | Success, damage, defense, access, production, or economic benefits. | Individual and total caps; locks; base versus effective values; some skills serve several roles. |
| Stats and vitals | Train Strength, Dexterity, Intelligence; equip and buff. | Health, stamina, mana, damage, speed, carrying capacity. | Raw training caps, effective-stat caps, and vital formulas are separate. |
| Character level | Develop skills, stats, and reputation. | A 1–100 rating used by other systems. | It is calculated, not earned XP; it omits parts of equipment power. |
| Offline study | Choose a permitted training subject and log out. | Probabilistic skill progress. | Time, eligibility, attempt cap, and total/individual skill limits. |
| Training items and scrolls | Obtain books, consumables, or skill-cap scrolls. | Training, accelerated gain, or a higher ceiling depending on item. | A cap increase does not itself train the skill. |
| Titan progression | Complete the qualifying progression. | Higher total skill/stat capacity. | A larger capacity also changes the denominator of the level rating. |
| Creature races | Adopt a creature template. | Skill/stat/resistance/Luck or other template effects. | Many variants; some bonuses depend on calculated character level. |
| Reputation and virtues | Fight, help, steal, beg, or follow related activities. | Fame, Karma, titles, eligibility, and selected mechanics. | Reputation participates in level and rewards; positive and negative paths differ. |
| Death and resurrection | Recover from defeat and choose a recovery route. | Return to play, sometimes with tribute/fees or loss. | Origin, tribute, novice gates, and resurrection implementation all matter. |

**Deep guide:** [Character and progression](character-and-progression.md), including the full 58-skill glossary and source traces.

## Win and survive fights

| System | What you do | What you gain | Main limit / connection |
| --- | --- | --- | --- |
| Melee and ranged attacks | Land weapon attacks and manage positioning. | Direct damage and on-hit effects. | Hit chance, cadence, skill contributions, item bonuses, resistances, and special abilities. |
| Weapon abilities | Spend resources on a special attack. | Burst, debuffs, control, extra hits, or area effects. | Weapon/skill/resource gates; many custom abilities. |
| Defense | Combine avoidance, parry, resistances, and mitigation. | Less incoming damage and longer survival. | Different caps and exceptions; armor choice also affects other activities. |
| Healing and regeneration | Bandage, cast, drink, regenerate, or leech. | Less downtime and greater endurance. | Delays, interruption/slips, poison, resource costs, and distinct regen caps. |
| Magic | Learn a school, carry its resources, choose effects. | Damage, healing, control, summons, travel, utility, or enhancements. | Schools use different skills/costs; some exclude one another; access paths differ. |
| Temporary buffs and forms | Maintain a spell, stance, form, or consumable benefit. | A situational advantage. | Duration, exclusions, skill requirements, and interaction with shared caps. |
| Equipment and artifacts | Find, craft, buy, and combine properties. | Stronger or more flexible builds. | Nominal properties can exceed effective caps; acquisition channels are very different. |
| Levelable equipment | Wear eligible items while earning credited kills. | Item XP, item levels, and spendable upgrade points. | Each equipped eligible item can progress; this is separate from character level. |
| Pets and summons | Tame/train/bond or summon and command allies. | Damage, tanking, utility, carrying, and support. | Follower slots, four-skill thresholds, control, upkeep, and pet-specific damage rules. |
| Enemy AI and scaling | Face different behaviors and area/event rules. | Challenge, target priorities, and rewards. | Base creature, specific subclass, area scaling, AI, and spawn definition can all contribute. |
| Consent, crime, and conflict | Choose modes and interact under regional/social rules. | Permitted conflict or protection in particular paths. | Direct players, pets, government, events, theft, and beneficial actions need separate checks. |
| Turn-based combat | Optional alternate PvP mode in the source. | Action-point/initiative structure when enabled and accepted. | **Disabled in checked-in configuration**; ordinary analysis uses real time. |

**Deep guide:** [Combat and builds](combat-and-builds.md). The spell index covers all 346 registrations, while selected spell effects and shared formulas receive deeper review.

## Adventure, discover, and acquire

| System | What you do | What you gain | Main limit / connection |
| --- | --- | --- | --- |
| Ordinary hunting | Defeat creatures and collect loot. | Gold, equipment, resources, reputation, equipment XP. | Damage rights, Luck, creature-specific rewards, travel and recovery cost. |
| Dungeons and regional difficulty | Enter increasingly dangerous authored areas. | Access to threats, rewards, and discoveries. | Region rules, travel restrictions, scaling, and actual world placement. |
| Random encounters | Explore eligible locations. | Additional fights and their rewards. | Player difficulty uses calculated level; XML pools and engine eligibility matter. |
| Champions | Complete waves and defeat a boss. | Shared scroll pool, skull, gold, and other eligible rewards. | Contribution/rights, wave progress, region and spawn properties. |
| Monster nests | Defeat a nest and activate remains. | Independent nearby-player reward. | Nearby character count affects total reward creation; contribution gates differ from champions. |
| Named bosses and Goliaths | Defeat individually authored opponents. | Profile-specific loot and accomplishments. | Specific AI, abilities, loot, spawn availability, and existing balance profiles. |
| Bulletin/fishing jobs | Take a generated job, complete and return. | Currency, reputation, and related progress. | Available world targets, completion state, and cooldown. |
| Story and special quest families | Follow a chain, collect, solve, or unlock. | Unique goals and item/access outcomes. | Every family has its own state and gates; some are indexed rather than fully traced here. |
| Artifact searches and collections | Buy clues, investigate, collect or sell antiquities. | Chosen artifact pursuit, collection, or economic reward. | Information reliability, cooldown, travel, item value, and economic skills. |
| Treasure maps | Decode, travel, dig, handle guardians/locks/traps. | Tiered chest rewards. | Effective chest level and its own Luck/reward formulas. |
| Searching, stealth, tracking, locks, traps, theft | Scout or reach value without relying only on damage. | Access, avoidance, information, loot, and wealth transfers. | Range, armor, skill contests, security, and crime rules. |
| Sailing, fishing, salvage, piracy, and cargo | Navigate water and take sea objectives. | Resources, valuables, cargo/bounties, Seafaring progress. | Boat access, geography, training gates, danger, and port/guild bonuses. |
| Invasions and staff events | Participate when an event is established. | Encounter/event rewards. | Staff control and placement; installation does not prove a live automatic schedule. |

**Deep guides:** [Adventure and economy](adventure-and-economy.md) and [World and exploration](world-and-exploration.md).

## Produce, trade, build, and belong

| System | What you do | What you gain | Main limit / connection |
| --- | --- | --- | --- |
| Harvesting | Gather renewable resources from spatial banks. | Materials and skill. | Depletion, refill, resource type, skill, tools, load, and location. |
| Crafting | Turn inputs into a recipe. | Gear, supplies, furnishings, and professional progress. | Required skills, success/quality, resources, tool uses. |
| Runic tools and material enhancement | Spend special materials/tools on gear. | Random or material-specific properties. | Property ranges, limited tools, and branch-specific risk. |
| Guild enhancement | Qualify and pay for selected upgrades. | Permanent equipment properties. | Gold, item eligibility, property limits, and shared combat caps. |
| Bulk orders | Fulfill specified craft requirements. | Gold, tools, runics, and skill-cap rewards. | Deed mix, quantities, quality/material, and completion time. |
| Homestead and food production | Grow and prepare crops, food, drink, or household goods. | Renewable supplies, production, expression. | Space, recipe inputs, skill thresholds, growth/picking timers. |
| Gardening and apiculture | Maintain plants or hives. | Specialist products and collection goals. | Separate state/growth systems; only family-level review in this study. |
| Hunger and thirst | Carry and use food/drink. | Maintained meters and avoidance of penalties. | Timers, Camping, race, and safe-area exceptions. |
| NPC trade and services | Buy supplies or sell eligible goods. | Goods or newly created gold from NPC purchases. | Price tables, economic skills, guilds, item eligibility. |
| Player vendors | Buy/sell between players. | Transferred wealth and access to goods. | Demand, stock, property, and a small ordinary upkeep charge. |
| Shoppes | Fulfill generated shop orders and cash out. | Created income and professional progress. | Inputs, timers, stored-gold cap, and a stepped cash-out formula. |
| Housing and storage | Own, organize, decorate, and use property. | Durable convenience, production space, identity. | Container/security rules remain; checked-in decay and ownership settings favor accumulation. |
| Government | Form cities, elect officials, fund services, tax, and govern. | Collective facilities and social goals. | Citizens, treasury, upkeep, access, and interactions with conflict rules. |
| Voting and referrals | Engage through account/community paths. | Referral training reward; voting has no material payout in the traced base path. | Eligibility, time, account checks; external rewards unverified. |
| Books, titles, chat, guilds, and stories | Express identity and participate socially. | Knowledge, reputation among players, records, and belonging. | These families are present; subjective value needs player input. |
| Staff tools, XML spawners, and integrations | Staff authors/manages the world and services. | Operational control over access, content, and rewards. | This can alter balance through saved state without changing a C# formula. |

**Deep guide:** [Adventure and economy](adventure-and-economy.md). [Coverage](coverage-and-evidence.md) identifies sampled and indexed-only families so a short row is never mistaken for a complete per-item audit.

## A shared vocabulary

| Term | Meaning here |
| --- | --- |
| Base skill / raw stat | Lasting trained value before ordinary equipment and temporary modifiers. |
| Effective value | Value the current calculation sees after applicable modifiers and caps. Different calculations can use different versions. |
| Cap | A limit on one quantity or one branch. Always identify which one. |
| Rating | A summary of other properties; the character's level is one. |
| Item XP | Actual progression stored on eligible equipment; separate from character rating. |
| Threshold | A value that unlocks something or makes the formula jump. |
| Gold faucet | Code creates spendable currency. |
| Gold sink | Code removes currency from the economy. |
| Transfer | Existing currency changes hands; it is not destroyed. |
| Throughput | Useful completions per unit of time, including relevant downtime. |
| Source fact | Behavior established by a reviewed code path, not a live measurement. |

## Source trace and scope

Each row summarizes the evidence in the linked domain chapter and its source register. The [source inventory](data/source-inventory.csv) and [directory-family table](data/source-families.csv) include the remaining code paths. A family listed here can have both deeply reviewed shared rules and individually unreviewed content variants. No source move, gameplay adjustment, or migration is authorized by this map.
