# Batch3 / FIX15 client005 final evidence review

**Disposition: the bounded Batch3 B3-01–04 client acceptance and safe closeout gates are satisfied, with the qualifications below. No material gameplay or containment blocker remains in the reviewed evidence.** Tracker/workbook reconciliation, final documentation and the local acceptance commit remain the controller's work; this review does not claim those steps are complete. It also does not authorize deployment or live-shard changes.

Evidence root: `D:/ConficturaUOTestTools/foundation-ingame-20261025/batch-3/client-005/evidence`. Source candidate is latest tested commit `e528dbec6e5005c28b9aca13baf2a5fa39e0e610`, containing the script repair committed in `b55c6f13eafe716c81e1379506164a96a278ed9e`. See [the prelaunch/repair review](D:/ConficturaUOTestTools/foundation-ingame-20261025/reviews/batch3-bandage-prelaunch-review.md) for the failed-candidate diagnosis, patch review and exact committed-source correspondence.

## Evidence inspected and results

Independently read all 15 case before/after XML pairs, all 10 queued-callback drain records, four ordinary-use start records, complete case event log, actual-client JSONL, all before/after client observations/journals, three model request/response/meta triplets, restore records and cleanup/disarm records. Viewed **all 38 saved native JPEGs** and checked their metadata/file correspondence; all 38 image hashes are distinct. Native actions are classified from the controller's recorded operations and correlated actual server receipts; this reviewer did not operate or observe a live native session.

| Coverage | Reviewed result and evidence boundary |
| --- | --- |
| B3-01 ordinary self/other/range, cases0–2 | Qwen chose the offered self/other heals. Each produced one actual0x06 use, one0x6C target and one consumed bandage. Self health10→186 with two skill callbacks; other health10→150 with one counted OnHeal/two skill callbacks. Range case retained health10, zero heal/skill delta and two Player-visible refusal copies. Its bridge action correctly reports failure; the actual refusal and authoritative no-heal assertions establish the expected negative result. |
| B3-02 queued A/new B, cases3/6 | Owned future A was queued once before Stop; ordinary server Timer.Slice drained it. Drain snapshots retain B's context/timer and, in case6, the real manager effect, with no health/heal/skill change at that boundary. B later healed once. Qwen's case3 inspection is an actual client observation, not an autonomous race setup. |
| B3-03 suspend/resume/turn, cases4–7 | Repeated suspension and partial advancement preserve remaining time; resume changes the timer. Old queued dispatch stays inert. Cases5/6 each have exactly one recorded button1 reply and one owned turn transition; settled state is Player round2, one heal/two skill callbacks, no context and zero effect remaining. Saved native UI shows Bandaging1.5s before and EffectsNone afterward. Case7 uses explicitly instrumented61-second production StartOfTurn advancement, followed by ordinary old-queue drain; it is not another native turn response. |
| B3-04 deleted/direct/AI/cancel, cases8–14 | Deleted patient/healer and cancellation produce no heal/skill change. All three direct/registered replacement permutations preserve the newer owner through old dispatch and complete once. The actual public OmniAI.TryToHeal implementation creates its hidden context and completes the expected6-second heal, health10→200, one OnHeal/two skill callbacks. Its invocation and controlled AI timer setup are instrumented. Repeated direct public-method/OnTick checks afterward stay inert. |

There are **four actual use/target pairs and two actual turn replies**, not 15 ordinary network actions. The remaining cases are supervised copied-world queue/callback/AI exercises with actual connected-client observation. All final cases0–14 are present and checked; all case XML error fields are empty, no evaluator errors.log exists, and complete Player progression fingerprints remain equal throughout the cases. The final193,298 cumulative `Need` checks include repeated large compatibility scans and must not be presented as a gameplay assertion count.

Live journal intervals for every case share their bridge boot and contain every monotonic entry ID in the interval; none is truncated. Exact zero-packet and exact effect-clear-call counts remain component evidence. The live evidence establishes retained client feedback, state/ownership and final effect removal; it is not a packet or clear-call counter. Before/after client and server observations are separately timed samples, not atomic snapshots.

## Journal checker correction and Qwen qualifications

The initial offline checker failed at case10 expecting two finish messages but seeing zero. The original checker `reconcile_journals-before-npc-recipient.py` SHA `2820aad70e035121aee9a45745c806aa89e0d51eb903e117715a692062882edf`, failure log and failed result remain retained.

Correction is source-grounded: `Bandage.cs:724–731,859–862` addresses healer/patient with `SendLocalizedMessage` and **Local**OverheadMessage. `Data/System/Source/Mobile.cs:13025–13035,13124–13215` sends these only to that mobile's NetState. Cases10–12 use disposable NPC healers and patients without NetStates, so zero Player feedback is required, as in case13. The corrected checker SHA `3435a4ca8d0569209360d8a45accd63f81bc16e5af9d8342034c9420a4446b0b` preserves continuity/other counts and additionally checks non-Player participants, one heal/two skill calls, released contexts, no private-message leak and all15 cases. Independently recalculated the original intervals and verified all referenced snapshot hashes in `journal-reconciliation-20261027T013214Z.json`: all15 pass. No runtime assertion or production behavior was altered by this offline correction.

Verified all recorded request/response hashes for Qwen3.5:9b, digest `6488c96fa5faab64bb65cbd30d4289e20e6130ef535a93ef9a49f42eda893ea7`:

- `5f7faaa8-471f-496a-b50d-1f43c92e0d74`: self-heal.
- `2bf4e5a1-12a9-4eee-87a6-5722cb682179`: other-heal on observed patient572.
- `e5cb094d-3492-4caa-a0f4-b71a85813cf3`: case3 inspection.

Each request uses Player-visible context bound to Nephtest521 and offers the exact bounded action or wait. Actual outcomes, not model narration, establish success. The other-heal hypothesis's target-queue-clearing causal claim is unsupported; the self/inspection hypotheses end mid-sentence. These warrant retained feedback and limit any competence claim. They do not negate the verified supervised actions. No broad autonomous-play or staff-control result is claimed.

## Native and restoration limits

- Case02-before has a client Options overlay partially obscuring the scene; its origin is unestablished. Preserve it as such. The after-image shows the distant patient; exact target/movement/refusal/state evidence comes from the ordinary receipt, observations and server assertions. This is not an unobscured before/after range rendering pair.
- Case06-turn-reply captures an intermediate NPC turn. Case06-after is the settled Player round2/EffectsNone image and supports the final turn UI claim.
- NPC direct/deleted-case images show the connected observer's scene before/after; overlapping NPC renderings do not independently establish their exact numeric health or callback counts. Those conclusions come from the identity-bound authoritative assertions.
- `restored-original.jpg` shows original equipment and35/35/10 stats but still displays health300/70. The authoritative restore response is70/70 with436 pending bytes. Final health pixels were not observed settled; do not claim complete final client-cache convergence. This does not undermine the separately verified server restoration/full copied-file rollback.
- Client stdout retains BAR-memory, localized-file fallback and ambiguous asset-name warnings. No warning-free or normal native-exit claim is supported. Reconnect and full-world save/reload were not run in client005 and are not inferred.

## Candidate integrity and closeout

Gameplay script SHA `13dbd548aa8bdd0c64d3950257d52fb7f91ac903fcf2ee17c8ea2459cfe486a2`, unchanged core `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`, evaluator `1c1088301281f9afefb78d1c165fdc4299e66a936b5c9762323ff757a34ef791` and source manifest `745352113615efe78e63bbf27598ea66bc17cbba801ad7a109e6190018245231` verify. Rehashed all6,599 source/8 tool/92 runtime-code/2,746 runtime-data entries, resolving only the three declared disabled runtime paths: no mismatch. All exact-gameplay fixture source/assembly identities and normal run results verify: payments74/actions261/bandages110 = **445 separate-process component assertions**, in addition to the prior6,995 candidate and445 copied-compile checks.

Restore response `response-2307a5ae0413446a9fcfa9884e9b8d5c.xml` at01:23:19Z passes original progression, properties, location and stashed-root parent/layer assertions, restores handlers/turn configuration and retires fixture ownership. Original/restored progression and **24 actor-rooted inventory serial/type/amount rows** match byte-for-byte. This inventory fingerprint does not compare every serialized item property. Full copied Saves and profile rollback subsequently verify independently, as do **all14 protected originals and backups** against the retained manifest.

Server57820/client5948 were deliberately forcibly terminated with exit1 at01:23:21Z. Resident wrapper close completion is recorded; its model shutdown may use forced owned-process termination, so no graceful model-process exit is claimed. Cleanup at01:23:43Z records owned identities absent, task-copy census empty and four test ports free. This reviewer read those process/port records without querying live processes. At01:23:50Z all three executable/cache/evaluator `.disabled` archives hash correctly and original runtime paths are absent. Prior failed/blocked attempts and their disarms remain preserved.

The review performed only saved-file/image inspection and read-only hash/structured-data analysis. No native control, process discovery, runtime/model/fixture launch, production edit or Git operation was performed. The sole controller may now reconcile the bounded acceptance status and prepare the required focused local closeout commit while retaining these limitations and the separate pre-existing buff/raw-stat concern.

## Final digest cross-check

Read the later `final-integrity-20261027T013637Z.json` (SHA `8715aa7b087d9295270334705744d720349cd5b8cfab4e5b46640891850c3f21`): it records all14 protected originals/backups exact, full copied Saves/profiles restored, unchanged source/core, disabled archives exact, no owned/global test runtimes and all five ports free. These recorded results agree with the earlier independently rehashed artifacts; this reviewer performed no fresh process census.

The initial `acceptance-summary.json` (SHA `ad0ca3981f4f7e41d3fa520e99fcae73444454e99c2300dd22a0921d2fab2e68`) agrees on all15 cases, four use/target pairs and two actual turn replies. All110 listed evidence hashes verify. Three model-meta keys require resolution against `client-005/`, whereas the other107 resolve against the declared `evidence_root`; this is a digest-path ambiguity, not missing evidence. The controller was asked to normalize these three relative keys or declare their separate base before finalizing that summary. The gameplay/containment disposition above is unchanged.
