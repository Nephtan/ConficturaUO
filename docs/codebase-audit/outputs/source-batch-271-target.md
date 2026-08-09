# SOURCE-BATCH-271 SpaceDyes Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-271`
- Candidate: `SB271-CAND-001`
- System: `Items:Technology / SpaceDyes`
- Source file: `Data/Scripts/Items/Technology/SpaceDyes.cs`
- Behavior: add stale/null/mobile/source-dye/target-item guards to `SpaceDyes.OnDoubleClick(Mobile from)` and `DyeTarget.OnTarget(Mobile from, object targeted)` before backpack checks, target assignment, hue mutation, sound/revealing action, bottle return, or dye consumption.

## Allowed Source Change

- In `SpaceDyes.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `DyeTarget.OnTarget(Mobile from, object targeted)`, return immediately when `from == null || from.Deleted`.
- In `DyeTarget.OnTarget(Mobile from, object targeted)`, use the existing backpack-use failure `1060640` and return when the source dye is null, deleted, or no longer in the backpack.
- In `DyeTarget.OnTarget(Mobile from, object targeted)`, use the existing invalid-target message and return when the target item is null or deleted.

## Must Stay Unchanged

- Target range `1` and target prompt text.
- Backpack-use failure message `1060640`.
- In-pack target rule and message.
- Stackable item, item ID `8702`, and item ID `4011` rejection rules.
- Invalid-target messages.
- Hue assignment from `m_Dye.vialHue`.
- `RevealingAction`, sound `0x23E`, `Bottle` return, and source dye `Consume()` semantics.
- `vialHue` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit row remains `Documented`; it does not block this guard-only source repair.

## Ready Goal Shape

`/goal SOURCE-BATCH-271 SpaceDyes Guard Repair`
