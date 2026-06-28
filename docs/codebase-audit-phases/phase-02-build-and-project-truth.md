# Phase 2: Build And Project Truth

## Purpose

Phase 2 reconciles what exists on disk with what the maintained Visual Studio
build actually compiles. This must happen before file moves or system-level
claims, because `Scripts.csproj` explicitly lists source files.

## Required Inputs

- Phase 1 project include parser.
- `ConficturaUO.sln`.
- `Data/Scripts/Scripts.csproj`.
- `Data/System/Source/Server.csproj`.
- Full source file inventory.

## Required Outputs

- Project Truth Register.
- Missing compile target list.
- Unincluded source list.
- Intentional non-compiled source list.
- Project cleanup backlog.
- Build verification notes.

## Subphase 2.1: Solution Configuration Review

1. Record all solution configurations and platforms.
2. Confirm how solution configurations map `Server` and `Scripts`.
3. Record which configurations build both projects.
4. Record configurations that only build the server.

Output table:

| Solution Config | Server Config | Scripts Config | Builds Both |
| --- | --- | --- | --- |

Completion gate:

- The build command chosen later matches the intended coverage.

## Subphase 2.2: Project Include Reconciliation

For each `Compile Include` in `Scripts.csproj`:

1. Decode the include path.
2. Check literal filesystem existence.
3. Identify moved folder patterns.
4. Identify stale gump path patterns.
5. Identify obsolete references.
6. Identify intentionally missing compatibility references, if any.

For each real `.cs` file under script roots:

1. Check whether it is included in `Scripts.csproj`.
2. Classify unlisted files as active, documentation sample, backup, old, or
   unknown.

Completion gate:

- Every missing target and unlisted source has a classification.

## Subphase 2.3: Common Drift Classes

Classify project drift with these labels:

- `MovedGumpFolder`
- `RenamedPackagePath`
- `EscapedPathMismatch`
- `LegacyObsoleteInclude`
- `ActiveSourceMissingInclude`
- `GeneratedOutput`
- `BackupOrOldSource`
- `IntentionalNotCompiled`
- `Unknown`

Completion gate:

- Drift patterns are visible enough to fix in batches.

## Subphase 2.4: Build Impact Triage

Prioritize issues:

1. Active source file not compiled.
2. Project includes missing file in active subsystem.
3. Stale include in high-risk subsystem.
4. Gump response file excluded from project.
5. Staff command excluded from project.
6. Documentation-only mismatch.

Output table:

| Priority | Path | System | Reason | Proposed Fix |
| --- | --- | --- | --- | --- |

Completion gate:

- The project cleanup backlog is ordered by runtime impact.

## Subphase 2.5: Safe Fix Plan

For each project-file repair:

1. Edit `Scripts.csproj` only for the targeted paths.
2. Avoid unrelated formatting churn.
3. Keep path style consistent with surrounding entries.
4. Re-run the project truth parser.
5. Run the narrowest build that covers the touched project.

Completion gate:

- No project repair is proposed without a verification command.

## Subphase 2.6: Build Verification

Use the narrowest relevant command:

```powershell
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

If MSBuild is unavailable:

- Record that build verification was unavailable.
- Record the static checks that were completed.
- Do not claim build success.

## Review Checklist

- Literal path checks used.
- URI-escaped path segments handled.
- `bin` and `obj` excluded.
- Unlisted files classified.
- Missing includes classified.
- Fixes are batched and verifiable.

## Exit Criteria

Phase 2 is complete when the project truth register explains every mismatch and
there is either a clean project/source match or an explicit backlog item for
every remaining discrepancy.
