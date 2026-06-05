# Phase 1: Reproducible Inventory Scripts

## Purpose

Phase 1 turns manual discovery into repeatable evidence. The audit cannot rely
on one-off searches because the codebase is large, project files are explicit,
and source paths include spaces, apostrophes, and bracketed package names.

The outcome is a set of documented commands or scripts that can regenerate the
same inventory tables from a clean checkout.

## Required Inputs

- Phase 0 baseline.
- `ConficturaUO.sln`.
- `Data/Scripts/Scripts.csproj`.
- `Data/System/Source/Server.csproj`.
- Script roots under `Data/Scripts`.
- Documentation roots under `docs`.

## Required Outputs

- Source file inventory.
- Project include inventory.
- Missing include target inventory.
- Unincluded source inventory.
- Runtime marker inventory.
- Serialization marker inventory.
- Documentation inventory.
- Command log explaining how each output was generated.

## Subphase 1.1: File Inventory Command Set

Build repeatable commands for:

1. All `.cs` source files excluding `bin` and `obj`.
2. All `.csproj` and `.sln` files.
3. All `.md` documentation files.
4. All XML/config files under script and data roots.
5. All nested `AGENTS.md` files.

Rules:

- Prefer `rg --files` for broad file discovery.
- Use `Get-ChildItem -LiteralPath` when paths may contain brackets.
- Exclude generated outputs unless the audit explicitly reviews them.

Completion gate:

- File counts can be regenerated and compared later.

## Subphase 1.2: Project Include Parser

Create a project parser that:

1. Loads `Scripts.csproj` as XML.
2. Reads every `Compile Include`.
3. URI-decodes escaped path segments such as `%27`.
4. Joins include paths against `Data/Scripts`.
5. Uses literal path checks.
6. Compares project entries with filesystem source files.

Output table:

| IncludePath | Exists | FilesystemMatch | Issue |
| --- | --- | --- | --- |

Completion gate:

- Missing targets and unlisted files are produced without wildcard false
  positives.

## Subphase 1.3: Runtime Marker Scans

Generate marker inventories for:

- `public static void Initialize`
- `CommandSystem.Register`
- `EventSink.`
- `PacketHandlers.Register`
- `Timer.DelayCall`
- custom `Timer` subclasses
- `OnSpeech`
- `OnMovement`
- `OnLogin`
- `OnLogout`
- `WorldSave`
- `WorldLoad`
- `OnResponse`
- region overrides

Output table:

| Marker | File | Line | LikelySystem | Notes |
| --- | --- | --- | --- | --- |

Completion gate:

- High-risk runtime entry points can be reviewed without re-searching manually.

## Subphase 1.4: Serialization Marker Scans

Generate marker inventories for:

- `Serialize(GenericWriter`
- `Deserialize(GenericReader`
- `Serial serial`
- `writer.Write`
- `reader.Read`
- version reads/writes

Output table:

| File | Type | SerializeLine | DeserializeLine | VersionPattern |
| --- | --- | --- | --- | --- |

Completion gate:

- The serialization review can prioritize high-density files and systems.

## Subphase 1.5: Documentation Marker Scans

Generate documentation inventories for:

- Markdown files.
- Wiki index entries.
- Source trace sections.
- `Code-Verified` markers.
- `Needs Rework` markers.
- legacy slug notes.
- duplicate normalized slugs.
- local markdown links.

Output table:

| DocPath | Indexed | SourceTrace | KnownIssues | CanonicalOrAlias |
| --- | --- | --- | --- | --- |

Completion gate:

- Documentation truth work starts from measurable coverage, not impressions.

## Subphase 1.6: Inventory Storage Policy

Decide where generated inventories live:

- Temporary command output can stay out of the repository.
- Curated audit outputs belong under a reviewed docs/audit folder.
- Large raw outputs should be summarized unless they are needed for automation.

Completion gate:

- Reviewers know which outputs are committed and which are regenerated on
  demand.

## Review Checklist

- Commands avoid wildcard bugs.
- Commands exclude `bin` and `obj`.
- Project parser handles URI-escaped paths.
- Runtime scans cover commands, events, packets, timers, gumps, and regions.
- Serialization scans cover both write and read sides.
- Documentation scans identify source-trace gaps.

## Exit Criteria

Phase 1 is complete when another maintainer can rerun the inventory commands
and reproduce the same categories of evidence without relying on chat history.
