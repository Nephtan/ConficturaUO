# Homestead System

## Overview
The Homestead package is a bundled agriculture-and-food collection, not a single mechanic. The compiled folder includes crop planting and harvesting, fruit trees, cooking craft menus, brewing, juicing, winecrafting, a community beehive addon, and a milk-and-cheese subsystem.

## Crop Planting And Growth
- Seeds must be in the player's backpack to be used.
- Planting is blocked while mounted because `CropHelper.CanWorkMounted` is `false`.
- The planting check uses the player's current tile, not a targeted tile.
- `CropHelper.CheckCanGrow` accepts crop placement on configured farm, house, dirt, ground, sand, swamp, or garden-plot tiles depending on each seed's overrides.
- Many homestead seeds, including `WheatSeed` and `AppleTreeSeed`, explicitly allow deeded garden dirt (`ItemID 0x32C9`) through `CanGrowGarden`.
- The system rejects planting on an occupied tile and also rejects overcrowding.
- Standard field crops use a tighter crowding rule than trees. `WheatSeed` fails if more than 3 nearby crops are found, or sometimes when exactly 3 are nearby. `AppleTreeSeed` uses a 5-neighbor threshold instead.
- Seedlings do not mature immediately. `CropHelper.GrowTimer` waits 1 hour, then ticks every 36 seconds until its counter passes the random threshold.
- When that timer resolves, the seedling either becomes the intended crop or becomes `Weeds`. The success roll uses the original sower's Cooking skill divided by 50.

## Row Crops
- Typical row-crop classes follow the `WheatCrop` pattern: capacity 8, start empty, then regrow after a 10-minute delay with 15-second refill ticks until full.
- Harvesting is Cooking-based, using `Cooking / 20` as the base roll.
- The original sower gets double effective harvest value and refreshes the crop's last-visit timestamp on harvest.
- Harvests can fail even on a mature plant if the random pick roll returns 0.
- Empty crops show an uproot confirmation gump instead of auto-deleting.
- Row crops decay only when the sower has failed to revisit within `SowerPickTime`, which defaults to 7 days.

## Fruit Trees
- Fruit trees are separate from row crops and use `TreeHelper`.
- Saplings grow into trees after 5 minutes by default.
- Chopped trees regrow from stumps after 30 minutes by default.
- Fruit picking is effectively allowed while mounted because `TreeHelper.CanPickMounted` is `true`.
- Tree harvesting is not Cooking-based. `AppleTree` uses Lumberjacking skill divided by 20.
- Fruit trees have much larger capacity than row crops. `AppleTree` refills toward a capacity of 25 fruit.

## Cooking Menus
- `BakersBoard` opens `DefBaking`.
- `CooksCauldron` opens `DefBoiling`.
- `FryingPan` opens `DefGrilling`.
- All three craft systems use the Cooking skill.

## Brewing
- `BrewersTools` opens `DefBrewing`, which also uses Cooking.
- Brewing crafts keg outputs, not bottled drinks: `MeadKeg`, `AleKeg`, and `CiderKeg`.
- Mead and ale require 50 hops, a keg, water, and either malt or barley.
- Cider uses a keg, 100 apples, water, and sugar.
- The brewing menu lets the crafter choose among multiple hop subresources.

## Juicing
- `JuicersTools` opens `DefJuicing`, which uses Cooking.
- Juicing crafts a `JuiceKeg`, not a direct bottle item.
- The base recipe is 25 fruit, 5 water, 1 keg, and 1 bag of sugar.
- The menu supports many fruit subresources.
- `JuiceKeg` starts with a 1-second bottling delay and a default fill of 75 servings.
- Crafted juice can also inherit custom farm naming through `FarmLabelMaker`.

## Winecrafting
- `WinecraftersTools` opens `DefWinecrafting`, which uses Alchemy rather than Cooking.
- Winecrafting produces `WineKeg`, not ready-to-drink bottles.
- Each wine keg needs 50 grapes, 1 keg, 1 sugar, and 1 yeast, with the grape variety chosen as a subresource.
- `WineKeg` defaults to a 7-day bottling delay before `AllowBottling` becomes true.
- Staff can open the vine-placement gump with `[addgv]`.
- Players can also open the same gump with `GrapevinePlacementTool`.
- Player grapevine placement is rule-checked: by default it must be inside a house the player owns, on allowed dirt/house/vineyard tiles, and it costs 250 gold per vine.
- Harvesting grapevines uses Cooking-based picking and respects ownership through the stored `Placer`.

## Bundled Side Systems
- `CommunityBeeHiveAddon` places a beehive addon that generates honey and wax on a randomized timer.
- New hives default to 7-11 minute random production intervals, up to 10 honey and 10 wax unless staff properties are changed.
- Double-clicking the hive attempts to move all stored wax and honey into the user's backpack at once.
- `MilkBucket` supports milking sheep, goats, and cows.
- Each successful milking adds 1 liter, reduces the animal's stamina by 3, and locks the bucket to that milk type until emptied.
- `CheeseForm` consumes 15 liters from a milk bucket, starts a fermentation timer, and later yields normal cheese, magic cheese, or failure based on a random quality roll plus Cooking skill.

## Known Code Caveat
- The crop uproot flow is incomplete. Many crop classes open `UpRootGump`, but `UpRootGump.OnResponse` only dispatches nine specific crop classes, so confirming the gump will do nothing for many other crops.

## Audience
Players using farming, beverage, and food-production content, plus staff configuring planted content, grapevines, and bundled production addons.
