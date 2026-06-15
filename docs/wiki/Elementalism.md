# Elementalism

## Overview
Elementalism is a custom `SpellbookType.Elementalism` spell line using spell IDs `300` through `331`. It is built around `ElementalSpell`, `ElementalSpellbook`, `ElementalSpellbookGump`, `ElementalShrine`, thirty-two `SpellScroll` subclasses, player cast commands, and several temporary field, portal, and summoned `Mobile` classes.

The spell line uses `SkillName.Elementalism` as both `CastSkill` and `DamageSkill`. A player focus value on `PlayerMobile.CharacterElement` selects the active element:

| CharacterElement | Element | Book ItemID | Primary damage theme |
| ---: | --- | ---: | --- |
| `0` | Air | `0x6421` | Energy and physical |
| `1` | Earth | `0x641F` | Physical and poison |
| `2` | Fire | `0x6422` | Fire and physical |
| `3` | Water | `0x6420` | Cold and physical |

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `ElementalSpell` | `Spell` subclass | Shared Elementalism cast skill, mana, stamina, resist, armor fizzle, spell metadata, book cover, and element helper logic. |
| `ElementalSpellbook` | `Spellbook` item | Talisman-layer spellbook with `BookOffset = 300`, `BookCount = 32`, and element-specific cover art. |
| `ElementalSpellbookGump` / `ElementalSpellHelp` | `Gump` classes | Spellbook UI, spell-detail pages, help pages, and spell button dispatch for IDs `300` through `331`. |
| `ElementalCommands` | command registrar | Registers one player command per Elementalism spell. |
| `ElementalScrolls.cs` | `SpellScroll` subclasses | One scroll item for each Elementalism spell ID. |
| `ElementalShrine` | speech and movement `Item` | Changes a player's focused element on walkover and recolors equipment on `culara` speech. |
| `ElementalEffect` | temporary `Item` | Generic timed visual item used by water splashes, portals, holds, and summon effects. |
| `ElementalExit` | movement `Item` | Returns a player from the Lyceum to the location stored by `Elemental Sanctuary`. |
| `Mobiles/*` | `BaseCreature` / `BaseMount` classes | Summoned steeds, called elementals, fiends, greater summons, lords, and spirits. |

`Initializer` registers spell IDs `300` through `331`, and `Spellbook.GetTypeForSpell` maps the ID range to `SpellbookType.Elementalism`. The `Scripts.csproj` project includes the Elementalism command, base, shrine, spellbook, scroll, gump, mobile, and sphere files.

## Acquisition And Learning
| Access path | Compiled behavior |
| --- | --- |
| Spellbook | `ElementalSpellbook` is constructable. `MyElementalSpellbook` is a loot-style subclass with randomized name, randomized starting content tiers, and randomized book properties. |
| Scroll crafting | `DefInscription` adds all thirty-two Elementalism scrolls under `Elemental Spells`. Each recipe consumes `1` `BlankScroll` and `2 + sphereIndex` `DaemonBlood`; the sphere mana values are `5, 7, 10, 14, 19, 24, 40, 50`. |
| Vendor stock | Store sales lists use `ElementalSpell.ScrollPrice(id, false)` for buy prices and `ScrollPrice(id, true)` for sell prices. Base buy prices by sphere are `10, 16, 22, 28, 34, 40, 46, 52`; sell prices are `(buy / 2) - 1`. |
| Loot | `Loot.RandomScroll(..., SpellbookType.Elementalism)` is used by several loot paths, including creature loot and special containers. |
| Item identification | Identifying certain paper types can create Elementalism scrolls `300` through `331`. |
| Learning | Scrolls are normal `SpellScroll` items. Dropping one onto a compatible `ElementalSpellbook` uses the shared `Spellbook.OnDragDrop` learning path. |

## Elemental Focus And Shrines
`ElementalShrine` is an invisible, immovable item. Shard placement determines its behavior by setting `Name` to `air`, `earth`, `fire`, or `water`.

| Trigger | Requirement | Effect |
| --- | --- | --- |
| Walk over shrine | `Mobile` is a `PlayerMobile` and has `Elementalism.Base >= 5.0`. | If the shrine name differs from the player's current `CharacterElement`, the player is refocused to that element, all non-artifact owned Elementalism books are changed to the matching cover, incompatible equipped books are moved to backpack, and open spell bars/spellbook gumps are refreshed or closed. |
| Say `culara` near shrine | Player is within range `2`, shrine name matches the player's current focus, and speech contains `culara`. | Applies element visual effects and recolors the player's clothing through `MorphingTime.ColorOnlyClothes`. |

## Shared Casting Rules
| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Elementalism`. |
| Damage skill | `SkillName.Elementalism`. |
| Mana by sphere | Sphere I-VIII cost `5, 7, 10, 14, 19, 24, 40, 50`. |
| Stamina by sphere | Defaults to the same value as mana, then `GetStam()` attempts to reduce it with `LowerRegCost`. |
| Skill range | `avg = (100.0 / 7.0) * (int)Circle`; `min = avg - 20.0`; `max = avg + 20.0`. Scroll casts subtract `2` from the circle before this calculation. |
| Displayed skill in book | Sphere I-VIII pages display `0, 0, 9, 23, 38, 52, 66, 80` Elementalism. |
| Base cast delay | `(3 + (int)Circle) * CastDelaySecondsPerTick`, except spells that override delay. |
| Resist check | Uses the standard Elementalism resist formula: `max(target.MagicResist / 5.0, target.MagicResist - (((caster.Elementalism - 20.0) / 5.0) + (1 + circle) * 5.0)) / 2.0`. |
| Armor fizzle | If `ArmorFizzle(Caster) >= Utility.RandomMinMax(1, 100)`, the cast fizzles and sends the armor percentage message. |
| Book focus gate | `ElementalSpell.CanUseBook` rejects a `PlayerMobile` whose `CharacterElement` does not match the book cover ItemID. |

## Armor Fizzle Formula
Only equipped `BaseArmor` with no `MageArmor` and no `SpellChanneling` contributes. Leather adds `2 * slotValue`, wood adds `4 * slotValue`, and metal adds `6 * slotValue`.

| Layer | Slot value |
| --- | ---: |
| `OuterTorso` | 5 |
| `TwoHanded`, `Arms`, `Cloak` | 3 |
| `Helm` | 2 |
| `OuterLegs`, `InnerLegs`, `InnerTorso`, `Pants`, `Shirt` | 4 |
| `Neck`, `Gloves`, `Shoes`, `Waist` | 1 |

## Player Commands
Each command is registered at `AccessLevel.Player`, checks `Multis.DesignContext.Check`, then verifies the caster has the matching spell in an Elementalism spellbook with `Spellbook.Find(from, spellID)`.

| Command | Spell ID | Spell |
| --- | ---: | --- |
| `[EArmor` | 300 | Elemental Armor |
| `[EBolt` | 301 | Elemental Bolt |
| `[EMend` | 302 | Elemental Mend |
| `[ESanctuary` | 303 | Elemental Sanctuary |
| `[EPain` | 304 | Elemental Pain |
| `[EProtection` | 305 | Elemental Protection |
| `[EPurge` | 306 | Elemental Purge |
| `[ESteed` | 307 | Elemental Steed |
| `[ECall` | 308 | Elemental Call |
| `[EForce` | 309 | Elemental Force |
| `[EWall` | 310 | Elemental Wall |
| `[EWarp` | 311 | Elemental Warp |
| `[EField` | 312 | Elemental Field |
| `[ERestoration` | 313 | Elemental Restoration |
| `[EStrike` | 314 | Elemental Strike |
| `[EVoid` | 315 | Elemental Void |
| `[EBlast` | 316 | Elemental Blast |
| `[EEcho` | 317 | Elemental Echo |
| `[EFiend` | 318 | Elemental Fiend |
| `[EHold` | 319 | Elemental Hold |
| `[EBarrage` | 320 | Elemental Barrage |
| `[ERune` | 321 | Elemental Rune |
| `[EStorm` | 322 | Elemental Storm |
| `[ESummon` | 323 | Elemental Summon |
| `[EDevastation` | 324 | Elemental Devastation |
| `[EFall` | 325 | Elemental Fall |
| `[EGate` | 326 | Elemental Gate |
| `[EHavoc` | 327 | Elemental Havoc |
| `[EApocalypse` | 328 | Elemental Apocalypse |
| `[ELord` | 329 | Elemental Lord |
| `[ESoul` | 330 | Elemental Soul |
| `[ESpirit` | 331 | Elemental Spirit |

## Spell Reference
Damage splits use `SpellHelper.Damage` or `AOS.Damage` percentages in physical, fire, cold, poison, energy order.

| ID | Sphere | Spell | Core mechanics |
| ---: | --- | --- | --- |
| 300 | I | Elemental Armor | Self toggle. Adds `15 + floor(Elementalism / 20)` to the focus resistance and `-5` to the other four resistances. Earth buffs physical, air energy, fire fire, water cold. |
| 301 | I | Elemental Bolt | Harmful mobile target, delayed damage. Damage is `GetNewAosDamage(10, 1, 4, target) + floor(Elementalism / 5)`. Air is `50/0/0/0/50`, earth `50/0/0/50/0`, fire `50/50/0/0/0`, water `50/0/50/0/0`. |
| 302 | I | Elemental Mend | Beneficial mobile heal. Rejects dead bonded pets, animated dead, golems, poisoned targets, and mortal wounds. Heal is `floor(Elementalism * 0.1) + Utility.Random(1, 5)`, passed through `PlayerLevelMod`; healing another target multiplies by `1.5`. |
| 303 | I | Elemental Sanctuary | Recall-like escape to `Map.Sosaria` at `(1438, 1360, 80)`. Stores the caster's current location in `PlayerMobile.CharacterPublicDoor`, teleports pets, and blocks overload, public regions, boats, spaceships, nearby combat, and non-outdoor/non-dungeon restrictions by skill. |
| 304 | II | Elemental Pain | Harmful mobile target. Damage is `GetNewAosDamage(17, 1, 5, target) + floor(Elementalism / 5)`, then full damage at range `<= 1`, half at range `2`, and quarter beyond range `2`. Air is `50/0/0/0/50`, earth `100/0/0/0/0`, fire `0/100/0/0/0`, water `0/0/100/0/0`. |
| 305 | II | Elemental Protection | Self toggle. Adds physical resistance mod `-15 + min(floor(Elementalism / 20), 15)`, Magic Resist skill mod `-35 + min(floor(Elementalism / 20), 35)`, and `Registry[target] = 100.0` so `Spell.OnCasterHurt` prevents spell disturbance while active. |
| 306 | II | Elemental Purge | Beneficial mobile cure. Poison cure chance is `(10000 + Elementalism * 75 - ((poison.Level + 1) * poisonScalar)) / 100`, where AOS uses `3300` below level 4 and `3100` at level 4+, otherwise `1750`. Success requires `chanceToCure > Utility.Random(100)`. |
| 307 | II | Elemental Steed | Summons a non-combat mount for `clamp(Elementalism * 6, 480, 1500)` seconds and requires one follower slot. Air becomes an air dragon, earth a bear, fire a phoenix, and water a water beetle. |
| 308 | III | Elemental Call | Summons a one-slot called elemental for `(Elementalism * 27) / 60` minutes. Air uses `ElementalCalledAir`, earth `ElementalCalledEarth`, fire `ElementalCalledFire`, water `ElementalCalledWater`. |
| 309 | III | Elemental Force | Harmful mobile target, delayed damage. Damage is `GetNewAosDamage(19, 1, 5, target) + floor(Elementalism / 5)`. Air is `25/0/0/0/75`, earth `100/0/0/0/0`, fire `25/75/0/0/0`, water `25/0/75/0/0`. |
| 310 | III | Elemental Wall | Targeted three-tile blocking line. Creates visual and blocking `InternalItem`s for each tile. Non-friends cannot move over the blocking item. Timer uses `10 + floor(Elementalism / 2)` seconds. |
| 311 | III | Elemental Warp | Teleport within the current map. Rejects faction sigil carriers, overloaded casters, travel-blocked source/destination, blocked spawn points, and multis; teleports pets with the caster. |
| 312 | IV | Elemental Field | Targeted five-tile damaging field. Damage per tick/moveover is `floor(Elementalism / 10)`. Duration is `((15 + Elementalism.Fixed / 5) / 4) + floor(Elementalism / 2)` seconds. Air deals energy, earth `50/0/0/50/0`, fire fire, water cold. |
| 313 | IV | Elemental Restoration | Beneficial mobile heal. Uses the same target exclusions as Mend. Heal is `floor(Elementalism * 0.4) + Utility.Random(1, 10)`, passed through `PlayerLevelMod`. |
| 314 | IV | Elemental Strike | Harmful mobile target. Damage is `GetNewAosDamage(23, 1, 4, target) + floor(Elementalism / 5)`. Air is `50/0/0/0/50`, earth `100/0/0/0/0`, fire `50/50/0/0/0`, water `50/0/50/0/0`. |
| 315 | IV | Elemental Void | Recall-style travel from marked rune, runebook default, boat key, or valid house raffle deed. Checks recall source, escape, recall region, destination teleport permission, spawn fit, multis, and overload; moves pets and creates temporary void effects at source and destination. |
| 316 | V | Elemental Blast | Harmful mobile target with one-second delayed callback. Base damage is `floor((Elementalism + Int) / 5)`, capped at `60`, plus `floor(Elementalism / 5)` for `PlayerMobile` casters, then randomized from `damage` to `damage + 4`. Deals 100 percent focus element. |
| 317 | V | Elemental Echo | Self magic absorb. Requires one focus gem in the backpack: `Amethyst` for air, `Emerald` earth, `Ruby` fire, `Sapphire` water. Consumes the gem and sets `Caster.MagicDamageAbsorb = floor(Elementalism / 2)`. |
| 318 | V | Elemental Fiend | Targeted summon requiring two follower slots. Duration is `120 + floor(Elementalism / 2)` seconds. Summons `ElementalFiendAir`, `ElementalFiendEarth`, `ElementalFiendFire`, or `ElementalFiendWater`. |
| 319 | V | Elemental Hold | Harmful mobile target paralysis. Duration starts as `floor(DamageSkill / 10 - targetResist / 10) + floor(Elementalism / 2)`, adds `2` when `Core.SE` is false, triples against non-player targets, and floors at `0`. |
| 320 | VI | Elemental Barrage | Harmful mobile target, delayed damage. Damage is `GetNewAosDamage(40, 1, 5, target) + floor(Elementalism / 5)`. Deals 100 percent energy, poison, fire, or cold based on focus. |
| 321 | VI | Elemental Rune | Marks a `RecallRune` in the caster's backpack after Mark travel checks, PirateRegion rejection, teleport-region approval, and multi checks. |
| 322 | VI | Elemental Storm | Harmful mobile target with timer delay `3.0` seconds under AOS or `2.5` otherwise. Damage is `GetNewAosDamage(40, 1, 5, defender) + floor(Elementalism / 5)`. Air is `30/0/0/0/70`, earth `50/0/0/50/0`, fire `30/70/0/0/0`, water `30/0/70/0/0`. |
| 323 | VI | Elemental Summon | Summons a two-slot greater elemental for `(Elementalism * 18) / 60` minutes. Air uses `ElementalSummonLightning`, earth `ElementalSummonEnt`, fire `ElementalSummonMagma`, water `ElementalSummonIce`. |
| 324 | VII | Elemental Devastation | Targeted radius `5` area spell in the caster's current map. Collects mobiles in the same region except the caster and the caster's controlled pets. Damage is `GetNewAosDamage(48, 1, 5, pvp) + floor(Elementalism / 5)`, doubled and divided across targets when more than one target is hit. HouseRegion targets are skipped. |
| 325 | VII | Elemental Fall | Targeted radius `5` area spell with LOS, blessed, region, master, and harmful checks. Damage is `GetNewAosDamage(51, 1, 5, pvp) + floor(Elementalism / 5)`, doubled and divided across targets when more than one target is hit, then multiplied by `GetDamageScalar(target)`. |
| 326 | VII | Elemental Gate | Opens temporary `Moongate` items for 30 seconds from a rune, runebook default, or valid house raffle deed. Checks gate travel, escape, recall-region, destination teleport permission, destination spawn fit, multis, and existing gate stacking under `Core.SE`. |
| 327 | VII | Elemental Havoc | Harmful mobile target. Damage is `GetNewAosDamage(48, 1, 5, target) + floor(Elementalism / 5)`. Air is `30/0/0/0/70`, earth `30/0/0/70/0`, fire `30/70/0/0/0`, water `30/0/70/0/0`. |
| 328 | VIII | Elemental Apocalypse | Caster-centered area spell with radius `1 + floor(Elementalism / 15)`. Hits valid indirect hostile targets in the same region and LOS. Damage is `target.Hits / 2`, clamped to `15..100` for non-players, plus `Utility.RandomMinMax(0, 15)` and `floor(Elementalism / 5)`. Air/fire/water are `40/60` physical/element; earth is 100 percent physical. |
| 329 | VIII | Elemental Lord | Summons a three-slot lord for `(Elementalism * 9) / 60` minutes. Air uses `ElementalLordAir`, earth `ElementalLordEarth`, fire `ElementalLordFire`, water `ElementalLordWater`. |
| 330 | VIII | Elemental Soul | Beneficial target spell. Self-cast deletes existing owned `SoulOrb`s and creates a new focus-colored soul orb. Dead player targets receive a `ResurrectGump`; dead controlled creatures try to send `PetResurrectGump` to their master. Henchman item targets reset `HenchDead` and restore the item name. |
| 331 | VIII | Elemental Spirit | Targeted two-slot spirit summon for `90 + floor(Elementalism / 2)` seconds. Air uses `ElementalSpiritAir`, earth `ElementalSpiritEarth`, fire `ElementalSpiritFire`, water `ElementalSpiritWater`. |

## Summoned Mobiles
| Spell | Focus | Class | Slots | Key stats |
| --- | --- | --- | ---: | --- |
| Elemental Steed | Air | `ElementalSteed` modified to air dragon | 1 | `BaseMount`; tamable, `MinTameSkill = 29.1`; no explicit combat stats in the base class. |
| Elemental Steed | Earth | `ElementalSteed` modified to bear | 1 | Same base mount, body and sound changed by spell. |
| Elemental Steed | Fire | `ElementalSteed` modified to phoenix | 1 | Same base mount, body/item art and hue changed by spell. |
| Elemental Steed | Water | `ElementalSteed` modified to water beetle | 1 | Same base mount, body/item art and hue changed by spell. |
| Elemental Call | Air/Earth/Fire/Water | `ElementalCalled*` | 1 | Generally `Str/Dex/Int = 100/100/50`, `Damage = 6-10`, `VirtualArmor = 20`, basic magery and melee skills. |
| Elemental Fiend | Air/Earth/Fire/Water | `ElementalFiend*` | 2 | Generally `Str/Dex/Int = 150/150/100`, `Hits = 160`, `Damage = 10-14`, `VirtualArmor = 40`, zero Fame/Karma. |
| Elemental Summon | Air/Earth/Fire/Water | `ElementalSummonLightning`, `ElementalSummonEnt`, `ElementalSummonMagma`, `ElementalSummonIce` | 2 | Generally `Str/Dex/Int = 200/70/70`, `Hits = 180`, `Damage = 14-21`, `VirtualArmor = 34`, elemental resist profile. |
| Elemental Lord | Air/Earth/Fire/Water | `ElementalLord*` | 3 | Generally `Str = 300`, `Int = 200`, high resist profile, `VirtualArmor = 40`; exact Dexterity, Hits, damage, skills, and damage types vary by focus. |
| Elemental Spirit | Air/Earth/Fire/Water | `ElementalSpirit*` | 2 | Generally `Str/Dex/Int = 200/200/100`, `Hits = 140`, `Damage = 14-17`, `VirtualArmor = 40`, zero Fame/Karma. |

## Serialization Notes
| Class | Versioning behavior |
| --- | --- |
| `ElementalSpellbook` | Writes version `1`, then `EllyOwner`, then `EllyPage`. |
| `ElementalShrine` | Writes version `0` only. |
| `ElementalExit` | Writes version `0` only. |
| `ElementalEffect` | Writes version `0` only; duration and owner are not serialized. |
| `ElementalScrolls.cs` scroll classes | Each scroll writes version `0` only after base serialization. |
| `Elemental_Wall_Spell.InternalItem` | Writes version `1`, then `m_End`. |
| `Elemental_Field_Spell.ElementalFieldItem` | Writes version `2`, then `m_Damage`, `m_Caster`, and `m_End`; older versions fall through with defaults. |
| `Elemental_Gate_Spell.InternalItem` | Calls `base.Serialize(writer)` without writing a local version and deletes itself during `Deserialize`. |
| Summoned `Mobile` classes | Each traced summon class writes a version integer, usually `0`, after base serialization. |

## Known Issues
* `ElementalSpell.GetElement(Mobile m)` directly casts `m` to `PlayerMobile`. Any non-player `Mobile` caster, null caster, or future NPC spell use can throw before spell-specific logic runs.
* `ElementalSpell.GetStam()` performs integer division in `double drop = reduce / 100;`. Any positive `LowerRegCost` below 100 makes stamina cost drop to zero instead of scaling proportionally.
* `ElementalSpellbook.Deserialize` reads `int EllyPage = reader.ReadInt();` into a local variable, so the persisted `EllyPage` field is never restored.
* `ElementalEffect` stores duration in static `m_Seconds`, does not serialize `m_Owner` or per-item duration, starts a timer in both the serial constructor and `Deserialize`, and resets deserialized duration to `1.0`.
* `Elemental_Wall_Spell.InternalItem` starts a timer for `10 + floor(Elementalism / 2)` seconds but serializes `m_End = DateTime.Now + 10.0` only, so restart persistence truncates skilled wall duration. It also does not serialize `m_Caster`, which can break friend checks after reload.
* `Elemental_Field_Spell.ElementalFieldItem.InternalTimer` iterates `GetMobilesInRange(0)` without freeing the pooled enumerable, and `Elemental_Apocalypse_Spell` does the same through `Caster.GetMobilesInRange(...)`.
* `Elemental_Echo_Spell` assumes `Caster.Backpack` is non-null while checking and consuming focus gems.
* `Elemental_Soul_Spell` sends `PetResurrectGump` through `master.CloseGump` and `master.SendGump` without checking whether `BaseCreature.GetMaster()` returned null.
* `Elemental_Gate_Spell.InternalItem.Serialize` has no local version integer, despite the class overriding serialization. The class deletes itself on deserialize, but the block does not follow the repository's standard serializer pattern.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0035.
- Backlog rows: RB-06687.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Elementalism/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Command=1; Gump=15; Initialize=1; Movement=1; Speech=1; Timer=7.
- Data/Scripts/Magic/Elementalism/ElementalCommands.cs:L15 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Magic/Elementalism/ElementalCommands.cs:L74 Command CommandSystem.Register access=Unknown
- Data/Scripts/Magic/Elementalism/ElementalEffect.cs:L15 Movement OnMovement access=GlobalOrInternal
- Data/Scripts/Magic/Elementalism/ElementalEffect.cs:L56 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L20 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L154 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L159 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L164 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L169 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs:L174 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/ElementalSpellbook.cs:L82 Gump SendGump access=Internal
- Data/Scripts/Magic/Elementalism/Gumps/ElementalSpellbookGump.cs:L509 Gump OnResponse access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 60.
- Data/Scripts/Magic/Elementalism/ElementalEffect.cs:Server.Items.ElementalEffect version=0 serialize=L42 deserialize=L48 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalExit.cs:Server.Items.ElementalExit version=0 serialize=L114 deserialize=L120 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Apocalypse_Scroll version=0 serialize=L865 deserialize=L871 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Armor_Scroll version=0 serialize=L25 deserialize=L31 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Barrage_Scroll version=0 serialize=L625 deserialize=L631 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Blast_Scroll version=0 serialize=L505 deserialize=L511 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Bolt_Scroll version=0 serialize=L55 deserialize=L61 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Call_Scroll version=0 serialize=L265 deserialize=L271 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Devastation_Scroll version=0 serialize=L745 deserialize=L751 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Echo_Scroll version=0 serialize=L535 deserialize=L541 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Fall_Scroll version=0 serialize=L775 deserialize=L781 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs:Server.Items.Elemental_Field_Scroll version=0 serialize=L385 deserialize=L391 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Magic/Elementalism/ElementalCommands.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalCommands.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalEffect.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalEffect.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalExit.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalExit.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalScrolls.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalShrine.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalSpell.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalSpell.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalSpellbook.cs=Keep
- Data/Scripts/Magic/Elementalism/ElementalSpellbook.cs=Keep
- Data/Scripts/Magic/Elementalism/Gumps/ElementalSpellbookGump.cs=Keep
- Data/Scripts/Magic/Elementalism/Gumps/ElementalSpellbookGump.cs=Keep
- Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledAir.cs=Keep
- Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledAir.cs=Keep
- Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledEarth.cs=Keep
- Data/Scripts/Magic/Elementalism/Mobiles/ElementalCalledEarth.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
