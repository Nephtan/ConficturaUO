# SOURCE-BATCH-302 DuctTape Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-302`
- Candidate: `SB302-CAND-001`
- System: `Items:Technology / DuctTape`
- Source file: `Data/Scripts/Items/Technology/DuctTape.cs`
- Behavior: add stale/null/mobile/source-tape/target-item/backpack guards to `DuctTape.OnDoubleClick(Mobile m)`, `ConsumeCharge(DuctTape tape, Mobile from)`, and `RepairTarget.OnTarget(Mobile from, object targeted)` before range checks, target assignment, repair mutation, or tape consumption.

## Allowed Source Changes

- In `OnDoubleClick(Mobile m)`, return immediately when `m == null || m.Deleted || Deleted`.
- In `ConsumeCharge(DuctTape tape, Mobile from)`, return immediately when the source tape or mobile is null/deleted.
- In `RepairTarget.OnTarget`, return immediately when `from`, `m_From`, or `m_Tape` is null/deleted.
- In `RepairTarget.OnTarget`, treat deleted target armor/weapons or missing backpacks through the existing backpack-repair failure message.

## Must Stay Unchanged

- One-tile source range check.
- Prompt and failure/success messages.
- `BaseArmor` and `BaseWeapon` eligibility.
- Target item backpack requirement.
- Full-repair check.
- `MaxHitPoints` decrement and `HitPoints` assignment repair behavior.
- Sound `0x3E4`.
- `RevealingAction`.
- Duct tape `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-302 DuctTape Guard Repair`
