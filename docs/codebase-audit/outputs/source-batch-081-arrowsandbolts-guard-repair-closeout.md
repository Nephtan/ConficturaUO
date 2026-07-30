# SOURCE-BATCH-081 ArrowsAndBolts Guard Repair Closeout

## Summary

`SOURCE-BATCH-081` completed fresh candidate discovery, selected `SB081-CAND-001`, and implemented the `ArrowsAndBolts` guard repair.

`Data/Scripts/Items/Explorers/ArrowsAndBolts.cs` now guards null/deleted mobiles, deleted source bundles, missing backpacks, and out-of-backpack source bundles before bundle splitting, overhead messages, or bundle deletion state is evaluated.

## Preservation

The batch preserved:

- Backpack failure message.
- `Arrow(100)`, `Arrow(1000)`, `Bolt(100)`, and `Bolt(1000)` quantities.
- `AddToBackpack` behavior.
- `PrivateOverheadMessage` text, hue, and message type.
- Bundle `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Explorers/ArrowsAndBolts.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Explorers/ArrowsAndBolts.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-081-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-bundle/backpack guards and preserved bundle quantity/message/delete evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-081` is closed. The `source-batch-081-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-082+` requires fresh candidate discovery.
