# SOURCE-BATCH-312 GiftStave Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-312`
- Candidate: `SB312-CAND-001`
- Behavior: add stale/null/mobile/source-stave/backpack/gem guards to GiftStave interaction and helper paths.
- System: `Items:Magical:Gifts / GiftStave`
- File: `Data/Scripts/Items/Magical/Gifts/Weapons/GiftStave.cs`

## Allowed Source Change

Add guard-only checks to:

- `BaseGiftStave.OnDoubleClick(Mobile from)`
- `GemTarget.OnTarget(Mobile from, object targeted)`
- `BaseGiftStave.HasStaff(Mobile from)`

The guards may return early for null/deleted mobiles, deleted source staves, missing backpacks, deleted gem targets, or stale helper state before dereferences, target assignment, gem conversion, or helper backpack lookups.

## Must Stay Unchanged

- GiftStave/GiftSceptre constructors
- Combat/ranged behavior
- Gift identity
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
/goal SOURCE-BATCH-312 GiftStave Guard Repair
```
