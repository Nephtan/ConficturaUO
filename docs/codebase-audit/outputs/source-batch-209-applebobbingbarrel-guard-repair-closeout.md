# SOURCE-BATCH-209 AppleBobbingBarrel Guard Repair Closeout

## Summary

`SOURCE-BATCH-209` implemented `SB206-CAND-004` in `Data/Scripts/Items/Gifts/Holiday/Halloween/Decorations/AppleBobbingBarrel.cs`.

`AppleBobbingBarrel.OnDoubleClick(Mobile from)` and `StopBobbing(object state)` now guard stale/null/deleted interaction state before reading mobile state, changing movement state, animating, playing sounds, or rewarding apples.

## Preserved Behavior

- Mounted rejection message, bobbing start message, valid `CantWalk` true/false transition, direction, animation IDs, sound `37`, delay duration `6` seconds, apple chance `.30`, apple reward, success/failure messages, public overhead messages, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Gifts/Holiday/Halloween/Decorations/AppleBobbingBarrel.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Candidate CSV import: passed for `SB206-CAND-004`.
- Targeted source scan: passed; new mobile/source guards are present and existing bobbing behavior remains present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Ready for focused `SOURCE-BATCH-209` commit.
