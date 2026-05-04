# System Name: Point Command

### Overview
The Point Command is a player-access emote utility. It lets a player target a mobile, item, or map location and produce pointing overhead messages.

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
| Map location / `IPoint3D` | The command creates a temporary marker item and emits location-pointing overhead messages. |
| Unsupported target | The player receives `Cannot point at this for some reason!`. |

The command first checks `from.Name == null`. If the player has no name, it sends `Your name is not valid fix it now` and stops.

---

### Temporary Pointer Item
When the selected target is an `IPoint3D` location rather than a mobile or item, the command:

1. Creates a generic `Item(8302)`.
2. Moves it to the targeted `Point3D` on the player's current map.
3. Sets `Movable = false`.
4. Starts `PointTimer`.
5. Emits `*<player> Points at*` over the player and `*This Spot*` over the marker.

`PointTimer` runs once after five seconds, deletes the marker item, and stops itself.

---

### Serialization
This system does not define a custom `Item`, `Mobile`, or other serializable RunUO object. The temporary marker is a generic `Item`, and the `PointTimer` deletes it after five seconds.

There is no custom `Serialize` or `Deserialize` version for the Point Command system.

---

### Audit Notes
* Wiki claimed `[Point` starts targeting -> traced `Point.Initialize` and `Point_OnCommand` -> code registers `Point` for players and assigns `from.Target = new PointTarget()`.
* Wiki claimed pointing at creatures displays overhead emotes -> traced the mobile branch in `PointTarget.OnTarget` -> code emits `*<player> Points at*` over the player and either the mobile's name or `*<player> whatever it is!*` over the target.
* Wiki claimed pointing at items displays the item name or `Points Here` -> traced the item branch -> code emits the item name when present, otherwise `*Points Here*`.
* Wiki claimed pointing at locations creates a temporary pointer for five seconds -> traced the `IPoint3D` branch and `PointTimer` -> code creates generic `Item(8302)`, makes it immovable, and deletes it after five seconds.
* Wiki claimed success/failure requires a valid player name -> traced the guard -> code only checks `from.Name == null`; the command accepts no name argument.
* Ledger claimed this system awards points -> traced the compiled script -> code contains no points, rewards, or score mutation.
