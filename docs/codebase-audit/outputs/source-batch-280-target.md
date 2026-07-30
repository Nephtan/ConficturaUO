# SOURCE-BATCH-280 SavageTalisman Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-280`
- Candidate: `SB280-CAND-001`
- System: `Items:Magical / SavageTalisman`
- Source file: `Data/Scripts/Items/Magical/SavageTalisman.cs`
- Behavior: add stale/null/mobile/source-item guards to `SavageTalisman.OnEquip(Mobile from)` and `OnDoubleClick(Mobile from)` before owner-message handling or worn-slot messaging.

## Allowed Source Change

- In `SavageTalisman.OnEquip(Mobile from)`, return `false` when `from == null || from.Deleted || Deleted`.
- In `SavageTalisman.OnDoubleClick(Mobile from)`, return immediately when `from == null || from.Deleted || Deleted`.

## Must Stay Unchanged

- `ItemOwner` equip restriction.
- Owner overhead message text.
- Worn-slot message text.
- `SkillBonuses` Camping and Cooking values.
- Construction metadata, including `Name`, `ItemID`, `Layer`, `Weight`, `Hue`, and `Resource`.
- `ItemOwner` serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, and reorganization state.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.

## Ready Goal Shape

`/goal SOURCE-BATCH-280 SavageTalisman Guard Repair`
