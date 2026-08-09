# SOURCE-BATCH-101 Flax Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-101`
- Candidate: `SB099-CAND-003`
- Behavior: add stale/null/mobile/source-flax/backpack/target guards to `Flax.OnDoubleClick(Mobile from)` and `PickWheelTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Tailor Resources / Flax`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/Flax.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Flax.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Flax.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return immediately when the mobile or source flax is stale/null/deleted before the existing movable/access checks.
- In `PickWheelTarget.OnTarget`, return immediately when the mobile or cached flax is stale/null/deleted and keep missing-backpack state on the existing backpack-use failure path.

## Must Stay Unchanged

- Movable gate, controlled `BaseCreature` access rule, prompt `502655`, inaccessible message `500447`, and backpack message `1042001`.
- Target range `3`.
- `Flax.OnSpun` output amount `yarn.Amount * 6`, `SpoolOfThread` hue copy, yarn deletion, backpack placement, and message `1010577`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
