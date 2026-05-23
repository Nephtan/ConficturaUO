# Housing System

## Scope

This page documents the RunUO housing framework centered on `Server.Multis.BaseHouse`.
It covers house placement, ownership, access control, item lockdowns and secures, decay, sign and region behavior, customizable foundations, and persistence rules.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Items/Houses/BaseHouse.cs` | Main `BaseHouse` implementation: owner table, decay, access lists, lockdowns, secures, vendors, moving crates, deletion cleanup, transfer targets, and serialization. |
| `Data/Scripts/Items/Houses/HousePlacement.cs` | Tile, region, road, static, item, mobile, and neighboring-house placement validation. |
| `Data/Scripts/Items/Houses/HousePlacementTool.cs` | `HousePlacementTool` construction contract, placement catalog gumps, preview house, bank withdrawal, and `HousePlacementEntry` definitions. |
| `Data/Scripts/Items/Houses/Deeds.cs` | Legacy `HouseDeed` placement target and deed-based static house placement. |
| `Data/Scripts/Items/Houses/HouseSign.cs` | House sign properties, sign double-click, decay refresh, owner claim flow, and vendor context entries. |
| `Data/Scripts/System/Regions/HouseRegion.cs` | Runtime `HouseRegion` for entry rules, speech commands, spell restrictions, item decay suppression, logout delay, and secure-container interaction. |
| `Data/Scripts/Items/Houses/HouseFoundation.cs` | Customizable `HouseFoundation`, design packet handlers, fixture melting, customization commit cost, and the `DesignInsert` staff command. |
| `Data/Scripts/System/Gumps/HouseGump.cs` | Pre-AOS house management gump. |
| `Data/Scripts/System/Gumps/HouseGumpAOS.cs` | AOS house management gump pages for information, security, storage, customization, ownership, lists, vendor inventory, and appearance changes. |
| `Data/Scripts/System/Gumps/HouseDemolishGump.cs` | Demolition confirmation and deed return behavior. |
| `Data/Scripts/System/Gumps/ConfirmHouseResize.cs` | Resize confirmation path for converting eligible houses into customizable foundations. |
| `Data/Scripts/System/Gumps/SetSecureLevelGump.cs` | Secure container access-level gump. |
| `Data/Scripts/Items/Houses/Houses.cs` | Legacy/static house classes such as `SmallOldHouse`, `Tower`, `Keep`, `Castle`, and shops. |
| `Data/Scripts/Items/Houses/NewHouses.cs` | Additional static house classes and sign offsets. |
| `Data/Scripts/Items/Houses/SmallTents.cs`, `TentsEast.cs`, `TentsSouth.cs`, `CircusTentsEast.cs`, `CircusTentsSouth.cs` | Tent house deeds and tent `BaseHouse` subclasses. |
| `Data/Scripts/Items/Houses/MovingCrate.cs` | Packing crate for relocated house contents and vendors. |
| `Data/Scripts/Items/Houses/InteriorDecorator.cs` | House-bound item movement tool. |

## Configuration

Housing behavior reads several values from `Server.Misc.MyServerSettings`.

| Setting method | Default field value | Runtime behavior |
| --- | ---: | --- |
| `HousesDecay()` | `false` | Seeds `BaseHouse.DecayEnabled`; when false, `DecayType` becomes `Ageless`. |
| `HomeDecay()` | `365.0` days | Values below `30` are clamped to `30`; `BaseHouse.DecayPeriod` is `TimeSpan.FromDays(HomeDecay())`. |
| `HousesPerAccount()` | `5` | `0` is coerced to `1`; any negative value is coerced to `-1`, which disables the account limit. |
| `AllowCustomHomes()` | `true` | Enables construction-contract categories for 2-story and 3-story customizable foundations. |
| `AllowHouseDyes()` | `true` | Allows non-foundation placement to copy the construction contract hue to the placed house. |
| `HouseStorage()` | `false` | When true, `HouseRegion.OnDecay` suppresses decay for all Items in the house region. |
| `HouseOwners()` | `false` | When true, listed co-owners pass `IsOwner`. |

Static limits:

| Limit | Value |
| --- | ---: |
| Maximum co-owners | `15` |
| Maximum friends, non-AOS rules | `50` |
| Maximum friends, AOS rules | `140` |
| Maximum bans, non-AOS rules | `50` |
| Maximum bans, AOS rules | `140` |
| Maximum barkeepers | `2` |

## Placement Flow

There are two placement entry points:

| Entry point | Item | Cost path | Flow |
| --- | --- | --- | --- |
| Legacy deed | `HouseDeed` subclasses in `Deeds.cs` | No bank withdrawal in `HouseDeed.OnPlacement`; the deed is deleted on success. | Double-click requires the deed in the backpack and checks `BaseHouse.HasAccountHouse(from)` for non-staff callers. The `HousePlacementTarget` checks `Region.AllowHousing`, then calls `HousePlacement.Check`. |
| Construction contract | `HousePlacementTool` | Non-staff callers must pay `HousePlacementEntry.Cost` from the bank. | Double-click opens classic/custom category gumps. Selecting an entry targets a location, validates it, creates a `PreviewHouse`, moves blocking movables to the preview ban location, shows a warning gump, and only places the real house after confirmation. |

Staff with `AccessLevel.GameMaster` bypass the placement validation in `HousePlacement.Check` and are not charged gold, but the code reports what would have been withdrawn.

The placement catalog is data-driven through `HousePlacementEntry` arrays:

| Catalog | Array | House type |
| --- | --- | --- |
| Classic houses | `HousePlacementEntry.ClassicHouses` | Static `BaseHouse` subclasses and tent houses. |
| 2-story custom foundations | `HousePlacementEntry.TwoStoryFoundations` | `HouseFoundation` entries using multi IDs beginning at `0x13EC`. |
| 3-story custom foundations | `HousePlacementEntry.ThreeStoryFoundations` | Larger `HouseFoundation` entries using multi IDs beginning at `0x140B`. |

`HousePlacementEntry` stores two storage/lockdown pairs: legacy values and AOS/new-vendor-system values. `Storage` and `Lockdowns` return the new pair when `BaseHouse.NewVendorSystem` is true.

## Placement Validation

`HousePlacement.Check(Mobile from, int multiID, Point3D center, out ArrayList toMove)` validates the target before placement.

| Check | Failure result |
| --- | --- |
| Map is `null` or `Map.Internal` | `BadLand` |
| Target center is in `NoHousingRegion` | `BadRegion` |
| Any component tile's `Region.AllowHousing(from, testPoint)` is false | `BadRegion`, or `BadRegionTemp` for `TempNoHousingRegion` |
| Foundation component conflicts vertically with terrain | `BadLand` |
| Foundation component intersects blocking static tiles | `BadStatic` |
| Foundation component intersects non-movable blocking Items | `BadItem` |
| Foundation component intersects movable Items or Mobiles | Added to `toMove`; moved to the house ban location after placement. |
| Foundation or border tile is on a road ID | `BadLand`, except for hard-coded special regions/world areas. |
| Border tile is impassable land, blocking static, or blocking non-movable Item | `BadLand`, `BadStatic`, or `BadItem` |
| Yard tile overlaps an existing `BaseHouse` | `BadStatic` |
| All checks pass | `Valid` |

The code defines `NoSurface`, `BadRegionHidden`, and `InvalidCastleKeep` results. The traced `HousePlacement.Check` body does not currently return `NoSurface`, `BadRegionHidden`, or `InvalidCastleKeep`; the calling gumps still contain feedback cases for them.

## Ownership And Access

`BaseHouse` keeps a static `Dictionary<Mobile, List<BaseHouse>>` keyed by the owner Mobile. Constructors, deserialization, owner reassignment, and deletion maintain this table.

| Relationship | Stored in | Access rule |
| --- | --- | --- |
| Owner | `m_Owner` | Exact owner, GameMaster or above, same account under AOS rules, or co-owner when `MyServerSettings.HouseOwners()` is true. |
| Co-owner | `m_CoOwners` | Owner or explicit co-owner; under non-AOS rules, same-account characters count as co-owners. |
| Friend | `m_Friends` | Co-owner or explicit friend. |
| Access invite | `m_Access` | Explicit invite list used by private AOS houses. |
| Ban | `m_Bans` | Explicit Mobile ban, plus matching account ban; owners and staff above player access are never banned. |

Account ownership is enforced through `BaseHouse.HasAccountHouse(Mobile m)`. It scans every Mobile on the caller's `Account` and calls `HasHouse` on each. `HasHouse` compares the number of owned houses to `MyServerSettings.HousesPerAccount()`, where `-1` means no limit.

If an ownerless house sign is double-clicked by a player who is a valid friend or co-owner and the account does not already own a house, the sign offers a warning gump to claim the house. Claiming sets `Owner = from` and updates `LastTraded`.

## House Sign And Management Gumps

Double-clicking a `HouseSign` calls `ShowSign`.

| Condition | Behavior |
| --- | --- |
| Caller is a friend and below GameMaster | Refreshes decay. In ML, only the owner refresh path is enabled; outside ML, any friend refreshes. |
| `BaseHouse.IsAosRules` | Opens `HouseGumpAOS` on the Information page. |
| Not AOS rules | Opens legacy `HouseGump`. |
| New vendor system and sign context menu is available | Shows vendor placement and vendor inventory reclaim entries when the house exposes them. |

`HouseGumpAOS` exposes these pages:

| Page | Main functions |
| --- | --- |
| Information | Owner, placement status, public/private status, decay state, built date, last traded date, value, visits. |
| Security | Co-owner, friend, ban, and access list management; public/private toggle. |
| Storage | Secure storage, moving crate use, lockdown use, vendor counts, and available capacity. |
| Customize | Enter foundation customization, rename house, change sign, sign hanger, sign post, foundation type, relocate moving crate, and convert eligible houses. |
| Ownership | Demolish house, trade house, and disabled primary-house button. |
| Vendors | Select available house vendors when opened through the sign context menu. |

The legacy `HouseGump` exposes owner, storage, co-owner/friend/ban management, ownership transfer, demolition, lock toggling, public/private state, and renaming.

## Speech Commands

`HouseRegion.OnSpeech` handles house speech after confirming the speaker is alive, a friend/co-owner/owner, inside the active house, and has sufficient role for the action.

| Speech or keyword | Required role | Action |
| --- | --- | --- |
| `I wish to resize my house` | Owner | ML-only resize prompt, only while standing on the sign tile and at least one hour after `BuiltOn`. |
| `remove thyself` | Friend | Targets a Mobile for `HouseKickTarget`; valid targets are moved to `BanLocation`. |
| `I ban thee` | Friend | Targets a Mobile for `HouseBanTarget`; banned targets are added to `m_Bans` and moved to `BanLocation`. Private AOS houses reject ban and require access revocation instead. |
| `I wish to lock this down` | Co-owner | Targets an Item for lockdown. |
| `I wish to release this` | Co-owner | Targets a locked-down or secure Item for release. |
| `I wish to secure this` | Owner | Targets a Container for secure storage, or locks down non-container Items. |
| `I wish to unsecure this` | Owner | Targets a secure Item for release. |
| `I wish to place a strongbox` | Co-owner but not owner | Places a co-owner strongbox. Owners are explicitly refused. |
| `trash barrel` | Co-owner | Places a trash barrel. |

## Region Rules

`BaseHouse.UpdateRegion()` unregisters the existing region and registers a new `HouseRegion` whenever the house map/location changes or the house is constructed/deserialized.

| Hook | Behavior |
| --- | --- |
| `HouseRegion.OnLogin` | If a Mobile logs into a private house and is not a friend, it is moved to `BanLocation`. |
| `AllowHarmful` | Friend/owner/co-owner vs friend/owner/co-owner is allowed. Player-vs-player harmful action is otherwise rejected. Harmful action across different regions is rejected. |
| `OnBeginSpellCast` | Only owners, co-owners, and friends may begin spell casts in the house region. Others receive "That does not seem to work here." |
| `OnLocationChanged` and `OnMoveInto` | Enforce bans, private AOS access, combat restriction, uncontrolled creature entry, summon restrictions, and exclusive foundation customization. |
| `OnDecay` | Suppresses all item decay when `HouseStorage()` is true; otherwise suppresses decay for locked-down or secure Items inside the house. |
| `GetLogoutDelay` | Friends inside the house log out instantly unless they recently aggressed a player within `CombatHeatDelay` of 30 seconds. |
| `OnDoubleClick` | Secure containers check `SecureInfo.Level`; inaccessible containers block the double-click. |
| `OnSingleClick` | Locked-down Items receive `[locked down]`; secure Items receive `[locked down & secure]`. |

## Lockdowns, Secures, And Storage

Lockdown is available to co-owners while the house is active. The target must be movable, not already secured, inside the house unless the caller bypasses that parameter, not imbued by `Ethics.Ethic`, and not inside a secure container. If the target has a parent Item, that parent must already be locked down.

Lockdown cost is:

| Ruleset | Limit check |
| --- | --- |
| Non-AOS | `LockDownCount + (1 + item.TotalItems) <= MaxLockDowns` |
| AOS | `CheckAosLockdowns(1 + item.TotalItems)` and `CheckAosStorage(1 + item.TotalItems)` |

Securing is owner-only while the house is active. A non-container target falls through to lockdown. A container must be inside the house, on the ground, movable, not already locked down, within secure-container limits, and within AOS storage limits. New secure containers are stored as `SecureInfo(container, SecureLevel.Owner)`, set `IsSecure = true`, made immovable, removed from `m_LockDowns`, and shown in `SetSecureLevelGump`.

Secure levels are:

| SecureLevel | Access rule |
| --- | --- |
| `Owner` | `IsOwner(m)` |
| `CoOwners` | `IsCoOwner(m)` |
| `Friends` | `IsFriend(m)` |
| `Anyone` | Always true |
| `Guild` | Same guild as owner |

AOS storage uses `GetAosCurSecures`:

| Source | Counted as |
| --- | --- |
| Secure containers | Sum of `SecureInfo.Item.TotalItems` |
| Secure container slots | Added to lockdown count |
| Locked-down Items | Added through `GetLockdowns()` |
| Player vendor backpacks when the new vendor system is disabled | Added to vendor storage |
| Moving crate | `MovingCrate.TotalItems`, minus `PackingBox` wrapper containers |

`GetAosMaxSecures`, `GetAosMaxLockdowns`, and `GetNewVendorSystemMaxVendors` come from the matching `HousePlacementEntry` multiplied by `BonusStorageScalar`, which is `1.2` under ML and `1.0` otherwise.

## Decay

`BaseHouse.Configure()` sets the item lockdown/secure flags and starts a one-minute repeating timer that calls `Decay_OnTick`. That timer calls `CheckDecay` for every `BaseHouse` in `m_AllHouses`.

`DecayType` is computed as follows:

| Condition | DecayType |
| --- | --- |
| `RestrictDecay`, global decay disabled, or zero decay period | `Ageless` |
| No owner or no account | AOS: `Condemned`; non-AOS: `ManualRefresh` |
| Owner account or any account character is GameMaster or above | `Ageless` |
| Non-AOS rules | `ManualRefresh` |
| AOS owner account is inactive | `Condemned` |
| AOS owner account has multiple houses and this house is newest by `max(LastTraded, BuiltOn)` | `AutoRefresh` |
| AOS owner account has multiple houses and this house is not newest | `ManualRefresh` |

Only `Condemned` and `ManualRefresh` houses can decay.

`DecayLevel` uses this formula:

```text
percent = ((DateTime.Now - LastRefreshed).Ticks * 1000) / DecayPeriod.Ticks
```

| Percent threshold | DecayLevel |
| ---: | --- |
| Not decayable | `Ageless`; also sets `LastRefreshed = DateTime.Now` |
| `>= 1000` | `Collapsed`, unless rented vendors or vendor inventories exist, then `DemolitionPending` |
| `>= 950` | `IDOC` |
| `>= 750` | `Greatly` |
| `>= 500` | `Fairly` |
| `>= 250` | `Somewhat` |
| `>= 005` | `Slightly` |
| Otherwise | `LikeNew` |

`RefreshDecay()` refuses condemned houses, stores the old level, sets `LastRefreshed = DateTime.Now`, and returns true only if the prior level was worse than `LikeNew`.

When `CheckDecay()` sees `Collapsed`, it schedules `Decay_Sandbox`. That path creates a `TempNoHousingRegion` under ML, destroys player vendors and barkeepers, then deletes the house.

## Demolition, Deletion, And Transfer

Owner transfer runs through `BeginConfirmTransfer`, `HouseTransferGump`, a secure trade `TransferItem`, and `EndConfirmTransfer`. Transfer is blocked when the house is deleted, the owner is dead, the caller is not owner, the decay level is `DemolitionPending`, the recipient already has an account house, or positioning checks fail. Successful transfer clears bans, friends, and co-owners, sets `Owner = newOwner`, updates `LastTraded`, and removes old owner keys.

Deletion cleanup releases or deletes house-linked objects:

| Object set | Cleanup behavior |
| --- | --- |
| Region | Unregistered and nulled. |
| Sign and trash barrel | Deleted. |
| Doors | Deleted and list cleared. |
| Lockdowns and vendor rental contracts | `IsLockedDown` and `IsSecure` cleared, made movable, `SetLastMoved()` called, list cleared. |
| Secures | Strongboxes are destroyed; other secure containers are released, made movable, and marked moved. |
| Addons | If the addon has a deed, the deed is moved to the addon's world location; retained hue is copied when supported; addon item is deleted. |
| Vendor inventories | Deleted. |
| Moving crate | Deleted. |
| Vendors and barkeepers | Destroyed/deleted through `KillVendors()`. |

`BaseHouse.HandleDeletion(Mobile mob)` transfers or deletes houses when an owning Mobile is deleted. If another account character exists, ownership transfers to that character. If no alternate character exists and the house has co-owners, owner becomes `null` so a co-owner can claim it. If no alternate character and no co-owners exist, the house is deleted.

## Customizable Foundations

`HouseFoundation` is a `BaseHouse` that always reports AOS rules and becomes inactive while `Customizer != null`.

`BeginCustomize(Mobile m)` checks the player is alive, relocates entities, moves Items and other Mobiles to `BanLocation`, adds a `DesignContext`, hides the customizer, moves them to the foundation, sends `BeginHouseCustomization`, and sends the current design state to the client.

The custom house packet handlers are registered in `HouseFoundation.Initialize`:

| Packet/command | Handler |
| --- | --- |
| Extended `0x1E` | `QueryDesignDetails` |
| Encoded `0x02` | `Designer_Backup` |
| Encoded `0x03` | `Designer_Restore` |
| Encoded `0x04` | `Designer_Commit` |
| Encoded `0x05` | `Designer_Delete` |
| Encoded `0x06` | `Designer_Build` |
| Encoded `0x0C` | `Designer_Close` |
| Encoded `0x0D` | `Designer_Stairs` |
| Encoded `0x0E` | `Designer_Sync` |
| Encoded `0x10` | `Designer_Clear` |
| Encoded `0x12` | `Designer_Level` |
| Encoded `0x13` | `Designer_Roof` |
| Encoded `0x14` | `Designer_RoofDelete` |
| Encoded `0x1A` | `Designer_Revert` |

Commit pricing is:

```text
newPrice = oldPrice
    + CustomizationCost
    + ((DesignState.Components.List.Length - (CurrentState.Components.List.Length + CurrentState.Fixtures.Length)) * 500)

cost = newPrice - oldPrice
```

`CustomizationCost` is `0` under AOS and `10000` otherwise. Positive `cost` is withdrawn from the bank. Negative `cost` is deposited as a refund. GameMasters receive informational messages instead of bank transactions.

On commit, the code clears visible fixtures, melts fixtures from the design state, adds melted fixtures, assigns `CurrentState`, updates `Price`, removes the `DesignContext`, sends `EndHouseCustomization`, sends detailed design data, checks the signpost, ejects all Items and Mobiles to `BanLocation`, and restores relocated entities.

`MaxLevels` is `4` when the component width or height is at least `14`; otherwise it is `3`. `GetLevelZ(level, house)` clamps invalid levels to `1` and returns `(level - 1) * 20 + 7`.

## Admin Commands

| Command | Access | Usage | Description | Behavior |
| --- | --- | --- | --- | --- |
| `DesignInsert` | `GameMaster` | `[DesignInsert` | `Inserts multiple targeted items into a customizable houses design.` | Starts a target cursor. Each targeted Item must be inside a `HouseFoundation`, and all targeted Items in one run must belong to the same foundation. Valid Items are added to both current and design component lists, deleted from the world, and the target repeats until canceled. |

No player `[Command]` registration was found for normal house management. Players use deeds, the construction contract gumps, house sign gumps, context menus, and house speech keywords.

## Serialization

| Class | Version | Notes |
| --- | ---: | --- |
| `BaseHouse` | `14` | Calls `base.Serialize`, then writes relative ban location, vendor rental contracts, internalized vendors, relocated entities, vendor inventories, last refresh, restrict-decay flag, visits, price, access list, built/traded timestamps, addons, secure list, public flag, owner, co-owner/friend/ban lists, sign, trash barrel, doors, lockdowns, max lockdowns, and max secures. Deserialization uses a fall-through `switch` and restores regions, flags, owner table entries, lockdown flags, secure flags, relocated entities, and old-version defaults. |
| `HouseFoundation` | `5` | Writes its own version, signpost, signpost graphic, foundation type, sign hanger, revision, fixtures, current/design/backup states, then calls `base.Serialize`. Deserialization reads the foundation payload first and calls `base.Deserialize` last. This is locally matched but opposite of the usual `base.Serialize` first pattern used by most house subclasses. |
| `HouseSign` | `0` | Writes the owning `BaseHouse` Item and original owner Mobile. Deleting the sign deletes the owning house if the house still exists. |
| `HouseDeed` | `1` | Writes placement offset then multi ID. Version 0 only stored multi ID. |
| `HousePlacementTool` | `0` | No custom fields; fixes zero weight to `3.0` during load. |
| Static house subclasses | `0` | Most concrete static house classes call `base.Serialize`, write version `0`, and only read the version on load. |

## Known Issues

| Issue | Evidence from trace | Risk |
| --- | --- | --- |
| Road-build exceptions use the caller's current region and coordinates, not each target tile. | `HousePlacement.Check` captures `in_reg` from `from.Location`, and the road exceptions test `in_reg`, `from.Map`, `from.X`, and `from.Y` inside both component and border loops. | Standing in a special allowed area can bypass road checks for a target elsewhere, while standing outside one can reject a valid target inside it. |
| `InvalidCastleKeep` is defined and handled by placement gumps but never returned by the traced validator. | `HousePlacementResult.InvalidCastleKeep` exists, and `HousePlacementTool` has feedback cases for it, but `HousePlacement.Check` does not return that value. | Castle/keep special placement failures cannot reach the intended localized message. |
| `HousePlacementEntry.ConstructHouse` swallows every constructor exception. | The method catches all exceptions with an empty `catch { }` and returns `null`. | Bad placement entries or constructor regressions fail silently, leaving staff without diagnostics. |
| `BaseHouse.HandleDeletion` assumes the deleted owning Mobile has an `Account`. | The method casts `mob.Account as Account`, then immediately uses `acct.Length` without guarding `acct == null`. | Deleting a Mobile that owns houses but has no account can throw before transfer/deletion cleanup. |
| ML resize speech can dereference a missing sign. | `HouseRegion.OnSpeech` stores `Item sign = m_House.Sign`, then the resize branch reads `sign.Map` and `from.InRange(sign, 0)` without a null check. | A damaged or partially deserialized house with no sign can crash this speech path. |
| Foundation commit path contains an acknowledged temporary delete/customization workaround. | `HouseFoundation.EndConfirmCommit` has an inline note that a deleted foundation should boot the client out of customization mode in the delete handler. | Customization cleanup around deleted foundations is explicitly incomplete. |
| `HouseFoundation` serialization order is nonstandard. | It serializes derived foundation state before calling `base.Serialize`, then deserializes derived state before `base.Deserialize`. | The current read/write order matches, but future serializer edits must preserve this order exactly or world saves will shift. |
