# SOURCE-BATCH-061 OilSpinel Guard Repair

## Target

- Candidate: `SB049-CAND-013`
- Behavior: add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to OilSpinel interactions.
- System: `Items:Potions / Oils / OilSpinel`
- Source file: `Data/Scripts/Items/Potions/Oils/OilSpinel.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in `OilSpinel.OnDoubleClick(Mobile from)` and `OilTarget.OnTarget(Mobile from, object targeted)`.

## Must Stay Unchanged

Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-061 OilSpinel Guard Repair
```
