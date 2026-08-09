# SOURCE-BATCH-380 Safe Guard Repair Closeout

## Result

`SOURCE-BATCH-380` implemented `SB380-CAND-001`, a non-gated guard repair for `Safe`.

## Source Change

- File: `Data/Scripts/Items/Containers/Safe.cs`
- Added an early return in `OnDoubleClick` when `from == null || from.Deleted || Deleted`.
- Added a `false` return in `CheckAccess` when `m == null || m.Deleted || Deleted`.

## Preserved Behavior

Safe item ID/name/weight, movable-secured failure message, range/visibility/line-of-sight checks and messages, house secure-access policy, `BankBox.Open` behavior, context menu behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Containers/Safe.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Containers/Safe.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved secure-access and bank-box behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-380` committed as `e970df96`. `SOURCE-BATCH-381+` should run fresh candidate discovery after `SOURCE-BATCH-380`.
