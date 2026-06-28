# SOURCE-BATCH-094 FreshBrain Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-094`
- Candidate: `SB093-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-food guards to `FreshBrain.OnDoubleClick(Mobile from)`.
- System: `Items:Food / FreshBrain`
- Source file: `Data/Scripts/Items/Food/FreshBrain.cs`

## Fence Result

- POST-BATCH-Y gate hits for `FreshBrain.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Food/FreshBrain.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- Return immediately when `from` is null/deleted or the source food is deleted.
- Treat missing backpacks as the existing backpack-use failure using message `This must be in your backpack to eat.`

## Must Stay Unchanged

- `BaseRace.BrainEater` eligibility and rejection message.
- `Thirst = 20`, hunger delta, and full-state clamp.
- Consumption, random eat sound, human animation, and `AwardKarma(from, -50, true)` behavior.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
