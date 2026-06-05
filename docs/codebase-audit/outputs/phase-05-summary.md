# Phase 5 Runtime Hook Map Summary

Generated: 2026-06-05T17:07:19.2703856-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Runtime marker scans | Present: `phase-01-runtime-marker-inventory.csv` |
| CrossTreeRuntimeInventory | Present: `cross-tree-runtime-inventory.csv` |
| System Cards | Present: `outputs/system-cards/` and `system-owner-map.csv` |
| Project Truth Register | Present: `project-truth-register.csv` |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| `runtime-hook-map.csv` | 6604 | Canonical runtime hook map. |
| `phase-05-runtime-hook-map.csv` | 6604 | Phase-scoped hook map. |
| `phase-05-global-hook-risk-list.csv` | 2979 | Global, high-risk, and critical hook rows. |
| `phase-05-command-surface-register.csv` | 499 | Command registration surface with access and duplicate counts. |
| `phase-05-packet-handler-register.csv` | 17 | Packet handler rows treated as critical network entry points. |
| `phase-05-gump-response-risk-register.csv` | 4066 | Gump send/response rows with guard-review flags. |
| `phase-05-timer-world-hook-register.csv` | 1043 | Timer and world save/load hook rows. |

## Hook Type Counts

| Hook Type | Count |
| --- | ---: |
| Command | 499 |
| Event | 168 |
| Gump | 4066 |
| Initialize | 427 |
| Login | 14 |
| Logout | 6 |
| Movement | 172 |
| Packet | 17 |
| Region | 89 |
| Speech | 103 |
| Timer | 1004 |
| WorldLoad | 16 |
| WorldSave | 23 |

## Risk Counts

| Risk | Count |
| --- | ---: |
| Critical | 17 |
| High | 1531 |
| Medium | 5056 |

## Exit Criteria

- Runtime triggers are mapped to system, file, hook type, trigger, access, guard evidence, risk, and documentation target.
- Commands, packet handlers, gump paths, global hooks, timers, and world hooks have focused registers.
- Guard fields are conservative marker scans; rows marked `NeedsSourceReview` require manual code review before repair.
- Source comments are deferred to Phase 11 comment targets rather than added in this phase.
