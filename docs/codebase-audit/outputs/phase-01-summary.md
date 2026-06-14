# Phase 1 Reproducible Inventory Summary

Generated: 2026-06-13T20:35:32.1949725-05:00

## Required Inputs

| Input | Status |
| --- | --- |
| Phase 0 baseline | Present: docs/codebase-audit/outputs/phase-00-baseline.md |
| ConficturaUO.sln | Present |
| Data/Scripts/Scripts.csproj | Present and parsed as XML |
| Data/System/Source/Server.csproj | Present |
| Script roots under Data/Scripts | Present and scanned with literal paths |
| Documentation roots under docs | Present and scanned |

## Generated Outputs

| Output | Rows | Purpose |
| --- | ---: | --- |
| phase-01-source-files.csv | 6700 | All .cs source files under audit roots, excluding generated output folders. |
| phase-01-project-files.csv | 5 | .sln and .csproj files. |
| phase-01-config-files.csv | 190 | XML/config/data files under Data, excluding generated output folders. |
| phase-01-agents.csv | 40 | Instruction scope files. |
| phase-01-project-includes.csv | 6581 | Scripts.csproj compile includes decoded and resolved with literal path checks. |
| phase-01-missing-compile-targets.csv | 0 | Compile includes whose target file does not exist. |
| phase-01-unincluded-source-files.csv | 0 | Real script .cs files absent from Scripts.csproj. |
| phase-01-namespace-type-inventory.csv | 6700 | Namespace and declared type markers. |
| phase-01-runtime-marker-inventory.csv | 6605 | Runtime entry marker hits for commands, hooks, timers, gumps, and regions. |
| phase-01-command-registration-inventory.csv | 499 | Command registration marker hits. |
| phase-01-event-packet-hook-inventory.csv | 1228 | Event, packet, timer, and world hook marker hits. |
| phase-01-serialization-marker-inventory.csv | 5342 | Serializer-related marker summary by file. |
| phase-01-gump-inventory.csv | 4066 | Gump open and response marker hits. |
| phase-01-config-reference-inventory.csv | 114 | String references to XML/config/text/json data files in source. |
| phase-01-documentation-inventory.csv | 199 | Markdown inventory with source-trace, code-verified, needs-rework, slug, and link markers. |
| phase-01-duplicate-doc-slugs.csv | 9 | Duplicate normalized documentation slugs. |

## Reproduction Command

Run from the repository root:

```powershell
.\docs\codebase-audit\tools\Invoke-CodebaseAuditInventory.ps1
```

The script uses PowerShell XML parsing for `Scripts.csproj`, `Test-Path -LiteralPath` for include existence checks, and generated-output filtering for `bin`, `obj`, `.vs`, `Logs`, `Saves`, `Backups`, and `__pycache__` path segments.

## Storage Policy

The script is committed under `docs/codebase-audit/tools/`. Curated Phase 1 inventories are committed under `docs/codebase-audit/outputs/` even though that directory is ignored by local Git exclude rules, so they must be force-staged intentionally.

## Exit Criteria

- Inventory can be regenerated from a clean checkout with one script command.
- Source files are distinguished from generated `bin` and `obj` outputs.
- Project include matching uses URI decoding and literal path checks, including paths with spaces, apostrophes, and brackets.
- Runtime, serialization, gump, command, documentation, and config reference marker inventories exist for later phases.
