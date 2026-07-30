# SOURCE-BATCH-263 Reagent Jars Guard Repair Closeout

## Summary

`SOURCE-BATCH-263` implemented `SB263-CAND-001` in `Data/Scripts/Items/Trades/Resources/Reagents/Reagents.cs`.

The three reagent jar `OnDoubleClick(Mobile from)` paths now return immediately for null/deleted mobiles or deleted source jars before backpack checks, reagent creation, overhead messaging, or deleting the jar. `reagents_magic_jar1` also treats missing backpacks as the existing backpack-use failure path.

## Preserved Behavior

- Wizard jar backpack requirement and failure message.
- Necromancer and alchemical jar interaction policy; no new backpack restriction was added there.
- All reagent item types and counts.
- Private overhead success message.
- Source jar `Delete()` semantics.
- `AddNameProperties` text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed three stale/null/mobile/source-item guards, the existing wizard jar backpack failure path, reagent creation count, private overhead messages, `Delete()` calls, `AddNameProperties`, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
