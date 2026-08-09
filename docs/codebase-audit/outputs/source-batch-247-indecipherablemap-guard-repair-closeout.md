# SOURCE-BATCH-247 IndecipherableMap Guard Repair Closeout

## Summary

`SOURCE-BATCH-247` created fresh candidate discovery and implemented `SB247-CAND-001` in `Data/Scripts/Items/Trades/Maps/IndecipherableMap.cs`.

`IndecipherableMap.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before sending the ruined-map message.

## Preserved Behavior

- Localized message `1070801`, label number `1070799`, random hue selection, `MapItem` inheritance, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Trades/Maps/IndecipherableMap.cs`: `0`
- Exact-file unresolved active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed. Guard and preserved behavior markers were found.
- Serializer diff scan: passed. No `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` diff lines were present.
- Forbidden-surface diff scan: passed. No command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, target, or reorganization diff lines were present.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with line-ending warnings only.
- Generated root build artifacts restoration: completed.

## Result

Verified and ready for focused commit.
