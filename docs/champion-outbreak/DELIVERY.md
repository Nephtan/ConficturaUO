# Arachnid Outbreak delivery checklist

Status: CompletePrototype. Production activation is disabled. Specification: [SPECIFICATION.md](SPECIFICATION.md).

Targeted native results and their evidence limits: [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md).

## Current acceptance checkpoint

[RELEASE_REVIEW.md](RELEASE_REVIEW.md) is the final source/evidence reconciliation. Historical sections below retain earlier states and failures. The user explicitly approved all six permanent footprints. Real-policy preflight, approved-catalog/housing/controller regressions and copied startup now pass; see GEOGRAPHY_APPROVAL.md. Six reservations/catalog entries are reviewed; DeliveryReady stays false.

| Gate | Current evidence | Remaining boundary |
| --- | --- | --- |
| Source/runtime | 6742 current inputs match;13 outbreak project entries; forced runtime compile/startup in corrected native copies; current manager/bridge fault regression and web92 | Static inventory is separate from runtime, saved-world and gameplay evidence |
| Geography | Chosen October17 snapshot6/6 clearance with186 regions;6 native routes/1356 steps;6 boss plus2 brood presentation samples | Approval received; actual-policy6/6 preflight passes; foliage caveats and future deployment checks remain |
| Participation/claims/crafting | Component/fresh-process records; native005 controlled claim collection;6 independently verified native role types; actual archmage earns6 glands/1 skull/15000 ground gold | Controlled scenes and earning/collection remain distinct |
| Web/TBC | Production/AP tests; natural live003 reaction/escape; live-dodge0022AP avoidance; connected ally rescue | No stopped-scheduler timing or universal pixel claim |
| Veteran |3/3 comparable complete mounted-v4 runs;24.426736-minute median;2 without antivenom | No rerun needed; tuning0; no broad AI change |
| Zanthus | Original macro stationary partial133.473s and assisted clear277.502s; earned power preserved | One bounded observation per mode; no repeatable-farm/current-live-character claim |
| Economic observations | Actual finite supplies; nominal recurring subtotals4198/3742/4318; separate pending/ground/backpack reward records | Pickup/corpse value/market demand unknown; no profit or antivenom-causality claim |
| Delivery audit |44 per-requirement source/test records and final dispositions; guarded reproduction/teardown and operational/rollback notes | Approved catalog completed/verified; production deployment remains outside authorization |

[ACCEPTANCE_REQUIREMENTS.csv](ACCEPTANCE_REQUIREMENTS.csv) preserves each evidence class and a separate final-review disposition. ComponentEvidence/NativePartial never become generic live passes. The final review explicitly retains all measurement and presentation limits. Recent focused checkpoints include6fc15604,fb88e3ad,67713875 andec27468c.

## Mounted reference checkpoint - 2026-10-29

Mounted-v4 no-antivenom run001 completes in 1452.7598366 seconds (24 minutes 13 seconds), all three broods and all 40 owned deaths including Mephitis. The character remains alive; actual loss/restoration is 7846 each, with 2094 native run requests, no walk requests/refusals and no client errors. No production ability, AI or tuning change was made. This is one complete candidate run, not the required three-run median.

The unchanged terminal capture, frozen capabilities/policy and verification remain at D:/ConficturaUOTestTools/outbreak-mounted-benchmark-20261029-001/evidence/. VerifyBenchmark.py --require-clear passes 6742 production inputs, matching core, 20 private save files, protected originals and acknowledged forced no-save teardown. Resource deltas and source-price limits are recorded in ECONOMICS.md. The missing-initial-status preparation wait now tolerates startup without restarting the server; the series verifier accepts consistent prepared v3 or v4 series and still rejects mixed capabilities/versions. Its three-run integration remains pending. The second fresh copy is prepared from the same baseline and candidate for comparison.

## Recovered second complete reference

The original mounted-v4 antivenom run002 continues through the model-capacity interruption without a duplicate or restart. The same actual server/client identities, live process-bound observer, frozen fixture and driver are verified on recovery. It completes in 1526.8203048 seconds (25 minutes 27 seconds), all 40 enemies defeated, no player death, 1746 run requests, no walk/refusal/client errors. VerifyBenchmark --require-clear passes 6742 source inputs, matching core, 20 private saves, protected originals and acknowledged no-save teardown. Native health loss/restoration is 8131/8100. The third comparable run remains required; no median is accepted yet. Mounted-v4 run003 is now running without antivenom after the same native preparation and a successful capability comparison with run001; its fixture and driver remain unchanged.

Evidence remains in D:/ConficturaUOTestTools/outbreak-mounted-benchmark-20261029-002/evidence/. ECONOMICS records actual finite consumption and exclusions. The capacity interruption did not change source, character capabilities, policy or production flags. The first-clear/helper/register batch is committed as 90f465ae. The untouched three original untracked files remain outside this work.

## Accepted three-run veteran pacing series

The existing third run is recovered after completion, with no duplicate or combat rerun. Its unchanged terminal capture reports 1465.6041709 seconds (24 minutes 26 seconds), all 40 enemies defeated, alive and mounted. The original owned processes are identified, acknowledged no-save cleanup removes both processes/listener, and VerifyBenchmark --require-clear passes. The single refused native path request is retained; the driver recovers and completes with no client errors.

VerifyBenchmarkSeries rechecks all three complete outcomes, prepared v4 capabilities, fixture/client policy, production inputs, starting saves/stocks, core and ordinary running policy. Times are 1452.7598366 without antivenom, 1526.8203048 with antivenom and 1465.6041709 without antivenom. Median 24.42673618166667 minutes passes the 20-35 minute gate, with zero player deaths and tuning0. Individual run reports do not assess a median; the separate series report now accepts it.

Portable aggregate: [VETERAN_SERIES.json](VETERAN_SERIES.json). Private originals: D:/ConficturaUOTestTools/outbreak-mounted-benchmark-20261029-003/evidence/mounted-series-verification.json and the three run evidence folders. This closes veteran pacing only. Native dodge/cooperation/ground presentation, current offline-save/site approval, ordinary loot/economic assessment and the final requirement audit remain open. Production and all proposed reservations remain disabled; broader NPC AI remains read-only.

## Baseline - 2026-10-28

Work on the current branch. Preserve these unrelated, pre-existing untracked files:

- `Confictura-backlog-review-2026-10-17.html`
- `scripts/Test-SkillVerificationProtection.ps1`
- `tests/SkillVerificationProtection.cs`

No production launch, save access, deployment, or automatic activation is authorized by this delivery.

## Prototype operation and enable/disable gates

These are source-verified operating notes for a later explicitly approved environment. This checkout retains DeliveryReady=false, with six explicitly approved permanent reservations and six derived reviewed site descriptors. No live deployment occurs.

| Client command (normal prefix [) | Access | Behavior |
|---|---|---|
| ArachnidOutbreak status | Administrator | Reports the unique controller's phase/site/cleared mask/status; missing or multiple controllers produce a count/error and no action |
| ArachnidOutbreak create | Administrator | With zero controllers, creates one disabled controller; creation itself is permitted while DeliveryReady is false |
| ArachnidOutbreak start | Administrator | Calls TryEnable; currently refuses because DeliveryReady remains false; later also requires a unique controller and a reviewed site |
| ArachnidOutbreak stop | Administrator | Disables the director, stops its timer and removes only entities matching its controller/attempt ownership |
| OutbreakClaim | Player | Collects that caller's pending awards subject to normal alive/backpack checks; uncollected/full-pack entitlements remain queued |
| BroodAntivenom | Player | Shows current protection/reuse status through the production status interface |

The prototype native/pacing/observational/requirement gates and six-footprint approval are complete within their recorded scopes. Future enabling requires an explicit deployment decision and refreshed copied-world occupancy, maximum-city-growth/service/portal checks for changes since October 17. Retain the approved catalog, permanent protection and presentation caveats; validate matching source/core, forced runtime compilation and copied-world reload before changing the delivery gate in that separately approved candidate. Test start/status/stop and legacy coexistence there first. These instructions do not authorize a production flag edit, world launch or deployment.

Stop is the operational disable path. Reservation protection is geography independent of the director, so stopping/deleting an encounter does not remove an approved housing ban. Pending awards are separate persistent claims; stopping is not a reward refund or replay. A missing/ambiguous controller must be inspected rather than silently creating/replacing active state. Do not use private fixture reflection as an operating procedure.

For rollback, retain serialized outbreak/claim/treatment classes so existing saves can load; disarm the director before considering source removal or geography changes. Keep matching backups and verification evidence in the approved operational workflow. This delivery performs no production save operation or binary save edits.

Source traces: [ArachnidOutbreakController.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs) Initialize/OnCommand/TryEnable/Stop/Restore/OnAfterDelete; [ArachnidOutbreakSites.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSites.cs); [WildernessEncounterReserve.cs](../../Data/Scripts/Custom/Champions/Outbreak/WildernessEncounterReserve.cs); [ArachnidOutbreakClaim.cs](../../Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakClaim.cs). [RESERVATIONS.md](RESERVATIONS.md) and [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md) retain the outstanding evidence boundaries.

## Batch 1 - Lifecycle, placement, persistence, staff controls

- [x] Separate versioned controller and state; default disabled.
- [x] Six terrain-validated, traversed/sampled and explicitly approved permanent footprints; real-policy preflight and derived catalog verified.
- [x] Availability, engagement, three brood stages, boss, recovery, cooldown.
- [x] Restart persistence, ownership cleanup, legacy coexistence (component/fresh-process evidence).
- [x] Staff access/guards and status/start/stop controls.
- [x] Component tests, runtime script compile, and focused commit.

## Batch 2 - Participation, rewards, crafting

- [x] Stage ledger, combat/pet/support credit and abuse exclusions; six controlled native cooperative role types retain separate scope.
- [x] Persistent idempotent pending awards and claim command.
- [x] Glands, two-treatment Alchemy recipe, prevention and cooldown.
- [x] Fixed shared gold shower, skull, artifact opportunity; no power scrolls.
- [x] Save/reload and old champion regressions; core build/artifact if needed (see dated evidence scopes).
- [x] Focused verified commit.

## Batch 3 - Combat and client acceptance

- [x] Finite defenders/reinforcements and guarded death transitions.
- [x] Web warning, avoidance, restraint, escape/rescue and turn-based integration within recorded component/native scopes.
- [x] Freeze legal veteran fixture and private Zanthus stress fixture.
- [x] Three full reference runs (20-35 minute median), one without antivenom.
- [x] Original Zanthus stationary/faster-clear observations and targeted native acceptance; repeatable farming remains unmeasured.
- [x] Separate economic/supply observations, retained failures and tuning0; unknown pickup/market values stay unknown.
- [x] Test-input preservation, owned forced no-save teardown, enable/disable instructions and focused commits.

## Completion audit

- [x] Trace every requirement to source and appropriately scoped evidence or the explicit geography decision.
- [x] Distinguish component, runtime compile, save/reload, scripted native, Computer Use and field/deployment evidence.
- [x] No missing gate is represented as passed. Geography approval is recorded and production activation remains disabled.

## Evidence and decisions

- Initial check: no tracked changes; only the three unrelated files listed above.
- Existing `scripts/Test-Foundation.ps1` snapshots source and compiles runtime scripts without loading saves or opening listeners. Reuse it for component gates.
- No implementation or test pass is claimed by this checklist.

### Step 1a - Serializable lifecycle state (2026-10-28)

Implemented `ArachnidOutbreakState`; this is the controller's state model, not yet a world encounter or autonomous controller. Production has no new startup hook or activation path.

- Default disabled; enabled state searches for a reviewed site before starting.
- Unique attempt identity; three independently cleared broods; boss access only after all three.
- Undiscovered attempts do not expire. Attributed activity starts/refreshes the recovery clock.
- Duplicate stage completions neither repay nor refresh the clock. Rewards are not implemented yet.
- Completion and thirty-minute abandonment enter a thirty-to-sixty-minute cooldown.
- Positional version-0 state serialization, including identity, site, cleared mask, activity and cooldown dates.
- Production tests: 214 assertions through real state methods and `BinaryFileWriter`/`BinaryFileReader`, all six brood orders, exact boundaries, restart round trips, and a trailing sentinel.

Verification snapshot: `D:/ConficturaUOTestTools/outbreak-20261028-state-001`.
Command: `powershell -NoProfile -File scripts/Test-Foundation.ps1 -Snapshot D:\ConficturaUOTestTools\outbreak-20261028-state-001 -Fixtures outbreak -Prepare`.

Release/x86 server build, forced runtime script compile (`-compileonly -nocache`) and outbreak component fixture passed. All snapshot runtime/core input hashes were rechecked against the worktree, and the fixture copy matches exactly. No saves, initialization, timer thread, listener or game client was started by that fixture.

Scripts project hygiene passed using the already-built x86 server assembly and `BuildProjectReferences=false`, with isolated output/intermediate directories under the snapshot. The initial direct project attempt incorrectly propagated AnyCPU to the x86-only server dependency and failed with CS0227; its log is retained as `ide-project-build.log`. Corrected build: `ide-project-build-attempt-2.log`. This IDE build is separate from runtime-compile proof.

Project truth: zero missing includes and zero unlisted runtime scripts after adding the new source and its include. Full report: `D:/ConficturaUOTestTools/outbreak-20261028-project-truth.json`.

Next: owned world controller, reviewed site survey/catalog, guarded staff controls, and reconciliation/cleanup. Batch 1 remains InProgress. Batches 2 and 3 remain pending, including save-world reload and native client acceptance. No matching tracked core executable replacement is needed for this script-only step.

### Placement correction requested by user - 2026-10-28

All six original terrain candidates were in regions that allow player housing. At each exact center a standard small-house test returned BadStatic, which does not establish a permanent prohibition across the encounter area. The user chose: **Use only permanently non-housing locations, even if that limits suitable outbreak areas.**

Retired candidates (x,y): (914,716), (2258,588), (3794,844), (1106,1768), (2514,2152), (3858,2600). They were never client-reviewed, activated, or deployed.

The specification now requires an existing unconditional housing prohibition across the whole footprint and buffer; the candidate catalog is empty pending replacement review. No housing rules were changed. Source: `Region.AllowHousing`, `HousePlacement.Check`, `OutDoorBadRegion.AllowHousing`, and `NoHousingRegion`.

Evidence: `D:/ConficturaUOTestTools/outbreak-20261028-placement-003/outbreak-sites-run-attempt-2.log` loads all 186 checked-in Sosaria region definitions, reports their XML hash, and runs actual small-house placement checks without creating houses, loading saves or opening listeners.

The provisional footprint and buffer span 161x161 tiles. Explicit unconditional outdoor no-housing regions are substantially smaller; a layout follow-up is pending (compact objectives, linked smaller sites, or broader investigation). Do not silently shrink the layout or promote an unverified site to bypass this decision.

### Overworld survey requested by user - 2026-10-28

The user clarified that the locations must support a future family of overworld champion events, and directed investigation of the actual map and recent copied saves. No compact-layout option was selected. Continue the larger placement investigation, including immutable map-based building restrictions; do not treat the small named no-housing regions as the entire search space.

- Reused Pandora's installed `Ultima.dll` renderer; source copies are under `tools/champion-outbreak/`.
- Caught and corrected a legacy SDK mismatch: Trammel's reader defaults to file index 0. Remap map0/statics0/staidx0 reads to Sosaria's map1/statics1/staidx1 in the helper process. This does not alter asset files or installed app settings.
- Verified server and SDK land IDs/elevations agree at four coordinates after remapping. The two earlier mismapped images are rejected observations, not site evidence.
- Rendered `D:/ConficturaUOTestTools/outbreak-map-survey-20261028/sosaria-constraints-corrected.png` with houses, portal candidates and unconditional outdoor no-housing region boundaries.
- Private copied-world export: `D:/ConficturaUOTestTools/outbreak-map-survey-20261028/occupancy-001`. Reused the CCWM exporter source with two explicit bootstrap changes (load the already verified runtime assembly and prepend the verified test asset path) plus a placement-only geometry exporter. No script Initialize, server loop or listeners were started.
- Source save is the recent test world's copied save dated **2026-10-17**. Snapshot preparation hashed all 20 files before/after copying; original and private copied saves were still byte-identical after export. No production saves were modified.
- The filtered mainland overlay contains **133 house-area rectangles** and **390 portal-candidate points**, with no city-stone rectangles found on mainland Sosaria in that save. These are geometry rows, not unique house counts or proof of today's live occupancy. Dynamic checks remain mandatory.
- Existing CCWM committed export is dated May 2026 and was not used as current occupancy evidence. No canonical CCWM files or installed client profiles were changed.

Current placement-004 candidate: server build and forced runtime compile passed; **234** lifecycle/geometry assertions passed; all six retired housing-eligible candidates are rejected specifically by the permanent-housing rule. No replacement location has been approved. Next: map-backed regional/terrain housing analysis and a reusable ranked site atlas, then owned world controller integration.

Subsequent actual placement probe found legal small-house locations **inside all six original encounter footprints**, not merely housing-permissive region flags. This conclusively disqualifies those sites under the user's requirement. Private evidence: `occupancy-001/housing-eligibility-probe.log`; reproducible probe source is `tools/champion-outbreak/HousingEligibilityProbe.cs`. This probe only disqualifies; absence of one small-house placement would not prove permanent non-housing status for all supported shapes.

Placement-004 IDE/project hygiene build passed against the verified x86 server assembly with BuildProjectReferences=false and isolated outputs. Core/runtime source and exact fixture hashes were revalidated before the focused placement checkpoint commit. Batch 1 is still InProgress; the replacement site search and world controller are not complete.


### Step 1b - Disarmed owned-world controller (2026-10-28)

Implemented the separate controller and owned spiders/champion/web signs. This is a gated intermediate implementation, not a playable or deployed outbreak. `ArachnidOutbreakController.DeliveryReady` remains false and the reviewed catalog remains empty. Startup registers a command but never creates a controller.

- Entry point: `ArachnidOutbreakController.Initialize` registers `[ArachnidOutbreak <status|create|start|stop>]` at Administrator access and restores loaded controllers. The handler independently guards null/deleted/low-access mobiles. Creation makes one disabled controller; start rejects incomplete delivery, ambiguous controllers, and absent reviewed sites.
- A five-second controller timer (enabled only after readiness) handles search, recovery, reconciliation and cooldown. Reviewed-site terrain validation and live nest occupancy checks precede placement. There is no random-coordinate fallback.
- Three owned guardians/signs progress through real guarded death callbacks. Deletion is not victory. Owned-object cleanup requires both controller identity and attempt identity, including contaminated saved reference lists.
- Reconciliation preserves cleared stages, detects removed guardians/bosses, and cancels invalid attempts without victory. A failed leash return cancels instead of stranding an internalized creature whose AI may stop thinking.
- Permanent housing protection and structural clearance are rechecked every thirty seconds; new reinforcement groups also check clearance. All four identified temporary travel-spell gate classes exclude fresh placement but do not cancel an active attempt. Actual spawn points still avoid all portals and houses.
- Brood reinforcement masks allow four initial defenders and two at each health threshold; boss masks permit six total. Healing/repeated callbacks do not replay spent budgets. Failed placement consumes that threshold budget, preserving an upper bound rather than an unbounded retry source.
- `BaseChampion.AwardLegacyChampionRewards` defaults true; the new boss opts out of legacy scroll/gold/skull transactions so the new reward ledger can own its complete transaction. Existing champion serializers and reward defaults are unchanged. New rewards and participation are not implemented yet.

New positional serializers (all version 0, after their base serializer):

| Type | Custom field order |
|---|---|
| `ArachnidOutbreakController` | version, versioned state, strong mobile list, strong item list, next search date, last site string, boss-created bool |
| `ArachnidOutbreakSpider` | version, controller item, attempt string, stage int, guardian bool, reinforcement-mask int |
| `ArachnidOutbreakChampion` | version, controller item, attempt string, reinforcement-mask int |
| `ArachnidBroodSign` | version, controller item, attempt string |

The state serializer's existing version-0 field order is unchanged. All new world types have Serial constructors. Timer, placement cache, safety-check date and status are derived, not serialized.

Source traces: `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs` (`Initialize`, `OnCommand`, `TryEnable`, `TryStart`, `Reconcile`, `Cleanup`, `KeepCreatureInBounds`, `Serialize`/`Deserialize`); `ArachnidOutbreakCreatures.cs` (`OnDamage`, `OnThink`, `OnDeath`); `ArachnidOutbreakSite.cs` (`HasStructuralConflict`, `IsPortal`, `IsTravelSpellGate`); `Data/Scripts/Custom/Champions/System/Mobiles/BaseChampion.cs` (`AwardLegacyChampionRewards`).

Verification and accepted snapshot are recorded below after the final fixture run. Batch 1 remains InProgress: approved placement/layout and actual saved-world reload are still required. Batch 2, turn-based mechanics, native client feedback and pacing acceptance remain pending.


Accepted controller snapshot: `D:/ConficturaUOTestTools/outbreak-20261028-controller-007`.
Command: `python scripts/foundation_fixture_runner.py --snapshot D:\ConficturaUOTestTools\outbreak-20261028-controller-007 --fixtures outbreak outbreak-controller champion --prepare`.

Release/x86 server build and forced runtime script compile passed. Components passed **234** lifecycle/serialization assertions, **56** controller assertions and **679** existing champion reward assertions. Exact production runtime/core input hashes and the copied controller fixture were rechecked against the worktree. Project truth is zero missing includes and zero unlisted runtime sources. Scripts.csproj IDE/project hygiene is verified separately with the matching x86 server reference and isolated output paths.

The fixtures test real death callbacks, duplicate outcomes, reference-list ownership contamination, binary serialization/sentinels, finite reinforcement replay, command access and disabled activation, four temporary travel gate families, and failed leash recovery. They use private reflection setup for terrain/catalog prerequisites and deliberately do not claim real placement approval. No saves, listeners, main timer thread or native game client are started by these component fixtures. Full saved-world reload, native behavior and pacing remain unavailable as acceptance evidence at this checkpoint.

Failed observations are retained: controller-001 initially failed runtime compile for a missing `Server.Items` import; a controller-004 reinforcement test initially used health above the threshold it intended to test, then corrected its input. Later accepted snapshots passed. No core source was changed and the tracked root executable is untouched.

### Map-backed placement design review - 2026-10-28

See [SITE_SURVEY.md](SITE_SURVEY.md) for the measured protected-landmark atlas and search limits. The broader eight-tile scan examined 250,240 sample positions and 1,094 candidate centers; all candidates failed the rigid current layout. This does not prove there are no sites at unsampled centers or with different geometry.

Central Orc Fort provides 4,041 connected tiles within an existing 74x64 permanent housing ban, plus no copied houses/portals within 24 tiles in the October 17 save. It is a concrete lead, not an approved deployment site: the current footprint/buffer does not fit. Smaller protected landmarks have differing terrain, portal and adjacent-house constraints. The user is reviewing whether to fit encounter layouts to existing protected geography or reconsider larger reserved wilderness areas. No new ban, footprint relaxation, activation or production catalog entry was applied.


### Step 1c - Actual fresh-process saved worlds (2026-10-28)

`tests/ArachnidOutbreakWorldReload.cs`, registered as `outbreak-world`, uses the real `World.Save(false,false)` and `World.Load` in separate bounded workers. Each case starts in a new GUID-named directory beneath the fixture snapshot. No existing save is copied or loaded. All output, synthetic saves and expected metadata remain in that private directory; no listener, script-wide initialization or main timer thread starts.

Accepted run: `outbreak-20261028-controller-007/outbreak-world-run-attempt-2.log`: **13 worlds / 26 fresh workers passed**. Cases cover disabled, searching, undiscovered brood activity, engaged activity before a clear, all six partial cleared masks, boss pending creation, boss created, and cooldown. Assertions check typed reconstruction, strong reference lists, controller back-references, stage/role and reinforcement masks, attempt/site identity and exact activity/cooldown timestamps. Calling the actual controller Initialize then verifies the disabled delivery gate cleans only owned objects, preserving a foreign item and unrelated creature.

This proves saved-world reconstruction and gated startup cleanup. It does not prove active encounter recovery at a real approved site, client behavior or turn-based behavior; those remain delivery gates. The original eleven-case run is retained; the accepted expansion also covers engagement and the interval before boss creation. The fixture preserves worker output on failure/timeout and stops only its own child process.

### Reservation direction selected by user (2026-10-28)

The user selected **Consider new permanent wilderness reservations for larger encounter layouts**. Develop a mapped, source-validated proposal for the larger layout rather than silently compacting it into the fort. Permanent housing protection must outlive individual encounter starts/stops. No new reservation or housing restriction has been activated by this decision record. Candidate evaluation must include full terrain/staging, towns and protected services, copied houses/cities/portals/vendors, approaches and native client review; today's loaded world still needs a preflight before production activation.


### Step 1d - Gated permanent reservation policy and native map reconnaissance (2026-10-28)

Canonical proposal, source traces, accepted tests, existing-save finding and native limits: [RESERVATIONS.md](RESERVATIONS.md). Six full-size alternate reservations are defined but all review flags remain false; no new housing ban or approved runtime site is active.

Implemented center/full-multi housing exclusion, unchanged staff bypass, future-maximum city territory exclusion at founding, and bounded post-property XML legacy champion placement exclusion. Fixed legacy champion rectangle translation without changing version-5 serialization. The relocation test exposed the previous negative-dimension defect; a read-only matching-serializer copy also found one already malformed inactive legacy controller, retained unchanged for staff review.

Accepted snapshot `D:/ConficturaUOTestTools/outbreak-20261028-reservations-004`: server build, forced runtime compile, IDE/project hygiene and source/project truth passed. Fixtures: lifecycle234, controller59, reservations73, existing champion679 and existing actions261. Production core/runtime input hashes match exactly. The initial reservation-003 run exposed the real rectangle bug; reservation-004 then exposed an incorrect test property name, corrected from SpawnRange to actual SpawnRadius. All logs remain private and retained.

Computer Use inspected all six boss-point viewports in an independent copied native shard/client; scripted relocation and a blessed ordinary Player were used. This is visual reconnaissance only: no native translation or brood-anchor traversal is credited. All owned processes are stopped; forced exits are explicitly recorded. All captured source-save/settings/profile hashes are unchanged. A separate matching-serializer full copied-world preflight found all six hypothetical layouts clear of house areas (all elevations), existing cities' maximum future extent and valid legacy areas; save bytes remained unchanged. Details and limitations are in RESERVATIONS.md.

Batch1 stays InProgress for native traversal/anchors, western spawn visibility, approved catalog promotion and active-site restart acceptance. Batch2 contribution/rewards/crafting and Batch3 mechanics/benchmark remain pending. Continue independent reward work while the remaining placement acceptance is resolved; do not turn the reservation proposal into a new stopping point.


### Step 2a - Stage accounting and persistent gland claims (2026-10-28)

Implemented the first Batch2 slice; Batch2 remains InProgress. `ArachnidOutbreakLedger` tracks each stage separately, caps qualification at ten points and seals completed stages. The defined initial health budgets are 4,320 per brood (3,000 + ten maximum-132-health defenders) and 9,792 for Mephitis (9,000 + six defenders). Budgets include finite reinforcements even when a fast clear prevents them from appearing. Creature health constants and budget calculation share their source. Damage uses the existing owned-creature OnDamage callback, clamped to the victim's pre-hit health, with current pet/provocation attribution. Post-resolution health attribution and support provenance remain a subsequent gate; the validated-support/healing accounting methods do not themselves observe or validate spells.

Guardian/boss death queues one/three glands per qualified character, then seals and advances lifecycle. `ArachnidOutbreakClaim` is an independent hidden internal Bag with owner/attempt/stage and the actual earned item as its child. Stop/deletion of the encounter does not delete claims. `[OutbreakClaim` is a Player command with caller/owner/alive/backpack guards; a full backpack retains the original item. Successful collection transfers or merges it and deletes the empty claim; claim attempts never regenerate loot. Lifecycle and sealed-stage checks prevent a consumed claim being recreated by duplicate death callbacks. Pending keys are idempotent; callers must not use QueueGlands to replay a paid stage outside lifecycle.

Serialization: controller **version1** preserves every version0 field in its existing order, then appends ledger-present bool and contained ledger. Version0 omits these and defaults to no ledger; the unchanged delivery gate safely disarms old encounters. Ledger v0 writes attempt then four ordered budget/sealed/count/(Mobile,double points) blocks. Mobile eligibility is checked after loading, because Item loading can precede Mobile fields. Claim v0 adds Mobile owner/string attempt/int stage after Bag serialization; gland v0 adds only version after Item serialization. No old serialized type/namespace was renamed.

Accepted snapshot `D:/ConficturaUOTestTools/outbreak-20261028-rewards-002`: operational server build, forced runtime compile, Scripts.csproj IDE hygiene, exact core/runtime input hashes and project/source truth pass. Rewards30, controller59, lifecycle234, existing champion679 and actions261 assertions pass. Attribution tests use actual Damage for current pet owners, ownership transfer, correct provocation target and unattributed wild combat. Delivery tests use actual guardian death, duplicate callbacks before/after collection, dead/offline and full-pack claims, encounter deletion and merging a three-gland boss award.

Fresh-process World.Save/Load fixture now passes **14 worlds / 29 bounded workers**. It checks partial damage/support values, sealed qualification and pending item identity across actual item-before-mobile loading, disarm, dead/offline/full-pack collection, then another save/reload after payment to prove claimed items stay paid. Four real synthetic version0 saved worlds from the prior accepted snapshot (disabled, partial broods, boss, cooldown) load successfully with the new controller. No existing player saves, listeners or native gameplay are used by those fixtures.

Still required: health/wound provenance hooks, relevant cure/prevention/rescue/bard support, collectible recipients, 15,000 shared gold, antivenom recipe/effect, reward native feedback and active-site acceptance. Production activation and all reservation review flags remain false. Matching tracked core executable is not needed for this script-only slice; no core source or tracked executable changed.


### Step 2b - Resolved health observation interface (2026-10-28)

Core `Mobile.SetAttributedHits` scopes only the authoritative Damage/Heal health assignment. The existing Hits setter still performs deleted/TBC/clamping checks first and emits `EventSink.HealthChanged` only after a committed value change. Readonly `HealthChangedEventArgs` supplies Mobile, Source, OldHits, NewHits and kind (Damage/Healing/Unattributed). Direct writes and regeneration are unattributed. Health callbacks run without inheriting the operation source, including same-target writes; nested explicit operations establish their own context, unwound in finally. Observer failures are logged separately and cannot interrupt death or later observers. EventSink.Reset clears the subscription. There is no player command, cancellation permission or new saved field in this interface.

Entry/source traces: `Data/System/Source/Mobile.cs`: private HealthChangeContext/SetAttributedHits, public Damage/Heal/Hits; `Data/System/Source/EventSink.cs`: HealthChangedEventArgs, HealthChanged, InvokeHealthChanged, Reset. Existing action authorization precedes attribution and mutation. Callers are existing game healing/damage or permitted direct setters; server scripts may subscribe. No outbreak subscriber or wound provenance adapter is added in this commit, so this hook alone awards no support contribution.

Accepted exact snapshot `D:/ConficturaUOTestTools/outbreak-20261028-health-002`: Release/x86 operational build, forced runtime compile, health16, rewards30, controller59, existing actions261, bandages110, champion679 and actual saved-world14/29 all pass. Health fixtures exercise clamped overhealing, canceled callbacks, turn-denied healing, exception unwinding, direct same/other-target callback writes, nested healing/damage, invulnerability, overkill, observer failure and reset. The initial health-001 snapshot passed its original14 cases; review narrowed context to the authoritative assignment and added the two callback isolation cases before accepting health-002.

All124 core inputs and all captured runtime inputs match the accepted build. Matching tracked `ConficturaServer.exe` SHA-256: `2e85738177189538a325221c0fbb5886ab49d7b65c46b7555855b6a998b16464`. Process-identity checks before replacement confirmed no running process used the repository executable. Accepted artifact is the uninstrumented operational build; tests and private overlays are not included. No serialization methods, field order, class identity or runtime project include changed. Build logs, hashes and artifact acceptance record remain in the private snapshot. No native gameplay or production deployment is claimed. All outbreak/reservation activation stays disabled.


### Step 2c - Effective encounter healing and injury provenance (2026-10-28)

Wired `ArachnidOutbreakController.OnHealthChanged` from Initialize (remove/add for idempotent registration). Unique active controller, matching ledger attempt, exact owned enemy/stage, resolved health delta and current player/pet/provocation attribution gate damage credit. Removed the old creature OnDamage activity/score bridge; only committed loss now counts. Incoming enemy attacks alone do not refresh engagement. Effective player healing of recorded encounter injury does, including after a player's score reaches qualification.

`ArachnidOutbreakWounds` stores oldest-first stage/amount injury segments per patient. Preexisting/unrelated/self-inflicted injury is unknown; every actual restoration consumes the balance, including self-healing, direct restoration and regeneration. Only the part restored by an identified other player (or attributed helper) from an unsealed encounter stage scores. Players and participating controlled/summoned pets qualify as patients. Player-controlled/summoned/provoked event enemies cannot create eligible wounds. Released patients are forgotten on their next health change; their old wounds cannot be exchanged for later unrelated injury. Maximum-health changes reconcile conservatively without credit. Completed stages cannot acquire late eligibility. Cleanup cancels unearned injury balances while retaining earned stage scores/independent claims.

Review found that exactly-zero health can still be alive in this engine. The initial zero-health shortcut was removed. New core `EventSink.MobileDeath` fires from Mobile.Kill **after both death vetoes accept, before inventory/corpse/OnDeath/resurrection callbacks**. This is a death commitment notification, not a guarantee that Body is already a ghost. It covers bonded pets that skip base.OnDeath; vetoed deaths emit nothing. Observer errors are isolated and Reset removes the subscription. `OnMobileDeath` forgets only injury debt, preserving already earned contribution and pending rewards. Exact-zero survivors retain real healing eligibility; actual deaths and immediate resurrection callbacks cannot recycle previous wounds.

Serialization: controller **version2** preserves its v0 prefix and v1 optional ledger exactly, then appends wound data. Wounds v0 writes count, then (Mobile,segment count,(int stage,int amount)*) in matching order. Older controllers default to no wound data. Patient references survive item-before-mobile loading without premature Player checks. Active attempts with missing/mismatched ledgers disarm safely when delivery is eventually opened. No existing serialized type/namespace or core serializer changed. Scripts now require the matching health/death core APIs in the tracked executable.

Accepted snapshot `D:/ConficturaUOTestTools/outbreak-20261028-wounds-004`: operational Release/x86 build, forced runtime compile, IDE/project hygiene, exact core/runtime input hashes and project truth all pass. Health18, rewards53 (accepted run-attempt-2), controller59, lifecycle234, actions261, bandages110 and champion679 assertions pass. The real saved-world fixture passes14 worlds/29 bounded workers; the engaged case now records an actual incoming hit, saves, reloads in another process and verifies actual healing consumes its saved injury exactly once. Eight actual older synthetic worlds (four v0, four v1) load with controller v2. Native client/support-role acceptance remains open.

Retained iterations: wounds-001/002 passed their earlier cases. Review then replaced the zero-health shortcut and moved the new death notification into Kill, since bonded pets skip base.OnDeath. Wounds-003 stopped on an incomplete fixture bootstrap: fake players needed RaceDefinitions.Configure for actual death. The production death path was not altered to mask that fixture error. Wounds-004 adds race setup and passes actual player death, exact-zero survival, bonded-pet death/resurrection, released-pet reuse and immediate resurrection inside PlayerDeath callback.

All124 core inputs and captured runtime inputs match the accepted build. Matching uninstrumented tracked executable SHA-256: `afa208cfdaa769b08de479249a1cdc35ba9490b618332d737536e754f2060da4`; process-identity check passed before replacement. Core source, matching executable and this script slice are one focused commit. No production launch, save write or site approval occurred; activation and all reservation review flags remain false. Still open: cure/prevention/rescue/bard credit, collectibles/shared gold, recipe/treatment effect and Batch3 native/pacing gates.


### Step 2d - Shared boss gold and collectible claims (2026-10-28)

`ArachnidOutbreakController.TryBossRewards` prevalidates25 loot points from the existing connected terrain cache, first within12 tiles of the boss anchor, then within56. `ArachnidOutbreakPlacement.TryFindLoot` ignores occupying mobiles/corpses while preserving terrain, house and portal checks. No safe cached ground cancels the boss outcome into cooldown without a coordinate fallback or new boss awards. Valid victory drops25 ordinary Gold stacks of600: exactly15,000 shared ground gold. Existing corpse loot remains separate.

Qualified boss participants receive three glands each. Exactly one Venom skull is queued to a uniformly chosen qualified character; the existing `BaseChampion.GetArtifact` opportunity is invoked once through `ArachnidOutbreakChampion.RollVictoryArtifact` and, if it returns an item, queued to an independently chosen qualified character. Existing probabilities/pools stay5% UniqueList,10% SharedList,15% DecorativeList,70% none. Support-only, dead and offline qualifiers have the same recipient access. With no qualifying character there is no collectible recipient and no artifact roll. Existing Mephitis GenerateLoot (four UltraRich packs, including its ordinary gold/random-artifact rolls) is retained, not replaced or represented as the single champion collectible opportunity. New champion scroll/goodies/skull paths remain opted out; legacy champions are unchanged.

Claims now distinguish Glands/Skull/Artifact. Claim **version1** appends int kind after the complete version0 owner/attempt/stage prefix; older pending claims default to Glands. A deferred item factory is called only when no matching pending key exists. All reward kinds retain the actual earned item and use the same owner/alive/backpack transfer guards. Controller/creature serialization stays unchanged. Completed-stage and lifecycle guards prevent a repeated death callback from repeating gold, rolls or claims.

Accepted snapshot `D:/ConficturaUOTestTools/outbreak-20261028-bossloot-001`: operational build, forced runtime compile, exact core/runtime input hashes, rewards90 (run-attempt-2), controller59 and existing champion679 pass. Boss tests cover mixed damage/support, guaranteed support-only recipients, a successful forced named artifact, an empty artifact outcome, exact shared gold, no new power scrolls, duplicate death callbacks and portal-blocked loot cancellation. The forced factory lives only in the component test subclass; production delegates to the unchanged GetArtifact method.

Actual world fixture14/29 (run-attempt-2) now saves gland/skull/named-artifact claim kinds and exact payload identities, loads and pays them, then reloads the paid world to verify no duplication. A separate three-worker upgrade uses the old rewards-002 executable to save actual pending version0 claims, then the current executable to load/pay/reload them; the default Glands kind and exactly-once transfer pass. Private upgrade folder: `bossloot-001/v0-claim-upgrade-ea526b9a96b145689c741d716b4b2652`. Core source and tracked executable are unchanged from c445a3ce (accepted SHA-256 afa208cfdaa769b08de479249a1cdc35ba9490b618332d737536e754f2060da4). This is script-only; no native or production payout is claimed.

Design question raised during review: a fixed ten-point qualification threshold is not crowd-normalized. With twenty evenly contributing damage players and no extra regenerated health, each might earn only about five points and nobody qualify. At this checkpoint an optional user question was pending; the rule remained unchanged. **Resolved subsequently by explicit user decision2A; see Step3h.** This historical checkpoint is not current authorization to retain the fixed rule.

Still open: recipe/treatment effect, cure/prevention/rescue/bard credit and Batch3 combat/client/pacing acceptance. All encounter/reservation activation flags remain false.


### Step 2e - Authorized poison prevention and resolved poison/cure interfaces (2026-10-28)

Core `Mobile.ApplyPoison` preserves its higher-poison and existing-immunity checks first, then requires a Poison mutation scope before invoking `EventSink.PoisonPrevention`. A prevention can only opt in once; later handlers cannot consume another protection. A prevented application invokes normal immunity feedback and does not replace/cure existing poison. `CurePoison` now also obtains a mutation scope. `ApplyPoisonResult.Blocked` is appended (old enum values unchanged); denied operations and a denied null-poison cure report Blocked instead of claiming success. The two infectious-strike callers explicitly exclude Blocked when issuing success/karma feedback. Valid non-turn applications/cures retain their paths.

`PoisonChangedEventArgs` reports Mobile, Source, OldPoison, NewPoison and Application/Cure/Unattributed kind after the authorized setter completes. Target-specific thread-local context wraps only the authoritative assignment and is removed around observer callbacks. Direct assignments, including same-poison timer replacements, are unattributed and can invalidate stale provenance. Existing immunity precedes the new prevention hook. Callback errors are isolated; Reset clears both subscriptions. Entry points are server ApplyPoison/CurePoison/Poison property calls, not player commands; the existing mutation guards and new scopes precede prevention/change callbacks. No core saved field or serializer changes.

Accepted exact snapshot `D:/ConficturaUOTestTools/outbreak-20261028-poison-002`: operational Release/x86 build, forced runtime compile, poison16, health18, rewards90, actions261, bandages110, champion679 and actual saved-world14/29 pass. Cases include immunity-before-prevention, weaker repeated application, effective/failed/denied cures, direct assignment authorization, stale attribution, denied application without a prevention call, null-poison cure denial and subscription reset. The retained poison-001 run exposed the incorrect null-poison success return; poison-002 fixes it and passes. No treatment handler or cure-contribution adapter exists yet; these hooks alone consume no antivenom or award support.

All124 core inputs and captured runtime inputs match the accepted uninstrumented build. Matching tracked executable SHA-256 `703695f310d6171d97c2f17f855f3ad63167e7e14b100b0dd39ecbb8110f1091`; process check before replacement found no running/unknown server process using the repository executable. Source, matching executable, caller updates and fixture are committed together. No production launch or native poison acceptance is claimed. Production activation remains disabled.


### Step 2f - Brood Antivenom, Alchemy and relevant poison support (2026-10-28)

Implemented `BroodAntivenom` as a tradable stackable preparation with a beneficial target cursor. It validates current backpack ownership, living recipient, same map/range2/LOS, beneficial permission, self/party membership or controlled-pet ownership, and the existing Vampiric Embrace garlic restriction before spending anything. A denied Effect mutation creates no effect and consumes nothing. Success consumes one treatment, returns one bottle and creates three protections; it never cures existing poison. Active effects cannot stack/refresh, and exhausted recipients retain their two-minute reuse delay. Deadly/lethal wild BaseCreature applications only are prevented, after existing immunity/higher-poison/turn checks; players, controlled/summoned/provoked sources and lower poison severities spend no charge.

`BroodAntivenomEffect` is a hidden internal version0 Item, independent of controller cleanup. After base/version it saves Mobile owner, Mobile applicator, int charges, DateTime expiry and DateTime next-use in matching order. Serial constructors register pending objects without reading mobile eligibility before world loading completes. Expiry/reuse use UTC wall time; checks enforce them immediately, with periodic expired-record cleanup. Command `[BroodAntivenom` (Player, independent caller guards) opens a status gump for self/current controlled pets; it shows charges, remaining time and reuse delay, with refresh and guarded response. Application/consumption messages identify protection, and pet owners receive feedback. Full native presentation/action-point/clock-policy acceptance remains Batch3 work; mock mutation-denial checks do not establish that broader result.

`DefAlchemy` adds a known single-recipe80-100 preparation requiring one BroodVenomGland, two OrangePetals, six Garlic, two Bottle. ICraftable output is exactly two and ForceNonExceptional disables potency/quality variance. ConsumeOnFailure exempts only this recipe's gland, delegating other resources to ordinary policy. Actual 100-skill CompleteCraft yields two and consumes exact ingredients; actual locked80-skill CompleteCraft fails, retains the gland, yields nothing and consumes common ingredients under the normal All policy.

Relevant cure tracking is transient (`ArachnidOutbreakPoisons`). Current Mobile serialization deliberately omits active poison (`Mobile.cs` commented Poison.Serialize; older readers only consume historical poison data). Therefore no additional controller save-format change is needed: controller stays version2, earned points persist, and no poison is invented after restart. The private antivenom-001 scratch snapshot briefly exercised an unreleased version3 proposal; its synthetic worlds are transitional test artifacts, not supported source-world inputs. They were never player/production saves. Application provenance records only owned unsealed wild encounter stages. Any direct/unrelated replacement clears it; only actual matching removal by another helper earns two points. Null/repeated/self cures do not. Actual consumed protection credits its identified applicator only for a relevant owned encounter source and another participant/pet. Cleanup/death clears unearned conditions.

The first real PlayerMobile cure test exposed an existing override bypass: PlayerMobile.CurePoison copied the old core implementation, so it lacked attribution and could falsely report a turn-denied cure. It now delegates to base and removes the Poisoned buff only after success. This preserves ordinary cure callbacks while using the authorized core path. Source trace: PlayerMobile.CurePoison; controller Initialize/OnPoisonChanged/RecordPoisonPrevention/OnMobileDeath; ArachnidOutbreakPoisons.Observe; BroodAntivenom.TryApply/OnCraft; BroodAntivenomEffect.Initialize/OnPrevention/OnStatus/Cleanup; DefAlchemy.InitCraftList/ConsumeOnFailure. No existing serialized class or prefix changed.

Accepted final runtime snapshot `D:/ConficturaUOTestTools/outbreak-20261028-antivenom-004`: operational build, forced runtime compile, exact core/runtime hashes, antivenom53 and actual world15/32 pass. The new world case saves a partly consumed effect, reloads without refreshing clocks, consumes another protection on the offline owner, saves again and checks it in a third process. Snapshot antivenom-003 also passed poison16, rewards90, actions261, bandages110 and champion679; its IDE/project-truth build passed with the identical project includes. Final004 differs only in status display/response guards and has its own runtime/feature/world verification. Tracked core is unchanged from f2812c1e, SHA-256703695f310d6171d97c2f17f855f3ad63167e7e14b100b0dd39ecbb8110f1091.

Retained fixture corrections: the party constructor needs the caller to set the leader's Party reference; the test now follows that actual setup. The first failure-resource test incorrectly used the bulk Half branch; this recipe explicitly uses ordinary single-recipe All and is tested through that path and real CompleteCraft. A later test used the read-only Skill.Lock property; corrected to the actual SetLockNoRelay API. None of those fixture repairs weakened production gates.

Separate existing crafting finding, outside this recipe: CraftItem.ConsumeRes first sets protected failure resources to zero, but its UseAllRes/Half branch later clamps every amount to at least one. The private Half test reproduced loss of the gland there. This recipe has UseAllRes=false, so its normal failure path preserves the gland. General protected-resource bulk crafting needs separate review; no global CraftItem repair is claimed here.

Production encounter/reservation activation remains disabled. Batch2/3 still require effective bard and rescue credit, web/turn-based mechanics, reviewed native sites, benchmark/stress runs, native treatment feedback and complete requirement audit. No live deployment, production save write or native treatment acceptance is claimed.

### Step 2g - Effective bard contribution (2026-10-29)

Implemented timed support accounting for actual Discordance, Peacemaking and useful provocation. A character receives at most one point per ten useful actor seconds in each stage, shared across simultaneous effects and affected enemies. Another participant must have recent real involvement in that stage: resolved outgoing damage, encounter damage/poison received, or consumed protection against encounter poison. Participating pets map to their player. The window is fifteen enemy actor seconds. Solo damage retains its damage credit but cannot also manufacture support for the same character; unrelated combat, idle targets, controlled/summoned targets, failed effects and completed stages earn no time. Existing provocation damage attribution remains in place.

Source traces: `ArachnidOutbreakBards.RecordCombat/Observe/HelpsAnother/Add`; controller `ObserveBards/RecordBardCombat/BardEffectBoundary`, Initialize's existing resolved-health/poison handlers, Tick and Cleanup; `Discordance.GetBard/ProcessDiscordance/DiscordanceTarget.OnTarget`; `BaseCreature.Pacify`, `BardPacified/BardProvoked/BardMaster/BardTarget/BardEndTime`. Existing skill validation determines whether an effect actually applies. Before/after effect boundaries flush the old interval and establish the new baseline. Refreshes, replacement casters and early breaks cannot replay elapsed time. Peacemaking now retains a transient caster and clears it when pacification ends; ordinary skill behavior and expiry rules are unchanged.

These are server skill/property/event/timer paths, with no new player or staff command. The controller checks uniqueness, active state, matching attempt/ledger, roster ownership and unsealed stage; effect boundaries skip World.Loading. Null/deleted principals are excluded. Time comes from `TurnBasedCombatBridge.GetTime`; a paused actor contributes no wall-clock time. Caster and target must use the same combat mode, and the real manager's participant/group lookup excludes different or missing battles. Stopped observers and backward/large clock transitions reset their baseline rather than pay retroactively. Existing skill-effect expiry remains the skill system's policy; complete real turn-based action/clock acceptance is still open.

Observation windows and effect attribution are transient, matching the unsaved skill effects. Earned fractional points use the existing saved ledger. No existing serialized type, version or write/read order changed; the controller remains version 2. The fresh-world fixture now checks a 4.25-point support value across item-before-mobile loading. The tracked core executable remains unchanged at SHA-256 `703695f310d6171d97c2f17f855f3ad63167e7e14b100b0dd39ecbb8110f1091`; no matching-artifact replacement is needed for these script changes.

Accepted snapshot: `D:/ConficturaUOTestTools/outbreak-20261029-bards-006`. Operational Release/x86 build, forced recursive runtime compilation, bard59, antivenom53 and actual world15/32 pass. Bard evidence is `outbreak-bards-run-attempt-3.log`: real successful/failed skill targeting and production effect methods, with a controlled bridge clock. The battle-identity cases use the real read-only manager lookup with synthetic participant/group records. Neither is native gameplay, full AP flow or a legal veteran benchmark. Setup uses effective skill125 and at most20 real rolls to establish a successful effect, with the test clock held fixed; it does not measure skill-use cooldown or AP. World evidence is `outbreak-world-run.log`, with 15 synthetic worlds and 32 separate bounded workers. No existing saves/listeners are used. Snapshot003 additionally passed rewards90, actions261, bandages110 and legacy champion679. Final006 adds attribution clearing and stricter clock/group/domain guards, covered by its own feature/world checks.

Project truth reports zero missing/unlisted sources and zero duplicate includes; the new runtime file is explicitly included. Scripts.csproj Release/AnyCPU hygiene uses the isolated built x86 core as its reference and is separate from runtime proof. All captured final runtime/core input hashes are rechecked against the worktree. Retained tooling failures: the first002/003 invocations named nonexistent fixture `champions`; rerunning003 with `champion` passed. The first004 IDE build lacked its private core reference (CS0006); copying the already verified private executable into the isolated reference directory repaired that preparation error. No production code or gate was weakened for those repairs.

Repeated testing caught another fixture assumption: requesting skill1000 still produces effective skill125, so a real Peacemaking roll can fail. The first006 run and exploratory repeat7 retained that legitimate failure. The setup now explicitly uses125 and bounded actual attempts rather than assuming guaranteed success; zero-Musicianship failures remain single actual attempts. No production skill chance was changed. Thirty final bounded workers pass; evidence is `bard-bounded-repetitions.json` and its per-worker logs. `final-input-verification.json` records the exact final input/fixture match and unchanged tracked core.

Production and all reservation reviews remain disabled. Effective bard accounting now has component/reload evidence. Rescue credit, web mechanics, full turn-based/native acceptance, six reviewed sites, fixed veteran and Zanthus runs, bounded pacing work and the complete delivery audit remain required.

### Step 3a - Web warning, restraint, escape and rescue (2026-10-29)

Implemented a single-target ground web for the outbreak Mephitis, default interval15 boss actor seconds. The current hostile player or participating pet receives a visible red web and warning message at their original tile. Leaving the tile avoids impact. After at least three real seconds, an authorized impact replaces it with a binding web; it blocks only the movement event, leaving turning, attacks, healing and casting available. Artwork0x10D5 has no Impassable flag. Double-clicking the binding web frees its target within range2/LOS and existing beneficial permissions. A successful rescue of another participant/pet earns two boss-stage points once; self escape earns none. Expired/deleted/relocated/dead targets and repeated rescue attempts do not pay. An active restraint cannot be stacked or extended.

Source traces: `ArachnidOutbreakChampion.ThinkWeb/TryStartWeb/PerformWebAction`; controller `IsWebContext/RegisterWeb/RecordWebRescue/Cleanup`; `ArachnidOutbreakWebItem.Valid/SameMode`, `ArachnidWebWarning.TryResolve/OnTimer`, `ArachnidWebRestraint.Initialize/OnMovement/OnDeath/Find/TryFree/OnDoubleClick`. These are boss AI, item-use, movement, death and bounded timer paths, with no new command. Guards include controller uniqueness, active Boss phase, matching attempt/ledger, owned roster, unsealed stage, victim identity, visibility, map, footprint, range, LOS, harmful/beneficial permissions and player/pet eligibility. Staff targets/rescuers are excluded. Controlled/summoned/provoked bosses cannot create reward-bearing webs; paralysis, freeze and pacification prevent web attacks. Hostile actions use normal reveal/aggression bookkeeping.

In turn-based combat, telegraph and impact each reserve and commit four AP through the existing ItemUse allowance and an authorized Effect scope. An existing pending boss action cannot obtain a free nested web. The target must start and finish a reaction turn, then a later legal boss pulse may impact; the wall timer cannot perform that impact. Standard item use charges four AP for escape/rescue. The web explicitly classifies the trapped mobile as the beneficial-action target before release; an item-only lease cannot bypass a required battle join/merge. State is rechecked after authorization, so an invalidated old clock earns no rescue points. Mode, participant or group changes cancel pending/active webs; a change to the boss's clock resets its attack cooldown rather than import another clock.

The existing five-second actor interval would otherwise expire a four-second restraint before its victim's first action. `TurnParticipant.ActionTime/ActionSecondsThisTurn/TurnsStarted` and manager `GetActionTime/AdvanceActionTime/CompleteAction/EndTurn/StartNextActor/AddParticipant` provide a separate transient committed-action clock. Paid AP advances0.25seconds each; cancelled/refunded actions do not advance it. Ending the turn advances its unspent interval. A web can therefore expire after sixteen paid AP, leaving four AP for movement in the default twenty-AP turn. Existing logical time, regeneration, poison and other status-clock policies are unchanged. These runtime participant fields are not serialized.

New web items have a version0 common prefix (controller Item, champion Mobile, attempt String, victim Mobile) and a version0 derived suffix. Their transient timers/clock references are deliberately unsaved; Deserialize schedules marker deletion and never rebinds a loading mobile or resumes a hostile callback. Existing controller/boss serializers remain byte-order/version compatible. The new world case saves both web types and checks their typed references, absent movement binding, and owned cleanup after fresh-process loading. Controller delivery gating remains disabled, and foreign saved sentinels survive.

Accepted final snapshot `D:/ConficturaUOTestTools/outbreak-20261029-webs-008`: operational Release/x86 build and forced recursive runtime script compilation; web92, antivenom53 and world16/34 pass. Snapshot005 also passed rewards90, actions261, bandages110, bard59 and legacy champion679; final008 adds the classified beneficiary check and post-authorization revalidation covered by its new cross-battle case. Thirty additional final web workers use the same fixture executable and frozen private copies of the checked-in turn catalogs (`web-repetitions.json`). The web fixture uses real manager construction, action leases, native weapon handling, Mobile.Use and turn transitions with a synthetic roster; deadlines are instrumented for fast repetition. It is not client presentation, natural three-second visual timing, native AI pacing or a legal veteran benchmark. An actual Close Wounds cast begins while held; actual damage/healing and ordinary movement paths also run. The expanded world fixture passes16 worlds/34 bounded workers, including new serialized web objects. Project truth has zero missing/unlisted/duplicate sources; Scripts.csproj Release/AnyCPU hygiene passes separately from runtime proof. The tracked core executable is unchanged; no core build-artifact replacement is required.

Retained repair evidence: initial web001 used an unsupported one-argument Timer.DelayCall; corrected to the local TimeSpan/callback API. The first movement test omitted MovementImpl.Configure, and an attack test omitted Fists.Initialize and expected a lease from BeginAttack even though the real manager handles that native attack internally. The expanded healing test also needed missing health established after normal start-turn regeneration. The cross-battle case exposed a real item-use boundary: a generic item lease already opens a mutation scope, so relying solely on BeginMutation did not classify the affected mobile or merge groups. The explicit beneficial-action request now does so and rejects the invalidated restraint after a merge; same-battle item use still costs four AP once. Those repairs did not weaken production guards or change character abilities. Initial and repaired logs remain in snapshots001-008.

Separate casting finding, outside this feature: a synthetic PlayerMobile with Int100, Magery.Base0 and Elementalism.Base0 has effective values15/15 from `Data/System/Source/Skills.cs:Skill.TotalSkillValue`; `Data/Scripts/Magic/Base/Spell.cs:Spell.CantMixSpell` rejects Magery for that profile using effective values. That caused the original Heal cast check to fail with neither freeze nor paralysis set. Close Wounds with ordinary mana/tithing requirements verifies casting independently of that mixing gate. No global spell-mixing fix or claim about every live character is made; a real caster profile still needs validation when freezing the veteran benchmark.

Production and reservation activation remain disabled. Server web/rescue behavior now has component, real-manager-path and fresh-world evidence. Targeted native warning/escape/treatment acceptance, reviewed traversal/sites, fixed legal veteran and private Zanthus fixtures, three pacing runs, bounded tuning, economic observations and the full requirement audit remain open.

### Step 3b - Bounded native warning and treatment checkpoint (2026-10-29)

[NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md) records the exact private copies, accepted cases, retained failures and evidence boundaries. Native keyboard input now demonstrates actual movement, warning avoidance, blocked movement while restrained and resumed movement after natural release. Native item use demonstrates an antivenom target cursor, self-application, one consumed treatment/returned bottle and active-treatment rejection. The rig suppresses boss AI and constructs the scene; no full combat, traversal or pacing acceptance is claimed.

Native double-click escape/rescue and status display remain open. Capture/action latency exceeded one real four-second restraint window; no stale-object click was counted. Command-entry focus was not established for the status command, and no chat message was sent. The initial private dose setup used AddItem without container placement; the accepted fresh copy uses DropItem. Fixture preparation errors and uncertain coordinate attempts remain recorded rather than being presented as production failures.

The abstract web base's Serial constructor is now public to satisfy the runtime verifier's public reflection lookup. Version0 and all serialized field order/type remain unchanged. Accepted follow-up snapshot `D:/ConficturaUOTestTools/outbreak-20261029-native-checkpoint-001`: operational Release/x86 build, forced runtime compile without the web serialization warning, web92, antivenom53 and world16/34 pass. Native observations used the prior web008 candidate; this constructor access correction changes neither web timing nor treatment behavior. Core source and tracked executable are unchanged.

Both owned client/server pairs were forcibly stopped after process identity checks; no graceful shutdown is claimed. Original saves/settings/profiles and private saves remain byte-identical; private loopback listeners and processes are gone. All remaining native, site, fixture, pacing/economic and completion-audit gates stay open. Production and reservation reviews remain disabled.

### Step 3c - Native escape/rescue, status/claims and wall input throttle (2026-10-29)

Native005 now demonstrates self-escape for four AP/zero points, controlled-pet rescue for four AP/two points, paid self/pet treatment, the production status gump and refresh response, and collection/visibility of a scripted pending gland entitlement. The fixture uses real manager enrollment and native packets with a stopped scheduler and directly constructed restraints. These are item/target/AP and presentation results, not a full natural AI/warning sequence or benchmark. Full details and retained004 failures are in [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md).

Native testing exposed a paused-clock lock in the0x06 XMLSpawner use override and core fallback. The half-second packet/lift `NextActionTime` throttle now uses wall time consistently, and combat teardown no longer rebases it. Skill/spell/combat cooldowns retain actor time, and existing AP/legality/staff behavior stays intact. The compatibility scanner and canonical documentation distinguish this input delay from participant cooldowns; synthetic probes still reject skill/spell/combat clock mixing. No serialized layout changed.

Accepted snapshot `D:/ConficturaUOTestTools/outbreak-20261029-input-throttle-001`: Release/x86 build, forced recursive runtime compile, packet80, web92, antivenom53, actions261, bandages110, champion679 and actual world16/34 pass. Baseline packet failure and fixture-bootstrap failure remain in the preceding native-checkpoint snapshot. All6,742 runtime/core inputs and124 core inputs match the accepted build. Matching tracked executable SHA-256: `dbb7b2360c435834b32e0f1f4b0854a859cd9086971a4c270981b58d8f2f3ddb`; host process checks found no running server before replacement.

The reusable guarded fixture and parameterized preparation/atomic-request tools are now repository artifacts; see [NATIVE_TESTING.md](../../tools/champion-outbreak/NATIVE_TESTING.md). Native004 crashed on a stop-request file sharing race;005 has the explicit forced-stop marker and successful input preservation. Final fixture I/O hardening is compile-checked in007, with atomic publication/pending-overwrite rejection checked separately. No new live collision test or graceful shutdown is claimed.

Production and reservation flags remain disabled. Live-scheduler/human-ally/support-role acceptance, reviewed traversal/sites, legal veteran/Zanthus fixtures, all pacing/economic gates and the complete requirement audit remain open.

### Step 3d - Reproducible copied-client site traversal (2026-10-29)

Added a guarded travel fixture outside the production script tree, explicit preparation selection, copied-profile walking macros, an incomplete-site request guard and an exact trace/preservation verifier. Actual `Movement.CheckMovement` plans routes; native keyboard input starts normal client walking. The server only places starting points and observes subsequent positions. No route-following `Move`/`MoveToWorld` callback is used.

Five sites complete every planned step/anchor/boundary: central226, northern224 on retry, eastern228, northwestern228 and western226. Southeastern failed both fixed-macro attempts and remains unresolved. The verifier reports five of six, rather than a passed batch. Native views reveal foliage concerns near northern/western bosses and eastern brood0. See [SITE_TRAVERSAL.md](SITE_TRAVERSAL.md) for the complete boundary between planning, connected-client traces, visual observations and open gates.

Private survey `D:/ConficturaUOTestTools/outbreak-sites-20261029-001`: final forced runtime compile passed; private compatibility scan6,612/Unknown0; two-step native calibration passed; live incomplete-site guard refused publication without moving the scene. The initial macro-format client crash, compile failures and all partial/deviating attempts remain recorded. The macro is a bounded test driver, not a learned/general navigation agent.

Final verification matches the fixture, accepted unchanged core and all6,742 production inputs. Original saves/settings/profiles and all20 private save files remain byte-identical; explicit forced no-save stop and host process/listener teardown are recorded. There was no new core change/build or executable replacement in this test-only batch. No source review flag or housing restriction was enabled. Southeastern diagnosis, anchor/encounter visibility and current-world preflight remain required alongside the other Batch3/completion gates.

### Step 3e - Instrumented southeastern walk and six-route coverage (2026-10-29)

Extended the same guarded fixture with observation of original0x02 handlers/throttle callbacks and next expected tile occupancy. Original handler/callback results, packet reader position, length and in-game restrictions are preserved. The instrumented southeast run completes224 exact walking steps with8 turns and no refusal/deviation. Including calibration,235 packet/throttle observations have zero diagnostic I/O failures. Historical misses did not reproduce, so no movement-rule repair is justified or included.

The verifier now supports explicit site subsets and exact historical fixture Git identity while retaining current production-input checks; subset reports cannot claim the full six-site gate. The combined proof rechecks the five accepted001 routes against fixture commit `fe87553f` and southeastern002 against current fixture source. All six distinct routes/1,356 exact steps, identical starting saves/core, source/save preservation and owned teardown pass. The failed original001 report and both partial southeastern attempts remain unchanged. Reproduction and evidence: [SITE_TRAVERSAL.md](SITE_TRAVERSAL.md).

Final002 runtime compile and private compatibility6,612/Unknown0 pass. Core source/executable are unchanged. Production and reservation flags remain false. Walking access is now covered at every proposal; native encounter/ground-warning visibility, current-world preflight, live-scheduler/ally/support-role acceptance, legal veteran/Zanthus benchmarks, pacing/economics and the full completion audit remain open.

### Step 3f - Legal reference and first normal-AI combat failure (2026-10-29)

Added a separately selected guarded benchmark fixture and copied ClassicAssist client policy. The reference uses 980/1,000 base skills, 250 raw stats and ten real named artifacts validated against `Loot.ArtyTypes`. Actual equipment/durability/effective-stat XML and source/policy hashes were frozen before combat. Forced runtime compilation, world load, loopback listening and native reference inspection passed after retained preflight failures. No combat AI, resource pool or player ability was overridden during the run.

The first no-antivenom attempt died after130.215 seconds with one guardian killed and ten stage defenders still alive. It consumed13 bandages,44 cure potions,5 heal potions and1 petal; no client driver exception occurred. This is not a completed benchmark or pacing acceptance. The sticky guardian-first targeting needs an independently frozen defender-first experiment. Source review also exposed inherited HP-scaled spider breath and ordinary paralysis outside the new boss warning/restraint system; the review proposes an explicit enemy-profile decision before any such change. No tuning pass or production gameplay change occurred.

[BENCHMARK.md](BENCHMARK.md) records exact capabilities, policy, observations and reproduction. [INHERITED_SPIDER_REVIEW.md](INHERITED_SPIDER_REVIEW.md) records the source finding and concrete proposed override. The verifier separates preserved provenance from combat success and can require a real clear. All6,742 production inputs, 20 private save files and original source/settings/profiles match. Host recheck confirms both owned processes and listener63928 are gone after acknowledged cleanup and forced no-save termination. The initial transient teardown observation remains retained. Private compatibility6,612/Unknown0 passes; no core rebuild/executable replacement or site approval occurred. Three full runs, native/cooperative/stress/visibility/economics gates and the final audit remain open.

### Step 3g - Toolkit reassessment, defender-first trial and connected navigation (2026-10-29)

The user requires full-build toolkit review and an OmniAI comparison before balance conclusions. [PALADIN_TOOLKIT_REVIEW.md](PALADIN_TOOLKIT_REVIEW.md) audits all ten Chivalry spells, all five shard-specific Gift Katana moves, poison cure alternatives, consumables, passives, preparation and omitted options. Runtime preflight003 exports all five skill gates true at requirements70/80/90/100/110 and no-context mana costs12/9/15/9/27, plus actual costs for all ten spells. Native execution/effect checks and a final competent policy remain required; neither a source audit nor a legal character alone qualifies the benchmark.

Copy002's defender-first policy clears one brood alive (guardian plus ten defenders), then repeatedly fails native pathfinding to the distant next anchor. The retained final stop at717.355seconds includes that navigation stall and is not a pacing result. Supplies used:20 bandages,24 cure potions,0 heal potions,2 petals. No encounter tuning occurred. The failure record and original frozen inputs remain intact.

The corrected driver follows short waypoints on the connected placement mesh and bounds repeated rejection. Copy003 performs a separately requested blessed navigation-only run, with no controller/encounter: all three inter-anchor routes finish in68.923seconds, zero native walk rejections, no combat actions/damage/supply use. The verifier checks intermediate waypoint observations, endpoint, all6,742 production inputs, 20 private save files, original saves/settings/profiles and host-confirmed forced no-save teardown. `VerifyBenchmark.py <copy003> --require-navigation` passes while combat acceptance remains false. Native inspection observes ordinary client movement and final arrival; no server movement occurs during the run. Fresh forced runtime compilation/world load/listening passed. Evidence roots are `D:/ConficturaUOTestTools/outbreak-benchmark-20261029-002` and `003`.

User decisions1A/2A were received with their transcript context: remove inherited breath only from outbreak spiders, retaining normal mage AI/poison/boss web; adapt qualification to active meaningful contributors. Those scoped production-script changes are the next batch. They do not authorize a broader redesign or activation. All full-toolkit combat, cooperative/stress/visibility/economics and final audit gates remain open.

### Step 3h - Approved outbreak profile and adaptive crowd qualification (2026-10-29)

Implemented decisions1A/2A in two existing production scripts. `ArachnidOutbreakSpider.HasBreath` returns false only for outbreak guardians/defenders; ordinary Dread Spider, mage AI, melee, poison and boss warning/restraint remain. No health, reinforcement, ordinary loot, player ability or global monster change occurs.

`ArachnidOutbreakLedger` counts meaningful stage contributors with at least2 validated points. Its target is `max(2,min(10,50/N))`, or10 with none: half an equal share of the100-point damage budget, capped by the original target and absolute effort floor. Groups up to5 retain10;20 equal contributors can qualify at2.5;25+ reach2. Nearby/party/sub-floor entries do not lower it. Earned dead/offline effort persists. Rates and stored10-point cap remain; stage sealing prevents later dilution. New methods derive from the existing persisted dictionary. Serializer version0, write/read order, types and names are unchanged.

Fresh candidate `D:/ConficturaUOTestTools/outbreak-approved-20261029-002` reuses the accepted unchanged core only after matching every non-script input and tracked executable SHA. It snapshots all6,742 production inputs and verifies exactly the two approved changed scripts. Forced runtime compilation and component suites pass: legacy champion679, lifecycle/serialization234, controller63, rewards99, webs92. Added regressions cover twenty equal damage participants, fifty at the effort floor, sub-floor tag inflation, support-only effort, stage isolation, dead/offline contributors, sealing, unchanged-version round trip/read sentinel, ordinary versus outbreak breath, and retained mage AI/poison/melee/health. The first candidate001 retains its missing web-catalog argument failure; the corrected runner002 passes end to end. Full verbose traces stay in files; console/summary output is bounded.

Reproduction: `python tools/champion-outbreak/VerifyApprovedChanges.py --baseline D:/ConficturaUOTestTools/outbreak-20261029-input-throttle-001 --output <fresh-copy> --assets D:/ConficturaUOTestTools/shard/Data/Files`. Results: `approved-changes-verification.json`, input/fixture hashes and per-process logs/results. No core build or tracked executable replacement is needed; no core source changed.

Copied-world smoke `D:/ConficturaUOTestTools/outbreak-approved-world-20261029-001` then forces compilation, loads copied saves, initializes to idle-ready and verifies its owned loopback listener64859 without a client/encounter. Startup/preparation includes terrain/route work and a retained one-time timer-stall observation; this is not live combat timing. Acknowledged cleanup and forced no-save termination leave no owned process/listener. `evidence/startup-verification.json` confirms all6,742 current production inputs, 20 private save files and protected original saves/settings/profiles match. Private compatibility scans6,612 scripts with Unknown0. Production/source reservation flags remain false.

Historical benchmark/nav reproduction after the approved source changes uses `VerifyBenchmark.py <old-copy> --production-ref 08e1292e`; changed input bytes are checked against that exact commit while remaining inputs still match the frozen manifest. This does not accept old combat as new-profile evidence. Full-toolkit native execution, a frozen revised loadout/policy, three complete reference runs, Zanthus stress, cooperative/visibility/current-world/economics gates and final completion audit remain required. Explicit design revisions are not counted as automatic tuning passes.

### Step 3i - Controlled native paladin action checks (2026-10-29)

Accepted copied toolkit002 passes20/20 controlled native actions: seven consumable actions, eight solo Chivalry actions and all five actual sword moves. Correct weapon selections are observed through normal player commands before attacking; paired immediate damage, multi-target effects, resources and cure/heal/stat changes are retained. The first fixture compiler error and one-second spell-target-wait failure remain in copy001; its12/13 partial result stays partial. Fresh002 corrects only the test fixture/driver, forces runtime compilation and copied-world startup, and succeeds with the longer target wait. No production/core change or tuning pass occurs.

`VerifyToolkitProbes.py --require-complete` verifies the frozen inputs, 20/20 results, all6,742 production inputs, 20 private save files, protected original source/settings/profiles, loopback configuration and acknowledged forced no-save teardown. Both copies' owned processes/listeners are gone. `StopNativeCopy.ps1` requires the private-root guard, no-save acknowledgement, exact owned executable paths and listener ownership before terminating. The ordinary benchmark verifier refuses calibration without a measured final-run artifact; the separate toolkit verifier preserves that boundary.

These tests use stopped targets and between-case setup, so they do not qualify normal-AI policy, active fear movement, isolated DPS or encounter pacing. Source/effect findings and reproduction: [PALADIN_TOOLKIT_REVIEW.md](PALADIN_TOOLKIT_REVIEW.md#controlled-native-toolkit-checks-2026-10-29). A legitimate Sacred Journey destination, cooperative Noble Sacrifice, resource-aware policy/fresh loadout freeze and three full reference runs remain required, alongside the other open Batch3 gates. Production/source reservation flags remain false.

### Step 3j - Native retreat/preparation and first resource-aware pilot

Additional guarded copy `outbreak-retreat-20261029-001` passes actual Sacred Journey to a checked marked wilderness rune,11-waypoint native walking return, Noble Sacrifice on a poisoned/injured controlled dog with the caster's actual resource sacrifice, and unassisted mana0-to10 recovery. Corrected forced runtime compilation/startup pass after a retained test-only XML ordering failure. `VerifyToolkitProbes.py --require-cases journey return noble regeneration` checks these four cases, all6,742 production inputs, 20 private save files, protected originals and acknowledged forced no-save teardown. The original20 controlled cases remain independently verified in toolkit002. Pet rescue does not replace another-player cooperation; peaceful travel does not establish emergency survival under attack.

The fixture and client can now perform native finite preparation before a v3 combat freeze. Stocks, marked rune and return waypoints appear in the actual reference manifest; run mode must match its already-stocked antivenom presence, with no mid-run grants. Updated benchmark verification reads frozen actual supply quantities rather than assuming the old loadout. `FreezeBenchmark.py --prepare --start no-antivenom` requests real potion/petal/bandage use, waits for observed full preparation, freezes the reference/driver/policy and starts combat. Both pilot004 and corrected005 reach this milestone with normal AI and blessing removed.

Pilot004 is retained as a policy-invalid partial run: one brood/19 owned deaths,275.402 seconds, alive at the captured stopped observation, no driver exception/walk refusal, no cure/heal potion use. The policy compared qualified and short Enemy of One type names and could waste mana resetting its selection. Fresh005 freezes a one-line normalized comparison. No encounter/player ability/core change or automatic tuning pass occurs. `CaptureBenchmark.py` preserves an unchanged terminal native observation and can watch for an outcome, then request guarded cleanup; it refuses an unstarted/nonterminal scene and never labels a stop as a clear. Status readers retry transient missing-file/permission gaps during atomic replacement/new-file publication; such an observation gap does not restart a live run.

Pilot005 ends at130.192 seconds from a fixture `File.Replace` sharing violation, with the player alive at317 HP and ten owned enemies dead. Its unchanged terminal failure, source/profile/save preservation and forced teardown pass; no clear/pacing credit is taken. `VerifyStatusSharing.py` reproduces replacement denial from an ordinary Python reader and proves that a Windows share-delete reader permits publication while retaining complete old/new snapshots. Accepted tiny-file proof: `outbreak-status-sharing-20261029-002/verification.json`; initial001's test-shell argument error remains retained. `FreezeBenchmark.open_shared_delete` now transfers a read/share-read-write-delete handle to a Python file object; status observation/capture use this path. No production writer, core code or combat policy changes for this repair. Fresh006 is prepared for the same frozen combat policy with corrected external observation.

Remaining: qualify the normal-AI policy and complete three comparable veteran runs, plus Zanthus stress, cooperative/live-scheduler/visibility/current-world/economics gates and the completion audit. Source site approvals and production activation remain false.

### Step 3k - Retained far-edge failure and native navigation recovery

Replacement-safe pilot006 survives through all three broods and reaches Mephitis2892 HP, then the client stops after ten refused walks at986.927 seconds. The actor has drifted east to2572,2098; the last requested stage goal is58 tiles away. The existing fixed-east web sidestep and distant reacquisition policy are the diagnosed driver problem. `navigation-failure.json`, the unchanged terminal snapshot and normal-AI/client logs remain. Provenance, all6,742 production inputs, 20 private saves, protected originals and acknowledged forced no-save teardown pass. This driver-error outcome is correctly accepted as preserved failure evidence, never as a clear or pacing result.

The v3c client chooses dodge candidates toward the fixed boss anchor and tries nearby native pathfinding alternatives. Long reacquisition requests use local goals within six tiles until the original goal is within24. The1,000-decision planner bound passes; it does not prove native collision behavior. Dedicated copied navigation001 then replays the actual far-edge origin with a blessed travel-only Player and no encounter. All measured movement uses the client:33.550 seconds to the boss anchor, zero refused walks/actions/damage/supplies. `--require-recovery-navigation` verifies this subset and all preservation/teardown; the three-leg navigation aggregate remains false for this test rather than being promoted. Forced compile/startup pass and no owned process/listener remains for the navigation copy.

Fresh combat007 stages the same character capabilities and v3c policy for a separately frozen no-antivenom run. No encounter/core/player-ability changes or automatic tuning passes occur. `CaptureBenchmark.py` includes navigation terminal outcomes and briefly waits for the independent client finish log before cleanup. The actual Archmage Overlord macro remains requested: port4713 is offline and the available protected profile contains only foundation macros; no substitute is passed off as the Zanthus stress case.

### Step 3l - Exact short-route recovery and actual macro discovery

Combat007 survives all broods but stops at1190.100 seconds with Mephitis1806 HP after repeated refusal of a22-tile path. The v3d driver now tries local native alternatives after a rejected short goal and goes directly to the boss anchor during boss reacquisition. Native navigation002 replays the failed origin/destination and returns to the anchor in10.213 seconds, including one refused direct request followed by accepted native movement. The pinned waypoint, no actions/damage/supply consumption, forced runtime compile/startup, all6,742 production inputs, 20 private saves, protected originals and acknowledged forced no-save teardown pass. Planner invariants pass separately. Combat007 remains preserved failure evidence, not a pacing sample. See [BENCHMARK.md](BENCHMARK.md#v3d-short-route-recovery-and-series-gate).

The original selected Archmage Overlord macro and its imported archmage_combat module have now been located read-only in the personal client. The selected entry uses an external FilePath; its empty inline Macro field did not mean the macro was absent. Private snapshots and provenance hashes are retained under outbreak-zanthus-source-20261029-001. The original profile/macro/module hashes remain unchanged. Static dependency inspection finds the main macro imports time plus the pure helper module; it preserves Wraith Form, Bless, Restoration, control, summon, Ignite and Mass Death behavior. This is dependency evidence, not execution or toolkit acceptance.

Separately selected ArachnidCharacterInspection.cs loads only the historical private save through matching serializers, reads the copied character and emits private XML without setting character fields. Character inspection001 finds exactly one Zanthus. Forced compile/startup and VerifyCharacterInspection.py pass with production/core/save/original preservation and owned forced no-save teardown. No personal client is launched and no profile or current/live-character claim follows from this historical copy. Complete copied-macro execution still needs a guarded fixture, actual resource/toolkit observations and combat evidence.

VerifyBenchmarkSeries.py adds the explicit three-run acceptance gate; incomplete004/005/006 are correctly rejected. Fresh008 is prepared for v3d. Accepted complete combat clears remain0/3 and automatic tuning passes0. Production activation, all reservation flags and site approvals remain false; the other native/cooperative/current-world/economics/completion gates remain open.

### Step 3m - Native original-macro preparation and boss-search repair

The actual copied Zanthus and unchanged original macro/helper now pass bounded native preparation in archmage-preparation003. The initial blessed fixture blocked Spell.Cast;001 is retained as that invalid observation,002 as server setup stopped before client execution, and corrected003 as unblessed native maintenance. Server-observed expected pools, Wraith Form, follower and existing jewel pass; macro timers alone are not treated as effects. Each scope has an explicit preservation/forced-teardown result. Character data, scripts and journals remain private. Full stationary/route-assisted combat stress remains open. Source traces, limits and reproduction: [ARCHMAGE_STRESS.md](ARCHMAGE_STRESS.md).

Combat008 reveals a different driver weakness: the boss leaves client visibility and fixed-anchor reacquisition does not follow its last seen location. The alive partial is stopped at1573.851 seconds after all broods, without clear/pacing credit. v3e stores client-seen boss positions and adds bounded search. The retained loss geometry,12,769-position visibility-distance coverage and existing navigation planner invariants pass; normal-AI combat remains required. Fresh009 uses a separately frozen v3e policy. See [BENCHMARK.md](BENCHMARK.md#v3e-boss-memory-and-search). All production/core/encounter/player ability values, source site flags and the empty approved catalog remain unchanged. Automatic tuning passes0 and accepted complete veteran clears0/3.

### Step 3n - Finite tithes and initial replacement-cost observation

Benchmark verification now reports net tithing balance reduction when present in both frozen reference and terminal observation. Partial008's balance falls by 630 points; this is not per-cast gross accounting. Source-extracted SBAlchemist/SBHealer base prices and the one-gold donation conversion give a 2,744 gold priced replacement subtotal for its recorded consumables/tithes. Petals, repairs, equipment, time, taxes, actual checkout and reward collection are excluded; this is not a completed-run or profit claim. [ECONOMICS.md](ECONOMICS.md) records the table, source traces, conditional NPC stock and remaining demand/reward questions. Gold remains shared and no reward/rate changes occur.

The boss-search planner report now explicitly identifies its synthetic remembered point rather than implying reconstructed native memory. Updated002 and navigation invariants pass. Pilot009 remains the separately frozen normal-AI check; all complete-series/native/stress/site/completion gates retain their prior scope.

### Step 3o - Ordinary running and actual request observation

Pilot009 reacquires Mephitis through actual client memory/search but stops as an alive incomplete reference at 1920.977 seconds. Its copied profile has running disabled; existing MeleeAI low-health flee behavior makes pursuit part of the competency check. No encounter/AI/HP/player ability changes follow. The terminal partial, all production/private-save/original preservation and forced teardown pass.

Running is now configured only in a fresh private profile before launch and frozen/checked as part of reference policy. Navigation003's 26.493-second recovery is supplemented by004's independently compiled passive movement observer: 26.454 seconds,86 actual running requests,0 walking/refused requests,0 actions/damage/supplies/tithes. Exact endpoint,6,742 production inputs, 20 private saves, originals and acknowledged forced teardown pass. The new hook only observes the guarded copied Actor and never changes event state or movement. Client metadata confirms Pathfind's boolean controls distance checking; the read-only IL inspector's two-byte-prefix bug is repaired. Scope and source traces: [BENCHMARK.md](BENCHMARK.md#v3f-ordinary-running-and-movement-evidence).

Fresh010 uses unchanged v3e combat code with normal running and actual request counters as v3f. Three complete comparable running references remain required, including a no-antivenom run. Accepted full clears0/3, automatic tuning0, source reservations unapproved, catalog empty and production disabled.

### Step 3p - Actual original stationary macro and explicit behavior decision

The actual copied Zanthus runs the unchanged original macro/helper against normal encounter AI in stationary002. It clears one brood/eleven enemies and earns one actual gland entitlement, with zero observed health loss/deaths/movement requests. The bounded case ends stationary-idle at133.473 seconds including120 seconds without further encounter health loss. Frozen prepared inputs, original character identity/equipment/skills, private resource deltas,6,742 production inputs, 20 private saves, original profile/scripts and acknowledged forced no-save teardown pass. Initial001's test-only HashSet compiler failure remains retained; the fixture uses Dictionary instead. This is a partial observation, not full-clear/farming/pacing acceptance. [ARCHMAGE_STRESS.md](ARCHMAGE_STRESS.md) records reproduction and limits.

Computer Use capture shows the copied character window but overlays obscure the field. Input and its single refreshed retry fail with "failed to activate captured window"; no input or ground pixels are accepted. Backend/native script observations remain separate.

Running-enabled reference010 still exposes prolonged low-health pursuit. A concrete owned-Mephitis-only territorial behavior proposal and alternatives are recorded in [BOSS_BEHAVIOR_REVIEW.md](BOSS_BEHAVIOR_REVIEW.md). A user design decision is requested because AI behavior falls outside the agreed automatic tuning bounds. No proposed AI change has been applied; ordinary game behavior, player power, rewards, reservations and production remain unchanged. Other independent acceptance work can continue while the decision is pending.

### Step 3q - Completed original archmage route and failure containment

Corrected route002 completes the actual encounter in277.502 seconds with the original macro/helper and explicitly separate idle walking assistance. The actual copied Zanthus retains earned capabilities: all three broods/40 owned deaths including Mephitis,0 player deaths,400 observed health loss/restoration,474 native movement requests. Actual pending rewards are6 glands and1 skull, with0 artifact claims and15,000 new encounter ground gold. Ordinary corpse loot is unassessed; private net resource deltas remain separate from per-cast cost/profit. No minimum duration is imposed on this earned-power stress case.

Initial route001 remains a fixture failure, not a clear: the terminal XML writer attempted attributes after child elements and crashed the private server. The corrected writer and process-bound observer prevent that publication failure and stale-status monitoring; a real live/exit guard check passes. Failed-server teardown records no false native acknowledgement. Route002's initial client launch correction happens before login/preparation/combat freezing and remains retained separately.

The completed route, stationary historical case and native preparation case pass their separate verification scopes. Captured forced runtime compile-only output,6,742 production inputs, accepted core, 20 private saves, protected originals, personal source hashes and acknowledged forced no-save teardown pass. Source traces and reproduction are in [ARCHMAGE_STRESS.md](ARCHMAGE_STRESS.md).

Reference010 is stopped alive/incomplete at2261.441 seconds, with4,841 actual running requests. Its preservation passes; it contributes no complete pacing sample. Accepted veteran full clears remain0/3, tuning0. The AI design decision, three comparable reference clears, natural TBC/support acceptance, site visibility/current-world preflight/approval and final completion audit remain open. Production activation remains disabled.

### Step 3r - Natural turn-based warning/reaction/impact/self-escape

Fresh live-turn003 passes the process-bound observer and post-teardown verifier. The real manager scheduler drives production Mephitis pulses after explicit test-arm scene enrollment. Actual copied Zanthus receives a legal reaction turn, waits at least3.5 real seconds, ends it through the normal client command and later escapes the restraint through a real UseObject packet at4 AP. No warning/restraint construction, turn advancement or attack weakening is supplied by the live fixture. All6,742 production inputs, accepted core, 20 private saves and protected originals pass preservation; activation remains disabled.

Starter001's death and002's polling-baseline false negative remain retained without full acceptance. Corrected read-only observation uses the warning's actual stored turn baseline. Computer Use confirms welcome-panel dismissal by right-click in003, but panels still obscure the field; no ground-warning pixel or Computer Use escape-input acceptance is claimed. [NATIVE_ACCEPTANCE.md](NATIVE_ACCEPTANCE.md) records exact scope and reproduction. Pacing, cooperative/live-dodge/ground/site gates and final release work remain open.

### Step 3s - Pursuit toolkit and territorial-survival reassessment

The monitor relays guidance to consider mounts/speed powers and preserve Mephitis's effort to survive within her territory. The earlier stand-and-fight snippet is withdrawn from the current recommendation, not applied. Source confirms mounted running has a shorter allowed delay, and Celerity uses the higher of Swords/Psychology: the existing swords reference can pass its skill gate without Psychology training. Actual mounting, usable learned power/resources and native speed effects remain to be tested before revising the frozen reference. Wind Runner's monk-equipment restriction and Syth Speed's Karma restriction prevent treating all speed powers interchangeably.

The revised review proposes bounded retreat inside the existing combat footprint with ordinary attacks/webs and no new healing/budgets/power nerfs. General NPC AI remains a read-only exploration of the existing exact-type tactical rollout and phase5 roadmap. No AI overhaul phase, broad rollout, new player ability, source reservation approval or production activation is executed here.

### Step 3t - Native mounted mobility candidate

Mounted-navigation001 consumes a declared vendor-obtainable cage through normal client use, then mounts its owned horse through normal BaseMount dispatch. The fixture never assigns Rider or grants taming skill. Before/after reference signatures confirm other capabilities unchanged. Mounted/type/ownership/rider now form part of frozen reference comparison, with historical v3 omissions normalized as unmounted. Cage consumption is preparation, not a navigation expenditure or an actual NPC purchase.

The same far-edge recovery completes in21.179 seconds with86 running/0 walking requests,0 refusals and no combat/navigation resources/tithes used. Computer Use shows the armored rider on the horse. All6,742 production inputs, 20 private saves, protected originals and acknowledged forced teardown pass. CaptureBenchmark now binds the actual private server process to avoid stale-status monitoring. Full mounted combat, all three comparable pacing clears and AI territorial-retreat implementation remain unproven; tuning stays0 and production disabled.

## Native dodge checkpoint and chosen world basis

Live-dodge002 completes a natural reaction-turn dodge for 2 observed AP, cancels the original warning and remains free after a later boss turn. Current source, private/original input preservation and acknowledged teardown pass. Live-dodge001 retains the normal 30-second turn timeout caused by the test's visual-inspection wait; no dodge is credited there. Bounded optional handoff and automatic failed-client stop protect later attempts. This is scripted native input; Computer Use ground-warning acceptance remains separate. See NATIVE_ACCEPTANCE.md.

The user explicitly selects the available October 17 offline test-world save for the final site preflight. Its 20 files are confirmed byte-identical to the native baseline. Use that declared snapshot basis rather than waiting for another folder; do not describe it as current live occupancy. Repeat matching-source occupancy/maximum-city/portal checks in a separate private copy and retain the source save hashes. Specific reservation/site approval and production activation remain separate and disabled.

## Resumed verification and selected-snapshot clearance

After explicit user resume, the worktree matches the pause checkpoint, with no intervening commits and zero owned runtimes/jobs/listeners. Retained live-dodge002 and live-turn003 escape verification pass without replaying combat; the native-dodge batch is committed as f07b5e77. The accepted mounted series remains intact and is not rerun.

The new selected-snapshot preflight002 passes all six hypothetical reserves with 186 static regions, zero house/max-city-growth/valid-legacy/portal intersections and unchanged 20 source/private save files. All 6742 current production inputs and matching assemblies/Regions.xml are verified. The initial probe compile failure is retained before world loading. RESERVATIONS.md and SELECTED_WORLD_PREFLIGHT.json retain method, evidence and remaining boundaries. The data-folder question is resolved by the user's chosen October 17 basis; no current-live-world or reservation approval is claimed.

## Native presentation fault and narrow rejection repair

Presentation001 provides central/northern sample images, then encounters an actual combatant-intent bridge fault; eastern sampling is not accepted. The rejected-outsider logging null dereference is reproduced on the old assembly and corrected without changing rejection eligibility or any combat/player/reward values. The new candidate passes forced runtime script compilation, actual-manager/core-bridge rejection checks and 92 web assertions. See [TBC_REJECTION_FIX.md](TBC_REJECTION_FIX.md) for source/evidence, pending native rerun and explicit carry-forward limits. Production remains disabled; frozen pacing and offline preflight reports are preserved rather than silently relabeled or replayed.

## Completed staged presentation samples

Corrected presentation002 completes six boss and two brood-anchor Computer Use samples without a bridge fault or fixture error. The western tree/eastern foreground overlap remains a readability concern; sampled warning visibility is not universal tile visibility or geography approval. AI and turn scheduler are stopped in this rendering fixture. VerifyPresentation --require-six passes 6742 production inputs, 6611 corresponding private runtime inputs, 20 private saves, protected originals and acknowledged forced no-save teardown. [PRESENTATION.md](PRESENTATION.md) and PRESENTATION_SAMPLES.json retain scope/reproduction. Original failed001 remains partial. RunLiveTurn historical verification now requires an explicit matching commit when production inputs differ. Cooperative/support roles, ordinary reward collection/economics, final requirement audit and explicit geography approval remain open.

## Controlled cooperative role acceptance

Six independent native role types pass across retained004 (healing5 points; cure/rescue/prevention2 each; party treatment3 protections with0 application points) and isolated006 (bard0.50302964 useful-time points). Both actors are connected living Players. These are synthetic controlled scenes with staged injury/poison/party/resources, paused enemy AI and real-clock sampling of the production bard observer; no group difficulty/pacing/economics or Computer Use role acceptance is inferred. The five successful cases are preserved rather than replayed for the bard isolation. All6742 production inputs match across copies; source/private/original preservation and acknowledged forced teardown pass. [COOPERATIVE.md](COOPERATIVE.md) records failures, the fixture Player-flag correction, the client packet-wait race, reproduction and carry-forward scope. No production source changes occur. Reward collection/economics, final requirement audit and explicit geography/occlusion review remain open.

## Final observational and source reconciliation

RELEASE_REVIEW.md and the44-row final-review column consolidate the unchanged gameplay/source evidence. CheckDeliveryInventory verifies6742 current production inputs,13 project-listed outbreak scripts and44 source/test path pairs; it is a static check. RecordEconomicObservations reads retained accepted results without replay: all3 completed references plus stationary/route original-macro records retain supply, retreat, ground/pending/backpack and unknown collection/value categories separately. The economic record is not a profit/market pass. Exact permanent geography approval is the remaining decision before catalog completion; no deployment or source flag changes occur.

## Completed user-approved geography

The user approved all six footprints. GEOGRAPHY_APPROVAL.md records exactly two production source changes: reviewed definitions and a catalog derived from those definitions. Fresh candidate compile, reservation80/controller63 assertions, actual-policy copied-save preflight6/6 and copied -service -nocache startup all pass. Inputs/core/project/serializer boundaries remain explicit;20 source/private save files and protected originals are preserved. Owned startup teardown is acknowledged forced no-save, not graceful. No unchanged pacing/support runs are repeated. All prototype batches and requirement dispositions are reconciled; no pending design decision remains. DeliveryReady stays false, and live deployment/current-world changes remain outside this completed prototype.
