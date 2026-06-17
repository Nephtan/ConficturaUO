# SOURCE-BATCH-029 Closeout: Key Interaction Guard Repair

Reviewed at: 2026-06-16T20:14:30-05:00

## Summary

`SOURCE-BATCH-029` implemented `SB028-CAND-002`, a non-gated `Key` guard repair in `Data/Scripts/Items/Misc/Key.cs`.

The batch adds stale/null safety before mobile, source key, backpack, target item, lock/unlock, rename, copy, or key destruction state is evaluated. It preserves existing key behavior, persistence, layout, and gated systems.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before dereferencing backpack state.
- `OnDoubleClick(Mobile from)` now treats deleted keys, missing backpacks, or keys outside the backpack as the existing unreachable-key outcome.
- `RenamePrompt.OnResponse(Mobile from, string text)` now returns for null/deleted mobiles before prompt processing.
- `RenamePrompt.OnResponse(Mobile from, string text)` now treats null/deleted/out-of-backpack source keys as the existing unreachable-key outcome.
- `UnlockTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles before target processing.
- `UnlockTarget.OnTarget(Mobile from, object targeted)` now treats null/deleted/out-of-backpack source keys as the existing unreachable-key outcome.
- `UnlockTarget.OnTarget(Mobile from, object targeted)` now treats deleted target items as the existing cannot-unlock outcome before lock state is evaluated.
- `CopyTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles before target processing.
- `CopyTarget.OnTarget(Mobile from, object targeted)` now treats null/deleted/out-of-backpack source keys as the existing unreachable-key outcome.
- `CopyTarget.OnTarget(Mobile from, object targeted)` now treats deleted target keys as the existing not-a-key outcome before copy logic is evaluated.

## Preserved Behavior

- `KeyValue`, `Link`, and `MaxRange` behavior.
- `UseLocks` behavior.
- `RenamePrompt` flow.
- Unlock target behavior.
- Copy target behavior.
- Tinkering copy skill check.
- 10 percent key destruction chance.
- Existing messages.
- `CheckLOS = false` behavior.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Key.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Key.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source key, backpack, deleted target item, and deleted target key guards.
- Targeted source scan confirmed `KeyValue`, `Link`, `MaxRange`, `UseLocks`, `RenamePrompt`, `UnlockTarget`, `CopyTarget`, `CheckTargetSkill`, `Utility.RandomDouble() <= 0.1`, `CheckLOS = false`, key localized messages, `writer.Write`, and `reader.Read` markers remain present.
- POST-BATCH-Y gate scan returned `0`.
- Active overlay scan returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only the expected line-ending warning was reported by Git for the edited source file.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-029` is complete.
- No gated behavior changed.
- `SOURCE-BATCH-030+` remains pending the next concrete non-gated source target from `source-batch-028-candidate-discovery.csv`.
