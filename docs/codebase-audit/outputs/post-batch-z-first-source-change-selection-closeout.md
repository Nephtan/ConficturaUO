# POST-BATCH-Z First Source Change Selection Closeout

Reviewed at: 2026-06-15T12:30:09.7126988-05:00

## Summary

POST-BATCH-Z selects the first safe source-change boundary after POST-BATCH-Y. The active overlay has no unresolved pre-source statuses, and the POST-BATCH-Y gate register has no `BlocksSourceWork` rows.

No source, project, XML/config/data, serializer, namespace, gameplay, staff workflow, or region behavior files changed in this batch.

## Reconciliation

| Check | Result |
| --- | --- |
| Unresolved pre-source statuses | 0 |
| POST-BATCH-Y `AcceptedFence` rows | 83 |
| POST-BATCH-Y `BlocksOnlyThisDomain` rows | 7 |
| POST-BATCH-Y `BlocksSourceWork` rows | 0 |
| Phase 13 `Project include drift` active overlay rows | Fixed=61 |
| Phase 13 `Packet handlers` active overlay rows | Fixed=3; ReviewedNoChange=14 |

The historical Phase 13 batch order was reviewed. `BATCH-001` is not selected because it is IDE project hygiene and the active overlay records the project include drift rows as fixed. `BATCH-002` is not selected because the active overlay records packet-handler rows as already fixed or reviewed no-change.

## Selected Source-Change Boundary

Selected candidate: `SOURCE-BATCH-001 Non-Gated Focused Source Change`

Selection filter:

- One concrete user-requested source-code behavior in one focused system/file set.
- The planned edit must not cross any POST-BATCH-Y `BlockedChangeSurface`.
- If a planned file appears in `post-batch-y-source-change-gate-register.csv`, the source batch must prove the edit does not touch that row's blocked surface.
- The batch must not be used for broad cleanup, broad reorganization, or policy-sensitive behavior change.

Explicit excluded gate areas:

- staff workflow/access changes
- economy/reward tuning
- travel, spawning, PvP, housing, government, vendor, map, or region-precedence behavior changes
- folder moves, namespace/type renames, package relocation
- `HouseFoundation` serializer ordering or save migration changes

## Required Verification For SOURCE-BATCH-001

- `git status --short` before editing, with pre-existing changes classified.
- Applicable `AGENTS.md` files for the touched source paths.
- POST-BATCH-Y gate preflight for the selected files and behavior.
- Targeted source scan for the requested behavior.
- Serializer scan if persistence is touched.
- Hook/gump/command scan if runtime entry points are touched.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- Compile-only runtime verification for runtime-visible script changes.
- `git diff --check`.
- Restore generated build artifacts before staging.

## Ready-To-Run Goal Command

```text
/goal SOURCE-BATCH-001 First Non-Gated Source Change

Implement one concrete user-requested source-code change only if it satisfies the POST-BATCH-Y and POST-BATCH-Z first allowed source boundary.

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for every touched source path.
3. Read:
   - `docs/codebase-audit/outputs/post-batch-y-source-change-gate-register.csv`
   - `docs/codebase-audit/outputs/post-batch-y-source-change-readiness-closeout.md`
   - `docs/codebase-audit/outputs/post-batch-z-first-source-change-selection-closeout.md`
4. Identify the exact requested behavior, system, and files from the user's source-change request. If the request is not concrete enough to identify behavior and files, stop before source edits and ask for a narrower target.
5. Preflight the planned files and behavior against the POST-BATCH-Y gate register.

Allowed scope:
- One focused source-code behavior in one focused system/file set.
- Source maintenance, source repair, or feature work that does not cross any recorded POST-BATCH-Y blocked surface.

Excluded scope:
- staff workflow/access changes
- economy/reward tuning
- travel, spawning, PvP, housing, government, vendor, map, or region-precedence behavior changes
- folder moves, namespace/type renames, package relocation
- `HouseFoundation` serializer ordering or save migration changes
- project-file-only cleanup unless the user explicitly requests project hygiene

Required implementation rules:
- Keep edits minimal and source-verified.
- Do not make formatting-only rewrites or unrelated cleanup.
- Do not move, rename, reorder, or change serialized classes, namespaces, fields, or read/write order without explicit migration approval.
- If a planned file appears in the POST-BATCH-Y gate register, record why the edit does not touch that row's `BlockedChangeSurface`.

Required verification:
- targeted source scan for the changed behavior
- serializer scan if persistence is touched
- hook/gump/command scan if runtime entry points are touched
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for runtime-visible script changes
- `git diff --check`

Required closeout:
- summarize files changed and behavior changed
- record POST-BATCH-Y fence check result
- record verification results and unavailable checks, if any
- restore generated build artifacts before staging
- stage only intended files
- commit with a focused Conventional Commit message matching the source change

Exit criteria:
- The source change is implemented without crossing excluded fences.
- Required verification is complete or unavailable with exact evidence.
- No unrelated files are staged.
- The batch is committed.
```

## POST-BATCH-Z Verification

- Active overlay unresolved pre-source statuses: 0
- POST-BATCH-Y gate reconciliation: `AcceptedFence=83`; `BlocksOnlyThisDomain=7`; `BlocksSourceWork=0`
- Selected candidate does not cross a blocked surface because it is a selection filter that excludes all recorded blocked surfaces.
- Changed-file scope is limited to `docs/codebase-audit` artifacts.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
- Source build and compile-only runtime verification are not required for POST-BATCH-Z because no source/project/XML/config/data behavior changed.

## Outputs

- `docs/codebase-audit/outputs/post-batch-z-first-source-change-selection.csv`
- `docs/codebase-audit/outputs/post-batch-z-first-source-change-selection-closeout.md`
