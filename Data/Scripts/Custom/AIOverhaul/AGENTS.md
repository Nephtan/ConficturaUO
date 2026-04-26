# AGENTS.md (AI Overhaul Project)

This file extends the root `AGENTS.md` for the `Data/Scripts/Custom/AIOverhaul/` workspace.

## Purpose

This folder is the versioned coordination workspace for the AI overhaul project. It stores the phased roadmap, the live handoff record, and any future no-op scaffolding or project-local support files created to drive the overhaul safely.

## Operating Rules

- Review `Data/Scripts/Custom/AIOverhaul/AI_HANDOFF.md` before starting any work after phase 1.
- Keep `AI_HANDOFF.md` synchronized with the actual state of the project. Update it before ending every phase.
- Preserve legality-first targeting unless the active phase explicitly audits and approves a change. This includes faction, ethic, honor, bard, control-order, and harmful-action semantics.
- Keep the rollout whitelist-first. Do not broaden into civilians, vendors, healers, `ForcedAI` actors, summon-ranking overrides, runtime switchers, citizen-tagged semantics, or encounter-critical families unless the active phase explicitly audits them.
- Preserve restart safety. Any new owner-side or shell-side AI state must have a documented persistence strategy if it must survive save/load or server restart.
- Treat `AIObject` and `OnAction*()` compatibility as mandatory audit surfaces whenever assignment, shell behavior, movement cadence, or dispatcher flow is touched.
- Document every new exception, opt-out, compatibility shim, or rollout guard in `AI_HANDOFF.md` immediately.
- Prefer no-op scaffolding, explicit rollout guards, and opt-in behavior over silent global behavior changes.

## Documentation Expectations

- `AI_PHASED_OVERHAUL_PLAN.md` is the execution roadmap.
- `AI_HANDOFF.md` is the live state record and exception register.
- If future files are added in this folder, they should clearly state whether they are documentation, no-op scaffolding, or live implementation support.

## Code Expectations

- If future C# implementation files are added under this folder, follow the root project rules:
- Use the `Server.Custom.Confictura` namespace.
- Use PascalCase class and file names.
- Comment non-obvious logic clearly and succinctly.
- Keep new code inert by default until the active phase explicitly calls for live rollout.

## Escalation Rules

- If a phase needs to touch gameplay code outside this folder, record the touched files and behavior seams in `AI_HANDOFF.md`.
- If a phase uncovers a new family that does not fit the current whitelist assumptions, stop broadening the rollout, document the family as an exception, and carry the issue into the next handoff.
