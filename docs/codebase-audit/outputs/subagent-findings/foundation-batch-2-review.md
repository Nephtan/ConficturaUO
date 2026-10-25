# Foundation Batch 2 independent review

Design reviewed 2026-10-25 after Batch 1 commit `d9b4703f`. Scope: FIX-11 XML rewards, FIX-12 BOD delivery, FIX-13 city prompt authorization, FIX-14 paid corpse retrieval. The reviewer changes only this report. Root/audit instructions and the relevant XML Quest/Government gump instructions were read; the latter's historical Ubuntu/no-runtime statements do not describe this Windows test environment. No layout redesign or new UI pattern is needed for this bounded callback repair.

**Current status: code/regression review complete; in-game acceptance pending.** The user's later October 25 requirement adds isolated in-game Qwen testing through the existing FIX-01–05 test session before Batch 2 may be called fully accepted or complete. Preserve the earlier source/component review and results as evidence; they do not fulfill this additional gate.

## Design findings and required boundaries

### R1: XML reward selection must remain the offer the actor saw

`QuestRewardGump.OnResponse` and `PointsRewardGump.OnResponse` currently validate credits but not minimum points, original actor, selected page or stale entry identity. The hidden button is not authorization. Their `XmlQuestPointsRewards`/`XmlPointsRewards` entries expose mutable public `Cost`, `MinPoints`, `RewardType` and `RewardArgs` fields; retaining only the list/entry reference does not bind an offer. Snapshot the selected offer fields/arguments, then compare with current list entry before construction, while checking live points/credits and nonnegative cost. Original actor, deleted actor, bounds, invalid entries, page eligibility and per-response ownership belong at the callback boundary.

Make each gump response consumable once before invoking arbitrary constructors or delivery callbacks. Recheck payment success before declaring a completed purchase. Preserve existing zero-cost behavior, mobile control policy and XML item ground fallback. `Mobile.AddToBackpack` returning false means it attempted the current world fallback, not necessarily delivery failure; merged stack items can also be deleted on successful delivery. Never delete a successful stacked reward merely because its original object is deleted.

Construction failure needs an explicit diagnostic/outcome and no credit charge. An arbitrary constructor can register an Item and then throw before `Activator.CreateInstance` returns its reference. This bounded repair cannot promise universal cleanup of arbitrary constructor side effects; do not hide that limitation or introduce unsafe global world-difference deletion. Similarly, mobile/attachment callback transactions are not universally reversible.

### R2: a reconstructed BOD needs an owner until purchase commits

`BODBuyGump.OnResponse` reconstructs a deed before `CheckHold`, then abandons it on missing/full-pack rejection. Keep the new nonstackable deed under local `try/finally` ownership. A useful narrow order is validated quote -> reconstruction -> successful `TryDropItem` -> successful existing pack-or-bank charge -> remove the book entry/credit vendor; failed delivery or payment deletes the new deed. Book entries and vendor funds must remain unchanged until that success boundary. Reject a nonpositive offer and overflowing vendor `HoldGold` before debit. Revalidate original responder, live book/vendor, retained entry and current displayed price; preserve legitimate successful behavior.

`CheckHold` is only a precheck. Tests need a container which accepts it but rejects `TryDropItem`, not only an ordinary full pack. Do not retain `AddToBackpack` world fallback for this promised backpack purchase. BOD construction, delivery failure and insufficient funds must preserve the entry and resource totals and leave no newly owned deed.

### R3: city authority must hold at the mutation boundary

`CityManagementGump.OnResponse` currently authorizes mayor or `AccessLevel.GameMaster` and above. Reuse that precise role rule in the treasury/tax prompts with the original actor and live stone; safely guard cancel/reopen paths too. Preserve legitimate staff actions and avoid adding a range, assistant-mayor, city-membership or other new policy.

Use `TryParse` rather than a broad mutation catch so a real operation exception is not mislabeled as invalid numeric input. Preserve tax bounds 0..100 and current treasury behavior, including the existing `CityTreasury <= requestedAmount` refusal and 1,000,000 maximum. Null/deleted entries in announcement/vendor collections may be skipped locally to keep an authorized operation from failing during notification; do not claim that this authorizes a broad city prompt sweep.

### R4: corpse eligibility is a pure check until payment succeeds

`CityCorpseFeeGump` currently charges first, then tests corpse/quota while reporting that failed service took no money. Merely moving `RegCorpse` earlier would spend quota before a failed withdrawal. Split quota eligibility from quota recording, preserve `RegCorpse` as the free-service check-and-record wrapper, and keep the existing three-use weekly allowance and serialized layout/version unchanged.

Bind original actor, quoted fee and live stone/sign/city identities. At response validate current membership, same map/range, current nondeleted corpse, nonnegative unchanged fee, remaining quota and nonoverflowing treasury addition before bank debit. A failed eligibility check must leave even a previously null `Ghosts` table unchanged. Repeated/stale callbacks cannot charge or record quota twice. On a changed quote, request a fresh offer; do not silently charge a different amount. Success charges, credits treasury, moves the corpse and records usage exactly once. Preserve the separate free path; paid resurrection confirmation is explicitly out of this batch.

## Acceptance matrix

- Both XML menus: forged hidden/minimum-points response; points or credits changed after open; negative/changed cost; replaced/deleted/invalid/out-of-bounds entry; changed reward type/arguments; actor mismatch/deletion; unoffered page; duplicate response; construction failure; ordinary item, successful stacking and existing full/missing-pack world fallback. Invalid eligibility must precede construction and debit. Qualify arbitrary constructor/attachment/mobile side-effect limits honestly.
- BOD: missing/full/refusing backpack; insufficient funds; changed/nonpositive quote; replaced entry/deleted book/vendor; original-actor mismatch; normal pack and bank purchases once. Compare book entries, vendor HoldGold, buyer resources and newly reconstructed world objects across failure/success.
- City prompts: mayor replaced after opening; staff demoted; actor mismatch; stone/actor deleted; cancel/reopen; valid current mayor and GM; tax endpoints and invalid values; strict treasury balance/max rules. Unauthorized responses must emit no success or financial mutation.
- Corpse: missing/deleted corpse; no membership; moved actor/map; dead sign/stone/city; changed or negative fee; treasury overflow; exhausted quota; insufficient funds; successful retrieval; stale/repeated response; all three valid uses and fourth refusal; `ClearGhosts`; free `RegCorpse` wrapper. Check exact bank, treasury, location and quota state, and accurate failure messages.
- Forced isolated runtime script compilation against the exact candidate. Relevant current payment/Food/accounting/Research/champion regressions as appropriate. Compare serializer bodies before/after; no new save format. Existing Batch 1 tracked core stays unchanged for script-only Batch 2.

## Initial result

Design review completed before implementation. Existing accepted repairs and neighboring historical RB dispositions remain separate. The reviewer launched no test runtime and owns no cleanup action. The completed code/regression review is recorded below; full batch acceptance is pending the added in-game gate.

## Candidate review checkpoint

Reviewed the eight-file production candidate after the design handoff. Shared `XmlRewardOffer` is placed in an existing compiled file, snapshots and revalidates offer fields before and after construction, checks the actual debit result, and preserves stack/ground/mobile/attachment behavior with cleanup of returned objects on failed delivery. The prompt predicate preserves mayor-or-GM authorization and numeric limits. Paid corpse eligibility is checked without quota mutation before the bank call; the free `RegCorpse` wrapper and all serializer bodies remain intact.

The pre-final candidate001 evidence reports seven historical reproductions on the Batch 1 assembly and 217 current action assertions, plus payment/Food/accounting regressions. These are useful but do not accept later source revisions. A final review request identified a BOD delivery edge: a custom container can report success while discarding or deleting the new deed. The candidate now requires a live deed still beneath the pack after `TryDropItem` and before payment. This bounded correction needs its own frozen runtime-compiled assembly and fixture proof before acceptance.

The reviewer also requested final tests for XML attachment compatibility, changed reward type/deleted actor, replaced city links/map, quota reset, and BOD dead vendor/book/overflow. Earlier harness setup failures and the pre-final candidate remain historical evidence; they are not to be overwritten or counted as current acceptance.

## Completed independent code/regression review

**Code/regression review complete for the scoped Batch 2 local script commit; in-game acceptance pending.** No unresolved material source or component-review finding remains after the BOD retained-deed guard and expanded cases. Independent inspection rehashed all **6,729** candidate002 input records against both the frozen tree and current checkout, verified generated assembly hashes, and confirmed the final action fixture matches its recorded copy and repository source. `CityResurrectionStone.Serialize` and `Deserialize` remain identical to the Batch 1 committed baseline, including version 1 and version 0 handling. `git diff --check` passes.

Evidence root: `C:/Users/nepht/AppData/Local/Temp/confictura-foundation-20261025-batch2-002`.

- Release/x86 operational core build and forced runtime script compile passed with normal exit. Generated test core SHA-256: `3cb51da8b2886ff2ed69b20cb7e9bd52fa3fa714cc64e5696b67e87833c50878`; generated script assembly SHA-256: `1c6c9f746ec3492c89a62656d55164613e3d54659dc82cd37411d8bc8af0a070`.
- `actions-run-attempt-2.log`: **261** current action assertions, exit zero, expected marker, no forced termination. Identity `actions-8c7ddc59.identity.json`; source SHA-256 `85cd6a74c523e08668163e71f3a9a9e9c274771b00d1bed06f98096df1291d06`.
- Current regressions on those same candidate assemblies: payments **74**, Food **1,853**, accounting **3,972**; all required markers and normal exits were independently inspected.
- Historical reproduction is separately preserved under Batch 1 candidate001: `actions-historical-run-attempt-6.log`, **7** assertions across XML points, stale city authority, premature corpse charging and abandoned BOD. Earlier harness initialization/setup failures remain retained and are not accepted runs.

The final matrix includes both XML menu implementations; live minimum-points/credits, changed offer fields/args and page, actor mismatch/deletion, reentry/replay, deliberate factory/drop failure, item stacking, full/missing-pack ground fallback, normal controlled mobile and `XmlStr` attachment behavior. BOD tests cover refusal, throw, claimed drop without retention, deletion, missing/full pack, funds, price/entry changes, dead book/vendor, HoldGold overflow and pack/bank success. Prompt tests cover mayor/staff success and loss of authority, actor mismatch, deleted stone, cancellation, repeat, parsing and existing limits. Corpse tests cover failed resources/messages, changed fee/corpse/sign/city/map, pure quota checks, exact successful payment/treasury/quota, repeated responses, three paid uses/fourth refusal and reset, plus the free wrapper.

Batch 2 changes no core source or tracked executable. The tracked production core remains the accepted Batch 1 artifact SHA-256 `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`. Candidate002's isolated rebuild is verification, not a replacement artifact. Snapshot input count excludes `Server.csproj.user` intentionally; the original IDE file still exists in the repository and was not removed.

### Limits retained

Completed evidence covers actual production callbacks in bounded in-memory fixtures and forced runtime compilation. No new real client, full copied-world save/load, live activation or deployment is claimed; required isolated in-game Qwen acceptance is still pending. Capture sockets are unbound and never connect/listen; fixture cleanup closes them. The factory failure fixture deliberately throws before returning an object; arbitrary constructors that register entities and then throw, arbitrary callback side effects, attachment replacement side effects, and mutation after partial delivery are not universally rolled back or certified. Shared currency/balance policies and paid resurrection confirmation remain out of scope. These limits do not negate the specifically verified guards, returned-object cleanup and unchanged-resource failure paths.
