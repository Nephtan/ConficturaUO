# SOURCE-BATCH-273 PortableSmelter Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-273`
- Candidate: `SB273-CAND-001`
- System: `Items:Technology / PortableSmelter`
- Source file: `Data/Scripts/Items/Technology/PortableSmelter.cs`
- Behavior: add stale/null/mobile/source-tool/backpack guards to `PortableSmelter.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, ore range checks, skill/resource checks, ingot conversion, or charge consumption.

## Allowed Source Change

- In `PortableSmelter.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `PortableSmelter.OnDoubleClick(Mobile from)`, use the existing backpack-use failure path when the mobile has no backpack or the smelter is no longer in the backpack.
- In `InternalTarget.OnTarget(Mobile from, object targeted)`, return immediately when `from == null || from.Deleted`.
- In `InternalTarget.OnTarget(Mobile from, object targeted)`, use the existing backpack-use failure path when the source smelter is null, deleted, or no longer in the backpack.

## Must Stay Unchanged

- `Movable` rule and backpack-use failure message.
- Target prompt text and target range `2`.
- `BaseOre`-only eligibility and ore distance message.
- Per-resource difficulty table, Mining threshold checks, and `CheckTargetSkill` behavior.
- Ore amount and `ItemID` handling, ingot conversion, ingot backpack placement, sound `0x208`, localized smelting/failure messages, and source ore deletion/amount mutation.
- Smelter charge consumption, broken-tool message, `SpaceJunkA` replacement, and smelter delete semantics.
- `Charges` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-273 PortableSmelter Guard Repair`
