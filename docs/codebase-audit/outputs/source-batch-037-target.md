# SOURCE-BATCH-037 EverlastingBottle Guard Repair Target

## Target

- Batch: SOURCE-BATCH-037
- Candidate: SB037-CAND-001
- Behavior: add stale/null/mobile/deleted-item guards to `EverlastingBottle.OnDoubleClick` before assigning thirst, sending messages, or playing sound.
- System: Items:Magical / Artifacts / Minor / EverlastingBottle
- File: `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- ItemID `0x2827`
- Hue `0x849`
- Name `Everlasting Bottle`
- `Artefact` label
- `from.Thirst = 20`
- drink refill message
- `Utility.RandomList(0x30, 0x2D6)`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Ready Goal Command

```text
/goal SOURCE-BATCH-037 EverlastingBottle Guard Repair

Implement the SB037-CAND-001 guard-only repair in Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingBottle.cs.

Add stale/null/mobile/deleted-item guards to EverlastingBottle.OnDoubleClick before assigning thirst, sending messages, or playing sound.

Preserve thirst refill behavior, item identity, sound, message, serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

Verify POST-BATCH-Y gate hits=0 and active overlay rows=0, run targeted source preservation scans, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, restore generated root artifacts, update audit records, and commit as:
fix: guard EverlastingBottle interaction
```
