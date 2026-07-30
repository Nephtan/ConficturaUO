# SOURCE-BATCH-361 PaganArtifact Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-361`
- Candidate: `SB361-CAND-001`
- System: `Quests:Pagan / PaganArtifact`
- File: `Data/Scripts/Quests/Pagan/PaganArtifact.cs`

## Intended Source Change

Add local guards to `PaganArtifact.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference `from.Backpack`, play sounds, close/send gumps, or construct `PaganArtifactGump`.

Allowed changes:

- Return immediately when `from == null || from.Deleted || Deleted`.
- Treat `from.Backpack == null` the same as the existing out-of-backpack failure using localized message `1060640`.

## Must Stay Unchanged

Artifact setup, random item/color/name behavior, `PaganItem`, `PaganColor`, `PaganName`, `PaganPoints`, localized message `1060640`, sound `0x2D`, `CloseGump`/`SendGump` behavior, `PaganArtifactGump.OnResponse`, reward point behavior, serialized field order, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
