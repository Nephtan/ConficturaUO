# SOURCE-BATCH-489 ParagonChest Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-489`
- Candidate: `SB489-CAND-001`
- System: `Items:Containers / ParagonChest`
- File: `Data/Scripts/Items/Containers/ParagonChest.cs`
- Behavior: add stale/null mobile and deleted source chest guards to the label rendering path.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `ParagonChest.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- ParagonChest item/container identity
- construction metadata
- reward population
- treasure map drop behavior
- relic drop behavior
- valid `LabelTo(from, 1063449, m_Name)` behavior
- `GetProperties` text
- `Flip` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
