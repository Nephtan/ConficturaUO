# SOURCE-BATCH-388 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-388` ran fresh non-gated candidate discovery after `SOURCE-BATCH-387` and selected `SB388-CAND-001`.

## Recommended Target

- Candidate: `SB388-CAND-001`
- Batch: `SOURCE-BATCH-388`
- System: `Items:Trades / Fishing / WetClothes`
- File: `Data/Scripts/Items/Trades/Fishing/WetClothes.cs`
- Behavior: add stale/null mobile and deleted source-item guards to `WetClothes.OnDoubleClick(Mobile from)` before the existing squeeze message, sound, dry-clothing creation, and source deletion behavior.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`WetClothes.OnDoubleClick` is a narrow item interaction that transforms a wet clothing item into the matching dry clothing item and deletes the wet item. The guard repair prevents invalid interaction state from dereferencing a null/deleted mobile or mutating a deleted source item without changing clothing output mappings, random dyed hue behavior, sound, message, AddToBackpack behavior, serialization, or policy behavior.

## Deferred Or Excluded Work

Backpack ownership restrictions, range checks, dry clothing output changes, randomization changes, fishing reward tuning, staff/access policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
