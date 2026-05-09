# Boat Core Mechanics

## Overview
The core boat system is implemented by `BaseBoat` in `Data/Scripts/Items/Boats/BaseBoat.cs`. It drives standard player ships, magic carpets, NPC boats, launch deeds, docked-boat items, and the attached tiller, hold, plank, and cabin-door components. The checked-in wiki now reflects the compiled behavior of those classes instead of generic RunUO boat assumptions.

## Core Classes
- `BaseBoat` is the moving multi. It currently serializes at version `3` and stores the associated map, current navigation pin, facing, decay time, owner, planks, tiller, hold, boat door, anchored state, and ship name.
- `BaseBoatDeed` launches a fresh boat from a deed in the player's backpack.
- `BaseDockedBoat` is the dry-docked item form for boat classes that implement `DockedBoat`. It serializes at version `1` and preserves the ship name.
- `TillerMan` opens the owner/admin control gump, accepts dropped maps, and shows the boat status text.
- `Hold` is the cargo container and enforces onboard, not-moving access checks.
- `Plank` handles boarding, disembarking, key locking, and auto-close timing.
- `BoatDoor` and `DeckDoor` implement the cabin hatch and lower-deck return path for non-carpet boats.

## Who Can Control A Boat
- Spoken tiller keywords are accepted from any mobile physically on the boat because `BaseBoat.OnSpeech` only checks `CanCommand(from)` and `Contains(from)`, and `CanCommand()` always returns `true`.
- The tiller-man double-click gump is more restrictive. Only the boat owner or an administrator can use it.
- If that owner/admin is standing on the boat, double-clicking the tiller opens `TillerManGump`.
- If that owner/admin is off the boat, double-clicking the tiller attempts the dry-dock flow instead. Non-carpet boats must also be near a dock for that path.

## Launching A Boat
Launching from `BaseBoatDeed` or `BaseDockedBoat` follows the same rules:

1. The item must be in the player's backpack.
2. Non-carpet boats require `DockSearch.NearDock(from) == true`.
3. The caller cannot be in a house or already standing on another boat.
4. The targeted placement point cannot be in a dungeon, house region, or champion-spawn region.
5. The caller's current region must be one of the allowed outdoor/sea/public region groups checked in `OnDoubleClick()` and `OnPlacement()`.
6. `BaseBoat.IsValidLocation()` and `boat.CanFit(...)` must both succeed.

When placement succeeds, the boat:
- sets `Owner = from`
- starts with `Anchored = true`
- copies the stored ship name when launched from a docked-boat item
- creates one ship key for the bank box and one for the backpack
- hues the tiller, hold, and planks to match the launched boat
- reveals the `BoatDoor` only when the launched object is not a carpet and the owner has at least `90.0` base `Seafaring`

## Boat Components
### Tiller Man
- Shows the boat wear/decay status in its property list.
- Accepts a `MapItem` dropped on it and passes that map to `AssociateMap(...)`.
- Uses the ship name for its single-click and property label when the boat has been named.

### Hold
- Can only be opened, dragged into, lifted from, or otherwise used by someone physically on the boat.
- All hold interactions are blocked while the boat is moving.
- Default cargo weight is `1000`.
- Weight limits are raised for larger hulls: `1800` for `MediumBoat`, `2600` for `LargeBoat`, `1400` for `SmallDragonBoat`, `2200` for `MediumDragonBoat`, and `3200` for `LargeDragonBoat`.

### Planks
- Newly created planks start locked unless their key value is `0`.
- Double-clicking from onboard toggles the plank open/closed.
- Double-clicking from off-boat requires an unlocked plank or `GameMaster` access.
- Open planks auto-close on a 15-second timer if nothing is blocking the tile.
- Walking across an open plank while already aboard tries to place the mobile on nearby valid land up to 6 tiles away and teleports pets with the player.

### Cabin Door
- Normal boats create a hidden `BoatDoor`; carpets do not.
- Using the hatch stores a per-player `CharacterBoatDoor` token and teleports the player to `Map.Sosaria` at `(3254, 3477, 30)`, the "Ship's Lower Deck" interior.
- `DeckDoor` uses that saved token to return the player to the matching hatch if it still exists in the same world.

## Speech Commands And Navigation
The speech parser supports more than the short list in the old wiki. The compiled keyword table includes:
- rename commands: set name, remove name, report name
- continuous movement: forward, backward, left, right, and the four diagonals at fast or slow speed
- one-tile movement: one-step left/right/forward/backward and diagonals
- turning: turn right, turn left, come about
- state commands: stop, drop anchor, raise anchor, report current nav point
- course commands: `start`, `continue`, `goto #`, and `single #`

Map-course behavior is stricter than the old page implied:
- The associated map must be a real `MapItem`, not a `BlankMap`.
- The map must contain at least one pin.
- The map must remain on the same map as the boat and physically inside the boat bounds.
- `goto #` and `single #` both parse the first digits found in the spoken phrase.
- `start` always resets `NextNavPoint` to `0`.
- `continue` resumes the current `NextNavPoint`.
- `single #` stops after reaching the chosen nav point.
- `start`, `continue`, and `goto #` run a full course until pins are exhausted.

## Movement Model
- `StartMove(...)` and `Turn(...)` refuse to run while anchored.
- Fast forward speed is `3` tiles per tick at `0.75s` intervals.
- Drift and backward styles use speed `1`, with slow drift using a `1.5s` interval.
- Turning is delayed by a `0.5s` timer before `SetFacing(...)` executes.
- `Move(...)` checks water fit, visible blocking items, and other multis before moving.
- Boats wrap inside hard-coded sea rectangles for Lodoria, Serpent Island, Ambrosia, Isles of Dread, Bottle World of Kuldar, Umber Veil, and the default sea rectangle.
- `Teleport(...)` moves mobiles and visible deck items with the hull.

## Decay, Deletion, And Sinking
- `Refresh()` resets decay to `DateTime.Now + TimeSpan.FromDays(MyServerSettings.BoatDecay())`.
- The decay timer is refreshed when the boat or planks refresh. Lowering the anchor does not refresh decay.
- `CheckDecay()` only starts decay when the boat is not moving and `DateTime.Now >= m_DecayTime`.
- Normal age decay starts a `DecayTimer` that lowers the boat one Z every 5 seconds for five ticks, then deletes the boat.
- `OnAfterDelete()` deletes the tiller, hold, boat door, planks, and active move/turn timers.
- Shipwreck generation is not part of ordinary age decay. It exists in separate helper methods such as `CreateSunkenShip(...)` and `SinkShip(...)` for other systems to call.

## Dry Docking
- `CheckDryDock(...)` requires the caller to be alive, possess a matching ship key in backpack or bank, have the anchor lowered, have an empty hold, and have no extra items or mobiles on deck.
- Successful dry-docking removes the owner's keys, adds the `DockedBoat` item to the backpack, and deletes the active boat.
- The docked item preserves the boat hue and ship name for standard dry-dockable hulls.

## Known Code Issue
- `BaseBoat.EndDryDock()` writes `boat.Hue = hue;` before verifying that `DockedBoat` is non-null. Boat classes that leave `DockedBoat` unimplemented, such as `TinyBoat` and the galleon variants, cannot safely use the dry-dock conversion path.

## Audience
Players and Staff
