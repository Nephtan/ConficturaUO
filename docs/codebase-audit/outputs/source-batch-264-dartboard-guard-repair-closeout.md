# SOURCE-BATCH-264 DartBoard Guard Repair Closeout

## Summary

`SOURCE-BATCH-264` implemented `SB264-CAND-001` in `Data/Scripts/Items/Construction/Addons/DartBoard.cs`.

`DartBoard.OnDoubleClick(Mobile from)` and `DartBoard.Throw(Mobile from)` now return immediately for null/deleted mobiles or deleted source components before direction, range, LOS, weapon, animation, effect, sound, scoring, or reach-message paths.

## Preserved Behavior

- `NeedsWall` and `WallPosition` behavior.
- Direction selection.
- Range 4 and LOS rules.
- East/south valid throw directions.
- Knife requirement message.
- Animation choices.
- Moving effect and sound `0x238`.
- Random scoring thresholds and messages.
- Addon/deed behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file save-compat rows remain `IntentionalLegacy`; this batch did not edit serialization.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the new guards in `OnDoubleClick` and `Throw` and preserved wall placement, direction selection, range/LOS checks, knife requirement, animation/effect/sound, scoring messages, addon/deed classes, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `IntentionalLegacy` save-compat rows remain nonblocking because serialization was untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
