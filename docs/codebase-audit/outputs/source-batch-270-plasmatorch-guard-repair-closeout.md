# SOURCE-BATCH-270 PlasmaTorch Guard Repair Closeout

## Summary

`SOURCE-BATCH-270` implemented `SB270-CAND-001` in `Data/Scripts/Items/Technology/PlasmaTorch.cs`.

`PlasmaTorch.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source torches before backpack checks and target assignment. `UnlockTarget.OnTarget(Mobile from, object targeted)` now returns immediately for null/deleted mobiles and uses the existing backpack-use failure for null/deleted source torches before evaluating lock/trap/door targets.

## Preserved Behavior

- Target range `1`, `CheckLOS = true`, and target prompt text.
- Backpack-use failure message `1060640`.
- BaseHouseDoor, BookBox, UnidentifiedArtifact, UnidentifiedItem, CurseItem, BaseDoor, and ILockable handling.
- Dungeon-door unlock behavior and TreasureMapChest behavior.
- Lock/trap mutation rules, lock level adjustment, picker assignment, and trap clearing.
- Messages, sound `0x227`, `RevealingAction`, and source torch `Consume()` semantics.
- Constructor metadata, serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; this batch did not change documentation policy, serialization, source layout, or policy behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-tool guards and preserved range/LOS, target prompt, backpack message, lock/trap/door handling markers, sound, revealing action, source torch consumption, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented` row remains nonblocking because source behavior and docs policy were untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
