# Shoppes Vendors

## Overview

The Shoppes Vendors system is a player-owned work-order system built around `BaseShoppe` Items. A qualified player uses a matching trade vendor's context menu to buy or recover one work shoppe, locks it down in a house, stocks it with tools and resources, and fulfills generated customer contracts through `ShoppeGump`.

The same folder also contains `MerchantCrate`, a house-locked sale container that converts crafted or listed items into stored gold after a timer.

The traced system does not register custom `[Command]` handlers, packet handlers, `EventSink` hooks, or XMLSpawner attachments. Normal administration uses standard RunUO construction and property tools such as `[add AlchemistShoppe]`, `[add BaseShoppe]`, or `[add MerchantCrate]`.

Code-Verified: 2026-05-08

## Core Scripts

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Trades/Shoppes/BaseShoppe.cs` | `Server.Items` | Shared `Item`, secure access, purchase flow, stocking rules, customer timers, serialization, and `ShoppeGump`. |
| `Data/Scripts/Trades/Shoppes/Customers.cs` | `Server.Misc` | Customer-slot generation, reward formulas, success chance, contract completion, refusal, and packed customer string parsing. |
| `Data/Scripts/Trades/Shoppes/*Shoppe.cs` | `Server.Items` | Eleven concrete work shoppe subclasses with title, skill, guild, tool/resource labels, sound, and task text generator. |
| `Data/Scripts/Trades/Shoppes/MerchantCrate.cs` | `Server.Items` | Sale container, crate cash-out, pickup timers, and item price table. |
| `Data/Scripts/Mobiles/Base/BaseVendor.cs` | `Server.ContextMenus` | Adds `SetupShoppeEntry` when a player has the matching skill and is using a matching vendor class. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | `Server.Mobiles` | Sells `MerchantCrate` and the `WorkShoppes` book from vendor inventories. |

`Data/Scripts/Scripts.csproj` explicitly compiles all Shoppes scripts.

## Vendor Setup Flow

`BaseVendor.AddCustomContextEntries` adds `SetupShoppeEntry` when the caller has at least `50.0` in the matching trade skill and the vendor class matches that trade. The entry is enabled by `vendor.CheckVendorAccess(from)`.

Selecting the context entry opens `ExplainShopped`. Pressing the confirmation button calls `BaseShoppe.GiveNewShoppe(m_From, m_Merchant)`.

The compiled purchase flow is:

1. Pick the concrete shoppe type from the merchant `Mobile` class.
2. Start with a `10,000` gold fee.
3. If `BaseVendor.BeggingPose(from) > 0`, reduce the fee by `min((int)(Begging * 25), 3000)`.
4. Reject players with `from.Kills > 0`.
5. If the player already owns the same concrete shoppe type anywhere in `World.Items`, copy the old shoppe state into the new item, add the new item to the backpack, delete the old one, and do not charge a fee.
6. Otherwise, when `from.TotalGold >= fee` and a concrete shoppe was created, consume `fee` gold from `from.Backpack`, set `ShoppeOwner` and `ShoppeName`, add the shoppe to the backpack, and immediately run a customer cycle.

## Shoppe Types

The `ShelfSkill` ID is resolved by `BaseShoppe.GetSkillValue(int job, Mobile from)`. The two combined IDs are special cases: `55` averages Druidism and Veterinary, while `56` averages Forensics and Necromancy.

| Shoppe class | Vendor gate | Purchase skill gate | Contract skill | Guild bonus | Stocked tools | Stocked resources |
| --- | --- | --- | --- | --- | --- | --- |
| `AlchemistShoppe` | `Alchemist`, `AlchemistGuildmaster` | `Alchemy >= 50.0` | Alchemy (`ShelfSkill = 1`) | `AlchemistsGuild` | `MortarPestle`, `GodBrewing` | Any reagent recognized by `MaterialInfo.IsReagent`. |
| `BakerShoppe` | `Cook`, `Baker`, `CulinaryGuildmaster` | `Cooking >= 50.0` | Cooking (`14`) | `CulinariansGuild` | `RollingPin`, `Skillet` | `Dough`. |
| `BlacksmithShoppe` | `Blacksmith`, `BlacksmithGuildmaster` | `Blacksmith >= 50.0` | Blacksmith (`8`) | `BlacksmithsGuild` | `GodSmithing`, `SledgeHammer`, `SmithHammer`, `Tongs`, `AncientSmithyHammer` | Ingot item IDs `0x1BE3..0x1BFA`. |
| `BowyerShoppe` | `Bowyer`, `ArcherGuildmaster` | `Bowcraft >= 50.0` | Bowcraft (`20`) | `ArchersGuild` | `FletcherTools` | `BaseLog`, `BaseWoodBoard`. |
| `CarpentryShoppe` | `Carpenter`, `CarpenterGuildmaster` | `Carpentry >= 50.0` | Carpentry (`11`) | `CarpentersGuild` | `DovetailSaw`, `DrawKnife`, `Hammer`, `Froe`, `Inshave`, `WoodworkingTools`, `JointingPlane`, `MouldingPlane`, `Nails`, `Saw`, `Scorp`, `SmoothingPlane` | `BaseLog`, `BaseWoodBoard`. |
| `CartographyShoppe` | `Mapmaker`, `CartographersGuildmaster` | `Cartography >= 50.0` | Cartography (`12`) | `CartographersGuild` | `MapmakersPen` | `BlankScroll`, `BlankMap`. |
| `HerbalistShoppe` | `Herbalist`, `DruidTree`, `Druid`, `DruidGuildmaster` | `Druidism >= 50.0` | Average of Druidism and Veterinary (`55`) | `DruidsGuild` | `DruidCauldron` | Reagents that pass `MaterialInfo.IsReagent` and `DruidPouch.isDruidery`. |
| `LibrarianShoppe` | `Scribe`, `Sage`, `LibrarianGuildmaster` | `Inscribe >= 50.0` | Inscribe (`26`) | `LibrariansGuild` | `ScribesPen` | `BlankScroll`. |
| `MorticianShoppe` | `Undertaker`, `NecroMage`, `Witches`, `Necromancer`, `NecromancerGuildmaster` | `Forensics >= 50.0` | Average of Forensics and Necromancy (`56`) | `NecromancersGuild` | `WitchCauldron` | Reagents that pass `MaterialInfo.IsReagent` and `WitchPouch.isWitchery`. |
| `TailorShoppe` | `Weaver`, `Tailor`, `LeatherWorker`, `TailorGuildmaster` | `Tailoring >= 50.0` | Tailoring (`49`) | `TailorsGuild` | `GodSewing`, `SewingKit` | `BoltOfCloth`, `Cloth`, `UncutCloth`. |
| `TinkerShoppe` | `Tinker`, `TinkerGuildmaster` | `Tinkering >= 50.0` | Tinkering (`51`) | `TinkersGuild` | `TinkerTools` | Ingot item IDs `0x1BE3..0x1BFA`. |

## Stocking Rules

`BaseShoppe.OnDragDrop` accepts only the tool and resource classes listed above. The shoppe must be immovable, and the player must not have murder counts.

| Stock type | Counter | Capacity | Consumption behavior |
| --- | ---: | ---: | --- |
| Resources | `ShoppeResources` | `5000` | Adds `dropped.Amount`; if over capacity, returns the overflow amount to the backpack and deletes the consumed portion. |
| Tools | `ShoppeTools` | `1000` | Casts the dropped item to `BaseTool`, adds `UsesRemaining`, caps the counter at `1000`, then deletes the whole tool item. |

The shoppe does not inspect resource quality, tool quality, or crafter data for contract stocking. It only counts accepted stack amount or tool uses.

## Customer Cycle

Each shoppe has twelve persisted customer string slots, `Customer01` through `Customer12`. `CustomerCycle` calls `AddCustomers` for all twelve slots and fills only slots whose packed status field resolves to `0`.

Customer timers:

| Timer | Starts from | Delay | Behavior |
| --- | --- | ---: | --- |
| `QuickTimer` | `BaseShoppe` constructor and `Deserialize` | 60 seconds | Calls `Customers()` once. |
| `CustomerTimer` | `Customers()` when `ShoppeOwner != null` | 2 hours | Calls `Customers()` again and reschedules itself. |

If `Customers()` runs with `ShoppeOwner == null`, the shoppe deletes itself.

## Customer Data Format

Customer records are plain strings split by `#`. `Customers.GetDataElement` returns `"0"` for missing or empty fields.

| Section | Field | Meaning |
| ---: | --- | --- |
| 1 | Task text | Generated by the concrete shoppe's `MakeThisTask()`. |
| 2 | Customer name | Random male or female `NameList` name plus `HenchmanFunctions.GetTitle()`. |
| 3 | Status | Always written as `1` for generated requests. Empty records read as `0`. |
| 4 | Gold | Payment added to `ShoppeGold` on success. |
| 5 | Tools | Tool counter consumed on attempt. |
| 6 | Resources | Resource counter consumed on attempt. |
| 7 | Difficulty | Required skill benchmark used by `GetChance`. |
| 8 | Reputation | Reputation gained on success or lost on failure/refusal. |

## Contract Formulae

`Customers.AddCustomers` first computes effective reputation:

```text
effectiveReputation = ShoppeReputation
if PlayerMobile.NpcGuild == ShelfGuild:
    effectiveReputation += 500 + (GetSkillValue(ShelfSkill, from) * 5)
```

Then it generates a contract:

| Value | Formula |
| --- | --- |
| `repMax` | `effectiveReputation / 10` |
| `repMin` | `repMax / 5`, minimum `10`; if `repMin >= repMax`, set `repMax = repMin + 10` |
| Base gold | `Utility.RandomMinMax(repMin, repMax)` |
| Final gold | `(int)((baseGold * 4) * (MyServerSettings.GetGoldCutRate() * .01))` |
| Reputation reward | `Utility.RandomMinMax(gold / 20, (gold / 20) + Utility.RandomMinMax(0, 3))`, clamped to `5..50` |
| Difficulty field | `Utility.RandomMinMax((gold / 125) + 35, ((gold / 125) + 35) + Utility.RandomMinMax(0, 5))`, clamped to `30..120` |
| Tool cost | `Utility.RandomMinMax(gold / 100, (gold / 100) + Utility.RandomMinMax(0, 2))`, clamped to `1..10` |
| Resource cost | `Utility.RandomMinMax(gold / 20, (gold / 20) + Utility.RandomMinMax(0, 5))`, minimum `5` |

The success chance shown in `ShoppeGump` and used by `FillOrder` is:

```text
rate = skill - difficulty
if rate < 1: rate = 0
if rate > 0: rate += 25
if rate > 100: rate = 100
```

## Fulfilling or Refusing Contracts

`ShoppeGump` shows six customer records per page. Page `0` displays slots 1 through 6, and page `1` displays slots 7 through 12. A fulfill button is drawn only when the displayed contract has a positive chance, the shoppe has enough tools and resources, and `ShoppeGold + taskGold < 500001`.

When a contract is fulfilled:

1. `Customers.FillOrder` recomputes chance from current skill and the stored difficulty.
2. If chance is below `1`, or the shoppe lacks tools/resources, the method does nothing.
3. On success, the shoppe plays success sounds, adds stored gold, caps `ShoppeGold` at `500000`, adds stored reputation, and caps `ShoppeReputation` at `10000`.
4. On failure, the shoppe plays failure sounds and subtracts stored reputation, floored at `0`.
5. Successful and failed attempts both subtract stored tools and resources, floor both counters at `0`, remove the customer slot, and call `BaseShoppe.ProgressSkill` twice.

When a contract is refused, `Customers.CancelOrder` plays a refusal sound, subtracts the stored reputation value with a `0` floor, and removes the customer slot. Refusing does not consume tools or resources.

## Skill Progression

`BaseShoppe.ProgressSkill` calls `Mobile.CheckSkill(skill, 0, 125)` twice per completed attempt because `FillOrder` invokes it twice after removing the customer entry.

| Shoppe | Skill checked |
| --- | --- |
| `AlchemistShoppe` | Alchemy |
| `BakerShoppe` | Cooking |
| `BlacksmithShoppe` | Blacksmith |
| `BowyerShoppe` | Bowcraft |
| `CarpentryShoppe` | Carpentry |
| `CartographyShoppe` | Cartography |
| `HerbalistShoppe` | Druidism |
| `LibrarianShoppe` | Inscribe |
| `MorticianShoppe` | Forensics |
| `TailorShoppe` | Tailoring |
| `TinkerShoppe` | Tinkering |

## Cash Out

A secured shoppe adds a context-menu cash-out entry when `CheckAccess(from)` passes. `CheckAccess` uses the containing `BaseHouse` and the shoppe's `SecureLevel` (`Level`, default `Anyone`).

Cash-out creates a `BankCheck` and resets `ShoppeGold` to `0`.

```text
barter = (int)Mercantile
if PlayerMobile.NpcGuild == MerchantsGuild:
    barter += 25

cash = ShoppeGold + (ShoppeGold * (barter / 100))
```

The division is integer division because `barter` is an `int`.

## Task Text Generators

Task text is flavor only; completion logic uses the generated contract numbers, not the task text.

| Shoppe | Task generator behavior |
| --- | --- |
| `AlchemistShoppe` | Builds brew/create/concoct/boil phrasing around elixir, potion, draught, mixture, or philter text using random creature, material, mixture, and effect word lists. |
| `BakerShoppe` | Builds make/cook/bake phrasing around quantity words, taste adjectives, optional odd descriptors, and food names. |
| `BlacksmithShoppe` | Chooses one of 86 weapon/armor/shield Item classes, optionally gives it magic-style adjective text, otherwise prefixes a random metal resource name, then deletes the temporary item. |
| `BowyerShoppe` | Builds repair/fix/enhance/modify/restring/etc. phrasing around a bow/crossbow magic-name path or a random wood/resource bowcraft item path. |
| `CarpentryShoppe` | Builds repair/fix/sand/modify/polish/etc. phrasing around a wooden weapon/armor magic-name path or a random wood/resource furniture/tool path. |
| `CartographyShoppe` | Generates a made-up city, dungeon, or kingdom, then wraps it in map actions such as redraw, decipher, copy, trail-map, or treasure-map work. |
| `HerbalistShoppe` | Builds the same brew/create/concoct/boil structure as alchemy, but with herbal ingredient lists and druid-flavored mixtures/effects. |
| `LibrarianShoppe` | Randomly chooses document/spell-name work, law/story/journal/record binding work, titled book work, or spellbook transcription work. |
| `MorticianShoppe` | Has a one-in-four autopsy/remains/body examination path using a generated name/title; otherwise builds witch-style brew text with reagent and occult word lists. |
| `TailorShoppe` | Builds repair/fix/enhance/resize/embroider/etc. phrasing around a magic clothing path or a random cloth/clothing item path. |
| `TinkerShoppe` | Builds repair/fix/enhance/modify/resize/alter phrasing around a magic jewelry/light path or a possessive tinker item path. |

If a concrete `MakeThisTask()` ever returns null or empty, `BaseShoppe.MakeTask` falls back to `Craft that item they need for their upcoming journey`.

## Merchant Crate

`MerchantCrate` is a `Container`, not a `BaseShoppe`. It is still part of the Shoppes folder and is referenced by shoppe help text.

Compiled behavior:

| Area | Behavior |
| --- | --- |
| Vendor stock | Provisioner-style buy lists can sell `MerchantCrate` for `500` gold with stock `1`; sell lists can buy it for `250` gold. |
| Container display | Hides content and weight display, reports `CrateGold`, and reports current `Sale()` value of contained items. |
| Use gate | Double-click refuses when `CrateGold >= 500000`, when movable, or when the caller lacks range unless staff. |
| Drop gate | `OnDragDrop` refuses when gold-capped, movable, or murderer; `OnDragDropInto` refuses when gold-capped or movable. |
| Pickup timer | Direct or nested drops start `EmptyTimer`, which fires after 2 hours. Deserialize starts a 60-second `QuickTimer`. |
| Emptying | `Empty()` sums `GetItemValue(item, amount)` for each contained item into `CrateGold`, deletes the item, and stops the timer. |
| Cash out | Uses the same Mercantile/Merchants Guild integer-division formula as shoppe cash-out, then resets `CrateGold` to `0`. |

`GetItemValue` is a large explicit item-class table. Unsupported items are worth `0`.

`GetPrice` then adjusts supported crafted equipment:

| Item type | Price modifiers |
| --- | --- |
| `BaseArmor` | Low quality `* 0.60`; exceptional `* 1.25`; resource multipliers for metal, leather, scale, and wood resources; `+ 100 * Durability`; `+ 100 * ProtectionLevel`; non-player-constructed armor is worth `0`. |
| `BaseWeapon` | Low quality `* 0.60`; exceptional `* 1.25`; resource multipliers for metal and wood resources; `+ 100 * DurabilityLevel`; `+ 100 * DamageLevel`; non-player-constructed weapons are worth `0`. |
| `BaseInstrument` | Low quality `* 0.60`; exceptional `* 1.25`; resource multipliers for metal and wood resources; fewer than `300` uses or null `Crafter` makes it worth `0`. |
| `BaseClothing` | Non-player-constructed clothing is worth `0`. |
| `BaseTool` | Fewer than `50` uses makes the tool worth `0`. |

## Serialization

`BaseShoppe.Serialize` writes version `0`, then writes the secure level, owner, name, gold, tool count, resource count, reputation, page, shelf metadata, and all twelve customer strings. `Deserialize` reads the same positional sequence and starts `QuickTimer`.

All concrete `*Shoppe` subclasses write/read their own version `0` but do not persist subclass-specific fields. `HerbalistShoppe.Deserialize` and `MorticianShoppe.Deserialize` reset `Name`, `ShoppeName`, and `ShelfTitle` after reading the base data; the other concrete shoppe subclasses do not.

`MerchantCrate.Serialize` writes version `0` and `CrateGold`; `Deserialize` reads both values and starts the 60-second pickup timer.

## Known Issues

* `BaseShoppe.AddNameProperties` dereferences `ShoppeOwner.Name` without checking `ShoppeOwner` for null. Constructed, deserialized, or property-viewed shoppes with a missing owner can throw before the cleanup timer deletes them.
* `ShoppeGump.OnResponse` validates empty customer slots for button IDs `201..212`, but it does not reject fulfill button IDs above `212`. A spoofed reply such as `213` reaches `FillOrder` as customer `13`, which defaults to `Customer01` and then fails to remove any slot, allowing repeated attempts against the first contract while tools/resources last.
* Shoppe and merchant-crate cash-out use `barter / 100` with integer division. Mercantile values below `100` add no cash bonus, while `100` adds a full extra copy of stored gold.
* `MerchantCrate.OnDragDropInto` does not repeat the murderer check used by `OnDragDrop`, so a murderer who can open the crate path can still add nested items.
* `MerchantCrate.Empty()` can push `CrateGold` above `500000` because it sums item values without a cap; the cap is only checked before opening/dropping.
* Merchant crate player text says pickup happens "about a day" and the speech text says "Every day", but the compiled pickup timer is 2 hours, with a 60-second pickup pass after deserialize.
* Tool overfill on a shoppe deletes the entire dropped tool after capping `ShoppeTools` at `1000`; unlike resources, unused over-cap tool charges are not returned.
