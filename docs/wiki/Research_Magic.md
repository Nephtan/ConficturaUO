# Research Magic

## Overview
Research Magic is the Archmage ancient spell system. It combines a player-owned `ResearchBag`, an optional `AncientSpellbook`, sixty-four `ResearchSpell` subclasses, player cast commands, spell-bar commands, and Search quest integration.

The spell IDs are `600` through `663`, and `Spellbook.GetTypeForSpell` maps that range to `SpellbookType.Archmage`. `Initializer` registers each concrete `Server.Spells.Research` class in that ID range.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `ResearchBag` | `Item` | Player-owned research pack. Stores blank scrolls, quill charges, octopus ink, normal Magery/Necromancy scroll research flags, rune flags, ancient spell research flags, prepared ancient scroll counts, and four Archmage toolbar layouts. |
| `ResearchBag.ResearchGump` | `Gump` | Main research UI. Shows resources, discovered research, search clues, scribing actions, prepared spell counts, and toolbar buttons. |
| `ResearchSettings` | static helper class | Chooses bag mode vs book mode, locates a player's `ResearchBag` or `AncientSpellbook`, and transfers saved Archmage toolbar strings from the bag to `PlayerMobile`. |
| `Research` | static helper class | Search location assignment, rune discovery, spell metadata, reagent counting/consumption, normal scroll scribing, ancient spell scribing, and bag-mode command dispatch. |
| `ResearchSpell` | `Spell` subclass | Shared Archmage spell behavior: Inscribe cast/damage skill identity, Magery/Necromancy cast gate, prepared-scroll gate, book reagent consumption, mantra/effects, and power formulas. |
| `AncientSpellbook` | `Spellbook` item | Talisman-layer Archmage spellbook using `BookOffset = 600` and `BookCount = 64`. Stores owner, page supply, quill supply, and owner-name snapshot. |
| `AncientSpellbookGump` | `Gump` | Book UI with school pages, spell detail pages, spell buttons, and embedded Archmage command help. |
| `ResearchCommands` | command registrar | Registers one player command per ancient spell, such as `[CastConjure` and `[CastMassDeath`. |
| `SpellBars*` classes | command/gump classes | Register `[ancient`, `[archspell1..4`, `[archtool1..4`, and `[archclose1..4` for book/bag mode and Archmage quick bars. |

## Acquisition And Research Flow
| Step | Compiled behavior |
| --- | --- |
| Buying a research pack | Dropping exactly `500` gold on a `Scribe`, `Sage`, or `LibrarianGuildmaster` creates a `ResearchBag` if the player has `Inscribe >= 30` and does not already own one. If any existing owned bag is found anywhere in `World.Items`, it is moved back to the player's backpack instead. |
| Bag setup | `SetupBag` assigns `BagOwner`, zeroes scroll/quill/ink counters, assigns initial search locations for ink, Magery, Necromancy, runes, and research, and initializes all research bitfields to `0#...`. |
| Search integration | `SearchBase` checks the player's backpack for an owned `ResearchBag` and calls `Research.SearchResult(from, bag)`. |
| Rune gate | `SearchResult` can always progress the rune search. Magery scroll research, Necromancy scroll research, octopus ink, and ancient spell research only trigger after `Research.GetRunes(bag, 26)` is true. |
| Location matching | Search success compares `from.Region.Name` to the bag's stored location strings. A matching location calls `SetRunes`, `SetWizardry`, `SetNecromancy`, `SetInk`, or `SetResearch`. |
| Ancient spell unlock order | `SetResearch` unlocks the first `0` entry in `ResearchSpells`, so ancient spells are learned in index order from `1` through `64`. |
| Later research location difficulty | After unlocks above `8`, `16`, `24`, `32`, `40`, `48`, and `56`, the next research location uses higher `PickWorld` levels: `0/1`, `0/2`, `1/2`, `2/3`, `3/4`, `4/5`, then `5/7`. |
| Octopus ink quantity | `SetInk` grants base `1` ink, plus one each for Alchemy, Cooking, and Tasting checks against `Utility.RandomMinMax(25, 150)`, plus one for `GetPlayerInfo.LuckyPlayer(from.Luck)`. Bag ink caps at `50000`. |

## Bag And Book Modes
`PlayerMobile.UsingAncientBook` selects whether Research Magic uses a bag or a book. The `[ancient` command toggles that flag and prints either "You are now using the ancient spellbook." or "You are now using the research bag."

| Mode | Access path | Resource behavior |
| --- | --- | --- |
| Research bag mode | Player commands, research gump buttons, and Archmage quick bars use the owned `ResearchBag`. | The caster must have the spell researched and at least one prepared copy in `ResearchPrep1` or `ResearchPrep2`. `ConsumeScroll` can decrement the prepared count after a successful cast. |
| Ancient spellbook mode | `AncientSpellbookGump` creates spells through `SpellRegistry.NewSpell(spellID, from, m_Book)`. | The owned book must contain the spell bit. Casting checks `paper >= 1` and `quill >= 1`. Reagent consumption uses the player's backpack and decrements book paper/quill when the reagent spend succeeds. |

Dropping a normal `Spellbook` or existing `AncientSpellbook` onto an owned `ResearchBag` creates a new `AncientSpellbook`, copies the source book properties with `OnAfterDuped`, assigns the caster as owner, copies all researched ancient spell bits into the new book, deletes the source book, and puts the new book in the caster's backpack.

## Shared Casting Rules
| Rule | Compiled behavior |
| --- | --- |
| Spell IDs | `600..663`, with ancient spell indexes `1..64` stored in `Research.SpellInformation`. |
| Spellbook type | `SpellbookType.Archmage`. |
| Cast skill identity | `ResearchSpell.CastSkill` and `ResearchSpell.DamageSkill` both return `SkillName.Inscribe`. |
| Cast gate | `ResearchSpell.CastingSkill(from)` returns the higher of `Necromancy.Value` and `Magery.Value`; this must be at least the spell's `RequiredSkill`. |
| Damage/power skill | `ResearchSpell.DamagingSkill(from)` returns `(Necromancy.Value + Magery.Value + Spiritualism.Value + Psychology.Value) / 2`. |
| Power helper | `ComputePowerValue(from, div) = floor(sqrt((-Karma) + 20000 + ((Necromancy.Fixed + Magery.Fixed) * 10))) / div`. |
| Mana | `GetMana()` returns `ScaleMana(RequiredMana)`. |
| Cast skill window | `GetCastSkills(out min, out max)` sets `min = RequiredSkill` and `max = RequiredSkill + 40.0`. |
| Hands and recovery | `ClearHandsOnCast = true`; `CastRecoveryBase = 1`. |
| Book reagent reduction | In book mode, if `LowerRegCost > Utility.Random(100)` and the spell's `alwaysConsume` is false, `ConsumeReagents()` succeeds without consuming backpack reagents or book paper/quill. |
| Bag scroll reduction | In bag mode, `ConsumeScroll` rolls `Utility.RandomMinMax(0, high) > LowerRegCost` or consumes automatically. `high` is `200` for Charm and `125` for other spells. |
| Skill gain on bag scroll use | `ConsumeScroll` checks Magery, Necromancy, Spiritualism, and Psychology from `RequiredSkill - 20` through `RequiredSkill + 20`. |
| Karma loss helper | `KarmaMod(from, mod)` awards negative karma equal to `-(70 + mod)`. Many death/destruction spells pass `RequiredSkill + RequiredMana`. |

## Scribing Rules
| Action | Compiled behavior |
| --- | --- |
| Normal scroll scribing | `CreateNormalSpell` checks mana, Inscribe skill, bag ink, quills, blank scrolls, and reagents from `ScrollInformation`. Success spends mana, ink, one blank scroll, one quill, and reagents, then creates the normal scroll item. |
| Ancient scroll scribing | `CreateResearchSpell` checks mana, Inscribe skill, quills, blank scrolls, ancient reagents, and a prepared count below `500`. Success spends mana, one blank scroll, one quill, ancient reagents, and increments the prepared spell count. |
| Scribe-most ancient scrolls | `CreateManySpells` loops until blocked by mana, skill, quills, scrolls, reagents, or prepared count. It spends mana only once per batch because `manaCheck` is set false after the first mana spend. |
| Failure roll | Both normal and ancient scribing fail when `from.Skills[Inscribe].Value < Utility.RandomMinMax(skill - 25, skill + 25)`. Failure may independently consume mana, scrolls, quills, ink or reagents through additional skill-vs-`Utility.RandomMinMax(0, 125)` checks. |
| Prepared storage | Spell indexes `1..32` are stored in `ResearchPrep1`; indexes `33..64` are stored in `ResearchPrep2`. |

## Player Commands
All spell commands are registered at `AccessLevel.Player` and have `[Description("Casts the Ancient Researched Spell")]`. They call `ResearchCommands.CanCast`, which checks for an owned research pack, known spell, and `Multis.DesignContext.Check`.

| Command | Purpose |
| --- | --- |
| `[ancient` | Toggle between research-bag casting and ancient-spellbook casting. |
| `[archspell1`, `[archspell2`, `[archspell3`, `[archspell4` | Open Archmage spell-bar editors. |
| `[archtool1`, `[archtool2`, `[archtool3`, `[archtool4` | Open Archmage quick bars. |
| `[archclose1`, `[archclose2`, `[archclose3`, `[archclose4` | Close the matching Archmage editor and quick-bar gumps. |
| `[Cast*` commands | Cast individual ancient spells listed below. |

## Spell Metadata
`Forced consume` means the spell's metadata sets the `regs` flag used by the spell's `alwaysConsume` override. In bag mode this forces prepared-scroll consumption when the spell passes that value into `ConsumeScroll`; in book mode it bypasses the Lower Reagent Cost skip in `ConsumeReagents`.

| # | Spell | School | Circle | Mana | Skill | Spell ID | Command | Forced consume |
|---:|---|---|---:|---:|---:|---:|---|---|
| 1 | Conjure | Conjuration | 1 | 10 | 15 | 610 | `[CastConjure` | Yes |
| 2 | Death Speak | Death | 1 | 5 | 10 | 614 | `[CastDeathSpeak` | No |
| 3 | Sneak | Enchanting | 1 | 10 | 10 | 648 | `[CastSneak` | No |
| 4 | Create Fire | Sorcery | 1 | 5 | 15 | 611 | `[CastCreateFire` | No |
| 5 | Electrical Elemental | Summoning | 1 | 40 | 70 | 655 | `[CastElectricalElemental` | No |
| 6 | Confusion Blast | Thaumaturgy | 1 | 15 | 40 | 609 | `[CastConfusionBlast` | No |
| 7 | See Truth | Theurgy | 1 | 60 | 20 | 645 | `[CastSeeTruth` | No |
| 8 | Icicle | Wizardry | 1 | 10 | 15 | 632 | `[CastIcicle` | No |
| 9 | Extinguish | Conjuration | 2 | 20 | 35 | 624 | `[CastExtinguish` | No |
| 10 | Rock Flesh | Death | 2 | 10 | 15 | 644 | `[CastRockFlesh` | No |
| 11 | Mass Might | Enchanting | 2 | 99 | 66 | 638 | `[CastMassMight` | No |
| 12 | Endure Cold | Sorcery | 2 | 15 | 20 | 619 | `[CastEndureCold` | No |
| 13 | Weed Elemental | Summoning | 2 | 40 | 70 | 660 | `[CastWeedElemental` | No |
| 14 | Spawn Creatures | Thaumaturgy | 2 | 20 | 45 | 652 | `[CastSpawnCreature` | No |
| 15 | Healing Touch | Theurgy | 2 | 15 | 30 | 631 | `[CastHealingTouch` | No |
| 16 | Snow Ball | Wizardry | 2 | 10 | 10 | 649 | `[CastSnowBall` | No |
| 17 | Clone | Conjuration | 3 | 25 | 45 | 607 | `[CastClone` | No |
| 18 | Grant Peace | Death | 3 | 35 | 75 | 629 | `[CastGrantPeace` | No |
| 19 | Sleep | Enchanting | 3 | 15 | 40 | 646 | `[CastSleep` | No |
| 20 | Endure Heat | Sorcery | 3 | 15 | 20 | 620 | `[CastEndureHeat` | No |
| 21 | Ice Elemental | Summoning | 3 | 40 | 70 | 657 | `[CastIceElemental` | No |
| 22 | Ethereal Travel | Thaumaturgy | 3 | 20 | 35 | 622 | `[CastEtherealTravel` | No |
| 23 | Wizard Eye | Theurgy | 3 | 30 | 50 | 663 | `[CastWizardEye` | No |
| 24 | Frost Field | Wizardry | 3 | 15 | 30 | 627 | `[CastFrostField` | No |
| 25 | Create Gold | Conjuration | 4 | 35 | 55 | 612 | `[CastCreateGold` | Yes |
| 26 | Animate Bones | Death | 4 | 40 | 70 | 653 | `[CastAnimateBones` | No |
| 27 | Cause Fear | Enchanting | 4 | 35 | 45 | 605 | `[CastCauseFear` | No |
| 28 | Ignite | Sorcery | 4 | 30 | 40 | 633 | `[CastIgnite` | No |
| 29 | Mud Elemental | Summoning | 4 | 40 | 70 | 658 | `[CastMudElemental` | No |
| 30 | Banish Daemon | Thaumaturgy | 4 | 40 | 80 | 603 | `[CastBanishDaemon` | No |
| 31 | Fade from Sight | Theurgy | 4 | 15 | 50 | 625 | `[CastFadefromSight` | No |
| 32 | Gas Cloud | Wizardry | 4 | 25 | 45 | 621 | `[CastGasCloud` | No |
| 33 | Swarm | Conjuration | 5 | 15 | 40 | 661 | `[CastSwarm` | No |
| 34 | Mask of Death | Death | 5 | 70 | 90 | 636 | `[CastMaskofDeath` | No |
| 35 | Enchant | Enchanting | 5 | 45 | 75 | 618 | `[CastEnchant` | No |
| 36 | Flame Bolt | Sorcery | 5 | 15 | 30 | 626 | `[CastFlameBolt` | No |
| 37 | Gem Elemental | Summoning | 5 | 50 | 80 | 656 | `[CastGemElemental` | No |
| 38 | Call Destruction | Thaumaturgy | 5 | 25 | 40 | 604 | `[CastCallDestruction` | Yes |
| 39 | Divination | Theurgy | 5 | 30 | 50 | 617 | `[CastDivination` | No |
| 40 | Frost Strike | Wizardry | 5 | 40 | 67 | 628 | `[CastFrostStrike` | No |
| 41 | Magic Steed | Conjuration | 6 | 30 | 50 | 635 | `[CastMagicSteed` | No |
| 42 | Create Golem | Death | 6 | 40 | 70 | 613 | `[CastCreateGolem` | No |
| 43 | Sleep Field | Enchanting | 6 | 30 | 60 | 647 | `[CastSleepField` | No |
| 44 | Conflagration | Sorcery | 6 | 20 | 35 | 608 | `[CastConflagration` | No |
| 45 | Acid Elemental | Summoning | 6 | 50 | 82 | 650 | `[CastAcidElemental` | No |
| 46 | Meteor Shower | Thaumaturgy | 6 | 40 | 70 | 640 | `[CastMeteorShower` | No |
| 47 | Intervention | Theurgy | 6 | 25 | 50 | 634 | `[CastIntervention` | No |
| 48 | Hail Storm | Wizardry | 6 | 25 | 55 | 630 | `[CastHailStorm` | No |
| 49 | Aerial Servant | Conjuration | 7 | 50 | 80 | 600 | `[CastAerialServant` | No |
| 50 | Open Ground | Death | 7 | 65 | 85 | 641 | `[CastOpenGround` | Yes |
| 51 | Charm | Enchanting | 7 | 60 | 82 | 606 | `[CastCharm` | No |
| 52 | Explosion | Sorcery | 7 | 30 | 60 | 623 | `[CastExplosion` | No |
| 53 | Poison Elemental | Summoning | 7 | 50 | 86 | 659 | `[CastPoisonElemental` | No |
| 54 | Invoke Devil | Thaumaturgy | 7 | 60 | 95 | 654 | `[CastInvokeDevil` | No |
| 55 | Air Walk | Theurgy | 7 | 55 | 65 | 601 | `[CastAirWalk` | No |
| 56 | Avalanche | Wizardry | 7 | 40 | 70 | 602 | `[CastAvalanche` | No |
| 57 | Death Vortex | Conjuration | 8 | 60 | 90 | 615 | `[CastDeathVortex` | No |
| 58 | Withstand Death | Death | 8 | 70 | 90 | 662 | `[CastWithstandDeath` | Yes |
| 59 | Mass Sleep | Enchanting | 8 | 50 | 85 | 639 | `[CastMassSleep` | Yes |
| 60 | Ring of Fire | Sorcery | 8 | 55 | 85 | 643 | `[CastRingofFire` | No |
| 61 | Blood Elemental | Summoning | 8 | 50 | 90 | 651 | `[CastBloodElemental` | No |
| 62 | Armageddon | Thaumaturgy | 8 | 80 | 100 | 616 | `[CastDevastation` | No |
| 63 | Restoration | Theurgy | 8 | 50 | 80 | 642 | `[CastRestoration` | No |
| 64 | Mass Death | Wizardry | 8 | 55 | 90 | 637 | `[CastMassDeath` | Yes |

## Spell Effect Families
| Family | Compiled mechanics |
| --- | --- |
| Direct damage spells | Most direct damage spells use `DamagingSkill(Caster)` divided by a spell-specific divisor, apply spell-specific minimum/maximum clamps, pass the base value through `GetNewAosDamage`, then call `SpellHelper.Damage` with school-specific damage-type splits. |
| Area damage spells | Area spells gather nearby valid harmful targets, often double the base damage and divide by target count when more than one target is hit. Several use `IPooledEnumerable` explicitly and free it; several field/item scans do not. |
| Field spells | `FrostField`, `Conflagration`, `SleepField`, `CreateFire`, `RingOfFire`, and `OpenGround` create temporary `Item` instances that damage, block, paralyze, or gate movement through `OnMoveOver`, `OnMovement`, or repeating `Timer` logic. |
| Summoning spells | Summons check follower capacity before casting. Most elemental or creature summons use `DamagingSkill(Caster) * 6` seconds; `AerialServant` uses minutes, and several spell-specific classes set `ControlSlots` before `SpellHelper.Summon` or `BaseCreature.Summon`. |
| Buff and transformation spells | Buffs use `ResistanceMod`, `StatMod`, `SkillMod`, body/hue changes, or custom static tables. Durations are usually based on `DamagingSkill(Caster)`. |
| Travel and information spells | `EtherealTravel` integrates with `RunebookGump`, runebook entries, travel checks, charges, and pet movement. `SeeTruth`, `WizardEye`, and `Divination` open information gumps or reveal target state rather than dealing damage. |
| Catastrophic spells | `Armageddon` targets mobiles within 20 tiles in the caster's region and line of sight, kills players/controlled creatures with `10000` damage, deletes uncontrolled `BaseCreature` targets, marks criminal/kills for player/vendor style victims, and starts a three-second caster death timer. |

## Serialization
| Class | Versioning behavior |
| --- | --- |
| `ResearchBag` | Writes version `0`, then owner, resource counters, current page, search locations/worlds/messages, Magery/Necromancy research flags, rune flags, ancient research flags, prepared spell strings, and four toolbar strings. `Deserialize` reads the same fields without version branching. |
| `AncientSpellbook` | Writes version `0`, then `owner`, `paper`, `quill`, and `names`. `Deserialize` reads the same fields without version branching. |
| Field/effect items | Several spell-created items implement their own version `0` serializers. Some restore timers on load; others delete or restart short cleanup timers. |
| Runtime tables | Buff/effect tables such as `ResearchRockFlesh.TableStoneFlesh` and other spell timer tables are static runtime state and are not world-save persisted. |

## Known Issues
| Issue | Evidence from code trace | Impact |
| --- | --- | --- |
| `Scripts.csproj` points at the wrong gump path. | The project includes `Magic\Research\AncientSpellBookGump.cs`, but the traced class file is `Magic\Research\Gumps\AncientSpellBookGump.cs`. | Solution/project builds can miss or fail the Archmage book gump unless another file is generated outside the traced tree. |
| Direct `[Cast*` commands require a `ResearchBag` even in ancient-spellbook mode. | `ResearchCommands.CanCast` returns false when `ResearchSettings.ResearchMaterials(from) == null` before checking `ResearchSettings.HasSpell`. | A player with an owned, populated `AncientSpellbook` but no owned `ResearchBag` can cast from the book gump but not from typed spell commands or quick-bar paths that rely on those commands. |
| Backpack null assumptions are widespread. | `GetAncientTome`, `ResearchMaterials`, `ResearchTransfer`, `CheckReagents`, `RegCount`, and some spell-specific checks call `m.Backpack` or `from.Backpack` directly. | Non-player casters, corrupted players, or future scripted calls without a backpack can throw instead of failing cleanly. |
| Search location assignment can break when no `SearchBase` target exists for a picked world. | `FindLocation` counts matching targets, then calls `Utility.RandomMinMax(1, aCount)` without guarding `aCount == 0`. | Missing world search markers can produce invalid random bounds during bag setup or follow-up research location assignment. |
| Search result checks assume a non-null region. | `SearchResult` reads `from.Region.Name` for every location comparison. | Calls from a Mobile without a region can null-reference. |
| Several range scans do not free pooled enumerables. | `ResearchMassMight`, `ResearchCreateFire`, `ResearchOpenGround`, `ResearchFrostField.InternalTimer`, and `ResearchConflagration.InternalTimer` iterate range results with `foreach` and no explicit `Free()`. | Repeated field ticks or casts can leak pooled enumerable resources in the RunUO server. |
| `Armageddon` metadata and actual forced consumption disagree. | The spell description says the scroll always crumbles, but index `62` sets `much = "true"` and does not set `regs = "true"`, while `ResearchDevastation` passes the default `alwaysConsume` into `ConsumeScroll`. | Bag-mode Armageddon prepared scrolls can benefit from the normal Lower Reagent Cost skip instead of always consuming. |
| `ResearchRockFlesh` contains inline fix/band-aid commentary and altered consumption logic. | The file includes comments describing a prior NullReferenceException, redundant checks, and an adjusted `ConsumeScroll` argument using `Scroll != null`. | The spell is functional enough to document, but the implementation is visibly patched and should be reviewed before further changes. |
