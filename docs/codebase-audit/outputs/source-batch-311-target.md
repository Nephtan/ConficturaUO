# SOURCE-BATCH-311 LevelStave Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-311`
- Candidate: `SB311-CAND-001`
- Behavior: add stale/null/mobile/source-stave/backpack/gem guards to LevelStave interaction and helper paths.
- System: `Items:Magical:God / LevelStave`
- File: `Data/Scripts/Items/Magical/God/Weapons/LevelStave.cs`

## Allowed Source Change

Add guard-only checks to:

- `BaseLevelStave.OnDoubleClick(Mobile from)`
- `GemTarget.OnTarget(Mobile from, object targeted)`
- `BaseLevelStave.HasStaff(Mobile from)`

The guards may return early for null/deleted mobiles, deleted source staves, missing backpacks, deleted gem targets, or stale helper state before dereferences, target assignment, gem conversion, or helper backpack lookups.

## Must Stay Unchanged

- LevelStave/LevelSceptre constructors
- Combat/ranged behavior
- Leveling behavior
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
/goal SOURCE-BATCH-311 LevelStave Guard Repair
```
