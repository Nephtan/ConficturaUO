# SOURCE-BATCH-271 SpaceDyes Guard Repair Closeout

## Summary

`SOURCE-BATCH-271` implemented `SB271-CAND-001` in `Data/Scripts/Items/Technology/SpaceDyes.cs`.

`SpaceDyes.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source dyes before backpack checks and target assignment. `DyeTarget.OnTarget(Mobile from, object targeted)` now returns immediately for null/deleted mobiles, uses the existing backpack-use failure for stale source dye state, and uses the existing invalid-target path for null/deleted target items before hue mutation or source dye consumption.

## Preserved Behavior

- Target range `1` and target prompt text.
- Backpack-use failure message `1060640`.
- In-pack target rule and message.
- Stackable item, item ID `8702`, and item ID `4011` rejection rules.
- Invalid-target messages.
- Hue assignment from `m_Dye.vialHue`.
- `RevealingAction`, sound `0x23E`, `Bottle` return, and source dye `Consume()` semantics.
- `vialHue` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; this batch did not change documentation policy, serialization, source layout, or policy behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-dye/target-item guards and preserved target range, target prompt, backpack message, rejection rules, hue assignment, sound, revealing action, bottle return, source dye consumption, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented` row remains nonblocking because source behavior and docs policy were untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
