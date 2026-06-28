# SOURCE-BATCH-110 HardCrystals Guard Repair Closeout

## Summary

Implemented `SB107-CAND-004` from `source-batch-107-candidate-discovery.csv`.

`Data/Scripts/Items/Trades/Resources/Blacksmithing/HardCrystals.cs` now guards stale/null mobile, deleted source crystals, and missing backpack state before anvil/forge, backpack, skill, ingot creation, sound, message, or delete state is evaluated.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source crystals.
- `OnDoubleClick(Mobile from)` now treats a missing backpack as the existing localized backpack-use failure.

## Preserved Behavior

- Backpack localized message `1060640` is unchanged.
- Forge requirement and forge failure message are unchanged.
- Blacksmith skill threshold `Value >= 50` and apprentice failure message are unchanged.
- All existing crystal-name to ingot mappings are unchanged.
- Valid smelting still copies `this.Amount` to the created ingot, adds it to the backpack, plays sound `0x208`, sends the existing success message text, and deletes the source crystals.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Blacksmithing/HardCrystals.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Blacksmithing/HardCrystals.cs`: `0`.

## Verification

- Targeted source scan confirmed the new mobile/source/backpack guards.
- Targeted source scan confirmed forge check, skill threshold, ingot amount copy, sound, existing success message text, and source deletion remain present.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-110 is complete and ready to commit. The `source-batch-107-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-111+` requires fresh candidate discovery.
