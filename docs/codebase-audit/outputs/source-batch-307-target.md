# SOURCE-BATCH-307 WaxSculptors Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-307`
- Candidate: `SB307-CAND-001`
- System: `Trades:Apiculture / WaxSculptors`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/WaxSculptors.cs`
- Behavior: add stale/null/mobile/source-sculptor/target guards and a safe self-target type check to `WaxSculptors.OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` before target assignment, mobile/title inspection, sculptor naming mutation, or invalid self-target casts.

## Allowed Source Changes

- In `WaxSculptors.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `WaxSculptors.OnDoubleClick(Mobile from)`, treat missing backpacks as the existing backpack-use failure path.
- In `WaxTarget.OnTarget`, return immediately when `from` is null/deleted.
- In `WaxTarget.OnTarget`, treat null/deleted/out-of-backpack source sculptor state as the existing backpack-use failure path.
- In `WaxTarget.OnTarget`, treat deleted mobile targets as the existing invalid-target failure path.
- In `WaxTarget.OnTarget`, ensure the self-target branch only casts `targeted` when it is an `Item`.

## Must Stay Unchanged

- Sculptor prompt.
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

`/goal SOURCE-BATCH-307 WaxSculptors Guard Repair`
