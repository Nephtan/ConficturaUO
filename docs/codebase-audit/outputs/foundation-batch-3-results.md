# Foundation Batch 3: FIX-15 acceptance

**Accepted within the independently reviewed isolated scope on October 26, 2026 (America/Chicago).** All 15 fresh client005 cases passed on committed candidate `e528dbec6e5005c28b9aca13baf2a5fa39e0e610`, containing ownership repair `143e9fb33c4c56540ea9261f70b267441ab44284` and actor-effect repair `b55c6f13eafe716c81e1379506164a96a278ed9e`. The local acceptance commit accompanies this report; its exact ID is recorded after commit in `D:/ConficturaUOTestTools/foundation-ingame-20261025/batch-3/CLIENT005-CLOSEOUT.md`. No push or deployment.

The original controller remained the sole writer and runtime controller. Direct human re-enable restored native discovery; no repeated permission request or new endpoint was required. Computer Use was reset during file/build work and again immediately after gameplay restoration. All owned game/model runtimes are stopped, copied inputs restored, and the test executable/cache/evaluator disarmed.

## Repair and source boundary

`Data/Scripts/Items/Trades/Misc/Bandage.cs` binds a context to its owned table entry and exact timer. Replacement retires the old context; cancellation/completion cannot remove a newer owner. Suspended contexts reject real-time completion; resume creates a different timer. Timer callbacks deliberately do not require `Running`, because the normal one-shot scheduler stops a queued timer before `Timer.Slice`. Direct constructors used by `Custom/ThirdParty/OmniAI/OmniAI Shared.cs` share ownership while preserving their historical invisibility to public `GetContext` and normal turn capture.

Real-client attempt003 exposed an additional failure: one actual End Turn response advanced the round but left healing suspended. `TurnBasedCombatManager.ProcessActorEffects` runs after `Pending` is cleared, so its own mutation request was denied. The core bridge also matches scopes by target. The additive script repair authorizes only the exact synchronous internal request for the current registered participant/group, opens the captured-patient scope for other-healing, restores its token/disposes scopes in `finally`, and preserves an effect context replaced during `OnHeal`. See [the actor-effect diagnosis and regression](foundation-batch-3-actor-effects.md).

Source traces: `TurnBasedCombatManager.AuthorizeMutation`, `BeginActorEffectMutation`, `ProcessActorEffects`, `StartNextActor`, `EndTurn`; `BandageContext.AdvanceTurnBased`, `EndHeal`, `StopHeal`; core `TurnBasedCombat.BeginMutation`, `ScopeMatches`, and `Mobile.Heal`. The new regression is `tests/FoundationBandageRegression.cs`. No global timer, formula, core source/executable, serializer, namespace, project inclusion or production configuration changed.

## Candidate and component verification

Evidence root below is `D:/ConficturaUOTestTools/foundation-ingame-20261025/batch-3/`.

- `candidate-baseline-e528dbec.json` and `committed-files-e528dbec.json`: all 9,479 committed Git blobs and 6,729 runtime inputs verified; accepted correspondence for all 124 unchanged core inputs retained. Git CRLF materialization was normalized to exact Git content, not treated as an unexplained source difference.
- `candidate-actor-effects-001`: forced production runtime script compile passed with normal exit 0. Seven groups passed **6,995 component assertions**: payments74, actions261, bandages110, Food1853, accounting3972, Research46, champion679. All production inputs match the final candidate. Known unconnected capture-socket teardown diagnostics remain; no warning-free claim.
- `client-005`: forced copied-session compile passed; full isolated startup on `127.0.0.1:25215` had zero region-creation errors. Payments74/actions261/bandages110 passed against both the compile-only and exact gameplay assembly: **445 assertions per run**, under `exact-session-component-005` and `exact-gameplay-component-005`. These separate-process checks are component evidence.
- Source-only changes retain the accepted core build; a new `Server.csproj` build was not needed. Optional IDE/project hygiene builds were not run because project inclusion did not change. Runtime compilation, component checks, and actual client results remain distinct.

| Identity | SHA-256 |
| --- | --- |
| Unchanged tracked core | `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7` |
| Production repair scripts | `f8ccb45a485db7c8787b0be6945e05217cbdc8aae874dabe9dc473d2298fd174` |
| Client005 gameplay scripts | `13dbd548aa8bdd0c64d3950257d52fb7f91ac903fcf2ee17c8ea2459cfe486a2` |
| Client005 source manifest | `745352113615efe78e63bbf27598ea66bc17cbba801ad7a109e6190018245231` |
| External test evaluator | `1c1088301281f9afefb78d1c165fdc4299e66a936b5c9762323ff757a34ef791` |
| Canonical repaired manager | `0463fd3d4fadf430195d8efbb61f694c46730ae89da9e711c518302196c648da` |
| Bandage regression | `2e1f7e508495b780518984777d22d6685665dd1a8cbcf4c5f8e3f9c206e56dc6` |

The copied client inherits verified client004-lineage configuration. Eight declared isolation overlays plus one external evaluator are the only copied-script differences. Compatibility gating remains enabled; the production enable path was used. The final reviewer rehashed 6,599 sources, eight tools, 92 runtime-code files and 2,746 runtime-data files, resolving only the three explicitly disabled paths after teardown.

## Actual client and instrumented-world cases

Saved login plus a supported Return completed authentication as ordinary Player Nephtest521. The exact client005 guarded ClassicAssist macro was selected and played once. Resident autonomy stayed stopped; bridge/control/model endpoints remained the existing isolated `25291/25292/21447` services. There were **four actual bandage use/target pairs, two native End Turn replies, three supervised Qwen choices, and 38 saved native images**. Fifteen cases do not mean fifteen ordinary network actions.

| Case | Accepted observation |
| --- | --- |
| 00 self | Actual Qwen-selected use/target consumed one bandage; original 6s delay, health10→186, two skill callbacks. |
| 01 other | Actual Qwen-selected use/target on patient572; 1461ms delay, health10→150, one OnHeal/two skill callbacks. |
| 02 range | Actual use/target; fixture moved patient586 out of range. No heal/training; complete journal has two refusal copies. Expected bridge action failure retained. |
| 03 cancelled-newer | Injected owned A queued before Stop; ordinary server Timer.Slice drained it without changing B or health/skills. B completed once; Qwen subsequently inspected the result. |
| 04 suspend-resume | Repeated suspension/partial advance retained the clock; resume created one new timer. Old ordinary queue dispatch inert, new timer completed once. |
| 05 turn-native | Actual bandage captured at1461ms; one native button1 reply and one owned transition. Player round1→2, one heal/two skills, context/effect cleared; UI changed Bandaging1.5s→EffectsNone. |
| 06 turn-newer | Old queued A could not remove captured B. One native button1 reply/transition completed B once; settled Player round2 with no context/effect. |
| 07 turn-complete-old | Explicitly instrumented 61s production StartOfTurn advancement completed A, then ordinary old queued dispatch stayed inert. This is not another native turn reply. |
| 08 deleted-patient | Disposable NPC patient deleted; ordinary stale dispatch caused no healing/training and released ownership. |
| 09 deleted-healer | Disposable NPC healer deleted, Player untouched; no healing/training and no residual context. |
| 10 direct-direct | NPC hidden B remained owned through old A dispatch and completed once after3s. |
| 11 direct-regular | NPC registered B remained current through old hidden A dispatch and completed once after1461ms. |
| 12 regular-direct | NPC hidden B replaced registered A, survived old dispatch and completed once after3s. |
| 13 OmniAI self | Actual public OmniAI.TryToHeal invoked in controlled fixture; hidden context, original6s delay, health10→200, one OnHeal/two skills. Its AI timer setup/invocation are instrumented. |
| 14 cancel-only | Queued owned A was stopped; normal Timer.Slice produced no healing/training/context. |

All strict progression fingerprints stayed equal. Positive cases produced 22 total skill callbacks; four actual uses reduced Player bandages100→96. Ten saved queue-drain boundaries separately prove old-dispatch inertness. Later direct public-method/OnTick duplicate checks are separate from ordinary queue evidence. Exact zero-packet/effect-clear-call counts belong to component tests; live evidence establishes retained feedback, context ownership and final effect removal. The evaluator's 193,298 cumulative checks include repeated large world/compatibility scans and are not a distinct gameplay assertion count.

Raw files: `client-005/evidence/case-*-before.xml`, `case-*-drained.xml`, `case-*-after.xml`, four `*-actual-start.xml`, `bandage-events.txt`, `observations/`, `native/`, `actual-client.jsonl` and model request triplets. [Curated identity/acceptance record](foundation-batch-3-acceptance-verification.json) references the external 110-file hash digest.

## Independent review and evidence limits

[Final independent review](subagent-findings/foundation-batch-3-client-review.md) found the bounded B3-01–04 client and safe-closeout gates satisfied. It inspected all 15 XML pairs, ten drains, four starts, all journals/model traces, all 38 native JPEGs and restoration records. The reviewer used saved evidence only; native actions are correlated to controller operations and actual server receipts.

The initial offline journal checker expected Player messages from NPC-only cases10–12 and failed at10. The original script/log/result remain retained. `Bandage.cs:724–731,859–862` sends actor-private/local messages; `Mobile.cs:13025–13035,13124–13215` requires that actor's own NetState. The independently reviewed correction requires zero Player feedback for those NPC pairs, while additionally asserting NPC identity, one heal/two skills, released context and no private-message leak. All15 same-boot, contiguous journal intervals pass in `journal-reconciliation-20261027T013214Z.json`. No gameplay assertion was relaxed.

Qwen3.5:9b digest `6488c96fa5faab64bb65cbd30d4289e20e6130ef535a93ef9a49f42eda893ea7` selected only offered actions or wait. Traces `5f7faaa8-471f-496a-b50d-1f43c92e0d74`, `2bf4e5a1-12a9-4eee-87a6-5722cb682179` and `e5cb094d-3492-4caa-a0f4-b71a85813cf3` establish bounded self-heal, other-heal and inspection. The other-heal hypothesis's target-queue causal claim is unsupported; self/inspection hypotheses are truncated. That feedback is retained, speech was not transmitted, and general autonomous or reasoning competence is not inferred.

Native limits: case02-before has a transient Options overlay of unknown origin; it is not an unobscured range image pair. Some feedback faded before screenshots; complete journal intervals supply that evidence separately. Overlapping NPCs need authoritative serial/health assertions. Case06's immediate reply image is an intermediate NPC turn; the later after-image is settled. A later case05 automatic timeout is not evidence of duplicate healing. Client asset/localization/BAR warnings remain.

No new full-world save/reload or deliberate authenticated reconnect was run in client005. Prior accepted network/save evidence remains under its own candidate and scope; unchanged core correspondence is verified, but those old results are not relabeled new Bandage-candidate acceptance. No live-shard, deployment or autonomous-play claim is made.

## Restoration, retained attempts and bookkeeping

Online restore passed at01:23:19Z (October26 20:23 local): original progression, properties, location, item parent/layer and manager/handler configuration restored. Original/restored progression and24 actor-rooted serial/type/amount inventory rows match exactly. The inventory fingerprint does not compare every serialized item property; the progression fingerprint excludes current HP/stamina/mana. The final native image shows restored equipment/stats but health300/70; the authoritative response is70/70 with436 pending bytes. Final client-health pixels were not observed settled.

Server57820/client5948 were deliberately forcibly stopped with exit1 at01:23:21Z, not graceful shutdown. Resident wrapper cleanup completed and owned model identities are absent; no graceful model-process claim. Full copied Saves/settings/DataProfiles/ClassicAssistProfiles rollback passed01:23:43Z; all14 protected originals and backups match. Core/cache/evaluator disarm passed01:23:50Z with exact hashes. Fresh read-only `final-integrity-20261027T013637Z.json` reverified restoration, unchanged production source/core, absent test runtimes and five free ports25205/25215/25291/25292/21447. Computer Use is inactive.

Attempts001/002 retain strict progression failures and the diagnosis of existing BuffIcons removal rewriting fixture RawStr200→150. Only Player fixture setup changed to150; the equality assertion stayed strict. The separate buff/raw-stat concern is not fixed by FIX15. Attempt003 retains the real manager failure and all partial passes. Attempt004 retains the physical-Escape capability stop with no client cases credited. All previous copies remain restored/stopped/disarmed. Earlier 12 historical/62 current ownership assertions and their original candidate evidence remain preserved; the110 current bandage component checks extend them.

Only existing overlay `POST-FOUNDATION-FIX-15` changes to Fixed: **6,831 records,964 Fixed,0 Open**. Prior6,830 records remain byte-identical in CSV and workbook record XML. Historical statuses and the wider seven domain-only blockers retain their meaning. The workbook adds October26's date bucket and preserves native formulas, filters, charts, styles and relationships. Artifact Tool authored/recalculated the change; numeric counts passed. Its known text-date COUNTIF/shared-formula limitation remains (date reconciliation -6815 with stale OK label); delivered native saved caches reconcile independently to CSV, and qualified preview uses those exact caches. Native Excel recalculation is not claimed. See [workbook verification](foundation-batch-3-workbook-verification.json).

No further work remains in the authorized Batch3 acceptance after the focused local closeout commit. The four unrelated untracked paths remain excluded. No new controller, scheduled run, push, live restart, production-save mutation or Mira control was performed.
