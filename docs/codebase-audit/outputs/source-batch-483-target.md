# SOURCE-BATCH-483 BasePiece Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-483`
- Candidate: `SB483-CAND-001`
- System: `Items:Misc / Games / BasePiece`
- File: `Data/Scripts/Items/Misc/Games/BasePiece.cs`
- Behavior: add stale/null mobile and deleted source item guard coverage to the final board-piece label dispatch.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

In `BasePiece.OnSingleClick(Mobile from)`, keep the existing `m_Board` cleanup and reparent checks first, then return before `base.OnSingleClick(from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- `BasePiece` item identity and `IsVirtualItem` behavior
- `Board` property behavior
- existing orphan-board `Delete` behavior
- existing reparent-to-board `DropItem` behavior
- valid base `OnSingleClick` behavior
- `OnDragLift` board checks
- `CanTarget=false`
- `DropToMobile`, `DropToItem`, and `DropToWorld` behavior
- `GetLiftSound=-1`
- chess/checkers derived piece identities
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
