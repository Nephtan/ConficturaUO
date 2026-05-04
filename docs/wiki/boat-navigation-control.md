# System Name: Boat Navigation Control

## Overview

`BoatNavigationControl` is a `Server.Gumps.Gump` opened by player commands. It is not a physical totem or item, despite the `BoatNavigationTotem` folder name and `BoatNavigationTotam.cs` filename.

The gump finds the `BaseBoat` under the player when a button is pressed, then delegates anchor, movement, turning, stopping, and plank actions to the boat and plank APIs.

---

## Player Use

1. Stand on a `BaseBoat`.
2. Type `[bcblack`, `[bcwhite`, or `[bctransparent`.
3. Use the gump buttons to raise or lower the anchor, set movement, turn, stop, open planks, or close the gump.

The three commands choose the current gump background only. The background choice is not saved as an account, character, boat, or world setting.

---

## Code-Verified Behavior

* **Command registration:** `Initialize()` registers `bcblack`, `bcwhite`, and `bctransparent` at `AccessLevel.Player`.
* **Gump replacement:** Each command closes an existing `BoatNavigationControl` gump on the player before sending a new one.
* **Boat lookup:** `OnResponse` calls `BaseBoat.FindBoatAt(from, from.Map)` when a button is pressed.
* **Action gate:** Button actions return only when no boat is found. The compiled alive check is `if (!from.Alive && boat == null)`, followed by `else if (boat == null)`, so dead players standing on a boat are not blocked by this gump.
* **Ownership and key checks:** The gump does not check `BaseBoat.Owner`, ship keys, party membership, or `BaseBoat.CanCommand`. In the current boat core, `CanCommand(Mobile m)` returns `true`.
* **Anchor controls:** The anchor buttons say `Raise The Anchor!` or `Lower The Anchor!`, call `RaiseAnchor(true)` or `LowerAnchor(true)`, then reopen the same background gump.
* **Movement controls:** Cardinal and diagonal buttons usually rotate the boat toward a world-facing heading, then call `StartMove(...)`; diagonal buttons often use `Utility.Random(2)` to choose between two adjacent heading paths.
* **One-tile controls:** The one-step buttons call `OneMove(Direction.East)`, `OneMove(Direction.West)`, `OneMove(Direction.North)`, or `OneMove(Direction.South)`. In `BaseBoat`, those direction constants are relative to the boat's current facing during movement.
* **Turning controls:** Turn right calls `Turn(90, true)`, turn left calls `Turn(-90, true)`, and turn around calls `Turn(180, true)`.
* **Stop and close:** Stop calls `StopMove(true)` and reopens the gump. Close calls `CloseGump(typeof(BoatNavigationControl))`.
* **Plank controls:** The port and starboard buttons call `Open()` on the matching plank and set `Locked = false`.

---

## Technical Trace

* Wiki claimed `[bcblack`, `[bcwhite`, and `[bctransparent` open themed consoles -> traced `Initialize`, `bcblack_OnCommand`, `bcwhite_OnCommand`, and `bctransparent_OnCommand` -> code registers all three as player commands, closes the old gump, and sends a new gump with the selected `BackgroundType`.
* Wiki claimed actions only occur while alive and standing on a boat -> traced `OnResponse` -> code finds a boat under the mobile and returns when no boat exists, but the alive check only returns when the player is dead and no boat exists.
* Wiki claimed a player-controlled boat is affected -> traced `OnResponse`, `BaseBoat.Owner`, and `BaseBoat.CanCommand` -> code does not enforce owner or key checks, and `CanCommand` currently returns `true`.
* Wiki claimed anchor, movement, turn, stop, close, and plank controls -> traced the `Buttons` enum and switch cases -> code delegates to `RaiseAnchor`, `LowerAnchor`, `StartMove`, `OneMove`, `Turn`, `StopMove`, `CloseGump`, and `Plank.Open`.
* Wiki claimed plank controls extend planks -> traced `PPlankControl`, `SPlankControl`, and `Plank.Close` -> code opens the selected plank and clears `Locked`, while `Close()` only changes the item graphic and does not relock it.
* Wiki implied direct cardinal and diagonal heading controls -> traced movement cases and `BaseBoat.Move` -> cardinal buttons rotate to the requested world-facing direction before moving relative-forward; diagonal buttons may randomly choose one of two adjacent path labels before movement.

---

## Persistence

`BoatNavigationControl` has no `Serialize` or `Deserialize` methods. It stores only the caller and selected background type while the gump instance exists.

The affected boat state is persisted by `BaseBoat` version 3, including map item, navigation point, facing, decay time, owner, planks, tiller man, hold, boat door, anchor state, and ship name.

The plank `Locked` flag changed by this gump is persisted by `Plank` serialization.

---

## XMLSpawner

This system has no XMLSpawner hooks, spawn definitions, attachments, or XMLSpawner configuration references. It is command- and gump-driven.

---

## Known Code Issues

* Dead players standing on a boat can still operate the gump because the alive check is combined with `boat == null`.
* The gump does not enforce ownership or key access before issuing boat commands.
* Plank buttons set `Locked = false`, and normal plank closing does not restore the locked state.
