# Confictura Introduction

Confictura is a heavily customized RunUO shard whose server assembly names it
`Confictura: Legend & Adventure`. That title is the cleanest short answer:
this is a shard about wandering, choosing a fate, surviving strange lands,
building systems of your own, and letting old Ultima Online mechanics become a
platform for a much wider adventure game. ?F:Data/System/Source/AssemblyInfo.cs�L30-L33??

This guide is a new top-level orientation for returning contributors, curious
players, and staff. It favors player-facing identity first, then backs the
claims with source and documentation references so the picture stays tied to
the current codebase.

## The Short Version

Confictura is not an era replica. It is a custom-systems freeshard built on
RunUO, with a multi-map world, unusual character origins, alternate skill caps,
creature-race play, many magic and class fantasies, dynamic dungeon encounters,
champion and invasion events, player-run cities, homestead production, boats,
XML-driven world tools, and a growing canonical character-level layer.

The shard uses RunUO's familiar skeleton, but the experience is closer to a
large Ultima-derived sandbox RPG:

- Pick a fate, not just a template.
- Travel across multiple worlds instead of a single stock map.
- Grow through skills, stats, reputation, class identity, and special cap paths.
- Encounter dungeon pressure that reacts to region, map, time, land type, and
  player level.
- Build civic and domestic life through government, housing, farming, cooking,
  brewing, juicing, and winecrafting.
- Support staff-driven live content through invasions, XMLSpawner, static gumps,
  diagnostics, and a large command surface.

## World Identity

Confictura's map layer is one of the strongest signals that this is its own
world. The engine exposes `Lodor`, `Sosaria`, `Underworld`, `SerpentIsland`,
`IslesDread`, `SavagedEmpire`, `Atlantis`, and `Internal` as maps rather than
only relying on the classic UO facet vocabulary. ?F:Data/System/Source/Map.cs�L69-L99??

The shard also gives those worlds different rules and moods. `SosariaRules`
combine free movement with beneficial and harmful restrictions, while `Lodor`
uses its own rule slot. ?F:Data/System/Source/Map.cs�L35-L40?? The random
encounter documentation shows active dungeon-like encounter coverage across
Lodor, Sosaria, Underworld, Serpent Island, Savaged Empire, and Atlantis, with
hundreds of region-specific encounter entries. ?F:docs/wiki/Random_Encounter_Engine.md�L106-L116??

The player-facing lore reinforces the same structure. Human tarot starts send
characters to places such as Britain, Devil Guard, Grey, Montor, Moon, Mountain
Crest, Umbra, Yew, Lodoria, Elidor, the Savaged Empire, and the Shuttle Crash
Site. These are not just spawn points; the text frames them as life paths with
local history, difficulty, culture, and consequence. ?F:Data/Scripts/Mobiles/Civilized/ShardGreeter.cs�L754-L862??

## Starting Fate

Character creation in Confictura is unusually expressive. The tarot greeter
does not simply ask where a new character begins. It offers a fate. Most starts
are ordinary human starts, but several routes deliberately change the character
model.

The fugitive route starts in the Britain Dungeons and describes a harder life:
guards attack on sight, civilized areas become hostile or restricted,
resurrection costs are doubled, guild access narrows, and the player receives a
larger skill-cap path in exchange for social danger. ?F:Data/Scripts/Mobiles/Civilized/ShardGreeter.cs�L818-L826??
The underlying region and player-setting flow sets the wanted state through
`SetWanted`. ?F:Data/Scripts/System/Regions/WantedRegion.cs�L53-L64?? ?F:Data/Scripts/Mobiles/Base/PlayerSettings.cs�L186-L225??

The savage route starts in the Savaged Empire. It is framed as a barbaric,
survival-first path with extra skill capacity, harsh wilderness, dinosaurs,
dangerous hunting, and starter survival gear. ?F:Data/Scripts/Mobiles/Civilized/ShardGreeter.cs�L845-L853??
The code marks the Savaged Empire discovered, applies the savage skill beginning,
strips normal clothing, sets barbaric identity flags, and grants a barbaric
satchel and gear. ?F:Data/Scripts/Mobiles/Base/PlayerSettings.cs�L50-L78??

The alien route starts at a shuttle crash site and turns Confictura openly
science-fantasy. The character wakes from stasis with no memory, no skills, no
gold, a massive learning ceiling, trouble with magical resurrection, no luck
benefits, and a crash-site survival premise. ?F:Data/Scripts/Mobiles/Civilized/ShardGreeter.cs�L856-L862??
The implementation moves the character to Lodor, applies the alien skill
beginning, strips inventory, resets skills to zero, and rebuilds a sparse
starter kit. ?F:Data/Scripts/Mobiles/Base/PlayerSettings.cs�L136-L165??

Creature-race starts are a separate branch. If `RaceID` is present, the greeter
chooses a starting area such as cave, ice, pits, sand, sea, sky, swamp, tomb,
water, or woods. ?F:Data/Scripts/Mobiles/Civilized/ShardGreeter.cs�L878-L930??
The race system stores those start areas and maps them to readable zone names
like `The Cave`, `The Tundra`, `The Desert`, `The Sea`, `The Mountains`, and
`The Woods`. ?F:Data/Scripts/Mobiles/Races/BaseRace.cs�L740-L812??

## Progression Model

Confictura still has recognizable UO skills, but the skill list itself has been
reshaped. Alongside familiar skills are shard-specific or expanded roles such
as `Druidism`, `Mercantile`, `Searching`, `Psychology`, `Seafaring`,
`Forensics`, `Spiritualism`, `Tasting`, `Bludgeoning`, `FistFighting`,
`Knightship`, `Bushido`, `Ninjitsu`, `Elementalism`, `Mysticism`, and
`Imbuing`. ?F:Data/System/Source/Skills.cs�L38-L96??

The start paths are backed by different cap families. The settings layer gives
default characters a 10000 skill-start value, savage characters 11000,
fugitives 13000, and aliens 40000, then builds `Skills.Cap` from
`SkillStart + SkillBoost + SkillEther`. ?F:Data/Scripts/System/Misc/Settings.cs�L1398-L1411??
The player-facing text describes those as different numbers of skills that can
reach grandmaster-like development: default 10, savage 11, fugitive 13, and
alien 40, adjusted by the global skill boost setting. ?F:Data/Scripts/System/Misc/Settings.cs�L1374-L1395??

The Titan/Ether path adds another progression layer. The Pagan Obsidian reward
raises skill cap by 5000, sets `SkillEther` to 5000, and raises `StatCap` to
300. ?F:Data/Scripts/Quests/Pagan/ApproachObsidian.cs�L54-L64?? Player
deserialization repairs known cap families, including 10000/15000,
11000/16000, 13000/18000, and 40000/45000 skill-cap states, then recomputes
`Skills.Cap` from the stored parts. ?F:Data/Scripts/Mobiles/Base/PlayerMobile.cs�L4564-L4618??

The current codebase has also moved beyond the older duplicate level formulas.
`GetPlayerInfo.GetPlayerLevel` now delegates to the canonical
`CharacterLevelService`, while `GetPlayerDifficulty` continues to derive coarse
difficulty buckets from the visible player level. ?F:Data/Scripts/System/Misc/Players.cs�L893-L910??

The canonical service defines internal archetype lanes for Confictura's broad
class ecosystem: martial, archer, assassin, ninja, samurai, knight, death
knight, arcane mage, elementalist, necromancer, witch, druid, holy man, mystic
monk, Jedi, Syth, researcher, ranger, bard, thief, jester, crafter, gatherer,
merchant, seafarer, creature race, and alien. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L11-L40??
It exposes overall level, archetype level, encounter alias level, and diagnostic
reporting rather than treating total skill alone as the whole character.
?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L99-L224??

The overall score intentionally blends focused adventure power, total skill
progress, stat progress, and bounded fame/karma reputation. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L258-L265??
Skill and stat progress are normalized against the character's actual skill and
stat caps, not a universal default. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L566-L615??
That matters because Confictura has real alternate progression paths, and the
same raw skill total does not mean the same thing for a default, savage,
fugitive, alien, or Ether character.

## Builds And Archetypes

Confictura's build identity is much wider than classic dexxer, mage, bard, and
tamer shorthand. The system audit lists many high-complexity magic and class
systems: Bard Song Magic, Bushido, Death Knight, Druidism, Elementalism,
Enchantments, Holy Man, Jedi, Jester, Knight Chivalry, Mystic, Ninjitsu,
Research Magic, Shinobi, Syth, Witchcraft, and Necromancy. ?F:docs/SystemAudit.md�L48-L64??

The character level service shows how those ideas are being normalized into
staff-visible and encounter-visible archetypes. Martial combat checks weapon
skills with tactics, anatomy, parry, focus, and healing. Archery blends
marksmanship, bowcraft, tactics, anatomy, and focus. Assassin and thief lanes
value stealth, stealing, snooping, lockpicking, hiding, and remove trap.
?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L284-L323?? ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L461-L480??

Magic and spiritual paths are similarly plural. Arcane mage uses magery,
alchemy, inscription, meditation, magic resist, and psychology; elementalist
uses elementalism with meditation, psychology, and focus; necromancer and witch
care about necromancy, spiritualism, poison, forensics, tasting, and dark
karma; holy man and knight paths reward positive karma; Syth and death knight
reward negative karma. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L331-L444??

Nature, support, trade, travel, and oddball roles have a place too. Druid,
ranger, bard, crafter, gatherer, merchant, seafarer, creature race, and alien
all have explicit scoring lanes. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L391-L460?? ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L482-L545??

The practical player takeaway is simple: Confictura is friendly to focused
characters. It wants a fighter to feel like a fighter, a bard to feel like a
bard, a fugitive thief to feel like a different life, and a strange alien or
creature-race character to remain legible inside the same world model.

## Dynamic PvE

The current random encounter engine is one of the clearest recent development
signals. It spawns temporary mobiles or items near eligible players based on
facet, region type, region name, land type, time of day, probability, and
player level. It hot-reloads the active XML after writes, skips hidden players
when configured, avoids staff candidates, and cleans up temporary spawned
objects with rules for combat, tamed creatures, nearby players, and grace
attempts. ?F:docs/wiki/Random_Encounter_Engine.md�L5-L18??

The live random encounter XML is currently dungeon-focused. It uses
`DungeonRegion`, `CaveRegion`, and `BardDungeonRegion` tables and does not use
outdoor or town placeholder defaults. ?F:docs/wiki/Random_Encounter_Engine.md�L5-L9??
The current gate curve is intentionally broad: common patrols at `1:Overall`,
tough common tables at `25:Overall`, dangerous tables around `45:<alias>`,
elite tables around `65:<alias>`, and rare spikes around `85:<alias>`.
?F:docs/wiki/Random_Encounter_Engine.md�L68-L75??

The XML itself shows those tiered gates in practice, including rare
`85:Necromancer` and elite `65:Ranger` entries, followed by `45`, `25`, and `1`
tiers with `scaleUp="false"`. ?F:Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml�L17-L35??
At runtime, the encounter engine calculates the chosen player's level against
the encounter's `LevelType` before spawning that tier. ?F:Data/Scripts/Custom/RandomEncounters/EncounterEngine.cs�L944-L964??
For players, the helper now routes encounter level checks into the canonical
character level service. ?F:Data/Scripts/Custom/RandomEncounters/Helpers.cs�L113-L121??

Champion spawns provide another style of PvE escalation. They build waves that
advance an altar toward a champion boss, reward eligible participants with
power scrolls or Scrolls of Transcendence, and can drop rare artifacts and
champion skulls. ?F:docs/wiki/Champion_Spawns.md�L3-L18??

The invasion system is a staff-run live-event layer. Staff open the invasion
gump with `[invasion]`, choose a destination city, and start or stop large-scale
city assaults that create spawners and waypoint chains for coordinated invader
routes. ?F:docs/wiki/Invasion_System.md�L3-L21??

Monster nests, goliath monsters, relic items, grave robbing, scripted quests,
and large boss/creature systems are all present in the system audit even where
full documentation is not yet complete. ?F:docs/SystemAudit.md�L22-L47?? ?F:docs/SystemAudit.md�L102-L109??

## AI And Creature Behavior

Confictura's AI surface is best understood as an ecosystem. The AI overhaul
audit says the shard does not have one AI system: `BaseCreature`, `BaseAI`,
`Behavior.cs`, engine activation, sector logic, constructor-assigned AI,
`ForcedAI`, and bespoke `OnThink` or combat hooks all combine to define how
creatures behave. ?F:docs/AI_OVERHAUL_AUDIT.md�L16-L35??

That means combat feel is not controlled by one class replacement. Target
selection, activation, pathing, home behavior, perception, bard/control states,
custom encounter hooks, and script-specific logic all matter. The audit calls
`AcquireFocusMob` a targeting choke point, `ChangeAIType` a central assignment
seam for stock shells, and `OnThink` plus combat hooks the main way bespoke
encounter behavior piggybacks on AI timers. ?F:docs/AI_OVERHAUL_AUDIT.md�L27-L35??

OmniAI is the modular custom combat AI layer. It lets creatures react to field
spells, swap weapons, heal themselves, and invoke skill-system abilities such
as magery, necromancy, bushido, knightship, and ninjitsu when their skills
support those modules. ?F:docs/wiki/OmniAI.md�L3-L17??
Creature scripts opt into OmniAI by overriding `ForcedAI` to return
`new OmniAI(this)`. ?F:docs/wiki/OmniAI.md�L18-L25??

## Crafting, Production, And Domestic Play

Confictura is not only combat content. Its production layer is large and
heavily customized.

The Homestead system expands agriculture and food production with growable
crops, cooking, brewing, juicing, and wine making. Players plant crops, wait
through growth stages, harvest produce, use specialized cooking tools, brew
mead/ale/cider, press fruit juice, and ferment grape varieties into wine.
?F:docs/wiki/Homestead_System.md�L3-L35??

The system audit shows a broader trade ecosystem around apiculture, bulk
orders, standard crafting, gardening, grave robbing, trade guild enhancements,
harvest resources, librarian trade, runic tools, shoppes vendors,
stonecrafting, and taxidermy. Some of these are fully documented while others
are still marked missing, but the code surface is clearly there. ?F:docs/SystemAudit.md�L35-L47??

Bulk orders provide classic crafting contracts through small and large BODs.
Players obtain deeds from blacksmith or tailor NPCs, combine matching crafted
items or small deeds into them, and turn completed deeds in for gold, fame, and
reward table items. ?F:docs/wiki/Bulk_Order_System.md�L3-L31??

Offline skill training adds a softer progression loop. Study books can schedule
skill training on logout, grant 0.1 skill every 30 seconds up to a session cap,
apply diminishing returns above 100.0, and grant accelerated gains after long
study sessions. ?F:docs/wiki/offline-skill-training.md�L1-L46??

## Player Society

The Government System lets players found and manage cities. Mayors control
growth, taxes, civic services, city limits, bans, war, alliances, assistant
mayors, and elections. Cities progress through six ranks, from Outpost to
Empire, as their citizen population grows. ?F:docs/wiki/Government_System.md�L3-L33??

Government turns housing and settlement into gameplay. City hall placement has
distance rules and starting treasury requirements, characters with houses
inside city limits can join, and services such as banks, taverns, healers,
stables, and moongates can be enabled by city leadership. ?F:docs/wiki/Government_System.md�L6-L33??

The audit also lists a PvP Consent System, voting rewards, tell-a-friend
referrals, point rewards, access-level tools, and other social or administrative
systems. Not all of them have complete docs yet, but they are part of the shard
surface. ?F:docs/SystemAudit.md�L16-L33??

## Travel, Ships, And Exploration

Confictura has serious travel infrastructure. The boat system manages tiller
men, holds, planks, optional boat doors, anchors, naming, decay, sinking, and
shipwreck creation. Players can issue speech commands such as `forward`,
`backward`, `turn right`, `drop anchor`, `single #`, and `goto #`, and boats can
follow map-pin courses from maps placed in the hold. ?F:docs/wiki/Boat_Core_Mechanics.md�L3-L32??

The system audit adds more exploration-adjacent systems: camping gear, farmable
crops, race tokens, region rules, custom quests, core housing, vendor systems,
seafaring skills, and in-game guide material for sailing, commands, banking,
ranger survival, mage advice, and world basics. ?F:docs/SystemAudit.md�L70-L99??

## Staff And Operations

Confictura is built for active curation. Staff have high-leverage tools for
spawning, diagnostics, live events, region behavior, static editing, city
administration, AI review, and content search.

Key operational commands include:

| Area | Commands | Purpose |
| --- | --- | --- |
| Random encounters | `[rand init]`, `[rand stop]`, `[rand now <RegionType>]`, `[rand import]` | Load, stop, force-check, or import encounter data. ?F:docs/wiki/Random_Encounter_Engine.md�L20-L28?? |
| Character level | `[CharLevel]`, `[CharLevelTarget]` | Show canonical level diagnostics for yourself or a targeted mobile. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelCommands.cs�L12-L35?? |
| Government | `GovHelp`, `AdminAdd`, `FindCities`, `GovTesting`, `UpgradeCitySystem` | Player help, admin item creation, city discovery, testing timers, and city upgrades. ?F:docs/wiki/Government_System.md�L45-L52?? |
| Invasions | `[invasion]` | Open the city invasion gump and start or stop city assaults. ?F:docs/wiki/Invasion_System.md�L6-L21?? |
| XMLSpawner | `[XmlAdd]`, `[XmlEdit]`, `[XmlFind]`, `[XmlHome]`, `[XmlLoad]`, `[XmlLoadHere]`, `[XmlSave]`, `[XmlSaveAll]`, attachment commands | Create, edit, find, load, save, and attach XML-driven spawner content. ?F:docs/wiki/XML_Spawner_Enhancements.md�L3-L18?? |
| Champion spawns | `[add ChampionSpawn]` plus properties such as `Type`, `RandomizeType`, `SpawnArea`, and `ConfinedRoaming` | Create and configure escalating champion encounters. ?F:docs/wiki/Champion_Spawns.md�L10-L18?? |

The XMLSpawner system is especially important for staff identity. It extends
RunUO's standard spawner model with XML-driven definitions, smart spawning,
attachment support, disk import/export, in-game gumps, and administrative
commands. ?F:docs/wiki/XML_Spawner_Enhancements.md�L3-L18??

## Current Development Pulse

The most recent direction in the repo points toward making Confictura more
coherent rather than merely larger.

First, character level is becoming canonical. The older recon report proposed a
single service, public facades, random encounter integration, diagnostics, and
retuned XML gates. ?F:docs/wiki/Character_Level_Recon_Report.md�L190-L207??
The current code now has that service, routes visible player level through it,
and routes random encounter player level checks through it. ?F:Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs�L99-L224?? ?F:Data/Scripts/System/Misc/Players.cs�L893-L901?? ?F:Data/Scripts/Custom/RandomEncounters/Helpers.cs�L113-L121??

Second, random encounters have been moved from placeholder-ish low gates toward
a real 1-100 dungeon curve. The earlier recommendation was to move to
`1/25/45/65/85` after the canonical service existed. ?F:docs/wiki/Character_Level_Recon_Report.md�L179-L188??
The current XML and documentation show that curve in live tables. ?F:Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml�L17-L35?? ?F:docs/wiki/Random_Encounter_Engine.md�L68-L75??

Third, AI work is being treated with caution and respect for the shard's
complexity. The AI audit explicitly frames AI as layered behavior rather than
one replaceable class, and warns that global AI changes affect different
mobiles and encounters unevenly. ?F:docs/AI_OVERHAUL_AUDIT.md�L16-L35??

Fourth, documentation is actively catching up to systems. The wiki index now
collects documented systems such as point commands, range-sensitive
deactivation, offline skill training, boating, random encounters, and character
level work, while the system audit honestly marks many other systems as
missing or stubbed. ?F:docs/wiki/INDEX.md�L1-L16?? ?F:docs/SystemAudit.md�L3-L33??

## What This Means For A Returning Contributor

If it has been a long time since you worked on Confictura, expect the shard to
feel less like a pile of independent custom scripts and more like a game whose
major systems are starting to talk to each other.

The most important mental model is:

1. `Data/System/Source` is the RunUO engine foundation.
2. `Data/Scripts` contains the active game scripts that depend on that engine.
3. `Data/Scripts/Custom` contains many shard-specific systems, but key gameplay
   also lives in core script areas such as `Magic`, `Mobiles`, `Items`,
   `Trades`, `Quests`, and `System`.
4. `docs/wiki` now contains player/staff documentation for several major
   systems, while `docs/SystemAudit.md` is still the broadest inventory of what
   exists and what remains underdocumented.
5. The newest high-signal systems to understand first are character level,
   random encounters, AI/OmniAI, government, homestead, and XMLSpawner.

The safest first reads are:

- `docs/wiki/Random_Encounter_Engine.md` for the current live dynamic PvE model.
- `docs/wiki/Character_Level_Recon_Report.md` plus
  `Data/Scripts/Custom/CharacterLevel/CharacterLevelService.cs` for progression.
- `docs/AI_OVERHAUL_AUDIT.md` before touching creature AI.
- `docs/SystemAudit.md` for the full system inventory and documentation gaps.
- `docs/wiki/Government_System.md`, `docs/wiki/Homestead_System.md`,
  `docs/wiki/Bard_Song_Magic.md`, and `docs/wiki/XML_Spawner_Enhancements.md`
  for major player and staff-facing systems.

## One-Sentence Answer

Confictura is a custom RunUO adventure shard where classic UO mechanics become
a multi-world RPG sandbox: players choose strange fates, grow through
skill-cap-aware archetypes, face curated dynamic PvE, build cities and
homesteads, explore by land and sea, and rely on a deep staff toolchain that
keeps the world adjustable, inspectable, and alive.
