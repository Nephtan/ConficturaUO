# SOURCE-BATCH-388 WetClothes Guard Repair Closeout

## Result

`SOURCE-BATCH-388` implemented `SB388-CAND-001`, a non-gated guard repair for `WetClothes`.

## Source Change

- File: `Data/Scripts/Items/Trades/Fishing/WetClothes.cs`
- Added an early return in `WetClothes.OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Preserved Behavior

WetClothes randomized adjective, clothing type, item ID, hue, name, weight, squeeze message, sound `0x026`, every dry clothing output mapping, `Utility.RandomDyedHue` output behavior, `AddToBackpack` behavior, source `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Fishing/WetClothes.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Trades/Fishing/WetClothes.cs`: `0`
- Gated approval crossed: `No`

## Verification

- Candidate CSV import: passed; one candidate row imported with required fields present.
- Targeted source scan: passed; the new guard and preserved WetClothes drying behavior are present.
- Exact-file POST-BATCH-Y scan: passed with `0` gate hits.
- Exact-file active overlay scan: passed with `0` active overlay rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed.
- Generated root build artifacts restored before staging: passed.

## Commit

`SOURCE-BATCH-388` commit hash pending. `SOURCE-BATCH-389+` should run fresh candidate discovery after `SOURCE-BATCH-388`.
