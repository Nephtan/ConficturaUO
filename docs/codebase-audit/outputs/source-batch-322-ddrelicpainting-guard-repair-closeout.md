# SOURCE-BATCH-322 DDRelicPainting Guard Repair Closeout

## Summary

`SOURCE-BATCH-322` implemented `SB322-CAND-001` as a focused, non-gated `DDRelicPainting` guard repair.

## Source Change

- File: `Data/Scripts/Items/Relics/DDRelicPainting.cs`
- Added null/deleted mobile and deleted source relic guards before backpack checks, muck cleanup, hue/name mutation, or item-id flip mapping.
- Added a missing-backpack guard to the existing backpack failure path.

## Preserved Behavior

- Backpack-use failure message
- Muck cleanup branch and message
- Hue reset/randomization
- Name replacement behavior
- Hard-coded painting `ItemID` mapping
- `RelicGoldValue` persistence
- Constructor/name randomization
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Relics/DDRelicPainting.cs`: `0`
- Exact-file active overlay rows for `Data/Scripts/Items/Relics/DDRelicPainting.cs`: `0`
- No staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization boundary was crossed.

## Verification

- Candidate CSV import: passed.
- Targeted source scan: passed; guards are present and failure-message, muck-cleanup, item-id-map, and serializer behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization change.
- Changed-file scan: passed; source diff is limited to `Data/Scripts/Items/Relics/DDRelicPainting.cs` plus audit artifacts.
- `Data/System/Source/Server.csproj` Debug/x86 Visual Studio MSBuild: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated tracked root build artifacts were restored before staging.

## Next Step

`SOURCE-BATCH-323+` requires fresh non-gated candidate discovery after `SOURCE-BATCH-322` is committed.
