# SOURCE-BATCH-099 Wool Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-099`
- Candidate: `SB099-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-wool/target/dye-sender guards to `Wool.OnDoubleClick(Mobile from)`, `Wool.Dye(Mobile from, DyeTub sender)`, and `PickWheelTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Tailor Resources / Wool`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/Wool.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Wool.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/Wool.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- In `Dye(Mobile from, DyeTub sender)`, return `false` when the wool, mobile, or dye tub state is stale/null/deleted.
- In `OnDoubleClick(Mobile from)`, return immediately when the mobile or source wool is stale/null/deleted.
- Treat missing backpacks as the existing backpack-use failure using localized message `1042001`.
- In `PickWheelTarget.OnTarget`, return immediately when the mobile or cached wool is stale/null/deleted and keep missing-backpack state on the existing backpack-use failure path.

## Must Stay Unchanged

- Backpack message `1042001` and spinning-wheel prompt `502655`.
- Target range `3`.
- `Wool.OnSpun` output amount `yarn.Amount * 3`, `DarkYarn` hue copy, yarn deletion, backpack placement, and message `1010576`.
- `TaintedWool.OnSpun` callback choice and tainted output behavior.
- Valid-state dye behavior `Hue = sender.DyedHue`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
