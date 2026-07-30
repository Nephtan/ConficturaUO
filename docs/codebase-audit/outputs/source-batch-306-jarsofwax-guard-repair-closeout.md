# SOURCE-BATCH-306 JarsOfWax Guard Repair Closeout

## Summary

`SOURCE-BATCH-306` implemented `SB306-CAND-001` from `source-batch-306-candidate-discovery.csv`.

The batch adds stale/null/mobile/source-wax/target-item/backpack guards to `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs` without crossing POST-BATCH-Y gates or changing wax item-family behavior.

## Source Changes

- `JarsOfWaxMetal.OnDoubleClick`, `JarsOfWaxLeather.OnDoubleClick`, and `JarsOfWaxInstrument.OnDoubleClick` now return immediately when the mobile is null/deleted or the source wax is deleted before backpack checks or target assignment.
- Missing backpacks in those `OnDoubleClick` paths use the existing backpack-use failure message.
- Each nested `WaxTarget.OnTarget` now returns immediately for invalid mobiles and handles null/deleted/out-of-backpack source wax state before target inspection.
- Null/deleted target items use the existing invalid-target failure message path before material/type checks, bonus mutation, Bottle return, or wax consumption.

## Preserved Behavior

- Metal, leather, and instrument wax prompts.
- Target range.
- Material/type eligibility.
- Already-good and invalid-target messages.
- Metal/leather durability bonus `+10` behavior.
- Instrument `UsesRemaining +20` behavior.
- Sound `0x242`.
- Bottle return.
- Wax `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs`: `0`.
- Exact-file active overlay rows for `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs`: `0`.
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval was crossed.

## Verification

- Candidate CSV import: passed; `SB306-CAND-001` is recommended with gate hits `0` and active overlay rows `0`.
- Targeted source scan: passed; mobile, wax, target, backpack, prompts, bonus increments, instrument uses, Bottle return, consume, sound, and serializer-preservation evidence found.
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

`SOURCE-BATCH-307+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-306` is committed.
