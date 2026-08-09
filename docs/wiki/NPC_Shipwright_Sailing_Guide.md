# NPC Shipwright Sailing Guide

## Scope

This page documents the shipwright sailing advice exposed through the shared `SpeechGump` dialogue system.
It covers the `Shipwright` and `DrunkenPirate` `Mobile` entry points, the `"Shipwright"` branch of `SpeechFunctions.SpeechText()`, the memorized chat entry, the shipwright return-to-boat service, and the boat mechanics described by the dialogue.

The guide is not a standalone command system. It does not register `[Command]` handlers, EventSink hooks, packet handlers, or XMLSpawner attachments.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Misc/Talk.cs` | Shared `SpeechGump` UI and `SpeechFunctions.SpeechText(..., "Shipwright")` text provider. |
| `Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs` | Shipwright vendor `Mobile`, vendor stock registration, context-menu dialogue entry, key return service, and serialization. |
| `Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs` | Stationary vendor `Mobile` that opens the same `"Shipwright"` dialogue text. |
| `Data/Scripts/System/Commands/Player/MyChat.cs` | Memorizes the `"Shipwright"` conversation as player chat entry `39`. |
| `Data/Scripts/Mobiles/Base/StoreSalesList.cs` | `SBShipwright` vendor catalog for boat deeds and sailing tools. |
| `Data/Scripts/Items/Boats/BaseBoatDeed.cs` | Fresh boat deed launch flow. |
| `Data/Scripts/Items/Boats/BaseDockedBoat.cs` | Dry-docked boat relaunch flow and ship-name persistence. |
| `Data/Scripts/Items/Boats/BaseBoat.cs` | Active boat multi, keys, movement, map-course navigation, dry docking, decay, and serialization. |
| `Data/Scripts/Items/Boats/TillerMan.cs` | Tiller double-click, map drop, and dry-dock entry point. |
| `Data/Scripts/Items/Boats/Gumps/TillerManGump.cs` | Boat control gump for direction, stop, anchor, turn, rename, and one-step mode. |
| `Data/Scripts/Items/Boats/DockingLantern.cs` | `DockingLantern` item and `DockSearch.NearDock()` dock/lantern/lawn-pier checks. |

## Dialogue Entry Points

| Entry point | Behavior |
| --- | --- |
| `Shipwright.GetContextMenuEntries()` | Adds `SpeechGumpEntry(from, this)` after the base vendor context menu. |
| `DrunkenPirate.GetContextMenuEntries()` | Adds the same `SpeechGumpEntry(from, this)` pattern. |
| Caller gate | `OnClick()` returns unless the stored caller is a `PlayerMobile`. |
| Duplicate gump gate | If the player already has a `SpeechGump`, no new guide gump is sent. |
| Greeting | `IntelligentAction.SayHey(m_Giver)` runs before the gump opens. |
| Gump title | `The High Seas`. |
| Text source | `SpeechFunctions.SpeechText(m_Giver, m_Mobile, "Shipwright")`. |
| Memorized chat | `SpeechText()` calls `MyChat.speechText("Shipwright", from)`, which maps to chat entry `39`. |

`SpeechGump` builds a single scrollable HTML panel. Its close button submits button `0`, and `OnResponse(NetState sender, RelayInfo info)` only plays sound `0x4A`.

## NPC Profiles

| Mobile | Vendor title | Guild | Skills | Vendor catalogs |
| --- | --- | --- | --- | --- |
| `Shipwright` | `the shipwright` | `NpcGuild.FishermensGuild` | `Carpentry` 60.0-83.0, `Bludgeoning` 36.0-68.0, `Seafaring` 75.0-98.0 | `SBShipwright`, `SBSailor`, `SBHighSeas`, `SBBuyArtifacts` |
| `DrunkenPirate` | `the drunken pirate` | `NpcGuild.FishermensGuild` | `Carpentry` 60.0-83.0, `Seafaring` 75.0-98.0, `Cartography` 90.0-100.0 | `SBShipwright`, `SBFisherman`, `SBSailor`, `SBHighSeas`, `RSScrolls`, `SBMapmaker`, `SBBuyArtifacts` |

## Dialogue Topics

The `"Shipwright"` text is one static HTML string built from the player name and NPC name.

| Topic | Code-backed content |
| --- | --- |
| Ship deeds | The dialogue says a purchased ship is carried as a deed or dry-docked ship item and launched into water. |
| Dock requirement | Non-carpet launches and dry docks require `DockSearch.NearDock(from)` unless the caller qualifies through a special region or `Seafaring.Base >= 100.0`. |
| Keys | Successful launch creates one ship key in the caller's bank box and one in the caller's backpack or at the caller's feet if backpack insertion fails. |
| Recall key | Keys link to the `BaseBoat`, and the dialogue describes using recall magic on that key. |
| Shipwright return service | Dropping a valid linked ship key on a `Shipwright` can teleport the player to the boat for `1,000` gold. |
| Docking lanterns | A secured `DockingLantern` can satisfy dock proximity near a house when house access and secure access pass. |
| Boat hue | The dialogue says dyed docked boats keep their color. Launch code copies the deed or docked item hue to the active boat, tiller man, hold, and planks. |
| Tiller control | The owner or an administrator double-clicks the `TillerMan`; on deck this opens `TillerManGump`, off deck it attempts dry docking. |
| Sea charts | Dropping a pinned `MapItem` onto the tiller associates the course; speech commands then start or continue navigation. |
| Decay warning | The dialogue says abandoned boats last about 30 sunrises, but the default `MyServerSettings.S_BoatDecay` is `365.0` days and `BoatDecay()` only clamps values below `5.0`. |

## Shipwright Vendor Stock

`SBShipwright.InternalBuyInfo` adds most stock behind `MyServerSettings.SellChance()` and adds `DockingLantern` behind `SellRareChance()`. The default settings are `50%` regular sell chance and `25%` rare sell chance, each rolled against `Utility.RandomMinMax(1, 100)`.

| Item | Price | Quantity | Chance helper |
| --- | ---: | ---: | --- |
| `SmallBoatDeed` | 10,000 | random 1 to 15 | `SellChance()` |
| `SmallDragonBoatDeed` | 11,000 | random 1 to 15 | `SellChance()` |
| `MediumBoatDeed` | 12,000 | random 1 to 15 | `SellChance()` |
| `MediumDragonBoatDeed` | 13,000 | random 1 to 15 | `SellChance()` |
| `LargeBoatDeed` | 14,000 | random 1 to 15 | `SellChance()` |
| `LargeDragonBoatDeed` | 15,000 | random 1 to 15 | `SellChance()` |
| `DockingLantern` | 58 | random 1 to 15 | `SellRareChance()` |
| `Sextant` | 13 | random 1 to 15 | `SellChance()` |
| `GrapplingHook` | 58 | random 1 to 15 | `SellChance()` |
| `BoatStain` | 26 | random 1 to 15 | `SellChance()` |

## Return-To-Boat Service

`Shipwright.OnDragDrop(Mobile from, Item o)` handles ship keys dropped onto a shipwright.

| Gate | Behavior |
| --- | --- |
| Item gate | The dropped item must be a `Key` with nonzero `KeyValue`, and `Key.Link` must be a `BaseBoat`. |
| Boat/key gate | The linked boat must not be deleted, and `boat.CheckKey(key.KeyValue)` must match either plank key. |
| Cost | The caller's backpack must consume `1,000` `Gold`. |
| Destination | `ReturnToBoat(boat.GetMarkedLocation(), boat.Map, from)` uses the boat's facing-adjusted mark location. |
| Success effects | The player pays, pets teleport, recall particles play at source and destination, and the player moves to the boat mark location. |

`ReturnToBoat()` applies recall-style travel checks from the caller's current location, escape rules, origin recall-region rules, destination teleport-region rules, overweight blocking, and `map.CanSpawnMobile(loc.X, loc.Y, loc.Z)`.

## Launch And Dry Dock

`BaseBoatDeed` and `BaseDockedBoat` use parallel launch flows.

| Gate | Behavior |
| --- | --- |
| Backpack | The deed or docked boat item must be in the caller's backpack. |
| Dock proximity | Non-carpet boats require `DockSearch.NearDock(from)` before targeting and again during placement. |
| Current region | Launch is allowed from sea towns or a set of outdoor/public/main-region checks. |
| Targeted region | The target point cannot be in `DungeonRegion`, `HouseRegion`, or `ChampionSpawnRegion`. |
| Caller position | Placement fails if the caller is in a `HouseRegion` or already on another boat. |
| Water and collision | `BaseBoat.IsValidLocation(p, map)` and `boat.CanFit(p, map, boat.ItemID)` must pass. |

On successful launch, the server deletes the carried item, sets `boat.Owner = from`, starts the boat anchored, creates and assigns a matching key value, applies hue to boat components, and moves the boat multi to the target map. `BaseDockedBoat` also restores the preserved `ShipName`.

Dry docking starts from `TillerMan.OnDoubleClick()` or `OnDoubleClickDead()`. Owner/admin callers on the boat open the control gump; owner/admin callers off the boat try to dry dock. `BaseBoat.CheckDryDock()` requires a live caller, a matching key in backpack or bank, lowered anchor, empty hold, valid map, and no extra items or mobiles inside the boat bounds. `ConfirmDryDockGump` calls `BaseBoat.EndDryDock()`, which removes keys, adds the docked boat item to the caller's backpack, plays a sound, and deletes the active boat.

## Dock Search

`DockSearch.NearDock(Mobile m)` returns true for these conditions:

| Condition | Details |
| --- | --- |
| Region or world exemption | Ambrosia, sea towns, Isle of the Lich, Island of Poseidon, Buccaneer's Den, Underworld, Island of the Black Knight, or Island of Stonegate. |
| Skill exemption | `Seafaring.Base >= 100.0`. |
| Docking lantern | A non-movable `DockingLantern` within 30 tiles, placed in a house the caller can access, with secure access at the lantern's `SecureLevel`. |
| Lawn pier | A `LawnItem` with item ID `942`, `20403`, or `20404` in an accessible house. |
| Lawn pier piece | A `LawnPiece` with item ID `0x1AD0` whose parent `LawnItem` has an accessible house. |

## Tiller And Navigation

`TillerManGump` is a visual control surface for a `BaseBoat`.

| Control | Method called |
| --- | --- |
| Direction buttons | `StartMove(direction, true)` or `OneMove(direction)` when one-step mode is active. |
| Stop | `StopMove(true)`. |
| Rename | `BeginRename(m_From)`. |
| One step | Toggles the gump-local `ToggleOneStep` flag and reopens the gump. |
| Anchor | `RaiseAnchor(true)` if anchored, otherwise `LowerAnchor(true)`. |
| Turn left/right | `StartTurn(-2, true)` or `StartTurn(2, true)`. |
| Come about | `StartTurn(-4, true)`. |

`BaseBoat.OnSpeech()` also handles tiller speech keywords from any mobile for which `CanCommand(from)` and `Contains(from)` pass. In this codebase, `CanCommand(Mobile m)` returns `true`, so speech control is effectively gated by being on the boat.

For sea-chart navigation, a mobile on the boat drops a non-blank `MapItem` with pins onto the tiller. `AssociateMap()` stores the map and resets `NextNavPoint` to `-1`. Speech commands then drive the course:

| Speech command group | Behavior |
| --- | --- |
| `start` | Sets `NextNavPoint = 0` and starts following the full course. |
| `continue` | Starts or resumes the current course. |
| `goto #` | Selects a numbered pin and follows the course from there. |
| `single #` | Selects a numbered pin and travels only to that pin. |

## Serialization

| Type | Version | Serialized fields after version | Notes |
| --- | ---: | --- | --- |
| `Shipwright` | 0 | None | Standard `BaseVendor` data only. |
| `DrunkenPirate` | 0 | None | Standard `BaseVendor` data only. |
| `DockingLantern` | 0 | `SecureLevel` as `int` | Restores house secure-level gate. |
| `BaseBoatDeed` | 0 | `MultiID`, `Offset` | Restores launch multi and offset, then normalizes default hue. |
| `BaseDockedBoat` | 1 | `MultiID`, `Offset`, `ShipName` | Version 0 additionally consumes an old `uint`; load normalizes blessed loot type and hue. |
| `BaseBoat` | 3 | Map item, next nav point, facing, decay delta, owner, planks, tiller man, hold, boat door, anchored flag, ship name | Older versions initialize missing navigation/facing/decay data. |
| `TillerMan` | 0 | Boat reference | Deletes itself if the boat reference is missing during load. |

## Admin Commands

None. The traced dialogue and shipwright return service do not register admin commands.

Staff can inspect and edit exposed `[CommandProperty]` values on related objects such as `DockingLantern.Level`, boat component references, boat owner, facing, anchor state, and ship name.

## Known Issues

| Issue | Impact |
| --- | --- |
| `SpeechGump.OnResponse()` dereferences `sender.Mobile` and calls `from.SendSound()` without checking `sender` or `sender.Mobile`. | A malformed or stale `NetState` response can throw in the shared speech gump close path. |
| `SpeechFunctions.SpeechText()` reads `from.Name`, `m.Name`, and calls `MyChat.speechText()` before validating either `Mobile`. | Passing a null NPC or player can throw before any text is returned. |
| `Shipwright.SpeechGumpEntry.OnClick()` and `DrunkenPirate.SpeechGumpEntry.OnClick()` only check that the stored caller is a `PlayerMobile`. | Stale context-menu clicks do not revalidate range, visibility, liveness, deletion state, or NPC availability. |
| `Shipwright.OnDragDrop()` calls `from.Backpack.ConsumeTotal(...)` through a local `pack` variable without a null backpack guard. | Edge-case mobiles without backpacks can throw when using the return-to-boat service. |
| `Shipwright.OnDragDrop()` consumes `1,000` gold before `ReturnToBoat()` verifies all travel and destination gates. | A player can be charged even when recall, region, weight, or spawn checks later block the teleport. |
| The dialogue says abandoned boats last about 30 sunrises, but the default code setting is `365.0` days. | Player-facing guidance can be wrong unless shard configuration overrides boat decay. |
| `DockSearch.NearDock()` iterates `m.GetItemsInRange(30)` without freeing the pooled enumerable. | Repeated dock checks can leak pooled range enumerables. |
| `BaseBoatDeed.OnPlacement()` and `BaseDockedBoat.OnPlacement()` set `boat.Hue = hue` before checking whether `Boat` returned null. | Broken or custom deed classes with null `Boat` properties can null-reference during launch. |
| `BaseBoat.EndDryDock()` sets `DockedBoat.Hue` before checking for a null docked boat item. | Hull classes without a valid `DockedBoat` implementation cannot safely dry dock. |
| `BaseBoat.CanCommand(Mobile m)` returns `true`, and `BaseBoat.OnSpeech()` accepts commands from any contained mobile. | Any mobile physically on the boat can issue tiller speech commands, not only the owner or key holder. |
| `TillerMan.OnDoubleClickDead()` opens `TillerManGump` for dead owners/admins on the boat, and `TillerManGump.OnResponse()` does not check `Alive`. | Dead owners/admins can still operate the visual boat controls while onboard. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0004; PBN-0125.
- Backlog rows: RB-06732; RB-06733.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Misc/Talk.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs (CurrentFile)
- Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/MyChat.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/StoreSalesList.cs (CurrentFile)
- Data/Scripts/Items/Boats/BaseBoatDeed.cs (CurrentFile)
- Data/Scripts/Items/Boats/BaseDockedBoat.cs (CurrentFile)
- Data/Scripts/Items/Boats/BaseBoat.cs (CurrentFile)
- Data/Scripts/Items/Boats/TillerMan.cs (CurrentFile)
- Data/Scripts/Items/Boats/Gumps/TillerManGump.cs (CurrentFile)
- Data/Scripts/Items/Boats/DockingLantern.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Event=1; Gump=15; Initialize=1; Speech=1; Timer=4; WorldSave=1.
- Data/Scripts/Items/Boats/BaseBoat.cs:L1447 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L1732 Gump SendGump access=Internal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2058 Speech OnSpeech access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2270 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2882 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2906 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2909 Event EventSink access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2909 WorldSave WorldSave access=GlobalOrInternal
- Data/Scripts/Items/Boats/BaseBoat.cs:L2917 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Items/Boats/Gumps/TillerManGump.cs:L56 Gump OnResponse access=Internal
- Data/Scripts/Items/Boats/Gumps/TillerManGump.cs:L503 Gump SendGump access=Internal
- Data/Scripts/Items/Boats/TillerMan.cs:L112 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 7.
- Data/Scripts/Items/Boats/BaseBoat.cs:Server.Multis.BaseBoat version=3 serialize=L663 deserialize=L688 alignment=CountMatchNeedsTypeReview:UnknownWrites=8
- Data/Scripts/Items/Boats/BaseBoatDeed.cs:Server.Multis.BaseBoatDeed version=0 serialize=L56 deserialize=L64 alignment=CountMatchNeedsTypeReview:UnknownWrites=2
- Data/Scripts/Items/Boats/BaseDockedBoat.cs:Server.Multis.BaseDockedBoat version=1 serialize=L71 deserialize=L82 alignment=CountMismatch:Writes=3;Reads=4
- Data/Scripts/Items/Boats/DockingLantern.cs:Server.Items.DockingLantern version=0 serialize=L74 deserialize=L81 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Boats/TillerMan.cs:Server.Items.TillerMan version=0 serialize=L174 deserialize=L183 alignment=CountMatchNeedsTypeReview:UnknownWrites=1
- Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs:Server.Mobiles.DrunkenPirate version=0 serialize=L99 deserialize=L106 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs:Server.Mobiles.Shipwright version=0 serialize=L188 deserialize=L195 alignment=AlignedByCountAndKnownTypes

### Project And Runtime Coverage

- Data/Scripts/Items/Boats/BaseBoat.cs=Keep
- Data/Scripts/Items/Boats/BaseBoat.cs=Keep
- Data/Scripts/Items/Boats/BaseBoatDeed.cs=Keep
- Data/Scripts/Items/Boats/BaseBoatDeed.cs=Keep
- Data/Scripts/Items/Boats/BaseDockedBoat.cs=Keep
- Data/Scripts/Items/Boats/BaseDockedBoat.cs=Keep
- Data/Scripts/Items/Boats/DockingLantern.cs=Keep
- Data/Scripts/Items/Boats/DockingLantern.cs=Keep
- Data/Scripts/Items/Boats/Gumps/TillerManGump.cs=Keep
- Data/Scripts/Items/Boats/Gumps/TillerManGump.cs=Keep
- Data/Scripts/Items/Boats/TillerMan.cs=Keep
- Data/Scripts/Items/Boats/TillerMan.cs=Keep
- Data/Scripts/Mobiles/Base/StoreSalesList.cs=Keep
- Data/Scripts/Mobiles/Base/StoreSalesList.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/DrunkenPirate.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs=Keep
- Data/Scripts/Mobiles/Civilized/Vendors/Shipwright.cs=Keep
- Data/Scripts/System/Commands/Player/MyChat.cs=Keep
- Data/Scripts/System/Commands/Player/MyChat.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
