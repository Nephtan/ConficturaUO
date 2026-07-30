# SOURCE-BATCH-026 Target: SlayerDeed Guard Repair

Reviewed at: 2026-06-16T20:10:00-05:00

## Target

- Batch: `SOURCE-BATCH-026`
- Candidate: `SB025-CAND-002`
- Behavior: add stale/null/backpack/source-deed/target guards to `SlayerDeed.OnDoubleClick` and `SlayerTarget.OnTarget` before mobile backpack state, source deed state, `SlayerType`, target item state, slayer mutation, or deed deletion is evaluated.
- System: `Items:Magical / SlayerDeed`
- File: `Data/Scripts/Items/Magical/SlayerDeed.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Magical/SlayerDeed.cs`: `0`
- Active overlay rows for `Data/Scripts/Items/Magical/SlayerDeed.cs`: `0`
- Gated approval crossed: `None`

## Must Stay Unchanged

- `BaseWeapon`, `BaseInstrument`, and `Spellbook` eligibility.
- `HolyManSpellbook` exclusion.
- `GetDeedSlayer` mapping.
- `Slayer` then `Slayer2` assignment order.
- Existing messages, including current wording.
- Target range.
- Deed `Delete()` semantics.
- Serialization layout/versioning.
- Namespace, type, and file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.
