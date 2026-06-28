# Death Knight Magic

## Overview
Death Knight magic is a custom `SpellbookType.DeathKnight` spell line using spell IDs `750` through `763`. The system is built around a bound `DeathKnightSpellbook`, a bound `SoulLantern`, fourteen `DeathKnightSkull###` spell scroll items, player cast commands, a custom spellbook `Gump`, and the shared `DeathKnightSpell` base class.

The spell line uses `Knightship` as both `CastSkill` and `DamageSkill`. It rejects casters with positive Karma, requires a `SoulLantern` owned by the caster to hold enough trapped souls, and spends those souls when spells complete their effect path.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `DeathKnightSpell` | `Spell` subclass | Shared Death Knight cast rules, soul accounting, Karma award, mantra, fizzle effects, and power formulas. |
| `DeathKnightSpellbook` | `Spellbook` subclass | Bound spellbook. Uses `SpellbookType.DeathKnight`, `BookOffset = 750`, and `BookCount = 15`. Opens `DeathKnightSpellbookGump` only for its `owner`. |
| `DeathKnightSpellbookGump` | `Gump` | Lists learned spells, shows lore/help pages, and casts learned spells from button IDs `750` through `763`. |
| `SoulLantern` | `MagicLantern` subclass | Bound lantern that stores `TrappedSouls`, equips into `Layer.TwoHanded`, and displays the owner's soul count. |
| `DeathKnightSkull750` through `DeathKnightSkull763` | `SpellScroll` subclasses | Skull-shaped scrolls used to teach spell IDs `750` through `763` to the Death Knight spellbook. |
| `DevilPact` | `BaseCreature` subclass | Summoned devil Mobile used by `Devil Pact`. |
| `DeathKnightCommands` | command registrar | Registers one player command per spell. |
| `MagicForges` | speech-reactive `Item` | Grants the initial book/lantern at the Tomb of Kas and grants unique skulls at named Death Knight shrines. |
| `BaseCreature` death logic | `Mobile` loot/death code | Converts eligible corpse coin value into `SoulLantern.TrappedSouls`. |

## Acquisition And Learning
The starting book and lantern are granted by `MagicForges.OnSpeech` when all of these checks pass:

| Requirement | Compiled condition |
| --- | --- |
| Speaker | `e.Mobile` must be a player and within range `10` of the forge item. |
| Region | Player must be in `the Tomb of Kas the Bloody Handed`. |
| Spoken phrase | The speech must contain `Mortem Mangone`. |
| Alignment and skill | `m.Karma < 0` and `m.Skills[SkillName.Knightship].Base > 0`. |
| Existing assets | Every `SoulLantern` and `DeathKnightSpellbook` in `World.Items` whose `owner == m` is deleted before new ones are granted. |
| Granted items | A new `SoulLantern(m)` and a new `DeathKnightSpellbook((ulong)0, m)` are added to the player's backpack. |

Each Death Knight skull is also granted by `MagicForges.OnSpeech`. For non-trigger forge items, `keyword` is set to the forge item's `Name`, so speaking the shrine name near the matching forge can grant a skull when `m.Karma < 0` and `m.Skills[SkillName.Knightship].Base > 0`. Before a skull is created, every existing skull item of the same class is deleted from `World.Items`.

| Forge name / keyword | Skull item | Spell ID | Teaches |
| --- | --- | --- | --- |
| `Kargoth` | `DeathKnightSkull750` | 750 | Banish |
| `Monduiz` | `DeathKnightSkull751` | 751 | Demonic Touch |
| `Kath` | `DeathKnightSkull752` | 752 | Devil Pact |
| `Myrhal` | `DeathKnightSkull753` | 753 | Grim Reaper |
| `Maeril` | `DeathKnightSkull754` | 754 | Hag Hand |
| `Farian` | `DeathKnightSkull755` | 755 | Hellfire |
| `Androma` | `DeathKnightSkull756` | 756 | Lucifer's Bolt |
| `Oslan` | `DeathKnightSkull757` | 757 | Orb of Orcus |
| `Rezinar` | `DeathKnightSkull758` | 758 | Shield of Hate |
| `Thyrian` | `DeathKnightSkull759` | 759 | Soul Reaper |
| `Minar` | `DeathKnightSkull760` | 760 | Strength of Steel |
| `Urkar` | `DeathKnightSkull761` | 761 | Strike |
| `Luren` | `DeathKnightSkull762` | 762 | Succubus Skin |
| `Khayven` | `DeathKnightSkull763` | 763 | Wrath |

Skulls are normal `SpellScroll` items with skull art (`ItemID` randomly selected from `0x1AE0` through `0x1AE3`) and hue `0xB9A`. Dropping one skull onto a compatible `DeathKnightSpellbook` uses the base `Spellbook.OnDragDrop` path: if the skull's `SpellID` maps to `SpellbookType.DeathKnight`, the book does not already have that spell, and the ID is inside the book's offset/count range, the matching content bit is set and the skull is deleted.

## Soul Economy
`SoulLantern` must be equipped in `Layer.TwoHanded` to collect souls. It can be double-clicked from the backpack by its owner to equip, and double-clicked again while equipped to return it to the backpack.

Soul collection occurs in `BaseCreature` death/loot handling:

| Rule | Compiled behavior |
| --- | --- |
| Eligible victim | The killed creature must be slain by `SlayerName.Repond` and have positive coin value. |
| Credited killer | The source is `this.LastKiller`; if that is a `BaseCreature`, the code uses its master. |
| Credited player | The credited killer must be a `PlayerMobile`. |
| Required item | The credited player must have a `SoulLantern` equipped in `Layer.TwoHanded`. |
| Soul value | Added souls equal `this.TotalGold + ceil(DDCopper / 10.0) + ceil(DDSilver / 5.0)`. |
| Cap | `TrappedSouls` is capped at `100000`. |
| Coin cleanup | `Gold`, `DDCopper`, and `DDSilver` items found in the killed creature's backpack are deleted after the souls are added. |

Casting does not require the lantern to be equipped or in the caster's backpack. `DeathKnightSpell.GetSoulsInLantern` and `DrainSoulsInLantern` scan all `World.Items` for any `SoulLantern` whose `owner == caster`.

## Shared Casting Rules
All Death Knight spells inherit these shared rules unless an individual spell overrides them:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Knightship`. |
| Damage skill | `SkillName.Knightship`. |
| Cast recovery base | `7`. |
| Positive Karma | `CheckCast` and `CheckFizzle` reject `Caster.Karma > 0`. |
| Required skill | Caster's `Knightship` value must be at least the spell's `RequiredSkill`. |
| Souls check | `CheckCast` requires at least `RequiredTithing` souls. `CheckFizzle` uses `GetTithing(Caster, spell)`, which can reduce the cost to `0` through a `LowerRegCost` roll. |
| Mana check | Caster must have at least `GetMana()`, which returns `ScaleMana(RequiredMana)`. |
| Cast skill window | `GetCastSkills` sets `min = RequiredSkill` and `max = RequiredSkill + 40.0`. |
| Karma award | `ComputeKarmaAward()` returns `-(40 + (10 * floor(RequiredSkill / 10)))`, with a minimum circle of `1`. |
| Mantra | `SayMantra()` broadcasts `Info.Mantra` overhead and plays sound `0x19E`. |
| Begin cast effect | `OnBeginCast()` applies fixed effect `0x37C4`. |

### Power Formulas
| Helper | Formula |
| --- | --- |
| `GetKarmaPower(from)` | `karma = clamp(from.Karma * -1, 0, 15000); return karma / 125.0;` Maximum output is `120.0`. |
| `ComputePowerValue(from, div)` | `floor(sqrt((from.Karma * -1) + 20000 + (from.Skills.Knightship.Fixed * 10))) / div`. |
| `GetTithing(caster, spell)` | Returns `0` when `AosAttributes.GetValue(caster, AosAttribute.LowerRegCost) > Utility.Random(100)`, otherwise returns `spell.RequiredTithing`. |

## Spell Reference
| ID | Spell / command | Delay | Skill | Mana | Souls | Target and actual effect |
| --- | --- | --- | --- | --- | --- | --- |
| 750 | Banish / `[DKBanish]` | 1s | 40.0 | 36 | 56 | Harmful `BaseCreature` target. `ControlSlots == 666` targets are deleted when `caster.Knightship > Utility.RandomMinMax(1, 100)`. Other dispellable creatures use `(50 + ((100 * (Knightship - DispelDifficulty)) / (DispelFocus * 2))) / 100` as the delete chance. |
| 751 | Demonic Touch / `[DKDemonicTouch]` | 1s | 15.0 | 16 | 21 | Beneficial Mobile target. Heals for `PlayerLevelMod((int)(GetKarmaPower(caster) / 2), caster)`. |
| 752 | Devil Pact / `[DKDevilPact]` | 1s | 90.0 | 60 | 98 | Point target. Requires four free follower slots. Summons a `DevilPact` for `90 + floor(Knightship / 2)` seconds on AOS cores; non-AOS uses `Utility.Random(80, 40) + floor(Knightship / 2)` seconds. |
| 753 | Grim Reaper / `[DKGrimReaper]` | 0.5s | 30.0 | 28 | 42 | Self effect. Sets `PlayerMobile.EnemyOfOneType = null` and `WaitingForEnemy = true` until a timer expires. Duration is `clamp(ComputePowerValue(1) / 60, 1.5, 3.5)` minutes. |
| 754 | Hag Hand / `[DKHagHand]` | 1s | 5.0 | 8 | 7 | Beneficial Mobile or item target. On a Mobile, computes a Karma-based chance and calls `RemoveCurseSpell.RemoveBadThings(m)` on success. On `BookBox` or `CurseItem`, moves contained items to the caster's backpack and deletes the container when `caster.CheckSkill(Knightship, 0, 100)` succeeds and caster's inverted Karma is positive. |
| 755 | Demonic Hellfire / `[DKHellfire]` | 1s | 70.0 | 52 | 84 | Harmful Mobile target. Deals `GetKarmaPower(caster) / 5` fire damage, then starts a 5-tick burn every 2 seconds. Burn tick damage is `Utility.RandomMinMax(level, level * 2)`, doubled against non-player targets, with levels descending from 5 to 1. |
| 756 | Lucifer's Bolt / `[DKLucifersBolt]` | 1s | 25.0 | 24 | 35 | Harmful Mobile target. Rejects already frozen/paralyzed/casting AOS targets, checks reflect at circle `4`, then paralyzes for `7.0 + (GetKarmaPower(target) * 0.2)` seconds. |
| 757 | Orb of Orcus / `[DKOrbOrcus]` | 1s | 80.0 | 56 | 80 | Self defensive spell. Ends existing `DefensiveSpell`, rejects existing `MagicDamageAbsorb`, then sets `Caster.MagicDamageAbsorb = (int)(GetKarmaPower(caster) / 4)`. |
| 758 | Shield of Hate / `[DKShieldHate]` | 1s | 60.0 | 48 | 77 | Beneficial Mobile target. Adds resistance mods for `GetKarmaPower(caster)` seconds: Fire `+0`, Poison `+0`, Cold `+0`, Physical `+100`. Recasting on an affected target expires the old timer before applying the new one. |
| 759 | Soul Reaper / `[DKSoulReaper]` | 1s | 45.0 | 40 | 63 | Harmful Mobile target. Starts a 30 second timer ticking every 1.5 seconds. Each tick removes 10 Mana, or sets Mana to 0 if below 10. |
| 760 | Strength of Steel / `[DKStrengthSteel]` | 1s | 20.0 | 20 | 28 | Beneficial Mobile target. Adds a Strength bonus of `PlayerLevelMod((int)(GetKarmaPower(target) / 3), caster)` for `GetKarmaPower(target) / 10` minutes. |
| 761 | Strike / `[DKStrike]` | 1s | 10.0 | 12 | 14 | Harmful Mobile target. Deals `GetKarmaPower(caster) / 2` energy damage. |
| 762 | Succubus Skin / `[DKSuccubusSkin]` | 3s | 35.0 | 32 | 49 | Beneficial Mobile target. For `GetKarmaPower(caster)` seconds, heals every 4 seconds for `PlayerLevelMod(Utility.RandomMinMax(5, 10), target)`. The first heal tick can occur almost immediately after the timer starts. |
| 763 | Wrath / `[DKWrath]` | 1s | 50.0 | 44 | 70 | Point target with radius `5`. Excludes the caster and the caster's controlled pets. Base damage is `GetNewAosDamage(32, 1, 4, caster.Player && playerVsPlayer) + floor(GetKarmaPower(caster) / 5)` for player casters, then `damage = (damage * 2) / targetCount`. Deals energy damage to each target outside a `HouseRegion`. |

## Summoned DevilPact Mobile
`DevilPact` is a summoned melee `BaseCreature` with `ControlSlots = 4`, `DeleteCorpseOnDeath = Summoned`, `DispelDifficulty = 80.0`, and `DispelFocus = 20.0`.

| Property | Value |
| --- | --- |
| AI / fight mode | `AIType.AI_Melee`, `FightMode.Closest` |
| Body / title | Body `102`, title `the Devil` |
| Stats | Str `200`, Dex `200`, Int `100` |
| Hits / Stam / Mana | Hits `140` on SE cores, otherwise `70`; Stam `250`; Mana `0` |
| Damage | `14` to `17`, `100%` Energy |
| Resistances | Physical `60-70`, Fire `40-50`, Cold `40-50`, Poison `40-50`, Energy `90-100` |
| Skills | MagicResist `99.9`, Tactics `100.0`, FistFighting `120.0` |
| Other | Fame `0`, Karma `0`, VirtualArmor `40`, Bleed immune, Lethal poison immune |

## Commands
All spell commands are registered at `AccessLevel.Player`. Each command first checks `Multis.DesignContext.Check(e.Mobile)`, then verifies the matching spell ID is present in a spellbook found by `Spellbook.Find(from, spellID)`. If the spell is missing, the command sends localized message `500015`.

| Usage | Description | Spell ID |
| --- | --- | --- |
| `[DKBanish]` | Casts Banish | 750 |
| `[DKDemonicTouch]` | Casts Demonic Touch | 751 |
| `[DKDevilPact]` | Casts Devil Pact | 752 |
| `[DKGrimReaper]` | Casts Grim Reaper | 753 |
| `[DKHagHand]` | Casts Hag Hand | 754 |
| `[DKHellfire]` | Casts Hellfire | 755 |
| `[DKLucifersBolt]` | Casts Lucifers Bolt | 756 |
| `[DKOrbOrcus]` | Casts Orb of Orcus | 757 |
| `[DKShieldHate]` | Casts Shield of Hate | 758 |
| `[DKSoulReaper]` | Casts Soul Reaper | 759 |
| `[DKStrengthSteel]` | Casts Strength of Steel | 760 |
| `[DKStrike]` | Casts Strike | 761 |
| `[DKSuccubusSkin]` | Casts Succubus Skin | 762 |
| `[DKWrath]` | Casts Wrath | 763 |

The spellbook help page also lists Death Knight toolbar commands from the shared spell bar system:

| Usage | Description |
| --- | --- |
| `[deathspell1]` | Opens Death Knight spell bar editor 1. |
| `[deathspell2]` | Opens Death Knight spell bar editor 2. |
| `[deathtool1]` | Opens Death Knight spell bar 1. |
| `[deathtool2]` | Opens Death Knight spell bar 2. |
| `[deathclose1]` | Closes Death Knight spell bar window 1. |
| `[deathclose2]` | Closes Death Knight spell bar window 2. |

## Spellbook And Gump Behavior
- `DeathKnightSpellbook.OnDoubleClick` rejects non-owners with "These pages appears as scribbles to you."
- The owner can open the gump only when the book is directly held by the Mobile or directly in the owner's backpack.
- Gump page `1` lists only learned spell IDs from `750` through `763`.
- Gump pages `2` through `8` show two spell detail panels each.
- Gump page `9` contains the Death Knight overview and the command list.
- Gump spell buttons cast only when `HasSpell(from, info.ButtonID)` returns true.

## Serialization
| Type | Version | Serialized fields |
| --- | --- | --- |
| `DeathKnightSpellbook` | `0` | `owner` Mobile reference after base `Spellbook` data. |
| `SoulLantern` | `1` | `owner` Mobile reference, then `TrappedSouls` integer. |
| `DeathKnightSkull750` through `DeathKnightSkull763` | `0` | No custom fields after version. |
| `DevilPact` | `0` | No custom fields after version. |

## Known Issues
- Most spells call `CheckFizzle()` after `CheckSequence()`, `CheckHSequence()`, or `CheckBSequence()` has already performed a fizzle check and charged the spell. This creates a second independent skill/resource gate after mana and Karma costs may already have been applied.
- `BanishSpell.OnCast()` calls `CheckFizzle()` before assigning the target, but if that check fails it does not call `DoFizzle()` or `FinishSequence()`.
- `LowerRegCost` soul reduction is inconsistent. `CheckCast()` still requires the full `RequiredTithing`, while `CheckFizzle()` and `DrainSoulsInLantern()` roll separate reduction chances.
- `GetSoulsInLantern()` reads the last owned `SoulLantern` found in `World.Items`, while `DrainSoulsInLantern()` subtracts from every owned lantern it finds. Duplicate owned lanterns can therefore make displayed/checked souls differ from drained souls.
- The spellbook gump lists Orb of Orcus as costing `200` souls, but `OrbOfOrcusSpell.RequiredTithing` is `80`.
- `DeathKnightSpellbook.BookCount` returns `15`, but the initializer, skull classes, commands, and gump cover only 14 spell IDs (`750` through `763`).
- `HagHandSpell.TargetItem()` can remove `BookBox` and `CurseItem` containers without calling `CheckSequence()` and without draining souls.
- `StrengthOfSteelSpell` scales its bonus and duration from the target's Karma, but adds the buff icon to the caster rather than the target receiving the Strength bonus.
- `SoulReaperSpell.Target()` starts a new timer without stopping an existing timer for the same target, which can leave older timers running after the hash table entry is overwritten.
- `HellfireSpell.DoBurn()` calls `m.Damage(damage, m)`, so burn ticks credit the target as the damager instead of the caster.
- `WrathSpell` drains souls after a successful point cast even when no valid targets are found in range.
- `SoulLantern.Deserialize()` reads the version but does not branch on it. Version `1` always reads both `owner` and `TrappedSouls`, so older layouts would not be migrated safely.
