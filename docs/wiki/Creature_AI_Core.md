# Creature AI Core

## Overview

`BaseCreature` is the shard's shared non-player `Mobile` base class. It owns creature AI selection, fight modes, target acquisition hooks, controlled and summoned follower state, taming and loyalty, creature loot generation, death rewards, breath/healing hooks, bard state, serialization, and a large set of shard-specific spawn/death side effects.

The concrete AI state machine lives in `Behavior.cs` as `BaseAI` plus the derived AI classes. `BaseCreature.ChangeAIType()` selects one of those `BaseAI` implementations, and the AI timer drives `BaseCreature.OnThink()`, bard behavior, uncontrolled `Think()`, or controlled pet `Obey()` depending on the creature state.

This is not a command package. No `CommandSystem.Register()` entry point is present in the traced files. Staff interaction is through normal RunUO command-property tools such as `Props`, pet context menus, speech keywords, and other systems that instantiate or mutate `BaseCreature`.

## Source Files

| File | Type | Purpose |
| --- | --- | --- |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | `Mobile` subclass | Core creature state, lifecycle hooks, taming, pet control fields, loot/death behavior, combat hooks, and persistence. |
| `Data/Scripts/Mobiles/Base/Behavior.cs` | AI framework | `AIType`, `ActionType`, `BaseAI`, pet command target flow, movement helpers, and concrete AI classes. |
| `Data/Scripts/Custom/Combat/AIOverhaul/AITacticalTargeting.cs` | helper | Exact-type tactical targeting whitelist, target bonus scoring, and skirmisher spacing bands referenced by `BaseCreature` and `Behavior.cs`. |
| `Data/Scripts/Scripts.csproj` | project file | Explicit compile list for scripts. `BaseCreature.cs` and `Behavior.cs` are listed here. |

## Core Entry Points

| Entry point | Trigger | Compiled behavior |
| --- | --- | --- |
| `BaseCreature(AIType, FightMode, int, int, double, double)` | Creature construction | Converts old perception range `10` to default `16`, initializes loyalty to `100`, stores AI/ranges/fight mode/speeds, creates spell lists, clears control fields, sets the next reacquire time, calls `ChangeAIType(AI)`, calls `SpeechType.OnConstruct()` when present, and calls `GenerateLoot(true)`. |
| `ChangeAIType(AIType)` | Constructor, `AI` property set, deserialization | Stops the previous AI timer, uses `ForcedAI` if supplied, otherwise creates a concrete `BaseAI` for selected AI types. |
| `BaseAI.AITimer.OnTick()` | AI timer tick | Stops deleted or internal-map creatures, delays deactivation for inactive sectors, calls `BaseCreature.OnThink()`, then dispatches bard pacify/provoke, uncontrolled `Think()`, or controlled `Obey()`. Searching-capable creatures also periodically run hidden-player detection. |
| `AcquireFocusMob(...)` | AI target search | Applies bard/control/constant-focus overrides, enforces reacquire delay, scans mobiles in range, filters invalid targets, scores candidates, adds tactical bonuses when enabled, and stores the chosen `FocusMob`. |
| `OnAfterSpawn()` | Spawner lifecycle | Stores swim/walk defaults, applies dangerous-region line-of-sight hiding, computes world difficulty heat, runs many map/region/type-specific mutations, and can call `BeefUp(this, Heat)`. |
| `AggressiveAction(...)` | Mobile aggression | Propagates owner criminal/aggressor handling for controlled creatures, lets AI retarget, clears flee state, forces target reacquisition, handles Deathly Mask breakage, and may order pets to attack a new aggressor. |
| `OnDamage(...)` / `OnDamagedBySpell(...)` / melee hooks | Combat | Applies special damage scalars, honor/tactical reacquire hooks, ammo recovery, stealing-on-hit coin theft, poison, dispel, race sounds, and custom attack speech. |
| `OnBeforeDeath()` | Before corpse/death finalization | Applies citizen murder penalties, difficulty loot, player-kill special drops, death-knight/holy/vampire/ranger reward hooks, treasure maps/paragon chests, final loot generation, inhuman speech death hooks, and honor kill completion. |
| `OnDeath(Container)` | Death finalization | Activates premium spawners, resolves pet/summon/bard killer ownership, logs battles, checks quests, drops relics, handles bonded-pet death state, awards fame/karma/quest/faction kill credit, and optionally deletes the corpse. |
| `Serialize()` / `Deserialize()` | World save/load | Writes version `19` positional creature state and restores older versions with guarded reads and migrations. |

## AI Types And Action States

### `FightMode`

| Mode | Targeting behavior found in code |
| --- | --- |
| `None` | Clears `FocusMob`; no acquisition. |
| `Aggressor` | Only valid if the creature has aggressor/aggressed entries or faction/ethic allegiance; candidates must have attacked or been attacked, or be allegiance enemies. |
| `Strongest` | Scores by `Tactics.Value + Str`. |
| `Weakest` | Scores by negative current hits, so lower-hit targets rank higher. |
| `Closest` | Scores by negative distance, so closest targets rank higher. |
| `Evil` | Starts with aggressor/allegiance checks, then also accepts players or controlled pets with low karma, criminal state, or kills. Valid evil targets receive `CriminalAction(false)`. |
| `Good` | No special branch was found; it falls through to the default non-aggressor acquisition path and default distance scoring. |
| `CharmMonster` / `CharmAnimal` | Uses aggressor-style filtering, then accepts uncontrolled negative-karma `BaseCreature` candidates. Deserialization remaps saved `CharmMonster` to `Closest` and `CharmAnimal` to `Aggressor`. |

### `AIType`

| `AIType` | Runtime AI selected by `ChangeAIType()` | Notes |
| --- | --- | --- |
| `AI_Use_Default` | Property setter resolves it to `m_DefaultAI` before calling `ChangeAIType()`. | No direct switch case. |
| `AI_Melee` | `MeleeAI` | Standard acquire, chase, melee, guard, and low-health flee behavior. |
| `AI_Animal` | `AnimalAI` | Acquires targets like melee, but uncontrolled/unsummoned animals flee at under 10% hits. |
| `AI_Archer` | `ArcherAI` | Maintains ranged spacing, checks ranged ammo or quiver ammo, and flees when out of ammo. |
| `AI_Healer` | `HealerAI` | Looks for same-team `BaseCreature` targets needing cure, greater heal, or heal, otherwise follows weakest friend or wanders. |
| `AI_Vendor` | `VendorAI` | Wanders/interacts with nearby speakers and flees when attacked. |
| `AI_Mage` | `MageAI` | Uses normal AI plus spell target processing, healing, teleport-on-stuck, dispel, poison, and spell selection behavior. |
| `AI_Berserk` | `BerserkAI` | Acquires closest faction friend or foe using closest-style combat. |
| `AI_Predator` | `MeleeAI` | The `PredatorAI` class exists but the switch currently instantiates `MeleeAI`. |
| `AI_Thief` | `ThiefAI` | Chases targets, prepares disarm, and attempts to steal weapons, reagents, spellbooks, runebooks, potions, scrolls, staves, or gold. |
| `AI_Citizen` | None | The enum and `CitizenAI` class exist, but `ChangeAIType()` has no `AI_Citizen` case. |

### `ActionType`

| State | Base behavior |
| --- | --- |
| `Wander` | Clears warmode/combat focus and moves randomly in home range unless herded, waypointed, animated-dead-following, or idling. |
| `Combat` | Faces the combatant or returns to `Wander` when the combatant is gone, dead, off-map, or a dead bonded pet. |
| `Guard` | Holds warmode for about 10 seconds, then returns to `Wander`. |
| `Flee` | Moves away from `FocusMob` until it reaches the configured flee distance, then returns to `Guard`. |
| `Backoff` | Base implementation is inert; animal and predator AI override it. |
| `Interact` | Base implementation is inert; vendor AI uses it for nearby speakers. |

## Target Acquisition

`AcquireFocusMob()` is the central targeting path used by most AI classes. It short-circuits in this order:

| Step | Behavior |
| --- | --- |
| Deleted creature | Returns false. |
| Bard provoked | Uses `BardTarget` if it still exists. |
| Controlled creature | Uses `ControlTarget` if alive, visible, in range, and not a dead bonded pet; otherwise clears non-master stale targets. |
| Constant focus | Uses `ConstantFocus` directly. |
| `FightMode.None` | Clears focus and returns false. |
| `FightMode.Aggressor` with no aggression/allegiance state | Clears focus and returns false. |
| Reacquire delay | Uses `NextReacquireTime`; default `ReacquireDelay` is 10 seconds. |
| Range scan | Copies `Map.GetMobilesInRange()` into a `List<Mobile>`, frees the pooled enumerable, then scores the copy. |

Candidate filters reject deleted, blessed, self, dead, dead bonded pets, staff, non-player targets when `bPlayerOnly` is true, unseen targets, invalid summon targets, non-friends for `bFacFriend`, honor-active players unless already the combatant, and invalid faction foes when required.

| Ranking mode | Formula |
| --- | --- |
| `Strongest` | `m.Skills[SkillName.Tactics].Value + m.Str` |
| `Weakest` | `-m.Hits` |
| Default / closest modes | `-GetDistanceToSqrt(m)` |

The selected target must have the highest score and be in line of sight.

## Tactical Targeting Overlay

`BaseCreature.TacticalTargetProfile` calls `AITacticalTargeting.ResolveProfile(this)`. The helper only enables exact-type, uncontrolled, unsummoned creatures with a non-null `AIObject`, `FightMode.Closest`, and `AI_Melee` or `AI_Archer`.

| Profile | Exact types currently whitelisted | Bonus behavior |
| --- | --- | --- |
| `Bruiser` | `HeadlessOne`, `Ratman`, `Lizardman` | `+1.0` for current combatant; `+2.0` if target is within `RangeFight + 1`. |
| `Skirmisher` | `RatmanArcher`, `LizardmanArcher` | `+1.0` for current combatant; `+3.0` if the target is casting; `+2.0` if target health is `<= 35%`. |
| `Captain` | None in the current whitelist | Scoring code exists: `+2.0` casting, `+1.5` if target health is `<= 50%`, `+1.0` if nearby team size is at least two. |
| `Support` | None in the current whitelist | No bonus logic beyond the default zero bonus. |

All tactical bonuses are clamped to `0.0..4.0`. Archer skirmishers also use a spacing band of minimum `3` and maximum `Math.Min(6, Math.Max(3, weaponMaxRange - 1))`.

Tactical reacquisition is requested when a tactical creature takes at least `ceil(HitsMax * 0.15)` damage from a non-combatant attacker, is damaged by a spell from a non-combatant, or is hit by a new melee attacker. The request sets `NextReacquireTime` to `DateTime.MinValue`.

## Pet Control And Speech Commands

Controlled creatures use `OrderType` plus `ControlMaster`, `ControlTarget`, and `ControlDest`. Owners and pet friends can also use context menu entries; friends are limited to `Follow`, `Stay`, and `Stop`.

| Order | Speech examples in code comments/keywords | Behavior |
| --- | --- | --- |
| `Come` | `all come`, `<name> come` | Clears target and follows the master to range 1; loses the order if master is beyond perception. |
| `Drop` | `<name> drop` | Moves backpack contents to the creature's world location when `CanDrop` allows it. |
| `Follow` | `all follow`, `all follow me`, `<name> follow`, `<name> follow me` | Follows a picked target or the speaker; distant targets can cause direct `MoveToWorld()` relocation. |
| `Friend` / `Unfriend` | `<name> friend`, `<name> unfriend` | Adds or removes pet friends after target selection, trade checks, young-player checks, and owner validation. |
| `Guard` | `all guard`, `all guard me`, `<name> guard` | Guards the owner and looks through the owner's aggressor/aggressed lists for nearest valid threats. |
| `Attack` | `all kill`, `all attack`, `<name> kill`, `<name> attack` | Begins a target pick, rejects blessed/scary/faction-guard targets, then attacks if control succeeds. |
| `Patrol` | `<name> patrol` | Stubbed with "This order is not yet coded." |
| `Release` | `<name> release` | Opens confirmation for normal pets or immediately releases summons; special henchman/porter/robot branches reset linked items. |
| `Stay` | `all stay`, `<name> stay` | Holds position and keeps the order active. |
| `Stop` | `all stop`, `<name> stop` | Clears target, sets home to current location, and under non-ML resets order to `None`. |
| `Transfer` | `<name> transfer` | Starts a secure trade containing an internal `TransferItem` after control, young-player, trade, follower-slot, and recent-combat checks. |

Game Masters can say a creature's name plus `obey` to call `SetControlMaster(e.Mobile)`; summoned creatures also move `SummonMaster` to the GM.

## Control Chance, Taming, And Loyalty

`CheckControlChance(Mobile)` rolls `GetControlChance(m)`. Success increases loyalty by `1`; failure plays anger animation/sound and lowers loyalty by `3`.

| Step | Formula or behavior |
| --- | --- |
| Automatic success | Returns `1.0` when `MinTameSkill <= 29.1`, the creature is summoned, or the caller is `GameMaster+`. |
| Mastery adjustment | If `MinTameSkill > -24.9` and `Taming.CheckMastery(m, this)` succeeds, the effective minimum tame skill becomes `-24.9`. |
| Taming value | `(Taming.Base or Taming.Value) * 10`, cast to `int`. |
| Lore value | Starts from Taming, not Animal Lore. It is replaced by Druidism when Druidism is higher than Taming for the chosen base/value mode. |
| Skill deltas | `SkillBonus = taming - (MinTameSkill * 10)` and `LoreBonus = lore - (MinTameSkill * 10)`. |
| Delta multipliers | Positive skill and lore deltas use `6`; negative skill deltas use `28`; negative lore deltas use `14`. |
| Base chance | `chance = 700 + ((SkillBonus * SkillMod + LoreBonus * LoreMod) / 2)`. |
| Clamp | If `chance >= 0 && chance < 200`, it becomes `200`; if `chance > 990`, it becomes `990`. |
| Loyalty penalty | Subtracts `(MaxLoyalty - Loyalty) * 10`. Final return is `chance / 1000.0`. |

Feeding a controlled pet by dragging preferred food onto it can restore stamina and loyalty. Under `Core.SE`, any valid food sets loyalty to `MaxLoyalty`; otherwise each unit has a 50% chance to add `10` loyalty. Feeding by the owner also starts or completes bonding when `IsBondable`, taming requirements are met, and `BondingDelay` has elapsed.

`LoyaltyTimer` ticks every five minutes but only performs the full scan once per hour. It lowers controlled commandable pets by `10` loyalty per hourly pass, warns below `10`, releases at `0`, deletes abandoned dead bonded pets after `BondingAbandonDelay`, and deletes eligible previously owned untamed creatures.

## Spawn Scaling And Loot

| Mechanic | Compiled behavior |
| --- | --- |
| `AdditionalHitPoints(BaseCreature, int)` | If difficulty is greater than `1`, adds `(difficulty * 10)%` to `HitsMax`; then refills hits. |
| `BeefUp(BaseCreature, int)` | Fame caps difficulty: fame `>= 20000` forces `up = 0`; `>= 18000` caps to `1`; `>= 15000` caps to `2`; `>= 10000` caps to `3`. Paragons return without changes. |
| `BeefUp` stat buffs | For positive `up`: hits seed `+10% * up`, strength `+10% * up`, intelligence/dexterity/skills `+30% * up`, fame/karma `+10% * up`, damage min/max `+up`, existing backpack gold gains `+10% * up`, tamable `MinTameSkill` gains `+15% * up`. |
| Searching grant | If creature searching is enabled and the creature's Searching skill is not already above 10, it is set to `IntelligentAction.GetCreatureLevel(bc) + 10`. |
| `BeefUpLoot(BaseCreature, int)` | Non-paragon creatures with backpacks get one extra loot pack when `up >= Utility.Random(7)`, selected by fame tier from `Meager` through `UltraRich`. |
| `GenerateLoot(bool spawning)` | Sets spawning context, stores killer luck when not spawning, calls virtual `GenerateLoot()`, adds an extra paragon loot pack by fame tier, then clears context. |
| `AddLoot(LootPack)` | Does nothing for summoned creatures; otherwise creates a non-movable backpack if missing and calls `pack.Generate(...)`. |

## Combat Hooks

| Mechanic | Formula or behavior |
| --- | --- |
| Breath eligibility | `CanBreath` is `HasBreath && !Summoned`. Breath checks run from `OnThink()` when the delay has elapsed, target is valid, in LOS, in `BreathRange`, and the creature is not bard pacified. |
| Breath damage | `damage = (int)(Hits * BreathDamageScalar)`, default scalar `0.20`; paragon damage is divided by `Paragon.HitsBuff`; damage is capped at `100` and then raised to at least `DamageMax`. |
| Breath timing | Default next-breath delay is `10.0 + RandomDouble() * 15.0` seconds; effect delay is `1.3` seconds and damage delay is `1.0` second. |
| Self/owner healing | `CanHeal` and `CanHealOwner` default false. When enabled, self healing starts below `78%` hits or poison; owner healing starts below `78%` owner hits. Default heal delay is `6.5` seconds; owner interval is `30` seconds. |
| Healing amount | On successful normal healing: `min = Anatomy / 10 + Healing / 6 + 4`, `max = Anatomy / 8 + Healing / 3 + 4`, plus `10` max when healing self; result is multiplied by `HealScalar`. |
| Poison cure chance | `(Healing - 30.0) / 50.0 - poisonLevel * 0.1`, requiring Healing and Anatomy at least `60.0`. |
| Damage received | AOS controlled non-summoned pets have a 20% chance to multiply incoming damage by `BonusPetDamageScalar`; hostile wild creatures can have damage or critical multipliers applied by `MyServerSettings`. Evil Omen adds `25%`; Blood Oath reflects `110%` damage to the attacker when oath target matches this creature. |
| Melee given | Applies `HitPoison` by `HitPoisonChance`, increases paragon poison level, dispels dispellable defenders, and calls custom attack speech. |
| Melee received | Can dispel dispellable attackers and supports shard-specific close-range coin theft by high-stealing/snooping players against creatures with `Coins`. |

## Death, Awards, And Bonded Pets

`OnBeforeDeath()` and `OnDeath()` are highly overloaded. Important shared behavior:

| Stage | Behavior |
| --- | --- |
| Citizen murder | If `AI == AI_Citizen`, the last killer is resolved through summoned, controlled, or bard masters. Player killers are marked criminal, gain one kill, and lose disguises. |
| Difficulty loot | Non-paragon creatures with heat above zero call `BeefUpLoot(this, Heat)`. |
| Special player kill hooks | Player slayers can trigger `IntelligentAction`, hoard piles, summon quests, sailor skill, necromancer reagent drops, generic reagent drops, forensics embalming fluid, Death Knight soul transfer, Holy Man banish transfer, race food drops, and ranger golden feathers. |
| Treasure | Non-summoned, non-bonded, award-enabled creatures can pack paragon chests or treasure maps by `TreasureMapLevel`. Young players killing level-one Sosaria targets lower the map level to zero. |
| Final loot | Non-summoned, award-enabled creatures call `GenerateLoot(false)` once when `m_HasGeneratedLoot` is false. |
| Killer ownership | `OnDeath()` resolves summoned, controlled, and bard-provoked creature killers to their owning master before battle logging and quest checks. |
| Bonded pet death | Bonded creatures do not go through normal `base.OnDeath(c)`. They play death sound, clear combat/poison, set hits/stam/mana to zero, set `IsDeadPet = true`, follow their owner, clear aggressor combatants, and track abandon time. |
| Awards | Non-summoned award-enabled deaths distribute fame/karma through looting rights, party shares, faction death handling, XML quest kills, and active quest `OnKill`. |

## Serialization

`BaseCreature` writes version `19`. The stream is positional and must stay aligned with deserialization.

| Version | Data introduced or handled |
| --- | --- |
| `19` | `m_HitsBeforeMod`. |
| `18` | `m_Coins`, `m_CoinType`, `m_SpawnerID`, `m_Swimmer`, `m_NoWalker`. |
| Base block | Current/default AI, range perception/fight, team, active/passive/current speed, home X/Y/Z. |
| `1` | `RangeHome`, attack spell type names, defense spell type names. |
| `2` | `FightMode`, control fields, `MinTameSkill`, `Tamable`, `Summoned`, summon end time, control slots. Versions below `9` read and discard the removed max tame skill. |
| `3` | Loyalty. Older saves default to `MaxLoyalty`. |
| `4` | Current waypoint item. |
| `5` | `SummonMaster`. |
| `6` | Hits/stam/mana/damage min/max seeds. |
| `7` | Physical/fire/cold/poison/energy resistance and damage percentages. |
| `8` | Owners list. |
| `10` | Dead pet, bonded, bonding begin, owner abandon time. |
| `11` | `HasGeneratedLoot`; older saves force it true. |
| `12` | Paragon flag. |
| `13` | Pet friends list and `OrderType.Unfriend` migration for older saves. |
| `14` | `RemoveIfUntamed`, `RemoveStep`. |
| `16` | Loyalty migration multiplies non-max older loyalty by 10. |
| `17` | Delete timer remaining time for unstabled/uncontrolled previously owned creatures. |

After reading, deserialization recalculates standard speeds, starts delete timers, fixes older paragon hue `0x31`, checks stat timers, recreates the AI object, adds follower counts, remaps charm fight modes, and clamps `FollowersMax` to `5` when higher.

## Admin Surface

There are no direct in-game admin commands registered by this system. The important staff-facing properties are exposed through `[CommandProperty]` and therefore are normally edited or inspected with the generic property tools.

| Property group | Examples |
| --- | --- |
| AI and targeting | `AI`, `Debug`, `Team`, `FocusMob`, `FightMode`, `RangePerception`, `RangeFight`, `RangeHome`, `TacticalTargetProfile`, `UsesAITacticalTargeting`. |
| Movement and home | `ActiveSpeed`, `PassiveSpeed`, `CurrentSpeed`, `Home`, `CurrentWayPoint`, `TargetLocation`, `Swimmer`, `NoWalker`. |
| Pet control | `Controlled`, `ControlMaster`, `SummonMaster`, `ControlTarget`, `ControlDest`, `ControlOrder`, `MinTameSkill`, `Tamable`, `Summoned`, `ControlSlots`. |
| Bard state | `BardProvoked`, `BardPacified`, `BardMaster`, `BardTarget`, `BardEndTime`. |
| Persistence/special state | `IsStabled`, `IsPrisoner`, `HitsBeforeMod`, `Coins`, `CoinType`, `SpawnerID`, `IsBonded`, `BondingBegin`, `OwnerAbandonTime`, `RemoveIfUntamed`, `RemoveStep`. |
| Damage/resistance seeds | Physical/fire/cold/poison/energy resist and damage seeds, plus `ChaosDamage` and `DirectDamage` properties. |

## Known Issues

| Issue | Evidence from trace | Operational impact |
| --- | --- | --- |
| `AI_Citizen` does not instantiate an AI object. | `AIType.AI_Citizen` and `CitizenAI` exist, and multiple mobiles assign `AI_Citizen`, but `ChangeAIType()` has no `AI_Citizen` switch case. | Citizen mobiles can have `AIObject == null`, so they do not get the `BaseAI` timer, speech/context-menu AI behavior, or normal AI activation. |
| `AI_Predator` is mapped to `MeleeAI`. | The `AI_Predator` switch case comments out `new PredatorAI(this)` and creates `new MeleeAI(this)` instead. | The compiled `PredatorAI` class is effectively unused through normal AI selection. |
| The tactical targeting helper moved during POST-BATCH-H. | `Data/Scripts/Scripts.csproj` now includes `Custom\Combat\AIOverhaul\AITacticalTargeting.cs`, matching the moved helper path. | Maintained project hygiene and live runtime script visibility are aligned for the helper path; this row remains a source-trace note rather than an active project drift issue. |
| Several pooled range enumerables are not freed. | `AcquireFocusMob()` explicitly frees `Map.GetMobilesInRange()`, but other paths use `foreach` directly over `GetMobilesInRange()`, including `GetTeamSize()`, `TeleportPets()`, `DoOrderAttack()`, `HealerAI.Find()`, and `Searching()`. | Repeated AI scans can leak pooled enumerables in active areas. |
| AI searching can divide by zero and has collapsed delay math. | `AITimer.OnTick()` calculates `int delay = (15000 / m_Owner.m_Mobile.Int)` after only checking Searching skill, then uses integer ratios `9 / 10` and `10 / 9` for min/max bounds. | A Searching-capable creature with `Int == 0` can throw; otherwise the lower bound becomes zero seconds instead of the intended 90% delay. |
| Control chance does not use Animal Lore. | `GetControlChance()` initializes both `taming` and `lore` from `SkillName.Taming`; only higher Druidism can replace `lore`. | Animal Lore has no effect on pet control chance despite the lore-style formula. |
| Release logic assumes some special followers have backpacks. | `DoOrderRelease()` builds `new ArrayList(m_Mobile.Backpack.Items)` for `HenchmanFamiliar`, `PackBeast`, and `GolemPorter` branches without checking `Backpack`. | Releasing a special follower without a backpack can null-reference. |
| `ChaosDamage` and `DirectDamage` command properties are not serialized. | The fields and properties exist, but version `7` only writes/reads physical, fire, cold, poison, and energy damage/resistance values. | Staff-set chaos/direct damage seeds reset on world save/load. |
| Guard movement response looks incomplete. | `OnMovement()` retains `m_NoDupeGuards` and guard-lock helpers, but after the murderer-in-range filter it performs no guard dispatch or state change. | Human murderer proximity handling appears to be a dead branch in the traced code. |
| Hourly loyalty release can dereference a null AI object. | `LoyaltyTimer` calls `c.AIObject.DoOrderRelease()` without checking `AIObject`. | Any controlled commandable creature with an unmapped/null AI can throw when loyalty reaches zero. |
