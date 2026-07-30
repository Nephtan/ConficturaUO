# Codebase Audit Tools

This directory contains the PowerShell generators used by the codebase systems audit. They produce the CSV and Markdown artifacts under `docs/codebase-audit/outputs/`.

These scripts are not passive reports. Most of them rewrite tracked output files. Run them only when the current task intentionally refreshes audit evidence, and inspect the resulting diff before staging.

## Common Invocation

Run scripts from the repository root unless a task explicitly uses another working directory:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/codebase-audit/tools/Invoke-CodebaseAuditInventory.ps1
```

Every script accepts these optional parameters:

| Parameter | Default |
| --- | --- |
| `RepoRoot` | The repository root, resolved from the script location. |
| `OutputDir` | `docs/codebase-audit/outputs` under `RepoRoot`. |

Example with explicit paths:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File docs/codebase-audit/tools/New-SerializationRegister.ps1 -RepoRoot D:\ConficturaUO -OutputDir D:\ConficturaUO\docs\codebase-audit\outputs
```

Use `pwsh` instead of `powershell` only when that is the local shell standard for the session. The scripts use Windows PowerShell-compatible syntax.

## Before Running A Generator

1. Run `git status --short`.
2. Confirm no unrelated user changes overlap the output files the script will rewrite.
3. Read [../AGENTS.md](../AGENTS.md) and the relevant phase plan under `docs/codebase-audit-phases/`.
4. Confirm required input files exist.
5. Know which outputs are expected to change.

After running:

1. Review `git diff --stat` and targeted `git diff`.
2. Run `git diff --check`.
3. Record the command and result in [../RUN_LOG.md](../RUN_LOG.md) if this is a durable audit update.
4. Update [../PHASE_STATUS.md](../PHASE_STATUS.md) when the durable state changes.

## Script Reference

| Script | Phase | Reads | Writes | Use When |
| --- | --- | --- | --- | --- |
| `Invoke-CodebaseAuditInventory.ps1` | Phase 1 | Source tree, project files, docs, config/data files, instruction files. | `phase-01-*` inventories and `phase-01-summary.md`. | Rebuild the base inventory after broad source/docs/project changes. |
| `New-ProjectTruthRegister.ps1` | Phase 2 | Phase 1 source/project inventories and `Scripts.csproj`. | `project-truth-register.csv`, missing/unincluded/project cleanup outputs, `phase-02-summary.md`. | Refresh Visual Studio project hygiene evidence. |
| `New-CrossTreeRuntimeInventory.ps1` | Phase 3 | Phase 1 marker inventories and project truth. | `cross-tree-runtime-inventory.csv`, root summaries, unknown owner list, `phase-03-summary.md`. | Reclassify source roles and system ownership after source layout or marker changes. |
| `New-SystemCards.ps1` | Phase 4 | Runtime inventory, project truth, marker-derived hook/serialization/docs data. | `system-cards/`, `system-owner-map.csv`, card index/backlog/priority outputs, `phase-04-summary.md`. | Refresh high-risk system cards and file-to-system ownership. |
| `New-RuntimeHookMap.ps1` | Phase 5 | Runtime marker inventory, cross-tree inventory, source files. | `runtime-hook-map.csv`, command/packet/gump/timer focused registers, `phase-05-summary.md`. | Rebuild hook, command, packet, timer, gump, and region evidence. |
| `New-SerializationRegister.ps1` | Phase 6 | Serialization markers, runtime inventory, project truth, system owner map, source files. | `serialization-register.csv`, high-risk serializer, move-risk, comment-target, and save-backlog outputs, `phase-06-summary.md`. | Recheck save format, read/write order, version handling, or move/rename risk. |
| `New-DocumentationTruthAudit.ps1` | Phase 7 | Docs, wiki index/backlog, system cards, hook map, serializer register, project truth. | `documentation-truth-table.csv`, canonical/alias/stale/coverage outputs, `phase-07-summary.md`. | Refresh documentation truth and source-trace coverage. |
| `New-DependencyGraph.ps1` | Phase 8 | System cards, owner map, runtime inventory, hook map, serializer register, docs truth, project truth, config references. | `dependency-graph.csv`, hard/soft/conflict/standalone outputs, `phase-08-summary.md`. | Rebuild source-verified system relationship evidence. |
| `New-SynergyConflictMatrix.ps1` | Phase 9 | Dependency graph, system cards, docs truth, runtime hook map, docs evidence. | `synergy-conflict-matrix.csv`, domain, balance, docs-risk, staff, preservation, objective outputs, `phase-09-summary.md`. | Reassess gameplay, balance, staff, and documentation relationships. |
| `New-RiskSpecificReviewTracks.ps1` | Phase 10 | Project truth, hook map, serializer register, dependency graph, system cards, synergy matrix, config/reference outputs, source scans. | `risk-track-findings.csv`, repair-backlog candidates, non-issues, accepted risks, comment-target additions, pooled enumerable review, `phase-10-summary.md`. | Generate risk-track findings for manual review. |
| `New-CommentTargetRegister.ps1` | Phase 11 | Comment candidates, serializers, hooks, dependencies, findings. | `comment-target-register.csv`, approved/rejected/source-comment edit outputs, `phase-11-summary.md`, `phase-11-verification-notes.md`. | Review candidate source comments and record which are approved, rejected, or deferred. |
| `New-ReorganizationDesign.ps1` | Phase 12 | System cards, dependency graph, serializer register, project truth, docs truth, risk findings, owner map. | `reorganization-design.*`, move proposals, keep-in-place decisions, namespace/save/project/docs plans, `phase-12-summary.md`. | Refresh design-only reorganization proposals. |
| `New-RepairBacklog.ps1` | Phase 13 | Project truth, system cards, hook map, serializer register, docs truth, dependency graph, synergy matrix, risk findings, comment decisions, move proposals. | `repair-backlog.csv`, accepted/deferred registers, batch plan, verification matrix, `phase-13-summary.md`. | Rebuild the historical audit backlog from phase evidence. |
| `New-FinalVerificationWorkflow.ps1` | Phase 14 | Phase status, run log, root plan, Phase 14 plan, canonical outputs, git metadata. | `phase-14-*` closeout outputs. | Rebuild final audit closeout evidence, not routine source-repair state. |

## Dependency Order

The generators are phase ordered. Later scripts usually require earlier outputs:

1. `Invoke-CodebaseAuditInventory.ps1`
2. `New-ProjectTruthRegister.ps1`
3. `New-CrossTreeRuntimeInventory.ps1`
4. `New-SystemCards.ps1`
5. `New-RuntimeHookMap.ps1`
6. `New-SerializationRegister.ps1`
7. `New-DocumentationTruthAudit.ps1`
8. `New-DependencyGraph.ps1`
9. `New-SynergyConflictMatrix.ps1`
10. `New-RiskSpecificReviewTracks.ps1`
11. `New-CommentTargetRegister.ps1`
12. `New-ReorganizationDesign.ps1`
13. `New-RepairBacklog.ps1`
14. `New-FinalVerificationWorkflow.ps1`

For a focused repair, run only the script that proves the touched risk area. For example, after a serializer repair, `New-SerializationRegister.ps1` is usually more appropriate than regenerating every phase.

## Important Cautions

- Generator output is evidence, not automatic approval to edit source.
- Marker-based outputs can over-report risk. Manual source review decides whether a row is a real issue.
- `New-RepairBacklog.ps1` rewrites the historical Phase 13 backlog. It does not include later post-audit overlay dispositions unless the script is intentionally updated to do so.
- `New-FinalVerificationWorkflow.ps1` is a closeout generator. It is not the right tool for normal post-audit repair batches.
- Reorganization proposals from `New-ReorganizationDesign.ps1` are design-only. Moves still require explicit save compatibility, runtime compile visibility, project update, docs update, verification, and rollback review.
- Do not use these scripts to paper over source changes. If source changed, run the source build and compile-only verification required by the root instructions.

## Useful Checks

Check which files a script changed:

```powershell
git diff --name-only -- docs/codebase-audit
```

Check for whitespace problems:

```powershell
git diff --check
```

Check whether a required input exists:

```powershell
Test-Path docs/codebase-audit/outputs/serialization-register.csv
```

Count rows in a generated CSV:

```powershell
(Import-Csv docs/codebase-audit/outputs/runtime-hook-map.csv).Count
```
