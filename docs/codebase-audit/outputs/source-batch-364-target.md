# SOURCE-BATCH-364 RobotSchematics Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-364`
- Candidate: `SB364-CAND-001`
- System: `Quests:Robots / RobotSchematics`
- File: `Data/Scripts/Quests/Robots/RobotSchematics.cs`

## Intended Source Change

Add a local guard to `RobotSchematics.OnDoubleClick(Mobile from)` so stale/null interaction state cannot play sounds, close/send gumps, or construct `RobotSchematicsGump` with a deleted source schematic.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

Schematic requirement setup, resource counters, `OnDragDrop` behavior, `TinkerLocation`, sound `0x54D`, `CloseGump`/`SendGump` behavior, `RobotSchematicsGump` content, `OnResponse` behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
