# SOURCE-BATCH-479 Furs Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-479`
- Candidate: `SB479-CAND-001`
- System: `Items:Trades / Tailor Items / Furs`
- File: `Data/Scripts/Items/Trades/Tailor Items/Furs.cs`
- Behavior: add stale/null mobile and deleted source item guards to the fur label interactions.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add early returns in:

- `Furs.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `FursWhite.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`

## Must Stay Unchanged

- `Furs` and `FursWhite` item identity
- item ID
- hues
- stackability
- weight
- amount handling
- Name-based singular/plural label text
- fallback `furs` label text
- `AsciiMessage` label packet behavior for valid mobiles
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
