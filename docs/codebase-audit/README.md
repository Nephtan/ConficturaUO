# Codebase Audit Guide

This directory is the durable working memory for the Confictura codebase systems audit and post-audit repair program. It is meant to answer practical maintainer questions without relying on chat history: what was audited, what evidence exists, what is still historical, what is currently active, and what verification is required before changing the shard.

The audit is not just a documentation dump. It separates live runtime script compile truth, Visual Studio project hygiene, save compatibility evidence, runtime hook evidence, documentation truth, and repair backlog state. Keep those distinctions intact when reading or updating anything under this directory.

## Read This First

Start with these files in this order:

| Need | File |
| --- | --- |
| Current audit and post-audit status | [PHASE_STATUS.md](PHASE_STATUS.md) |
| Exact command history and verification notes | [RUN_LOG.md](RUN_LOG.md) |
| Local rules for audit artifacts | [AGENTS.md](AGENTS.md) |
| Output artifact catalog and CSV tips | [outputs/README.md](outputs/README.md) |
| Generator script guide | [tools/README.md](tools/README.md) |
| Original full audit plan | [../../CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md](../../CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md) |
| Detailed phase plans | [../codebase-audit-phases/](../codebase-audit-phases/) |

For active repair work, read [PHASE_STATUS.md](PHASE_STATUS.md), then [outputs/post-audit-active-backlog-status.csv](outputs/post-audit-active-backlog-status.csv), then the relevant `post-batch-*` review artifact. The generated Phase 13 backlog remains important evidence, but it is not the whole current state after post-audit repair batches.

## Directory Map

| Path | What It Is | How To Use It |
| --- | --- | --- |
| [AGENTS.md](AGENTS.md) | Local instructions for audit artifacts. | Read before editing any file here. It defines the runtime vs project truth labels that audit outputs must preserve. |
| [PHASE_STATUS.md](PHASE_STATUS.md) | Human-readable phase and post-audit status ledger. | Use it as the current summary of completed phases, latest batches, verification, blockers, and important commits. |
| [RUN_LOG.md](RUN_LOG.md) | Command log. | Use it when you need exact commands, cwd, timestamps, results, and output paths. Add to it when running new audit batches. |
| [outputs/](outputs/) | Durable generated and reviewed audit artifacts. | Use CSVs and Markdown outputs to answer source ownership, hooks, serializers, docs truth, dependencies, and backlog questions. |
| [outputs/system-cards/](outputs/system-cards/) | Per-system engineering cards. | Use these for system summaries before drilling into raw CSVs. |
| [tools/](tools/) | PowerShell generators. | Use only when intentionally regenerating or refreshing audit outputs. Generator reruns can rewrite tracked files. |

## The Two Truth Models

The most important distinction in this directory is `RuntimeScriptCompileTruth` vs `ScriptsProjectTruth`.

`RuntimeScriptCompileTruth` means what the live server startup compiler sees. The maintained server executable compiles runtime scripts by recursively gathering `.cs` files under `Data/Scripts`. This is the truth that matters for whether the shard can compile scripts at startup. Relevant files include:

- [outputs/live-build-and-runtime-script-compile-model.md](outputs/live-build-and-runtime-script-compile-model.md)
- [outputs/runtime-script-compile-inventory.csv](outputs/runtime-script-compile-inventory.csv)
- [outputs/source-build-and-runtime-compile-baseline.md](outputs/source-build-and-runtime-compile-baseline.md)
- [outputs/compile-only-verification-baseline.md](outputs/compile-only-verification-baseline.md)

`ScriptsProjectTruth` means what `Data/Scripts/Scripts.csproj` lists for Visual Studio and solution/project hygiene. Missing project includes and stale project paths are real IDE/project findings, but they are not live runtime absence by themselves. Relevant files include:

- [outputs/project-truth-register.csv](outputs/project-truth-register.csv)
- [outputs/missing-compile-targets.csv](outputs/missing-compile-targets.csv)
- [outputs/unincluded-source-files.csv](outputs/unincluded-source-files.csv)
- [outputs/project-cleanup-backlog.csv](outputs/project-cleanup-backlog.csv)

Do not describe a `Scripts.csproj` mismatch as a live runtime blocker unless a server startup compile, compile-only run, or source evidence proves the same runtime failure.

## Current State In Plain Terms

The original audit phases 0 through 14 are complete and committed. The phase artifacts are retained as historical generated evidence. After the full audit, additional runtime context and repair batches changed the execution order:

- The original [outputs/repair-backlog.csv](outputs/repair-backlog.csv) is the Phase 13 generated backlog.
- [outputs/post-audit-active-backlog-status.csv](outputs/post-audit-active-backlog-status.csv) is the active overlay for reviewed or fixed backlog rows.
- [outputs/post-audit-next-steps.md](outputs/post-audit-next-steps.md) explains why [outputs/phase-13-batch-plan.csv](outputs/phase-13-batch-plan.csv) was superseded for implementation order.
- The `post-batch-*` CSV and Markdown files record reviewed source batches after the original audit closeout.

When checking whether a backlog row still needs work:

1. Look for the `BacklogId` in [outputs/post-audit-active-backlog-status.csv](outputs/post-audit-active-backlog-status.csv).
2. If it is present, use `ActiveStatus`, `PostAuditBatch`, `ReviewArtifact`, `SourceEvidence`, and `Commit` from that overlay.
3. If it is absent, fall back to the row in [outputs/repair-backlog.csv](outputs/repair-backlog.csv).
4. Confirm the latest narrative in [PHASE_STATUS.md](PHASE_STATUS.md), because it summarizes completed batches and known supersessions.

## Common Questions

Use these paths instead of searching every file first.

| Question | Read These First |
| --- | --- |
| What is the current audit state? | [PHASE_STATUS.md](PHASE_STATUS.md), then [outputs/post-audit-next-steps.md](outputs/post-audit-next-steps.md) |
| What command produced or verified something? | [RUN_LOG.md](RUN_LOG.md), then the relevant `phase-*-summary.md` or `post-batch-*` artifact |
| Which system owns a file? | [outputs/system-owner-map.csv](outputs/system-owner-map.csv), [outputs/cross-tree-runtime-inventory.csv](outputs/cross-tree-runtime-inventory.csv), then a card in [outputs/system-cards/](outputs/system-cards/) |
| What runtime hooks or commands exist? | [outputs/runtime-hook-map.csv](outputs/runtime-hook-map.csv), [outputs/phase-05-command-surface-register.csv](outputs/phase-05-command-surface-register.csv), [outputs/phase-05-packet-handler-register.csv](outputs/phase-05-packet-handler-register.csv) |
| Is a serialized type safe to move or edit? | [outputs/serialization-register.csv](outputs/serialization-register.csv), [outputs/phase-06-move-rename-risk-list.csv](outputs/phase-06-move-rename-risk-list.csv), [outputs/phase-06-high-risk-serializer-list.csv](outputs/phase-06-high-risk-serializer-list.csv) |
| Is a docs page source-traced or stale? | [outputs/documentation-truth-table.csv](outputs/documentation-truth-table.csv), [outputs/phase-07-stale-claim-backlog.csv](outputs/phase-07-stale-claim-backlog.csv) |
| What depends on what? | [outputs/dependency-graph.csv](outputs/dependency-graph.csv), [outputs/phase-08-hard-dependency-list.csv](outputs/phase-08-hard-dependency-list.csv), [outputs/phase-08-soft-dependency-list.csv](outputs/phase-08-soft-dependency-list.csv) |
| Which systems support or conflict with each other? | [outputs/synergy-conflict-matrix.csv](outputs/synergy-conflict-matrix.csv), [outputs/phase-09-balance-risk-list.csv](outputs/phase-09-balance-risk-list.csv) |
| What repair work is still actionable? | [outputs/post-audit-active-backlog-status.csv](outputs/post-audit-active-backlog-status.csv), [outputs/repair-backlog.csv](outputs/repair-backlog.csv), [outputs/verification-matrix.csv](outputs/verification-matrix.csv) |
| What reorganization was proposed? | [outputs/reorganization-design.md](outputs/reorganization-design.md), [outputs/phase-12-move-proposal-table.csv](outputs/phase-12-move-proposal-table.csv), [outputs/phase-12-keep-in-place-decisions.csv](outputs/phase-12-keep-in-place-decisions.csv) |

## How To Read Large CSVs

The CSVs are large enough that a text editor may be awkward. Use PowerShell from the repository root when filtering:

```powershell
Import-Csv docs/codebase-audit/outputs/post-audit-active-backlog-status.csv |
    Where-Object { $_.BacklogId -eq 'RB-03235' } |
    Format-List
```

```powershell
Import-Csv docs/codebase-audit/outputs/runtime-hook-map.csv |
    Where-Object { $_.System -eq 'XMLSpawner' -and $_.Risk -in @('High', 'Critical') } |
    Select-Object File,HookType,Registration,Handler,Risk |
    Format-Table -AutoSize
```

```powershell
Import-Csv docs/codebase-audit/outputs/serialization-register.csv |
    Where-Object { $_.File -like '*PlayerMobile.cs' } |
    Select-Object Class,CurrentVersion,VersionHandling,FieldAlignment,MoveRenameRisk |
    Format-List
```

Tips:

- Prefer `Import-Csv` filters over manual spreadsheet edits.
- Use `Select-Object` to reduce wide rows before reading them.
- Treat semicolon-delimited cell contents as compact evidence lists, not as independent rows.
- Keep repository-relative paths unchanged when copying evidence into docs or backlog notes.
- If Excel or another spreadsheet tool rewrites quoting, dates, or line endings, discard that edit and use PowerShell instead.

## Status And Disposition Terms

Phase statuses in [PHASE_STATUS.md](PHASE_STATUS.md):

| Status | Meaning |
| --- | --- |
| `NotStarted` | Phase has not begun. |
| `InProgress` | Work has started but exit criteria are not met. |
| `Complete` | Exit criteria are met, but the phase is not necessarily committed. |
| `Committed` | Phase output was completed and committed. |
| `Blocked` | Work cannot safely continue without a named blocker resolution. |
| `Deferred` | Work is intentionally postponed with evidence. |
| `Intentional` | State is intentionally left as-is. |

Backlog and review dispositions commonly seen in post-audit files:

| Status | Meaning |
| --- | --- |
| `Ready` | Historical backlog item was considered actionable at generation time. Check the active overlay before acting. |
| `Open` | Historical item remained open at generation time. Check the active overlay before acting. |
| `Deferred` | Not approved for immediate implementation. Usually requires a later design, migration, or reorganization batch. |
| `Fixed` | Source or docs were changed and the artifact records the commit and verification. |
| `ReviewedNoChange` | Source was reviewed and no change was made because behavior was acceptable or already covered elsewhere. |
| `SafeNoChange` | Source was reviewed and the original risk signal was determined safe without edits. |
| `IntentionalLegacy` | The behavior is intentionally retained for compatibility or legacy reasons. |
| `FalsePositive` | The generated risk was not a real issue after source review. |

## Safe Use Rules

Before changing source or audit outputs:

1. Run `git status --short`.
2. Read the relevant `AGENTS.md` files. This directory has its own [AGENTS.md](AGENTS.md).
3. Read [PHASE_STATUS.md](PHASE_STATUS.md) for the latest known state.
4. Find the relevant source evidence in `outputs/`, not just a generated risk row.
5. Decide whether the change is documentation-only, generator-output refresh, project hygiene, source repair, serialization-affecting, or reorganization.
6. Use the verification level from [outputs/verification-matrix.csv](outputs/verification-matrix.csv) and the root instructions.

Never approve or perform source reorganization from audit tables alone. A move still needs:

- Phase 12 design evidence.
- Save compatibility review for serialized types.
- Runtime script compile visibility review.
- `Scripts.csproj` update when preserving Visual Studio project hygiene.
- Documentation source-trace updates.
- A rollback plan.
- Build and compile-only verification appropriate to the touched files.

For serializer changes, preserve RunUO positional serialization. Do not reorder reads or writes, change serialized type names, rename namespaces, or move save-sensitive classes without a migration plan and explicit approval.

For runtime hooks, commands, packet handlers, gumps, regions, timers, and world save/load paths, verify trigger scope and guards in source. Generated marker rows are triage leads; source review decides the actual fix.

## Verification Shortcuts

Documentation-only changes under this directory usually need:

```powershell
git diff --check
```

Also check any new Markdown links or referenced paths with `Test-Path` or targeted `rg`.

Source-code repair batches usually need the narrowest relevant source checks plus:

```powershell
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
.\ConficturaServer.exe -compileonly -nocache
```

Use the Visual Studio MSBuild path recorded in audit outputs if `msbuild` is not on `PATH`. The old full startup smoke is not the default safe verification path for this checkout because startup can load saves and bind listeners. Use the compile-only path when verifying runtime script compilation.

`Scripts.csproj` or solution builds are IDE/project hygiene verification. They do not prove live runtime script compile unless paired with server compile/startup evidence.

## Updating This Directory

Use these habits when adding or changing audit artifacts:

- Keep generated phase outputs phase-labeled.
- Keep canonical copies, such as `serialization-register.csv`, clearly tied to their phase-scoped source.
- Preserve historical outputs instead of rewriting them to hide old decisions.
- Use overlay files for post-audit dispositions when reconciling historical backlog rows.
- Record new commands in [RUN_LOG.md](RUN_LOG.md) when running audit or verification batches.
- Update [PHASE_STATUS.md](PHASE_STATUS.md) when the durable state changes.
- Stage only intended files. `outputs/` has historically been ignored by local exclude rules in some checkouts, so force-staging may be required for intended output artifacts.

## Regenerating Outputs

The scripts in [tools/](tools/) can rewrite many tracked files under [outputs/](outputs/). Regenerate only when you intend to refresh that phase or register.

Before rerunning a generator:

1. Read [tools/README.md](tools/README.md).
2. Check that required input artifacts exist.
3. Run `git status --short`.
4. Decide whether the output refresh is part of the current task.
5. After the run, inspect the diff carefully. Generated churn can be large.
6. Record the command, result, and changed outputs in [RUN_LOG.md](RUN_LOG.md).

For routine source repair, do not regenerate every audit artifact. Refresh only the register that proves the repair, such as the runtime hook map after packet or hook work, or the serialization register after serializer work.
