# SOURCE-BATCH-376 DDRelicDrink Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-376`
- Candidate: `SB376-CAND-001`
- System: `Items:Relics / DDRelicDrink`
- File: `Data/Scripts/Items/Relics/DDRelicDrink.cs`

## Intended Source Change

Add a local guard to `DDRelicDrink.OnDoubleClick(Mobile from)` so stale/null interaction state cannot mutate player thirst/stamina, consume the source drink, play sound, or return a container through a null or deleted mobile, or from a deleted source drink item.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

`RelicGoldValue`, randomized item IDs, randomized hue/name/weight/liquid setup, stamina and thirst assignment, `Consume()` behavior, sound behavior, `Barrel`/`PotionKeg`/`Bottle` return behavior, messages, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
