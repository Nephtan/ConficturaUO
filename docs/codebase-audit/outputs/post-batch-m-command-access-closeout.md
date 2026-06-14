# POST-BATCH-M Command Access Source Review Closeout

Generated: 2026-06-14T13:15:22.7769319-05:00

## Summary

`POST-BATCH-M-01A` processed all 174 `QueuedSourceFollowUp` rows from POST-BATCH-L. The scoped rows are all P2 `Command access` findings and were reviewed against current source to resolve helper registrations, access variables, parser line offsets, legacy command surfaces, and uncertain command policy cases.

No runtime source, public API, namespace, serialized type name, save version, command name, command access level, handler, project file, XML/config file, runtime file location, or gameplay behavior was changed.

## Decision Counts

| Decision | Count |
| --- | ---: |
| `FalsePositive` | 2 |
| `IntentionalLegacy` | 11 |
| `NeedsHumanDecision` | 2 |
| `ReviewedNoChange` | 159 |

## Review Classes

| Review class | Count |
| --- | ---: |
| `PlayerSpellbarCommandSurface` | 67 |
| `XMLSpawnerStaffCommandSurface` | 32 |
| `XMLPointsCommandSurface` | 19 |
| `PassthroughHelperResolved` | 12 |
| `IntentionalLegacyCommandSurface` | 11 |
| `PlayerSpellCommandSurface` | 10 |
| `DirectCommandAccessResolved` | 8 |
| `AccessVariableResolved` | 7 |
| `CompatibilityWrapperResolved` | 2 |
| `GenericCommandFramework` | 2 |
| `ParserLineOffsetFalsePositive` | 2 |
| `ConfigDrivenCommandRehashPolicy` | 1 |
| `PlayerConsoleDumpPolicy` | 1 |

## Systems

| System | Count |
| --- | ---: |
| `System:Commands` | 80 |
| `XMLSpawner` | 53 |
| `Regions` | 13 |
| `Spell Framework` | 10 |
| `System:Misc` | 7 |
| `Custom:NPC Control` | 4 |
| `Clone Offline Player Characters` | 3 |
| `Items:Doors` | 2 |
| `Housing` | 1 |
| `System:Chat` | 1 |

## Human Decisions

| BacklogId | File | Command surface | Decision needed |
| --- | --- | --- | --- |
| `RB-04621` | `Data/Scripts/System/Misc/Captcha.cs` | `fonts; dumpFonts` | Decide whether `dumpFonts` should remain Player-access because it writes server-console output, or whether the captcha font tools should become staff-only. |
| `RB-04256` | `Data/Scripts/Custom/XMLSpawner/XmlSpawner2.cs` | `ChangeCommand` | Decide whether XMLSpawner `ChangeCommand` config may lower command access, must preserve current access, or should clamp to the stricter old/new access. |

## Verification

- `git status --short` was clean before the batch.
- Applicable root and `docs/codebase-audit/AGENTS.md` instructions were re-read; the root audit plan plus Phase 13 and Phase 14 plans were re-read for backlog and verification rules.
- Input scope check: `post-batch-l-p2-residual-backlog-review.csv` contains exactly 174 rows with `Decision=QueuedSourceFollowUp`; every scoped row is P2 `Command access`.
- Source review resolved all 174 rows without source edits: access variables, wrapper helper callsites, duplicate command names, parser line offsets, and legacy command surfaces are recorded in the CSV source evidence.
- `post-batch-m-command-access-source-review.csv` contains exactly 174 rows.
- `post-audit-active-backlog-status.csv` contains exactly 174 POST-BATCH-M rows and 0 remaining active POST-BATCH-L `QueuedSourceFollowUp` rows.
- Comparing all `repair-backlog.csv` rows to the active overlay leaves 0 unreviewed rows.
- Source build and runtime script compile were not run because this batch changed only audit artifacts; no runtime source, project file, XML/config file, serialized layout, command registration, or access level changed.
- git diff --check: Passed with expected LF-to-CRLF working-copy warnings and no whitespace errors

## Boundary

POST-BATCH-M closes POST-BATCH-L command-access source-review follow-ups. The two `NeedsHumanDecision` rows remain active policy decisions; no source command-access change is approved until those decisions are made.
