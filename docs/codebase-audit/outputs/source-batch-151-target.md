# SOURCE-BATCH-151 AlchemistPouch Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-151`
- Candidate: `SB144-CAND-008`
- Behavior: add stale/null/mobile/source-pouch guards to `AlchemistPouch.OnDragLift`.
- System: `Items:Containers / AlchemistPouch`
- File: `Data/Scripts/Items/Containers/AlchemistPouch.cs`

## Fence Result

- POST-BATCH-Y gate hits for `AlchemistPouch.cs`: `0`
- Active overlay rows for `AlchemistPouch.cs`: `0`
- No staff/access, command policy, balance/economy, region/map policy change, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Must Stay Unchanged

- Valid-state organize message.
- Valid-state return remains `base.OnDragLift(from)`.
- Potion restrictions.
- Gump behavior and hotbar behavior.
- Serialization layout/versioning.
- Namespace/type/file layout and project/config/data files.

## Ready Goal Shape

`/goal SOURCE-BATCH-151 AlchemistPouch Guard Repair`
