# Harvest Resource System

## Overview
The harvest resource system is the shared `Server.Engines.Harvest` framework used by mining, lumberjacking, fishing, and several custom trade systems. A harvest-capable `Item` starts a target cursor, `HarvestTarget` converts the selected `Static`, `StaticTarget`, or `LandTarget` into tile details, and a `HarvestTimer` resolves range checks, resource-bank checks, skill checks, item construction, delivery, bonus resources, and tool wear.

The traced harvest framework does not register custom `[Command]` handlers, `[Usage]` attributes, `Gump` classes, `NetState` packet handlers, `EventSink` hooks, or XMLSpawner attachments. Player entry is item double-click targeting. Administration uses ordinary RunUO construction such as `[add Shovel]`, `[add Pickaxe]`, `[add FishingPole]`, `[add GargoylesPickaxe]`, `[add ProspectorsTool]`, `[add OreShovel]`, `[add GraveShovel]`, or `[add Monocle]`.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Trades/Harvest/HarvestSystem.cs` | Abstract framework: target setup, captcha gate, concurrency lock, harvest lifecycle, skill checks, resource mutation, item construction, delivery, bonus resources, tool wear, and default tile-detail extraction. |
| `Data/Scripts/Trades/Harvest/HarvestDefinition.cs` | Definition data object: bank geometry, tile whitelist, messages, skill, animation/sound settings, resources, veins, bonus resources, bank cache, tile validation, vein selection, and bonus selection. |
| `Data/Scripts/Trades/Harvest/HarvestBank.cs` | Per-map/per-cell resource bank with current count, randomized maximum, respawn timer, default vein, and randomized vein reset. |
| `Data/Scripts/Trades/Harvest/HarvestResource.cs` | Primary resource entry with required base skill, skill-check range, success message, and constructable result types. |
| `Data/Scripts/Trades/Harvest/HarvestVein.cs` | Weighted vein entry with primary resource, fallback resource, and fallback chance. |
| `Data/Scripts/Trades/Harvest/BonusHarvestResource.cs` | Optional post-success bonus table entry with base-skill gate, chance, message, and result type. |
| `Data/Scripts/Trades/Harvest/HarvestTarget.cs` | Target handler plus lumberjacking special cases for `IChopable`, `IAxe`, `ICarvable`, furniture destruction, and mining `TreasureMap` digging. |
| `Data/Scripts/Trades/Harvest/HarvestTimer.cs` | Repeating effect timer that calls `OnHarvesting` until the last effect tick. |
| `Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs` | Delayed sound timer that plays harvest sounds and calls `FinishHarvesting` on the final tick. |
| `Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs` | Base item for shovel-style harvest tools, use-count persistence, double-click entry, mining stone context menu, and craft-quality use scaling. |

## HarvestSystem Consumers
| Consumer | Tool entry | Primary definition purpose |
| --- | --- | --- |
| `Mining` | `Pickaxe`, `Shovel`, `SturdyPickaxe`, `SturdyShovel`, `GargoylesPickaxe`, `OreShovel` | Ore, stone, sand, regional ore/granite substitutions, and gargoyle pickaxe elemental spawns. |
| `Lumberjacking` | `BaseAxe` weapons and axe-derived harvest tools | Logs, regional log substitutions, wood bonus resources, chopping/carving/furniture special cases. |
| `Fishing` | `FishingPole` | Seafaring-based fish, special fishing loot, sea-serpent loot carriers, wreck/ruin/crash fishing hooks, and pole skill bonuses. |
| `GraveRobbing` | `GraveShovel` | Forensics-based grave tiles, body-part resources, relic bonuses, criminal side effects, undead spawns, and grave chests. |
| `Librarian` | `Monocle` | Inscription-based library tile searches for scrolls, books, and relic texts. |

## Player Harvest Flow
1. The player double-clicks a harvest `Item`.
2. `BaseHarvestTool.OnDoubleClick` requires the item to be in the player's backpack or equipped layer before it calls `HarvestSystem.BeginHarvesting`.
3. `BaseAxe.OnDoubleClick` allows axe-style harvest weapons that are in line of sight, within two tiles, and accessible.
4. `BeginHarvesting` checks tool wear, then either assigns `from.Target = new HarvestTarget(tool, this)` or sends a captcha first when macro resource harvesting is disabled.
5. `HarvestTarget.OnTarget` handles lumberjacking carve/chop/furniture cases and mining `TreasureMap` digging before falling back to `StartHarvesting`.
6. `StartHarvesting` calls `GetHarvestDetails`, resolves the matching `HarvestDefinition` from the target tile ID, checks range/resources/system-specific gates, obtains the concurrency lock, starts `HarvestTimer`, and calls `OnHarvestStarted`.
7. `HarvestTimer` runs a random effect count from `def.EffectCounts`. Each tick revalidates tool, tile, range, resources, and system-specific gates, then plays effects and starts a `HarvestSoundTimer`.
8. The final `HarvestSoundTimer` calls `FinishHarvesting`.
9. `FinishHarvesting` repeats validation, handles `SpecialHarvest`, retrieves the bank and vein, mutates the vein/resource/type, performs the skill check, constructs the `Item`, applies amount and regional substitutions, consumes the bank, delivers the item, rolls bonus resources, decrements tool uses, and calls `OnHarvestFinished`.

## Concurrency Locking
The base `GetLock` returns `typeof(HarvestSystem)`, so a `Mobile` can only run one harvest action at a time across all harvest subclasses. `Fishing.GetLock` overrides this and returns the `Fishing` singleton instance, allowing the fishing lock to be distinct from the base type.

If `from.BeginAction(lockObject)` fails, `OnConcurrentHarvest` is called. The base method is empty; fishing sends localized message `500972` (`You are already fishing.`).

## Tile Validation
`GetHarvestDetails` accepts:

| Target object | Tile ID used | Map/location source |
| --- | --- | --- |
| Immovable `Static` | `(ItemID & 0x3FFF) \| 0x4000` | Static's own `Map` and world location. |
| `StaticTarget` | `(ItemID & 0x3FFF) \| 0x4000` | Player's current `Map` and target location. |
| `LandTarget` | `TileID` | Player's current `Map` and target location. |

`HarvestDefinition.Validate` has two modes:

| Mode | Behavior |
| --- | --- |
| `RangedTiles = false` | Treats `Tiles` as a sorted exact-tile list and scans until the first tile that is greater than or equal to the target tile. |
| `RangedTiles = true` | Treats `Tiles` as start/end pairs and accepts target tile IDs inside any inclusive pair. |

Fishing sets `RangedTiles = true`; mining, sand, lumberjacking, grave robbing, and librarian definitions use exact-tile mode. `Lumberjacking.Initialize()` sorts its tree tile array at script initialization.

## Bank and Respawn Model
Each `HarvestDefinition` owns an in-memory `Dictionary<Map, Dictionary<Point2D, HarvestBank>>`. `GetBank(map, x, y)` rejects `null` and `Map.Internal`, divides `x` and `y` by `BankWidth` and `BankHeight`, then lazily creates a bank at that cell.

`HarvestBank` behavior:

| Step | Compiled behavior |
| --- | --- |
| Creation | `Maximum` is randomized once with `Utility.RandomMinMax(def.MinTotal, def.MaxTotal)`. `Current` starts at `Maximum`. |
| Initial vein | `DefaultVein` is chosen by `def.GetVeinAt(map, bankX, bankY)`. |
| First consumption after full | Sets `Current = Maximum - amount` and schedules `NextRespawn = DateTime.Now + random minutes between MinRespawn and MaxRespawn`. |
| Later consumption | Subtracts `amount` and clamps at `0`. |
| Respawn check | If `Current != Maximum` and `NextRespawn <= DateTime.Now`, `Current` resets to `Maximum`; when `RandomizeVeins` is true, `DefaultVein` is rerolled and assigned to `Vein`. |

Banks are not serialized by this framework. They are rebuilt lazily after process restart, world load, or script reload.

## Vein and Resource Selection
`HarvestDefinition.GetVeinAt` uses one of two random models:

| Setting | Behavior |
| --- | --- |
| `RandomizeVeins = true` | Uses `Utility.RandomDouble()` each time a bank's vein is created or rerolled. |
| `RandomizeVeins = false` | Uses `new Random((bankX * 17) + (bankY * 11) + (map.MapID * 3))`, making the bank's vein deterministic from map and bank coordinates. |

`GetVeinFrom(randomValue)` multiplies the random value by `100`, walks the vein table, subtracts each `VeinChance`, and returns the first vein whose chance covers the remaining value.

`MutateResource` then chooses primary or fallback:

| Condition | Result |
| --- | --- |
| `vein.ChanceToFallback > Utility.RandomDouble() + 0.20` for an elf with `def.RaceBonus = true` | Fallback resource. |
| `vein.ChanceToFallback > Utility.RandomDouble()` for everyone else | Fallback resource. |
| Fallback exists and current effective skill is below `primary.ReqSkill` or `primary.MinSkill` | Fallback resource. |
| Otherwise | Primary resource. |

For fishing, the effective skill used in `MutateResource` is `Fishing.FishSkills(from, true)`, which equals current `Seafaring.Value` plus fishing-pole material bonus.

## Skill and Success Formula
After resource selection, `FinishHarvesting` reads:

| Value | Source |
| --- | --- |
| Base skill gate | `from.Skills[def.Skill].Base`, except fishing uses `Fishing.FishSkills(from, false)`. |
| Skill-check value | `from.Skills[def.Skill].Value`, except fishing uses `Fishing.FishSkills(from, true)`. |
| Check range | `resource.MinSkill..resource.MaxSkill`. |

The harvest only constructs a primary item when `skillBase >= resource.ReqSkill` and the skill test passes. Non-fishing systems use `from.CheckSkill(def.Skill, resource.MinSkill, resource.MaxSkill)`.

Fishing has a special shore-fishing branch: if the tool is a `FishingPole`, the player is not on a boat, and base Seafaring is at least `50.0`, the code sends `You will only get better at seafaring if you fish from a boat.` and sets success to `Utility.RandomMinMax(0, 200) > Fishing.FishSkills(from, true)` instead of calling `CheckSkill`.

## Amounts and Bank Consumption
Stackable harvest results receive an explicit `Amount`.

| Case | Amount |
| --- | --- |
| Normal area | `def.ConsumedPerHarvest`. |
| `Worlds.GetMyWorld(...) == "the Isles of Dread"` | `def.ConsumedPerIslesDreadHarvest`. |
| `reg.IsPartOf("the Mines of Morinia")`, item is `BaseOre`, and `Utility.RandomMinMax(1, 3) > 1` | `def.ConsumedPerIslesDreadHarvest`. |
| `BlankScroll` | Random from `def.ConsumedPerHarvest` through `def.ConsumedPerHarvest + (Inscribe.Value / 10)`. |

The bank consumes `item.Amount`, not always the definition's base amount. Non-stackable items keep their constructor amount and still pass that amount to `bank.Consume`.

## Delivery and Tool Wear
`Give(m, item, placeAtFeet)` first tries `m.PlaceInBackpack(item)`. If backpack delivery fails and `placeAtFeet` is false, the harvest is treated as pack-full. If `placeAtFeet` is true, the system scans items at the player's feet, tries to stack with any existing item, and then moves the item to the world at the player's location.

Bonus resources always call `Give(from, bonusItem, true)`, regardless of the definition's `PlaceAtFeetIfFull` setting.

On a successful primary harvest, any `IUsesRemaining` tool:

1. Sets `ShowUsesRemaining = true`.
2. Decrements `UsesRemaining` by one if it is above zero.
3. Deletes the tool and sends `ToolBrokeMessage` when uses drop below one.

`BaseHarvestTool` serializes version `1` with `Crafter`, `Quality`, and `UsesRemaining`. Version `0` contained only `UsesRemaining`.

## Concrete Definition Summary
| Definition | Bank size | Bank total | Respawn | Skill | Range | Normal amount | Isles of Dread amount | Effects |
| --- | ---: | ---: | --- | --- | ---: | --- | --- | --- |
| Mining ore/stone | `8 x 8` | `10..34` | `10..20` min | `Mining` | `2` | `1 * MyServerSettings.Resources()` | Normal plus half plus `3` | Action `11`, sounds `0x125/0x126`, count `1`, delay `1.6s`, sound delay `0.9s`. |
| Mining sand | `8 x 8` | `6..12` | `10..20` min | `Mining` | `2` | `1 * MyServerSettings.Resources()` | Normal plus half plus `2` | Action `11`, sounds `0x125/0x126`, count `6`, delay `1.6s`, sound delay `0.9s`. |
| Lumberjacking | `4 x 3` | `20..45` | `20..30` min | `Lumberjacking` | `2` | `5 * MyServerSettings.Resources()` | Normal plus half plus `Utility.RandomMinMax(5, 10)` | Action `13`, sound `0x13E`, count `1` on AOS or random `1/2/2/2/3`, delay `1.6s`, sound delay `0.9s`. |
| Fishing | `8 x 8` | `5..15` | `10..20` min | `Seafaring` | `4` | `1 * MyServerSettings.Resources()` | Normal plus half plus `2` | Action `12`, no harvest sounds, count `1`, effect delay `0`, sound delay `8.0s`. |

## Mining Resources
For ore/stone, `Mining.GetResourceType` normally returns the ore type. A `PlayerMobile` with `StoneMining`, `ToggleMiningStone`, and base Mining at least `100.0` has a `10%` chance to receive the granite type instead. The elemental type is used only by `GargoylesPickaxe` post-harvest spawn logic.

| Primary result | ReqSkill | Skill check | Vein weight | Fallback chance | Fallback | Extra type |
| --- | ---: | --- | ---: | ---: | --- | --- |
| `IronOre` / `Granite` | `0.0` | `0.0..100.0` | `45%` | `0%` | None | None |
| `DullCopperOre` / `DullCopperGranite` | `65.0` | `25.0..105.0` | `15%` | `50%` | Iron | `DullCopperElemental` |
| `ShadowIronOre` / `ShadowIronGranite` | `70.0` | `30.0..110.0` | `11%` | `50%` | Iron | `ShadowIronElemental` |
| `CopperOre` / `CopperGranite` | `75.0` | `35.0..115.0` | `8%` | `50%` | Iron | `CopperElemental` |
| `BronzeOre` / `BronzeGranite` | `80.0` | `40.0..120.0` | `6%` | `50%` | Iron | `BronzeElemental` |
| `GoldOre` / `GoldGranite` | `85.0` | `45.0..125.0` | `5%` | `50%` | Iron | `GoldenElemental` |
| `AgapiteOre` / `AgapiteGranite` | `90.0` | `50.0..130.0` | `4%` | `50%` | Iron | `AgapiteElemental` |
| `VeriteOre` / `VeriteGranite` | `95.0` | `55.0..135.0` | `3%` | `50%` | Iron | `VeriteElemental` |
| `ValoriteOre` / `ValoriteGranite` | `99.0` | `59.0..139.0` | `2%` | `50%` | Iron | `ValoriteElemental` |
| `DwarvenOre` / `DwarvenGranite` | `100.1` | `69.0..140.0` | `1%` | `50%` | Iron | `EarthElemental` |

`GargoylesPickaxe` mutates ore/stone veins up one table row when possible and then has a `10%` post-harvest chance to spawn the vein's elemental type if the primary resource was delivered. `OreShovel` mutates any ore/stone vein back to the first vein, forcing iron.

Sand is a separate mining definition with one resource: `Sand`, `ReqSkill = 100.0`, skill check `70.0..400.0`, and `100%` vein weight. `Mining.CheckHarvest` requires the caller to be a `PlayerMobile` with `SandMining = true` and base Mining at least `100.0`.

When `Core.ML` is enabled, mining also installs bonus resources: `99.4%` no bonus, and `0.1%` each for `Amber`, `Amethyst`, `Citrine`, `Diamond`, `Emerald`, `Ruby`, `Sapphire`, `StarSapphire`, and `Tourmaline`, all gated by base Mining `90.0`.

## Lumberjacking Resources
Lumberjacking requires the axe to be equipped for serious wood chopping. `LumberAxe` mutates every lumber vein to ordinary logs.

| Primary result | ReqSkill | Skill check | Vein weight | Fallback chance | Fallback |
| --- | ---: | --- | ---: | ---: | --- |
| `Log` | `0.0` | `0.0..85.0` | `30%` | `0%` | None |
| `AshLog` | `55.0` | `25.0..90.0` | `15%` | `50%` | Ordinary log |
| `CherryLog` | `60.0` | `30.0..95.0` | `10%` | `50%` | Ordinary log |
| `EbonyLog` | `65.0` | `35.0..100.0` | `9%` | `50%` | Ordinary log |
| `GoldenOakLog` | `70.0` | `40.0..105.0` | `8%` | `50%` | Ordinary log |
| `HickoryLog` | `75.0` | `45.0..110.0` | `7%` | `50%` | Ordinary log |
| `MahoganyLog` | `80.0` | `50.0..115.0` | `6%` | `50%` | Ordinary log |
| `OakLog` | `85.0` | `55.0..120.0` | `5%` | `50%` | Ordinary log |
| `PineLog` | `90.0` | `65.0..125.0` | `4%` | `50%` | Ordinary log |
| `RosewoodLog` | `95.0` | `75.0..130.0` | `3%` | `50%` | Ordinary log |
| `WalnutLog` | `100.0` | `85.0..135.0` | `2%` | `50%` | Ordinary log |
| `ElvenLog` | `100.1` | `95.0..140.0` | `1%` | `50%` | Ordinary log |

Bonus resources are always compiled for lumberjacking: `83.9%` no bonus, `10%` `OilWood`, `3%` `ReaperOil`, `2%` `MysticalTreeSap`, and `1%` `HomePlants_Mushroom`, all non-empty bonuses gated by base Lumberjacking `100.0`.

## Fishing Resources
Fishing uses one bank resource, `Fish`, with `ReqSkill = 0.0`, skill check `0.0..100.0`, and a `100%` vein. The definition skill is `SkillName.Seafaring`, but the system adds fishing-pole material bonuses through `Fishing.FishSkills`.

Fishing's `MutateType` can replace normal fish before construction. Its chance formula is:

`chance = (Fishing.FishSkills(from, true) - MinSkill) / (MaxSkill - MinSkill)`

The replacement happens when `skillBase >= ReqSkill`, the deep-water requirement passes, and `chance > Utility.RandomDouble()`.

| Replacement group | ReqSkill | Chance range | Deep water required | Result pool |
| --- | ---: | --- | --- | --- |
| Special nets | `80.0` | `(skill - 80.0) / 4000.0` | Yes | `SpecialFishingNet`, `SpecialFishingNet`, `NeptunesFishingNet`, `FabledFishingNet`, `FabledFishingNet` |
| Treasure/salvage | `90.0` | `(skill - 80.0) / 4000.0` | Yes | `TreasureMap`, `PearlSkull`, `MessageInABottle` |
| Rare fish | `0.0` | `(skill - 125.0) / -2500.0` | No | `PrizedFish`, `WondrousFish`, `TrulyRareFish`, `PeculiarFish` |
| Junk/seaweed | `0.0` | `(skill - 105.0) / -525.0` | No | `WetClothes`, `RustyJunk`, `SpecialSeaweed` |
| Nets/sailor/bag/fish | `50.0` | `(skill - 125.0) / -1125.0` | No | `FishingNet`, `CorpseSailor`, `SunkenBag`, `NewFish` |
| Null result sentinel | `0.0` | `(skill - 200.0) / -400.0` | No | `null` |

Fishing bonus resources are `99.4%` no bonus and `0.6%` `MysticalPearl` gated by base Seafaring plus pole bonus of at least `80.0`.

When the delivered item is a treasure map, message in a bottle, fishing net, special fishing net, fabled fishing net, or Neptune's fishing net, `Fishing.Give` moves the item into a spawned sea creature instead of giving it directly to the player. The creature type is randomly selected from `GiantEel`, `GiantSquid`, `SeaSerpent`, `DeepSeaSerpent`, and `RottingSquid`.

## Shared Regional Post-Processing
After a stackable item is constructed and before delivery, `HarvestSystem` applies several hard-coded world and region substitutions.

| Condition | Result |
| --- | --- |
| Sea exploration, `Shipwreck Grotto`, or `Barnacled Cavern`; item is non-ordinary `BaseLog`; `Utility.RandomBool()` passed | Replaces the log with `DriftwoodLog`. |
| Sea exploration, `Shipwreck Grotto`, `Barnacled Cavern`, or sea town; item is non-standard `BaseOre`; `Utility.RandomBool()` passed | Replaces the ore with `NepturiteOre`. |
| Same sea conditions; item is non-standard `BaseGranite`; `Utility.RandomBool()` passed | Replaces the granite with `NepturiteGranite`. |
| Underworld world, `Map.SavagedEmpire`, item is `AgapiteOre`, `VeriteOre`, or `ValoriteOre`, and `Utility.RandomMinMax(1, 2) == 1` | Replaces ore with `XormiteOre`. |
| Underworld world on other maps, same ore types and roll | Replaces ore with `MithrilOre`. |
| Serpent Island world, same ore types and roll | Replaces ore with `ObsidianOre`. |
| Underworld world, `Map.SavagedEmpire`, item is `AgapiteGranite`, `VeriteGranite`, or `ValoriteGranite`, and `Utility.RandomMinMax(1, 2) == 1` | Replaces granite with `XormiteGranite`. |
| Underworld world on other maps, same granite types and roll | Replaces granite with `MithrilGranite`. |
| Serpent Island world, same granite types and roll | Replaces granite with `ObsidianGranite`. |
| `NecromancerRegion`, item is ash/cherry/golden oak/hickory/mahogany log | Replaces log with `EbonyLog`. |
| `NecromancerRegion`, item is walnut/rosewood/pine/oak log | Replaces log with `GhostLog`. |
| Underworld world and item is a non-ordinary `BaseLog` | Replaces log with `PetrifiedLog`. |
| Mining in the Savaged Empire and Mining skill value is greater than `Utility.RandomMinMax(1, 500)` | Adds `DugUpCoal(1..2)` to the backpack. |
| Mining on the Island of Umber Veil and Mining skill value is greater than `Utility.RandomMinMax(1, 500)` | Adds `DugUpZinc(1..2)` to the backpack. |
| Fishing near a huge shipwreck, space crash, or underwater ruins and effective fishing skill is at least `Utility.RandomMinMax(1, 250)` | Calls the matching special fishing loot hook. |

## Serialization and Persistence
The core harvest definitions, bank cache, timers, veins, resources, and bonus-resource entries are not RunUO world-serialized objects. Their state is rebuilt from static scripts and lazy bank creation.

Harvest tools are serialized as `Item` subclasses:

| Class | Version behavior |
| --- | --- |
| `BaseHarvestTool` | Version `1` writes `Crafter`, `Quality`, and `UsesRemaining`; version `0` reads only `UsesRemaining`. |
| `Shovel`, `SturdyShovel`, `FishingPole`, `OreShovel`, `GraveShovel`, `Monocle` | Call base serialization and then write/read version `0` with no extra fields. |
| `BaseAxe` | Version `2` writes `ShowUsesRemaining`; version `1` writes `UsesRemaining`; version `0` ensures old axes get at least `150` uses. |
| `Pickaxe`, `SturdyPickaxe`, `GargoylesPickaxe` | Call axe/base weapon serialization and then write/read version `0` with no extra fields. |
| `ProspectorsTool` | Version `1` writes `UsesRemaining`; version `0` falls back to `50` uses. |

## Known Issues
* `HarvestSystem.Give` iterates `m.GetItemsInRange(0)` without storing and freeing the returned `IPooledEnumerable`, so every pack-full/place-at-feet stack scan can leak pooled enumerables.
* Bonus-resource failure deletes the primary harvested `item` instead of the failed `bonusItem`, so a pack-full bonus failure can destroy the already-delivered primary resource object reference path rather than cleaning up the bonus object.
* `FinishHarvesting` does not null-check `bonusItem` before calling `Give(from, bonusItem, true)`. If a bad bonus type fails construction, `Mobile.PlaceInBackpack` can dereference the null item.
* Shore fishing at base Seafaring `50.0+` bypasses `CheckSkill` and succeeds when `Utility.RandomMinMax(0, 200) > Fishing.FishSkills(from, true)`, making this success gate get worse as effective skill rises.
* `HarvestSystem.Construct` catches all exceptions and converts them into a normal harvest failure, which hides broken item constructors or script load regressions from administrators.
* Ranged tile validation assumes an even number of start/end entries and can read past the array if a future `HarvestDefinition` supplies an odd-length ranged tile list.
* `HarvestDefinition.GetBank` divides by `BankWidth` and `BankHeight` without guard checks. Current traced definitions set non-zero values, but a malformed future definition would throw.
* `HarvestDefinition.GetVeinFrom` returns `null` if the configured `VeinChance` values do not cover the random roll. Current traced core definitions sum to `100%`, but malformed future definitions can silently abort a harvest.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0022.
- Backlog rows: RB-06702.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Trades/Harvest/HarvestSystem.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestDefinition.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestBank.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestResource.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestVein.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/BonusHarvestResource.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestTarget.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestTimer.cs (CurrentFile)
- Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs (CurrentFile)
- Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Timer=2.
- Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs:L5 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Trades/Harvest/HarvestTimer.cs:L5 Timer CustomTimerSubclass access=GlobalOrInternal

### Serialization Evidence

- Serialized rows matched: 1.
- Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs:Server.Items.BaseHarvestTool version=1 serialize=L222 deserialize=L234 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs=Keep
- Data/Scripts/Items/Trades/Harvest Tools/BaseHarvestTool.cs=Keep
- Data/Scripts/Trades/Harvest/BonusHarvestResource.cs=Keep
- Data/Scripts/Trades/Harvest/BonusHarvestResource.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestBank.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestBank.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestDefinition.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestDefinition.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestResource.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestResource.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestSoundTimer.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestSystem.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestSystem.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestTarget.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestTarget.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestTimer.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestTimer.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestVein.cs=Keep
- Data/Scripts/Trades/Harvest/HarvestVein.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
