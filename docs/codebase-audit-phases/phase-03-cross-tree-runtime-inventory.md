# Phase 3: Cross-Tree Runtime Inventory

## Purpose

Phase 3 maps what every source file does at runtime. The audit must not assume
that `Data/Scripts/Custom` contains all custom behavior. Confictura behavior is
spread across `Custom`, `Items`, `Magic`, `Mobiles`, `Quests`, `System`, and
`Trades`.

## Required Inputs

- Phase 1 file inventory.
- Phase 2 project truth register.
- Runtime marker scans.
- Serialization marker scans.

## Required Outputs

- CrossTreeRuntimeInventory table.
- Runtime role summary by root folder.
- Unknown owner list.
- High-risk root summary.

## Subphase 3.1: Root Folder Role Summary

For each root under `Data/Scripts`, record:

- File count.
- Initialization file count.
- Command file count.
- Event hook file count.
- Packet hook file count.
- Serialization file count.
- Gump reference file count.

Root folders:

- `Custom`
- `Items`
- `Magic`
- `Mobiles`
- `Quests`
- `System`
- `Trades`

Completion gate:

- Reviewers understand which roots are persistence-heavy, hook-heavy, or
  content-heavy.

## Subphase 3.2: File-Level Runtime Role

Assign one primary role and any secondary roles:

- `Persistence`
- `StartupWiring`
- `CommandSurface`
- `PacketNetwork`
- `CombatPolicy`
- `WorldState`
- `PlayerProgression`
- `Economy`
- `Crafting`
- `StaffTooling`
- `GumpUI`
- `RegionPolicy`
- `MobileContent`
- `ItemContent`
- `LegacyCompatibility`
- `Unknown`

Output table:

| Path | PrimaryRole | SecondaryRoles | Evidence |
| --- | --- | --- | --- |

Completion gate:

- Every source file has at least a provisional role.

## Subphase 3.3: System Owner Assignment

Assign each file to a system owner using:

- Folder name.
- Namespace.
- Class names.
- Command names.
- Documentation links.
- Project path.
- Direct references from known systems.

Use `Unknown` when ownership is not proven.

Completion gate:

- Unknown files are tracked, not ignored.

## Subphase 3.4: Entry Point Extraction

For each file, record:

- Commands registered.
- `Initialize` methods.
- Gumps opened.
- Items/mobiles constructed by commands.
- NPC speech triggers.
- Movement triggers.
- World load/save hooks.
- Login/logout hooks.
- Packet handlers.
- Timers started.

Completion gate:

- Runtime behavior can be traced from startup or user action to handler method.

## Subphase 3.5: Config And Data Usage

Record references to:

- XML files.
- text files.
- version files.
- save-sidecar files.
- generated data.
- external URLs or vote links.

Completion gate:

- Systems depending on non-code data are visible.

## Subphase 3.6: Root-Level Risk Summary

Produce a summary for each root:

| Root | Main Role | Main Risk | First Follow-Up |
| --- | --- | --- | --- |

Example risk categories:

- `Items`: save compatibility.
- `Mobiles`: save compatibility and AI behavior.
- `System`: runtime wiring and global policy.
- `Magic`: registry and framework coupling.
- `Trades`: economy and pooled enumerable usage.
- `Custom`: imported package boundaries and mixed staff/player systems.

## Review Checklist

- Every root scanned.
- `Unknown` used honestly.
- Roles assigned from evidence.
- Runtime and persistence roles separated.
- Config files included.

## Exit Criteria

Phase 3 is complete when every source file has a runtime role, owner system or
unknown task, and evidence pointing to why it was classified that way.
