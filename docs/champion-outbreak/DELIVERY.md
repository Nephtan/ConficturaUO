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
