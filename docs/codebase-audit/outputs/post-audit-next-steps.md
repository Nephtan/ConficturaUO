# Post-Audit Next Steps

Generated: 2026-06-06T16:42:02.3829332-05:00

## Current State

The audit phase runner completed Phases 0 through 14 and the worktree was clean at the start of this post-audit batch. The post-audit live-runtime context commits supersede the original Phase 13 execution order:

- `db0eef4f docs: document live runtime script compile model`
- `9dce70de docs: record source build baseline`
- `09b7b7e5 feat: add compile-only script verification`
- `3259f43b fix: harden packet handler paths`

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

`repair-backlog.csv` is preserved as historical Phase 13 generated evidence. Active post-audit dispositions are tracked in `post-audit-active-backlog-status.csv` so completed rows can be reconciled without rewriting historical generation output.

## Superseded Historical Plan

`phase-13-batch-plan.csv` is preserved as historical audit evidence. Its first batch placed `Scripts.csproj` project drift before packet handlers and save compatibility because the live server startup compile model was not yet known. For implementation after `9dce70de`, use `post-audit-batch-plan.csv` instead.

## Immediate Implementation Sequence

Completed: `POST-BATCH-000` added and verified `.\ConficturaServer.exe -compileonly -nocache`.

Completed runtime-risk batch: `POST-BATCH-A` source-reviewed the 17 P0 packet handler rows in `post-batch-a-packet-handler-review.csv`. The batch applies only source-confirmed packet-handler fixes:

- XMLSpawner `UseReq` now preserves the core action throttle update and Counselor bypass threshold while keeping attachment hooks.
- XMLSpawner book content override now enforces writable, range, and accessibility checks before accepting page edits.
- Monopoly gump response fallback now restores the packet reader position before delegating unmatched responses to the previous/core handler.

Verification passed for this batch:

- `New-RuntimeHookMap.ps1` completed with 17 packet rows.
- `Server.csproj` Debug/x86 build passed.
- `.\ConficturaServer.exe -compileonly -nocache` exited 0 with compile-only success and no listener output.

Active backlog reconciliation:

- `post-audit-active-backlog-status.csv` maps `RB-03235` through `RB-03251` to the packet-handler review artifact.
- Active packet-handler disposition is 3 `Fixed` rows and 14 `ReviewedNoChange` rows.
- The canonical Phase 13 `repair-backlog.csv` remains unchanged as historical generated evidence.

Started: `POST-BATCH-B` P0 save compatibility triage in `post-batch-b-save-compatibility-triage.csv`.

- The triage file scopes all 304 P0 critical save-compatibility rows.
- Initial source-reviewed decisions cover the 19 `ServerCore` high-blast-radius rows.
- Current reviewed decisions are 7 `FalsePositive`, 6 `IntentionalLegacy`, and 6 `SafeNoChange`.
- The remaining 285 rows are queued for later source review and do not approve source edits.
- No serialized type name, namespace, field order, version, or file-location change is approved by this triage batch.

Next:

1. If compile-only reports future errors, repair those exact runtime compile blockers before backlog risk work.
2. Continue `POST-BATCH-B` with XMLSpawner and obsolete/system serializers, then cross-system persistence, `Custom:Mobiles`, and remaining P0 critical save rows.
3. Do not change serialized layout, type names, namespaces, or file locations without a migration plan and explicit approval.

## Reorganization Status

Reorganization remains deferred. No source file move should be executed until runtime compile verification is available, save compatibility for affected types is reviewed, documentation/source traces are updated, and rollback is recorded.
