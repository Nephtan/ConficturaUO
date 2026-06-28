# SOURCE-BATCH-106 UncutCloth Guard Repair Closeout

## Summary

Implemented `SB104-CAND-003` from `source-batch-104-candidate-discovery.csv`.

`Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs` now guards stale/null mobile, source folded cloth, dye tub, and scissors state before dye hue, visibility, or scissor-helper behavior is evaluated.

## Source Changes

- `Dye(Mobile from, DyeTub sender)` now returns `false` for null/deleted mobiles, null/deleted dye tubs, or deleted source folded cloth.
- `Scissor(Mobile from, Scissors scissors)` now returns `false` for null/deleted mobiles, null/deleted scissors, deleted source folded cloth, or existing cannot-see state.

## Preserved Behavior

- Valid dye state still assigns `Hue = sender.DyedHue`.
- Scissoring still calls `base.ScissorHelper(from, new Bandage(), 1)` and returns `true` for valid state.
- `OnSingleClick` labels and amount display are unchanged.
- `Deserialize` still resets `ItemID = 0x1765` and `Name = "folded cloth"`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/UncutCloth.cs`: `0`.

## Verification

- Targeted source scan confirmed the new `Dye` and `Scissor` guards.
- Targeted source scan confirmed valid-state dye, scissor-helper, single-click label, and deserialize item/name reset behavior remain present.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-106 is complete and ready to commit. The `source-batch-104-candidate-discovery.csv` implementation queue is exhausted; `SOURCE-BATCH-107+` requires fresh candidate discovery.
