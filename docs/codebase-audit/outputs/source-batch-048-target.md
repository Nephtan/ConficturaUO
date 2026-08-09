# SOURCE-BATCH-048 OilWood Guard Repair Target

## Target

- Batch: SOURCE-BATCH-048
- Candidate: SB046-CAND-003
- Behavior: add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilWood.OnDoubleClick` and `OilTarget.OnTarget`.
- System: Items:Potions / Oils / OilWood
- File: `Data/Scripts/Items/Potions/Oils/OilWood.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- carpentry or bowcraft skill threshold `90`
- wood item eligibility
- artifact rejection
- oil cloth requirement and deletion
- `Bottle` return
- `m_Oil.Consume()` behavior
- `MorphingItem.MorphMyItem` behavior
- weapon and armor resource updates
- messages and sound `0x23E`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
