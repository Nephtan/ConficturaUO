# Copied-client site traversal - 2026-10-29

Status: **Partial acceptance: five of six routes complete.** These tests do not approve reservations or activate outbreaks. All six review flags and the production delivery gate remain false.

## Method and evidence boundary

Private root: `D:/ConficturaUOTestTools/outbreak-sites-20261029-001`. The matching uninstrumented core is the accepted input-throttle build, SHA-256 `dbb7b2360c435834b32e0f1f4b0854a859cd9086971a4c270981b58d8f2f3ddb`. Runtime scripts are that candidate plus the existing eight isolation overlays and one guarded test-only fixture. The fixture remains outside the repository's production script tree. Its final copied source passed forced recursive runtime compilation; its private compatibility scan classified6,612 scripts with zero unknown dispositions. The first two compile attempts failed because a helper named `Path` shadowed `System.IO.Path`; the corrected helper is `FindPath`. Service diagnostics are retained in the private `Data/Logs/Console.log`, and the successful non-service compile is `evidence/runtime-compile-attempt-3.log`.

The fixture loads the existing isolated client005 copied world, checks structural exclusions, and builds ground routes using actual `Movement.CheckMovement` for an unmounted Player who cannot swim. Each route starts on the north combat boundary, visits all three fixed brood anchors and the boss anchor, and ends on the south boundary. Counts below are actual movement reachability in this copy, distinct from the earlier spawnable-tile survey and from current production occupancy.

Only starting-point setup uses `MoveToWorld`. Normal ClassicUO Walk macros, started through native Computer Use keyboard input, perform route translation through ordinary movement packets. The server fixture records positions and never follows a route for the player. A two-step calibration verifies mapping, turn-before-step behavior and delay before the full walks. The player has travel-only invulnerability; background wildlife/monsters, lighting and weather remain active. These are access tests, not combat, AI difficulty or pacing tests.

| Site | Movement-connected tiles | Planned walking steps | Current result |
| --- | ---: | ---: | --- |
| central-woodland | 12,478 | 226 | Complete, zero deviations |
| northern-woodland | 12,534 | 224 | Complete on retry, zero deviations |
| eastern-woodland | 12,439 | 228 | Complete, zero deviations |
| northwestern-woodland | 11,553 | 228 | Complete, zero deviations |
| southeastern-woodland | 12,422 | 224 | Both attempts failed; unresolved |
| western-woodland | 12,636 | 226 | Complete, zero deviations |

`site-routes.csv`, `site-events.csv` and `site-state.json` contain plans, all attempts and transient telemetry. The final verifier matched every position, order, connected-client flag and checkpoint in the latest complete attempts:1,132 walking steps across five sites. It reports `passed=false` for the six-site gate and `preservation_passed=true` in `evidence/site-verification.json`. The southeastern latest attempt contains214 actual moves but only its first63 match the plan;151 deviations are retained. Its first attempt matched119 steps before104 deviations. A complete walking result does not establish that every tile in the reservation is usable.

The exact fixture/core identity and all6,742 production source inputs match. Original saves/settings/profiles and all20 private save files remain byte-identical. The fixture recorded its forced no-save stop at09:18:32Z; host process identity checks then confirmed both owned processes and the private54603 listener were gone. The copied client was forcibly stopped; no graceful logout is claimed. Evidence: `site-forced-stop.txt`, `final-process-check.json`, `protected-source-inputs.json`, `prepared.json` and `site-verification.json`.

## Native visual observations

These are observations from the displayed client, paired with route telemetry. They are not generated map images or claims that every anchor/effect was visually tested.

- Central: the north/south approaches are traversable; foliage briefly obscures the character near `(2521,2087)`. Earlier central-site web presentation evidence remains separate in [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md).
- Northern: brood0 `(2214,632)` and brood1 `(2270,632)` have visible standing space. Near the boss, the view at `(2242,653)` places a large bare tree over the character. The exit also has foreground branches. Boss/web visibility remains open.
- Eastern: brood0 `(3718,856)` sits beneath dense canopy and partially conceals the character. Brood1 `(3774,856)` is comparatively open. The boss `(3746,876)` is visible beside a small pond, and brood2 `(3746,906)` is visible on the edge of the denser forest. The first brood needs a combat-scene visibility check or revised placement.
- Northwestern: brood0 `(1180,908)` is visible with the river/cliff to the west. The boss `(1208,928)` has visible standing space beside the river. The captured eastern/southern approaches were open; their exact brood anchor positions are established by the trace, rather than claimed as exact-point visual captures. The full route reaches the opposite boundary despite the water edge.
- Southeastern: brood0 `(3734,2724)` is a broad clearing. The failed first attempt's later view was off the planned route and is not accepted as an exact boss-anchor inspection.
- Western: brood0 `(1094,1684)` and brood1 `(1150,1684,2)` are visible. At the boss `(1122,1704,2)`, a bare tree overlaps the standing character. The later southern approach is open, but the earlier exact southern-anchor foliage concern is not closed by an approach capture.

Do not promote these proposals merely because a route can be walked. In particular, a spawnable5x5 patch does not prove that a ground warning will remain readable beneath the canopy. Revisit the northern/western boss positions and eastern first brood with actual encounter visuals; prefer clear placements within the agreed encounter area. Do not remove world trees or require a client foliage modification to manufacture acceptance. The new reservations still need a fresh deployment-world occupancy/maximum-city-growth preflight and the existing malformed legacy controller's disposition.

## Retained failures and limits

The first calibration crashed the copied client: generated Delay actions used `str` rather than the required `text` XML attribute, creating the wrong macro object type. The rejected XML, client crash log and `client-calibration-failure.json` remain private. The corrected two-step calibration and subsequent routes use the documented client format; this was a fixture error, not a shard defect.

The first northern run stopped at step219 of224 with no deviation. The unchanged retry completed. The first southeastern run missed the final eastward step before brood1, then its fixed macro continued from a position one tile west. The retry missed a step approaching brood0 and then continued off the plan. Both complete failed traces and `southeastern-attempt-1-failed.json`/`southeastern-attempt-2-failed.json` are retained. No third retry was run. No failure is reclassified as a complete route. `Mobile.CheckShove` can refuse a player crossing another mobile unless stamina is full on facets without FreeMovement; this is a plausible transient mechanism, not an established cause for these specific misses. The macro has no movement acknowledgement/recovery logic and must not be presented as a general navigation agent. Southeast needs a bounded diagnostic that observes blocked movement, rather than more blind repetitions or a guessed shard fix.

The atomic publisher now refuses leaving an incomplete/deviating site without explicit `--allow-incomplete`; a live refusal check published no request and left the route untouched. The earlier northern-to-eastern transition preceded this guard and did not silently become a passed northern result. The final verifier retains earlier attempts and reports failed latest attempts as failures, separately from successful preservation checks.

## Source traces

- `tests/ArachnidOutbreakSiteTraversal.cs`: guarded Initialize, structural checks, `Build`/`Search`/`FindPath`, setup-only `Select`, position-only `Tick`, no-save `Stop` and45-minute watchdog.
- `tools/champion-outbreak/PrepareNativeAcceptance.py`: explicit fixture selection and fresh guarded copy.
- `InstallSiteTraversalMacros.py`, `SendNativeRequest.py`, `VerifySiteTraversal.py` in the same tools directory: copied-profile generation, atomic requests/incomplete guard, exact trace and preservation verification.
- `Data/System/Source/Movement.cs`: movement implementation entry point. `Data/Scripts/System/Misc/Paths.cs`: actual terrain/static movement checks. `Data/System/Source/Mobile.cs`: normal Move/CheckShove behavior.
- `Data/Scripts/Custom/Champions/Outbreak/WildernessEncounterReserve.cs` and `ArachnidOutbreakSite.cs`: unchanged inactive proposals, bounds and fixed anchors.
- [Reproduction guide](../../tools/champion-outbreak/NATIVE_TESTING.md), [reservation policy](RESERVATIONS.md), [delivery checklist](DELIVERY.md).
