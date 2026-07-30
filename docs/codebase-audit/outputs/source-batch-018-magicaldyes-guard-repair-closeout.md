# SOURCE-BATCH-018 Closeout: MagicalDyes Guard Repair

Reviewed at: 2026-06-16T19:08:27.3028568-05:00

## Summary

`SOURCE-BATCH-018` implemented `SB017-CAND-002`, a non-gated `MagicalDyes` interaction guard repair in `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`.

The batch adds stale/null/backpack safety before mobile, source dye, target backpack, hue mutation, Bottle return, or dye consumption state is evaluated. It does not change dye eligibility, hue behavior, item return/consumption, persistence, layout, or gated systems.

## Source Changes

- `MagicalDyes.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `MagicalDyes.OnDoubleClick(Mobile from)` now uses the existing pack failure when the dye is deleted, the backpack is missing, or the dye is outside the backpack.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now returns immediately when the mobile is null or deleted.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now uses the existing pack failure when the source dye is null, deleted, or no longer in the backpack.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now treats deleted target item state as the existing cannot-dye failure.

## Preserved Behavior

- Randomized dye names and hues.
- Backpack-use message `1060640`.
- Target range.
- Backpack-as-target rule.
- In-backpack dye rule.
- Stackable and item ID exclusions.
- Hue assignment, including `0x2EF` reset.
- `RevealingAction`.
- Sound `0x23E`.
- Bottle return.
- `Consume()` semantics.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source dye, backpack, and deleted-target guards.
- Targeted source scan confirmed backpack-as-target rule, in-backpack dye rule, stackable/item ID exclusions, hue assignment/reset, `RevealingAction`, sound `0x23E`, Bottle return, `m_Dye.Consume()`, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/Dyes/MagicalDyes.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-018` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-019+` remains pending the next concrete non-gated source target.
