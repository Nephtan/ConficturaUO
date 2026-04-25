# AI Overhaul Audit

## Purpose

This document is a static-analysis audit of the current AI surface in the Confictura RunUO shard. Its goal is to give future overhaul work a single reference for:

- where AI actually lives
- how AI is assigned to mobiles and NPCs
- what systems silently influence targeting and behavior
- which scripts bypass the stock AI shells
- which gameplay rules are coupled to AI decisions
- where central changes are safe, and where they are likely to break shard behavior

This audit is based on source review and codewide search only. It does not rely on a live server run.

## Executive Summary

The shard does not have one AI system. It has at least six layers that interact:

1. `BaseCreature` owns AI assignment, targeting legality, pet orders, bard state, and reacquire timing.
2. `BaseAI` and the classes in `Data/Scripts/Mobiles/Base/Behavior.cs` provide the stock state-machine shells.
3. Individual mobiles assign an `AIType` and `FightMode` in constructors.
4. Some scripts switch `AI` at runtime.
5. Some mobiles bypass the stock shell system entirely with `ForcedAI`.
6. Many mobiles and encounters layer custom behavior on top with `OnThink`, `OnMovement`, `OnDamage`, `OnDamagedBySpell`, and `OnGotMeleeAttack`.

The most important practical conclusion is this:

- `AcquireFocusMob` is the legal and mechanical choke point for target selection.
- `ChangeAIType` is the central assignment seam for stock shells.
- `OnThink` and the combat event hooks are the main places where bespoke encounter behavior piggybacks on the AI timer.
- `ForcedAI`, runtime `AI = ...` changes, summon-specific target ranking, and civilian/vendor subclasses mean a "global AI change" will not affect everything equally.

Any large overhaul has to treat AI as an ecosystem, not as a single class replacement.

## Review Method

This audit used two levels of review:

- direct reading of the core framework files and high-risk exception files
- codewide pattern searches to inventory assignment styles, override hotspots, and bypass mechanisms

The pattern counts in this document are meant as audit guidance, not perfect gameplay taxonomy. They are most reliable for identifying risk concentrations and exception classes.

## Core Framework

### Core files

These are the primary files that define the AI framework:

- `Data/Scripts/Mobiles/Base/BaseCreature.cs`
- `Data/Scripts/Mobiles/Base/Behavior.cs`
- `Data/Scripts/Mobiles/Base/BaseNPC.cs`
- `Data/Scripts/Mobiles/Base/BasePerson.cs`
- `Data/Scripts/Mobiles/Base/BaseVendor.cs`
- `Data/Scripts/Mobiles/Base/BaseHealer.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Core.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Shared.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Bushido.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Knightship.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Magery.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Necromancy.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Ninjitsu.cs`

### FightMode

`FightMode` determines the broad targeting rule before all the deeper filters and rankings:

- `None`
- `Aggressor`
- `Strongest`
- `Weakest`
- `Closest`
- `Evil`
- `Good`
- `CharmMonster`
- `CharmAnimal`

Important implication:

- `FightMode` is not just "who feels best to attack." It feeds into legality, faction/ethic logic, aggression rules, and the ranking pass inside `AcquireFocusMob`.

### AIType

The stock AI enum currently exposes:

- `AI_Use_Default`
- `AI_Melee`
- `AI_Animal`
- `AI_Archer`
- `AI_Healer`
- `AI_Vendor`
- `AI_Mage`
- `AI_Berserk`
- `AI_Predator`
- `AI_Thief`
- `AI_Citizen`

Important implication:

- The enum advertises more behaviors than most content actually uses.
- `AI_Predator` is especially dangerous to assume is live, because the current `ChangeAIType` mapping routes `AI_Predator` to `MeleeAI`, not `PredatorAI`.

### ActionType

The stock state machine operates in these action states:

- `Wander`
- `Combat`
- `Guard`
- `Flee`
- `Backoff`
- `Interact`

The stock AI shells are mostly different implementations of how those action states transition and what each state actually does.

## BaseCreature Responsibilities

`BaseCreature` is not only a data container. It is a major AI coordinator.

### AI assignment

`BaseCreature.ChangeAIType(AIType)` is the central mapping point from enum to stock AI class. That means any system that writes `AI = ...` eventually passes through the same shell-selection seam, unless `ForcedAI` intercepts it first.

Current stock mapping:

- `AI_Melee` -> `MeleeAI`
- `AI_Animal` -> `AnimalAI`
- `AI_Berserk` -> `BerserkAI`
- `AI_Archer` -> `ArcherAI`
- `AI_Healer` -> `HealerAI`
- `AI_Vendor` -> `VendorAI`
- `AI_Mage` -> `MageAI`
- `AI_Predator` -> `MeleeAI` in practice, with the `PredatorAI` line commented out
- `AI_Thief` -> `ThiefAI`

### ForcedAI

`ForcedAI` short-circuits `ChangeAIType`. If a mobile overrides `ForcedAI`, the normal enum-to-shell mapping effectively stops mattering.

This is one of the biggest reasons a naive overhaul can miss important content.

### Target ranking

`GetFightModeRanking` is the stock score hook used by `AcquireFocusMob` after legality checks. The default behavior is:

- `Strongest`: `Tactics + Str`
- `Weakest`: `-Hits`
- default: `-distance`

Important implication:

- This is the safest central seam for adding richer scoring, but only after all the engine legality filters have already accepted the candidate.

### Reacquire timing

The central reacquire controls are:

- `NextReacquireTime`
- `ReacquireDelay` with a default of 10 seconds
- `ReacquireOnMovement` with a default of `false`
- `ForceReacquire()`, which simply resets `NextReacquireTime`

Important implication:

- The engine already prefers "reacquire occasionally or when something meaningful happens" over maintaining a persistent threat table.
- This strongly favors event-driven tactical retargeting rather than permanent target bookkeeping.

### Behavior-affecting state owned by BaseCreature

Several shard rules that feel unrelated to AI are still wired directly into the AI surface:

- `ControlTarget` and `ControlOrder`
- `ConstantFocus`
- `BardProvoked`
- `BardPacified`
- `Combatant`
- `FightMode`
- `Team`
- `Controlled` and `Summoned`

These are not cosmetic fields. They alter how `AcquireFocusMob`, `Obey`, `Think`, and aggression resolution behave.

## The BaseAI Timer And Execution Flow

The stock timer loop lives in `Behavior.cs` and runs the AI in a fixed order:

1. sector and activity guards
2. `m_Mobile.OnThink()`
3. bard pacify/provoke handling
4. `Think()` for uncontrolled creatures, `Obey()` for controlled ones
5. optional searching pass

This matters for overhaul planning because:

- encounter scripts often rely on `OnThink()` happening before stock combat logic
- controlled pets do not use the same path as wild mobs
- bard effects get their own branch before normal combat logic
- "AI speed" and "script cadence" are partly the same thing

## AcquireFocusMob: The Real Targeting Choke Point

`AcquireFocusMob` is the single most important function in the stock targeting stack.

It does much more than "pick a target."

### What it handles before ranking

Before normal candidate scoring, `AcquireFocusMob` already handles:

- bard-provoked fixed targeting
- controlled-pet `ControlTarget`
- `ConstantFocus`
- `FightMode.None`
- reacquire timing gates
- some fight-mode-specific short circuits

### What it filters during the candidate loop

The candidate loop includes gameplay-sensitive checks such as:

- deleted, blessed, self, dead, staff, and visibility checks
- player-only constraints
- line of sight
- summon restrictions
- faction and ethic hostility
- honor restrictions
- harmful action legality
- aggression-mode semantics
- karma / criminal / kill-count / evil-good filters

### Why this matters

If you replace or bypass this logic with a custom threat table, you risk:

- attacking illegal or invalid targets
- breaking bard mechanics
- breaking pet control rules
- ignoring faction and ethic hostility
- creating desync between the AI's "best target" and the server's legal target rules

This is the reason central targeting improvements should be layered after legality, not instead of it.

## Stock AI Shells

### AnimalAI

Current role:

- simple stock brain for creatures using `AI_Animal`
- only flees at very low health for uncontrolled and unsummoned creatures
- otherwise acquires focus and attacks

Implication:

- many "animal" mobs are not truly ecosystem or herd AI; they are simpler combat mobs with slightly different flee/backoff behavior

### ArcherAI

Current role:

- standard ranged brain
- tries to maintain range
- flees when out of ammo
- may flee at low health

Implication:

- a large part of "archer behavior" is really movement envelope plus ammo handling, not tactical target selection

### MageAI

Current role:

- stock caster shell with combat spellcasting, healing, dispel, poison timing, and movement logic
- has a `SmartAI` branch, but that branch currently defaults to `m_Mobile is BaseVendor`

Implication:

- the code already contains richer mage behavior than most hostile mage mobs are currently using
- expanding mage intelligence can likely reuse existing code rather than requiring a new mage brain from scratch

### MeleeAI

Current role:

- standard melee shell for the majority of hostile mobs
- handles reacquisition, chase logic, and target swaps when current target is far or movement is blocked

Implication:

- if you want a shard-wide tactical upgrade, `MeleeAI` plus `AcquireFocusMob` is the largest value seam

### HealerAI

Current role:

- ally-support shell, not normal hostile target-selection logic
- scans same-team visible `BaseCreature` allies
- cures poison first, then greater-heal threshold, then lesser-heal threshold
- when nobody needs help, drifts toward the weakest hostile target

Implication:

- healer behavior is already role-based and team-aware
- forcing a generic hostile scoring model onto healer logic would be a regression

### VendorAI

Current role:

- service NPC behavior
- reacts to `FocusMob` interaction
- flees when attacked

Implication:

- vendor brains are not combat difficulty content and should be excluded from combat-overhaul assumptions

### BerserkAI

Current role:

- aggressive minimal brain
- close on the nearest valid target and stay committed

Implication:

- a good candidate for "low-cost simple aggressor" if you want some mobs to stay dumb on purpose

### ThiefAI

Current role:

- role-specific disruptive AI
- behavior is driven by stealing logic, not pure combat optimization

Implication:

- thief mobs are specialists and should keep their identity rather than being flattened into a generic skirmisher

### CitizenAI

Current role:

- effectively inert shell
- action methods return `false`

Implication:

- any content that sets `AI_Citizen` is opting out of normal combat shell behavior

### PredatorAI

Current role in source:

- an avoidance-first roaming/combat pattern that backs off from perceived danger until hurt or engaged

Current role in practice:

- not mapped by `ChangeAIType`

Implication:

- `PredatorAI` is a dormant or legacy behavior asset, not a live stock assignment path

## OmniAI

`OmniAI` is the closest thing in the codebase to "player-like AI."

### What OmniAI can do

From the `Custom/OmniAI` files, OmniAI is built to:

- inspect the creature's trained skills and decide what systems it can use
- cast from multiple magic schools
- self-heal through several possible skill paths
- swap weapons
- use weapon abilities and stun moves
- react to field spells
- hide when appropriate
- reacquire better targets when the current one gets far away or pathing fails

### Why OmniAI matters

OmniAI shows what the shard already considers "advanced behavior," but it also shows the cost and complexity of full-stack intelligence:

- it is a broad skill-using tactical layer, not a small targeting tweak
- it is mostly used through `ForcedAI`
- it is a better model for special actors than for universal deployment on every mob

### Practical reading

Use OmniAI as a design reference, not as the default answer for ordinary mobs.

Good ideas to borrow:

- hazard awareness
- self-preservation
- better mid-fight reacquire logic
- readable utility actions

Bad idea to universalize:

- every NPC using a full player-kit decision tree

## How AI Is Assigned Today

AI assignment happens in three major ways.

### 1. Constructor-time shell selection

This is the dominant pattern.

Most mobiles pass `AIType` and `FightMode` directly through the `BaseCreature` constructor.

Observed single-line constructor pattern counts:

- `AI_Melee`: 492
- `AI_Mage`: 349
- `AI_Animal`: 96
- `AI_Archer`: 14
- `AI_Citizen`: 1

Observed single-line constructor `FightMode` counts:

- `Closest`: 807
- `Aggressor`: 101
- `Evil`: 20
- `None`: 8

Use these as directional counts, not as perfect truth. Multi-line constructors and nonstandard initialization patterns can hide additional assignments.

### 2. Runtime reassignment

Some scripts explicitly write `AI = AIType.X` after construction or during behavior changes.

Observed runtime AI assignment counts:

- `AI_Melee`: 49
- `AI_Mage`: 29
- `AI_Archer`: 18
- `AI_Citizen`: 11

This matters because constructor AI is not always the final AI.

### 3. ForcedAI bypass

Some scripts ignore normal shell mapping and inject a concrete `BaseAI` instance through `ForcedAI`.

This is a hard bypass of any enum-level overhaul.

## Assignment Patterns By Mobile Family

A quick group-by of single-line constructor signatures shows where the stock shell families are concentrated.

Largest observed buckets:

- `Humanoids / Melee`: 85
- `Animals / Animal`: 65
- `Humanoids / Mage`: 48
- `Reptilian / Melee`: 46
- `Undead / Melee`: 45
- `Undead / Mage`: 41
- `Elementals / Mage`: 37
- `Dragons / Mage`: 34
- `Constructs / Melee`: 31
- `Elementals / Melee`: 29

Practical takeaway:

- most of the shard's regular combat population is still stock `MeleeAI` or `MageAI`
- an overhaul centered on those shells plus target scoring will reach a large amount of content
- that still does not cover the exception systems discussed below

## Civilian, Service, And Social NPCs

These are especially important because they look like mobs in code, but they are not normal combat content.

### BaseNPC

Characteristics:

- built on `AI_Animal` with `FightMode.Aggressor`
- `Blessed = true`
- `AlwaysAttackable = false`
- `ReacquireOnMovement = true`
- `Unprovokable = true`
- `Uncalmable = true`

Implication:

- this is a service/town actor base, not a generic animal combatant

### BasePerson

Characteristics:

- built on `AI_Melee` with `FightMode.Closest`
- `AlwaysAttackable = true`
- `ReacquireOnMovement = true`
- `Unprovokable = true`
- `Uncalmable = true`
- custom `IsEnemy` logic tied to `IntelligentAction`

Implication:

- this class looks like a plain melee person on the surface, but it is already using custom social/region logic

### BaseVendor

Characteristics:

- service NPC base class
- `PlayerRangeSensitive = true`
- interacts with vendor, training, restock, and buy/sell systems

Implication:

- vendor AI is tied to service availability and sector activity, not only combat

### BaseHealer

Characteristics:

- subclass of `BaseVendor`
- when not invulnerable, sets `AI = AI_Mage`, `FightMode = Aggressor`
- also has movement-triggered resurrection and healing logic for nearby players

Implication:

- healers are hybrid service/combat actors and need their own audit bucket

## Dynamic AI Switchers

Some scripts use AI as a mode switch rather than a permanent creature identity.

### Adventurers

Pattern:

- constructor starts from `AI_Melee`
- sets `AI_Citizen`
- then promotes to `AI_Mage`, `AI_Melee`, or `AI_Archer` based on generated citizen type and loadout

Implication:

- constructor AI alone is not enough to classify this mobile

### Syth and Jedi

Pattern:

- `SwitchTactics()` flips between `AI_Melee` and `AI_Mage`

Implication:

- some content already uses AI shells as stance changes
- a future profile system needs to survive AI shell swaps cleanly

### RuneGuardian

Pattern:

- starts as a mage-style creature and can randomly switch to `AI_Melee`

Implication:

- some uniques vary shell identity per spawn, not just per class

### Generated crews and transformed actors

Example:

- `Mobiles/Humanoids/Sailors/Galleons/BasePirate.cs` spawns crew and then assigns AI, fight mode, karma, and identity at runtime

Implication:

- generated encounters can inherit AI from staging code rather than from the class you think you are balancing

## ForcedAI Inventory

These are the main `ForcedAI` users found in the codebase:

- `Data/Scripts/Custom/CloneOfflinePlayerCharacters/CharacterClone.cs`
- `Data/Scripts/Custom/OmniAI/AITester.cs`
- `Data/Scripts/Magic/Jester/Spells/Clowns.cs`
- `Data/Scripts/Magic/Ninjitsu/MirrorImage.cs`
- `Data/Scripts/System/Obsolete/Obsolete.cs`

How they matter:

- `CharacterClone` forces `OmniAI`
- `MirrorImage` forces `CloneAI`
- `Clowns` forces `ClownAI`
- test and obsolete content also touch this seam

### CloneAI and ClownAI

These are not general combat brains.

They mostly:

- follow their summon master
- avoid normal searching
- exist to preserve illusion/summon behavior rather than to make tactical combat decisions

Implication:

- do not fold these into a generic targeting overhaul

## Custom Target-Ranking Overrides

The following scripts override `GetFightModeRanking` directly:

- `Data/Scripts/Magic/Death Knight/DevilPact.cs`
- `Data/Scripts/Magic/Druidism/Effects/TreefellowSpell.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendAir.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendEarth.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendFire.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalFiendWater.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritAir.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritEarth.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritFire.cs`
- `Data/Scripts/Magic/Elementalism/Mobiles/ElementalSpiritWater.cs`
- `Data/Scripts/Magic/Enchantments/Mobiles/DevilPactMagic.cs`
- `Data/Scripts/Magic/Misc/SummonDragonSpell.cs`
- `Data/Scripts/Magic/Misc/SummonSkeleton.cs`
- `Data/Scripts/Magic/Misc/SummonSnakesSpell.cs`
- `Data/Scripts/Mobiles/Summoned/BladeSpirits.cs`
- `Data/Scripts/Mobiles/Summoned/DeathVortex.cs`
- `Data/Scripts/Mobiles/Summoned/EnergyVortex.cs`
- `Data/Scripts/Mobiles/Summoned/GasCloud.cs`
- `Data/Scripts/Mobiles/Summoned/Swarm.cs`

Pattern:

- most of these are summons, conjurations, or temporary magical entities

Implication:

- summon identity often depends on nonstandard prey logic
- a universal tactical score must either preserve or explicitly layer on top of these overrides

## Reacquire And Retarget Special Cases

### Default behavior

By default, creatures do not rapidly retarget:

- `ReacquireDelay` defaults to 10 seconds
- `ReacquireOnMovement` defaults to `false`

### Known explicit delay overrides

Observed `ReacquireDelay` overrides:

- `Data/Scripts/Mobiles/Mystical/Wisp.cs`
- `Data/Scripts/Mobiles/Mystical/DarkWisp.cs`
- `Data/Scripts/System/Obsolete/Obsolete.cs`

The live mystical examples both cut delay to 1 second.

### Movement-based retargeting

Observed codewide count for `override bool ReacquireOnMovement`: 286

Important implication:

- movement-triggered retargeting is already a major behavior surface
- a global change to movement-driven reacquire will have wide, uneven effects

### Paragon interaction

Base movement handling also forces reacquire for paragons, even without a per-class override.

Implication:

- not all movement-reactive creatures advertise that behavior with a local override

## Scripted Encounter Hooks

Many important encounters are not stock AI plus stats. They piggyback on the AI timer and combat events.

### Exodus

Relevant behavior:

- `ReacquireOnMovement` is enabled while uncontrolled
- `OnThink()` reactivates field state when not hurt
- `Move()` emits field particles in combat

Implication:

- movement and think cadence are encounter mechanics here, not just pathing details

### PlagueBeast

Relevant behavior:

- `OnThink()` devours nearby corpses that it killed
- corpse handling increases survivability and progression state

Implication:

- changing AI cadence can change boss phase behavior even if no explicit combat formula changes

### Galleon pirates

Relevant behavior:

- `OnThink()` builds ships, validates spawn context, and spawns crews

Implication:

- for some content, AI cadence doubles as encounter bootstrap logic

## Codewide Override Surface

Observed codewide grep counts across `Data/Scripts`:

- `ForcedAI`: 8 matching lines
- `override double GetFightModeRanking`: 19
- `override bool ReacquireOnMovement`: 286
- `override TimeSpan ReacquireDelay`: 3
- `override void OnThink(...)`: 71
- `override void OnMovement(...)`: 131
- `override void OnDamage(...)`: 58
- `override void OnDamagedBySpell(...)`: 19
- `override void OnGotMeleeAttack(...)`: 161
- `override void AggressiveAction(...)`: 2
- `ChangeAIType(...)` call sites: 5
- runtime `AI = AIType...` assignments: 107

How to read these numbers:

- they size the review surface
- they do not mean every hit is a normal hostile AI script
- they do mean encounter logic, summon logic, social NPC logic, and AI assignment are spread across the codebase

## Gameplay Systems Tightly Coupled To AI

Any overhaul has to explicitly respect these gameplay systems because they are already wired into the AI path.

### Pet and summon control

AI interacts directly with:

- `ControlOrder`
- `ControlTarget`
- master-follow behavior
- attack-order propagation

Changing target selection without understanding pet order logic will produce bad pet behavior.

### Bard systems

AI is explicitly branched by:

- bard pacify
- bard provoke

These are not optional buffs layered on top. They are part of the AI flow itself.

### Faction, ethic, honor, and aggression legality

Target selection is already aware of:

- faction relationships
- ethic hostility
- honor rules
- harmful-action legality
- evil/good/criminal/karma filters

Any overhaul that ignores these rules will create gameplay regressions, not just tactical differences.

### Region and service behavior

Town actors, vendors, healers, and some quest actors use AI-adjacent hooks for interaction, resurrection, healing, teaching, or scripted conversation.

### Summon identity

Several magical summons intentionally override prey ranking. Their feel is part of spell identity, not just combat power.

## What Is Safe To Centralize

These are the safest high-value central seams for a future overhaul:

- post-legality target scoring layered onto `AcquireFocusMob`
- opt-in tactical profiles on top of stock shells
- role-sensitive reacquire timing
- event-driven `ForceReacquire()` hooks for meaningful combat events
- better use of existing mage and healer role logic

## What Is Not Safe To Globalize

These are the most dangerous things to change globally without a whitelist:

- replacing `AcquireFocusMob` legality with an external threat table
- changing bard, control-order, or harmful-action semantics
- flattening summon-specific `GetFightModeRanking` overrides
- assuming constructor AI tells the whole truth for every mobile
- treating `ForcedAI` actors as normal stock mobs
- globally speeding up `OnMovement`-driven reacquire
- changing AI timer cadence without auditing `OnThink` encounter scripts

## Recommended Overhaul Workstreams

### Workstream 1: Core tactical layer

Scope:

- add tactical scoring after legality
- leave `FightMode`, control logic, and summon legality intact

### Workstream 2: Whitelist rollout

Scope:

- start with ordinary uncontrolled hostile mobs using stock `MeleeAI`, `MageAI`, and `ArcherAI`
- exclude civilians, vendors, healers, `ForcedAI` actors, scripted summons, and major encounter scripts

### Workstream 3: Exception register

Maintain an explicit list of:

- `ForcedAI` users
- runtime AI switchers
- custom target ranking users
- special reacquire users
- encounter scripts that depend on `OnThink` cadence

### Workstream 4: Mage and healer pass

Scope:

- reuse the richer logic already present in `MageAI`
- preserve healer/support identity rather than forcing generic aggressor scoring

### Workstream 5: Encounter audit

Scope:

- deep-audit bosses and event systems that override `OnThink`, `OnDamage`, `OnDamagedBySpell`, or `OnGotMeleeAttack`
- verify whether each one depends on stock AI cadence, combatant changes, or movement-triggered reacquire

## Minimum Exclusion List For V1

If an overhaul starts tomorrow, the first version should exclude at least:

- `BaseNPC`
- `BasePerson`
- `BaseVendor`
- `BaseHealer`
- all `ForcedAI` users
- all `GetFightModeRanking` override users
- all obvious runtime AI switchers
- scripted encounter bosses and event spawners
- summon illusion/follow-master AI

## Refresh Commands

These are the most useful commands for refreshing this audit after future changes:

```powershell
rg -n "enum FightMode|public enum FightMode" Data
rg -n "AIType\\.|ActionType\\.|class .*AI" Data/Scripts/Mobiles/Base/Behavior.cs Data/Scripts/Mobiles/Base/BaseCreature.cs
rg -l "ForcedAI" Data/Scripts | Sort-Object
rg -l "override double GetFightModeRanking" Data/Scripts | Sort-Object
rg -n "AI = AIType\\.AI_|ChangeAIType\\(" Data/Scripts | Sort-Object
rg -n ": base\\(AIType\\.AI_" Data/Scripts | Sort-Object
rg -n "override bool ReacquireOnMovement|override TimeSpan ReacquireDelay" Data/Scripts | Sort-Object
rg -n "override void OnThink\\(|override void OnMovement\\(|override void OnDamage\\(|override void OnDamagedBySpell\\(|override void OnGotMeleeAttack\\(" Data/Scripts | Sort-Object
```

## Bottom Line

The shard's AI is already a layered, rule-heavy system. The right way to overhaul it is:

- keep legality and shard-rule enforcement centralized
- treat stock shells, runtime assignment, and `ForcedAI` as separate concerns
- roll out tactical improvements with a whitelist
- preserve specialist identity for summons, support mobs, vendors, and scripted encounters

If the overhaul is approached as "replace the AI," it will likely break shard rules and bespoke content.

If it is approached as "add a tactical layer to the stock shells, then audit exceptions deliberately," it has a much better chance of improving gameplay without collateral damage.
