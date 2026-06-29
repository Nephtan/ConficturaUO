# SOURCE-BATCH-232 UnknownLiquid Identification Guard Repair Closeout

## Summary

`SOURCE-BATCH-232` created fresh candidate discovery and implemented `SB232-CAND-001` in `Data/Scripts/Items/Unknown/UnknownLiquid.cs`.

`UnknownLiquid.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before movable, range, backpack-policy, skill, effect, reaction, or delete state reads.

## Preserved Behavior

- Movable rejection, range 3 check, `IdentifyItemsOnlyInPack` policy, tasting skill behavior, sound/animation behavior, liquid effect/reaction behavior, source item `Delete()` behavior, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Unknown/UnknownLiquid.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; confirmed the stale/null/mobile/source-item guard and preserved movable rejection, range check, backpack policy, skill/effect behavior, source delete behavior, and serialization methods.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with expected CRLF warnings only.
- Generated root build artifacts restoration: completed.

## Result

Verified and ready for commit.
