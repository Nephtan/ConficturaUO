# Phase 0: Baseline And Guardrails

## Purpose

Phase 0 creates the control point for the entire audit. The goal is to know the
exact repository state, instruction scope, build entry points, and current risk
boundaries before any broad inventory, documentation rewrite, or source
reorganization begins.

This phase prevents two common failures: auditing stale state and changing files
under the wrong local instructions.

## Required Inputs

- Root `AGENTS.md`.
- Any nested `AGENTS.md` files in paths that may be touched later.
- `git status --short`.
- Current branch and latest commit.
- `ConficturaUO.sln`.
- `Data/System/Source/Server.csproj`.
- `Data/Scripts/Scripts.csproj`.
- Root plan file: `CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`.

## Required Outputs

- Baseline status note.
- Instruction scope register.
- Build entry point register.
- Initial risk boundary note.
- Explicit decision that no source reorganization begins in this phase.

## Subphase 0.1: Worktree Baseline

1. Run `git status --short`.
2. Record whether the worktree is clean.
3. If dirty, classify every changed file as audit-related, user-owned, build
   output, generated output, or unknown.
4. Stop before editing if unknown changes overlap the planned files.
5. Do not revert user changes.

Output table:

| Path | Status | Owner | Action |
| --- | --- | --- | --- |
| Example | Modified | User | Leave untouched |

Completion gate:

- Every pre-existing change is accounted for.

## Subphase 0.2: Instruction Scope Register

1. Run `rg --files -g AGENTS.md`.
2. Record every instruction file.
3. Map instruction files to the directories they govern.
4. For this phase expansion work, confirm whether root-only instructions apply
   or whether a docs-specific instruction applies.
5. Before later source edits, repeat this check for the exact target folder.

Output table:

| Instruction File | Applies To | Notes |
| --- | --- | --- |
| `AGENTS.md` | Repository root | General shard instructions |

Completion gate:

- Every path planned for editing has a known instruction scope.

## Subphase 0.3: Build Surface Baseline

1. Inspect `ConficturaUO.sln`.
2. Confirm the solution includes `Server` and `Scripts`.
3. Confirm `Scripts` depends on `Server`.
4. Inspect target frameworks, platforms, output names, and output paths.
5. Record the expected narrow and full build commands.

Output table:

| Project | Role | Framework | Platform | Output | Notes |
| --- | --- | --- | --- | --- | --- |
| Server | Core executable | .NET Framework 4.8 | x86 | `ConficturaServer` | Solution dependency root |
| Scripts | Script library | .NET Framework 4.8 | AnyCPU | `ClassLibrary` | Depends on Server |

Completion gate:

- The audit knows which build command proves which class of change.

## Subphase 0.4: Source Root Boundary

Record the roots that must be included in later phases:

- `Data/Scripts/Custom`
- `Data/Scripts/Items`
- `Data/Scripts/Magic`
- `Data/Scripts/Mobiles`
- `Data/Scripts/Quests`
- `Data/Scripts/System`
- `Data/Scripts/Trades`
- `Data/System/Source`
- `docs`
- XML/config data files used by scripts

Completion gate:

- The audit scope is cross-tree, not only `Custom`.

## Subphase 0.5: Risk Boundary

Create a first-pass risk note for areas that must not be casually moved or
rewritten:

- Serialized `Item` and `Mobile` types.
- `PlayerMobile` fields.
- Global event hooks.
- Packet handlers.
- `Scripts.csproj` compile entries.
- XMLSpawner attachments.
- Spell registry and magic initialization.
- Region and Notoriety policy.
- Staff event tools.
- Legacy compatibility files.

Completion gate:

- Later phases have an explicit list of areas requiring extra proof.

## Review Checklist

- Worktree status captured.
- Instruction scopes captured.
- Solution/project shape captured.
- Source roots captured.
- High-risk areas identified.
- No broad source edits made.

## Exit Criteria

Phase 0 is complete when a reviewer can answer:

- What exact repository state did the audit start from?
- Which instructions govern edited paths?
- Which projects and build commands matter?
- Which roots are in scope?
- Which systems require extra caution before edits?
