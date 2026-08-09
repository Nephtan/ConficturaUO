# SOURCE-BATCH-304 DurabilityPotion Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-304`
- Candidate: `SB304-CAND-001`
- System: `Items:Potions / DurabilityPotion`
- Source file: `Data/Scripts/Items/Potions/Special/DurabilityPotion.cs`
- Behavior: add stale/null/mobile/source-potion/target-item/backpack guards to `DurabilityPotion.Drink(Mobile m)`, `ConsumeCharge(DurabilityPotion potion, Mobile from)`, and `DurabilityTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, durability mutation, or potion consumption.

## Allowed Source Changes

- In `Drink(Mobile m)`, return immediately when `m == null || m.Deleted || Deleted`.
- In `ConsumeCharge(DurabilityPotion potion, Mobile from)`, return immediately when the source potion or mobile is null/deleted.
- In `DurabilityTarget.OnTarget`, return immediately when `from`, `m_From`, or `m_Potion` is null/deleted.
- In `DurabilityTarget.OnTarget`, treat deleted target armor/weapons or missing backpacks through the existing backpack-use failure message.

## Must Stay Unchanged

- One-tile source range check.
- Prompt and failure/success messages.
- `BaseArmor` and `BaseWeapon` eligibility.
- Target item backpack requirement.
- Durability cap check.
- `MaxHitPoints += 10` durability increase behavior.
- Sound `0x23E`.
- `RevealingAction`.
- Durability potion `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-304 DurabilityPotion Guard Repair`
