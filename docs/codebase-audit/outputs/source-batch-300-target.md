# SOURCE-BATCH-300 LargeBODTarget Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-300`
- Candidate: `SB300-CAND-001`
- System: `Trades:Bulk Orders / LargeBODTarget`
- Source file: `Data/Scripts/Trades/Bulk Orders/LargeBODTarget.cs`
- Behavior: add stale/null/mobile/source-deed/backpack guards to `LargeBODTarget.OnTarget(Mobile from, object targeted)` before deed backpack checks or `EndCombine` forwarding.

## Allowed Source Changes

- In `OnTarget(Mobile from, object targeted)`, return immediately when `from == null || from.Deleted || m_Deed == null || m_Deed.Deleted`.
- In `OnTarget(Mobile from, object targeted)`, return through the existing silent failure path when `from.Backpack == null`.

## Must Stay Unchanged

- Target range `18`.
- Existing silent failure behavior.
- `LargeBOD.IsChildOf(from.Backpack)` requirement.
- `m_Deed.EndCombine(from, targeted)` forwarding.
- Namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-300 LargeBODTarget Guard Repair`
