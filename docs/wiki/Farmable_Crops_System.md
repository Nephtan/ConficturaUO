# Farmable Crops System

## Scope

This page documents the `FarmableCrop` harvest-item package centered on `Data/Scripts/Items/Farming/`.
It covers static harvestable crop items, their player entry points, crop outputs, one-shot picked state, and reagent crop variants that inherit the same base class.

This package is separate from the bowl-based `Gardening System` and the seed/timer crop loop in the `Homestead System`.
Farmable crops do not register commands, gumps, event hooks, or a planting system.

## Source Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Farming/FarmableCrop.cs` | Shared immovable harvest item base, double-click and walk-over entry points, harvest output placement, spawner unlink helper, and picked-state serialization. |
| `Data/Scripts/Items/Farming/FarmableCabbage.cs` | Cabbage field art and harvested `Cabbage` output. |
| `Data/Scripts/Items/Farming/FarmableCarrot.cs` | Carrot field art and harvested `Carrot` output. |
| `Data/Scripts/Items/Farming/FarmableCorn.cs` | Corn field art and harvested `Corn` output. |
| `Data/Scripts/Items/Farming/FarmableCotton.cs` | Cotton field art and harvested `Cotton` output. |
| `Data/Scripts/Items/Farming/FarmableFlax.cs` | Flax field art and harvested `Flax` output. |
| `Data/Scripts/Items/Farming/FarmableLettuce.cs` | Lettuce field art and harvested `Lettuce` output. |
| `Data/Scripts/Items/Farming/FarmableOnion.cs` | Onion field art and harvested `Onion` output. |
| `Data/Scripts/Items/Farming/FarmablePumpkin.cs` | Standard, large, tall, green, and giant pumpkin field variants. |
| `Data/Scripts/Items/Farming/FarmableTomato.cs` | Tomato field art and harvested `Tomato` output. |
| `Data/Scripts/Items/Farming/FarmableTurnip.cs` | Turnip field art and harvested `Turnip` output. |
| `Data/Scripts/Items/Farming/FarmableWatermelon.cs` | Watermelon field art and harvested `Watermelon` output. |
| `Data/Scripts/Items/Farming/FarmableWheat.cs` | Wheat field art and harvested `WheatSheaf` output. |
| `Data/Scripts/Items/Trades/Resources/Reagents/Farm/FarmableGarlic.cs` | Garlic reagent crop using the shared `FarmableCrop` base. |
| `Data/Scripts/Items/Trades/Resources/Reagents/Farm/FarmableGinseng.cs` | Ginseng reagent crop using the shared `FarmableCrop` base. |
| `Data/Scripts/Items/Trades/Resources/Reagents/Farm/FarmableMandrakeRoot.cs` | Mandrake root reagent crop using the shared `FarmableCrop` base. |
| `Data/Scripts/Items/Trades/Resources/Reagents/Farm/FarmableNightshade.cs` | Nightshade reagent crop using the shared `FarmableCrop` base. |

## Player Entry Points

| Entry point | Behavior |
| --- | --- |
| Double-click a crop | The crop checks that it is a world item, not movable, not locked down, not secure, and not on `Map.Internal`. |
| Range and line of sight | The player must be within 2 tiles and in line of sight, otherwise the crop sends the standard reach failure message. |
| Walk over a crop | Alive `PlayerMobile` instances call the same double-click path through `OnMoveOver`. |
| Picked crop | Once `m_Picked` is true, the crop no longer produces output. There is no player-facing reset path in this package. |

## Harvest Flow

1. The crop changes its `ItemID` to the picked-art ID returned by `GetPickedID()`.
2. The crop creates the output item returned by `GetCropObject()`.
3. Stackable output normally receives amount `1`, except `FarmableWheat`, which uses `MyServerSettings.Resources()` and doubles that amount in the Isles of Dread.
4. The item is placed in the player's backpack when possible.
5. If the backpack cannot hold it, the item is moved to the crop's world location.
6. The base item sets `m_Picked = true` and persists that state.

## Crop Outputs

| Crop item | Harvested output | Notes |
| --- | --- | --- |
| `FarmableCabbage` | `Cabbage` | Food item supplied by the Homestead crop-food package. |
| `FarmableCarrot` | `Carrot` | Food item supplied by the Homestead crop-food package. |
| `FarmableCorn` | `Corn` | Food item supplied by the Homestead crop-food package. |
| `FarmableLettuce` | `Lettuce` | Food item supplied by the Homestead crop-food package. |
| `FarmableOnion` | `Onion` | Food item supplied by the Homestead crop-food package. |
| `FarmablePumpkin` | `Pumpkin` | Standard pumpkin output. |
| `FarmablePumpkinLarge` | `PumpkinLarge` | Large pumpkin output from the core vegetable item file. |
| `FarmablePumpkinTall` | `PumpkinTall` | Tall pumpkin output from the core vegetable item file. |
| `FarmablePumpkinGreen` | `PumpkinGreen` | Green pumpkin output from the core vegetable item file. |
| `FarmablePumpkinGiant` | `PumpkinGiant` | Giant pumpkin output from the core vegetable item file. |
| `FarmableTomato` | `Tomato` | Food item supplied by the Homestead crop-food package. |
| `FarmableTurnip` | `Turnip` | Food item supplied by the Homestead crop-food package. |
| `FarmableWatermelon` | `Watermelon` | Food item supplied by the Homestead crop-food package. |
| `FarmableCotton` | `Cotton` | Tailoring resource; stackable but harvested as one item by the base crop logic. |
| `FarmableFlax` | `Flax` | Tailoring resource with output art forced to item ID `6812`; stackable but harvested as one item by the base crop logic. |
| `FarmableWheat` | `WheatSheaf` | Stack amount follows the shard resource multiplier and Isles of Dread doubling. |
| Reagent farm crops | `Garlic`, `Ginseng`, `MandrakeRoot`, `Nightshade` | Stackable reagent outputs, harvested one at a time by the base crop logic. |

## Relationship To Other Farming Systems

| System | Difference |
| --- | --- |
| `Gardening System` | Uses plant bowls, seeds, plant care, growth timers, cross-pollination, and plant resources. |
| `Homestead System` | Uses player-planted seeds, growth timers, row crops, fruit trees, Cooking-based harvest rolls, and craft menus. |
| `Farmable Crops System` | Uses already-placed world items that can be harvested once by double-clicking or walking over them. |

## Serialization Notes

| Type | Serialized data |
| --- | --- |
| `FarmableCrop` | Encoded version `0`, then `m_Picked`. |
| Individual food, textile, and reagent crop subclasses | Encoded version `0`; no custom fields beyond the base picked state. |

The base class has an `Unlink()` helper that removes a crop from its spawner, but normal harvest and deserialization paths do not call it.

## Known Issues

| Issue | Impact |
| --- | --- |
| Picked crops remain in the world with `m_Picked = true`, and the available `Unlink()`/delete cleanup is not called after harvest or load. | Spawner-backed crop fields can retain permanently picked items and may continue occupying spawner slots. |
| Stackable output amount logic gives the shard resource multiplier only to `FarmableWheat`; cotton, flax, and reagent crops are forced back to amount `1`. | Resource-rate settings and Isles of Dread doubling do not affect most stackable farmable outputs. |
| `OnPicked()` reads `spawn.Stackable` before checking whether `GetCropObject()` returned a non-null item. | A future custom crop subclass that returns null could throw before the null guard. |

## Admin Notes

Use this package for simple static harvest nodes rather than player-run agriculture.
For seed planting, crop growth, harvest skills, and player-owned field behavior, use the Homestead crop package.
For plant bowls, plant care, pollination, and rare plant resources, use the Gardening package.
