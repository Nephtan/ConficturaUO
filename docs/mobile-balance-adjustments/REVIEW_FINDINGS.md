# Mobile Balance Review Findings

Review date: July 28, 2026.

Current source workbook: `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx`

Original source workbook: `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx`

Canonical workbook: `docs/mobile-balance-adjustments/workbooks/MobileBalanceTemplate.xlsx`

## Corrected Workbook Facts

The staff-filled `MobileChanges` data is populated correctly:

- 35 mobile change rows.
- 18 `MobileChanges.Notes` cells match `lower damage` case-insensitively.
- All 35 mobile rows have `DamageMin` and `DamageMax` values.
- All `MobileChanges.LootAssignmentIds` resolve to rows in `LootAssignments`.
- All `LootAssignments.ItemId` values resolve to rows in `NewLootItems`.
- The normalized sheets contain 64 mobile skill rows, 73 item skill rows, and 110 item bonus rows.
- `Reference` is an instructional sheet; all structured tables use row 4 for headers and row 5 onward for data.

## P1 Findings

### Signed Skill Modifiers Need Custom Handling

Many requested item skill modifiers are negative, and several items have more than five skill-mod entries. These cannot be represented safely as stock `AosSkillBonuses.SetValues` data.

The workbook contains 36 negative skill modifiers across 19 items. Use the normalized `ItemSkillMods` sheet and `outputs/itemskillmods.csv`. Rows marked `CustomSignedEquipSkillMod` require custom equip/unequip `SkillMod` logic during source implementation.

Evidence:

- `Data/Scripts/System/Misc/AOS.cs` processes only five stock skill-bonus slots.
- The AOS skill-bonus packing reads bonus values back as positive packed values, so signed values should not be stored there.

Decision required:

1. Preserve all signed values with a reusable custom equip/unequip `SkillMod` implementation.
2. Remove the negative modifiers.
3. Return the 19 affected items for redesign.

The recommended implementation choice is option 1 because it preserves the staff-authored balance data without misusing stock packed skill bonuses.

## Resolved Decisions

### Loot Semantics

All 39 assignments are independent percentage rolls on the corpse:

- `Guaranteed=No`
- `DropRule=ChancePercentOnCorpse`
- `DropSemanticsStatus=Ready`

### Ownership

No requested item is owner-bound:

- `OwnerBound=No`
- `OwnerBoundPolicyStatus=NotOwnerBound`

### Mace Class Conflict

`ITEM-014` is now the proposed class `DreadMace` with display name `The Dread Mace`. Stale `Pestilence` references were removed from assignments, skill rows, bonus rows, and references.

### New Mobile Loot

Blank loot links on `MC-034 Fire Illusion` and `MC-035 Lovecraftian` mean no loot change requested.

## P2 Findings

### New Mobile Priorities Need Staff Decision

`MC-034 Fire Illusion` and `MC-035 Lovecraftian` intentionally have blank `Priority` values. Choose `High`, `Medium`, or `Low` for each.

Recommendation: `High`, because 22 of the other 24 `EpicBossCandidate` rows are already `High`.

### Canonical Name Cleanup Applied

The enhanced workbook normalizes these known class-name tokens:

| Raw token | Canonical token |
| --- | --- |
| `PowderofTemperment` | `PowderOfTemperament` |
| `PheonixFeather` | `PhoenixFeather` |
| `RoseofMoonPetal` | `RoseOfMoonPetal` |
| `MetalPigmentsofIslesDread` | `MetalPigmentsOfIslesDread` |

### Skill Aliases And Misspellings Mapped

The enhanced workbook maps staff-entered skill aliases and misspellings into canonical `SkillName` values:

| Raw token | Canonical token |
| --- | --- |
| `Magic Resist` | `MagicResist` |
| `Parrying` | `Parry` |
| `Blacksmithing` | `Blacksmith` |
| `Merchantile` | `Mercantile` |
| `Aantomy` | `Anatomy` |

### Non-Skill Bonuses Moved To Item Bonuses

These values appeared in `SkillBonusesRaw` but are not skills:

- `PhysicalBonus`
- `FireBonus`
- `ColdBonus`
- `EnergyBonus`

They are now represented in `ItemBonuses` with `BonusGroup=BaseArmorResistanceBonus`.

### Bonus Aliases Mapped

The enhanced workbook maps common bonus aliases into source-facing names:

| Raw token | Canonical token |
| --- | --- |
| `HitChance` | `AttackChance` |
| `BonusStamina` | `BonusStam` |
| `HitsRegen` | `RegenHits` |

## Non-Issues

- `DamageMin` and `DamageMax` are not blank in the staff-filled workbook.
- Repeated assignment IDs are intentional group-style rows for mobiles with multiple possible item rows. They are retained.
- Existing mobile display names such as `Dio's Monster`, `Wolfgang's Dragon`, and `Immortal Genie of the Vase` resolve to source classes or name/title pairs.
