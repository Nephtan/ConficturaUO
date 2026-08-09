# SOURCE-BATCH-040 RewardBlackDyeTub Guard Repair Target

## Target

- Batch: SOURCE-BATCH-040
- Candidate: SB040-CAND-001
- Behavior: add stale/null/mobile/deleted-item guards to `RewardBlackDyeTub.OnDoubleClick` before `RewardSystem.CheckIsUsableBy` or `base.OnDoubleClick`.
- System: Items:Misc / Dyes / RewardBlackDyeTub
- File: `Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- LabelNumber `1006008`
- Hue and DyedHue `0x0001`
- `Redyable = false`
- blessed loot
- `RewardSystem.CheckIsUsableBy`
- `base.OnDoubleClick(from)`
- `IsRewardItem` serialization
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Ready Goal Command

```text
/goal SOURCE-BATCH-040 RewardBlackDyeTub Guard Repair

Implement the SB040-CAND-001 guard-only repair in Data/Scripts/Items/Misc/Dyes/RewardBlackDyeTub.cs.

Add stale/null/mobile/deleted-item guards to RewardBlackDyeTub.OnDoubleClick before RewardSystem.CheckIsUsableBy or base.OnDoubleClick.

Preserve reward usability checks, base DyeTub behavior, hue, messages, serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

Verify POST-BATCH-Y gate hits=0 and active overlay rows=0, run targeted source preservation scans, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, restore generated root artifacts, update audit records, and commit as:
fix: guard RewardBlackDyeTub interaction
```
