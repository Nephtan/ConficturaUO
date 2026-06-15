# System Name: Creature Transformation Guide

## Overview

The Creature Transformation Guide is the player-facing help and selection surface for creature race characters. It is built around the constructable `RacePotions` `Item`, the nested `RacePotionsGump`, and `CreatureHelpGump`. The guide text describes what the shelf does, while the actual transformation mechanics are executed by `BaseRace.CreateRace()` and `BaseRace.BackToHuman()`.

The guide has no `[Command]` entry point. Players reach it by double-clicking the gypsy potion shelf, by pressing the shelf help button, through the built-in library entry, or through Help settings while already playing a creature.

Code-Verified: 2026-05-09

## Script Inventory

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Races/RacePotions.cs` | Defines the gypsy potion shelf, race preview/selection gump, and guide text strings. |
| `Data/Scripts/Mobiles/Races/BaseRace.cs` | Defines creature race records, unlock filtering, ability text, race token creation, human reversion, level scaling, and token serialization. |
| `Data/Scripts/System/Commands/Player/CreatureHelp.cs` | Defines `CreatureHelpGump`, which renders `RacePotions.RaceHelp()` and `RacePotions.RaceEquipment()`. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Exposes Creature Magic, Creature Type, and Creature Sounds settings. |
| `Data/Scripts/System/Misc/Settings.cs` | Provides `MonsterCharacters()` and `MonstersAllowed()` setting gates. |
| `Data/System/Source/Mobile.cs` | Persists `RaceID`, race sounds, race settings, and current shelf page. |

`Data/Scripts/Scripts.csproj` explicitly compiles `BaseRace.cs`, `NPCRace.cs`, and `RacePotions.cs`.

## Entry Points

| Entry point | Code requirement | Behavior |
| --- | --- | --- |
| Double-click `RacePotions` | Caller must be within 4 tiles and `MyServerSettings.MonstersAllowed()` must be true. | Ensures `Mobile.RaceSection >= 1`, closes `GypsyTarotGump`, `WelcomeGump`, and old `RacePotionsGump`, then opens `RacePotionsGump(from, 0)`. |
| Shelf help button | `RacePotionsGump` button ID `9999`. | Reopens the shelf gump and opens `CreatureHelpGump(from, m_Tavern)`. |
| Built-in library | `MyLibrary` button `401`. | Opens `CreatureHelpGump(from, 0)`. |
| Help settings: Creature Type | `RaceID > 0` and caller is in region name `the Tavern`, or in Sosaria rectangle `X 6982..6999`, `Y 694..713`. | Sets `RaceSection = 1` and opens `RacePotionsGump(from, 1)`, the tavern-limited appearance selector. |
| Help settings: Creature Magic | `RaceID > 0`, region name `the Tavern`, and the current race has Magery or Necromancy in its raw skill slots. | Cycles `RaceMagicSchool` through Default, Magery, Necromancy, and Elementalism, then reapplies race skill bonuses. |
| Help settings: Creature Sounds | `RaceID > 0`. | Toggles `RaceMakeSounds` and reopens the Help settings gump. |

## Server Setting Gates

| Setting | Behavior |
| --- | --- |
| `MyServerSettings.MonsterCharacters()` | Returns the configured creature-character tier. The default traced value is `1`. |
| `MyServerSettings.MonstersAllowed()` | Returns true only when `MonsterCharacters() > 0`. |
| Race row `Size` | `BaseRace.GetMonsterSize()` allows a creature record only when `Size <= MonsterCharacters()`. |
| Tavern mode species filter | When `RacePotionsGump` is opened with `tavern > 0`, allowed creature records must match the player's current `SpeciesFamily`, except the current body can still be shown. |

The Human page is always reachable inside the gump and its OK button calls `BaseRace.BackToHuman()`.

## Gump Flow

`RacePotionsGump` uses `Mobile.RaceSection` as the current page. Page `1` is the Human preview. Creature pages map to race records by setting `race = 79999 + page`; pages `2..173` correspond to the 172 hard-coded creature records.

The preview gump shows:

| Display area | Source |
| --- | --- |
| Species | `BaseRace.SpeciesFamily`, capitalized. |
| Alignment | `BaseRace.SpeciesAlignment`, color-coded good, neutral, or evil. |
| Creature image | `SpeciesIcon`, centered by `SpeciesWide` and `SpeciesHigh`. |
| Paperdoll image | `50000 + SpeciesGump`. |
| Ability text | `BaseRace.GetAbilities(SpeciesID)`. |
| OK button | `80000 + SpeciesID` for creatures, `1000` for Human. |

Next and previous arrows skip creature records blocked by `GetMonsterSize()`. Category buttons jump `RaceSection` near the first record for that family.

## Transformation Mechanics

| Action | Runtime effect |
| --- | --- |
| Choose a creature | `RacePotionsGump.OnResponse()` subtracts `80000` from the button ID and calls `BaseRace.CreateRace(from, race, true)`. |
| Create a new race token | `CreateRace()` creates a configured `BaseRace` token from `GetCostume(id)`, deletes the current `Layer.Special` item, deletes other `BaseRace` world items owned by the same player, adds the token to the backpack, and equips it. |
| Refresh existing token | If a `BaseRace` is already equipped, `CreateRace()` refreshes ownership, `BodyMod`, `HueMod`, `RaceID`, and race sound fields from the token. |
| Equip race token | `BaseRace.OnEquip()` sets `BodyMod = SpeciesID`, `HueMod = 0`, `RaceID = SpeciesID`, copies race sound IDs, forces `Female` from `SpeciesFemale`, and dismounts the player. |
| Return to Human | `BackToHuman()` deletes the current special-layer item, clears body/hue/race/sound/home fields, and restores `Female` from `RaceWasFemale`. |

`CreateRace()` only runs for living `PlayerMobile` instances. If the race is disabled later, `PlayerMobile.Deserialize()` clears creature race state and `BaseRace.Deserialize()` schedules loaded race tokens for deletion.

## Guide Text Rules

The guide text is generated by `RacePotions.RaceHelp(int origin)` and rendered by `CreatureHelpGump`.

| Topic | Source behavior |
| --- | --- |
| Shelf usage | Included only when `origin < 1`; describes using shelf arrows and OK buttons to select a creature. |
| Equipment | `RaceEquipment()` explains that creature equipment appears as paperdoll icons rather than visible worn art. |
| Mounts | The guide states creature characters cannot ride mounts; `OnEquip()` enforces this by dismounting. |
| Level scaling | The guide says creature attributes fluctuate by character level; `BaseRace.SetProperties()` performs the scaling. |
| Food rules | The guide describes undead, plants, vampires, and brain-eating creatures; the exact behavior comes from race food codes. |
| Alignment | The guide distinguishes good, neutral, and evil starts; the exact alignment is stored per race row. |
| Later settings | The guide points players to Creature Sounds, Creature Magic, and Creature Type settings in the Help gump. |
| Superficial race looks | Included only when `origin < 1`; describes masks, helms, and hue-changing items as cosmetic only. |

## Family Catalog

`BaseRace.RaceDefined()` contains 172 creature records. The table below groups those records by `SpeciesFamily`. `Min setting` is the category button's minimum `MonsterCharacters()` value; individual appearances still require their own `Size` code to be less than or equal to `MonsterCharacters()`.

| Family | Min setting | Appearances | Display names | Alignment | Start | Size codes | Food rules | Skill bonuses |
| --- | ---: | ---: | --- | --- | --- | --- | --- | --- |
| aquatic | 1 | 5 | Lurker, Neptar, Tritun | neutral | sea | 1 | Normal | Alchemy +10, Seafaring +10, Tactics +10 |
| bugbear | 1 | 1 | Bugbear | neutral | cave | 1 | Normal | ArmsLore +10, Tactics +10 |
| centaur | 1 | 1 | Centaur | good | woods | 1 | Normal | Tactics +10, Marksmanship +10 |
| cyclops | 2 | 3 | Cyclops | neutral | sky | 2, 3 | Normal | Searching +10, Tactics +10 |
| daemon | 2 | 10 | Balron, Daemon | evil | ice, pits, sea | 2, 3 | Normal | Seafaring +10, MagicResist +10 |
| dagon | 2 | 2 | Dagon | evil | sea | 2 | Normal | Seafaring +10, Magery +10 |
| demon | 1 | 6 | Devil Kin, Shadow Demon, Demon, Devil | evil | pits | 1, 2 | Normal, No food/drink | MagicResist +10 |
| devil | 3 | 5 | Balron, Devil | evil | ice, pits, sea | 3 | Normal | MagicResist +10 |
| dragon | 2 | 1 | Dragon Ogre | neutral | cave | 2 | Normal | None |
| drakkul | 1 | 3 | Drakkul | neutral | cave | 1, 2 | Normal | None |
| ettin | 2 | 7 | Ettin | neutral | cave, ice | 2, 3 | Normal | Magery +10, Tactics +10 |
| fey | 1 | 3 | Fairy, Pixie | good, neutral | woods | 1 | Normal | Magery +10, Snooping +10, Stealing +10 |
| gargoyle | 1 | 4 | Astral Gargoyle, Gargoyle | neutral | pits | 1, 2 | Normal | None |
| giant | 3 | 13 | Abysmal Giant, Cloud Giant, Earth Giant, Fire Giant, Forest Giant, Frost Giant, Hill Giant, Jungle Giant, Sand Giant, Sea Giant, Stone Giant | neutral | cave, ice, pits, sand, sea, sky, swamp, woods | 3 | Normal | Alchemy +10, Blacksmith +10, Camping +10, Searching +10, Seafaring +10, Magery +10, Tactics +10, Lumberjacking +10, Mining +10 |
| gnoll | 1 | 1 | Gnoll | neutral | cave | 1 | Normal | Tactics +10 |
| goblin | 1 | 3 | Goblin | neutral | cave | 1 | Normal | Hiding +10, Stealth +10 |
| golem | 1 | 2 | Flesh Golem | evil | tomb | 1, 2 | Normal | Forensics +10 |
| hobgoblin | 1 | 1 | Hobgoblin | neutral | cave | 1 | Normal | Tactics +10 |
| illithid | 1 | 1 | Mind Flayer | evil | cave | 1 | Fresh brains | Magery +10, Necromancy +10 |
| imp | 1 | 2 | Imp | neutral | pits | 1 | Normal | Magery +10, MagicResist +10 |
| kilrathi | 1 | 1 | Kilrathi | neutral | woods | 1 | Normal | Hiding +10, Stealth +10 |
| kobold | 1 | 3 | Kobold | neutral | cave | 1 | Normal | Lockpicking +10, Magery +10 |
| minotaur | 1 | 6 | Minotaur | neutral | cave | 1 | Normal | Searching +10, Tactics +10 |
| mummy | 1 | 2 | Mummy | evil | sand | 1, 2 | No food/drink | Forensics +10, Tactics +10 |
| mushroom | 1 | 2 | Fungal | neutral | cave | 1 | No food | Alchemy +10, Tasting +10 |
| naga | 1 | 3 | Naga | neutral | pits, sea | 1 | Normal | Magery +10 |
| ogre | 1 | 3 | Ogre | neutral | cave | 1, 2 | Normal | Tactics +10 |
| orc | 1 | 10 | Orc, Urk | neutral | cave | 1 | Normal | ArmsLore +10, Magery +10, Tactics +10, Meditation +10 |
| owlbear | 2 | 1 | Owlbear | neutral | woods | 2 | Normal | Druidism +10 |
| plant | 1 | 2 | Shambler, Swamp Thing | evil | swamp | 1, 2 | No food | Alchemy +10, Tasting +10 |
| reptilian | 1 | 8 | Grathek, Lizardman, Sakkhra, Sleestax | neutral | swamp | 1 | Normal | ArmsLore +10, Tactics +10 |
| revenant | 2 | 1 | Revenant | evil | tomb | 2 | No food/drink | Tactics +10, Necromancy +10 |
| rodent | 1 | 7 | Ratman | neutral | cave | 1 | Normal | Magery +10, Snooping +10, Stealing +10, Meditation +10 |
| salamander | 1 | 1 | Salamander | neutral | pits | 1 | Normal | None |
| satyr | 1 | 1 | Satyr | good | woods | 1 | Normal | Peacemaking +10, Musicianship +10 |
| serpent | 1 | 7 | Ophidian, Serpyn | neutral | sea, swamp | 1, 2 | Normal | Seafaring +10, Magery +10, Poisoning +10 |
| skeleton | 1 | 12 | Skeleton | evil | tomb | 1 | No food/drink | Magery +10, Tactics +10, Necromancy +10 |
| sphinx | 1 | 2 | Sphinx | neutral | sand | 1, 2 | Normal | Magery +10, MagicResist +10 |
| succubus | 1 | 3 | Succubus | evil | pits | 1, 2 | Normal | MagicResist +10, Tactics +10 |
| titan | 2 | 2 | Titan | neutral | sky | 2, 3 | Normal | Magery +10 |
| tree | 2 | 5 | Ent, Reaper | good, neutral | woods | 2, 3 | No food | Druidism +10 |
| troll | 1 | 6 | Troll | neutral | cave, ice, woods | 1 | Normal | None |
| vampyre | 1 | 3 | Vampyre | evil | tomb | 1, 2 | Fresh blood | Magery +10, Tactics +10, Necromancy +10 |
| zombi | 1 | 7 | Zombi, Ghoul, Wight | evil | sea, tomb | 1 | Fresh brains | Anatomy +10, Seafaring +10, Forensics +10, Tactics +10, Poisoning +10, Spiritualism +10, Necromancy +10 |

Food rules map to code `0 = normal`, `1 = no food`, `2 = no food or drink`, `3 = fresh blood`, and `4 = fresh brains`.

## Race Definition Schema

Each hard-coded creature row uses 44 comma-separated fields.

| Entry | Field | Runtime mapping |
| --- | --- | --- |
| 1 | Name | `BaseRace.Name`. |
| 2 | Index | `SpeciesIndex`. |
| 3 | ItemID | Race token `ItemID`. |
| 4 | Gump | `SpeciesGump`; paperdoll art uses `50000 + SpeciesGump`. |
| 5 | Body | `SpeciesID`; applied to `Mobile.BodyMod` and `Mobile.RaceID`. |
| 6 | Icon | `SpeciesIcon`. |
| 7-8 | x/y | `SpeciesWide` and `SpeciesHigh` preview offsets. |
| 9 | Sound | Base sound ID; normal races derive anger, idle, death, attack, and hurt sounds from this value. Raw `9999` uses a mushroom-specific sound set. |
| 10-13 | Species, Alignment, Start, Size | Family metadata and unlock tier. |
| 14-18 | Phy/Fir/Cld/Poi/Eny | Each raw value is multiplied by `5` into AOS resistances. |
| 19-24 | Str/Dex/Int/Hits/Stam/Mana | Each raw value is multiplied by `5` into matching AOS attributes. |
| 25-28 | RegHits/RegStam/RegMana/Night | Direct regeneration and NightSight attributes. |
| 29-30 | Attack/Defend | Each raw value is multiplied by `5`. |
| 31-32 | CastRecover/CastSpd | Direct casting attributes. |
| 33-35 | Potions/LowMana/LowReg | Each raw value is multiplied by `5`. |
| 36 | Luck | Raw value is multiplied by `300`. |
| 37-40 | Reflect/SpellDmg/WepDmg/WepSpeed | Each raw value is multiplied by `5`. |
| 41-42 | Skill1/Skill2 | Raw skill code maps through `RaceSkill()` and grants `+10`; code `100` means no skill. |
| 43 | Food | Food-rule code. |
| 44 | Gender | `1` forces female; all other values force male. |

## Level Scaling

`BaseRace.SetProperties()` recalculates the equipped token when the player's current level differs from `SpeciesLevel`. Levels below `2` are treated as `0`, and all casts truncate fractional values.

| Base property, when positive | Recalculated value |
| --- | --- |
| Physical, Fire, Cold, Poison, Energy resistances | `base + (int)(level * 0.1)` |
| AttackChance, BonusDex, BonusInt, BonusStr, DefendChance, SpellDamage, WeaponDamage | `base + (int)(level * 0.1)` |
| BonusHits, BonusMana, BonusStam | `base + (int)(level * 0.3)` |
| CastRecovery, CastSpeed, RegenHits, RegenMana, RegenStam | `base + (int)(level * 0.03)` |
| EnhancePotions | `base + (int)(level * 0.4)` |
| LowerManaCost, LowerRegCost | `base + (int)(level * 0.3)` |
| Luck | `base + (int)(level * 5)` |
| ReflectPhysical, WeaponSpeed | `base + (int)(level * 0.2)` |
| Skill bonus values | `base + (int)(level / 2)` |

`PlayerMobile` calls `BaseRace.SyncRace()` on movement, resurrection, and death. Alive players with missing body or missing token state have the token recreated from `RaceID`; dead players have the equipped `BaseRace` token deleted while `RaceID` remains for later restoration.

## Creature Magic Focus

Creature Magic applies only to race rows whose raw skill slots include Magery or Necromancy. The Help setting cycles:

| `RaceMagicSchool` | Label | Skill-slot rewrite |
| ---: | --- | --- |
| `0` | Default | Leaves Magery and Necromancy as defined by the race row. |
| `1` | Magery | Rewrites a single magic slot to Magery. |
| `2` | Necromancy | Rewrites a single magic slot to Necromancy. |
| `3` | Elementalism | Rewrites a single magic slot to Elementalism; if both Magery and Necromancy are present, slot 1 becomes Elementalism and slot 2 becomes Meditation. |

The Help setting is visible only in taverns and only for creatures that pass `BaseRace.GetMonsterMage(RaceID)`.

## Persistence

| Object | Version/payload |
| --- | --- |
| `RacePotions` | Writes version `1`; no custom fields after the version. |
| `BaseRace` | Writes version `0`, then serializes AOS attributes, resistances, skill bonuses, owner, species metadata, sounds, level, food, and gender. |
| `Mobile` | Persists `RaceMakeSounds`, `RaceMagicSchool`, `RaceWasFemale`, `RaceSection`, `RaceHomeLand`, `RaceID`, and race sound fields. |

## Known Issues

* `RacePotionsGump.OnResponse()` trusts any button ID above `80000` and passes `ButtonID - 80000` to `BaseRace.CreateRace()` without revalidating the selected race against `MonsterCharacters()`, tavern species restrictions, current range, or current server enablement.
* `RacePotionsGump.OnResponse()` and `CreatureHelpGump.OnResponse()` read `state.Mobile`/`sender.Mobile` and use the returned `Mobile` without null, deleted, or stale-session guards.
* `BaseRace.CreateRace()` deletes whatever item is currently on `Layer.Special` before equipping the new `BaseRace`, so unrelated special-layer systems can be removed during race changes.
* `BaseRace.OnEquip()` calls `base.OnEquip(m)` once for the condition and again as the return value, which can duplicate any base equip side effects.
* Forged or invalid race button IDs can create an unconfigured `BaseRace` token because `GetCostume()` returns a blank token when `RaceDefined()` has no row for the supplied body.
* `RacePotions.Serialize()` writes version `1` even though the item has no custom payload and no guarded version branches. This is harmless for the current format but leaves no documented reason for the bumped version.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0087.
- Backlog rows: RB-06683.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Mobiles/Races/RacePotions.cs (CurrentFile)
- Data/Scripts/Mobiles/Races/BaseRace.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/CreatureHelp.cs (CurrentFile)
- Data/Scripts/System/Help/Gumps/HelpGump.cs (CurrentFile)
- Data/Scripts/System/Misc/Settings.cs (CurrentFile)
- Data/System/Source/Mobile.cs (CurrentFile)
- Data/Scripts/Scripts.csproj (CurrentFile)

### Runtime Evidence

- Hook summary: Event=11; Gump=87; Initialize=1; Movement=3; Speech=7; Timer=14; WorldLoad=1.
- Data/Scripts/Mobiles/Races/BaseRace.cs:L4069 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L39 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1057 Gump OnResponse access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1068 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1258 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1263 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1264 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1270 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Races/RacePotions.cs:L1315 Gump SendGump access=Internal
- Data/Scripts/System/Commands/Player/CreatureHelp.cs:L68 Gump OnResponse access=Internal
- Data/Scripts/System/Commands/Player/CreatureHelp.cs:L74 Gump SendGump access=Internal
- Data/Scripts/System/Help/Gumps/HelpGump.cs:L42 Gump OnResponse access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 3.
- Data/Scripts/Mobiles/Races/BaseRace.cs:Server.Items.BaseRace version=0 serialize=L3975 deserialize=L4003 alignment=CountMatchNeedsTypeReview:UnknownWrites=18
- Data/Scripts/Mobiles/Races/RacePotions.cs:Server.Items.RacePotions version=1 serialize=L47 deserialize=L53 alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Mobile.cs:Server.Mobile version=35 serialize=L6183 deserialize=L5700 alignment=CountMismatch:Writes=98;Reads=104

### Project And Runtime Coverage

- Data/Scripts/Mobiles/Races/BaseRace.cs=Keep
- Data/Scripts/Mobiles/Races/BaseRace.cs=Keep
- Data/Scripts/Mobiles/Races/RacePotions.cs=Keep
- Data/Scripts/Mobiles/Races/RacePotions.cs=Keep
- Data/Scripts/System/Commands/Player/CreatureHelp.cs=Keep
- Data/Scripts/System/Commands/Player/CreatureHelp.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
