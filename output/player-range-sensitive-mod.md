# System Name: Player Range Sensitive Deactivation

### Overview
Administrators can configure how long NPCs remain active after players move out of range.

---

### How It Works
* **Command:** `[SetDeactivation` adjusts the global AI deactivation delay.
* **Default:** 20 minutes if no value is set.
* **Usage:** `[SetDeactivation 30` sets the delay to 30 minutes.
* **Success/Failure Conditions:** Command accepts numeric minutes; other values are ignored.

---

### Getting Started
1. Log in with Administrator access.
2. Issue `[SetDeactivation <minutes>`.
3. Receive confirmation of the new delay.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| None | Global setting only. |

---

### Player Commands
* `[SetDeactivation`: Sets or reports the default AI deactivation delay.

---

### Technical Deep Dive (Optional)
* **Default Delay:** 20 minutes.
* **Command Access Level:** Administrator.
