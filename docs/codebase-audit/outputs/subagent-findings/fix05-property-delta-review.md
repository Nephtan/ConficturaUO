# FIX-05 property-delta correction: independent review

Latest disposition: the candidate004 follow-up below resolves R1 at source/component level. Earlier sections retain the candidate003 review and failure history. Real-client acceptance remains open.

Reviewed 2026-10-19 UTC by a read-only subagent. The parent remains the sole native/runtime controller and owns source, test execution and Git. This review adds only this file; the pre-existing four-source-file repair and documentation WIP, untracked review HTML and FIX-01 harness are untouched. The review destination did not previously exist.

## Disposition

The candidate003 change correctly addresses the reproduced properties-only `Item.ProcessDelta` overflow. No additional defect was found in its coalescing, FIFO barriers, dispatch visibility checks or saved format. **FIX-05 acceptance remains open:** full property-list responses use a separate, untested producer route described below, and the candidate003 real-client rendering/interactions/reconnect/recovery/Qwen gates remain outstanding. A passing socket fixture does not complete those gates.

## R1 — full property replies still consume the copied control backlog

**Priority: P1 acceptance concern; source-confirmed route, not yet a reproduced candidate003 client failure.** `Item.SendPropertiesTo` still invokes `from.Send(PropertyList)` (`Data/System/Source/Item.cs:1116-1118`). Both `PacketHandlers.BatchQueryProperties` and `QueryProperties` call it after visibility/range checks (`Data/System/Source/Network/PacketHandlers.cs:1741-1807`; registrations at lines 148 and 165). `PropertyList` is the full variable-length `0xD6` packet, distinct from the nine-byte `0xDC` hash notification now deferred by this correction (`Data/System/Source/ObjectPropertyList.cs:64-75,207-211`).

When these responses are requested while world entries remain queued, `CaptureOrQueueWorldPacket` copies their compiled bytes behind the remaining FIFO and disconnects once aggregate copied control data would exceed 96 KiB (`Data/System/Source/Network/NetState.cs:1123-1140`). These replies cannot coalesce with incoming item references, and the copied responses are not revalidated at dispatch. Thus enough valid full-property replies behind a crowded incoming backlog can still reach `control_packet_limit`; whether the actual isolated client produces that traffic pattern is not established by the current evidence. Do not identify this route as the cause of the preserved client004 failure, which has a separately reproduced properties-delta cause.

Before accepting candidate003, exercise both actual query handlers against a pending crowded world FIFO, including realistic compressed replies, repeated requests and view change/removal. Verify exact full-property delivery and continued connection without increasing limits or weakening assertions. Retain the ordinary equipped/container response behavior and virtual `SendPropertiesTo` contract in any scoped repair. The current `PropertyDeltas` fixture does not call either handler or compare full `PropertyList` responses: it compares `SendInfoTo`/`OPLPacket` bytes only (`NetworkStreamingRegression.cs:285-355`).

## Confirmed correction and evidence

- Candidate002 to candidate003 changes only the properties-only branch in `Item.ProcessDelta` and `NetState`'s world entry/admission/dispatch representation. World items use `SendWorldItemProperties`; parented items retain the existing direct notification path (`Item.cs:5270-5290`).
- Same-generation, same-control-epoch merging ORs `SendOpl` and ANDs `PropertiesOnly`; an incoming request promotes a properties-only entry to a complete virtual incoming group. A control/remove barrier prevents merging across that boundary (`NetState.cs:846-879,881-896,1136-1138`). Generation and current item/viewer validity are checked before dispatch, with another validity check after capture (`NetState.cs:975-1004,1030-1067`). Property-only dispatch emits only the current `OPLPacket`, preserving the original notification kind. No serialization, namespace, type, project inclusion or hard-limit change appears in the candidate diff.
- The preserved real client004 failure is explicit: authenticated ordinary Player serial 521, 20,000 disposable items, `control_packet_limit`, `work=28995`, `controlBytes=98301`, and lost connection. `failure.json` lists later checks as not run; the console and streaming log agree. The existing identity assertion was retained.
- The identical fixture source reproduces the old candidate002 disconnect at its unchanged `actual properties-only ProcessDelta must not overflow copied control budget` assertion, then passes candidate003. Candidate003 `all.log` records **2,698 cumulative checks**, including actual `MoveToWorld` plus `ProcessDelta` for 20,000 items, exact complete wire bytes, standalone OPL-only output, both coalescing orders, control ordering and generation cancellation. It uses real loopback sockets/production send callbacks but manually advances the drain; no real client, world load, native UI or Qwen runs occur in that fixture. Its general stale-entry tests exercise the shared incoming validity path; properties-only invalidation directly covers generation, not every stale condition independently.
- Candidate003 build validation records successful server build and forced runtime script compilation. This review launched neither; their process results and source/assembly identities were inspected. Source-build success is distinct from runtime script compile success, and both are distinct from client acceptance. No project-file change requires an IDE project hygiene build.

## Exact reviewed identities and locators

External root: `D:/ConficturaUOTestTools/ordered-repairs-20261018/`.

| Artifact | SHA-256 |
| --- | --- |
| Main at candidate003 review and candidate003 `Item.cs` | `ae01a7532d24299ee1193470d958beaefc82eb69b8dcaed0b1f6708e30271f1f` |
| Main at candidate003 review and candidate003 `Network/NetState.cs` | `ef01921495ed8c25e626bae58171c080c3c47f22014efc879d4c737a5c814aa4` |
| Fixture source, both recorded results | `c3685643d4477a3a5661cdc56b203d730ef0d8ff17b8de5ef1980ee8b35549df` |
| Candidate003 core, verified on disk | `dc423d219041bd2321b74a2b494c47e6eed242147a283f56086b4b1083c79b93` |
| Candidate003 source manifest, recorded in run result | `8730fae0cb462decb81abbf2b9d48d9d78d048a547dbf1d5c757aea90e66ea16` |

Evidence inspected:

- `resume-20261019/CLIENT004-FAILURE.md` and `client-004/evidence/failure-01-control-packet-limit/{failure.json,WorldItemStreaming.log,Console.log,client-cache-errors.log,in-memory-restore.json}`.
- `fix05/nonui-candidate-002/fixtures/property-delta-repro/{properties-result.json,properties.log}`.
- `fix05/nonui-candidate-003/fixtures/network/{all-result.json,all.log,NetworkStreamingRegression.cs}` and `build-validation.json`.
- `git diff --no-index` for candidate002/candidate003 `Item.cs` and `NetState.cs`, bounded numbered source reads, `rg` producer searches, and SHA-256 comparisons against current main. The runtime-script search found no current script override of `SendPropertiesTo`; its virtual API still needs preservation.

No acceptance rerun, runtime/client/model launch, native control, production edit or Git mutation was performed by this reviewer. The parent must consolidate this concern, record any resulting regression/repair and rerun affected acceptance before a local FIX-05 commit. FIX-02/03/04 remain outside this review.

## Candidate004 follow-up: R1 reproduced and corrected

The parent reproduced R1 on candidate003 through the actual batch and single query handlers, with the unchanged assertion `actual full property query handlers must not overflow copied control budget`. The failing result is preserved in `fix05/nonui-candidate-003/fixtures/property-query-repro/{queries-result.json,queries.log}` (fixture `80637edfa447fcbe226168fa06fc4d925fe2dce550945071a3bbcb54148204fe`). This establishes a separate component failure; it does not retroactively change the identified client004 failure cause.

Candidate004 changes only `NetState.cs` and the two item-reply call sites in `PacketHandlers.cs` relative to candidate003. The original packet-handler visibility/range checks and mobile-query paths remain intact. `NetState.SendItemProperties` (`NetState.cs:847-878`) preserves parented replies through immediate virtual `SendPropertiesTo`. World replies instead enqueue distinct `PropertyListOnly` references and advance the control epoch. They are deliberately absent from the incoming coalescing dictionary, so a full reply is not reduced to a hash notification or merged away; repeated requests retain separate FIFO positions. Existing generation and item validity checks apply before dispatch (`NetState.cs:1013-1037`), and the virtual callback runs outside the admission lock inside the bounded packet capture (`NetState.cs:1064-1103`). A property-query entry cannot remove a later incoming dictionary entry because dequeue removal requires reference identity. No saved format, namespace, type, project inclusion or queue limit changes. No new source blocker found.

The expanded actual-handler fixture passed on candidate004: `fixtures/network/all-result.json` records exit 0, no timeout, process exited, and `all.log` records **2,761 cumulative checks**, including 20,002 full query replies with exact 1,548,137-byte wire comparison. It covers batch/single handlers, repeated replies, the virtual multi-packet callback, ordinary control ordering, stale generation/visibility/range/map/parent/deletion, and both equipped/container reply paths. Expected packet caches are cleared before actual dispatch. Candidate004 server build and forced runtime script compile also passed in `build-validation.json`. These are inspected parent-run results, not reviewer-launched tests or real-client acceptance.

One narrow test-strengthening request was sent to the parent: the current virtual ordering case is `query -> ordinary control -> incoming`, which includes an independent epoch barrier. Add `incoming -> query -> incoming` for the same item with no ordinary control packet, checking three entries and exact wire order, to isolate the new query barrier itself. This is a regression-coverage gap; the source increment is present and correct.

**Closed after follow-up inspection:** `fix05/nonui-candidate-004/fixtures/query-barrier/NetworkStreamingRegression.cs:443-456` adds exactly that sequence, asserts three entries, exact complete bytes and one virtual callback. `queries-result.json` and `queries.log` record 73 passing query checks, exit 0, no timeout and process exited, on the same candidate004 core/manifest. Fixture SHA-256 is `7bce8e224db7ce51a93998acd98c61d3e0d6d6bb81bc94242d7703d4c24b5433`. The earlier full-suite source/result remain separately preserved; the follow-up is not mislabeled as another full-suite run.

Verified candidate004 identities:

| Artifact | SHA-256 |
| --- | --- |
| Main and candidate004 `Network/NetState.cs` | `7c7da3adae870ec9bfcca896eda6d62c593ae5194c176a3202075ee65dacd77f` |
| Main and candidate004 `Network/PacketHandlers.cs` | `248241948604cfd40fd402138a040d33aec27c732dfc694147d9a3e4ae896d0b` |
| Candidate004 core | `36ab5701d188c636db8f9c4abe0b957d42f0228dc9d4d43f2840b9134d492778` |
| Candidate004 source manifest | `b5dea57ac126292044e3e4feeaf14b814e0850f4ff04601a7adebd8d25188412` |
| Expanded passing fixture | `a688c468393a8a59b35cb20d356f32629909728a1d49a553a62b2c4831133af5` |

R1 is resolved for the reviewed source and component reproduction. Real-client rendering, interactions, authenticated reconnect/recovery and supervised Qwen checks must still pass on the applicable candidate before final acceptance/local commit. Reviewer's scope remains this additive findings file only; no runtime launch, native control or Git mutation was performed.
