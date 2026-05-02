# Character Level Recon Report

This report is the reconnaissance pass for a unified Confictura player level system. It audits the deployed 1-100 player level formula, the random encounter helper formula, nonstandard starting paths, cap mutations, class and magic systems, and current consumers before any gameplay behavior is changed.

## Executive Summary
- `GetPlayerInfo.GetPlayerLevel` is the visible player level and correctly clamps output to 1-100, but it measures skills against the global default `SkillBase()` path and caps stats at 250. That is useful evidence, not a complete Confictura character model. Source: `Data/Scripts/System/Misc/Players.cs:L892-L960`.
- The random encounter helper has its own level calculation. It ignores `LevelType`, hard-caps skills at 10000, hard-caps stats at 250, and therefore diverges from player display level when `SkillBoost`, `SkillStart`, or `SkillEther` matter. Source: `Data/Scripts/Custom/RandomEncounters/Helpers.cs:L114-L167`.
- Confictura has real cap variants: default 10000 skill cap, savage 11000, fugitive 13000, alien 40000, and Titan/Ether upgrades that add 5000 skill cap and raise stat cap to 300. Sources: `Data/Scripts/System/Misc/Settings.cs:L1398-L1416`, `Data/Scripts/Quests/Pagan/ApproachObsidian.cs:L56-L58`.
- Cap state is persisted and reconstructed after deserialization, so a canonical level system should trust `PlayerMobile.SkillStart`, `SkillBoost`, `SkillEther`, `Skills.Cap`, and `StatCap` when present. Source: `Data/Scripts/Mobiles/Base/PlayerMobile.cs:L4564-L4618`.
- Creature level should stay separate. `IntelligentAction.GetCreatureLevel` uses a related creature formula but clamps to 125 and is consumed by AI, loot, spells, stealing, relics, boats, and creature display logic. Source: `Data/Scripts/Mobiles/Base/Behavior.cs:L1276-L1320`.
- Current random encounter XML uses only `Overall` gates, all `scaleUp="false"`, and very low gates of 1, 3, 5, 8, and 12. Retuning to a true 1-100 curve should happen only after the canonical level service is implemented. Source: `Data/Scripts/Custom/RandomEncounters/RandomEncounters.xml:L19-L33`.

## Bottom Line
The random encounter system is not set up properly for a true 1-100 player level curve yet. It is gated by a duplicate overall formula that ignores class level types and nondefault caps. The player level formula itself is not broken for a default 10000-skill, 250-stat-cap character, but it becomes a blunt signal for savage, fugitive, alien, and Titan-cap characters because it compresses their additional progression into the same capped contribution.

The right next implementation is a single `CharacterLevelService` used by `GetPlayerInfo.GetPlayerLevel` and random encounters. That service should normalize skill progress against actual character cap state, normalize stats against actual `StatCap`, keep fame and absolute karma as bounded reputation modifiers, and expose class/archetype scores behind the existing XML-compatible aliases.

## Current Level Formulas

| Formula | Current behavior | Strength | Weakness | Recommendation |
| --- | --- | --- | --- | --- |
| `GetPlayerInfo.GetPlayerLevel` | Fame capped at 15000, absolute karma capped at 15000, skills clamped to `MyServerSettings.SkillBase()`, skills scaled to 15000, stats capped at 250 and scaled to 15000, then normalized and clamped 1-100. | Stable display number; aware of global `SkillBoost`. | Uses global default cap rather than the player's actual cap family; ignores stat cap 300; reputation can dominate low-skill profiles. | Keep public API as facade, replace internals with canonical service. |
| `GetPlayerInfo.GetPlayerDifficulty` | Buckets player level: 25, 50, 75, 95. | Good coarse gate for quests and scripted difficulty checks. | Sensitive to any level formula shift. | Preserve thresholds initially; regression-test callers after service change. |
| Random encounter `CalculateLevelForMobile` | Duplicate overall formula with hard 10000 skill cap and 250 stat cap; ignores `LevelType`. | Produces similar results to display level for default characters while live `SkillBoost=0`. | Not actually class-aware; unfair to nondefault starts; inconsistent if global skill boost changes. | Replace with canonical service call. |
| `IntelligentAction.GetCreatureLevel` | Creature-focused duplicate formula, hard 10000 skill cap, 250 stat cap, clamp 1-125. | Separate creature tuning range already used by many creature systems. | Not suitable as player level. | Preserve semantics unless a specific caller expects player level. |
| `FameBasedLevel` | Fame divided by 3500 and clamped 1-6. | Simple loot/creature tier helper. | Not a player progression metric. | Keep separate. |

## Start Path Inventory

Current global skill boost is `0`, so the current cap outcomes below use `SkillBoost=0`. If setting 86 changes, `SkillBoost()` returns `setting * 1000`, and each `SkillBegin` result increases by that amount. Sources: `Info/settings.xml:L338-L340`, `Data/Scripts/System/Misc/Settings.cs:L1359-L1371`.

### Human Tarot Starts

| Page | Card | Visible start | Map/location | Start/cap outcome | Notes |
| ---: | --- | --- | --- | --- | --- |
| 1 | THE EMPEROR | City of Britain | Sosaria `(2999,1030,0)` | Default: `SkillStart=10000`, `Skills.Cap=10000`, `StatCap=250` | Normal human start. Character creation already applies default skill cap and stat cap. |
| 2 | THE DEVIL | Town of Devil Guard | Sosaria `(1617,1502,2)` | Default | Normal human start despite ominous card name. |
| 3 | THE HERMIT | Village of Grey | Sosaria `(851,2062,1)` | Default | Normal human start. |
| 4 | THE TOWER | City of Montor | Sosaria `(3220,2606,1)` | Default | Normal human start. |
| 5 | THE MAGICIAN | Town of Moon | Sosaria `(806,710,5)` | Default | Normal human start. |
| 6 | THE FOOL | Town of Mountain Crest | Sosaria `(4546,1267,2)` | Default | Normal human start in a tougher area, but no special cap mutation. |
| 7 | DEATH | Undercity of Umbra | Sosaria `(2666,3325,0)` | Default | Recolors clothing and starts near necromantic content; no cap mutation. |
| 8 | THE SUN | Village of Yew | Sosaria `(2460,893,7)` | Default | Normal human start. |
| 9 | THE HANGED MAN | Britain Dungeons | Sosaria `(4104,3232,0)` | Fugitive via region: `SkillStart=13000`, `Skills.Cap=13000`, `Kills=1`, `Profession=1`, `BardsTaleWin=true` | The card text advertises fugitive rules; the actual mutation occurs when `WantedRegion.OnEnter` calls `PlayerSettings.SetWanted`. Sources: `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:L819-L826`, `Data/Scripts/System/Regions/WantedRegion.cs:L53-L64`, `Data/Scripts/Mobiles/Base/PlayerSettings.cs:L186-L316`. |
| 10 | THE HIEROPHANT | City of Lodoria | Lodor `(2111,2187,0)` | Default | Normal Lodor start. |
| 11 | THE HIGH PRIESTESS | City of Elidor | Lodor `(2930,1327,0)` | Default | Normal Lodor start. |
| 12 | STRNGTH | Savaged Empire | SavagedEmpire `(251,1949,-28)` | Savage via region: `SkillStart=11000`, `Skills.Cap=11000` | The region conversion calls `SetSavage`, which marks the Savaged Empire discovered and removes clothes. Sources: `Data/Scripts/System/Regions/SavageRegion.cs:L53-L67`, `Data/Scripts/Mobiles/Base/PlayerSettings.cs:L50-L55`. |
| 13 | THE STAR | Shuttle Crash Site | Sosaria `(4109,3775,2)`, then crash conversion to Lodor `(7000,4000,0)` | Alien via region: `SkillStart=40000`, `Skills.Cap=40000` | The region conversion calls `SetSpaceMan`, moves the player to Lodor, and removes clothes. Sources: `Data/Scripts/System/Regions/CrashRegion.cs:L48-L64`, `Data/Scripts/Mobiles/Base/PlayerSettings.cs:L136-L143`. |

Location and card evidence: `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:L754-L862`, `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:L1041-L1095`.

### Creature-Race Tarot Starts

Creature starts run through a separate `RaceID > 0` branch. Start location comes from `BaseRace.StartArea`, with cave, ice, pits, sand, sea, sky, swamp, tomb, water, and woods mappings. Source: `Data/Scripts/Mobiles/Civilized/ShardGreeter.cs:L878-L930`.

| Page | Card | World | Start/cap outcome | Notes |
| ---: | --- | --- | --- | --- |
| 1 | THE DAY | Sosaria | Default cap unless race setup changes unrelated attributes | Normal creature start; sets Sosaria discovered and `RaceHomeLand=1`. |
| 2 | THE NIGHT | Sosaria | Fugitive: `SkillStart=13000`, `Kills=1`, `Profession=1`, `BardsTaleWin=true` | Direct gypsy mutation for evil/fugitive creature flow. |
| 3 | THE LIGHT | Lodoria | Default cap | Normal creature start; sets Lodoria discovered and `RaceHomeLand=2`. |
| 4 | THE DARK | Lodoria | Fugitive: `SkillStart=13000`, `Kills=1`, `Profession=1`, `BardsTaleWin=true` | Direct gypsy mutation for evil/fugitive Lodoria creature flow. |

Creature attributes explicitly scale from character level, and the race documentation tells players that level is a mix of total skills, stats, fame, and karma. Sources: `Data/Scripts/Mobiles/Races/RacePotions.cs:L1328-L1342`, `Data/Scripts/Mobiles/Races/BaseRace.cs:L2388-L2404`.

### Cap Families

| Family | SkillStart | SkillEther | Skills.Cap with live boost | StatCap | How reached |
| --- | ---: | ---: | ---: | ---: | --- |
| Default | 10000 | 0 | 10000 | 250 | Character creation and most gypsy human starts. |
| Savage | 11000 | 0 | 11000 | 250 | Savaged Empire card or entering `SavageRegion`. |
| Fugitive | 13000 | 0 | 13000 | 250 | Wanted/Britain Dungeon flow or creature fugitive pages. |
| Alien | 40000 | 0 | 40000 | 250 | Shuttle crash flow or entering `CrashRegion`. |
| Default + Ether | 10000 | 5000 | 15000 | 300 | Titan/Ether quest. |
| Savage + Ether | 11000 | 5000 | 16000 | 300 | Savage character after Titan/Ether quest. |
| Fugitive + Ether | 13000 | 5000 | 18000 | 300 | Fugitive character after Titan/Ether quest. |
| Alien + Ether | 40000 | 5000 | 45000 | 300 | Alien character after Titan/Ether quest. |

Deserialization recognizes the 10000/15000, 11000/16000, 13000/18000, and 40000/45000 cap families and reconstructs `SkillStart`, `SkillBoost`, `SkillEther`, and `Skills.Cap`. Source: `Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1752-L1866`, `Data/Scripts/Mobiles/Base/PlayerMobile.cs:L4564-L4618`.

## Progression Input Inventory

| Input | Current source | Reliability | Level-system implication |
| --- | --- | --- | --- |
| `Skills.Total` | Runtime skill collection | Reliable for current total, but not enough alone. | Normalize against actual cap state, not a universal 10000. |
| `Skills.Cap` | `SkillBegin`, Titan/Ether quest, deserialization repair | Reliable after deserialization repair. | Primary denominator for total skill progress. |
| `SkillStart` | Start family marker | Reliable on `PlayerMobile`; repaired for known cap families. | Useful for diagnostics, start-family fairness, and tests. |
| `SkillBoost` | Server setting 86 times 1000 | Live setting is 0; code supports 0-10000. | Must be included in denominator and regression cases. |
| `SkillEther` | Titan/Ether quest marker | Known value `5000`; repairs stat cap to 300 when present. | Important for 15000/16000/18000/45000 skill caps and stat cap 300. |
| Raw stats | `RawStr + RawDex + RawInt` | Reliable current stat total. | Normalize against `StatCap`; do not hard-cap at 250 for Ether characters. |
| `StatCap` | 250 at creation, 300 after Titan/Ether | Reliable; repaired during deserialization adjustment. | Denominator for stat progress. |
| Fame | Mobile fame | Reliable but volatile. | Keep bounded; should not overpower skill/stat development. |
| Karma | Mobile karma | Reliable but polarity matters for schools. | Overall can use magnitude; class formulas should preserve polarity where relevant. |
| `Profession`, `Kills`, wanted flags | Fugitive setup and guards | Reliable indicators of fugitive path and social restrictions. | Not direct level inputs, but important profile metadata. |
| `RaceID` and creature metadata | Race potion and BaseRace systems | Reliable creature-state indicator. | Race bonuses already consume level, so level shifts affect combat stats. |

## Class And Magic System Survey

The old random encounter enum has `Fighter`, `Ranger`, `Mage`, `Necromancer`, `Thief`, and `Overall`. Confictura has more archetypes than that. The implementation can keep XML compatibility while adding internal archetypes that roll up into those aliases.

| System | Primary signals | Secondary signals and restrictions | Proposed internal archetype | XML-compatible alias |
| --- | --- | --- | --- | --- |
| Weapon fighter | Best of Marksmanship, Fencing, Swords, Bludgeoning, FistFighting | Tactics, Anatomy, Focus, Parry; already represented in unused helper. | Martial | `Fighter` |
| Ranger and beastcraft | Tracking, Taming, Druidism | Cartography, Camping, Veterinary, Herding; Druid spells cast from Druidism and damage from Veterinary. | Ranger/Nature | `Ranger` |
| Magery | Magery | Meditation, Inscribe, Alchemy, MagicResist, Psychology; spellbook gates require natural Magery for normal spellbooks. | Arcane | `Mage` |
| Elementalism | Elementalism | Uses mana and stamina, no reagents, armor fizzle, one element focus, and conflicts with magery, necromancy, wands, runic magic, and research. | Elementalist | `Mage` for XML, separate internal score |
| Bard songs | Musicianship | Provocation, Discordance, Peacemaking; songs use Musicianship as cast and damage skill. | Bard | Likely `Ranger` or `Mage` alias, but should be internally distinct |
| Necromancy | Necromancy | Spiritualism, Poisoning, negative karma; necromancer spells use Necromancy and Spiritualism. | Necromancer | `Necromancer` |
| Witch/undead liquids | Necromancy | Forensics and negative karma pressure. | Witch | `Necromancer` |
| Knight/Paladin | Knightship | Positive karma, tithing, Knightship skill windows. | Knight | `Fighter` or internal Divine |
| Death Knight | Knightship | Negative karma and souls in lantern. | DeathKnight | `Fighter` or `Necromancer`, but internally separate |
| Holy Man | Spiritualism | Healing, positive karma, piety/souls. | Holy/Healer | `Mage` for XML, internal support score |
| Bushido | Bushido | Mana, Samurai Empire support, RequiredSkill windows. | Samurai | `Fighter` |
| Ninjitsu/Shinobi | Ninjitsu | Tithing for Shinobi, stealth mobility, required skill windows. | Ninja/Shinobi | `Thief` |
| Thief | Stealing, Snooping | Lockpicking, Hiding, Stealth, Begging, Searching, RemoveTrap; already represented in unused helper. | Thief | `Thief` |
| Mystic/Monk | FistFighting | Tithing, equipment restrictions, no normal weapons. | Monk/Mystic | `Fighter` or internal Mystic |
| Jester | Begging | Psychology, jester role check, pranks. | Jester | `Thief` or internal Trickster |
| Jedi | Psychology | Swords, Tactics, positive karma, Jedi gear, special power damage from karma/Tactics/Swords. | Jedi | `Fighter` or internal ForceLight |
| Syth | Psychology | Swords, Tactics, negative karma, Syth gear, special power damage from negative karma/Tactics/Swords. | Syth | `Fighter` or internal ForceDark |
| Research/enchantments | Research-specific systems | Elementalism explicitly blocks magic research. | Scholar/Research | `Mage` if ever exposed |
| Creature-race magic | Race-specific effects and creature stats | Race attributes scale from character level and may have family/alignment constraints. | RaceAffinity | No current XML alias; should affect only race scaling unless explicitly needed |

Evidence examples: existing unused class scoring lives in `Data/Scripts/Custom/RandomEncounters/Helpers.cs:L170-L280`; bard skills are in `Data/Scripts/Magic/Bard/SongSpells.cs:L61-L105`; elementalism restrictions are described in `Data/Scripts/Magic/Elementalism/Gumps/ElementalSpellbookGump.cs:L771-L773`; spellbook gates are in `Data/Scripts/Magic/Magery/Spellbook.cs:L727-L829`; Druid, Knight, Death Knight, Holy Man, Ninja, Shinobi, Mystic, Jester, Jedi, and Syth base classes expose the primary skills in their respective spell base files.

## Existing Consumers And Risk Audit

| Consumer | Files/call sites | Use type | Sensitivity | Recommendation |
| --- | --- | --- | --- | --- |
| Player info display | `Data/Scripts/System/Misc/Players.cs:L1641`, `L1787` | Display-only | Medium; players will notice level shifts. | Switch to service but document formula change. |
| Player difficulty buckets | `Players.cs:L963-L985`, quest/courier callers from `rg` | Quest gating and reward/difficulty selection | High; buckets can change behavior. | Keep thresholds initially; run synthetic profiles through old/new buckets. |
| Creature-race attributes | `Data/Scripts/Mobiles/Races/BaseRace.cs:L2388-L2404` | Combat/stat scaling | High; level changes directly alter resist bonuses. | Add explicit race-scaling regression profiles. |
| Random encounter gates | `Data/Scripts/Custom/RandomEncounters/EncounterEngine.cs:L946-L983` | Encounter eligibility | High after XML retune; currently low gates hide most differences. | Replace helper with service before retuning XML. |
| Random encounter scale-up | `Data/Scripts/Custom/RandomEncounters/EncounterEngine.cs:L1223-L1235` | Potential combat scaling | Low live risk because XML is `scaleUp=false`; high if enabled later. | Fix to use encounter level type when scale-up is introduced. |
| Random encounter import | `Data/Scripts/Custom/RandomEncounters/Import.cs:L350` | Legacy generated XML levels | Low unless importer is reused. | Update after service, or mark importer legacy. |
| Resurrection cost | `Data/Scripts/System/Misc/Players.cs:L988-L1045` | Economy and penalty scaling | Medium; similar formula but different penalties. | Audit separately before unifying. |
| Jester Hilarity | `Data/Scripts/Magic/Jester/Spells/Hilarity.cs:L160-L164` | Spell effect level lookup | Medium; mixes creature and player level. | Verify expected target branch before changing. |
| Creature level consumers | `rg` found AI, loot, spell, item, and boat callers | Creature AI/rewards/combat | High if changed. | Do not replace creature semantics with player service. |
| Fame-based level consumers | Many creature constructors and loot chest calls | Creature reward/tier helper | Low for player level; high for loot if touched. | Leave alone. |

## Representative Profile Matrix

The candidate column below is not a final formula. It is a reconnaissance model used to test whether cap-aware normalization produces a more believable distribution than the current formula. It uses:

`overall = clamp(1, 100, round(1 + 99 * (0.60 * skillPower + 0.25 * statProgress + 0.15 * reputationPower)))`

Where `skillPower = 0.75 * bestClassPower + 0.25 * totalSkillProgress`, `totalSkillProgress = Skills.Total / Skills.Cap`, `statProgress = RawStats / StatCap`, and `reputationPower = (positive fame cap + absolute karma cap) / 30000`.

| Profile | Skill progress | Stat progress | Fame/Karma | Class core | Current level | Candidate cap-aware level |
| --- | --- | --- | --- | --- | ---: | ---: |
| New normal | 500/10000 (5%) | 90/250 (36%) | 0/0 | 10% | 1 | 15 |
| Developed normal | 7000/10000 (70%) | 225/250 (90%) | 5000/5000 | 75% | 53 | 72 |
| Max normal | 10000/10000 (100%) | 250/250 (100%) | 15000/15000 | 100% | 100 | 100 |
| Savage cap | 11000/11000 (100%) | 250/250 (100%) | 10000/5000 | 90% | 73 | 88 |
| Fugitive cap | 13000/13000 (100%) | 250/250 (100%) | 10000/-10000 | 85% | 82 | 88 |
| Alien partial | 20000/40000 (50%) | 225/250 (90%) | 3000/0 | 60% | 47 | 59 |
| Alien max | 40000/40000 (100%) | 250/250 (100%) | 15000/15000 | 100% | 100 | 100 |
| Titan ether | 15000/15000 (100%) | 300/300 (100%) | 15000/15000 | 100% | 100 | 100 |
| Focused fighter | 6500/10000 (65%) | 245/250 (98%) | 7000/3000 | 95% | 53 | 82 |
| Focused mage | 6500/10000 (65%) | 220/250 (88%) | 7000/3000 | 95% | 50 | 80 |
| Focused thief | 6500/10000 (65%) | 210/250 (84%) | 5000/-5000 | 95% | 49 | 79 |
| Hybrid bard/mage | 9000/10000 (90%) | 235/250 (94%) | 8000/8000 | 80% | 71 | 81 |
| Crafter-heavy | 10000/10000 (100%) | 230/250 (92%) | 1000/1000 | 35% | 46 | 55 |
| High fame low skill | 2000/10000 (20%) | 150/250 (60%) | 15000/15000 | 20% | 67 | 43 |

Important reading: the current formula makes a high-fame, low-skill character level 67 because reputation has the same maximum contribution as skill and stats. A cap-aware candidate gives that profile a lower level while giving focused combat/caster/thief profiles much stronger dungeon readiness.

## Formula Comparison Matrix

| Concern | Current player level | Current random encounter helper | Candidate unified overall | Candidate class formulas |
| --- | --- | --- | --- | --- |
| Output range | 1-100 | 1-100 | 1-100 | 1-100 per archetype |
| Skill denominator | `MyServerSettings.SkillBase()` | Hard 10000 | Actual `Skills.Cap`, with `SkillStart/SkillBoost/SkillEther` diagnostics | Skill cap or 120-style per-skill caps, depending on skill API reliability |
| Stat denominator | Hard 250 | Hard 250 | Actual `StatCap` | Actual `StatCap` where stats are part of archetype |
| Fame | Positive fame capped 15000 | Positive fame capped 15000 | Positive fame capped 15000, lower weight | Only for archetypes where reputation is a power source |
| Karma | Absolute karma capped 15000 | Absolute karma capped 15000 | Absolute karma capped 15000 for overall | Polarity-sensitive for Jedi, Syth, Holy Man, Death Knight, Necromancy, Witch |
| Class support | None | `LevelType` ignored | Uses best class power as part of skill power | XML aliases backed by internal archetypes |
| Nondefault starts | Compressed | Compressed | Fair against actual caps | Fair per role, not total-cap dependent |
| Random encounter readiness | Current XML gates are too low for 1-100 | Same, plus duplicate formula | Meaningful after XML retune | Enables future class-specific encounter gates |

## Random Encounter Retuning Recommendation

Do not retune XML gates until a canonical service exists. The current random encounter XML uses `Overall` levels only and `scaleUp="false"` everywhere, so class formulas can be introduced without changing live XML semantics. The current gate set of 1, 3, 5, 8, and 12 was reasonable for a placeholder-ish formula but is too compressed for a real 1-100 model.

After the service is in place, use `1/25/45/65/85` as the first retuning curve for common, tough, dangerous, elite, and rare spike tiers. The representative profiles suggest those gates separate beginners, developed characters, focused dungeon builds, and endgame profiles more cleanly than the current XML. Validate against sampled live characters before committing the XML retune.

Systems that should probably keep difficulty buckets instead of raw level:
- Quest eligibility checks that already compare against `GetPlayerDifficulty`.
- Courier and standard quest selection.
- Any content whose design intent is coarse challenge bands rather than fine-grained level gating.

## Proposed Implementation Shape

1. Add a canonical service with methods similar to:
   - `GetOverallLevel(Mobile m)`
   - `GetClassLevel(Mobile m, CharacterArchetype archetype)`
   - `GetEncounterLevel(Mobile m, LevelType xmlType)`
   - `GetDiagnostics(Mobile m)` for staff/admin review.
2. Keep `GetPlayerInfo.GetPlayerLevel` and `GetPlayerDifficulty` as public facades so existing call sites do not churn.
3. Replace random encounter `Helpers.CalculateLevelForMobile` internals with the service, preserving `LevelType.Overall` behavior until XML retune.
4. Preserve `IntelligentAction.GetCreatureLevel` and `FameBasedLevel` semantics.
5. Add explicit mappings from XML aliases to internal archetypes:
   - `Fighter`: Martial, Knight, DeathKnight, Samurai, Jedi/Syth martial score.
   - `Ranger`: Ranger/Nature and possibly Bard support if no `Bard` XML enum is added.
   - `Mage`: Arcane, Elementalist, Holy/Mystic support if no new enums are added.
   - `Necromancer`: Necromancer, Witch, DeathKnight dark support.
   - `Thief`: Thief, Ninja/Shinobi, Jester/Trickster.
   - `Overall`: cap-aware overall.
6. Add staff-visible diagnostics before changing random encounter XML so outlier profiles can be inspected in game.

## Test And Validation Plan

Build a non-mutating analysis harness before code replacement:
- Synthetic profile tests for every profile in the matrix above.
- Deserialization-style cap reconstruction profiles for 10000/15000, 11000/16000, 13000/18000, and 40000/45000.
- Optional safe sampled character data from saves, read-only, if available in a development copy.

Validate invariants:
- Overall level always returns 1-100.
- Increasing total skill progress never lowers overall level when cap is unchanged.
- Increasing stat progress never lowers overall level.
- Increasing fame or moving karma farther from zero never lowers overall level, unless a class formula intentionally requires the opposite polarity.
- Savage, fugitive, alien, and Ether characters are judged against their actual caps.
- Focused builds score high in their class without requiring unrelated skills.
- Hybrid builds get fair overall credit without becoming best-in-every-class.
- Crafter-heavy profiles do not look like elite combat profiles solely because total skill is high.
- Race scaling remains bounded.
- `GetPlayerDifficulty` bucket changes are reviewed for every representative profile.
- Random encounter access tiers are checked before and after XML retune.

Run after later implementation:
- Static compile if possible: `msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform=x86`.
- Unit-style synthetic harness, if the repo gets a lightweight script test surface.
- Text checks that random encounter XML still uses only supported `LevelType` values.

## Open Questions For Implementation

| Question | Why it matters | Proposed answer for first pass |
| --- | --- | --- |
| Should total skill progress or best class power dominate overall level? | Focused fighters should not be punished for avoiding crafting, but crafters should not be treated as dungeon elites by total skill alone. | Use both: class power as the main skill signal, total progress as broad support. |
| Should fame and karma remain equal in weight to skill/stat growth? | Current formula can make high-fame, low-skill characters mid-high level. | Keep reputation, but reduce its overall weight and reserve polarity effects for class formulas. |
| Should alien 40000-cap characters level slowly because their cap is huge? | Alien starts can learn far more total skills, but a half-built alien should not automatically be endgame. | Normalize against actual cap; add class formulas so focused aliens still rate appropriately in their chosen role. |
| Should `LevelType` gain new XML enum values? | Confictura has Bard, Elementalist, Jedi/Syth, Mystic, Jester, and other systems. | Not in first pass. Add internal archetypes and keep XML aliases stable. |
| Should creature level be unified? | Many creature systems rely on the existing 1-125 creature curve. | No. Audit only; preserve. |

## Decision
Proceed to implementation only after this report is reviewed. The implementation should create a canonical player level service, leave creature level separate, route random encounters through the service, add diagnostics and synthetic profile tests, and postpone random encounter XML gate retuning until the new level distribution is visible.
