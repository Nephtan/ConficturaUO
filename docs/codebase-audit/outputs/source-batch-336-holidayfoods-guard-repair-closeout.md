# SOURCE-BATCH-336 HolidayFoods Guard Repair Closeout

## Summary

`SOURCE-BATCH-336` implemented `SB336-CAND-001`, a non-gated guard repair for `HolidayFoods`.

## Source Change

- File: `Data/Scripts/Items/Special/Holiday/HolidayFoods.cs`
- `CandyCane.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted candy canes before backpack/range checks, sound/animation, toothache timer updates, messages, or source deletion.
- `GingerBreadCookie.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted cookies before backpack/range checks, random message selection, base food consumption, or localized messaging.

## Preserved Behavior

- CandyCane item IDs, `Stackable=false`, and `LootType=Blessed`.
- GingerBreadCookie item IDs, `Stackable=false`, and `LootType=Blessed`.
- Backpack-or-range eligibility: `IsChildOf(from.Backpack) || from.InRange(this, 1)`.
- CandyCane sound `0x3a + Utility.Random(3)`, animation, toothache dictionary/timer behavior, message `1077387`, and source `Delete()` behavior.
- GingerBreadCookie random message table, `base.OnDoubleClick(from)` consumption path, and `SendLocalizedMessageTo(from, result)` behavior.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/Holiday/HolidayFoods.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Special/Holiday/HolidayFoods.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms both new guards and preserved backpack-or-range eligibility, candy sound/animation, toothache update, candy message, cookie base consumption path, cookie message path, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-336` committed as `f025916b` with `fix: guard HolidayFoods interactions`. `SOURCE-BATCH-337+` should run fresh candidate discovery before any further source edits.
