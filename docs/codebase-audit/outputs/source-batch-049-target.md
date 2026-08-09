# SOURCE-BATCH-049 OilAmethyst Guard Repair

## Target

- Candidate: `SB049-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-oil/target-item/oil-cloth guards to OilAmethyst interactions.
- System: `Items:Potions / Oils / OilAmethyst`
- Source file: `Data/Scripts/Items/Potions/Oils/OilAmethyst.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in:

- `OilAmethyst.OnDoubleClick(Mobile from)`
- `OilTarget.OnTarget(Mobile from, object targeted)`

The guards may return for null/deleted mobiles and may use the existing backpack-use or invalid-target outcomes for deleted/out-of-backpack source oils, missing backpacks, deleted target items, and missing/deleted oil cloth state.

## Must Stay Unchanged

Blacksmith skill threshold `90`, metal item eligibility, artifact rejection, oil cloth requirement/deletion, `Bottle` return, `m_Oil.Consume`, `MorphingItem.MorphMyItem`, weapon and armor resource updates, messages, sound `0x23E`, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-049 OilAmethyst Guard Repair
```
