# Turn-Based Combat Verification Record

Date: 2026-08-01

The feature is implemented but disabled by default in `Data/TurnBasedCombat/TurnBasedCombat.cfg`. Production activation remains gated on the manual gameplay, active-save/restart, rollback, and mass-combat soak scenarios in `IN_GAME_TEST_MATRIX.md`.

## Completed checks

| Check | Result | Evidence |
| --- | --- | --- |
| Compatibility regeneration | Pass | `scripts/Generate-TurnBasedCombatCompatibility.ps1` scanned 6,598 runtime scripts and generated a register with zero `Unknown` rows. |
| Compatibility drift gate | Pass | An isolated runtime configuration load accepted the generated register and current SHA-256 source hashes. |
| Canonical hash portability | Pass | LF, CRLF, BOM+LF, and BOM+CRLF variants produced one canonical SHA-256 hash, while a source-token change produced a different hash. Register regeneration was byte-for-byte reproducible with zero classification drift. |
| Deterministic configuration checks | Pass | Loaded defaults were 20 AP, five-second actor interval, eight effect rules, seven AI rules, and a two-AP sample movement action. |
| Operational core build | Pass | `Data/System/Source/Server.csproj`, `Debug|x86`, built with Visual Studio 2022 MSBuild. |
| Runtime script compile | Pass | `ConficturaServer.exe -compileonly -nocache` reported `Scripts: Compile-only verification completed successfully.` |
| Isolated runtime initialization | Pass | A copied current save completed script initialization, reported `Cloning Offline Players... 4/4`, and reached `Console ready` on test port `14508` without the prior console-handle exception. |
| Staff commands and self-test | Pass | An isolated `ServerStarted` probe invoked `TurnCombat Status`, `Compatibility`, and `SelfTest` through `CommandSystem`; all were handled, compatibility passed, and deterministic self-tests reported `PASS`. |
| Clone startup integrity | Pass | The isolated loaded world contained 753 clones with zero duplicate originals, zero clones in excluded regions, and zero player originals left outside their logout location on `Map.Internal`. |
| Visual Studio project hygiene | Pass | `ConficturaUO.sln`, `Debug|Any CPU`, built both Server and Scripts; existing legacy and x86/MSIL warnings remain. |
| Project/source truth | Pass | All five `Data/Scripts/Custom/Combat/TurnBased/*.cs` files exist, are included in `Scripts.csproj`, and have no missing/unincluded drift. |
| Serialization policy | Pass (static) | No combat-group, AP, initiative, lease, or actor-clock state was added to RunUO serializers. Existing `BaseCreature` summon fields remain canonical. |

## Runtime initialization gate

The gate was cleared by replacing the cursor-positioning progress display in `CloneOfflinePlayerCharacters.CheckFirstRun()` with sequential service-safe output. Clone discovery, creation, cleanup, and player logout restoration were not changed.

The isolated smoke used a temporary copy of the current save and runtime tree, changed the listener from production port `4508` to test port `14508`, and kept `Enabled=false`. A temporary test-only script in that copied tree exercised the three staff command handlers and inspected runtime clone state; it was never added to the repository. The process was stopped without saving after the listener and probe milestones.

This record still does not claim live client gameplay, active-combat save/restart, rollback rehearsal, or performance soak. Those gates must pass before changing `Enabled=false`.

## Required activation sequence

1. Run the vertical gameplay slice in `IN_GAME_TEST_MATRIX.md`: one player versus one stock creature, then repeat with a controlled pet.
2. Complete the remaining native action, effect, AI, and legality scenarios.
3. Rehearse active-combat save, process restart, emergency disable, and executable/script rollback against a backup.
4. Complete the uncapped mass-combat soak and confirm bounded AI slice behavior.
5. Deploy matching executable, scripts, and catalogs while disabled; enable only during the scheduled launch.
