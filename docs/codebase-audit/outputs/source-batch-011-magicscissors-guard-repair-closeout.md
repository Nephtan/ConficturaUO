# SOURCE-BATCH-011 Closeout: MagicScissors Guard Repair

Reviewed at: 2026-06-16T18:22:39.1548451-05:00

## Summary

`SOURCE-BATCH-011` implemented `SB007-CAND-005`, a non-gated `MagicScissors` interaction guard repair in `Data/Scripts/Items/Magical/MagicScissors.cs`.

The batch adds stale/null/backpack safety before mobile, source scissors, or target item state are dereferenced. It does not change the large appearance transform matrix, artifact policy, ownership rule, persistence, layout, or gated systems.

## Source Changes

- `MagicScissors.OnDoubleClick(Mobile from)` now returns immediately when the mobile is null or deleted.
- `MagicScissors.OnDoubleClick(Mobile from)` now uses the existing backpack-use failure when the scissors are deleted, the backpack is missing, or the scissors are outside the backpack.
- `WearTarget.OnTarget(Mobile from, object targeted)` now returns immediately when the mobile is null or deleted.
- `WearTarget.OnTarget(Mobile from, object targeted)` now uses the existing backpack-use failure when the source scissors are null, deleted, missing from the backpack, or the backpack is missing.
- `WearTarget.OnTarget(Mobile from, object targeted)` now avoids reading ownership, artifact, or transform state when the target is null, deleted, or not an item.

## Preserved Behavior

- Backpack-use message.
- Target range.
- Existing ownership rule.
- `MyServerSettings.AlterArtifact` policy.
- Every item ID/layer/name transform.
- Sound `0x248`.
- Success and failure messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/MagicScissors.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/MagicScissors.cs`: `0`
- Gated approvals crossed: `None`

## Verification

- Targeted source scan confirmed the new mobile, source scissors, backpack, and target item guards.
- Targeted source scan confirmed target range, ownership rule, artifact policy, sound `0x248`, success message, failure message, and serialization method presence remain intact.
- POST-BATCH-Y gate scan for `Data/Scripts/Items/Magical/MagicScissors.cs` returned `0`.
- Active overlay scan for `Data/Scripts/Items/Magical/MagicScissors.cs` returned `0`.
- Serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild passed with `0 Warning(s)` and `0 Error(s)`.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed; only expected line-ending warnings were reported by Git for edited files.
- Generated tracked root build artifacts were restored before staging.

## Result

- `SOURCE-BATCH-011` is complete.
- No gated behavior changed.
- The `source-batch-007-candidate-discovery.csv` implementation queue is exhausted.
- `SOURCE-BATCH-012+` remains pending a discovery-only pass for the next clean non-gated candidates.
