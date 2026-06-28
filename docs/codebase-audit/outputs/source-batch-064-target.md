# SOURCE-BATCH-064 GlassblowingBook Guard Repair

## Target

- Candidate: `SB064-CAND-001`
- Behavior: add stale/null/mobile/backpack/source-book guards to GlassblowingBook interactions.
- System: `Items:Trades / Specialized / GlassblowingBook`
- Source file: `Data/Scripts/Items/Trades/Specialized/GlassblowingBook.cs`

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gate crossed: none
- Approval required: none beyond normal non-gated preflight

## Allowed Change

Add guard-only checks in `GlassblowingBook.OnDoubleClick(Mobile from)` before mobile, backpack, source-book, skill, learning flag, or delete state is evaluated.

## Must Stay Unchanged

Grandmaster Alchemy threshold `100.0`, PlayerMobile-only learning, backpack localized message `1042001`, failure and success messages, `PlayerMobile.Glassblowing` assignment, `Delete()` semantics, serialization layout/versioning, namespace/type/file layout, project/config/data files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Ready Goal Command

```text
/goal SOURCE-BATCH-064 GlassblowingBook Guard Repair
```
