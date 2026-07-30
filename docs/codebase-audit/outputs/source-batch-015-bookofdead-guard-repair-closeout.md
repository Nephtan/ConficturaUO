# SOURCE-BATCH-015 Closeout: BookofDead Guard Repair

Reviewed at: 2026-06-16T18:47:37.0216166-05:00

## Summary

`SOURCE-BATCH-015` implemented `SB012-CAND-004`, a non-gated `BookofDead` interaction guard repair in `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`.

The batch adds stale/null/backpack safety before mobile, skills, follower state, resource consumption, or corpse creation state is evaluated. It does not change skill math, resource rules, summon behavior, persistence, layout, or gated systems.

## Source Changes

- `BookofDead.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `BookofDead.OnDoubleClick(Mobile from)` now uses the existing pack failure when the book is deleted, the backpack is missing, or the book is outside the backpack.

## Preserved Behavior

- Backpack localized message `1042001`.
- Necromancy threshold.
- Spiritualism scalar, siphon, and empowerment math.
- Follower requirement.
- Resource types and quantities.
- `ConsumeTotal` result messages.
- Corpse creation/control behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile and backpack/deleted item guards.
- Targeted source scan confirmed the Necromancy threshold, Spiritualism math, follower check, `ConsumeTotal`, corpse creation/control path, and serialization markers remain present.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Misc/Bodies/LivingDead/BookofDead.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-015` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-016+` remains pending the next concrete non-gated source target.
