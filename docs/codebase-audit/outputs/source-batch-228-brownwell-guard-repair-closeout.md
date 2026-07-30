# SOURCE-BATCH-228 BrownWell Guard Repair Closeout

## Summary

`SOURCE-BATCH-228` created fresh candidate discovery and implemented `SB228-CAND-001` in `Data/Scripts/Items/Construction/Wells/brownwell.cs`.

`BrownWellPiece.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before range, thirst, message, or parent-addon state reads.

## Preserved Behavior

- Range 4 check, not-thirsty message, randomized drink messages, get-closer message, thirst set-to-20 behavior, addon layout/deed behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Construction/Wells/brownwell.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; confirmed the stale/null/mobile/source-component/parent-addon guard and preserved range check, thirst behavior, messages, parent serialization, and serialization methods.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with expected CRLF warnings only.
- Generated root build artifacts restoration: completed.

## Result

Verified and ready for commit.
