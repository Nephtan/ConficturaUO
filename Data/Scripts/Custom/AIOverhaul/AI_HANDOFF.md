# AI Handoff

## Purpose

This file is the living state record for the AI overhaul project. Review it before every phase after phase 1. Update it before stopping any phase. Treat it as both the handoff log and the live exception register.

## Project Pointers

- Audit: `docs/AI_OVERHAUL_AUDIT.md`
- Roadmap: `Data/Scripts/Custom/AIOverhaul/AI_PHASED_OVERHAUL_PLAN.md`
- Local rules: `Data/Scripts/Custom/AIOverhaul/AGENTS.md`

## Current Phase

- Status: `Phase 0 - Planning complete / implementation not started`
- Next execution target: `Phase 1 - Workspace, Governance, And No-Op Infrastructure`

## Completed Work

- Created the dedicated AI overhaul workspace.
- Created the phased implementation roadmap.
- Created the local AI overhaul operating rules.
- Seeded the handoff structure and initial exclusion register.

## Files And Seams Touched

- `Data/Scripts/Custom/AIOverhaul/AI_PHASED_OVERHAUL_PLAN.md`
- `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md`
- `Data/Scripts/Custom/AIOverhaul/AGENTS.md`
- No live gameplay code touched yet.

## Unchanged Safety Invariants

- Legality-first targeting stays intact.
- Bard provoke, pacify, and unpacify-on-damage semantics stay intact.
- Pet and summon control-order semantics stay intact.
- Summon prey identity and custom prey-ranking overrides stay intact unless explicitly audited later.
- Save/load and restart-safe AI assignment remains mandatory.
- `AIObject` compatibility remains a required audit surface.
- Action-state callback behavior remains a required audit surface.
- Rollout stays whitelist-first.

## Initial Whitelist Target

- Ordinary uncontrolled hostile stock mobs using standard shells.

## Initial Exclusions

- Civilians, town actors, vendors, healers, and service NPCs.
- `ForcedAI` actors.
- Summons and magical constructs with custom target-ranking behavior.
- Citizen-tagged semantic AI usage.
- Runtime AI switchers and transformation-driven shell changes.
- Encounter-critical bosses or systems with cadence-sensitive `OnThink`, `OnDamage`, or `OnGotMeleeAttack` behavior.
- Any family that depends directly on `AIObject` manipulation until separately reviewed.

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

## Persistence And Restart Considerations

- `CurrentAI` and `DefaultAI` are owner-side persisted state.
- The live `BaseAI` shell is reconstructed on deserialize.
- Any new tactical profile, role layer, or shell-local state needs an explicit persistence story if it must survive restart.
- Any rollout guard that changes assignment or shell behavior must be checked against deserialize-time rehydration.

## Validation Performed

- Documentation-only initialization.
- Confirmed the project workspace lives outside `/docs`, which is gitignored.
- Confirmed no gameplay code has been changed yet.

## Open Risks

- The biggest future risk is accidental expansion beyond the hostile stock whitelist.
- Movement/cadence changes can spill into encounter behavior if not tightly scoped.
- Shell-local improvements can become restart bugs if persistence is not designed deliberately.
- External `AIObject` consumers and action-state callbacks can break even when shell inheritance looks unchanged.

## Next-Phase Prerequisites

- Review `docs/AI_OVERHAUL_AUDIT.md`.
- Review this handoff file.
- Keep phase 1 no-op with respect to live AI behavior.
- Confirm the whitelist and exclusions before adding any scaffolding.
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
