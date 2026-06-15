# POST-BATCH-AA Source Batch Roadmap Closeout

Reviewed at: 2026-06-15T12:40:26.6480047-05:00

## Summary

POST-BATCH-AA lays out the remaining logical source-change batches in sequential order using `post-audit-active-backlog-status.csv` as the source of truth. Historical Phase 13 rows are context only; they are not reopened unless the active overlay leaves them actionable.

Key conclusion: there is no unresolved active source-repair queue left. The remaining paths are either user-scoped non-gated source changes or conditional gated batches that require approval/design before source edits.

No source, project, XML/config/data, serializer, namespace, gameplay, staff workflow, or region behavior files changed in this batch.

## Roadmap Order

| Order | Batch | Status | Purpose |
| ---: | --- | --- | --- |
| 1 | `SOURCE-BATCH-001` | Immediate boundary selected by POST-BATCH-Z | First concrete non-gated focused source change. |
| 2 | `SOURCE-BATCH-002+` | Repeatable future boundary | Additional concrete non-gated focused source changes, one behavior/system per batch. |
| 3 | `GATED-SOURCE-BATCH-STAFF` | Conditional only | Staff workflow/access changes after explicit approval. |
| 4 | `GATED-SOURCE-BATCH-BALANCE` | Conditional only | Economy/reward tuning after gameplay/design approval. |
| 5 | `GATED-SOURCE-BATCH-REGION` | Conditional only | Region/map policy changes after policy approval. |
| 6 | `GATED-SOURCE-BATCH-HOUSEFOUNDATION` | Conditional only | HouseFoundation serializer migration or ordering changes after save-migration approval. |
| 7 | `GATED-SOURCE-BATCH-REORG` | Conditional only, last | Folder/namespace/package cleanup after reorganization approval and rollback planning. |

`SOURCE-BATCH-001` remains the only selected immediate source-change boundary. Every gated batch remains non-executable until its approval/design gate is crossed.

## Active Evidence

| Check | Result |
| --- | --- |
| Active overlay unresolved pre-source statuses | 0 |
| POST-BATCH-Y `AcceptedFence` rows | 83 |
| POST-BATCH-Y `BlocksOnlyThisDomain` rows | 7 |
| POST-BATCH-Y `BlocksSourceWork` rows | 0 |
| Staff tooling + command access fences | 34 rows |
| Economy and reward loop fences | 26 rows |
| Region and map assumption fences | 23 rows |
| HouseFoundation serializer fence | 1 row |
| Folder/namespace cleanup fences | 6 rows |

## Roadmap Rules

- Use `post-audit-active-backlog-status.csv` as the authoritative current backlog state.
- Do not reopen historical Phase 13 rows that the active overlay already fixed, documented, reviewed no-change, policy-resolved, or fenced.
- Use `post-batch-y-source-change-gate-register.csv` before every source batch.
- If a planned file appears in the gate register, the source batch must prove the edit does not touch that row's `BlockedChangeSurface`.
- Keep each source batch to one concrete behavior and one focused system/file set.
- Do not use a non-gated source batch for staff workflow/access changes, economy/reward tuning, region/map behavior changes, folder/namespace cleanup, package relocation, or HouseFoundation serializer migration.

## Goal Template - Non-Gated Source Batch

```text
/goal SOURCE-BATCH-00N Non-Gated Focused Source Change

Implement one concrete user-requested source-code change only if it satisfies the POST-BATCH-Y, POST-BATCH-Z, and POST-BATCH-AA source-boundary rules.

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every touched source path.
3. Read:
   - `docs/codebase-audit/outputs/post-batch-y-source-change-gate-register.csv`
   - `docs/codebase-audit/outputs/post-batch-z-first-source-change-selection-closeout.md`
   - `docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap-closeout.md`
4. Identify the exact requested behavior, system, and files. If the request is not concrete enough, stop before source edits and ask for a narrower target.
5. Preflight planned files and behavior against POST-BATCH-Y gates.

Allowed scope:
- One focused source-code behavior in one focused system/file set.
- Source maintenance, source repair, or feature work that does not cross a recorded blocked surface.

Excluded scope:
- staff workflow/access changes
- economy/reward tuning
- travel, spawning, PvP, housing, government, vendor, map, or region-precedence behavior changes
- folder moves, namespace/type renames, package relocation
- `HouseFoundation` serializer ordering or save migration changes
- project-file-only cleanup unless explicitly requested as project hygiene

Required verification:
- targeted source scan for the changed behavior
- serializer scan if persistence is touched
- hook/gump/command scan if runtime entry points are touched
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for runtime-visible script changes
- `git diff --check`

Exit criteria:
- The source change is implemented without crossing excluded fences.
- Required verification is complete or explicitly unavailable with evidence.
- Generated build artifacts are restored before staging.
- Only intended files are staged.
- The batch is committed.
```

## Goal Template - Staff/Access Gated Source Batch

```text
/goal GATED-SOURCE-BATCH-STAFF Staff Workflow And Access Source Change

Process a staff workflow/access source change only after explicit approval to cross the POST-BATCH-Y staff tooling or command-access fences.

Required approval evidence:
- approval must name the staff tool, command, workflow, access rule, or generated gump/tool behavior to change
- approval must state the intended new policy

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every touched source path.
3. Read `post-batch-y-source-change-gate-register.csv` and filter `Category in Staff tooling,Command access`.
4. Record the exact gate rows being crossed and why approval covers them.

Required verification:
- command/access scan
- gump/hook scan where applicable
- targeted source scan
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for runtime-visible script changes
- `git diff --check`

Exit criteria:
- Approved staff/access behavior is implemented with no unrelated workflow changes.
- Crossed gate rows and verification are recorded in the closeout.
- The batch is committed.
```

## Goal Template - Balance Gated Source Batch

```text
/goal GATED-SOURCE-BATCH-BALANCE Economy And Reward Source Tuning

Process economy/reward source tuning only after gameplay/design approval to cross specific POST-BATCH-Y economy and reward loop fences.

Required approval evidence:
- approval must name the relationship, system, reward path, progression incentive, economy loop, or paired-system tuning target
- approval must state the intended tuning policy

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every touched source path.
3. Read `post-batch-y-source-change-gate-register.csv` and filter `Category=Economy and reward loops`.
4. Record the exact gate rows being crossed and why approval covers them.

Required verification:
- source review of reward/economy paths
- affected-system documentation update if behavior changes
- gameplay/balance acceptance notes
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for runtime-visible script changes
- `git diff --check`

Exit criteria:
- Approved balance change is implemented without unrelated tuning.
- Crossed gate rows and verification are recorded in the closeout.
- The batch is committed.
```

## Goal Template - Region/Map Gated Source Batch

```text
/goal GATED-SOURCE-BATCH-REGION Region And Map Policy Source Change

Process region/map behavior changes only after policy approval to cross specific POST-BATCH-Y region and map assumption fences.

Required approval evidence:
- approval must name the travel, spawning, PvP, housing, government, vendor, map, region, or region-precedence behavior to change
- approval must state the intended policy or precedence change

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every touched source path.
3. Read `post-batch-y-source-change-gate-register.csv` and filter `Category=Region and map assumptions`.
4. Record the exact gate rows being crossed and why approval covers them.

Required verification:
- source review of region/map paths
- runtime hook or region scan where applicable
- affected documentation update if behavior changes
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for runtime-visible script changes
- `git diff --check`

Exit criteria:
- Approved region/map policy change is implemented without unrelated behavior drift.
- Crossed gate rows and verification are recorded in the closeout.
- The batch is committed.
```

## Goal Template - HouseFoundation Migration Batch

```text
/goal GATED-SOURCE-BATCH-HOUSEFOUNDATION HouseFoundation Serializer Migration

Process `HouseFoundation` serializer ordering or migration changes only after explicit save-migration approval.

Required approval evidence:
- approval must choose preserve-current subclass-before-base order, approved save conversion, or approved dual-format loader strategy
- approval must state old-save compatibility expectations

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for `Data/Scripts/Items/Houses/`.
3. Read the POST-BATCH-Y gate row for `RB-00724`.
4. Read the current `HouseFoundation` serializer source and record current write/read ordering before editing.

Required verification:
- before/after serializer map
- version/order compatibility proof
- migration or dual-format loader proof if changing format
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification
- save-load test where available, or explicit unavailable record
- `git diff --check`

Exit criteria:
- Approved serializer strategy is implemented without unrelated save-format changes.
- Old-save compatibility evidence is recorded.
- The batch is committed.
```

## Goal Template - Reorganization Batch

```text
/goal GATED-SOURCE-BATCH-REORG Approved Folder Namespace Package Cleanup

Process folder, namespace, or package cleanup only after explicit reorganization approval for named systems/files.

Required approval evidence:
- approval must name exact systems/files to move or reorganize
- approval must state namespace/type rename policy
- approval must include rollback expectations

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every source and target path.
3. Read `post-batch-y-source-change-gate-register.csv` and filter `Category=Folder and namespace cleanup`.
4. Record exact gate rows being crossed and why approval covers them.
5. Confirm save compatibility, runtime compile visibility, project truth, docs source traces, dependency edges, and rollback plan before editing.

Required verification:
- project truth rerun
- runtime compile visibility review
- serialization register review
- dependency graph check
- docs source-trace updates
- `Scripts.csproj` update when preserving IDE hygiene
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification
- rollback check
- `git diff --check`

Exit criteria:
- Approved moves or cleanup are implemented without unapproved namespace/type/save behavior changes.
- Project, docs, dependency, serialization, and rollback evidence are recorded.
- The batch is committed.
```

## POST-BATCH-AA Verification

- Active overlay unresolved pre-source statuses: 0
- POST-BATCH-Y gate reconciliation: `AcceptedFence=83`; `BlocksOnlyThisDomain=7`; `BlocksSourceWork=0`
- Roadmap marks only `SOURCE-BATCH-001` as the selected immediate source-change boundary.
- All gated categories are conditional only and not immediately executable.
- Changed-file scope is limited to `docs/codebase-audit` artifacts.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Source build and compile-only runtime verification are not required for POST-BATCH-AA because no source/project/XML/config/data behavior changed.

## Outputs

- `docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap.csv`
- `docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap-closeout.md`
