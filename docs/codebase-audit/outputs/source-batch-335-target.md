# SOURCE-BATCH-335 RustyJunk Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-335`
- Candidate: `SB335-CAND-001`
- System: `Items:Trades / Fishing / RustyJunk`
- Source file: `Data/Scripts/Items/Trades/Fishing/RustyJunk.cs`
- Behavior: add stale/null/mobile/source-rusted/backpack guards to `RustyJunk.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command policy, balance/economy, region/map, serializer migration/layout, project/config/data, XML/config/data, or reorganization approval is crossed.

## Allowed Change

- Return immediately from `OnDoubleClick` when `from` is null or deleted.
- Use the existing backpack-use failure message when the rusty item is deleted, the mobile has no backpack, or the rusty item is not in the mobile's backpack.
- Return immediately from `InternalTarget.OnTarget` when `from` is null or deleted.
- Use the existing backpack-use failure message when the source rusty item is null, deleted, the mobile has no backpack, or the source rusty item is no longer in the mobile's backpack.

## Must Stay Unchanged

- Generated rusty item names, item IDs, hues, weights, and scrap iron property text.
- Backpack-use message: `This must be in your backpack to use.`
- Target prompt: `Select the forge to smelt this item.`
- Target range `2`.
- `DefBlacksmithy.IsForge(targeted)` requirement.
- `difficulty=50.0`, `minSkill=25.0`, and `maxSkill=75.0`.
- Mining skill threshold message and `CheckTargetSkill` behavior.
- `IronIngot(1)` creation, `ingot.Amount = weight`, and `AddToBackpack` behavior.
- Sound `0x208`.
- Success and failure messages.
- Source rusty item `Delete()` behavior.
- Serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, balance/economy tuning, region/map policy, and reorganization state.

## Ready Goal Shape

`SOURCE-BATCH-335 RustyJunk Guard Repair`
