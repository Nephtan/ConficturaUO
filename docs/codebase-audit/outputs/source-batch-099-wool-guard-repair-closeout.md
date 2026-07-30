# SOURCE-BATCH-099 Wool Guard Repair Closeout

## Summary

`SOURCE-BATCH-099` created `source-batch-099-candidate-discovery.csv`, selected `SB099-CAND-001`, and implemented the `Wool` guard repair.

`Data/Scripts/Items/Trades/Resources/Tailor/Wool.cs` now guards stale/null/deleted mobile, source wool, backpack, target-response, and dye-sender state before backpack checks, target assignment, spinning-wheel conversion, or dye hue assignment is evaluated.

## Preservation

The batch preserved:

- Backpack message `1042001` and spinning-wheel prompt `502655`.
- Target range `3`.
- `Wool.OnSpun` output amount `yarn.Amount * 3`, `DarkYarn` hue copy, yarn deletion, backpack placement, and message `1010576`.
- `TaintedWool.OnSpun` callback choice and tainted output behavior.
- Valid-state dye behavior `Hue = sender.DyedHue`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Tailor/Wool.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Wool.cs`: `0`
- No gated approval was crossed.

## Verification

- `source-batch-099-candidate-discovery.csv` imported successfully and recorded five zero-gate, zero-overlay tailoring resource candidates.
- Targeted source scan confirmed the new mobile/source-wool/backpack/target/dye-sender guards and preserved dye, spinning-wheel callback, conversion amount, hue copy, deletion, backpack placement, and message evidence.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed after the source edit.
- `.\ConficturaServer.exe -compileonly -nocache` passed after the source edit.
- `git diff --check` passed with the expected LF-to-CRLF warning for edited text files.
- Generated tracked root build artifacts were restored before staging.

## Result

`SOURCE-BATCH-099` is closed. `SOURCE-BATCH-100+` remains pending the next concrete non-gated source target from `source-batch-099-candidate-discovery.csv`.
