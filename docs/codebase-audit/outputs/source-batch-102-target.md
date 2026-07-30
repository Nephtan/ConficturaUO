# SOURCE-BATCH-102 YarnsAndThreads Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-102`
- Candidate: `SB099-CAND-004`
- Behavior: add stale/null/mobile/source-cloth-material/backpack/target/dye-sender guards to `BaseClothMaterial.OnDoubleClick(Mobile from)`, `BaseClothMaterial.Dye(Mobile from, DyeTub sender)`, and `PickLoomTarget.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Tailor Resources / YarnsAndThreads`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/YarnsAndThreads.cs`

## Fence Result

- POST-BATCH-Y gate hits for `YarnsAndThreads.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/YarnsAndThreads.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- In `Dye(Mobile from, DyeTub sender)`, return `false` when the cloth material, mobile, or dye tub state is stale/null/deleted.
- In `OnDoubleClick(Mobile from)`, return immediately when the mobile or source material is stale/null/deleted.
- Treat missing backpacks as the existing backpack-use failure using localized message `1042001`.
- In `PickLoomTarget.OnTarget`, return immediately when the mobile or cached material is stale/null/deleted and keep missing-backpack state on the existing backpack-use failure path.

## Must Stay Unchanged

- Backpack message `1042001`, loom prompt `500366`, and target range `3`.
- Loom `Phase` math, material deletion, `BoltOfCloth` amount and hue copy, backpack placement, message `500368`, and incomplete-cloth note.
- Valid-state dye behavior `Hue = sender.DyedHue`.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
