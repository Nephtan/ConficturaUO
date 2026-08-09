# SOURCE-BATCH-391 LiarsDice Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-391`
- Candidate: `SB391-CAND-001`
- System: `Items:Misc / Games / LiarsDice`
- File: `Data/Scripts/Items/Misc/Games/LiarsDice/LiarsDice.cs`

## Intended Source Change

Add a local guard to `LiarsDice.OnDoubleClick(Mobile from)` so stale/null interaction state cannot dereference an invalid mobile or use a deleted game item before the existing game-entry behavior runs.

Allowed change:

- Return immediately from `OnDoubleClick` when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

LiarsDice item ID, name, weight, hue, `GOLD_PER_GAME`, `GAME_BALANCE_MIN`, `GAME_BALANCE_MAX`, `GAME_PLAYER_TO_ACT_SECONDS`, `GAME_MAX_PLAYERS`, `DiceState` initialization, range check, `Banker.GetBalance` threshold, `from.Frozen = true`, `ds.ShowNewGameGump(from)`, insufficient-bank message, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state must stay unchanged.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`
