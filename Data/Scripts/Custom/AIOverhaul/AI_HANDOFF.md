# AI Handoff

## Purpose

This file is the living state record for the AI overhaul project. Review it before every phase after phase 1. Update it before stopping any phase. Treat it as both the handoff log and the live exception register.

## Project Pointers

- Audit: `docs/AI_OVERHAUL_AUDIT.md`
- Roadmap: `Data/Scripts/Custom/AIOverhaul/AI_PHASED_OVERHAUL_PLAN.md`
- Local rules: `Data/Scripts/Custom/AIOverhaul/AGENTS.md`

## Current Phase

- Status: `Phase 4 - Movement, Spacing, And Cadence For The Whitelist completed`
- Next execution target: `Phase 5 - Role-Specialist Pass For Mage, Healer, And Squad Behaviors`

## Completed Work

- Created the dedicated AI overhaul workspace.
- Created the phased implementation roadmap.
- Created the local AI overhaul operating rules.
- Seeded the handoff structure and initial exclusion register.
- Refreshed the phase-1 governance artifacts against the current audit.
- Confirmed the existing three-file workspace is sufficient for phase 1 and does not need extra no-op scaffolding.
- Added the default-off tactical targeting helper and rollout resolver surface for phase 2.
- Added guarded `BaseCreature` tactical profile and event-driven reacquire seams without introducing persisted state.
- Added post-legality tactical scoring injection at the stock `AcquireFocusMob` ranking point.
- Confirmed phase 2 keeps all live content on the inert `None` tactical profile until phase 3 explicitly populates the rollout guard.
- Activated the tactical layer for a narrow exact-type whitelist of stock hostile melee and archer mobs.
- Added GM-visible inspection for `TacticalTargetProfile` and `UsesAITacticalTargeting` so live rollout status can be checked with `Props`.
- Confirmed the phase-3 rollout remains resolver-only, default-deny, and reversible from a single file.
- Added whitelist-bound movement cadence and spacing helpers for phase 4 without broadening the tactical rollout guard.
- Applied the phase-4 movement overlay only to whitelisted `Skirmisher` archers while keeping `Bruiser` movement on the stock melee chase path.
- Tightened phase-4 skirmisher chase cadence so whitelisted archers close distance faster when a target drifts beyond their preferred spacing band.

## Files And Seams Touched

- `Data/Scripts/Custom/AIOverhaul/AITacticalTargeting.cs` now owns the exact-type whitelist rollout guard, active phase-3 profile assignments, bounded scoring helper, and whitelist-bound movement cadence, chase, and spacing helpers.
- `Data/Scripts/Mobiles/Base/BaseCreature.cs` now exposes GM-visible tactical profile inspection in addition to the guarded tactical profile accessors and event-driven reacquire hooks.
- `Data/Scripts/Mobiles/Base/Behavior.cs` now layers tactical scoring only after the stock legality and mode filters pass, and phase 4 reuses that same whitelist data to drive archer-only spacing and cadence inside the stock combat branch.
- `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` refreshed to record the active whitelist, exclusions, rollout guard status, and the new phase-4 movement assumptions.

## Phase 1 Completion Notes

- No additional no-op scaffolding files were needed beyond `AI_PHASED_OVERHAUL_PLAN.md`, `AI_HANDOFF.md`, and `AGENTS.md`.
- No live gameplay code or live AI behavior changed in phase 1.

## Phase 2 Completion Notes

- The tactical seam now exists in live code, but phase 2 keeps it inert because `AITacticalTargeting.ResolveProfile(...)` returns `None` for every actor.
- No whitelist families are enabled yet; phase 3 remains the first rollout phase.
- The legality pipeline, bard/control-order semantics, movement, activation, deserialize behavior, and global timer cadence remain unchanged.
- `FightMode.Strongest`, `FightMode.Weakest`, and special legality-driven fight modes receive no tactical bonus in phase 2.

## Phase 3 Completion Notes

- Phase 3 activates the tactical layer only through the exact-type `AITacticalTargeting.ResolveProfile(...)` whitelist.
- The active whitelist is limited to `HeadlessOne`, `Ratman`, `Lizardman`, `RatmanArcher`, and `LizardmanArcher`.
- `Bruiser` is assigned to the whitelisted melee cohort and `Skirmisher` to the whitelisted archer cohort.
- The rollout remains default-deny for every other family, shell, and subclass.
- `Props` can now verify rollout membership through the GM-visible `TacticalTargetProfile` and `UsesAITacticalTargeting` properties.
- No new persisted state, legality changes, cadence changes, or deserialize changes were introduced in phase 3.

## Live Shard Testing Note

- Phase 3 is live on the main shard and public player testing is now running through `Data/Scripts/Custom/AIOverhaul/AI_PHASE3_PLAYER_TESTING.md`.
- Archer spacing or kiting feedback should be triaged as likely phase-4 movement feedback unless the report clearly involves target selection.

## Phase 4 Completion Notes

- Phase 4 keeps the exact-type whitelist and applies helper-driven spacing and cadence only to whitelisted `Skirmisher` archers.
- `Bruiser` movement remains on the stock `MeleeAI.DoActionCombat()` chase path in phase 4.
- Whitelisted skirmishers now use a baseline `0.5` second movement cadence, accelerate to `0.25` seconds while closing from beyond their preferred band, and prefer a `3..6` tile combat band clamped by weapon range.
- The stock `Combat` action dispatcher remains intact; phase 4 does not repurpose `ActionType.Backoff`, rewrite `OnActionChanged()`, or alter warmode or speed transition rules.
- No new persisted state, activation changes, deserialize changes, or global timer cadence changes were introduced in phase 4.
- Cornered or terrain-blocked skirmishers intentionally fall back to stock helper behavior and may keep firing in place rather than using bespoke retreat pathing.

## Unchanged Safety Invariants

- Legality-first targeting stays intact.
- Bard provoke, pacify, and unpacify-on-damage semantics stay intact.
- Pet and summon control-order semantics stay intact.
- Summon prey identity and custom prey-ranking overrides stay intact unless explicitly audited later.
- Save/load and restart-safe AI assignment remains mandatory.
- `AIObject` compatibility remains a required audit surface.
- Action-state callback behavior remains a required audit surface.
- Rollout stays whitelist-first.
- `AcquireFocusMob` legality ordering and final LOS confirmation remain intact.
- `AggressiveAction(...)` and `OnMovement(...)` reacquire behavior remain unchanged.
- No new persisted AI owner-side or shell-side state was introduced in phase 2.
- No shell assignment, movement, activation, cadence, bard, summon, control-order, or deserialize behavior changed in phase 2.
- Phase 4 reuses the stock action dispatcher and shared movement helpers rather than replacing global movement, activation, home, or timer systems.

## Active Phase 4 Whitelist

- `Bruiser`: `HeadlessOne`, `Ratman`, `Lizardman`
- `Skirmisher`: `RatmanArcher`, `LizardmanArcher`
- Rollout preconditions: uncontrolled, unsummoned, live `AIObject`, `FightMode.Closest`, and live enum shell `AI_Melee` or `AI_Archer`
- Rollout policy: exact concrete type equality only; no subclass inheritance, namespace-wide inclusion, or shell-wide inclusion

## Phase 3 Exclusions

- Civilians, town actors, vendors, healers, and service NPCs.
- `ForcedAI` actors.
- Summons and magical constructs with custom target-ranking behavior.
- Citizen-tagged semantic AI usage.
- Runtime AI switchers and transformation-driven shell changes.
- Encounter-critical bosses or systems with cadence-sensitive `OnThink`, `OnDamage`, or `OnGotMeleeAttack` behavior.
- Any family that depends directly on `AIObject` manipulation until separately reviewed.
- Stock mages, including `RatmanMage` and `OrcishMage`, until the later mage/healer role pass.
- `Orc` and `Urc`, which look like simple melee rollout candidates at first glance but can switch to `AI_Archer` inside their constructors.

## Exception Register

| Family Or Surface | Status | Reason | Follow-Up |
| --- | --- | --- | --- |
| `ForcedAI` actors | Excluded | Bypass stock enum-to-shell mapping | Review in phase 6 |
| Summon prey overrides | Excluded | Spell identity and custom targeting | Review in phase 6 |
| Runtime AI switchers | Excluded | Shell changes at runtime complicate rollout assumptions | Review in phase 6 |
| `AIObject` consumers | Excluded from broad rollout assumptions | External code reaches into mutable live-shell API | Review in phase 6 |
| `OnAction*()` piggybackers | Excluded from dispatcher rewrites | Combat logic may live in action callbacks instead of shells | Review in phase 6 |
| Encounter-critical `OnThink` families | Excluded | Timer/cadence sensitivity | Review in phase 6 |
| Vendors, healers, service NPCs | Excluded | Social/service semantics, not generic hostile combat | Keep excluded unless explicitly re-scoped |
| Citizen-tagged semantics | Excluded | Enum semantics do not imply a stock live shell | Keep excluded unless explicitly re-scoped |
| `Orc` and `Urc` | Excluded from phase 3 whitelist | Constructor-time shell switching prevents clean single-shell rollout assumptions | Revisit only if a later phase audits dynamic shell families |
| Stock hostile mages such as `RatmanMage` and `OrcishMage` | Deferred from phase 3 whitelist | Mage behavior is reserved for the later role-specific pass | Review in phase 5 |
| `FightMode.Strongest` and `FightMode.Weakest` tactical weighting | Deferred from phase-2 bonus | Heterogeneous score domains should not be rescaled in the first tactical pass | Revisit after whitelist rollout proves stable |
| `FightMode.Aggressor`, `FightMode.Evil`, `FightMode.CharmMonster`, `FightMode.CharmAnimal` tactical weighting | Deferred from phase-2 bonus | Special legality semantics should remain untouched in the first tactical pass | Revisit after whitelist rollout proves stable |
| `Support` tactical profile | Reserved / inactive | Forward-compatible role token; hostile support targeting is deferred | Review in phase 5 |

## Persistence And Restart Considerations

- `CurrentAI` and `DefaultAI` are owner-side persisted state.
- The live `BaseAI` shell is reconstructed on deserialize.
- Any new tactical profile, role layer, or shell-local state needs an explicit persistence story if it must survive restart.
- Any rollout guard that changes assignment or shell behavior must be checked against deserialize-time rehydration.
- Phase 2 added no serialized fields and made no `Serialize(...)` or `Deserialize(...)` changes.
- Phase 3 keeps the whitelist resolver code-defined and non-persistent; restart/load behavior still depends on the same owner-side enum plus live-shell rehydration path.

## Validation Performed

- Re-reviewed the audit sections on `BaseCreature` responsibilities, target ranking, reacquire timing, `AcquireFocusMob`, gameplay-coupled AI systems, and safe versus unsafe centralization before implementing phase 2.
- Confirmed the implementation scope is limited to `Data/Scripts/Mobiles/Base/BaseCreature.cs`, `Data/Scripts/Mobiles/Base/Behavior.cs`, `Data/Scripts/Custom/AIOverhaul/AITacticalTargeting.cs`, and this handoff file.
- Confirmed `AITacticalTargeting.ResolveProfile(...)` returns `None` for every actor in phase 2, keeping the new seam inert until phase 3 enables a whitelist.
- Re-read `AcquireFocusMob(...)` and confirmed tactical scoring is injected only after the existing early exits, legality filters, and stock ranking call.
- Confirmed the new reacquire seam is limited to `RequestTacticalReacquire(...)` and that `AggressiveAction(...)`, `OnMovement(...)`, `ReacquireDelay`, `ReacquireOnMovement`, and `ForceReacquire()` semantics remain unchanged.
- Confirmed no new serialized fields were added and no `Serialize(...)` or `Deserialize(...)` behavior changed.
- Confirmed `AIObject` consumers and `OnAction*()` callback dispatch are still flowing through the stock live-shell and action-state paths.
- Confirmed `FightMode.Strongest`, `FightMode.Weakest`, and special legality-driven fight modes remain untouched by tactical weighting in phase 2.
- Confirmed the whitelist target remains ordinary uncontrolled hostile stock mobs using standard shells and that no whitelist rollout is enabled yet.
- Re-reviewed the audit sections covering whitelist rollout, minimum exclusions, stock shell deployment reality, civilian/service actors, runtime AI switchers, summon identity, and safe versus unsafe centralization before enabling phase 3.
- Confirmed `AITacticalTargeting.ResolveProfile(...)` now enables only five exact concrete classes and returns `None` for all other types by default.
- Confirmed the phase-3 rollout guard additionally requires uncontrolled, unsummoned, live-shell, `FightMode.Closest`, and live enum shell `AI_Melee` or `AI_Archer`.
- Confirmed `RatmanMage`, `OrcishMage`, `Orc`, `Urc`, vendors, healers, `ForcedAI` actors, citizen-tagged content, and summon prey-override families remain excluded from active rollout.
- Confirmed phase 3 adds no new persisted state and does not modify `AcquireFocusMob(...)` legality ordering, deserialize behavior, or global reacquire cadence.
- Confirmed `BaseCreature.TacticalTargetProfile` and `BaseCreature.UsesAITacticalTargeting` are now GM-visible for live rollout inspection.
- Re-reviewed the audit sections covering movement/pathing, activation/timer flow, action callbacks, spawn/home containment, and movement-related overhaul-readiness guidance before implementing phase 4.
- Confirmed phase 4 only touches `Data/Scripts/Custom/AIOverhaul/AITacticalTargeting.cs`, `Data/Scripts/Mobiles/Base/Behavior.cs`, and this handoff file.
- Confirmed the exact-type whitelist in `AITacticalTargeting.ResolveProfile(...)` remains unchanged in phase 4.
- Confirmed non-whitelisted archers still use the stock `RangeFight..Weapon.MaxRange` spacing envelope and `1.0` second movement gate.
- Confirmed whitelisted `Skirmisher` archers alone now use helper-driven `0.5` second movement cadence, a faster `0.25` second chase cadence beyond their preferred band, and a preferred `3..6` tile spacing band clamped by weapon range.
- Confirmed `MeleeAI.DoActionCombat()` remains unchanged and `Bruiser` movement stays stock in phase 4.
- Confirmed phase 4 does not modify `DoMoveImpl(...)`, `MoveTo(...)`, `WalkRandomInHome(...)`, `Deactivate()`, `Activate()`, `OnCurrentSpeedChanged()`, `AITimer.OnTick()`, `PlayerRangeSensitive`, `ReacquireOnMovement`, or `AggressiveAction(...)`.
- Confirmed the `Combat` action path continues to own whitelisted archer repositioning and that `ActionType.Backoff`, `OnActionBackoff()`, and `OnActionChanged()` behavior remain untouched.

## Open Risks

- The biggest future risk is accidental expansion beyond the hostile stock whitelist.
- Phase 3 rollout can still overreach if later edits weaken the exact-type default-deny guard.
- Movement/cadence changes can spill into encounter behavior if not tightly scoped.
- Shell-local improvements can become restart bugs if persistence is not designed deliberately.
- External `AIObject` consumers and action-state callbacks can break even when shell inheritance looks unchanged.
- Deferred fight-mode domains and reserved role profiles still need separate audits before they can carry tactical weighting.
- Broadening from the exact-type cohort into stock mage or dynamic-shell families without a role-specific audit is still a high-risk step.
- Whitelisted skirmishers can still degrade to stock stand-and-fire behavior when poor terrain or blockers prevent them from reopening space cleanly.

## Next-Phase Prerequisites

- Review `docs/AI_OVERHAUL_AUDIT.md`.
- Review this handoff file.
- Keep phase 5 limited to role-specialist improvements for hostile mage, healer, and squad behavior on audited hostile content only.
- Preserve the exact-type rollout guard and phase-4 movement overlay while reviewing stock mage and support-role families; do not broaden civilians, vendors, healers, or service actors into the rollout.
- Preserve legality, control-order, bard, summon identity, `AIObject`, restart/load, action-state callback, and phase-4 movement invariants while adding hostile role behavior.
- Leave `ForcedAI`, summons with custom prey logic, citizen-tagged semantics, runtime switchers, and encounter-critical hook families excluded unless explicitly re-audited.
- Record any new exception or opt-out immediately.

## Update Checklist For Future Phases

- Update current phase and status.
- Add completed work.
- Add files and seams touched.
- Confirm unchanged safety invariants or record any deliberate, phase-approved change.
- Update whitelist and exclusions.
- Update persistence/restart considerations if new state was introduced.
- Record validation performed.
- Record open risks.
- Add next-phase prerequisites.
