# Phase 9 Synergy And Conflict Matrix Summary

Generated: 2026-06-13T19:43:24.1013002-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Dependency Graph | Present: `dependency-graph.csv` with 30211 rows |
| System Cards | Present: `phase-04-system-card-index.csv` with 27 rows |
| Documentation Truth Table | Present: `documentation-truth-table.csv` with 122 rows |
| Runtime Hook Map | Present: `runtime-hook-map.csv` with 6604 rows |
| Known Needs Rework Rows | Present: generated from docs markdown scan with 145 rows |
| Player-Facing Progression Docs | Present through documentation truth table and docs markdown scan; source-trace gaps are listed as documentation risks. |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `synergy-conflict-matrix.csv` | 351 | Canonical pairwise matrix for system-card systems. |
| `phase-09-synergy-conflict-matrix.csv` | 351 | Phase-scoped copy of the pairwise matrix. |
| `phase-09-domain-buckets.csv` | 27 | Domain grouping for Phase 9 review. |
| `phase-09-balance-risk-list.csv` | 26 | Gameplay, pacing, reward, and policy balance risks separated from code correctness. |
| `phase-09-documentation-risk-list.csv` | 141 | Source-trace, stale-claim, alias, and missing-path documentation risks. |
| `phase-09-staff-dependency-list.csv` | 32 | Relationships requiring staff tooling, staff events, or event override policy. |
| `phase-09-preservation-notes.csv` | 22 | Positive synergies to preserve during later cleanup. |
| `phase-09-player-objective-review.csv` | 27 | Immediate, medium-term, long-term, staff, exploration, crafting/economy, and progression goals by system. |

## Matrix Label Counts

| Label | Count |
| --- | ---: |
| BalanceRisk | 26 |
| Bypasses | 3 |
| Conflicts | 7 |
| DependsOn | 123 |
| DocRisk | 39 |
| Duplicates | 6 |
| NoLink | 205 |
| Overrides | 3 |
| StaffDependency | 32 |
| Supports | 22 |

## Required Spot Checks

| Pair | Expected Review | Generated Labels |
| --- | --- | --- |
| Character Level / Random Encounters | Character Level making Random Encounters objective-aware. | DependsOn;DocRisk;Supports |
| PvP Consent / Government | Consent versus government wars/bans/civic enforcement. | BalanceRisk;Conflicts;DocRisk;Overrides |
| PvP Consent / XMLSpawner | XMLPoints or staff event overrides versus consent policy. | BalanceRisk;Bypasses;Conflicts;DependsOn;DocRisk;StaffDependency |
| Spell Framework / Magic Schools | Spell registry capacity versus high-ID magic families. | BalanceRisk;DependsOn;DocRisk;Supports |
| Homestead / Crafting Core | Crafting/resource loops versus progression pacing. | BalanceRisk;DependsOn;DocRisk;Supports |
| XMLSpawner / Invasion | XMLSpawner supporting staff events and quests. | DocRisk;StaffDependency;Supports |

## Exit Criteria

- Pairwise labels have evidence from the dependency graph, documentation truth table, runtime hook map, or explicit Phase 9 checklist rules.
- Positive synergies are recorded in preservation notes.
- Conflicts and balance risks are listed separately from compiler, serializer, or runtime correctness issues.
- Staff dependencies and documentation risks are visible as focused registers.
- Player objective review records immediate, medium-term, long-term, staff, exploration, crafting/economy, and progression goals for every system-card system.
