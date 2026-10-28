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
