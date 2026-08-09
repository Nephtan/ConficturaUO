# SOURCE-BATCH-389 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-389` ran fresh non-gated candidate discovery after `SOURCE-BATCH-388` and selected `SB389-CAND-001`.

## Recommended Target

- Candidate: `SB389-CAND-001`
- Batch: `SOURCE-BATCH-389`
- System: `Items:Gifts / Holiday / Halloween / HalloweenMaiden`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/HalloweenMaiden.cs`
- Behavior: add stale/null mobile and deleted source-item guards to `HalloweenMaiden.OnDoubleClick(Mobile from)` before the existing furniture item-ID toggle.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`HalloweenMaiden.OnDoubleClick` is a narrow furniture interaction that flips between two item IDs. The guard repair prevents invalid interaction state from mutating a deleted item and rejects null/deleted mobiles without changing the valid furniture toggle behavior, item metadata, serialization, or policy behavior.

## Deferred Or Excluded Work

Furniture access restrictions, range checks, item-ID redesign, Halloween reward behavior, staff/access policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
