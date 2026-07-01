# SOURCE-BATCH-330 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-330+` selected a clean `BoatStain` guard repair.

## Recommended Target

- Candidate: `SB330-CAND-001`
- Batch: `SOURCE-BATCH-330`
- Source file: `Data/Scripts/Items/Boats/BoatStain.cs`
- Behavior: add stale/null/mobile/source-stain/backpack/deleted-target guards to `BoatStain.OnDoubleClick` and `DyeTarget.OnTarget` before backpack checks, target assignment, boat deed eligibility checks, hue mutation, or sound/reveal behavior.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- Boat policy, boat deed behavior, hue policy, serializer-layout, project/config/data, XML/config/data, and reorganization candidates remain excluded unless later explicitly approved.
- The next source batch must run fresh candidate discovery before editing another file.

## Result

`SB330-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change target range, prompts, messages, boat deed/docked boat eligibility, hue assignment, reveal/sound behavior, source stain consumption behavior, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
