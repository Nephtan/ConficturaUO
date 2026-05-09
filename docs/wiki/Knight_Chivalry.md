# Knight Chivalry

## Overview

Knight Chivalry is implemented as a `BookOfChivalry` `Spellbook` item plus ten `PaladinSpell` subclasses registered as spell IDs `200` through `209`. The line uses `SkillName.Knightship` for both cast and damage skill checks, uses normal `Mobile.TithingPoints` as its secondary resource, and activates through the shared spellbook client packet path.

There are no Knight-specific player `[Command]` registrations in `Data/Scripts/Magic/Knight/`. Players cast through the native spellbook UI, dragged spell icons, runebook integration for Sacred Journey, or any shard UI that sends the registered spell ID.

## Core Components

| Component | Type | Purpose |
| --- | --- | --- |
| `BookOfChivalry` | `Spellbook` `Item` | Knightship spellbook using `SpellbookType.Paladin`, `BookOffset = 200`, `BookCount = 10`, full default content `0x3FF`, and `Layer.Talisman`. |
| `PaladinSpell` | `Spell` subclass | Shared Knightship cast rules, Karma/tithing/mana checks, mantra output, fizzle sound, cast skill range, and the `ComputePowerValue` formula. |
| `CleanseByFireSpell` | `PaladinSpell` | Beneficial target poison cure that burns the caster. |
| `CloseWoundsSpell` | `PaladinSpell` | Beneficial target heal with poison, mortal wound, and undead guards. |
| `ConsecrateWeaponSpell` | `PaladinSpell` | Temporarily marks the current `BaseWeapon` as consecrated. |
| `DispelEvilSpell` | `PaladinSpell` | Area effect against summoned creatures, special `ControlSlots == 666` creatures, evil creatures, and Necromancy transformations. |
| `DivineFurySpell` | `PaladinSpell` | Temporary self buff that restores stamina and modifies hit chance, defense chance, and weapon damage. |
| `EnemyOfOneSpell` | `PaladinSpell` | Temporary `PlayerMobile` state that selects one non-player defender type and changes melee damage bonuses. |
| `HolyLightSpell` | `PaladinSpell` | Area energy damage around the caster. |
| `NobleSacrificeSpell` | `PaladinSpell` | Area beneficial effect that can resurrect, cure, heal, and remove curses, then drops the caster to 1 Hits/Stamina/Mana. |
| `RemoveCurseSpell` | `PaladinSpell` | Beneficial target curse removal and custom curse-container cleansing. |
| `SacredJourneySpell` | `PaladinSpell` | Recall-like travel to runes, runebooks, boat keys, and valid house raffle deeds. |
| `TithingGump` | `Gump` | Shared ankh donation UI that converts backpack gold to `Mobile.TithingPoints`. |
| `KeeperOfChivalry` | `BaseVendor` `Mobile` | Vendor titled `the Knight` that stocks `BookOfChivalry`, a `PaladinWarhorse`, and possible Knightship study books. |

## Registration And Activation

`Initializer` registers these Knight Chivalry spell IDs:

| Spell ID | Registry type |
| ---: | --- |
| `200` | `Chivalry.CleanseByFireSpell` |
| `201` | `Chivalry.CloseWoundsSpell` |
| `202` | `Chivalry.ConsecrateWeaponSpell` |
| `203` | `Chivalry.DispelEvilSpell` |
| `204` | `Chivalry.DivineFurySpell` |
| `205` | `Chivalry.EnemyOfOneSpell` |
| `206` | `Chivalry.HolyLightSpell` |
| `207` | `Chivalry.NobleSacrificeSpell` |
| `208` | `Chivalry.RemoveCurseSpell` |
| `209` | `Chivalry.SacredJourneySpell` |

`Spellbook.GetTypeForSpell` maps `200 <= spellID < 210` to `SpellbookType.Paladin`. On a client cast request, the shared `Spellbook.EventSink_CastSpellRequest` verifies that the `Spellbook` contains the spell ID, creates the registered `Spell` with `SpellRegistry.NewSpell(spellID, from, null)`, and calls `Cast()`.

The shared `Spellbook.EventSink_OpenSpellbookRequest` maps client spellbook type `3` to `SpellbookType.Paladin` and displays the first matching book found equipped on `Layer.Talisman` or directly in the caster's backpack.

## Acquisition And Book Rules

| Rule | Compiled behavior |
| --- | --- |
| Default book contents | `new BookOfChivalry()` calls `BookOfChivalry((ulong)0x3FF)`, so a constructed book starts with all ten spell bits set. |
| Item art and name | `BookOfChivalry(ulong content)` calls the base `Spellbook` constructor with item ID `0x2252`, sets `Name = "Book of Knightship"`, and equips in `Layer.Talisman`. |
| Equip gate | Shared `Spellbook.OnEquip` rejects a `BookOfChivalry` only when base Knightship is below `30.0` and Karma is below `0`. |
| Open gate | `Spellbook.OnDoubleClick` opens only when the book is equipped by the `Mobile` or directly in the `Mobile` backpack, not nested in another container. |
| Vendor | `KeeperOfChivalry` sells `BookOfChivalry` for `140` gold with random stock `1..15`; it also sells a `PaladinWarhorse` and may sell standard study books when `MyServerSettings.SellChance()` permits them. |
| Tithing | Ankhs add a context menu entry that opens `TithingGump` for alive `Mobile` users in range `4`. The gump donates backpack `Gold`, caps resulting `TithingPoints` at `100000`, and adds one tithing point per gold consumed. |

## Shared PaladinSpell Rules

| Rule | Actual behavior |
| --- | --- |
| Cast skill | `SkillName.Knightship` |
| Damage skill | `SkillName.Knightship` |
| Hands | `ClearHandsOnCast = false` |
| Cast recovery base | `7` |
| Skill check range | `RequiredSkill` through `RequiredSkill + 50.0` |
| Movement default | Inherited `Spell.BlocksMovement = true` unless an individual spell returns `false`. |
| Karma gate | `CheckCast` and `CheckFizzle` reject `Caster.Karma < 0`. |
| Tithing gate | `CheckCast` requires `Caster.TithingPoints >= RequiredTithing`. |
| Mana gate | `CheckCast` requires `Caster.Mana >= ScaleMana(RequiredMana)`. |
| Tithing spend | `CheckFizzle` spends `RequiredTithing`, unless `AosAttribute.LowerRegCost > Utility.Random(100)`, before the base fizzle check runs. |
| Mana spend | `CheckFizzle` spends `ScaleMana(RequiredMana)` only after `base.CheckFizzle()` succeeds. |
| Base mana cost | `GetMana()` returns `0`, so Paladin spells pay their mana in `PaladinSpell.CheckFizzle`, not the shared `Spell.CheckSequence` mana path. |
| Mantra | `SayMantra()` sends `Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, MantraNumber, "", false)`. |
| Cast visual | `SendCastEffect()` plays fixed effect `0x37C4` at the caster for a duration based on `GetCastDelay()`. |
| Fast cast cap | Shared `Spell.GetCastDelay()` caps Knightship faster casting at `4`, or `2` when the caster has at least `70.0` Magery; `ProtectionSpell` reduces the effective faster-cast value by `2`. |

### Power Formula

Most spell scaling uses:

```text
ComputePowerValue(div) = floor(sqrt(Caster.Karma + 20000 + (Caster.Skills.Knightship.Fixed * 10))) / div
```

`Skill.Fixed` is the RunUO fixed-point value, so `100.0` Knightship contributes `1000 * 10 = 10000` inside the square root.

## Ability Reference

| Ability | ID | Required Knightship | Mana | Tithe | Cast delay | Movement | Targeting / range | Compiled mechanics |
| --- | ---: | ---: | ---: | ---: | ---: | --- | --- | --- |
| Cleanse By Fire | 200 | 5.0 | 10 | 10 | 1.0s | Blocks | Beneficial `Mobile`, range `10` under `Core.ML`, otherwise `12` | Requires the target to be poisoned. Cure chance is `(10000 + Knightship.Value * 75 - ((Poison.Level + 1) * 2000)) / 100`, checked as `chance > Utility.Random(100)`. After the target effect path, the caster takes fire damage `50 - ComputePowerValue(4)`, clamped to `13..55`. |
| Close Wounds | 201 | 0.0 | 10 | 10 | 1.5s | Blocks | Beneficial `Mobile`, target object range `12`, final caster range `2` | Rejects animated dead `BaseCreature`, dead bonded pets, full-health targets, poisoned targets, and `MortalStrike` wounds. Heal amount is `PlayerLevelMod(ComputePowerValue(6) + Utility.RandomMinMax(0, 2), Caster)`, clamped to `7..39` and capped to missing Hits. |
| Consecrate Weapon | 202 | 15.0 | 10 | 10 | 0.5s | Does not block | Self/current weapon | Requires `Caster.Weapon as BaseWeapon` and rejects `Fists`. Sets the weapon's `Consecrated` flag for `Caster.Skills.Knightship.Value` seconds. While set, `BaseWeapon` converts outgoing weapon damage to `100%` of the defender's lowest physical/fire/cold/poison/energy resistance type. |
| Dispel Evil | 203 | 35.0 | 10 | 10 | 0.25s | Does not block | Hostile mobiles within range `8` | Builds a target list of valid harmful targets plus any `BaseCreature` with `ControlSlots == 666`. Summoned, non-animated-dead creatures can be deleted by the dispel chance. `ControlSlots == 666` creatures are deleted when `Knightship.Value > Utility.RandomMinMax(1, 100)`. Uncontrolled evil creatures can flee for 30 seconds. Necromancer transformations can lose stamina and mana. |
| Divine Fury | 204 | 25.0 | 15 | 10 | 1.0s | Does not block | Self | Restores caster stamina to `StamMax`. Duration is `ComputePowerValue(10)` seconds, clamped to `7..24`. While active, `BaseWeapon` gives the attacker `+10` hit chance, applies `-20` defend chance when the defender is under the effect, and adds `+10` weapon damage bonus. |
| Enemy of One | 205 | 45.0 | 20 | 10 | 0.5s | Does not block | Self | Duration is `ComputePowerValue(1) / 60` minutes, clamped to `1.5..3.5`. For `PlayerMobile` casters, sets `EnemyOfOneType = null` and `WaitingForEnemy = true`. The first non-player defender struck becomes the enemy type. Matching enemies take `+50` percentage weapon damage; non-player attackers of the caster whose type does not match take `+100` percentage weapon damage against that caster. |
| Holy Light | 206 | 55.0 | 10 | 10 | 1.75s | Does not block | Hostile mobiles within range `3` | Damages every valid harmful target around the caster. Damage is `ComputePowerValue(10) + Utility.RandomMinMax(0, 2)`, clamped to `8..24`, delivered as `100%` energy damage. |
| Noble Sacrifice | 207 | 65.0 | 20 | 30 | 1.5s | Does not block | Beneficial mobiles within range `3` and line of sight | Skips animated dead creatures and `Golem`. Dead targets get a resurrection gump when `0.1 + (0.9 * (Caster.Karma / 10000.0)) > Utility.RandomDouble()`. Living targets can be cured, healed for `PlayerLevelMod(ComputePowerValue(10) + Utility.RandomMinMax(0, 2), Caster)` clamped to `8..24`, and cleaned of curse effects. If any target is assisted, caster Hits, Stamina, and Mana are all set to `1`. |
| Remove Curse | 208 | 5.0 | 20 | 10 | 1.5s | Blocks | Beneficial `Mobile`, range `10` under `Core.ML`, otherwise `12`; also `BookBox` and `CurseItem` | Mobile target success chance is Karma-based: `0` below `-5000`, `sqrt(20000 + Karma) - 122` below `0`, `sqrt(Karma) + 25` below `5625`, otherwise `100`. On success it removes negative magic stat mods, paralyze, several Necromancy/Magery curse effects, `MortalStrike`, `BloodOath`, `MindRot`, and related buff icons. Curse containers are opened by moving their contents to the caster backpack and deleting the container. |
| Sacred Journey | 209 | 15.0 | 10 | 15 | 1.5s | Does not block | `RecallRune`, `Runebook`, boat `Key`, or valid `HouseRaffleDeed` | `CheckCast` rejects overloaded casters and failed `TravelCheckType.RecallFrom`. The effect path checks escape permissions, source recall rules, destination teleport rules, destination recall rules, overload again, blocked destination, multi collision, runebook charges when `m_Book` is non-null, and then teleports pets and caster to the target location. |

## Dispel Evil Formulas

| Target type | Formula / behavior |
| --- | --- |
| Summoned `BaseCreature` that is not animated dead | `dispelSkill = ComputePowerValue(2)`. Chance is `((50.0 + ((100 * (Knightship - DispelDifficulty)) / (DispelFocus * 2))) / 100) * (dispelSkill / 100.0)`. If the chance is greater than `Utility.RandomDouble()`, the target is deleted. |
| `BaseCreature.ControlSlots == 666` | Deleted when `Caster.Skills.Knightship.Value > Utility.RandomMinMax(1, 100)`. |
| Evil uncontrolled `BaseCreature` | `fleeChance = ((100 - sqrt(Fame / 2)) * Knightship * dispelSkill) / 1000000`. If it beats `Utility.RandomDouble()`, `BeginFlee(TimeSpan.FromSeconds(30.0))` is called. |
| Necromancer transformation | If `TransformationSpellHelper.GetContext(m)` returns a `NecromancerSpell`, drain chance is `0.5 * (Caster.Knightship / max(target.Necromancy, 1))`. On success, stamina and mana each lose `(5 * dispelSkill) / 100`. |

## Combat Integration

| Effect | Integration point | Actual behavior |
| --- | --- | --- |
| Consecrate Weapon | `BaseWeapon` damage type calculation | Reads defender physical/fire/cold/poison/energy resistances, selects the lowest resistance, zeros all damage-type percentages, and assigns `100` to the lowest resistance type. Ties keep the earlier type, starting with physical. |
| Divine Fury hit chance | `BaseWeapon` hit chance calculation | When the attacker is under `DivineFurySpell.UnderEffect`, `bonus += 10` before the global `45` hit-chance cap. |
| Divine Fury defense penalty | `BaseWeapon` hit chance calculation | When the defender is under the effect, their defend chance bonus is reduced by `20`. |
| Divine Fury damage | `BaseWeapon.GetDamageBonus` | Adds `10` weapon damage bonus before the global damage-bonus cap. |
| Enemy of One outgoing | `BaseWeapon` percentage damage bonus | A player attacker waiting for an enemy records the first non-player defender `Type`. Matching defender types get `+50` percentage bonus. |
| Enemy of One incoming | `BaseWeapon` percentage damage bonus | A non-player attacker gets `+100` percentage bonus against a `PlayerMobile` defender whose stored enemy type is non-null and different from the attacker's `Type`. |
| Enemy of One notoriety refresh | `PlayerMobile.EnemyOfOneType` setter | Calls `DeltaEnemies(oldType, newType)`, which sends `MobileMoving` or `MobileMovingOld` packets for nearby mobiles of the old or new type when the player's `NetState` is non-null. |

## Admin Notes

| Command | Access | Usage | Parameters | Behavior |
| --- | --- | --- | --- | --- |
| `AllSpells` | `GameMaster` | `[Usage("AllSpells")]` | None; prompts for a target after execution. | Shared spellbook command. If the target is a `Spellbook`, sets `Content` to every spell bit supported by that book's `BookCount`. For `BookOfChivalry`, this fills all ten spell IDs `200..209`. |

No Knight-specific `[Command]`, `[Usage]`, or `[Description]` handlers were found in the Knight spell folder.

## Serialization Notes

| Class | Version | Serialized data after version | Notes |
| --- | ---: | --- | --- |
| `BookOfChivalry` | `1` | None | Writes a version integer after `base.Serialize(writer)`. `Deserialize` reads the same integer and stores it in a local variable. |
| `Spellbook` base | `3` | `Crafter`, `Slayer`, `Slayer2`, `AosAttributes`, `AosSkillBonuses`, `Content`, `Count` | `BookOfChivalry` relies on this base class for actual content persistence. |
| `MyPaladinbook` | `0` after base | None | Decorative generated-book subclass of `BookOfChivalry`; base spellbook state is serialized first. |
| `MyChivalryBook` | `0` after base | None | Decorative generated-book subclass of `BookOfChivalry`; base spellbook state is serialized first. |
| Runtime spell effects | None | None | `ConsecrateWeapon`, `DivineFury`, and `EnemyOfOne` use static `Hashtable` timer state and are not persisted through world save/load. |

## Known Issues

| Issue | Impact |
| --- | --- |
| `ConsecrateWeaponSpell.ExpireTimer.OnTick()` calls `m_Table.Remove(this)`, but `m_Table` is keyed by `BaseWeapon`, not by the timer. | Expired consecration entries remain in the static table until overwritten by a later cast on the same weapon, leaving stale timer references. |
| `ConsecrateWeaponSpell` uses `Caster.Skills[SkillName.Knightship].Value` seconds for duration, while the source comment says the effect should last `3` to `11` seconds and be affected by Karma. | At high Knightship the compiled effect lasts far longer than the comment describes and does not use Karma scaling. |
| `PaladinSpell.CheckCast()` requires full `RequiredTithing` before casting, but `CheckFizzle()` can later waive the spend with `LowerRegCost`. | A caster with enough lower reagent cost to waive tithing still cannot begin the spell unless they already have the full listed tithing points. |
| `RemoveCurseSpell.TargetItem()` does not call `CheckSequence()` or `CheckBSequence()`. | Cleansing `BookBox` or `CurseItem` containers uses only `CheckSkill(Knightship, 0, 100)` and positive Karma, bypassing normal Paladin tithing and mana consumption after the initial pre-cast gate. |
| `DispelEvilSpell`, `HolyLightSpell`, `NobleSacrificeSpell`, and `PlayerMobile.DeltaEnemies()` iterate `GetMobilesInRange(...)` without freeing the pooled enumerable. | Repeated casts or Enemy of One target changes can leak pooled range enumerables in the RunUO range-scan infrastructure. |
| Sacred Journey's runebook charge path is effectively unused by current callers. | `RunebookGump` constructs `new SacredJourneySpell(from, null, e, null)`, and targeting a `Runebook` also calls `Effect(...)` without assigning `m_Book`, so `m_Book.CurCharges` is not decremented by the compiled Chivalry travel path. |
| `Spellbook.OnEquip` rejects `BookOfChivalry` only when the `Mobile` has both low Knightship and negative Karma. | The code permits low-Knightship nonnegative-Karma users and negative-Karma users with at least `30.0` Knightship to equip the book, which is narrower than the rejection message implies. |
