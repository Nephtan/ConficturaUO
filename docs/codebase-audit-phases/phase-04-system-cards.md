# Phase 4: System Cards

## Purpose

Phase 4 turns file inventories into maintainable system-level knowledge. A
system card is the canonical engineering summary for a feature, framework, or
staff tool.

## Required Inputs

- CrossTreeRuntimeInventory.
- Project Truth Register.
- Runtime Hook Map draft.
- Serialization Register draft.
- Existing wiki pages.
- `SystemAudit.md`.

## Required Outputs

- One system card per major system.
- System card backlog for unknown or partial systems.
- System owner map.
- High-risk system priority list.

## System Card Template

```markdown
# System: Name

## Classification

## Summary

## Source Files

## Data Files

## Player Entry Points

## Staff Entry Points

## Runtime Hooks

## Serialized State

## Dependencies

## Dependents

## Synergies

## Conflicts And Risks

## Documentation

## Verification Status

## Follow-Up Work
```

## Subphase 4.1: Seed High-Risk Cards

Create cards first for:

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
- Spell framework.
- Magic schools.
- Crafting core.
- Harvest system.
- Housing.
- Boats.
- Regions.
- PlayerMobile core.
- Vendor core.
- Obsolete scripts.

Completion gate:

- The riskiest systems have cards before low-risk content systems.

## Subphase 4.2: Classification Decision

Choose one primary classification:

- `Standalone`
- `SharedService`
- `GameplayLayer`
- `StaffEventTool`
- `ImportedPackage`
- `LegacyCompatibility`
- `BrokenOrStale`

Record why the classification was chosen. A standalone claim must prove absence
of global hooks, shared serialized state, and cross-system policy effects.

Completion gate:

- Classification is evidence-based.

## Subphase 4.3: Entry Point Documentation

For each system, record:

- Player commands.
- Staff commands.
- Items that open behavior.
- Mobiles/NPCs that trigger behavior.
- Gumps and response IDs.
- Speech keywords.
- Movement triggers.
- Region triggers.
- Timers.

Completion gate:

- A maintainer can answer how a user or staff member enters the system.

## Subphase 4.4: Persistence Documentation

Record:

- Serialized classes.
- Current version.
- Save ownership.
- Linked objects.
- Timers rebuilt after load.
- World sidecar files.
- Type-name move risk.

Completion gate:

- The system card points to the full Serialization Register rows.

## Subphase 4.5: Dependency Documentation

Record:

- Direct code dependencies.
- Data/config dependencies.
- Runtime hook dependencies.
- Documentation-only dependencies.
- Systems this system affects indirectly.

Completion gate:

- The Dependency Graph can be generated from system card data.

## Subphase 4.6: Verification Status

Use these statuses:

- `Verified`
- `PartiallyVerified`
- `StaleDocumentation`
- `BuildDrift`
- `NeedsSourceTrace`
- `NeedsRuntimeReview`
- `NeedsSerializationReview`
- `Blocked`

Completion gate:

- System cards do not overstate certainty.

## Review Checklist

- Each high-risk system has a card.
- Cards cite source files, not just docs.
- Entry points are explicit.
- Persistent state is explicit.
- Dependencies and dependents are separated.
- Follow-up work is actionable.

## Exit Criteria

Phase 4 is complete when all high-risk systems have cards that explain what the
system is, how it runs, how it saves, what it touches, and what remains unknown.
