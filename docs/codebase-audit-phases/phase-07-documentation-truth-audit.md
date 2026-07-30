# Phase 7: Documentation Truth Audit

## Purpose

Phase 7 makes documentation trustworthy. Existing docs are evidence, not proof.
Every canonical page must be verified against current source, runtime hooks,
project files, serialized state, and checked-in data.

## Required Inputs

- Documentation inventory.
- Wiki index.
- Wiki backlog.
- System Cards.
- Runtime Hook Map.
- Serialization Register.
- Project Truth Register.

## Required Outputs

- Documentation Truth Table.
- Canonical page map.
- Alias/legacy slug map.
- Stale claim backlog.
- Source-trace coverage report.

## Documentation Truth Table

| Field | Meaning |
| --- | --- |
| `DocPath` | Markdown page. |
| `CanonicalPage` | Canonical, alias, stale duplicate, or unknown. |
| `IndexCategory` | Wiki index group. |
| `SourceTracePresent` | Yes/no. |
| `VerifiedSourceFiles` | Files reviewed. |
| `RuntimeHooksCovered` | Commands, events, packets, timers, gumps covered. |
| `SerializationCovered` | Persistent state documented. |
| `StaleClaims` | Claims contradicted by source. |
| `MissingClaims` | Important behavior omitted. |
| `BacklogId` | Existing or new backlog item. |

## Subphase 7.1: Canonical And Alias Classification

For every wiki page:

1. Normalize the slug.
2. Detect duplicate normalized slugs.
3. Decide canonical page.
4. Decide whether aliases are intentional.
5. Ensure aliases link to canonical pages.
6. Ensure aliases avoid independent behavior claims.

Completion gate:

- Duplicate docs are either merged, marked intentional, or backlogged.

## Subphase 7.2: Source Trace Standard

Every canonical page must include:

- exact source files reviewed;
- important symbols reviewed;
- commands and access levels;
- gumps and response IDs;
- serialized classes and versions;
- XML/config files;
- related docs.

Completion gate:

- No canonical system page lacks source trace.

## Subphase 7.3: Claim Verification

For each documentation claim:

1. Find source evidence.
2. Mark true, false, partial, stale, or unverified.
3. Replace vague claims with precise behavior.
4. Move implementation notes out of player-facing sections where needed.
5. Preserve useful old notes as history only when labeled.

Completion gate:

- Player-facing docs do not overpromise behavior.

## Subphase 7.4: Runtime Documentation

Verify docs mention:

- player commands;
- staff commands;
- command access levels;
- NPC speech;
- movement triggers;
- gump actions;
- region behavior;
- global hooks;
- timers.

Completion gate:

- A player or staff member can understand how to enter the system.

## Subphase 7.5: Persistence Documentation

Verify docs mention:

- serialized player fields;
- persistent items/mobiles/controllers;
- version numbers for key systems;
- reset behavior on login/logout;
- timers restored after deserialize;
- sidecar files.

Completion gate:

- Save-affecting behavior is documented.

## Subphase 7.6: Wiki Index Review

Review the index categories:

- Start Here.
- Player Commands And Account Tools.
- Combat, Magic, And Character Progression.
- Crafting, Harvesting, Trades, And Economy.
- World And Gameplay Systems.
- Staff And Administration.
- Technical And Engine Reference.

Completion gate:

- Canonical pages are linked in the right audience category.

## Review Checklist

- Canonical pages identified.
- Alias pages clear.
- Source traces present.
- Stale claims backlogged.
- Runtime entry points documented.
- Persistence documented.
- Index links reviewed.

## Exit Criteria

Phase 7 is complete when every important page is either source-verified,
explicitly partial, or backlogged with exact missing verification work.
