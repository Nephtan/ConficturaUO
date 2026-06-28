# SOURCE-BATCH-103 PolishBoneBrush Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-103`
- Candidate: `SB099-CAND-005`
- Behavior: add stale/null/mobile/source-brush/backpack/target guards to `PolishBoneBrush.OnDoubleClick(Mobile from)` and `PickBones.OnTarget(Mobile from, object targeted)`.
- System: `Items:Trades / Tailor Resources / PolishBoneBrush`
- Source file: `Data/Scripts/Items/Trades/Resources/Tailor/PolishBoneBrush.cs`

## Fence Result

- POST-BATCH-Y gate hits for `PolishBoneBrush.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Trades/Resources/Tailor/PolishBoneBrush.cs`: `0`
- No staff/access, command policy, balance/economy tuning, region/map, serializer migration, project/config/data, XML/config/data, or reorganization gate is crossed.

## Allowed Change

- In `OnDoubleClick(Mobile from)`, return immediately when the mobile or source brush is stale/null/deleted.
- Treat missing backpacks as the existing backpack-use failure using localized message `1042001`.
- In `PickBones.OnTarget`, return immediately when the mobile or cached brush is stale/null/deleted.
- In `PickBones.OnTarget`, require the source brush to still be in the user's backpack before polishing and keep missing-backpack state on the existing backpack-use failure path.
- Treat null/non-item/deleted targets as the existing invalid-target path without polishing or deleting anything.

## Must Stay Unchanged

- Backpack message `1042001`, target prompt `Which bones do you want to polish?`, and target range `1`.
- In-pack-only target bone rule, container rejection, valid bone/skull item IDs, output counts, success/failure messages, `PolishedBone`, `PolishedSkull`, revealing action, sound `0x04F`, and target bone deletion.
- Serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
