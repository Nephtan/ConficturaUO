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

## Open acceptance

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
