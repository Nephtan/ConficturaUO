# SOURCE-BATCH-315 Waterskin Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-315`
- Candidate: `SB315-CAND-001`
- Behavior: add stale/null/mobile/source-drink/backpack guards to waterskin drinking helper paths.
- System: `Items:Food / Waterskin drinking helpers`
- File: `Data/Scripts/Items/Food/Waterskin.cs`

## Allowed Source Change

Add guard-only checks to:

- `Waterskin.OnDoubleClick(Mobile from)`
- `DirtyWaterskin.OnDoubleClick(Mobile from)`
- `DrinkingFunctions.CheckWater(Mobile from, int range, out bool soaked)`
- `DrinkingFunctions.OnDrink(Item drink, Mobile from)`
- `DrinkingFunctions.DrinkBenefits(Mobile from)`

The guards may return early for null/deleted mobiles, deleted drink items, missing backpacks, or stale helper state before dereferences, water checks, backpack checks, thirst mutation, drink consumption, or benefit calculation.

## Must Stay Unchanged

- Water target IDs
- `CheckWater` range/LOS behavior
- Fill/drink backpack messages
- Waterskin/canteen item ID/name/weight transitions
- Thirst increments and messages
- `BloodDrinker`/`BrainEater` restrictions
- Dirty waterskin conversion
- `DrinkBenefits` stamina/poison behavior
- Sound/animation/gump behavior
- Consume semantics
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-315 Waterskin Guard Repair
```
