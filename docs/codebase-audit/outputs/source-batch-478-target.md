# SOURCE-BATCH-478 GraveStones Rename Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-478`
- Candidate: `SB478-CAND-001`
- System: `Items:Decorative / GraveStones`
- File: `Data/Scripts/Mobiles/Elementals/Necromental.cs`
- Behavior: add stale/null/deleted guards to the grave stone rename interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add early returns in:

- `GraveStones.OnDoubleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`
- `RenamePrompt.OnResponse(Mobile from, string text)` when `from == null`, `from.Deleted`, `m_Sign == null`, or `m_Sign.Deleted`

## Must Stay Unchanged

- `Necromental` creature identity
- Necromental stats, resistances, skills, breath/combat behavior, loot, and grave stone drop behavior
- `GraveStones` item identity
- randomized grave stone item ID selection
- valid rename prompt message
- valid rename prompt assignment to `from.Prompt`
- valid `RenamePrompt` name assignment and confirmation message
- serialization layout/versioning for both classes
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
