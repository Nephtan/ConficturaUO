# System Name: Monster Nest System

## Overview

Monster Nest System is a custom world-spawn framework built around an `Item` (`MonsterNest`) plus a linked `BaseCreature` proxy (`MonsterNestEntity`). The item owns the spawn list, respawn timer, regeneration timer, health pool, home range, and loot level. The proxy mobile exists so players can attack the nest through normal RunUO combat targeting.

Stock constructables in this folder are `RatmanNest`, `LizardmanNest`, and `UndeadNest`. They all inherit from `MonsterNest`, raise the base durability/throughput settings, and call `InitializeMonsterNestEntity()` after their final stats are assigned.

There are no commands, gumps, `EventSink` hooks, or XMLSpawner attachments in this system. It is driven entirely by placed items, timers, spawned `BaseCreature` instances, and the proxy combat mobile.

## Script Inventory

* `Data/Scripts/Custom/PvE/MonsterNests/MonsterNest.cs`
* `Data/Scripts/Custom/PvE/MonsterNests/MonsterNestEntity.cs`
* `Data/Scripts/Custom/PvE/MonsterNests/MonsterNestLoot.cs`
* `Data/Scripts/Custom/PvE/MonsterNests/Types/RatmanNest.cs`
* `Data/Scripts/Custom/PvE/MonsterNests/Types/LizardmanNest.cs`
* `Data/Scripts/Custom/PvE/MonsterNests/Types/UndeadNest.cs`

`Data/Scripts/Scripts.csproj` explicitly compiles all six scripts, so the system is part of the maintained script assembly.

## Administrator Surface

The Monster Nest System does not register any custom `CommandSystem` commands, `[Usage]` metadata, packet handlers, or Gumps. Administrative placement uses RunUO's standard `[add <ConstructableType>]` command surface for the constructable item classes:

| Constructable type | Result |
| --- | --- |
| `MonsterNest` | Places the unconfigured base nest item. |
| `RatmanNest` | Places a configured ratman-family nest. |
| `LizardmanNest` | Places a configured lizardman-family nest. |
| `UndeadNest` | Places a configured legacy string-spawn nest using `NestSpawnType = "Undead"`. |

The base `MonsterNest` exposes these `[CommandProperty(AccessLevel.GameMaster)]` properties for in-game property editing:

| Property | Serialized field | Purpose |
| --- | --- | --- |
| `NestSpawnType` | `m_NestSpawnType` | Legacy string type name resolved by `SpawnerType.GetType()`. |
| `MaxCount` | `m_MaxCount` | Maximum number of living tracked spawn mobiles. |
| `RespawnTime` | `m_RespawnTime` | Delay used when each spawn timer is rescheduled. |
| `HitsMax` | `m_HitsMax` | Maximum nest item hit points. |
| `Hits` | `m_Hits` | Current nest item hit points. |
| `RangeHome` | `m_RangeHome` | `BaseCreature.RangeHome` assigned to spawned mobiles. |
| `LootLevel` | `m_LootLevel` | Multiplier used by the remains reward formula. |

## Base `MonsterNest` Defaults

The bare `[add MonsterNest` constructable is only a scaffold. Its constructor does not configure a usable spawn profile.

| Property | Base value |
| --- | --- |
| `ItemID` | `4962` |
| `Name` | `Monster Nest` |
| `Hue` | `1818` |
| `Weight` | `0.1` |
| `HitsMax` / `Hits` | `300 / 300` |
| `RangeHome` | `10` |
| `RespawnTime` | `15` seconds |
| `NestSpawnType` | `null` |
| `MaxCount` | `0` |
| `LootLevel` | `0` |
| `Movable` | `false` |

The base constructor starts `InternalTimer` and `RegenTimer`, but it does not call `InitializeMonsterNestEntity()`. That means a plain `MonsterNest` added by a GM has no combat proxy until custom code creates one, and it will not spawn anything until `MaxCount` and either `NestSpawnType` or an overridden `CreateSpawnMobile()` path are supplied.

## Stock Nest Variants

| Constructable | Visuals | Shared combat/spawn settings | Spawn pool |
| --- | --- | --- | --- |
| `RatmanNest` | `ItemID 9986`, hue `0`, name `Ratman Nest` | `MaxCount = 30`, `RespawnTime = 3s`, `HitsMax/Hits = 5000`, `RangeHome = 20`, `LootLevel = 1` | 40% `Ratman`, 40% `RatmanArcher`, 20% `RatmanMage` |
| `LizardmanNest` | `ItemID 9956`, hue `0`, name `Lizardman Nest` | `MaxCount = 30`, `RespawnTime = 3s`, `HitsMax/Hits = 5000`, `RangeHome = 20`, `LootLevel = 1` | 50% `Lizardman`, 30% `LizardmanArcher`, 20% `Reptalar` |
| `UndeadNest` | `ItemID 13335`, hue `1573`, name `Undead Nest` | `MaxCount = 30`, `RespawnTime = 3s`, `HitsMax/Hits = 5000`, `RangeHome = 20`, `LootLevel = 1` | Legacy single-type path: `NestSpawnType = "Undead"` resolves through `SpawnerType.GetType()` |

`RatmanNest` and `LizardmanNest` override `CreateSpawnMobile()` directly. `UndeadNest` keeps the base string-driven path, so it depends on `ScriptCompiler.FindTypeByName("Undead")` resolving to a constructable `BaseCreature`.

## Spawn Lifecycle

### Timer startup

`MonsterNest()` starts two timers immediately:

* `InternalTimer`, scheduled from the current `RespawnTime`
* `RegenTimer`, scheduled every 1 minute

Because C# runs the base constructor before the derived constructor body, the stock nest types still schedule their first spawn tick using the base `15` second respawn value. After that first tick, later `InternalTimer` instances use the derived `3` second `RespawnTime`.

### Spawn routine

`DoSpawn()` runs when `InternalTimer` ticks.

1. If `m_Entity` exists, it is moved to the nest's current `Location` and `Map`.
2. The nest only attempts a spawn when `m_Spawn != null` and `Count() < m_MaxCount`.
3. `CreateSpawnMobile()` produces the next `BaseCreature`.
4. The mobile is added to `m_Spawn`.
5. `OnBeforeSpawn(Location, Map)` runs.
6. The creature is moved to the nest's world location.
7. `Home` is set to the nest location.
8. `RangeHome` is set from the nest's `RangeHome` property.

The base `CreateSpawnMobile()` path only works when `NestSpawnType` is non-null, the resolved type exists, and the created object is a `BaseCreature`.

If the string-driven spawn path throws during creation, the catch block silently clears `NestSpawnType` to `null`, which disables future legacy spawns until a GM reconfigures the nest.

## Combat Flow

### How players attack a nest

`MonsterNest.OnDoubleClick(Mobile from)` sends `You begin to attack the object.` and assigns `from.Combatant = m_Entity` when the proxy mobile exists.

The nest item itself stores the health pool in `Hits` and `HitsMax`. The proxy mobile is only the combat-facing shell.

### `MonsterNestEntity` behavior

`MonsterNestEntity` is a frozen, immobile `BaseCreature` with:

* `AIType.AI_Melee`
* `FightMode.Aggressor`
* `CantWalk = true`
* `Damage = 0`
* all stats and resistances set to `0`
* `Body = 399`
* `Fame = 5000`
* `Karma = -5000`

Its important overrides are:

* `OnThink()`: keeps the proxy frozen, snaps its `Location` to the nest's location, and copies `nest.Hits` into the mobile's `Hits`
* `OnDamage(int amount, Mobile from, bool willkill)`: forwards the damage to `nest.Damage(amount)`
* `OnBeforeDeath()`: always returns `false`, so the proxy does not die through normal `BaseCreature` death handling

`MonsterNest.Damage(int damage)` subtracts from the item's `Hits`, emits the damage amount as an overhead message on the item, and calls `Destroy()` when `Hits <= 0`.

## Destruction And Cleanup

When `Destroy()` runs, the nest:

1. Calls `AddLoot()` to drop `MonsterNestLoot` at the nest location.
2. Iterates `m_Spawn` and calls `Kill()` on each tracked mobile.
3. Deletes the proxy `m_Entity` if present.
4. Deletes itself.

This is different from `OnDelete()`. If the nest is deleted administratively rather than destroyed through damage, `OnDelete()` calls `Delete()` on the tracked mobiles instead of `Kill()`, then deletes the proxy.

## Loot Remains

`AddLoot()` creates a `MonsterNestLoot` item with:

* `ItemID = 6585`
* the nest hue
* the nest loot level
* name prefix `Monster Nest remains`

The `MonsterNestLoot` constructor appends ` (double click to loot)` to the display name and sets `Movable = false`.

### Reward distribution

When anyone double-clicks the remains, `MonsterNestLoot.OnDoubleClick(Mobile from)` gathers every mobile within 20 tiles, filters that list to `PlayerMobile`, calls `AddLoot(m)` for each player found, and then deletes the remains item.

The reward is not reserved for the clicker. It is paid once to every nearby `PlayerMobile`.

### Reward formula

`AddLoot(Mobile m)` calculates:

`chance = Utility.Random(5, 20) * m_LootLevel`

In this codebase, `Utility.Random(5, 20)` produces an integer from `5` through `24`.

| Chance band | Reward |
| --- | --- |
| `< 10` | `Gold(500..999)` |
| `< 20` | `Gold(1000..1999)` |
| `< 30` | `BankCheck(2000..2999)` |
| `< 40` | `BankCheck(3000..3999)` |
| `< 50` | `BankCheck(4000..4999)` |
| `< 60` | `BankCheck(5000..5999)` |
| `< 70` | `BankCheck(6000..6999)` |
| `< 80` | `BankCheck(7000..7999)` |
| `< 90` | `BankCheck(8000..8999)` |
| `>= 90` | `BankCheck(7000..9999)` |

All three stock nests use `LootLevel = 1`, so their exact live payout odds are:

| Stock loot result | Probability |
| --- | --- |
| `Gold(500..999)` | 25% |
| `Gold(1000..1999)` | 50% |
| `BankCheck(2000..2999)` | 25% |

## Persistence

### `MonsterNest`

`MonsterNest.Serialize()` writes version `0`, then persists:

1. `m_NestSpawnType`
2. `m_Spawn` via `WriteMobileList`
3. `m_MaxCount`
4. `m_RespawnTime`
5. `m_HitsMax`
6. `m_Hits`
7. `m_RangeHome`
8. `m_LootLevel`
9. `m_Entity`

`Deserialize()` reads that same version `0` payload back in the same order.

### `MonsterNestEntity`

`MonsterNestEntity.Serialize()` writes version `0` plus the linked nest `Item` reference. `Deserialize()` reads the reference back with `ReadItem()`.

### `MonsterNestLoot`

`MonsterNestLoot.Serialize()` writes version `0` plus `m_LootLevel`. `Deserialize()` reads the same fields back in order.

## Observed Implementation Gaps

* `MonsterNest.Deserialize()` restores the saved state, but it never restarts `InternalTimer` or `RegenTimer`. Existing nests therefore stop spawning and stop regenerating after a world save/load cycle unless they are reconstructed from the constructable path.
* The bare `MonsterNest` constructable is half-configured for live use: `NestSpawnType` is `null`, `MaxCount` and `LootLevel` default to `0`, and the constructor never creates `MonsterNestEntity`.
* `MonsterNestEntity.OnThink()` assigns `this.Location = this.m_MonsterNest.Location` before checking whether `m_MonsterNest` is null, so an orphaned proxy can null-dereference if its saved item reference is missing.

## Technical Trace

* Wiki claim: nests are item-driven spawners with a proxy combat mobile -> traced `MonsterNest`, `MonsterNestEntity`, and the constructable nest types -> the item stores spawn state and health, while the mobile proxy exists only to take combat targeting and forward damage.
* Wiki claim: stock nests use different creature pools -> traced `RatmanNest.CreateSpawnMobile()`, `LizardmanNest.CreateSpawnMobile()`, and `UndeadNest` -> Ratman and Lizardman nests use hard-coded weighted pools, while Undead uses the legacy `NestSpawnType = "Undead"` type-resolution path.
* Wiki claim: nest destruction kills the active wave and drops remains -> traced `MonsterNest.Damage()`, `Destroy()`, and `AddLoot()` -> reducing `Hits` to zero spawns a `MonsterNestLoot` item, calls `Kill()` on the tracked spawn list, deletes the proxy, and deletes the nest.
* Wiki claim: the loot item rewards the clicker -> traced `MonsterNestLoot.OnDoubleClick()` -> the remains actually reward every `PlayerMobile` within 20 tiles, then delete themselves.
* Wiki claim: saved nests resume automatically after restart -> traced `Serialize()` and `Deserialize()` -> the system persists state, but the deserialize path does not restart either timer.

## XMLSpawner

This system does not contain XMLSpawner attachments, packet hooks, or XMLSpawner registration code. The only dynamic type lookup is the legacy `NestSpawnType` string path, which uses `SpawnerType.GetType()` and `ScriptCompiler.FindTypeByName()` rather than XMLSpawner infrastructure.
