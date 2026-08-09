# SOURCE-BATCH-237 Pillows Guard Repair Closeout

## Summary

`SOURCE-BATCH-237` created fresh candidate discovery and implemented `SB237-CAND-001` in `Data/Scripts/Items/Decorations/Pillows.cs`.

`Pillows.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before pillow item-ID flip state is evaluated or mutated.

## Preserved Behavior

- Flip ID fields, random construction item IDs and hues, `ItemID` toggle behavior, name, weight, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Decorations/Pillows.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; the `OnDoubleClick` guard exists and flip assignments, random hue, name, `Serialize`, and `Deserialize` remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff lines.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization diff lines.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Verified and ready for focused `SOURCE-BATCH-237` commit.
