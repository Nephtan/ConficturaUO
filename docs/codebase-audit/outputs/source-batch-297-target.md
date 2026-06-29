# SOURCE-BATCH-297 Seed Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-297`
- Candidate: `SB297-CAND-001`
- System: `Trades:Gardening / Seed`
- Source file: `Data/Scripts/Trades/Gardening/Seed.cs`
- Behavior: add stale/null/mobile/source-seed/backpack/target-plant guards to `Seed.OnSingleClick(Mobile from)`, `Seed.OnDoubleClick(Mobile from)`, and `InternalTarget.OnTarget(Mobile from, object targeted)` before label sends, target assignment, or `PlantSeed` forwarding.

## Allowed Source Changes

- In `OnSingleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1042664`.
- In `InternalTarget.OnTarget`, return immediately when `from == null || from.Deleted || m_Seed == null || m_Seed.Deleted`.
- In `InternalTarget.OnTarget`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1042664`.
- In `InternalTarget.OnTarget`, treat a deleted `PlantItem` target as the existing invalid-target path with localized message `1061919`.

## Must Stay Unchanged

- Seed label clilocs and bright/type label formatting.
- Backpack message `1042664`.
- Target prompt `1061916`.
- Invalid-target message `1061919`.
- `PlantSeed(from, m_Seed)` forwarding.
- `PlantType`, `PlantHue`, and `ShowType` semantics.
- Random seed factories.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact overlays: `RB-06693` and `RB-06694` are fixed documentation-trace rows; no design/tuning changes are allowed in this batch.

## Ready Goal Shape

`/goal SOURCE-BATCH-297 Seed Guard Repair`
