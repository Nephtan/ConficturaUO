# SOURCE-BATCH-013 Closeout: HydraTooth Guard Repair

Reviewed at: 2026-06-16T18:37:56.3208151-05:00

## Summary

`SOURCE-BATCH-013` implemented `SB012-CAND-002`, a non-gated `HydraTooth` interaction guard repair in `Data/Scripts/Items/Magical/HydraTooth.cs`.

The batch adds stale/null/backpack safety before mobile, design-context, backpack, spell construction, or cast state is evaluated. It does not change summon behavior, item identity, persistence, layout, or gated systems.

## Source Changes

- `HydraTooth.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `HydraTooth.OnDoubleClick(Mobile from)` now uses the existing pack failure when the tooth is deleted, the backpack is missing, or the tooth is outside the backpack.

## Preserved Behavior

- `Multis.DesignContext.Check(from)` behavior.
- Backpack localized message `1042001`.
- `SummonSkeletonSpell(from, this)` construction.
- `spell.Cast()` behavior.
- Item id, name, amount, and stacking behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/HydraTooth.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/HydraTooth.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile and backpack/deleted item guards.
- Targeted source scan confirmed `Multis.DesignContext.Check(from)`, `from.SendLocalizedMessage(1042001)`, `new SummonSkeletonSpell(from, this)`, `spell.Cast()`, item id, stacking, amount, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/HydraTooth.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/HydraTooth.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-013` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-014+` remains pending the next concrete non-gated source target.
