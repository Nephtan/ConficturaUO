# Phase 9: Synergy And Conflict Matrix

## Purpose

Phase 9 evaluates what the dependency graph means for gameplay, maintenance,
balance, and documentation. Two systems can both work locally while still
conflicting globally. Two other systems may have useful synergy that should be
documented and preserved.

## Required Inputs

- Dependency Graph.
- System Cards.
- Documentation Truth Table.
- Runtime Hook Map.
- Known `Needs Rework` rows.
- Player-facing progression docs.

## Required Outputs

- Synergy and Conflict Matrix.
- Balance risk list.
- Documentation risk list.
- Staff-dependency list.
- Preservation notes for good synergies.

## Matrix Labels

- `NoLink`
- `Supports`
- `DependsOn`
- `Overrides`
- `Bypasses`
- `Duplicates`
- `Conflicts`
- `BalanceRisk`
- `DocRisk`
- `StaffDependency`

## Subphase 9.1: Domain Buckets

Group systems into domains:

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

Completion gate:

- Reviewers can evaluate related systems together.

## Subphase 9.2: Pairwise Review

For each important system pair:

1. Check dependency graph edges.
2. Check docs for player-facing claims.
3. Check whether one system bypasses another.
4. Check whether rewards or progression stack.
5. Check whether staff intervention is required.
6. Classify the relationship.

Completion gate:

- Matrix labels are evidence-based.

## Subphase 9.3: Synergy Notes

Record positive relationships:

- Character Level making Random Encounters objective-aware.
- Government giving civic goals to housing/economy systems.
- Homestead deepening non-combat economy.
- XMLSpawner supporting staff events and quests.
- Magic schools supporting varied build identity.

Completion gate:

- Good relationships are preserved during cleanup.

## Subphase 9.4: Conflict Notes

Record risky relationships:

- PvP Consent versus Government war and city-ban rules.
- XMLPoints event overrides versus normal PvP consent.
- Spell registry capacity versus high-ID magic families.
- Crafting/resource loops versus progression pacing.
- Staff-spawned events versus automated PvE systems.
- Documentation aliases versus canonical pages.

Completion gate:

- Conflicts become backlog items or accepted risks.

## Subphase 9.5: Balance Risk Review

For each gameplay system, ask:

- Does it trivialize progression?
- Does it bypass intended danger?
- Does it grant permanent power too quickly?
- Does it require weeks of grind without meaningful objectives?
- Does it depend on staff events to feel complete?
- Does the documentation set correct expectations?

Completion gate:

- Balance notes are separated from code correctness issues.

## Subphase 9.6: Player Objective Review

Record how each system creates or supports goals:

- immediate goals;
- medium-term goals;
- long-term goals;
- social/staff-driven goals;
- exploration goals;
- crafting/economy goals;
- progression goals.

Completion gate:

- Objective-driven player experience is explicitly documented.

## Review Checklist

- Pairwise labels have evidence.
- Positive synergies captured.
- Conflicts captured.
- Balance and correctness separated.
- Staff dependency visible.
- Documentation risk visible.

## Exit Criteria

Phase 9 is complete when the audit can explain not only how systems connect,
but whether those connections help, harm, duplicate, bypass, or confuse the
overall shard experience.
