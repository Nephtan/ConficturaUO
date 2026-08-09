# SOURCE-BATCH-025 Target: LuckyHorseShoes Guard Repair

Reviewed at: 2026-06-16T20:06:00-05:00

## Target

- Batch: `SOURCE-BATCH-025`
- Candidate: `SB025-CAND-001`
- Behavior: add stale/null/backpack/source-deed/target guards to `LuckyHorseShoes.OnDoubleClick` and `LuckTarget.OnTarget` before mobile backpack state, source deed state, target item state, luck mutation, or deed deletion is evaluated.
- System: `Items:Magical / LuckyHorseShoes`
- File: `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/LuckyHorseShoes.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `BaseWeapon`, `BaseClothing`, `BaseJewel`, `BaseArmor`, and `Spellbook` eligibility.
- RootParent ownership rule for target items.
- `+100` Luck increment.
- Luck cap at `1000`.
- Existing messages.
- Target range.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
