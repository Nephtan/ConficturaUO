# SOURCE-BATCH-001 Target Intake

Reviewed at: 2026-06-15T15:45:07.1199481-05:00

## Target

Source batch: `SOURCE-BATCH-001 OilCloth Guard Repair`

Behavior to change: add stale/null/backpack guards to OilCloth interaction paths.

System: `Items:Misc / OilCloth`

Expected file:

- `Data/Scripts/Items/Misc/OilCloth.cs`

## Must Stay Unchanged

- poison-charge reduction behavior
- firebomb conversion behavior
- oil cloth consumption semantics
- localized messages
- targeting flow
- serialization layout and versioning
- region/map behavior
- economy/reward tuning
- staff/access behavior
- folder, namespace, type, and package layout

## Fence Result

POST-BATCH-Y gate preflight result: `OilCloth.cs` has zero gate hits.

No gated approval is crossed. The target is non-gated under the POST-BATCH-AA roadmap rules.

## Source Evidence

Current source file: `Data/Scripts/Items/Misc/OilCloth.cs`

Relevant current interaction paths:

- `OnDoubleClick(Mobile from)` begins targeting when the oil cloth is in the user's backpack.
- `OnTarget(Mobile from, object obj)` handles backpack checks, poison cleaning, liquor-to-firebomb conversion, already-firebomb feedback, and invalid-target feedback.
- `Serialize`/`Deserialize` are present and must not be changed by this batch.

## Ready Goal Command

```text
/goal SOURCE-BATCH-001 OilCloth Guard Repair

Implement the SOURCE-BATCH-001 target recorded in `docs/codebase-audit/outputs/source-batch-001-target.md`.

Required preflight:
1. Run `git status --short` and classify pre-existing changes.
2. Re-read applicable `AGENTS.md` files for `Data/Scripts/Items/Misc/OilCloth.cs`.
3. Read:
   - `docs/codebase-audit/outputs/source-batch-001-target.md`
   - `docs/codebase-audit/outputs/source-batch-intake-register.csv`
   - `docs/codebase-audit/outputs/post-batch-y-source-change-gate-register.csv`
   - `docs/codebase-audit/outputs/post-batch-aa-source-batch-roadmap-closeout.md`
4. Confirm `Data/Scripts/Items/Misc/OilCloth.cs` has zero POST-BATCH-Y gate hits.

Allowed scope:
- Add stale/null/backpack guards to OilCloth interaction paths.
- Keep edits limited to `Data/Scripts/Items/Misc/OilCloth.cs` unless direct source review proves a helper edit is required.

Must stay unchanged:
- poison-charge reduction behavior
- firebomb conversion behavior
- oil cloth consumption semantics
- localized messages
- targeting flow
- serialization layout and versioning
- region/map behavior
- economy/reward tuning
- staff/access behavior
- folder, namespace, type, and package layout

Required verification:
- targeted source scan for OilCloth interaction paths
- serializer scan confirming OilCloth serialization is unchanged
- hook/gump/command scan is not required unless implementation adds or touches runtime entry points outside existing item interaction overrides
- `Data/System/Source/Server.csproj` Debug/x86 build
- compile-only runtime verification for the runtime-visible script change
- `git diff --check`

Required closeout:
- summarize files changed and behavior changed
- record POST-BATCH-Y fence result
- record verification results and unavailable checks, if any
- restore generated build artifacts before staging
- stage only intended files
- commit with a focused Conventional Commit message

Exit criteria:
- OilCloth stale/null/backpack guard repair is implemented.
- No gated behavior is changed.
- Required verification is complete or explicitly unavailable with evidence.
- The batch is committed.
```

## Gated Decisions From Interview

- Staff/access: no approval; remains blocked pending explicit approval.
- Economy/reward tuning: no approval; remains blocked pending explicit approval.
- Region/map behavior: no approval; remains blocked pending explicit approval.
- `HouseFoundation` serializer migration: no approval; remains blocked pending explicit approval.
- Folder/namespace/package reorganization: no approval; remains blocked pending explicit approval.

## Intake Verification

- `Data/Scripts/Items/Misc/OilCloth.cs` POST-BATCH-Y gate hits: 0
- `SOURCE-BATCH-001` intake status: `ReadyForSourceBatch`
- Gated roadmap batches remain blocked pending explicit approval.
- No source/project/XML/config/data files changed in this intake batch.
- `git diff --check` passed with expected LF-to-CRLF warnings only.
