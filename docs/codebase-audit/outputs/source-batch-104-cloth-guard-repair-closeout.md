# SOURCE-BATCH-104 Cloth Guard Repair Closeout

## Summary

Implemented `SB104-CAND-001` from `source-batch-104-candidate-discovery.csv`.

`Data/Scripts/Items/Trades/Resources/Tailor/Cloth.cs` now guards stale/null mobile, source cloth, dye tub, and scissors state before dye hue assignment, fold placement/delete, visibility, or scissor-helper behavior is evaluated.

## Source Changes

- `Dye(Mobile from, DyeTub sender)` now returns `false` for null/deleted mobiles, null/deleted dye tubs, or deleted source cloth.
- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles or deleted source cloth before creating folded cloth and deleting the source.
- `Scissor(Mobile from, Scissors scissors)` now returns `false` for null/deleted mobiles, null/deleted scissors, deleted source cloth, or existing cannot-see state.

## Preserved Behavior

- Valid dye state still assigns `Hue = sender.DyedHue`.
- Folding still creates `new UncutCloth(Amount)`, copies hue, calls `from.AddToBackpack(cloth)`, sends `You fold up the cloth.`, and deletes the source cloth.
- Scissoring still calls `base.ScissorHelper(from, new Bandage(), 1)` and returns `true` for valid state.
- `OnSingleClick` label behavior is unchanged.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Tailor/Cloth.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Cloth.cs`: `0`.
- `Hides.cs` remained excluded because it has one `SafeNoChange` overlay row.

## Verification

- `source-batch-104-candidate-discovery.csv` imports successfully with `Import-Csv`.
- Targeted source scan confirmed the new `Dye`, `OnDoubleClick`, and `Scissor` guards.
- Targeted source scan confirmed valid-state dye, fold, delete, single-click, and scissor-helper behavior remain present.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-104 is complete and ready to commit. `SOURCE-BATCH-105+` remains pending `SB104-CAND-002` / BoltOfCloth Guard Repair from `source-batch-104-candidate-discovery.csv`.
