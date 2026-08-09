# SOURCE-BATCH-292 ShipwreckedItem Guard Repair Closeout

## Summary

`SOURCE-BATCH-292` implemented `SB292-CAND-001` in `Data/Scripts/Items/Trades/Fishing/Misc/ShipwreckedItem.cs`.

`ShipwreckedItem.OnSingleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted shipwrecked items before shipwreck label send. `ShipwreckedItem.Dye(Mobile from, DyeTub sender)` now returns `false` for null/deleted mobiles, dye tubs, or shipwrecked items before hue assignment or failure-message send.

## Preserved Behavior

- `LabelTo` clilocs and shipwreck label format.
- `AddNameProperties` clilocs and `ShipName` display.
- `ShipName` persistence.
- Eligible armor ItemID dye range.
- Successful dye behavior: `Hue = sender.DyedHue` and `return true`.
- Ineligible-item failure message: `from.SendLocalizedMessage(sender.FailMessage)` and `return false`.
- `IShipwreckedItem` behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item/dye-tub guards and preserved shipwreck label clilocs/format, `AddNameProperties` clilocs, `ShipName` persistence, eligible ItemID dye range, hue assignment, ineligible failure message, `IShipwreckedItem` behavior, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Trades/Fishing/Misc/ShipwreckedItem.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
