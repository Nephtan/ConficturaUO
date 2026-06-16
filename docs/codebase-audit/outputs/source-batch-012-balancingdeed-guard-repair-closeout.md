# SOURCE-BATCH-012 Closeout: BalancingDeed Guard Repair

Reviewed at: 2026-06-16T18:32:00.1198582-05:00

## Summary

`SOURCE-BATCH-012` implemented `SB012-CAND-001`, a non-gated `BalancingDeed` interaction guard repair in `Data/Scripts/Items/Magical/BalancingDeed.cs`.

The batch adds stale/null/backpack safety before mobile, source deed, target ranged weapon, and deed deletion state are dereferenced. It does not change ranged target eligibility, balance flag behavior, persistence, layout, or gated systems.

## Source Changes

- `BalancingDeed.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `BalancingDeed.OnDoubleClick(Mobile from)` now uses the existing pack failure when the deed is deleted, the backpack is missing, or the deed is outside the backpack.
- `BalancingTarget.OnTarget(Mobile from, object target)` now returns immediately when the mobile is null or deleted.
- `BalancingTarget.OnTarget(Mobile from, object target)` now uses the existing pack failure when the source deed is null, deleted, or no longer rooted to the mobile.
- `BalancingTarget.OnTarget(Mobile from, object target)` now avoids mutation or deed deletion when the target is not a live `BaseRanged`.

## Preserved Behavior

- `BaseRanged`-only target eligibility.
- Existing `Balanced` flag check and assignment.
- `RootParent` ownership rule.
- Existing success and failure messages.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/BalancingDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/BalancingDeed.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source deed, backpack, and live `BaseRanged` target guards.
- Targeted source scan confirmed `item.Balanced == true`, `item.Balanced = true`, `item.RootParent != from`, and `m_Deed.Delete()` remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/BalancingDeed.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/BalancingDeed.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-012` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-013+` remains pending the next concrete non-gated source target.
