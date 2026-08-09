# SOURCE-BATCH-284 Artifact_AcidProofRobe Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-284`
- Candidate: `SB284-CAND-001`
- System: `Items:Magical / Artifacts / Artifact_AcidProofRobe`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Artifact_AcidProofRobe.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to `Artifact_AcidProofRobe.OnDoubleClick(Mobile from)` and `OnDragLift(Mobile from)` before cooldown math, parent checks, bottle consumption, acid creation, messages, or drag-lift messaging.

## Allowed Source Change

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, treat `from.Backpack == null` as the existing empty-bottle failure path.
- In `OnDragLift(Mobile from)`, return `false` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- Worn-robe requirement.
- 120-minute cooldown and wait message.
- Empty-bottle failure message.
- `Bottle` consumption, `BottleOfAcid` creation, sound `0x240`, success message, and `TimeUsed` update.
- Drag-lift message for valid `PlayerMobile` users.
- Artifact setup call and construction metadata.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file overlay evidence: `RB-00739` is POST-BATCH-U `FalsePositive` save-compat evidence and remains nonblocking because serialization is untouched.

## Ready Goal Shape

`/goal SOURCE-BATCH-284 Artifact_AcidProofRobe Guard Repair`
