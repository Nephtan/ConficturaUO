# SOURCE-BATCH-100 Cotton Guard Repair Closeout

## Summary

`SOURCE-BATCH-100` implemented `SB099-CAND-002`, the `Cotton` guard repair from `source-batch-099-candidate-discovery.csv`.

`Data/Scripts/Items/Trades/Resources/Tailor/Cotton.cs` now guards stale/null/deleted mobile, source cotton, backpack, target-response, and dye-sender state before movable/access checks, range checks, spinning-wheel conversion, or dye hue assignment is evaluated.

## Preservation

The batch preserved:

- Movable gate, controlled `BaseCreature` access rule, prompt `502655`, inaccessible message `500447`, and backpack message `1042001`.
- Target range `3`.
- `Cotton.OnSpun` output amount `yarn.Amount * 6`, `SpoolOfThread` hue copy, yarn deletion, backpack placement, and message `1010577`.
- Valid-state dye behavior `Hue = sender.DyedHue`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Tailor/Cotton.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Cotton.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-099-candidate-discovery.csv` still imported successfully.
- Targeted source scan confirmed the new mobile/source-cotton/backpack/target/dye-sender guards and preserved accessibility, dye, spinning-wheel conversion amount, hue copy, deletion, backpack placement, and message evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-100` is closed. `SOURCE-BATCH-101+` remains pending the next concrete non-gated source target from `source-batch-099-candidate-discovery.csv`.
