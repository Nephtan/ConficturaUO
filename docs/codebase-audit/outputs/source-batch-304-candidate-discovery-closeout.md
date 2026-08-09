# SOURCE-BATCH-304 Candidate Discovery Closeout

## Summary

Fresh candidate discovery for `SOURCE-BATCH-304+` selected the queued sibling durability potion guard repair from `SOURCE-BATCH-303` discovery.

## Recommended Target

- Candidate: `SB304-CAND-001`
- Batch: `SOURCE-BATCH-304`
- Source file: `Data/Scripts/Items/Potions/Special/DurabilityPotion.cs`
- Behavior: add stale/null/mobile/source-potion/target-item/backpack guards to `DurabilityPotion.Drink(Mobile m)`, `ConsumeCharge(DurabilityPotion potion, Mobile from)`, and `DurabilityTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, durability mutation, or potion consumption.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated staff/access, command, balance/economy, region/map, serializer-migration, project/config/data, XML/config/data, or reorganization approval is crossed.

## Result

`SB304-CAND-001` is ready for a focused non-gated source batch. This is guard-only and does not change DurabilityPotion range, prompt/failure/success messages, `BaseArmor`/`BaseWeapon` eligibility, backpack target requirement, durability cap/math, sound, `RevealingAction`, `Consume()` semantics, serialization, staff/access behavior, balance/economy tuning, region/map policy, project/config/data files, XML/config/data files, or reorganization state.
