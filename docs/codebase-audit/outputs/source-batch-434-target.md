# SOURCE-BATCH-434 MysticPack Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-434`
- Candidate: `SB434-CAND-001`
- System: `Magic:Mystic / MysticPack`
- File: `Data/Scripts/Magic/Mystic/MysticPack.cs`

## Behavior

Add early stale/null guards to `MysticPack.OnDoubleClick(Mobile from)`, `OnDragDropInto(Mobile from, Item dropped, Point3D p)`, and `OnDragDrop(Mobile from, Item dropped)` before existing owner/monk access checks and container behavior.

Allowed source changes:

- In `OnDoubleClick`, return immediately when `from == null || from.Deleted || Deleted`.
- In drag/drop paths, return `false` when `from == null || from.Deleted || Deleted || dropped == null || dropped.Deleted`.

## Must Stay Unchanged

Owner comparison, FistFighting `>= 100` threshold, `Server.Misc.GetPlayerInfo.isMonk(from)` check, `Open(from)` behavior, base drag/drop behavior, failure message `You cannot seem to open the rucksack.`, `Weight`, `MaxItems`, name/hue, owner persistence, weight reduction overrides, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Fence Result

Exact-file POST-BATCH-Y gate hits for `Data/Scripts/Magic/Mystic/MysticPack.cs`: `0`.

Exact-file active overlay rows for `Data/Scripts/Magic/Mystic/MysticPack.cs`: `0`.

No gated approval is crossed.
