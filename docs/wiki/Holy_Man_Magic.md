# Holy Man Magic

## Overview
Holy Man Magic is a custom `SpellbookType.HolyMan` spell line using spell IDs `770` through `783`. The system is built around a bound `HolyManSpellbook`, a bound `HolySymbol` piety talisman, fourteen `SpellScroll` holy symbol items, player cast commands, a custom spellbook `Gump`, priest-grave speech triggers, and the shared `HolyManSpell` base class.

The spell line uses `SkillName.Spiritualism` as `CastSkill` and `SkillName.Healing` as `DamageSkill`. Casters need at least `2500` Karma, enough Spiritualism for the prayer, enough mana, and enough piety stored on a `HolySymbol` owned by the caster.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `HolyManSpell` | `Spell` subclass | Shared Karma, Spiritualism, piety, mana, mantra, fizzle, and power-value rules. |
| `HolyManSpellbook` | `Spellbook` subclass | Bound prayer book. Uses `SpellbookType.HolyMan`, `BookOffset = 770`, and `BookCount = 15`. Opens `HolyManSpellbookGump` only for its `owner`. |
| `HolyManSpellbookGump` | `Gump` | Lists learned prayers, displays clue/help pages, and casts learned spells by button IDs `770` through `783`. |
| `HolySymbol` | `MagicTalisman` subclass | Bound piety store. Equips only for its `owner` and displays `BanishedEvil` as piety. |
| `HolyManSymbol770` through `HolyManSymbol783` | `SpellScroll` subclasses | Grave-granted symbols that teach spell IDs `770` through `783` to the Holy Man spellbook. |
| `HolyManCommands` | command registrar | Registers one player command per prayer. |
| `MalletStake` | `Item` | Vampire corpse staking reward tracker used to qualify for the initial book and symbol. |
| `Priest` | vendor `Mobile` | Accepts `MalletStake` items, pays the tracked reward, and grants or retrieves a Holy Man book and symbol when priest requirements are met. |
| `MagicForges` | speech-reactive `Item` | Handles `Priest Grave` speech mantras and creates the matching spell symbol. |
| `BaseCreature` death logic | `Mobile` death/loot code | Converts eligible undead or demon coin value into piety when the killer has a `HolySymbol` equipped in `Layer.Talisman`. |

`Initializer` registers spell IDs `770` through `783`, and `Spellbook.GetTypeForSpell` maps `770 <= spellID < 784` to `SpellbookType.HolyMan`. `Scripts.csproj` includes the Holy Man base, command, book, symbol, stake, and spell files, but its gump include path is incorrect; see Known Issues.

## Acquisition
### Becoming A Priest
`MalletStake` can appear in dungeon loot. It tracks `VampiresSlain`, which is actually the gold reward value accumulated from targeted vampire corpses.

| Corpse owner type | Stake value added |
| --- | ---: |
| `VampireWoods` | 10 |
| `Vampire` | 20 |
| `VampireLord` | 40 |
| `VampirePrince` | 60 |
| `Dracula` | 400 |
| `VampiricDragon` | 500 |

The stake value is capped at `10000`. After a valid corpse is staked, the corpse's `VisitedByTaxidermist` flag is set, preventing repeat use by the same stake/taxidermy flag path.

Dropping a `MalletStake` on a `Priest` pays `Gold(reward)` when `reward > 0`, then deletes the stake. If all of the following are true, the priest also grants or retrieves the Holy Man assets:

| Requirement | Compiled condition |
| --- | --- |
| Stake reward | `stake.VampiresSlain >= 1000` |
| Karma | `from.Karma >= 2500` |
| Spiritualism | `from.Skills[SkillName.Spiritualism].Base > 0` |
| Healing | `from.Skills[SkillName.Healing].Base > 0` |

The priest scans all `World.Items` for any `HolySymbol` or `HolyManSpellbook` whose `owner == from` and moves those items to the player's backpack. Missing assets are created as `new HolySymbol(from)` and `new HolyManSpellbook((ulong)0, from)`.

### Learning Prayers
Prayer symbols are learned by dropping the matching `HolyManSymbol###` item onto the player's `HolyManSpellbook`. This uses the shared `Spellbook.OnDragDrop` path: the dropped item must be a single `SpellScroll`, its `SpellID` must map to `SpellbookType.HolyMan`, the book must not already have the spell, and the ID must fall inside the book's offset/count range. On success, the book content bit is set, the book count is incremented, the symbol is deleted, and a sound is played.

`MagicForges.OnSpeech` grants the prayer symbols. The speaker must be a player, within range `10` of a `MagicForges` item named `Priest Grave`, must speak the matching mantra text, must have `Karma >= 2500`, and must already have at least one owned `HolySymbol` or `HolyManSpellbook` somewhere in `World.Items`. Before creating a symbol, the code deletes every existing item of that symbol class in `World.Items`.

| Grave X check | Priest clue name | Spoken mantra | Symbol item | Spell ID | Prayer |
| ---: | --- | --- | --- | ---: | --- |
| 4378 | Patriarch Morden | `exilium` | `HolyManSymbol770` | 770 | Banish |
| 926 | Archbishop Halyrn | `accipe spiritum` | `HolyManSymbol771` | 771 | Dampen Spirit |
| default branch | Bishop Leantre | `fascinare` | `HolyManSymbol772` | 772 | Enchant |
| 2983 | Deacon Wilems | `malleo fidei` | `HolyManSymbol773` | 773 | Hammer of Faith |
| 3082 | Drumat the Apostle | `caelesti lumine` | `HolyManSymbol774` | 774 | Heavenly Light |
| 805 | Vincent the Priest | `famem prohibere` | `HolyManSymbol775` | 775 | Nourish |
| 1398 | Abigayl the Preacher | `deiectionem` | `HolyManSymbol776` | 776 | Purge |
| 4167 | Cardinal Greggs | `reditus vitae` | `HolyManSymbol777` | 777 | Rebirth |
| 981 | Father Michal | `sacrum munus` | `HolyManSymbol778` | 778 | Sacred Boon |
| 3249 | Sister Tiana | `benedicite` | `HolyManSymbol779` | 779 | Sanctify |
| 2697 | Brother Kurklan | `spiritus mundi` | `HolyManSymbol780` | 780 | Seance |
| 1854 | Edwin the Pope | `percutiat` | `HolyManSymbol781` | 781 | Smite |
| 1756 | Xephyn the Monk | `tactus vitae` | `HolyManSymbol782` | 782 | Touch of Life |
| 2163 | Chancellor Davis | `igne iudicii` | `HolyManSymbol783` | 783 | Trial by Fire |

## Piety Economy
Piety is stored as `HolySymbol.BanishedEvil`.

`BaseCreature` death logic awards piety when all of these checks pass:

| Rule | Compiled behavior |
| --- | --- |
| Eligible victim | The killed creature must be slain by `SlayerName.Silver` or `SlayerName.Exorcism`. |
| Credited killer | The source is `this.LastKiller`; if that is a `BaseCreature`, the code uses its master. |
| Credited player | The credited killer must be a `PlayerMobile`. |
| Required item | The credited player must have a `HolySymbol` equipped in `Layer.Talisman`. |
| Piety value | Added piety equals `this.TotalGold + ceil(DDCopper / 10.0) + ceil(DDSilver / 5.0)`. |
| Cap | `BanishedEvil` is capped at `100000`. |
| Coin cleanup | `Gold`, `DDCopper`, and `DDSilver` found in the killed creature's backpack are deleted after piety is added. |

Casting does not require the symbol to be equipped or in the caster's backpack. `HolyManSpell.GetSoulsInSymbol` and `DrainSoulsInSymbol` scan all `World.Items` for any `HolySymbol` whose `owner == caster`.

Lower reagent cost affects piety in two places. `GetTithing` returns `0` when `AosAttributes.LowerRegCost > Utility.Random(100)`, otherwise it returns the spell's `RequiredTithing`. `DrainSoulsInSymbol` makes its own lower-reg-cost roll before subtracting piety.

## Shared Casting Rules
All Holy Man prayers inherit these shared rules unless an individual spell overrides them:

| Rule | Compiled behavior |
| --- | --- |
| Cast skill | `SkillName.Spiritualism` |
| Damage/effect skill | `SkillName.Healing` |
| Cast delay | Every traced prayer returns `TimeSpan.FromSeconds(3)` for `CastDelayBase`. |
| Cast recovery | `CastRecoveryBase = 7` |
| Hand clearing | `ClearHandsOnCast = false` |
| Karma gate | `CheckCast` and `CheckFizzle` reject `Caster.Karma < 2500`. |
| Skill gate | Caster's Spiritualism value must be at least the prayer's `RequiredSkill`. |
| Piety gate | `CheckCast` requires at least the prayer's full `RequiredTithing`; `CheckFizzle` uses `GetTithing`, which can become `0` from lower reagent cost. |
| Mana gate | Caster must have at least `GetMana()`, which returns `ScaleMana(RequiredMana)`. |
| Cast skill range | `GetCastSkills` returns `RequiredSkill` to `RequiredSkill + 40.0`. |
| Power helper | `ComputePowerValue(from, div) = (int)Math.Sqrt((from.Karma * -1) + 20000 + (from.Skills.Spiritualism.Fixed * 10)) / div`. |

## Prayer Catalog
The "Code piety" column is the actual `RequiredTithing` value. The "Gump piety" column is what the prayer-book help page displays; mismatches are tracked in Known Issues.

| ID | Prayer | Mantra | Command | Spiritualism | Mana | Code piety | Gump piety | Compiled effect |
| ---: | --- | --- | --- | ---: | ---: | ---: | ---: | --- |
| 770 | Banish | `Exilium` | `[HMBanish` | 60.0 | 30 | 30 | 120 | Harmful target. Rejects players, non-undead/non-demon targets, and bonded creatures. Non-dispellable demons and targets with `Fame >= 23000` rebound `GetNewAosDamage(48, 1, 5, Caster)` as 100% energy damage to the caster. Other valid undead/demon targets are deleted after a 1.5 second timer. |
| 771 | Dampen Spirit | `Accipe Spiritum` | `[HMDampenSpirit` | 70.0 | 35 | 30 | 140 | Harmful target. Positive-Karma targets are not drained. Otherwise transfers `GetDamageSkill(Caster) - GetResistSkill(target)` mana, halved against non-player targets, clamped by target mana and caster missing mana. Clears target paralysis and buff icons. |
| 772 | Enchant | `Fascinare` | `[HMEnchant` | 90.0 | 45 | 40 | 180 | Targets a `BaseWeapon` in the caster's backpack. Creates an `EnchantSpellStone`, stores original weapon name, serial, hue, damage, and slayers, then sets hue `0x9C4`, appends `[enchanted]`, adds `50` weapon damage, and sets `SlayerName.Silver` plus `SlayerName.Exorcism`. Duration is `min((int)Healing, 100)` minutes. |
| 773 | Hammer of Faith | `Malleo Fidei` | `[HMHammerFaith` | 50.0 | 25 | 15 | 100 | Creates a temporary owner-only `WarHammer` in the caster's backpack. The hammer is blessed for the owner, has Silver and Exorcism slayers, `LowerStatReq = 100`, `+10` Bludgeoning, `Supremely` accuracy, `Vanq` damage, and `+30` attack chance. Duration is `(int)(Healing / 5.0)` minutes. |
| 774 | Heavenly Light | `Caelesti Lumine` | `[HMHeavenlyLight` | 10.0 | 5 | 20 | 20 | Beneficial target. Starts `LightCycle.NightSightTimer`, sets `LightLevel` from dungeon light level multiplied by Healing/100, adds the night-sight buff, and drains piety through `GetTithing`. Under AOS it uses the target's Healing value; otherwise it uses the caster's Healing value. |
| 775 | Nourish | `Famem Prohibere` | `[HMNourish` | 10.0 | 5 | 20 | 20 | Beneficial `PlayerMobile` target. Adds `(int)(Caster.Healing / 5)` to `Hunger` and `Thirst`, capping both at `20`. |
| 776 | Purge | `Deiectionem` | `[HMPurge` | 40.0 | 20 | 15 | 80 | Beneficial target. Plays purge effects and calls `Server.Spells.Chivalry.RemoveCurseSpell.RemoveBadThings(target)`. |
| 777 | Rebirth | `Reditus Vitae` | `[HMRebirth` | 80.0 | 40 | 40 | 400 | Beneficial range-1 target. Self-targeting deletes existing owned `SoulOrb` items, creates a new one, adds it to the caster's backpack, and registers it with `SoulOrb.OnSummoned`. Dead `PlayerMobile` targets receive a `ResurrectGump`; dead `BaseCreature` targets send a `PetResurrectGump` to `pet.GetMaster()`. Henchman item targets reset `HenchDead` and restore the item name. |
| 778 | Sacred Boon | `Sacrum Munus` | `[HMSacredBoon` | 20.0 | 10 | 40 | 40 | Beneficial target. Adds a 30 second heal aura in a static table. Every 4 seconds it heals `PlayerLevelMod((int)(1 + Healing / 25.0 + Spiritualism / 25.0), target)`. |
| 779 | Sanctify | `Benedicite` | `[HMSanctify` | 30.0 | 15 | 60 | 60 | Self transformation-style buff. Adds Str, Dex, Int, Parry, Tactics, and Anatomy by `PlayerLevelMod((int)(Healing / 25 + Spiritualism / 25), Caster)`. Effect timer is `(int)(Healing + Spiritualism / 2)` seconds. |
| 780 | Seance | `Spiritus Mundi` | `[HMSeance` | 60.0 | 30 | 40 | 120 | Self transformation. Requires the caster to dismount, not be transformed, not be disguised, and not already be under Seance. Sets body `970`, hue `0x9C4`, and `Blessed = true`. Duration is capped at `100` seconds from `(int)(Healing + Spiritualism / 2)`, then restores body/hue, calls `RaceBody()`, clears Blessed, and ends the action. |
| 781 | Smite | `Percutiat` | `[HMSmite` | 40.0 | 20 | 15 | 80 | Harmful target. Deals 100% energy damage equal to `GetNewAosDamage(23, 1, 4, target) + nBenefit`, where `nBenefit = (int)(Healing / 10 + Spiritualism / 10)`. The benefit is doubled against Silver or Exorcism slayer targets. |
| 782 | Touch of Life | `Tactus Vitae` | `[HMTouchLife` | 20.0 | 10 | 20 | 40 | Beneficial target. Heals and restores stamina by `PlayerLevelMod(1 + (int)(Healing / 10 + Spiritualism / 10), Caster)`. |
| 783 | Trial by Fire | `Igne Iudicii` | `[HMTrialFire` | 30.0 | 15 | 50 | 500 | Self defensive prayer. Ends existing defensive spell state, rejects if `MagicDamageAbsorb > 0`, then sets `MagicDamageAbsorb = (int)((Healing + Spiritualism) / 4)` and plays holy-flame effects. |

## Player Commands
Each direct prayer command is registered at `AccessLevel.Player`, checks `Multis.DesignContext.Check`, then verifies the caster has the matching spell in a spellbook with `Spellbook.Find(from, spellID)` and `book.HasSpell(spellID)`.

| Command | Usage | Description | Spell ID |
| --- | --- | --- | ---: |
| `[HMBanish` | `HMBanish` | Casts Banish | 770 |
| `[HMDampenSpirit` | `HMDampenSpirit` | Casts Dampen Spirit | 771 |
| `[HMEnchant` | `HMEnchant` | Casts Enchant | 772 |
| `[HMHammerFaith` | `HMHammerFaith` | Casts Hammer of Faith | 773 |
| `[HMHeavenlyLight` | `HMHeavenlyLight` | Casts Heavenly Light | 774 |
| `[HMNourish` | `HMNourish` | Casts Nourish | 775 |
| `[HMPurge` | `HMPurge` | Casts Purge | 776 |
| `[HMRebirth` | `HMRebirth` | Casts Rebirth | 777 |
| `[HMSacredBoon` | `HMSacredBoon` | Casts Sacred Boon | 778 |
| `[HMSanctify` | `HMSanctify` | Casts Sanctify | 779 |
| `[HMSeance` | `HMSeance` | Casts Seance | 780 |
| `[HMSmite` | `HMSmite` | Casts Smite | 781 |
| `[HMTouchLife` | `HMTouchLife` | Casts Touch of Life | 782 |
| `[HMTrialFire` | `HMTrialFire` | Casts Trial by Fire | 783 |

The global spell-bar system also registers Holy Man toolbar commands:

| Command | Usage | Description |
| --- | --- | --- |
| `[holyspell1` | `holyspell1` | Opens Spell Bar Editor For Prayers - 1. |
| `[holyspell2` | `holyspell2` | Opens Spell Bar Editor For Prayers - 2. |
| `[holytool1` | `holytool1` | Opens Spell Bar For Prayers - 1. |
| `[holytool2` | `holytool2` | Opens Spell Bar For Prayers - 2. |
| `[holyclose1` | `holyclose1` | Close Spell Bar Windows For Prayers - 1. |
| `[holyclose2` | `holyclose2` | Close Spell Bar Windows For Prayers - 2. |

## Serialization Notes
| Class | Version | Serialized fields and load behavior |
| --- | ---: | --- |
| `HolyManSpellbook` | 0 | Writes `owner` after the version. Reads the same `Mobile`. |
| `HolySymbol` | 1 | Writes `owner` and `BanishedEvil`. Reads both unconditionally. |
| `HolyManSymbol770` through `HolyManSymbol783` | 0 | Each symbol writes only a version integer after base serialization. |
| `MalletStake` | 0 | Writes `VampiresSlain`. |
| `EnchantSpellStone` | 1 | Writes enchant owner, weapon serial, original name, damage, hue, and both slayers. On deserialize it starts a 10 second timer that calls `EnchantSpell.EndEffects(null)`. |
| `HammerOfFaith` | 0 | Writes owner and expiration `DateTime`, then starts a timer on deserialize. |
| `SoulOrb` | 0 | Writes only a version integer and deletes itself on deserialize, so Rebirth's death-protection item does not survive a world restart. |

Temporary `SacredBoon`, `Sanctify`, `Seance`, and `TrialByFire` effects are timer/static/mobile-state effects and do not have their own persistent item serialization.

## Known Issues
* `Scripts.csproj` includes `Magic\Holy Man\HolyManSpellBookGump.cs`, but the traced file is `Magic\Holy Man\Gumps\HolyManSpellBookGump.cs`. Unless another copy exists outside the trace, the `HolyManSpellbookGump` type is not compiled by the maintained project file.
* Piety charging has overlapping paths. `HolyManSpell.FinishSequence()` always calls `DrainSoulsInSymbol(Caster, RequiredTithing)`, while most spell success branches also call `DrainSoulsInSymbol`. Many target classes also call `FinishSequence()` in both the spell target method and `OnTargetFinish`, so successful, failed, or canceled targeting paths can charge more piety than the listed `RequiredTithing`.
* The prayer-book gump displays stale piety requirements for Banish, Dampen Spirit, Enchant, Hammer of Faith, Purge, Rebirth, Seance, Smite, Touch of Life, and Trial by Fire. The actual compiled `RequiredTithing` values are much lower for those spells.
* `GetSoulsInSymbol` returns the last matching owned `HolySymbol` found in `World.Items`, while `DrainSoulsInSymbol` subtracts from every matching owned symbol. Duplicate owned symbols can make the availability check and drain behavior diverge.
* `HolySymbol` writes version `1`, but `Deserialize` unconditionally reads `owner` and `BanishedEvil` with no version switch for older saves.
* `EnchantSpell.Target` dereferences `Caster.Backpack` before checking for null. It also checks `CanBeginAction(typeof(EnchantSpell))` but never calls `BeginAction`, relying instead on the temporary orb being in the backpack. On restart, `EnchantSpellStone.Deserialize` calls `EndEffects(null)`, which deletes every enchant orb but only restores the last stored weapon serial in that pass.
* `RebirthSpell` assumes `pet.GetMaster()` is non-null before opening `PetResurrectGump`; a dead `BaseCreature` without a master can null-reference.
* `SanctifySpell` starts its timer with seconds but adds its buff with `TimeSpan.FromMinutes((int)span)`, so the displayed buff duration does not match the actual effect duration.
* `SeanceSpell.OnCast` calls `CheckSequence()` twice on the success path and leaves its `m_Timers` entry behind when the timer expires naturally. It also restores by setting `Blessed = false`, which can overwrite another source of blessed state.
* `SacredBoonSpell.Target` reports that an unseen target cannot be seen, but then continues into the duplicate/effect checks because the next condition is not chained with `else`.
* `BanishEvilSpell` casts the target to `BaseCreature` but dereferences `bc.IsBonded` and `bc.IsDispellable` without checking whether the cast succeeded.
* `MalletStake.CorpseTarget` calls `c.Owner.GetType()` without checking whether the corpse owner is null.
