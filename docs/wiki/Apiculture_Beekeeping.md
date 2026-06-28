# System Name: Apiculture (Beekeeping)

## Overview

Apiculture is a beehive addon and wax-crafting system. A placed `apiBeeHive` grows through daily world-save checks, consumes potion treatments, scans nearby `Item` names for water and flowers, produces honey and wax after maturity, and exposes player interaction through `Gump` classes.

There are no custom `CommandSystem.Register` commands, `[Usage]` attributes, packet handlers, or XMLSpawner attachments in this system. Administration uses standard RunUO constructable placement such as `[add apiBeeHiveDeed]`, `[add apiBeeHive]`, `[add HiveTool]`, `[add apiSmallWaxPot]`, `[add apiLargeWaxPot]`, and `[add WaxingPot]`.

## Source Trace

| Script | Purpose |
| --- | --- |
| `Data/Scripts/Trades/Apiculture/BeeHiveAddon.cs` | `apiBeeHive`, hive addon components, bee visual component, and deed. |
| `Data/Scripts/Trades/Apiculture/Gumps/BeeHiveGumps.cs` | Main hive gump, production gump, and destroy confirmation gump. |
| `Data/Scripts/Trades/Apiculture/BeeHiveHelp.cs` | Help gump text for hive care and wax rendering. |
| `Data/Scripts/Trades/Apiculture/beehivehelper.cs` | Enums, world-save update hook, daily hive update logic, and heat-source search helper. |
| `Data/Scripts/Trades/Apiculture/Items/HiveTool.cs` | Harvesting tool with remaining-use tracking. |
| `Data/Scripts/Trades/Apiculture/Items/SmallWaxPot.cs` | Raw-wax rendering pot and `apiBeeHiveSmallPotGump`. |
| `Data/Scripts/Trades/Apiculture/Items/LargeWaxPot.cs` | Pure-wax melting pot. |
| `Data/Scripts/Trades/Apiculture/Items/RawBeeswax.cs` | Stackable raw wax output. |
| `Data/Scripts/Trades/Apiculture/Items/Slumgum.cs` | Stackable rendering waste output. |
| `Data/Scripts/Items/Misc/Beeswax.cs` | Stackable rendered wax item used by wax crafting and other trade recipes. |
| `Data/Scripts/Trades/Apiculture/WaxCrafting.cs` | `WaxingPot` `BaseTool` entry point. |
| `Data/Scripts/Trades/Apiculture/DefWaxCrafting.cs` | Cooking-based wax craft system. |
| `Data/Scripts/Trades/Apiculture/Craft/CandleReligious.cs` | Religious candle wax craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/ColorCandleLong.cs` | Long colored candle wax craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/ColorCandleShort.cs` | Short colored candle wax craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs` | Jarred wax craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/PaintCanvas.cs` | Paint canvas wax craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs` | Wax painting craft output. |
| `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs` | Wax sculpture craft output. |
| `Data/Scripts/Mobiles/Civilized/Vendors/Beekeeper.cs` | Beekeeper vendor using `SBBeekeeper`. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | Buy/sell entries for beekeeping goods. |

`Data/Scripts/Scripts.csproj` includes most Apiculture scripts, but its gump entry points to `Trades\Apiculture\BeeHiveGumps.cs` while the actual file is under `Trades\Apiculture\Gumps\BeeHiveGumps.cs`. See Known Issues.

## Hive Addon Construction

`apiBeeHive` inherits `BaseAddon`.

| Component | Type | ItemID | Offset | Notes |
| --- | --- | --- | --- | --- |
| Table | `AddonComponent` | `2868` | `0,0,0` | Static support component. |
| Hive top | `apiBeeHiveComponent` | `2330` | `0,0,+6` | Opens the main hive gump and displays properties. |
| Bee swarm | `apiBeesComponent` | `0x91B` | `0,0,+6` | Starts hidden and becomes visible after the hive reaches producing stage. |

`apiBeeHiveDeed` is the redeed item. It has `ItemID = 2330`, `Name = "beehive"`, and returns a new `apiBeeHive` from `Addon`.

## Core State

| Field/property | Default or cap | Behavior |
| --- | --- | --- |
| `MaxHoney` | `255` | Maximum stored honey. |
| `MaxWax` | `255` | Maximum stored hive wax. |
| `LessWax` | `true` | Wax production is divided by 3 after base production is calculated. |
| `m_Health` | `10` | Current hive health. Setter clamps to `0..MaxHealth` and calls `Die()` at zero. |
| `MaxHealth` | `10 + ((int)HiveStage * 2)` | Stage-scaled health cap. |
| `HiveStage` | `Stage1` | Uses `HiveStatus`: `Empty = 0`, `Colonizing = 1`, `Brooding = 3`, `Producing = 5`, with stage aliases `Stage1..Stage5`. |
| `Population` | `1` | Clamped to `0..10`; shown as tens of thousands of bees. |
| `ParasiteLevel` | `0` | Clamped to `0..2`. |
| `DiseaseLevel` | `0` | Clamped to `0..2`. |
| Potion counters | `0` | Agility, heal, cure, strength, and poison counters each clamp to `0..2`. |
| `Range` | `Population + 2 + potAgility` | Search range for flower and water item scans. |

`OverallHealth` is calculated from `Health * 100 / MaxHealth`.

| Health percent | Result |
| --- | --- |
| `< 33` | `HiveHealth.Dying` |
| `< 66` | `HiveHealth.Sickly` |
| `< 100` | `HiveHealth.Healthy` |
| `>= 100` | `HiveHealth.Thriving` |

## Daily Update Flow

`BeeHiveHelper.Configure()` subscribes `EventSink.WorldSave` to `EventSink_WorldSave`. Each world save calls `HiveUpdateAll()`, which iterates `World.Items.Values` and runs `HiveUpdate()` for every `apiBeeHive`.

`HiveUpdate(apiBeeHive hive)` exits when the hive is not checkable or `DateTime.Now < hive.NextCheck`. Otherwise it:

1. Sets `NextCheck = DateTime.Now + TimeSpan.FromHours(24.0)`.
2. Resets `LastGrowth` to `None`.
3. Increments `HiveAge`.
4. Recounts flowers and water.
5. Applies potion benefit effects.
6. Applies malady and resource damage.
7. Runs growth or production.
8. Updates new maladies.
9. Invalidates the hive component properties.

## Resource Detection

The hive searches nearby world `Item` instances only.

| Resource | Detection logic |
| --- | --- |
| Water | `item.ItemData.Name.ToUpper().IndexOf("WATER") != -1` |
| Flowers | Name contains `FLOWER`, `SNOWDROP`, or `POPPIE` |

Resource status is scaled against population.

| Method | Formula | Thresholds |
| --- | --- | --- |
| `ScaleWater()` | `WaterInRange * 250 / Population` | `0 = None`; `<33 VeryLow`; `<66 Low`; `<101 Normal`; `<133 High`; otherwise `VeryHigh` |
| `ScaleFlower()` | `FlowersInRange * 100 / Population` | `0 = None`; `<33 VeryLow`; `<66 Low`; `<101 Normal`; `<133 High`; otherwise `VeryHigh` |

`ResourceStatus.TooHigh` exists and the main gump renders it, but neither scaling method returns it.

## Potion Treatment

Players apply potions through the main hive gump buttons. The gump searches the player's backpack for matching `BasePotion` or `PotionKeg` instances.

| Button | Accepted effects | Counter | Cap |
| --- | --- | --- | --- |
| Agility | `PotionEffect.AgilityGreater` | `potAgility` | `2` |
| Poison | `PotionEffect.PoisonGreater`, `PotionEffect.PoisonDeadly` | `potPoison` | `2` |
| Cure | `PotionEffect.CureGreater` | `potCure` | `2` |
| Heal | `PotionEffect.HealGreater` | `potHeal` | `2` |
| Strength | `PotionEffect.StrengthGreater` | `potStrength` | `2` |

Lesser and normal poison, cure, heal, and strength potions return "This potion is not powerful enough to use on a beehive!" Other potion effects return "You cannot use that on a beehive!"

Benefit effects run before growth:

| Effect | Formula |
| --- | --- |
| Poison treatment | Reduces `ParasiteLevel` by available `potPoison`; unused poison remains for later poison damage. |
| Cure treatment | Reduces `DiseaseLevel` by available `potCure`; unused cure remains for poison neutralization. |
| Heal treatment | If the hive has no parasite or disease after treatment, `Health += potHeal * 7`; if no heal potion is present, `Health += 2`. |
| Agility treatment | Increases `Range`, honey production, and wax production for that update cycle; reset after production. |
| Strength treatment | Reduces new parasite and disease chance for that update cycle; reset in `UpdateMaladies()`. |

## Malady And Damage Formulas

After treatment, `ApplyMaladiesEffects()` calculates damage.

| Cause | Damage |
| --- | --- |
| Parasites | `ParasiteLevel * Utility.RandomMinMax(3, 6)` when `ParasiteLevel > 0` |
| Disease | `DiseaseLevel * Utility.RandomMinMax(3, 6)` when `DiseaseLevel > 0` |
| Low water | `(2 - (int)ScaleWater()) * Utility.RandomMinMax(3, 6)` when water status is below `Low` |
| Low flowers | `(2 - (int)ScaleFlower()) * Utility.RandomMinMax(3, 6)` when flower status is below `Low` |

`UpdateMaladies()` then rolls for new parasite and disease levels.

| Malady | Chance |
| --- | --- |
| Parasite | `0.30 - (potStrength * 0.075) + (((int)ScaleWater() - 3) * 0.10) + (HiveAge * 0.01)` |
| Disease | `0.30 - (potStrength * 0.075) + (((int)ScaleFlower() - 3) * 0.10) + (HiveAge * 0.01)` |

If poison remains after parasite treatment, cure potions can neutralize it. Any remaining poison damages the hive by `potPoison * Utility.RandomMinMax(3, 6)`.

## Growth And Production

`Grow()` uses the final post-damage health and resource states.

| Condition | Result |
| --- | --- |
| `OverallHealth < Healthy` | No growth or production; `LastGrowth = NotHealthy` unless population already dropped. |
| `ScaleFlower() < Low` or `ScaleWater() < Low` | No growth or production; `LastGrowth = LowResources` unless population already dropped. |
| `HiveStage < Stage5` | Increments `HiveStage` by one and sets `LastGrowth = Grown`. |
| `HiveStage >= Stage5` | Produces wax and honey, may grow population, and shows the bee swarm component. |

Production formulas:

| Resource | Formula |
| --- | --- |
| Wax | `baseWax = 1`; `+1` if `OverallHealth == Thriving`; `+ potAgility`; `* Population`; if `LessWax`, `Math.Max(1, baseWax / 3)`; then added to `Wax` and clamped by `MaxWax`. |
| Honey | `baseHoney = 1`; `+1` if `OverallHealth == Thriving`; `+ potAgility`; `* Population`; then added to `Honey` and clamped by `MaxHoney`. |
| Population | If `Population < 10` and both flower and water status are at least `Normal`, increments population by one and sets `LastGrowth = PopulationUp`. |

If `Health` reaches zero, `Die()` runs. Producing hives lose one population and set `LastGrowth = PopulationDown`; if population becomes zero, the hive becomes `Empty`. Pre-producing hives become `Empty` immediately. The bee swarm component is hidden.

## Gumps

### `apiBeeHiveMainGump`

Opened by double-clicking `apiBeeHiveComponent`. It shows stage, last-growth indicator, parasite and disease icons, water and flower icons, potion counters, overall health, production access, help, and destroy access.

`OnResponse(NetState sender, RelayInfo info)` exits on button `0`, deleted hive, or the user being more than 3 tiles from the hive. It then requires `m_hive.IsAccessibleTo(from)`.

### `apiBeeHiveProductionGump`

Static settings:

| Setting | Value | Meaning |
| --- | --- | --- |
| `NeedHiveTool` | `true` | Honey and wax harvesting require a `HiveTool` in the backpack. |
| `PureWax` | `false` | Hive wax harvest creates `RawBeeswax`, not `Beeswax`. |

Harvesting behavior:

| Button | Requirements | Output | Cost |
| --- | --- | --- | --- |
| Honey | Producing hive, `Honey >= 3`, `Bottle` in backpack, backpack space, and `HiveTool` if required | One `JarHoney` | Consumes one bottle, subtracts `3` honey, decrements hive tool uses by one |
| Wax | Producing hive, `Wax >= 1`, backpack space, and `HiveTool` if required | `RawBeeswax(m_hive.Wax)` because `PureWax` is false | Sets hive wax to `0`, decrements hive tool uses by one |

When a `HiveTool` reaches zero uses after harvesting, the tool is deleted.

### `apiBeeHiveDestroyGump`

The destroy confirmation creates a new `apiBeeHiveDeed` in the user's backpack. If the deed cannot be placed, the hive is not deleted. If the deed is placed successfully, the hive addon is deleted.

### `apiBeeHiveHelpGump`

`type = 0` documents hive care. `type = 1` documents wax rendering and wax crafting. This help gump is opened from the main hive gump and the small wax pot gump.

## Wax Processing Items

| Item | Default uses | Capacity | Interaction |
| --- | --- | --- | --- |
| `HiveTool` | `50` | N/A | Double-click only displays usage text; harvesting consumes uses from production gump. |
| `apiSmallWaxPot` | `50` | `255` raw or pure wax | Gump-driven raw-wax loading, rendering, and emptying. |
| `apiLargeWaxPot` | `50` | `999` melted pure wax | Target pure `Beeswax` to melt it, or target the pot itself to empty it. Must be in backpack. |
| `WaxingPot` | `20` | Tool uses | Opens the `DefWaxingPot` craft system. |

### Small Wax Pot Rendering

The small pot only accepts `RawBeeswax` from the user's backpack and will not mix raw wax with rendered wax. Rendering requires:

| Requirement | Value |
| --- | --- |
| Uses | `UsesRemaining >= 1` |
| Raw wax | `RawBeeswax >= 10` |
| Heat source | Any heat-source item or static within 2 tiles and overlapping Z range |

Rendering sets the pot graphic to `0x142B`, decrements uses, rolls waste as `Utility.RandomMinMax(1, RawBeeswax / 5)`, optionally gives `Slumgum(Math.Max(1, waste))`, sets `PureBeeswax = RawBeeswax - waste`, and clears `RawBeeswax`.

### Large Wax Pot Melting

The large pot requires the pot to be in the user's backpack. It accepts only rendered `Beeswax`, requires at least one remaining use, requires capacity below `999`, and requires the same heat-source helper as the small pot. It adds the targeted wax into `MeltedBeeswax`, changes `ItemID` to `5162`, plays sound `43`, decrements uses, and retargets until full.

Targeting the large pot itself empties melted wax back into the backpack as `Beeswax(MeltedBeeswax)` and resets `ItemID` to `2541`.

## Wax Crafting

`WaxingPot` is a `BaseTool` whose `CraftSystem` is `DefWaxingPot.CraftSystem`. The wax craft system uses `SkillName.Cooking`, title string `WAX CRAFTING MENU`, `CraftECA.ChanceMinusSixtyToFourtyFive`, and `GetChanceAtMin()` returns `0.5`.

### Craft Menu

| Category | Item | Cooking range | Main resource | Extra resources |
| --- | --- | --- | --- | --- |
| Candles | `Candle` ("Candle, Small") | `5.0..45.0` | `20 Beeswax` | `2 IronIngot` |
| Candles | `CandleLarge` ("Candle, Large") | `15.0..55.0` | `20 Beeswax` | `2 IronIngot` |
| Candles | `ColorCandleShort` | `10.0..50.0` | `10 Beeswax` | None |
| Candles | `ColorCandleLong` | `20.0..60.0` | `20 Beeswax` | None |
| Candles | `WallSconce` | `50.0..90.0` | `20 Beeswax` | `2 IronIngot` |
| Candles | `CandleSkull` | `50.0..90.0` | `20 Beeswax` | `1 Head` |
| Candles | `CandleReligious` | `80.0..120.0` | `20 Beeswax` | `2 IronIngot` |
| Wax Polish | `JarsOfWaxInstrument` | `60.0..100.0` | `10 Beeswax` | `1 Bottle` |
| Wax Polish | `JarsOfWaxLeather` | `60.0..100.0` | `10 Beeswax` | `1 Bottle` |
| Wax Polish | `JarsOfWaxMetal` | `60.0..100.0` | `10 Beeswax` | `1 Bottle` |
| Encaustic Paintings | `WaxPainting` ("Painting, Large") | `60.0..100.0` | `50 Beeswax` | `1 Dyes`, `1 PaintCanvas`, `4 Board` |
| Encaustic Paintings | `WaxPaintingA` through `WaxPaintingG` | `60.0..100.0` | `30 Beeswax` | `1 Dyes`, `1 PaintCanvas`, `4 Board` |
| Wax Scupltors | `WaxSculptors` through `WaxSculptorsD` | `60.0..100.0` | `40 Beeswax` | None |
| Wax Scupltors | `WaxSculptorsE` ("Sculptor, Dragon") | `80.0..120.0` | `60 Beeswax` | None |

The category string is misspelled as `Wax Scupltors` in the craft definition.

### Wax Polish Effects

| Item | Target | Effect | Cap check |
| --- | --- | --- | --- |
| `JarsOfWaxMetal` | Backpack `BaseWeapon` or `BaseArmor` that `MaterialInfo.IsAnyKindOfMetalItem()` accepts | Adds `10` durability bonus and returns a `Bottle` | Refuses when existing durability bonus is greater than `50` |
| `JarsOfWaxLeather` | Backpack `BaseArmor` that `MaterialInfo.IsAnyKindOfClothItem()` accepts | Adds `10` durability bonus and returns a `Bottle` | Refuses when existing durability bonus is greater than `50` |
| `JarsOfWaxInstrument` | Backpack `BaseInstrument` | Adds `20` `UsesRemaining` and returns a `Bottle` | Refuses when existing uses are greater than `200` |

All three wax polish items consume themselves on successful use.

### Paintings And Sculptors

`WaxPainting` and `WaxSculptors` can be double-clicked from the backpack, then targeted at a human or elf body (`606`, `605`, `0x191`, or `0x190`) to rename the item after that character. If the target is the item itself, the code generates a random male or female name and random title.

`WaxPainting` and `WaxSculptors` also store configurable flip IDs through owner-level command properties. The derived painting and sculptor variants write their own version integer and flip IDs after the base class writes the same base data, so the duplicate serialized fields are intentional in the current positional format.

## Vendor Integration

`Beekeeper` inherits `BaseVendor`, calls `m_SBInfos.Add(new SBBeekeeper())`, and uses the `SBBeekeeper` stock list.

| Vendor action | Items |
| --- | --- |
| Buys from vendor | `JarHoney`, sometimes `Beeswax`, `apiBeeHiveDeed`, `HiveTool`, `apiSmallWaxPot`, `apiLargeWaxPot`, `WaxingPot` depending on server sell-chance helpers. |
| Sells to vendor | `JarHoney`, `Beeswax`, `apiBeeHiveDeed`, `HiveTool`, `apiSmallWaxPot`, `apiLargeWaxPot`, `WaxingPot`, and several candle outputs depending on buy-chance helpers. |

The general tinkering craft system also consumes `Beeswax` for `CandleLarge`, `Candelabra`, and `CandelabraStand`, and can craft `WaxingPot` from `10 IronIngot` at `20.0..60.0` tinkering skill.

## Serialization

All Apiculture objects use version `0`.

| Type | Serialized after version |
| --- | --- |
| `apiBeeHive` | Bee component item, health, next check, hive status, last growth, age, population, parasite, disease, flower count, water count, wax, honey, agility/heal/cure/strength/poison counters, hive component item. |
| `apiBeesComponent` | Parent hive item. |
| `apiBeeHiveComponent` | Parent hive item. |
| `apiBeeHiveDeed` | No custom fields. |
| `HiveTool` | Uses remaining. |
| `apiSmallWaxPot` | Uses remaining, raw wax, pure wax. |
| `apiLargeWaxPot` | Uses remaining, melted wax. |
| `RawBeeswax`, `Slumgum`, `Beeswax`, `WaxingPot`, `PaintCanvas`, candle items, simple sculptor items | No custom fields beyond base item data. |
| `WaxPainting` and derived painting classes | Base writes version and flip IDs; derived classes write an additional version and the same flip IDs. |
| `WaxSculptors` and derived sculptor classes | Base writes version and flip IDs; derived classes write an additional version and the same flip IDs. |

Do not reorder or remove fields without a version bump and compatible read path. The current deserializers mostly read version `0` directly and do not include migration branches for newer versions.

## Known Issues

* `Data/Scripts/Scripts.csproj` points to `Trades\Apiculture\BeeHiveGumps.cs`, but the actual gump file is `Trades\Apiculture\Gumps\BeeHiveGumps.cs`. Project-file builds can omit `apiBeeHiveMainGump`, `apiBeeHiveProductionGump`, and `apiBeeHiveDestroyGump`.
* Hive water and flower detection only scans world `Item` instances returned by `Map.GetItemsInRange()`. It does not scan land tiles or static tiles, so natural/static water and flowers are not counted even though the heat-source helper does scan static tiles.
* `ResourceStatus.TooHigh` is defined and rendered by the main gump, but `ScaleWater()` and `ScaleFlower()` never return it.
* `WaxSculptors.WaxTarget.OnTarget()` casts non-mobile targets directly to `Item` in the self-target branch. Targeting a non-`Item` object can throw instead of showing the failure message.
* `WaxingPot(int uses)` ignores the `uses` constructor parameter and always sets `UsesRemaining = 20`.
* `DefWaxingPot.CanCraft()` treats tools as worn out only when `UsesRemaining < 0`, so a zero-use `WaxingPot` is not rejected by that check.
* The production gump contains `ToDo` comments for getting hurt or poisoned while harvesting honey or wax, but the implemented harvest paths have no injury or poison roll.
