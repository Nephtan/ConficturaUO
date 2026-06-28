# SOURCE-BATCH-016 Closeout: MagicPigment Guard Repair

Reviewed at: 2026-06-16T18:52:04.9029004-05:00

## Summary

`SOURCE-BATCH-016` implemented `SB012-CAND-005`, a non-gated `MagicPigment` interaction guard repair in `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`.

The batch adds stale/null/backpack safety before mobile, source pigment, target ownership/backpack, or hue mutation state is evaluated. It does not change paint eligibility, hue behavior, persistence, layout, or gated systems.

## Source Changes

- `MagicPigment.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `MagicPigment.OnDoubleClick(Mobile from)` now uses the existing pack failure when the pigment is deleted, the backpack is missing, or the pigment is outside the backpack.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now returns immediately when the mobile is null or deleted.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now uses the existing pack failure when the source pigment is null, deleted, or no longer in the backpack.
- `DyeTarget.OnTarget(Mobile from, object targeted)` now treats deleted target item state as the existing cannot-paint failure.

## Preserved Behavior

- Randomized pigment names.
- Backpack-use message `1060640`.
- Target range.
- Backpack-as-target rule.
- In-backpack paint rule.
- Stackable and item ID exclusions.
- Hue assignment, including `0x2EF` reset.
- `RevealingAction`.
- Sound `0x23F`.
- Success/failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source pigment, backpack, and deleted-target guards.
- Targeted source scan confirmed backpack-as-target rule, in-backpack paint rule, stackable/item ID exclusions, hue assignment/reset, `RevealingAction`, sound `0x23F`, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/Dyes/MagicPigment.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-016` is complete.
- No gated behavior changed.
- The current `source-batch-012-candidate-discovery.csv` implementation queue is exhausted.
- `SOURCE-BATCH-017+` remains pending candidate discovery.
