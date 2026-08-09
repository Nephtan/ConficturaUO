# SOURCE-BATCH-378 DoorStuck Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-378`
- Candidate: `SB378-CAND-001`
- System: `Items:Doors / DoorStuck`
- File: `Data/Scripts/Items/Doors/DoorStuck.cs`

## Intended Source Change

Add local guards to `DoorStuck.OnDoubleClick(Mobile m)` and `DoorStuck.OnDoubleClickDead(Mobile m)` so stale/null interaction state cannot send the existing locked-door message through a null/deleted mobile or deleted source door.

Allowed changes:

- Return immediately when `m == null || m.Deleted || Deleted`.

## Must Stay Unchanged

Door item ID/name, locked-door message text, valid-mobile message behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
