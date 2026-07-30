# SOURCE-BATCH-272 Chainsaw Guard Repair Closeout

## Summary

`SOURCE-BATCH-272` implemented `SB272-CAND-001` in `Data/Scripts/Items/Technology/Chainsaw.cs`.

`Chainsaw.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted source chainsaws before movable and backpack checks. It also uses the existing backpack-use failure when the mobile has no backpack. `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately for null/deleted mobiles and uses the existing backpack-use failure for null/deleted/out-of-backpack source chainsaw state before evaluating logs, range, skills, board conversion, or charge consumption.

## Preserved Behavior

- `Movable` rule and backpack-use failure message.
- Target prompt text and target range `2`.
- `BaseLog`-only eligibility and log distance message.
- Per-resource difficulty table, Lumberjacking threshold checks, and `CheckTargetSkill` behavior.
- Log amount checks, board conversion, board backpack placement, sound `0x21C`, failure/ruin messages, and source log deletion/amount mutation.
- Chainsaw charge consumption, broken-tool message, `SpaceJunkA` replacement, and chainsaw delete semantics.
- `Charges` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; this batch did not change documentation policy, serialization, source layout, crafting/economy tuning, or policy behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-tool/backpack guards and preserved target prompt, target range, `BaseLog` eligibility, distance message, difficulty switch, Lumberjacking skill check, board placement, charge consumption calls, broken-tool message, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented` row remains nonblocking because source behavior and docs policy were untouched.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Technology/Chainsaw.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
