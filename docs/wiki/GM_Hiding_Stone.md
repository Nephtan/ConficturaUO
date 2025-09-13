# System Name: GM Hiding Stone

### Overview
Allows staff members to toggle their hidden status with customizable visual effects. Only those with Counselor access or higher can use the stone.

---

### How It Works
* **Activation:** Double-click the stone. Staff above the Player access level may use it.
* **Toggle:** When hidden, the stone triggers the selected AppearEffect and reveals you; otherwise it plays the HideEffect and conceals you.
* **Effects:** Available effects include Gate, FlameStrike1, FlameStrike3, FlameStrikeLightningBolt, Sparkle1, Sparkle3, Explosion, ExplosionLightningBolt, DefaultRunUO, Snow, Glow, PoisonField, Fireball, FireStorm1, and FireStorm2.
* **Customization:** Use property commands to set `AppearEffect`, `HideEffect`, `AppearEffectHue`, and `HideEffectHue`.
* **Restrictions:** Players receive "You are unable to use that!" when attempting to activate the stone.

---

### Getting Started
1. Create the stone with `[add gmhidingstone` and place it in your backpack.
2. Configure desired effects and hues using `[props` or `[set AppearEffect <effect>]`, `[set HideEffect <effect>]`, `[set AppearEffectHue <hue>]`, and `[set HideEffectHue <hue>]`.
3. Double-click the stone to hide or appear with the configured effect.

---

### Associated Items & NPCs
| Item/NPC Name | Purpose |
| :--- | :--- |
| GM Hiding Stone | Toggles staff visibility with configurable effects. |

---

### Staff Commands
* `[add gmhidingstone` — creates the stone.
* `[props` — opens the property gump for effect and color settings.
* `[set AppearEffect <effect>]` — sets the effect used when appearing.
* `[set HideEffect <effect>]` — sets the effect used when hiding.
* `[set AppearEffectHue <hue>]` — assigns hue for the appear effect.
* `[set HideEffectHue <hue>]` — assigns hue for the hide effect.

---

### Technical Deep Dive (Optional)
* **Available Effects:** Gate, FlameStrike1, FlameStrike3, FlameStrikeLightningBolt, Sparkle1, Sparkle3, Explosion, ExplosionLightningBolt, DefaultRunUO, Snow, Glow, PoisonField, Fireball, FireStorm1, FireStorm2.