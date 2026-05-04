# System Name: Player Range Sensitive Deactivation

### Overview
The Player Range Sensitive Deactivation mod delays AI shutdown for eligible creatures after their sector loses nearby players. It is a runtime performance feature, not a spawn system and not an XMLSpawner extension.

The only custom script in this system is `Data/Scripts/Custom/Player Range Sensitive Mod/setdeactivation.cs`. The actual AI behavior is implemented through the existing sector activation flow in `Sector`, `Map`, `BaseCreature`, and `BaseAI`.

---

### Admin Command
* **Command:** `[SetDeactivation [minutes]`
* **Access Level:** Administrator
* **Default Delay:** 20 minutes from `SetDeactivation.DefaultDeactivationDelay`.
* **With an Argument:** The first argument is parsed with `double.Parse` and assigned directly to `DefaultDeactivationDelay`.
* **Without an Argument:** No value is changed, but the command still sends the current delay using the same "set to" message.
* **Parse Failures:** Non-numeric arguments are swallowed by an empty `catch`, leaving the previous delay in place.

The command does not clamp values, require positive finite input, or persist the value. The delay resets to the script default after a server restart unless another runtime action sets it again.

---

### Runtime Behavior
Sectors become active when a player enters range and may deactivate after the last nearby player leaves. When a sector deactivates, `BaseCreature.OnSectorDeactivate` deliberately does not stop AI immediately; the comment states that delayed deactivation is handled by the AI timer.

For player-range-sensitive creatures, `BaseAI.AITimer.OnTick` performs the delay math:

```csharp
m_DeactivationTime = DateTime.Now + TimeSpan.FromMinutes(DeactivationDelay);
```

While the creature's sector is active, each AI tick refreshes `m_DeactivationTime` to now plus the configured delay. Once the sector is inactive, the AI timer keeps running until `DateTime.Now > m_DeactivationTime`. At that point `Deactivate()` stops the AI timer and the creature stops thinking until its sector activates again.

When the sector activates again, `BaseCreature.OnSectorActivate` calls `m_AI.Activate()` for player-range-sensitive creatures, restarting the AI timer if it is not already running.

---

### Sensitive And Non-Sensitive Mobiles
`BaseCreature.PlayerRangeSensitive` returns `true` only when the creature has no current waypoint. Creatures following a waypoint continue running AI even without nearby players.

Known overrides include:

| Script | Behavior |
| :--- | :--- |
| `BaseVendor` | Forces `PlayerRangeSensitive` to `true`. |
| `Citizens` | Forces `PlayerRangeSensitive` to `true`. |
| `DragonRider` | Forces `PlayerRangeSensitive` to `false`. |
| Chasing pirate captain and crew scripts | Force `PlayerRangeSensitive` to `false`. |

---

### Spawn Return Behavior
When `BaseAI.Deactivate()` runs, it can also return an uncontrolled region-spawned creature to its spawn home. This applies when the creature's spawner is a `SpawnEntry`, `ReturnOnDeactivate` is true, and the creature is not controlled.

The return-home logic uses the spawn entry's home location or region acceptance checks, then schedules `ReturnToHome()` with `Timer.DelayCall(TimeSpan.Zero, ...)`.

---

### Persistence And Validation
This system has no item or mobile serializer. Its configured delay is a static field in the command class, so no `Serialize` or `Deserialize` version exists for this system.

The command is functionally incomplete as an admin-facing setting because invalid numeric values are accepted. `NaN`, infinity, extremely large values, negative values, and zero are not rejected by the command before the AI timer later calls `TimeSpan.FromMinutes(DeactivationDelay)`.

---

### Audit Notes
* Wiki claimed `[SetDeactivation` adjusts a global delay -> traced `SetDeactivation.Initialize` and `SetDeactivation_OnCommand` -> code registers the command for administrators and stores the first parsed argument in `DefaultDeactivationDelay`.
* Wiki claimed non-numeric values are ignored -> traced `double.Parse` inside an empty `catch` -> code ignores parse failures but does not validate numeric values that parse successfully.
* Wiki claimed NPCs remain active after players leave range -> traced `Sector.OnLeave`, `Map.DeactivateSectors`, `BaseCreature.OnSectorDeactivate`, and `BaseAI.AITimer.OnTick` -> code deactivates sectors immediately but delays AI shutdown until the configured deadline has passed.
* Wiki omitted activation rules -> traced `BaseCreature.PlayerRangeSensitive` and overrides -> code excludes waypoint-following creatures by default and has several explicit overrides.
* Wiki omitted spawn return behavior -> traced `BaseAI.Deactivate` and `SpawnEntry.ReturnOnDeactivate` -> code can return uncontrolled region spawns home during AI deactivation.
* Serialization check -> traced the command-only script -> no custom save version exists because the delay is runtime static state.
