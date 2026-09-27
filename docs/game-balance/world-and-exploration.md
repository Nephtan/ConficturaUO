# World and exploration

[Start here](README.md) · [Systems](systems-at-a-glance.md) · [Adventure and economy](adventure-and-economy.md)

## At a glance

**Purpose:** make places, knowledge, routes, and noncombat skills matter.

**Player loop:** prepare → travel → scout → find an opportunity → handle danger or a puzzle → bring the reward home.

**Main balance question:** does exploring stay valuable after players learn the fastest repeatable route?

## The ground the game stands on

The server has an engine and a large script layer. At startup the engine gathers C# scripts recursively from `Data/Scripts`, compiles them, runs `Configure`, loads regions and the saved world, then runs `Initialize`. An item, NPC, or spell can exist in code without currently being placed, obtainable, or enabled in the world.

The checked-in expansion is `SA`, which also makes the earlier AOS, SE, and ML branches active. Standard Ultima Online descriptions therefore cannot be assumed to match this shard. For example, its expansion setup disables ordinary stat-to-skill influences and sets insurance off.

| Layer | Plain meaning | What can change gameplay here |
| --- | --- | --- |
| Engine | Common definitions of a mobile, item, skill, timer, map, packet, and damage event. | A common skill check can change hundreds of activities. |
| Scripts | Rules for characters, enemies, items, spells, quests, production, and events. | A base class can affect whole families; an override can bypass it. |
| Settings and data | Values and definitions loaded by scripts. | Gold scaling, access, cooldowns, regions, spawn definitions, combat modes. |
| Saved world | Actual characters, properties, equipment, NPCs, spawners, and other placed objects. | Old items, staff changes, player wealth, and actual world availability. |
| Player practice | Routes, builds, tactics, markets, automation, and social organization. | Determines which available rules dominate actual play. |

Source: [ScriptCompiler.cs:587](../../Data/System/Source/ScriptCompiler.cs#L587), [Main.cs:582](../../Data/System/Source/Main.cs#L582), [CurrentExpansion.cs:8](../../Data/Scripts/System/Misc/CurrentExpansion.cs#L8), [Main.cs:251](../../Data/System/Source/Main.cs#L251).

## Maps and regions are different

Seven playable maps are registered: **Lodor, Sosaria, Underworld, SerpentIsland, IslesDread, SavagedEmpire, and Atlantis**. `Internal` is the engine's holding space. Named lands can occupy only part of a map: map identity and region identity must both be recorded when comparing encounters.

All seven playable registrations use `MapRules.LodorRules`. A name like Sosaria does not by itself imply stock Trammel safety. Regions, consent, government, and the harmful-action handlers add their own rules.

Regions define areas such as dungeons, caves, towns, houses, prisons, pirate zones, and special story locations. When areas overlap, the engine sorts dynamic regions first, then higher priority, then deeper child regions. Houses and cities can therefore affect the rule that a player encounters at a particular location.

Source: [MapDefinitions.cs:18](../../Data/Scripts/System/Misc/MapDefinitions.cs#L18), [Map.cs:32](../../Data/System/Source/Map.cs#L32), [Region.cs:122](../../Data/System/Source/Region.cs#L122), [Region.cs:554](../../Data/System/Source/Region.cs#L554). The generated [region inventory](data/regions.csv) records checked-in XML definitions, not current occupancy or spawned counts.

## Getting somewhere and getting out are separate rules

Recall and Gate Travel perform several checks. Their shared travel matrix currently contains permissive entries, but the actual spells also call custom world helpers. Reading that matrix alone would give the wrong answer about unrestricted travel.

| Check | Player meaning | Examples from current code |
| --- | --- | --- |
| Basic travel validation | Is this a valid destination, and may this character travel? | Null/internal/out-of-bounds rejection; player jail check. |
| `Worlds.AllowEscape` | May the character leave this special situation? | Camping tent, dungeon room, Lyceum, Chasm, ship's lower deck; other named-world conditions. |
| `RegionAllowedRecall` | May recall or gate leave this area? | Skara Brae, Moonlight Cavern, Kuldar, Ambrosia, Ravendark restrictions. |
| `RegionAllowedTeleport` | May the spell arrive there? | Dungeons and many named special regions/worlds reject arrival. |
| Spell-specific checks | Can this particular cast complete? | Spell cost/sequence, destination fit, runebook charges, and other branch conditions. |

These are examples, not a universal travel whitelist. Other transport items, boats, portals, and custom spells need their own route trace.

**Balance connection:** access is a reward. A route that removes a return journey or bypasses a dangerous approach can improve rewards per hour without adding damage. Travel restrictions can preserve an expedition's significance, but a repeated uneventful journey can also become pure friction.

Source: [SpellHelper.cs:621](../../Data/Scripts/Magic/Base/SpellHelper.cs#L621), [SpellHelper.cs:799](../../Data/Scripts/Magic/Base/SpellHelper.cs#L799), [Recall.cs:61](../../Data/Scripts/Magic/Magery/Magery%204th/Recall.cs#L61), [GateTravel.cs:48](../../Data/Scripts/Magic/Magery/Magery%207th/GateTravel.cs#L48), [World.cs:781](../../Data/Scripts/System/Misc/World.cs#L781), [World.cs:852](../../Data/Scripts/System/Misc/World.cs#L852), [World.cs:934](../../Data/Scripts/System/Misc/World.cs#L934).

## Scouting, stealth, and theft are their own form of capability

| System | How it works in the reviewed path | Cost or limit | Why a balance review must include it |
| --- | --- | --- | --- |
| Searching | Target an area; radius is integer `Searching / 10`, halved on a failed skill roll. A house friend gets radius 22. Opposed checks reveal hidden mobiles; other paths detect traps, doors, and hidden chests. | Six-second use delay; access checks; reveal contests; maximum arguments may be normalized by the common skill checker. | Creates reward access and counters stealth; not simply a cosmetic information skill. |
| Hiding | Checks combat and nearby attackers, then the skill roll. House friendship supplies a bonus. A successful use also hides controlled pets owned by that player. | Spell/combat/line-of-sight checks; commonly a two-second retry delay. | Survivability and pet concealment can matter as much as extra armor. |
| Stealth | Requires hiding first and at least 30 effective Hiding in the active ML branch. Armor carries a stealth penalty; Mage Armor pieces do not add that penalty. | Penalty of 42 or more blocks this path. Successful use grants `floor(Stealth / 5)` steps, minimum one. | Equipment choice changes mobility and risk; a heavy suit can close this route. |
| Tracking | Search radius is `25 + floor(Tracking / 2)`. Tracking a player pits Tracking plus Searching against Hiding plus Stealth, with transformation adjustments. | Target eligibility and chance checks; a tracking arrow has its own update rules. | Counters stealth and makes knowledge skills useful in conflict. |
| Lockpicking | A three-second attempt checks a minimum requirement, then the lock's skill range. | Certain lock values cannot be picked; lockpicks can break; other consequences depend on the target. | Lets a character reach loot through an investment other than combat damage. |
| Remove Trap | Container trap level sets a hard eligibility gate, followed by a skill roll. Hidden floor traps and faction traps have distinct paths. | Five-second use delay; faction kits and extra Tinkering checks in that branch. | Reduces expedition risk and adds a preparation role. |
| Stealing | Different branches handle dungeon containers, items, stacks, and special stealable artifacts. Ordinary item checks depend on weight. | Adjacent range, ownership/mobility/blessed checks, detection/criminal consequences; special artifacts require 100 effective Stealing. | Can transfer wealth or open special rewards; NPC vendors and player vendors have explicit exclusions. |

### Three examples

1. **Searching 100:** the ordinary radius is 10 tiles. A failed skill check halves it to 5; that is different from failing to search at all. Hidden-target contests are additional checks.
2. **Stealth 100 with acceptable armor:** a successful use grants 20 steps in the active AOS branch. More skill improves the allowance, but unsuitable armor can prevent starting entirely.
3. **Tracking versus an untransformed player:** with Tracking 100 and Searching 100 against Hiding 100 and Stealth 100, the formula gives 75 before the 0–99 random comparison. This is the difficulty branch only; finding an eligible target and range checks still apply.

**Do not assume PvP consent automatically protects items from every theft path.** Harmful combat, snooping, stealing, corpse rights, and criminality use different code. A consent-by-theft-path test matrix belongs in the follow-up research.

Source: [Searching.cs:19](../../Data/Scripts/System/Skills/Searching.cs#L19), [Searching.cs:136](../../Data/Scripts/System/Skills/Searching.cs#L136), [Hiding.cs:25](../../Data/Scripts/System/Skills/Hiding.cs#L25), [Stealth.cs:84](../../Data/Scripts/System/Skills/Stealth.cs#L84), [Tracking.cs:567](../../Data/Scripts/System/Skills/Tracking.cs#L567), [Tracking.cs:636](../../Data/Scripts/System/Skills/Tracking.cs#L636), [LockPick.cs:188](../../Data/Scripts/Items/Trades/Thieving/LockPick.cs#L188), [LockPick.cs:289](../../Data/Scripts/Items/Trades/Thieving/LockPick.cs#L289), [RemoveTrap.cs:17](../../Data/Scripts/System/Skills/RemoveTrap.cs#L17), [Stealing.cs:52](../../Data/Scripts/System/Skills/Stealing.cs#L52), [Stealing.cs:273](../../Data/Scripts/System/Skills/Stealing.cs#L273), [Stealing.cs:468](../../Data/Scripts/System/Skills/Stealing.cs#L468).

## The configured combat mode

Turn-based combat is present in the source but `Data/TurnBasedCombat/TurnBasedCombat.cfg` sets `Enabled=false`. Its configured activation mode is PvP-only and it has a compatibility gate. Ordinary real-time formulas are the basis of this study. A future activation would require a separate review of action points, actor-time effects, group joining, pets, disconnects, and timeouts.

Source: [TurnBasedCombat.cfg:1](../../Data/TurnBasedCombat/TurnBasedCombat.cfg#L1), [TurnBasedCombatConfiguration.cs:93](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatConfiguration.cs#L93), [TurnBasedCombatManager.cs:243](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs#L243). See the [existing player guide](../wiki/Turn_Based_Combat.md) and its acceptance matrix for deeper mode-specific material; this study did not activate it.

## Settings are inputs, not always the final rule

`MyServerSettings.Configure` reads `Info/settings.xml` in numbered order. The [gameplay settings table](data/gameplay-settings.csv) matches those positions to source fields. Some getters clamp values; some contain arithmetic or ignore values within part of the documented range. A label in the settings file is not enough to establish its effect.

Three examples worth resolving before future tuning:

- `SkillGain()` initializes a local value to zero and assigns 10 only when the input exceeds 10. Inputs from 1 through 10 still return zero. Its caller subtracts the returned value from a gain denominator.
- `BondDays()` returns seven for every input from 0 through 30; negative values return zero and values above 30 return 30. `BondingDays()` describes the raw configured value, so some settings could disagree with behavior. `BaseCreature.BondingDelay` calls `BondDays()`.
- `FoodCheck()` also uses a fixed local default for most values. No call to `FoodCheck()` outside its definition was found in this C# census. The actual hunger/thirst timers must be traced independently; changing this label cannot be assumed to change them.

These are source findings for a future correctness review, not changes made by this study. The current SkillGain=0 and BondDays=7 inputs do not exercise all of the mismatches.

Source: [Settings.cs:127](../../Data/Scripts/System/Misc/Settings.cs#L127), [Settings.cs:1426](../../Data/Scripts/System/Misc/Settings.cs#L1426), [SkillCheck.cs:203](../../Data/Scripts/System/Skills/SkillCheck.cs#L203), [Settings.cs:1439](../../Data/Scripts/System/Misc/Settings.cs#L1439), [BaseCreature.cs:446](../../Data/Scripts/Mobiles/Base/BaseCreature.cs#L446).

## What must be measured in the actual world

The definitions do not answer how many useful spawners exist, how often players encounter them, which travel routes they use, how many old items remain, or whether a supposedly rare discovery is routinely farmed. Record map, named region, route time, encounter source, current configuration, and reward destination alongside the fight itself.

For discovery, also record first success separately from repeated success. A puzzle solved once is a meaningful discovery; the same reward collected fifty times is a production route with a different balance question.
