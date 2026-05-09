# Ninjitsu

## Overview
Ninjitsu is the Samurai Empire stealth and combat-technique system built around `SpellbookType.Ninja`, `SkillName.Ninjitsu`, spell IDs `500` through `507`, a talisman-layer `BookOfNinjitsu`, three `NinjaSpell` casts, five `SpecialMove` combat abilities, animal-form transformations, and craftable ninja tools.

`Initializer` registers the eight Ninjitsu ability IDs. `Spellbook.GetTypeForSpell` maps `500 <= spellID < 508` to `SpellbookType.Ninja`, and the spellbook client request path either casts a `NinjaSpell` or toggles a registered `SpecialMove`.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `BookOfNinjitsu` | `Spellbook` subclass | Ninja spellbook using `BookOffset = 500`, `BookCount = 8`, item ID `0x23A0`, and `Layer.Talisman`. New constructable books start with content `0xFF`, so all eight abilities are present. |
| `NinjaSpell` | `Spell` subclass | Shared Ninjitsu spell behavior for Animal Form, Shadowjump, and Mirror Image. Uses `SkillName.Ninjitsu` for cast and damage skills, does not reveal on cast, does not clear hands, and spends scaled mana during `CheckFizzle()`. |
| `NinjaMove` | `SpecialMove` subclass | Shared Ninjitsu special-move skill gain wrapper. Skill gain checks use `RequiredSkill - 12.5` through `RequiredSkill + 37.5`. |
| `AnimalForm` | `NinjaSpell` | Opens the animal form `Gump`, applies `BodyMod`, `HueMod`, skill mods, speed packets, and a one-second maintenance timer. |
| `Shadowjump` | `NinjaSpell` | Targeted stealth teleport with travel, sigil, overload, spawn, and multi checks. |
| `MirrorImage` | `NinjaSpell` plus `Clone` Mobile | Creates a summoned `Clone` Mobile and records clone counts for attack redirection. |
| `FocusAttack`, `DeathStrike`, `KiAttack`, `SurpriseAttack`, `Backstab` | `NinjaMove` subclasses | Combat moves activated through the special-move spellbook path. |
| `SmokeBomb`, `EggBomb`, `LeatherNinjaBelt`, `Fukiya`, `Shuriken`, `FukiyaDarts` | Items | Supporting Ninjitsu tools for hiding and ranged poison projectile use. |

## Spellbook And Entry Points
| Entry point | Compiled behavior |
| --- | --- |
| Spellbook request type | Client open request type `4` maps to `SpellbookType.Ninja`. |
| Spell ID range | Spell IDs `500` through `507` map to `SpellbookType.Ninja`. |
| Cast request | `Spellbook.EventSink_CastSpellRequest` checks the spellbook content bit. If the ID is a registered `SpecialMove`, it calls `SpecialMove.SetCurrentMove`; otherwise it creates and casts a `Spell`. |
| Equip gate | `BookOfNinjitsu` can only be equipped when `from.Skills[SkillName.Ninjitsu].Base >= 30`. |
| Admin fill command | `[AllSpells]` targets any `Spellbook` and sets `Content` to all spell bits for that book count. |
| Player commands | No Ninjitsu-specific `[Command]` registrar was found. Player use is through the spellbook/client spellbar request path and item double-click/context-menu paths. |

## Registered Abilities
| Spell ID | Ability | Class | Type | Required skill | Mana | Cast delay or timing |
| ---: | --- | --- | --- | ---: | ---: | --- |
| 500 | Focus Attack | `FocusAttack` | `SpecialMove` | `30.0` on ML, otherwise `60.0` | `10` on ML, otherwise `20` | Applied to the next melee hit. |
| 501 | Death Strike | `DeathStrike` | `SpecialMove` | `85.0` | `30` | Applies delayed damage after 5 seconds or after 5 movement steps. |
| 502 | Animal Form | `AnimalForm` | `NinjaSpell` | `0.0` to cast; per-form requirements apply in `Morph()` | `0` | `1.0` second cast delay. |
| 503 | Ki Attack | `KiAttack` | `SpecialMove` | `80.0` | `25` | Must hit within 2 seconds after activation for the distance bonus. |
| 504 | Surprise Attack | `SurpriseAttack` | `SpecialMove` | `60.0` on ML, otherwise `30.0` | `20` | Applies to the next stealth swing. |
| 505 | Backstab | `Backstab` | `SpecialMove` | `40.0` on ML, otherwise `20.0` | `30` | Applies to the next stealth swing. |
| 506 | Shadowjump | `Shadowjump` | `NinjaSpell` | `50.0` | `15` | `1.0` second cast delay, then range-11 target. |
| 507 | Mirror Image | `MirrorImage` | `NinjaSpell` | `20.0` on ML, otherwise `40.0` | `10` | `1.5` second cast delay. |

## Shared Spell Rules
`NinjaSpell` uses `SkillName.Ninjitsu` for both `CastSkill` and `DamageSkill`. It returns `false` for `RevealOnCast`, `ClearHandsOnCast`, `ShowHandMovement`, and `BlocksMovement`. `CastRecoveryBase` is `7`, and `GetMana()` returns `0`; actual mana spending happens in `CheckFizzle()` after the base fizzle check succeeds.

For player casters, `NinjaSpell.CheckExpansion()` requires a live `NetState` that supports `Expansion.SE`. Non-player Mobiles pass the expansion check automatically. `SpecialMove.SetCurrentMove()` separately refuses all special moves when `Core.SE` is disabled.

## Special Move Mechanics
| Ability | Validation | Formula or effect |
| --- | --- | --- |
| Focus Attack | Requires a melee `BaseWeapon` in either hand and no `BaseShield` on `Layer.TwoHanded`; player casters also fail during the Bushido `HonorableExecution` penalty. | Damage scalar is `1.0 + (Ninjitsu^2 / 43636)`. Property leech bonus is `1.0 + ((Ninjitsu^2 / 43636) * 3 + 0.01)`. |
| Death Strike | Validates and consumes mana on hit. The initial success roll is `30 + (Ninjitsu - 85) * 2.2` percent below `100.0` Ninjitsu, otherwise `63 + (Ninjitsu - 100) * 1.1` percent. | The weapon hit itself has scalar `0.5`. ML delayed damage uses `floor(min(60, (Ninjitsu / 3) * (0.3 + 0.7 * scalar) + stalkingBonus))` after 5 steps, or cap `30` with `Ninjitsu / 9` before 5 steps. `scalar = min(1, (Hiding + Stealth) / 220)`. Ranged Death Strike halves the ML delayed damage. Pre-ML damage uses divisor `30` after 5 steps or `80` before 5 steps, cap `62` or `22`, plus repeat-strike damage bonus. |
| Ki Attack | Cannot be used while in stealth mode. On ML, ranged weapons are refused. Activation stores the caster location and starts a 2-second timer. | Damage scalar is `1.0 + min(distanceMoved, 20) / 10`, unless the attacker is hidden, in which case it is `1.0`. No distance moved gives no bonus message and no skill gain. |
| Surprise Attack | Requires `Hidden == true` and `AllowedStealthSteps > 0`. `OnBeforeSwing()` consumes mana and suppresses normal stealth handling for 5 seconds. | On hit, applies a defense malus for 8 seconds: `Ninjitsu.Fixed / 60 + (int)Tracking.GetStalkingBonus(attacker, defender)`. `BaseWeapon.CheckHit()` subtracts the malus from the defender bonus. |
| Backstab | Requires `Hidden == true` and `AllowedStealthSteps > 0`. `OnBeforeSwing()` consumes mana and suppresses normal stealth handling for 5 seconds. | Damage scalar is `1.0 + (Ninjitsu / 360) + Tracking.GetStalkingBonus(attacker, defender) / 100`. The attacker is revealed on hit or miss. |

## Animal Form
Animal Form itself always passes `CheckFizzle()` and spends no mana. The real form gate is `AnimalForm.Morph(m, entryID)`.

Morph behavior:

| Step | Compiled behavior |
| --- | --- |
| Last form | `m_LastAnimalForms[m]` stores the attempted entry ID before the skill check. |
| Skill requirement | If `Ninjitsu.Value < entry.ReqSkill`, morphing returns `NoSkill`. |
| Success chance | If `Ninjitsu.Value < entry.ReqSkill + 37.5`, chance is `(Ninjitsu - ReqSkill) / 37.5`; otherwise success is guaranteed. |
| Skill gain | Successful morphs call `CheckSkill(SkillName.Ninjitsu, 0.0, 37.5)`. |
| Body changes | The caster is dismounted, `BodyMod` and `HueMod` are assigned from the entry, optional skill mods are attached, and optional mount-speed packets are sent. |
| Timer | `AnimalFormTimer` ticks every second and removes the form if the Mobile is deleted, dead, no longer has the expected body, or no longer has the expected nonzero hue. |
| Removal | `RemoveContext()` removes skill mods, disables speed boost, optionally resets body and hue, plays particles, and stops the timer. |

### Animal Forms
| Entry | Required Ninjitsu | BodyMod | Bonuses and hooks |
| --- | ---: | ---: | --- |
| Kirin | `100.0` | `132` | Speed boost. Adds `20` stamina-regeneration points through `RegenRates` before the ML player cap. |
| Unicorn | `100.0` | `122` | Speed boost. |
| Mystical Fox | `82.5` | `246` | Speed boost. `PlayerMobile.StrMax` adds `20` while in this form. |
| Grey Wolf | `82.5` | `225` | Speed boost. `PlayerMobile.StrMax` adds `20` while in this form. |
| Llama | `70.0` | `220` | Speed boost. |
| Forest Ostard | `70.0` | `210` | Speed boost. |
| Bull Frog | `50.0` | `81` | When hit by a melee weapon, the attacker receives `Poison.Regular`. |
| Giant Serpent | `50.0` | `21` | On melee hit, the defender receives `Poison.Lesser`. |
| Dog | `40.0` | `217` | Hit-point regeneration points gain `Ninjitsu.Fixed / 30`. |
| Cat | `40.0` | `201` | Hit-point regeneration points gain `Ninjitsu.Fixed / 30`. |
| Rat | `0.0` | `238` | Adds a non-cap-obeying `+20.0` Stealth skill mod. |
| Rabbit | `20.0` | `205` | Adds a non-cap-obeying `+20.0` Stealth skill mod. |
| Squirrel | `20.0` | `278` | No special bonus in this source. |
| Ferret | `40.0` | `279` | Adds a non-cap-obeying `+10.0` Stealing skill mod. |
| Cu Sidhe | `60.0` | `277` | Speed boost. Every 8 timer ticks, if hurt and carrying bandages in backpack, consumes one bandage and heals `20..50` hits. |
| Reptalon | `90.0` | `2000` | Speed boost. In warmode, breathes at a new combat target after LOS/range checks, freezes the caster for 1 second, then applies the compiled `AOS.Damage(target, caster, 20, !target.Player, 0, 100, 0, 0, 0)` call. |

### Animal Form Restrictions
While `AnimalForm.UnderTransformation(this)` is true, `PlayerMobile.AllowSkillUse()` blocks `ArmsLore`, `Begging`, `Discordance`, `Forensics`, `Inscribe`, `Mercantile`, `Meditation`, `Peacemaking`, `Provocation`, `RemoveTrap`, `Spiritualism`, `Stealing`, and `Tasting`.

## Shadowjump
`Shadowjump.CheckCast()` requires `PlayerMobile.IsStealthing` for player casters. Targeting then checks stealth again, refuses faction sigil carriers, refuses overloaded casters, runs `SpellHelper.CheckTravel()` for `TeleportFrom` and `TeleportTo`, checks `map.CanSpawnMobile()`, checks `SpellHelper.CheckMulti()`, then calls `CheckSequence()`.

On success, the caster turns toward the original target point, moves directly by assigning `m.Location = to`, calls `ProcessDelta()`, sends particles at the old location, plays sound `0x512`, and calls `Stealth.OnUse(m)` for `PlayerMobile` casters.

## Mirror Image
Mirror Image refuses mounted casters, follower counts that would exceed `FollowersMax`, and `HorrificBeastSpell` transformation. On success it creates a `Clone` at the caster location.

The `Clone` Mobile:

| Field or behavior | Compiled behavior |
| --- | --- |
| AI | `AI_Melee`, `FightMode.None`, forced `CloneAI`. |
| Appearance | Copies body, hue, sex, name, title, kills, hair, facial hair, all skills, and simple cloned visible items by `ItemID`, `Hue`, and `Layer`. |
| Summon ownership | `Summoned = true`, `SummonMaster = caster`, `ControlOrder = Follow`, `ControlTarget = caster`. |
| Duration | `TimeSpan.FromSeconds(30 + caster.Skills.Ninjitsu.Fixed / 40)`. |
| Damage behavior | Any `OnDamage()` call deletes the clone. Corpse is deleted on death. |
| Buff | Adds `BuffIcon.Projection` to the caster for the clone duration and removes it on delete. |
| Attack redirection | `BaseWeapon.OnHit()` checks `MirrorImage.HasClone(defender)` and rolls `defender.Skills.Ninjitsu.Value / 150.0`; on success it redirects the hit to a nearby summoned `Clone` owned by the defender. |

## Ninja Tools
| Item | Use path | Requirements | Effect |
| --- | --- | --- | --- |
| `SmokeBomb` | Double-click from backpack | `Ninjitsu >= 50.0`, skill cooldown elapsed, `Mana >= 10` | Temporarily enables `SkillHandlers.Hiding.CombatOverride`, calls `UseSkill(Hiding)`, spends 10 mana and consumes the item only if hiding succeeds. |
| `EggBomb` | Double-click from backpack | Same as `SmokeBomb` | Same as `SmokeBomb`; deserialization remaps item ID `0x2809` back to `0x2808`. |
| `LeatherNinjaBelt` | Double-click to throw; context menu to load/unload | Must be rooted on the user, loaded with `Shuriken`, have a free hand, and not be on `NinjaWepCooldown` | Throws shuriken at range `2..10`, damage `3..5`, with optional poison. Max loaded uses is `10`. |
| `Fukiya` | Double-click to shoot; context menu to load/unload | Must be rooted on the user, loaded with `FukiyaDarts`, have a free hand, and not be on `NinjaWepCooldown` | Shoots at range `0..6`, damage `4..6`, with optional poison. Max loaded uses is `10`. |
| `Shuriken` | Ammo item and `ICraftable` | Crafted stack amount stored as `UsesRemaining`; exceptional crafting doubles uses | Can carry poison and poison charges. |
| `FukiyaDarts` | Ammo item and `ICraftable` | Crafted stack amount stored as `UsesRemaining`; exceptional crafting doubles uses | Can carry poison and poison charges. |

Crafting definitions found in source:

| Item | Craft system | Skill range | Resource |
| --- | --- | ---: | --- |
| `SmokeBomb` | Alchemy, `Potions` | `90.0..120.0` | `1` Eggs plus `3` Ginseng |
| `Shuriken` | Blacksmithy, `Bladed` | `45.0..95.0` | `5` Iron Ingots |
| `FukiyaDarts` | Bowcraft/Fletching | `50.0..90.0` | `1` Board |
| `Fukiya` | Carpentry | `60.0..85.0` | `6` Boards |
| `LeatherNinjaBelt` | Tailoring | `50.0..75.0` | `5` Leather |

## Serialization
| Class | Versioning behavior |
| --- | --- |
| `BookOfNinjitsu` | Calls `base.Serialize()`, writes version `1`, and has no extra fields. Base `Spellbook` serializes crafter, slayers, attributes, skill bonuses, content, and count. |
| `Clone` | Calls `base.Serialize()`, writes encoded version `0`, and writes `m_Caster`. On deserialize it reads `m_Caster` and calls `MirrorImage.AddClone(m_Caster)`. |
| `SmokeBomb`, `EggBomb` | Version `0`, no custom fields. `EggBomb` also repairs old item ID `0x2809` to `0x2808` on load. |
| `Shuriken`, `FukiyaDarts`, `Fukiya`, `LeatherNinjaBelt` | Version `0`, then `UsesRemaining`, serialized `Poison`, and `PoisonCharges`. |
| Runtime tables | `AnimalForm` contexts, last-form selections, `DeathStrike` pending hits, `KiAttack` pending starts, `SurpriseAttack` maluses, and special-move contexts are static runtime tables and are not explicitly serialized by these classes. |

## Known Issues
| Issue | Evidence from trace | Impact |
| --- | --- | --- |
| `AnimalFormEntry` ignores its `hueMod` constructor parameter. | The constructor receives `hueMod` but assigns `m_HueMod = hue`. | Animal form body hue comes from the display hue/random animal hue, and explicit `hueMod` values cannot be represented. |
| `AnimalFormGump.OnResponse()` does not re-run the full cast restrictions. | Reply handling checks mana and `BaseFormTalisman.EntryEnabled(sender.Mobile, entry.Type)`, then calls `Morph(m_Caster, entryID)`. It does not repeat the polymorph, transformation, disguise, or body-mod checks from `CheckCast()` and `OnCast()`. | A stale form-selection Gump can apply a form after caster state has changed. |
| `Shadowjump` can finish the same spell sequence twice. | `Target()` calls `FinishSequence()` and `InternalTarget.OnTargetFinish()` also calls `m_Owner.FinishSequence()`. | This can double-run cleanup/state transitions for the same cast. |
| Mirror Image attack redirection leaks a pooled mobile-range enumerable. | `BaseWeapon` iterates `defender.GetMobilesInRange(4)` while searching for a `Clone`, with no visible `Free()` call. | Repeated combat redirection checks can leak pooled enumerables. |
| Ninja projectile double-clicks assume `PlayerMobile`. | `Fukiya.OnDoubleClick()` and `LeatherNinjaBelt.OnDoubleClick()` cast `from` directly to `PlayerMobile`. | Non-player Mobile callers can throw instead of failing cleanly. |
| Ninja projectile combat has a suspicious attacker-skill typo. | `NinjaWeapon.CombatCheck()` assigns `Skill atkSkill = defender.Skills.Ninjitsu` and later calls `attacker.CheckSkill(atkSkill.SkillName, chance)`. | Current behavior still checks the attacker's Ninjitsu skill name, but the defender-sourced skill object is an unnecessary hardcoded band-aid and should be corrected for clarity and safety. |
