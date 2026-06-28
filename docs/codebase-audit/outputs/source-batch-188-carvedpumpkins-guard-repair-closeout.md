# SOURCE-BATCH-188 CarvedPumpkins Guard Repair Closeout

## Summary

`SOURCE-BATCH-188` implemented `SB188-CAND-001` in `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/CarvedPumpkins.cs`.

All twenty `CarvedPumpkin` `OnDoubleClick(Mobile from)` paths now return immediately when the mobile is null/deleted or the source pumpkin item is deleted, before reading `from.InRange`, sending the reach message, or delegating to `base.OnDoubleClick(from)`.

## Preserved Behavior

- Valid out-of-range behavior still sends localized reach message `1019045`.
- Valid in-range behavior still delegates to `base.OnDoubleClick(from)`.
- Lit/unlit item IDs, names, weight, light type, burning state, secure-level context-menu behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Gifts/Holiday/Halloween/Rewards/CarvedPumpkins.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Candidate CSV import: passed; `SB188-CAND-001` imported with `PostBatchYGateHitCount=0` and `ActiveOverlayRows=0`.
- Targeted source scan: passed; twenty `OnDoubleClick` methods, twenty stale/null/deleted guards, twenty range checks, twenty reach messages, and twenty `base.OnDoubleClick(from)` calls remain.
- Serializer diff scan: passed; zero-context diff showed no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump policy expansion, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes outside the named source/audit files.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache`: passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check`: passed with expected LF-to-CRLF checkout warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Verified and ready for commit.
