# Post-Audit Next Steps

Generated: 2026-06-06T22:44:00.7275262-05:00

## Current State

The audit phase runner completed Phases 0 through 14 and the worktree was clean at the start of this post-audit batch. The post-audit live-runtime context commits supersede the original Phase 13 execution order:

- `db0eef4f docs: document live runtime script compile model`
- `9dce70de docs: record source build baseline`
- `09b7b7e5 feat: add compile-only script verification`
- `3259f43b fix: harden packet handler paths`
- `ef8d35b9 docs: reconcile post-audit backlog state`
- `5396b5d1 docs: triage XMLSpawner save compatibility`
- `c69dc894 docs: triage remaining XMLSpawner serializers`
- `54e10f06 docs: triage obsolete serializers`
- `53ea0b48 docs: triage civilized mobile serializers`
- `bf8673c1 docs: triage construct mobile serializers`
- `9ea68a00 docs: triage cultist mobile serializers`
- `7eed1dcc docs: triage daemon mobile serializers`
- `3d5aca5a docs: triage dragon mobile serializers`
- `48fc844d docs: triage elemental mobile serializers`
- `9107fc1f docs: triage goliath mobile serializers`
- `236faf68 docs: triage kobold mobile serializers`
- `60639ced docs: triage minotaur mobile serializers`

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
- `post-audit-active-backlog-status.csv` also maps 97 reviewed save-compatibility rows from `POST-BATCH-B-02A`, `POST-BATCH-B-02B`, `POST-BATCH-B-02C`, and `POST-BATCH-B-03A` through `POST-BATCH-B-03J` to the save triage artifact.
- The canonical Phase 13 `repair-backlog.csv` remains unchanged as historical generated evidence.

Started: `POST-BATCH-B` P0 save compatibility triage in `post-batch-b-save-compatibility-triage.csv`.

- The triage file scopes all 304 P0 critical save-compatibility rows.
- Source-reviewed decisions cover the 19 `ServerCore` high-blast-radius rows, 10 `POST-BATCH-B-02A` XMLSpawner central persistence rows, 20 `POST-BATCH-B-02B` remaining XMLSpawner serializer rows, 28 `POST-BATCH-B-02C` `System:Obsolete` rows, and 39 `POST-BATCH-B-03*` `Custom:Mobiles` rows.
- Current reviewed decisions are 25 `FalsePositive`, 16 `IntentionalLegacy`, and 75 `SafeNoChange`.
- The remaining 188 rows are queued for later source review and do not approve source edits.
- No serialized type name, namespace, field order, version, or file-location change is approved by this triage batch.

Completed review-only subbatch: `POST-BATCH-B-02A` reviewed XMLSpawner central persistence rows in `BaseXmlSpawner.cs`, `XmlAttachment.cs`, `XmlSpawner2.cs`, and `XmlQuestPoints.cs`.

- 10 rows were reviewed with no source edits.
- 6 rows were classified `FalsePositive` where the generated risk came from helper/interface serializers with no applicable base serializer requirement.
- 4 rows were classified `SafeNoChange` where source review confirmed current write/read alignment.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-02B` reviewed the remaining XMLSpawner attachment, item, mobile, challenge, and quest serializer rows.

- 20 rows were reviewed with no source edits.
- 15 rows were classified `SafeNoChange` where source review confirmed current write/read alignment.
- 5 rows were classified `IntentionalLegacy` where source review confirmed old save shapes are consumed but no longer written by the current format.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-02C` reviewed `System:Obsolete` serializers in `Data/Scripts/System/Obsolete/Obsolete.cs`.

- 28 rows were reviewed with no source edits.
- 12 rows were classified `FalsePositive` where source review confirmed helper/static serializers with no applicable base serializer requirement.
- 11 rows were classified `SafeNoChange` where source review confirmed current write/read/version alignment.
- 5 rows were classified `IntentionalLegacy` where source review confirmed old save shapes are consumed but no longer written by the current format.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03A` reviewed `Custom:Mobiles/Civilized` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03B` reviewed `Custom:Mobiles/Constructs` serializers.

- 5 rows were reviewed with no source edits.
- 5 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03C` reviewed `Custom:Mobiles/Cultists` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03D` reviewed `Custom:Mobiles/Daemons` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03E` reviewed `Custom:Mobiles/Dragons` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03F` reviewed `Custom:Mobiles/Elementals` serializers.

- 7 rows were reviewed with no source edits.
- 7 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03G` reviewed `Custom:Mobiles/Goliaths` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03H` reviewed `Custom:Mobiles/Kobolds` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03I` reviewed `Custom:Mobiles/Minotaurs` serializers.

- 3 rows were reviewed with no source edits.
- 3 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Completed review-only subbatch: `POST-BATCH-B-03J` reviewed `Custom:Mobiles/Mystical` serializers.

- 4 rows were reviewed with no source edits.
- 4 rows were classified `SafeNoChange` where source review confirmed base calls, version write/read alignment, and no custom serialized fields.
- No row was classified `ConfirmedIssue`, `NeedsMigrationPlan`, or `NeedsHumanDecision`.

Next:

1. If compile-only reports future errors, repair those exact runtime compile blockers before backlog risk work.
2. Continue `POST-BATCH-B` with the remaining `Custom:Mobiles` rows, keeping review-only commits scoped by folder or small system group.
3. Do not change serialized layout, type names, namespaces, or file locations without a migration plan and explicit approval.

## Reorganization Status

Reorganization remains deferred. No source file move should be executed until runtime compile verification is available, save compatibility for affected types is reviewed, documentation/source traces are updated, and rollback is recorded.
