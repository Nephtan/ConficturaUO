# Mobile Balance Data Dictionary

This file defines the canonical workbook sheets and CSV outputs used for mobile balance implementation planning.

## Workbook Layout

- Structured sheet headers are always on row 4.
- Structured data begins on row 5.
- Rows 1 and 2 contain the sheet title and staff guidance.
- `Reference` is not a structured table. It uses separate guidance, skill-alias, canonical-skill, and item-attribute sections.
- Raw staff text remains in `*Raw` columns. Canonical columns and normalized child sheets are the implementation-facing data.

## Core Sheets

### `MobileChanges`

One row per mobile change request.

| Column | Meaning |
| --- | --- |
| `ChangeId` | Stable staff/Codex identifier for the requested mobile change. |
| `MobileClassOrName` | Staff-entered mobile class or display name. |
| `ResolvedClassName` | Source-resolved C# class name when known. |
| `KnownFilePath` | Repository-relative source path for the resolved mobile class. |
| `Scope` | `ExactMobile`, `ListedMobiles`, or `EpicBossCandidate`. |
| `DamageMin`, `DamageMax` | Requested melee damage range. Blank means no damage change. |
| `SkillChangesRaw` | Original staff skill-change text. |
| `LootAssignmentIds` | Assignment group linked to `LootAssignments`. |
| `Priority` | `High`, `Medium`, or `Low`. |
| `ReviewStatus` | Data review state. |
| `ImplementationStatus` | Code implementation readiness state. |
| `Notes` | Staff notes. |
| `CodexNotes` | Review and implementation notes from Codex. |

### `NewLootItems`

One row per requested item or existing item reference.

| Column | Meaning |
| --- | --- |
| `ItemId` | Stable item identifier referenced by loot assignments. |
| `ClassNameRaw` | Original staff class name. |
| `CanonicalClassName` | Normalized class name for implementation. |
| `BaseItemRaw` | Original staff base item class. |
| `CanonicalBaseItem` | Normalized base item class. |
| `ExistingClassStatus` | Whether the class exists, is proposed, needs review, or conflicts. |
| `DisplayName` | In-game display name requested by staff. |
| `ItemIDGraphic`, `Hue`, `Layer`, `LootType` | Item art, color, equipment layer, and loot type. |
| `SkillBonusesRaw` | Original staff skill-bonus text. |
| `AttributesRaw` | Original staff attribute text. |
| `OwnerBound` | Staff flag for owner-bound behavior. |
| `OwnerBoundPolicyStatus` | Owner-binding policy state. |
| `PropertyLabel` | Staff-facing property label such as `Artefact`. |
| `ImplementationStatus` | Code implementation readiness state. |
| `Notes`, `CodexNotes` | Staff and Codex notes. |

### `LootAssignments`

One row per requested drop relationship between a mobile and an item.

| Column | Meaning |
| --- | --- |
| `AssignmentId` | Stable group identifier linked from `MobileChanges`. |
| `MobileClassOrName` | Staff-entered mobile target. |
| `ResolvedMobileClass` | Source-resolved mobile class when known. |
| `ItemId` | Item identifier from `NewLootItems`. |
| `CanonicalItemClass` | Normalized item class. |
| `DropTiming` | Where implementation should place the drop logic. |
| `ChancePercent` | Requested chance value. |
| `QuantityMin`, `QuantityMax` | Requested drop quantity range. |
| `Guaranteed` | Staff flag from the source workbook. |
| `DropRule` | Explicit implementation rule. `NeedsDecision` blocks source implementation. |
| `DropSemanticsStatus` | Whether drop semantics are ready or need policy. |
| `Notes`, `CodexNotes` | Staff and Codex notes. |

## Normalized Child Sheets

### `MobileSkillChanges`

One row per parsed skill change from `MobileChanges.SkillChangesRaw`.

Canonical skill names come from `Data/System/Source/Skills.cs`.

`SkillMin` and `SkillMax` represent a requested range. `SkillValue` represents a single requested value. `Operation=SetSkill` maps to the mobile's skill-setting surface.

### `ItemSkillMods`

One row per parsed item skill modifier from `NewLootItems.SkillBonusesRaw`.

Use `ModifierKind` to distinguish:

| Value | Meaning |
| --- | --- |
| `StockPositiveSkillBonus` | Can use stock `SkillBonuses.SetValues` if the item has five or fewer positive skill bonuses. |
| `CustomSignedEquipSkillMod` | Requires custom equip/unequip `SkillMod` handling. Use this for negative values or more than five skill modifiers. |

### `ItemBonuses`

One row per parsed item bonus/property from `NewLootItems.AttributesRaw`, plus non-skill values moved out of `SkillBonusesRaw`.

| Bonus group | Implementation surface |
| --- | --- |
| `AosAttributes` | `item.Attributes.*` |
| `AosWeaponAttributes` | `weapon.WeaponAttributes.*` |
| `AosArmorAttributes` | `armor.ArmorAttributes.*` |
| `BaseWeaponProperty` | Direct weapon properties such as `MinDamage` and `MaxDamage`. |
| `BaseArmorResistanceBonus` | Direct armor resistance bonus properties. |
| `ItemProperty` | Direct item properties such as `Weight`. |
| `Unknown` | Needs manual mapping before implementation. |

## Reference Sheets

| Sheet | Purpose |
| --- | --- |
| `Ref_Skills` | Source-generated `SkillName` values. |
| `Ref_ItemClasses` | Source-generated item classes, including the 23 implemented balance classes. |
| `Ref_MobileClasses` | Source-generated mobile candidate classes and display names. |
| `Ref_BonusNames` | Canonical bonus/property names and implementation surfaces. |
| `Ref_DropRules` | Allowed drop-rule semantics. |
| `Reference` | Staff instructions, common aliases, canonical `SkillName` values, and common item attribute names. |

## Dropdown Sources

| Entry column | Dropdown source |
| --- | --- |
| Mobile class/display columns | `Ref_MobileClasses` |
| Item class/base-item columns | `Ref_ItemClasses` |
| Canonical skill columns | `Ref_Skills.SkillName` |
| Canonical bonus and bonus-group columns | `Ref_BonusNames` |
| Drop rule columns | `Ref_DropRules` |
| Scope, priority, status, layer, loot type, owner-bound, and modifier-kind columns | Inline controlled lists documented below and enforced by workbook validation. |

Reference-backed list formulas use absolute endpoints. They must not drift when rows are copied or added.

## Status Values

| Status | Meaning |
| --- | --- |
| `Ready` | Data is structurally ready for implementation. |
| `NeedsDecision` | Staff policy is needed before implementation. |
| `NeedsNormalization` | Raw text needs canonical mapping. |
| `Blocked` | Cannot safely implement until fixed. |
| `Pending` | Awaiting implementation. |
| `ReadyForImplementation` | Approved and ready for code changes. |
| `Implemented` | Verified source implementation exists and matches normalized data. |
| `NeedsPolicyDecision` | A policy field is unresolved. |
| `NeedsClassNameDecision` | Class naming needs a decision. |
| `BlockedByNameConflict` | Class name conflicts with an existing source type. |

## Current Approved Values

| Field | Approved value |
| --- | --- |
| `LootAssignments.Guaranteed` | `No` on all 39 rows |
| `LootAssignments.DropRule` | `ChancePercentOnCorpse` on all 39 rows |
| `LootAssignments.DropSemanticsStatus` | `Ready` on all 39 rows |
| `NewLootItems.OwnerBound` | `No` on all 39 rows |
| `NewLootItems.OwnerBoundPolicyStatus` | `NotOwnerBound` on all 39 rows |
| `NewLootItems[ITEM-014].CanonicalClassName` | `DreadMace` |
| `NewLootItems[ITEM-014].DisplayName` | `The Dread Mace` |
| `MobileChanges[MC-034].Priority` | `High` |
| `MobileChanges[MC-035].Priority` | `High` |
| `MobileChanges.ImplementationStatus` | `Implemented` on all 35 rows |
| `NewLootItems.ImplementationStatus` | `Implemented` on all 39 rows |
| `ItemSkillMods.CustomSignedEquipSkillMod` | 70 rows implemented through reusable equip/unequip `SkillMod` handling |

## Implementation Mapping

- `MobileChanges` and `MobileSkillChanges` map to `MobileBalanceCatalog.ApplyProfile`.
- `LootAssignments` map to independent `MobileBalanceCatalog.DropLoot` rolls; existing loot remains intact.
- `NewLootItems` map to 23 new public classes or 16 per-drop configurations of existing classes.
- `ItemSkillMods` contains 70 custom signed rows and three stock positive rows. Only Wolfgang Sword's two positive bonuses and Weight of the World's Anatomy bonus use `SkillBonuses.SetValues`.
- `ItemBonuses` maps to native AOS, weapon, armor, resistance, direct damage-bound, and item property APIs.
- All generated drop instances use `LootType.Regular`. Source snapshot text remains unchanged.
