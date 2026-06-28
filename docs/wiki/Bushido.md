# Bushido

## Overview

Bushido is implemented as a Samurai `Spellbook` plus six ability IDs registered through the shared spell registry. Three abilities are instant `SamuraiSpell` stances (`Confidence`, `Evasion`, and `CounterAttack`), and three are `SamuraiMove` special moves (`HonorableExecution`, `LightningStrike`, and `MomentumStrike`) that execute through the weapon-combat pipeline.

There are no Bushido-specific player or staff `[Command]` registrations in `Data/Scripts/Magic/Bushido/`. Players activate the abilities through the Samurai spellbook client packet path, shared spell toolbar systems, or any other shard UI that sends the registered spell ID.

## Core Components

| Component | Type | Purpose |
| --- | --- | --- |
| `BookOfBushido` | `Spellbook` Item | Samurai spellbook with `BookOffset = 400`, `BookCount = 6`, full default content `0x3F`, and `Layer.Talisman`. |
| `SamuraiSpell` | `Spell` base class | Shared stance casting rules, Bushido skill/mana checks, SE expansion checks, and client special-ability toggles. |
| `SamuraiMove` | `SpecialMove` base class | Shared special-move skill gain using `SkillName.Bushido`. |
| `Confidence` | `SamuraiSpell` | Temporary defensive stance with passive regeneration and block rewards. |
| `Evasion` | `SamuraiSpell` | Temporary defense state that increases parry and can evade single-target spell damage. |
| `CounterAttack` | `SamuraiSpell` | Prepares one retaliatory swing after the next successful block. |
| `HonorableExecution` | `SamuraiMove` | Next-hit special move that rewards a kill or penalizes a failed execution. |
| `LightningStrike` | `SamuraiMove` | Next-hit special move with accuracy and armor-ignore chance. |
| `MomentumStrike` | `SamuraiMove` | Next-hit special move that transfers a swing to another enemy fighting the attacker. |

## Registration And Activation

`Initializer` registers Samurai ability IDs `400` through `405`:

| Spell ID | Registry type |
| ---: | --- |
| `400` | `Bushido.HonorableExecution` |
| `401` | `Bushido.Confidence` |
| `402` | `Bushido.Evasion` |
| `403` | `Bushido.CounterAttack` |
| `404` | `Bushido.LightningStrike` |
| `405` | `Bushido.MomentumStrike` |

`Spellbook.GetTypeForSpell` maps IDs `400 <= spellID < 406` to `SpellbookType.Samurai`. When the client requests a cast, `EventSink_CastSpellRequest` first verifies that the `Spellbook` contains the spell ID. If the registry entry is a `SpecialMove`, the current move is set with `SpecialMove.SetCurrentMove(from, move)`. Otherwise the spell is constructed with `SpellRegistry.NewSpell(spellID, from, null)` and cast normally.

`BookOfBushido` can be equipped only when the `Mobile` has at least `30.0` base Bushido skill. This equip gate is in the shared `Spellbook.OnEquip` logic, not in the book class itself.

## Shared Bushido Spell Rules

| Rule | Actual behavior |
| --- | --- |
| Cast skill | `SkillName.Bushido` |
| Damage skill | `SkillName.Bushido` |
| Hands | `ClearHandsOnCast = false` |
| Movement | `BlocksMovement = false` |
| Hand movement animation | `ShowHandMovement = false` |
| Fast-cast delay scalar | `0` |
| Cast recovery base | `7` |
| Skill check range | `RequiredSkill - 12.5` through `RequiredSkill + 37.5` |
| Mana payment | `CheckFizzle` consumes `ScaleMana(RequiredMana)` after base fizzle succeeds. |
| Expansion gate | Player `Mobile` casters require a non-null `NetState` supporting `Expansion.SE`; non-player casters bypass this check. |
| Stance exclusivity | A successful Bushido stance ends active `Evasion`, `Confidence`, and `CounterAttack` before enabling the new client toggle. |

## Shared Samurai Move Rules

Samurai moves inherit the shared `SpecialMove` pipeline. Player `Mobile` validation requires `Core.SE`, no active Honorable Execution penalty, no Ninjitsu `AnimalForm`, enough Bushido skill, and enough scaled mana. Setting a Samurai move clears any current `WeaponAbility`; setting a `WeaponAbility` clears any current Samurai move.

The move hooks are called from `BaseWeapon`: hit chance uses `GetAccuracyBonus`, damage uses `GetDamageScalar`, armor-ignore checks call `IgnoreArmor`, successful hits call `OnHit`, and misses call `OnMiss`.

## Ability Reference

| Ability | ID | Class | Required Bushido | Mana | Timing | Mechanics |
| --- | ---: | --- | ---: | ---: | --- | --- |
| Honorable Execution | 400 | `HonorableExecution` | 25.0 | 0 | Current special move until next hit or clear | Adds damage scalar `1.0 + (Bushido * 20) / 10000`. If the defender dies, the attacker gains `20 + floor(Bushido^2 / 480)` Hits and a swing-speed bonus `max(1, floor(Bushido^2 / 720))`. If the defender survives, the attacker receives `-40` to all five resistances and loses all current Magic Resist skill value through a temporary `SkillMod`. |
| Confidence | 401 | `Confidence` | 25.0 | 10 | 0.25s cast, 15s stance | Starts a confidence timer and a one-second regeneration timer. The regeneration pool is `15 + (Bushido.Fixed^2 / 57600)`. While confident, a successful block restores random Hits from `1` to `floor(Bushido / 12)` and random Stamina from `1` to `floor(Bushido / 5)`. |
| Evasion | 402 | `Evasion` | 60.0 | 10 | 0.25s cast, variable duration, 20s action cooldown | Requires an equipped weapon or shield. Under `Core.ML`, an equipped weapon also requires at least `50.0` base skill in that weapon skill. While active, parry chance is multiplied by the Evasion scalar, and single-target spell damage can be reduced to zero when `CheckSpellEvasion` succeeds. |
| Counter Attack | 403 | `CounterAttack` | 40.0 | 5 | 0.25s cast, 30s stance | Requires an equipped weapon or shield. On the next successful block, if the defender's current weapon is a `BaseWeapon`, it immediately calls `weapon.OnSwing(defender, attacker)` and then ends the counter state. |
| Lightning Strike | 404 | `LightningStrike` | 50.0 | 5 | Current special move until hit or clear; delayed context | Adds `50` hit-chance bonus before the global hit-chance cap. On hit, consumes mana, sends strike messages, plays effects, checks Bushido gain, and sets delayed context. Armor-ignore chance is `Bushido^2 / 72000`. |
| Momentum Strike | 405 | `MomentumStrike` | 70.0 | 10 | Current special move until hit or clear | On hit, scans mobiles within the attacker's weapon `MaxRange`, keeps mobiles whose `Combatant == attacker` and are not the original defender, and randomly swings at one valid target with damage bonus `Bushido / 100`. If the original defender died, the damage bonus is multiplied by `1.5`. |

## Evasion Formulas

| Formula | `Core.ML == false` | `Core.ML == true` |
| --- | --- | --- |
| Duration | `8` seconds | `floor(3 + max(0, (Bushido - 60) / 20) + bonus)` seconds, where `bonus = 1` only when Anatomy `>= 100.0`, Tactics `>= 100.0`, and Bushido `> 100.0`. |
| Parry scalar | `1.5` | `1.0 + bonus`, where `bonus` starts at `0`, adds `((Bushido - 60) * 0.004) + 0.16` when Bushido `>= 60.0`, and adds `0.10` only when Anatomy `>= 100.0`, Tactics `>= 100.0`, and Bushido `> 100.0`. |

`CheckSpellEvasion` also rejects spell evasion under `Core.ML` when the defender is actively casting. If the defender passes the equipment checks, is currently evading, and `BaseWeapon.CheckParry(defender)` succeeds, the spell damage scalar is set to zero by the shared `Spell` damage code.

## Parry And Block Integration

`BaseWeapon.CheckParry` uses both Parry and Bushido:

| Defender equipment | Chance formula before Evasion and Dexterity adjustments |
| --- | --- |
| Shield equipped | `(Parry - Bushido.TotalSkillValue) / 400.0`, clamped to zero, plus `0.05` if Parry or Bushido is at least `100.0`. |
| One-handed or two-handed melee weapon without shield | `(Parry * Bushido) / divisor`, where divisor is `48000.0` for one-handed weapons and `41140.0` for two-handed weapons. The AOS fallback chance is `Parry / 800.0`. Parry `>= 100.0` adds `0.05` to both values; otherwise Bushido `>= 100.0` adds `0.05` only to the Bushido formula. |

If Evasion is active, the calculated chance is multiplied by `Evasion.GetParryScalar(defender)`. If Dexterity is below `80`, the chance is multiplied by `(20 + Dex) / 100`.

On a successful block, damage is set to zero. The block removes an Honorable Execution penalty, triggers `CounterAttack` if active, and grants the `Confidence` block reward if active.

## Honor And Perfection Integration

The Honor virtue context is not stored in the Bushido folder, but Bushido skill drives the Perfection values used by melee damage and loot luck.

| Event | Perfection behavior |
| --- | --- |
| Honored target hit by source | If Bushido is at least `50.0`, adds `floor(Bushido / 10)` perfection, capped at `100`. |
| Honored target missed by source | Subtracts `25` perfection, bottoming at `0`. |
| Damage against honored target | Adds `PerfectionDamageBonus` directly to `BaseWeapon` percentage damage bonus. |
| Loot luck against honored target | Adds `PerfectionLuckBonus = Perfection^2 / 10` to killer luck. |
| Honored target killed | If perfection is positive, restores Hits, Stamina, and Mana by `min(Perfection * (target.Fame + 5000) / 25000, 10)`. |

## Serialization Notes

| Class | Version | Serialized data after version |
| --- | ---: | --- |
| `BookOfBushido` | 1 | No custom fields. |
| `MySamuraibook` | 0 after base | No custom fields beyond the base `BookOfBushido` serialization. |
| `Confidence`, `Evasion`, `CounterAttack` | None | Active stance timers are static runtime tables and are not serialized. |
| `HonorableExecution`, `LightningStrike`, `MomentumStrike` | None | Current special moves and Honorable Execution effects are static runtime tables and are not serialized. |

`BookOfBushido` writes version `1` even though it has no custom fields. This is a save-format quirk rather than an active read/write mismatch, because `Deserialize` reads the same version integer immediately after the base spellbook state.

## Admin Notes

No Bushido-specific `[Command]`, `[Usage]`, or `[Description]` handlers were found. Staff can use the shared spellbook infrastructure:

| Command | Access | Usage | Notes |
| --- | --- | --- | --- |
| `AllSpells` | `GameMaster` | `[AllSpells` then target a spellbook | Shared `Spellbook` command. When targeted at a `BookOfBushido`, it fills the book with all six Samurai ability bits because `BookCount` is `6`. |

`BookOfBushido` and `MySamuraibook` are `[Constructable]` Items, so they can also be created through the shard's normal item-add tooling.

## Known Issues

* Successful `HonorableExecution` swing-speed bonuses do not expire after their intended 20-second timer. The timer calls `EndEffect`, but `EndEffect` delegates to `RemovePenalty`, and `RemovePenalty` returns immediately for non-penalty entries.
* `Confidence.RegenTimer` adds the final remainder at tick five, stops the timer, and then still adds `m_Hits / 5` again in the same tick. The actual healing is therefore one fifth higher than the computed `m_Hits` pool.
* `MomentumStrike.OnHit` iterates `attacker.GetMobilesInRange(weapon.MaxRange)` directly. `Mobile.GetMobilesInRange` returns an `IPooledEnumerable`, so this path should free the enumerable after use.
* `LightningStrike.Validate` stores unscaled `BaseMana` in `PlayerMobile.ExecutesLightningStrike`, and `PlayerMobile.OnManaChange` clears the move when mana falls below that base value. Lower Mana Cost and other `ScaleMana` effects can therefore be ignored by this pre-hit clear path.
* `HonorableExecution.GetDamageScalar` has a source `TODO` for `20 -> Perfection`, but the compiled damage scalar uses a hard-coded `20` multiplier rather than the active Perfection value.
