# SOURCE-BATCH-303 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-303+` selected the queued sibling repair potion guard repair from `SOURCE-BATCH-302` discovery.

## Recommended Target

- Candidate: `SB303-CAND-001`
- Batch: `SOURCE-BATCH-303`
- Source file: `Data/Scripts/Items/Potions/Special/RepairPotion.cs`
- Behavior: add stale/null/mobile/source-potion/target-item/backpack guards to `RepairPotion.Drink(Mobile m)`, `ConsumeCharge(RepairPotion potion, Mobile from)`, and `RepairTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, repair mutation, or potion consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Deferred Candidates

- `DurabilityPotion.cs`: zero gate/overlay hits, deferred as a sibling one-item batch.

## Result

`SB303-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change RepairPotion range, prompt/failure/success messages, `BaseArmor`/`BaseWeapon` eligibility, backpack target requirement, repair math, sound, `RevealingAction`, `Consume()` semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
