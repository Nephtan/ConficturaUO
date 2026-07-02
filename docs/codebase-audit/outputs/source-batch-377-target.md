# SOURCE-BATCH-377 DDRelicOrbs Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-377`
- Candidate: `SB377-CAND-001`
- System: `Items:Relics / DDRelicOrbs`
- File: `Data/Scripts/Items/Relics/DDRelicOrbs.cs`

## Intended Source Change

Add a local guard to `DDRelicOrbs.OnDoubleClick(Mobile from)` so stale/null interaction state cannot build and send the existing private overhead message through a null/deleted mobile, deleted source orb, or missing `NetState`.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted || from.NetState == null`.

## Must Stay Unchanged

`RelicGoldValue`, randomized item IDs, randomized hue/name setup, random vision text choices, `PrivateOverheadMessage` text and hue, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
