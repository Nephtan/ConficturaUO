# Agent Instructions for CCWM Documentation

This file applies to `docs/CCWM/` and all descendants. Follow the repository root instructions first, then use these CCWM-specific rules for this documentation project.

## Scope and Purpose

The CCWM folder maintains the Confictura Codex World Map simulated-player record. Treat `Confictura_Codex_World_Map.md` and `Simulation_State.yaml` as a paired narrative and mechanical state record: the map log explains what the player experiences, while the YAML preserves enough state and evidence for the next run to continue without inventing access, safety, inventory, UI, or world state.

Do not treat these files as a generic wiki. CCWM updates should preserve the internal-monologue style and the code-traced survival simulation described by `ccwm-autoprompt.md`.

## CCWM Run Workflow

Before any simulated CCWM run, read `ccwm-autoprompt.md` and load `Simulation_State.yaml`. If the state file is missing, initialize only from traced code paths. If it exists, load the complete account, character, and world-facing state.

Treat missing fields as unknown, not false. If an unknown can affect access, survival, movement, combat, inventory, social reaction, tarot availability, visible UI, open gumps, target cursors, or route choice, trace the relevant code or data before acting.

Keep `Simulation_State.yaml` authoritative at the end of each CCWM run. Preserve schema metadata, run number, last update time, source paths traced, random branches, unresolved branches, visual scans, AI pressure classification, unavailable-action gates, and enough evidence for the next run to reconstruct the same state.

## Player-Visible Simulation Rules

Simulate only what a normal player can do through visible game flow. Do not forge gump ButtonIDs, call destination helpers directly, invoke private helpers as player actions, assume hidden starts or tarot cards are available, or choose any route that the current character/account state would not expose.

Before choosing a movement, interaction, wait, gump response, context-menu action, or target action, account for the current visible client state: open gumps, context menus, target cursors, nearby mobiles/items, carried risks, follower position, terrain/statics, hunger/thirst or timer pressure, and any current blockers.

When writing the map log, record mechanical friction plainly: unavailable cards, missing discovery flags, position requirements, skill checks, inventory shortages, movement blockers, PvP rules, hunger/thirst pressure, combat risk, random branches, or unresolved live-only timers.

## Live State and Markers

Treat `live-state/manifest.yaml` plus the JSONL/YAML files it references as the canonical saved live-state snapshot until a manual refresh is explicitly requested. Do not re-export live state as routine automation, and do not hand-edit exported snapshot files unless the direct task explicitly asks for that.

Use `spawners.jsonl`, `mobiles.jsonl`, `items.jsonl`, and relevant `live-state/scans/*.yaml` entries before falling back to static spawn, region, decoration, or binary map data. A live `PremiumSpawner` with spawned refs represents saved entities already present in the snapshot, not merely a hypothetical definition.

Load `client-worldmap-markers/` as the player's visible world-map overlay. Marker CSV rows use `x,y,map,name,type_or_icon,color,zoom_or_layer`; filter by the current map and include the matching map file plus any matching `-Common` file. Markers are navigation knowledge, not in-world entities, discovery flags, region triggers, roads, shelter, safety, or proof that anything is inside the 18-tile update range.

For terrain and statics, resolve the current map through `Data/Scripts/System/Misc/MapDefinitions.cs` before reading binary map files. Use the server's map ID, map index, file index, and `TileMatrix` block order when decoding `map*.mul`, `staidx*.mul`, or `statics*.mul`.

## Data Editing Standards

Preserve file formats exactly: Markdown stays append-readable, YAML stays structured and parseable, JSONL remains one complete JSON object per line, and CSV marker rows keep their existing column order.

Make narrow, evidence-backed edits. Do not collapse repeated state fields into vague summaries when the next run needs them for access, survival, inventory, movement, UI, follower, or risk continuity.

When correcting a prior CCWM mistake, record both the correction and the reason: the code path, data source, live-state snapshot, marker filter, terrain decode, or movement/gump rule that invalidated the old conclusion.

## Verification

For instruction-only edits in this folder, verify with `git diff -- docs/CCWM/AGENTS.md`. No MSBuild run is required unless C# code changes are part of the task.

For CCWM simulation-run updates, verify that `Simulation_State.yaml` and `Confictura_Codex_World_Map.md` agree on the final location, run number, visible state, current UI, follower state, unresolved risks, and next-action pressure before finishing.
