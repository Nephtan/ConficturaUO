# Druidism

## Overview
Druidism is a potion-backed custom spell line in the `Server.Spells.Herbalist` namespace. Players brew druidic mixtures with a `DruidCauldron`, store ingredients and finished mixtures in a `DruidPouch`, and cast the effects by double-clicking the resulting `SpellScroll`-derived potion items.

The same `SkillName.Druidism` also has a direct skill-use handler that opens a creature lore `Gump` for valid animal or creature targets. Spell casting uses `Druidism` as `CastSkill` and `Veterinary` as `DamageSkill`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `HerbalistSpell` | `Spell` subclass | Shared Druidism spell base. Sets `CastSkill = Druidism`, `DamageSkill = Veterinary`, no hand clearing, required-skill checks, mana cost, cast delay, and magic-resist formulas. |
| `DefDruidism` | `CraftSystem` | Druidic herbalism recipe list. Uses `Druidism` as the main craft skill and `Veterinary` as the secondary skill. |
| `DruidCauldron` | `BaseTool` | Crafting tool that opens `DefDruidism.CraftSystem`. |
| `BookDruidBrewing` | `Item` with nested `Gump` | In-game recipe/help book named `Druidic Herbalism`. |
| `HerbalistPotions.cs` classes | `SpellScroll` subclasses | Sixteen stackable potion items with spell IDs `147` through `162`. |
| `DruidPouch` | `Bag` subclass | Weight-reducing storage for druidic brewing items, recipe book, cauldron, jars, reagents, and finished mixtures. Also manages the Druid potion bar. |
| `DruidPouch.DruidBag` | `Gump` | Pouch configuration UI. Toggles vertical/horizontal bar mode, title display, and individual potion buttons. |
| `DruidPouch.DruidBar` | `Gump` | Quick potion bar. Each visible button calls `DruidPouch.castSpell`, which double-clicks the matching potion in the player's backpack. |
| `Druidism` | skill handler | Direct `Druidism` skill use targeter for creature lore. |
| `DruidismGump` | `Gump` | Displays creature stats, resistances, damage profile, taming data, pack instinct, food preferences, barding difficulty, and combat/lore skills. |

## Acquisition And Commands
| Access path | Compiled behavior |
| --- | --- |
| Brewing | Double-click a `DruidCauldron` accessible to the player to use the `DefDruidism` craft menu. |
| Vendor stock | `SBDruid`, `SBDruidTree`, `SBHerbalist`, and `SBDruidGuild` sell Druidism reagents, `DruidCauldron`, `BookDruidBrewing`, and `DruidPouch`. |
| Tinkering | `DefTinkering` can craft `DruidCauldron` from 5 iron ingots with a `20.0` to `70.0` Tinkering range. |
| Potion casting | Double-click a potion item in the backpack. `SpellScroll.OnDoubleClick` creates the spell through `SpellRegistry.NewSpell`. |
| Quick bar | `[quickbar` opens the global Quick Bar. Its Druid button toggles `DruidPouch.DruidBar` when a `DruidPouch` is in the caller's backpack. |
| Skill help | `[skill` opens the player skill text gump, which includes a Druidism description. |
| Direct skill use | Using the `Druidism` skill starts a target cursor asking what animal to inspect. |

No Druidism-specific staff or admin command was found in the traced scripts.

## Shared Casting Rules
| Rule | Compiled behavior |
| --- | --- |
| Spell IDs | Registered as `147` through `162` under the `Herbalist` namespace. |
| Cast skill | `SkillName.Druidism`. |
| Damage/effect skill | `SkillName.Veterinary`. |
| Required skill | Each spell defines `RequiredSkill`; `HerbalistSpell.CheckCast` rejects casters below that value. |
| Cast skill range | `GetCastSkills` returns `RequiredSkill` to `RequiredSkill + 20.0`. |
| Mana | Every traced Druidism spell returns `RequiredMana = 0`. |
| Reagents after brewing | Potion casting uses a non-null scroll item, so the base `Spell.ConsumeReagents` path returns true without consuming reagents. |
| Potion consumption | On successful spell sequencing, the base `Spell` class consumes the potion item and adds one `Jar` back to the caster's backpack. |
| Potion enhancement | Effects call `BasePotion.EnhancePotions`, which returns capped `EnhancePotions` item bonus plus Alchemy skill bonus. Item bonus caps at `50`; Alchemy fixed skill thresholds add `10`, `20`, or `30`. |
| Resist percent | Base Druidism resist uses `max(target.MagicResist / 5.0, target.MagicResist - (((caster.Druidism - 20.0) / 5.0) + (1 + circle) * 5.0)) / 2.0`. |

## Brewing Recipes
All recipes are in the `Brews` category. Each recipe consumes one `Jar`; failed brewing adds a new `Jar` back to the player's backpack.

| Potion item | Spell ID | Display name | Druidism range | Veterinary range | Ingredients |
| --- | ---: | --- | ---: | ---: | --- |
| `LureStonePotion` | 158 | stone in a jar | 10.0-30.0 | 5.0-15.0 | `MoonCrystal`, `SilverWidow`, `Jar` |
| `NaturesPassagePotion` | 159 | nature passage mixture | 15.0-35.0 | 10.0-20.0 | `SeaSalt`, `FairyEgg`, `Jar` |
| `ShieldOfEarthPotion` | 147 | shield of earth liquid | 20.0-40.0 | 15.0-25.0 | `Ginseng`, `BlackPearl`, `Jar` |
| `WoodlandProtectionPotion` | 148 | woodland protection oil | 25.0-45.0 | 20.0-30.0 | `Garlic`, `SwampBerries`, `Jar` |
| `StoneCirclePotion` | 156 | stone rising concoction | 30.0-50.0 | 25.0-35.0 | `BeetleShell`, `SeaSalt`, `Jar` |
| `GraspingRootsPotion` | 151 | grasping roots mixture | 35.0-55.0 | 30.0-40.0 | `MandrakeRoot`, `Ginseng`, `Jar` |
| `DruidicRunePotion` | 157 | druidic marking oil | 40.0-60.0 | 35.0-45.0 | `BlackPearl`, `EyeOfToad`, `Jar` |
| `HerbalHealingPotion` | 150 | herbal healing elixir | 45.0-65.0 | 40.0-50.0 | `RedLotus`, `Garlic`, `Jar` |
| `BlendWithForestPotion` | 152 | forest blending oil | 50.0-70.0 | 45.0-55.0 | `SilverWidow`, `Nightshade`, `Jar` |
| `FireflyPotion` | 162 | jar of fireflies | 55.0-75.0 | 50.0-60.0 | `SpidersSilk`, `ButterflyWings`, `Jar` |
| `MushroomGatewayPotion` | 160 | mushroom gateway growth | 60.0-80.0 | 55.0-65.0 | `Bloodmoss`, `EyeOfToad`, `Jar` |
| `SwarmOfInsectsPotion` | 153 | jar of insects | 65.0-85.0 | 60.0-70.0 | `ButterflyWings`, `BeetleShell`, `Jar` |
| `ProtectiveFairyPotion` | 149 | fairy in a jar | 70.0-90.0 | 65.0-75.0 | `FairyEgg`, `MoonCrystal`, `Jar` |
| `TreefellowPotion` | 155 | treant fertilizer | 75.0-95.0 | 70.0-80.0 | `SwampBerries`, `MandrakeRoot`, `Jar` |
| `VolcanicEruptionPotion` | 154 | volcanic fluid | 80.0-110.0 | 75.0-85.0 | `Brimstone`, `SulfurousAsh`, `Jar` |
| `RestorativeSoilPotion` | 161 | jar of magical mud | 85.0-120.0 | 80.0-90.0 | `Nightshade`, `RedLotus`, `Jar` |

## Spell Effects
| Spell | Required skill | Cast delay | Target | Effect |
| --- | ---: | ---: | --- | --- |
| `LureStoneSpell` | 10.0 | 1.0s | Point, range 12 | Creates a hidden-then-visible lure stone pair for `30 + EnhancePotions(caster)` seconds. The main stone handles movement within range 600 and points eligible `BaseCreature.TargetLocation` to the stone. At `Druidism >= 99.9`, idle creatures can be lured regardless of tamability; otherwise only idle tamable creatures with `MinTameSkill <= Druidism + Veterinary / 100 + 0.1` are affected. |
| `NaturesPassageSpell` | 15.0 | 0.5s | Marked rune, runebook default, or valid boat key | Recall-style travel. Rejects overload, blocked source/destination regions, invalid spawn points, and multis when checked. Teleports pets to the target location and decrements runebook charges only when cast from a runebook entry. |
| `ShieldOfEarthSpell` | 20.0 | 1.0s | Point, range 12 | Creates up to five blocking foliage field items in a line facing east-west or north-south based on caster-to-target position. Each item lasts `30 + EnhancePotions(caster)` seconds and blocks fit. |
| `WoodlandProtectionSpell` | 25.0 | 1.0s | Self | Adds all five resistance mods at `int(Veterinary / 5) + int(EnhancePotions(caster) / 5)`. Duration is `(Veterinary / 2) * 60` seconds. Uses `BeginAction` to prevent recast stacking. |
| `StoneCircleSpell` | 30.0 | 3.0s | Point, range 12 | Creates sixteen blocking stone items around a fixed coordinate pattern near the target. Each item lasts `30 + EnhancePotions(caster)` seconds and `OnMoveOver` blocks crossing. |
| `GraspingRootsSpell` | 35.0 | 1.5s | Harmful Mobile, range 12 | Paralyzes and adds `GraspingRoots` buff. Duration starts at `7 + Veterinary * 0.2`, is halved when `Veterinary < (target.Str + target.Dex) * 0.5`, is clamped to `0..9`, then adds `EnhancePotions(caster) / 10`. Creates a temporary root visual item for 30 seconds. |
| `DruidicRuneSpell` | 40.0 | 3.0s | `RecallRune`, range 12 | Marks a rune in the caster's backpack after Mark travel checks, PirateRegion rejection, region teleport approval, and multi-location checks pass. |
| `HerbalHealingSpell` | 45.0 | 0.5s | Beneficial Mobile, range 12 | Rejects animated dead and dead bonded pets. Cures poison, heals `Druidism + Veterinary + EnhancePotions(caster)`, and calls `RemoveCurseSpell.RemoveBadThings`. |
| `BlendWithForestSpell` | 50.0 | 1.0s | Mobile, range 12 | Hides, squelches, and paralyzes the target for 20 seconds, places a temporary visual item, and hides all controlled pets whose `ControlMaster` is the target. |
| `FireflySpell` | 55.0 | 1.0s | Harmful Mobile, range 12 | Reflect check uses circle `3`. On an unresisted target, clears combat and warmode. `BaseCreature` targets are pacified for `Veterinary / 5 + EnhancePotions(target) / 10` seconds; other Mobile targets are paralyzed for the same duration and get a `Firefly` buff. Uncalmable creatures reject the effect. |
| `MushroomGatewaySpell` | 60.0 | 3.0s | Marked rune, runebook default, or valid boat key | Gate-style travel. Checks GateFrom, source escape/recall permissions, destination teleport permission, spawn blocking, and multi blocking. Creates mushroom-shaped `Moongate` items at the caster and, when the source permits teleport, at the destination. Each gate lasts `30 + EnhancePotions(caster)` seconds. |
| `SwarmOfInsectsSpell` | 65.0 | 2.0s | Harmful Mobile, range 12 | Reflect check uses circle `7`. Calls resist only for Magic Resist skill gain and ignores the return value. Damage is `((caster.Druidism + target.Veterinary) / 10) + int(EnhancePotions(target) / 2)`, minimum `1`. A first hit starts a 20-second timer that restores `int(damage * 0.5)` hits; repeat hits while the timer exists deal one-tenth damage. |
| `ProtectiveFairySpell` | 70.0 | 3.0s | Self | Summons `DruidFairy` for `(Druidism + Veterinary) * 9` seconds. The fairy is an AI mage with 200 hits, 300 stamina, 300 mana, 6-9 damage, 2 control slots, and high energy resistance. |
| `TreefellowSpell` | 75.0 | 5.0s | Point, range 12 | Summons `SummonedTreefellow` for `90 + EnhancePotions(caster)` seconds if the map can spawn at the target location. The treant is an AI melee summoned Mobile with 14-17 energy damage, 1 or 2 control slots depending on `Core.SE`, poison/bleed immunity, and summoned-corpse deletion. |
| `VolcanicEruptionSpell` | 80.0 | 2.0s | Point, range 12 | Targets all mobiles except the caster within `1 + int(Veterinary / 10)` tiles. Base damage is `Utility.Random(27, 22)`, then `EnhancePotions(caster)` is added. Resisted targets take 70 percent of that value. Damage is passed to `SpellHelper.Damage` with type arguments `(50, 100, 0, 0, 0)`. |
| `RestorativeSoilSpell` | 85.0 | 2.0s | Beneficial Mobile or Item, range 1 | Self-targeting deletes existing `SoulOrb` items owned by the caster and adds a new backpack `SoulOrb` named `mystical mud`. Dead `PlayerMobile` targets in range 2 receive a `ResurrectGump`; dead `BaseCreature` targets send `PetResurrectGump` to `pet.GetMaster()`. Henchman item targets clear `HenchDead`, restore the item name, and invalidate properties. |

## Druid Pouch Behavior
`DruidPouch` accepts only the Druidic Herbalism book, `DruidCauldron`, `Jar`, supported reagents, and the sixteen Druidism potion classes. Other dropped items are rejected.

Opening the pouch's organizer sets the pouch's own weight to `1.0`. The pouch overrides total weight so contents count as `5 percent` of their actual item weight. It persists the bar orientation flag, title flag, and one integer toggle for each of the sixteen potion buttons.

`DruidPouch.castSpell` looks for the matching potion type with `from.Backpack.FindItemByType(...)`, which searches backpack contents recursively, and double-clicks the first matching item. If the selected potion type is missing, the pouch warns the player to single-click the bag to organize it.

## Direct Druidism Skill Use
The skill handler registers `SkillInfo.Table[(int)SkillName.Druidism].Callback`. Using the skill targets at range 8.

The caller must be alive and the target must be a `BaseCreature`; henchman mobiles are explicitly rejected. Dead pets are rejected. Creature slayer categories `SlimyScourge`, `ElementalBan`, `Repond`, `Silver`, `GiantKiller`, and `GolemDestruction` are normally rejected as "not an animal", except controlled special creatures of types `RidingDragon`, `Daemonic`, `DrakkhenRed`, `DrakkhenBlack`, `SkeletonDragon`, or `FrankenFighter` owned by the caller.

| Target state | Required behavior |
| --- | --- |
| Caller owns the creature | Opens `DruidismGump` without a skill check. |
| Wild or non-tamable and caller has Druidism below 100.0 | Rejects with "At your skill level, you can only lore tamed creatures." |
| Non-tamable and caller has Druidism below 110.0 | Rejects with "At your skill level, you can only lore tamed or tameable creatures." |
| Other valid creature | Requires `CheckTargetSkill(SkillName.Druidism, c, 0.0, 125.0)` before opening `DruidismGump`. |

## Serialization Notes
Most potion classes serialize only a version integer after `SpellScroll` base serialization. Some potion deserializers also normalize `ItemID` and, for several renamed display items, `Name`.

`BookDruidBrewing`, `DruidCauldron`, `DruidFairy`, and `SummonedTreefellow` all serialize version `0`. `DruidPouch` serializes version `0` followed by all bar and potion-toggle integers in a fixed positional order.

Temporary field and visual items use mixed persistence behavior. Some write a remaining `TimeSpan` and restart their timer, some delete themselves on deserialization, and some have broken read/write alignment; see known issues below before relying on world-save persistence for active Druidism effects.

## Known Issues
| Issue | Impact |
| --- | --- |
| `GraspingRootsSpell.InternalItem.Serialize` writes version `1` and a `TimeSpan`, but `Deserialize` reads only the version and never consumes the `TimeSpan`. | Active Grasping Roots visual items can corrupt subsequent deserialization data in the item stream and do not restart their cleanup timer. |
| `BlendWithForestSpell.InternalItem.Serialize` writes version, `TimeSpan`, `Mobile`, and `bool`, but `Deserialize` reads `Mobile` and `bool` immediately after the version. | Active Blend With Forest items have mismatched serialization and can restore the wrong data or corrupt reads. |
| `BlendWithForestSpell` hides controlled pets owned by the target but never unhides those pets when the effect ends. | Pets can remain hidden after the target's 20-second effect expires. |
| `BlendWithForestSpell.InternalItem.OnAfterDelete` restores `Squelched` but does not restore `Hidden`; only the timer callback sets `Hidden = false`. | Manual deletion or non-timer cleanup can leave the target hidden. |
| `BlendWithForestSpell` adds the `BlendWithForest` buff to `caster`, not to the target Mobile that was hidden and paralyzed. | Buff UI can appear on the wrong player when the caster targets another Mobile. |
| `ShieldOfEarthSpell`, `StoneCircleSpell`, and `LureStoneSpell` timer fields set `m_End` to `DateTime.Now + 30 seconds` while their timers use `30 + EnhancePotions(caster)` seconds. | If saved while active, enhanced-duration fields reload with only the unenhanced remaining duration. |
| `StoneCircleSpell` constructs each blocking stone at the caster location, passes the caster-location line-of-sight check, then moves it to fixed offsets around the target without per-tile fit adjustment. | Stones can be placed in blocked or otherwise invalid positions around the target. |
| `MushroomGatewaySpell.InternalItem` is a `Moongate` that deletes itself on deserialization and does not write a local version. | Active mushroom gates do not survive world saves, regardless of remaining duration. |
| `LureStone` has no version integer in `Serialize` and deletes itself in `Deserialize`. | Any placed legacy `LureStone` item is intentionally non-persistent and does not follow the repository's current serialization standard. |
| `FireflySpell` and `SwarmOfInsectsSpell` call `BasePotion.EnhancePotions(m)` on the target, not on the caster. `SwarmOfInsectsSpell` also uses the target's `Veterinary` in its damage formula. | Target equipment/Alchemy and target Veterinary affect hostile Druidism outcomes. |
| `SwarmOfInsectsSpell` calls `CheckResisted(m)` but ignores the return value. | Magic Resist can gain/check, but resistance never reduces or blocks Swarm damage. |
| `ProtectiveFairySpell.CheckCast` requires room for `Followers + 3`, while `DruidFairy.ControlSlots` is `2`. | The spell rejects some casters who actually have enough follower slots for the summoned fairy. |
| `RestorativeSoilSpell` assumes `pet.GetMaster()` is non-null before calling `CloseGump` and `SendGump`. | Resurrecting a dead `BaseCreature` without a master can throw a null-reference exception. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0088.
- Backlog rows: RB-06686.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Magic/Druidism/HerbalistSpell.cs (CurrentFile)
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs (CurrentFile)
- Data/Scripts/Magic/Druidism/DruidPouch.cs (CurrentFile)
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Gump=16.
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs:L403 Gump OnResponse access=Internal
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs:L418 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs:L649 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L437 Gump OnResponse access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L447 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L450 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L454 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L461 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L475 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L489 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L672 Gump SendGump access=Internal
- Data/Scripts/Magic/Druidism/DruidPouch.cs:L684 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 18.
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs:Server.Items.BookDruidBrewing version=0 serialize=L656 deserialize=L662 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/DruidPouch.cs:Server.Items.DruidPouch version=0 serialize=L1128 deserialize=L1152 alignment=CountMatchNeedsTypeReview:UnknownWrites=18
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.BlendWithForestPotion version=0 serialize=L200 deserialize=L206 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.DruidicRunePotion version=0 serialize=L371 deserialize=L377 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.FireflyPotion version=0 serialize=L543 deserialize=L549 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.GraspingRootsPotion version=0 serialize=L166 deserialize=L172 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.HerbalHealingPotion version=0 serialize=L132 deserialize=L138 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.LureStonePotion version=0 serialize=L405 deserialize=L411 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.MushroomGatewayPotion version=0 serialize=L474 deserialize=L480 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.NaturesPassagePotion version=0 serialize=L440 deserialize=L446 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.ProtectiveFairyPotion version=0 serialize=L97 deserialize=L103 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs:Server.Items.RestorativeSoilPotion version=0 serialize=L508 deserialize=L514 alignment=AlignedByCountAndKnownTypes
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs=Keep
- Data/Scripts/Magic/Druidism/BookDruidBrewing.cs=Keep
- Data/Scripts/Magic/Druidism/DruidPouch.cs=Keep
- Data/Scripts/Magic/Druidism/DruidPouch.cs=Keep
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs=Keep
- Data/Scripts/Magic/Druidism/HerbalistPotions.cs=Keep
- Data/Scripts/Magic/Druidism/HerbalistSpell.cs=Keep
- Data/Scripts/Magic/Druidism/HerbalistSpell.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
