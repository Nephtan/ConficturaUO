# SOURCE-BATCH-305 PowderOfTemperament Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-305`
- Candidate: `SB305-CAND-001`
- System: `Items:Bulk Order Rewards / PowderOfTemperament`
- Source file: `Data/Scripts/Items/Special/Bulk Order Rewards/Blacksmithy/PowderOfTemperament.cs`
- Behavior: add stale/null/mobile/source-powder/target-item/backpack guards to `OnSingleClick(Mobile from)`, `OnDoubleClick(Mobile from)`, and `InternalTarget.OnTarget(Mobile from, object targeted)` before label display, target assignment, durability mutation, or powder use decrement/delete.

## Allowed Source Changes

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, return immediately when the mobile is null/deleted or the source powder is deleted.
- In `OnDoubleClick(Mobile from)`, treat a missing backpack as the existing pack-use failure path.
- In `InternalTarget.OnTarget`, return immediately when `from` is null/deleted.
- In `InternalTarget.OnTarget`, treat null/deleted/used-up source powder state as the existing used-up failure path where a valid mobile exists.
- In `InternalTarget.OnTarget`, treat deleted target items or missing backpacks as the existing pack-use failure path.

## Must Stay Unchanged

- Durability label display.
- Target assignment.
- Pack-use message `1042001`.
- Used-up message `1049086`.
- `CanFortify` rejection message `1049083`.
- Success message `1049084`.
- Cannot-improve message `1049085`.
- `Core.AOS ? 255 : wearable.InitMaxHits` calculation.
- Bonus cap of `10`.
- Durability scaling/unscaling and 255 caps.
- `UsesRemaining` decrement and source powder delete semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-305 PowderOfTemperament Guard Repair`
