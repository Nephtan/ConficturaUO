# SOURCE-BATCH-303 RepairPotion Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-303`
- Candidate: `SB303-CAND-001`
- System: `Items:Potions / RepairPotion`
- Source file: `Data/Scripts/Items/Potions/Special/RepairPotion.cs`
- Behavior: add stale/null/mobile/source-potion/target-item/backpack guards to `RepairPotion.Drink(Mobile m)`, `ConsumeCharge(RepairPotion potion, Mobile from)`, and `RepairTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, repair mutation, or potion consumption.

## Allowed Source Changes

- In `Drink(Mobile m)`, return immediately when `m == null || m.Deleted || Deleted`.
- In `ConsumeCharge(RepairPotion potion, Mobile from)`, return immediately when the source potion or mobile is null/deleted.
- In `RepairTarget.OnTarget`, return immediately when `from`, `m_From`, or `m_Potion` is null/deleted.
- In `RepairTarget.OnTarget`, treat deleted target armor/weapons or missing backpacks through the existing backpack-repair failure message.

## Must Stay Unchanged

- One-tile source range check.
- Prompt and failure/success messages.
- `BaseArmor` and `BaseWeapon` eligibility.
- Target item backpack requirement.
- Full-repair check.
- `MaxHitPoints` decrement and `HitPoints` assignment repair behavior.
- Sound `0x23E`.
- `RevealingAction`.
- Repair potion `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-303 RepairPotion Guard Repair`
