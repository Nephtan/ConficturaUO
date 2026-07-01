# SOURCE-BATCH-340 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-340+` selected a clean `RuneStoneGate` guard repair.

## Recommended Target

- Candidate: `SB340-CAND-001`
- Batch: `SOURCE-BATCH-340`
- Source file: `Data/Scripts/Quests/Underworld/RuneStoneGate.cs`
- Behavior: add stale/null/mobile/source-item guards to `RuneStoneGate.OnDoubleClick` before sending the existing static runes message.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Region/map text candidates, travel behavior, quest reward tuning, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB340-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change item ID, `Movable`, name, message text, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
