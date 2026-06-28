# Bulk Order System

## Overview
The bulk order system lives under `Server.Engines.BulkOrders` and is built around `SmallBOD`, `LargeBOD`, the smith/tailor deed subclasses, the reward calculators, and `BulkOrderBook`. On this shard, blacksmithing and tailoring vendors hand out blessed deed items, players fill them through deed-specific combine targeting, and completed deeds are turned back in to the matching vendor for gold, fame, and one weighted reward item.

## Core Scripts
- `SmallBOD.cs`: shared small-deed item, combine validation, gump entry point, and version `0` serialization.
- `LargeBOD.cs`: shared large-deed item, small-deed combine logic, and version `0` serialization.
- `SmallSmithBOD.cs` / `SmallTailorBOD.cs`: random small-deed generation plus smith/tailor reward calculation hooks.
- `LargeSmithBOD.cs` / `LargeTailorBOD.cs`: random large-deed generation plus smith/tailor reward calculation hooks.
- `Rewards.cs`: point, fame, gold, and weighted reward tables.
- `Books/BulkOrderBook.cs`: 500-deed storage book with filters, secure level, and version `2` serialization.
- `Mobiles/Base/BaseVendor.cs` plus the smith/tailor vendor classes: deed request and turn-in flow.

## Getting Deeds
### Eligible vendors
- Smith BODs come from blacksmith-family vendors that override `CreateBulkOrder`, including `Blacksmith`, `Weaponsmith`, and `Garth`.
- Tailor BODs come from tailoring vendors including `Tailor` and `Weaver`.
- Vendors only support BODs for players with the matching skill above `0.0`.

### Request flow
- Opening the vendor context-menu entry checks `GetNextBulkOrder`. If the timer is zero, the vendor says an order is available now.
- Context-menu requests always attempt to create a deed immediately.
- Selling items to a matching vendor can also trigger a BOD offer, but only on a `20%` roll because `CreateBulkOrder(..., false)` requires `0.2 > Utility.RandomDouble()`.

### Cooldown behavior actually in code
- The current shard code sets both smith and tailor cooldowns to `TimeSpan.FromMinutes(0.01)` for every skill bracket.
- That is about `0.6` seconds, not an hourly OSI-style wait.
- On `Core.SE`, successfully turning in a deed resets the request timer back to zero through `OnSuccessfulBulkOrderReceive`.
- On `Core.ML`, vendors also enforce a separate `NextBODTurnInTime` throttle of `10` seconds between completed deed turn-ins.

## What Vendors Generate
### Small smith deeds
- `SmallSmithBOD.CreateRandomFor` picks either the armor list or weapon list.
- Quantity is weighted by skill:
  - below `50.1`: `10, 10, 15, 20`
  - `50.1` to `70.0`: `10, 15, 15, 20`
  - `70.1+`: `10, 15, 20, 20`
- Material requirements only roll when the armor path was chosen and the crafter has at least `70.1` blacksmith.
- Exceptional requirement chance at `70.1+` is `(skill + 80.0) / 200.0`.
- The final entry list is filtered so the player only receives items they can actually craft, and exceptionally-only deeds also require a positive exceptional success chance.

### Small tailor deeds
- `SmallTailorBOD.CreateRandomFor` picks cloth or leather entries.
- Leather deeds only become eligible once the player has at least `6.2` tailoring.
- Quantity and exceptional chance use the same formulas as smith deeds.
- Material requirements only roll at `70.1+` tailoring and only for leather-style entries.
- The final item pool is also filtered against real crafting success and exceptional chance.

### Large deeds
- At `70.1+` skill, a vendor may issue a large deed instead of a small one.
- The chance is `((skill - 40.0) / 300.0)`, so `70.1` skill is just over `10%` and `100.0` skill is `20%`.
- Large smith deeds randomly choose one of eight set families: ring, plate, chain, axes, fencing, maces, polearms, or swords.
- Large tailor deeds randomly choose one of fourteen set families such as farmer, pirate, wizard, hat set, leather sets, studded set, bone set, and others.
- Accepting a large deed also places one starter small deed for each entry into the backpack. Those small deeds already match the large deed's item type, quantity, material, and exceptional requirement.

## Filling Deeds
### Small deeds
- Players must double-click the deed while it is in their backpack.
- If `MyServerSettings.AllowMacroResources()` returns `true`, the gump opens directly.
- If it returns `false`, the gump is delayed behind `CaptchaGump`.
- `Combine` targets one item in the backpack at a time.
- Accepted items must:
  - match the requested type or subclass
  - be a `BaseWeapon`, `BaseArmor`, or `BaseClothing`
  - match the required ore or leather material when one is specified
  - be exceptional when required
- Every accepted item is deleted and increments `AmountCur` by `1`.

### Large deeds
- Large deeds also must be in the backpack to open or combine.
- `Combine` targets a completed `SmallBOD`, not crafted items directly.
- The targeted small deed must match:
  - one of the large deed entry types
  - the same exceptional requirement if the large deed requires exceptional quality
  - the same ore or leather material when one is specified
  - the same quantity (`10`, `15`, or `20`)
  - full completion (`AmountCur == AmountMax`)
- A successful combine deletes the small deed and adds its full completed amount into the matching large entry.

## Turn-In Rewards
Completed small and large deeds are turned in by dragging them onto the matching vendor.

Vendor turn-in checks:
- wrong profession deed: rejected
- incomplete deed: rejected
- ML turn-in throttle active: rejected

Successful turn-in does this:
1. Calls `GetRewards(out reward, out gold, out fame)` on the deed.
2. Adds one weighted reward item to the backpack when the reward table returned one.
3. Pays gold as loose gold up to `1000`, otherwise as a bank check.
4. Awards fame with `Titles.AwardFame`.
5. Deletes the turned-in deed.

### Reward math
- Fame is derived from reward points using `(points / 50)^2`.
- Gold uses profession-specific lookup tables in `Rewards.cs`, then applies a random range from `90%` to `111%` of the table value.
- The final gold payout is multiplied by `MyServerSettings.GetGoldCutRate() * 0.01`.

### Smith reward point rules
- quantity: `10 -> +10`, `15 -> +25`, `20 -> +50`
- exceptional: `+200`
- large-deed item family bonus: `+200` ring or bardiche/halberd, `+300` chain or fencing set, `+350` axes/swords/maces, `+400` plate
- material: `+200` for dull copper, then `+50` more per tier up to valorite

### Tailor reward point rules
- quantity: `10 -> +10`, `15 -> +25`, `20 -> +50`
- exceptional: `+100`
- large-deed size bonus: `+300` for 4-part sets, `+400` for 5-part sets, `+500` for 6-part sets
- material: `+50` spined, `+100` horned, `+150` barbed

### Reward tables actually used
- Smith rewards progress from sturdy tools and mining gloves up through gargoyle/prospector tools, powder of temperament, runic hammers, blacksmith power scrolls, colored anvils, and ancient smithy hammers.
- Tailor rewards progress from colored cloth and sandals up through stretched hides, runic sewing kits, tapestries, bear rugs, tailoring power scrolls, and clothing bless deeds.
- `GetRewards` chooses one item from the highest reward group whose point threshold is met, using the weight values in `RewardGroup`.

## Bulk Order Books
- `BulkOrderBook` is a blessed item with item ID `0x2259`.
- It can store up to `500` deeds.
- The book must be in the player's backpack to accept dragged-in deeds.
- Default secure level is `CoOwners`.
- The custom book name is optional and is trimmed to `40` characters.
- The internal `ItemCount` only increases by `1` per five stored deeds, which is what the container total system sees.
- Book serialization is version `2`; it stores item-count metadata, secure level, book name, filter, and serialized deed entries.

## Data Files And Persistence
- Small and large entry definitions are loaded from `Data/System/Bulk Orders/<Profession>/*.cfg`.
- `SmallBulkEntry.LoadEntries` parses each non-comment line into an item type plus graphic and converts that to a localized cliloc number.
- Serialization versions:
  - `SmallBOD`: `0`
  - `LargeBOD`: `0`
  - `SmallSmithBOD`, `SmallTailorBOD`, `LargeSmithBOD`, `LargeTailorBOD`: subclass version `0` layered on top of the base deed serializer
  - `BulkOrderBook`: `2`

## Code-Verified Notes
- Wiki claim: vendors simply hand out small or large deeds -> traced `LargeBODAcceptGump.OnResponse` -> accepted large deeds also inject matching starter small deeds for every entry.
- Wiki claim: players just obtain a deed from a blacksmith or tailor -> traced `BaseVendor.CreateBulkOrder` overrides -> context-menu requests are guaranteed when the cooldown is zero, but sale-triggered offers only happen on a `20%` roll.
- Wiki claim: `AllowMacroResources` controls whether a CAPTCHA is shown -> traced `SmallBOD.OnDoubleClick` and `LargeBOD.OnDoubleClick` -> CAPTCHA is only used when `AllowMacroResources()` returns `false`.
- Wiki claim: rewards are chosen from tables based on quantity, material, and exceptional status -> traced `Rewards.cs` -> point totals also depend on large-deed family or entry-count bonuses, fame is `(points / 50)^2`, and gold is further randomized and scaled by the shard gold-cut setting.

## Audience
Players and staff
