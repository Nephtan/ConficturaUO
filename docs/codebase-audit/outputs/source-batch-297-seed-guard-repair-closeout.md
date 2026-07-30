# SOURCE-BATCH-297 Seed Guard Repair Closeout

## Summary

`SOURCE-BATCH-297` implemented `SB297-CAND-001` in `Data/Scripts/Trades/Gardening/Seed.cs`.

`Seed.OnSingleClick(Mobile from)` and `Seed.OnDoubleClick(Mobile from)` now return immediately for null/deleted mobiles or deleted seeds before label sends, backpack checks, or target assignment. `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately for null/deleted mobiles or seeds, treats missing backpacks through the existing backpack-use failure message, and treats deleted plant targets through the existing invalid-target message.

## Preserved Behavior

- Seed label clilocs and bright/type label formatting.
- Backpack message `1042664`.
- Target prompt `1061916`.
- Invalid-target message `1061919`.
- `PlantSeed(from, m_Seed)` forwarding.
- `PlantType`, `PlantHue`, and `ShowType` semantics.
- Random seed factories.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact overlays: `RB-06693` and `RB-06694` are fixed documentation-trace rows; no design/tuning changes were made.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-seed/backpack/target-plant guards and preserved seed labels, backpack message, target prompt, invalid-target message, `PlantSeed` forwarding, plant type/hue/show-type semantics, random seed factories, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows and only resolved docs-trace overlay evidence.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Trades/Gardening/Seed.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
