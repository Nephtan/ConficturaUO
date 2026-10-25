# Foundation Batch 2: FIX-11 through FIX-14

**Current disposition: accepted within the final reviewed isolated in-game scope.** See [acceptance/pause checkpoint](foundation-acceptance-pause-20261025.md). Earlier component-only statements below are historical, not the current acceptance status.


Code/regression work and independent review are complete. **In-game acceptance is pending** in the previous FIX-01–05 Qwen session, as requested on 2026-10-25. This report does not claim batch completion or client testing.

## Bounded changes

- FIX-11: both XML reward menus bind the original actor and displayed offer, consume a response once, and validate live minimum points, credits, nonnegative cost, entry identity, type and arguments before construction and debit. The shared `XmlRewardOffer` helper is in the existing QuestRewardGump file. Returned rewards are cleaned up and debits refunded on failed delivery; existing item stacking/ground fallback, controlled mobiles and attachments remain supported.
- FIX-12: BOD purchases retain ownership of a reconstructed deed until delivery and payment succeed. Missing/full/refusing packs, failed construction, insufficient funds and stale/invalid offers preserve resources and the book entry. A successful drop must leave a live deed beneath the backpack before payment. Cleanup runs on exceptions too.
- FIX-13: treasury/tax prompts recheck the original responder, live city and current mayor-or-GameMaster authority at final action and safe reopen. Existing numeric limits and legitimate staff actions remain; parsing failures are distinguished from operation failures.
- FIX-14: paid corpse retrieval checks current actor, corpse, city/sign/stone, map/range, membership, quoted fee, treasury capacity and remaining quota before withdrawal. `CanRegCorpse` is pure; the existing free `RegCorpse` wrapper still records successful uses. Failed eligibility leaves bank, treasury and quota unchanged with accurate messages. Paid resurrection confirmation remains deferred.

Eight existing script files changed. No project entries, core source, serialization layout or skill/origin policy changed. `CityResurrectionStone.Serialize`/`Deserialize` are identical to the Batch 1 baseline.

## Reproduction and regression evidence

Evidence roots are under `C:/Users/nepht/AppData/Local/Temp/`:

| Evidence | Result |
| --- | --- |
| `confictura-foundation-20261025-batch1-001/actions-historical-run-attempt-6.log` | Seven historical assertions reproduce both XML minimum-points defects, stale treasury/tax authority, premature corpse charging and BOD abandoned-deed paths on the frozen pre-Batch-2 scripts. These are historical reproductions, not current regression passes. |
| `confictura-foundation-20261025-batch2-002` | Operational Release/x86 build and forced `-compileonly -nocache` runtime script compile passed. All 6,729 frozen inputs match the source candidate. |
| `actions-run-attempt-2.log` | 261 current assertions passed; fixture identity `actions-8c7ddc59.identity.json`, source SHA-256 `85cd6a74c523e08668163e71f3a9a9e9c274771b00d1bed06f98096df1291d06`. |
| Same candidate regressions | Payments 74, Create Food 1,853, FIX-04 accounting 3,972; all passed with expected milestones, exit zero and no forced termination. |

Coverage includes invalid/changed offers, deleted/mismatched actors, repeated and reentrant responses, delivery refusal/throw/false-success, normal item/mobile/attachment rewards, stack and ground fallback, BOD pack/bank purchases, current mayor/GM success, lost authority, numeric boundaries, exact corpse bank/treasury/quota changes, three paid uses/fourth refusal/reset and the free wrapper. Full details and independent hash checks are in [the review](subagent-findings/foundation-batch-2-review.md).

Earlier harness setup failures (capture socket/state, city collections/name and race/vendor initialization), candidate001 and its pre-final 217 assertions remain archived. They are not credited as final candidate evidence. Review found a BOD false-success delivery edge; candidate002 includes and tests the retained-deed guard. The runner intentionally excludes the unrelated `Server.csproj.user` input; that IDE file remains untouched in the repository.

## Executables, cleanup and limits

Candidate002 generated core SHA-256: `3cb51da8b2886ff2ed69b20cb7e9bd52fa3fa714cc64e5696b67e87833c50878`; scripts: `1c6c9f746ec3492c89a62656d55164613e3d54659dc82cd37411d8bc8af0a070`. These are the exact assemblies used by the fixtures. Core source is unchanged from Batch 1; the tracked matching executable remains `42591637093a792a6e734eb31b5473c070f9f36fa1bf09e66685188632b353c7`. The isolated fresh rebuild is verification, not a second packaged artifact.

All owned compile/test processes exited. Capture sockets were unbound, never connected/listening, and closed in fixture cleanup. No copied world, original saves/accounts, client, Resident, model or prior controller was opened; there is no copied fixture to restore. Runtime compile returns before world load/listening. No push or deployment occurred.

Arbitrary constructors can register side effects before returning a reference; arbitrary mobile/attachment/delivery callbacks are not universally reversible. This repair does not implement or claim a universal transaction system. Real-client observations, full copied-world save/load and Qwen acceptance are unrun and remain required under the updated acceptance instruction. The prior session will receive a dedicated scenario/candidate handoff after Batch 3 code/regression work.
