# Runic Tools Crafting

## Overview

Runic tools are `BaseRunicTool` items that open a normal `CraftGump` for a specific `CraftSystem` and, on supported crafted outputs, apply random AOS attributes based on the tool's `CraftResource`.

The shared tool behavior comes from `BaseTool` and `BaseRunicTool`. The normal crafting hooks that consume runic bonuses are `BaseWeapon.OnCraft` and `BaseArmor.OnCraft`; the static `BaseRunicTool.ApplyAttributesTo(...)` overloads are also used by loot, artifacts, fishing rewards, containers, and special crafting code outside the runic-tool item path.

## Source Trace

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Trades/Tools/BaseTool.cs` | Shared craft-tool access checks, craft-gump opening, uses serialization, and `IUsesRemaining` behavior. |
| `Data/Scripts/Items/Trades/Tools/BaseRunicTool.cs` | Runic resource persistence, intensity scaling, unique property selection, skill-bonus selection, and AOS attribute application. |
| `Data/Scripts/Items/Weapons/BaseWeapon.cs` | Applies runic tool attributes to crafted `BaseWeapon` outputs under `Core.AOS`. |
| `Data/Scripts/Items/Armor/BaseArmor.cs` | Applies runic tool attributes to crafted `BaseArmor` and `BaseShield` outputs under `Core.AOS`. |
| `Data/Scripts/Items/Trades/Tools/RunicHammer.cs` | Classic BOD runic smithy hammer item. |
| `Data/Scripts/Items/Trades/Tools/RunicSewingKit.cs` | Classic BOD runic sewing kit item. |
| `Data/Scripts/Items/Trades/Tools/RunicDovetailSaw.cs` | Classic runic carpentry saw item. |
| `Data/Scripts/Trades/Runics/BaseRunicHammer.cs` | Custom constructable smithing runic tool classes. |
| `Data/Scripts/Trades/Runics/BaseRunicSewingKit.cs` | Custom constructable tailoring runic tool classes. |
| `Data/Scripts/Trades/Runics/BaseRunicTinkerTools.cs` | Custom constructable tinkering runic tool classes. |
| `Data/Scripts/Trades/Runics/BaseRunicSaw.cs` | Custom constructable carpentry runic tool classes. |
| `Data/Scripts/Trades/Runics/BaseRunicFletcherTools.cs` | Custom constructable bowcraft/fletching runic tool classes. |
| `Data/Scripts/System/Misc/ResourceInfo.cs` | `CraftAttributeInfo` runic attribute count and intensity definitions by resource. |
| `Data/Scripts/Trades/Bulk Orders/Rewards.cs` | Blacksmith and tailor BOD reward construction for classic runic hammers and sewing kits. |

## Use Flow

1. A player double-clicks a runic tool in their backpack or equipped on their Mobile.
2. `BaseTool.OnDoubleClick` optionally routes through `CaptchaGump` when macro resources are disallowed.
3. The tool opens a `CraftGump` for its `CraftSystem` if `CanCraft` permits the attempt.
4. `CraftItem.CompleteCraft` consumes resources and decrements `tool.UsesRemaining` on successful skill checks and on failed skill checks after resource handling.
5. If `UsesRemaining` drops below 1, the tool is deleted.
6. For crafted `BaseWeapon` and `BaseArmor` outputs under `Core.AOS`, the item `OnCraft` calls `((BaseRunicTool)tool).ApplyAttributesTo(...)`.

Validation failures that happen before the skill attempt, such as inaccessible tool state or missing resources, return a gump or localized message without consuming a use.

## Tool Families

| Class or family | CraftSystem | ItemID | Default display/name behavior | Resources in compiled constructable subclasses |
| --- | --- | ---: | --- | --- |
| `RunicHammer` | `DefBlacksmithy.CraftSystem` | `0x13E3` | Classic "runic smithy hammer" labels by metal index. | Any resource passed to constructor; BOD rewards use Dull Copper through Valorite. |
| `RunicHammer1`, `*Hammer` | `DefBlacksmithy.CraftSystem` | `0x5445` | Name forced to `runic smithing tools`. | Bronze, Copper, DullCopper, Gold, ShadowIron, Valorite, Verite, Agapite. |
| `RunicSewingKit` | `DefTailoring.CraftSystem` | `0x4C81` | Localized `~1_LEATHER_TYPE~ runic sewing kit`. | Any resource passed to constructor; BOD rewards use Spined, Horned, and Barbed leather. |
| `RunicSewingKit1`, `*SewingKit` | `DefTailoring.CraftSystem` | `0x4C81` | Typed subclasses set `runic sewing kit`. | SpinedLeather, BarbedLeather, HornedLeather. |
| `RunicDovetailSaw` | `DefCarpentry.CraftSystem` | `0x1029` | Classic dovetail saw labels by wood index. | Any resource passed to constructor. |
| `RunicSaw`, `*Saw` | `DefCarpentry.CraftSystem` | `0x5446` | Name forced to `runic carpentry tools`. | Ash, Cherry, Ebony, GoldenOak, Hickory, Mahogany, Oak, Pine, Rosewood, Walnut. |
| `RunicFletcherTools`, `*FletcherTools` | `DefBowFletching.CraftSystem` | `0x5444` | Name forced to `runic bowcrafting tools`. | Ash, Cherry, Ebony, GoldenOak, Hickory, Mahogany, Oak, Pine, Rosewood, Walnut. |
| `RunicTinkerTools`, `*TinkerTools` | `DefTinkering.CraftSystem` | `0x5443` | Name forced to `runic tinker tools`. | Verite, Agapite, Bronze, Copper, DullCopper, Gold, ShadowIron, Valorite. |

Typed custom subclasses default to 50 uses. Constructors that accept `(CraftResource resource)` inherit `BaseTool`'s random 25 to 75 initial uses unless a subclass or caller overrides `UsesRemaining`.

## BOD Reward Sources

### Blacksmith Runic Hammers

`CreateRunicHammer(int type)` accepts types 1 through 8 and returns a classic `RunicHammer` using `CraftResource.Iron + type`.

| Type | Resource | AOS uses | Non-AOS uses | Reward point entries |
| ---: | --- | ---: | ---: | --- |
| 1 | DullCopper | 50 | 50 | 500, 550 |
| 2 | ShadowIron | 45 | 50 | 550, 600, 625 |
| 3 | Copper | 40 | 50 | 650, 675 |
| 4 | Bronze | 35 | 50 | 700 |
| 5 | Gold | 30 | 50 | 950 |
| 6 | Agapite | 25 | 50 | 1050 |
| 7 | Verite | 20 | 50 | 1150 |
| 8 | Valorite | 15 | 50 | 1200 |

Some reward groups contain multiple weighted `RewardItem` entries. The point entries above identify the groups where each type can appear; use the BOD reward calculator for final group selection behavior.

### Tailor Runic Sewing Kits

`CreateRunicKit(int type)` accepts types 1 through 3 and returns a classic `RunicSewingKit` using `CraftResource.RegularLeather + type`.

| Type | Resource | Uses | Reward point entry |
| ---: | --- | ---: | ---: |
| 1 | SpinedLeather | 45 | 350 |
| 2 | HornedLeather | 30 | 600 |
| 3 | BarbedLeather | 15 | 700 |

## Runic Resource Profiles

`BaseRunicTool.ApplyAttributesTo(BaseWeapon)` and `BaseRunicTool.ApplyAttributesTo(BaseArmor)` read the tool's `CraftResourceInfo.AttributeInfo`, roll an attribute count between `RunicMinAttributes` and `RunicMaxAttributes`, then use the resource's runic intensity range.

### Metal Resources

| Resource | Attribute count | ML intensity | Non-ML intensity |
| --- | ---: | ---: | ---: |
| DullCopper | 1-2 | 40-100 | 10-35 |
| ShadowIron | 2 | 45-100 | 20-45 |
| Copper | 2-3 | 50-100 | 25-50 |
| Bronze | 3 | 55-100 | 30-65 |
| Gold | 3-4 | 60-100 | 35-75 |
| Agapite | 4 | 65-100 | 40-80 |
| Verite | 4-5 | 70-100 | 45-90 |
| Valorite | 5 | 85-100 | 50-100 |

### Leather Resources

| Resource | Attribute count | ML intensity | Non-ML intensity |
| --- | ---: | ---: | ---: |
| SpinedLeather | 1-3 | 40-100 | 20-40 |
| HornedLeather | 3-4 | 45-100 | 30-70 |
| BarbedLeather | 4-5 | 50-100 | 40-100 |

### Wood Resources

| Resource | Attribute count | ML intensity | Non-ML intensity |
| --- | ---: | ---: | ---: |
| AshTree | 3-4 | 45-100 | 30-70 |
| CherryTree | 4-5 | 50-100 | 40-100 |
| EbonyTree | 2 | 45-100 | 20-45 |
| GoldenOakTree | 3-4 | 60-100 | 35-75 |
| HickoryTree | 1-2 | 40-100 | 10-35 |
| MahoganyTree | 2-3 | 50-100 | 25-50 |
| OakTree | 3 | 55-100 | 30-65 |
| PineTree | 4 | 65-100 | 40-80 |
| RosewoodTree | 4-5 | 70-100 | 45-90 |
| WalnutTree | 5 | 85-100 | 50-100 |

## Attribute Scaling

For runic tools, `Scale(min, max, low, high)` uses a uniform random intensity percentage between the resource's runic min and max. The result is then mapped into the selected property's raw value range:

```text
percent = Utility.RandomMinMax(min, max)
scaledBy = 10000 / (Abs(high - low) + 1)
percent = percent * (10000 + scaledBy)
value = low + (((high - low) * percent) / 1000001)
```

Properties that pass a `scale` argument divide the low/high bounds before scaling, then multiply the result by `scale`. Examples include 2-point hit effect steps, 5-point weapon speed steps, and 10-point durability or lower-requirement steps.

When applying non-runic static loot attributes, `Scale` instead generates `Utility.RandomMinMax(0, 10000)`, square-roots it, subtracts from 100, optionally adds 10 for `LootPack.CheckLuck(m_LuckChance)`, then clamps the result to the passed min/max intensity.

## Weapon Attribute Pool

Crafted `BaseWeapon` outputs use 26 unique property slots. `BaseRanged` weapons pre-block the Use Best Skill / Mage Weapon slot.

| Slot group | Result |
| --- | --- |
| Hit area | One of physical, fire, cold, poison, or energy area, 2-50 in steps of 2. |
| Hit spell | One of magic arrow, harm, fireball, or lightning, 2-50 in steps of 2. |
| Skill conversion | Use Best Skill 1 or Mage Weapon 1-10. Blocked for `BaseRanged`. |
| Primary AOS attributes | Weapon Damage 1-50, Defend Chance 1-15, Cast Speed 1, Attack Chance 1-15, Luck 1-100, Weapon Speed 5-30 in steps of 5, Spell Channeling 1. |
| Hit utility | Hit Dispel, Hit Leech Hits, Hit Lower Attack, Hit Lower Defend, Hit Leech Mana, Hit Leech Stamina, each 2-50 in steps of 2. |
| Weapon secondary | Lower Stat Requirement 10-100 in steps of 10, Physical/Fire/Cold/Poison/Energy Resist Bonus 1-15, Durability Bonus 10-100 in steps of 10. |
| Slayer | `SlayerDeed.GetDeedSlayer(Utility.RandomMinMax(1, 34))`. |
| Elemental damage | Randomly allocates existing physical damage into cold, energy, fire, and poison buckets in 10-point steps, then updates the weapon hue. |
| Skill bonus | One slot at index 0, value 1-15, selected from a weapon-skill-specific pool plus Tactics. |

Applying `SpellChanneling` also subtracts 1 from Cast Speed in the shared `ApplyAttribute(AosAttributes, ...)` helper.

## Armor And Shield Attribute Pool

Crafted `BaseArmor` outputs use a separate armor/shield path. Shields roll from 7 base slots; non-shield armor rolls from 20 slots offset into the armor-specific cases.

| Item surface | Property behavior |
| --- | --- |
| Shield | Spell Channeling 1, Defend Chance 1-15, Reflect Physical 1-15 or Attack Chance 1-15, Cast Speed 1, skill bonus or Lower Stat Requirement 10-100, skill bonus or Self Repair 1-5, Durability Bonus 10-100. |
| Non-shield armor | Skill bonus or Lower Stat Requirement, skill bonus or Self Repair, Durability Bonus, Mage Armor 1, Regen Hits 1-2, Regen Stamina 1-3, Regen Mana 1-2, Night Sight 1, Bonus Hits 1-5, Bonus Stamina 1-8, Bonus Mana 1-8, Lower Mana Cost 1-8, Lower Reagent Cost 1-20, Luck 1-100, Reflect Physical 1-15, and physical/fire/cold/poison/energy resistance bonuses 1-15. |
| Armor exclusions | Armor with `MeditationAllowance.All` blocks Mage Armor. Leather armor from RegularLeather through BarbedLeather blocks Lower Stat Requirement and Durability Bonus. Elf-only armor blocks Night Sight. |

Exceptional armor distributes normal exceptional resistance bonuses before runic attributes. When the tool is a `BaseRunicTool`, exceptional armor uses only 6 distributed bonus points instead of the normal Core.SE 15 or pre-SE 14.

## Static Attribute Helpers Outside Normal Runic Crafting

`BaseRunicTool` also exposes static attribute applicators for `BaseHat`, `BaseClothing`, `BaseJewel`, `BaseQuiver`, `BaseInstrument`, and `Spellbook`. These are used by loot/artifact/special systems, but the normal `OnCraft` methods traced for clothing, jewels, instruments, and quivers do not call the matching static runic helper when the crafting tool is a `BaseRunicTool`.

| Target type | Static pool summary |
| --- | --- |
| `BaseHat` | 34 slots covering stats, regen, Night Sight, cost reduction, Luck, reflect, combat/casting attributes, self repair, durability, elemental resists, and five skill-bonus slots. |
| `BaseClothing` | 32 slots covering reflect, regen, stats, cost reduction, Luck, five skill-bonus slots, combat/casting attributes, and elemental resists. |
| `BaseJewel` and `BaseInstrument` | 32 slots covering elemental resists, combat/casting attributes, stats, potion enhancement, cost reduction, Luck, regen, reflect, weapon speed, and five skill-bonus slots. Instruments use the bard skill pool. |
| `BaseQuiver` | Rolls Damage Increase, Lower Ammo Cost, and Weight Reduction separately, then rolls 22 primary AOS attribute slots. |
| `Spellbook` | 16 slots covering Bonus Int, Bonus Mana, Cast Speed, Cast Recovery, Spell Damage, four skill-bonus slots, Lower Reagent Cost, Lower Mana Cost, Regen Mana, and a random slayer. |

## Skill Bonus Selection

Skill bonuses are selected from different pools based on `AosSkillBonuses.Owner`:

| Owner type | Possible skills |
| --- | --- |
| `BaseShield` | Fencing, Bludgeoning, Parry, Swords, Tactics. |
| `BaseArmor` | Marksmanship, Bushido, Knightship, Fencing, Focus, Healing, Bludgeoning, Parry, Swords, Tactics, FistFighting. |
| `BaseWeapon` with Swords/Marksmanship/Fencing/Bludgeoning/FistFighting skill | The matching weapon skill plus Tactics. |
| `Spellbook` | Magery, Meditation, Psychology, MagicResist. |
| `BaseInstrument` | Discordance, Musicianship, Peacemaking, Provocation. |
| Other owners | Broad global pool from Alchemy through FistFighting as listed in `m_PossibleBonusSkills`. |

The code tries to avoid selecting a skill already present in one of the five bonus slots before setting the requested bonus index.

## Serialization Notes

`BaseTool` serializes version 1 with `Crafter`, `Quality`, and `UsesRemaining`, and keeps a version 0 fallback for old saves that only stored uses.

`BaseRunicTool` serializes version 0 with a single `CraftResource` integer after the base tool data. Concrete runic tool classes then write their own version 0 marker. Several custom concrete classes repair display state during `Deserialize`, such as resetting `ItemID` and forcing the expected tool name.

No custom runic tool class writes additional fields beyond its version marker.

## Admin Commands

No `CommandSystem.Register`, `[Usage]`, or `[Description]` entries are defined in the traced runic tool scripts. The traced administration surface is constructor-based item creation through the normal RunUO constructable-item tooling.

## Known Issues

| Issue | Impact |
| --- | --- |
| `BaseWeapon.OnCraft` and `BaseArmor.OnCraft` apply runic attributes under `Core.AOS` whenever `tool is BaseRunicTool`, without checking that the crafted item resource matches the runic tool resource. The non-AOS weapon branch does perform a resource equality check. | High-tier runic tools can apply their resource's attribute profile to outputs crafted from other materials under AOS rules. |
| Static runic helper pools exist for hats, clothing, jewels, quivers, instruments, and spellbooks, but normal `OnCraft` hooks for clothing, jewels, instruments, and quivers do not call them when a runic tool is used. | Runic tinker, carpentry, tailoring, or bowcraft tools can consume uses on item classes that receive no runic attributes through the normal craft path. |
| `ApplySkillBonus` contains impossible mutual-exclusion checks such as `check == SkillName.Necromancy && check == SkillName.Elementalism`. | The intended Magery/Necromancy/Elementalism conflict prevention does not work; only exact duplicate skill matches are blocked. |
| `ResourceInfo` assigns the non-ML PetrifiedTree runic fallback to `walnuttree.RunicMinIntensity` and `walnuttree.RunicMaxIntensity` instead of `petrifiedtree`. | Non-ML PetrifiedTree runic intensity remains at default values while WalnutTree can be overwritten during PetrifiedTree setup. |
| Blacksmith and tailor BOD rewards construct the classic `RunicHammer` and `RunicSewingKit` classes. No scripted reward source was found in the traced files for the custom `Trades/Runics` subclass families. | Custom runic saw, fletcher, tinker, alternate hammer, and alternate sewing kit subclasses appear to be admin-constructable rather than naturally distributed by the traced reward code. |
