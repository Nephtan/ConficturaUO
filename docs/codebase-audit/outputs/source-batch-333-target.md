# SOURCE-BATCH-333 SlaversNet Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-333`
- Candidate: `SB333-CAND-001`
- Behavior: add stale/null/mobile/source-net/backpack/deleted-target guards to the slaver net interaction path.
- System: `Items:Special / SlaversNet`
- File: `Data/Scripts/Items/Special/SlaversNet.cs`

## Allowed Source Change

Add guard-only checks to `SlaversNet.OnDoubleClick(Mobile from)` and `SlaveTarget.OnTarget(Mobile from, object targeted)`.

The guards may return early for null/deleted mobiles, deleted source nets, null source net state, missing backpacks, source nets outside the backpack, or deleted target mobiles before dereferencing those values. Missing backpacks or stale source-net state should use the existing backpack failure message; deleted target mobiles should use the existing invalid-target failure path.

## Must Stay Unchanged

- Target range `6`
- Localized backpack message `1060640`
- Target prompt
- Invalid-target and capture failure messages
- `BaseCreature` eligibility
- Paragon, tamable, controlled, and follower-slot checks
- `Utility.RandomMinMax(50, 200)` capture odds
- `Utility.RandomBool()` torn-net branch
- `ControlSlots` mutation
- `MinTameSkill` clamp
- `SetControlMaster`, `ControlTarget`, `IsBonded`, and `ControlOrder` behavior
- Invisible-pet visibility fixes
- Sound `0x059`
- Source net `Delete` behavior
- Serialization layout/versioning
- Namespace/type/file layout
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Ready Goal

```text
/goal SOURCE-BATCH-333 SlaversNet Guard Repair
```
