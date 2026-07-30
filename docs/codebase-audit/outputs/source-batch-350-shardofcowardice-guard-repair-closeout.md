# SOURCE-BATCH-350 ShardOfCowardice Guard Repair Closeout

## Summary

`SOURCE-BATCH-350` implemented `SB350-CAND-001`, a non-gated guard repair for `ShardOfCowardice`.

## Source Change

- File: `Data/Scripts/Quests/Shadowlords/ShardOfCowardice.cs`
- `ShardOfCowardice.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before reading `from.Backpack`.
- Deleted source shards, missing backpacks, and shards outside the backpack now use the existing backpack-use failure message before the existing success path.

## Preserved Behavior

- Item ID `0x3155`, `Name = "Shard of Cowardice"`, `Weight = 1.0`, and `Hue = 0x491`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You feel the cowardice emanating from this shard.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Shadowlords/ShardOfCowardice.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Shadowlords/ShardOfCowardice.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved constructor metadata, backpack-use message, success message, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-350` was committed as `a6777fbe` with `fix: guard ShardOfCowardice interactions`. `SOURCE-BATCH-351+` should run fresh candidate discovery because this deferred sibling queue is exhausted.
