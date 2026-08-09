# SOURCE-BATCH-298 RedLeaves Guard Repair Closeout

## Summary

`SOURCE-BATCH-298` implemented `SB298-CAND-001` in `Data/Scripts/Trades/Gardening/MiscItems/RedLeaves.cs`.

`RedLeaves.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted red leaves before backpack checks or target assignment. `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately for null/deleted mobiles or source red leaves, treats missing backpacks through the existing backpack-use failure message, and treats null/deleted target items through the existing backpack-use failure path without consuming red leaves or mutating books.

## Preserved Behavior

- Backpack message `1042664`.
- Target prompt `1061907`.
- Book-only message `1061911`.
- Already-sealed message `1061909`.
- Success message `1061910`.
- `BaseBook` eligibility.
- `Writable = false` mutation.
- Red leaves `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-red-leaves/backpack/target-item guards and preserved backpack message, target prompt, book-only message, already-sealed message, success message, `BaseBook` eligibility, `Consume()` behavior, `Writable = false` mutation, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Trades/Gardening/MiscItems/RedLeaves.cs` as a tracked source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
