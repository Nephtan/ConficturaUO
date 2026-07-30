# SOURCE-BATCH-324 Bola Guard Repair Closeout

## Summary

`SOURCE-BATCH-324` implemented `SB324-CAND-001` as a focused, non-gated `Bola` guard repair.

## Source Change

- File: `Data/Scripts/Items/Misc/Bola.cs`
- Added null/deleted mobile and deleted source bola guards before double-click backpack checks or throw setup.
- Added target callback guards for null/deleted mobiles, null/deleted source bolas, missing backpacks, and deleted mobile targets before target validation or combat checks.

## Preserved Behavior

- Pack-use failure message `1040019`
- Cooldown message `1049624`
- Already-used message `1049631`
- Hand, mount, and animal-form restrictions
- Target invalid/no-reason messages
- Target range `8` and `TargetFlags.Harmful`
- `Core.AOS` behavior
- `BeginAction` cooldown flow
- `CanBeHarmful` and `DoHarmful` behavior
- Bola `Consume()` behavior
- Animation and moving effect behavior
- `Timer.DelayCall` callbacks
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/Bola.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Misc/Bola.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and pack-use, cooldown, target, consume, timer, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Misc/Bola.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-325+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-324` is committed.
