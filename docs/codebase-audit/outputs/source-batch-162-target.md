# SOURCE-BATCH-162 QuiveringPalmScroll Guard Repair

## Target

- Candidate: `SB154-CAND-009`
- System: `Magic:Mystic / QuiveringPalmScroll`
- File: `Data/Scripts/Magic/Mystic/Scrolls/QuiveringPalmScroll.cs`
- Behavior: add stale/null/mobile/source-scroll guards to `QuiveringPalmScroll.OnDoubleClick(Mobile from)` and `OnDragLift(Mobile from)` before comparing owner, sending messages, or deleting the scroll.

## Fence Result

- POST-BATCH-Y gate hits: 0
- Active overlay rows: 0
- Gated approval crossed: No

## Allowed Change

- `OnDoubleClick(Mobile from)` returns immediately when `from == null`, `from.Deleted`, or the scroll is deleted.
- `OnDragLift(Mobile from)` returns `false` when `from == null`, `from.Deleted`, or the scroll is deleted.

## Must Stay Unchanged

- Owner serialization
- Valid non-owner crumble message and scroll delete behavior
- Valid owner double-click tome message
- Valid-state `OnDragLift` `true` return
- Namespace/type/file layout
- Project/config/data files
- Staff/access behavior
- Economy/reward tuning
- Region/map policy
- Reorganization state
