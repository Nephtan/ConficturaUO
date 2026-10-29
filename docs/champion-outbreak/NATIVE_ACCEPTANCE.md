# Arachnid Outbreak native acceptance

Status: Partial, 2026-10-29. Production activation and reservation review flags remain false.

## Evidence boundary

Private copies `D:/ConficturaUOTestTools/outbreak-native-20261029-002` and `003` used the previously verified web008 runtime candidate, copied October17 saves and an isolated ClassicUO profile. Listeners were loopback only. Autosave was disabled. The connected character retained its copied stats/equipment, with Player access and no blessing. A private harness created the boss scene and repeatedly stopped boss AI. This establishes presentation and selected native interactions; it is not normal boss AI, site approval, traversal, a legal veteran fixture or a pacing benchmark.

Computer Use supplied actual client keyboard/mouse input. `evidence/native-events.jsonl` and `native-state.json` are instrumented server observations; they do not independently prove pixels or input origin. Pixel/input observations are retained in the Codex session tool history. There is no separate archived screenshot set for these cases. Summary and provenance: `003/evidence/acceptance-checkpoint.json`, `native-harness-preparation.json`, and each copy's `prepared.json`.

## Accepted observations

| Case | Result and limits |
| --- | --- |
| Native movement | In002, a private client Walk N macro, invoked with Ctrl+E, moved `(2517,2088)` to `(2517,2087)`. Initial east attempts did not establish translation. No backend movement method supplied these steps. |
| Warning feedback | The real production warning message appeared in red. The real white restraint artwork and explanation appeared on impact. Night lighting was present; dense-combat/daylight contrast still needs review. |
| Native avoidance | Warning at06:54:54.717Z; native movement to y2086 at06:54:57.216Z; warning cancelled without a held event. The attack was triggered by the harness, with its real three-second minimum unchanged. |
| Movement while held | Warning at06:55:23.509Z; held at06:55:26.774Z. The previously working native shortcut produced the blocked-web message without translation. Native pixels and server telemetry showed an active restraint. |
| Natural release | Release observed at06:55:30.675Z. The subsequent same native shortcut moved to y2085 at06:55:40.764Z. Poll timestamps approximate the actual four-second timer boundary. |
| Native treatment | In003, double-clicking a visible stack showed `4 Brood Antivenom` and the beneficial target cursor. Native self-targeting displayed three protections/five-minute feedback and the explicit no-cure message. The next tooltip showed three treatments, and a returned bottle was visible. |
| Active-treatment rejection | Native repeat targeting displayed `That recipient already has active antivenom protection.` No refreshed effect is claimed from this rejected action. Component tests separately verify unchanged quantities/state. |

## Earlier incomplete acceptance (002/003)

- Native double-click web escape and rescue: no successful input before expiry. One capture returned after the natural four-second restraint had already disappeared; no click on that stale object was attempted. This is an incomplete native check, not evidence that the handler fails. Existing real-manager/`Mobile.Use` component checks remain distinct.
- Native treatment status command/gump: command-entry focus was not established. The Chat menu opened channels only; no chat message was sent. No native status-display acceptance is claimed.
- Native turn-based action/clock flow, party/pet treatment, poison prevention feedback, and bard/healing/cure/rescue roles.
- Six reviewed sites and actual traversal, legal veteran and Zanthus fixtures, three full reference runs, bounded tuning and economic observations.

## Retained failures and cleanup

The002 harness's first two launches crashed on its AI-timer reflection assumption; the corrected private helper used the actual public timer field. Those observations remain in that copy. The first treatment fixture used `AddItem`, which does not assign container coordinates; it earned no treatment acceptance. The003 fixture uses `DropItem` before native testing. Its preparation initially failed after copying because `exec` shared a root variable; only that identified partial preparation was resumed using separate globals. No production behavior was changed for these fixture repairs.

A right-double-click request was rejected by the Computer Use API; state was reobserved and that unsupported input was not repeated. The partly clipped client caption required coordinate correction. Uncertain clicks were not classified as server defects.

Both owned servers ended through the private no-save stop request, and both owned clients were forcibly stopped after process-path checks. These are forced exits, not graceful logout/shutdown evidence. Host process/listener checks confirmed teardown. `evidence/restoration-verification.json` in each copy records byte-identical original saves/settings/profiles and unchanged private save files after testing. No production process/save was used.

## Source and verification follow-up

Relevant production paths: `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakChampion.cs` (`TryStartWeb`), `ArachnidOutbreakWebs.cs` (`TryResolve`, `OnMovement`, `TryFree`), and `BroodAntivenom.cs` (`OnDoubleClick`, `TryApply`, effect/status classes). The private harness bypasses site approval/phase preparation only in its copied process; `ArachnidOutbreakController.DeliveryReady` remains false in repository source.

The runtime verifier in `Data/System/Source/Main.cs` uses the public `GetConstructor(Serial)` lookup. The abstract web base had a protected Serial constructor, causing a warning despite successful derived-item reloads. It is now public; version0 and all write/read ordering remain unchanged. This access change does not alter the native behavior tested above. No core source or tracked executable changes.

Final follow-up snapshot: `D:/ConficturaUOTestTools/outbreak-20261029-native-checkpoint-001`. Operational Release/x86 build, forced recursive runtime script compile, web92, antivenom53 and actual world16/34 pass. The web-base serialization warning is absent. This is component/reload evidence for the constructor access correction, separate from the native008 candidate observations.

## Turn-based item/AP acceptance and input repair (004/005)

These later scenes retain the original copied character and real manager. The harness uses a harmless opening intent and the existing process-local PvE test arm to enroll the boss, player and optional controlled Dog through actual group creation; actual EndTurn transitions prepare the player's turn. It constructs the restraint directly and stops the scheduler for inspection. This is native item/target/AP and presentation acceptance, not a natural telegraph/AI reaction sequence or timing benchmark. Copy004 used a private compatibility-gate override; copy005 retained `RequireCompatibilityGate=true` and successfully enabled through a generated register for all6,612 private runtime scripts. Neither enables production.

| Case in005 | Observed result |
| --- | --- |
| Controlled-pet rescue | Native double-click removed the pet's web; AP20→16; boss-stage credit0→2. Server events at07:51:39.669Z agree with native feedback. |
| Self escape | Native double-click removed the player's web; AP20→16; contribution remained0. |
| Several item actions in one turn | After escape, opening the backpack cost four AP under the existing catalog (16→12). Starting treatment reserved four AP (12→8); self-target completion cleared Pending without another charge and displayed successful treatment. The inventory-opening cost was observed, not changed. |
| Status presentation | A native QA button dispatched the real Player command through `CommandSystem.Handle`. The production gump showed three protections,281seconds and101seconds reuse delay. Native refresh later showed160seconds/0seconds. This validates display/response behavior; native chat typing remains untested. |
| Claim visibility | The harness queued one explicit stage entitlement; a native QA button dispatched the real claim command. Feedback showed one claim collected/zero pending, and native inventory inspection showed `Brood Venom Gland`. This proves presentation/collection, not combat earning or victory. |
| Pet treatment | Native targeting applied treatment to the controlled pet while it remained restrained. AP20→16, Pending cleared, and the production status gump named the pet with three protections,275seconds and95seconds reuse delay. |

The004 pet attempt exposed an actual input-clock error. The active0x06 packet handler is `XmlAttach.UseReq`, registered by `Data/Scripts/Custom/XMLSpawner/PacketHandlerOverrides.cs`; the core fallback has the same boundary. Both set a half-second `NextActionTime` using paused actor time, so subsequent requests could remain blocked for minutes. The component fixture reproduces this against the prior native-checkpoint candidate (`packet-input-throttle-run-attempt-2.log`); its first attempt lacked normal world-dictionary setup and is retained as a fixture failure.

`Mobile.NextActionTime` now consistently means wall-clock packet/lift flood control. The core use handler, XML override, bandage packet, actual Lift and HordeMinion's lift preparation use DateTime.Now. `RebaseCooldowns` leaves this deadline alone while still rebasing skill/spell/combat cooldowns. Half-second throttling, staff policy, native legality and paid AP gates remain. No serializer or saved-field format changed. The compatibility generator now distinguishes `Clock.InputThrottle` from participant cooldown mixing. Synthetic scanner probes accept the former and still reject direct skill/spell/combat mixing; the repository's old hash register is not silently promoted.

Accepted repair snapshot: `D:/ConficturaUOTestTools/outbreak-20261029-input-throttle-001`. Operational Release/x86 build, forced runtime compile, packet80 (actual core/XML packet dispatch, actual Lift, controlled bridge clock and actual cooldown rebase), web92, antivenom53, actions261, bandages110, champion679 and world16/34 pass. Packet evidence is its final `packet-input-throttle-run-attempt-2.log`; earlier76-assertion output is retained. Native005 uses that matching build. The uninstrumented matching tracked executable SHA-256 is `dbb7b2360c435834b32e0f1f4b0854a859cd9086971a4c270981b58d8f2f3ddb`. All6,742 core/runtime inputs,124 core inputs and final fixture identities were checked; no local server process was running before executable replacement.

Copy004 crashed while reading a concurrently written stop-request file; its console log retains the IOException and it has no successful stop marker. This is a private-harness failure, not graceful shutdown evidence. Copy005 has an explicit forced no-save stop marker; its client was forcibly stopped. Both original inputs and private saves remain byte-identical. The reusable fixture now retries a sharing violation, checks its deadline before request I/O and prevents further ticks after stopping. The atomic publisher refuses to overwrite an unconsumed request. Final harness source is forced-compile checked in007; those I/O-only hardening changes have not had a new live collision test. Native005 used the earlier consumer.

Reproducible source and commands: [native testing guide](../../tools/champion-outbreak/NATIVE_TESTING.md), `tests/ArachnidOutbreakNativeAcceptance.cs`, `tools/champion-outbreak/PrepareNativeAcceptance.py`, and `SendNativeRequest.py`. The fixture stays outside the production runtime tree; its exact private-root guard must pass before initialization in a copy. Preparation and scanner generation were rerun in006/007 without starting a world/client; the publisher's complete-write and pending-request rejection checks pass.

Current open gates: native live-scheduler telegraph/reaction flow, another human ally, poison/cure/bard/healing role feedback, reviewed site traversal, legal veteran/Zanthus manifests and combat runs, bounded tuning/economics, and the full requirement audit. Production and reservation reviews remain disabled.

## Copied-client site traversal (2026-10-29)

The separate guarded survey fixture uses an invulnerable travel-only Player, real world movement checks for planning and native-started ClassicUO walking macros for all route translation. Five complete connected-client routes include every brood anchor, boss anchor and opposite combat boundaries: central226, northern224 on retry, eastern228, northwestern228, western226. The1,132 accepted steps are checked individually against the plans. Southeastern failed both attempts and remains unresolved; the six-site verifier reports failure, separately from passed input/save preservation. Full source traces, raw evidence paths, visual concerns and limits: [SITE_TRAVERSAL.md](SITE_TRAVERSAL.md).

Native views exposed character occlusion by trees near northern/western boss positions and eastern brood0. Walkability is not approval of ground-warning readability. No actual fight or encounter timing was tested by this fixture. All6,742 production inputs, the matching core/fixture and original/private save/profile inputs were checked. The exact owned processes/listener are gone; teardown was forced without saving. This leaves live-scheduler/ally/support-role acceptance, southeastern diagnostic and anchor visibility, current-world preflight, legal veteran/Zanthus pacing and economics, and the full requirement audit open.
