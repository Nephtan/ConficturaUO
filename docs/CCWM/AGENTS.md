# Agent Instructions for CCWM Documentation

This file applies to `docs/CCWM/` and all descendants. Follow the repository root instructions first, then use these CCWM-specific rules for this documentation project.

## Scope and Purpose

The CCWM folder maintains the Confictura Codex World Map simulated-player record. Treat `Confictura_Codex_World_Map.md` as the historical play log and `Simulation_State.yaml` as the current playable state needed to continue the next run.

Do not treat these files as a generic wiki. Future CCWM runs should read like a human player using the client: visible screen, world-map overlay, inventory, status, open UI, and remembered player knowledge first. Server code is diagnostic evidence only when client-visible play hits a shard-specific wall.

## CCWM Run Workflow

Before any simulated CCWM run, read `ccwm-autoprompt.md` and load `Simulation_State.yaml`. If the state file is missing, initialize only the minimum current playable state needed to begin from visible client flow.

Treat missing fields as unknown, not false. Unknowns block play only when a normal player could not continue without resolving them, such as an open gump, target cursor, dead/alive status, inventory availability, or a visible blocker. Do not stop normal travel to resolve private server timers, ambient creature wandering, or exact pet pathing.

Keep `Simulation_State.yaml` authoritative for the next run, but keep it lean. Preserve current location, facing, vitals, inventory, open UI, follower order/presence, visible entities, relevant markers, known terrain blockers, route intent, and any recent client-visible friction. Do not preserve old blocker policies or historical trace lists as active instructions.

## Player-Visible Simulation Rules

Simulate only what a normal player can do through visible game flow. Do not forge gump ButtonIDs, call destination helpers directly, invoke private helpers as player actions, assume hidden starts or tarot cards are available, or choose any route that the current character/account state would not expose.

Use client-side heuristics for ordinary play:

- Hostile or dangerous mobile within 0-10 tiles: evade, fight, defend, or stop for a real player decision.
- Hostile or dangerous mobile within 11-18 tiles: route away, inspect cautiously, or stop if the next movement would close distance.
- Passive, off-screen, or ambient creatures: carry as route context, not active blockers.
- Pets ordered to `Follow`: assume practical rubber-banding during travel. Track exact pet coordinates only for combat, body-blocking, stuck/pathing failures, ownership/order changes, or direct pet interactions.

C# tracing is a diagnostic fallback, not the player’s native perception. Use it for custom mechanics, failed or gated gumps, hidden access gates, unexpected custom-script behavior, or shard-specific systems that the client cannot explain. Do not trace routine movement, standard combat swings, normal ambient creature wandering, or ordinary client corrections like walking into a tree, shoreline, mountain, or visible wall.

When writing the map log, record mechanical friction plainly: unavailable cards, missing discovery flags, position requirements, skill checks, inventory shortages, visible movement blockers, PvP rules, hunger/thirst pressure, combat risk, failed custom interactions, or unresolved client-visible choices.

## Live State and Markers

Treat `live-state/manifest.yaml` plus the JSONL/YAML files it references as the canonical saved live-state snapshot until a manual refresh is explicitly requested. Do not re-export live state as routine automation, and do not hand-edit exported snapshot files unless the direct task explicitly asks for that.

Use `spawners.jsonl`, `mobiles.jsonl`, `items.jsonl`, and relevant `live-state/scans/*.yaml` entries before falling back to static spawn, region, decoration, or binary map data. A live `PremiumSpawner` with spawned refs represents saved entities already present in the snapshot, not merely a hypothetical definition.

Load `client-worldmap-markers/` as the player's visible world-map overlay. Marker CSV rows use `x,y,map,name,type_or_icon,color,zoom_or_layer`; filter by the current map and include the matching map file plus any matching `-Common` file. Markers are navigation knowledge, not in-world entities, discovery flags, region triggers, roads, shelter, safety, or proof that anything is inside the 18-tile update range.

For terrain and statics, resolve the current map through `Data/Scripts/System/Misc/MapDefinitions.cs` before reading binary map files. Use the server's map ID, map index, file index, and `TileMatrix` block order when decoding `map*.mul`, `staidx*.mul`, or `statics*.mul`. Check terrain data when entering dense or ambiguous areas, when movement fails, or when a visible obstacle matters; do not audit every ordinary open-ground step.

## Data Editing Standards

Preserve file formats exactly: Markdown stays append-readable, YAML stays structured and parseable, JSONL remains one complete JSON object per line, and CSV marker rows keep their existing column order.

Make narrow, evidence-backed edits. For `Simulation_State.yaml`, prefer current playable continuity over historical audit detail. If an old state field sounds like a command to resolve private server state before play can continue, remove it from active state instead of summarizing it.

When correcting a prior CCWM mistake, record the correction in the map log only when it is part of a run. For instruction or policy migrations, update instructions and state policy without appending disclaimers to the historical play log.

## Verification

For instruction-only edits in this folder, verify with `git diff -- docs/CCWM/AGENTS.md docs/CCWM/ccwm-autoprompt.md docs/CCWM/Simulation_State.yaml`, `git diff --check`, and a YAML parse of `Simulation_State.yaml`. No MSBuild run is required unless C# code changes are part of the task.

For CCWM simulation-run updates, verify that `Simulation_State.yaml` and `Confictura_Codex_World_Map.md` agree on the final location, run number, visible state, current UI, follower state, and next client-visible decision before finishing.
