# System Name: Boat Navigation Console

### Overview
Provides a gump interface for steering boats, raising anchors, and managing planks with customizable backgrounds.

---

### How It Works
* **Activation:** Use `[bcblack`, `[bcwhite`, or `[bctransparent` to open the navigation console with a black, white, or transparent background.
* **Anchor controls:** Raise or lower the ship's anchor.
* **Movement:** Buttons adjust the ship's heading in cardinal or diagonal directions or nudge it one tile forward, backward, left, or right.
* **Turning:** Dedicated buttons turn the ship left, right, or completely around.
* **Plank controls:** Extend the port or starboard boarding planks.
* **Stop/Close:** Stop the ship's movement or close the console.
* **Success/Failure Conditions:** Actions only occur if you are alive and standing on a boat.

---

### Getting Started
1. Stand aboard your boat.
2. Type `[bcblack`, `[bcwhite`, or `[bctransparent` for your preferred background.
3. Use the buttons to steer, turn, or manage planks as needed.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| Player-controlled boat | Vessel affected by the console's commands. |

---

### Player Commands
* `[bcblack`: Opens the console with a black background.
* `[bcwhite`: Opens the console with a white background.
* `[bctransparent`: Opens the console with a transparent background.

---

### Technical Deep Dive (Optional)
* **Available Buttons:** RaiseAnchor, LowerAnchor, NorthWest, SouthEast, SouthWest, NorthEast, North, South, West, East, PPlankControl, SPlankControl, Stop, Close, TurnRight, NorthEastOne, SouthWestOne, NorthWestOne, SouthEastOne, TurnLeft, TurnAround, ForwardOnCurrentHeading, BackwardOnCurrentHeading.