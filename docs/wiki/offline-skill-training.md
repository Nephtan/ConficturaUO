# System Name: Offline Skill Training

### Overview
Study books let your character train skills while logged out and can grant temporary accelerated gains.

---

### How It Works
* **Study Books:** Each book is tied to a specific skill and a maximum level.
* **Starting Study:** Double-click the book in your backpack; on your next logout, study begins automatically.
* **Skill Gain:** Every 30 seconds of offline study grants 0.1 skill, up to 35.0 skill per session and not exceeding the book's cap.
* **Diminishing Returns:** Gains above 100.0 skill occur at half speed.
* **Accelerated Gains:** Studying for 5 or more hours grants a 30-minute accelerated gain buff upon login.
* **Book Wear:** There is a small chance the book crumbles after use.
* **Vendors:** Study Bookbinders sell various books and reward gold for returning used ones.
* **Success/Failure Conditions:** The book must be in your backpack, set to increase the chosen skill, and not used in combat. Only one book can be studied at a time.

---

### Getting Started
1. Purchase a study book from a Study Bookbinder.
2. Place the book in your backpack.
3. Double-click to schedule study on logout.
4. Log out and remain offline to train.
5. Log back in to apply gained skill and possibly receive an accelerated gain buff.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| Study Book | Enables offline skill training for a specific skill. |
| Study Bookbinder | Vendor selling and buying study books. |

---

### Player Commands
* None.

---

### Technical Deep Dive (Optional)
* **Gain Interval:** 30 seconds per 0.1 skill.
* **Max Gain Per Session:** 35.0 skill.
* **Accelerated Gain Trigger:** After 5 hours of study, grants 30 minutes of accelerated gains.
* **Book Return Rewards:** Up to 10,000 gold for legendary books.