# Dynamic Turn-Based Combat

This system keeps the shard world and ordinary PvE in real time. Turn combat begins only when a legal hostile action crosses two different player-controlled sides; players, controlled pets, and summons resolve through their effective player principal. Once PvP has seeded a group, a wild NPC can join only through a legal hostile interaction within the configured range and line of sight.

The feature is disabled by default in `Data/TurnBasedCombat/TurnBasedCombat.cfg`. Before enabling it, regenerate `compatibility-register.csv`, run the in-shard self-test, build the source server, force a runtime script compile, and rehearse save/restart and rollback against an isolated copy of a current world save.

## Compatibility register

Run from the repository root:

```powershell
./scripts/Generate-TurnBasedCombatCompatibility.ps1
```

The generator scans every live runtime `.cs` file under `Data/Scripts`, excluding generated `bin` and `obj` trees. `Scripts.csproj` remains IDE/project hygiene rather than the runtime boundary.
It seeds system ownership from the completed codebase-audit runtime inventory and records a canonical SHA-256 hash for every runtime script. Hashing reads source as text, normalizes CRLF and lone CR to LF, and encodes UTF-8 without a byte-order mark. Line-ending or BOM-only deployment transformations therefore remain compatible, while all other whitespace and source changes still produce drift. Enable is refused when a script is added, removed, or meaningfully changed after generation.
The generator also fails the gate when a runtime script directly mixes a participant cooldown (`NextActionTime`, `NextSkillTime`, `NextSpellTime`, or `NextCombatTime`) with `DateTime.Now` instead of the actor-time bridge.

## Runtime controls

- `[TurnCombat Status`
- `[TurnCombat Compatibility`
- `[TurnCombat Inspect`
- `[TurnCombat SelfTest`
- `[TurnCombat Reload`
- `[TurnCombat Enable`
- `[TurnCombat Disable`
- `[TurnCombat ArmTest`
- `[TurnCombat DisarmTest`
- `[TurnCombat EndTurn`
- `[TurnCombat Escape`
- `[EndTurn`
- `[EscapeCombat`

Enable is refused when the compatibility register is missing, contains an `Unknown` disposition, or the core bridge is unregistered or faulted. `[TurnCombat Status` reports `bridge: ready` before activation. A bridge fault blocks participant-affecting operations, schedules one emergency dissolve, and requires a process restart before re-enabling. Reload is refused while groups exist. Emergency disable dissolves groups and restores adapted native timers and combat scheduling.

`ArmTest` targets one living non-player mobile and permits that mobile to seed a PvE regression group. This exception is process-local, appears in Status, never serializes, and is cleared by Disable or restart.

The deterministic command validates bridge health, null-safe mutation handling, combatant-intent decisions, locked defaults, AP conversion, initiative math, catalog loading, travel blocking, CSV parsing, and source-hash compatibility. Run the operational scenarios in `IN_GAME_TEST_MATRIX.md` on an isolated world copy before enabling the feature.

## Persistence and rollback

Combat groups, AP, initiative, action leases, and actor clocks are never serialized. Canonical mobile, creature, spell, item, ownership, and summon fields retain their existing formats. An ordinary world save does not dissolve an in-memory group; a process restart intentionally does.
