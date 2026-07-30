# SOURCE-BATCH-387 MedicalRecord Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-387`
- Candidate: `SB387-CAND-001`
- System: `Items:Technology / MedicalRecord`
- File: `Data/Scripts/Items/Technology/MedicalRecord.cs`

## Intended Source Change

Add local guards to `MedicalRecord.OnDoubleClick(Mobile e)` and `MedicalRecordGump.OnResponse(NetState state, RelayInfo info)` so stale/null interaction state cannot dereference an invalid mobile, deleted record, or null response state before the existing gump and sound behavior runs.

Allowed changes:

- Return immediately from `OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Return immediately from `MedicalRecordGump.OnResponse` when `state == null`.
- In `MedicalRecordGump.OnResponse`, return immediately when `from == null || from.Deleted` before sound playback.

## Must Stay Unchanged

MedicalRecord item ID/name/weight/light/hue, randomized patient/planet setup, `MedicalRecordGump` text and layout, sound `0x54D` behavior, `DataPatient`/`DataPlanet` persistence, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
