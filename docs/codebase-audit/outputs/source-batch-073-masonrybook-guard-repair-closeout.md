# SOURCE-BATCH-073 MasonryBook Guard Repair Closeout

## Summary

`SOURCE-BATCH-073` completed fresh candidate discovery, selected `SB073-CAND-001`, and implemented the `MasonryBook` guard repair.

`Data/Scripts/Items/Trades/Specialized/MasonryBook.cs` now guards null/deleted mobiles, deleted source books, and missing backpacks before backpack, skill, learning flag, or delete state is evaluated. The non-PlayerMobile failure path now sends the existing failure message through `from` instead of dereferencing a null `PlayerMobile` cast.

## Preservation

The batch preserved:

- Backpack message `1042001`.
- Grandmaster Carpentry threshold `100.0`.
- PlayerMobile-only learning and `PlayerMobile.Masonry` success assignment.
- Existing failure/success messages and book `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Specialized/MasonryBook.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-073-candidate-discovery.csv` imported successfully.
- Targeted source scan confirmed the new mobile/source-book/backpack guards and preserved learning behavior evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-073` is closed. `SOURCE-BATCH-074+` remains pending the next concrete non-gated source target from `source-batch-073-candidate-discovery.csv`: `SB073-CAND-002` / `StoneMiningBook Guard Repair`.
