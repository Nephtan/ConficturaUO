# Shipwright Sailing Tutorial

## Scope

This page documents the RunUO sailing tutorial exposed through shipwright-style NPC speech.
It covers the `Shipwright` and `DrunkenPirate` context-menu entry points, the shared `SpeechGump`, the `"Shipwright"` text returned by `SpeechFunctions.SpeechText()`, shipwright return-to-boat service, vendor stock, and the concrete boat mechanics that the tutorial points players toward.

The tutorial itself is not an admin command system, EventSink hook, packet handler, or XMLSpawner attachment.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs` | Shipwright `Mobile`, vendor catalogs, return-to-boat key service, context-menu tutorial entry, and serialization. |
| `Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs` | Alternate `BaseVendor` that opens the same `"Shipwright"` tutorial text. |
| `Data/Scripts/System/Misc/Talk.cs` | Shared `SpeechGump` and `SpeechFunctions.SpeechText()` provider for the `"Shipwright"` tutorial page. |
| `Data/Scripts/System/Commands/Player/MyChat.cs` | Marks the `Shipwright` conversation as memorized in player chat/conversation settings. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | `SBShipwright`, `SBSailor`, and `SBHighSeas` vendor stock and sell-back entries. |
| `Data/Scripts/Items/Boats/BaseBoatDeed.cs` | Launch flow for a fresh boat deed in the player's backpack. |
| `Data/Scripts/Items/Boats/BaseDockedBoat.cs` | Launch flow for a dry-docked boat item and persistence of the ship name. |
| `Data/Scripts/Items/Boats/BaseBoat.cs` | Active boat multi, movement, course navigation, dry docking, decay, keys, and serialization. |
| `Data/Scripts/Items/Boats/TillerMan.cs` | Owner/admin tiller interaction, map drop handling, and dry-dock entry point. |
| `Data/Scripts/Items/Boats/Gumps/TillerManGump.cs` | Direction, turn, one-step, stop, anchor, and rename controls opened from the tiller man. |
| `Data/Scripts/Items/Boats/Gumps/ConfirmDryDockGump.cs` | Confirmation gump that calls `BaseBoat.EndDryDock()`. |
| `Data/Scripts/Items/Boats/DockingLantern.cs` | `DockingLantern` secure item and `DockSearch.NearDock()` dock/lantern/lawn-pier detection. |
| `Data/Scripts/Items/Boats/BoatStain.cs` | Item that resets a docked boat item or deed to standard boat hue `0x5BE`. |
| `Data/Scripts/Items/Misc/Dyes/DyeTub.cs` | Generic dye tub support for dyeing `BaseBoatDeed` and `BaseDockedBoat` items. |
| `Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs` | Optional player `[bcblack]`, `[bcwhite]`, and `[bctransparent]` boat-control gumps. |

## Tutorial Entry Points

### Shipwright Mobile

`Shipwright` derives from `BaseVendor`, uses vendor title `"the shipwright"`, and belongs to `NpcGuild.FishermensGuild`.

| Skill | Range |
| --- | ---: |
| `Carpentry` | 60.0 to 83.0 |
| `Bludgeoning` | 36.0 to 68.0 |
| `Seafaring` | 75.0 to 98.0 |

`InitSBInfo()` adds `SBShipwright`, `SBSailor`, `SBHighSeas`, and `SBBuyArtifacts`. `InitOutfit()` keeps the base vendor outfit and adds a `SmithHammer` plus a random dyed `TricorneHat`.

### Drunken Pirate Mobile

`DrunkenPirate` is a stationary `BaseVendor` with title `"the drunken pirate"` and the same `NpcGuild.FishermensGuild`. It sets `Carpentry` 60.0 to 83.0, `Seafaring` 75.0 to 98.0, and `Cartography` 90.0 to 100.0. Its vendor stock combines `SBShipwright`, `SBFisherman`, `SBSailor`, `SBHighSeas`, `RSScrolls`, `SBMapmaker`, and `SBBuyArtifacts`.

The drunken pirate uses the same tutorial gump title, `"The High Seas"`, and the same `"Shipwright"` speech text as the shipwright.

### Context Menu Flow

| Step | Behavior |
| --- | --- |
| Context menu entry | Both NPCs append `SpeechGumpEntry(from, this)` after `base.GetContextMenuEntries()`. |
| Caller gate | `OnClick()` returns unless the stored caller is a `PlayerMobile`. |
| Duplicate gump gate | If the player already has a `SpeechGump`, no new tutorial gump is sent. |
| NPC greeting | `IntelligentAction.SayHey(m_Giver)` may play a greeting sound before the gump opens. |
| Gump title | `The High Seas`. |
| Text source | `SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Shipwright")`. |
| Memorized conversation | `SpeechFunctions.SpeechText()` calls `MyChat.speechText("Shipwright", from)`, which maps this conversation to entry number `39`. |

`SpeechGump` itself is a simple scrollable HTML gump. The close button submits button `0`, and `OnResponse()` only plays sound `0x4A`.

## Tutorial Text Topics

The `"Shipwright"` tutorial text is a single static HTML string built from the player name and NPC name.

| Topic | Code-backed tutorial content |
| --- | --- |
| Buying and launching ships | Players receive a deed, launch from water near a dock, and receive a portable representation when dry docking. |
| Keys | Launching creates a ship key in the backpack and another in the bank box. |
| Recall and return service | Keys are linked to the `BaseBoat`; shipwrights can return the key holder to the boat for 1,000 gold when travel gates pass. |
| Docking lanterns | The tutorial says secured home lanterns allow launch or dry docking near a water-side home, subject to the lantern security level. |
| Ship hue | Docked boat items and boat deeds can be dyed in the backpack; that hue is copied to the active boat, tiller, hold, and planks on launch. |
| Tiller control | The owner/admin must be on the deck to open `TillerManGump`; off-deck double-clicking the tiller attempts dry docking instead. |
| Anchor, movement, stop, turn, one-step | The tiller gump controls anchor state, directional movement, stop, turn left/right, come about, rename, and one-step mode. |
| Sea-chart courses | Dropping a non-blank `MapItem` with pins onto the tiller associates it with the boat; speech commands such as `start`, `continue`, `goto #`, and `single #` drive course navigation. |
| Hold and deck requirements | Dry docking requires the boat to be anchored, keyed, alive caller, empty hold, and no extra items or mobiles on deck. |
| Boat decay | Active boats decay after `MyServerSettings.BoatDecay()` days without refresh when not moving. The default setting in code is 365 days, not the 30 sunrises stated by the tutorial text. |

## Shipwright Return-To-Boat Service

`Shipwright.OnDragDrop(Mobile from, Item o)` handles dropped ship keys.

| Gate | Behavior |
| --- | --- |
| Item type | The dropped item must be a `Key` with nonzero `KeyValue` and a `Link` that is a `BaseBoat`. |
| Boat/key validity | The linked boat must not be deleted, and `boat.CheckKey(key.KeyValue)` must match either plank key. |
| Cost | The caller's backpack must contain 1,000 `Gold`; `pack.ConsumeTotal(typeof(Gold), 1000)` is used. |
| Destination | `ReturnToBoat(boat.GetMarkedLocation(), boat.Map, from)` uses the boat's facing-adjusted `MarkOffset`. |
| Feedback | On payment, the player receives `You pay 1,000 gold.`; otherwise the shipwright says the service costs 1,000 gold and the player receives `You do not have enough gold.` |

`ReturnToBoat()` applies travel and destination gates before moving the player:

| Gate | Behavior |
| --- | --- |
| Origin travel | `SpellHelper.CheckTravel(from, TravelCheckType.RecallFrom)` must pass; failure produces no local message in this method. |
| Escape rules | `Worlds.AllowEscape()` must allow travel from the caller's current location. |
| Origin recall region | `Worlds.RegionAllowedRecall()` must allow recall from the caller's current region. |
| Destination teleport region | `Worlds.RegionAllowedTeleport()` must allow teleporting to the boat mark location. |
| Destination travel | `SpellHelper.CheckTravel(from, map, loc, TravelCheckType.RecallTo)` must pass. |
| Weight | `WeightOverloading.IsOverloaded(from)` blocks travel with localized message `502359`. |
| Spawn fit | `map.CanSpawnMobile(loc.X, loc.Y, loc.Z)` must be true. |
| Success | Pets teleport with the player, location particles play at source and destination, and the player moves to the boat mark location. |

## Vendor Stock

`SBShipwright.InternalBuyInfo` uses `MyServerSettings.SellChance()` for normal stock and `SellRareChance()` for docking lanterns. By default, the configured regular sell chance is `50%` and rare sell chance is `25%`; both compare their setting against `Utility.RandomMinMax(1, 100)`.

| Catalog | Item | Price | Quantity | Chance helper |
| --- | --- | ---: | ---: | --- |
| `SBShipwright` | `SmallBoatDeed` | 10,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `SmallDragonBoatDeed` | 11,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `MediumBoatDeed` | 12,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `MediumDragonBoatDeed` | 13,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `LargeBoatDeed` | 14,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `LargeDragonBoatDeed` | 15,000 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `DockingLantern` | 58 | random 1 to 15 | `SellRareChance()` |
| `SBShipwright` | `Sextant` | 13 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `GrapplingHook` | 58 | random 1 to 15 | `SellChance()` |
| `SBShipwright` | `BoatStain` | 26 | random 1 to 15 | `SellChance()` |
| `SBSailor` | `Harpoon` | 40 | random 3 to 31 | Always added |
| `SBSailor` | `HarpoonRope` | 2 | random 50 to 250 | Always added |
| `SBSailor` | `FishingPole` | 15 | random 3 to 31 | Always added |
| `SBSailor` | `SwordsAndShackles` | 50 | random 1 to 15 | Always added |
| `SBSailor` | `StandardSeafaringStudyBook` | 7,500 | 1 | `SellChance()` |
| `SBSailor` | `BoatStain` | 26 | random 1 to 15 | `SellChance()` |
| `SBSailor` | `Sextant` | 13 | random 1 to 15 | `SellChance()` |
| `SBSailor` | `GrapplingHook` | 58 | random 1 to 15 | `SellChance()` |
| `SBSailor` | `DockingLantern` | 58 | random 3 to 31 | `SellChance()` |
| `SBSailor` | `SmallBoatDeed` | 9,500 | random 1 to 15 | `SellChance()` |

`SBHighSeas.InternalSellInfo` lets vendors buy back some sea goods when the matching buy-chance helper passes.

| Item | Sell-back price | Chance helper |
| --- | ---: | --- |
| `Harpoon` | 20 | `BuyRareChance()` |
| `HarpoonRope` | 1 | `BuyRareChance()` |
| `SeaShell` | 58 | `BuyChance()` |
| `DockingLantern` | 29 | `BuyChance()` |
| `RawFishSteak` | 1 | `BuyRareChance()` |
| `Fish` | 1 | `BuyRareChance()` |

## Launch And Dry-Dock Mechanics

`BaseBoatDeed` and `BaseDockedBoat` use parallel launch logic.

| Gate | Deed or docked item behavior |
| --- | --- |
| Backpack | The item must be a child of the caller's backpack. |
| Dock proximity | Non-carpet boats require `DockSearch.NearDock(from)` before targeting and again before placement. |
| Current region | The caller's current region must be one of the sea/outdoor/public/main-region checks in `OnDoubleClick()` and `OnPlacement()`. |
| Targeted region | The target point cannot be in a `DungeonRegion`, `HouseRegion`, or `ChampionSpawnRegion`. |
| Current house/boat | Placement fails if the caller is in a `HouseRegion` or already on another boat. |
| Fit checks | `BaseBoat.IsValidLocation(p, map)` and `boat.CanFit(p, map, boat.ItemID)` must both pass. |

On successful launch, the server deletes the deed/docked item, sets `boat.Owner = from`, starts the boat anchored, creates a matching key in bank and backpack, applies the boat hue to tiller/hold/planks, links plank keys, and moves the boat multi to the world. `BaseDockedBoat` also restores the preserved `ShipName`.

If the owner has at least `90.0` base `Seafaring` and the boat has a `BoatDoor`, launch makes the cabin hatch visible and hues it to match the boat.

Dry docking starts through `TillerMan.OnDoubleClick()` or `OnDoubleClickDead()`: owner/admin callers on the boat open `TillerManGump`, while owner/admin callers off the boat attempt `BeginDryDock()`; non-carpet boats must be near a dock. `CheckDryDock()` then requires a live caller, matching ship key in backpack or bank, lowered anchor, empty hold, valid map, and no extra items or mobiles within the boat bounds. On confirmation, `EndDryDock()` removes keys, adds the docked boat item to the caller's backpack, plays the placement sound, and deletes the active boat.

## Docking Lanterns And Dock Search

`DockSearch.NearDock(Mobile m)` returns true when any of these conditions apply:

| Condition | Details |
| --- | --- |
| Region/world exemptions | Ambrosia, sea towns, Isle of the Lich, Island of Poseidon, Buccaneer's Den, Underworld, Island of the Black Knight, or Island of Stonegate. |
| Skill exemption | `Seafaring.Base >= 100.0` allows launch/dry dock anywhere. |
| Docking lantern | A `DockingLantern` within 30 tiles, non-movable, in a house the caller can access, and with secure access at the lantern's `SecureLevel`. |
| Lawn pier | A `LawnItem` pier with item ID `942`, `20403`, or `20404` in a house the caller can access. |
| Lawn pier piece | A `LawnPiece` with item ID `0x1AD0` whose parent lawn item has a house the caller can access. |

`DockingLantern` is an `Item` named `docking lantern`, weighs `2.0`, emits `LightType.Circle300`, implements `ISecurable`, and serializes only its `SecureLevel`.

## Tiller And Navigation

`TillerMan.OnDoubleClick()` only opens the gump for the boat owner or an administrator. The caller must be standing on the boat; otherwise the same double-click attempts dry docking.

`TillerManGump` buttons call the same `BaseBoat` methods used by speech commands:

| Control | Method behavior |
| --- | --- |
| Direction buttons | `StartMove(direction, true)` for continuous fast movement, or `OneMove(direction)` when one-step mode is toggled. |
| Stop | `StopMove(true)`. |
| Rename | `BeginRename(m_From)`, which prompts for a new ship name. |
| One step | Toggles the gump-local `ToggleOneStep` flag and reopens the gump. |
| Anchor | Calls `RaiseAnchor(true)` if anchored, otherwise `LowerAnchor(true)`. |
| Turn left/right | `StartTurn(-2, true)` or `StartTurn(2, true)`. |
| Come about | `StartTurn(-4, true)`. |

`BaseBoat.OnSpeech()` also handles tiller keywords from any mobile for which `CanCommand(from)` and `Contains(from)` pass. In this codebase `CanCommand()` returns true, so the practical speech gate is being physically on the boat.

| Speech group | Behavior |
| --- | --- |
| Name commands | Set name, remove name, and report current name. |
| Continuous movement | Fast and slow forward/backward/left/right/diagonal movement. |
| One-step movement | One tile in forward/backward/left/right/diagonal directions. |
| Turning | Turn right, turn left, and come about. |
| State commands | Stop, drop anchor, raise anchor, and report current navigation point. |
| Course commands | `start`, `continue`, `goto #`, and `single #`. |

For sea-chart courses, a player on the boat drops a non-blank `MapItem` with at least one pin onto the tiller. `AssociateMap()` stores that map, resets `NextNavPoint` to `-1`, and the tiller says `A map!`. `start` resets `NextNavPoint` to `0` and follows the full course. `continue` resumes the current nav point. `goto #` selects a numbered pin and follows the course from there. `single #` travels only to the selected pin.

## Optional Boat Navigation Commands

The separate `BoatNavigationControl` gump registers three player-level commands. These are not required by the shipwright tutorial, but they expose an alternate boat-control UI.

| Command | Access | Usage metadata | Behavior |
| --- | --- | --- | --- |
| `[bcblack]` | `Player` | `[Usage("[bcblack")]` | Opens `BoatNavigationControl` with a black background. |
| `[bcwhite]` | `Player` | `[Usage("[bcwhite")]` | Opens `BoatNavigationControl` with a white background. |
| `[bctransparent]` | `Player` | `[Usage("bctransparent")]` | Opens `BoatNavigationControl` with no background. |

The command handlers do not require the player to be on a boat. The gump response later finds a boat at the player's current location with `BaseBoat.FindBoatAt(from, from.Map)` and returns if none is found.

## Boat Hue Tools

Generic `DyeTub` targeting explicitly allows `BaseBoatDeed` and `BaseDockedBoat` items when they are movable, nearby, and not worn. The selected hue is copied into `item.Hue`, and a zero or negative boat hue is corrected to standard `0x5BE`.

`BoatStain` is narrower: it must be in the caller's backpack, targets a docked ship item or boat deed in the caller's backpack, and sets that item hue to `0x5BE`.

## Serialization

| Type | Version | Serialized fields after version | Load behavior |
| --- | ---: | --- | --- |
| `Shipwright` | 0 | None | Standard `BaseVendor` load. |
| `DrunkenPirate` | 0 | None | Standard `BaseVendor` load. |
| `DockingLantern` | 0 | `SecureLevel` as `int` | Restores the lantern security level. |
| `BaseBoatDeed` | 0 | `MultiID`, `Offset` | Restores launch multi and offset, normalizes weight and default hue. |
| `BaseDockedBoat` | 1 | `MultiID`, `Offset`, `ShipName` | Version 0 additionally consumes an old `uint`; normalizes blessed loot type, weight, and default hue. |
| `BaseBoat` | 3 | Map item, next nav point, facing, decay delta, owner, port plank, starboard plank, tiller, hold, boat door, anchored flag, ship name | Older versions initialize missing nav/facing/decay fields and add the boat to the static instance list. |
| `TillerMan` | 0 | Boat `Item` reference | Deletes itself if the boat reference is missing. |
| `ConfirmDryDockGump` | None | Not serialized | Ephemeral gump only. |
| `BoatStain` | 0 | None | Standard item load. |

Concrete boat, deed, and docked-boat classes such as `SmallBoat`, `SmallBoatDeed`, and `SmallDockedBoat` write their own version `0` after calling the base serializer.

## Admin Commands

None. The shipwright tutorial and return-to-boat service do not register admin-only commands.

The optional boat navigation gump registers player commands only: `[bcblack]`, `[bcwhite]`, and `[bctransparent]`.

Staff can inspect or edit exposed `[CommandProperty]` values on boats, planks, docking lanterns, and boat components.

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpeechGump.OnResponse()` reads `sender.Mobile` and calls `from.SendSound()` without null checks. | A stale or malformed gump response can null-reference in the shared speech close handler. |
| `SpeechFunctions.SpeechText()` reads both mobile names and calls `MyChat.speechText()` before validating either `Mobile`. | Any caller that passes a null NPC or player can throw before returning tutorial text. |
| `Shipwright.SpeechGumpEntry.OnClick()` and `DrunkenPirate.SpeechGumpEntry.OnClick()` only validate that the stored caller is a `PlayerMobile`. | Stale context-menu clicks do not re-check that the NPC still exists, is alive, is visible, or remains in range. |
| `Shipwright.OnDragDrop()` assigns `Container pack = from.Backpack` and calls `pack.ConsumeTotal(...)` without checking for a null backpack. | Edge-case callers without a backpack can throw while using the 1,000-gold return-to-boat service. |
| The tutorial text says abandoned boats last about 30 sunrises, but `MyServerSettings` defaults `S_BoatDecay` to `365.0` days and clamps only values below 5. | Player-facing guidance can be wrong unless shard configuration overrides the default decay setting to about 30 days. |
| `DockSearch.NearDock()` iterates `m.GetItemsInRange(30)` without freeing the pooled enumerable. | Repeated launch/dry-dock checks can leak pooled range enumerables. |
| `BaseBoatDeed.OnPlacement()` and `BaseDockedBoat.OnPlacement()` set `boat.Hue = hue` before checking whether `Boat` returned null. | Custom or broken deed/docked classes with a null `Boat` property can null-reference during launch. |
| `BaseBoat.EndDryDock()` sets `DockedBoat.Hue` before verifying `DockedBoat` is non-null. | Hull classes that leave `DockedBoat` unimplemented, including galleon variants, cannot safely use this dry-dock conversion path. |
| `BaseBoat.EndDryDock()` casts the dry-docking caller to `PlayerMobile` while processing lower-deck stowaways. | A non-player administrator/mobile dry-docking a boat with lower-deck players can throw. |
| `BoatNavigationControl.OnResponse()` does not enforce owner, key, or `CanCommand()` checks before controlling the boat found under the player. | Any player standing on a boat can use the optional `[bc...]` gump to operate it. |
| `BoatNavigationControl` opens planks by setting `Locked = false` and does not restore the locked state afterward. | The optional navigation gump can permanently unlock planks until another system or staff action changes them. |
