# Phase 14: Verification And Commit Workflow

## Purpose

Phase 14 defines how audit and repair work is proven, staged, committed, and
reported. It keeps documentation-only work, project-file work, and code changes
from being verified with the wrong level of rigor.

## Required Inputs

- Backlog items.
- Changed files.
- Applicable `AGENTS.md` instructions.
- Relevant phase outputs.
- Build tool availability.

## Required Outputs

- Verification notes.
- Clean or explained worktree.
- Focused commits.
- Final status report.

## Subphase 14.1: Change Classification

Classify the batch:

- documentation-only;
- project-file only;
- source code;
- serialization-affecting code;
- reorganization/move;
- generated data;
- mixed.

Completion gate:

- Verification matches change risk.

## Subphase 14.2: Documentation-Only Verification

For docs-only changes:

1. Run markdown path/link checks where available.
2. Run `git diff --check`.
3. Review changed docs for stale paths.
4. Confirm no source files changed unintentionally.
5. Confirm citations or evidence notes where required.

Completion gate:

- Docs are internally consistent and whitespace-clean.

## Subphase 14.3: Project File Verification

For `.csproj` changes:

1. Re-run project truth parser.
2. Confirm intended missing/unlisted deltas changed.
3. Run narrow build for `Scripts.csproj` or `Server.csproj`.
4. Run full solution build when cross-project behavior changed.

Completion gate:

- Project includes and build agree.

## Subphase 14.4: Source Code Verification

For C# changes:

1. Run relevant static searches.
2. Run serializer checks if persistence touched.
3. Run hook scans if runtime wiring touched.
4. Run narrow MSBuild target.
5. Run broader build for shared frameworks.

Completion gate:

- Code compiles or unavailable build tools are explicitly reported.

## Subphase 14.5: Serialization-Affecting Verification

For serializer changes:

1. Compare write/read maps before and after.
2. Confirm version bump policy.
3. Confirm old versions still consume old fields.
4. Confirm defaults for old saves.
5. Add comments for fragile order.
6. Build.

Completion gate:

- Save compatibility is reviewed, not assumed.

## Subphase 14.6: Reorganization Verification

For file moves:

1. Confirm no namespace/type rename unless approved.
2. Update `Scripts.csproj`.
3. Update docs source traces.
4. Update dependency graph evidence.
5. Run project truth parser.
6. Build.

Completion gate:

- Moved files remain compiled and documented.

## Subphase 14.7: Git Hygiene

Before commit:

1. Run `git status --short`.
2. Review staged diff.
3. Ensure only intended files are staged.
4. Use a focused conventional commit message.
5. Do not amend history.

Completion gate:

- Commit contains only intended work.

## Subphase 14.8: Final Report

Final report must include:

- files changed;
- commit hash;
- verification performed;
- verification not performed and why;
- worktree status;
- any unrelated pre-existing changes.

Completion gate:

- Reviewer can understand exactly what changed and how it was checked.

## Verification Command Menu

Documentation:

```powershell
git diff --check
git status --short
```

Scripts build:

```powershell
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
```

Server build:

```powershell
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
```

Full solution:

```powershell
msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

## Review Checklist

- Change risk classified.
- Verification matches risk.
- Staged diff reviewed.
- Commit focused.
- Worktree clean or explained.
- Final report concise and cited.

## Exit Criteria

Phase 14 is complete when the work is committed, the verification performed is
appropriate to the change type, and the final report gives reviewers enough
evidence to trust the result.
