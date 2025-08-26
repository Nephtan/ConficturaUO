# System Name: Point Command

### Overview
Allows players to emote pointing at creatures, items, or locations.

---

### How It Works
* **Activation:** Use the `[Point` command to start targeting.
* **Pointing at creatures:** Both you and the target display overhead emotes.
* **Pointing at items:** You emote pointing and the item displays its name, or "Points Here" if unnamed.
* **Pointing at locations:** Creates a temporary pointer on the ground for five seconds, along with overhead emotes.
* **Success/Failure Conditions:** Requires a valid player name; otherwise the command aborts.

---

### Getting Started
1. Type `[Point` in-game.
2. Target a creature, item, or spot.
3. Watch the emote or temporary marker appear.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| Pointer (item 8302) | Temporary marker created when pointing at a location. |

---

### Player Commands
* `[Point`: Targets something to display pointing emotes.

---

### Technical Deep Dive (Optional)
* **Pointer Duration:** 5 seconds.
* **Command Access Level:** Player.
