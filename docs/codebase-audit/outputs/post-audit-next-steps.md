# Post-Audit Next Steps

Generated: 2026-06-06T14:18:55.2116400-05:00

## Current State

The audit phase runner completed Phases 0 through 14 and the worktree was clean at the start of this post-audit batch. The post-audit live-runtime context commits supersede the original Phase 13 execution order:

- `db0eef4f docs: document live runtime script compile model`
- `9dce70de docs: record source build baseline`

`Server.csproj` Debug/x86 build passed in the source-build baseline. Runtime script inventory found 6,581 live-visible `.cs` files under `Data/Scripts`, excluding `bin` and `obj`.

The old full-startup smoke remains unsafe for this checkout because the server startup path compiles scripts, then loads `Saves`, invokes script `Initialize`, creates `MessagePump`, initializes `NetState`, and binds listeners. `compile-only-verification-baseline.md` now records a safe runtime script compile verification path.

## Backlog Position

The repair backlog remains the main implementation source of truth:

| Metric | Count |
| --- | ---: |
| Total backlog rows | 6,815 |
| P0 rows | 375 |
| P1 rows | 4,699 |
| P2 rows | 1,429 |
| P3 rows | 298 |
| P4 rows | 14 |

P0 runtime-risk categories are:

| Category | P0 count | Execution meaning |
| --- | ---: | --- |
| Save compatibility | 304 | Review before serializer edits or moves; migration approval required for layout changes. |
| Packet handlers | 17 | Critical network-facing handlers; smallest high-risk code review batch after compile verification. |
| Runtime hooks | 17 | High-risk startup/global hooks requiring guard and side-effect review. |
| PlayerMobile coupling | 8 | Shared save/runtime core coupling; review with serializer and hook context. |
| Project include drift | 29 | IDE/project hygiene only after live-runtime context; no longer first runtime blocker. |

## Superseded Historical Plan

`phase-13-batch-plan.csv` is preserved as historical audit evidence. Its first batch placed `Scripts.csproj` project drift before packet handlers and save compatibility because the live server startup compile model was not yet known. For implementation after `9dce70de`, use `post-audit-batch-plan.csv` instead.

## Immediate Implementation Sequence

Completed: `POST-BATCH-000` added and verified `.\ConficturaServer.exe -compileonly -nocache`.

Next:

1. Proceed to `POST-BATCH-A`: review the 17 P0 packet handler rows.
2. Use `Server.csproj` build plus `-compileonly -nocache` as the verification gate for source batches.
3. If compile-only reports future errors, repair those exact runtime compile blockers before backlog risk work.
4. After packet handlers, proceed to P0 save compatibility triage.

## Reorganization Status

Reorganization remains deferred. No source file move should be executed until runtime compile verification is available, save compatibility for affected types is reviewed, documentation/source traces are updated, and rollback is recorded.
