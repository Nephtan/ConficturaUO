# SOURCE-BATCH-039 MusicBox Guard Repair Target

## Target

- Batch: SOURCE-BATCH-039
- Candidate: SB037-CAND-003
- Behavior: add stale/null/mobile/deleted-item guards to `MusicBox.OnDoubleClick` before sending music packets/messages or advancing `Mplay` state.
- System: Items:Misc / MusicBox
- File: `Data/Scripts/Items/Misc/MusicBox.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none

## Must Stay Unchanged

- Name `Lute of Many Songs`
- `Mplay` property
- all `PlayMusic.GetInstance` calls and message labels
- `Mplay` increment behavior
- `Mplay` reset to `1`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map behavior
- reorganization state

## Ready Goal Command

```text
/goal SOURCE-BATCH-039 MusicBox Guard Repair

Implement the SB037-CAND-003 guard-only repair in Data/Scripts/Items/Misc/MusicBox.cs.

Add stale/null/mobile/deleted-item guards to MusicBox.OnDoubleClick before sending music packets/messages or advancing Mplay state.

Preserve song order, messages, Mplay increment/reset behavior, serialization, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

Verify POST-BATCH-Y gate hits=0 and active overlay rows=0, run targeted source preservation scans, serializer diff scan, forbidden-surface diff scan, Server.csproj Debug/x86 build, compile-only runtime verification, git diff --check, restore generated root artifacts, update audit records, and commit as:
fix: guard MusicBox interaction
```
