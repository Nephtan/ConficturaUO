# SOURCE-BATCH-314 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-314+` selected a clean JukaBow guard repair.

## Recommended Target

- Candidate: `SB314-CAND-001`
- Batch: `SOURCE-BATCH-314`
- Source file: `Data/Scripts/Items/Weapons/Bows/JukaBow.cs`
- Behavior: add stale/null/mobile/source-bow/backpack/gears guards to `JukaBow.OnDoubleClick` and `OnTargetGears` before modified-state checks, backpack checks, skill checks, target assignment, gear consumption, hue mutation, or slayer assignment.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- None. `SOURCE-BATCH-315+` requires fresh candidate discovery after `SOURCE-BATCH-314` is committed.

## Result

`SB314-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change `IsModified` behavior, backpack-use messages, Bowcraft `100.0` threshold, target range/callback flow, `Gears` requirement and consume semantics, hue `0x453`, `Slayer` random assignment, success/failure messages, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
