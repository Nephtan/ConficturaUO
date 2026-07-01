# SOURCE-BATCH-341 JokeBook Guard Repair Closeout

## Summary

`SOURCE-BATCH-341` implemented `SB341-CAND-001`, a non-gated guard repair for `JokeBook`.

## Source Change

- File: `Data/Scripts/Quests/Jester/JokeBook.cs`
- `JokeBook.OnDoubleClick(Mobile from)` now returns safely for null/deleted mobiles or deleted joke book items before the existing `PlayerMobile` joke/sound flow.

## Preserved Behavior

- Item ID `0x1A98`, `Weight = 1.0`, generated owner-name prefix, and `Hue = 0xAFF`.
- `PlayerMobile`-only behavior.
- `Utility.RandomMinMax(0, 8)` joke selection.
- Sound `from.Female ? 801 : 1073` and all existing speech strings.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Quests/Jester/JokeBook.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Quests/Jester/JokeBook.cs`: `0`
- No gated approval was crossed.

## Verification

- Candidate CSV imports successfully with `Import-Csv`.
- Targeted source scan confirms the new guard and preserved `PlayerMobile` gate, random selection, sound behavior, speech strings, constructor metadata, and serializer methods.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-341` is ready to commit as `fix: guard JokeBook interactions`. `SOURCE-BATCH-342+` should run fresh candidate discovery before any further source edits.
