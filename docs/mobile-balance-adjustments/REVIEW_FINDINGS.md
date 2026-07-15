# Mobile Balance Review Findings

Review date: July 15, 2026.

Source workbook: `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx`

Canonical workbook: `docs/mobile-balance-adjustments/workbooks/MobileBalanceTemplate.xlsx`

## Corrected Workbook Facts

The staff-filled `MobileChanges` data is populated correctly:

- 33 mobile change rows.
- 18 `MobileChanges.Notes` cells match `lower damage` case-insensitively.
- All 33 mobile rows have `DamageMin` and `DamageMax` values.
- All `MobileChanges.LootAssignmentIds` resolve to rows in `LootAssignments`.
- All `LootAssignments.ItemId` values resolve to rows in `NewLootItems`.

## P1 Findings

### Signed Skill Modifiers Need Custom Handling

Many requested item skill modifiers are negative, and several items have more than five skill-mod entries. These cannot be represented safely as stock `AosSkillBonuses.SetValues` data.

Use the normalized `ItemSkillMods` sheet and `outputs/itemskillmods.csv`. Rows marked `CustomSignedEquipSkillMod` require custom equip/unequip `SkillMod` logic during source implementation.

Evidence:

- `Data/Scripts/System/Misc/AOS.cs` processes only five stock skill-bonus slots.
- The AOS skill-bonus packing reads bonus values back as positive packed values, so signed values should not be stored there.

### Drop Semantics Need Staff Decision

All 39 loot assignment rows have `Guaranteed=Yes` and `ChancePercent < 100`.

The enhanced workbook preserves both values and flags every row as:

- `DropRule=NeedsDecision`
- `DropSemanticsStatus=NeedsDecision`

Staff must choose whether these are chance-based drops, guaranteed drops, guaranteed group rolls, or another rule before source implementation.

### Owner-Bound Items Need Binding Policy

Eight item rows have `OwnerBound=Yes`. The workbook flags them as `OwnerBoundPolicyStatus=NeedsOwnerBindingPolicy`.

Staff must choose when ownership binds, such as killer, top damager, looter, first equipper, or another rule.

### `Pestilence` Name Conflict

`ITEM-014` requests class `Pestilence` as a mace. The source tree already contains `Server.Items.Pestilence` as an obsolete quiver class.

This is flagged as `ExistingClassStatus=NameConflict` and `ImplementationStatus=BlockedByNameConflict`. Choose a new class name before implementation.

## P2 Findings

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
