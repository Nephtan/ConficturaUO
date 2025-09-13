# System Name: Bond Info Command

### Overview
Allows players to check how long remains until one of their pets bonds with them.

---

### How It Works
* **Activation:** Use the `[BondInfo` command to target one of your pets.
* **Bonded pets:** Displays "Bonded" if the pet is already bonded.
* **Not begun:** If bonding hasn't started, you're told to feed the pet and try again.
* **Timing:** Shows the start time and the remaining days, hours, and minutes until bonding completes.
* **Success/Failure Conditions:** Only works on creatures you control; targeting others or non-pets prompts you again.

---

### Getting Started
1. Type `[BondInfo`.
2. Target the pet you want to check.
3. Read the bonding status and remaining time.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| Any controlled pet | Target to view bonding status. |

---

### Player Commands
* `[BondInfo`: Targets a pet to display its bonding timer.

---

### Technical Deep Dive (Optional)
* **Bonding Timer Formula:** `(BondingBegin + BondingDelay) - current time`.