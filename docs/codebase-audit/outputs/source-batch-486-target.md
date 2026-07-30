# SOURCE-BATCH-486 BrewCauldron Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-486`
- Candidate: `SB486-CAND-001`
- System: `Items:Potions / Special / BrewCauldron`
- File: `Data/Scripts/Items/Potions/Special/BrewCauldron.cs`
- Behavior: add stale/null mobile and deleted source cauldron guards to the double-click interaction.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Prior source-batch completions for this exact file: `0`
- No gated approval crossed.

## Allowed Source Change

Add an early return in `BrewCauldron.OnDoubleClick(Mobile from)` when `from == null`, `from.Deleted`, or `Deleted`.

## Must Stay Unchanged

- BrewCauldron item identity
- random pool/use initialization
- `ItemID`, `Hue`, `Name`, `Weight`, and `Movable` metadata
- `AddToBackpack` potion selection by pool
- bottle amount/consume behavior
- range failure message
- empty-bottle failure message
- fill success sound/message
- `m_Uses` decrement behavior
- empty-cauldron `ItemID` and `Name` updates
- `OnAfterSpawn` Z adjustment
- decay timer behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state
