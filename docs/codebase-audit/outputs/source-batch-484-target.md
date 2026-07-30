# SOURCE-BATCH-484 DecayedCorpse Label Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-484`
- Candidate: `SB484-CAND-001`
- System: `Items:Misc / Bodies / DecayedCorpse`
- File: `Data/Scripts/Items/Misc/Bodies/Corpses/DecayedCorpse.cs`
- Behavior: add stale/null mobile and deleted source item guards to the localized remains label interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `DecayedCorpse.OnSingleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- `DecayedCorpse` item/container identity
- random corpse item IDs
- `Movable=false` behavior
- `Name` assignment
- `GumpID` and `DropSound` values
- `BeginDecay` behavior
- `InternalTimer` delete behavior
- `OnAfterDelete` timer stop behavior
- `CheckContentDisplay=false`
- `DisplaysContent=false`
- `AddNameProperty` localized remains label
- valid `OnSingleClick` localized label behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
