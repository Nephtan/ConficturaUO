# SOURCE-BATCH-307 WaxSculptors Guard Repair Closeout

## Summary

`SOURCE-BATCH-307` implemented `SB307-CAND-001` from `source-batch-307-candidate-discovery.csv`.

The batch adds stale/null/mobile/source-sculptor/target guards and a safe self-target type check to `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs` without crossing POST-BATCH-Y gates or changing wax sculptor naming behavior.

## Source Changes

- `WaxSculptors.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the sculptor is deleted before backpack checks or target assignment.
- Missing backpacks in `OnDoubleClick` use the existing backpack-use failure message.
- `WaxTarget.OnTarget(Mobile from, object targeted)` now returns immediately for invalid mobiles and handles null/deleted/out-of-backpack source sculptor state before target inspection.
- Deleted mobile targets use the existing invalid-target failure message path.
- The self-target branch now checks `targeted is Item` before casting, so non-mobile/non-item targets use the existing invalid-target path.

## Preserved Behavior

- Sculptor prompt.
- Target range.
- Valid mobile body eligibility.
- Invalid-target message.
- Real-person naming behavior.
- Fictional naming behavior.
- Title generation and random name/title behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs`: `0`.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval was crossed.

## Verification

- Candidate CSV import: passed; `SB307-CAND-001` is recommended with gate hits `0` and active overlay rows `0`.
- Targeted source scan: passed; mobile, source sculptor, backpack, deleted mobile target, safe self-target type check, prompt, naming, invalid-target, and serializer-preservation evidence found.
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

`SOURCE-BATCH-308+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-307` is committed. `WaxPaintings` remains a sibling candidate from this discovery, but it still requires exact-file gate/overlay preflight before source edits.
