# Arachnid Outbreak delivery checklist

Status: InProgress. Production activation is disabled. Specification: [SPECIFICATION.md](SPECIFICATION.md).

## Baseline - 2026-10-28

Work on the current branch. Preserve these unrelated, pre-existing untracked files:

- `Confictura-backlog-review-2026-10-17.html`
- `scripts/Test-SkillVerificationProtection.ps1`
- `tests/SkillVerificationProtection.cs`

No production launch, save access, deployment, or automatic activation is authorized by this delivery.

## Batch 1 - Lifecycle, placement, persistence, staff controls

- [ ] Separate versioned controller and state; default disabled.
- [ ] Six terrain-validated, client-reviewed mainland Sosaria sites.
- [ ] Availability, engagement, three brood stages, boss, recovery, cooldown.
- [ ] Restart persistence, ownership cleanup, legacy coexistence.
- [ ] Staff access/guards and status/start/stop controls.
- [ ] Component tests, runtime script compile, and focused commit.

## Batch 2 - Participation, rewards, crafting

- [ ] Stage ledger, combat/pet/support credit and abuse exclusions.
- [ ] Persistent idempotent pending awards and claim command.
- [ ] Glands, two-treatment Alchemy recipe, prevention and cooldown.
- [ ] Fixed shared gold shower, skull, artifact opportunity; no power scrolls.
- [ ] Save/reload and old champion regressions; core build/artifact if needed.
- [ ] Focused verified commit.

## Batch 3 - Combat and client acceptance

- [ ] Finite defenders/reinforcements and guarded death transitions.
- [ ] Web warning, avoidance, restraint, escape/rescue; turn-based integration.
- [ ] Freeze legal veteran fixture and private Zanthus stress fixture.
- [ ] Three full reference runs (20-35 minute median), one without antivenom.
- [ ] Zanthus macro/farming measurements and targeted native acceptance.
- [ ] Economic/supply observations, failure evidence, bounded tuning record.
- [ ] Test-input restoration, enable/disable instructions, focused commit.

## Completion audit

- [ ] Trace every specification requirement to source and appropriately scoped evidence.
- [ ] Clearly distinguish component, runtime compile, save/reload, scripted-client and native-client results.
- [ ] No missing gate is represented as passed. Production activation remains disabled.

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

- Entry point: `ArachnidOutbreakController.Initialize` registers `[ArachnidOutbreak <status|create|start|stop>]` at Administrator access and restores loaded controllers. The handler independently guards null/deleted/low-access mobiles. Creation is disabled by default; start rejects incomplete delivery, ambiguous controllers, and absent reviewed sites.
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
