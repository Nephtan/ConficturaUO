# SOURCE-BATCH-100 Cotton Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-100`
- Candidate: `SB099-CAND-002`
- Behavior: add stale/null/mobile/source-cotton/backpack/target/dye-sender guards to `Cotton.OnDoubleClick(Mobile from)`, `Cotton.Dye(Mobile from, DyeTub sender)`, and `PickWheelTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Tailor Resources / Cotton`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/Cotton.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Cotton.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Cotton.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- In `Dye(Mobile from, DyeTub sender)`, return `false` when the cotton, mobile, or dye tub state is stale/null/deleted.
- In `OnDoubleClick(Mobile from)`, return immediately when the mobile or source cotton is stale/null/deleted before the existing movable/access checks.
- In `PickWheelTarget.OnTarget`, return immediately when the mobile or cached cotton is stale/null/deleted and keep missing-backpack state on the existing backpack-use failure path.

## Must Stay Unchanged

- Movable gate, controlled `BaseCreature` access rule, prompt `502655`, inaccessible message `500447`, and backpack message `1042001`.
- Target range `3`.
- `Cotton.OnSpun` output amount `yarn.Amount * 6`, `SpoolOfThread` hue copy, yarn deletion, backpack placement, and message `1010577`.
- Valid-state dye behavior `Hue = sender.DyedHue`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
