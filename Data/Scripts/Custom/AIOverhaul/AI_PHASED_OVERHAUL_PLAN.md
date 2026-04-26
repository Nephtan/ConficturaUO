# AI Phased Overhaul Plan

## Purpose

This file is the versioned execution roadmap for the Confictura AI overhaul project. It turns the findings in `docs/AI_OVERHAUL_AUDIT.md` into a phased implementation sequence that improves regular NPC AI without breaking legality, control systems, restart safety, summon identity, or encounter scripting.

Use this file as the operational source of truth for rollout order, phase scope, validation expectations, and ready-to-paste execution prompts.

## Standing Rules

- Preserve legality-first targeting. Tactical improvements must layer after existing legality, faction, ethic, honor, bard, and harmful-action checks.
- Keep rollout whitelist-first. Start with ordinary uncontrolled hostile stock mobs only.
- Treat `AI_HANDOFF.md` as the live project state and exception register.
- Review `AI_HANDOFF.md` before every phase after phase 1.
- Update `AI_HANDOFF.md` before ending every phase, including documentation-only phases.
- Preserve save/load and restart safety whenever owner-side AI state, shell assignment, or shell-local tactical state changes.
- Do not broaden into `ForcedAI`, summon-ranking overrides, runtime AI switchers, civilian/service NPCs, or encounter-critical hooks unless the active phase explicitly audits them.
- Do not replace the movement, timer, or activation systems wholesale. Reuse existing helpers and extend them incrementally.
- Do not introduce a persistent threat table in the first rollout. Prefer bounded post-legality scoring plus explicit reacquire triggers.
- Any new exception, opt-out, compatibility shim, or rollout guardrail must be recorded in `AI_HANDOFF.md` immediately.

## Phase Table

| Phase | Goal | Scope | Exit Criteria |
| --- | --- | --- | --- |
| 1 | Workspace, governance, and no-op infrastructure | Project folder, handoff, local rules, whitelist/exclusion register, no-op scaffolding only | Workspace exists, no live AI behavior changed, handoff seeded, exclusions defined, first cohort named |
| 2 | Core tactical targeting seam | Post-legality target weighting and event-driven reacquire only | Tactical targeting exists behind rollout guards, legality untouched, no threat table introduced |
| 3 | Whitelist rollout to ordinary hostile stock mobs | Apply phase 2 logic to audited stock hostile mobs only | Whitelist is live, exclusions still enforced, no bleed into exception families |
| 4 | Movement, spacing, and cadence for the whitelist | Incremental use of existing movement helpers on whitelisted mobs | Spacing/chase improvements are stable without changing global activation or timer rules |
| 5 | Role-specialist pass for mage, healer, and squad behaviors | Reuse existing role logic and add bounded squad behaviors to audited hostile mobs | Role behaviors are improved without touching service NPC behavior or legality semantics |
| 6 | Exceptions, encounter audit, and expansion gate | Audit exception families and decide what stays excluded or gets a dedicated follow-up | Expansion decision is documented, exception register is current, no unaudited families are silently broadened |

## Phase Prompts

### Phase 1 Prompt: Workspace, Governance, And No-Op Infrastructure

```text
Review `docs/AI_OVERHAUL_AUDIT.md` first, especially the executive summary, methodology/confidence, core framework, base-creature responsibilities, and the safety-centralization sections.

Create or refresh the AI overhaul workspace at `Data/Scripts/Custom/AIOverhaul/`.

Goal:
Establish the project workspace, handoff system, local operating rules, whitelist/exclusion register, and any no-op scaffolding needed for later phases without changing live AI behavior.

In scope:
- Create or refresh `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md`.
- Create or refresh `Data/Scripts/Custom/AIOverhaul/AGENTS.md`.
- Create or refresh any no-op documentation or scaffolding files that later phases need in order to work safely.
- Define the initial rollout cohort as ordinary uncontrolled hostile stock mobs using standard shells.
- Seed the first exclusion register from the audit.

Explicit non-goals:
- No change to live targeting behavior.
- No change to shell assignment behavior.
- No change to movement, activation, cadence, bard, summon, or control-order logic.
- No rollout beyond documentation, guardrails, or no-op scaffolding.

Required implementation tasks:
- Build the workspace with the handoff file and local AGENTS file.
- Add a handoff template that records current phase, completed work, touched seams, unchanged safety invariants, whitelist/exclusions, persistence notes, validation performed, open risks, and next-phase prerequisites.
- Record the project’s standing invariants: legality-first targeting, whitelist-first rollout, restart-safe assignment, summon identity preservation, and explicit exception tracking.
- If code scaffolding is added, ensure it is inert by default and cannot change live behavior until a later phase explicitly enables it.

Validation requirements:
- Verify there is no live behavior change in this phase.
- Verify the initial whitelist and exclusion families match the audit.
- Verify the handoff file is sufficient for another engineer to continue from this phase with no hidden assumptions.

Before stopping:
- Update `AI_HANDOFF.md` with the workspace artifacts created, the chosen whitelist cohort, the initial exclusion register, and confirmation that this phase made no live behavior changes.
```

### Phase 2 Prompt: Core Tactical Targeting Seam

```text
Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` first.

Then review `docs/AI_OVERHAUL_AUDIT.md`, especially the sections on `BaseCreature` responsibilities, target ranking, reacquire timing, `AcquireFocusMob`, gameplay systems tightly coupled to AI, and what is safe versus unsafe to centralize.

Goal:
Add the core tactical targeting seam by improving post-legality target preference and event-driven reacquire behavior without changing the legality pipeline.

In scope:
- Richer post-legality target scoring after the existing legality pass.
- Bounded tactical weighting layered onto existing ranking logic.
- Event-driven reacquire triggers or guardrails that fit the existing reacquire model.
- Rollout guards or opt-in surfaces needed to keep the change limited to future whitelist use.

Explicit non-goals:
- No persistent threat table.
- No rewrite or bypass of legality checks in `AcquireFocusMob`.
- No bard, provoke, pacify, control-order, harmful-action, faction, ethic, or honor semantic changes.
- No global movement rewrite.
- No global timer cadence rewrite.
- No expansion into civilians, vendors, healers, `ForcedAI`, summons with custom prey logic, or runtime switchers.

Required implementation tasks:
- Add the tactical scoring seam at the existing post-legality ranking point.
- Keep the scoring bounded and compatible with stock shell behavior.
- Use explicit reacquire timing and event hooks rather than persistent threat storage.
- Preserve the default behavior for content outside the rollout guard.
- Record any new guard surfaces, compatibility assumptions, or opt-outs in the handoff file.

Validation requirements:
- Verify legality/control invariants still gate targeting before tactical scoring.
- Verify the change remains inert for non-whitelisted actors.
- Verify restart/load behavior remains safe if any owner-side or shell-side state is introduced.
- Verify no `AIObject` consumers or action-state callbacks are accidentally bypassed.

Before stopping:
- Update `AI_HANDOFF.md` with the targeting seam added, the rollout guard shape, the exact invariants preserved, the validation performed, and any newly identified exception families.
```

### Phase 3 Prompt: Whitelist Rollout To Ordinary Hostile Stock Mobs

```text
Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` first.

Then review `docs/AI_OVERHAUL_AUDIT.md`, especially the whitelist rollout guidance, minimum exclusion list, stock shell deployment notes, and the sections on civilian/service actors, dynamic AI switchers, and summon identity.

Goal:
Apply the tactical targeting layer to a narrowly defined whitelist of ordinary uncontrolled hostile stock mobs using standard shells.

In scope:
- Enable the new tactical layer for the approved hostile stock cohort.
- Keep rollout explicit and reversible.
- Refine the whitelist and exclusion register based on audited deployment reality.

Explicit non-goals:
- No civilians, vendors, healers, or service NPCs.
- No `ForcedAI` actors.
- No summon or magical-construct prey-ranking overrides.
- No citizen-tagged semantic AI usage.
- No runtime AI switchers such as multi-shell or transformation-driven actors unless separately audited.
- No encounter-critical bosses or scripts with phase-sensitive `OnThink`, `OnDamage`, or `OnGotMeleeAttack` behavior.

Required implementation tasks:
- Turn on the tactical layer only for the approved hostile stock cohort.
- Keep rollout logic explicit enough that future phases can add or remove families without ambiguous side effects.
- Update the handoff whitelist and exclusion register with the exact families included and excluded.
- Document any mob families that looked eligible at first glance but were excluded after audit.

Validation requirements:
- Verify the tactical layer is live only on the intended stock hostile cohort.
- Verify excluded families remain on baseline behavior.
- Verify legality, control, bard, summon identity, and restart/load safety still hold.
- Verify the rollout is readable enough for the next phase to build on without rediscovering family boundaries.

Before stopping:
- Update `AI_HANDOFF.md` with the active whitelist, all exclusions, the rollout guard status, validation results, and any rollout surprises or exceptions discovered during application.
```

### Phase 4 Prompt: Movement, Spacing, And Cadence For The Whitelist

```text
Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` first.

Then review `docs/AI_OVERHAUL_AUDIT.md`, especially the movement/pathing section, activation/timer flow, action callback surface, spawn/home behavior, and the movement-related entries in the overhaul-readiness matrix.

Goal:
Improve movement feel for the already-whitelisted hostile stock mobs by reusing and tuning existing movement helpers for spacing, chase, backoff, and pathing.

In scope:
- Incremental movement or spacing improvements for the current whitelist only.
- Better use of existing helper surfaces for chase distance, retreat distance, path recovery, and combat spacing.
- Cadence adjustments only where they are tightly bounded to the whitelisted tactical layer and do not globalize timer behavior.

Explicit non-goals:
- No global `PlayerRangeSensitive` rewrite.
- No rewrite of sector activation or deactivation rules.
- No broad timer cadence rewrite.
- No spawn/home/waypoint rule rewrite.
- No bypass of the stock action dispatcher or `OnAction*()` callbacks.
- No expansion into exception families.

Required implementation tasks:
- Improve movement behavior using existing helper surfaces rather than inventing a second movement engine.
- Keep any cadence changes opt-in and whitelist-bound.
- Preserve action-state callback behavior and home/spawn containment semantics.
- Record any movement-specific compatibility notes or opt-outs in the handoff file.

Validation requirements:
- Verify targeting behavior from earlier phases remains stable.
- Verify movement/spatial changes do not affect excluded families.
- Verify activation, home, waypoint, and region containment behavior are still intact.
- Verify `OnAction*()` piggyback logic and `AIObject` consumers are not broken by movement changes.

Before stopping:
- Update `AI_HANDOFF.md` with the movement helpers used, the cadence assumptions preserved, the validation performed, and any newly discovered movement-specific exceptions.
```

### Phase 5 Prompt: Role-Specialist Pass For Mage, Healer, And Squad Behaviors

```text
Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` first.

Then review `docs/AI_OVERHAUL_AUDIT.md`, especially the stock shell notes for `MageAI` and `HealerAI`, the role-specific workstream guidance, the team-aware helper surface, and the service/social NPC sections that must remain excluded.

Goal:
Improve role-specific behavior for audited hostile content by reusing existing mage, healer, and squad-oriented shell strengths before inventing new bespoke brains.

In scope:
- Better use of existing `MageAI` strengths on approved hostile content.
- Better use of existing `HealerAI` strengths on approved hostile content.
- Bounded squad or role overlays such as captain, support, skirmisher, or pack behavior for audited hostile families only.

Explicit non-goals:
- No changes to vendor or healer service NPC behavior.
- No global role rewrite across all content.
- No OmniAI-grade parity rollout for ordinary NPCs.
- No changes to legality, bard, control-order, summon identity, or civilian semantics.
- No expansion into `ForcedAI`, runtime switchers, or encounter-critical exceptions.

Required implementation tasks:
- Reuse the existing role logic where possible before introducing new surfaces.
- Keep role overlays bounded, readable, and tied to approved hostile families.
- Preserve the difference between hostile role logic and service/social behavior.
- Record any role-specific rollout guards, exclusions, or compatibility assumptions in the handoff file.

Validation requirements:
- Verify service NPCs remain untouched.
- Verify whitelisted hostile role behavior improves without altering legality or assignment semantics.
- Verify restart/load safety if role metadata or tactical profile state is persisted.
- Verify squad-aware behavior does not require a new persistent threat or coordination system.

Before stopping:
- Update `AI_HANDOFF.md` with the role behaviors added, the hostile families using them, the service families kept excluded, the validation performed, and any remaining role-specific risks.
```

### Phase 6 Prompt: Exceptions, Encounter Audit, And Expansion Gate

```text
Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` first.

Then review `docs/AI_OVERHAUL_AUDIT.md`, especially the `ForcedAI` inventory, custom target-ranking overrides, runtime AI reassignment inventory, action callback and `AIObject` integration surfaces, AI-adjacent `OnThink` hooks, scripted encounter hooks, and the minimum exclusion list.

Goal:
Audit the exception families and decide, with explicit documentation, what remains excluded from the main rollout and what deserves a dedicated future phase.

In scope:
- Review `ForcedAI` actors.
- Review dynamic/runtime shell switchers.
- Review `AIObject` consumers.
- Review action-callback piggybackers.
- Review summon identity overrides.
- Review encounter-critical `OnThink`, `OnDamage`, and `OnGotMeleeAttack` families.
- Produce a go/no-go expansion decision backed by the current handoff state.

Explicit non-goals:
- No automatic rollout into exception families just because they were audited.
- No silent compatibility break for live-shell consumers.
- No change to encounter cadence or boss scripting without explicit dedicated follow-up scope.

Required implementation tasks:
- Turn the handoff’s exception register into a reviewed decision record.
- Separate families into three buckets: remain excluded, require dedicated follow-up phase, or safe for future opt-in expansion.
- Document exact reasons for each bucket so later work does not reopen the same analysis from scratch.
- If any compatibility shims are required for `AIObject` consumers or action-callback users, document them explicitly.

Validation requirements:
- Verify all prior-phase safety invariants still hold.
- Verify the exception register is current and grounded in audited code surfaces.
- Verify no broader rollout occurs unless the handoff explicitly says the invariants still hold and the target family has been reclassified deliberately.

Before stopping:
- Update `AI_HANDOFF.md` with the reviewed exception families, go/no-go decisions, future-phase candidates, validation performed, and the final recommendation for whether broader rollout should proceed.
```

## Acceptance Checklist For Expansion Beyond The Initial Whitelist

- The previous phase updated `AI_HANDOFF.md` completely.
- The current whitelist and exclusion register are current and explicit.
- Legality, bard, control-order, summon identity, and restart/load invariants are still intact.
- Any new owner-side AI state or shell-side tactical state has a documented persistence story.
- `AIObject` consumers and `OnAction*()` callback users were checked for compatibility.
- No scripted encounter family was broadened without direct audit.
- No phase broadened from stock hostile whitelist mobs into exception families without an explicit handoff decision.

## Default Implementation Posture

- Favor central improvements at the post-legality scoring seam.
- Favor event-driven reacquire over persistent threat storage.
- Favor opt-in tactical overlays over stock-shell replacement.
- Favor movement-helper reuse over new pathing subsystems.
- Favor explicit exclusions over optimistic rollout.
- Favor audit-backed documentation over hidden assumptions.
