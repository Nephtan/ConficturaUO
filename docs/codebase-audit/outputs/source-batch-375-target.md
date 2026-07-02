# SOURCE-BATCH-375 DDRelicTablet Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-375`
- Candidate: `SB375-CAND-001`
- System: `Items:Relics / DDRelicTablet`
- File: `Data/Scripts/Items/Relics/DDRelicTablet.cs`

## Intended Source Change

Add a local guard to `DDRelicTablet.OnDoubleClick(Mobile e)` so stale/null interaction state cannot run existing house access, backpack, intelligence, gump, or message behavior through a null or deleted mobile, or from a deleted source tablet item.

Allowed changes:

- Return immediately when `e == null || e.Deleted || Deleted`.

## Must Stay Unchanged

`RelicGoldValue`, `RelicFlipID1`, `RelicFlipID2`, `RelicDescription`, `SearchDungeon`, `SearchType`, `SearchItem`, `SearchReal`, house access behavior, backpack-read message, intelligence check, `TabletGump` behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
