# System Name: Voting System

### Overview
Encourages players to vote for the shard by launching a configured website.

---

### How It Works
* **Vote Triggers:** Double-click a Vote Stone or use the `[Vote` command.
* **Browser Launch:** Successful votes open the configured URL in your browser.
* **Cooldown:** Each account may vote once every 24 hours by default.
* **Feedback:** The system sends confirmation or cooldown messages.
* **Configuration:** Staff can adjust site name, URL, and cooldown through properties or the `[VoteConfig` command.
* **Success/Failure Conditions:** Voting is blocked if the cooldown has not expired or if the site URL is invalid.

---

### Getting Started
1. Find a Vote Stone or type `[Vote`.
2. If eligible, your browser opens to the voting page.
3. Cast your vote and return to the game.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| Vote Stone | Physical object players can double-click to vote. |

---

### Player Commands
* `[Vote`: Attempts to cast a vote.
* `[VoteConfig`: (GM) Shows configuration settings.
* `[VoteInstance`: (GM) Views the internal vote item instance.

---

### Technical Deep Dive (Optional)
* **Default Cooldown:** 24 hours between votes.
* **Vote Stone Message:** "Use: Launches your browser to cast a vote for <server>".
* **System validates URLs and tracks vote times per account.**
