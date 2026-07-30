# SOURCE-BATCH-301 SmallBODTarget Guard Repair Closeout

## Summary

`SOURCE-BATCH-301` implemented `SB301-CAND-001` in `Data/Scripts/Trades/Bulk Orders/SmallBODTarget.cs`.

`SmallBODTarget.OnTarget(Mobile from, object targeted)` now returns safely for null/deleted mobiles, null/deleted source deeds, or missing backpacks before checking deed containment or forwarding to `SmallBOD.EndCombine`.

## Preserved Behavior

- Target range `18`.
- Existing silent failure behavior.
- `SmallBOD.IsChildOf(from.Backpack)` requirement.
- `m_Deed.EndCombine(from, targeted)` forwarding.
- Namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/source-deed/backpack guards and preserved target range and `EndCombine(from, targeted)` forwarding.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file active overlay scan found `0` rows.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, economy/reward, or reorganization changes.
- Passed: changed-file scan found only `Data/Scripts/Trades/Bulk Orders/SmallBODTarget.cs` as a tracked source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
