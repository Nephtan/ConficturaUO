# SOURCE-BATCH-387 MedicalRecord Guard Repair Closeout

## Result

`SOURCE-BATCH-387` implemented `SB387-CAND-001`, a non-gated guard repair for `MedicalRecord`.

## Source Change

- File: `Data/Scripts/Items/Technology/MedicalRecord.cs`
- Added an early return in `MedicalRecord.OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Added null/deleted response guards in `MedicalRecordGump.OnResponse` before sound playback.

## Preserved Behavior

MedicalRecord item ID/name/weight/light/hue, randomized patient/planet setup, `MedicalRecordGump` text and layout, sound `0x54D` behavior, `DataPatient`/`DataPlanet` persistence, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Technology/MedicalRecord.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Technology/MedicalRecord.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved medical record gump behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-387` committed as `164d39cf`. `SOURCE-BATCH-388+` should run fresh candidate discovery after `SOURCE-BATCH-387`.
