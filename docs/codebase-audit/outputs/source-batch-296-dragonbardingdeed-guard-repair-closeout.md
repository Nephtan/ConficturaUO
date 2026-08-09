# SOURCE-BATCH-296 DragonBardingDeed Guard Repair Closeout

## Summary

`SOURCE-BATCH-296` implemented `SB296-CAND-001` in `Data/Scripts/Items/Deeds/DragonBardingDeed.cs`.

`DragonBardingDeed.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted deeds before backpack checks or target assignment. `DragonBardingDeed.OnTarget(Mobile from, object obj)` now returns immediately for null/deleted mobiles or deleted deeds before target-pet checks, and treats deleted targeted swamp dragons and missing backpacks through existing failure-message paths.

## Preserved Behavior

- Target prompt `1053024`.
- Backpack messages `1042001` and `1060640`.
- Invalid target message `1053025`.
- Ownership message `1053026`.
- Success message `1053027`.
- Swamp dragon eligibility and ownership rules.
- `BardingExceptional`, `BardingCrafter`, `BardingHP`, `BardingResource`, `HasBarding`, and `Hue` assignments.
- Deed `Delete` behavior.
- Craft metadata.
- Serialization layout/versioning, including current version 1 write/read order and version 0 legacy read.
- Constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact overlay: `RB-00643` is `IntentionalLegacy/Reviewed`; serialization was not changed.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-deed/backpack/target-pet guards and preserved target prompt, backpack messages, invalid target and ownership messages, success message, barding assignments, deed delete, craft metadata, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows and only resolved `RB-00643` overlay evidence.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Items/Deeds/DragonBardingDeed.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
