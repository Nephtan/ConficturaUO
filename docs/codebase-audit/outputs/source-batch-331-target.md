# SOURCE-BATCH-331 RobotBatteries Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-331`
- Candidate: `SB331-CAND-001`
- Behavior: add stale/null/mobile/source-battery/backpack/deleted-target guards to the robot battery interaction path.
- System: `Quests:Robots / RobotBatteries`
- File: `Data/Scripts/Quests/Robots/RobotBatteries.cs`

## Allowed Source Change

Add guard-only checks to `RobotBatteries.OnDoubleClick(Mobile from)` and `PowerTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source batteries, null source battery state, missing backpacks, source batteries outside the backpack, or deleted target items before dereferencing those values. Missing backpacks or stale source-battery state should use the existing backpack failure message; deleted target robot items should use the existing robot-in-pack failure path.

## Must Stay Unchanged

- Target range `1`
- Localized backpack message `1060640`
- Target prompt
- Robot-in-pack failure message
- Invalid-target message
- `RobotItem` eligibility
- `+1` charge increment
- Charge cap `100`
- Success and already-full messages
- `RevealingAction`
- Sound `0x559`
- `InvalidateProperties`
- Source battery `Delete` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-331 RobotBatteries Guard Repair
```
