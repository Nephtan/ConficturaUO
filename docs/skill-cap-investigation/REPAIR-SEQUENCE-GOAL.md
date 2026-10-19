# Ordered skill-cap repair continuation

**Current FIX-05 checkpoint (2026-10-19):** Required isolated acceptance and independent review passed on candidate004, including client011 house customization entry/rendering/normal exit. Tracker reconciliation passed; the accepted implementation is committed locally as `69cc69b6ac2f7e474a9e2aee4ca5dead7bb95fb7`. All owned runtimes are stopped; full copied saves/profiles and all 14 protected records were restored and verified. FIX-02, FIX-03 and FIX-04 follow in order. See [current validation](FIX-05-RESUME-VALIDATION.md).

Started 2026-10-18 on `main`, baseline `ac9546500860482f8c2a37193361b80ae3dcbdd7`.

**Active continuation:** The current direct human resume supersedes the historical account pause. This chat is the sole controller. The [account handoff](ACCOUNT-HANDOFF.md) records the latest accepted batch and next work.

The user authorized development, isolated testing and clean local commits in this exact order: **FIX-05, FIX-02, FIX-03, FIX-04**. Push, deployment, live restart, production-save changes, model training and unrelated Resident/Shardmind changes are excluded. Complete and review each repair before advancing. No empty implementation commit is needed for a repair already satisfied by existing code.

Initial worktree: no tracked modifications; user-owned `Confictura-backlog-review-2026-10-17.html` and intentionally excluded FIX-01 harness files `scripts/Test-SkillVerificationProtection.ps1` and `tests/SkillVerificationProtection.cs` are untracked and preserved. Root `AGENTS.md` and nested audit-output instructions apply. No `.agents/skills` directory was found; the current local shell provides ripgrep. No branch change or source reorganization is planned.

## Sequence and acceptance

| Repair | Status | Required outcome and verification |
| --- | --- | --- |
| FIX-05 | Committed | Retain existing Create Food rejection containment (`0eb2a07a`). Complete the separately documented crowded nearby/login transmission and staff recovery gaps with bounded work, preserved packet compatibility, no world-item deletion or decay-policy change. Verify existing food variants/full-pack behavior, queue boundaries, stale/deleted/moved entities, login/movement/reconnect/recovery, real Player effects and bounded Qwen choices where useful. |
| FIX-02 | NotStarted | Source-verified transactional player grant/transfer boundary; route discovered automatic grants and explicit privileged exceptions. Preserve existing points, locks/caps and NPC/load intent. Test all identified grant families, rejection and conservation, repeated/stale responses, and normal valid gains. |
| FIX-03 | NotStarted | One source-supported cap calculation, origin eligibility at the action boundary, idempotent Ether completion and non-destructive cap-change handling. Test all origin/boost/Ether combinations, established characters, stale responses and old save compatibility. Ask only if repository evidence cannot resolve a consequential policy choice. |
| FIX-04 | NotStarted | Reliable player identification and real-total accounting; restrict automatic XML accounting/identity mutation without breaking explicit staff/load semantics. Test false/high/low caches and flags, reflection routes, callbacks and persistence; keep FIX-01 preservation green. |

For each repair: inspect current source and applicable instructions, record acceptance/design and source traces, make the smallest useful change, use isolated production-method tests, obtain independent review, resolve findings, update active CSV/workbook and reports/Resident feedback, verify staged scope and commit locally with a descriptive body. Core-source fixes must include the matching validated tracked `ConficturaServer.exe`, with isolated build provenance, configuration and hash; never include a test overlay or unfinished later fix. Script-only changes do not inherently require a core rebuild. The already accepted FIX-05 artifact is reconciled additively without rewriting its source commit. Keep source and documentation commits focused and avoid duplicate backlog credit.

## Isolation and desktop control

Use fresh disposable copies beneath `D:\ConficturaUOTestTools`, verified consistent input saves, source hashes and finite fixtures. The existing direct-local task `01a14f0a-6dfa-7133-a92a-f2f5bef78a02` is the sole native UI/client/Qwen controller. Desktop control is inactive for source, build and waiting work; activate supported Computer Use only for a concrete UI-dependent step, then release/stop active control immediately. Do not alter persistent security settings or bypass a denial.

Preserve Owner/Player separation and independently verify actual client effects; a Resident result label alone is not evidence of success. Keep positive demonstrations separate from failing examples. Restore original saves/profiles by hashes, retain exercised copies as evidence, stop task-owned runtimes and release desktop control after every acceptance run. Do not commit fixtures, credentials, saves or runtime output.

## Historical initial checkpoints

- Initial source/history inspection confirms FIX-01 commits `1b49df74` / `ac954650`. The only pre-existing changes are the three preserved untracked files above.
- FIX-05 is not fully satisfied by `0eb2a07a`: `post-batch-runtime-performance-closeout.md` explicitly excludes broader network/queue work; current `Mobile.SendEverything` and `Mobile.SetLocation` still synchronously send every nearby item. `NetState.Send` disconnects on the unchanged 96 KiB queue ceiling. Existing Create Food behavior must not be duplicated or reverted.
- Independent read-only FIX-05 review requested. Native UI and gameplay runtimes have not been started for this continuation.
