# Turn-Based Combat Verification Record

Date: 2026-08-03

The PvP-only feature is implemented but disabled by default in `Data/TurnBasedCombat/TurnBasedCombat.cfg`. Ordinary PvE remains native. Production activation remains gated on the manual gameplay, active-save/restart, rollback, and mass-PvP soak scenarios in `IN_GAME_TEST_MATRIX.md`.

## Completed checks

| Check | Result | Evidence |
| --- | --- | --- |
| Compatibility regeneration | Pass | `scripts/Generate-TurnBasedCombatCompatibility.ps1` scanned 6,598 runtime scripts and generated a register with zero `Unknown` rows. |
| Compatibility drift gate | Pass | An isolated runtime configuration load accepted the generated register and current SHA-256 source hashes. |
| Canonical hash portability | Pass | LF, CRLF, BOM+LF, and BOM+CRLF variants produced one canonical SHA-256 hash, while a source-token change produced a different hash. Register regeneration was byte-for-byte reproducible with zero classification drift. |
| Deterministic configuration checks | Static/compile pass; in-shard rerun required | Defaults are `PvPOnly`, 20 AP, five actor-seconds, 12-tile LOS joining, 18-tile disengagement, nine effect rules, and seven AI rules. Self-tests now cover PvE-native classification, join/prune boundaries, cooldown rebasing, exact tick counts, and running-flag cleanup. |
| Bridge reliability checks | Pass | Deterministic self-tests verified null participant lookup, actorless mutation authorization, and registered/healthy handler reporting. Static bridge review verified first-fault latching, one emergency-disable schedule, post-fault action/mutation/notification blocking, and restart-only recovery. The isolated runtime probe reported `bridge=True` while combat remained disabled. |
| Combatant intent checks | Static/compile pass; live client required | Group seeding requires different effective player principals (or one process-local armed test NPC). Wild NPC joins require an actual legal interaction within 12 tiles and LOS. Edge pruning and component splitting preserve participant state. |
| Operational core build | Pass | `Data/System/Source/Server.csproj`, `Debug|x86`, built with Visual Studio 2022 MSBuild. |
| Runtime script compile | Pass | `ConficturaServer.exe -compileonly -nocache` reported `Scripts: Compile-only verification completed successfully.` |
| Isolated runtime initialization | Pass | A temporary ignored runtime/current-save copy compiled and initialized while `Enabled=false`; the final probe used gameplay port `14520`, reached `ServerStarted`, completed its checks, and shut down without saving. The temporary copy was removed. |
| Deterministic self-test | Pass locally; owner preflight still required | An isolated `ServerStarted` probe reported `enabled=False`, mode `PvPOnly`, zero groups, zero test arms, bridge ready, compatibility pass, and deterministic self-test `PASS`. The same results remain required on `C:\ConficturaUO-Test`. |
| Clone startup integrity | Pass | The isolated loaded world contained 753 clones with zero duplicate originals, zero clones in excluded regions, and zero player originals left outside their logout location on `Map.Internal`. |
| Visual Studio project hygiene | Pass | `ConficturaUO.sln`, `Debug|Any CPU`, built both Server and Scripts; existing legacy and x86/MSIL warnings remain. |
| Project/source truth | Pass | All five `Data/Scripts/Custom/Combat/TurnBased/*.cs` files exist, are included in `Scripts.csproj`, and have no missing/unincluded drift. |
| Serialization policy | Pass (static) | No combat-group, AP, initiative, lease, or actor-clock state was added to RunUO serializers. Existing `BaseCreature` summon fields remain canonical. |

## Runtime initialization gate

The gate was cleared by replacing the cursor-positioning progress display in `CloneOfflinePlayerCharacters.CheckFirstRun()` with sequential service-safe output. Clone discovery, creation, cleanup, and player logout restoration were not changed.

The original gate used a temporary copy with port `14508` and a test-only command/clone probe. The final PvP-only build was independently started from another ignored current-save copy on port `14520` with `Enabled=false`; it reached `ServerStarted`, reported the healthy bridge/gate/self-test state, stopped without saving, and was removed. Current-build staff-command and gameplay checks remain owner-run gates on the test shard.

This record still does not claim live client gameplay, active-combat save/restart, rollback rehearsal, or performance soak. Those gates must pass before changing `Enabled=false`.

## Required activation sequence

1. Verify an ordinary fox, Orc, and random-encounter creature remain real time with no HUD and zero groups.
2. Test direct PvP in both attack orders, then player/pet, pet/player, and pet/pet initiation.
3. Test bounded wild-NPC joining, automatic disengagement/splitting, running cleanup, exact resource regeneration, poison expiry/cure, and bandage exit rebasing.
4. Complete the PvP consent, Government, guild, AoE, XMLPoints/challenge, and structured-event legality matrices.
5. Rehearse active-combat save, process restart, emergency disable, and executable/script rollback against a backup.
6. Complete the mass-PvP soak and confirm bounded AI slice behavior.
7. Deploy matching executable, scripts, and catalogs while disabled; enable only during the scheduled launch.
