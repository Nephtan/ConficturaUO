# SOURCE-BATCH-275 MaterialLiquifier Guard Repair Closeout

## Summary

`SOURCE-BATCH-275` implemented `SB275-CAND-001` in `Data/Scripts/Items/Technology/MaterialLiquifier.cs`.

`MaterialLiquifier.OnDoubleClick` now returns immediately for null/deleted mobiles or deleted liquifiers and uses the existing backpack-use failure when the mobile has no backpack. `OnDragDrop` now returns `false` for null/deleted stale drag-drop state and treats missing backpacks like the existing no-bottle destruction path. `GetColor` now returns `false` for null/deleted items, null/deleted mobiles, missing backpacks, or a missing/deleted bottle before bottle consumption. `MaterialLiquifierGump.OnResponse` now returns for null `NetState` or null/deleted mobiles before sound playback.

## Preserved Behavior

- Backpack-use failure `1060640`, gump open sound `0x54D`, drag-drop sound `0x55B`, dye success sound `0x23E`, and `RevealingAction`.
- Destroyed-item messages, `SpaceDyes` rejection, material-name matching, and material color/name mapping.
- Bottle consumption, `SpaceDyes` vial creation, vial `Name`, `Hue`, and `vialHue` assignment.
- Charge consumption, out-of-charges message `1019073`, source liquifier delete behavior, and dropped item delete semantics.
- Gump layout/text/sound response.
- `ItemCharges` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, material/reward policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit rows remain `Documented`, `FalsePositive`, and `ReviewedNoChange`; this batch did not change documentation policy, serialization, source layout, material/reward tuning, or gated behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/item/backpack/bottle/gump guards and preserved destroyed-item messages, charge consumption, dropped item delete, `SpaceDyes` rejection, bottle consumption, vial creation, gump send path, sounds, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented`, `FalsePositive`, and `ReviewedNoChange` rows remain nonblocking because source behavior and docs policy were untouched except stale/null safety.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes. The only gump-surface diff is the named stale response guard.
- Passed: changed-file scan found only `Data/Scripts/Items/Technology/MaterialLiquifier.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
