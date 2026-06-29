# SOURCE-BATCH-323 DDRelicStatue Guard Repair Closeout

## Summary

`SOURCE-BATCH-323` implemented `SB323-CAND-001` as a focused, non-gated `DDRelicStatue` guard repair.

## Source Change

- File: `Data/Scripts/Items/Relics/DDRelicStatue.cs`
- Added null/deleted mobile and deleted source relic guards before backpack checks, identification guidance, or item-id flip behavior.
- Added a missing-backpack guard to the existing backpack failure path.

## Preserved Behavior

- Identification guidance message
- Backpack-use failure message
- `RelicFlipID1`/`RelicFlipID2` item-id toggle behavior
- `RelicDescription` properties
- Statue generation and `MakeOriental` behavior
- `RelicGoldValue` and flip-id persistence
- Constructor/name randomization
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Relics/DDRelicStatue.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Relics/DDRelicStatue.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and failure-message, flip-id, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Relics/DDRelicStatue.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-324+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-323` is committed.
