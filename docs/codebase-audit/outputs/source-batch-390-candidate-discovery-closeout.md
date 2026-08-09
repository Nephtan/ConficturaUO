# SOURCE-BATCH-390 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-390` ran fresh non-gated candidate discovery after `SOURCE-BATCH-389` and selected `SB390-CAND-001`.

## Recommended Target

- Candidate: `SB390-CAND-001`
- Batch: `SOURCE-BATCH-390`
- System: `Items:Gifts / Holiday / Halloween / ChocolateMonster`
- File: `Data/Scripts/Items/Gifts/Holiday/Halloween/HalloweenBag.cs`
- Behavior: add stale/null mobile and deleted source-item guards to `ChocolateMonster.OnDoubleClick(Mobile m)` before the existing sound, animation, overhead text, stat restoration, hunger/thirst assignment, and source deletion behavior.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`ChocolateMonster.OnDoubleClick` is a narrow candy consumption interaction inside a zero-gate file. The guard repair prevents invalid interaction state from dereferencing a null/deleted mobile or deleting a stale item without changing valid candy randomization, phrase selection, animation, stat restoration, hunger/thirst behavior, serialization, or policy behavior.

## Deferred Or Excluded Work

Candy reward tuning, stat restoration tuning, hunger/thirst tuning, animation/sound changes, Halloween gift-bag behavior, staff/access policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
