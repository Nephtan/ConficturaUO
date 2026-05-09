# System Name: GM Hiding Stone

### Overview
The GM Hiding Stone is a blessed staff item implemented by `Server.Items.GMHidingStone`. It lets any mobile above `AccessLevel.Player` toggle `Hidden` with one of several visual effects. Player-level users cannot activate it and receive the message, "You are unable to use that!"

The compiled script is `Data/Scripts/Custom/CEO's GM Hiding Stone [2.0]/gmhidingstone.cs`, and it is explicitly included in `Data/Scripts/Scripts.csproj`. This system has no NPCs, XMLSpawner integration, or separate engine hook.

---

### Item Defaults
* **Class:** `GMHidingStone : Item`
* **Namespace:** `Server.Items`
* **Constructable command:** `[add GMHidingStone`
* **ItemID:** `0x1870`
* **Name:** `GM hiding stone`
* **Weight:** `1.0`
* **Hue:** `0`
* **LootType:** `Blessed`
* **Default AppearEffect:** `DefaultRunUO`
* **Default HideEffect:** `DefaultRunUO`
* **Default effect hues:** `0`

---

### Activation Rules
* Double-clicking calls `OnDoubleClick(Mobile m)`.
* The access check is `m.AccessLevel > AccessLevel.Player`. In the current enum ordering, this includes Counselor, GameMaster, Seer, Administrator, Developer, and Owner.
* If the mobile is already hidden, the stone uses `AppearEffect` and `AppearEffectHue`.
* If the mobile is visible, the stone uses `HideEffect` and `HideEffectHue`.
* Player-level users are rejected with the exact message, "You are unable to use that!"

---

### Effect Behavior
Most effects use the default `ToggleHidden` branch, which simply flips `m.Hidden`. When appearing from hidden, the flip happens before the effect is sent; when hiding from visible, the effect is sent before the flip because `OnDoubleClick` orders the calls differently. There are three special timing cases:

* **Gate:** `ToggleHidden` does not flip visibility. The effect freezes the mobile, plays gate particles and sound, then uses delayed callbacks. A second visual fires after 0.65 seconds, and `Hidden` is flipped after 1.5 seconds when the mobile is unfrozen.
* **FireStorm1:** `ToggleHidden` does not flip visibility. `SendStoneEffects` flips `Hidden` immediately, sends the first firestorm burst, and starts a timer that expands the flame pattern outward.
* **FireStorm2:** `ToggleHidden` does not flip visibility. `SendStoneEffects` starts an inward firestorm timer, and `Hidden` is flipped when that timer finishes.

Positive effect hues are decremented by one before the effect packets are sent. Hue `0` is left unchanged.

---

### Available Effects
The `StoneEffect` enum supports:

* `Gate`
* `FlameStrike1`
* `FlameStrike3`
* `FlameStrikeLightningBolt`
* `Sparkle1`
* `Sparkle3`
* `Explosion`
* `ExplosionLightningBolt`
* `DefaultRunUO`
* `Snow`
* `Glow`
* `PoisonField`
* `Fireball`
* `FireStorm1`
* `FireStorm2`

---

### Staff Configuration
The following properties are exposed with `CommandProperty(AccessLevel.Counselor)`:

* `AppearEffect`
* `HideEffect`
* `AppearEffectHue`
* `HideEffectHue`

Typical configuration uses `[props` on the item or direct property commands such as `[set AppearEffect FireStorm1` and `[set HideEffect Gate`.

---

### Persistence
`GMHidingStone` implements the standard RunUO `Serial` constructor and overrides `Serialize` and `Deserialize`.

Current saves write version `2`, followed by:

1. `AppearEffect` as an `int`
2. `HideEffect` as an `int`
3. `AppearEffectHue` as an `int`
4. `HideEffectHue` as an `int`

Deserialization supports version `1`, which only reads the appear and hide effects, and version `2`, which also reads both hue values.
