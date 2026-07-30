# SOURCE-BATCH-299 OrangePetals Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-299`
- Candidate: `SB299-CAND-001`
- System: `Trades:Gardening / OrangePetals`
- Source file: `Data/Scripts/Trades/Gardening/MiscItems/OrangePetals.cs`
- Behavior: add stale/null/mobile/source-orange-petals/context guards to `OrangePetals.CheckItemUse`, `OnDoubleClick(Mobile from)`, context helper methods, and `OrangePetalsTimer.OnTick()` before root-parent checks, context table access, effect messaging, timer stopping, or item consumption.

## Allowed Source Changes

- In `CheckItemUse(Mobile from, Item item)`, return `false` when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, use the existing RootParent requirement and localized message `1042038` when the orange petals are no longer rooted to `from`.
- In `AddContext`, `RemoveContext`, and `GetContext`, return safely for null mobile/context state.
- In `RemoveContext`, stop the context timer only when the timer is non-null.
- In `OrangePetalsTimer.OnTick`, send the wear-off message only when the stored mobile is non-null and not deleted.

## Must Stay Unchanged

- RootParent use requirement and backpack-use message `1042038`.
- Already-under-effect message `1061904`.
- Success message `1061905`.
- Sound `0x3B`.
- Five-minute effect timer duration.
- Wear-off message text.
- `AddContext`, `RemoveContext`, and `UnderEffect` behavior for valid mobiles.
- Orange petals `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-299 OrangePetals Guard Repair`
