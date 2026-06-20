Update `docs/CCWM/Confictura_Codex_World_Map.md` by acting as a simulated human player grinding through the shard's mechanics. You are not writing a wiki; you are the internal monologue of a player surviving what the client shows.

CRITICAL RULE: Simulate only what a normal player can do through visible game flow. Do not forge gump ButtonIDs, call destination helpers directly, invoke private helpers as player actions, or choose starts/cards/routes that the current character/account state would not expose.

PHASE 0: LOAD CURRENT PLAYER STATE

Run `python docs/CCWM/tools/ccwm_verify.py` before acting. Treat failures as blocking until resolved. Treat warnings as visible risk that must be acknowledged, especially stale live-state snapshots and current dirty run/state files.

Read `docs/CCWM/Simulation_State.yaml` before acting.

If the state file does not exist, initialize a minimal first-time account/character state from visible client flow and necessary access gates:

- account and character identity known to the client
- current stage, location, map, facing, open UI, target cursor, and login/create/tarot status
- visible inventory, equipment, vitals, skills relevant to the next action, and follower state
- current visual scan, world-map overlay markers, recent client-visible friction, and route intent
- evidence for non-obvious access gates such as tarot availability or hidden discovery flags

If the state file exists, load it as current playable continuity. Treat missing fields as unknown, not false. Unknowns block action only when a normal player could not continue without resolving them, such as alive/dead status, open UI, target cursor, inventory availability, visible blockers, or a gated interaction currently being attempted.

Do not rebuild historical trace state before acting. The map log is the archive; the YAML is only the next-run state.

PHASE 1: ACCESS CHECK BEFORE TAROT

Before picking a tarot card, trace only the access gates a normal player is attempting to use:

- `ShardGreeterEntry.OnClick` only opens `GypsyTarotGump` if the player is at the proper start-table position or has `RaceID > 0`.
- `GypsyTarotGump.pageShow` controls which cards a normal player can page to.
- Lodoria requires an account character with "the Land of Lodoria" discovered.
- Savage requires an account character with "the Savaged Empire" discovered.
- Alien requires `MyServerSettings.AllowAlienChoice()`.
- For a fresh first-time human account with no discoveries, Savage is not available. Treat page 12 / STRNGTH as hidden even though `EnterLand` has a page-12 branch.

If the current state is pre-tarot, the next logical action is usually one of:

- move to the gypsy/table position
- talk to the gypsy
- inspect the visible tarot deck
- choose one visible, ungated card
- defer choosing while recording what the player learned

PHASE 1.5: CLIENT VISION AND WORLD-MAP CONTEXT

You are a human looking at a client screen. The standard client update range is roughly 18 tiles. Before choosing the next action, scan the current screen and obvious route context.

- First load `docs/CCWM/live-state/manifest.yaml` and the JSONL snapshots beside it. Treat `docs/CCWM/live-state/` as the canonical live save state until it is manually refreshed.
- Do not refresh live state automatically. The exporter is offline CCWM tooling under `docs/CCWM/tools/live-state-exporter/`, not a server script under `Data/Scripts`.
- Use `spawners.jsonl`, `mobiles.jsonl`, `items.jsonl`, and relevant `scans/*.yaml` entries before consulting static spawn, decoration, region, or binary map data.
- Load `docs/CCWM/client-worldmap-markers/` as the player's visible world-map overlay. Filter markers to the current map and record relevant nearby markers separately from screen-visible entities.
- Treat markers as navigation knowledge, not in-world proof of shelter, roads, discovery, safety, or anything inside the 18-tile screen.
- For terrain/statics, resolve the current map through `Data/Scripts/System/Misc/MapDefinitions.cs` before reading binary files. Use binary terrain checks when entering dense terrain, when movement fails, when a visible obstacle matters, or when the route crosses ambiguous water/mountain/ruin/dungeon geometry.
- Use terrain and static tiles as visual context. A hidden teleporter may be mechanically invisible while surrounding cave mouths, stairs, floors, or paths still make a normal player believe walking that way is possible.
- If there is an NPC, corpse, chest, usable item, sign, road, shelter, water source, hostile, or unusual mobile within visual range, acknowledge it before forming the next goal.

Persist the scan in `Simulation_State.yaml` as current client state: scan center, map, visible mobiles/items, relevant markers, visible blockers, recent route context, and scan confidence.

PHASE 1.6: THREAT HEURISTICS

You are a player, not the server. Do not predict private creature timers, scheduler fields, random wandering, or exact ambient AI branches before moving.

Classify visible and recently visible creatures with client-side heuristics:

- Hostile or dangerous within 0-10 tiles: immediate threat. The next beat must evade, fight, defend, use a visible escape, or stop for a real player decision.
- Hostile or dangerous within 11-18 tiles: potential threat. You may travel, but route away or avoid closing distance unless deliberately engaging.
- Passive, tame, vendor, townsperson, or low-risk wildlife: visible context. Inspect or interact only when it helps the player goal.
- Off-screen creatures: remembered route risk only. They do not block routine travel unless the chosen route intentionally returns toward them or a visible result brings them back onto the screen.

Do not stop just because a creature's next ambient step is unpredictable. Humans handle uncertainty by moving away, keeping distance, or changing route.

PHASE 1.75: OPEN GUMPS ARE VISIBLE UI

If any gump is open, or if the chosen action opens a gump, treat that gump as part of the player's screen before choosing the next action.

- Trace visible text and controls when the gump is custom, information-rich, gated, or confusing.
- Record title/header, visible body summary, visible controls, text-entry fields, close/drag/resize flags, and whether the text has been read.
- A newly opened information-rich gump usually makes the next chronological action "read and think over the visible text" before pressing next, OK, close, or choice buttons.
- Gump text can teach player knowledge, but it does not grant mechanical discovery flags or mutate state unless the response path actually writes that state.
- Do not skip from opening a gump to using a response unless the player has already read or intentionally ignored the visible text and the response control is visible.

PHASE 2: INTENTION-BASED MACRO RUNS

Based on current State, Location, Inventory, Last Action, client visual scan, open UI, and route goal, decide up to three chronological normal-player beats for this hourly wake.

Each beat is one of:

- a normal player action
- a visible wait/event the player would choose
- a macro travel intention

Macro travel covers routine movement by intention instead of tile-by-tile auditing. State the intention in player terms, such as "run west along the open grass toward the Mines marker" or "skirt north around the animal line."

Macro travel rules:

- Assume ordinary open-terrain pathing works unless the client-visible route suggests dense forest, ruin, dungeon, water, mountain, wall, door, or other maze-like blocking.
- Reuse previously learned route knowledge and visible terrain. Do not verify every grass tile or every facing-mask update.
- Pets ordered to `Follow` are assumed to keep up within practical update range. Do not calculate their exact tile offsets during travel.
- Invisible spawner home ranges are area risk, not visible entities or active blockers. Spawned refs from the live-state snapshot still matter when visible.
- Stop macro travel when a hostile or dangerous mobile enters the 0-10 tile threat radius, a visible 11-18 tile threat changes the route decision, a gump/context menu/target cursor appears, movement fails, stamina hits 0, the route hits a physical boundary, a meaningful region/teleport/discovery/resource/combat state changes, or the destination/waypoint is reached.
- Walking into a visible tree, shoreline, mountain, wall, or other ordinary obstacle is a client-side correction: route around it. Do not open C# unless the failure is caused by unexplained custom shard behavior.

Visible interactives and unread information-rich gumps outrank long-term travel. If a tutorial NPC is standing two tiles away, the next move is likely to click them. If a readable gump just opened, the next move is likely to read it. If a dangerous creature enters close range, the next move is survival.

Safety is not speed. Stop after the current beat if the player would naturally pause to read, inspect, choose, fight, flee, or reroute.

PHASE 3: DIAGNOSTIC C# TRACING

C# tracing is post-action or gate-specific diagnostic evidence. Use it only when the client-visible world refuses to explain the result.

Trace for:

- custom Confictura mechanics
- failed or gated gumps
- hidden access gates
- custom region, teleporter, quest, item, or skill friction
- unexpected custom-script behavior
- a normal action that produced a shard-specific message or no-op the client cannot explain

Do not trace for:

- routine open-ground movement
- standard combat swings
- normal ambient creature wandering
- ordinary pet follow during travel
- visible terrain collision with trees, rocks, shorelines, mountains, walls, or doors

When tracing is justified, identify the exact friction: inaccessible card, missing discovery flag, position requirement, inventory shortage, skill check, PvP rule, custom region gate, unexpected no-op, or other shard-specific wall.

PHASE 4: ASSIMILATION AND SAVING

1. Update `docs/CCWM/Confictura_Codex_World_Map.md` with the player-facing result and any mechanical friction learned. Cut lore/fluff aggressively.
2. Update `docs/CCWM/Simulation_State.yaml` with current playable continuity, not a historical trace dump.
3. Preserve current location/facing, vitals, inventory, open UI, follower order/presence, visible entities, relevant markers, known blockers, route intent, and next client-visible decision.
4. Delete or avoid active state fields that command future runs to resolve private timers, exact ambient AI, exact pet leash ticks, or old route-blocker classifications before normal play.
5. If a route becomes newly available because of discovery, record the exact discovered flag and the visible/code gate that made it available.
6. Do not stage or commit during automated CCWM runs unless the user explicitly asks for a repository commit.
7. Run `python docs/CCWM/tools/ccwm_index.py` after log/state changes so `generated/Run_Index.md` and `generated/Knowledge_Index.yaml` stay derived from the current sources.
8. Run `python docs/CCWM/tools/ccwm_verify.py` and report any failures or warnings in the final Simulation Log.

OUTPUT

Print a brief Simulation Log:

- Starting state
- beats_attempted
- beats_completed
- Action taken for each completed beat
- Macro travel route summary, if used
- Diagnostic C# paths traced, if any
- Access gates checked
- Ending state
- early_stop_reason
- next_client_visible_decision
- `git diff --check` result
