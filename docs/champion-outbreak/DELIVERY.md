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
