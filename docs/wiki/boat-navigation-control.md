# Boat Navigation Control

## Summary

`BoatNavigationControl` is a player command gump implemented in
`Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs`. It is not a
placeable totem item. The three commands open the same control panel with
different background art, then `OnResponse` finds the `BaseBoat` under the
caller and issues direct boat or plank calls.

Code-verified against:

- `Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs`
- `Data/Scripts/Items/Boats/BaseBoat.cs`
- `Data/Scripts/Items/Boats/Plank.cs`

Audit pass date: 2026-05-06.

## Player Access

1. Stand on any `BaseBoat`.
2. Type `[bcblack`, `[bcwhite`, or `[bctransparent`.
3. Use the gump buttons to anchor, move, turn, stop, or open planks.

The background choice is per-gump only. It is not stored on the character,
account, boat, or world state.

## Verified Behavior

### Commands and Gump Lifecycle

- `Initialize()` registers `bcblack`, `bcwhite`, and `bctransparent` at
  `AccessLevel.Player`.
- Each command closes any existing `BoatNavigationControl` gump on the caller,
  then sends a new gump with the selected `BackgroundType`.
- The file and folder names are misspelled as `BoatNavigationTotam.cs` inside
  `BoatNavigationTotem`, but the live gump class is `BoatNavigationControl`.

### Boat Selection and Access Rules

- `OnResponse` resolves the controlled ship with
  `BaseBoat.FindBoatAt(from, from.Map)`.
- `FindBoatAt` returns the first `BaseBoat` in the caller's sector whose
  footprint contains the caller's coordinates.
- The gump does not check `BaseBoat.Owner`, ship keys, party status, or any
  other access rule before issuing commands.
- In the current boat core, `BaseBoat.CanCommand(Mobile m)` always returns
  `true`, but this gump does not call `CanCommand` anyway.
- Dead users are not fully blocked. The first guard is
  `if (!from.Alive && boat == null)`, so a dead player standing on a boat can
  still operate the gump.

### Anchor Buttons

- Raise anchor says `Raise The Anchor!`, calls `boat.RaiseAnchor(true)`, then
  reopens the same gump background.
- Lower anchor says `Lower The Anchor!`, calls `boat.LowerAnchor(true)`, then
  reopens the gump.
- `BaseBoat.StartMove(...)` and `BaseBoat.StartCourse(...)` still refuse to move
  while `Anchored` is true, so the gump cannot bypass anchor checks.

### Movement Buttons

- The main heading buttons try to face the boat toward a world direction, then
  call `StartMove(...)`.
- `North`, `South`, `West`, and `East` all rotate the hull as needed, then call
  `StartMove(Direction.North, true)`, which is boat-relative forward movement
  after the turn.
- `NorthWest` and `SouthEast` use `Direction.Up` or `Direction.Right`
  depending on current facing, sometimes chosen with `Utility.Random(2)`.
- `NorthEast` and `SouthWest` likewise mix `Direction.Right` and
  `Direction.Up` after turning to one of two adjacent facings.
- The diagonal buttons are not precise world-diagonal pathing. They choose one
  of two nearby headings and drift directions based on the current facing.

### One-Step Buttons

- The four one-step buttons call `boat.OneMove(...)`.
- Those calls use boat-relative aliases from `BaseBoat`, not absolute world
  directions:
- `OneMove(Direction.North)` means one tile forward.
- `OneMove(Direction.South)` means one tile backward.
- `OneMove(Direction.East)` means one tile starboard/right drift.
- `OneMove(Direction.West)` means one tile port/left drift.

### Turn, Stop, and Current-Heading Buttons

- Turn right calls `boat.Turn(90, true)`.
- Turn left calls `boat.Turn(-90, true)`.
- Turn around calls `boat.Turn(180, true)`.
- Stop calls `boat.StopMove(true)`.
- The current-heading forward button calls `boat.StartMove(Direction.North,
  false)`.
- The current-heading backward button calls `boat.StartMove(Direction.South,
  false)`.
- Those two buttons use the slower `fast = false` movement path.

### Plank Buttons

- Port plank says `Extend The Port Plank!!`, calls `boat.PPlank.Open()`, then
  sets `boat.PPlank.Locked = false`.
- Starboard plank says `Extend The Starboard Plank!!`, calls
  `boat.SPlank.Open()`, then sets `boat.SPlank.Locked = false`.
- `Plank.Close()` changes the art back to a closed plank but does not restore
  `Locked = true`.
- `Plank` serialization writes version `0`, then persists `m_Boat`, `m_Side`,
  `m_Locked`, and `m_KeyValue`, so the unlocked state survives saves.

## Technical Trace

- Wiki claimed the system is a command-opened navigation console, not a world
  item -> traced `Initialize`, the three command handlers, and the constructor
  -> code opens a `Gump` directly and never creates or requires a navigation
  item.
- Wiki claimed the gump only works while alive and on a boat -> traced
  `BoatNavigationControl.OnResponse` -> code only rejects when no boat is found;
  the dead-player check does not block a dead caller already standing on a
  boat.
- Wiki claimed the caller controls their own ship -> traced
  `BaseBoat.FindBoatAt` and `BaseBoat.CanCommand` -> code uses the boat under
  the caller and has no owner or key restriction in this gump path.
- Wiki claimed the main compass buttons set headings -> traced the per-button
  switch cases and `BaseBoat.StartMove` -> code rotates the hull, then uses
  boat-relative movement directions to travel on the new heading.
- Wiki claimed the one-step buttons are directional nudges -> traced
  `BoatNavigationControl` one-step cases and `BaseBoat` direction aliases ->
  code maps them to relative forward, backward, left, and right drift moves.
- Wiki claimed plank buttons extend planks -> traced `Plank.Open`,
  `Plank.Close`, and `Plank.Serialize` -> code opens the plank, clears
  `Locked`, and persists that unlock.

## Persistence

- `BoatNavigationControl` itself has no `Serialize` or `Deserialize` methods.
- `BaseBoat` serializes version `3`, including `MapItem`, `NextNavPoint`,
  `Facing`, decay time, owner, both planks, tiller man, hold, optional boat
  door, anchor state, and ship name.
- `Plank` serializes version `0`, including its boat reference, side, locked
  flag, and key value.

## XMLSpawner

This system has no XMLSpawner hooks, attachments, or XML configuration. It is a
plain command-plus-gump control surface.

## Known Code Issues

- Dead players standing on a boat can still use the console.
- The console does not enforce ownership, keys, or any other access check.
- Plank buttons clear `Locked`, and normal plank closing does not relock the
  plank.
- The gump exposes only direct helm controls. It does not expose the map-course
  features that `BaseBoat` supports through speech keywords such as `goto` and
  `single`.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0003.
- Backlog rows: RB-06667.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs (CurrentFile)
- Data/Scripts/Items/Boats/BaseBoat.cs (CurrentFile)
- Data/Scripts/Items/Boats/Plank.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=3; Event=1; Gump=27; Initialize=2; Speech=1; Timer=5; WorldSave=1.
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L17 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L19 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L24 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L29 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L43 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L53 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L63 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L197 Gump OnResponse access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L219 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L227 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L300 Gump SendGump access=Internal
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs:L373 Gump SendGump access=Internal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 2.
- Data/Scripts/Items/Boats/BaseBoat.cs:Server.Multis.BaseBoat version=3 serialize=L663 deserialize=L688 alignment=CountMatchNeedsTypeReview:UnknownWrites=8
- Data/Scripts/Items/Boats/Plank.cs:Server.Items.Plank version=0 serialize=L47 deserialize=L59 alignment=CountMatchNeedsTypeReview:UnknownWrites=3

### Project And Runtime Coverage

- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs=Keep
- Data/Scripts/Custom/BoatNavigationTotem/BoatNavigationTotam.cs=Keep
- Data/Scripts/Items/Boats/BaseBoat.cs=Keep
- Data/Scripts/Items/Boats/BaseBoat.cs=Keep
- Data/Scripts/Items/Boats/Plank.cs=Keep
- Data/Scripts/Items/Boats/Plank.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
