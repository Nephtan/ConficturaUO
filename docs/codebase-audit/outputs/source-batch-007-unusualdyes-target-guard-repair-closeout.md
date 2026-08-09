# SOURCE-BATCH-007 Closeout: UnusualDyes Target Guard Repair

Reviewed at: 2026-06-16T14:53:11.7853163-05:00

## Summary

`SOURCE-BATCH-007` implemented `SB007-CAND-001`, a non-gated `UnusualDyes` interaction guard repair in `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`.

The batch adds stale/null/backpack safety before dye interaction state is dereferenced, before dye color is read, before target mutation, and before source dye consumption. It does not change dye behavior, persistence, layout, policy, or gated systems.

## Source Changes

- `UnusualDyes.OnDoubleClick(Mobile from)` now returns immediately when `from` is null or deleted.
- `UnusualDyes.OnDoubleClick(Mobile from)` now uses the existing localized backpack-use failure `1060640` when the dye item is deleted, the backpack is missing, or the dye item is not in the backpack.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now returns immediately when `from` is null or deleted.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now uses the existing localized backpack-use failure `1060640` when the source dye is null, deleted, missing from the backpack, or the backpack is missing.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now routes null or deleted target items through the existing invalid-target message `500857`.
- The `m_Dye.DyeColor` read now occurs after source dye, mobile, and backpack guards.

## Preserved Behavior

- `DyeColor` persistence and serialization layout/versioning.
- Randomized jar names and hues.
- `DyeTub.Redyable` eligibility.
- `BlackDyeTub` rejection.
- `MagicPigment` hue assignment.
- `from.RevealingAction()`.
- Sound `0x23E`.
- `from.AddToBackpack(new Jar())`.
- `m_Dye.Consume()` semantics.
- Existing localized messages.
- Target eligibility except stale/deleted/null safety.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source dye, backpack, and deleted-target guards.
- Targeted source scan confirmed `DyeTub.Redyable`, `BlackDyeTub`, `MagicPigment`, `RevealingAction`, sound `0x23E`, `AddToBackpack(new Jar())`, and `m_Dye.Consume()` remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/Dyes/UnusualDyes.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-007` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-008+` remains pending a concrete non-gated source target.
