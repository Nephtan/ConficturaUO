# Witchcraft

## Overview
Witchcraft is a potion-backed custom magic system in the `Server.Spells.Undead` namespace. Players brew witch mixtures with a `WitchCauldron`, store ingredients and finished mixtures in a `WitchPouch`, and cast the resulting `SpellScroll`-derived potion items by double-clicking them or by using the pouch quickbar.

The brewing side uses `SkillName.Forensics` as the main crafting skill. The casting side uses `SkillName.Necromancy` as `CastSkill` and `SkillName.Forensics` as `DamageSkill`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `UndeadSpell` | `Spell` subclass | Shared Witchcraft spell base. Sets Necromancy as cast skill, Forensics as effect skill, required-skill checks, zero hand clearing, and per-spell mana lookup. |
| `DefWitchery` | `CraftSystem` | Witch brewing recipe list. Uses Forensics as the main craft skill and Necromancy as the secondary skill. |
| `WitchCauldron` | `BaseTool` | Crafting tool that opens `DefWitchery.CraftSystem`. |
| `BookWitchBrewing` | `Item` with nested `Gump` | In-game recipe/help book named `The Witch's Brew`. |
| `WitchBrews.cs` classes | `SpellScroll` subclasses | Sixteen stackable potion items with spell IDs `131` through `146`, item ID `0x282F`, names, and hues. |
| `WitchPouch` | `Bag` subclass | Weight-reducing storage for witch ingredients, cauldrons, jars, books, and finished brews. Also persists quickbar button toggles. |
| `WitchPouch.WitchBag` | `Gump` | Pouch configuration UI. Toggles vertical/horizontal bar mode, title display, and each potion button. |
| `WitchPouch.WitchBar` | `Gump` | Quick potion bar. Each visible button calls `WitchPouch.castSpell`, which double-clicks the matching potion in the player's backpack. |
| `QuickBar` button set 39 | Player command gump entry | Global `[quickbar` support for "Witch Potions" when the quickbar setting is enabled and a `WitchPouch` is in the backpack. |

## Acquisition And Commands
| Access path | Compiled behavior |
| --- | --- |
| Brewing | Double-click an accessible `WitchCauldron` to use the `DefWitchery` craft menu. |
| Vendor stock | `Witches` vendors load `SBWitches`, which sells witch reagents, `WitchCauldron`, `BookWitchBrewing`, and `WitchPouch`. |
| Tinkering | `DefTinkering` can craft `WitchCauldron` from 5 iron ingots with a `20.0` to `70.0` Tinkering range. |
| Dungeon loot | `DungeonLoot` can include `BookWitchBrewing` and `WitchCauldron` in its random book/tool pools. |
| Work shoppe integration | `MorticianShoppe` is displayed as "Witches Work Shoppe", accepts witchery reagents as resources, and accepts `WitchCauldron` as a tool. `MerchantCrate` values each finished Witchcraft brew at `28 * amount` gold and `WitchCauldron` at `8 * amount` gold. |
| Potion casting | Double-click a brew item in the backpack. On successful spell sequencing, the base `Spell` class consumes the brew and adds one `Jar` to the caster's backpack. |
| Pouch organization | Single-click or context-click the `WitchPouch` to open `WitchPouch.WitchBag` while the pouch is in the player's backpack. |
| Quick bar | `[quickbar` opens the global Quick Bar. Its button set `39` is labeled "Witch Potions" and toggles `WitchPouch.WitchBar`. |

No Witchcraft-specific staff or admin command was found in the traced scripts.

## Shared Casting Rules
| Rule | Compiled behavior |
| --- | --- |
| Spell IDs | Registered as `131` through `146` under the `Undead` namespace. |
| Cast skill | `SkillName.Necromancy`. |
| Damage/effect skill | `SkillName.Forensics`. |
| Required skill | Each spell defines `RequiredSkill`; `UndeadSpell.CheckCast` rejects casters below that value unless a derived spell overrides `CheckCast` without calling `base.CheckCast()`. |
| Cast skill range | `GetCastSkills` returns `RequiredSkill` to `RequiredSkill + 20.0`. |
| Mana | Every traced Witchcraft spell returns `RequiredMana = 0`. |
| Base karma effect | `UndeadSpell.CheckCast` awards `-50` karma when the caster's karma is above `-2459`. |
| Brew consumption | `Spell.CheckSequence` consumes the brew item and adds one `Jar` to the caster's backpack for all sixteen Witchcraft brew item types. |
| Potion enhancement | Several effects call `BasePotion.EnhancePotions(caster)` and add that value directly or in a smaller divisor to durations, damage, healing, skill checks, or lock strength. |
| Common targeting | Most brews use a range `12` target cursor. `VampireGiftSpell` uses range `1`. |

## Brewing Recipes
All recipes are in the `Brews` category. Each recipe consumes one `Jar`; failed brewing adds a new `Jar` back to the player's backpack.

| Brew item | Spell ID | Display name | Forensics range | Necromancy range | Ingredients |
| --- | ---: | --- | ---: | ---: | --- |
| `UndeadEyesScroll` | 137 | eyes of the dead mixture | 10.0-30.0 | 5.0-15.0 | `MummyWrap`, `EyeOfToad`, `Jar` |
| `NecroUnlockScroll` | 145 | tomb raiding concoction | 15.0-40.0 | 10.0-20.0 | `Maggot`, `BeetleShell`, `Jar` |
| `NecroPoisonScroll` | 141 | disease draught | 20.0-45.0 | 15.0-25.0 | `VioletFungus`, `NoxCrystal`, `Jar` |
| `PhantasmScroll` | 146 | phantasm elixir | 25.0-50.0 | 20.0-30.0 | `DriedToad`, `GargoyleEar`, `Jar` |
| `RetchedAirScroll` | 136 | retched air elixir | 30.0-55.0 | 25.0-35.0 | `BlackSand`, `GraveDust`, `Jar` |
| `ManaLeechScroll` | 132 | lich leech mixture | 35.0-60.0 | 30.0-40.0 | `DriedToad`, `RedLotus`, `Jar` |
| `WallOfSpikesScroll` | 138 | wall of spikes draught | 40.0-65.0 | 35.0-45.0 | `BitterRoot`, `PigIron`, `Jar` |
| `NecroCurePoisonScroll` | 133 | disease curing concoction | 45.0-70.0 | 40.0-50.0 | `Wolfsbane`, `SwampBerries`, `Jar` |
| `BloodPactScroll` | 140 | blood pact elixir | 50.0-75.0 | 45.0-55.0 | `BloodRose`, `DaemonBlood`, `Jar` |
| `SpectreShadowScroll` | 131 | spectre shadow elixir | 55.0-80.0 | 50.0-60.0 | `VioletFungus`, `SilverWidow`, `Jar` |
| `GhostPhaseScroll` | 144 | ghost phase concoction | 60.0-85.0 | 55.0-65.0 | `BitterRoot`, `MoonCrystal`, `Jar` |
| `HellsGateScroll` | 142 | demonic fire ooze | 65.0-90.0 | 60.0-70.0 | `Maggot`, `BlackPearl`, `Jar` |
| `GhostlyImagesScroll` | 143 | ghostly images draught | 70.0-95.0 | 65.0-75.0 | `MummyWrap`, `Bloodmoss`, `Jar` |
| `HellsBrandScroll` | 134 | hellish branding ooze | 75.0-100.0 | 70.0-80.0 | `WerewolfClaw`, `Brimstone`, `Jar` |
| `GraveyardGatewayScroll` | 135 | black gate draught | 80.0-105.0 | 75.0-85.0 | `BlackSand`, `Wolfsbane`, `PixieSkull`, `Jar` |
| `VampireGiftScroll` | 139 | vampire blood draught | 85.0-120.0 | 80.0-90.0 | `WerewolfClaw`, `BatWing`, `BloodRose`, `Jar` |

## Spell Effects
| Spell | Required skill | Target | Effect |
| --- | ---: | --- | --- |
| `SpectreShadowSpell` | 55.0 | Beneficial `Mobile`, range 12 | Hides the target unless the target is a vendor, player vendor, barkeeper, or has higher `AccessLevel`. Duration is `((6 * caster.Forensics.Fixed) / 50) + 1` seconds. Uses a static timer table keyed by target `Mobile`. |
| `ManaLeechSpell` | 35.0 | Harmful `Mobile`, range 12 | Fizzles if the caster is poisoned. On success, checks reflect circle `7`, clears target paralysis, drains `GetDamageSkill(caster) - GetResistSkill(target) + EnhancePotions(caster)` mana, halves that amount against non-players, clamps it to target mana and caster missing mana, subtracts it from the target, then adds `toDrain + 25` mana to the caster. |
| `UndeadCurePoisonSpell` | 45.0 | Beneficial `Mobile`, range 12 | Attempts to cure target poison. Chance is `(10000 + EnhancePotions(caster) + int(caster.Forensics * 75) - ((poison.Level + 1) * 1750)) / 100`, compared with `Utility.Random(100)`. |
| `HellsBrandSpell` | 75.0 | `RecallRune`, range 12 | Mark-style rune binding. Requires Mark travel checks, rejects `PirateRegion`, rejects blocked world teleport regions and multis, and requires the rune to be in the caster's backpack. |
| `UndeadGraveyardGatewaySpell` | 80.0 | Marked rune, runebook default, or valid house raffle deed | Gate-style travel. Creates a "black gate" at the caster and another at the destination when region checks allow it. Gates last `30 + EnhancePotions(caster)` seconds and delete themselves on load. |
| `RetchedAirSpell` | 30.0 | Harmful `Mobile`, range 12 | Delayed poison-type damage using `GetNewAosDamage(19, 1, 5, target)` and `SpellHelper.Damage(..., 0 physical, 100 poison, 0 fire, 0 cold, 0 energy)`. |
| `UndeadEyesSpell` | 10.0 | `Mobile`, range 10 | Gives Eyes of the Dead buff, sets `LightLevel` to `12`, and holds a `LightCycle` action until a timer expires. The timer duration is `Utility.Random(15 + enhance, 25 + enhance)` minutes using RunUO's `Random(from, count)` semantics. |
| `UndeadWallOfSpikesSpell` | 40.0 | Point, range 12 | Creates up to three blocking invisible-then-visible `InternalItem` fields in an east-west or north-south line based on caster-target position. Timer duration is `10 + int(caster.Forensics / 2) + EnhancePotions(caster)` seconds. |
| `VampireGiftSpell` | 85.0 | Beneficial `Mobile` or `Item`, range 1 | On self, deletes existing owned `SoulOrb` items, creates a "blood of a vampire" `SoulOrb`, and places it in the backpack. On dead `PlayerMobile`, opens a `ResurrectGump`. On dead `BaseCreature`, opens `PetResurrectGump` for `pet.GetMaster()`. On henchman items, resets `HenchDead` to `0` and restores the item name. |
| `UndeadBloodPactSpell` | 50.0 | Beneficial `Mobile`, range 12 | Heals a living, unpoisoned, non-wounded target by `int(caster.Forensics * 0.4) + EnhancePotions(caster) + Utility.Random(8, 15)`, then damages the caster by `int(caster.Forensics * 0.2) + Utility.Random(5, 10)`. |
| `NecroPoisonSpell` | 20.0 | Harmful `Mobile`, range 12 | Checks reflect circle `3`, clears target paralysis, then poison-resist checks against a circle `8` formula. If the caster is within range `2` on the same map, poison level is based on `(Necromancy.Fixed + Poisoning.Fixed + EnhancePotions(caster)) / 2`: `>= 1000` gives level `3`, `> 850` gives level `2`, `> 650` gives level `1`, otherwise level `0`. Outside range `2`, level is `0`. |
| `HellsGateSpell` | 65.0 | Marked rune, runebook default, valid boat key, or runebook-entry constructor path | Recall-style travel. Rejects overload, escape-blocked and teleport-blocked regions, invalid destinations, blocked spawn points, and checked multis. Teleports pets and moves the caster. Only the constructor path with a non-null `Runebook` decrements runebook charges. |
| `GhostlyImagesSpell` | 70.0 | Point, range 12 | Requires follower room for one or two slots depending on `Core.SE`. Summons `dj_nc_decoy`, a melee `BaseCreature` copy of the caster body, hue, name, title, skills, and equipped item layers. Duration is `(EnhancePotions(caster) + caster.Forensics + caster.Necromancy) / 2` seconds. Hides the caster and applies the Ghostly Images buff until the decoy is deleted. |
| `GhostPhaseSpell` | 60.0 | Point, range 12 | Teleport-style movement. Rejects faction sigils, overload, travel-blocked source/destination, blocked spawn points, and multis. Teleports pets for `PlayerMobile` casters, moves the caster by assigning `Location`, and plays teleport particles. |
| `NecroUnlockSpell` | 15.0 | `IPoint3D`, range 12 | Unlocks dungeon doors and `LockableContainer` targets. Container unlock level is `int(caster.Necromancy) + EnhancePotions(caster)`, capped at `90`, and must meet `cont.RequiredSkill`; treasure map chests are excluded. Secured house containers are rejected. |
| `PhantasmSpell` | 25.0 | `TrapableContainer`, range 12 | Disarms a trap when `item.TrapLevel <= int(caster.Forensics)`. On success, sets `TrapType = None`, `TrapPower = 0`, and `TrapLevel = 0`. |

## Witch Pouch Behavior
`WitchPouch` accepts only witchery-related items: the brewing book, cauldron, jars, witch reagents, necromancy reagents used by recipes, and the sixteen brew item classes. Its `GetTotal(TotalType.Weight)` and `UpdateTotal` implementations reduce contained weight to `5%` of normal item weight.

The configuration gump stores these integer flags on the pouch:

| Field | Meaning |
| --- | --- |
| `Bar` | `1` means vertical quickbar; any other value builds the horizontal bar. |
| `Titles` | `1` enables labels beside vertical bar buttons. |
| `UndeadEyes` through `VampireGift` | One integer toggle per brew button. A value greater than `0` displays that brew on the bar. |

The quickbar does not search inside the pouch. `WitchPouch.castSpell` searches the player's main `Backpack` for the matching brew type, double-clicks the first match, and warns "You don't have that brewed!" when no matching item is found.

## Serialization Notes
| Class | Serialized version | Fields |
| --- | ---: | --- |
| `WitchCauldron` | `0` | No custom fields beyond `BaseTool`. |
| Each `WitchBrews.cs` brew item | `0` | No custom fields beyond `SpellScroll`; `Deserialize` resets `ItemID = 0x282F`. |
| `BookWitchBrewing` | `0` | No custom fields. |
| `WitchPouch` | `0` | Writes `Bar`, `Titles`, then the sixteen brew toggle integers in button order. |
| `dj_nc_decoy` | `0` | No custom fields. The caster reference used for buff cleanup is not serialized. |
| `UndeadWallOfSpikesSpell.InternalItem` | `1` | Writes a delta time `m_End`; version `0` fallback restores a 10 second duration. |
| `UndeadGraveyardGatewaySpell.InternalItem` | none | Overrides `Serialize` and `Deserialize` without writing a local version. It deletes itself during `Deserialize`. |

## Known Issues
* `GhostPhaseSpell`, `HellsGateSpell`, and `UndeadGraveyardGatewaySpell` override `CheckCast()` without calling `UndeadSpell.CheckCast()`, so the shared required-skill and karma behavior is bypassed at initial cast validation on those paths.
* Most target spells call `FinishSequence()` inside the spell effect method and again from `OnTargetFinish`, creating repeated finalization paths.
* `WitchPouch.WitchBag`, `WitchPouch.WitchBar`, `WitchPouch.castSpell`, and the global Quick Bar Witch button assume `state.Mobile`, `from`, and `from.Backpack` are non-null.
* `UndeadBloodPactSpell` sends "You cannot heal yourself" for self-targeting but does not return or use `else`, so self-targets can still continue into the healing branch.
* `UndeadEyesSpell` calculates a Necromancy-scaled light level and then immediately overwrites it with `level = 12`; its duration also uses `Utility.Random(from, count)` with `15 + enhance` and `25 + enhance`, not `RandomMinMax`.
* `UndeadWallOfSpikesSpell.InternalItem` starts a timer for `10 + Forensics / 2 + EnhancePotions` seconds, but stores `m_End` as `DateTime.Now + 10.0` seconds, so a save/load shortens enhanced fields.
* `SpectreShadowSpell` hides the target but adds the `SpectralShadow` buff to the caster instead of the target.
* `VampireGiftSpell` assumes `BaseCreature.GetMaster()` returns a non-null `Mobile` before opening `PetResurrectGump`, so dead untamed or masterless creatures can null-reference.
* `HellsGateSpell` only decrements runebook charges when the spell is constructed with a non-null `Runebook`; targeting a `Runebook` from the normal target cursor uses its default entry without setting `m_Book`.
* `UndeadGraveyardGatewaySpell.InternalItem` overrides serialization without writing a version integer; it relies on deleting itself during load instead of restoring state.
