# Approved foundation repairs, 2026-10-25

All three batches were logged before implementation. Implement and independently review code/regressions in order. Final acceptance also requires the previous FIX-01–05 Qwen in-game session. This is a scoped post-audit repair sequence, not a restart of the completed audit runner or prior test controllers.

## Baseline and identity reconciliation

`main` at `c747ecd82b6096e92aae5bdd6f31989b528ff614`; tracked worktree clean. FIX-01 through FIX-05 remain accepted with their existing meanings. Core source has no diff from FIX-04 (`89470b0c`), and tracked executable SHA-256 is `a6d8a30f17d9718158fa505a9fdf99805d42956da2e6ed53c37d7a920eb345a9`.

FIX-06, FIX-07 and FIX-08 remain reserved historical P2 recommendations in `docs/skill-cap-investigation/REPAIR_RECOMMENDATIONS.md`. They have no dedicated active-overlay rows. FIX-07 only partially overlaps response-time authorization: these repairs do not close its SkillStone/election policy scope. New IDs begin at FIX-09. Existing RB/P10 dispositions and historical Phase 13 outputs are unchanged.

Source-confirmed review: `C:/Users/nepht/AppData/Local/Temp/Confictura-next-fixes-2026-10-24.md`, recommendations 1-6. That report is static evidence, not runtime acceptance. The earlier `post-audit-next-steps.md` 6,819/952 headline is historical; the verified opening active CSV has 6,824 unique rows, 957 Fixed. This sequence adds seven distinct rows.

Unrelated untracked paths: `.codex-remote-attachments/`, `Confictura-backlog-review-2026-10-17.html`, `scripts/Test-SkillVerificationProtection.ps1`, `tests/SkillVerificationProtection.cs`. Preserve all. No `.agents/skills` exists in this checkout. Root, audit, Government Gumps and XML Quest Gumps instructions were read; obsolete Ubuntu/static-only environment assumptions do not describe this Windows execution environment and do not supersede the user's explicit isolated-test authorization.

## Ordered batches and acceptance

| Batch | ID / overlay ID | Status | Source boundary and required acceptance |
| --- | --- | --- | --- |
| 1 | FIX-09 / POST-FOUNDATION-FIX-09 | Code/regression complete; in-game pending | `Item.Consume`; all `Container.ConsumeTotal`/grouped and `ConsumeUpTo` paths; `Banker.Withdraw`; `BaseVendor.OnBuyItems`. Negative/mixed-array inputs cause no mutation/callback; retain zero, positive, exact/multiple-stack use. Invalid/extreme costs and duplicate serials reject before payment/stock change; valid purchases charge delivered amount. Matching isolated core executable required. |
| 1 | FIX-10 / POST-FOUNDATION-FIX-10 | Code/regression complete; in-game pending | Thin fixture preflight: frozen core/scripts/config/reference inputs, exact candidate assembly hashes, fixture compile before execution, current versus historical manifest. API/reference mismatch, stale snapshot, missing milestone and timeout fail clearly; owned process exit recorded. No new test framework, saves, listener or client needed for component scope. |
| 2 | FIX-11 / POST-FOUNDATION-FIX-11 | Code/regression complete; in-game pending | Both XML `QuestRewardGump` and `PointsRewardGump`: current actor/entry/points/credits/cost eligibility before creation/debit; replaced entries and repeated responses reject; success exactly once. Preserve intentional ground fallback, control slots and existing prices; no arbitrary callback rollback claim. |
| 2 | FIX-12 / POST-FOUNDATION-FIX-12 | Code/regression complete; in-game pending | `BODBuyGump.OnResponse`: full/missing pack, missing funds and construction/delivery failure preserve money/book entry and leave no newly abandoned deed. Preserve successful stacking/delivery semantics. |
| 2 | FIX-13 / POST-FOUNDATION-FIX-13 | Code/regression complete; in-game pending | `CityTreasuryWithdrawPrompt`, `CityTaxesIncomePrompt`: bind original actor/live stone and recheck current mayor-or-GameMaster at final action and safe reopen. Lost office, demotion, deleted stone, mismatched actor reject without mutation; valid mayor/staff and existing limits pass. |
| 2 | FIX-14 / POST-FOUNDATION-FIX-14 | Code/regression complete; in-game pending | `CityCorpseFeeGump`, `CityResurrectionStone`: pure eligibility before debit, agreed fee, valid corpse/actor/location/quota, commit quota/treasury once. Missing/deleted corpse, quota exhausted, insufficient funds, moved actor, changed fee and replay leave resources unchanged. Preserve free `RegCorpse` callers and serialization. Paid resurrection completion is explicitly deferred. |
| 3 | FIX-15 / POST-FOUNDATION-FIX-15 | Code/regression complete; in-game pending | `BandageContext`: context plus timer ownership, idempotent cancel/completion. Queued A after replacement B cannot heal, message, skill-check, remove B or clear B's effect registration. Suspend/resume and turn advancement complete once; deleted participants reject. Preserve global Timer, timing formulas, skills and serialization. No second timer defect selected. |

Each batch requires historical reproduction, current pass/fail regressions, forced runtime script compilation, relevant existing regression checks, independent review and a focused local commit before advancing. A core change also needs an operational Release/x86 build from a frozen complete core snapshot and the identical validated tracked executable. Runtime compile and IDE/project hygiene are distinct. No new production source files or project changes are planned.

Related rows retain their meanings: RB-01609/01610, RB-01970/01971, RB-04875/04876 (numeric/vendor neighbors); RB-05217, RB-02823/02828/03778/03783 (reward neighbors); RB-02561/02562/03516/03517 (city menu opens); RB-00093/SERIAL-0032 and RB-05293 (corpse serialization/coupling); accepted Dice records (different ownership repair). Government relocation and balance fences remain.

## Execution and evidence policy

Reuse accepted fixtures and tooling patterns under `D:/ConficturaUOTestTools` read-only; use a unique temporary snapshot for new component evidence, so old STOP/controller state remains intact. Freeze all candidate build inputs together, hash copied source and assemblies, and run production methods without saves/listeners. Retain every failed attempt and explicitly distinguish historical reproductions from current acceptance. No client, Resident or model is started by this component task. The user now requires in-game Qwen acceptance through the previous FIX-01–05 session after all code/regression batches; the parent coordinates that existing session. No new controller/session is substituted. No original saves/accounts, production runtime, push, deployment, permissions or Codex memories are changed.

Opening process observation: `Get-Process` found no Confictura/RunUO process. CIM process querying was unavailable (access error); this is not startup evidence. Recheck exact process paths before replacing the tracked executable. Build and runtime acceptance are pending.

## Batch results

Batch 1 code/regression work complete after measured checks and independent review; see [results](foundation-batch-1-results.md). Delivered in the focused source/matching-core commit containing this update. Batch 1 commit: d9b4703f. Batch 2 code/regression work complete; see [results](foundation-batch-2-results.md). Batch 2 local code commit: `7f146df6`. Batch 3 local code commit: `143e9fb3`. Batch 3 code/regression complete; see [results](foundation-batch-3-results.md). All three require in-game acceptance before complete/accepted status. CSV has 6,831 unique rows / 957 Fixed; seven foundation rows remain Open pending final acceptance; workbook reconciliation is complete: native features and all6,824 prior rows retained, seven foundation rows Open. See [tracker verification](foundation-workbook-verification.json).

Acceptance update, 2026-10-25: prior Batch 1 accepted wording referred to its component scope and is superseded by the explicit in-game gate. Preserve commit d9b4703f and all evidence. Independent code review allows sequential implementation to continue, but does not close final acceptance.

Combined candidate with the exact tracked core and final scripts passed forced runtime compilation and74/261/62 focused assertions. [In-game handoff](foundation-ingame-handoff.md) and [durable manifest](foundation-candidate-manifest.json) are ready for original session01a14f0a-6dfa-7133-a92a-f2f5bef78a02, dispatched by the parent through coordinator01a151f3-ef87-7102-8b24-e85667fb40d9. This coding task does not activate it.

Coding-task cleanup verified at2026-10-25T15:07:25Z: no owned foundation processes and no repository server process. No world/client/model/controller was started; no copied fixture restoration was necessary. The user-required in-game stage remains pending with the original session.
