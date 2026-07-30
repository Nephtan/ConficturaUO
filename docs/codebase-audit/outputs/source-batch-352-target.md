# SOURCE-BATCH-352 SpecialSeaweed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-352`
- Candidate: `SB352-CAND-001`
- System: `Items:Trades:Fishing / SpecialSeaweed`
- File: `Data/Scripts/Items/Trades/Fishing/SpecialSeaweed.cs`

## Intended Source Change

Add local guards to `SpecialSeaweed.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, `from.Backpack`, or a deleted source item before the existing skill and bottle-consumption flow runs.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.
- When `from.Backpack == null`, use the existing empty-bottle failure message and return.
- Do not require `SpecialSeaweed` to be inside the backpack because the existing method does not enforce that policy.

## Must Stay Unchanged

Randomized seaweed names, hues, `SkillNeeded` values, stack/amount behavior, `Seafaring` skill check range, bottle requirement, empty-bottle message, `PlaySound(0x240)`, all potion output mappings, failure and success messages, `Consume()` behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
