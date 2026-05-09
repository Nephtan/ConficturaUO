# Mystic Magic

## Overview
Mystic Magic is the monk ability system built around `SpellbookType.Mystic`, spell IDs `250` through `259`, `SkillName.FistFighting`, tithing points, a bound `MysticSpellbook`, owner-bound learning scrolls, a required `MysticMonkRobe`, and an optional `MysticPack` rucksack.

`Initializer` registers the ten Mystic spell classes, and `Spellbook.GetTypeForSpell` maps `250 <= spellID < 260` to `SpellbookType.Mystic`. Player casting entry points are the spellbook `Gump`, ten direct player commands, and the shared monk spell-bar commands.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `MysticSpell` | `Spell` subclass | Shared FistFighting cast skill, monk-equipment gate, tithing and mana spending, mantra, fizzle, resistance, and skill-window behavior. |
| `MysticSpellbook` | `Spellbook` subclass | Bound `Monk's Tome` using `BookOffset = 250`, random pilgrimage clue assignments, scroll-learning checks, rucksack shrine checks, and versioned persistence. |
| `MysticSpellbookGump` | `Gump` | Displays learned spells, clue text, spell costs, help text, and cast buttons for learned spell IDs `250` through `259`. |
| `MysticMonkRobe` | `BaseGiftOuterTorso` subclass | Blessed owner-only robe required by `MysticSpell.MonkNotIllegal` for every Mystic ability except `CreateRobe`. |
| `MysticPack` | `LargeSack` subclass | Owner-only monk rucksack with `MaxItems = 100` and 5 percent effective carried item weight. |
| `AstralProjectionScroll` through `WindRunnerScroll` | `SpellScroll` subclasses | Owner-bound writing items with spell IDs `250` through `259`; dragging or double-clicking by anyone other than `owner` deletes the parchment. |
| `CastMonkSpells` | command registrar | Registers one `AccessLevel.Player` command per Mystic ability. |

## Becoming A Monk
The broader player-title helper `GetPlayerInfo.isMonk(m)` returns true only when both conditions pass:

| Condition | Compiled behavior |
| --- | --- |
| Owned Mystic tome | `Spellbook.FindMystic(m)` must return a `MysticSpellbook`, and that tome's `owner` must equal the Mobile. |
| Legal monk state | `MysticSpell.MonkNotIllegal(m)` must return true. |

`MysticSpell.MonkNotIllegal(from)` enforces the active monk gear and skill rules:

| Rule | Compiled behavior |
| --- | --- |
| One-handed layer | Pugilist glove classes are allowed. Any other `BaseWeapon` on `Layer.OneHanded` fails. |
| Two-handed layer | Any `BaseWeapon` on `Layer.TwoHanded` fails. `BaseArmor` there fails unless `Attributes.SpellChanneling != 0`. |
| Armor | `RegenRates.GetArmorOffset(from) > 0` fails. |
| Robe | `Layer.OuterTorso` must contain a `MysticMonkRobe`. |
| Skills | `Focus.Base >= 100` and `Meditation.Base >= 100` are required. |

The shared spell gate exempts only `CreateRobe` from `MonkNotIllegal`; it still uses the normal spell skill, mana, and tithing checks.

## Tome Binding And Learning
`MysticSpellbook` is constructable as an empty `Monk's Tome` with hue `0xB61`, item ID `0x1A97`, `SpellbookType.Mystic`, `BookOffset = 250`, and `BookCount = 11`. The book binds to the first Mobile that double-clicks or drag-lifts it while `owner == null`. Other Mobiles receive "The book doesn't seem to open."

New books randomize ten pilgrimage clue slots:

| Spell clue slot | Spell learned | Location pool |
| ---: | --- | --- |
| 1 | Astral Projection | Two of the `other` pool slots use Writ slots 1 and 10. |
| 2 | Astral Travel | Four of the `felucca` pool slots use Writ slots 2, 6, 7, and 9. |
| 3 | Create Robe | Four of the `trammel` pool slots use Writ slots 3, 4, 5, and 8. |
| 4 | Gentle Touch | Four of the `trammel` pool slots use Writ slots 3, 4, 5, and 8. |
| 5 | Leap | Four of the `trammel` pool slots use Writ slots 3, 4, 5, and 8. |
| 6 | Psionic Blast | Four of the `felucca` pool slots use Writ slots 2, 6, 7, and 9. |
| 7 | Psychic Wall | Four of the `felucca` pool slots use Writ slots 2, 6, 7, and 9. |
| 8 | Purity of Body | Four of the `trammel` pool slots use Writ slots 3, 4, 5, and 8. |
| 9 | Quivering Palm | Four of the `felucca` pool slots use Writ slots 2, 6, 7, and 9. |
| 10 | Wind Runner | Two of the `other` pool slots use Writ slots 1 and 10. |

The available pilgrimage locations are hardcoded in `SetPrayer`:

| Pool | `pray` values | Maps used |
| --- | --- | --- |
| `trammel` | `1` through `8` | `Map.Sosaria` |
| `felucca` | `9` through `15` | `Map.Lodor` |
| `other` | `16` through `19` | `Map.SavagedEmpire`, `Map.IslesDread`, `Map.SerpentIsland` |

To learn an ability, the owner opens the tome while standing inside the matching stored map and rectangular coordinate bounds. `KiTest` checks spell IDs in order from `250` to `259`. If the first matching unlearned clue has a `BlankScroll` in the player's backpack, the code consumes one blank scroll, deletes any existing owned writing for that same spell from `World.Items`, creates the matching owner-bound `SpellScroll`, puts it in the backpack, speaks the stored chant, and plays a karma-dependent sound.

The generated writing must then be dropped onto the `MysticSpellbook`. The shared `Spellbook.OnDragDrop` path accepts a single `SpellScroll` only when `GetTypeForSpell(scroll.SpellID)` matches the book type, the spell is not already present, and `scroll.SpellID - BookOffset` is inside the book count range. On success it sets the content bit, increments `m_Count`, deletes the scroll, invalidates properties, and plays a sound.

## Rucksack Shrine
Each new tome randomly chooses one `PackShrine` string from these values:

| Roll | Stored `PackShrine` |
| ---: | --- |
| 0 | `Shrine of Intelligence` |
| 1 | `Shrine of Strength` |
| 2 | `Shrine of Wisdom` |
| 3 | `Shrine of Dexterity` |

Opening the owner-bound tome calls `PackTest` before opening the gump. It summons a `MysticPack` only when all checks pass:

| Requirement | Compiled condition |
| --- | --- |
| Region | `from.Region.Name == book.PackShrine` |
| Skill | `FistFighting.Value >= 100` |
| Monk status | `GetPlayerInfo.isMonk(from)` |
| Reagent item | Backpack contains `MysticalPearl` |

When successful, the code deletes every `MysticPack` in `World.Items` whose `owner == from`, deletes the pearl, creates a new rucksack, assigns `owner = from`, and adds it to the backpack. The rucksack can be opened or used for drag/drop only by its owner while that owner still has `FistFighting.Value >= 100` and `GetPlayerInfo.isMonk(from)`.

`MysticPack.GetTotal(TotalType.Weight)` returns `TotalItemWeights() * 0.05` as an integer. `UpdateTotal` applies the same 5 percent multiplier to weight deltas.

## Shared Casting Rules
All Mystic spells inherit these rules unless an individual spell adds checks:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.FistFighting` |
| Damage skill | `SkillName.FistFighting` |
| Cast skill window | `min = RequiredSkill`; `max = RequiredSkill + 50.0` |
| Cast recovery | `CastRecoveryBase = 2` |
| Hand clearing | `ClearHandsOnCast = false` |
| Base mana | `GetMana()` returns `0`; `MysticSpell.CheckFizzle()` manually drains `ScaleMana(RequiredMana)` after `base.CheckFizzle()` succeeds. |
| Tithing gate | `CheckCast()` requires `Caster.TithingPoints >= RequiredTithing`. |
| Tithing spend | `CheckFizzle()` rolls `AosAttribute.LowerRegCost > Utility.Random(100)`; on success the spent tithing becomes `0`, otherwise it spends `RequiredTithing`. Tithing is deducted before `base.CheckFizzle()`. |
| Monk gate | `CheckCast()` and `CheckFizzle()` require `MonkNotIllegal(Caster)` except for `CreateRobe`. |
| Wind Runner region gate | `WindRunner` is blocked when `NoMountsInCertainRegions()` is enabled and `AnimalTrainer.IsNoMountRegion(...)` returns true. |
| Mantra | `SayMantra()` sends the spell mantra as a public overhead message. |
| Fizzle sound | Normal and hurt fizzles play sound `0x1D6`; disturb with message also plays `0x1D6`. |

### Resistance Formula
`GetResistSkill` and `CheckResisted` use the spell's `MysticSpellCircle`:

| Value | Formula |
| --- | --- |
| Resist gain cap check | `maxSkill = ((1 + circle) * 10) + ((1 + (circle / 6)) * 25)` using integer division for `circle / 6`. |
| First resist percent | `target.MagicResist.Value / 5.0` |
| Second resist percent | `target.MagicResist.Value - (((Caster.FistFighting.Value - 20.0) / 5.0) + ((1 + circle) * 5.0))` |
| Final resist percent | `max(firstPercent, secondPercent) / 2.0` |

## Commands
All Mystic ability commands are registered at `AccessLevel.Player`. Each command checks `Multis.DesignContext.Check(e.Mobile)`, requires that `Spellbook.Find(from, spellID)` returns a book containing the spell, and then constructs the spell with `scroll = null`.

No Mystic-specific administrator command was found. The shared `[AllSpells` command from `Spellbook.Initialize` can fill a targeted spellbook, including a Mystic tome.

| Usage | Description attribute | Spell ID | Handler action |
| --- | --- | ---: | --- |
| `AstralProjection` | `Casts AstralProjection` | 250 | `new AstralProjection(e.Mobile, null).Cast()` |
| `AstralTravel` | `Casts AstralTravel` | 251 | `new AstralTravel(e.Mobile, null).Cast()` |
| `CreateRobe` | `Casts CreateRobe` | 252 | `new CreateRobe(e.Mobile, null).Cast()` |
| `GentleTouch` | `Casts GentleTouch` | 253 | `new GentleTouch(e.Mobile, null).Cast()` |
| `Leap` | `Casts Leap` | 254 | `new Leap(e.Mobile, null).Cast()` |
| `PsionicBlast` | `Casts PsionicBlast` | 255 | `new PsionicBlast(e.Mobile, null).Cast()` |
| `PsychicWall` | `Casts PsychicWall` | 256 | `new PsychicWall(e.Mobile, null).Cast()` |
| `PurityOfBody` | `Casts PurityOfBody` | 257 | `new PurityOfBody(e.Mobile, null).Cast()` |
| `QuiveringPalm` | `Casts QuiveringPalm` | 258 | `new QuiveringPalm(e.Mobile, null).Cast()` |
| `WindRunner` | `Casts WindRunner` | 259 | `new WindRunner(e.Mobile, null).Cast()` |

The spellbook help text also names the shared monk toolbar commands: `[monkspell1`, `[monkspell2`, `[monktool1`, `[monktool2`, `[monkclose1`, and `[monkclose2`.

## Ability Reference
These are the compiled spell-class values, not the values printed by the gump text.

| ID | Command | Ability | Delay | Skill | Mana | Tithing | Circle | Compiled effect |
| ---: | --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| 250 | `[AstralProjection` | Astral Projection | 3.0s | 80.0 | 55 | 50 | 8 | Requires unmounted caster, no transformation, no disguise, and no conflicting body-mod action. Changes body to `970`, hue to `0x4001`, sets `Blessed = true`, and starts a timer for `min(FistFighting.Value, 100)` seconds that restores old body, old hue, `Blessed = false`, and ends the action. |
| 251 | `[AstralTravel` | Astral Travel | 3.0s | 50.0 | 40 | 35 | 4 | Recall-style travel to a marked `RecallRune`, a `Runebook` default entry, a keyed `BaseBoat`, or a valid `HouseRaffleDeed`. Checks overload, recall-from, custom world escape/recall gates, recall-to, spawn fit, multis, and runebook charges. Teleports pets and decrements runebook charges when a runebook was supplied to the spell. |
| 252 | `[CreateRobe` | Create Robe | 3.0s | 25.0 | 20 | 40 | 1 | Deletes every `MysticMonkRobe` in `World.Items` whose `m_Owner == Caster`, then creates a hue `2422` robe with `m_Owner = Caster`, `m_Gifter = "Mystical Monk's Robe"`, `m_How = "Belongs to"`, and `m_Points = (int)(FistFighting.Value * 2)`. |
| 253 | `[GentleTouch` | Gentle Touch | 3.0s | 30.0 | 25 | 15 | 1 | Beneficial Mobile target. Rejects unseen targets, dead bonded pets, animated dead, golems, poisoned targets, and mortal-wounded targets. Heals `PlayerLevelMod((FistFighting.Value / 10) + Utility.Random(1, 10), Caster)`. |
| 254 | `[Leap` | Leap | 0.5s | 35.0 | 20 | 10 | 3 | Point target with range `11` on ML cores or `12` otherwise. Checks overload, teleport-from, teleport-to, spawn fit, and multis. Player casters teleport pets, then the caster's `Location` is moved to the target point and `ProcessDelta()` is called. |
| 255 | `[PsionicBlast` | Psionic Blast | 3.0s | 30.0 | 35 | 15 | 5 | Harmful Mobile target. Performs `SpellHelper.CheckReflect(5, ref from, ref target)`. Base damage is `min((FistFighting.Value + Caster.Int) / 4, 60)`, then delayed damage rolls `Utility.RandomMinMax(damage, damage + 4)` as 100 percent energy damage. Slayer spellbooks do not scale this spell. |
| 256 | `[PsychicWall` | Psychic Wall | 3.0s | 60.0 | 45 | 80 | 4 | Ends existing defensive spell state, fails if `Caster.MagicDamageAbsorb > 0`, then sets `MagicDamageAbsorb = (int)(FistFighting.Value / 2)` and adds `BuffIcon.PsychicWall`. |
| 257 | `[PurityOfBody` | Purity of Body | 2.0s | 40.0 | 35 | 25 | 3 | Self-only beneficial sequence. If poisoned, cure chance is `(10000 + (FistFighting.Value * 75) - ((Poison.Level + 1) * poisonFactor)) / 100`, where `poisonFactor` is `3300` for AOS poison levels below 4, `3100` for AOS poison level 4 or higher, and `1750` outside AOS. Cure succeeds when `chanceToCure > Utility.Random(100)`. |
| 258 | `[QuiveringPalm` | Quivering Palm | 0.5s | 20.0 | 20 | 20 | 1 | Requires the equipped `BaseWeapon` to be one of the pugilist glove classes. Sets `weapon.Consecrated = true` for `FistFighting.Value` seconds, then the timer clears it and plays sound `0x1F8`. |
| 259 | `[WindRunner` | Wind Runner | 3.0s | 70.0 | 50 | 75 | 2 | Rejects mounted casters, Hermes boots, Sprinter's Sandals, and race-restricted Hiking Boots. Applies `SpeedControl.MountSpeed` for `(int)(FistFighting.Value * 5)` seconds, tracks the caster in `TableWindRunning`, adds `BuffIcon.WindRunner`, and removes the effect when the timer expires or no-mount region logic calls `RemoveEffect`. |

## Serialization Notes
| Class | Version | Serialized after `base.Serialize` | Deserialize behavior |
| --- | ---: | --- | --- |
| `MysticSpellbook` | 0 | `owner`, `PackShrine`, then ten Writ groups. Each group writes place, world, coordinate string, chant, `Map`, X1, Y1, X2, Y2. | Reads the same positional sequence with no version branching beyond the version integer. |
| `MysticPack` | 0 | `owner` | Reads `owner`, then resets `Weight = 1.0`, `MaxItems = 100`, and `Name = "monk rucksack"`. |
| `MysticMonkRobe` | 0 | No MysticMonkRobe-specific fields after the version integer. | Reads only the version integer after `base.Deserialize`. Robe ownership and point state are inherited from `BaseGiftOuterTorso`. |
| Mystic scrolls | 0 | Each scroll writes its `owner` after the base `SpellScroll` serializer writes `SpellID`. | Reads `owner` after base scroll deserialization. |

## Known Issues
| Issue | Evidence from trace | Impact |
| --- | --- | --- |
| Project include path for the gump is wrong. | `Scripts.csproj` includes `Magic\Mystic\MysticSpellBookGump.cs`, but the actual file is `Magic\Mystic\Gumps\MysticSpellBookGump.cs`. | Maintained MSBuild project builds can miss or fail the Mystic spellbook gump even though the source file exists. |
| `AstralProjection.OnCast()` calls `CheckSequence()` twice. | It first returns on `!CheckSequence()`, then later enters `else if (CheckSequence())`. `MysticSpell.CheckFizzle()` spends tithing before base fizzle and mana after a successful base fizzle. | Successful command/book casts can spend tithing and mana twice. Scroll-backed casts can consume the scroll during the first sequence and then fail the second sequence. |
| Astral Projection restore state is timer-only and forces `Blessed = false`. | The timer stores old body and hue in memory, then restores body/hue and sets `Blessed = false` on expiration. | If the server saves/restarts while projected, the old body/hue timer state is not serialized. Any pre-existing blessed state is also lost when the timer expires. |
| `QuiveringPalm` removes the wrong key from its timer table. | `m_Table` is keyed by `weapon`, but `ExpireTimer.OnTick()` calls `m_Table.Remove(this)`. | Expired timer entries can remain in the static table until overwritten by a later cast on the same weapon. |
| The spellbook gump shows stale resource costs. | Gump text lists costs such as Astral Projection `Mana 50 / Tithe 300`, Create Robe `Tithe 150`, Psychic Wall `Tithe 500`, and Wind Runner `Tithe 250`; the spell classes use the lower values documented in the ability table. | Players reading the tome can see incorrect mana or tithing requirements. |
| `MysticSpellbook.BookCount` is `11` for ten registered Mystic spell IDs. | `BookOffset = 250`, `BookCount = 11`, while `Spellbook.GetTypeForSpell` maps only IDs `250` through `259` to Mystic and the gump lists ten abilities. | Shared book-fill or content-bit logic can set an unused eleventh bit that no Mystic spell uses. |
| Tome rucksack and learning checks assume backpack and region objects exist. | `PackTest` reads `from.Region.Name` and `from.Backpack.FindItemByType(...)`; `KiTest` also reads `from.Backpack.FindItemByType(...)` without null checks. | Nonstandard Mobile states or scripted use without a backpack/region can throw instead of failing cleanly. |
