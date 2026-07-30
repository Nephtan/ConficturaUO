# SOURCE-BATCH-306 JarsOfWax Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-306`
- Candidate: `SB306-CAND-001`
- System: `Trades:Apiculture / JarsOfWax`
- Source file: `Data/Scripts/Trades/Apiculture/Craft/JarsOfWax.cs`
- Behavior: add stale/null/mobile/source-wax/target-item/backpack guards to the metal, leather, and instrument wax `OnDoubleClick(Mobile from)` and `WaxTarget.OnTarget(Mobile from, object targeted)` paths before target assignment, material/type checks, bonus mutation, Bottle return, or wax consumption.

## Allowed Source Changes

- In each wax `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In each wax `OnDoubleClick(Mobile from)`, treat missing backpacks as the existing backpack-use failure path.
- In each `WaxTarget.OnTarget`, return immediately when `from` is null/deleted.
- In each `WaxTarget.OnTarget`, treat null/deleted/out-of-backpack source wax state as the existing backpack-use failure path.
- In each `WaxTarget.OnTarget`, treat null/deleted target items as the existing invalid-target failure path.

## Must Stay Unchanged

- Metal, leather, and instrument wax prompts.
- Target range.
- Material/type eligibility.
- Already-good and invalid-target messages.
- Metal/leather durability bonus `+10` behavior.
- Instrument `UsesRemaining +20` behavior.
- Sound `0x242`.
- Bottle return.
- Wax `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-306 JarsOfWax Guard Repair`
