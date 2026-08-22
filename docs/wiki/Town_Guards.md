# Town Guards

## Overview

`Server.Mobiles.TownGuards` is the shard's standard town-law mobile. A town guard is a melee `BasePerson` that can be attacked and killed, targets criminals and other configured enemies, supports guard dialogue and bounty turn-ins, and can be placed by the checked-in premium-spawner maps or requested through `GuardedRegion`.

Killing a guard is intentionally different from earning a normal creature kill. The credited player becomes criminal and receives one long-term murder count, while ordinary creature kill awards and usable corpse loot are suppressed. The existing `BaseCreature` death path remains responsible for spawner activation, turn-combat cleanup, mount deletion, and final mobile deletion.

## Source Trace

| File | Purpose |
| --- | --- |
| `Data/Scripts/Mobiles/Civilized/TownGuards.cs` | Guard construction, stats, services, regional appearance, targeting, death policy, and serialization. |
| `Data/Scripts/Mobiles/Base/BasePerson.cs` | Always-attackable notoriety, corpse deletion, killer-owner resolution, and criminal/murder-count consequence. |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | Shared death rewards, `NoKillAwards`, premium-spawner activation, corpse cleanup, and mount cleanup. |
| `Data/Scripts/Mobiles/Base/Behavior.cs` | `IntelligentAction.GetMyEnemies()` target filtering. |
| `Data/Scripts/Mobiles/Base/PlayerMobile.cs` | Guard sentencing and prison transfer when guards do not sentence players to death. |
| `Data/Scripts/System/Regions/GuardedRegion.cs` | Guard commands, criminal-candidate timers, guard-call speech, reuse, and emergency construction. |
| `Data/Scripts/System/Misc/Settings.cs` and `Info/settings.xml` | Guard sentence, patrol, and sprint settings. |
| `Data/Scripts/System/Misc/Spawning.cs` | Checked-in `.map` parsing and `PremiumSpawner` replacement behavior. |
| `Data/Spawns/towns.map` and `Data/Spawns/land.map` | Checked-in town-guard placement, count, and delay data. |
| `Data/Scripts/System/Misc/Notoriety.cs` | `AlwaysAttackable` creatures resolve to `Notoriety.CanBeAttacked`. |
| `Data/System/Source/Mobile.cs` and `Data/System/Source/TurnBasedCombat.cs` | Core damage/death/delete flow and turn-combat deletion bridge. |
| `Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs` | Removes deleted mobiles from turn-combat participant and group state. |

## Construction And Combat Profile

`TownGuards` inherits `BasePerson`, whose constructor selects `AIType.AI_Melee`, `FightMode.Closest`, perception range `10`, fight range `1`, active speed `0.2`, and passive speed `0.4`. When guard sprinting is enabled, the guard constructor changes active speed to `0.15` and passive speed to `0.25`.

| Property | Compiled behavior |
| --- | --- |
| Strength, Dexterity, Intelligence | Each rolls from `100` through `300`. |
| Hits | Rolls from `1,500` through `2,000`. |
| Damage | `40` through `85`. |
| Virtual armor | `200`. |
| Combat skills | Anatomy, Magic Resist, Bludgeoning, Fencing, Fist Fighting, Swords, and Tactics are each `125.0`. |
| Bard and poison protection | Bard immune, unprovokable, uncalmable, and immune through `Poison.Deadly`. |
| Equipment | The inherited backpack is replaced with an immovable backpack. Spawn equipment and weapons are assigned by region/world and are not intended as player loot. |
| Mount | Roughly half of outdoor guards receive a world-appropriate mount, except on Serpent Island. The mount is deleted when its guard is deleted. |

`OnAfterSpawn()` selects clothing, armor, weapons, hues, and some names by the guard's current region and world. Those presentation rules are independent of whether the guard can die.

## Attackability, Enemies, And Law

`BasePerson.AlwaysAttackable` returns `true`, so guard notoriety resolves to `CanBeAttacked`. Guard hostility is a separate decision made by `TownGuards.IsEnemy()` and `IntelligentAction.GetMyEnemies()`.

The common enemy filter requires visibility and line of sight, excludes staff and civilian/vendor types, and protects ordinary players who are neither criminal nor carrying a long-term murder count. It also applies protected/public/start/safe-region exclusions and the shard's existing disguise, race, alignment, creature, and region rules. A guard will not pursue an otherwise valid target across a region boundary when `GuardsPatrolOutside()` is false.

The checked-in settings currently configure:

| Setting | Current value | Effect |
| --- | --- | --- |
| `GuardsSentenceDeath()` | `false` | Guards chase qualifying targets; a player they defeat is sent to prison rather than completing normal player death. |
| `GuardsPatrolOutside()` | `true` | Guards may recognize qualifying enemies outside their own town-region boundary when the normal target filters pass. |
| `GuardsSprint()` | `true` | Guards use the faster active/passive movement speeds assigned by their constructor. |

When a guard is credited with defeating a player while sentence-death is disabled, `PlayerMobile.OnBeforeDeath()` cancels that death, teleports the player and pets to the world-appropriate prison, restores hits/stamina/mana, and deletes many non-blessed consumables and tools. Guard killability does not change this sentencing path.

## Killing A Guard

`TownGuards.OnBeforeDeath()` sets `NoKillAwards = true` and delegates to `BasePerson.OnBeforeDeath()`.

The inherited path resolves a summoned, controlled, or bard-provoked creature killer to its player master. When the resolved killer is a `PlayerMobile`, it sets `Criminal = true` and increments `Kills` by one. This is the long-term murder counter; `ShortTermMurders` is not changed by this path. A death without a player-resolved killer does not assign that penalty.

`NoKillAwards` suppresses the ordinary `BaseCreature` treasure-map, generated-loot, fame, karma, faction, XML quest-kill, and normal quest-kill award path. Generic battle logging and specialized death callbacks outside that gate remain intact. Current spawned guards do not meet the traced special-reward prerequisites.

The engine may create a corpse container during death assembly, but `BasePerson.DeleteCorpseOnDeath` causes `BaseCreature.OnDeath()` to delete it immediately. The guard's immovable equipment and backpack are deleted with the mobile, and a mounted guard's mount is deleted during `BaseCreature.OnDelete()`. No usable guard corpse or equipment should remain.

## Replacement And Spawn Data

`BaseCreature.OnDeath()` and `BaseCreature.OnDelete()` call `PremiumSpawner.ActivateSpawner(this)`. A guard spawned from a single-count premium spawner carries that spawner's serial in `SpawnerID`; death reactivates the stopped spawner and schedules the replacement with its configured delay.

The checked-in `.map` inventory is source data, not proof of the current live-world population. The parser resolves type names case-insensitively, so both `TownGuards` and `townguards` rows are relevant.

| Checked-in guard spawn data | Value |
| --- | ---: |
| Rows | 287 |
| Configured NPC count per row | 1 |
| Total configured guard slots | 287 |
| 5–10 minute rows | 268 |
| 45–60 minute rows | 5 |
| 1 minute rows | 11 |
| 0 minute rows | 3 |

No guard spawn-map change is required for killability. The existing delays determine how long guard services and local law enforcement are absent after a kill.

## Guard Calls And Services

`GuardedRegion` tracks eligible criminal/red candidates for 15 seconds. Guard-call speech searches nearby mobiles for a candidate, then reuses an idle `TownGuards` within eight tiles or attempts to construct the configured guard type.

Emergency construction is a separate known defect: `GuardedRegion.MakeGuard()` passes a focus mobile to `Activator.CreateInstance`, while `TownGuards` has only parameterless and serial constructors. The exception is silently swallowed. Existing nearby guards can still be redirected, and checked-in premium-spawner guards are constructed through the parameterless constructor. See [Region System](Region_System.md) for region commands and the deferred issue.

Living guards retain their existing context-menu duties dialogue, wanted-person notes, pirate-bounty handling, and supported head/token/item turn-ins. This page intentionally does not list locations or hidden reward solutions.

## Serialization And Compatibility

`TownGuards` keeps its required serial constructor and version `0` serializer. It writes no fields after the version integer, and deserialization reads the same version integer after `base.Deserialize(reader)`. Guard killability does not change the class name, namespace, constructors, serialized layout, spawn-map format, settings schema, or any public interface.

## Staff Verification

Use a disposable staging world, not production data or an occupied production port.

1. Record a normal test player's `Criminal`, `Kills`, and `ShortTermMurders` values with the staff properties gump.
2. Use `[Add TownGuards`, place the guard, and use `[Props` on it to confirm its runtime type and starting hits.
3. Reduce the guard to low health through the properties gump if needed, then let the normal test player deliver lethal damage.
4. Confirm the guard and any mount disappear, no usable corpse or equipment remains, the player is criminal, `Kills` increased by exactly one, and `ShortTermMurders` did not change.
5. Repeat with a controlled or summoned creature delivering lethal damage and confirm the owner receives the same consequence.
6. Kill a representative checked-in spawner guard and confirm its associated premium spawner schedules one replacement using that row's configured delay.
7. With turn-based combat enabled, confirm the dead guard leaves the encounter/participant state cleanly.
8. Confirm a replacement guard still targets an eligible criminal, accepts its supported turn-ins, and opens the duties dialogue.
9. While `GuardsSentenceDeath()` is false, let a guard defeat a test player and confirm the existing prison-transfer behavior remains unchanged.

Source inspection confirms the intended code paths. Exact live population, replacement timing, award absence, and prison behavior remain staging/runtime acceptance checks.
