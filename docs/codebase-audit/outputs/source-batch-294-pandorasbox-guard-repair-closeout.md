# SOURCE-BATCH-294 PandorasBox Guard Repair Closeout

## Summary

`SOURCE-BATCH-294` implemented `SB294-CAND-001` in `Data/Scripts/Items/Magical/Artifacts/Minor/PandorasBox.cs`.

`PandorasBox.ConsumeCharge(Mobile from)` and `PandorasBox.OnDoubleClick(Mobile from)` now return immediately for null/deleted mobiles or deleted boxes before charge decrement, replacement item creation, backpack add, or bank-box access.

## Preserved Behavior

- `Charges` property semantics.
- `ConsumeCharge` decrement behavior.
- Zero-charge localized message `1019073`.
- `MetalBox` replacement, hue `0x492`, backpack add, and source item `Delete`.
- Valid-state `BankBox.Open` behavior.
- Artifact label and property text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
- No backpack, range, ownership, access-level, or policy restrictions were added.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item guards and preserved charge decrement, zero-charge message, `MetalBox` replacement, hue, backpack add, source item delete, bank box open behavior, artifact labels/properties, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Magical/Artifacts/Minor/PandorasBox.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
