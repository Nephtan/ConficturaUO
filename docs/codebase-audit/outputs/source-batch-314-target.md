# SOURCE-BATCH-314 JukaBow Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-314`
- Candidate: `SB314-CAND-001`
- Behavior: add stale/null/mobile/source-bow/backpack/gears guards to JukaBow modification paths.
- System: `Items:Weapons:Bows / JukaBow`
- File: `Data/Scripts/Items/Weapons/Bows/JukaBow.cs`

## Allowed Source Change

Add guard-only checks to:

- `JukaBow.OnDoubleClick(Mobile from)`
- `JukaBow.OnTargetGears(Mobile from, object targ)`

The guards may return early for null/deleted mobiles, deleted source bows, missing backpacks, null/deleted gears, or stale target state before dereferences, target assignment, gear consumption, hue mutation, or slayer assignment.

## Must Stay Unchanged

- `IsModified` behavior
- Backpack-use messages
- Bowcraft `100.0` threshold
- Target range/callback flow
- `Gears` requirement and consume semantics
- Hue `0x453`
- `Slayer = (SlayerName)Utility.Random(2, 25)`
- Success/failure messages
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-314 JukaBow Guard Repair
```
