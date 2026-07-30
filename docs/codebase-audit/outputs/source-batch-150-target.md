# SOURCE-BATCH-150 MovingBox Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-150`
- Candidate: `SB144-CAND-007`
- Behavior: add stale/null/mobile/source-container guards to `MovingBox.OnDragLift`.
- System: `Items:Containers / MovingBox`
- File: `Data/Scripts/Items/Containers/MovingBox.cs`

## Fence Result

- POST-BATCH-Y gate hits for `MovingBox.cs`: `0`
- Active overlay rows for `MovingBox.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state owner assignment when `owner == null`.
- Valid-state return remains `true`.
- Owner serialization layout/versioning.
- `IsEnabled` behavior.
- Home/bank checks.
- Open/drop restrictions and messages.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-150 MovingBox Guard Repair`
