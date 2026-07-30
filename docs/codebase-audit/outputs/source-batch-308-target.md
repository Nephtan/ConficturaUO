# SOURCE-BATCH-308 WaxPaintings Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-308`
- Candidate: `SB308-CAND-001`
- System: `Trades:Apiculture / WaxPaintings`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/WaxPaintings.cs`
- Behavior: add stale/null/mobile/source-painting/target guards to `WaxPainting.OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` before target assignment, mobile/title inspection, or painting naming mutation.

## Allowed Source Changes

- In `WaxPainting.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `WaxPainting.OnDoubleClick(Mobile from)`, treat missing backpacks as the existing backpack-use failure path.
- In `WaxTarget.OnTarget`, return immediately when `from` is null/deleted.
- In `WaxTarget.OnTarget`, treat null/deleted/out-of-backpack source painting state as the existing backpack-use failure path.
- In `WaxTarget.OnTarget`, treat deleted mobile targets as the existing invalid-target failure path.

## Must Stay Unchanged

- Painting prompt.
- Target range.
- Valid mobile body eligibility.
- Invalid-target message.
- Real-person naming behavior.
- Fictional naming behavior.
- Title generation and random name/title behavior.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-308 WaxPaintings Guard Repair`
