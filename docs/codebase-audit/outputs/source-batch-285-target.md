# SOURCE-BATCH-285 Obsolete AcidProofRobe Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-285`
- Candidate: `SB285-CAND-001`
- System: `Items:Magical / Artifacts / Obsolete AcidProofRobe`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Obsolete/Obsolete_AcidProofRobe.cs`
- Behavior: add stale/null/mobile/source-item/backpack guards to legacy `AcidProofRobe.OnDoubleClick(Mobile from)` before cooldown math, parent checks, bottle consumption, acid creation, or messages.

## Allowed Source Change

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, treat `from.Backpack == null` as the existing empty-bottle failure path.

## Must Stay Unchanged

- Legacy runtime-visible file path and `AcidProofRobe` type identity.
- Worn-robe requirement.
- 180-minute cooldown and wait message.
- Empty-bottle failure message.
- `Bottle` consumption, `BottleOfAcid` creation, sound `0x240`, success message, and `TimeUsed` update.
- Name properties.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file overlay evidence: `RB-00865=FalsePositive`; `RB-05756/RB-06080=IntentionalLegacy`; all remain nonblocking because serialization, type identity, and layout are untouched.

## Ready Goal Shape

`/goal SOURCE-BATCH-285 Obsolete AcidProofRobe Guard Repair`
