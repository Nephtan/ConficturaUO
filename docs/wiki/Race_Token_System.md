# System Name: Race Token System

## Overview

The Race Token System lets a player become a creature-model character by equipping a hidden `BaseRace` `Item` on `Layer.Special`. The placed `RacePotions` shelf opens `RacePotionsGump`, previews hard-coded creature records, and calls `BaseRace.CreateRace()` when the player confirms a creature. The selected race item stores the creature body, sounds, alignment metadata, food rules, AOS attributes, AOS resistances, and AOS skill bonuses.

The same folder also contains `NPCRace`, a smaller `Layer.Special` token used by `MorphingTime` to assign creature bodies and sounds to `BaseCreature` mobiles. `NPCRace` is not the player-facing shelf flow, but it shares the same token model and should be treated as part of the race-token implementation.

There are no custom `CommandSystem.Register` commands, packet handlers, EventSink hooks, or XMLSpawner attachment classes in the race folder. The system is driven by constructable items, gumps, help-menu settings, and `Mobile`/`PlayerMobile` lifecycle hooks.

## Script Inventory

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Races/BaseRace.cs` | Player race token item, hard-coded race catalog, player transformation, race sync, level scaling, food/alignment helpers, serialization. |
| `Data/Scripts/Mobiles/Races/RacePotions.cs` | `RacePotions` shelf item and nested `RacePotionsGump` selector/help UI. |
| `Data/Scripts/Mobiles/Races/NPCRace.cs` | NPC-only special-layer race token used by scripted morphing paths. |
| `Data/System/Source/Mobile.cs` | Persists race fields, reapplies `BodyMod` through `SetRace()`/`RaceBody()`, and remaps human sounds through `RaceSound()`. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Calls `BaseRace.SyncRace()` on movement, resurrection, and death; clears race state during deserialize when creature races are disabled. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Exposes creature magic focus, creature type, and creature sound settings. |
| `Data/Scripts/System/Misc/Settings.cs` | Supplies `MonsterCharacters()` and `MonstersAllowed()` server setting gates. |
| `Data/Scripts/System/Commands/Player/CreatureHelp.cs` | Defines `CreatureHelpGump`, which renders the race help text from `RacePotions`. |

`Data/Scripts/Scripts.csproj` explicitly compiles `BaseRace.cs`, `NPCRace.cs`, and `RacePotions.cs`.

## Administrator Surface

The system does not register a named admin command. Use standard RunUO constructable placement for the shelf:

| Command surface | Result |
| --- | --- |
| `[add RacePotions` | Places the player-facing `gypsy potion shelf`, item ID `0x506C`, hue `0xABE`, weight `1.0`. |
| `[add BaseRace` | Places an unconfigured player race token. This is constructable but not a complete race until configured through code. |
| `[add NPCRace` | Places an unconfigured NPC race token. This is constructable but normally created by `NPCRace.CreateRace()`. |

The useful runtime properties are exposed through `[CommandProperty]` on `BaseRace` and `NPCRace`. `BaseRace.Attributes` is player-visible; `Resistances`, `SkillBonuses`, `Owner`, and all `Species*` fields are GameMaster properties.

## Server Setting Gates

| Setting method | Behavior |
| --- | --- |
| `MyServerSettings.MonsterCharacters()` | Returns the configured creature-character size cap. The help text documents `0`, `1`, `2`, and `3`; the code treats any positive value as enabled. |
| `MyServerSettings.MonstersAllowed()` | Returns `true` only when `MonsterCharacters() > 0`. |
| `MyServerSettings.AllowAlienChoice()` | Separate alien-origin setting. It is not the same as creature race tokens. |

The shelf only opens on double-click when the caller is within 4 tiles and `MonstersAllowed()` is true. `RacePotionsGump` then filters creature records through `BaseRace.GetMonsterSize()`: a race is selectable when its configured `Size` is less than or equal to the `MonsterCharacters()` cap. In tavern mode, the selector is also limited to the player's current `SpeciesFamily`, unless the row is the player's current body.

## Player Flow

1. A player double-clicks `RacePotions` within 4 tiles while creature characters are enabled.
2. The shelf ensures `Mobile.RaceSection >= 1`, closes `GypsyTarotGump`, `WelcomeGump`, and any old `RacePotionsGump`, then opens `RacePotionsGump(from, 0)`.
3. The gump previews the current race page, alignment, species family, creature art, paperdoll art, and the text returned by `BaseRace.GetAbilities()`.
4. Clicking a category button jumps `RaceSection` near the first record in that creature family.
5. Clicking the next/previous arrows changes `RaceSection` to another allowed record.
6. Clicking the race OK button sends a button ID of `80000 + SpeciesID`. `OnResponse()` subtracts `80000` and calls `BaseRace.CreateRace(from, race, true)`.
7. Clicking the human OK button sends button ID `1000` and calls `BaseRace.BackToHuman(from)`.

The help-menu creature-type setting opens `RacePotionsGump(from, 1)`. That tavern mode is shown only when `RaceID > 0` and the caller is in region name `the Tavern` or inside the hard-coded Sosaria rectangle `X 6982..6999`, `Y 694..713`.

## Category Buttons

The shelf renders the Human button unconditionally, then renders creature-family quick buttons based on `MonsterCharacters()`.

| Quick ID | Category | Minimum `MonsterCharacters()` |
| --- | --- | --- |
| Human | Human | Always shown |
| 1 | Aquatic | 1 |
| 2 | Bugbear | 1 |
| 3 | Centaur | 1 |
| 4 | Cyclops | 2 |
| 5 | Daemon | 2 |
| 6 | Dagon | 2 |
| 7 | Demon | 1 |
| 8 | Devil | 3 |
| 9 | Dragon | 2 |
| 10 | Drakkul | 1 |
| 11 | Ettin | 2 |
| 12 | Fey | 1 |
| 13 | Gargoyle | 1 |
| 14 | Giant | 3 |
| 15 | Gnoll | 1 |
| 16 | Goblin | 1 |
| 17 | Golem | 1 |
| 18 | Hobgoblin | 1 |
| 19 | Illithid | 1 |
| 20 | Imp | 1 |
| 21 | Kilrathi | 1 |
| 22 | Kobold | 1 |
| 23 | Minotaur | 1 |
| 24 | Mummy | 1 |
| 25 | Mushroom | 1 |
| 26 | Naga | 1 |
| 27 | Ogre | 1 |
| 28 | Orc | 1 |
| 29 | Owlbear | 2 |
| 30 | Plant | 1 |
| 31 | Reptilian | 1 |
| 32 | Revenant | 2 |
| 33 | Rodent | 1 |
| 34 | Salamander | 1 |
| 35 | Satyr | 1 |
| 36 | Serpent | 1 |
| 37 | Skeleton | 1 |
| 38 | Sphinx | 1 |
| 39 | Succubus | 1 |
| 40 | Titan | 2 |
| 41 | Tree | 2 |
| 42 | Troll | 1 |
| 43 | Vampyre | 1 |
| 44 | Zombi | 1 |

## Race Definition Schema

`BaseRace.RaceDefined(int val)` hard-codes 172 comma-delimited rows. `ConfigureCostume()` maps those fields into the token item.

| Entry | Field | Runtime target or formula |
| --- | --- | --- |
| 1 | Name | `BaseRace.Name` |
| 2 | Index | `SpeciesIndex` |
| 3 | ItemID | `ItemID` |
| 4 | Gump | `SpeciesGump`; paperdoll art uses `50000 + SpeciesGump` |
| 5 | Body | `SpeciesID`; applied to `Mobile.BodyMod` and `Mobile.RaceID` |
| 6 | Icon | `SpeciesIcon` |
| 7 | x | `SpeciesWide`; used to center the gump preview image |
| 8 | y | `SpeciesHigh`; used to center the gump preview image |
| 9 | Sound | Base sound ID; anger is `sound`, idle `sound + 1`, attack `sound + 2`, hurt `sound + 3`, death `sound + 4`, except raw `9999` uses mushroom sounds |
| 10 | Species | `SpeciesFamily` |
| 11 | Alignment | `SpeciesAlignment` (`good`, `neutral`, or `evil`) |
| 12 | Start | `SpeciesStart` |
| 13 | Size | `SpeciesSize`; compared to `MonsterCharacters()` |
| 14-18 | Phy/Fir/Cld/Poi/Eny | Each value multiplied by `5` into AOS resistances |
| 19-24 | Str/Dex/Int/Hits/Stam/Mana | Each value multiplied by `5` into matching AOS attributes |
| 25-27 | RegHits/RegStam/RegMana | Direct AOS regeneration attributes |
| 28 | Night | Direct `NightSight` attribute |
| 29-30 | Attack%/Defend% | Each value multiplied by `5` |
| 31-32 | CastRecover/CastSpd | Direct casting attributes |
| 33-35 | Potions/LowMana/LowReg | Each value multiplied by `5` |
| 36 | Luck | Value multiplied by `300` |
| 37-40 | Reflect/SpellDmg/WepDmg/WepSpeed | Each value multiplied by `5` |
| 41-42 | Skill1/Skill2 | If not `100`, converted through `RaceSkill()` and assigned `+10` |
| 43 | Food | Food-rule code |
| 44 | Gender | `SpeciesFemale`; `1` forces female on equip, all other values force male |

Food codes:

| Food code | Helper behavior |
| --- | --- |
| `0` | Normal food and drink rules. |
| `1` | `NoFood()` true: does not need food, still needs drink. |
| `2` | `NoFoodOrDrink()` true. |
| `3` | `BloodDrinker()` true: must consume fresh blood. |
| `4` | `BrainEater()` true: must consume fresh brains. |

## Applying A Race

`BaseRace.CreateRace(Mobile m, int id, bool makeOne)` only runs for living `PlayerMobile` instances.

If the player already has a `BaseRace` on `Layer.Special`, the method refreshes ownership, `BodyMod`, `HueMod`, `RaceID`, and race sound fields from that token. If `makeOne` is true, it creates a new configured token from `GetCostume(id)`, deletes the current `Layer.Special` item, deletes any other `BaseRace` world items whose `Owner` is the same player, adds the new token to the backpack, and equips it.

`BaseRace.OnEquip()` stores the caller's original gender in `RaceWasFemale` when `RaceID == 0`, then applies:

| Mobile field | Value |
| --- | --- |
| `BodyMod` | `SpeciesID` |
| `HueMod` | `0` |
| `RaceID` | `SpeciesID` |
| `RaceAngerSound` / `RaceIdleSound` / `RaceDeathSound` / `RaceAttackSound` / `RaceHurtSound` | Token sound fields |
| `Female` | `true` when `SpeciesFemale == 1`, otherwise `false` |

The equip path dismounts the player by calling `EtherealMount.EthyDismount(m)` and clearing `mt.Rider` when a mount is present.

`BaseRace.BackToHuman()` deletes the current `Layer.Special` item, clears `BodyMod`, resets `HueMod` to `-1`, clears `RaceID`, race sounds, and `RaceHomeLand`, then restores `Female` from `RaceWasFemale`.

## Lifecycle Sync

Race state is not only stored on the item. Core `Mobile` serialization also stores `RaceID`, five race sound fields, `RaceMakeSounds`, `RaceMagicSchool`, `RaceWasFemale`, `RaceSection`, and `RaceHomeLand`.

| Hook | Race behavior |
| --- | --- |
| Login (`Announce.World_Login`) | Calls `Mobile.SetRace()`, which reapplies `BodyMod = RaceID` and `HueMod = 0` when `RaceID > 0`. |
| `PlayerMobile.OnLocationChange()` | Calls `BaseRace.SyncRace(this, false)`. |
| `PlayerMobile.Resurrect()` | Calls `BaseRace.SyncRace(this, true)` after restoring vitals. |
| `PlayerMobile.OnDeath()` | Calls `BaseRace.SyncRace(this, false)` after death handling. |
| `BaseRace.SyncRace()` while alive | Recreates the token if `BodyMod == 0` or no `Layer.Special` item exists; optionally calls level scaling. |
| `BaseRace.SyncRace()` while dead | Deletes the equipped `BaseRace` item. `RaceID` remains, so resurrection can recreate it. |
| `PlayerMobile.Deserialize()` with creature races disabled | Clears body/hue and race ID/sounds if `MonstersAllowed()` is false. |

`BaseRace.Deserialize()` also schedules the token for deletion after 10 seconds when `MonstersAllowed()` is false.

## Level Scaling

`BaseRace.SetProperties(Mobile m)` recalculates a token only when `current.SpeciesLevel` differs from `GetPlayerInfo.GetPlayerLevel(m)`. Levels below `2` are treated as `0`. All casts to `int` truncate fractional results.

| Current base property | Scaling when base property is positive |
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

After recalculating, the method deletes the temporary comparison token, reapplies AOS skill bonuses, reapplies stat bonuses, and calls `CheckStatTimers()`.

## Creature Magic Focus

Some race rows use skill code `25` (`Magery`) or `49` (`Necromancy`). `BaseRace.GetMonsterMage()` returns true when either slot contains those raw codes, and the help settings can cycle `RaceMagicSchool` through:

| `RaceMagicSchool` | Label | Slot rewrite behavior |
| --- | --- | --- |
| `0` | Default | Code `25` stays Magery; code `49` stays Necromancy. |
| `1` | Magery | A single code `25` or single code `49` slot becomes Magery. |
| `2` | Necromancy | A single code `25` or single code `49` slot becomes Necromancy. |
| `3` | Elementalism | A single magic slot becomes Elementalism; if both Magery and Necromancy slots are present, slot 1 becomes Elementalism and slot 2 becomes Meditation. |

The creature magic button is only rendered in help settings when the player is a creature, the region name is `the Tavern`, and `GetMonsterMage(RaceID)` is true.

## Race Sounds

New characters default `RaceMakeSounds = true`. Combat sound hooks in `BaseCreature` and `BaseWeapon` only play player race attack/hurt sounds when the player is a creature, `RaceMakeSounds` is true, the relevant sound field is positive, and `Utility.RandomBool()` succeeds.

Core `Mobile.PlaySound()` always passes through `RaceSound()`, which remaps many human vocal sound IDs to the race idle, attack, anger, hurt, or death sound whenever `RaceID > 0` or `RaceID == -700`.

## NPC Race Token

`NPCRace.CreateRace(Mobile m, int id, int hue)` is restricted to living `BaseCreature` instances. It creates an `NPCRace`, configures it from `NPCRace.RaceDefined(id)`, deletes the current `Layer.Special` item if present, assigns the requested hue, adds the item to the creature backpack, and equips it.

`NPCRace.OnEquip()` sets the creature's gender and base human body (`400` male, `401` female), then applies `BodyMod`, `RaceID`, `HueMod`, `RaceSection`, token name, race sounds, and dismount logic. `NPCRace` serializes version `0`, species body, item ID, five sound fields, and gender.

## Serialization

| Class | Version | Serialized fields |
| --- | --- | --- |
| `RacePotions` | `1` | Version only. |
| `BaseRace` | `0` | AOS attributes, AOS resistances, AOS skill bonuses, `Owner`, `SpeciesIndex`, `SpeciesID`, `SpeciesGump`, `SpeciesIcon`, `SpeciesWide`, `SpeciesHigh`, `SpeciesFamily`, `SpeciesAlignment`, `SpeciesStart`, `SpeciesSize`, five race sounds, `SpeciesLevel`, `SpeciesFood`, `SpeciesFemale`. |
| `NPCRace` | `0` | `SpeciesBody`, `SpeciesItemID`, five race sounds, `SpeciesGender`. |
| `Mobile` core | Versioned in core `Mobile` | `RaceID`, five race sounds, `RaceMakeSounds`, `RaceMagicSchool`, `RaceWasFemale`, `RaceSection`, `RaceHomeLand`. |

`BaseRace` and `NPCRace` read their version integers but do not branch on older versions. Current field order is positional and must remain aligned with the write order.

## Known Issues

* `RacePotionsGump.OnResponse()` trusts any button ID above `80000` and calls `BaseRace.CreateRace()` without rechecking shelf range, `MonstersAllowed()`, `MonsterCharacters()` size limits, tavern species limits, or whether the requested body came from the rendered gump. A stale or tampered response can bypass the selector filters or create an unconfigured `BaseRace` token.
* `BaseRace.CreateRace()`, `BaseRace.BackToHuman()`, and `NPCRace.CreateRace()` delete whatever item is currently on `Layer.Special`; only some paths first verify that the item is a race token. Any unrelated special-layer item on the mobile can be destroyed by race changes or human reset.
* `BaseRace.OnEquip()` and `NPCRace.OnEquip()` call `base.OnEquip(m)` once for the condition and again for the return value. The current base implementation only calls `ProcessClothing()`, but this double invocation is brittle and can repeat future base side effects.
* `RacePotionsGump.OnResponse()` immediately dereferences `state.Mobile` and closes/sends gumps without guarding null, deleted, or non-player state.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0060.
- Backlog rows: RB-06753.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Mobiles/Races/BaseRace.cs (CurrentFile)
- Data/Scripts/Mobiles/Races/RacePotions.cs (CurrentFile)
- Data/Scripts/Mobiles/Races/NPCRace.cs (CurrentFile)
- Data/System/Source/Mobile.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/PlayerMobile.cs (CurrentFile)
- Data/Scripts/System/Help/Gumps/HelpGump.cs (CurrentFile)
- Data/Scripts/System/Misc/Settings.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/CreatureHelp.cs (CurrentFile)
- Data/Scripts/Scripts.csproj (CurrentFile)

### Runtime Evidence

- Hook summary: Event=15; Gump=90; Initialize=2; Login=1; Logout=1; Movement=3; Speech=8; Timer=22; WorldLoad=1.
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L798 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L806 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L807 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L808 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L809 Event EventSink access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L813 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1003 Login OnLogin access=Internal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1030 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1047 Gump SendGump access=Internal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1068 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1328 Logout OnLogout access=Internal
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:L1367 Timer Timer.DelayCall access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 5.
- Data/Scripts/Mobiles/Base/PlayerMobile.cs:Server.Mobiles.PlayerMobile version=37 serialize=L5042 deserialize=L4577 alignment=CountMismatch:Writes=120;Reads=119
- Data/Scripts/Mobiles/Races/BaseRace.cs:Server.Items.BaseRace version=0 serialize=L3975 deserialize=L4003 alignment=CountMatchNeedsTypeReview:UnknownWrites=18
- Data/Scripts/Mobiles/Races/NPCRace.cs:Server.Items.NPCRace version=0 serialize=L669 deserialize=L683 alignment=CountMatchNeedsTypeReview:UnknownWrites=8
- Data/Scripts/Mobiles/Races/RacePotions.cs:Server.Items.RacePotions version=1 serialize=L47 deserialize=L53 alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Mobile.cs:Server.Mobile version=35 serialize=L6183 deserialize=L5700 alignment=CountMismatch:Writes=98;Reads=104

### Project And Runtime Coverage

- Data/Scripts/Mobiles/Base/PlayerMobile.cs=Keep
- Data/Scripts/Mobiles/Base/PlayerMobile.cs=Keep
- Data/Scripts/Mobiles/Races/BaseRace.cs=Keep
- Data/Scripts/Mobiles/Races/BaseRace.cs=Keep
- Data/Scripts/Mobiles/Races/NPCRace.cs=Keep
- Data/Scripts/Mobiles/Races/NPCRace.cs=Keep
- Data/Scripts/Mobiles/Races/RacePotions.cs=Keep
- Data/Scripts/Mobiles/Races/RacePotions.cs=Keep
- Data/Scripts/System/Commands/Player/CreatureHelp.cs=Keep
- Data/Scripts/System/Commands/Player/CreatureHelp.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep
- Data/Scripts/System/Misc/Settings.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
