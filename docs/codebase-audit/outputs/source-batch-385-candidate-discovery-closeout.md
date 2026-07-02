# SOURCE-BATCH-385 Candidate Discovery Closeout

## Result

`SOURCE-BATCH-385` ran fresh non-gated candidate discovery after `SOURCE-BATCH-384` and selected `SB385-CAND-001`.

## Recommended Target

- Candidate: `SB385-CAND-001`
- Batch: `SOURCE-BATCH-385`
- System: `Items:Books / BulletinBoards / StatusBoard`
- File: `Data/Scripts/Items/Books/BulletinBoards/StatusBoard.cs`
- Behavior: add stale/null mobile, source-board, and gump response guards to `StatusBoard.OnDoubleClick(Mobile e)` and `StatusGump.OnResponse(NetState sender, RelayInfo info)` before range checks, gump construction, pagination, or mobile state dereferences.

## Fence Evidence

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- Gated approval crossed: `No`

## Selection Rationale

`StatusBoard.OnDoubleClick` opens a status gump after a range check but dereferences the mobile before validating stale/null state. `StatusGump.OnResponse` also checks `from.Deleted` before proving `from` is non-null. The guard repair prevents invalid interaction state from causing null-reference failures without changing status visibility, access-level filtering, account/client counts, gump layout, pagination, serialization, or policy behavior.

## Deferred Or Excluded Work

Status display design changes, staff/access policy changes, account/client visibility policy changes, command policy, balance/economy tuning, region/map behavior, serializer migration/layout, project/config/data, XML/config/data, and reorganization remain excluded unless separately approved.
