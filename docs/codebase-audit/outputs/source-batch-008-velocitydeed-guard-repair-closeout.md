# SOURCE-BATCH-008 Closeout: VelocityDeed Guard Repair

Reviewed at: 2026-06-16T18:08:44.8478282-05:00

## Summary

`SOURCE-BATCH-008` implemented `SB007-CAND-002`, a non-gated `VelocityDeed` interaction guard repair in `Data/Scripts/Items/Magical/VelocityDeed.cs`.

The batch adds stale/null/backpack safety before deed and target state are dereferenced and before the deed is deleted. It does not change velocity amount, target eligibility, ownership policy, persistence, layout, or gated systems.

## Source Changes

- `VelocityDeed.OnDoubleClick(Mobile from)` now returns immediately when `from` is null or deleted.
- `VelocityDeed.OnDoubleClick(Mobile from)` now uses the existing backpack failure message when the deed is deleted, the backpack is missing, or the deed is not in the backpack.
- `VelocityTargetx.OnTarget(Mobile from, object target)` now returns immediately when `from` is null or deleted.
- `VelocityTargetx.OnTarget(Mobile from, object target)` now uses the existing cannot-add-velocity failure when the source deed is null, deleted, or no longer rooted to the mobile.
- `VelocityTargetx.OnTarget(Mobile from, object target)` now avoids mutating or deleting anything when the target is not a live `BaseRanged`.

## Preserved Behavior

- `BaseRanged`-only target eligibility.
- `+10` Velocity increment.
- Existing success and failure messages.
- Deed `RootParent` ownership rule.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/VelocityDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/VelocityDeed.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, deed, backpack, and live `BaseRanged` target guards.
- Targeted source scan confirmed `item.Velocity += 10`, success/failure messages, and `m_Deed.Delete()` remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/VelocityDeed.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/VelocityDeed.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-008` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-009+` remains pending the next concrete non-gated source target.
