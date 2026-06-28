# NPC Real Estate Broker Appraisal

## Scope

This page documents the RunUO house deed appraisal and buyback behavior handled by `Server.Mobiles.RealEstateBroker`.
It covers the broker `Mobile`, the `HouseDeed` target flow, drag/drop deed sale, hardcoded refund math, related vendor stock, and persistence.

The compiled system is a `BaseVendor` implementation. It is not a player command system, does not register `[Command]` handlers, does not use a `Gump`, and does not use XMLSpawner attachments.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/RealEstateBroker.cs` | Broker `Mobile`, speech keyword handling, movement prompt, deed drag/drop sale, refund table, and serialization. |
| `Data/Scripts/Items/Houses/Deeds.cs` | Base `HouseDeed` `Item`, placement target, standard house deed classes, and deed serialization. |
| `Data/Scripts/Items/Houses/SmallTents.cs` | Additional `HouseDeed` subclasses for blue and green tents. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | `SBRealEstateBroker` vendor buy/sell catalog used by `InitSBInfo()`. |
| `Data/Scripts/System/Misc/AOS.cs` | `AOS.Scale(int input, int percent)` helper used by refund math. |

## Mobile Definition

`RealEstateBroker` derives from `BaseVendor`, uses the vendor title `"the architect"`, and belongs to `NpcGuild.MerchantsGuild`.

`InitSBInfo()` adds one `SBRealEstateBroker` instance to the broker's `SBInfos` list. The catalog sells house tools, teleporters, and signs. It does not define the deed appraisal payout; appraisal and deed buyback are handled directly in `RealEstateBroker`.

## Movement Prompt

`OnMovement(Mobile m, Point3D oldLocation)` sends a private overhead hint when all of the following are true:

| Gate | Code behavior |
| --- | --- |
| Cooldown | `DateTime.Now > m_NextCheckPack`; the cooldown is then set to two seconds after a backpack is found. |
| Entered range | The moving `Mobile` is now within 4 tiles and was not within 4 tiles at `oldLocation`. |
| Player only | `m.Player` must be true. |
| Backpack exists | `m.Backpack` must not be null. |
| Deed present | The backpack must contain a direct, non-recursive `HouseDeed` found by `pack.FindItemByType(typeof(HouseDeed), false)`. |

When all gates pass, the broker privately tells that player's `NetState` that it can appraise or buy the deed and that handing over the deed sells it.

## Speech Appraisal

`HandlesOnSpeech(Mobile from)` returns `true` when the speaking `Mobile` is alive and within 3 tiles of the broker. Otherwise, it delegates to `BaseVendor`.

`OnSpeech(SpeechEventArgs e)` starts appraisal targeting only when:

| Gate | Behavior |
| --- | --- |
| Event unhandled | `!e.Handled` must be true. |
| Speaker alive | `e.Mobile.Alive` must be true. |
| Speech keyword | `e.HasKeyword(0x38)` must match the encoded `*appraise*` keyword. |

When those gates pass, the broker asks which deed should be appraised and calls `e.Mobile.BeginTarget(12, false, TargetFlags.None, Appraise_OnTarget)`. The speech event is marked handled.

`Appraise_OnTarget(Mobile from, object obj)` checks only the selected object's type:

| Target | Result |
| --- | --- |
| `HouseDeed` with positive computed price | Broker publicly states the gold payout and tells the player to hand over the deed to sell it. |
| `HouseDeed` with computed price `0` | Broker says it is not interested. |
| Any other target | Broker says it cannot appraise things it knows nothing about. |

Appraisal does not transfer gold, delete the deed, or verify backpack containment.

## Deed Sale

Players sell a supported deed by dragging and dropping the `HouseDeed` onto the broker.

`OnDragDrop(Mobile from, Item dropped)` handles only `HouseDeed` items. Non-`HouseDeed` items delegate to `BaseVendor.OnDragDrop`.

| Sale step | Behavior |
| --- | --- |
| Type check | `dropped is HouseDeed` must be true. |
| Price calculation | `ComputePriceFor(HouseDeed deed)` returns the hardcoded refund amount. |
| Unsupported deed | A computed price of `0` makes the broker say it is not interested and return `false`. |
| Bank deposit | `Banker.Deposit(from, price)` is called. |
| Successful deposit | Broker announces the deposited amount, deletes the deed, and returns `true`. |
| Failed deposit | Broker says the bank box is full and returns `false`; the deed is not deleted. |

## Refund Formula

`ComputePriceFor()` assigns a hardcoded base price by deed type, then returns `AOS.Scale(price, 80)`.
`AOS.Scale(input, percent)` is integer arithmetic: `(input * percent) / 100`.

| Deed type | Base price | Broker refund |
| --- | ---: | ---: |
| `StonePlasterHouseDeed` | 43,800 | 35,040 |
| `FieldStoneHouseDeed` | 43,800 | 35,040 |
| `SmallBrickHouseDeed` | 43,800 | 35,040 |
| `WoodHouseDeed` | 43,800 | 35,040 |
| `WoodPlasterHouseDeed` | 43,800 | 35,040 |
| `ThatchedRoofCottageDeed` | 43,800 | 35,040 |
| `BrickHouseDeed` | 144,500 | 115,600 |
| `TwoStoryWoodPlasterHouseDeed` | 192,400 | 153,920 |
| `TwoStoryStonePlasterHouseDeed` | 192,400 | 153,920 |
| `TowerDeed` | 433,200 | 346,560 |
| `KeepDeed` | 665,200 | 532,160 |
| `CastleDeed` | 1,022,800 | 818,240 |
| `LargePatioDeed` | 152,800 | 122,240 |
| `LargeMarbleDeed` | 192,800 | 154,240 |
| `SmallTowerDeed` | 88,500 | 70,800 |
| `LogCabinDeed` | 97,800 | 78,240 |
| `SandstonePatioDeed` | 90,900 | 72,720 |
| `VillaDeed` | 136,500 | 109,200 |
| `StoneWorkshopDeed` | 60,600 | 48,480 |
| `MarbleWorkshopDeed` | 60,300 | 48,240 |

Any `HouseDeed` subclass not listed above returns `0` and cannot be sold to the broker through this path.

## Serialization

`RealEstateBroker` uses standard RunUO mobile persistence:

| Method | Serialized data |
| --- | --- |
| `RealEstateBroker(Serial serial)` | Required world-load constructor delegates to `base(serial)`. |
| `Serialize(GenericWriter writer)` | Calls `base.Serialize(writer)` and writes version `0`. |
| `Deserialize(GenericReader reader)` | Calls `base.Deserialize(reader)` and reads the version integer. |

The broker has no custom persisted fields. `m_NextCheckPack` and `m_SBInfos` are runtime state.

The base `HouseDeed` serializer writes version `1`, then writes `Offset` and `MultiID`. Its deserializer reads version `1` with a fall-through to version `0`, where `MultiID` is read. Standard deed subclasses each write and read their own version `0` after the base deed serialization.

## Admin Commands

None. The traced broker code contains no `[Command]` registration and no command handler methods.

## Known Issues

| Issue | Impact |
| --- | --- |
| `ComputePriceFor()` checks `SmallBrickHouseDeed` twice in the first deed group. | Harmless duplicate branch, but it shows the refund table is manually maintained. |
| `BlueTentDeed` and `GreenTentDeed` are `HouseDeed` subclasses but are not included in `ComputePriceFor()`. | The broker recognizes them as house deeds, but appraisal and drag/drop sale return `0` and say the broker is not interested. |
| Appraisal targeting checks only `obj is HouseDeed`. | A player can appraise a targeted deed without proving the deed is in their backpack; no gold is paid until drag/drop sale, so this is an information/validation gap rather than a payout path. |
| Refund prices are hardcoded in `RealEstateBroker` instead of derived from the deed or housing price data. | New `HouseDeed` subclasses or changed house prices will not automatically update broker payouts. |
