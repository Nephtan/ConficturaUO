# SOURCE-BATCH-038 EverlastingLoaf Guard Repair Target

## Target

- Batch: SOURCE-BATCH-038
- Candidate: SB037-CAND-002
- Behavior: add stale/null/mobile/deleted-item guards to `EverlastingLoaf.OnDoubleClick` before assigning hunger, sending messages, playing sound, or animating.
- System: Items:Magical / Artifacts / Minor / EverlastingLoaf
- File: `Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingLoaf.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- ItemID `0x136F`
- Hue `0`
- Name `Everlasting Loaf`
- `Artefact` label
- `from.Hunger = 20`
- bite/reform message
- `Utility.Random(0x3A, 3)`
- human/not-mounted animation `34`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Ready Goal Command

```text
/goal SOURCE-BATCH-038 EverlastingLoaf Guard Repair

Implement the SB037-CAND-002 guard-only repair in Data/Scripts/Items/Magical/Artifacts/Minor/EverlastingLoaf.cs.

Add stale/null/mobile/deleted-item guards to EverlastingLoaf.OnDoubleClick before assigning hunger, sending messages, playing sound, or animating.

Preserve hunger refill behavior, item identity, sound, message, animation condition, serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

Verify POST-BATCH-Y gate hits=0 and active overlay rows=0, run targeted source preservation scans, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, restore generated root artifacts, update audit records, and commit as:
fix: guard EverlastingLoaf interaction
```
