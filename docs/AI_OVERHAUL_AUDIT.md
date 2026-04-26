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

The shard does not have one AI system. It has a layered behavior stack:

1. `BaseCreature` owns AI assignment, targeting legality, control state, bard state, perception/fight ranges, home anchors, movement affordances, and reacquire timing.
2. `BaseAI` and the classes in `Data/Scripts/Mobiles/Base/Behavior.cs` provide the stock state-machine shells.
3. `Behavior.cs` movement helpers and timer logic define much of how AI feels in practice, including movement cadence, pathing fallback, home wandering, and activation/deactivation.
4. Engine classes such as `Sector`, `Region`, and spawning code directly affect when AI runs and what targets are legal.
5. Many mobiles assign `AIType` and `FightMode` in constructors, but some scripts switch `AI` later and some bypass stock assignment entirely with `ForcedAI`.
6. Many encounters layer bespoke logic on top with `OnThink`, `OnMovement`, `OnDamage`, `OnDamagedBySpell`, `OnGotMeleeAttack`, and similar hooks.

The most important practical conclusions are:

- `AcquireFocusMob` is the legal and mechanical choke point for target selection.
- `ChangeAIType` is the central assignment seam for stock shells, but it does not tell the whole truth for every creature.
- `OnThink` and the combat event hooks are the main places where bespoke encounter behavior piggybacks on the AI timer.
- `PlayerRangeSensitive`, sector activation, and deactivation-return rules mean "AI cadence" is partly a world-activation system, not just a combat system.
- `ForcedAI`, runtime `AI = ...` changes, summon-specific target ranking, civilian/vendor subclasses, and script-driven encounter logic mean a "global AI change" will not affect everything equally.

Any large overhaul has to treat AI as an ecosystem, not as a single class replacement.

## Methodology And Confidence

This audit deliberately separates verified deep-read findings from search-derived inventories.

### Direct-read review

These files were read directly because they define the framework or high-risk behavior seams:

- `Data/Scripts/Mobiles/Base/BaseCreature.cs`
- `Data/Scripts/Mobiles/Base/Behavior.cs`
- `Data/Scripts/Mobiles/Base/BaseNPC.cs`
- `Data/Scripts/Mobiles/Base/BasePerson.cs`
- `Data/Scripts/Mobiles/Base/BaseVendor.cs`
- `Data/Scripts/Mobiles/Base/BaseHealer.cs`
- `Data/System/Source/Sector.cs`
- `Data/System/Source/Region.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Core.cs`
- `Data/Scripts/Custom/OmniAI/OmniAI Shared.cs`
- key exception files for clones, summons, pirates, champions, and scripted bosses

Facts in the core-framework, timer, control-order, movement/pathing, legality, and engine-hook sections should be treated as high-confidence findings because they are grounded in direct source review.

### Search-derived inventories

Several appendices use grep-style inventories to size the override surface:

- `ForcedAI`
- `GetFightModeRanking`
- `ReacquireDelay`
- `ReacquireOnMovement`
- `PlayerRangeSensitive`
- `OnThink`
- other AI-adjacent event hooks such as `OnMovement`, `OnDamage`, `OnDamagedBySpell`, and `OnGotMeleeAttack`

Those inventories are useful for locating risk concentrations, but they are not the same thing as a gameplay taxonomy. Some hits are:

- constructors or spawn-time setup, not live combat pivots
- service or social NPC logic, not hostile AI
- item, quest, region, or environment hooks, not mobile brains
- custom systems that borrow AI-adjacent events for non-combat reasons

### Count conventions used in this document

- When a list says `exact`, it means the listed paths were directly confirmed for that symbol or override pattern.
- When a list says `search-derived`, it means the list or count came from code search and may include mixed semantics.
- Unless otherwise noted, refreshed counts in the appendix exclude `Data/Scripts/System/Obsolete` and `Data/Scripts/Custom/XMLSpawner` so the numbers better reflect live behavior rather than archive or plugin noise.

### Confidence boundaries

This document is overhaul-ready, but it is still a static-analysis document. It does not prove live encounter behavior under all shard content combinations. Whenever a section makes a rollout recommendation, that recommendation should be read as "safe based on source architecture" rather than "battle-tested on a running shard."

## Core Framework

### Core files

These are the primary files that define the AI framework:

- `Data/Scripts/Mobiles/Base/BaseCreature.cs`
- `Data/Scripts/Mobiles/Base/Behavior.cs`
- `Data/Scripts/Mobiles/Base/BaseNPC.cs`
- `Data/Scripts/Mobiles/Base/BasePerson.cs`
- `Data/Scripts/Mobiles/Base/BaseVendor.cs`
- `Data/Scripts/Mobiles/Base/BaseHealer.cs`
- `Data/System/Source/Sector.cs`
- `Data/System/Source/Region.cs`

These files are not optional reading for an overhaul. They define the real assignment seams, the real timer loop, the real target-legality path, and the real world-activation behavior.

### FightMode

`FightMode` determines the broad targeting rule before all deeper legality filters and rankings:

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

- `FightMode` is not just "who feels best to attack." It feeds into legality, faction and ethic logic, aggression rules, and the ranking pass inside `AcquireFocusMob`.

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

Important implications:

- The enum advertises more behaviors than the live `ChangeAIType` mapping actually instantiates.
- `AI_Use_Default` is not its own shell. It is an alias value that collapses back to `DefaultAI` in the `AI` property setter.
- `AI_Predator` is especially dangerous to assume is live, because `ChangeAIType` currently routes `AI_Predator` to `MeleeAI`, not `PredatorAI`.
- `AI_Citizen` is even trickier: the enum value exists and the `CitizenAI` class exists, but `ChangeAIType` does not currently map `AI_Citizen` to `CitizenAI`. Content that sets `AI_Citizen` is therefore not selecting a live stock shell through the normal mapping path, even though other systems still branch on the enum value semantically.

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

`BaseCreature.ChangeAIType(AIType)` is the central stock mapping point from enum to shell. Any system that writes `AI = ...` eventually passes through this seam unless `ForcedAI` intercepts it first.

Current stock factory mapping:

- `AI_Melee` -> `MeleeAI`
- `AI_Animal` -> `AnimalAI`
- `AI_Berserk` -> `BerserkAI`
- `AI_Archer` -> `ArcherAI`
- `AI_Healer` -> `HealerAI`
- `AI_Vendor` -> `VendorAI`
- `AI_Mage` -> `MageAI`
- `AI_Predator` -> `MeleeAI` in practice, with the `PredatorAI` line commented out
- `AI_Thief` -> `ThiefAI`
- `AI_Citizen` -> no stock mapping in `ChangeAIType`

Important implications:

- `AIType` is part enum, part content marker, and part real factory input
- for `AI_Predator` and `AI_Citizen`, the enum label is not the same thing as the instantiated shell
- even when a mapping exists in `ChangeAIType`, that still does not prove broad live deployment of the corresponding shell

### Enum vs Live Shell State

The live AI system is split across two different state layers:

- owner-side enum state on `BaseCreature`: `CurrentAI` and `DefaultAI`
- the instantiated live shell object exposed through `AIObject`

Observed behavior:

- the constructor sets both `m_CurrentAI` and `m_DefaultAI`, then calls `ChangeAIType(AI)`
- the `AI` property setter updates `m_CurrentAI`
- if the new value is `AI_Use_Default`, the setter collapses it back to `m_DefaultAI`
- `ChangeAIToDefault()` skips the alias and directly re-enters `ChangeAIType(m_DefaultAI)`
- `AIObject` returns the current live `BaseAI` instance, which outside code can inspect or drive directly

Important implications:

- the enum fields are persistent owner state, not the same thing as the live shell instance
- `AI_Use_Default` is a reset instruction, not a separate behavior family
- unmapped enum values such as `AI_Citizen` leave `ChangeAIType` without creating a stock shell, so `AIObject` can remain `null` unless something later reassigns the AI or `ForcedAI` supplies one
- any overhaul that adds new shell-local state has to decide whether that state belongs on the owner, on the shell, or both

### Constructor-time behavior knobs

The `BaseCreature` constructor stores several core AI parameters that define how the creature perceives, fights, and moves:

- `RangePerception`
- `RangeFight`
- `FightMode`
- `ActiveSpeed`
- `PassiveSpeed`
- `CurrentSpeed`
- `DefaultAI`
- `CurrentAI`

Additional notes:

- legacy `RangePerception` values of `10` are normalized to `DefaultRangePerception = 16`
- speed values are passed through `SpeedInfo.GetSpeeds(...)` before storage
- the constructor initializes `NextReacquireTime` and immediately calls `ChangeAIType(AI)`

Important implication:

- large parts of "AI behavior" are actually encoded in spawn-time numeric parameters, not only in the shell class

### Behavior-affecting live properties

Several `BaseCreature` properties materially change how the AI executes:

- `CurrentWayPoint`
- `TargetLocation`
- `ConstantFocus`
- `Home`
- `RangeHome`
- `RangePerception`
- `RangeFight`
- `ActiveSpeed`
- `PassiveSpeed`
- `CurrentSpeed`
- `ControlTarget`
- `ControlOrder`
- `Team`
- `Combatant`
- `FightMode`
- `Controlled`
- `Summoned`
- `BardProvoked`
- `BardPacified`
- `CanOpenDoors`
- `CanMoveOverObstacles`
- `CanDestroyObstacles`
- `DisallowAllMoves`

Important implications:

- `TargetLocation` hard-forces `CurrentSpeed` to `0.3` while it is set
- changing `CurrentSpeed` calls `m_AI.OnCurrentSpeedChanged()` and resets the AI timer interval
- `ConstantFocus` bypasses normal target acquisition
- door and obstacle affordances are part of the movement brain, not map-only metadata

### Persistence And Rehydration

`BaseCreature.Deserialize(...)` is an AI lifecycle path, not just a save-data concern. The load path restores or reconstructs all of the following before re-entering the stock AI factory:

- `CurrentAI` and `DefaultAI`
- `RangePerception`, `RangeFight`, `Home`, `RangeHome`, and `Team`
- `FightMode`
- `ActiveSpeed`, `PassiveSpeed`, and `CurrentSpeed`, including post-load normalization through `SpeedInfo.GetSpeeds(...)`
- `Controlled`, `ControlMaster`, `ControlTarget`, `ControlDest`, and `ControlOrder`
- `Tamable`, `Summoned`, `SummonEnd`, and the unsummon timer restart
- `CurrentWayPoint`
- loyalty, owners, bonded-pet state, remove-if-untamed state, and delete timers

The load path then:

- restarts delete and unsummon timers when needed
- runs `CheckStatTimers()`
- calls `ChangeAIType(m_CurrentAI)`

Direct-read inference about reconstructed shell state:

- `BaseCreature.Serialize(...)` persists `m_CurrentAI` and `m_DefaultAI`, but not the live `BaseAI` object itself
- `ChangeAIType(m_CurrentAI)` therefore recreates the shell on load
- the `BaseAI` constructor allocates a new timer and resets `Action = ActionType.Wander`
- shell-local timing fields such as `NextMove` and searching cadence live on `BaseAI`, so they are rebuilt from startup behavior rather than individually restored through `BaseCreature` serialization

Important implications:

- an AI overhaul has to be save/load safe and restart-safe, not only combat-safe
- `CurrentAI` is not just a spawn-time choice; it is persisted state that gets re-instantiated on deserialize
- transient shell state is reconstructed, not replayed exactly, unless the owner explicitly persists enough data to rebuild it
- any new AI assignment surface, tactical profile, or role layer needs an explicit serialization story if it is meant to survive a server restart

### ForcedAI

`ForcedAI` short-circuits `ChangeAIType`. If a mobile overrides `ForcedAI`, the normal enum-to-shell mapping stops mattering for that actor.

This is one of the biggest reasons a naive overhaul can miss important content.

### Target ranking

`GetFightModeRanking` is the stock score hook used by `AcquireFocusMob` after legality checks. The default behavior is:

- `Strongest`: `Tactics + Str`
- `Weakest`: `-Hits`
- default: `-distance`

Important implication:

- this is the safest central seam for richer target scoring, but only after all legality filters have already accepted the candidate

### Team-aware helper surface

`BaseCreature.GetTeamSize(int range)` already counts visible same-team nearby `BaseCreature` allies.

Important implication:

- the codebase already contains a lightweight squad-context helper, so future tactical profiles do not need a brand-new group-awareness system just to ask "how many allies are near me?"

### Reacquire timing

The central reacquire controls are:

- `NextReacquireTime`
- `ReacquireDelay`, with a default of 10 seconds
- `ReacquireOnMovement`, with a default of `false`
- `ForceReacquire()`, which simply resets `NextReacquireTime` to `DateTime.MinValue`

Important implication:

- the engine already prefers "reacquire occasionally or when something meaningful happens" over maintaining a persistent threat table

## Activation, Timer, And Execution Flow

The stock timer loop lives in `Behavior.cs` and is more than a simple combat tick.

### BaseAI construction and startup activation

`BaseAI` constructs an `AITimer`, but it does not always start it immediately.

Startup activation rules:

- if `PlayerRangeSensitive` is `false`, the timer starts immediately
- if the world is loading, `PlayerRangeSensitive` actors start inactive
- if the mobile is on `Map.Internal`, has no map, or is in an inactive sector, `PlayerRangeSensitive` actors start inactive
- otherwise, the timer starts normally

Important implication:

- for many actors, "does this AI exist?" and "is this AI currently running?" are different questions

### PlayerRangeSensitive delayed deactivation

While the timer is running, `AITimer.OnTick()` rechecks `PlayerRangeSensitive` actors:

- if their sector is inactive, they stay alive until `DeactivationDelay` expires
- once the delay expires, `BaseAI.Deactivate()` stops the timer
- if the creature came from a `SpawnEntry` with `ReturnOnDeactivate`, deactivation may also send it back home

Important implication:

- activation and home-return behavior are partly spawn-system behavior, not just combat behavior

### Tick order

The timer loop runs in this order:

1. stop if deleted or on `Map.Internal`
2. handle player-range-sensitive activation and delayed deactivation
3. call `m_Mobile.OnThink()`
4. handle bard pacify and bard provoke
5. call `Think()` for uncontrolled creatures, `Obey()` for controlled ones
6. run a separate `Searching()` pass if the creature has the `Searching` skill and its independent cooldown has expired

Important implications:

- encounter scripts often rely on `OnThink()` happening before stock combat logic
- controlled pets do not run the same behavior path as wild mobs
- bard effects get their own branch before normal combat logic
- "AI speed" and "script cadence" are partly the same thing
- the `Searching` pass is its own cadence, not just part of combat state transitions

### Speed changes rewire timer cadence

`OnCurrentSpeedChanged()` stops the timer, randomizes a new delay, and restarts the interval based on the new `CurrentSpeed`.

Important implication:

- any overhaul that changes when `CurrentSpeed` flips between active and passive values is also changing timer cadence

## Controlled Pet Order Pipeline

Controlled creatures use `Obey()`, not `Think()`. This is a separate behavior pipeline and should be treated as a first-class AI system.

### Order dispatch table

`Obey()` directly dispatches to order-specific handlers for:

- `None`
- `Come`
- `Drop`
- `Friend`
- `Unfriend`
- `Guard`
- `Attack`
- `Patrol`
- `Release`
- `Stay`
- `Stop`
- `Follow`
- `Transfer`

Important implication:

- a "generic tactical layer" that assumes all combatants choose targets the same way will break pets unless it respects the order pipeline

### OnCurrentOrderChanged side effects

Changing a pet order is not just intent metadata. It immediately mutates behavior state.

Examples:

- `None` and `Stop` reset `Home` to the current location, set passive speed, clear `Combatant`, and drop out of warmode
- `Come`, `Patrol`, and `Follow` switch to active speed but clear `Combatant`
- `Guard` switches to active speed, enters warmode, and tells the master the pet is guarding
- `Attack` switches to active speed, enters warmode, and clears the old `Combatant` so the attack order can take over
- `Release` and `Transfer` clear combat and move the creature back toward passive state

Important implication:

- control orders already own a large amount of movement state, pacing, and combat intent; they are not optional overlays on the stock wild-AI model

### ControlTarget short-circuit

When a creature is controlled, `AcquireFocusMob` first checks `ControlTarget`. If the control target is valid, alive, visible, and in range, it becomes `FocusMob` immediately.

Important implication:

- pet target selection is command-first, not score-first

## Movement, Pathing, And Spatial Constraints

Much of the shard's perceived AI quality is actually movement behavior.

### Action-state side effects

`OnActionChanged()` does more than change labels:

- `Wander` clears `Combatant`, clears `FocusMob`, disables warmode, and sets passive speed
- `Combat` enables warmode, clears `FocusMob`, and sets active speed
- `Guard` enables warmode, clears both `Combatant` and `FocusMob`, sets active speed, and starts a guard timeout
- `Flee` enables warmode, clears `FocusMob`, and sets active speed
- `Interact` and `Backoff` use passive speed and disable warmode

Important implication:

- changing action-transition rules will also change movement speed and warmode behavior unless handled very carefully

### Core move stack

The shared movement stack includes:

- `CheckMove()`
- `DoMove(...)`
- `DoMoveImpl(...)`
- `MoveTo(...)`
- `WalkMobileRange(...)`
- `WalkRandomInHome(...)`
- `PathFollower`

This is the main shared infrastructure for chase, spacing, flee, and wandering.

### Movement blockers and affordances

`DoMoveImpl(...)` refuses movement when the creature is:

- deleted
- frozen
- paralyzed
- casting a spell
- under `DisallowAllMoves`
- still within the current move-delay window

When blocked, the same function can also:

- open unlocked or unlock-free doors if `CanOpenDoors`
- ignore movable impassables if `CanMoveOverObstacles`
- destroy movable impassables if `CanDestroyObstacles`
- auto-turn and retry when a straight move fails

Important implication:

- movement capability is a meaningful AI differentiator already; it is not just map traversal plumbing

### Home, spawner regions, and roaming boundaries

`WalkRandomInHome(...)` uses `Home` and `RangeHome` to keep creatures within their roaming boundary.

Special cases:

- if `Home == Point3D.Zero` and the creature came from a `SpawnEntry`, the code can temporarily constrain wandering to the spawner region
- if the current region does not accept spawns from the spawner region, the creature may bias toward `region.GoLocation`
- if the creature is outside `RangeHome`, the logic steers it back toward `Home`

Important implication:

- spawn regions and home anchors are already part of the AI movement contract; changing wander behavior without respecting them can break ecology, encounter spacing, and spawn containment

### Pathing and teleport recovery

`MoveTo(...)` tries direct movement first. If direct movement fails while trying to close distance, it creates a `PathFollower` and falls back to pathing.

`OnTeleported()` forces repathing if a path already exists.

Important implication:

- a lot of stock chase logic is "simple move first, pathing second." Any replacement system should match that bias unless there is a deliberate performance reason not to.

### Range control helper

`WalkMobileRange(...)` is the main shared helper for spacing around another mobile:

- used for melee closing
- used for flee/backoff spacing
- used for archer and caster envelopes
- used by controlled followers to maintain distance from masters

Important implication:

- many shell differences are envelope-management differences, not entirely separate brains

### TargetLocation and waypoint-driven exceptions

Two special movement surfaces deserve extra care:

- `CurrentWayPoint` affects how wander logic traverses scripted waypoint chains
- `TargetLocation` forces a `CurrentSpeed` of `0.3`, so it silently overrides normal active/passive movement pacing

Important implication:

- escorts, guided actors, or scripted movers can "feel strange" if an overhaul assumes `CurrentSpeed` always reflects the shell's own combat state

## AcquireFocusMob: The Real Targeting Choke Point

`AcquireFocusMob` is the single most important function in the stock targeting stack.

It does much more than "pick a target."

### What it handles before normal ranking

Before the candidate loop even begins, `AcquireFocusMob` already handles:

- bard-provoked fixed targeting
- controlled-pet `ControlTarget`
- `ConstantFocus`
- `FightMode.None`
- `FightMode.Aggressor` early exit when there is no aggression, faction, or ethic context
- reacquire timing gates through `NextReacquireTime`

Important implication:

- several gameplay systems bypass ranking entirely and never enter the "best target" comparison loop

### Reacquire timing gate

If `NextReacquireTime` has not expired, `AcquireFocusMob` clears `FocusMob` and returns `false`. When the function does proceed, it immediately advances `NextReacquireTime` by `ReacquireDelay`.

Important implication:

- the reacquire cadence is intentionally sparse unless a hook calls `ForceReacquire()`

### Boat-freeze workaround

The function does not iterate the pooled enumerable directly. It first copies nearby mobiles into a `List<Mobile>` and only then scores them.

The inline comment documents this as a "random freeze fix while sailing on a boat."

Important implication:

- this is a shard-specific safeguard, not cosmetic code. An optimization pass that "simplifies" the loop back to direct pooled iteration may reintroduce a live bug.

### What it filters during the candidate loop

The candidate loop includes gameplay-sensitive checks such as:

- deleted, blessed, self, dead, staff, and visibility checks
- player-only constraints
- line of sight
- summon restrictions
- faction and ethic hostility
- honor restrictions
- harmful-action legality
- aggression-mode semantics
- karma, criminal, kill-count, and evil-good filters

Only after those filters pass does the function call `GetFightModeRanking(...)`.

### Why this matters

If you replace or bypass this logic with a custom threat table, you risk:

- attacking illegal or invalid targets
- breaking bard mechanics
- breaking pet control rules
- ignoring faction and ethic hostility
- creating desync between the AI's "best target" and the server's legal target rules

This is the reason central targeting improvements should be layered after legality, not instead of it.

## Stock AI Shells

These summaries separate shell behavior in source from deployment reality. Prevalence notes in this section are refreshed search-derived signals, not exact live-NPC totals, and they exclude `Data/Scripts/System/Obsolete` plus `Data/Scripts/Custom/XMLSpawner`.

### Deployment-aware shell matrix

| AIType / shell | Factory mapping in `ChangeAIType` | Search-derived deployment signal | Practical note |
| --- | --- | --- | --- |
| `AI_Melee` / `MeleeAI` | direct | constructor-heavy, common runtime setter | dominant general-purpose hostile shell and also a service-class fallback |
| `AI_Mage` / `MageAI` | direct | constructor-heavy, common runtime setter | live hostile caster shell plus non-hostile reuse |
| `AI_Animal` / `AnimalAI` | direct | regular constructor use, no refreshed runtime setter hits | simple creature shell |
| `AI_Archer` / `ArcherAI` | direct | sparse constructor use, meaningful runtime setter use | often appears as a switchable role rather than a permanent species identity |
| `AI_Healer` / `HealerAI` | direct | no refreshed non-obsolete constructor or runtime assignments confirmed | available shell and semantic tag, but not a broadly confirmed live assignment path |
| `AI_Vendor` / `VendorAI` | direct | no refreshed non-obsolete constructor or runtime assignments confirmed | available shell and semantic tag, but `BaseVendor` does not start here |
| `AI_Berserk` / `BerserkAI` | direct | no refreshed non-obsolete constructor or runtime assignments confirmed | available in source, not currently confirmed as a live assignment lane |
| `AI_Thief` / `ThiefAI` | direct | no refreshed non-obsolete constructor or runtime assignments confirmed | specialist shell present in the factory, but current deployment looks sparse or absent |
| `AI_Citizen` / `CitizenAI` | no stock factory mapping | one refreshed constructor hit, multiple runtime setter hits | inconsistent state or content marker rather than a reliable shell assignment |
| `AI_Predator` / `PredatorAI` | `AI_Predator` instantiates `MeleeAI` | no refreshed non-obsolete deployment confirmed | legacy label; the source class exists, but the stock factory does not instantiate it |

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
- contains a `SmartAI` branch
- that branch currently defaults to `m_Mobile is BaseVendor`
- is also reused by non-invulnerable `BaseHealer` instances through runtime `AI = AIType.AI_Mage`

Implication:

- the code already contains richer mage behavior than most hostile mage mobs are currently using
- expanding mage intelligence can likely reuse existing code rather than requiring a brand-new mage brain from scratch

### MeleeAI

Current role:

- standard melee shell for the majority of hostile mobs
- handles reacquisition, chase logic, and target swaps when current target is far or movement is blocked
- is also the constructor-time shell used by `BaseVendor`, which means not every `AI_Melee` actor is meant to be difficulty content

Implication:

- if you want a shard-wide tactical upgrade, `MeleeAI` plus `AcquireFocusMob` is the largest value seam

### HealerAI

Current role in source:

- ally-support shell, not normal hostile target-selection logic
- scans same-team visible `BaseCreature` allies
- cures poison first, then greater-heal threshold, then lesser-heal threshold
- when nobody needs help, drifts toward the weakest hostile target

Deployment reality:

- refreshed non-obsolete searches did not find constructor-time `: base(AIType.AI_Healer, ...)` or runtime `AI = AIType.AI_Healer` assignments
- `BaseHealer` does not instantiate `HealerAI` by default; vulnerable healers switch to `AI_Mage` instead
- import and encounter tooling still treat `AI_Healer` as a meaningful semantic label

Implication:

- healer behavior is already role-based and team-aware
- `HealerAI` should currently be treated as an available role asset and semantic marker, not as a broadly confirmed live shell
- forcing a generic hostile scoring model onto healer logic would be a regression if this shell is reintroduced or expanded later

### VendorAI

Current role in source:

- service-NPC behavior
- reacts to `FocusMob` interaction
- flees when attacked

Deployment reality:

- refreshed non-obsolete searches did not find constructor-time `: base(AIType.AI_Vendor, ...)` or runtime `AI = AIType.AI_Vendor` assignments
- `BaseVendor` starts as `AI_Melee` and layers most vendor behavior through class-specific logic and `OnThink()`
- import and encounter tooling still check `AI_Vendor` semantically

Implication:

- vendor behavior is not a reliable `AI_Vendor` deployment story; it is mostly service-class behavior riding the shared AI/timer framework
- vendor brains are not combat difficulty content and should be excluded from combat-overhaul assumptions

### BerserkAI

Current role in source:

- aggressive minimal brain
- closes on the nearest valid target and stays committed

Deployment reality:

- refreshed non-obsolete searches did not confirm live constructor-time or runtime `AI_Berserk` assignments

Implication:

- a good candidate for "low-cost simple aggressor" if you want some mobs to stay dumb on purpose
- treat it as available-but-undeployed until live usage is individually verified

### ThiefAI

Current role in source:

- specialist disruptive AI, not a generic skirmisher
- closes on its target like a melee shell, but then looks for a weapon to disarm and steal
- if the target is not currently wielding something useful, it searches the backpack for bandages, regs, spellbooks, runebooks, potions, scrolls, magic staffs, and gold
- if nothing worth stealing is found, or if conditions turn against it, it can pivot into flee behavior

Deployment reality:

- refreshed non-obsolete searches did not confirm live constructor-time or runtime `AI_Thief` assignments

Implication:

- thief mobs are identity-driven utility enemies
- flattening them into a generic tactical scorer would remove disarm-and-loot behavior that is currently their core combat gimmick
- if this shell is brought back into active use, it should be treated as a specialist exception, not normal melee flavor

### CitizenAI

Current role in source:

- `CitizenAI` exists as a class
- its action methods return `false`

Current role in practice:

- `AI_Citizen` is not currently mapped by `ChangeAIType`
- content that sets `AI = AIType.AI_Citizen` is therefore not receiving `new CitizenAI(this)` through the normal stock assignment path
- refreshed non-obsolete searches found one constructor-time `AI_Citizen` assignment and multiple runtime setters, which reinforces that the enum is still used even though the shell is not factory-backed
- `BaseCreature.OnBeforeDeath()` contains citizen-specific death handling behind `if (AI == AIType.AI_Citizen)`
- `BaseRegion` blocks harmful action for `AI_Citizen` actors when `FightMode == None`

Implication:

- `AI_Citizen` should be treated as a semantic gameplay flag with no stock shell mapping, not as a reliably instantiated shell
- any overhaul or cleanup pass should explicitly decide whether to wire `CitizenAI` back in or continue treating `AI_Citizen` as a rules-bearing enum state that may have no `AIObject`

### PredatorAI

Current role in source:

- an avoidance-first roaming/combat pattern that backs off from perceived danger until hurt or engaged

Current role in practice:

- `AI_Predator` currently instantiates `MeleeAI`, not `PredatorAI`
- refreshed non-obsolete searches did not confirm live constructor-time or runtime `AI_Predator` assignments

Implication:

- `PredatorAI` is a dormant or legacy behavior asset, not a live stock assignment path
- the enum value should be treated as legacy labeling unless and until the factory mapping is intentionally restored

## OmniAI

`OmniAI` is not the stock system, but it matters because it demonstrates what the shard already considers desirable "player-like" behavior.

### What OmniAI can do

Observed capabilities include:

- hazard awareness
- self-healing and recovery behavior
- action selection across trained skills
- mobility and spacing tools
- weapon and style changes
- role-specific utility usage

### Why OmniAI matters

Practical implication:

- it is a design reference for readable tactical behavior
- it should not be assumed to be a drop-in replacement for stock NPCs
- its value for a broad overhaul is architectural inspiration, not blanket deployment

### Practical reading

If the goal is "make regular mobs act more like players without turning them into stat sticks," OmniAI is best treated as a library of desirable combat instincts:

- preserve legality-first target selection from the stock framework
- borrow readable tactical choices and spacing patterns
- avoid granting every stock NPC the full player-kit decision tree

## How AI Is Assigned Today

AI assignment is spread across several different mechanisms.

### 1. Constructor-time shell selection

The most common pattern is still:

- pick an `AIType`
- pick a `FightMode`
- define ranges and speeds in the constructor

This is the baseline hostile-mobile assignment path.

### 2. Runtime reassignment

A second layer changes `AI` later by writing the `AI` property, which re-enters `ChangeAIType(...)`.

This surface is mixed:

- some uses are true runtime switches
- some are spawn-time or role-randomization assignments
- some are temporary or scripted transformations

Important implication:

- counting `AI = AIType...` lines is useful for risk discovery, but those counts should not be read as "number of live combat shapeshifters"

### 3. ForcedAI bypass

`ForcedAI` is a separate assignment lane. Anything using it is outside the normal enum factory.

### 4. Deserialize-time rehydration

`BaseCreature.Deserialize(...)` restores `m_CurrentAI`, `m_DefaultAI`, control state, movement parameters, summon timers, waypoint state, and adjacent lifecycle fields, then calls `ChangeAIType(m_CurrentAI)`.

Important implication:

- saved AI state is part of the live assignment story
- an overhaul that only works for freshly spawned mobs but not for deserialized ones is incomplete

### 5. Service-class emulation and semantic AIType usage

Some actor families are not assigned through a special-purpose shell even though the enum or source tree suggests they might be:

- `BaseVendor` starts `AI_Melee`, not `AI_Vendor`
- non-invulnerable `BaseHealer` switches to `AI_Mage`, not `AI_Healer`
- `AI_Predator` currently resolves to `MeleeAI`

Separately, some code still treats dormant enum values semantically:

- `RandomEncounters.Import` treats `AI_Healer` and `AI_Vendor` as non-hostile
- `EncounterEngine` checks `AI_Vendor` before force-assigning `Combatant`

Important implication:

- the enum is used as a blend of factory input, legacy label, and semantic metadata
- a safe overhaul has to separate "what shell gets instantiated" from "what downstream systems infer from the enum"

## Assignment Patterns By Mobile Family

At a high level, the shard currently uses a few broad assignment patterns:

- standard hostile creatures usually pick one stock shell in the constructor and stay there
- some humanoids randomize between melee, archer, and mage shells during construction
- some special actors begin in a social or citizen-like state and later switch shells
- some summons and clone-like actors bypass stock assignment entirely
- some encounters layer custom behavior mostly through hooks rather than shell replacement
- some service and social classes emulate their role through `AI_Melee` or `AI_Mage` plus class-specific hooks rather than through a bespoke enum-backed shell

Important implication:

- an overhaul needs to classify content by assignment pattern, not just by species or folder

## AIType Deployment Prevalence

These counts are refreshed search-derived prevalence signals, not exact live gameplay totals. They are useful for prioritization because they distinguish dominant assignment lanes from dormant or semantic-only ones.

### Constructor-time prevalence

- `AI_Melee`: 460 non-obsolete constructor hits
- `AI_Mage`: 324 non-obsolete constructor hits
- `AI_Animal`: 90 non-obsolete constructor hits
- `AI_Archer`: 14 non-obsolete constructor hits
- `AI_Citizen`: 1 non-obsolete constructor hit
- `AI_Healer`, `AI_Vendor`, `AI_Berserk`, `AI_Thief`, and `AI_Predator`: no refreshed non-obsolete constructor hits confirmed

### Runtime setter prevalence

- `AI_Melee`: 48 refreshed non-obsolete `AI = AIType.AI_Melee` hits
- `AI_Mage`: 29 refreshed non-obsolete `AI = AIType.AI_Mage` hits
- `AI_Archer`: 18 refreshed non-obsolete `AI = AIType.AI_Archer` hits
- `AI_Citizen`: 11 refreshed non-obsolete `AI = AIType.AI_Citizen` hits
- `AI_Animal`, `AI_Healer`, `AI_Vendor`, `AI_Berserk`, `AI_Thief`, and `AI_Predator`: no refreshed non-obsolete runtime setter hits confirmed

Important implication:

- the first overhaul wave should optimize for `MeleeAI`, `MageAI`, `AnimalAI`, and `ArcherAI`, because those are the clearly dominant deployment paths
- the remaining AIType values should be treated as exceptions, dormant assets, or semantic labels until individually re-verified

## Civilian, Service, And Social NPCs

These actors are high-risk to globalize because their use of AI is not mainly about combat.

### BaseNPC

Current role:

- provides basic civilian or service-style behavior
- overrides `ReacquireOnMovement`
- frequently participates in systems like speech, teaching, or quest interaction

Implication:

- many `BaseNPC` derivatives should be out of scope for a combat overhaul unless the goal explicitly includes social NPC behavior

### BasePerson

Current role:

- civilian-personality layer on top of the base mobile stack
- also overrides `ReacquireOnMovement`

Implication:

- this hierarchy is closer to town and life simulation behavior than to hostile AI

### BaseVendor

Current role:

- `PlayerRangeSensitive` is explicitly `true`
- `OnThink()` is actively used
- constructor-time assignment is `AI_Melee`, not `AI_Vendor`
- vendor behavior is mostly class-specific service logic, not proof of live `VendorAI` deployment

Implication:

- vendor behavior sits directly on the same activation and timer framework as combat AI, but it exists for service interactions rather than difficulty design
- vendor content should be classified by service-class behavior and activation hooks, not by assuming the `AI_Vendor` shell is active

### BaseHealer

Current role:

- vulnerable instances explicitly switch to `AI_Mage` with `FightMode.Aggressor`
- uses mage-style support logic for service and resurrection behavior rather than a broadly confirmed live `HealerAI` shell

Implication:

- healer NPCs are one of the clearest examples of AI code being reused for non-hostile gameplay

## Dynamic AI Switchers

These are the most important reviewed examples of content that does not keep one shell forever.

### Adventurers

Observed pattern:

- start by setting `AI_Citizen`
- then branch to `AI_Mage`, `AI_Melee`, or `AI_Archer` based on generated role

Implication:

- this class mixes social-NPC setup with combat-role specialization, which makes it a useful model for staged-role assignment but a risky target for blanket assumptions about `AI_Citizen`

### Syth and Jedi

Observed pattern:

- override combat event hooks
- swap between mage and melee AI states in response to encounter conditions

Implication:

- these are true runtime switchers and should be treated as bespoke combat actors

### RuneGuardian

Observed pattern:

- participates in runtime AI reassignment and movement-sensitive combat behavior

Implication:

- it belongs in the special-case register, not the generic stock-mob bucket

### Generated crews and transformed actors

Observed pattern:

- ship crews, animated spell creatures, and some summon or scripted systems assign AI after construction as part of spawn or transformation setup

Implication:

- "runtime AI assignment" spans several meanings and should be grouped before any refactor tries to normalize it

## Action Callback Extension Surface

This is a small but important extension seam that does not require a custom `BaseAI` subclass.

### What the seam is

`BaseCreature` exposes these virtual callbacks:

- `OnActionWander()`
- `OnActionCombat()`
- `OnActionGuard()`
- `OnActionFlee()`
- `OnActionInteract()`
- `OnActionBackoff()`

`Behavior.cs` invokes the matching callback immediately before dispatching the corresponding `DoAction*()` branch.

Important implication:

- content can inject behavior on action-state transitions without replacing the stock shell
- an overhaul that bypasses or rewires the normal action dispatcher can break bespoke combat logic that does not advertise itself as a custom AI class

### Current reviewed deployment

Refreshed non-obsolete searches found only two live overrides of this surface:

- `SandVortex.OnActionCombat()`
- `OrcBomber.OnActionCombat()`

In both cases, the mobile keeps its stock shell but adds combat-only special behavior through the callback seam.

## External AIObject Control Surface

`AIObject` is a public live-shell integration point, not only an internal convenience getter.

### What outside code does with it

Refreshed non-obsolete searches show several kinds of direct shell control:

- release-oriented systems call `AIObject.DoOrderRelease()` to force pet or construct cleanup
- combat scripts force `AIObject.Action = ActionType.Combat`
- movement choreography code pushes `AIObject.NextMove`

Representative examples:

- `Revenant` forces combat state through `AIObject.Action = ActionType.Combat`
- `SavageShaman` delays movement animations by writing `AIObject.NextMove`
- quest or familiar cleanup paths such as `GolemFighter` and `Familiar` call `AIObject.DoOrderRelease()`

Important implications:

- outside code already depends on the mutable public shell API, not just on `BaseCreature` properties
- if a future overhaul hides, renames, or virtualizes shell internals, the `AIObject` consumers become a compatibility surface that must be audited deliberately
- this also reinforces why unmapped enum states matter: `AIObject` can be `null`, and several consumers already guard for that explicitly

## Engine Hooks And World Activation

AI execution is coupled to the world engine, not just to the mobile hierarchy.

### PlayerRangeSensitive

By default, `BaseCreature.PlayerRangeSensitive` returns `CurrentWayPoint == null`.

Important implications:

- creatures following waypoints stay active even when players are not nearby
- ordinary wandering creatures are generally eligible for sector-based activation and deactivation

### OnSectorActivate and location reactivation

`BaseCreature.OnSectorActivate()` calls `m_AI.Activate()` when the creature is `PlayerRangeSensitive`.

`BaseCreature.OnLocationChange(...)` also reactivates AI when the creature enters an active sector and already has an AI instance.

Important implication:

- AI activation is not a one-time spawn decision; it is refreshed by world movement and sector activity

### Sector player activation

`Sector.OnEnter(...)` and `Sector.OnLeave(...)` activate and deactivate sectors when players enter or leave them.

Important implication:

- the presence of players is literally part of whether many AI timers run

### Vendor override

`BaseVendor` explicitly overrides `PlayerRangeSensitive` to `true`.

Important implication:

- service NPCs are intentionally wired into the same player-proximity activation model as ordinary mobs

### Region combat and legality hooks

The region system contributes directly to AI legality and combat state:

- `Region.OnCombatantChange(...)`
- `Region.AllowHarmful(...)`
- `Region.OnCriminalAction(...)`

Important implication:

- region rules are not downstream consequences of target selection; they are part of the target-selection and combat-state pipeline itself

## Shard-Specific Behavioral Quirks

These are easy-to-miss live rules that an overhaul could erase by accident.

### Bard unpacify on damage

`BaseCreature.OnDamage(...)` can break bard pacification probabilistically based on missing health.

Implication:

- bard control is not only managed in the timer branch; damage events can also alter it

### Ocean-monster leap behavior

The same `OnDamage(...)` path contains special behavior for some ocean monsters:

- when hurt by a player on shore under the right conditions, the creature can set `Home` to its current spot and leap to the attacker

Implication:

- this is combat-adjacent movement behavior living outside the stock shell methods

### AcquireFocusMob boat-freeze workaround

The target-acquisition loop snapshots nearby mobiles into a list before scoring them.

Implication:

- this is documented in source as a freeze workaround while sailing and should be preserved unless replaced by something proven safe

## ForcedAI Inventory

This list is exact for non-obsolete gameplay code reviewed through search.

### Live `ForcedAI` overrides

- `Data/Scripts/Custom/CloneOfflinePlayerCharacters/CharacterClone.cs`
- `Data/Scripts/Custom/OmniAI/AITester.cs`
- `Data/Scripts/Magic/Jester/Spells/Clowns.cs`
- `Data/Scripts/Magic/Ninjitsu/MirrorImage.cs`

Practical reading:

- clones, test harnesses, mirror images, and clown summons all bypass normal stock assignment
- these are high-confidence exclusions for any stock-NPC tactical rollout

## Custom Target-Ranking Overrides

This list is exact for non-obsolete gameplay code reviewed through search.

### Live `GetFightModeRanking(...)` overrides

Summon and conjuration identity dominates this list:

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

Practical reading:

- this override surface is not a generic hostile-mob surface
- it is strongly tied to summon identity and magical flavor

## Runtime AI Reassignment Inventory

This section is intentionally mixed and search-derived. The point is to identify categories of reassignment rather than pretend every `AI = AIType...` hit means the same thing.

### Reviewed dynamic switchers

These are the clearest true switchers confirmed by direct review:

- `Data/Scripts/Mobiles/Humanoids/Humans/Adventurers.cs`
- `Data/Scripts/Mobiles/Humanoids/Aliens/Syth.cs`
- `Data/Scripts/Mobiles/Humanoids/Aliens/Jedi.cs`
- `Data/Scripts/Mobiles/Unique/RuneGuardian.cs`

### Search-derived reassignment families

Other `AI = AIType...` hits cluster into several families:

- spell-created or animated creatures such as `AnimateDeadSpell`, `MagicLock`, and alchemical or magical summons
- generated ship and pirate crews
- constructor-time role randomizers for humanoids and townsfolk
- special social or encounter actors that begin in one AI state and are later reassigned

Practical reading:

- runtime AI reassignment is a real surface, but its semantics vary too much to treat it as one uniform mechanism

## Reacquire And Retarget Special Cases

### Default behavior

Default stock behavior is:

- `ReacquireDelay` = 10 seconds
- `ReacquireOnMovement` = `false`
- `ForceReacquire()` clears the time gate

### Known explicit delay overrides

Exact non-obsolete overrides:

- `Data/Scripts/Mobiles/Mystical/DarkWisp.cs`
- `Data/Scripts/Mobiles/Mystical/Wisp.cs`

Practical reading:

- delay overrides are rare
- when they exist, they are deliberate and should be treated as role or encounter tuning

### Movement-based retargeting

`BaseCreature.OnMovement(...)` calls `ForceReacquire()` when:

- `ReacquireOnMovement` is `true`
- or the creature is a paragon

Practical reading:

- movement-driven retargeting already exists, but it is opt-in and broad when enabled

### Aggression-driven retargeting

`BaseCreature.AggressiveAction(...)` stops flee and calls `ForceReacquire()`.

Practical reading:

- the codebase already has an event-driven "wake up and re-evaluate" pattern on meaningful combat events

## AI-Adjacent OnThink Hooks

The current codebase has a large `OnThink()` surface. Some of it is encounter logic, some of it is summon identity, and some of it is civilian behavior.

### Directly reviewed encounter-critical examples

- `Exodus`
- `PlagueBeast`
- `BasePirate` and pirate-captain style ship logic
- `CharacterClone`

These should be treated as encounter or system logic piggybacking on AI cadence.

### Common OnThink families

Search-derived clusters include:

- summoned magical entities
- elemental and spirit conjurations
- base vendors and social NPCs
- citizen and training NPCs
- pirate and crew logic
- clones and champion minions

Practical reading:

- `OnThink` counts size the cadence-sensitive surface
- they do not mean every hit is a bespoke combat boss

## Scripted Encounter Hooks

Many important encounters are not stock AI plus stats. They piggyback on the AI timer and combat events.

### Exodus

Relevant behavior:

- `ReacquireOnMovement` is enabled while uncontrolled
- `OnThink()` reactivates field state when not hurt
- movement is part of the encounter effect surface

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

These refreshed counts exclude `Data/Scripts/System/Obsolete` and `Data/Scripts/Custom/XMLSpawner`.

### Exact or narrowly search-derived counts

- `ForcedAI`: 4 files
- `GetFightModeRanking(...)`: 19 files
- `ReacquireDelay`: 2 files
- `PlayerRangeSensitive`: 5 files
- `ReacquireOnMovement`: 255 files
- `OnThink(...)`: 69 files
- `OnAction*()` overrides: 2 files in refreshed non-obsolete searches, both `OnActionCombat()`
- `AI_Use_Default`: 2 hits
- `ChangeAIToDefault()`: 1 file
- semantic `AI == AIType.AI_Citizen` branches: 3 hits

### Live-shell integration surface

- `AIObject`: 14 refreshed non-obsolete hits, including the public getter, internal base-class uses, and external consumers
- direct `AIObject` member writes or calls are concentrated around `DoOrderRelease()`, `Action`, and `NextMove`

### Broader event-hook counts

These are still useful, but they include mixed systems such as items, quests, and environment logic:

- `OnMovement(...)`: 91 files
- `OnDamage(...)`: 58 files
- `OnDamagedBySpell(...)`: 19 files
- `OnGotMeleeAttack(...)`: 157 files
- `AggressiveAction(...)`: 2 files

### How to read these numbers

- they size the review surface
- they do not mean every hit is a normal hostile AI script
- they do mean encounter logic, summon logic, social NPC logic, pathing behavior, AI assignment, action-state callbacks, and live-shell integrations are spread across the codebase

## Gameplay Systems Tightly Coupled To AI

Any overhaul has to explicitly respect these gameplay systems because they are already wired into the AI path.

### Pet and summon control

AI interacts directly with:

- `ControlOrder`
- `ControlTarget`
- master-follow behavior
- attack-order propagation
- control-state speed and warmode changes

Changing target selection without understanding pet order logic will produce bad pet behavior.

### Bard systems

AI is explicitly branched by:

- bard pacify
- bard provoke
- unpacify-on-damage behavior

These are not optional buffs layered on top. They are part of the AI flow itself.

### Faction, ethic, honor, and aggression legality

Target selection is already aware of:

- faction relationships
- ethic hostility
- honor rules
- harmful-action legality
- evil-good and criminal filters
- aggression semantics

Any overhaul that ignores these rules will create gameplay regressions, not just tactical differences.

### Region and service behavior

Town actors, vendors, healers, and some quest actors use AI-adjacent hooks for interaction, resurrection, healing, teaching, scripted conversation, and region-controlled combat legality.

### Persistence and restart behavior

Deserialization restores AI assignment, control state, timers, and movement parameters before re-entering `ChangeAIType(m_CurrentAI)`. See `Persistence And Rehydration` above for the owner-state versus reconstructed-shell distinction.

Any overhaul that ignores restart/load behavior can create bugs that only appear after a world save, reboot, or deserialized spawn.

### Spawn and home behavior

AI activation, deactivation, and wandering already depend on:

- sector activity
- `PlayerRangeSensitive`
- waypoint exceptions
- `SpawnEntry.ReturnOnDeactivate`
- `Home`, `RangeHome`, and region acceptance

### Summon identity

Several magical summons intentionally override prey ranking. Their feel is part of spell identity, not just combat power.

## What Is Safe To Centralize

These are the safest high-value central seams for a future overhaul:

- post-legality target scoring layered onto `AcquireFocusMob`
- opt-in tactical profiles on top of stock shells
- role-sensitive reacquire timing
- event-driven `ForceReacquire()` hooks for meaningful combat events
- better use of existing mage and healer role logic
- grouped use of existing movement helpers rather than replacing the entire movement stack at once

## What Is Not Safe To Globalize

These are the most dangerous things to change globally without a whitelist:

- replacing `AcquireFocusMob` legality with an external threat table
- changing bard, control-order, or harmful-action semantics
- flattening summon-specific `GetFightModeRanking` overrides
- assuming constructor AI tells the whole truth for every mobile
- treating `ForcedAI` actors as normal stock mobs
- globally speeding up `OnMovement`-driven reacquire
- changing AI timer cadence without auditing `OnThink` encounter scripts
- rewriting movement rules without respecting `Home`, `RangeHome`, waypoint, and spawn-region behavior
- assuming `AI_Citizen` is a live shell instead of checking the actual mapping

## Recommended Overhaul Workstreams

### Workstream 1: Core tactical layer

- keep legality in `AcquireFocusMob`
- add richer post-legality target weighting
- keep the first rollout stateless and event-driven

### Workstream 2: Whitelist rollout

- start with ordinary uncontrolled hostile mobs using stock shells
- exclude civilians, vendors, healers, `ForcedAI` actors, and summon-ranking overrides

### Workstream 3: Exception register

- maintain a live register of `ForcedAI`, target-ranking overrides, runtime shell switchers, and cadence-sensitive `OnThink` actors

### Workstream 4: Mage and healer pass

- reuse existing `MageAI` and `HealerAI` role logic before inventing new brains
- treat support behavior as a role problem, not just a target-scoring problem

### Workstream 5: Movement and activation audit

- audit anything that changes `CurrentSpeed`, `OnActionChanged`, `PlayerRangeSensitive`, or movement helpers as a separate risk stream from target selection

### Workstream 6: Encounter audit

- explicitly review bosses and systems where `OnThink`, `OnDamage`, or `OnGotMeleeAttack` are part of the encounter mechanic

## Overhaul-Readiness Matrix

Use this matrix to decide what can be centralized and what must stay exception-driven.

| Surface | Current owner | Centralize carefully? | Why |
| --- | --- | --- | --- |
| Target legality | `AcquireFocusMob`, `Region`, faction and ethic code | No | This is rules enforcement, not tactical flavor. |
| Post-legality target preference | `GetFightModeRanking` and shell logic | Yes | This is the best seam for better choices without breaking lawfulness. |
| Control orders | `Obey()` and `OnCurrentOrderChanged()` | No | Pets are command-driven, not generic score-driven. |
| Movement envelope | `WalkMobileRange`, `MoveTo`, shell logic | Yes, but incrementally | Shell identity depends heavily on spacing and chase rules. |
| Home and wander constraints | `Home`, `RangeHome`, `SpawnEntry`, waypoint logic | No | This is ecology and encounter containment behavior. |
| Timer cadence | `CurrentSpeed`, `AITimer`, `OnThink()` hooks | No, except by whitelist | Many systems borrow the same cadence. |
| World activation | `PlayerRangeSensitive`, `Sector`, `OnSectorActivate()` | No | This is a server-activation and performance system. |
| Deserialize-time AI rehydration | `BaseCreature.Deserialize(...)` and `ChangeAIType(m_CurrentAI)` | No | Assignment changes have to survive save/load and restart cycles. |
| Action-state callbacks | `OnAction*()` on `BaseCreature` plus `Behavior.cs` dispatch | No, except by review | Some content piggybacks on stock action transitions instead of custom shells. |
| Summon prey identity | summon-specific ranking overrides | No | This is spell identity. |
| Stock melee, archer, and mage tactics | shell methods plus post-legality scoring | Yes | This is the main opportunity for better combat feel. |

## Minimum Exclusion List For V1

Any first-pass tactical overhaul should exclude at least:

- `ForcedAI` actors
- stock summons and magical constructs with `GetFightModeRanking(...)` overrides
- vendors, healers, town actors, and service NPCs
- citizen-tagged content until `AI_Citizen` behavior is explicitly resolved
- runtime switchers such as `Adventurers`, `Syth`, `Jedi`, and `RuneGuardian`
- encounter actors with important `OnThink()` mechanics
- anything whose behavior depends on custom `OnDamage`, `OnDamagedBySpell`, or `OnGotMeleeAttack` reactions in a boss-style context

## Refresh Commands

These commands are useful for refreshing the inventories after future content changes.

### Exact override surfaces

```powershell
rg -n -F "protected override BaseAI ForcedAI" Data/Scripts
rg -n -F "public override double GetFightModeRanking(" Data/Scripts
rg -n -F "public override TimeSpan ReacquireDelay" Data/Scripts
rg -n -F "public override bool PlayerRangeSensitive" Data/Scripts
rg -n "public override void OnAction(Wander|Combat|Guard|Flee|Interact|Backoff)\\(" Data/Scripts
rg -n -F "AI_Use_Default" Data/Scripts
rg -n -F "ChangeAIToDefault(" Data/Scripts
rg -n -F "AI == AIType.AI_Citizen" Data/Scripts
```

### Large search-derived surfaces

```powershell
rg -n -F "public override bool ReacquireOnMovement" Data/Scripts
rg -n -F "public override void OnThink(" Data/Scripts
rg -n -F "override void OnMovement(" Data/Scripts
rg -n -F "override void OnDamage(" Data/Scripts
rg -n -F "override void OnDamagedBySpell(" Data/Scripts
rg -n -F "override void OnGotMeleeAttack(" Data/Scripts
```

### Assignment and switching surfaces

```powershell
rg -n "AI\\s*=\\s*AIType\\." Data/Scripts
rg -n "ChangeAIType\\(" Data/Scripts
rg -n "CurrentAI\\s*=|DefaultAI\\s*=" Data/Scripts
rg -n -F "AIObject" Data/Scripts
rg -n -F "AIObject." Data/Scripts
rg -n --glob '!Data/Scripts/System/Obsolete/**' --glob '!Data/Scripts/Custom/XMLSpawner/**' ": base\\(AIType\\.AI_" Data/Scripts
rg -n --glob '!Data/Scripts/System/Obsolete/**' --glob '!Data/Scripts/Custom/XMLSpawner/**' "AI\\s*=\\s*AIType\\." Data/Scripts
rg -n "AIType\\.AI_(Vendor|Healer|Citizen|Predator)" Data/Scripts/Mobiles/Base/BaseVendor.cs Data/Scripts/Mobiles/Base/BaseHealer.cs Data/Scripts/Custom/RandomEncounters/Import.cs Data/Scripts/Custom/RandomEncounters/EncounterEngine.cs
rg -n -F "AIType.AI_Citizen" Data/Scripts/Mobiles/Base Data/Scripts/System Data/Scripts/Mobiles/Civilized
```

### Suggested exclusions when refreshing counts

For a "live gameplay" count instead of a raw repository count, exclude at least:

- `Data/Scripts/System/Obsolete`
- `Data/Scripts/Custom/XMLSpawner`

## Bottom Line

The current shard AI is not one replaceable brain. It is a distributed behavior system built from:

- `BaseCreature` state and assignment
- deserialize-time AI rehydration
- `BaseAI` timer and state-machine shells
- movement and pathing helpers
- world-activation rules
- target-legality rules
- pet orders and bard branches
- action-state callbacks
- summon identity overrides
- encounter-specific event hooks

That is not bad news. It means the safest path to a strong overhaul is visible:

- keep legality and control semantics intact
- improve post-legality target choice
- respect movement and cadence as gameplay systems
- whitelist ordinary hostile stock mobs first
- maintain an explicit exception register for everything else

If future work follows those rules, the shard can make ordinary NPCs smarter and more fun without turning the server into a pile of broken edge cases.
