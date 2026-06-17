# SOURCE-BATCH-030 Target: PuzzleCube Guard Repair

Reviewed at: 2026-06-16T20:18:00-05:00

## Target

- Batch: `SOURCE-BATCH-030`
- Candidate: `SB028-CAND-003`
- Behavior: add stale/null/mobile/backpack guards to `PuzzleCube.OnDoubleClick` before backpack state or puzzle state is evaluated.
- System: `Items:Misc / Games / PuzzleCube`
- File: `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Misc/Games/PuzzleCube.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- Solved and scrambled item IDs `0x202A` and `0x202B`.
- Random success check.
- Sound `0x04B`.
- Existing messages.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
