# Trade Guild Enhancements

## Overview
Trade Guild Enhancements are an item-driven equipment enhancement system in `Server.Items`. Players use one of five extraordinary guild tools, target an eligible `Item` in their backpack, and choose an `AttributeHandler` entry from an `EnhancementGump`. Successful upgrades spend gold, then write directly to the target item's existing AOS attribute collections, resistance collections, or armor bonus properties.

The traced system does not register any custom `[Command]` handlers, packet handlers, `EventSink` hooks, or XMLSpawner attachments. Normal administration uses standard RunUO construction such as `[add GuildHammer]`, `[add GuildCarpentry]`, `[add GuildFletching]`, `[add GuildSewing]`, or `[add GuildTinkering]`.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Trades/Guild/EnhancementStoneProcess.cs` | Shared `GuildCraftingProcess` state, base target validation, gold spending, creator discount, sound effects, and upgrade cost formula. |
| `Data/Scripts/Trades/Guild/AttributeHandler.cs` | Static enhancement catalog, item-type eligibility rules, conflict filters, current-value lookup, and actual attribute writes. |
| `Data/Scripts/Trades/Guild/Gumps/EnhancementGump.cs` | Two-column `Gump` that lists available enhancement buttons and dispatches button IDs back to `GuildCraftingProcess`. |
| `Data/Scripts/Trades/Guild/GuildHammer.cs` | Blacksmith guild tool for metal weapons and armor. |
| `Data/Scripts/Trades/Guild/GuildCarpentry.cs` | Carpenter guild tool for wooden melee weapons and wooden armor. |
| `Data/Scripts/Trades/Guild/GuildFletching.cs` | Archer guild tool for wooden `BaseRanged` weapons. |
| `Data/Scripts/Trades/Guild/GuildSewing.cs` | Tailor guild tool for clothing, cloth armor, and selected cloth/leather special weapons or magic wearables. |
| `Data/Scripts/Trades/Guild/GuildTinkering.cs` | Tinker guild tool for ordinary jewelry-style `BaseJewel` targets. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | Vendor buy lists sell the five extraordinary guild tools for `500` gold with random stock `1..5`. |

## Tool Entry Requirements
Every tool only starts targeting for a `PlayerMobile`. The player must belong to the matching `NpcGuild`, have at least `90.0` in the matching skill, and satisfy the location gate.

The location gate passes when any one of these is true:

1. A matching guildmaster `Mobile` is within `20` tiles of the tool `Item`.
2. A matching immovable `BaseShoppe` subtype is within `20` tiles and has `ShoppeOwner == from`.
3. The player is on `Map.SavagedEmpire` with `X > 1054`, `X < 1126`, `Y > 1907`, and `Y < 1983`.

| Tool class | Constructed name | ItemID / Hue / Weight | Guild and skill gate | Target classes accepted before shared enhancement checks |
| --- | --- | --- | --- | --- |
| `GuildHammer` | `Extraordinary Smithing Hammer` | `0xFB5` / `0x430` / `5.0` | `NpcGuild.BlacksmithsGuild`; `SkillName.Blacksmith >= 90.0` | `BaseWeapon` or `BaseArmor` for which `MaterialInfo.IsAnyKindOfMetalItem(item)` is true. |
| `GuildCarpentry` | `Extraordinary Woodworking Tools` | `0x1EBA` / `0x430` / `5.0` | `NpcGuild.CarpentersGuild`; `SkillName.Carpentry >= 90.0` | Non-`BaseRanged` `BaseWeapon` or `BaseArmor` for which `MaterialInfo.IsAnyKindOfWoodItem(item)` is true. |
| `GuildFletching` | `Extraordinary Bowcrafting Tools` | `0x1EB8` / `0x430` / `5.0` | `NpcGuild.ArchersGuild`; `SkillName.Bowcraft >= 90.0` | `BaseRanged` for which `MaterialInfo.IsAnyKindOfWoodItem(item)` is true. |
| `GuildSewing` | `Extraordinary Sewing Kit` | `0x4C81`, randomly changed to `0x4C80` / `0x430` / `2.0` | `NpcGuild.TailorsGuild`; `SkillName.Tailoring >= 90.0` | `BaseClothing`; cloth `BaseArmor`; selected magic robe/hat/cloak/boots/belt/sash targets; `BaseWhip`, `ThrowingGloves`, `PugilistGlove`, `PugilistGloves`, and `PugilistMits`. |
| `GuildTinkering` | `Extraordinary Tinkers Tools` | `0x1EBB` / `0x430` / `5.0` | `NpcGuild.TinkersGuild`; `SkillName.Tinkering >= 90.0` | `BaseJewel` except magic torches, talismans, candles, robes, hats, cloaks, boots, belts, and sashes. |

All five tools reject targets whose `RootParent` is not the player and reject `ILevelable` items with the message `You cannot enhance legendary artefacts!`.

## Enhancement Flow
1. The tool sends a target cursor with `from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnTarget))`.
2. `OnTarget` applies the tool-specific target class and material checks.
3. A `GuildCraftingProcess` is created with `Owner = from` and `ItemToUpgrade = target`.
4. `BeginProcess()` accepts only `BaseShield`, `BaseClothing`, `BaseArmor`, `BaseWeapon`, `BaseJewel`, or `Spellbook`.
5. The process loops through `AttributeHandler.Definitions`, reads each current value, counts every value greater than `0`, and counts every value at that handler's `MaxValue`.
6. If `CurrentAttributeCount > 10` or `MaxedAttributes >= 10`, the process blocks enhancement. At exactly `10` current attributes, the `Gump` can still show existing non-maxed attributes but hides brand-new ones.
7. `EnhancementGump` lists handlers whose `IsUpgradable(ItemToUpgrade)` returns true. Button IDs are `1000 + definitionIndex`.
8. `OnResponse(NetState sender, RelayInfo info)` maps the button ID back to `AttributeHandler.Definitions[info.ButtonID - 1000]` and calls `BeginUpgrade(handler)`.
9. `BeginUpgrade` recomputes cost, spends gold, calls `handler.Upgrade(ItemToUpgrade, false)`, and reopens the process.

## Gold Cost Formula
`GuildCraftingProcess` uses a fixed `BaseCost` of `100`. `AttrCountAffectsCost` is compiled as `false`, so the attribute-count multiplier always remains `1`.

| Condition | Formula effect |
| --- | --- |
| Target was crafted by the same `Mobile` | `BaseCost` is halved from `100` to `50`, but only for `BaseClothing`, `BaseArmor`, and `BaseWeapon` because `IsCraftedByEnhancer` checks only those classes. |
| Target was not crafted by the same `Mobile` | `BaseCost` remains `100`. |
| Current value is below max | `cost = ((currentValue + 1) * handler.Cost) * effectiveBaseCost`. |
| Current value is at or above max | `cost = 0`, and `BeginUpgrade` refuses the upgrade. |
| Caller has `AccessLevel.GameMaster` or higher | The purchase succeeds without consuming gold and sends the admin would-have-withdrawn message. |
| Normal player has backpack gold | Backpack `Gold` is consumed first. |
| Normal player lacks backpack gold but has bank gold | Bank `Gold` is consumed through `FindBankNoCreate()`. |
| Normal player lacks both | Localized message `500192` is sent when a backpack exists and the bank spend also fails. |

## Enhancement Eligibility Filters
These filters are applied after the tool-specific target checks.

| Target type | Additional filters |
| --- | --- |
| `BaseShield` | `ArtifactRarity > 0` blocks most handlers. `AttackChance` is blocked when `Core.ML` is true. `ReflectPhysical` is blocked when `Core.ML` is false. `LowerStatReq` is blocked when `!Core.AOS` or the shield resource is leather through barbed leather. |
| `BaseArmor` | `ArtifactRarity > 0` blocks most handlers. `LowerStatReq` is blocked when `!Core.AOS` or the armor resource is leather through barbed leather. `NightSight` is blocked for elf-required armor. |
| `BaseWeapon` | `ArtifactRarity > 0` blocks most handlers. `UseBestSkill` and `MageWeapon` are mutually exclusive. Only one area hit property is allowed among physical, fire, cold, poison, and energy. Only one spell hit property is allowed among magic arrow, harm, fireball, and lightning. |
| `BaseJewel` | `ArtifactRarity > 0` blocks enhancement. |
| `BaseClothing` | `ArtifactRarity > 0` blocks enhancement. |
| `Spellbook` | Only handler allow-flags are checked; there is no artifact-rarity check in the traced code. |
| Any handler | If the current stored value is greater than or equal to that handler's `MaxValue`, the handler is not upgradable. |

## Attribute Catalog
Every compiled handler currently has `IncrementValue = 1`. The `Cost unit` column is multiplied by the formula above, not spent directly.

| Display name | Storage target | Max | Cost unit | Eligible base types from definition |
| --- | --- | ---: | ---: | --- |
| Spell Channeling | `AosAttribute.SpellChanneling` | 1 | 200 | `BaseWeapon`, `BaseShield` |
| Defense Chance Increase | `AosAttribute.DefendChance` | 15 | 10 | `BaseArmor`, `BaseWeapon`, `BaseJewel`, `BaseShield`, `BaseClothing` |
| Reflect Physical Damage | `AosAttribute.ReflectPhysical` | 50 | 2 | `BaseArmor`, `BaseShield`, `BaseClothing` |
| Hit Chance Increase | `AosAttribute.AttackChance` | 15 | 10 | `BaseArmor`, `BaseWeapon`, `BaseJewel`, `BaseShield`, `BaseClothing` |
| Lower Requirements | `AosArmorAttribute.LowerStatReq` | 100 | 2 | `BaseArmor`, `BaseShield` |
| Lower Requirements | `AosWeaponAttribute.LowerStatReq` | 100 | 2 | `BaseWeapon` |
| Self Repair | `AosArmorAttribute.SelfRepair` | 5 | 100 | `BaseArmor`, `BaseShield` |
| Mage Armor | `AosArmorAttribute.MageArmor` | 1 | 200 | `BaseArmor` |
| Hit Point Regeneration | `AosAttribute.RegenHits` | 5 | 5 | `BaseArmor`, `BaseClothing` |
| Stamina Regeneration | `AosAttribute.RegenStam` | 5 | 5 | `BaseArmor`, `BaseClothing` |
| Mana Regeneration | `AosAttribute.RegenMana` | 5 | 5 | `BaseArmor`, `Spellbook`, `BaseClothing` |
| Night Sight | `AosAttribute.NightSight` | 1 | 6 | `BaseArmor`, `BaseJewel`, `BaseClothing` |
| Hit Point Increase | `AosAttribute.BonusHits` | 20 | 5 | `BaseArmor`, `BaseClothing` |
| Stamina Increase | `AosAttribute.BonusStam` | 20 | 5 | `BaseArmor`, `BaseClothing` |
| Mana Increase | `AosAttribute.BonusMana` | 20 | 5 | `BaseArmor`, `Spellbook`, `BaseClothing` |
| Lower Mana Cost | `AosAttribute.LowerManaCost` | 50 | 5 | `BaseArmor`, `BaseJewel`, `Spellbook`, `BaseClothing` |
| Lower Reagent Cost | `AosAttribute.LowerRegCost` | 50 | 5 | `BaseArmor`, `BaseJewel`, `Spellbook`, `BaseClothing` |
| Luck | `AosAttribute.Luck` | 500 | 2 | `BaseArmor`, `BaseWeapon`, `BaseJewel`, `BaseClothing` |
| Physical Resist | `BaseArmor.PhysicalBonus` | 20 | 5 | `BaseArmor` |
| Fire Resist | `BaseArmor.FireBonus` | 20 | 5 | `BaseArmor` |
| Cold Resist | `BaseArmor.ColdBonus` | 20 | 5 | `BaseArmor` |
| Poison Resist | `BaseArmor.PoisonBonus` | 20 | 5 | `BaseArmor` |
| Energy Resist | `BaseArmor.EnergyBonus` | 20 | 5 | `BaseArmor` |
| Hit Physical Area | `AosWeaponAttribute.HitPhysicalArea` | 50 | 3 | `BaseWeapon` |
| Hit Fire Area | `AosWeaponAttribute.HitFireArea` | 50 | 3 | `BaseWeapon` |
| Hit Cold Area | `AosWeaponAttribute.HitColdArea` | 50 | 3 | `BaseWeapon` |
| Hit Poison Area | `AosWeaponAttribute.HitPoisonArea` | 50 | 3 | `BaseWeapon` |
| Hit Energy Area | `AosWeaponAttribute.HitEnergyArea` | 50 | 3 | `BaseWeapon` |
| Hit Magic Arrow | `AosWeaponAttribute.HitMagicArrow` | 50 | 3 | `BaseWeapon` |
| Hit Harm | `AosWeaponAttribute.HitHarm` | 50 | 3 | `BaseWeapon` |
| Hit Fireball | `AosWeaponAttribute.HitFireball` | 50 | 3 | `BaseWeapon` |
| Hit Lightning | `AosWeaponAttribute.HitLightning` | 50 | 3 | `BaseWeapon` |
| Use Best Weapon Skill | `AosWeaponAttribute.UseBestSkill` | 1 | 10 | `BaseWeapon` |
| Mage Weapon | `AosWeaponAttribute.MageWeapon` | 1 | 5 | `BaseWeapon` |
| Damage Increase | `AosAttribute.WeaponDamage` | 50 | 5 | `BaseWeapon`, `BaseJewel` |
| Swing Speed Inrease | `AosAttribute.WeaponSpeed` | 40 | 6 | `BaseWeapon` |
| Hit Dispel | `AosWeaponAttribute.HitDispel` | 50 | 3 | `BaseWeapon` |
| Hit Life Leech | `AosWeaponAttribute.HitLeechHits` | 50 | 3 | `BaseWeapon` |
| Hit Lower Attack | `AosWeaponAttribute.HitLowerAttack` | 50 | 3 | `BaseWeapon` |
| Hit Lower Defense | `AosWeaponAttribute.HitLowerDefend` | 50 | 3 | `BaseWeapon` |
| Hit Mana Leech | `AosWeaponAttribute.HitLeechMana` | 50 | 3 | `BaseWeapon` |
| Hit Stamina Leech | `AosWeaponAttribute.HitLeechStam` | 50 | 3 | `BaseWeapon` |
| Physical Resist | `AosElementAttribute.Physical` | 20 | 2 | `BaseJewel`, `BaseClothing` |
| Fire Resist | `AosElementAttribute.Fire` | 20 | 2 | `BaseJewel`, `BaseClothing` |
| Cold Resist | `AosElementAttribute.Cold` | 20 | 2 | `BaseJewel`, `BaseClothing` |
| Poison Resist | `AosElementAttribute.Poison` | 20 | 2 | `BaseJewel`, `BaseClothing` |
| Energy Resist | `AosElementAttribute.Energy` | 20 | 2 | `BaseJewel`, `BaseClothing` |
| Strength Bonus | `AosAttribute.BonusStr` | 10 | 10 | `BaseJewel`, `BaseClothing` |
| Dexterity Bonus | `AosAttribute.BonusDex` | 10 | 10 | `BaseJewel`, `BaseClothing` |
| Intelligence Bonus | `AosAttribute.BonusInt` | 10 | 10 | `BaseJewel`, `Spellbook`, `BaseClothing` |
| Enhance Potions | `AosAttribute.EnhancePotions` | 25 | 2 | `BaseJewel`, `BaseClothing` |
| Faster Casting | `AosAttribute.CastSpeed` | 20 | 4 | `BaseJewel`, `Spellbook`, `BaseClothing` |
| Faster Cast Recovery | `AosAttribute.CastRecovery` | 20 | 4 | `BaseJewel`, `Spellbook`, `BaseClothing` |
| Spell Damage Increase | `AosAttribute.SpellDamage` | 24 | 4 | `BaseJewel`, `Spellbook`, `BaseClothing` |

## Persistence
The extraordinary guild tools are normal world-save `Item` subclasses. Each has a `Serial` constructor, calls `base.Serialize(writer)`, writes version `0`, calls `base.Deserialize(reader)`, and reads the version integer. `GuildSewing.Deserialize` also normalizes the `ItemID` back to `0x4C81` if the saved item ID is not one of the two expected sewing-kit IDs.

`GuildCraftingProcess`, `EnhancementGump`, and `AttributeHandler` do not serialize their own state. Enhanced stats persist through the target item's normal AOS attribute, resistance, weapon-attribute, armor-attribute, or armor-bonus serialization.

## Known Issues
* `Data/Scripts/Scripts.csproj` includes `Trades\Guild\EnhancementGump.cs`, but the source file is actually `Data/Scripts/Trades/Guild/Gumps/EnhancementGump.cs`. In an explicit `.csproj` build this can either omit the gump source or fail on a missing compile item.
* `EnhancementGump.OnResponse` trusts any `ButtonID >= 1000` and indexes `AttributeHandler.Definitions` without an upper-bound check, so stale or crafted gump replies can throw.
* `EnhancementGump.OnResponse` does not validate `sender.Mobile`, ownership, backpack containment, deletion state, or whether the target is still eligible before spending gold and applying the upgrade.
* The five tool entry checks iterate `GetMobilesInRange(20)` and `GetItemsInRange(20)` without freeing the returned `IPooledEnumerable`.
* `GuildCraftingProcess.SpendGold` plays the guild sound after a failed purchase and gives no failure message when a non-GM owner has no backpack container.
* `AttributeHandler.Initialize()` appends to the static public `Definitions` list without clearing it first, so repeated initialization in the same AppDomain would duplicate every enhancement entry.
* The gump layout has space for 24 visible entries. A target with more than 24 eligible handlers can wrap back onto column two and overwrite earlier rows/buttons.
* `AttributeHandler.IsUpgradable` can re-enable `MageArmor` on `BaseArmor` after earlier checks set `allowed = false` when the armor already has `ArmorMeditationAllowance.All`.
* Target callbacks ignore non-`Item` targets without feedback.
