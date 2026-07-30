# SOURCE-BATCH-358 SerpentSpawners Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-358`
- Candidate: `SB358-CAND-001`
- System: `Quests:Serpents / SerpentSpawners`
- File: `Data/Scripts/Quests/Serpents/SerpentSpawners.cs`

## Intended Source Change

Add local guards to `SerpentSpawnerOrder.OnDoubleClick(Mobile from)` and `SerpentSpawnerChaos.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from`, use a missing/deleted backpack, or operate on a deleted source statue before the existing serpent-item lookup and spawn behavior runs.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.
- Treat a missing or deleted backpack as the existing no-serpent failure message for the matching statue.
- Read the serpent item from a guarded local backpack variable.

## Must Stay Unchanged

Item metadata, item hues/names/weights, `BlackrockSerpentOrder` and `BlackrockSerpentChaos` lookup types, `SerpentOfOrder` and `SerpentOfChaos` spawn classes, spawn location/map, sound `0x217`, source item `Delete` behavior, blue/red no-serpent messages, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
