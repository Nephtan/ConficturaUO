# Skill Scripts

## Overview
The Skill Scripts system is the shard's direct skill-use layer plus the shared skill-gain engine. Core skill storage, caps, locks, titles, and `SkillInfo` metadata live in `Data/System/Source/Skills.cs`. Direct player skill buttons are wired by scripts under `Data/Scripts/System/Skills/`, where `Initialize()` methods assign `SkillInfo.Table[skillID].Callback`.

Not every `SkillName` has a direct-use script. Craft, harvest, combat, magic, and passive skills often gain through their own subsystems and call `Mobile.CheckSkill`, `Mobile.CheckTargetSkill`, or `Mobile.CheckLocationSkill` without exposing a skill-button callback.

## Source Files
| File | Type | Purpose |
| --- | --- | --- |
| `Data/System/Source/Skills.cs` | Core engine | Defines `SkillName`, `Skill`, `Skills`, `SkillInfo`, direct-use dispatch, skill storage, title metadata, and skill serialization. |
| `Data/Scripts/System/Skills/SkillCheck.cs` | Shared gain engine | Installs the `Mobile` skill-check handlers through XMLSpawner hooks, calculates success and gain chances, applies anti-macro checks, and raises skills/stats. |
| `Data/Scripts/System/Skills/Anatomy.cs` | Skill callback | Inspects Mobile strength, dexterity, and stamina. |
| `Data/Scripts/System/Skills/ArmsLore.cs` | Skill callback | Identifies weapons, armor, barded swamp dragons, relic values, and `UnidentifiedItem` containers requiring Arms Lore. |
| `Data/Scripts/System/Skills/Begging.cs` | Skill callback | Toggles begging demeanor, begs attackers to stop, or takes small gold amounts from human NPC packs. |
| `Data/Scripts/System/Skills/Discordance.cs` | Skill callback | Bard instrument target skill that applies resistance, stat, and skill penalties until bard conditions break. |
| `Data/Scripts/System/Skills/Druidism.cs` | Skill callback and Gump | Animal/creature lore target flow and `DruidismGump` stat display. |
| `Data/Scripts/System/Skills/Forensics.cs` | Skill callback | Investigates thieves, corpses, coffers, lockpickers, graves, and ship/wagon remains. |
| `Data/Scripts/System/Skills/Hiding.cs` | Skill callback | Hides the caller, with combat and house bonuses, then hides controlled pets. |
| `Data/Scripts/System/Skills/Inscribe.cs` | Skill callback | Copies written `BaseBook` content into a writable destination book. |
| `Data/Scripts/System/Skills/ItemIdentification.cs` | Skill callback | Mercantile appraisal and identification for unknown scrolls, wands, artifacts, items, and XmlSpawner attachments. |
| `Data/Scripts/System/Skills/Meditation.cs` | Skill callback | Starts active meditation after equipment, mana, health, target, and armor checks. |
| `Data/Scripts/System/Skills/Peacemaking.cs` | Skill callback | Bard instrument self-area calm or single-target pacification. |
| `Data/Scripts/System/Skills/Poisoning.cs` | Skill callback | Applies poison potions to eligible weapons, darts, shuriken, food, and drinks. |
| `Data/Scripts/System/Skills/Provocation.cs` | Skill callback | Bard instrument two-target creature provocation. |
| `Data/Scripts/System/Skills/Psychology.cs` | Skill callback | Inspects Mobile intellect and mana strength. |
| `Data/Scripts/System/Skills/RemoveTrap.cs` | Skill callback | Disarms trapable containers, faction traps, and hidden floor traps. |
| `Data/Scripts/System/Skills/Searching.cs` | Skill callback | Reveals hidden Mobiles and detects traps, hidden doors, hidden floor traps, and hidden chests. |
| `Data/Scripts/System/Skills/Snooping.cs` | Container hook | Installs `Container.SnoopHandler` and resolves snooping checks when containers are opened. |
| `Data/Scripts/System/Skills/Spiritualism.cs` | Skill callback | In AOS mode, casts a zero-mana Spiritualism spell for self healing; otherwise enables ghost hearing. |
| `Data/Scripts/System/Skills/Stealing.cs` | Skill callback | Handles item/mobile theft, thief-guild gates, weight difficulty, coffer theft, and stolen item return windows. |
| `Data/Scripts/System/Skills/Stealth.cs` | Skill callback | Converts hidden state into stealth steps after armor and Hiding gates. |
| `Data/Scripts/System/Skills/Taming.cs` | Skill callback | Tames eligible `BaseCreature` targets through a delayed repeated timer. |
| `Data/Scripts/System/Skills/Tasting.cs` | Skill callback | Tastes food, potions, kegs, and mushroom statics/addons. |
| `Data/Scripts/System/Skills/Tracking.cs` | Skill callback and Gumps | Category gump, target list gump, tracking arrows, and stalking bonus state. |
| `Data/Scripts/System/Skills/Weapon Abilities/` | Related combat commands | Weapon special ability classes, special attack gumps, ability book, and player commands under the same skill tree. |

## Direct Skill Use Pipeline
| Stage | Compiled behavior |
| --- | --- |
| Callback registration | Direct-use scripts assign `SkillInfo.Table[...] .Callback = new SkillUseCallback(OnUse)` from `Initialize()`. |
| Use request | `Skills.UseSkill(Mobile from, int skillID)` rejects dead Mobiles, rejects blocked region skill use, and calls `from.AllowSkillUse((SkillName)skillID)`. |
| Callback lookup | If the skill id is in range and `SkillInfo.Callback` is null, the player receives "That skill cannot be used directly." |
| Cooldown and spell gate | A callback only fires when `from.NextSkillTime <= DateTime.Now` and `from.Spell == null`; otherwise `from.SendSkillMessage()` is used. |
| Dispatch | The engine calls `from.DisruptiveAction()`, executes the callback, and sets `from.NextSkillTime` to `DateTime.Now + callbackReturn`. |
| Callback overrides | Some target-based scripts set `NextSkillTime` inside target handlers, often using long placeholder delays during target selection and resetting on invalid/cancelled targets. |

## Skill Storage And Serialization
| Object | Versioning and save format |
| --- | --- |
| `Skills` | Writes version `3`, then the total cap, saved skill count, and one serialized skill slot per `SkillInfo.Table` entry. Older version `2` also has cap; versions before `3` read and discard the old total. |
| `Skill` | Uses a compact byte. `0xFF` means default `Base = 0`, `Cap = 1000`, `Lock = Up`. Otherwise bit `0x1` writes base, `0x2` writes cap, and `0x4` writes lock. |
| Invalid locks | Deserialization logs bad skill locks to console and resets them to `SkillLock.Up`. |
| `WeaponAbilityBook` | The related ability book writes version `0` and no custom fields. |

## Skill Gain Engine
`SkillCheck.Initialize()` installs XMLSpawner-aware handlers on `Mobile.SkillCheckLocationHandler`, `Mobile.SkillCheckDirectLocationHandler`, `Mobile.SkillCheckTargetHandler`, and `Mobile.SkillCheckDirectTargetHandler`. The original local handlers remain in the file and are called by the XMLSpawner wrapper path.

| Step | Formula or behavior |
| --- | --- |
| Location object | Location checks bucket the caller's X/Y by `LocationSize = 5`, so repeated use inside the same 5x5 bucket shares anti-macro history. |
| Range success chance | For min/max checks, `chance = (skill.Value - minSkill) / (maxSkill - minSkill)`. Values below `minSkill` fail immediately; values at or above `maxSkill` succeed immediately. |
| Direct chance | Direct checks fail when `chance < 0.0`, succeed when `chance >= 1.0`, otherwise use the supplied chance. |
| Base gain divisor | `gainer` starts at `2.0`. For `PlayerMobile` guild skills, `gainer` is randomly set to `1.5`, `1.4`, `1.3`, `1.2`, `1.1`, or `1.0`, then reduced by `MyServerSettings.SkillGain()`. |
| Success roll | `success = chance >= Utility.RandomDouble()`. |
| Gain chance | `gc = (((Skills.Cap - Skills.Total) / Skills.Cap) + ((skill.Cap - skill.Base) / skill.Cap)) / gainer`; then `gc += (1.0 - chance) * (success ? 0.5 : (Core.AOS ? 0.0 : 0.2))`; then `gc /= gainer`; then `gc *= skill.Info.GainFactor`; floor is `0.01`. |
| Controlled creatures | Controlled `BaseCreature` skill gain chance is doubled. |
| Gain gate | Alive Mobiles gain when `gc >= Utility.RandomDouble()` and anti-macro allows it, or when the skill base is under `10.0`. |
| Seafaring gate | Seafaring at `50.0+` only gains while `Worlds.IsOnBoat(from)` is true; otherwise the caller receives boat-training feedback. |
| Skill amount | `Gain()` adds `1` fixed-point step, except skills at or below `10.0` gain `1` to `4`. Scroll of Alacrity multiplies that by `2` to `5` for the accelerated skill while active. |
| Skill cap pressure | If a player is at or near total cap, the first different skill locked `Down` and high enough to cover the gain is reduced by the gain amount. |
| Stat gain | If the skill lock is `Up`, stat gains are rolled against `SkillInfo` stat factors divided by `MyServerSettings.StatGain()`, with player and pet gain delays from `MyServerSettings.StatGainDelay()` and `PetStatGainDelay()`. |
| Stat caps | Non-controlled Mobiles cannot raise when raw stat total is at `StatCap`. Individual raw stat caps are `150`, or `175` when `StatCap > 250`. |

## Anti-Macro Settings
Anti-macro is enabled when `MyServerSettings.NoMacroing()` returns true. The engine remembers target/location objects for `5` minutes and allows `3` gains against the same tracked object before blocking further gains.

| Anti-macro applies | Skills |
| --- | --- |
| Enabled entries | Anatomy, Druidism, Mercantile, ArmsLore, Begging, Peacemaking, Camping, Searching, Discordance, Psychology, Healing, Forensics, Herding, Hiding, Provocation, Lockpicking, Magery, MagicResist, Snooping, Musicianship, Poisoning, Spiritualism, Stealing, Taming, Tasting, Tracking, Veterinary, Lumberjacking, Mining, Meditation, Stealth, RemoveTrap, Necromancy, Knightship, Bushido, Ninjitsu, Elementalism |
| Disabled entries | Alchemy, Parry, Blacksmith, Bowcraft, Carpentry, Cartography, Cooking, Inscribe, Seafaring, Tactics, Marksmanship, Tailoring, Tinkering, Swords, Bludgeoning, Fencing, FistFighting, Focus |
| Missing from table | Mysticism, Imbuing, Throwing |

## Direct Skill Callback Summary
| Skill | Callback behavior | Key formulas, gates, or side effects |
| --- | --- | --- |
| Anatomy | Targets a Mobile within range `8`; self and invulnerable vendor targets return special messages. | Margin of error is `max(0, 25 - Anatomy / 4)`. Success uses `CheckTargetSkill(Anatomy, target, 0, 125)`. Strength, Dexterity, and stamina percent are bucketed into `0..10`; stamina text requires Anatomy base `65.0+`. |
| Arms Lore | Targets an Item within range `2`, `AllowNonlocal = true`, and delegates to `IDItem`. | Requires movable items, range `3`, and optionally backpack containment via `IdentifyItemsOnlyInPack()`. Identifies ArmsLore `UnidentifiedItem` containers unless `IDAttempt > 5`, reports relic values, marks weapons/armor identified, and reports weapon damage class, poison, armor durability, armor rating, and swamp dragon barding HP. |
| Begging | Reveals the caller and targets range `12`. Self-target toggles `PlayerMobile.CharacterBegging`. Hostile targets can be begged to drop combat; human NPCs can donate gold. | Combat-beg difficulty starts from `GetBaseDifficulty(target) - 10.0`; Begging over `100.0` reduces difficulty by half the excess. Successful creature pacify lasts `100 - diff / 1.5`, clamped `10..120` seconds. NPC gold payout is up to `10 + Fame / 2500`, clamped `10..14`, and consumes up to 10% of target-pack gold. Begging reduces fame/karma in several success/failure paths. |
| Discordance | Picks a `BaseInstrument`, targets in bard range, and applies debuffs to a valid harmful Mobile. | Difficulty is `instrument.GetDifficultyFor(target) - 10.0`, reduced by half Musicianship over `100.0`. Effect is `(int)(Discordance / -5.0)` and is halved for Core.SE targets with base difficulty `160.0+`. The negative scalar applies to all five resistances, Str/Int/Dex, and all positive skills. The effect ticks every `1.25` seconds and clears 15 seconds after the bard dies, hides, leaves range/map, or target dies/deletes. |
| Druidism | Targets a `BaseCreature` within range `8` and opens `DruidismGump` when allowed. | Rejects dead pets, henchmen, and creatures matching Slimy Scourge, Elemental Ban, Repond, Silver, Giant Killer, or Golem Destruction slayers unless the target is a controlled special creature. Skill under `100.0` can only lore tamed creatures; under `110.0` can only lore tamed or tamable creatures. Otherwise success uses `CheckTargetSkill(Druidism, creature, 0, 125)`. |
| Forensics | Targets range `10` and inspects Mobiles, coffers, corpses, lockpickable objects, and special chest/ship remains. | Mobile checks use `40.0..125.0` and reveal Thieves Guild players. Corpse checks use `0.0..125.0`, set `Corpse.m_Forensicist` when first inspected, report killer for human corpses, and list looters. `ILockpickable.Picker` is reported when present. |
| Hiding | Blocks active spells, cancels existing targets under ML, computes house bonuses, checks nearby combatants, and hides the caller. | Friend-owned house bonus is `100.0`; non-AOS nearby-house bonus is `50.0`. Search range is `min((100 - Hiding) / 2 + 8, 18)`. A high check `CheckSkill(Hiding, 0, 250)` can clear direct combat blocking. Final success uses `CheckSkill(Hiding, 0 - bonus, 100 - bonus)`, sets `Hidden = true`, clears warmode, and hides all controlled pets found in `World.Mobiles`. |
| Inscribe | Two-target book copy flow with one-minute target timeouts. | Source must be a non-empty `BaseBook` not already in the use table. Destination must be another writable `BaseBook`. Success uses `CheckTargetSkill(Inscribe, destination, 0, 50)` and copies title, author, and all page lines up to the destination page count. |
| Mercantile | `ItemIdentification` targets range `8`, `AllowNonlocal = true`, and identifies unknown/relic/appraisal objects. | Unknown scrolls use `CheckTargetSkill(Mercantile, item, -5, 125)` or Inscribe checks for a scroll-level bonus. Unknown wands and unidentified artifacts/items use Mercantile `-5..125` unless automatic. `UnidentifiedItem` must have `SkillRequired == "ItemID"` unless automatic. All paths call `XmlAttach.RevealAttachments(from, target)` at the end. |
| Meditation | Reveals the caller, blocks existing targets, low non-AOS health, full mana, and AOS armor penalties, then begins meditation. | Non-channeling hand items are moved to the backpack. Success uses `chance = (50 + ((Meditation - (Mana / ManaMax)) * 2)) / 100`, then `CheckSkill(Meditation, 0, 100)`, sets `Meditating = true`, and applies the active meditation buff. |
| Peacemaking | Picks an instrument and targets a Mobile in bard range. Self-target runs area peace; other targets run single-target peace. | Area mode requires musicianship and `CheckSkill(Peacemaking, 0, 120)` and pacifies harmful mobiles in range for `Musicianship / 10` seconds. Target mode uses `instrument.GetDifficultyFor(target) - 10.0`, reduced by half Musicianship over `100.0`; duration is `100 - diff / 1.5`, clamped `10..120`; success uses `CheckTargetSkill(Peacemaking, target, diff - 25, diff + 25)`. |
| Poisoning | First targets a `BasePoisonPotion`, then an eligible target within range `2`. | Eligible targets include fukiya darts, shuriken, food, stale bread, dried beef, beverages, waterskins, dirty waterskins, and weapons. Non-classic weapon poisoning requires an Infectious Strike or Shadow Infectious Strike ability on the weapon. Classic weapon poisoning requires one-handed slashing or piercing weapons. Success uses the potion's min/max skill, consumes the potion, returns a bottle, applies weapon/dart/shuriken charges `18 - poison.Level * 2`, or replaces one food/drink unit with poisoned food/liquid level `1..5`. Failure under `80.0` Poisoning has a 1-in-20 self-poison chance. |
| Provocation | Picks an instrument, targets one harmful uncontrolled `BaseCreature`, then targets a second `BaseCreature` in bard range. | Paragon creatures with base difficulty `160.0+`, controlled creatures, unprovokable creatures, bard-immune creatures, and out-of-range pairs are rejected. Difficulty is the average of both instrument difficulties minus `5.0`, reduced by half Musicianship over `100.0`; success uses `CheckTargetSkill(Provocation, second, diff - 25, diff + 25)` and calls `m_Creature.Provoke(from, creature, true)`. |
| Psychology | Targets range `8` and evaluates Mobile intelligence and mana strength. | Margin of error is `max(0, 20 - Psychology / 5)`. Success uses `CheckTargetSkill(Psychology, target, 0, 125)`. Int and mana percent are bucketed `0..10`; mana-strength text requires Psychology base `76.0+`. |
| Remove Trap | Targets range `2` and disarms trapable containers, faction traps, or `HiddenTrap` items. | Trapable containers require current skill value at least `TrapLevel * 10`, then check `RemoveTrap` from `0` to `TrapLevel * 10 + 20`. Faction traps require faction membership rules and, unless owner/commander, a base-pack removal kit; non-owner success requires both Remove Trap and Tinkering checks `80.0..125.0`. Hidden traps use `CheckSkill(RemoveTrap, 0, 125)`. |
| Searching | Targets ground, items, or mobiles in range `12`, then searches around the resolved point. | Base range is `Searching / 10`; failed `CheckSkill(Searching, 0, 125)` halves it; friend-owned house search range is `22`. Hidden mobiles reveal when caller access is high enough and caller skill contest beats Hiding, or the target is inside the caller's friend-owned house. Traps, hidden doors, hidden floor traps, and hidden chests are detected through `DetectSomething`. |
| Snooping | Configures `Container.SnoopHandler` and runs when a container is opened. | Requires staff access or range `1`. Blessed players are blocked. Town harmful restrictions block snooping blue human NPCs. Player failure notice occurs when Snooping value is below `Utility.Random(100)`. Success uses staff access or `CheckTargetSkill(Snooping, container, 0, 125)`, executes trapable-container traps, then displays the container. Failure may reveal the snooper based on Hiding. |
| Spiritualism | Under AOS, casts an internal zero-reagent spell. Outside AOS, checks Spiritualism to temporarily hear ghosts. | AOS spell searches corpses within range `3`; negative-karma casters can channel an unchanneled, unanimated corpse for no mana. Otherwise mana cost is `10`. Min heal/stam restore is `PlayerLevelMod(1 + Spiritualism * 0.25 + FistFighting * 0.15)`; max is `PlayerLevelMod(min + PlayerLevelMod(4))`. Success chance is `Spiritualism / 100.0` after `CheckSkill(Spiritualism, 0, 120)`. |
| Stealing | Targets item/mobile range `1`, requires empty hands, and attempts direct item theft or random backpack item theft from a Mobile. | Stack amount cap is `(Stealing / 10) / item.Weight`, minimum `1`. Weight difficulty uses `ceil(weight + totalWeight) * 10` with check range `weight - 22.5` to `weight + 27.5`; items over weight `10` are rejected. Innocent player theft requires the thief to be in the Thieves Guild. Successful stolen items are tracked for `2` minutes and can be returned on thief death. |
| Stealth | Requires the caller to already be hidden, then checks Hiding threshold and armor. | Hiding threshold is `30.0` in ML, `50.0` in SE, otherwise `80.0`. AOS armor penalty comes from the armor material/body-position table; armor rating `42+` in AOS or `26+` pre-AOS blocks stealth. Success uses `CheckSkill(Stealth, -20 + armor * 2, (Core.AOS ? 60 : 80) + armor * 2)` and grants `Stealth / 5` AOS steps or `Stealth / 10` pre-AOS steps, minimum `1`. |
| Taming | Targets `BaseCreature` range `2`, validates tamability, gender gates, control slots, owner count, subdue state, and skill/mastery gates, then starts a timer. | `MustBeSubdued` requires first-tame creatures with `SubdueBeforeTame` to be below 10% hits. Dark wolf familiar mastery auto-allows several wolf/fox classes. Timer ticks every `3` seconds for `3` or `4` ticks, repeatedly checking range `6`, alive state, LOS/path, tamability, damage after start, owner caps, and subdue state. Final difficulty is `MinTameSkill + Owners.Count * 6 + 24.9`; Druidism reduces only the lower bound by `Druidism / 5`. First tame scales skills to `90%`, or `86%` if paralyzed, and may scale stats to `50%`. |
| Tasting | Targets range `2`, `AllowNonlocal = true`. | Food success uses `CheckTargetSkill(Tasting, food, 0, 125)` and reports poison. Potions and potion kegs report known labels. Static targets and addon components with item IDs `3340..3348` can yield `Mushrooms` when Camping is at least `50.0` and `CheckTargetSkill(Tasting, target, 0, 100)` succeeds. |
| Tracking | Opens `TrackWhatGump`, then `TrackWhoGump`, then a `QuestArrow`. | Initial category success is `CheckSkill(Tracking, 0, 21.1)`. Search range is `25 + Tracking / 2`. Candidate mobiles must be in the same broad world land area or same region, pass category delegates, and pass `CheckDifficulty`. Player difficulty chance is `50 * (Tracking * 2 + Searching) / (Hiding + Stealth)`, modified by Necromancy forms. The target list shows at most 16 mobiles. The arrow range is twice the search range and updates every `2.5` seconds. |

## Unknown Scroll Identification
`ItemIdentification.IDScroll` maps unknown scroll metadata to concrete rewards.

| Scroll type | Level behavior |
| --- | --- |
| `1` Magery | Level 1 or other: paper types `1..12`; level 2: `13..24`; level 3: `25..36`; level 4: `37..48`; level 5: `49..60`; level 6: `57..64`. |
| `2` Necromancy | Level 1 or other: `65..67`; level 2: `68..70`; level 3: `71..73`; level 4: `74..76`; level 5: `77..79`; level 6: `80..81`. |
| `3` Bard | Any level: `82..97`. |
| `4` Note | Adds `SomeRandomNote`. |
| `5` Clue | Adds `ScrollClue`. |
| `6` Map | Adds `TreasureMap` at level capped to `6`; map selection randomly chooses Sosaria, Lodor, Serpent Island, Isles of Dread, Savaged Empire, Underworld, or leaves the caller's current map on unhandled roll values. |
| `7` Elementalism | Level 1 or other: `98..105`; level 2: `106..113`; level 3: `114..117`; level 4: `118..121`; level 5: `122..125`; level 6: `126..129`. |

## Searching Rewards
| Target found | Compiled result |
| --- | --- |
| `BaseTrap` | Sends trap-specific text for known trap subclasses and plays particles/sound at the trap. |
| Hidden door `BaseDoor` IDs | Sends "There is a hidden door nearby!" and plays particles/sound. |
| `HiddenTrap` | Sends hidden-floor-trap text, or dangerous-area text on spaceships, and plays particles/sound. |
| `HiddenChest` | Deletes the hidden chest marker after spawning either currency/resources or `HiddenBox`. |
| Currency/resource roll | 2-in-3 chance. Base gold is `Utility.RandomMinMax(100, 200) * level`, where level is `Searching / 10` clamped `1..10`, then randomized from `1..level`. Spaceships convert to `DDXormite`, Underworld converts to `DDJewels`, and later independent rare rolls can convert to gemstones, gold nuggets, silver, or copper. |
| Hidden box roll | 1-in-3 chance. Spawns `HiddenBox(level, where, mobile)` at the hidden chest marker. |

## Player Commands
The skill folder itself registers only weapon-special-attack commands; ordinary skill use comes through client skill buttons and the `Skills.UseSkill` pipeline.

| Command | Access | Usage metadata | Behavior |
| --- | --- | --- | --- |
| `[SpecialAttacksDisplay` | Player | `[Usage("SpecialAttacksDisplay")]`, alias `[SAD]`, opens weapon special attacks display. | Counts weapon abilities the player's weapon skill and Tactics qualify for, then opens `SpecialAttackGump`. |
| `[SAD` | Player | Alias for `SpecialAttacksDisplay`. | Same handler. |
| `[SetPrimaryAbility` | Player | `[Usage("SetPrimaryAbility")]`, alias `[Set1]`. | If `SpecialAttackGump` is open, submits button `1`. |
| `[SetSecondaryAbility` | Player | `[Usage("SetSecondaryAbility")]`, alias `[Set2]`. | If `SpecialAttackGump` is open and it has at least two abilities, submits button `2`. |
| `[SetThirdAbility` | Player | `[Usage("SetThirdAbility")]`, alias `[Set3]`. | If `SpecialAttackGump` is open and it has at least three abilities, submits button `3`. |
| `[SetFourthAbility` | Player | `[Usage("SetFourthAbility")]`, alias `[Set4]`. | If `SpecialAttackGump` is open and it has at least four abilities, submits button `4`. |
| `[SetFifthAbility` | Player | `[Usage("SetFifthAbility")]`, alias `[Set5]`. | If `SpecialAttackGump` is open and it has at least five abilities, submits button `5`. |
| `[abilitynames` | Player | `[Usage("abilitynames")]`, toggles weapon ability names on the toolbar. | Toggles `PlayerMobile.CharacterWepAbNames` between `1` and `0`. |

## Related Weapon Ability Layer
Weapon abilities are not `SkillInfo` callbacks, but they are stored under `Data/Scripts/System/Skills/Weapon Abilities/` and use skill values heavily.

| Component | Compiled behavior |
| --- | --- |
| `WeaponAbility` | Defines base mana, damage scalar, skill validation, mana calculation, current ability table, and the indexed ability array. Required skill is the configured special-ability base for primary, then +10, +20, +30, +40 for secondary through fifth ability slots. |
| Mana calculation | Starts at `BaseMana`, subtracts `10` when the sum of listed combat/special skills is at least `300.0`, or `5` at least `200.0`, applies Mind Rot scalar, applies Lower Mana Cost capped at `40%`, and doubles cost when another special move was used within 3 seconds. |
| `CustomWeaponAbilities` | Auto-opens the special attack gump on login when player settings allow it, counts qualified abilities, and maps weapon ability instances back to indexes in `WeaponAbility.Abilities`. |
| `SpecialAttackGump` | Displays up to five ability slots for the current weapon and uses button IDs `1..5` to set or clear current abilities through `CustomWeaponAbilities.ServerSideSetAbility`. |
| `AbilityBook` | Constructable book item that opens an informational gump and serializes only version `0`. |

## Known Issues
| Issue | Evidence from code trace |
| --- | --- |
| Anti-macro table does not cover every `SkillName`. | `SkillName` defines 58 entries through Throwing, but `UseAntiMacro` only has entries through Elementalism. `AllowGain` indexes `UseAntiMacro[skill.Info.SkillID]` without a bounds check, so anti-macro-enabled gain checks for Mysticism, Imbuing, or Throwing can throw. |
| Several pooled range enumerables are not freed. | `Hiding`, `Peacemaking`, `Spiritualism`, `Stealing`, `Tracking`, and several weapon AOE abilities use `GetMobilesInRange`, `GetItemsInRange`, or `GetClientsInRange` in `foreach` form without retaining and freeing the returned `IPooledEnumerable`. `Searching` and `Snooping` show the correct explicit `Free()` pattern nearby. |
| `Meditation.OnUse` uses integer mana division in its chance formula. | `m.Mana / m.ManaMax` is evaluated with integer operands, so non-full mana usually subtracts `0`; at Meditation `25.0`, the resulting chance reaches `1.0` before the random roll. |
| `Poisoning` assumes the skill user is `PlayerMobile` when poisoning weapons. | The BaseWeapon branch casts `((PlayerMobile)from).ClassicPoisoning` without verifying `from is PlayerMobile`, so non-player callers can crash this path. |
| Player-target Peacemaking makes higher Magic Resist easier to calm. | After the bard succeeds, the player branch applies calm/paralyze when `target.MagicResist > Utility.RandomMinMax(0, 125)`, so higher resistance increases the chance of being affected. |
| `Tasting` has silent target paths. | Non-mushroom `StaticTarget` objects and arbitrary non-food/non-potion Items can fall through without feedback because the fallback message exists only inside the `AddonComponent` branch. |
| `Stealing` has null and logging hazards. | The coffer branch calls `m_Thief.Backpack.FindItemByType(...)` without a backpack null check. It also logs `coffer.CofferGold` after setting it to `0`, so the log records zero gold stolen. |
| Special attack display assumes a valid player weapon. | `[SpecialAttacksDisplay` casts `e.Mobile` to `PlayerMobile` and `pm.Weapon` to `BaseWeapon` without null/type guards, so unarmed or unusual callers can throw before user feedback. |
| `GetTrueBestSkill` helper methods index empty lists. | `GetSkill()` and `GetSkillTop5()` create an empty `List<LGSkillLists>`, sort it, and then index it, making those helper paths unusable if called. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0107.
- Backlog rows: RB-06772.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/System/Source/Skills.cs (CurrentFile)
- Data/Scripts/System/Skills/ (CurrentDirectory)
- Data/Scripts/System/Skills/SkillCheck.cs (CurrentFile)
- Data/Scripts/System/Skills/Anatomy.cs (CurrentFile)
- Data/Scripts/System/Skills/ArmsLore.cs (CurrentFile)
- Data/Scripts/System/Skills/Begging.cs (CurrentFile)
- Data/Scripts/System/Skills/Discordance.cs (CurrentFile)
- Data/Scripts/System/Skills/Druidism.cs (CurrentFile)
- Data/Scripts/System/Skills/Forensics.cs (CurrentFile)
- Data/Scripts/System/Skills/Hiding.cs (CurrentFile)
- Data/Scripts/System/Skills/Inscribe.cs (CurrentFile)
- Data/Scripts/System/Skills/ItemIdentification.cs (CurrentFile)
- Data/Scripts/System/Skills/Meditation.cs (CurrentFile)
- Data/Scripts/System/Skills/Peacemaking.cs (CurrentFile)
- Data/Scripts/System/Skills/Poisoning.cs (CurrentFile)
- Data/Scripts/System/Skills/Provocation.cs (CurrentFile)
- Data/Scripts/System/Skills/Psychology.cs (CurrentFile)
- Data/Scripts/System/Skills/RemoveTrap.cs (CurrentFile)
- Data/Scripts/System/Skills/Searching.cs (CurrentFile)
- Data/Scripts/System/Skills/Snooping.cs (CurrentFile)
- Data/Scripts/System/Skills/Spiritualism.cs (CurrentFile)
- Data/Scripts/System/Skills/Stealing.cs (CurrentFile)
- Data/Scripts/System/Skills/Stealth.cs (CurrentFile)
- Data/Scripts/System/Skills/Taming.cs (CurrentFile)
- Data/Scripts/System/Skills/Tasting.cs (CurrentFile)
- Data/Scripts/System/Skills/Tracking.cs (CurrentFile)
- Data/Scripts/System/Skills/Weapon Abilities/ (CurrentDirectory)

### Runtime Evidence

- Hook summary: Command=13; Event=1; Gump=21; Initialize=28; Timer=20.
- Data/Scripts/System/Skills/Anatomy.cs:L11 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/ArmsLore.cs:L15 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Begging.cs:L13 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Begging.cs:L433 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/System/Skills/Discordance.cs:L11 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Druidism.cs:L16 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Druidism.cs:L92 Gump SendGump access=Internal
- Data/Scripts/System/Skills/Druidism.cs:L113 Gump SendGump access=Internal
- Data/Scripts/System/Skills/Druidism.cs:L323 Gump OnResponse access=Internal
- Data/Scripts/System/Skills/Forensics.cs:L13 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Hiding.cs:L20 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Skills/Inscribe.cs:L11 Initialize Initialize access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 3.
- Data/Scripts/System/Skills/Weapon Abilities/AbilityBook.cs:Server.Items.WeaponAbilityBook version=0 serialize=L687 deserialize=L693 alignment=AlignedByCountAndKnownTypes
- Data/System/Source/Skills.cs:Server.Skill version=Unknown serialize=L492 deserialize=L alignment=CountMismatch:Writes=5;Reads=0
- Data/System/Source/Skills.cs:Server.Skills version=3 serialize=L1573 deserialize=L alignment=CountMismatch:Writes=3;Reads=0

### Project And Runtime Coverage

- Data/Scripts/System/Skills/Anatomy.cs=Keep
- Data/Scripts/System/Skills/Anatomy.cs=Keep
- Data/Scripts/System/Skills/ArmsLore.cs=Keep
- Data/Scripts/System/Skills/ArmsLore.cs=Keep
- Data/Scripts/System/Skills/Begging.cs=Keep
- Data/Scripts/System/Skills/Begging.cs=Keep
- Data/Scripts/System/Skills/Discordance.cs=Keep
- Data/Scripts/System/Skills/Discordance.cs=Keep
- Data/Scripts/System/Skills/Druidism.cs=Keep
- Data/Scripts/System/Skills/Druidism.cs=Keep
- Data/Scripts/System/Skills/Forensics.cs=Keep
- Data/Scripts/System/Skills/Forensics.cs=Keep
- Data/Scripts/System/Skills/Hiding.cs=Keep
- Data/Scripts/System/Skills/Hiding.cs=Keep
- Data/Scripts/System/Skills/Inscribe.cs=Keep
- Data/Scripts/System/Skills/Inscribe.cs=Keep
- Data/Scripts/System/Skills/ItemIdentification.cs=Keep
- Data/Scripts/System/Skills/ItemIdentification.cs=Keep
- Data/Scripts/System/Skills/Meditation.cs=Keep
- Data/Scripts/System/Skills/Meditation.cs=Keep
- Additional project-truth rows are recorded in project-truth-register.csv for this source set.

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
