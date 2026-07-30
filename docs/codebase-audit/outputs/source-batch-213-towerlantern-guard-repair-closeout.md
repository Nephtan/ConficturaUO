# SOURCE-BATCH-213 TowerLantern Guard Repair Closeout

## Summary

`SOURCE-BATCH-213` implemented `SB212-CAND-002` in `Data/Scripts/Items/Construction/Lights/TowerLantern.cs`.

`TowerLantern.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before reading range or delegating to `BaseLight`.

## Preserved Behavior

- Range `1` reach check, localized reach message `1019045`, `base.OnDoubleClick(from)` delegation, `ISecurable` context menu behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Construction/Lights/TowerLantern.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; new mobile/source guard is present and existing range, reach-message, and `BaseLight` delegation behavior remain present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Ready for focused `SOURCE-BATCH-213` commit.
