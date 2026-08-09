# SOURCE-BATCH-347 BellOfCourage Guard Repair Closeout

## Summary

`SOURCE-BATCH-347` implemented `SB347-CAND-001`, a non-gated guard repair for `BellOfCourage`.

## Source Change

- File: `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs`
- `BellOfCourage.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before reading `from.Backpack`.
- Deleted source bells, missing backpacks, and bells outside the backpack now use the existing backpack-use failure message before the existing sound and success path.

## Preserved Behavior

- Item ID `0x1C12`, `Name = "Bell of Courage"`, and `Weight = 1.0`.
- `m_Sounds` values `{ 0x505, 0x506, 0x507 }` and random sound behavior.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You ring the bell, producing a courageous melody.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Shadowlords/BellOfCourage.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved constructor metadata, sound behavior, backpack-use message, success message, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-347` was committed as `def0aa20` with `fix: guard BellOfCourage interactions`. `SOURCE-BATCH-348+` should continue with `ShardOfHatred` if exact-file preflight remains clean.
