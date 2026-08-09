# SOURCE-BATCH-065 SandMiningBook Guard Repair

## Target

- Candidate: `SB064-CAND-002`
- Behavior: add stale/null/mobile/backpack/source-book guards to SandMiningBook interactions.
- System: `Items:Trades / Specialized / SandMiningBook`
- Source file: `Data/Scripts/Items/Trades/Specialized/SandMiningBook.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in `SandMiningBook.OnDoubleClick(Mobile from)` before mobile, backpack, source-book, skill, learning flag, or delete state is evaluated.

## Must Stay Unchanged

Grandmaster Mining threshold `100.0`, PlayerMobile-only learning, backpack localized message `1042001`, failure and success messages, `PlayerMobile.SandMining` assignment, `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-065 SandMiningBook Guard Repair
```
