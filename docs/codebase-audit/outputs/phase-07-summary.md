# Phase 7 Documentation Truth Audit Summary

Generated: 2026-06-13T23:33:48.8857455-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Documentation inventory | Present: `phase-01-documentation-inventory.csv` with 205 rows |
| Wiki index | Present: `docs/wiki/INDEX.md` with 108 indexed links |
| Wiki backlog | Present: `docs/wiki/wikibacklog.md` with 17 rows |
| System Cards | Present: `phase-04-system-card-index.csv` with 27 rows |
| Runtime Hook Map | Present: `runtime-hook-map.csv` with 6605 rows |
| Serialization Register | Present: `serialization-register.csv` with 9158 rows |
| Project Truth Register | Present: `project-truth-register.csv` with 13162 rows |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `documentation-truth-table.csv` | 122 | Canonical documentation truth table. |
| `phase-07-documentation-truth-table.csv` | 122 | Phase-scoped truth table. |
| `phase-07-canonical-page-map.csv` | 106 | Canonical indexed page map with source trace and coverage status. |
| `phase-07-alias-legacy-slug-map.csv` | 2 | Alias and legacy slug map with canonical targets and independent-claim flags. |
| `phase-07-stale-claim-backlog.csv` | 109 | Generated documentation verification backlog candidates. |
| `phase-07-source-trace-coverage-report.csv` | 8 | Source trace coverage grouped by wiki index category. |

## Coverage Counts

| Metric | Count |
| --- | ---: |
| Canonical wiki pages | 106 |
| Canonical pages missing Source Trace | 97 |
| Alias or legacy slug pages | 2 |
| Alias pages with independent claims | 2 |
| Generated backlog candidates | 109 |

## Exit Criteria

- Wiki pages are classified as canonical, alias, support, audit register, or unknown.
- Canonical pages missing source trace, indexed pages with missing source evidence, aliases with independent claims, stale markers, and missing source paths are backlogged instead of edited in Phase 7.
- Runtime and serialization coverage are derived from traced source files and the Phase 5/6 registers.
- This phase does not modify wiki pages; fixes are deferred to the wiki backlog or Phase 13 repair backlog.
