# SOURCE-BATCH-047 OilLeather Guard Repair Target

## Target

- Batch: SOURCE-BATCH-047
- Candidate: SB046-CAND-002
- Behavior: add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to `OilLeather.OnDoubleClick` and `OilTarget.OnTarget`.
- System: Items:Potions / Oils / OilLeather
- File: `Data/Scripts/Items/Potions/Oils/OilLeather.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- tailoring skill threshold `90`
- leather item eligibility
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
