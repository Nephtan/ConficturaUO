# Foundation Batch 3 independent review

2026-10-25 design review for FIX-15, limited to `Data/Scripts/Items/Trades/Misc/Bandage.cs`. This reviewer changes only audit review reports, launches no runtimes/controllers, and does not broaden the fix to other timers/spells. Applicable root/audit instructions were read. Batch 1/2 code and regression review results remain valid; full acceptance for every batch now requires the user's additional isolated in-game Qwen testing through the existing FIX-01–05 session.

**Current status: code/regression review complete; in-game acceptance pending.** The required isolated in-game Qwen session has not been exercised by this reviewer; this batch must not be called fully accepted or complete from the evidence below alone.

## Source-confirmed lifecycle defect

`Timer.TimerThread` enqueues a due timer and stops a completed one-shot timer (`Data/System/Source/Timer.cs:348-359`). `Timer.Stop` changes scheduling (`:736-741`), but `Timer.Slice` still invokes queued `OnTick` (`:398-409`). Therefore global `Running` checks or queue changes would break normal one-shot dispatch and are out of scope.

`BandageContext.StopHeal` removes the healer's table entry and sends `EffectTimerChanged(..., null)` unconditionally. `InternalTimer.OnTick` calls `EndHeal`, which first calls `StopHeal`. A queued old timer can clear a newer context/effect and apply the old action. Suspension retains the context while resume replaces its timer, so context identity alone cannot reject a pre-suspend callback.

## Required bounded design

- Own the logical action as well as each timer instance/generation. Cancellation/completion must invalidate old callbacks before effects/messages/skill checks. Timer dispatch must match both current action ownership and exact current timer (or generation), and must not complete a turn-suspended operation.
- Stop/cancel only this context's timer. Remove `m_Table[healer]` and clear turn-effect registration only if this context still owns that table entry/registration. Repeated stop/end calls are harmless to replacement state.
- `SuspendForTurnBased` is idempotent while already suspended: preserve the current remaining duration. `ResumeFromTurnBased` installs a replacement timer without reviving a previously queued dispatch. Turn advancement completes once through the same ownership boundary.
- Preserve the `BandageContext+InternalTimer` runtime type and the nonnull suspended `Timer` property used by `TurnBasedCombatManager.IsEffectEnabled` and configured effect-type matching. Do not make a suspended effect unclassifiable by simply nulling that property. Preserve all delay formulas, healing/skill/resurrection policy and serialized data.
- A current cancelled/deleted-participant operation may clear its own registration, but cannot heal, send completion messages or make skill checks. A stale operation cannot mutate a newer operation even if its participants are deleted.

## Direct-constructor compatibility finding

`Data/Scripts/Custom/ThirdParty/OmniAI/OmniAI Shared.cs:143` directly constructs `new BandageContext(m_Mobile, m_Mobile, delay)` without installing `m_Table`. This is an existing production caller, not a hypothetical API user. A blanket `GetContext(healer) == this` requirement would disable its healing.

Preserve this path explicitly. Simply registering all direct contexts in the existing public table also newly exposes those AI heals to slip handling and turn-based capture; that is a behavior change which must be deliberate and tested. A scoped private ownership mechanism can preserve direct-constructor visibility while protecting replacement actions, or the implementation can document and justify a bounded normalization. In either design, an old direct context must not become valid again after a newer registered context finishes and removes its table entry.

The parent proposed retaining private `m_Table` for all ownership and adding `m_Registered` so public `GetContext` still exposes only `BeginHeal` contexts. Constructor replacement retires the previous private owner; only the registered current owner may clear the public effect registration. This is an acceptable bounded design and avoids a second dictionary. Internal identity checks must use the private table rather than filtered `GetContext`. Older direct contexts are deliberately cancelled when a newer context starts; direct/regular replacement tests must prove that correction while preserving OmniAI's public invisibility.

Source trace: `BeginHeal` installs the regular table entry and calls the bridge after constructing the context (`Bandage.cs:789-802`). `TurnBasedCombatManager.EffectTimerChanged` clears an effect on null timer and captures/suspends the current context on a recognized timer (`:1032-1073`); effect advancement reads `context.Timer` (`:2014-2033`) and exit calls `ResumeFromTurnBased` (`:2479-2480`). The accepted Dice generation pattern at `DiceChannel.cs:846-920` is a reference, not another repair target.

## Independent fixture expectations

1. Reproduce on the historical assembly: enqueue A, replace it with B, invoke actual `Timer.Slice`; A currently performs old work/clears B. Keep this historical result separate from current regressions.
2. Candidate A queued -> stop -> B installed -> dispatch A: B's table/timer/effect registration remain exact; old patient hits, messages, skill checks and sounds do not change. Repeated old `StopHeal` and `EndHeal` remain inert toward B.
3. Dispatch current B once with `Running == false`, as normal one-shot scheduling does; it completes normally once. A second dispatch/end must not repeat effects or skill checks. Include an independent ordinary one-shot timer to show core scheduler compatibility.
4. A queued -> suspend -> dispatch A while suspended: no early completion. Advance partially; second suspension preserves remaining duration. Resume with timer B; dispatch A again: still inert. Dispatch B: exactly one normal completion.
5. Turn advance past remaining duration completes once, clears its own table/effect, and rejects all stale real-time dispatches/extra advances. Confirm the timer runtime type/property remain usable for turn-effect classification.
6. Deleted healer/patient, cancellation, stale contexts after a replacement has itself finished, and direct OmniAI-style construction must retain the intended ownership behavior. Include direct -> regular, regular -> direct and direct -> direct replacement according to the chosen design.
7. Check actual hits and callback side effects, not merely table membership. Use a bridge capture or focused real manager setup to prove stale callbacks do not clear B's effect registration. Deterministically queue actual production timer objects without a background timer thread; clean fixture queue/state and close owned capture resources.
8. Forced isolated runtime script compile, exact source/assembly/fixture hashes and relevant current regressions. No core executable change is expected. Then required isolated in-game Qwen acceptance remains a separate pending gate coordinated by the parent.

## Completed code/regression review

The implementation follows the reviewed private-table/registration design. Internal action identity is independent of the public `GetContext` filter. Constructor replacement retires any previous direct or registered owner; direct OmniAI construction remains publicly invisible while still healing normally. `StopHeal` clears table/effect state only for the owning registered operation. Timer dispatch requires exact timer identity and a nonsuspended context; `EndHeal` then rechecks current ownership. No `Running` check or core timer change was introduced. Suspension is idempotent, resumed callbacks use a new timer, and the suspended timer subtype remains available to the manager. Timing/healing formulas and `Bandage` serialization are unchanged.

No unresolved material source or component-regression finding remains. This is sufficient for the scoped local code commit, while the user's in-game acceptance gate remains pending.

Evidence root: `C:/Users/nepht/AppData/Local/Temp/confictura-foundation-20261025-batch3-001`.

- Isolated Release/x86 operational build and forced runtime script compilation passed with exit zero and required success milestone.
- Independent hashing verified all **6,729** input records against the frozen snapshot and current checkout. Test core SHA-256: `dab39ca00acda5c0fce2b59a9fdc95c4a054b1f02cd5db6c851ef9a29c0fad6e`; script assembly SHA-256: `c6fa63037b28ef3d4fab0b760c1d9637b38954f039d5d9ac1f451be4860bd0e5`.
- `bandages-run.log`: **62** current assertions through actual `Timer.Slice`, exit zero and normal process exit. Identity `bandages-aef97d5d.identity.json`; fixture SHA-256 `2e5c83626a3e81cc0a5402f7c5d23ae365c534dc2556539264882b9f43a8a24f`, independently matched to its copied and repository source.
- Actual coverage includes stopped one-shot completion, cancelled/replaced/repeated/queued callbacks, unchanged old hits/skill checks/packets, retained newer table/effect state, suspend/partial advance/repeated suspend/resume/turn completion, deleted participants, all direct/registered constructor replacement directions, stale direct callbacks after the newer context finishes, existing dex-based delays, and real `TurnBasedCombatManager` capture/type-resolution/clear methods driven by a bridge fixture.
- Current regressions on these exact assemblies also passed: payments **74**, actions **261**, Research **46**, champion **679**. Their structured results show expected markers, exit zero and no forced termination.
- Historical reproduction remains separate in Batch 2 candidate002: `bandages-historical-run.log`, **12** assertions proving old replacement clearing/healing, pre-suspend callback completion, repeated completion and stale cancellation. It is not current acceptance.

`git diff --check` passed. Direct comparison confirms the `Bandage` Item serializer region is unchanged from the committed baseline. No core source or tracked executable changed; the tracked production core remains Batch 1 SHA-256 `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`. The isolated rebuilt test core is not a replacement deliverable.

## Limits and remaining gate

The component fixture starts no listener, client, background timer thread or world load. It deterministically reproduces the engine's enqueue-before-stop state and dispatches through actual `Timer.Slice`; source review confirms the unchanged timer engine supplies that state in normal execution. The manager fixture exercises real effect methods with a minimal configuration and participant, not a whole live turn-based match.

Three `SocketException` shutdown diagnostics appear when deleted test mobiles dispose deliberately unconnected capture sockets. They are retained harness diagnostics, not a warning-free runtime claim; the fixture still reports its assertions and normal exit, and its finalizer closes capture sockets and unregisters the bridge. The reviewer launched no runtime and owns no cleanup process.

Required isolated in-game Qwen testing through the existing FIX-01–05 session remains pending for Batches 1, 2 and 3. No live deployment, full copied-world save/load, real client behavior or shard-wide timer correctness is certified by this review.
