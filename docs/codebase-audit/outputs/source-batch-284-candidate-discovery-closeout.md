# SOURCE-BATCH-284 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-284+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB284-CAND-001`
- Batch: `SOURCE-BATCH-284`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Artifact_AcidProofRobe.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `Artifact_AcidProofRobe.OnDoubleClick(Mobile from)` and `OnDragLift(Mobile from)` before cooldown math, parent checks, bottle consumption, acid creation, messages, or drag-lift messaging.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file overlay evidence: `RB-00739` is POST-BATCH-U `FalsePositive` save-compat evidence and remains nonblocking because serialization is untouched.
- Prior source-batch hits for this exact file: `0`

## Result

`SB284-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change the worn-robe requirement, cooldown timing, acid bottle creation, messages, artifact setup, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
