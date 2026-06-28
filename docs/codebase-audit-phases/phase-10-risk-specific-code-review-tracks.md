# Phase 10: Risk-Specific Code Review Tracks

## Purpose

Phase 10 runs specialized review tracks across the codebase. General reading is
not enough: serialization, packet handlers, gumps, pooled enumerables, economy
loops, and legacy compatibility each fail in different ways.

## Required Inputs

- Project Truth Register.
- Runtime Hook Map.
- Serialization Register.
- Dependency Graph.
- System Cards.
- Synergy and Conflict Matrix.

## Required Outputs

- Risk track findings.
- Non-issue records.
- Repair backlog items.
- Accepted risk notes.
- Comment target additions.

## Finding Template

| Field | Meaning |
| --- | --- |
| `FindingId` | Stable ID. |
| `Track` | Risk track. |
| `System` | Owning system. |
| `File` | Source file. |
| `Evidence` | Line/symbol/behavior. |
| `Impact` | Build, save, runtime, player, staff, balance, docs. |
| `Severity` | P0-P4. |
| `Recommendation` | Specific fix. |
| `Verification` | Command or review needed. |

## Track 10.1: Build Inclusion Drift

Review:

- missing compile targets;
- unlisted source files;
- moved gumps;
- escaped path mismatches;
- obsolete includes.

Exit gate:

- Every project truth issue is fixed, backlogged, or accepted.

## Track 10.2: Serialization Order And Versioning

Review:

- missing version reads/writes;
- mismatched field order;
- unsafe removed fields;
- type rename risk;
- timers not restored;
- linked objects not repaired.

Exit gate:

- Save risks have explicit findings.

## Track 10.3: Global Hooks And Startup Side Effects

Review:

- `Initialize` methods;
- duplicate subscriptions;
- command overwrites;
- event hooks in obsolete code;
- side effects during startup.

Exit gate:

- Global behavior has owners and guard notes.

## Track 10.4: Packet Handlers

Review:

- packet IDs and lengths;
- malformed packet handling;
- access control;
- state validation;
- packet override conflicts.

Exit gate:

- Network entry points are explicitly reviewed.

## Track 10.5: Gump Response Validation

Review:

- null `NetState`;
- null `Mobile`;
- deleted targets;
- stale lists;
- out-of-range buttons;
- missing text entries;
- insufficient access checks.

Exit gate:

- High-risk gumps have response maps and findings.

## Track 10.6: Command Access And Input Validation

Review:

- access level;
- usage metadata;
- argument parsing;
- target validation;
- player/staff assumptions;
- side effects.

Exit gate:

- Commands are documented and guarded.

## Track 10.7: Pooled Enumerable Ownership

Review all range scans:

- `GetItemsInRange`;
- `GetMobilesInRange`;
- `GetClientsInRange`;
- `IPooledEnumerable`.

Check whether enumerables are freed in all paths.

Exit gate:

- Leaks are fixed or backlogged.

## Track 10.8: Region And Map Assumptions

Review:

- map-specific behavior;
- region overrides;
- guarded/town checks;
- travel restrictions;
- city regions;
- housing regions.

Exit gate:

- Region policy conflicts are known.

## Track 10.9: PlayerMobile Field Coupling

Review fields used by multiple systems:

- PvP consent.
- city membership.
- titles.
- skill/stat caps.
- character level inputs.
- housing/account state.
- insurance.

Exit gate:

- Shared player state is documented.

## Track 10.10: Economy And Reward Loops

Review:

- crafting outputs;
- vendor prices;
- taxes;
- treasury flows;
- resource generation;
- artifacts and reward drops;
- staff gift systems.

Exit gate:

- Balance-impacting loops are visible.

## Track 10.11: Staff Tooling

Review:

- staff-only commands;
- event tools;
- static gumps;
- spawner tools;
- character manipulation tools.

Exit gate:

- Staff tools are documented with access and risk.

## Track 10.12: Legacy Compatibility

Review:

- obsolete compiled files;
- save shims;
- old commands;
- old packet handlers;
- delete-on-load behavior.

Exit gate:

- Legacy code is either justified or backlogged.

## Track 10.13: XML And Config Schemas

Review:

- XML node parsing;
- ignored nodes;
- hot reload;
- invalid defaults;
- missing schema docs.

Exit gate:

- Data-driven systems have schema notes.

## Track 10.14: Documentation Contradictions

Review:

- source trace mismatches;
- stale paths;
- duplicate aliases;
- player-facing false claims.

Exit gate:

- Documentation contradictions are fixed or backlogged.

## Review Checklist

- Every risk track run.
- Findings have evidence.
- Non-issues recorded where useful.
- Backlog items have verification commands.
- Accepted risks are explicit.

## Exit Criteria

Phase 10 is complete when each risk class has been reviewed with findings,
non-issues, and follow-up tasks that can be independently verified.
