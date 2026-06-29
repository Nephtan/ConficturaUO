# SOURCE-BATCH-285 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-285+` selected one narrow non-gated guard repair.

## Recommended Target

- Candidate: `SB285-CAND-001`
- Batch: `SOURCE-BATCH-285`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AcidProofRobe.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to legacy `AcidProofRobe.OnDoubleClick(Mobile from)` before cooldown math, parent checks, bottle consumption, acid creation, or messages.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file unresolved active overlay rows: `0`
- Resolved exact-file overlay evidence: `RB-00865` is POST-BATCH-U `FalsePositive`; `RB-05756` and `RB-06080` are POST-BATCH-L `IntentionalLegacy`; all remain nonblocking because serialization, type identity, and layout are untouched.
- Prior source-batch hits for this exact file: `0`

## Result

`SB285-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change legacy runtime visibility, class/type identity, worn-robe requirement, cooldown timing, acid bottle creation, messages, name properties, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
