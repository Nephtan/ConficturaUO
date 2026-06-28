# SOURCE-BATCH-035 Dice12 Guard Repair Target

## Target

- Batch: SOURCE-BATCH-035
- Candidate: SB031-CAND-005
- Behavior: add stale/null/mobile/deleted-item guards to `Dice12.OnDoubleClick` and `Dice12.OnTelekinesis` before range checks, particle effects, sound, or `Roll(from)` dereferences mobile state.
- System: Items:Misc / Games / DandD Dice12
- File: `Data/Scripts/Items/Misc/Games/DandD/Dice12.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- ItemID `0x301D`
- name and weight
- `AddNameProperties` labels
- range `2`
- telekinesis effects and sound `0x1F5`
- roll overhead message
- `Utility.Random(1, 12)`
- sound `0x34`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Ready Goal Command

```text
/goal SOURCE-BATCH-035 Dice12 Guard Repair

Implement the SB031-CAND-005 guard-only repair in Data/Scripts/Items/Misc/Games/DandD/Dice12.cs.

Add stale/null/mobile/deleted-item guards to Dice12.OnDoubleClick and Dice12.OnTelekinesis before range checks, particle effects, sound, or Roll(from) dereferences mobile state.

Preserve dice sides, overhead message, telekinesis effects, sounds, serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

Verify POST-BATCH-Y gate hits=0 and active overlay rows=0, run targeted source preservation scans, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, restore generated root artifacts, update audit records, and commit as:
fix: guard Dice12 interactions
```
