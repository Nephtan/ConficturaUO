# SOURCE-BATCH-221 UnidentifiedItem Guard Repair Closeout

## Summary

`SOURCE-BATCH-221` implemented `SB220-CAND-002` in `Data/Scripts/Items/Unknown/UnidentifiedItem.cs`.

`UnidentifiedItem.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before movability, backpack, range, skill-routing, or identification dispatch checks.

## Preserved Behavior

- Movable rejection message, backpack-use message, range message, `ArmsLore.IDItem` and `ItemIdentification.IDItem` routing, name properties, identification labels, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Unknown/UnidentifiedItem.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Targeted source scan: passed; confirmed the new mobile/source guard and preserved movability, backpack, range, ArmsLore/ItemIdentification dispatch, name property, and serialization methods.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache`: passed; runtime script compile completed successfully.
- `git diff --check`: passed with only expected CRLF working-copy warnings.
- Generated root build artifacts restoration: completed before staging.

## Result

Verified and ready for commit.
