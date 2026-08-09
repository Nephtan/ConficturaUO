# SOURCE-BATCH-328 HorseArmor Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-328`
- Candidate: `SB328-CAND-001`
- Behavior: add stale/null/mobile/source-armor/backpack/deleted-target guards to the horse barding interaction path.
- System: `Items:Armor / HorseArmor`
- File: `Data/Scripts/Items/Armor/HorseArmor.cs`

## Allowed Source Change

Add guard-only checks to `HorseArmor.OnDoubleClick(Mobile from)` and `HorseTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source armor, null source armor state, missing backpacks, source armor outside the backpack, or deleted target mobiles before dereferencing those values. Missing backpacks should use the existing backpack failure message; deleted target mobiles should use the existing horse-only failure message.

## Must Stay Unchanged

- Horse and `ZebraRiding` eligibility
- `ControlMaster` ownership rule
- `BaseMount` requirement
- Target range `8`
- Localized backpack message `1060640`
- Horse-only failure message
- Material hue mapping
- Stat, resistance, and skill mutation
- Sound `0x0AA`
- Source armor `Consume()` behavior
- `ArmorMaterial` persistence
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-328 HorseArmor Guard Repair
```
