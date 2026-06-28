# Vendor Core System

## Scope

This page documents the shard-facing RunUO vendor framework centered on `Server.Mobiles.BaseVendor`.
It covers NPC vendor catalog loading, client buy/sell packet flow, restocking, price math, direct drag/drop vendor services, bulk order turn-ins, and persistence behavior.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Base/BaseVendor.cs` | Main `BaseVendor` implementation, context menus, buy/sell logic, restock, drag/drop services, and serialization. |
| `Data/Scripts/Mobiles/Base/GenericBuy.cs` | `GenericBuyInfo` stock entries, display entity cache, purchase entity creation, and stock growth rules. |
| `Data/Scripts/Mobiles/Base/GenericSell.cs` | `GenericSellInfo` resale price formula and sellable type lookup. |
| `Data/Scripts/Mobiles/Base/AnimalBuy.cs` | `AnimalBuyInfo`, adding follower slot cost to buy entries that create Mobiles. |
| `Data/Scripts/Mobiles/Base/BeverageBuy.cs` | `BeverageBuyInfo`, creating beverage Items with a configured `BeverageType`. |
| `Data/Scripts/Mobiles/Base/PresetMapBuy.cs` | `PresetMapBuyInfo`, creating `PresetMap` Items from a `PresetMapEntry`. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | Large catalog of `SBInfo` buy/sell lists used by vendor subclasses. |
| `Data/System/Source/BaseVendor.cs` | Engine-side buy/sell state DTOs: `BuyItemResponse`, `SellItemResponse`, `BuyItemState`, `SellItemState`. |
| `Data/System/Source/Network/PacketHandlers.cs` | Client buy/sell reply packet handlers that dispatch to `IVendor.OnBuyItems` and `IVendor.OnSellItems`. |
| `Data/System/Source/Network/Packets.cs` | Vendor buy/sell packet serialization sent back to clients. |
| `Data/Scripts/System/Commands/Commands/Commands.cs` | GameMaster `Restock` generic command for targeted vendor Mobiles. |

## Vendor Model

`BaseVendor` is an abstract `BaseCreature` implementing `IVendor`. It is player range sensitive, can teach by default, exposes `IsActiveVendor`, `IsActiveBuyer`, `IsActiveSeller`, and `NpcGuild`, and requires subclasses to provide `SBInfos` plus `InitSBInfo()`.

Every vendor constructor calls `LoadSBInfo()`, initializes body/outfit, creates hidden `Layer.ShopBuy` and `Layer.ShopResale` backpacks, and sets `LastRestock` to the current time. Those hidden packs are required by the client vendor packet flow.

`LoadSBInfo()` clears the subclass `SBInfos` list, calls `InitSBInfo()`, then flattens every `SBInfo.BuyInfo` into `m_ArmorBuyInfo` and every `SBInfo.SellInfo` into `m_ArmorSellInfo`. The `SBInfo` abstraction itself is only two catalog properties:

| Property | Meaning |
| --- | --- |
| `List<GenericBuyInfo> BuyInfo` | Items or Mobiles the vendor sells to players. |
| `IShopSellInfo SellInfo` | Item types and base prices the vendor buys from players. |

Many entries in `StoreSalesList.cs` use `MyServerSettings.SellChance()`, `SellRareChance()`, `BuyChance()`, and `SoldResource()` at construction time, so catalog contents can randomize whenever `LoadSBInfo()` runs.

## Access Rules

The common access gate is `CheckVendorAccess(Mobile from)`.

| Check | Result |
| --- | --- |
| `from.Blessed` | Vendor refuses interaction. |
| `IntelligentAction.GetMyEnemies(from, this, false)` | Vendor refuses interaction. |
| Vendor is `BaseGuildmaster` and vendor `NpcGuild` does not match `PlayerMobile.NpcGuild` | Vendor refuses interaction. |
| Otherwise | Vendor permits interaction. |

Context menu entries are added when the caller is alive and the vendor is active:

| Context Menu Entry | Condition | Action |
| --- | --- | --- |
| Bulk order info | `SupportsBulkOrders(from)` | Shows whether a bulk order is available and may open an accept gump. |
| Buy | `IsActiveSeller` | Calls `VendorBuy(from)`. |
| Sell | `IsActiveBuyer` | Calls `VendorSell(from)`. |
| Setup Shoppe | Skill/vendor-type gates at 50.0 skill | Opens `ExplainShopped` if `CheckVendorAccess(from)` permits it. |

## Restocking

| Setting | Value |
| --- | --- |
| `RestockDelay` | 2 hours |
| `RestockDelayFull` | 15 minutes |
| Resold inventory decay | 2 hours since `Item.LastMoved` in `BuyPack` |

`VendorBuy()` and `VendorSell()` call `Restock()` when the normal delay expires, when the caller is in a `PublicRegion` and the full delay expires, or when the vendor is a `BaseGuildmaster` and the normal delay expires.

`Restock()` sets `LastRestock` to now, calls `LoadSBInfo()` again, deletes all gold in the vendor backpack, and repacks 30 to 120 gold scaled by `MyServerSettings.GetGoldCutRate()`. `GetGoldCutRate()` is clamped to 5 through 100, so default vendor pocket gold is effectively 25 percent of that range unless settings change it.

`GenericBuyInfo.OnRestock()` has stock growth logic:

| Current Amount | Max Amount Change | Final Amount |
| --- | --- | --- |
| `Amount <= 0` | `MaxAmount *= 2`, capped at 999 | Set to `MaxAmount` |
| `Amount > 0` and `Amount >= halfQuantity` | `MaxAmount = halfQuantity`; `halfQuantity` is 640 when max is at least 999, half of max when max is above 20, otherwise unchanged | Set to `MaxAmount` |
| Other positive amount | No max change | Set to `MaxAmount` |

Because `BaseVendor.Restock()` calls `LoadSBInfo()` instead of calling `GenericBuyInfo.OnRestock()` over existing entries, the actual Confictura runtime refreshes catalogs by reconstructing their `SBInfo` lists.

## Buying From Vendors

`VendorBuy(Mobile from)` is the server-side opener for the buy window.

1. It requires `IsActiveSeller`, `from.CheckAlive()`, and `CheckVendorAccess(from)`.
2. It restocks if the restock conditions are met.
3. It applies faction town tax by calling `UpdateBuyInfo()`.
4. It builds `BuyItemState` rows from `IBuyItemInfo` entries with positive stock.
5. It removes expired resale Items from `BuyPack`.
6. It appends resold Items from `BuyPack` when the item has sell info and the buy list has room.
7. It sends shop packs, buy-content packets, the buy-list packet, display packet, mobile status, and item/mobile property lists.

Buy row limits are hard-coded:

| Vendor Type | New-stock row limit |
| --- | --- |
| `Sage` | 500 |
| All others | 250 |

The client reply enters `PacketHandlers.VendorBuyReply`, which caps the packet payload at 100 requested rows, converts each row into `BuyItemResponse`, and dispatches to `IVendor.OnBuyItems(state.Mobile, buyList)`.

`OnBuyItems()` behavior:

| Step | Behavior |
| --- | --- |
| Validate | Requires active seller, alive buyer, vendor access, and at least one valid requested row. |
| Stock lookup | Display Item/Mobile serials are mapped back to `GenericBuyInfo` through `LookupDisplayObject()`. Resold Items are accepted only when `IShopSellInfo.IsResellable(item)` is true. |
| Follower slots | `AnimalBuyInfo.ControlSlots * amount` is subtracted from available follower slots before the purchase is accepted. |
| Cost | `totalCost += bii.Price * amount` for catalog stock, or `GetBuyPriceFor(item) * amount` for resold Items. |
| Payment | GameMasters buy free. Otherwise backpack gold is tried first; purchases of 2000 gold or more can fall back to bank gold. |
| Delivery | Stackable Items receive the purchased amount. Non-stackable Items are created one by one. Mobiles move to the buyer location, play idle sound, and `BaseCreature` purchases are assigned `ControlMaster`, `Tamable = true`, and `MinTameSkill = 29.1`. |
| Failure feedback | Empty orders, unaffordable orders, bank failures, artifact-display purchases, and partial follower-slot failures each get separate vendor speech. |

## Buying Price Formula

`BaseVendor.GetPriceScalar()` returns `100 + town.Tax` for faction towns and `100` outside faction towns. `UpdateBuyInfo()` assigns that scalar to each `IBuyItemInfo`.

`GenericBuyInfo.Price` returns:

| Condition | Formula |
| --- | --- |
| `PriceScalar != 0` and base price > 5,000,000 | `min(int.MaxValue, ((long)basePrice * PriceScalar + 50) / 100)` |
| `PriceScalar != 0` and base price <= 5,000,000 | `(basePrice * PriceScalar + 50) / 100` |
| `PriceScalar == 0` | `basePrice` |

## Selling To Vendors

`VendorSell(Mobile from)` opens the sell window.

1. If the caller is in a begging pose, the Mobile says one random begging phrase.
2. It requires `IsActiveBuyer`, `from.CheckAlive()`, and `CheckVendorAccess(from)`.
3. It restocks if needed.
4. It scans the caller's backpack for Items matching each `IShopSellInfo.Types` entry.
5. It skips non-empty containers and Items inside locked `LockableContainer` parents.
6. It accepts only standard-loot, movable, sellable Items.
7. It computes the visible sell price using Mercantile, guild membership, or Begging.
8. It sends `VendorSellList` when at least one sellable Item is found.

The client reply enters `PacketHandlers.VendorSellReply`, which checks the vendor is still within 10 tiles, caps rows below 100, converts them to `SellItemResponse`, and dispatches to `IVendor.OnSellItems(state.Mobile, sellList)`.

`OnSellItems()` behavior:

| Step | Behavior |
| --- | --- |
| Validate | Requires active buyer, alive seller, vendor access, positive amount, seller-owned root parent, standard loot, movable Item, and no non-empty containers. |
| Batch cap | At most 500 sellable Items per sale. |
| Resell path | If `IShopSellInfo.IsResellable(item)` is true, matching `IBuyItemInfo.Restock(item, amount)` can consume the Item into stock; otherwise the Item or split stack is dropped into `BuyPack`. |
| Non-resell path | The sold amount is removed or the Item is deleted. |
| Gold payout | Gold is added to backpack in chunks of 60,000 plus a final remainder. |
| Bulk order chance | If the vendor supports bulk orders, a `LargeBODAcceptGump` or `SmallBODAcceptGump` can be sent after a successful sale. |

## Selling Price Formula

`GenericSellInfo.GetSellPriceFor(Item item, int barter)` starts from the catalog price for the exact Item type, then applies these transformations:

| Item Class | Adjustments |
| --- | --- |
| `BaseArmor` | Low quality x0.60, exceptional x1.25, resource multiplier, `+ 100 * Durability`, `+ 100 * ProtectionLevel`, minimum 1 before global half-price step. |
| `BaseWeapon` | Low quality x0.60, exceptional x1.25, metal resource multiplier, `+ 100 * DurabilityLevel`, `+ 100 * DamageLevel`, minimum 1 before global half-price step. |
| `BaseBeverage` | Empty or milk uses container empty/base price; filled non-milk uses filled price. Pitcher uses 3 or 5, bottle 3 or 3, jug 6 or 6. |
| All Items | Final base is halved with integer division. If barter is positive, barter is clamped to 100 and final price is multiplied by `1 + barter * 0.03`. Final minimum is 1. |

`GetBuyPriceFor(Item item)` returns `1.90 * GetSellPriceFor(item, 0)` truncated to `int`.

Barter source:

| Condition | Barter Value |
| --- | --- |
| Normal sale | Seller's `Mercantile` skill value cast to `int`. |
| Vendor has `NpcGuild` and seller belongs to the same guild, with Mercantile below 100 | 100. |
| Seller is begging and is not receiving guild-member pricing | Seller's `Begging` skill value cast to `int`; may also award negative karma through `BeggingKarma()`. |

## Direct Drag/Drop Services

`BaseVendor.OnDragDrop()` adds many non-shop interactions before falling back to `base.OnDragDrop()`.

| Dropped Item | Required Vendor | Outcome |
| --- | --- | --- |
| `BaseQuiver` | `Bowyer` or `ArcherGuildmaster` | Toggles quiver `ItemID` between `0x5770` and a random `0x2B02/0x2B03`; does not consume the Item. |
| `DrakkhenEggRed`, `DrakkhenEggBlack` | `Druid`, `DruidTree`, or `DruidGuildmaster` | Delegates to the egg processor. |
| `GolemManual`, `OrbOfTheAbyss`, `RobotSchematics` | `Tinker` or `TinkerGuildmaster` | Delegates to the item-specific processor. |
| `AlienEgg`, `DragonEgg` | `AnimalTrainer` or `Veterinarian` | Delegates to the egg processor. |
| `DracolichSkull` | `NecromancerGuildmaster` | Delegates to the skull processor. |
| `DemonPrison` | Necromancer or mage-family vendors listed in code | Delegates to the prison processor. |
| Any Item with positive `RelicItems.RelicValue(dropped, vendor)` | Any matching relic vendor | Pays `RelicValue + skill bonus`, then deletes the Item. Guild members add another full `RelicValue`; beggars use Begging instead of Mercantile when not receiving guild pricing. |
| `Cargo` | Vendor matching `Cargo.CargoVendor` through `Cargo.VendorTest()` | Delegates cargo payout, karma, fame, and deletion to `Cargo.GiveCargo()`. |
| `Museums` | `VarietyDealer` | Pays `Museums.AntiqueTotalValue(...)`, deletes the antique, and speaks through `Museums.GiveAntique()`. |
| Rolled magic carpet or magic carpet deed | `Tailor` or `TailorGuildmaster` | Cycles carpet deed variant A through I, copies hue, deletes the original, and gives the new deed. |
| Exactly 1000 `Gold` | `Mapmaker` or `CartographersGuildmaster` | Deletes gold and gives a world map selected from current map and coordinate checks. |
| `DugUpCoal` | `Blacksmith` in the Savaged Empire | Requires matching ore through `DugUpCoal.CheckForDugUpCoal`; gives `SteelIngot(dropped.Amount)` and deletes coal on success. |
| `DugUpZinc` | `Blacksmith` in the Island of Umber Veil | Requires copper ore through `DugUpZinc.CheckForDugUpZinc`; gives `BrassIngot(dropped.Amount)` and deletes zinc on success. |
| `DDCopper`, `DDSilver` | `Minter` or `Banker` | Exchanges copper at 10:1 gold or silver at 5:1 gold, returning remainder coins. |
| `DDXormite`, `Crystals`, `DDJewels`, `DDGemstones`, `DDGoldNuggets` | `Minter` or `Banker` | Pays 3, 5, 2, 2, or 1 gold per dropped unit respectively, then deletes the stack. |
| Tomb-raiding artifact list | `ThiefGuildmaster` and player in `ThievesGuild` | Pays 5,000 through 12,000 gold by artifact type, awards fame equal to payout, awards negative karma equal to payout, and deletes the artifact. |
| `StealBox`, `StealMetalBox`, `StealBag` | `Thief` and seller has at least 10 Stealing | Pays `500 + skill bonus` and deletes the Item. |
| `StolenChest` | `Thief` and seller has at least 10 Stealing | Pays `ContainerValue + skill bonus` and deletes the chest. |
| Live henchman token Items | `InnKeeper`, `TavernKeeper`, or `Barkeeper` | Replaces live fighter, archer, or wizard henchman token with a fresh token preserving timer and bandages; refuses dead henchmen. |
| `BookBox`, `CurseItem` | Mage, chivalry, witch, necromancer, or holy vendor list in code | Moves contained Items to player backpack, deletes wrapper, and reports curse removal. |
| `SewageItem`, `SlimeItem` | Inn/tavern/bar service vendors | Moves contained Items to player backpack, deletes wrapper, and reports cleaning. |
| `WeededItem` | `Alchemist` or `Herbalist` | Moves contained Items to player backpack, deletes wrapper, and reports weed removal. |
| `SmallBOD`, `LargeBOD` | Vendor whose override accepts the BOD | Validates completion and vendor type, gives BOD reward item, bank check or gold, fame, calls `OnSuccessfulBulkOrderReceive()`, sets ML turn-in delay, and deletes the deed. |

## Admin Command

| Command | Access | Target | Behavior |
| --- | --- | --- | --- |
| `Restock` | GameMaster | Any NPC target; succeeds only for `BaseVendor` | Logs command use, calls `BaseVendor.Restock()`, and reports whether the target was a vendor. |

## Serialization

`BaseVendor.Serialize()` writes version `1`, then persists only depleted stock counts. For each `GenericBuyInfo` where `MaxAmount - Amount` is positive, it writes an encoded flattened catalog index and the depletion value. It terminates with encoded int `0`.

`BaseVendor.Deserialize()` reads the version, calls `LoadSBInfo()`, then for version `1` reads encoded stock depletion pairs until the sentinel `0`. It maps each flattened index back to `SBInfo` and `BuyInfo` indexes, clamps restored `Amount` to zero if needed, clears `IsParagon`, and fixes legacy AOS invulnerable name hue.

No resale pack Items, direct drag/drop service state, or `LastRestock` value are serialized by this class.

## Known Issues

| Issue | Evidence |
| --- | --- |
| Sage vendors can build 500 buy rows, but `VendorBuyList` writes `list.Count` as a single byte. The old warning about lists above 255 is commented out, so a large Sage list can truncate or corrupt the client buy-list count. | `VendorBuy()` sets `sales = 500` for `Sage`; `VendorBuyList` serializes `(byte)list.Count`. |
| Several paths cast `Mobile` directly to `PlayerMobile` without a type guard. This affects `VendorSell()`, `OnDragDrop()`, `CheckVendorAccess()`, `OnSellItems()`, and ML BOD turn-in checks. Non-player Mobile callers can throw `InvalidCastException`. | Direct casts such as `PlayerMobile pm = (PlayerMobile)from` and `((PlayerMobile)from).NextBODTurnInTime`. |
| `OnThink()` iterates `this.GetItemsInRange(range)` without freeing the returned `IPooledEnumerable`. | Core `Mobile.GetItemsInRange()` returns `IPooledEnumerable`, and `IPooledEnumerable` exposes `Free()`. |
| Catalog persistence is index-based while catalogs can be randomized during `LoadSBInfo()`. Saved stock depletion can apply to different entries, disappear, or become fragile if `SBInfos` order/count changes between saves. | `StoreSalesList.cs` uses settings-driven random chances in catalog constructors; `Serialize()` stores flattened indexes instead of stable item identifiers. |
| Cargo and museum drag/drop branches delegate to handlers that can delete the dropped Item but do not return afterward. Control falls through toward BOD checks and `base.OnDragDrop(from, dropped)`, which is fragile for deleted Items. | `Cargo.GiveCargo()` and `Museums.GiveAntique()` delete accepted Items; `BaseVendor.OnDragDrop()` does not return immediately after those delegate calls. |
