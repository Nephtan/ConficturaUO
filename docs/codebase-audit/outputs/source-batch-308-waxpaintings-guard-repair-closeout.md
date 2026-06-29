# SOURCE-BATCH-308 WaxPaintings Guard Repair Closeout

## Summary

`SOURCE-BATCH-308` implemented `SB308-CAND-001` from `source-batch-308-candidate-discovery.csv`.

The batch adds stale/null/mobile/source-painting/target guards to `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs` without crossing POST-BATCH-Y gates or changing wax painting naming behavior.

## Source Changes

- `WaxPainting.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the painting is deleted before backpack checks or target assignment.
- Missing backpacks in `OnDoubleClick` use the existing backpack-use failure message.
- `WaxTarget.OnTarget(Mobile from, object targeted)` now returns immediately for invalid mobiles and handles null/deleted/out-of-backpack source painting state before target inspection.
- Deleted mobile targets use the existing invalid-target failure message path.

## Preserved Behavior

- Painting prompt.
- Target range.
- Valid mobile body eligibility.
- Invalid-target message.
- Real-person naming behavior.
- Fictional naming behavior.
- Title generation and random name/title behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs`: `0`.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval was crossed.

## Verification

- Candidate CSV import: passed; `SB308-CAND-001` is recommended with gate hits `0` and active overlay rows `0`.
- Targeted source scan: passed; mobile, source painting, backpack, deleted mobile target, prompt, naming, invalid-target, and serializer-preservation evidence found.
- POST-BATCH-Y exact-file gate scan: passed with `0` hits.
- Active overlay exact-file scan: passed with `0` rows.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Changed-file scan: passed; no project/config/data files changed.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-309+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-308` is committed. `LargeWaxPot` remains a sibling candidate from this discovery, but it still requires exact-file gate/overlay preflight before source edits.
