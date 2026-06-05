# Agent Instructions for Confictura RunUO Shard

This file is the root-level agent instruction file for this repository. It applies to the entire repository unless a more deeply nested `AGENTS.md` applies to a specific path.

Use these instructions for normal Confictura development and for the codebase systems audit/reorganization program. Direct user instructions still control the current task, but do not violate the safety, save-compatibility, project-file, or verification rules below.

## Primary Objective

Assist with development, maintenance, audit, documentation, and safe reorganization of the Confictura RunUO shard.

Common work includes:

- creating C# scripts for items, mobiles, gumps, commands, systems, staff tools, and gameplay features;
- refactoring or repairing existing code with minimal, source-verified changes;
- analyzing runtime behavior, project inclusion, serialization, hooks, documentation, and system boundaries;
- maintaining the audit artifacts that explain what the shard does and how it is safely changed.

## Repository Operating Rules

- Work on the current checked-out branch. Do not create a new branch for the audit or for normal tasks unless the user explicitly asks for one.
- Do not revert, overwrite, or clean up user changes unless the user explicitly requests it.
- Before editing, run or inspect `git status --short` when shell access is available. Classify pre-existing changes as user-owned, audit-related, generated/build output, or unknown.
- Stop before editing if unknown pre-existing changes overlap the files needed for the task.
- Prefer LF for committed text files you edit. This checkout currently has `core.autocrlf=true` and no `.gitattributes`, so Git may materialize CRLF in the working tree; avoid mass line-ending-only rewrites.
- Use `rg`/ripgrep for broad searches. Use literal path handling for paths containing spaces, apostrophes, brackets, or escaped characters.
- Do not make style-only rewrites, namespace churn, broad folder moves, or formatting-only diffs while performing an audit or targeted repair.

## Environment Setup

- **Framework:** .NET Framework 4.8.
- **IDE/Build Tools:** Use Visual Studio 2022 with .NET Framework build tools. Visual Studio Community 2022 is sufficient, but other Visual Studio 2022 editions should work.
- **Core Dependency:** `Data/Scripts/Scripts.csproj` depends on `Data/System/Source/Server.csproj`.
- **Maintained Build Entry Point:** `ConficturaUO.sln` is the maintained Visual Studio/MSBuild workflow.
- **Legacy Manual Compile Reference:** `Data/System/Source/README` may mention an old direct `csc` build producing `World.exe`. Treat that executable name as stale documentation. The maintained server artifact is `ConficturaServer.exe`.

## Known Build Workflows

Use the narrowest build that proves the touched change. If MSBuild is unavailable in the current environment, record that build verification was unavailable, record the static checks that were completed, and do not claim build success.

### Full solution builds

Run from a Visual Studio Developer PowerShell/Command Prompt:

```powershell
msbuild ConficturaUO.sln /p:Configuration=Debug /p:Platform="Any CPU"
msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform="Any CPU"
msbuild ConficturaUO.sln /p:Configuration=Release /p:Platform=x86
```

Notes:

- The solution includes `Data/System/Source/Server.csproj` and `Data/Scripts/Scripts.csproj`.
- `Scripts` depends on `Server`.
- In full solution configurations, the solution maps `Server` to `x86` and `Scripts` to `Any CPU`.
- `Debug|x86` currently builds only the `Server` project; do not use it as proof that scripts compile.

### Narrow project builds

```powershell
msbuild Data/System/Source/Server.csproj /p:Configuration=Debug /p:Platform=x86
msbuild Data/System/Source/Server.csproj /p:Configuration=Release /p:Platform=x86
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Debug /p:Platform=AnyCPU
msbuild Data/Scripts/Scripts.csproj /p:Configuration=Release /p:Platform=AnyCPU
```

Notes:

- Building `Scripts.csproj` also pulls in `Server.csproj` through the project reference.
- `Server.csproj` builds `ConficturaServer.exe`.
- `Scripts.csproj` builds `ClassLibrary.dll`.
- `Debug|x86` server output goes to the repository root.
- `Release|x86` server output goes to `Data/System/Source/bin/Release/`.
- `Debug|AnyCPU` scripts output goes to `Data/Scripts/`.
- `Release|AnyCPU` scripts output goes to `Data/Scripts/bin/Release/`.

## Project File Rules

`Data/Scripts/Scripts.csproj` is an explicit source list.

- Any new, moved, renamed, or restored `.cs` file under `Data/Scripts` is incomplete until `Scripts.csproj` has the correct `Compile Include` entry.
- Do not assume a `.cs` file is compiled merely because it exists on disk.
- Do not assume a `Compile Include` is valid merely because it exists in the project file.
- Preserve the surrounding project-file path style and ordering when editing includes.
- Use literal path checks, not wildcard interpretation, when paths contain bracketed package names.
- URI-decode escaped path segments such as `%27` when comparing project entries to the filesystem.
- Exclude `bin` and `obj` from source/project truth inventories unless a phase explicitly reviews generated outputs.
- For `.csproj` changes, re-run the project/source truth check and run the narrowest build covering the touched project.

## Source Roots In Audit Scope

The audit is cross-tree, not `Custom`-only. Include these roots when inventorying runtime behavior, ownership, serialization, docs, and project truth:

- `Data/Scripts/Custom`
- `Data/Scripts/Items`
- `Data/Scripts/Magic`
- `Data/Scripts/Mobiles`
- `Data/Scripts/Quests`
- `Data/Scripts/System`
- `Data/Scripts/Trades`
- `Data/System/Source`
- `docs`
- XML/config/data files read or written by scripts

## Coding Conventions

- **Style Source:** Follow the style already used by maintained Confictura scripts in `Data/Scripts/Custom/` before applying generic C# preferences. When extending an imported custom system, match the nearby local style even if it differs from Confictura-native code.
- **Naming:** New C# files and classes use PascalCase, for example `MyNewItem.cs`.
- **Namespaces:** New Confictura-native scripts should use `Server.Custom.Confictura` or a child namespace such as `Server.Custom.Confictura.Mobiles`. When extending existing RunUO or imported systems, match nearby namespaces such as `Server.Items`, `Server.Mobiles`, `Server.Commands`, or `Server.Engines.*`.
- **File Location:** Place new Confictura-native custom scripts under `Data/Scripts/Custom/` and organize them by function only when that matches the current plan. Do not force framework, save-sensitive, or imported systems into `Custom` during audit work.
- **Local Variables:** Prefer explicit local variable types over `var`, including collection declarations and temporary values. Use explicit casts or `as` assignments followed by null checks where that is the surrounding pattern.
- **Formatting:** Use Allman braces for namespaces, classes, methods, switches, and multi-line condition blocks. Keep single-line guard clauses only when the nearby file already uses that RunUO style.
- **Null Checks and Compatibility:** Use `== null` and `!= null` for null checks. Avoid newer null syntax such as `is null`, `is not null`, and null-conditional `?.` in shard scripts unless the nearby subsystem already depends on it.
- **Gumps:** Build custom gumps in their constructors with standard RunUO calls such as `AddPage`, `AddBackground`, `AddHtml`, `AddLabel`, `AddButton`, and related helpers. Prefer named button enums or clear button ID ranges. In `OnResponse(NetState sender, RelayInfo info)` or `OnResponse(NetState state, RelayInfo info)`, take `Mobile from = sender.Mobile` or `state.Mobile`, guard invalid state, then switch on `info.ButtonID`.
- **Commands, Events, and Network Handlers:** Register commands and event hooks from `Initialize`. Use named static handler methods such as `Thing_OnCommand`, include `[Usage]` and `[Description]` attributes where command files already do so, and guard invalid mobiles early. Packet handlers should receive `NetState` and `PacketReader`, read packet fields explicitly, validate state/access, and delegate shared behavior to helper methods when needed.
- **Comments:** Add comments only for non-obvious gameplay rules, serialization behavior, runtime hooks, cross-system dependencies, XML/config assumptions, pooled enumerable ownership, staff-event assumptions, or legacy compatibility. Avoid comments that merely restate straightforward C#.

## RunUO Serialization Standards

RunUO serialization is positional: the order and type of every `writer.Write(...)` call defines the save-file format, and `Deserialize` must read the same values in the same order with matching `reader.Read...()` methods. A mismatched read order can corrupt the world save by shifting every later value.

For every `Item`, `Mobile`, or other RunUO object that overrides serialization:

- Include the `Serial` constructor required for world loading:

  ```csharp
  public MyThing(Serial serial)
      : base(serial)
  {
  }
  ```

- In `Serialize(GenericWriter writer)`, call `base.Serialize(writer);` before writing this class's data.
- Immediately after the base call, write this class's version integer. New serializers start at version `0`.

  ```csharp
  public override void Serialize(GenericWriter writer)
  {
      base.Serialize(writer);

      writer.Write((int)0); // version

      writer.Write(m_Field);
  }
  ```

- In `Deserialize(GenericReader reader)`, call `base.Deserialize(reader);` before reading this class's data.
- Immediately after the base call, read the version integer.

  ```csharp
  public override void Deserialize(GenericReader reader)
  {
      base.Deserialize(reader);

      int version = reader.ReadInt();

      m_Field = reader.ReadString();
  }
  ```

- Write and read fields with matching types and in identical order. For example, `writer.Write((Mobile)m_Owner)` must be matched by `reader.ReadMobile()`, `writer.Write((bool)m_Enabled)` by `reader.ReadBool()`, and enum values written as `int` must be read with `reader.ReadInt()` and cast back to the enum.
- Do not reorder, remove, or change the type of existing serialized values.
- When adding serialized data to an existing class, increment the version number and add guarded deserialization for the new version.
- If a previously serialized field is removed, stop writing it only in a bumped version, but keep a conditional read for older versions to consume and discard the old value.
- For old versions that did not contain a new field, initialize a safe default through field initializers, constructors, or explicit `if (version < N)` fallback logic.
- Even classes with no custom fields should still write and read a version integer after the base call.
- Preserve the local versioning style. Prefer the established RunUO `switch`/`goto case` fall-through pattern where it already exists; preserve `if (version >= N)` gates where that is the local pattern.
- Do not move, rename, reorder, or change serialized classes, namespaces, fields, or read/write order without a migration plan and explicit approval.
- Moving a file without changing namespace/type is lower risk than renaming a type, but still requires project truth, serialization register review, docs update, and build verification.

Example versioned upgrade pattern:

```csharp
public override void Serialize(GenericWriter writer)
{
    base.Serialize(writer);

    writer.Write((int)1); // version

    writer.Write(m_NewField); // version 1
    writer.Write(m_OldField); // version 0
}

public override void Deserialize(GenericReader reader)
{
    base.Deserialize(reader);

    int version = reader.ReadInt();

    switch (version)
    {
        case 1:
        {
            m_NewField = reader.ReadString();
            goto case 0;
        }
        case 0:
        {
            m_OldField = reader.ReadInt();
            break;
        }
    }
}
```

## Runtime Hook And Entry Point Rules

Map and verify runtime entry points before broad edits or reorganization.

High-risk entry points include:

- `public static void Initialize`
- `CommandSystem.Register`
- `EventSink` subscriptions
- `PacketHandlers.Register`
- timers and `Timer.DelayCall`
- custom `Timer` subclasses
- `OnSpeech`
- `OnMovement`
- login/logout handlers
- world save/load handlers
- region overrides
- gump `OnResponse` paths

For new or changed entry points, record who can trigger the behavior, access level, handler method, null/deleted/map/range/access/bounds guards, affected system, and verification command.

## Documentation Rules

Documentation is evidence, not proof. Verify player-facing and maintainer-facing claims against current source, project files, runtime hooks, serialized state, or checked-in data.

- Canonical system pages must include source traces.
- Source traces should name exact files, important symbols, commands/access levels, gumps/response IDs, serialized classes/versions, XML/config files, related docs, and verification notes.
- Alias or legacy-slug pages may exist only to preserve links. They must point to the canonical page and avoid independent behavior claims.
- Stale, misleading, partial, or unverified claims must be fixed or backlogged with evidence.
- Do not delete apparent duplicate docs until canonical/alias status is classified.

## Codebase Audit Phase Runner

When the user asks to run or continue the codebase systems audit, reorganization audit, phase plan, or Codex `/goal` controller, operate as a deterministic phase runner.

### Controlling files

Read the root plan and detailed phase files before acting:

- `CODEBASE_SYSTEMS_AUDIT_AND_REORG_PLAN.md`
- `docs/codebase-audit-phases/phase-00-baseline-and-guardrails.md`
- `docs/codebase-audit-phases/phase-01-reproducible-inventory-scripts.md`
- `docs/codebase-audit-phases/phase-02-build-and-project-truth.md`
- `docs/codebase-audit-phases/phase-03-cross-tree-runtime-inventory.md`
- `docs/codebase-audit-phases/phase-04-system-cards.md`
- `docs/codebase-audit-phases/phase-05-runtime-hook-map.md`
- `docs/codebase-audit-phases/phase-06-serialization-and-save-compatibility.md`
- `docs/codebase-audit-phases/phase-07-documentation-truth-audit.md`
- `docs/codebase-audit-phases/phase-08-dependency-graph.md`
- `docs/codebase-audit-phases/phase-09-synergy-and-conflict-matrix.md`
- `docs/codebase-audit-phases/phase-10-risk-specific-code-review-tracks.md`
- `docs/codebase-audit-phases/phase-11-inline-code-documentation.md`
- `docs/codebase-audit-phases/phase-12-reorganization-design.md`
- `docs/codebase-audit-phases/phase-13-repair-backlog.md`
- `docs/codebase-audit-phases/phase-14-verification-and-commit-workflow.md`

If these files live elsewhere in the checked-out repository, locate them with `rg --files` and record the actual paths in the run log.

### Required persistent state

Do not rely on chat memory to decide what is complete. The repository files are the source of truth.

Maintain these files:

- `docs/codebase-audit/PHASE_STATUS.md`
- `docs/codebase-audit/RUN_LOG.md`
- `docs/codebase-audit/outputs/README.md`

Maintain phase outputs under `docs/codebase-audit/outputs/`, including these artifacts when their phase reaches them:

- `project-truth-register.*`
- `cross-tree-runtime-inventory.*`
- `system-cards/`
- `runtime-hook-map.*`
- `serialization-register.*`
- `documentation-truth-table.*`
- `dependency-graph.*`
- `synergy-conflict-matrix.*`
- `risk-track-findings.*`
- `comment-target-register.*`
- `reorganization-design.*`
- `repair-backlog.*`
- `accepted-risk-register.*`
- `verification-matrix.*`

### Phase status values

Use stable status values in `PHASE_STATUS.md`:

- `NotStarted`
- `InProgress`
- `Complete`
- `Committed`
- `Blocked`
- `Deferred`
- `Intentional`

A phase is not complete just because notes exist. It is complete only when its exit criteria are met, required outputs are written, verification is recorded, and the relevant batch is committed or explicitly classified.

### Required phase loop

For each phase:

1. Run `git status --short` and classify unrelated/pre-existing changes.
2. Re-check applicable `AGENTS.md` files with `rg --files -g AGENTS.md` before editing a new area.
3. Read the root audit plan and the detailed phase file.
4. Restate required inputs, required outputs, exit criteria, and blockers in the phase output or run log.
5. Confirm required inputs exist. If inputs are missing, create a blocker record instead of guessing.
6. Perform the smallest useful, verifiable batch.
7. Generate or update durable output files under `docs/codebase-audit/`.
8. Run the narrowest relevant verification.
9. Update `PHASE_STATUS.md`.
10. Update `RUN_LOG.md` with command, cwd, timestamp, result, affected phase, and output path.
11. Stage only intended files.
12. Commit focused batches with descriptive Conventional Commit-style messages when git is available and the user has not prohibited commits.
13. Advance to the next phase only when the current phase is `Complete`, `Committed`, `Intentional`, or `Blocked` with exact evidence.

### Phase gates

- **Phase 0:** Baseline only. No source reorganization. Capture worktree, branch, recent commits, instruction scopes, build surface, source roots, and high-risk boundaries.
- **Phase 1:** Inventory commands/scripts must be reproducible from a clean checkout. Prefer `rg`, PowerShell XML parsing, structured project parsing, and literal path checks.
- **Phase 2:** Project truth must explain every missing compile target and unlisted source before broad source moves. Project repairs require rerunning the truth parser and the narrowest relevant build.
- **Phase 3:** Every `.cs` file gets a provisional runtime role and system owner, or an explicit `Unknown` marker with follow-up.
- **Phase 4:** High-risk systems get system cards before low-risk content. Classification must be evidence-based; standalone status requires proof of no hooks, shared state, shared policy, or hard dependencies.
- **Phase 5:** Runtime hooks, commands, packet handlers, timers, gumps, regions, speech, movement, login/logout, and world save/load paths must be mapped before moving or normalizing code.
- **Phase 6:** No source move is approved without save compatibility review. Serialized types require write/read order, version handling, deleted fields, and move/rename risk review.
- **Phase 7:** Canonical docs require source traces. Alias pages must point to canonical docs and avoid independent claims.
- **Phase 8:** Dependency graph edges must be source-verified. Docs-only or speculative edges must be marked as such.
- **Phase 9:** Synergy/conflict labels must be evidence-based and distinguish code correctness from gameplay/balance/doc risk.
- **Phase 10:** Specialized risk tracks must produce findings, non-issues, accepted risks, or backlog items. No high-risk finding may remain only in chat.
- **Phase 11:** Add source comments only from reviewed comment targets where the comment prevents a real future maintenance mistake.
- **Phase 12:** Design reorganization before moving files. Every proposed move needs current path, target path, reason, namespace impact, save risk, project update, docs update, verification, and rollback.
- **Phase 13:** Convert findings into a prioritized repair backlog with ID, priority, status, system, category, files, evidence, risk, recommendation, verification, and notes.
- **Phase 14:** Verify and commit each batch according to risk. Documentation-only, project-file, source-code, serialization-affecting, and reorganization batches require different verification levels.

### Blocked criteria

Use `Blocked` only when one of these is true:

- a human design decision is required;
- a save migration policy is required;
- build tools are unavailable for a build-dependent gate;
- source evidence conflicts and cannot be resolved locally;
- required test data or checked-in data is missing;
- unknown user-owned worktree changes overlap required edits.

A blocker record must include the phase, exact file/path/symbol involved, evidence, decision needed, attempted checks, and next safe action.

### Subagent policy

Use subagents only when the user explicitly asks or when the active Codex environment supports them and the task is read-only analysis.

- Subagents may perform inventory/review tracks in parallel.
- Subagents must write findings to `docs/codebase-audit/outputs/subagent-findings/`.
- Subagents must not edit source files unless explicitly authorized for a narrow batch.
- The parent agent must consolidate findings into the phase outputs and remain responsible for final status, verification, and commits.

## Reorganization Rules

Do not treat folder cleanup as the end product. The end product is source-verified knowledge plus safe, reviewed organization.

- Do not move files before Phase 12 reorganization design and Phase 6 save compatibility review for affected serialized types.
- Do not rename namespaces or serialized classes without explicit migration approval.
- Do not force stable framework code out of `Items`, `Mobiles`, `Magic`, `System`, `Trades`, or `Quests` just to place everything under `Custom`.
- Imported packages may be isolated or documented without being restyled.
- Every move must update `Scripts.csproj`, source traces, dependency evidence, and verification notes.
- Every move requires a rollback plan.

Candidate custom organization, when Phase 12 approves a move:

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

## High-Risk Systems And Review Areas

Treat these as high-risk until source-verified:

- Project include drift in `Scripts.csproj`.
- XMLSpawner commands, packet handlers, speech hooks, movement hooks, world save/load hooks, and attachment persistence.
- Spell registry capacity and high-ID spell families.
- Character Level and Random Encounters coupling.
- PvP Consent interactions with `PlayerMobile`, Notoriety, regions, harmful checks, beneficial checks, combat display, event gates, and persistence.
- Government persistence, regions, elections, taxes, treasuries, city vendors, city bans, wars/alliances, civic structures, and gumps.
- Homestead, cooking, brewing, juicing, winecrafting, crops, hunger, agriculture, and Trades overlap.
- Non-custom `Items` and `Mobiles`, because they contain thousands of serialized save surfaces.
- The `System` tree, because it owns much startup, command, event, packet, and framework wiring.
- Obsolete or legacy files that still register commands or hooks.
- Gump response validation, null guards, deleted target guards, stale list guards, and bounds checks.
- Pooled enumerable ownership for `GetItemsInRange`, `GetMobilesInRange`, `GetClientsInRange`, and `IPooledEnumerable`.

## Testing And Verification

There is no single test command for the whole repository. Verification depends on the changed batch.

### Documentation-only changes

- Run markdown link/path checks when available.
- Run `git diff --check`.
- Confirm no source files changed unintentionally.
- Confirm source traces, stale-path updates, aliases, and evidence notes are correct.

### Project-file changes

- Re-run the project truth parser or equivalent project/source comparison.
- Confirm intended missing/unlisted deltas changed.
- Run the narrowest build covering the changed project.

### Source-code changes

- Run relevant static searches for the touched area.
- Run serializer checks if persistence was touched.
- Run hook scans if runtime wiring was touched.
- Run narrow MSBuild target.
- Run a broader solution build when shared framework behavior changed.

### Serialization-affecting changes

- Compare write/read maps before and after.
- Confirm version bump policy.
- Confirm old versions still consume old fields.
- Confirm defaults for old saves.
- Add comments for fragile order only when needed.
- Build.

### Reorganization/file moves

- Confirm no namespace/type rename unless explicitly approved.
- Update `Scripts.csproj`.
- Update docs source traces and dependency graph evidence.
- Re-run project truth.
- Build.

## Git And Version Control

When the task modifies or creates files and git is available:

1. Run `git status --short` before editing.
2. Keep unrelated pre-existing changes untouched.
3. Stage only intended files.
4. Review the staged diff.
5. Run the verification appropriate for the batch.
6. Commit focused batches. Use brief, descriptive Conventional Commit-style messages such as `docs: add audit phase runner instructions`.
7. Do not amend history.
8. Run `git status --short` after committing.
9. Final reports must mention commit hash, verification performed, verification unavailable and why, worktree status, and unrelated pre-existing changes.

Pre-commit enforcement note: this checkout currently has no active commit-blocking linter, formatter, or static-analysis hook. `.git/hooks` contains only disabled `.sample` hook templates, `core.hooksPath` is unset, and the repository does not contain `.gitattributes`, `.pre-commit-config.yaml`, or `.pre-commit-config.yml`. If a pre-commit hook later exists and fails, fix the reported issues and retry the commit.

## Citation Guidelines For Agent Reports

When reporting work based on file browsing or terminal output, include citations or evidence references when the user or task asks for them.

Preferred formats:

- File evidence: `?F:<file_path>†L<line_start>(-L<line_end>)??`
- Terminal evidence: `?<chunk_id>†L<line_start>(-L<line_end>)??`

Rules:

- Use repository-relative file paths.
- Line numbers are 1-indexed.
- Cite only directly relevant evidence.
- Prefer file citations for source/docs and terminal citations for command output.
- For audit artifacts, also record evidence paths and commands in `RUN_LOG.md` and the relevant phase output.

## AGENTS.md Interpretation Rules

- The rules in an `AGENTS.md` file apply to the entire directory tree where that file is located.
- Direct user instructions for the current task override `AGENTS.md` instructions.
- More deeply nested `AGENTS.md` files take precedence over parent files for files under their scope.
- For every modified file, obey the applicable `AGENTS.md` instructions for that file path.
- During Phase 0 and before later source edits, locate all nested `AGENTS.md` files and record their scope.
