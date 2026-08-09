# SOURCE-BATCH-198 EObsidianPillar Guard Repair Closeout

## Summary

`SOURCE-BATCH-198` implemented `SB198-CAND-001` in `Data/Scripts/Items/Gifts/ShadowDeeds/EObsidianPillar.cs`.

`EObsidianPillarComponent.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source components before checking range.

## Preserved Behavior

- Out-of-range double-click still sends localized overhead message `1019045`.
- Valid in-range double-click remains a no-op.
- Range distance `2`, component item ID, addon layout, deed name, random item IDs, random hue, weight, deserialize item-id correction, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Gifts/ShadowDeeds/EObsidianPillar.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Candidate CSV import: passed; `SB198-CAND-001` imported with `PostBatchYGateHitCount=0` and `ActiveOverlayRows=0`.
- Targeted source scan: passed; mobile/source guard, range check, localized overhead message `1019045`, addon component, deed metadata, deserialize item-id correction, `Serialize`, and `Deserialize` remain present.
- Serializer diff scan: passed; zero-context diff showed no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes outside the named source/audit files.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache`: passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check`: passed with expected LF-to-CRLF checkout warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Verified and ready for commit.
