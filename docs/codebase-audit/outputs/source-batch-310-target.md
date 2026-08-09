# SOURCE-BATCH-310 WizardStaff Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-310`
- Candidate: `SB310-CAND-001`
- Behavior: add stale/null/mobile/source-staff/backpack/gem guards to WizardStaff interaction and helper paths.
- System: `Items:Weapons / WizardStaff`
- File: `Data/Scripts/Items/Weapons/Marksman/WizardStaff.cs`

## Allowed Source Change

Add guard-only checks to:

- `BaseWizardStaff.OnDoubleClick(Mobile from)`
- `GemTarget.OnTarget(Mobile from, object targeted)`
- `BaseWizardStaff.HasStaff(Mobile from)`

The guards may return early for null/deleted mobiles, deleted source staffs, missing backpacks, deleted gem targets, or stale helper state before dereferences, target assignment, gem conversion, or helper backpack lookups.

## Must Stay Unchanged

- WizardStaff/WizardStick constructors
- Combat/ranged behavior
- `damageType`
- Ammo consumption and `MageEye` item identity
- Possession rule for valid backpack/equipped state
- Gem eligibility and conversion amount math
- `MageEye` creation
- Messages
- Sound `0x243`
- `RevealingAction`
- Gem delete semantics
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-310 WizardStaff Guard Repair
```
