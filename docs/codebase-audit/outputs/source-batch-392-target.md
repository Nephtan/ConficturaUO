# SOURCE-BATCH-392 MahjongGame Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-392`
- Candidate: `SB392-CAND-001`
- System: `Items:Misc / Games / Mahjong`
- File: `Data/Scripts/Items/Misc/Games/Mahjong/MahjongGame.cs`

## Intended Source Change

Add a local guard to `MahjongGame.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or use a deleted game item before the existing Mahjong game-entry behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

MahjongGame item ID, name, weight, `MahjongPlayers` initialization, context menu behavior, player cleanup, join behavior, reset/recreate-game behavior, pending-player behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
