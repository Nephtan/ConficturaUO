# SOURCE-BATCH-272 Chainsaw Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-272`
- Candidate: `SB272-CAND-001`
- System: `Items:Technology / Chainsaw`
- Source file: `Data/Scripts/Items/Technology/Chainsaw.cs`
- Behavior: add stale/null/mobile/source-tool/backpack guards to `Chainsaw.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, log range checks, skill/resource checks, board conversion, or charge consumption.

## Allowed Source Change

- In `Chainsaw.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `Chainsaw.OnDoubleClick(Mobile from)`, use the existing backpack-use failure path when the mobile has no backpack or the chainsaw is no longer in the backpack.
- In `InternalTarget.OnTarget(Mobile from, object targeted)`, return immediately when `from == null || from.Deleted`.
- In `InternalTarget.OnTarget(Mobile from, object targeted)`, use the existing backpack-use failure path when the source chainsaw is null, deleted, or no longer in the backpack.

## Must Stay Unchanged

- `Movable` rule and backpack-use failure message.
- Target prompt text and target range `2`.
- `BaseLog`-only eligibility and log distance message.
- Per-resource difficulty table, Lumberjacking threshold checks, and `CheckTargetSkill` behavior.
- Log amount checks, board conversion, board backpack placement, sound `0x21C`, failure/ruin messages, and source log deletion/amount mutation.
- Chainsaw charge consumption, broken-tool message, `SpaceJunkA` replacement, and chainsaw delete semantics.
- `Charges` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-272 Chainsaw Guard Repair`
