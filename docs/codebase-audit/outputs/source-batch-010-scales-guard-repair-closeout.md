# SOURCE-BATCH-010 Closeout: Scales Guard Repair

Reviewed at: 2026-06-16T18:18:35.5080479-05:00

## Summary

`SOURCE-BATCH-010` implemented `SB007-CAND-004`, a non-gated `Scales` interaction guard repair in `Data/Scripts/Items/Misc/Scales.cs`.

The batch adds stale/null safety before mobile, source scales, and target item state are dereferenced. It does not change weighing rules, text, persistence, layout, or gated systems.

## Source Changes

- `Scales.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null/deleted or the scales item is deleted.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now returns immediately when the mobile or source scales state is stale.
- `InternalTarget.OnTarget(Mobile from, object targeted)` now routes null or deleted target item state through the existing cannot-weigh failure message.

## Preserved Behavior

- Target range.
- Self-weigh rejection.
- `RootParent` awkward-location rule.
- Movable-object rule.
- Weight formatting.
- Existing messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Scales.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Scales.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source scales, and target item guards.
- Targeted source scan confirmed target range, self-weigh rejection, `RootParent` awkward-location rule, movable-object rule, and weight formatting remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/Scales.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/Scales.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-010` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-011+` remains pending the next concrete non-gated source target.
