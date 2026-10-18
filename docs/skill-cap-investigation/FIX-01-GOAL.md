# FIX-01 protection repair goal

Started 2026-10-18, baseline `17c7217b99a3dc7a44d3a7567fcabfd87ddea520`, branch `main`.

Goal: region-entry validation must preserve existing skills, stats, profession and cap components even when validation or diagnostics fail. Provide bounded, useful diagnostics without inventing progression policy. Preserve ordinary gain checks and the serialized layout. The initial scope prohibited commits. The user later authorized clean local commits after testing; implementation is now `1b49df74e8b972eb53805a790797f1f4872ab001`. Push, live restart, production save changes, original Resident/Shardmind edits and model training remain outside the authorized scope.

Pre-existing unrelated files: `Confictura-backlog-review-2026-10-17.html` and `docs/codebase-audit/outputs/~$post-audit-active-backlog-status.xlsx` (untracked). Preserve both. No overlapping source edits were present. Root AGENTS applies to source/tests/scripts; `docs/codebase-audit/AGENTS.md` also applies to audit outputs. `.agents/skills` is absent in this checkout. `rg` is unavailable; use literal PowerShell/Python searches.

The current task supersedes the prior test-root instruction restricting original-project edits, but retains isolation, evidence and Resident feedback requirements. Original test snapshots and inventories remain protected. Work outside this checkout requires the environment's normal approval review.

## Acceptance and sequence

1. Confirm old destructive behavior on disposable in-memory characters, with baseline source and save backups identified. Add FIX-01 to the active backlog.
2. Replace SkillVerification correction with read-only validation and rate-limited exception-contained reporting. Keep SkillCheck, origin eligibility and serializers unchanged.
3. Exercise under/exact/over cap, original 500/1200 case, every configured boost tier, profession/virtue mismatches, cache disagreement, repeated region transitions, valid gains, validation exceptions, logging failure and save/load. Distinguish deterministic production-method tests, full-world restart, real-client and Qwen evidence.
4. Request independent read-only review, address findings, and update reports, workbook and Resident feedback with reproducible acceptance cases.
5. Preserve rollback originals, stop owned runtimes, verify protected saves/inventories and unrelated files, and honor the later local-commit authorization after review. Exclude the new regression harness from Git while preserving exact external copies; record commit identities in the external handoff. Push and activation remain user-owned.

## Progress

- Inspection complete: legacy numeric whitelist rejects 1200.0 even at 500.0 real points; failure calls SkillBegin, resets profession and every skill, and conditionally stats. Accepted branches also mutate profession/components.
- Existing historical tests in `tests/SkillCapInvestigation.cs` characterize the wipe and will remain historical evidence. New repair-specific fixtures will be separate.
- Policy boundary: removing region correction does not repair public grants, falsified Player/cache, load-time cap recalculation or accelerated gain loss. FIX-02 through FIX-04 (and FIX-06) remain separate; no new sanction or grant policy is required for preservation.
- Implementation, regression, save/restart, independent review and durable deliverables complete. Directly authorized native Player/client/Qwen continuation passed the bounded FIX-01 progress scope. Failed gate-result labeling and exact in-memory inventory comparison are retained separately; full input saves and client profiles were restored by verified hashes, and owned runtimes stopped. Implementation is locally committed as `1b49df74e8b972eb53805a790797f1f4872ab001`, not pushed or deployed. This evidence/documentation batch accompanies it; commit identities are recorded in the external handoff. Push and activation remain user-owned. See [FIX-01-REPORT.md](FIX-01-REPORT.md) for exact evidence and limits.

Local commit closeout: source commit `1b49df74e8b972eb53805a790797f1f4872ab001` is verified. This documentation/backlog batch and sanitized feedback accompany the implementation; commit identities are recorded in the external handoff. The two new regression files are intentionally excluded from Git and preserved externally with relative-path hashes; all gameplay/model runtimes remain stopped.
