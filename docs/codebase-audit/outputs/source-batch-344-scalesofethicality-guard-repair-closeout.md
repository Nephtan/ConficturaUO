# SOURCE-BATCH-344 ScalesOfEthicality Guard Repair Closeout

## Summary

`SOURCE-BATCH-344` implemented `SB344-CAND-001`, a non-gated guard repair for `ScalesOfEthicality`.

## Source Change

- File: `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs`
- `ScalesOfEthicality.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles before reading `from.Backpack`.
- Deleted source scales, missing backpacks, and scales outside the backpack now use the existing backpack-use failure message before the existing success path.

## Preserved Behavior

- Item ID `0x573A`, `Name = "Scales of Ethicality"`, `Weight = 1.0`, and `Hue = 0x4AB`.
- Existing backpack-use failure message: `This must be in your backpack to use.`
- Existing success message: `You scale seems to weigh the ethics of the situation.`
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Serpents/ScalesOfEthicality.cs`: `0`
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

`SOURCE-BATCH-344` committed as `145facd9` with `fix: guard ScalesOfEthicality interactions`. `SOURCE-BATCH-345+` should continue with `BookOfTruth` if exact-file preflight remains clean.
