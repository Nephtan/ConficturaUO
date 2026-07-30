# SOURCE-BATCH-150 MovingBox Guard Repair Closeout

## Summary

Implemented `SB144-CAND-007` from `source-batch-144-candidate-discovery.csv`.

`Data/Scripts/Items/Containers/MovingBox.cs` now guards stale/null mobile and deleted source container state before assigning the dragging mobile as owner.

## Source Changes

- `MovingBox.OnDragLift(Mobile from)` now returns `false` for null/deleted mobiles or deleted source containers.

## Preserved Behavior

- Valid-state owner assignment when `owner == null` remains unchanged.
- Valid-state return remains `true`.
- Owner serialization layout/versioning remains unchanged.
- `IsEnabled` behavior, home/bank checks, open/drop restrictions, messages, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state are unchanged.

## Gate And Overlay Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Containers/MovingBox.cs`: `0`.
- Active overlay rows for `Data/Scripts/Items/Containers/MovingBox.cs`: `0`.

## Verification

- Targeted source scan confirmed the new mobile/source guard, invalid-state `false` return, preserved valid-state owner assignment, and preserved valid-state `true` return.
- Serializer diff scan showed no changes to `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read`.
- Forbidden-surface scan showed no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes beyond the named guard repair.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with Visual Studio MSBuild.
- `.\ConficturaServer.exe -compileonly -nocache` passed.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Generated tracked root build artifacts were restored before staging.

## Result

SOURCE-BATCH-150 is complete and ready to commit. `SOURCE-BATCH-151+` remains pending the next concrete non-gated source target from `source-batch-144-candidate-discovery.csv`.
