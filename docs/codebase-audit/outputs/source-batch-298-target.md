# SOURCE-BATCH-298 RedLeaves Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-298`
- Candidate: `SB298-CAND-001`
- System: `Trades:Gardening / RedLeaves`
- Source file: `Data/Scripts/Trades/Gardening/MiscItems/RedLeaves.cs`
- Behavior: add stale/null/mobile/source-red-leaves/backpack/target-item guards to `RedLeaves.OnDoubleClick(Mobile from)` and `InternalTarget.OnTarget(Mobile from, object targeted)` before target assignment, backpack checks, book eligibility checks, wax consumption, or book `Writable` mutation.

## Allowed Source Changes

- In `OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.
- In `OnDoubleClick(Mobile from)`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1042664`.
- In `InternalTarget.OnTarget`, return immediately when `from == null || from.Deleted || m_RedLeaves == null || m_RedLeaves.Deleted`.
- In `InternalTarget.OnTarget`, treat `from.Backpack == null` as the existing backpack-use failure with localized message `1042664`.
- In `InternalTarget.OnTarget`, treat null or deleted target items as the existing backpack-use failure path without consuming red leaves or mutating books.

## Must Stay Unchanged

- Backpack message `1042664`.
- Target prompt `1061907`.
- Book-only message `1061911`.
- Already-sealed message `1061909`.
- Success message `1061910`.
- `BaseBook` eligibility.
- `Writable = false` mutation.
- Red leaves `Consume()` semantics.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-298 RedLeaves Guard Repair`
