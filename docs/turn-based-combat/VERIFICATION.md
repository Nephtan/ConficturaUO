# Turn-Based Combat Verification Record

Date: 2026-08-01

The feature is implemented but disabled by default in `Data/TurnBasedCombat/TurnBasedCombat.cfg`. Production activation remains gated on the manual gameplay, active-save/restart, rollback, and mass-combat soak scenarios in `IN_GAME_TEST_MATRIX.md`.

## Completed checks

| Check | Result | Evidence |
| --- | --- | --- |
| Compatibility regeneration | Pass | `scripts/Generate-TurnBasedCombatCompatibility.ps1` scanned 6,598 runtime scripts and generated a register with zero `Unknown` rows. |
| Compatibility drift gate | Pass | An isolated runtime configuration load accepted the generated register and current SHA-256 source hashes. |
| Deterministic configuration checks | Pass | Loaded defaults were 20 AP, five-second actor interval, eight effect rules, seven AI rules, and a two-AP sample movement action. |
| Operational core build | Pass | `Data/System/Source/Server.csproj`, `Debug|x86`, built with Visual Studio 2022 MSBuild. |
| Runtime script compile | Pass | `ConficturaServer.exe -compileonly -nocache` reported `Scripts: Compile-only verification completed successfully.` |
| Visual Studio project hygiene | Pass | `ConficturaUO.sln`, `Debug|Any CPU`, built both Server and Scripts; existing legacy and x86/MSIL warnings remain. |
| Project/source truth | Pass | All five `Data/Scripts/Custom/Combat/TurnBased/*.cs` files exist, are included in `Scripts.csproj`, and have no missing/unincluded drift. |
| Serialization policy | Pass (static) | No combat-group, AP, initiative, lease, or actor-clock state was added to RunUO serializers. Existing `BaseCreature` summon fields remain canonical. |

## Runtime initialization blocker

An isolated `-service -nocache` startup reaches successful runtime compilation but cannot complete script initialization in the current non-interactive test process. The pre-existing `CloneOfflinePlayerCharacters.CheckFirstRun()` startup path calls console buffer APIs and throws `System.IO.IOException: The handle is invalid` before the listener and in-shard self-test milestones.

This blocker is outside the turn-based combat files and was not changed as part of this implementation. Consequently, this record does not claim successful full initialization, live gameplay, active-combat save/restart, rollback rehearsal, or performance soak. Those gates must pass in an interactive isolated shard before changing `Enabled=false`.

## Required activation sequence

1. Start an isolated shard in a console environment where clone initialization completes.
2. Run `[TurnCombat Compatibility` and require zero unknown or drift entries.
3. Run `[TurnCombat SelfTest` and the scenarios in `IN_GAME_TEST_MATRIX.md`.
4. Rehearse active-combat save, process restart, emergency disable, and executable/script rollback against a backup.
5. Complete the uncapped mass-combat soak and confirm bounded AI slice behavior.
6. Deploy matching executable, scripts, and catalogs while disabled; enable only during the scheduled launch.
