# Isolated outbreak native acceptance

## Persistent infestation workflow

Current design and evidence classes: `docs/champion-outbreak/PERSISTENT_SIMULATION.md`. Use `VerifyInfestation.py` to freeze complete production inputs and force runtime compilation/separate component suites. Its `--core-candidate` must match the complete core source and tracked executable. `Scripts.csproj` builds remain IDE/project checks.

Prepare a fresh copy with `PrepareNativeAcceptance.py --fixture infestation` for natural growth, aggregate/town/frontier/conflict/selected-clone scenes, or `--fixture archmage-preparation` for actual saved-client cases. Wait for the preparation command to finish successfully and for `prepared.json` before any dependent setup or launch. A returned session/PID is not proof of completed preparation or fixture readiness. Never restart a rejected world in place.

Configure copied clients before starting the server. `PrepareTamerClient.py <root>` freezes the actual Sargar driver, saved pets and finite supplies; normal login can restore the saved mount and seven cats before the trainer fixture runs. `PrepareArchmageClient.py` retains the selected original Zanthus macro/helper; a private `infestation-case.txt` marker selects the new mode. No original profile/account/save is modified. Freeze and validate exact capability/equipment/skill/companion identities before combat; preparation regen and ordinary mount/login behavior are separate from grants.

The infestation idle walking adapter searches for the immediately available queen regardless of suppression milestones. If no enemy is visible, it uses only the last client-observed boss position (or the known initial encounter origin) and 33 finite search goals out to 120 tiles from that observation. Ordinary local client walking still determines reachability. This test policy is not a world encounter radius, hidden server tracking, a combat-macro rewrite or a guarantee of successful pursuit. Retain the older staged-adapter partial run rather than silently relabeling it.

For new actual-client fixtures, explicitly use `RunArchmageCase.py <root> --fixture-heartbeat` or `RunTamerCase.py <root>`. The file monitor validates guarded root, core hash, owned launch PID/path, stable instance and fresh UTC/file heartbeat. It refuses changed instances, stale status and permission failures. Bounded FileNotFound retries handle atomic replacement only; there is no permission fallback, elevation or automatic server relaunch. Historical process-handle mode is retained for old evidence and is not an instruction to retry denied access. File liveness is not OS exit proof.

Record the exact executable/PID/port and input hashes. Before launch, require completed preparation/freeze, matching core/harness/overlays, unchanged copied saves, a free loopback port and no earlier launch record. Start only the private executable. Verify actual fresh fixture status before opening its existing disposable-login client. Do not automate authentication/security dialogs. A normal-user host execution approval does not authorize changing Windows privileges or security settings.

Read live atomically replaced XML using the shared-delete readers in `FreezeBenchmark.py`; direct parsers can block File.Replace. Preserve failed launches, setup/driver failures, deaths and partial cases. Do not require a win. Assess resource grants as well as visits, and require more than 16 established non-retired cells for the natural depth gate. Synthetic scheduler probes, actual world NPCs, connected clients, paused artwork and VPS capacity are distinct evidence classes. Never use mass character clones as throughput evidence.

The client fixture snapshots existing ground gold and measures new gold around the queen's last actual damage/death location. The old fixed-site inspector can report zero after a long pursuit; retain that limitation rather than declaring a missing reward. Record pet deaths as well as player survival.

Request `native-stop.txt`, wait for the fixture's no-save acknowledgement, then use exact-owned `StopNativeCopy.ps1` and verify protected original inputs. Forced no-save termination is not graceful shutdown. Finish by reconciling every owned runtime and listener; do not infer teardown from a launched cleanup command.

## Historical staged workflow

Use only a disposable output. This parameterizes the established client005 copied-world workflow; it is not a production launcher or general gameplay agent.

Preparation requires a verified foundation snapshot matching current core/runtime inputs, the existing isolated client005 baseline with its eight recorded isolation overlays, and installed test assets. It checks the original save/profile/settings hashes, creates a fresh loopback port, disables autosave in the guarded fixture, disables plugins in the copied client only, and generates a source-hash compatibility register for the private tree. It never starts a process. Do not print copied settings or account-name files.

```powershell
python -X utf8 tools/champion-outbreak/PrepareNativeAcceptance.py `
  --baseline D:\ConficturaUOTestTools\foundation-ingame-20261025\batch-3\client-005 `
  --candidate D:\ConficturaUOTestTools\outbreak-20261029-input-throttle-001 `
  --output D:\ConficturaUOTestTools\outbreak-native-next `
  --assets D:\ConficturaUOTestTools\shard\Data\Files
```

From that copy's `shard` directory, force runtime compilation with `ConficturaServer.exe -service -nocache -compileonly`. Before starting its world, verify the recorded port is unoccupied and all eight isolation overlays are present. Start only the copied executable, using `-service -nocache`; capture process identity and console/error logs. Never use the repository executable against these or production saves. The copied client can use its prepared test profile with `-skiploginscreen true -autologin true`; do not automate authentication dialogs. Select its exact returned window through Computer Use.

Publish requests only through the atomic helper, and wait for consumption/telemetry before another request:

```powershell
python -X utf8 tools/champion-outbreak/SendNativeRequest.py `
  --output D:\ConficturaUOTestTools\outbreak-native-next turn-self
```

| Request | Private setup/action |
| --- | --- |
| live-turn | Explicit test-arm enrollment, running manager scheduler and production boss pulses; natural warning/impact, no directly constructed web or backend EndTurn. Use the bounded LiveTurnClient/RunLiveTurn workflow. |
| `show` | Display the native QA control gump. |
| `reset` | Disable/dissolve the private battle and recreate the stationary boss scene. |
| `warn`, `arm` | Trigger the real warning immediately or after eight seconds; natural warning/restraint durations remain unchanged. |
| `turn-self`, `turn-pet` | Real manager enrollment, stopped scheduler, directly constructed self/pet restraint and a prepared player turn. |
| `dose` | Place four real treatments within backpack bounds. |
| `status` | Dispatch the production status command with the copied Player; also available through a native QA button. |
| `queue` | Construct one explicit pending stage entitlement for claim presentation. Does not prove combat earning. |
| `claim` | Dispatch the real player claim command; also available through a native QA button. |
| `stop` | Stop the owned fixture without saving. Record it as forced termination, not graceful logout. |

The fixture requires `evidence/native-private-root.txt` to match its executable directory. Its watchdog is thirty minutes. `native-state.json` and `native-events.jsonl` report instrumented positions, AP, held state and contribution; they do not prove native pixels or input origin. Use native keyboard/mouse observations for that evidence. The native QA status/claim buttons call existing Player commands and validate the responding actor. They are not production UI additions.

Turn scenes bypass encounter discovery/placement and directly construct a restraint. The scheduler and boss/pet AI are stopped. Keep their native item/target/AP evidence separate from legal warning impact, live AI, pacing, site approval and combat rewards. Preserve the original character's equipment/stats; record any fixture-only scene changes. Opening the backpack currently costs the ordinary four-AP ItemUse allowance; prepare inventory before benchmarks and do not silently change that policy.

After testing, check the exact process paths before stopping owned processes, confirm their listeners are gone, and compare all original and private save hashes against `protected-source-inputs.json`/`prepared.json`. Preserve failed logs and identify crash exits separately from explicit stop markers. Never remove unrelated processes or profiles. Native004's request-sharing crash is retained;005 completed forced teardown;007 verifies final fixture compilation and atomic publication only.

## Copied-client site traversal

Pass `--fixture site-traversal` to preparation for `tests/ArachnidOutbreakSiteTraversal.cs` instead of the stationary combat fixture. Keep it outside the production script tree. Force runtime compilation, then start only the guarded copy. With `-service`, compiler/startup diagnostics go to the copy's `Data/Logs/Console.log`; an empty redirected stdout file does not mean compilation succeeded.

The fixture loads the copied Player, disables autosave, grants travel-only invulnerability, and checks all six inactive proposals against copied-world structural exclusions. Actual `Movement.CheckMovement` builds cardinal ground routes from a north boundary entrance through all three brood anchors and the boss anchor to a south boundary exit. It writes `site-routes.csv` only after every route is ready. This is stronger movement evidence than the spawnable-tile connectivity survey, but it is still planning until the client walks it.

Before launching the copied client, install its bounded macros:

```powershell
python -X utf8 tools/champion-outbreak/InstallSiteTraversalMacros.py --output <private-root>
```

This edits only the unambiguous copied `macros.xml`. The original profiles stay untouched. Control+Shift+Z runs two calibration steps; confirm exactly two connected-client positions with zero deviations before continuing. Then reset with the atomic publisher's `--allow-incomplete site-0` request, explicitly retaining the partial calibration attempt. Control+Shift+A through F run the respective six routes in proposal order. Select each subsequent starting point using `site-1` through `site-5`, wait for consumption and reobserve the client before input. Site changes refuse an incomplete or deviating prior attempt unless `--allow-incomplete` explicitly acknowledges it; inspect and retain the failure first. All translation after setup uses normal client Walk actions and movement packets; the server fixture never follows a route with `Move` or `MoveToWorld`. Walking requests turn first, wait500ms between inputs, and pause4000ms at anchors.

Observe the client during traversal and at visually questionable anchors; recorded positions alone cannot prove readable terrain. `site-events.csv` retains calibration, resets, deviations and checkpoint positions. `site-state.json` is transient telemetry. The fixture has a45-minute watchdog and remains in real time with turn-based combat disabled. Invulnerability and markers make these travel tests, not encounter/AI/pacing acceptance.

Use the publisher's `stop` request, stop only the exact copied client, confirm processes/listeners are gone, then run:

```powershell
python -X utf8 tools/champion-outbreak/VerifySiteTraversal.py --output <private-root>
```

The verifier requires every step and checkpoint of the latest attempt at every site, connected-client state, no deviation, an explicit no-save stop marker, matching fixture/core/production-source hashes, and unchanged private saves and original source/profile inputs. It writes partial results and returns nonzero if any route fails; preservation has a separate result. Native screenshots/visual findings remain separate. It does not approve reservations, establish current deployment-world clearance, or enable production. [SITE_TRAVERSAL.md](../../docs/champion-outbreak/SITE_TRAVERSAL.md) retains the initial five-of-six result and the later instrumented southeastern completion.

The current fixture also records original movement dispatch/throttle decisions and next-tile occupancy without following the route or overriding a movement rule. It preserves packet length/in-game guards, restores the packet-reader position, and forwards the original handler/callback once. `site-packets.csv`, `site-throttles.csv`, `site-next-steps.csv` and the teardown `site-packet-summary.json` separate dispatch rejection from a missing dispatch; throttle observation prevents treating pre-dispatch throttling as a client omission. A missing dispatch still does not prove why the client did not send/advance a Walk action.

`--sites <id> ...` explicitly verifies only selected routes; its report sets `full_six_site_gate=false`/`six_site_gate_passed=false` for a subset. Default verification still requires all six. `--fixture-ref <commit>` accepts a historical test fixture only when its copied bytes match the exact Git blob; production inputs/core must still match current source. Use a distinct `--report-name <name>.json` to preserve an older report. The002 combined proof rechecks five001 routes against `fe87553f`, one002 route against current source, then requires all six distinct sites and identical starting-save/core hashes. Separate evidence may be combined for walking coverage; it does not combine into encounter/visibility acceptance.

Client macro format source: [ClassicUO MacroManager](https://github.com/ClassicUO/ClassicUO/blob/main/src/ClassicUO.Client/Game/Managers/MacroManager.cs), `Macro.Load` and the Walk/Delay cases. Delay must have a `text` attribute so the loader creates `MacroObjectString`. The first private survey calibration used an incorrect `str` attribute and crashed the copied client; retain that rejected attempt and its logs, rather than treating it as a shard or traversal failure.

## Natural reaction-turn dodge

Prepare a fresh acceptance copy with PrepareLiveTurnClient.py --case dodge --zanthus-inspection <private matching inspection>. This selects LiveDodgeClient.py while the default escape case retains its original driver. After forced compile and isolated server/client startup, RunLiveTurn.py uses its usual actual-process guard and native live-turn scene. The client waits 3.5 seconds on the reaction turn, moves through normal client pathfinding, observes positive AP spending/original-warning cancellation, ends the turn normally and verifies a later boss turn without restraint. After acknowledged StopNativeCopy.ps1, RunLiveTurn.py --verify checks the case and preservation. The case file/driver/fixture are frozen before startup; no position or AP is set during play.

Optional --observe-warning allows a visual handoff for at most 8 seconds. Its expiry grants no visual acceptance. The normal player turn timeout remains 30 seconds; never extend gameplay time merely to accommodate tooling. Input is scripted native client input; Computer Use rendering/readability evidence stays separate. Retain failed attempts and their copied frozen bytes. Client failures request owned no-save stop; do not restart a failed case in place.

## Selected offline-world reservation preflight

Run RunReservationPreflight.py with --output <new D:/ConficturaUOTestTools/outbreak-reserve-preflight-...>, --candidate <current accepted source/build snapshot>, --saves D:/ConficturaUOTestTools/shard/Saves, --baseline <stopped private shard directory> and --assets D:/ConficturaUOTestTools/shard/Data/Files. The user selected the October 17 offline world; do not replace that evidence basis with a current-live-world claim.

The helper refuses an existing output, checks current production inputs/assembly copies, preserves a separate save copy, copies current Regions.xml, compiles the static probe and bounds its worker. Configure, static Region.Load and matching-serializer World.Load run in order; Initialize/listeners/timer-thread/save startup do not. Six hypothetical reservation checks include production terrain/service/portal rules plus all-elevation house areas and maximum existing-city growth. Record inactive malformed legacy controllers separately; do not repair saves from guessed bounds. Verify both source/private save hashes and source/assembly/config hashes after exit. Failed attempts stay retained; use a fresh root after repair. This is loaded-snapshot placement evidence, with no gameplay, native-presentation, live-occupancy or geography-approval inference.
