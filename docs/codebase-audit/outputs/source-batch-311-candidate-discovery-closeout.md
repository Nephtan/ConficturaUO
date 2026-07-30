# SOURCE-BATCH-311 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-311+` selected the queued sibling LevelStave guard repair from `SOURCE-BATCH-310` discovery.

## Recommended Target

- Candidate: `SB311-CAND-001`
- Batch: `SOURCE-BATCH-311`
- Source file: `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`
- Behavior: add stale/null/mobile/source-stave/backpack/gem guards to `BaseLevelStave.OnDoubleClick`, `GemTarget.OnTarget`, and `HasStaff` before possession checks, target assignment, gem backpack checks, gem conversion, or backpack helper lookups.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `GiftStave.cs`: zero gate/overlay hits, deferred as the matching gift-stave guard repair.

## Result

`SB311-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change LevelStave/LevelSceptre constructors, combat/ranged behavior, leveling behavior, `damageType`, ammo, possession rule, gem conversion ratios, `MageEye` output, messages, sound `0x243`, `RevealingAction`, gem delete behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
