# Phase 5: Runtime Hook Map

## Purpose

Phase 5 maps every way code enters runtime behavior. Hooks are the true wiring
of the shard: commands, initialization, packet handlers, events, timers, gumps,
regions, speech, movement, and world load/save can connect systems even when
folders look separate.

## Required Inputs

- Runtime marker scans.
- CrossTreeRuntimeInventory.
- System Cards.
- Project Truth Register.

## Required Outputs

- Runtime Hook Map.
- Global hook risk list.
- Command surface register.
- Packet handler register.
- Gump response risk register.
- Timer and world hook register.

## Hook Map Table

| Field | Meaning |
| --- | --- |
| `System` | Owning system. |
| `File` | Source file. |
| `HookType` | Initialize, command, event, packet, timer, gump, region, speech, movement, login, logout, world save/load. |
| `Registration` | Registration or override expression. |
| `Handler` | Method invoked. |
| `Access` | Player, Counselor, GM, Admin, Seer, internal, or global. |
| `Trigger` | What causes execution. |
| `Guards` | Null, deleted, map, range, access, bounds, state checks. |
| `Risk` | Low, medium, high, critical. |
| `Docs` | Documentation page that explains it. |

## Subphase 5.1: Initialization Map

1. Find every `public static void Initialize`.
2. Record command, event, packet, timer, and service startup done inside it.
3. Identify duplicate command names or overwritten handlers.
4. Identify startup code inside obsolete or legacy files.

Completion gate:

- Startup side effects are visible before code moves.

## Subphase 5.2: Command Surface

For every `CommandSystem.Register`:

1. Record command name.
2. Record access level.
3. Record handler.
4. Record usage and description attributes where present.
5. Record whether handler validates `Mobile`, `PlayerMobile`, range, target,
   arguments, and state.

Completion gate:

- Player and staff command surfaces can be reviewed separately.

## Subphase 5.3: EventSink Hooks

For every `EventSink` hook:

1. Record event type.
2. Record subscription location.
3. Record handler.
4. Determine whether it can duplicate subscription.
5. Determine whether it unsubscribes or persists for process lifetime.

High-priority hooks:

- Login.
- Logout.
- WorldSave.
- WorldLoad.
- Speech.
- Movement.
- Character creation.

Completion gate:

- Global event behavior is mapped to owning systems.

## Subphase 5.4: Packet Handlers

For every packet registration:

1. Record packet ID.
2. Record length.
3. Record encoded/unencoded mode.
4. Record handler.
5. Verify explicit reads from `PacketReader`.
6. Check access, state, and malformed packet guards.

Completion gate:

- Packet handlers are treated as high-risk network entry points.

## Subphase 5.5: Gump Response Paths

For every `OnResponse`:

1. Record button IDs.
2. Record text entry IDs.
3. Record switches and fallthrough.
4. Check null `NetState` and `Mobile`.
5. Check stale target and deleted object guards.
6. Check bounds for list indexes.

Completion gate:

- Gump behavior is auditable from button to side effect.

## Subphase 5.6: Timer And World Hooks

Record:

- Custom `Timer` subclasses.
- `Timer.DelayCall`.
- timers recreated in `Deserialize`.
- world save/load hooks.
- cleanup timers.
- hot-reload timers.

Completion gate:

- Long-running and recurring behavior is visible.

## Review Checklist

- Commands separated by access level.
- Packet handlers reviewed as critical.
- Global hooks have owners.
- Gump responses have button and state maps.
- Timers have lifecycle notes.

## Exit Criteria

Phase 5 is complete when a maintainer can start from any runtime trigger and
trace which system handles it, what state it touches, and which guards protect
it.
