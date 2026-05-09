# Jedi Magic

## Overview
Jedi Magic is a custom `SpellbookType.Jedi` spell line using spell IDs `280` through `289`. The system is built around a bound `JediSpellbook`, ten `JediDatacron##` `SpellScroll` items, `KaranCrystal` reagent charges, player cast commands, quick-bar `Gump` support, priest-grave speech triggers, and the shared `JediSpell` base class.

The spell line uses `SkillName.Psychology` as `CastSkill` and `SkillName.Swords` as `DamageSkill`. Casts reject negative Karma, require enough mana, require enough crystals stored in a `JediSpellbook` owned by the caster, and use sword/tactics/Karma formulas for several spell effects.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `JediSpell` | `Spell` subclass | Shared Jedi cast rules, crystal accounting, fizzle effects, spell metadata, command dispatch, and power formulas. |
| `JediSpellbook` | `Spellbook` subclass | Bound datacron. Uses `SpellbookType.Jedi`, `BookOffset = 280`, and `BookCount = 11`. Opens the custom gump only for its `owner`. |
| `JediSpellbookGump` | `Gump` | Displays learned powers, lore/help pages, power clue pages, quick-bar controls, and laser sword construction buttons. |
| `JediDatacron01` through `JediDatacron10` | `SpellScroll` subclasses | Holocron scroll Items that teach spell IDs `280` through `289` to the Jedi spellbook. |
| `KaranCrystal` | `BaseReagent` subclass | Crystal charge Item. Dropping it on an owned datacron increases `JediSpellbook.crystals` up to `50000`. |
| `CastJediSpells` | command registrar | Registers one player command per Jedi power. |
| `MagicForges` | speech-reactive `Item` | Grants the initial datacron at Zoda's tomb and grants unique holocrons at `Priest Grave` items. |
| `Priest` | vendor `Mobile` | Converts gold into `KaranCrystal` stacks for eligible Jedi players. |
| `DropRelic` creature death logic | loot helper | Drops `KaranCrystal` stacks from Exorcism-slayer victims for eligible Jedi killers. |
| `SoulOrb` | resurrection `Item` | Reused by `Replicate` as the resurrection crystal implementation. |

`Initializer` registers spell IDs `280` through `289`, and `Spellbook.GetTypeForSpell` maps `280 <= spellID < 290` to `SpellbookType.Jedi`.

## Acquisition
### Starting Datacron
The starting datacron is granted by `MagicForges.OnSpeech` when all of these checks pass:

| Requirement | Compiled condition |
| --- | --- |
| Speaker | `e.Mobile` must be a player. |
| Range | The player must be within `10` tiles of the `MagicForges` Item. |
| Region | `m.Region.IsPartOf("the Tomb of Zoda the Jedi Master")`. |
| Spoken phrase | Speech must contain `Oh Beh Wahn`. |
| Karma | `m.Karma >= 2500`. |
| Psychology | `m.Skills[SkillName.Psychology].Base >= 25`. |
| Existing assets | Every `JediSpellbook` in `World.Items` with `owner == m` is deleted before the new one is granted. |
| Granted item | `new JediSpellbook((ulong)0, m)` is added to the player's backpack. |

The new book starts with no spells, `crystals = 0`, `page = 0`, `names = 0`, `gem = 0`, and `steel = 0`.

### Being Treated As A Jedi
`GetPlayerInfo.isJedi(m, checkSword)` returns true only when both checks pass:

| Check | Compiled behavior |
| --- | --- |
| Bound datacron | `Spellbook.FindJedi(m)` must find a `JediSpellbook` whose `owner == m`. |
| Jedi legality | `JediSpell.JediNotIllegal(m, checkSword)` must return true. |

`JediNotIllegal` requires all of the following:

| Requirement | Compiled behavior |
| --- | --- |
| Sword | If `checkSword == true`, the Mobile must have a `BaseSword` in `Layer.OneHanded` or `Layer.TwoHanded`. If `checkSword == false`, the sword check is skipped. |
| Jedi item | The Mobile must wear at least one qualifying item with a name containing `Jedi`: shield in `Layer.TwoHanded`, allowed helm/hat art in `Layer.Helm`, robe in `Layer.OuterTorso`, cloak in `Layer.Cloak`, or talisman ItemID `0x543E` in `Layer.Talisman`. |
| Minimum skills | `Psychology.Base >= 10`, `Swords.Base >= 10`, and `Tactics.Base >= 10`. |

Casting additionally rejects `Caster.Karma < 0`.

### Learning Powers
Holocrons are granted by `MagicForges.OnSpeech` at `MagicForges` items named `Priest Grave`. The speaker must be a player within range `10`, must already pass `isJedi(m, false)`, must have non-negative Karma and positive Psychology base skill, and must speak the matching true name near the matching grave X coordinate. Before a holocron is created, every existing Item of that same holocron class is deleted from `World.Items`.

| Grave X check | True name keyword | Holocron item | Spell ID | Power | Clue location from datacron |
| ---: | --- | --- | ---: | --- | --- |
| `3082` | `jacen sollo` | `JediDatacron01` | 280 | Force Grip | the City of Britain, Land of Sosaria |
| `805` | `kiadi mundia` | `JediDatacron02` | 281 | Mind's Eye | the Town of Moon, Land of Sosaria |
| `981` | `kip fisto` | `JediDatacron03` | 282 | Mirage | the Village of Grey, Land of Sosaria |
| `3249` | `marra jade` | `JediDatacron04` | 283 | Throw Sabre | the City of Montor, Land of Sosaria |
| `1398` | `numi sunrider` | `JediDatacron05` | 284 | Celerity | the Town of Renika, the Island of Umber Veil |
| `2983` | `plo kune` | `JediDatacron06` | 285 | Psychic Aura | the City of Elidor, Land of Lodoria |
| `4378` | `kyle katran` | `JediDatacron07` | 286 | Deflection | the Village of Springvale, Land of Lodoria |
| `2697` | `kyp duron` | `JediDatacron08` | 287 | Soothing Touch | the Village of Islegem, Land of Lodoria |
| `4167` | `ganer rhysode` | `JediDatacron09` | 288 | Stasis Field | Greensky Village, Land of Lodoria |
| `6642` | `coran horn` | `JediDatacron10` | 289 | Replicate | the Kuldar Cemetery, the Bottle World of Kuldar |

Dropping a single holocron scroll onto a compatible `JediSpellbook` uses the shared `Spellbook.OnDragDrop` path: the dropped `SpellScroll.SpellID` must map to `SpellbookType.Jedi`, the book must not already contain that spell, and the ID must fall inside the book's offset/count range. On success, the matching content bit is set, the book count increments, the scroll is deleted, and sound `0x558` is played.

## Crystal Economy
`KaranCrystal` is a stackable `BaseReagent` with `ItemID = 0x3003`, hue `0xB96`, and name `karan crystal`.

| Source | Compiled behavior |
| --- | --- |
| Priest conversion | Dropping `Gold` with `Amount >= 5` on a `Priest` while `GetPlayerInfo.isJedi(from, false)` is true creates `new KaranCrystal(dropped.Amount / 5)` in the player's backpack and deletes the entire gold stack. |
| Exorcism-slayer victim | In `DropRelic`, if `SlayerName.Exorcism` slays the killed creature, the credited killer resolves to a `PlayerMobile`, and `Utility.RandomBool()` passes, an eligible Jedi killer receives a corpse drop of `KaranCrystal(Utility.RandomMinMax(minhs, maxhs))`. `minhs = max(1, from.Fame / 600)` and `maxhs = max(3, from.Fame / 400)`. |
| Datacron charging | Dropping `KaranCrystal` on a `JediSpellbook` adds its amount to `crystals`, capped at `50000`. If the stack would overfill the book, only the needed amount is consumed and the remainder stays in the dropped stack. |
| Lower reagent cost | `JediSpell.GetTithing` and `JediSpell.DrainCrystals` each treat `AosAttribute.LowerRegCost > Utility.Random(100)` as a zero-crystal roll. |

`JediSpell.GetCrystals` and `DrainCrystals` scan all `World.Items` for any `JediSpellbook` whose `owner == caster`; the datacron does not need to be equipped, but it must exist in world item storage and have the caster as owner.

## Datacron Item Transformation
Dropping eligible Items onto a `JediSpellbook` can transform or configure Jedi equipment:

| Dropped Item | Result |
| --- | --- |
| Single `Ruby`, `Amber`, `Amethyst`, `Citrine`, `Emerald`, `Diamond`, `Sapphire`, `StarSapphire`, or `Tourmaline` | Sets `JediSpellbook.gem` to the dropped gem hue, or to a hard-coded default hue when the gem has no hue, then deletes the gem. |
| `MaterialInfo.IsMagicTalisman(dropped)` | Sets `ItemID = 0x543E`, `Name = "Jedi Exacron"`, and hue `0` after the transform effect. |
| Item with name containing `Durasteel`, hue `0x7A9`, and `steel < 1` | Sets `JediSpellbook.steel = 1` and deletes the item. |
| `BaseHat` or `MagicHat` | Converts the hat to a Jedi hood. ItemID toggles between `0x2B71` and `0x4D09`; name becomes `Jedi hood`. |
| `BaseShield` | Sets `ItemID = 0x2B74`; name becomes `Jedi shield`. |
| Item in `Layer.OuterTorso` | Cycles robe art through `0x1F03`, `0x567D`, and `0x2B6A`; name becomes `Jedi robe`. |
| Cloak-layer item with one of the allowed cloak ItemIDs | Sets `ItemID = 0x1515`; name becomes `Jedi cloak`. |

The method blocks artifact alteration when `MyServerSettings.AlterArtifact(dropped)` returns false.

## Shared Casting Rules
All Jedi spells inherit these rules unless the individual spell adds more checks:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Psychology`. |
| Damage skill | `SkillName.Swords`. |
| Clear hands | `ClearHandsOnCast = false`. |
| Cast recovery base | `7`. |
| Karma gate | `Caster.Karma < 0` rejects the cast. |
| Required skill gate | `GetJediSkillMax(Caster) < RequiredSkill` rejects the cast. This helper returns the higher of Swords and Psychology values. |
| Crystal gate | `GetCrystals(Caster) < RequiredTithing` or current tithing rejects the cast. |
| Mana gate | `Caster.Mana < GetMana()` rejects the cast. `GetMana()` returns `ScaleMana(RequiredMana)`. |
| Celerity region gate | If no-mount regions are enabled, `Celerity` is blocked in `AnimalTrainer.IsNoMountRegion`. |
| Cast skill range | `GetCastSkills` returns `RequiredSkill` through `RequiredSkill + 20.0`. |

Shared power formulas:

| Formula | Compiled behavior |
| --- | --- |
| Jedi damage power | `karma = clamp(Caster.Karma, 0, 15000); power = (karma / 120) + Tactics.Value + Swords.Value`. |
| Unused power helper | `ComputePowerValue(from, div) = (int)Math.Sqrt(from.Karma + 20000 + (from.Skills.Psychology.Fixed * 10)) / div`. No Jedi spell calls it. |
| Hidden-mobile reveal chance | `chance = 50 * (JediDamage + Psychology.Fixed) / (target.Hiding.Fixed + target.Stealth.Fixed)`, or `100` when the target divisor is `0`. |

## Power List
| Spell ID | Command | Power | Required skill | Mana | Crystals | Cast delay | Primary behavior |
| ---: | --- | --- | ---: | ---: | ---: | ---: | --- |
| 280 | `[ForceGrip` | Force Grip | 10 | 5 | 6 | 0.5s | Telekinesis/use support for `ITelekinesisable`, containers, and movable loose Items. |
| 281 | `[MindsEye` | Mind's Eye | 20 | 8 | 16 | 0.5s | Reveals nearby traps, hidden doors, hidden traps, hidden chests, and hidden Mobiles around a targeted point. |
| 282 | `[Mirage` | Mirage | 30 | 12 | 24 | 0.75s | Summons a `JediMirage` clone and hides the caster. |
| 283 | `[ThrowSabre` | Throw Sabre | 40 | 16 | 12 | 0.5s | Throws the equipped `BaseSword` at a harmful target. |
| 284 | `[Celerity` | Celerity | 50 | 20 | 80 | 0.5s | Sends mount-speed movement to the caster for a timed duration. |
| 285 | `[PsychicAura` | Psychic Aura | 20 | 24 | 32 | 0.5s | Toggles self resistance mods. |
| 286 | `[Deflection` | Deflection | 70 | 28 | 100 | 3.0s | Sets `Caster.MagicDamageAbsorb` to a random absorb value. |
| 287 | `[SoothingTouch` | Soothing Touch | 10 | 32 | 48 | 0.5s | Beneficial target heal, poison cure, wound/bleed removal attempts. |
| 288 | `[StasisField` | Stasis Field | 50 | 36 | 52 | 0.5s | Paralyzes a harmful target for a calculated duration. |
| 289 | `[Replicate` | Replicate | 100 | 40 | 75 | 0.5s | Creates a resurrection `SoulOrb` named `replication crystal`. |

## Commands
All command handlers are registered at `AccessLevel.Player` and have `[Usage]` plus `[Description]` attributes. Each command checks `Multis.DesignContext.Check(e.Mobile)`, verifies the caster has the matching spell in a compatible spellbook, and sends localized message `500015` when the spell is missing.

| Usage | Description | Spell ID | Handler action |
| --- | --- | ---: | --- |
| `ForceGrip` | Casts Force Grip | 280 | `new ForceGrip(e.Mobile, null).Cast()` |
| `MindsEye` | Casts Minds Eye | 281 | `new MindsEye(e.Mobile, null).Cast()` |
| `Mirage` | Casts Mirage | 282 | `new Mirage(e.Mobile, null).Cast()` |
| `ThrowSabre` | Casts Throw Sabre | 283 | `new ThrowSabre(e.Mobile, null).Cast()` |
| `Celerity` | Casts Celerity | 284 | `new Celerity(e.Mobile, null).Cast()` |
| `PsychicAura` | Casts Psychic Aura | 285 | `new PsychicAura(e.Mobile, null).Cast()` |
| `Deflection` | Casts Deflection | 286 | `new Deflection(e.Mobile, null).Cast()` |
| `SoothingTouch` | Casts Soothing Touch | 287 | `new SoothingTouch(e.Mobile, null).Cast()` |
| `StasisField` | Casts Stasis Field | 288 | `new StasisField(e.Mobile, null).Cast()` |
| `Replicate` | Casts Replicate | 289 | `new Replicate(e.Mobile, null).Cast()` |

No Jedi-specific administrator command was found in the trace.

## Spell Details
### Force Grip
`ForceGrip` assigns a target with range `10` under ML or `12` otherwise. It handles three target categories:

| Target type | Behavior |
| --- | --- |
| `ITelekinesisable` | Turns the caster toward the target and calls `obj.OnTelekinesis(Caster)`. |
| `Container` | Handles inaccessible containers, snooping containers rooted under another Mobile, corpse loot checks, and region double-click permission. Successful region use calls `item.OnItemUsed(Caster, item)`. |
| Loose `Item` | Moves a single movable Item into the caster's backpack when `item.Amount <= 1`, `item.Weight <= Caster.Int / 20`, and `item.RootParentEntity == null`. |

### Mind's Eye
`MindsEye` targets an `IPoint3D`, requires line of sight, and scans two ranges:

| Scan | Range formula | Effects |
| --- | --- | --- |
| Items | `1 + (int)(Psychology.Value / 20.0)` | Detects `BaseTrap`, selected hidden-door `BaseDoor` ItemIDs, `HiddenTrap`, and `HiddenChest`. |
| Mobiles | `1 + (int)(JediDamage / 20.0)` | Reveals hidden Mobiles whose access rules and hide/stealth difficulty check pass. |

Hidden chests are consumed after processing. A detected hidden chest computes `level = clamp((int)(JediDamage / 50), 1, 6)`, randomizes again from `1..level`, then has a two-in-three chance to do nothing. If the surviving level is greater than `4`, it spawns a `HiddenBox` level `1` or `2`; otherwise it spawns `Gold(Utility.RandomMinMax(50, 100) * level)`.

### Mirage
`Mirage` requires the caster to be unmounted, have room for `3` follower slots, and not be under `HorrificBeastSpell`. On success it creates a `JediMirage` at the caster's location, sets `Caster.Hidden = true`, and starts a summon duration of `JediDamage / 2` seconds.

`JediMirage` copies the caster's body, hue, sex, name, title, kills, hair, facial hair, skills, and visible item art. It is a summoned, uncommandable, non-dispellable `BaseCreature` using `AI_Melee`, `FightMode.Closest`, `ControlSlots = 3`, `SetDamage(1, 1)`, physical damage type, physical/fire/cold/poison resist ranges `20..40`, and hits randomly selected between `60` and `JediDamage` with a minimum max of `60`.

### Throw Sabre
`ThrowSabre` requires an equipped `BaseSword` in one-handed or two-handed layers. On a harmful target it sends a moving effect using the sword art and deals AOS damage:

| Damage part | Formula |
| --- | --- |
| Minimum | `equippedSword.MinDamage + 10` |
| Maximum | `equippedSword.MaxDamage + (JediDamage / 7)` |
| Damage roll | `Utility.RandomMinMax(min, max)` |
| Damage types | Uses the sword's AOS physical/cold/fire/energy/poison distribution; if all are `0`, physical becomes `100`. |

### Celerity
`Celerity` rejects mounted casters, `BootsofHermes`, `Artifact_BootsofHermes`, `Artifact_SprintersSandals`, and `HikingBoots` when `Caster.RaceID > 0`. It sends `SpeedControl.MountSpeed`, stores the caster in `TableJediRunning`, starts an `InternalTimer`, and adds a Celerity buff.

Duration is `TotalTime = (int)(JediDamage * 4)`, with a minimum of `600` seconds.

### Psychic Aura
`PsychicAura` is a self-only toggle stored in a static `Hashtable`.

| Resistance | Modifier formula |
| --- | --- |
| Physical | `(int)((Inscribe.Value / 15) + (JediDamage / 50))` |
| Fire | `-5` |
| Cold | `-5` |
| Poison | `-5` |
| Energy | `(int)((Inscribe.Value / 25) + (JediDamage / 75))` |

Casting again removes the stored resistance mods and removes the Psychic Aura buff.

### Deflection
`Deflection` calls `DefensiveSpell.EndDefense(Caster)` and rejects the cast when `Caster.MagicDamageAbsorb > 0`. On success it sets:

```text
Caster.MagicDamageAbsorb = Utility.RandomMinMax(15, (int)(JediDamage / 4))
```

It then applies particles, plays sound `0x0F9`, and adds the Deflection buff.

### Soothing Touch
`SoothingTouch` is a beneficial Mobile target spell. It rejects unseen targets, animated dead creatures, dead bonded pets, and golems.

| Target state | Behavior |
| --- | --- |
| Mortal wound | Ends the wound when `JediDamage > Utility.RandomMinMax(185, 750)`, otherwise sends failure text. |
| Poisoned | Requires `Psychology.Value >= 60.0`, `JediDamage / 2 >= 60.0`, and `((Psychology.Value - 30.0) / 50.0) - (Poison.Level * 0.1) > Utility.RandomDouble()` before calling `m.CurePoison(Caster)`. |
| Bleeding | Ends bleeding when `JediDamage > Utility.RandomMinMax(185, 750)`, otherwise sends failure text. |
| Normal beneficial sequence | Heals `PlayerLevelMod((int)(Psychology.Value * 0.2) + (int)(JediDamage * 0.1) + Utility.Random(1, 10), Caster)`. |

### Stasis Field
`StasisField` is a harmful Mobile target spell. It rejects unseen targets and targets that are already frozen, paralyzed, or casting. It performs reflect handling, then computes:

```text
secs = (int)((JediDamage / 25) - (GetResistSkill(target) / 10) + (Psychology.Value / 2))
if (!Core.SE) secs += 2
if (!target.Player) secs *= 3
if (secs < 0) secs = 0
```

The target is paralyzed for `secs` seconds and receives a Stasis Field buff for the same duration.

### Replicate
`Replicate` scans `World.Items` and deletes every `SoulOrb` whose `m_Owner == Caster`. It then creates a new `SoulOrb`, sets `m_Owner = Caster`, `Name = "replication crystal"`, `ItemID = 0x703`, adds it to the caster's backpack, and calls `SoulOrb.OnSummoned(Caster, iOrb)`.

`SoulOrb.OnSummoned` stores the Mobile/orb pair in a static resurrection dictionary and adds a resurrection buff. On player death, the orb starts a `30` second timer; when it fires, the owner is resurrected, death penalty logic runs, the resurrection buff is removed, and the orb is deleted. `SoulOrb.Deserialize` deletes saved orbs on world load.

## Gumps And Quick Bars
`JediSpellbook.OnDoubleClick` opens `JediSpellbookGump` only when `owner == from` and the book is either directly in the player's backpack or parented to the player.

Main page buttons:

| Button ID | Behavior |
| ---: | --- |
| 2 | Opens the lore/help page. |
| 3 | Opens `JediSpellbook.PowerRow`, the horizontal quick bar. |
| 4 | Opens `JediSpellbook.PowerColumn`, the vertical quick bar. |
| 5 | Toggles `JediSpellbook.names` for vertical quick-bar labels. |
| 6 | Opens the laser sword construction page. |
| 9 | Closes Jedi quick bars. |
| 280-289 | Opens the matching power detail page. |
| 380-389 | Casts the matching learned spell from the main gump. |

The quick bars render only learned spells. Their reply button IDs are the spell IDs, and `OnResponse` dispatches through `JediSpell.CastSpell(from, info.ButtonID)`.

## Laser Sword Construction
The construction page displays build buttons only when all of these checks pass during gump construction:

| Requirement | Compiled condition |
| --- | --- |
| Jedi state | `GetPlayerInfo.isJedi(from, false)` must be true. |
| Skills | Psychology, Tactics, and Swords must each be at least `100.0`. |
| Fame and Karma | `from.Fame >= 15000` and `from.Karma >= 15000`. |
| Region | `Region.Find(from.Location, from.Map).IsPartOf("the Tomb of Zoda the Jedi Master")`. |
| Datacron material | `m_Book.steel >= 1`. |
| Datacron gem | `m_Book.gem >= 1`. |
| Gold | Backpack gold total must be at least `10000`. |

Button `7` creates a `LevelLaserSword`; button `8` creates a `LevelDoubleLaserSword`. The reply handler consumes `10000` gold, sets the sword hue from `m_Book.gem`, sets `from.Fame = 0`, sets `from.Karma = 0`, clears `m_Book.gem` and `m_Book.steel`, generates a personalized sword name, adds the sword to the backpack, logs the creation, and plays effects.

## Serialization
| Class | Versioning behavior |
| --- | --- |
| `JediSpellbook` | Writes version `0`, then `owner`, `crystals`, `page`, `names`, `gem`, and `steel`. Deserialization reads the same fields in the same order. |
| `JediDatacron01` through `JediDatacron10` | Each writes and reads version `0` only. |
| `KaranCrystal` | Writes and reads version `0` only. |
| `JediMirage` | Writes encoded version `0`, then `m_Caster`. Deserialization reads `m_Caster` and calls `MirrorImage.AddClone(m_Caster)`. |
| `SoulOrb` | Writes version `0`; deserialization reads the version and deletes the orb, so replicate crystals do not persist across world load. |

## Known Issues
| Issue | Evidence from trace | Impact |
| --- | --- | --- |
| `Scripts.csproj` references a missing Jedi gump path. | The project includes `Magic\Jedi\JediSpellBookGump.cs`, but the actual source file is `Magic\Jedi\Gumps\JediSpellBookGump.cs`. | A clean project build can miss or fail the Jedi spellbook gump depending on project-file enforcement. |
| The required-skill gate uses the higher of Swords and Psychology. | `CheckCast` and `CheckFizzle` compare `RequiredSkill` against `GetJediSkillMax`, and that helper returns Swords when Swords is greater than Psychology, otherwise Psychology. | A caster can satisfy high-level power requirements with only one of the two intended skills. |
| Crystal spending is duplicated across the shared and spell-specific paths. | `JediSpell.FinishSequence` always calls `DrainCrystals(Caster, RequiredTithing)`, while many successful spell effects also call `DrainCrystals` before finishing. Several target handlers also call `FinishSequence` in both the target method and `OnTargetFinish`. | Successful or even invalid target flows can spend more crystals than the table cost. |
| Some spells bypass standard mana consumption. | `Celerity` and `Replicate` call `CheckFizzle()` directly instead of `CheckSequence()`. `CheckSequence()` is the base path that subtracts mana after a successful fizzle check. | These powers can complete without spending the listed mana cost. |
| Jedi fizzle checks do not perform the base skill roll. | `JediSpell.CheckFizzle` rechecks Karma, skill threshold, crystals, mana, and Celerity region state, but does not call `base.CheckFizzle()` or `Caster.CheckSkill`. | Once thresholds are met, most Jedi powers do not appear to have a normal fizzle chance. |
| Laser sword construction is only fully validated while the page is drawn. | The gump constructor checks Jedi state, skills, region, materials, fame, Karma, and gold before showing buttons. The button reply handler only checks `pack.ConsumeTotal(typeof(Gold), 10000)`. | A stale or spoofed gump reply can craft after prerequisites change, and a null backpack can be dereferenced. |
| `JediSpellbook.OnDragDrop` has no owner guard. | Double-clicking checks `owner == from`, but `OnDragDrop` transforms gear, consumes gems/durasteel/crystals, and calls `from.ProcessClothing()` without verifying the dropper owns the datacron. | Another Mobile with access to the Item may alter or charge someone else's datacron. |
| `Replicate` names its orb differently than `SoulOrb` expects. | `Replicate` creates a `SoulOrb` named `replication crystal`; `SoulOrb` special resurrection/property text checks for `cloning crystal`. | The resurrection still uses `SoulOrb.OnSummoned`, but player-facing text falls back to generic soul-orb wording. |
| `JediMirage` deserialization and deletion lack caster null guards. | `JediMirage.Deserialize` calls `MirrorImage.AddClone(m_Caster)`, and `OnDelete` removes a Mirage buff from `m_Caster` without checking for null. | Loaded or orphaned mirages can pass a null Mobile into clone/buff cleanup paths. |
