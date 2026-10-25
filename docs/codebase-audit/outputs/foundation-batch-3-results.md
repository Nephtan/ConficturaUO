# Foundation Batch 3: FIX-15

**Code/regression work and independent review complete; in-game acceptance pending** in the existing FIX-01–05 Qwen session. Only `Data/Scripts/Items/Trades/Misc/Bandage.cs` changes production behavior. No other timer or spell repair was selected.

## Ownership repair

Every bandage context now owns its private table entry and timer. Replacement retires the earlier context. `StopHeal` removes only its own entry and clears a turn-based effect only for its registered owner. `EndHeal` requires current ownership, rejects suspended real-time completion and is idempotent after completion/cancellation. Deleted participants release the current action without healing or skill checks.

The timer callback checks both context ownership and exact timer identity. It deliberately does not require `Timer.Running`: the existing one-shot scheduler queues a timer and stops it before `Timer.Slice`. Suspension retains the timer subtype for turn-based effect classification, repeated suspension preserves the remaining duration, and resume creates a different timer so queued pre-suspend callbacks remain stale. Turn completion invalidates any queued real-time callback.

Independent review found the direct constructor used by `Custom/ThirdParty/OmniAI/OmniAI Shared.cs`. A private registration flag preserves that caller's prior invisibility to public `GetContext`, slipping and normal turn-based capture; it still heals. Direct and regular constructors now share replacement ownership, so an older direct heal cannot complete after a newer action. Tests cover all direct/regular replacement directions.

No global Timer change, timing/healing formula change, namespace/project change, serialization change, or skill/origin policy change was made.

## Measured evidence

Evidence roots are under `C:/Users/nepht/AppData/Local/Temp/`:

- `confictura-foundation-20261025-batch2-002/bandages-historical-run.log`: 12 historical assertions reproduce stale cancellation/replacement, suspend/resume and repeated completion on pre-Batch-3 scripts. The fixture populates the real queue in the scheduler's queue-before-Stop order, then calls production `Timer.Slice`; no timer thread is started.
- `confictura-foundation-20261025-batch3-001`: Release/x86 operational build and forced runtime compile passed. `bandages-run.log` passes 62 assertions through real `Timer.Slice`, production methods and real `TurnBasedCombatManager` effect capture/classification/clear methods within an isolated bridge harness.
- Same final candidate: actions 261, payments 74, Food 1,853, accounting 3,972, Research 46 and champion 679 passed. All compile/test processes exited normally with expected markers.

Bandage checks cover stale hits, skill-check count, packet/message count, table/effect preservation, cancelled/duplicate dispatch, normal stopped one-shot completion, suspension/repeated suspension/partial advance/resume, exact-once turn completion, deleted/null participants, direct constructor compatibility and unchanged self/other healing delays. Integration uses a deliberately minimal in-memory manager and configured bandage rule, not a running turn-based game session.

Three `SocketException` shutdown diagnostics remain in the current fixture log when deleted test mobiles dispose deliberately unconnected capture sockets. Assertions and process exit passed; these are retained harness teardown diagnostics, not warning-free client or live-runtime evidence. Capture sockets are never bound, connected or listening and are closed in `finally`.

## Identity and cleanup

All 6,729 candidate inputs match the checkout and snapshot. Manifest SHA-256 `1c9c71895ff56eb45625c5e499d0cd4e2f60bf51e382b174a70716dfed4878fc`; Bandage source `442bc92505a243bec5a0c998a4e3c2d2ad8504d96d1bad5c1f461db7d85a3215`; fixture `2e5c83626a3e81cc0a5402f7c5d23ae365c534dc2556539264882b9f43a8a24f`. Fixture identity: `bandages-aef97d5d.identity.json`.

Candidate test core `dab39ca00acda5c0fce2b59a9fdc95c4a054b1f02cd5db6c851ef9a29c0fad6e`; scripts `c6fa63037b28ef3d4fab0b760c1d9637b38954f039d5d9ac1f451be4860bd0e5`. Core source is unchanged from Batch 1 and the tracked executable remains `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`.

No saved world, account, client, Resident, model or prior controller was opened; there is no copied fixture to restore. No push/deployment occurred. Full copied-world/client/Qwen acceptance is required and unrun. [Independent review](subagent-findings/foundation-batch-3-review.md) completes the code/regression gate only; the sequence handoff will identify the combined candidate for the existing in-game session.
