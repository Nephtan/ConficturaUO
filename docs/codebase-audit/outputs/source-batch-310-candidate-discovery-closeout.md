# SOURCE-BATCH-310 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-310+` selected a clean WizardStaff interaction/helper guard repair and deferred the matching LevelStave and GiftStave repairs for separate one-item batches.

## Recommended Target

- Candidate: `SB310-CAND-001`
- Batch: `SOURCE-BATCH-310`
- Source file: `Data/Scripts/Items/Weapons/Marksman/WizardStaff.cs`
- Behavior: add stale/null/mobile/source-staff/backpack/gem guards to `BaseWizardStaff.OnDoubleClick`, `GemTarget.OnTarget`, and `HasStaff` before possession checks, target assignment, gem backpack checks, gem conversion, or backpack helper lookups.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `LevelStave.cs`: zero gate/overlay hits, deferred as the matching level-stave guard repair.
- `GiftStave.cs`: zero gate/overlay hits, deferred as the matching gift-stave guard repair.

## Result

`SB310-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change WizardStaff/WizardStick constructors, combat/ranged behavior, `damageType`, ammo, possession rule, gem conversion ratios, `MageEye` output, messages, sound `0x243`, `RevealingAction`, gem delete behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
