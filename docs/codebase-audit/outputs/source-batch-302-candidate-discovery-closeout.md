# SOURCE-BATCH-302 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-302+` selected one narrow non-gated repair consumable guard repair and queued two sibling potion files for later one-item batches.

## Recommended Target

- Candidate: `SB302-CAND-001`
- Batch: `SOURCE-BATCH-302`
- Source file: `Data/Scripts/Items/Technology/DuctTape.cs`
- Behavior: add stale/null/mobile/source-tape/target-item/backpack guards to `DuctTape.OnDoubleClick(Mobile m)`, `ConsumeCharge(DuctTape tape, Mobile from)`, and `RepairTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, repair mutation, or tape consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `RepairPotion.cs`: zero gate/overlay hits, deferred as a sibling one-item batch.
- `DurabilityPotion.cs`: zero gate/overlay hits, deferred as a sibling one-item batch.

## Result

`SB302-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change DuctTape range, prompt/failure/success messages, `BaseArmor`/`BaseWeapon` eligibility, backpack target requirement, repair math, sound, `RevealingAction`, `Consume()` semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
