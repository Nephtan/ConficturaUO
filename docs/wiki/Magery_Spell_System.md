# Magery Spell System

## Scope

This page documents the standard Magery spell line implemented under `Data/Scripts/Magic/Magery/`.
It covers the regular spellbook, scroll learning and casting, the `MagerySpell` shared rules, the 64 registered spell IDs, and the notable Confictura customizations inside the spell scripts.

This is not the player toolbar guide. For configurable Magery toolbar commands, see [Magic Toolbars Guide](Magic_Toolbars_Guide.md). For spell visual hue settings, see [Magery Spell Color Setting](Magery_Spell_Color_Setting.md). For mage NPC advice and wand charging, see [NPC Mage Advice](NPC_Mage_Advice.md).

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Magic/Magery/MagerySpell.cs` | Shared `Spell` subclass for standard Magery circle rules, reagent fallback, skill windows, mana costs, resist checks, and cast delay. |
| `Data/Scripts/Magic/Magery/Spellbook.cs` | Regular `Spellbook` item, spellbook packet handlers, scroll learning, spellbook lookup cache, `[AllSpells]` staff command, and spellbook serialization. |
| `Data/Scripts/Magic/Magery/Scrolls/SpellScroll.cs` | Base `SpellScroll` item used by Magery scrolls and other spell families. |
| `Data/Scripts/Magic/Magery/Magery 1st/` through `Magery 8th/` | Concrete spell classes for the 64 standard Magery spells. |
| `Data/Scripts/Magic/Magery/Scrolls/First Circle/` through `Eighth Circle/` | Concrete scroll item classes for each Magery spell. |
| `Data/Scripts/Magic/Base/Initializer.cs` | Registers Magery spell IDs `0` through `63` with `SpellRegistry`. |
| `Data/Scripts/Magic/Base/Spell.cs` | Shared casting state machine, sequence checks, resource spend, fizzle handling, and damage scaling used by Magery. |
| `Data/Scripts/Magic/Base/SpellHelper.cs` | Shared helpers for town checks, travel checks, fields, damage, stat mods, healing, and summons. |

## Shared Casting Rules

`MagerySpell` uses `SkillName.Magery` as both the cast skill and damage skill through the base `Spell` defaults.
Every concrete Magery spell exposes a `SpellCircle` value from `First` through `Eighth`.

| Rule | Compiled behavior |
| --- | --- |
| Spell IDs | Regular Magery occupies IDs `0` through `63`; `Spellbook.GetTypeForSpell()` maps that range to `SpellbookType.Regular`. |
| Skill window | `circle = (int)Circle`; scroll casts reduce that value by `2`; `avg = (100 / 7) * circle`; `min = avg - 20`; `max = avg + 20`. |
| Mana | Circle mana table is `4`, `6`, `9`, `11`, `14`, `20`, `40`, and `50`. |
| Cast delay | Base delay is `(3 + circle) * CastDelaySecondsPerTick`, then the shared `Spell` faster-casting logic applies. |
| Reagents | The base reagent check runs first. If it fails, `ArcaneGem.ConsumeCharges()` can pay the spell cost: `1` charge on `Core.SE`, otherwise `1 + circle`. |
| Resist training | `GetResistSkill()` and `CheckResisted()` can call `CheckSkill(MagicResist, 0.0, cap)` when the target is below the circle-derived maximum. |
| Resist chance | Resist percent compares target Magic Resist against caster Magery and circle, then halves the larger of the two calculated percentages. |
| Final effects | Most spells call `CheckSequence()`, `CheckBSequence(target)`, or `CheckHSequence(target)` before applying effects, then call `FinishSequence()`. |

## Acquisition And Learning

The regular `Spellbook` is constructable, has item ID `0xEFA`, weighs `3.0`, equips on `Layer.Talisman`, and defaults to an empty `Content` bitmask.
It has `BookOffset = 0`, `BookCount = 64`, and `SpellbookType.Regular`.

Equipping a regular spellbook requires at least `30.0` base Magery.
The book can also carry AOS attributes, AOS skill bonuses, a crafter reference, and up to two slayer names through inherited spellbook support.

Scroll learning uses `Spellbook.OnDragDrop()`:

| Gate | Behavior |
| --- | --- |
| Dropped item | Must be exactly one `SpellScroll`. |
| Spell family | `Spellbook.GetTypeForSpell(scroll.SpellID)` must match the book's `SpellbookType`. |
| Duplicate spell | Existing spells are rejected with localized message `500179`. |
| Content bit | `scroll.SpellID - BookOffset` selects the bit to set in `Content`; the scroll is deleted after success. |

Scroll casting uses `SpellScroll.OnDoubleClick()`.
The scroll must be in the caster's backpack, then `SpellRegistry.NewSpell(m_SpellID, from, this)` creates the cast object.
If registry construction fails, the caster receives localized message `502345`.

## Spellbook Runtime

`Spellbook.Initialize()` subscribes to `EventSink.OpenSpellbookRequest` and `EventSink.CastSpellRequest`.
Open requests map the client spellbook type to a `SpellbookType`, find an equipped or backpack book, and display it.
Cast requests verify that the requested spell exists in the supplied or discovered book, then call `SpellRegistry.NewSpell(spellID, from, null)` and `Cast()`.

`Spellbook.Find()` keeps a static cache from `Mobile` to a list of nearby spellbooks.
The lookup searches the talisman layer first and then direct backpack contents.
The regular book path accepts a book only when it is equipped or directly in the backpack, has `SpellbookType.Regular`, and contains the requested spell bit.

## Spell Catalog

| Circle | IDs | Spells | Main behavior group |
| --- | --- | --- | --- |
| First | `0`-`7` | Clumsy, Create Food, Feeblemind, Heal, Magic Arrow, Night Sight, Reactive Armor, Weaken | Low-cost stat debuffs, minor healing/damage, night vision, defensive armor, and Confictura race-aware food creation. |
| Second | `8`-`15` | Agility, Cunning, Cure, Harm, Magic Trap, Remove Trap, Protection, Strength | Stat buffs, poison cure, close-range damage, trap tools, protection toggles, and Confictura `TrapWand` support. |
| Third | `16`-`23` | Bless, Fireball, Magic Lock, Poison, Telekinesis, Teleport, Unlock, Wall of Stone | Combined stat buff, direct damage, locks, poison, remote item use, short travel, unlocking, and temporary walls. |
| Fourth | `24`-`31` | Arch Cure, Arch Protection, Curse, Fire Field, Greater Heal, Lightning, Mana Drain, Recall | Area cure/protection, target curse, field creation, stronger healing/damage, mana drain, and rune/runebook recall travel. |
| Fifth | `32`-`39` | Blade Spirits, Dispel Field, Incognito, Magic Reflection, Mind Blast, Paralyze, Poison Field, Summon Creature | Summons, field cleanup, disguise, reflect shield, attribute-based damage, paralysis, poison fields, and random creature summons. |
| Sixth | `40`-`47` | Dispel, Energy Bolt, Explosion, Invisibility, Mark, Mass Curse, Paralyze Field, Reveal | Dispel and damage spells, delayed explosion, hiding/invisibility tools, rune marking, area curse, paralyze fields, and Confictura trap/hidden-door/chest reveal behavior. |
| Seventh | `48`-`55` | Chain Lightning, Energy Field, Flame Strike, Gate Travel, Mana Vampire, Mass Dispel, Meteor Swarm, Polymorph | Area damage, fields, high damage, gate travel, mana steal, area dispel, meteor damage, and temporary body transformation. |
| Eighth | `56`-`63` | Earthquake, Energy Vortex, Resurrection, Air Elemental, Summon Daemon, Earth Elemental, Fire Elemental, Water Elemental | Area damage, vortex/daemon/elemental summons, resurrection, pet resurrection gumps, and Confictura henchman item revival. |

## Confictura Behavior Notes

Several standard spells have shard-specific behavior beyond stock Magery:

| Spell | Confictura behavior |
| --- | --- |
| `CreateFoodSpell` | Blood-drinker races receive `BloodyDrink`; brain-eater races receive `FreshBrain`; other casters receive random food plus `WaterFlask`. |
| `RemoveTrapSpell` | Targeting the caster deletes that caster's existing `TrapWand` items, creates a new `TrapWand`, sets `WandPower` from Magery, and adds a 30-minute buff. |
| `MagicLockSpell` | Can lock normal containers, dungeon doors, and can trap eligible uncontrolled `BaseCreature` targets into `IronFlaskFilled` items. |
| `TelekinesisSpell` | Can activate `ITelekinesisable` objects, use accessible containers, snoop carried containers, or pull loose lightweight items into the caster's backpack. |
| `RevealSpell` | Scans for traps, hardcoded hidden-door graphics, `HiddenTrap`, `HiddenChest`, and hidden mobiles; chest reveals may spawn gold or `HiddenBox`. |
| `RecallSpell` and `GateTravelSpell` | Use Confictura world-region gates through `Worlds.AllowEscape()`, `Worlds.RegionAllowedRecall()`, and `Worlds.RegionAllowedTeleport()`. |
| `ResurrectionSpell` | Self-targeting creates a `SoulOrb`; dead player targets receive `ResurrectGump`; dead controlled creatures use `PetResurrectGump`; henchman items can be revived through item targeting. |

## Commands

Magery itself has no dedicated player command registrar in `Data/Scripts/Magic/Magery/`.
Player casting comes from client spellbook packets, scroll double-clicks, magic staff charges, and the separate magic toolbar commands.

| Command | Access | Description |
| --- | --- | --- |
| `[AllSpells]` | `GameMaster` | Begins a target cursor. Targeting a `Spellbook` fills all supported content bits; regular Magery books receive all 64 spell bits. |

## Serialization Notes

| Class | Serialized data |
| --- | --- |
| `Spellbook` | Version `3`; crafter, slayer names, AOS attributes, AOS skill bonuses, content bitmask, and spell count. |
| `SpellScroll` | Version `0`; integer `m_SpellID`. |
| Magery scroll classes | Each calls `SpellScroll` serialization, then writes version `0`. |
| Temporary field, gate, and summon helper items | Most write no subclass payload beyond base item/mobile state and delete or expire through timers when appropriate. |
| `IronFlask` | Version `0`; no subclass fields. |
| `IronFlaskFilled` | Version `0`; trapped creature identity, body, hue, AI, stats, damage, resistances, movement flags, poison data, and sound IDs. |
| `LockedCreature` | Version `0`; on deserialize it schedules deletion after 10 seconds and does not restore the captured poison/sound fields. |

`MagerySpell` cast objects are transient and do not serialize world state.
Long-lived state is stored on spellbooks, scrolls, generated items, mobiles, fields, gates, buffs, or timers.

## Known Issues

| Issue | Impact |
| --- | --- |
| `MagicReflectSpell.CheckCast()` and `MagicReflectSpell.OnCast()` call `Caster.Backpack.FindItemByType(typeof(Diamond))` without guarding a missing backpack. | Nonstandard casters without backpacks can null-reference before the spell can report the missing diamond requirement. |
| `MagicLockSpell` uses `Caster.Backpack.FindItemByType(typeof(IronFlask))` in the creature-trapping path without a backpack guard. | Creature trapping can null-reference for a caster with no backpack instead of sending the empty-flask requirement. |
| `MagicLockSpell` only calls `CheckSequence()` for lockable containers; the dungeon-door and creature-trapping paths perform effects or custom success rolls without the shared final sequence check. | Those paths can bypass the normal Magery reagent, mana, scroll, fizzle, and sequence validation. |
| `EarthquakeSpell` iterates `Caster.GetMobilesInRange(...)` directly and never frees the pooled enumerable. | Repeated Earthquake casts can leak pooled range enumerables. |
| `ResurrectionSpell` assumes `BaseCreature.GetMaster()` returns a valid `Mobile` before opening `PetResurrectGump`. | Dead creatures without a master can null-reference in the pet resurrection branch. |
