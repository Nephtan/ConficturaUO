# SOURCE-BATCH-260 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-260+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB260-CAND-001`
- Batch: `SOURCE-BATCH-260`
- Source file: `Data/Scripts/Items/Special/Heritage Items/WallTorch.cs`
- Behavior: add stale/null/mobile/source-component guard to `WallTorchComponent.OnDoubleClick(Mobile from)` before range checking, item-id toggling, sound playback, and reach-message paths.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file save-compat rows: `PBJ-0974`, `PBJ-0975`, and `PBJ-0976` are `IntentionalLegacy` and remain nonblocking because this batch does not edit serialization.
- Prior source-batch intake hits for this exact file: `0`

## Result

`SB260-CAND-001` is ready for a focused non-gated source batch. Gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, and reorganization work remain excluded.
