# SOURCE-BATCH-263 Reagent Jars Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-263`
- Candidate: `SB263-CAND-001`
- System: `Items:Trades / Reagent Jars`
- Source file: `Data/Scripts/Items/Trades/Resources/Reagents/Reagents.cs`
- Behavior: add stale/null/mobile/source-item guards to the three reagent jar `OnDoubleClick(Mobile from)` paths before backpack checks, reagent creation, overhead messaging, or deleting the source jar.

## Allowed Source Change

- In each reagent jar `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- For `reagents_magic_jar1`, treat a missing backpack as the existing backpack-use failure path.

## Must Stay Unchanged

- Wizard jar backpack requirement and failure message.
- Necromancer and alchemical jar interaction policy; do not add new backpack restrictions there.
- All reagent item types and counts.
- Private overhead success message.
- Source jar `Delete()` semantics.
- `AddNameProperties` text.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-263 Reagent Jars Guard Repair`
