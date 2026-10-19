# FIX-05: bounded crowded-view delivery and recovery

2026-10-19. Candidate004 completed the required isolated build, component, full-world, real-client and supervised Qwen acceptance. Independent review passed; the closeout reconciles the active tracker and records a focused local commit. Baseline is `ac9546500860482f8c2a37193361b80ae3dcbdd7` on `main`. No deployment or live incident attribution follows from these tests.

## Problem and resulting behavior

Bulk nearby-item creation, movement, login and refresh previously compiled/sent every eligible item immediately. A crowded view could exceed the existing 96 KiB socket queue and disconnect, including during reconnect. Real isolated acceptance also exposed properties-only delta and property-query replies that bypassed the first draft's pacing. Candidate004 routes these world-item paths through one bounded per-connection FIFO while preserving complete packet groups and control/removal ordering.

Create Food rejection containment was already implemented in `0eb2a07a5799a4c1755588ecacb321623989827a`. It remains unchanged and receives no duplicate completion credit. Existing world items and house decay/storage policy remain intact. The new repair handles bounded delivery and recovery; it does not prove what caused the historical live incident or promise unlimited scene density.

## Source and compatibility

| Source / symbols | Behavior |
| --- | --- |
| `Data/System/Source/Network/NetState.cs`: SendWorldItem, SendWorldItemRemove, BeginWorldViewChange | Reference entries recheck current visibility/map/range/parent/deletion and view generation. Ordered removals/control packets remain barriers. |
| Same: SendItemProperties; Item.ProcessDelta; PacketHandlers.BatchQueryProperties/QueryProperties | Root-world property deltas and both actual item-query handlers share the bounded stream. Explicit queries remain distinct ordered entries and advance coalescing epochs; parented replies retain their existing path. |
| Same: DrainWorldSendQueue, WorldSendTimer, SendWorldItemGroup, SendPacket | A shared timer budget drains complete virtual world/OPL/corpse/house groups; packet encoding occurs exactly once in dispatch order. Raw and encoded group sizes are independently bounded. |
| Same: m_WorldSendLock and capture ownership | Background sends use synchronized admission/encoding and cannot join another thread's virtual-item capture. Virtual dispatch runs outside the admission lock. |
| Same: ReportWorldSend, WriteWorldSendDiagnostic | Best-effort asynchronous diagnostics are rate limited per connection and permit only one global outstanding writer. They are not a guaranteed audit trail. |
| `Data/System/Source/Network/SendQueue.cs`: PendingBytes | Exposes the existing conservative byte estimate without increasing the 96 KiB cap. |
| `Data/System/Source/Mobile.cs`: SendEverything, SetLocation, ClearScreen; `Item.cs`: MoveToWorld, Location, ProcessDelta | Routes bulk world-item entry/update/removal through the ordered producer; surrounding mobile/container/equipment protocols stay on their existing paths. |

Limits: 65,536 logical deferred entries; 96 KiB copied generic/control backlog; 64 KiB and 256 packets per captured group, with an independent encoded 64 KiB ceiling. Each 25 ms slice sends at most 64 entries and examines at most 512, starting dispatch only at or below 16 KiB socket pressure. A hard limit or dispatch exception explicitly disconnects rather than silently truncating a view. Coalescing is confined to the same generation and interval between barriers. Large scenes can take longer to arrive.

No serialized field/type/version/order, namespace, project include or runtime script changed. Item/Mobile Serialize/Deserialize bodies remain identical to baseline after newline normalization. The five changed source files match the frozen candidate004 manifest `b5dea57ac126292044e3e4feeaf14b814e0850f4ff04601a7adebd8d25188412`; core SHA-256 is `36ab5701d188c636db8f9c4abe0b957d42f0228dc9d4d43f2840b9134d492778`.

## Verification

| Scope | Result |
| --- | --- |
| Operational server build | Release/x86 Server.csproj passed, exit 0, 20.85 s. |
| RuntimeScriptCompileTruth | Forced `-service -compileonly -nocache` passed, 42.59 s. Later world/client copies also compiled their separately pinned test overlays and reached full isolated startup. |
| Existing regressions | Create Food 1,853; Research logout 46; FIX-01 protection 817 plus 60 registered-region transitions and historical gain matrix passed. |
| Actual-loopback component networking | Full matrix 2,761 checks plus separately reviewed query-barrier 73 passed: exact bytes/order, fresh packet caches, encoding/lifetime, control/query/remove barriers, worker admission, stale/changed items and bounded failure. Component drain scheduling is manually advanced. |
| Full world and save/reload | World007 registered producers/natural timer/recovery/save prepare and reload passed. 70,281 root-owner/serial/type/amount rows and 1,398 progression rows match across the strict baseline, network stages, save and reload. Existing recorded empty-BankBox normalization and passive Meditation/Focus suppression remain explicit fixture controls. This does not check every item property or intra-owner placement. |
| Actual authenticated Player | Client007 crowded login/reconnect, refresh and staff-assisted next-login recovery passed on candidate004 with unchanged progression and empty queues. Logical Atlantis identity is checked separately because Sosaria/Atlantis share MapID 1. |
| Native rendering/interactions | Client009 unobscured pile positions, populated corpse/foundation/sign, ordinary native movement and house-sign menu passed. All 20,000 client-cache entries were accounted for separately; pixels show nine overlapping pile positions. Client011 real customization editor entry/render/normal exit passed with DesignContext false→true→false. |
| Supervised local Qwen | Client009 real model choice opened the allowlisted corpse. Bridge dispatch alone did not confirm an effect; subsequent native contents rendering did. No autonomous Mira or new C11 inference claim. |
| Cleanup | Exercised copies archived; protected inputs and complete copied saves/profiles verified/restored; exact task processes stopped and loopback ports freed. |

Detailed current evidence, exact filenames, process/assembly/source pins, negative attempts and independent review links are in [resumed validation](FIX-05-RESUME-VALIDATION.md). Historical candidates are never relabeled as candidate004. No optional solution/Scripts.csproj build was needed because project inclusion did not change. No live shard restart, push, production-save write or production-load benchmark was performed.

Retained failures include early worker ordering, client004 property-delta overflow, candidate003 property-query overflow, client006/007 guarded travel exceeding the unchanged three-second observation lease, client008 clear-scene selection, client009 Price=0 menu setup, and client010 native activation. The C11 retry resolves the last remaining native gate; it does not erase earlier failures or establish their broader frequency.

## Tracker closeout

The active CSV/workbook adds one FIX-05 record: 6,824 total, 954 Fixed, with all prior records preserved. Affected summaries, native formulas, filters and chart caches were reconciled. Existing duplicate summary cell records were canonicalized so formulas are no longer shadowed by cached literals. Artifact Tool's wildcard-date calculation limitation is retained in the run log; date caches were independently checked against every CSV row. Native Excel recalculation was not performed. No broader workbook redesign or duplicate Create Food credit is included.

## Existing staff recovery procedure

Use existing serial-targeted property commands, not a new rescue endpoint or a second character sent into the crowded scene. `SerialCommandImplementor.Execute`, `SetCommand` and `Properties.SetValue` retain access checks and audit logging; `Mobile.LogoutMap`/`LogoutLocation` require GameMaster write access.

1. Preserve normal backup/evidence, identify the exact character serial and inspect its current and logout fields.
2. Stop client retries and wait for the disconnected character to reach Map.Internal. The normal logout timer can overwrite fields if it has not completed.
3. From a safe staff location, set both next-login fields to an independently verified safe destination through existing authorized serial/property commands. Read back and inspect audit records.
4. Have the ordinary Player reconnect and verify actual destination, usable view and retained progression/inventory. A staff success message alone is insufficient.
5. Investigate the original scene separately. Do not default to item deletion, house-decay changes, skill resets or moving the offline mobile into the world.

Client007 exercised Player/Counselor rejection and authorized audited GameMaster property writes followed by real saved-login recovery. No production destination or staff action is authorized by this report.

## Rollback and continuation

Reverse only this repair's five named core-file changes and rebuild through the normal operational workflow if rollback is requested; preserve independent Create Food/FIX-01 repairs and unrelated work. No save migration is required by this diff. Runtime activation remains user-owned.

After reviewed local FIX-05 closeout, continue FIX-02 → FIX-03 → FIX-04. The later grant/cap/accounting policies are separate source-verified work. [Resident feedback](FIX-05-RESIDENT-FEEDBACK.md) separates bounded positive observations from failure examples and does not authorize training.
