# Foundation batches: existing Qwen session handoff

**STOP: Batch1/2 accepted in their reviewed scope; Batch3 in-game acceptance remains pending and PAUSED. No automatic resume, heartbeat or schedule.** See [acceptance/pause checkpoint](foundation-acceptance-pause-20261025.md) and `D:/ConficturaUOTestTools/foundation-ingame-20261025/BATCH2-PAUSE-HANDOFF-20261025.md`. Resume only after explicit human instruction. The scenarios and original pre-test handoff below are retained history, not launch authorization. Use the same `ConficturaUO-mainortest` session that performed FIX-01–05. The parent verified original session `01a14f0a-6dfa-7133-a92a-f2f5bef78a02`; coordinator `01a151f3-ef87-7102-8b24-e85667fb40d9` can resume it using local codex_app tools. Parent dispatches testing; this coding task must not activate it. This coding task has not started a shard, client, Resident, model or controller, and has not created a replacement session. Do not run competing controllers.

The user explicitly added this gate on 2026-10-25. Batch 1's earlier component acceptance wording is superseded, without rewriting its commit or discarding evidence. All seven active-overlay records remain `Open` / `CodeRegressionComplete-InGamePending`. FIX-06–08 remain reserved historical P2 recommendations; existing FIX-01–05 and audit dispositions are unchanged.

## Commits and candidate identity

| Batch | IDs | Local code commit | Component evidence |
| --- | --- | --- | --- |
| 1 | FIX-09 shared deductions/vendor quotes; FIX-10 fixture preflight | `d9b4703f` | 21 historical assertions; 74 current payment assertions; five deliberate preflight failures detected; matching core delivered in the same commit. |
| 2 | FIX-11 XML rewards; FIX-12 BOD delivery; FIX-13 city authority; FIX-14 corpse payment/quota | `7f146df6` | Seven historical assertions; 261 current action assertions. |
| 3 | FIX-15 bandage context/timer ownership | `143e9fb3` | 12 historical assertions; 62 current assertions through real Timer.Slice and manager effect methods. |

All batches have independent code/regression review. Relevant existing regressions passed on final Batch 3 candidate001: Food1,853, accounting3,972, Research46, champion679, payments74 and actions261. No fixture assertion is an in-game test.

Combined handoff candidate: `C:/Users/nepht/AppData/Local/Temp/confictura-foundation-20261025-handoff-001`. Its complete frozen input manifest is `inputs.json`; exact assembly hashes are `assemblies.json`. [The durable candidate manifest](foundation-candidate-manifest.json) records all13 changed source hashes, fixture hashes and commit identities. This candidate uses a copy of the **tracked** executable, not another fresh rebuild:

- `ConficturaServer.exe`: `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7` (Batch 1 source/artifact commit).
- `Data.dll` / runtime `Data/Data.bin`: `ed2ed6bce7b75ab30b38d5ebb4be14f039561919f7f882853053466bf5b3f23b`.
- `packaged-core.json` verifies all124 operational core inputs against Batch 1's validated source snapshot; the unrelated IDE `.user` file is excluded and preserved.
- Forced runtime compile with that exact tracked executable passed; payments74/actions261/bandages62 compiled against and passed on these exact assemblies, with normal process exits.

The input manifest was captured while the latest commit was Batch 2 and Batch 3 source was reviewed but uncommitted; file hashes, subsequently committed as143e9fb3, are the candidate authority. Recheck current source hashes before setup. Do not copy test EXEs, environment folders, build intermediates or component fixture classes into production scripts. If the existing session runtime-compiles scripts again, record the newly generated assembly hash and source manifest; do not claim byte identity to this Data.dll unless verified.

## Session and fixture ownership

Read the existing session's current instructions and latest external FIX-04 closeout, plus [ACCOUNT-HANDOFF](../../skill-cap-investigation/ACCOUNT-HANDOFF.md). Its historical endpoints were localhost25215 and existing guarded bridge/control/model25291/25292/21447; these are references to recheck, not instructions to assume they are free. Verify process paths/owners and ports before any start. Keep existing origin/skill/security controls, account selection, safe manual macro policy and protected inputs unchanged.

Use the existing isolated copied shard/client profile. Before mutation, snapshot and hash its complete copied Saves, settings, DataProfiles and ClassicAssistProfiles and the previously defined14 protected originals/backups. Restore exactly after testing. Never use production saves/accounts or the repository executable against live data. All setup entities below are disposable copied-world fixtures with recorded serials and original state. No policy or global permission broadening is needed.

The committed standalone fixtures are recipes and evidence, not server scripts:

- `tests/FoundationPaymentRegression.cs`: `Vendor`, stock/resale and negative/duplicate/overflow scenarios.
- `tests/FoundationActionRegression.cs`: both XML reward lists, city/sign/stone, BOD book/vendor and resource accounting matrices.
- `tests/FoundationBandageRegression.cs`: queued one-shot setup, capture boundaries, direct/registered constructors and turn-based classification.
- `tests/foundation-fixtures.json` and `scripts/Test-Foundation.ps1`: exact-assembly preflight. Historical entries require explicit selection and cannot pass current acceptance.

Only the prior test-session owner should adapt any required temporary staff fixture command into its established copied-world overlay. Keep overlays out of `D:/ConficturaUO-main/Data/Scripts`; record their hashes and remove/restore them at closeout. Do not weaken account or command access. Each negative case needs both native observations and authoritative before/after resource snapshots. A hidden/forged callback or queued race cannot be proven merely by clicking rapidly: retain deterministic callback evidence and label its mechanism separately from the Qwen/native observation.

## Batch 1 in-game cases

Use an ordinary Player with1000 gold, a disposable vendor stocked with10 stackable arrows at10 gold each, an empty/roomy pack, and a separate bank-funded variant. Capture gold/check totals, stock amount and received item amounts.

| Case | Action and expected observation |
| --- | --- |
| B1-01 successful purchase | Qwen chooses and buys3 arrows. Native purchase completes once, pack gains3, funds fall30 and vendor stock falls3. For the bank variant set price2000, bank5000 and pack1000, then buy2: bank1000, pack1000, stock8 and delivery2. Existing bank fallback requires totalCost>=2000; do not expect a30-gold order to use bank funds. Staff free purchase remains a separate authorized GM case with no charge. |
| B1-02 insufficient funds / stock clamp | Make funds insufficient: refusal with no stock/item/payment change. Request more than stock: existing clamp delivers available amount and charges only that amount. |
| B1-03 shared deduction / crafting compatibility | On disposable stacks, invoke the exact negative/mixed-array deduction cases from the fixture through a bounded staff test hook: no count change, deletion or callback. Then a legitimate craft/positive exact-stack consumption succeeds with expected resource use. Qwen observes the resulting inventory; the hook and counts are recorded separately. |
| B1-04 duplicate / numeric refusal | The bounded hook submits duplicate vendor serial entries, negative quantity, negative price, product overflow and aggregate overflow using `OnBuyItems`. Each refuses the entire quote with unchanged funds, stock and inventory, including resale and staff paths. Native UI should remain usable for a valid follow-up purchase. This is a callback injection case, not a client packet-format claim. |
| B1-05 preflight and prior behavior | Run current fixture preflight against the session's exact core/scripts first; require expected markers, source/assembly identity and clean owned-process exit. The five deliberate failure probes already pass in component evidence; do not mislabel them as UI tests. Observe a normal bank withdrawal and unchanged skill/origin restrictions in the existing protected-player fixture. |

## Batch 2 in-game cases

Use separate disposable ordinary Player/mayor/GM fixtures; staff prepares state but does not promote the ordinary acceptance actor. Restore each case's input state before the next.

| Case | Setup, action and expected observation |
| --- | --- |
| B2-01 both XML menus | For each menu use a temporary reward entry with MinPoints50, cost10, points100 and credits100. Qwen selects it: one reward, credits90, clear success. Test item stacking and the intended full-pack ground fallback separately. Record normal controlled mobile and attachment outcomes if included in the session fixture. |
| B2-02 stale XML offer | Open the menu, then lower points below50 or credits below10, change cost/type/arguments, or replace the entry. Selecting the stale offered button must create nothing and debit nothing. A hidden-button injection with initial points0 also refuses before construction. Replayed response is inert. Repeat in both QuestRewardGump and PointsRewardGump; first reward response is button1000 in the fixture. |
| B2-03 BOD delivery | Place one10-gold BOD entry in a non-sale BulkOrderBook under a PlayerVendor. Full/refusing/missing backpack or insufficient funds: no new abandoned deed, entry retained, buyer funds and vendor HoldGold unchanged. Normal pack and bank purchase: one deed in pack, entry removed, buyer-10/vendor+10. Qwen observes failure and success; deterministic custom-container refusal/throw/claimed-success-without-retention stays separately labeled. Purchase response button2. |
| B2-04 final city authority | City treasury10000, tax0. Open withdrawal/tax prompt as mayor; change mayor before response. Enter10: no funds/tax change, no success. Repeat GM then demote before response. Fresh current mayor and legitimate GM may withdraw10 (treasury9990, delivered10) and set tax10 with existing vendor propagation. Cancel, invalid input, tax0/100 and strict treasury-balance limit retain current behavior. |
| B2-05 corpse preconditions | Linked live city/sign/resurrection stone, member Player within2 tiles on same map, fee10, bank100, valid recorded corpse. Open offer then delete/replace corpse, remove membership, move out of range/map, change fee or exhaust3-use quota: refuse with accurate 'no money was taken' message and identical bank/treasury/quota. Insufficient funds also preserves quota. Paid acceptance response button1; cancellation button2. |
| B2-06 corpse success and quota | Fresh valid offer: bank-10, treasury+10, corpse at actor, quota+1, native success/effect once. Replay is inert. Repeat fresh offers for uses2/3; fourth refuses without payment. Test ClearGhosts reset only on the disposable stone, then one successful use. Free RegCorpse behavior remains valid. Paid resurrection confirmation is outside this repair. |

## Batch 3 in-game cases

Use a living, fed healer and patient on the same map/in range, patient damaged, sufficient bandages, deterministic high Healing/Anatomy for the success observations. Record context/timer identity, hits, skill-check calls if the session hook exposes them, native messages and turn effect registration.

| Case | Action and expected observation |
| --- | --- |
| B3-01 ordinary healing | Qwen applies bandage to self and another patient. Expected duration and successful healing remain; moving out of range retains the existing failure. Normal one-shot dispatch must still complete although its timer is stopped by the scheduler before dispatch. |
| B3-02 cancelled A, newer B | Bounded copied-world hook queues A using the fixture's real queue-before-Stop order, starts B, then dispatches Timer.Slice. A must not heal, train, message, clear B's table entry or clear B's turn effect. Qwen observes only B's later completion. Keep actual identities and before/after snapshots; rapid double-bandaging alone does not prove the queued race. |
| B3-03 suspend/resume and turn | Capture a bandage into the existing turn-based fixture. Queue old timer, suspend, advance partially, resume with a different timer; dispatch old callback: no completion. Repeated suspension must not reset time. New timer completes once. Separately advance a suspended context to completion, then dispatch old callback: still one completion and effect cleared once. Verify the native turn UI remains coherent. |
| B3-04 stale/deleted/direct callers | Deleted patient/healer: no completion or skill change. Repeated cancel/completion remains inert. Existing OmniAI self-heal still works; direct constructor remains invisible to public GetContext. Direct->direct, direct->regular and regular->direct replacements retire the old action; after newer completion the old callback stays inert. |

## Required closeout before acceptance

Record actual Qwen task/choice/transcript, native screenshots/observations, candidate and overlay hashes, actor identities/access, resource deltas and every failed or unrun scenario. Qwen narration is not proof of a state change; inspect authoritative state after the response has actually completed. Keep component and client evidence separate. Full copied-world load/save/reload or client reconnect results should be recorded if run, never inferred from compile success.

Stop only session-owned runtimes, close Resident/model through existing controls, clear native bindings, and restore/verify all copied fixtures/profiles and protected originals. Record process/port checks and normal versus forced exit honestly. Independent review must inspect that evidence before changing each batch to accepted/complete or the seven overlay rows to Fixed. If anything fails, leave the affected acceptance pending with exact action, target, reason and next safe step. Push, deployment and live restart remain excluded.
