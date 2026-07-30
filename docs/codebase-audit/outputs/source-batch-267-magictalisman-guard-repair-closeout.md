# SOURCE-BATCH-267 MagicTalisman Guard Repair Closeout

## Summary

`SOURCE-BATCH-267` implemented `SB267-CAND-001` in `Data/Scripts/Items/Magical/MagicTalisman.cs`.

`MagicTalisman.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source talismans before sending the worn-slot message.

## Preserved Behavior

- Randomized talisman names, item IDs, hues, and skill bonus assignments.
- `Resource`, `Layer.Talisman`, and `Weight` setup.
- Worn-slot message text: `Trinkets are worn in the upper right slot.`
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `SafeNoChange`; this batch did not change serialization, source layout, or policy behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-item guard and preserved the worn-slot message and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `SafeNoChange` row remains nonblocking because source behavior and serialization policy were untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
