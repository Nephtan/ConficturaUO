# SOURCE-BATCH-322 DDRelicPainting Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-322`
- Candidate: `SB322-CAND-001`
- Behavior: add stale/null/mobile/source-item/backpack guards to the relic painting interaction path.
- System: `Items:Relics / DDRelicPainting`
- File: `Data/Scripts/Items/Relics/DDRelicPainting.cs`

## Allowed Source Change

Add guard-only checks to `DDRelicPainting.OnDoubleClick(Mobile from)`.

The guards may return early for null/deleted mobiles or a deleted source relic before dereferencing the mobile or the relic item. Missing backpacks should use the existing backpack failure path and message.

## Must Stay Unchanged

- Backpack-use failure message
- Muck cleanup branch and message
- Hue reset/randomization
- Name replacement behavior
- Hard-coded painting `ItemID` mapping
- `RelicGoldValue` persistence
- Constructor/name randomization
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-322 DDRelicPainting Guard Repair
```
