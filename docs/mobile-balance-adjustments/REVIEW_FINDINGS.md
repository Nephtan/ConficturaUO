# Mobile Balance Review Findings

Review date: July 28, 2026.

Current approved source workbook: `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-approved-2026-07-28.xlsx`

Earlier source workbooks remain immutable:

- `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-revised-2026-07-28.xlsx`
- `docs/mobile-balance-adjustments/source/MobileBalanceTemplate.staff-filled-2026-07-15.xlsx`

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

## Current Findings

There are no unresolved workbook review issues. `outputs/review-issues.csv` contains its header and no data rows.

## Resolved Implementation Requirements

### Signed Skill Modifiers Use Custom Handling

Many requested item skill modifiers are negative, and several items have more than five skill-mod entries. These cannot be represented safely as stock `AosSkillBonuses.SetValues` data.

The workbook contains 36 negative skill modifiers across 19 items. Use the normalized `ItemSkillMods` sheet and `outputs/itemskillmods.csv`. Rows marked `CustomSignedEquipSkillMod` require custom equip/unequip `SkillMod` logic during source implementation.

Evidence:

- `Data/Scripts/System/Misc/AOS.cs` processes only five stock skill-bonus slots.
- The AOS skill-bonus packing reads bonus values back as positive packed values, so signed values should not be stored there.

Decision: preserve all signed values with reusable equip/unequip `EquipedSkillMod` instances, `ObeyCap=false`, idempotent removal/recreation, and post-deserialization rehydration. The implementation applies all 70 custom rows, including positive modifiers on mixed-benefit/drawback items. Only the three explicitly stock-positive rows use `SkillBonuses.SetValues`.

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

`ITEM-014` is implemented as `DreadMace` with display name `The Dread Mace`. Stale `Pestilence` references were removed from assignments, skill rows, bonus rows, and references.

### New Mobile Loot

Blank loot links on `MC-034 Fire Illusion` and `MC-035 Lovecraftian` mean no loot change requested.

### New Mobile Priorities

`MC-034 Fire Illusion` and `MC-035 Lovecraftian` are approved as `High` priority.

### Source Implementation

- All 35 profiles and 64 skill changes are implemented.
- All 39 independent loot assignments are implemented as additional corpse rolls.
- Twenty-three new item classes and 16 configured existing-item drops are implemented.
- All 73 item skill rows and 110 item bonus rows are source-verified.
- All target mobiles use version 1 serialization; version-0 instances apply the shared profile once during deserialization.
- “Legendary Registry of Heroes” is accepted by the existing legal census interface.

## Normalization Findings

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
