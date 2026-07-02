# SOURCE-BATCH-385 StatusBoard Guard Repair Closeout

## Result

`SOURCE-BATCH-385` implemented `SB385-CAND-001`, a non-gated guard repair for `StatusBoard`.

## Source Change

- File: `Data/Scripts/Items/Books/BulletinBoards/StatusBoard.cs`
- Added an early return in `StatusBoard.OnDoubleClick` when `e == null || e.Deleted || Deleted`.
- Added null guards for `sender` and `info` in `StatusGump.OnResponse`.
- Reordered `StatusGump.OnResponse` mobile validation so `from == null` is checked before `from.Deleted` and `from.Map`.

## Preserved Behavior

StatusBoard item ID/name/weight/hue, range requirement, localized too-far message `502138`, `StatusGump` layout, displayed account/client/server values, access-level visibility filtering, pagination behavior, button IDs, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Books/BulletinBoards/StatusBoard.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Books/BulletinBoards/StatusBoard.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guards and preserved status-board behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-385` source commit is pending. `SOURCE-BATCH-386+` should run fresh candidate discovery after `SOURCE-BATCH-385`.
