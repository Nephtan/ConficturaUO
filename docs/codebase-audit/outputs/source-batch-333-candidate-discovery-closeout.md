# SOURCE-BATCH-333 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-333+` selected a clean `SlaversNet` guard repair.

## Recommended Target

- Candidate: `SB333-CAND-001`
- Batch: `SOURCE-BATCH-333`
- Source file: `Data/Scripts/Items/Special/SlaversNet.cs`
- Behavior: add stale/null/mobile/source-net/backpack/deleted-target guards to `SlaversNet.OnDoubleClick` and `SlaveTarget.OnTarget` before backpack checks, target assignment, `BaseCreature` reads, follower checks, capture mutation, sound behavior, or source net deletion.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Taming/capture balance, follower policy, creature-control policy, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB333-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, prompts, messages, `BaseCreature` eligibility, capture odds, follower rules, control/bonding behavior, sound behavior, source net deletion behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
