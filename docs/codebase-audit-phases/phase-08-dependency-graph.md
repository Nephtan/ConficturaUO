# Phase 8: Dependency Graph

## Purpose

Phase 8 records how systems actually depend on each other. This graph is the
answer to whether systems are standalone, interconnected, synergistic,
conflicting, or only related by documentation.

## Required Inputs

- System Cards.
- CrossTreeRuntimeInventory.
- Runtime Hook Map.
- Serialization Register.
- Documentation Truth Table.
- Project Truth Register.

## Required Outputs

- Dependency Graph table.
- Hard dependency list.
- Soft dependency list.
- Conflict edge list.
- Standalone proof list.

## Dependency Edge Table

| Field | Meaning |
| --- | --- |
| `SourceSystem` | System that depends on or affects another. |
| `TargetSystem` | System used or affected. |
| `EdgeType` | Direct reference, global hook, serialization, project include, XML/config, docs-only, gameplay concept, override, duplicate, conflict. |
| `Evidence` | File, symbol, config, or doc source trace. |
| `Strength` | Hard, soft, or speculative. |
| `Impact` | Build, runtime, gameplay, save, docs, staff, player. |
| `Notes` | Explanation. |

## Subphase 8.1: Direct Source References

Find direct references between systems:

- class names;
- static service calls;
- enum usage;
- namespaces;
- constructors;
- type checks;
- casts.

Completion gate:

- Direct code dependencies are recorded before inferred dependencies.

## Subphase 8.2: Runtime Hook Edges

Use the Runtime Hook Map to add edges:

- global event hook to affected mobile/item/system;
- packet handler to network behavior;
- command to system;
- gump response to target object;
- region override to combat/travel/housing policy.

Completion gate:

- Global side effects become explicit graph edges.

## Subphase 8.3: Serialization Edges

Add edges where serialized fields reference:

- `Mobile`;
- `Item`;
- `Map`;
- region/controller objects;
- linked stones;
- linked gumps/controllers;
- lists of citizens, vendors, addons, attachments, or spawners.

Completion gate:

- Persistent object graphs are represented.

## Subphase 8.4: XML And Config Edges

Add edges from:

- XMLSpawner XML.
- Random encounter XML.
- government version files.
- book publisher XML.
- voting URLs/config.
- docs automation memory files.

Completion gate:

- Data-driven behavior appears in the graph.

## Subphase 8.5: Documentation-Only Edges

Mark docs-only edges separately. A wiki page can suggest a relationship, but it
does not prove a runtime dependency until source or data confirms it.

Completion gate:

- Speculative or docs-only links are not mistaken for hard dependencies.

## Subphase 8.6: Standalone Proof

To mark a system standalone, prove:

- no global hooks;
- no packet handlers;
- no shared serialized fields;
- no direct references from major systems;
- no shared combat/progression/economy policy;
- no required external config;
- no source-traced doc dependency.

Completion gate:

- Standalone status is earned by absence-of-edge evidence.

## Initial Required Graph Areas

- Character Level to Random Encounters.
- PvP Consent to PlayerMobile, Notoriety, Regions, XMLPoints, event gates, and
  harmful items.
- Government to PlayerMobile, Regions, Housing, Vendors, Banking, taxes, city
  mobiles, and city gumps.
- XMLSpawner to attachments, quests, packet handlers, speech/movement hooks,
  staff commands, and Random Encounter cleanup attachments.
- Spell framework to magic schools, spellbooks, spellbars, special moves, and
  item spells.
- Homestead to cooking, brewing, juicing, winecrafting, crops, hunger, and
  Trades.

## Review Checklist

- Hard and soft edges separated.
- Docs-only links labeled.
- Standalone claims proven.
- Persistent edges included.
- Config-driven edges included.

## Exit Criteria

Phase 8 is complete when every major system has dependency edges and every
standalone label is backed by explicit negative evidence.
