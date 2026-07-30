# Codebase Systems Audit And Reorganization Plan

## Purpose

This plan defines the start-to-finish work required to pull apart the Confictura
codebase, document every meaningful game system, identify system boundaries,
map interconnections, record synergies and conflicts, and reorganize the shard
without breaking builds, saves, runtime hooks, or player-facing behavior.

The end product is not only cleaner folders. The end product is an
authoritative, source-verified understanding of the shard: what each system
does, where it lives, what it depends on, what depends on it, how it persists
state, what documentation is true, and where future maintainers need comments
in code to avoid unsafe edits.

Existing documentation is useful evidence, but it is not assumed to be true.
Every player-facing or maintainer-facing claim must be verified against current
source, project files, runtime hooks, serialized state, or checked-in data.

## Non-Negotiable Constraints

- Work on the current branch. Do not create a new branch for this audit.
- Preserve RunUO save compatibility. Do not move, rename, reorder, or change
  serialized types or fields without a migration plan.
- Treat `Scripts.csproj` as an explicit source list. New or moved scripts are
  not complete until project includes are correct.
- Treat docs as leads until they have source traces.
- Keep imported systems understandable. Do not normalize their style before
  understanding their local conventions and save behavior.
- Add code comments only where they prevent real maintenance mistakes.
- Verify with the narrowest relevant MSBuild target when code changes occur.

## Current Investigation Snapshot

These findings establish why the audit must be systematic rather than a simple
folder cleanup.

- The maintained build entry point is `ConficturaUO.sln`, containing the
  `Server` and `Scripts` projects. `Scripts` depends on `Server`.
- `Server.csproj` builds the .NET Framework 4.8 x86 executable
  `ConficturaServer`.
- `Scripts.csproj` builds the .NET Framework 4.8 library `ClassLibrary` and
  explicitly lists script source files.
- A project/source comparison found 6,571 `Compile` includes, 6,581 source
  files, 82 missing compile targets, and 92 real source files not included in
  `Scripts.csproj`.
- The script tree is not confined to `Data/Scripts/Custom`. Important runtime
  and persistence surfaces also live under `Items`, `Magic`, `Mobiles`,
  `Quests`, `System`, and `Trades`.
- Initial cross-tree marker counts show `Items` and `Mobiles` dominate
  serialization volume, while `System` owns much of the startup, command,
  event, and packet wiring.
- The wiki contains broad coverage, but only a small portion has explicit
  source-trace structure. Source traces must become mandatory.
- `SystemAudit.md` records many `Needs Rework` items. Those rows are triage
  inputs, not completed verification.
- Documentation aliases exist intentionally in some cases, such as
  `pvp-consent-system.md` preserving a legacy slug for
  `PvP_Consent_System.md`. Apparent duplicates must be classified before
  deletion or merging.

## Required Deliverables

### 1. Project Truth Register

One row per source file and one row per project include:

| Field | Meaning |
| --- | --- |
| `Path` | Repository-relative path. |
| `ExistsOnDisk` | Whether the path exists as a file. |
| `IncludedInScriptsProject` | Whether `Scripts.csproj` includes it. |
| `MissingCompileTarget` | Project entry points at a missing file. |
| `UnincludedSource` | Real source file absent from the project. |
| `GeneratedOrOutput` | Whether the file is under `bin`, `obj`, or generated output. |
| `LikelySystem` | System that owns the file. |
| `Action` | Keep, add include, remove stale include, move, or investigate. |

### 2. Cross-Tree Runtime Inventory

One row per `.cs` file, grouped by runtime role rather than only folder:

| Field | Meaning |
| --- | --- |
| `Path` | Source file. |
| `PrimaryRole` | Persistence, startup wiring, command, packet, combat policy, world state, progression, economy, staff tool, legacy, or documentation support. |
| `System` | Owning system or package. |
| `Namespace` | Declared namespace. |
| `Types` | Important declared types. |
| `Commands` | Registered commands. |
| `Hooks` | Event, packet, speech, movement, login, logout, world save/load, or timer hooks. |
| `SerializedTypes` | Serialized `Item`, `Mobile`, controller, addon, deed, or other object types. |
| `Gumps` | Gumps declared or opened. |
| `ConfigData` | XML, text, or other data files read or written. |

### 3. System Cards

Every meaningful system gets a card:

| Field | Meaning |
| --- | --- |
| `System` | Canonical system name. |
| `Classification` | Standalone, shared service, gameplay layer, staff/event tool, imported package, legacy compatibility, or broken/stale. |
| `Folders` | All folders touched by this system. |
| `PrimaryFiles` | Key scripts and data files. |
| `PlayerEntryPoints` | Commands, items, NPCs, gumps, speech, regions, or triggers available to players. |
| `StaffEntryPoints` | Commands, gumps, tools, or admin properties for staff. |
| `RuntimeHooks` | Global hooks, timers, packet handlers, world load/save, login/logout, movement, or speech. |
| `SerializedState` | Persistent objects and versioned state. |
| `Dependencies` | Systems this system needs. |
| `Dependents` | Systems that call into or rely on this system. |
| `KnownIssues` | Source-verified risks. |
| `Docs` | Canonical docs and aliases. |
| `VerificationStatus` | Verified, partial, stale, misleading, missing source trace, or blocked. |

### 4. Serialization Register

Every serialized type gets a row:

| Field | Meaning |
| --- | --- |
| `Class` | Serialized type. |
| `BaseType` | Parent type. |
| `File` | Source file. |
| `CurrentVersion` | Version integer written by current code. |
| `Writes` | Ordered list of values written after `base.Serialize`. |
| `Reads` | Ordered list of values read after `base.Deserialize`. |
| `OlderVersions` | Supported historical versions. |
| `RemovedFields` | Discarded or migrated fields. |
| `MoveRenameRisk` | Whether moving or renaming the type risks world-load failures. |
| `CommentNeeded` | Whether a source comment should warn maintainers. |

### 5. Runtime Hook Map

Every runtime entry point gets a row:

| Field | Meaning |
| --- | --- |
| `File` | Source file. |
| `System` | Owning system. |
| `HookType` | Initialize, command, event, packet, timer, speech, movement, login, logout, world save/load, or region override. |
| `Registration` | Registration call or override. |
| `Handler` | Method invoked. |
| `Scope` | Player, staff, global, map, region, item, mobile, or packet. |
| `Risk` | Low, medium, high, critical. |
| `CommentNeeded` | Whether the source needs an explanatory comment. |

### 6. Dependency Graph

Each edge must be source-verified:

| Field | Meaning |
| --- | --- |
| `SourceSystem` | System that depends on or interacts with another system. |
| `TargetSystem` | System being used or affected. |
| `EdgeType` | Direct reference, global hook, serialization, project include, XML/config, docs-only, gameplay concept, override, conflict, or duplicate. |
| `Evidence` | File, symbol, or data file proving the edge. |
| `Strength` | Hard, soft, or speculative. |
| `Impact` | Build, runtime, gameplay, docs, save, staff, or player impact. |

### 7. Synergy And Conflict Matrix

Each system pair receives one of these labels:

- `NoLink`: no verified interaction.
- `Supports`: one system reinforces another.
- `DependsOn`: one system cannot operate correctly without another.
- `Overrides`: one system bypasses normal rules of another.
- `Duplicates`: two systems implement overlapping behavior.
- `Conflicts`: behavior contradicts or undermines another system.
- `BalanceRisk`: progression, reward, or difficulty mismatch.
- `DocRisk`: docs disagree with source or with each other.
- `StaffDependency`: system requires staff action to feel complete.

### 8. Documentation Truth Table

Every documentation page gets a row:

| Field | Meaning |
| --- | --- |
| `DocPath` | Documentation file. |
| `CanonicalPage` | Whether this is canonical or an alias. |
| `LegacySlug` | Whether it intentionally preserves old links. |
| `SourceTracePresent` | Whether it lists source files and symbols reviewed. |
| `VerifiedFiles` | Source files checked. |
| `StaleClaims` | Claims contradicted by source. |
| `MissingClaims` | Important behavior not documented. |
| `KnownIssues` | Source-verified issues. |
| `IndexCategory` | Wiki index section. |
| `BacklogId` | Related wiki backlog item, if any. |

### 9. Comment Target Register

Every proposed source comment gets a row:

| Field | Meaning |
| --- | --- |
| `File` | Source file. |
| `Location` | Method, type, or field. |
| `CommentType` | Serialization, global hook, gameplay exception, dependency, XML schema, pooled enumerable, or staff-event assumption. |
| `Reason` | Maintenance mistake the comment prevents. |
| `DraftComment` | Exact proposed comment text. |

## System Classification Rules

### Standalone

A system is standalone only if all of these are true:

- No global hooks.
- No packet handlers.
- No shared serialized state.
- No external project include dependency beyond normal build inclusion.
- No shared combat, progression, economy, housing, region, or staff-event policy.
- No required XML/config loaded by another system.
- No docs that identify it as part of another system.

### Shared Service

A system is a shared service when many systems call into it or rely on its
global behavior. Examples to audit closely include:

- `PlayerMobile`.
- Notoriety and combat policy.
- Regions.
- Spell framework.
- XMLSpawner and attachments.
- Crafting and harvest frameworks.
- Vendor and banking helpers.
- Character level service.

### Gameplay Layer

Player-facing systems that create goals, progression, restrictions, rewards, or
repeatable play loops. Examples include Character Level, Random Encounters,
Government, Homestead, Champions, Monster Nests, Offline Skill Training,
Crafting, Magic schools, Housing, Boats, and PvP Consent.

### Staff/Event Tool

Systems primarily operated by staff, but which can heavily affect players.
Examples include XMLSpawner, Invasion System, Static Gump Tool, Pandora's Gift
Box, Staff Toolbar, Change Season, Character Swap, NPC Control, and event gates.

### Imported Package

Systems imported as packages or with visible upstream style differences. These
should be documented and contained before any style cleanup. Examples include
XMLSpawner, Homestead subpackages, Book Publisher, Staff Toolbar, Point Command,
and Access Level Stone.

### Legacy Compatibility

Code retained for old saves, old features, old commands, or compatibility. It
must be documented before removal. `System/Obsolete` must be audited especially
carefully because legacy code can still register commands or startup hooks.

## Initial High-Risk Systems

### Project File Drift

`Scripts.csproj` has stale includes and unlisted source files. Moved `Gumps`
paths are a recurring class of drift. This must be repaired before any broad
reorganization, because otherwise a build can silently exclude real code or
include stale paths.

### XMLSpawner

XMLSpawner is a high-risk shared service. It registers commands, packet
handlers, speech hooks, movement hooks, world save/load hooks, and attachment
persistence. It supports quests, points, attachments, spawners, staff tools, and
event workflows. Treat it as infrastructure, not as a self-contained script
folder.

### Spell Registry

`SpellRegistry` currently allocates a fixed type array of length 700 and ignores
registrations with IDs outside that range. The initializer registers spells at
IDs 700 and above. This is a concrete example of a source-verified issue where
documentation and code review must meet.

### Character Level And Random Encounters

Character Level is not standalone. Player level display and player random
encounter gating route through `CharacterLevelService`, while non-player mobile
level behavior remains legacy. Random Encounters also depends on its XML table,
timers, XML attachments, and cleanup behavior.

### PvP Consent

PvP Consent is not a gump-only system. It touches `PlayerMobile`, gumps, NPC
speech, event moongates, Notoriety, region harmful checks, beneficial action
checks, combat display, area damage items, and persistence. It also has a
canonical page and an intentional legacy slug page.

### Government

Government is a high-risk persistence and gameplay system. It owns player city
founding, regions, elections, mayors, citizens, taxes, treasuries, city vendors,
city bans, war/alliance lists, civic structures, and many gumps. It must be
audited as both a player objective system and a persistent controller system.

### Homestead And Trades

Homestead is very large and serialization-heavy. It also overlaps with cooking,
brewing, juicing, winecrafting, crops, hunger, agriculture, and trade systems.
Document its subpackages before trying to normalize layout.

### Items And Mobiles

The non-custom `Items` and `Mobiles` trees contain thousands of serialized
types. They are core save surfaces and cannot be treated as generic content.
Any reorganization must identify which systems each item and mobile belongs to.

### System Folder

The `System` tree has many initializers, commands, event hooks, packet hooks,
and framework files. It is the main runtime wiring layer and must be audited by
hook role, not just folder location.

## Phased Work Plan

Each phase below has a dedicated detailed plan file:

| Phase | Detailed Plan |
| --- | --- |
| Phase 0: Baseline And Guardrails | [phase-00-baseline-and-guardrails.md](docs/codebase-audit-phases/phase-00-baseline-and-guardrails.md) |
| Phase 1: Reproducible Inventory Scripts | [phase-01-reproducible-inventory-scripts.md](docs/codebase-audit-phases/phase-01-reproducible-inventory-scripts.md) |
| Phase 2: Build And Project Truth | [phase-02-build-and-project-truth.md](docs/codebase-audit-phases/phase-02-build-and-project-truth.md) |
| Phase 3: Cross-Tree Runtime Inventory | [phase-03-cross-tree-runtime-inventory.md](docs/codebase-audit-phases/phase-03-cross-tree-runtime-inventory.md) |
| Phase 4: System Cards | [phase-04-system-cards.md](docs/codebase-audit-phases/phase-04-system-cards.md) |
| Phase 5: Runtime Hook Map | [phase-05-runtime-hook-map.md](docs/codebase-audit-phases/phase-05-runtime-hook-map.md) |
| Phase 6: Serialization And Save Compatibility | [phase-06-serialization-and-save-compatibility.md](docs/codebase-audit-phases/phase-06-serialization-and-save-compatibility.md) |
| Phase 7: Documentation Truth Audit | [phase-07-documentation-truth-audit.md](docs/codebase-audit-phases/phase-07-documentation-truth-audit.md) |
| Phase 8: Dependency Graph | [phase-08-dependency-graph.md](docs/codebase-audit-phases/phase-08-dependency-graph.md) |
| Phase 9: Synergy And Conflict Matrix | [phase-09-synergy-and-conflict-matrix.md](docs/codebase-audit-phases/phase-09-synergy-and-conflict-matrix.md) |
| Phase 10: Risk-Specific Code Review Tracks | [phase-10-risk-specific-code-review-tracks.md](docs/codebase-audit-phases/phase-10-risk-specific-code-review-tracks.md) |
| Phase 11: Inline Code Documentation | [phase-11-inline-code-documentation.md](docs/codebase-audit-phases/phase-11-inline-code-documentation.md) |
| Phase 12: Reorganization Design | [phase-12-reorganization-design.md](docs/codebase-audit-phases/phase-12-reorganization-design.md) |
| Phase 13: Repair Backlog | [phase-13-repair-backlog.md](docs/codebase-audit-phases/phase-13-repair-backlog.md) |
| Phase 14: Verification And Commit Workflow | [phase-14-verification-and-commit-workflow.md](docs/codebase-audit-phases/phase-14-verification-and-commit-workflow.md) |

### Phase 0: Baseline And Guardrails

1. Capture `git status --short`.
2. List all `AGENTS.md` files and determine applicable scopes.
3. Record current branch and recent commits.
4. Confirm expected build tools: Visual Studio 2022 MSBuild and .NET Framework
   4.8 targeting pack.
5. Record the current solution/project structure.
6. Create an initial audit workspace under `docs/` or `docs/wiki/` only after
   confirming naming and scope.
7. Do not move source files in this phase.

Exit criteria:

- Clean or understood worktree.
- Instruction scopes recorded.
- Baseline commands captured.
- No source mutation except the audit plan itself.

### Phase 1: Reproducible Inventory Scripts

Create or document repeatable commands that generate:

- Source file list.
- Project include list.
- Missing compile targets.
- Unincluded source files.
- Namespace/type inventory.
- Command registrations.
- Event and packet hook registrations.
- Serialization overrides.
- Gump declarations and opens.
- XML/config file references.
- Documentation pages and index links.

Prefer `rg`, PowerShell XML parsing, and structured project parsing over ad hoc
manual lists.

Exit criteria:

- Inventory can be regenerated from a clean checkout.
- Output distinguishes source files from `bin` and `obj`.
- Path matching uses literal paths so bracketed package names are not treated as
  wildcards.

### Phase 2: Build And Project Truth

Parse `ConficturaUO.sln`, `Data/System/Source/Server.csproj`, and
`Data/Scripts/Scripts.csproj`.

Build the `Project Truth Register` and classify every discrepancy:

- Missing compile target.
- Real source file not included.
- Wrong moved `Gumps` path.
- Obsolete include.
- Generated output.
- Intentional non-compiled source.
- Documentation-only file.

Fix order:

1. Remove or correct stale project paths.
2. Add missing active source files.
3. Explicitly document intentional non-compiled files.
4. Re-run the project truth command.
5. Run the narrowest relevant MSBuild target.

Exit criteria:

- Project truth table reviewed.
- No unexplained active source files outside `Scripts.csproj`.
- No unexplained missing compile targets.

### Phase 3: Cross-Tree Runtime Inventory

Audit all script roots:

- `Data/Scripts/Custom`
- `Data/Scripts/Items`
- `Data/Scripts/Magic`
- `Data/Scripts/Mobiles`
- `Data/Scripts/Quests`
- `Data/Scripts/System`
- `Data/Scripts/Trades`

For each file, record runtime role, system owner, namespace, declared types,
entry points, hooks, gumps, serialization, and config usage.

Exit criteria:

- Every `.cs` file has an initial owner or `Unknown` marker.
- Every `Unknown` marker has a follow-up task.
- Every root folder has a role summary.

### Phase 4: System Cards

Create system cards for all player-facing, staff-facing, and framework systems.
Seed the first pass with:

- Character Level.
- Random Encounters.
- PvP Consent.
- Government.
- Homestead.
- XMLSpawner.
- Invasion.
- Champions.
- Monster Nests.
- Clone Offline Player Characters.
- Offline Skill Training.
- OmniAI.
- AI Overhaul.
- Staff Toolbar.
- Static Gump Tool.
- Spell framework.
- Magic schools.
- Crafting core.
- Harvest system.
- Bulk orders.
- Gardening.
- Housing.
- Boats.
- Regions.
- PlayerMobile core.
- Vendor core.
- Obsolete scripts.

Exit criteria:

- Each high-risk system has a card.
- Each card lists source files, docs, runtime hooks, serialized state, and
  dependencies.

### Phase 5: Runtime Hook Map

Extract and verify:

- `public static void Initialize`.
- `CommandSystem.Register`.
- `EventSink`.
- `PacketHandlers.Register`.
- Timers started from constructors or initialize methods.
- Speech handlers.
- Movement handlers.
- Login/logout hooks.
- World save/load hooks.
- Region overrides.
- Gump response paths.

For each hook, determine:

- Who can trigger it.
- Whether it is global.
- Whether it needs null, deleted, range, map, access, or bounds guards.
- Whether it is duplicated elsewhere.
- Whether docs mention it.

Exit criteria:

- Runtime hook map complete for high-risk systems.
- Global hooks have source comments where the reason is non-obvious.

### Phase 6: Serialization And Save Compatibility

Build the `Serialization Register`.

For every serialized type:

1. Confirm `Serial` constructor exists.
2. Confirm `base.Serialize` and `base.Deserialize` order.
3. Confirm version integer write/read.
4. Match every write with the corresponding read.
5. Record version gates.
6. Record deleted or discarded legacy fields.
7. Record type-name move/rename risk.
8. Add comments where future edits could corrupt saves.

Exit criteria:

- All high-risk serialized systems have reviewed save maps.
- No source move is approved without save compatibility review.
- Comments exist around complex or fragile serialization blocks.

### Phase 7: Documentation Truth Audit

For every wiki page:

1. Identify canonical page or alias.
2. Add or verify `Source Trace`.
3. Verify player-facing claims against source.
4. Verify staff-facing claims against commands and access levels.
5. Verify persistence claims against serializers.
6. Verify build/file claims against project truth.
7. Mark stale, misleading, partial, or verified claims.
8. Link to backlog item where work remains.

Canonical page template:

```markdown
# System Name

## Overview

## Source Trace

## Runtime Entry Points

## Serialized State

## Dependencies

## Synergies

## Conflicts And Risks

## Player Impact

## Staff Impact

## Verification Notes
```

Exit criteria:

- Every canonical page has a source trace.
- Alias pages clearly state their canonical target.
- Stale duplicate docs are merged or retired.

### Phase 8: Dependency Graph

Build system dependency edges using only verified evidence:

- Direct source references.
- Runtime hook registrations.
- Shared serialized fields.
- Project include relationships.
- XML/config file usage.
- Global policy hooks.
- Region or map behavior.
- Documentation that has source trace.

Initial required graph areas:

- Character Level to Random Encounters.
- PvP Consent to PlayerMobile, Notoriety, Regions, XMLPoints, event gates, and
  harmful items.
- Government to PlayerMobile, regions, housing, vendors, banking, taxes,
  city mobiles, and city gumps.
- XMLSpawner to attachments, quests, packet handlers, speech/movement hooks,
  staff commands, and Random Encounters cleanup attachments.
- Spell framework to magic schools, spellbooks, spellbars, special moves, and
  item spells.
- Homestead to cooking, brewing, juicing, winecrafting, crops, hunger, and
  trade systems.

Exit criteria:

- Every high-risk system has dependency edges.
- Standalone claims are proven, not assumed.

### Phase 9: Synergy And Conflict Matrix

Create a matrix for core gameplay domains:

- Progression.
- Combat.
- PvP.
- PvE.
- AI.
- Magic.
- Economy.
- Crafting.
- Housing.
- Travel.
- Government.
- Staff events.
- Documentation.

Initial examples to verify:

- Character Level supports Random Encounters by making dungeon pressure more
  objective-driven.
- Random Encounters, Champions, Invasions, Monster Nests, and Goliath monsters
  overlap in PvE escalation and reward pressure.
- PvP Consent overrides normal combat expectations and must be reconciled with
  Government wars, city bans, guilds, XMLPoints, pets, and region rules.
- Homestead and Trades support economy depth but can conflict with balance if
  production or resource loops bypass intended progression.
- XMLSpawner supports staff events but can obscure runtime behavior if spawns
  and attachments are not documented.
- Spell registry limits can silently disable high-ID spell families.

Exit criteria:

- Each major system has at least one synergy/conflict review.
- Balance risks are separated from code correctness risks.

### Phase 10: Risk-Specific Code Review Tracks

Run specialized review passes:

1. Build inclusion drift.
2. Serialization order and versioning.
3. Global hooks and startup side effects.
4. Packet handlers.
5. Gump response validation.
6. Command access and input validation.
7. Pooled enumerable ownership.
8. Region and map assumptions.
9. PlayerMobile field coupling.
10. Economy and reward loops.
11. Staff-only tooling.
12. Legacy compatibility.
13. XML/config schemas.
14. Documentation contradictions.

Exit criteria:

- Each track produces findings, non-issues, and repair tasks.
- No high-risk finding remains only in chat or memory.

### Phase 11: Inline Code Documentation

Add comments only where they prevent future errors. Good comment targets:

- Serialization version and field order.
- Why a global hook exists.
- Why a gameplay exception bypasses normal rules.
- Why a dependency crosses system boundaries.
- XML schema behavior that is not obvious from code.
- Pooled enumerable ownership.
- Staff-event assumptions.
- Legacy compatibility shims.

Avoid comments that restate straightforward C#.

Example comment types:

```csharp
// Save format: keep these reads in the same order as Serialize.
```

```csharp
// PvE event gates intentionally move players into NONPKinEvent; normal PvE
// combat protection does not apply until a return gate restores NONPK.
```

```csharp
// Random encounters use CharacterLevelService for players, but legacy mobile
// level math is preserved for spawned creatures and scale comparisons.
```

Exit criteria:

- Comment target register reviewed.
- Comments exist only where they explain non-obvious behavior or risk.

### Phase 12: Reorganization Design

Design the target layout before moving files.

Proposed top-level custom organization:

- `Data/Scripts/Custom/Core`
- `Data/Scripts/Custom/Progression`
- `Data/Scripts/Custom/PvE`
- `Data/Scripts/Custom/PvP`
- `Data/Scripts/Custom/Combat`
- `Data/Scripts/Custom/Magic`
- `Data/Scripts/Custom/Economy`
- `Data/Scripts/Custom/Crafting`
- `Data/Scripts/Custom/Housing`
- `Data/Scripts/Custom/Travel`
- `Data/Scripts/Custom/Government`
- `Data/Scripts/Custom/Events`
- `Data/Scripts/Custom/StaffTools`
- `Data/Scripts/Custom/Integrations`
- `Data/Scripts/Custom/Legacy`
- `Data/Scripts/Custom/ThirdParty`

Do not force every system into `Custom`. Some systems belong in existing roots
such as `Items`, `Mobiles`, `Magic`, `System`, or `Trades`. The reorganization
plan must define when to keep an existing root and when to move only custom
extensions.

Each proposed move requires:

1. Current path.
2. Target path.
3. Reason.
4. Project include update.
5. Namespace impact.
6. Save compatibility impact.
7. Documentation update.
8. Build verification command.
9. Rollback plan.

Exit criteria:

- Reorganization proposal reviewed before any file move.
- Third-party/imported packages are either preserved or isolated.
- Save-sensitive moves are explicitly approved.

### Phase 13: Repair Backlog

Create a prioritized backlog:

1. Project include drift.
2. Active source not compiled.
3. Missing compile targets.
4. Save compatibility risks.
5. Runtime hook risks.
6. Packet handler risks.
7. Gump guard risks.
8. Pooled enumerable leaks.
9. Documentation contradictions.
10. Stale aliases and duplicate docs.
11. Known `Needs Rework` system audit items.
12. Legacy code with active hooks.
13. Folder and namespace cleanup.
14. Inline comments.

Each item must include:

- ID.
- Priority.
- System.
- Files.
- Evidence.
- Risk.
- Proposed fix.
- Verification command.
- Status.

Exit criteria:

- Backlog is actionable.
- Every known high-risk finding has an item or an explicit accepted-risk note.

### Phase 14: Verification And Commit Workflow

For documentation-only batches:

1. Run link/path checks.
2. Re-run docs truth inventory.
3. Confirm no source files changed unintentionally.
4. Commit docs changes.

For code batches:

1. Run project truth check.
2. Run serialization/register checks for touched systems.
3. Run the narrowest relevant MSBuild target.
4. If shared frameworks changed, run full solution build.
5. Run `git status --short`.
6. Commit with a conventional message.

Expected build commands:

```powershell
msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"
msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform="Any CPU"
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
```

Exit criteria:

- Worktree clean except unrelated pre-existing changes.
- Commits are focused and descriptive.
- Verification output is recorded in the relevant audit or PR notes.

## Documentation Canonicalization Rules

Each system gets one canonical page. Alias pages may exist only when they
preserve existing links or old slugs.

Canonical pages must include:

- Overview.
- Source trace.
- Runtime entry points.
- Serialized state.
- Dependencies.
- Synergies.
- Conflicts.
- Known issues.
- Verification notes.

Alias pages must:

- Name the canonical page.
- Avoid independent behavior claims unless generated from the canonical page.
- Be listed as aliases in the docs truth table.

## Initial Manual Review Queue

1. `Scripts.csproj` path drift and unlisted files.
2. XMLSpawner packet, event, command, and save hooks.
3. Spell registry capacity and high-ID spell registrations.
4. Serialization-heavy systems: Homestead, Government, XMLSpawner, Invasion,
   Champions, custom Mobiles, Items, and Mobiles.
5. PvP Consent and Notoriety combat-policy intersections.
6. Random Encounters and Character Level gating.
7. Pooled enumerable ownership.
8. Stale docs without source trace.
9. Obsolete scripts that still register commands or hooks.
10. Gump response, null, and bounds validation.
11. Government taxes, city regions, and city serialization.
12. Homestead crop, cooking, brewing, juicing, and winecrafting boundaries.
13. Magic framework and spellbar interactions.
14. Housing, boats, and region interactions.

## Acceptance Criteria For The Full Audit Program

The codebase audit and reorganization program is complete only when all of the
following are true:

- The root plan exists and is committed.
- Every phase has a dedicated detailed plan file linked from this root plan.
- Project truth is documented and reproducible.
- Every `.cs` file has an owner system or explicit unknown task.
- Every high-risk system has a system card.
- Every serialized type in high-risk systems is registered.
- Runtime hooks are mapped.
- Dependency graph exists for all major systems.
- Synergy/conflict matrix exists for core gameplay domains.
- Canonical docs are source-traced.
- Alias docs are identified and intentional.
- Known stale docs are fixed or backlogged.
- Code comments exist for non-obvious serialization, global hook, dependency,
  gameplay exception, XML schema, and pooled enumerable risks.
- Reorganization proposal is reviewed before file moves.
- File moves update project includes and docs.
- Builds are run for code changes.
- Each batch ends with a clean or intentionally documented worktree.

## Definition Of Done For This Plan File

This file is complete when it is present at the repository root, committed, and
used as the controlling checklist for future audit and reorganization work. It
does not claim that the full audit has already been completed; it defines the
work required to complete it safely.
