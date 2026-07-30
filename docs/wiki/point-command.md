# System Name: Point Command

### Overview
The Point Command is a player-access emote utility. It lets a player target a mobile, item, or any other `IPoint3D` target and produces overhead emote text at the caller and the selected target.

This system does not award points, track score, grant rewards, or use XMLSpawner. Its only custom script is `Data/Scripts/Custom/Point Command [2.0]/Point.cs`.

---

### Command Flow
* **Command:** `[Point`
* **Access Level:** Player
* **Arguments:** None
* **Handler:** `Point_OnCommand`

When a player uses `[Point`, the command assigns `from.Target = new PointTarget()`. The selected target is then handled in `PointTarget.OnTarget`.

---

### Target Outcomes
| Target Type | Code Behavior |
| :--- | :--- |
| Mobile with a name | The player emits `*<player> Points at*`; the target mobile emits `*<target name>*`. |
| Mobile without a name | The player emits `*<player> Points at*`; the target mobile emits `*<player> whatever it is!*`. |
| Item with a name | The player emits `*<player> Points at*`; the item emits `* <item name>*`. |
| Item without a name | The player emits `*<player> Points at*`; the item emits `*Points Here*`. |
| Any `IPoint3D` target | The command creates a temporary marker item at the targeted coordinates on the caller's current map and emits `*This Spot*` over that marker. |
| Unsupported target | The player receives `Cannot point at this for some reason!`. |

The command first checks `from.Name == null`. If the player has no name, it sends `Your name is not valid fix it now` and stops.

---

### Temporary Pointer Item
When the selected target is an `IPoint3D` rather than a mobile or item, the command:

1. Creates a generic `Item(8302)`.
2. Converts the target to `Point3D`.
3. Moves the marker to that point on the player's current map.
4. Sets `Movable = false`.
5. Starts `PointTimer`.
6. Emits `*<player> Points at*` over the player and `*This Spot*` over the marker.

`PointTimer` runs once after five seconds, deletes the marker item, and stops itself.

The script does not validate map-specific placement beyond using `from.Map`, and it does not serialize the marker because the timer deletes it after one tick.

---

### Unused Constructor
`PointTarget` also declares a second constructor that takes `(Mobile from, Item targeted)`. That overload immediately emits `"<player> Points There"` and `"<player> Points Here"` style messages, but nothing in `Point.cs` calls it. The live command path always uses the parameterless constructor and `OnTarget`.

---

### Technical Trace
* Wiki claim: `[Point` starts a targeting cursor -> traced `Point.Initialize` and `Point_OnCommand` -> code registers the `Point` command for `AccessLevel.Player` and assigns `from.Target = new PointTarget()`.
* Wiki claim: the system points at mobiles, items, and locations -> traced `PointTarget.OnTarget` -> code has explicit branches for `Mobile`, `Item`, and any remaining object castable to `IPoint3D`.
* Wiki claim: pointing at a mobile shows emotes -> traced the mobile branch -> code emits `*<player> Points at*` over the caller and then either `*<target name>*` or `*<player> whatever it is!*` over the target.
* Wiki claim: pointing at an item shows the item's name -> traced the item branch -> code emits `* <item name>*` when `Item.Name` is not null, including the leading space inside the asterisks.
* Wiki claim: pointing at the ground creates a short-lived marker -> traced the `IPoint3D` branch and `PointTimer` -> code creates generic item `8302`, places it on `from.Map`, makes it immovable, and deletes it after 5 seconds.
* Wiki claim: the feature awards points -> traced the full script -> code contains no counters, rewards, persistence, or score mutation.

---

### Serialization
This system does not define a custom `Item`, `Mobile`, or other serializable RunUO object. The temporary marker is a generic `Item`, and `PointTimer` deletes it after five seconds.

There is no custom `Serialize` or `Deserialize` version for the Point Command system.

---

### XMLSpawner
This system does not use XMLSpawner hooks, attachments, spawn definitions, or XMLSpawner configuration references. It is driven entirely by the player command, target handling, a temporary generic item, and a one-shot timer.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0104.
- Backlog rows: RB-06745.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Point Command [2.0]/Point.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=1; Initialize=1; Timer=1.
- Data/Scripts/Custom/Point Command [2.0]/Point.cs:L13 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Point Command [2.0]/Point.cs:L15 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/Point Command [2.0]/Point.cs:L131 Timer CustomTimerSubclass access=GlobalOrInternal

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/Point Command [2.0]/Point.cs=Keep
- Data/Scripts/Custom/Point Command [2.0]/Point.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
