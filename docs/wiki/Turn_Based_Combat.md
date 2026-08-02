# Turn-Based Combat

## Current Status

Dynamic turn-based combat is implemented but disabled by default. The shard continues to use normal real-time combat unless staff have enabled this system for an isolated test. Production activation remains gated on the gameplay, active-save/restart, rollback, and mass-combat tests in the [in-game verification matrix](../turn-based-combat/IN_GAME_TEST_MATRIX.md).

The values in this page are the current configured defaults. Staff can change them in the turn-combat configuration, so the values used during a scheduled test may differ.

## Player Guide

### How Combat Starts

A legal hostile attack or other harmful action between two living characters opens a combat group. The opening action does not land before initiative: combat begins, everyone rolls, and the attacker must act again when their turn arrives. Existing shard rules still decide whether an action is legal, including PvP consent, regions, guild relations, pets, summons, and event restrictions.

Nearby owners and followers can enter with the character or creature that brought them into combat. Someone who attacks, helps, or is targeted after a group has started normally joins for the next round. If an action connects characters from two existing groups, the groups merge without granting duplicate turns.

Combat ends when no living hostile relationship remains. Individual participants also leave when they die, are deleted, escape, remain disconnected beyond the grace period, or are relocated in a way that is not part of a valid short tactical action.

### Initiative And Rounds

Each participant rolls initiative once when joining:

`initiative = d20 + (Dexterity / 10)`

Integer division is used for the Dexterity bonus. Higher totals act first. A tied total is resolved by higher raw Dexterity, then by the lower mobile Serial. The initiative order remains visible in the combat HUD; a participant marked `next round` cannot act until the group advances.

At the current defaults, an actor begins each turn with 20 action points (AP), and a player has 30 real-time seconds to act. AP reaching zero, choosing End Turn, disconnecting, or reaching the timeout passes control to the next eligible participant. A timeout cancels any open target cursor before advancing the turn.

### Combat HUD

Every connected player in the group receives a HUD that refreshes automatically. It shows:

- Group and round number.
- The current actor.
- Your remaining AP and pending action.
- Poison, paralysis, and freezing affecting your character.
- The initiative list and next-round markers.

The HUD buttons are:

- **End Turn:** Gives up all remaining AP and advances combat. The player command `[EndTurn` does the same thing.
- **Escape:** Attempts to leave the group. The player command `[EscapeCombat` performs the same check.
- **Refresh:** Reopens the HUD with current information.

Closing the HUD does not leave combat; it reopens on the next group refresh.

### Action Point Costs

| Action | Current AP rule |
| --- | --- |
| Turn in place | 0 AP |
| Take a step | Native movement delay divided by 0.25 seconds, rounded up; minimum 1 AP |
| Weapon attack | Native weapon delay divided by 0.25 seconds, rounded up and capped at the turn's AP maximum |
| Cast a spell | Cast delay plus cast recovery divided by 0.25 seconds, rounded up and currently clamped to 1-20 AP |
| Use a skill | 4 AP |
| Use an item or mobile | 4 AP |
| Lift, drop, or equip an item | 2 AP |
| End turn or escape | All remaining AP |

An action normally reserves its AP before native game checks complete. Canceling a target cursor or selecting an invalid target refunds the reservation. A legal attempt that misses, fizzles, or is resisted still consumes its cost. Rejected movement and inventory actions refund their cost, so an unsuccessful drop does not sacrifice the held item.

You cannot begin another unrelated action while a target is pending. The target cursor does not extend the 30-second turn deadline.

### Travel And Escape

Long-range travel is unavailable while participating in turn-based combat. The blocked catalog currently includes Recall, Gate Travel, Sacred Journey, Nature's Passage, Mushroom Gateway, Ethereal Travel, Astral Travel, and their cataloged potion or scroll variants. A blocked attempt is refunded.

Escape is allowed only during your turn while you still have AP. Every living hostile connected to you must be both farther than the current 18-tile escape range and outside your line of sight. A successful escape consumes the rest of your AP and removes you from the group; it does not move your character.

### Effects, Pets, And Disconnects

The world outside a combat group stays in real time. For participants, the current five-second actor interval advances that participant's local combat clock as turns return to them. Regeneration, poison, paralysis, freezing, summon expiry, and relevant cooldowns use that actor clock and return safely to native timers when the participant leaves combat or staff disable the system.

Creatures and controlled followers receive their own initiative and AP. Their existing AI and legality checks remain authoritative; the turn manager gives classified AI a bounded decision slice rather than replacing its behavior.

Disconnecting during your turn passes the turn immediately. Reconnecting within the current five-minute grace period keeps your place for later eligibility. Remaining offline beyond that grace period removes you from the group.

## Staff Guide

### Configuration And Catalogs

The system loads [TurnBasedCombat.cfg](../../Data/TurnBasedCombat/TurnBasedCombat.cfg), [TurnActions.csv](../../Data/TurnBasedCombat/TurnActions.csv), [TurnEffects.csv](../../Data/TurnBasedCombat/TurnEffects.csv), and [TurnAI.csv](../../Data/TurnBasedCombat/TurnAI.csv). The configuration owns startup enablement, AP, actor interval, player and disconnect timeouts, escape range, AI/scheduler limits, HUD refresh, compatibility gating, and file logging. The CSV catalogs classify actions, actor-clock effects, and supported AI.

`Enabled=true` requests activation during server initialization, but activation still must pass the compatibility and bridge gates. `[TurnCombat Enable` and `[TurnCombat Disable` change only the current process state; they do not rewrite the configuration file. `[TurnCombat Reload` rereads the files without changing the current enabled state and is refused while any combat group exists.

The checked-in default remains `Enabled=false` and `RequireCompatibilityGate=true`. Do not bypass the gate or enable production until the full verification matrix, a real-save backup, and rollback rehearsal have been signed off.

### Commands

`[TurnCombat` and all of its subcommands require Administrator access. With no subcommand it performs `Status`.

| Command | Staff behavior |
| --- | --- |
| `[TurnCombat Status` | Reports runtime enabled state, active group count, catalog version, compatibility result, and whether the core bridge is ready, unregistered, or faulted. |
| `[TurnCombat Compatibility` | Runs the compatibility check and reports its result plus loaded effect and AI rule counts. |
| `[TurnCombat Inspect` | Targets a participant and reports group, round, initiative, eligible round, AP, current-actor state, and pending action. |
| `[TurnCombat Enable` | Enables the current process only if the bridge is ready and the compatibility gate passes. |
| `[TurnCombat Disable` | Emergency rollback: disables the current process, dissolves every group, refunds pending actions, and restores native timers and combat scheduling. |
| `[TurnCombat Reload` | Reloads configuration and catalogs when there are zero active groups. |
| `[TurnCombat SelfTest` | Runs deterministic bridge, mutation, initiative, AP, catalog, travel, CSV, and compatibility checks. It does not replace the in-game matrix. |
| `[TurnCombat ForceEnd` | Targets the current actor and ends that turn. Targeting a non-current participant is refused. |
| `[TurnCombat ForceDissolve` | Targets any participant and dissolves that participant's entire group. |
| `[TurnCombat EndTurn` | Runs the normal End Turn action for the Administrator's own participant. |
| `[TurnCombat Escape` | Runs the normal escape check for the Administrator's own participant. |

`[EndTurn` and `[EscapeCombat` require only Player access and are the normal player equivalents of the HUD buttons.

### Compatibility, Failure Handling, And Logs

When the compatibility gate is required, enablement scans every runtime `.cs` file under `Data/Scripts`, excluding generated `bin` and `obj` trees, and compares it with [compatibility-register.csv](../turn-based-combat/compatibility-register.csv). Hashing normalizes line endings and an optional byte-order mark; meaningful source changes, added or missing scripts, malformed rows, conflicting hashes, or any `Unknown` disposition block enablement. Regenerate the register only for a reviewed authoritative source change, never merely to bless unexplained deployment drift.

The runtime fails closed around participant actions and state changes. An unclassified action is blocked, unsupported AI ends its current turn and logs the classification failure, and an unclassified actor-clock effect triggers emergency disable. An exception crossing the core bridge latches a fault, blocks participant-affecting work, schedules one emergency dissolve, and prevents re-enabling until the process restarts.

Events are always written to the server console. With the current `LogEnabled=true`, they are also appended to `Logs/TurnBasedCombat.log`. Useful event names include `enabled`, `disabled`, `group_opened`, `initiative`, `turn_started`, `action_reserved`, `action_committed`, `action_refunded`, `participant_removed`, `group_dissolved`, `unknown_action_blocked`, `unknown_ai_blocked`, and `self_test`.

### Saves, Restarts, And Rollback

Combat groups, rounds, initiative, AP, action leases, and actor clocks are in-memory state and are not serialized. An ordinary world save records the shard's canonical mobile, creature, item, spell, ownership, and summon state without dissolving active groups. A shutdown dissolves groups, and a restart always begins with zero groups.

This persistence boundary allows rollback to the prior matching executable, scripts, and catalogs without a turn-combat save migration. It does not remove the need to rehearse active-combat save/restart and rollback on an isolated copy of a current world save.

Operational references:

- [Engineering README](../turn-based-combat/README.md)
- [Verification record](../turn-based-combat/VERIFICATION.md)
- [Compatibility summary](../turn-based-combat/COMPATIBILITY_SUMMARY.md)
- [In-game verification matrix](../turn-based-combat/IN_GAME_TEST_MATRIX.md)

## Source Trace

Reviewed against the current working tree on 2026-08-02.

### Core And Runtime Entry Points

- [Data/System/Source/TurnBasedCombat.cs](../../Data/System/Source/TurnBasedCombat.cs): `ITurnBasedCombatHandler`, `TurnBasedCombatBridge`, action leases, mutation scopes, participant-local time, and latched bridge-fault handling.
- [Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatManager.cs): `Initialize`, group lifecycle, initiative, AP reservations, turns, AI slices, effects, escape, event hooks, saves, shutdown, and logging.
- [Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatCommands.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatCommands.cs): `Initialize` registrations for Administrator `[TurnCombat` and Player `[EndTurn`/`[EscapeCombat`.
- [Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatGump.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatGump.cs): HUD construction and response IDs `1` End Turn, `2` Escape, `3` Refresh, and `1000+` initiative pages.
- [Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatConfiguration.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatConfiguration.cs): configuration/catalog loading, validation, rule resolution, and compatibility hashing.
- [Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatSelfTest.cs](../../Data/Scripts/Custom/Combat/TurnBased/TurnBasedCombatSelfTest.cs): deterministic staff self-test coverage.

### Native Action And Effect Adapters

- [Data/System/Source/Mobile.cs](../../Data/System/Source/Mobile.cs): combatant selection, movement, attacks, item/mobile use, inventory actions, participant mutations, relocation, deletion, status timers, and scheduling restoration.
- [Data/System/Source/Skills.cs](../../Data/System/Source/Skills.cs), [Data/Scripts/Magic/Base/Spell.cs](../../Data/Scripts/Magic/Base/Spell.cs), and [Data/System/Source/Targeting/Target.cs](../../Data/System/Source/Targeting/Target.cs): skill/spell reservations, actor-local cooldown time, target completion, and refunds.
- [Data/Scripts/Mobiles/Base/Behavior.cs](../../Data/Scripts/Mobiles/Base/Behavior.cs): bounded turn-based AI pulse entry point.
- [Data/Scripts/System/Misc/Poison.cs](../../Data/Scripts/System/Misc/Poison.cs): participant-local poison ticks.
- [Data/Scripts/Mobiles/Base/BaseCreature.cs](../../Data/Scripts/Mobiles/Base/BaseCreature.cs) and [Data/Scripts/Magic/Base/UnsummonTimer.cs](../../Data/Scripts/Magic/Base/UnsummonTimer.cs): summon expiry suspension and restoration.

### Runtime And Persistence Coverage

The five custom turn-combat scripts are listed in [Data/Scripts/Scripts.csproj](../../Data/Scripts/Scripts.csproj) for Visual Studio project hygiene. Live script visibility comes from recursive runtime compilation under `Data/Scripts`, while the bridge is compiled into the server through [Data/System/Source/Server.csproj](../../Data/System/Source/Server.csproj). No turn-combat class adds a RunUO `Serialize` or `Deserialize` contract.
