# SOURCE-BATCH-290 BoltOfCloth Guard Repair Closeout

## Summary

`SOURCE-BATCH-290` implemented `SB290-CAND-001` in `Data/Scripts/Items/Trades/Resources/Tailor/BoltOfCloth.cs`.

`BoltOfCloth.OnSingleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted cloth bolts before localized cloth-quantity label packet send.

## Preserved Behavior

- Localized label numbers and amount multiplier.
- `MessageLocalized` packet values.
- `ICommodity` behavior.
- Existing `Dye` and `Scissor` behavior and guards.
- Construction metadata and stack behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item guard and preserved localized label numbers, amount multiplier, `MessageLocalized` packet values, `ICommodity` members, existing `Dye`/`Scissor` behavior, construction metadata, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Trades/Resources/Tailor/BoltOfCloth.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
