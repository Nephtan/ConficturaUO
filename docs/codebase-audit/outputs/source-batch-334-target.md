# SOURCE-BATCH-334 RobotSheetMetal Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-334`
- Candidate: `SB334-CAND-001`
- Behavior: add stale/null/mobile/source-metal/backpack guards to the robot sheet metal smelting path.
- System: `Quests:Robots / RobotSheetMetal`
- File: `Data/Scripts/Quests/Robots/RobotSheetMetal.cs`

## Allowed Source Change

Add guard-only checks to `RobotSheetMetal.OnDoubleClick(Mobile from)`.

The guards may return early for null/deleted mobiles, deleted source metals, missing backpacks, or source metals outside the backpack before forge checks, skill reads, ingot creation, backpack add, sound behavior, or source metal deletion. Missing backpacks or stale source-metal state should use the existing backpack failure message when a valid mobile can receive it.

## Must Stay Unchanged

- Localized backpack message `1060640`
- Forge requirement and message
- Blacksmith skill threshold `50`
- Apprentice blacksmith failure message
- `IronIngot` creation
- `Amount * 3` conversion amount
- `AddToBackpack` behavior
- Sound `0x208`
- Success message
- Source metal `Delete` behavior
- Construction metadata
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-334 RobotSheetMetal Guard Repair
```
