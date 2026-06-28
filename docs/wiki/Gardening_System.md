# Gardening System

## Overview
The gardening system lives in `Server.Engines.Plants` and centers on `PlantBowl`, `PlantItem`, `Seed`, `PlantSystem`, and the plant-care `Gump` classes. Players fill an empty plant bowl with dirt, soften the bowl with water, plant a seed, maintain the resulting `PlantItem`, and eventually harvest seeds or special resources from mature plants.

There are no in-game commands registered by this system. Runtime entry points are item double-clicks, targeting callbacks, `Gump` replies, and `PlantSystem.Configure`, which hooks RunUO world/load/save/login events.

## Core Scripts
| Script | Role |
| --- | --- |
| `Data/Scripts/Trades/Gardening/PlantBowl.cs` | Empty plant bowl item, dirt targeting, fertile dirt consumption, and dirt-patch tile matching. |
| `Data/Scripts/Trades/Gardening/PlantItem.cs` | Filled bowl/plant item, plant status, secure access, double-click gump entry, seed planting, watering, potion application, death, and persistence. |
| `Data/Scripts/Trades/Gardening/PlantSystem.cs` | Growth timer, health, maladies, care effects, seed/resource production, event hooks, and embedded persistence for non-decorative plants. |
| `Data/Scripts/Trades/Gardening/Seed.cs` | Seed item, random seed generators, seed labeling, and seed-to-plant targeting. |
| `Data/Scripts/Trades/Gardening/PlantType.cs` | Plant type table, first-generation plants, peculiar/bonsai generators, and type cross-pollination rules. |
| `Data/Scripts/Trades/Gardening/PlantHue.cs` | Plant hue table, first-generation hues, bright/crossable flags, and hue cross-pollination rules. |
| `Data/Scripts/Trades/Gardening/PlantResources.cs` | Mature resource lookup table and resource item factory. |
| `Data/Scripts/Trades/Gardening/PlantPourTarget.cs` | Target callback for pouring water, potions, or kegs onto a plant. |
| `Data/Scripts/Trades/Gardening/PollinateTarget.cs` | Target callback for self-pollination and cross-pollination. |
| `Data/Scripts/Trades/Gardening/Gumps/MainPlantGump.cs` | Main plant care gump, water/potion buttons, help-topic packets, and bowl-emptying entry. |
| `Data/Scripts/Trades/Gardening/Gumps/ReproductionGump.cs` | Pollination, resource, seed, and decorative-mode controls. |
| `Data/Scripts/Trades/Gardening/Gumps/SetToDecorativeGump.cs` | Confirmation gump that prunes a stage 9 plant into decorative mode. |
| `Data/Scripts/Trades/Gardening/Gumps/EmptyTheBowlGump.cs` | Confirmation gump that returns an empty `PlantBowl` and, for pre-plant growth stages, the seed. |
| `Data/Scripts/Trades/Gardening/Network/DisplayHelpTopic.cs` | Packet `0xBF` helper used by the plant gumps to open client help topics. |
| `Data/Scripts/Trades/Gardening/MiscItems/*.cs` | Harvested resources: `RedLeaves`, `OrangePetals`, `GreenThorns`, and `FertileDirt`. |

## Player Entry Flow
1. A player double-clicks a `PlantBowl` in their backpack.
2. The `PlantBowl` target accepts either a recognized dirt tile or a stack of `FertileDirt` in the player's backpack.
3. Targeting normal dirt creates `new PlantItem(false)`. Targeting fertile dirt creates `new PlantItem(true)` and consumes dirt.
4. The filled `PlantItem` starts as `PlantStatus.BowlOfDirt` and owns a `PlantSystem`.
5. A player must water the dirt until `PlantSystem.Water >= 2`.
6. A player double-clicks a `Seed` in their backpack and targets the filled bowl.
7. `PlantItem.PlantSeed` copies the seed's `PlantType`, `PlantHue`, and `ShowType`, deletes the seed, sets `PlantStatus.Seed`, and resets the plant system without clearing current potions.
8. Double-clicking a non-decorative `PlantItem` opens `MainPlantGump` if the plant is in range, in line of sight, and usable from a backpack, bank box, locked-down location, or secure container.

## Access Rules
| Action | Required state |
| --- | --- |
| Use empty `PlantBowl` | Bowl must be in the player's backpack. |
| Fill bowl from `FertileDirt` | Dirt must be in the player's backpack; amount must be at least 20 under `Core.ML`, otherwise 40. |
| Fill bowl from dirt patch | Target must be a matching land/static dirt tile within target range. |
| Use `Seed` | Seed must be in the player's backpack. |
| Open plant gump | Plant must be non-decorative, within two tiles and line of sight. |
| Care, pour, harvest, pollinate, empty, or prune | Plant must pass `IsUsableBy`: in backpack, in bank box, locked down and accessible, or inside an accessible secure container. |

## Plant State
| `PlantStatus` | Integer | Meaning in code |
| --- | ---: | --- |
| `BowlOfDirt` | 0 | Filled bowl with no seed. |
| `Seed` / `Stage1` | 1 | Newly planted seed. |
| `Sapling` / `Stage2` | 2 | Early sapling stage. |
| `Stage3` | 3 | Intermediate growth. |
| `Plant` / `Stage4` | 4 | Plant stage shown as a plant rather than only dirt/seed. |
| `Stage5` | 5 | Intermediate growth. |
| `Stage6` | 6 | Intermediate growth. |
| `FullGrownPlant` / `Stage7` | 7 | Full-grown and pollen-producing if type and hue are crossable. |
| `Stage8` | 8 | Late growth. |
| `Stage9` | 9 | Mature plant; can be pruned into decorative mode. |
| `DecorativePlant` | 10 | No upkeep, no `PlantSystem`, no more seed/resource production. |
| `DeadTwigs` | 11 | Dead plant item state for plants that died at full-grown or later. |

## Health And Growth
| Formula or rule | Code behavior |
| --- | --- |
| Growth interval | `PlantSystem.CheckDelay` is 23 hours. |
| Max hits | `10 + ((int)PlantStatus * 2)`. |
| Health percentage | `Hits * 100 / MaxHits`. |
| Health bands | `<33%` is `Dying`; `<66%` is `Wilted`; `<100%` is `Healthy`; `100%` is `Vibrant`. |
| Dirt-only tick | If status is `BowlOfDirt`, water decreases by 1 when `Water > 2` or on a 90% random roll, then the check ends. |
| Valid growth location | Locked down with no root parent, in an owning `Mobile` backpack, or in that `Mobile` bank box. |
| Invalid location | Sets `GrowthIndicator.InvalidLocation` and does not apply growth or care changes. |
| Not ready | If current time is before `NextGrowth`, sets `GrowthIndicator.Delay`. |
| Unhealthy plant | If health is below `Healthy`, sets `GrowthIndicator.NotHealthy` and does not advance stage. |
| Fertile dirt boost | Fertile dirt has a 10% chance to advance by two stages when current status is stage 5 or lower. |
| Normal growth | Healthy plants below stage 9 advance by one stage. |
| Stage 9 auto-pollination | If stage is 9 or higher and not already pollinated, the system self-pollinates with the plant's current type and hue. |

`PlantSystem.Configure` registers growth checks on world load, world save when `Misc.AutoRestart.Enabled` is false, and player login. Login checks plants in the logging-in `Mobile` backpack and bank box. `GrowAll` checks growable plants with no `Mobile` root parent.

## Care Inputs
| Input | Accepted item/effect | Runtime effect |
| --- | --- | --- |
| Water | Non-empty, pourable `BaseBeverage` containing `BeverageType.Water` | Decrements beverage quantity and increases `Water` by 1, clamped to 0-4. |
| Poison potion | `PotionEffect.PoisonGreater` or `PotionEffect.PoisonDeadly` from `BasePotion` or `PotionKeg` | Adds one `PoisonPotion` charge, max 2. During growth, charges first reduce infestation; leftover charges become plant poison. |
| Cure potion | `PotionEffect.CureGreater` | Adds one `CurePotion` charge, max 2. During growth, charges first reduce fungus; leftover charges become plant disease. |
| Heal potion | `PotionEffect.HealGreater` | Adds one `HealPotion` charge, max 2. During growth, charges reduce poison, then disease; if no maladies remain, leftover charges heal 7 hits each. |
| Strength potion | `PotionEffect.StrengthGreater` | Adds one `StrengthPotion` charge, max 2. During growth, strength reduces new infestation/fungus chance, then resets to 0. |
| Lesser/normal potion tiers | Lesser or normal poison/cure/heal/strength effects | Rejected as not powerful enough. |
| Other item or potion effects | Anything not listed above | Rejected as unusable on plants. |

After beneficial effects, the plant takes damage from maladies and water imbalance. Each point of infestation, fungus, poison, disease, excess water above 2, or missing water below 2 adds `Utility.RandomMinMax(3, 6)` damage. If hits reach zero, `PlantItem.Die` resets pre-full-grown plants to `BowlOfDirt` and clears potions; full-grown or later plants become `DeadTwigs`.

## Malady Updates
| Malady | New chance per growth check |
| --- | --- |
| Infestation | `0.30 - (StrengthPotion * 0.075) + ((Water - 2) * 0.10)`, plus `0.10` if the plant type is flowery, plus `0.10` if the hue is bright. |
| Fungus | `0.15 - (StrengthPotion * 0.075) + ((Water - 2) * 0.10)`. |
| Water decay | Water decreases by 1 when `Water > 2` or on a 90% random roll. |
| Poison | Remaining poison potion charges become poison levels. |
| Disease | Remaining cure potion charges become disease levels. |

Infestation, fungus, poison, disease, and potion charge fields are clamped to 0-2. Water is clamped to 0-4.

## Seeds And Cross-Pollination
| Area | Rule |
| --- | --- |
| First-generation seed type | Randomly one of `CampionFlowers`, `Fern`, or `TribarrelCactus`. |
| First-generation seed hue | Randomly one of `Plain`, `Red`, `Blue`, or `Yellow`. |
| Pollen-producing plant | Plant must be crossable and at least `FullGrownPlant`. |
| Crossable type | Only the first-generation 17 core plant types are marked crossable. Bonsai and peculiar plants are not crossable. |
| Crossable hue | `Plain`, red/blue/yellow, bright primary hues, mixed primary hues, and bright mixed hues are flagged crossable. Black, white, pink, magenta, aqua, and fire red are not. |
| Self-pollination | Sets seed type and hue to the source plant's type and hue. |
| Type cross | If the two type enum indexes are adjacent, randomly returns one parent; otherwise returns `(firstIndex + secondIndex) / 2`. |
| Hue cross | 1% chance of black or white. If either parent is plain, result is plain. Same non-bright hue becomes bright. Two primary hues combine. Primary plus secondary returns the primary. Two secondary hues return their bitwise intersection. |
| Seed production | Mature stage 9 plants produce one available seed per growth check while pollinated, crossable, and `LeftSeeds > 0`. |
| Seed cap | New plants start with `LeftSeeds = 8`; each produced seed moves one from left to available. |

The reproduction gump lets players inspect pollination, resource, and seed state; gather pollen; gather available resources; gather available seeds; and, at stage 9, open the decorative-mode confirmation.

## Plant Resources
Mature plants produce at most eight resources through the same `AvailableResources`/`LeftResources` model used for seeds. A plant produces resources only if its exact type and hue exist in `PlantResourceInfo`.

| Plant type | Hue | Resource item |
| --- | --- | --- |
| `ElephantEarPlant` | `BrightRed` | `RedLeaves` |
| `PonytailPalm` | `BrightRed` | `RedLeaves` |
| `CenturyPlant` | `BrightRed` | `RedLeaves` |
| `Poppies` | `BrightOrange` | `OrangePetals` |
| `Bulrushes` | `BrightOrange` | `OrangePetals` |
| `PampasGrass` | `BrightOrange` | `OrangePetals` |
| `SnakePlant` | `BrightGreen` | `GreenThorns` |
| `BarrelCactus` | `BrightGreen` | `GreenThorns` |

## Harvested Resource Items
| Item | Behavior |
| --- | --- |
| `RedLeaves` | Stackable 0.1-weight item. Double-click targets a `BaseBook` in the player's backpack; consumes one leaf and sets `BaseBook.Writable = false` if the book is still writable. |
| `OrangePetals` | Stackable 0.1-weight item. Consumes one petal and applies a five-minute static context to the `Mobile`; `Poison.cs` cures poison levels below 3 while the context is active. A second petal cannot be used until the first context expires. |
| `GreenThorns` | Stackable one-weight item. Targets land within three tiles, consumes one thorn, starts a three-minute per-mobile action cooldown, and plays a terrain-specific timed effect. |
| `FertileDirt` | Stackable one-weight item. Used to fill plant bowls with fertile dirt. |

## Green Thorn Effects
`GreenThornsEffect.Create` rejects locations where the map cannot spawn a mobile, then maps the targeted land tile to one effect class. Spawn helpers attempt up to five random adjacent placements around the target location.

| Terrain effect | Result |
| --- | --- |
| `DirtGreenThornsEffect` | Four waves of two reagent/fertile-dirt drops. Each drop is 10-25 of one randomly selected standard reagent or `FertileDirt`. |
| `SandGreenThornsEffect` | Same reagent/fertile-dirt drop sequence as dirt terrain. A commented alternate Solen-hive teleporter implementation remains in the file but is not compiled. |
| `FurrowsGreenThornsEffect` | Spawns one `VorpalBunny` and sets its `Combatant` to the planter if placement succeeds. |
| `SwampGreenThornsEffect` | Spawns one `WhippingVine` and sets its `Combatant` to the planter if placement succeeds. |
| `SnowGreenThornsEffect` | Spawns one `GiantIceWorm` and three `IceSnake` mobiles, each targeting the planter if placement succeeds. |

## Seed Sources
The core seed class exposes:

| Method | Output |
| --- | --- |
| `new Seed()` | Random first-generation type and first-generation hue with `ShowType = false`. |
| `new Seed(PlantType, PlantHue, bool)` | Exact seed constructor used by harvest and loot code. |
| `Seed.RandomBonsaiSeed()` | Bonsai seed with `increaseRatio = 0.5`, `PlantHue.Plain`, and `ShowType = false`. |
| `Seed.RandomBonsaiSeed(double increaseRatio)` | Weighted bonsai seed. Common entries are weighted at 1, uncommon at `k`, rare at `k^2`, exceptional at `k^3`, and exotic at `k^4`, where `k = max(increaseRatio, 0)`. |
| `Seed.RandomPeculiarSeed(int group)` | Group 1-4 peculiar plant seed, plain hue, and `ShowType = false`; values outside 1-3 fall through to group 4. |

External loot and vendor scripts create seeds and dirt from many locations. The gardening package itself does not register a command or a centralized drop table.

## Emptying And Decorative Mode
`EmptyTheBowlGump` creates a new empty `PlantBowl` in the player's backpack and deletes the current `PlantItem`. If the current plant is a seed or sapling below `PlantStatus.Plant`, it also returns a matching `Seed`. If the backpack cannot hold both returned items, it deletes the newly created items and leaves the plant unchanged.

`SetToDecorativeGump` is available only at `PlantStatus.Stage9`. Confirming it sets `PlantStatus.DecorativePlant`, which nulls out the `PlantSystem`, stops upkeep, and stops future seed/resource production.

## Serialization
| Type | Version | Serialized data |
| --- | ---: | --- |
| `PlantBowl` | 0 | No custom fields. |
| `Seed` | 0 | `PlantType`, `PlantHue`, `ShowType`; deserialize forces weight back to 1.0 if needed. |
| `PlantItem` | 1 | `SecureLevel`, `PlantStatus`, `PlantType`, `PlantHue`, `ShowType`, and `PlantSystem` only while status is below `DecorativePlant`. Version 0 loads default secure level as `CoOwners`. |
| `PlantSystem` | 1 | `FertileDirt`, `NextGrowth`, `GrowthIndicator`, water, hits, four malady fields, four potion fields, pollination state, seed type/hue/counts, and resource counts. Version 0 reads `NextGrowth` with `ReadDeltaTime`; version 1 reads `DateTime`. |
| `FertileDirt`, `RedLeaves`, `OrangePetals`, `GreenThorns` | 0 | No custom fields beyond the item stack state handled by base serialization. |

Decorative plants and dead twigs do not serialize a `PlantSystem`, because their status is `DecorativePlant` or higher.

## Known Issues
- `Data/Scripts/Scripts.csproj` lists the four gardening gumps as `Trades\Gardening\EmptyTheBowlGump.cs`, `MainPlantGump.cs`, `ReproductionGump.cs`, and `SetToDecorativeGump.cs`, but the actual source files live under `Data/Scripts/Trades/Gardening/Gumps/`. A command-line project build that trusts the explicit `Compile Include` list may fail to find those files or omit the gump classes.
- `MainPlantGump.OnResponse` directly calls `from.Backpack.FindItemsByType` for the water button. Locked-down or secured plants can be usable through `IsUsableBy` even when `from.Backpack` is null, so that button path lacks the null guard already used by `GetPotion`.
- `PlantSystem.Configure` skips the `WorldSave` growth hook when `Misc.AutoRestart.Enabled` is true. In that mode, plants outside a logging-in player's backpack or bank are only guaranteed to run `GrowAll` on world load.
