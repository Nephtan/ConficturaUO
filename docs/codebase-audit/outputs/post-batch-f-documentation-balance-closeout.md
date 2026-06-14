# POST-BATCH-F Documentation And Balance Closeout

Generated: 2026-06-13T19:48:57.0377992-05:00

## Summary

`POST-BATCH-F` reviewed all 540 scoped P2-P3 documentation and balance rows from `repair-backlog.csv`. The batch stayed documentation/audit-only: no `.cs`, `.xml`, `.cfg`, project, namespace, serialized type, save-version, or runtime file-location changes were made.

Nine wiki pages received objective documentation/source-trace cleanup where source evidence was clear: `AI_OVERHAUL_AUDIT.md`, `Apiculture_Beekeeping.md`, `Gardening_System.md`, `In_Game_Command_List.md`, `Player_Mobile_Core.md`, `Runic_Tools_Crafting.md`, `Shoppes_Vendors.md`, `Standard_Crafting.md`, and `wikibacklog.md`. `INDEX.md` now links the source-traced AI overhaul audit under technical references.

## Scope Counts

| Category | Count |
| --- | ---: |
| `Documentation contradictions` | 158 |
| `Economy and reward loops` | 26 |
| `Staff tooling` | 124 |
| `XML/config schemas` | 232 |

## Decision Counts

| Decision | Count |
| --- | ---: |
| `DeferredBalanceDecision` | 26 |
| `DeferredLegacyAuditTableCleanup` | 1 |
| `DeferredSourceTraceReview` | 130 |
| `DeferredSupportDocReview` | 2 |
| `FixedDocumentationTrace` | 20 |
| `FixedHistoricalPathReference` | 5 |
| `NeedsHumanDecision` | 32 |
| `QueuedSchemaDocumentation` | 232 |
| `QueuedSourceFollowUp` | 92 |

## Active Overlay Counts

| Active status | Count |
| --- | ---: |
| `DeferredBalanceDecision` | 26 |
| `Fixed` | 25 |
| `NeedsHumanDecision` | 32 |
| `QueuedSourceFollowUp` | 457 |

## Documentation Result

- Documentation truth audit after edits reports 106 canonical pages, 97 canonical pages still missing ## Source Trace, and 1 documentation row with missing source-file evidence.
- The remaining missing source-file evidence is isolated to `docs/wiki/SystemAudit.md`, whose legacy table shape/source-link cleanup is queued as `DeferredLegacyAuditTableCleanup`.
- The corrected docs use exact existing source paths instead of wildcard source references or multi-path command snippets that the audit parser treated as current missing source files.

## Balance And Staff/Config Result

- Balance/economy rows are recorded as `DeferredBalanceDecision`; no reward tables, economy source, config data, or gameplay code changed.
- Staff tooling rows are split between `NeedsHumanDecision` for relationship-level staff workflow/policy rows and `QueuedSourceFollowUp` for concrete command/access source rows.
- XML/config schema rows are `QueuedSchemaDocumentation`; source/config references were traced through the dependency graph, but no schemas were normalized or migrated.
- Regenerated Phase 9 outputs still report 26 balance-risk rows and 32 staff-dependency rows.

## Verification

- `git status --short` was clean before the batch.
- `rg --files -g AGENTS.md` was rerun; applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read.
- `New-DocumentationTruthAudit.ps1` regenerated Phase 7 documentation truth outputs after doc edits.
- `New-DependencyGraph.ps1` regenerated Phase 8 dependency outputs after source-trace changes.
- `New-SynergyConflictMatrix.ps1` regenerated Phase 9 balance/staff/documentation risk outputs after dependency evidence changed.
- Source/config/project verification was not run because this batch made no `.cs`, `.xml`, `.cfg`, `.csproj`, or runtime behavior changes.
- `git diff --check` passed before staging and commit with no whitespace errors.
- Final invariant checks found 540 POST-BATCH-F review rows, 540 POST-BATCH-F active overlay rows, no blank decisions, no blank verification fields, and no blank overlay actions.
