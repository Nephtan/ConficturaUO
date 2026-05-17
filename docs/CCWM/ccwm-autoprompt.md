Update docs/CCWM/Confictura_Codex_World_Map.md by acting as a simulated human player grinding through the shard's mechanics. You are not writing a wiki; you are the internal monologue of a player surviving the code.

CRITICAL RULE: Simulate only what a normal player can do through visible game flow. Do not forge gump ButtonIDs, call destination helpers directly, or choose starts/cards that the current character/account state would not expose.

PHASE 0: CHARACTER CREATION BEFORE FATE
Read docs/CCWM/Simulation_State.yaml.

If it does not exist, initialize a brand-new first-time account/character state from the code path:
- Trace CharacterCreation.EventSink_CharacterCreated.
- Create a PlayerMobile before any tarot choice.
- Save the actual pre-tarot properties:
- schema/meta: schema_version, run_number, last_updated, source_paths_traced, unresolved_unknowns, random_branches_chosen, random_branches_unresolved.
- account: account_id/name placeholder, access level, character slot index, other known account characters, and each character's decoded discovery flags because tarot gates can check the whole account.
- lifecycle: stage, last_action, current_goal, elapsed_play_time, session/hour marker, login/create/tarot status, current open gumps, and whether the character has left the start flow.
- identity: serial placeholder, name, title/profile, player flag, access level, young flag, public MyRunUO flag, female, body, hue, hair, facial hair, race, race_id, race_home_land, creature-race start area if any.
- fate/tarot: selected card/page/name if chosen, fate family, accessible tarot pages, inaccessible tarot pages with gate reason, AllowAlienChoice result, visitLodor result, visitSavage result, gypsy/table position requirement status.
- location: map, Point3D, region name/type, start-region status, nearby known NPCs/items, land bucket, time bucket, safe/guarded/public-area flags if traced.
- perception/visual_scan: scan_center, map, radius_tiles, sources_searched, visible_mobiles, visible_items, potential_entities, scan_confidence, and unresolved_spawn_sources. Distinguish mechanically traced entities from text-only scenery.
- travel/movement: travel_segment_policy, last_travel_segment, max segment tiles, scan/classifier boundary policy, follower shadowing policy, ambient AI policy, area-risk spawner policy, hard-stop conditions, and any known route blockers.
- vitals/status: alive/dead, death/corpse/resurrection state, hidden/criminal/wanted/murderer flags, hunger, thirst, poison/disease, hits/stam/mana current and max, raw stats, stat cap, fame, karma, kills.
- skills/leveling: full skill table with base/value/cap/lock where known, profession, SkillStart, SkillBoost, SkillEther, Skills.Cap, computed overall level, encounter alias levels, best archetype, and any anti-macro or skill-use cooldowns that affect the next action.
- inventory/equipment: backpack exists/movable, all backpack contents with type/name/amount/hue/loot type/blessed/owner/charges/durability/location where relevant, equipped items by layer, held light source, gold/currency, containers and nested contents, bank/stable/pet/follower state if discovered.
- quests/progression: CharacterDiscovered raw string and decoded nine-world map, CharacterKeys, CharacterGuilds, StandardQuest, FishingQuest, AssassinQuest, MessageQuest, BardsTaleQuest, ThiefQuest, EpicQuestName/Number, KilledSpecialMonsters, current quest object, completed quests/restart timers, acquired recipes, artifact quest timer, champion titles, ToT counters.
- playstyle/settings: CharacterMOTD, CharacterSkill, CharacterSheath, CharacterBoatDoor, CharacterPublicDoor, CharacterBegging, CharacterWepAbNames, CharacterElement, CharacterWanted, CharacterLoot, CharMusical, CharacterBarbaric, CharacterEvil, CharacterOriental, NONPK/PvP consent state, SkillDisplay, MagerySpellHue, ClassicPoisoning, GumpHue, WeaponBarOpen.
- UI/tools/books/bars: MyChat, RegBar, MyLibrary, QuickBar, MusicPlaylist, UsingAncientBook, all spellbar strings, open/closed toolbar state if relevant.
- guild/social/city: NPC guild, guild join time/game time, guild rank/message hues, alliance hue, perma flags, Solen friendship, city stone/title/show-title, owed back taxes and amount.
- timers/effects/cooldowns: peaced-until, ankh next use, honor/valor/compassion timers, compassion gains/day, last online, last deco confirmation, savage paint expiration, next tailor/smith bulk order, camp/bedroll references, auto-stabled pets, temporary hair/beard mods, active buffs/debuffs if traced.
- evidence: for every non-obvious value, record the C# path or doc path that established it. For every unavailable action, record the exact gate that blocks it. Record tarot branches explicitly:
  - accessible_tarot_cards: computed from GypsyTarotGump.pageShow, not guessed
  - last_action: "created character; has not chosen a tarot card"
Do not choose Savage, Lodoria, or any other gated route on this first initialization unless the account state proves the required discovery gate is already open.

If docs/CCWM/Simulation_State.yaml exists, load the complete account, character, and world-facing state. Treat missing fields as unknown, not false. If an unknown field would affect access, survival, movement, combat, inventory, social reaction, or tarot availability, trace the code before acting.

Required state sections:

- schema/meta: schema_version, run_number, last_updated, source_paths_traced, unresolved_unknowns, random_branches_chosen, random_branches_unresolved.
- account: account_id/name placeholder, access level, character slot index, other known account characters, and each character's decoded discovery flags because tarot gates can check the whole account.
- lifecycle: stage, last_action, current_goal, elapsed_play_time, session/hour marker, login/create/tarot status, current open gumps, and whether the character has left the start flow.
- identity: serial placeholder, name, title/profile, player flag, access level, young flag, public MyRunUO flag, female, body, hue, hair, facial hair, race, race_id, race_home_land, creature-race start area if any.
- fate/tarot: selected card/page/name if chosen, fate family, accessible tarot pages, inaccessible tarot pages with gate reason, AllowAlienChoice result, visitLodor result, visitSavage result, gypsy/table position requirement status.
- location: map, Point3D, region name/type, start-region status, nearby known NPCs/items, land bucket, time bucket, safe/guarded/public-area flags if traced.
- perception/visual_scan: scan_center, map, radius_tiles, sources_searched, visible_mobiles, visible_items, potential_entities, scan_confidence, and unresolved_spawn_sources. Distinguish mechanically traced entities from text-only scenery.
- travel/movement: travel_segment_policy, last_travel_segment, max segment tiles, scan/classifier boundary policy, follower shadowing policy, ambient AI policy, area-risk spawner policy, hard-stop conditions, and any known route blockers.
- vitals/status: alive/dead, death/corpse/resurrection state, hidden/criminal/wanted/murderer flags, hunger, thirst, poison/disease, hits/stam/mana current and max, raw stats, stat cap, fame, karma, kills.
- skills/leveling: full skill table with base/value/cap/lock where known, profession, SkillStart, SkillBoost, SkillEther, Skills.Cap, computed overall level, encounter alias levels, best archetype, and any anti-macro or skill-use cooldowns that affect the next action.
- inventory/equipment: backpack exists/movable, all backpack contents with type/name/amount/hue/loot type/blessed/owner/charges/durability/location where relevant, equipped items by layer, held light source, gold/currency, containers and nested contents, bank/stable/pet/follower state if discovered.
- quests/progression: CharacterDiscovered raw string and decoded nine-world map, CharacterKeys, CharacterGuilds, StandardQuest, FishingQuest, AssassinQuest, MessageQuest, BardsTaleQuest, ThiefQuest, EpicQuestName/Number, KilledSpecialMonsters, current quest object, completed quests/restart timers, acquired recipes, artifact quest timer, champion titles, ToT counters.
- playstyle/settings: CharacterMOTD, CharacterSkill, CharacterSheath, CharacterBoatDoor, CharacterPublicDoor, CharacterBegging, CharacterWepAbNames, CharacterElement, CharacterWanted, CharacterLoot, CharMusical, CharacterBarbaric, CharacterEvil, CharacterOriental, NONPK/PvP consent state, SkillDisplay, MagerySpellHue, ClassicPoisoning, GumpHue, WeaponBarOpen.
- UI/tools/books/bars: MyChat, RegBar, MyLibrary, QuickBar, MusicPlaylist, UsingAncientBook, all spellbar strings, open/closed toolbar state if relevant.
- guild/social/city: NPC guild, guild join time/game time, guild rank/message hues, alliance hue, perma flags, Solen friendship, city stone/title/show-title, owed back taxes and amount.
- timers/effects/cooldowns: peaced-until, ankh next use, honor/valor/compassion timers, compassion gains/day, last online, last deco confirmation, savage paint expiration, next tailor/smith bulk order, camp/bedroll references, auto-stabled pets, temporary hair/beard mods, active buffs/debuffs if traced.
- evidence: for every non-obvious value, record the C# path or doc path that established it. For every unavailable action, record the exact gate that blocks it.


PHASE 1: ACCESS CHECK BEFORE TAROT
Before picking a tarot card, trace the gypsy interaction:
- ShardGreeterEntry.OnClick only opens GypsyTarotGump if the player is at the proper start-table position or has RaceID > 0.
- GypsyTarotGump.pageShow controls which cards a normal player can page to.
- visitLodor requires an account character with "the Land of Lodoria" discovered.
- visitSavage requires an account character with "the Savaged Empire" discovered.
- Alien requires MyServerSettings.AllowAlienChoice().
- For a fresh first-time human account with no discoveries, Savage is NOT available. Treat page 12 / STRNGTH as hidden even though EnterLand has a page-12 branch.

If the current state is pre_tarot, the next logical action is usually one of:
- move to the gypsy/table position,
- talk to the gypsy,
- inspect the visible tarot deck,
- choose one visible, ungated card,
- or defer choosing while recording what the player learned.

PHASE 1.5: THE UPDATE RANGE (Simulated Client Vision)
You are a human looking at a screen, not a compiler reading a single script. The standard client update range is roughly 18 tiles. Before you decide your next move, you MUST mentally execute a `Map.GetMobilesInRange` and `Map.GetItemsInRange` centered on your current `Point3D`.
- First load `docs/CCWM/live-state/manifest.yaml` and the JSONL snapshots beside it. Treat `docs/CCWM/live-state/` as the canonical live save state until it is manually refreshed. Automation must not re-export on every run.
- Use `spawners.jsonl`, `mobiles.jsonl`, `items.jsonl`, and any relevant `scans/*.yaml` entry before consulting static `.map` files. A `PremiumSpawner` in the snapshot is live if `running: true`; its `spawned_refs` and the matching records in `mobiles.jsonl`/`items.jsonl` are already-visible saved entities, not hypothetical spawn definitions.
- Load `docs/CCWM/client-worldmap-markers/` as the player's visible world-map overlay. Marker CSV rows use `x,y,map,name,type_or_icon,color,zoom_or_layer`; filter them to the current map and record the nearest relevant markers separately from screen-visible mobiles/items. Include the map-specific file and any matching `-Common` file. These markers are navigation knowledge a normal client exposes, not in-world entities, discovery flags, region triggers, or proof that anything is within click/update range.
- To simulate this without a live server, search the repository's spawn data (e.g., XMLSpawners, initialization scripts, or region decorators) for your current location (e.g., Map.Sosaria, near 3579,3423).
- Use `.map` files as source attribution and unresolved fallback only after checking the live-state snapshot. A `.map` source line alone does not prove the entity is presently spawned when a live snapshot exists.
- Before any binary `map*.mul`, `staidx*.mul`, or `statics*.mul` scan, resolve the current map through `Data/Scripts/System/Misc/MapDefinitions.cs`. Record the map name, map index, map ID, and file index, then use the file index to choose the correct `mapN.mul`, `staidxN.mul`, and `staticsN.mul` files. Decode map/static blocks with the same column-major block order used by `TileMatrix.ReadLandBlock`: `((blockX * blockHeight) + blockY)`, not row-major `((blockY * blockWidth) + blockX)`. If the binary file number or block order does not match the server path, stop and correct the state before taking another simulated action.
- Do not just read the `WelcomeGump` text. What actual `Mobile` or `Item` entities are standing on the tiles next to you?
- Scan only the current map. Treat the range as roughly 18 tiles around the current `Point3D`; include deterministic spawn/decorator hits inside that range, but mark XMLSpawner, randomized, region-only, or otherwise non-deterministic entries as potential instead of certain.
- Use terrain and static tiles as visual context, not just raw tile IDs. A hidden `Teleporter` may be mechanically invisible while the surrounding cave mouth, path, floor, stairs, or wilderness tiles still make a normal player believe walking that way is possible.
- Persist the scan into `docs/CCWM/Simulation_State.yaml` before choosing the next move: scan_center, map, radius_tiles, sources_searched, visible_mobiles, visible_items, client_worldmap_markers, potential_entities, scan_confidence, and unresolved_spawn_sources.
- If the scan finds no nearby entities, record negative evidence: which source paths were searched, whether the result is `none_found` or `inconclusive`, and what unresolved spawn source could still change live visibility.
- If there is an NPC, a chest, or a corpse within visual range, you must acknowledge it in your internal monologue before you formulate your next goal. Players are easily distracted by shiny things; act like one.

PHASE 1.6: CREATURE/TIMER PRESSURE CLASSIFIER
The client view is not a paused screenshot, and off-screen is not hibernation. Before each player-controlled beat, and again after every waitable timer, visibility change, or gump/context-menu action, you MUST classify every mechanically traced visible, recently visible, or persisted-risk creature before deciding the next action. Routine movement inside a Phase 2 Travel Segment is the only exception: classify at the segment start, at the segment endpoint, and immediately at any segment stop event instead of after every compressed tile.
- Treat a creature as pressure-relevant if it is hostile, aggressive, high-damage, poison/breath-capable, uncontrolled with a non-None FightMode, recently primed by `OnMovement`/`ForceReacquire`, already has a Combatant, has a special timer such as `GiantToad.TeleportTimer`, could target the player/pet if its AI timer fires, or has a persisted unresolved AI note.
- Classify each pressure-relevant creature as exactly one of: `visible_active_pressure`, `recently_visible_pressure`, `timer_negative_evidence`, `carried_unresolved_not_active_blocker`, `ambient_unresolved`, or `not_relevant_this_beat`.
- Downgrade an off-screen passive creature to `ambient_unresolved` when it is outside the 18-tile client rectangle, has no Combatant/FocusMob/Aggressor/Aggressed/Faction/Ethic state involving the player or follower, has no special timer whose current scan can include the player or follower, and has not been recently primed in a way that can include them during the proposed next segment. Keep the note as uncertainty, but do not spend a beat re-litigating passive wandering just because the creature remains unresolved history.
- For each classified creature, persist: serial, type/name, current or last-known Point3D, dx, dy, `within_client_update_range_18`, `within_movement_notice_range_24`, `sector_active_possible`, `range_perception_contains_player`, `special_timer_contains_player`, `private_timer_available`, `chosen_status`, and `why`.
- Do not confuse the player's 18-tile client update rectangle with AI sleep or creature target scans. Use `Utility.InUpdateRange`/`Map.GetMobilesInRange` centered on the player only for visibility and click legality. Use `Map.GetMobilesInRange(creature.Location, creature.RangePerception)`, special timer scans, and the creature's acquisition filters for aggro. A creature may be visible at dx/dy 18 while the player is outside its `RangePerception`, and a creature outside the client rectangle may still be unresolved risk.
- Never claim off-screen hibernation unless a live export explicitly proves the timer stopped, the creature was deleted, or the creature despawned. Sector-active AI and delayed deactivation are the server truth; strict 18-tile culling is only a simulation safety policy, not a shard guarantee.
- If a high-risk AI or special timer tick is due or timing is unresolved, that pressure outranks routine movement, pet follow waits, long-term navigation, and curiosity clicks. Simulate the relevant timer path first (`AITimer.OnTick -> OnThink -> BaseAI.Think/current Action`, or the creature-specific timer such as `GiantToad.TeleportTimer`). If random movement, target ordering, LOS/pathing, range, sector-active state, or a private timer affects the result and cannot be resolved from state/code, stop the micro-batch and persist the unresolved branch instead of assuming safety.
- Repeated unexported private timer loop break: if the same high-risk branch has already been fully traced to an unexported private timer or live-only scheduler in at least two consecutive runs, and the current manifest/live-state snapshot has not changed, do not spend another run rechecking only that absent field. Persist the branch as `carried_unresolved_not_active_blocker`, keep it as risk evidence, and choose only conservative normal-player actions that reduce exposure or resolve visible uncertainty. Do not claim the creature is safe, gone, stationary, hibernated, or moved; carry both due/not-due outcomes as unresolved evidence.
- If the classifier or timer pass changes Combatant, FocusMob, Warmode, location, direction, visible range, damage, breath/melee timers, pet behavior, target cursor, region, or any other player-facing state, immediately re-scan and stop unless the next action is forced by visible UI or combat mechanics.
- Persist this as `ai_pressure_scan` in `docs/CCWM/Simulation_State.yaml` alongside the Phase 1.5 visual scan. Record negative evidence when no active high-risk pressure exists, including the source paths checked, the chosen classifier status, and why no AI or special timer result is committed.

PHASE 1.75: OPEN GUMPS ARE VISIBLE UI
If any gump is currently open, or if the chosen action opens a gump, treat that gump as part of the player's screen before choosing the next action.
- Trace the visible text and controls, not just the gump type, page number, or ButtonIDs. Inspect `AddHtml`, `AddLabel`, `AddHtmlLocalized`, `AddTextEntry`, `AddButton`, and helper methods that supply text such as `GypsySpeech()` or `cardText()`.
- Record the gump's title/header when traceable, visible body text summary, visible controls, text-entry fields, close/drag/resize flags, and whether the text has been read yet.
- A newly opened information-rich gump usually makes the next chronological action "read and think over the visible text" before pressing next, OK, close, or choice buttons.
- Gump text can teach actionable player knowledge, such as where to find a nearby shelf, journal, NPC, danger, or control. This knowledge can guide the next move, but it does not grant mechanical discovery flags, unlock tarot pages, move the character, or mutate account/character state unless the code path explicitly writes that state.
- Do not skip from opening a gump to using a response unless the current state says the player has already read or intentionally ignored the visible text and the response control is visible.

PHASE 2: THE HOURLY MICRO-BATCHED NEXT MOVE
Based purely on your current State, Location, Inventory, Last Action, persisted Phase 1.5 Visual Scan, AND any open-gump reading state from Phase 1.75, decide up to 3 chronological normal-player beats for this hourly wake.

Treat each beat as a real mini-run, except that routine travel can be compressed into one `Travel Segment` beat:
- Before beat 1, use the persisted Phase 1.5 scan and current open-gump state.
- Before beat 1, and before every later beat, use the persisted Phase 1.6 `ai_pressure_scan`. If any creature is classified `visible_active_pressure` or `recently_visible_pressure` with a due/unresolved AI or special timer result, the next beat must resolve that pressure or stop unless the exact branch is already marked `carried_unresolved_not_active_blocker`; do not take a routine player action first.
- If `ai_pressure_scan` marks a branch as `carried_unresolved_not_active_blocker`, treat it as risk evidence rather than an active pre-beat blocker. The next beat must still be conservative and risk-reducing or resolve visible uncertainty. If the next beat is a Travel Segment, re-scan and re-run the classifier at the segment endpoint or stop event.
- If `ai_pressure_scan` marks a branch as `ambient_unresolved`, carry it as background uncertainty rather than an active pre-beat blocker. It does not block routine travel unless the proposed segment would bring the player or follower back into that creature's visible range, target scan, movement-notice pressure, or special timer range.
- For every beat, choose exactly one normal-player action, one waitable visible/timed event, or one Travel Segment.
- Trace the C# path for that beat before mutating state.
- Apply the state mutation for that beat immediately.
- Re-scan visible range, re-run the creature/timer pressure classifier, and re-evaluate open UI before deciding whether beat 2 or beat 3 is legal. For a Travel Segment, this happens at segment end or at the first stop event, not after each compressed tile.
- Append the map entry as one run heading with clearly separated `Beat 1`, `Beat 2`, and `Beat 3` subsections when more than one beat completes. When a beat is a Travel Segment, use a `Travel Segment` subsection that records the route summary instead of listing every movement keypress as a separate beat. Do not create separate top-level headings like `Run 84.1` unless a future prompt explicitly changes this convention.
- Keep `docs/CCWM/Simulation_State.yaml` authoritative at the end of the hourly wake. Repeated unchanged sections may be summarized, but every completed beat must have its own state delta, code trace, visible-scan result, random branches, unresolved branches, evidence, and unavailable-action gates. For a Travel Segment, store this evidence in `last_travel_segment` and summarize the compressed substeps.

Travel Segment rules:
- Use a Travel Segment only for routine navigation across already-visible or mechanically probed terrain when no unread gump, target cursor, combat, active timer, visible interactive entity, or active high-risk AI pressure outranks movement.
- A Travel Segment may include up to `12` accepted player movement inputs, facing-only turns needed for those inputs, and controlled follower catch-up needed to keep existing `Follow` orders coherent.
- Pre-check the proposed route through the same movement evidence a normal player has earned: current map file index, binary land/static passability where needed, live saved mobile/item blockers, known region/teleporter/decorator evidence, and visible terrain context. Do not path through a tile whose passability, side-check, region, or blocker status is unknown.
- Stop the segment immediately if any normal stop rule fires: a visible hostile/high-risk mobile appears, any new NPC/corpse/chest/usable item/sign/road/shelter/water source/region text/gump/context menu/target cursor appears, movement fails, terrain/source evidence becomes uncertain, a random branch affects the outcome, or combat/damage/ownership/follower-count/quest/discovery/teleport/region/hunger/thirst/skill/open UI state changes.
- Scan and classify at the segment start and at the final endpoint. If the segment stops early, scan/classify at the stop point before choosing any later beat.
- Treat invisible spawner home ranges as `area_risk_summary` during travel, not as visible entities or active blockers. Spawned refs from the live-state snapshot still count as visible/potential entities when they enter range.
- Controlled followers on `Follow` may be shadowed back into their established trailing band, such as the current 2..3 tile `FriendsAvoidHeels` band, when they are on the same map, pathing over the compressed route is locally clear, and no combat, Guard/Attack order, ownership change, follower-count change, body-blocking, target cursor, or pet-specific interaction occurs. Persist this as approximate follower shadowing, not an exact `BaseAI.Obey` timer result. If follower pathing is blocked or uncertain, stop the segment and record the blocker.

Visible, mechanically traced interactive entities from Phase 1.5, high-risk AI pressure from Phase 1.6, and unread information-rich gumps from Phase 1.75 outrank long-term goals. If a tutorial NPC is standing two tiles away, your next move is likely to click them; if a readable gump just opened, your next move is likely to read and interpret it; if a poison-breath drake has a due or unresolved active AI tick, your next move is to resolve or stop on that pressure before walking past it. If no mechanically traced entity, unresolved active AI pressure, or unread gump competes, continue with the table/tarot flow or the next state-legal goal.

Stop the micro-batch early and save after the current beat when anything would make the next action a real player decision instead of routine continuation:
- A hostile or high-risk mobile is visible, enters range, or has unresolved active AI pressure that has not been marked `carried_unresolved_not_active_blocker` by the repeated private-timer loop-break rule.
- A new NPC, corpse, chest, usable item, sign, road, shelter, water source, region text, gump, context menu, or target cursor appears.
- A movement check fails, terrain/source evidence is uncertain, or binary map/static data has not been traced.
- A random branch affects outcome and was not already committed in state.
- Combat, damage, ownership, follower count, quest, discovery, teleport, region transition, hunger/thirst, skill gain, or open UI state changes.
- The player would naturally pause to read, inspect, choose, or react.

Safety beats speed. If any stop rule triggers, do not force beats 2 or 3.
- A new character does not already know Savage, Titan mechanics, Atlantis, or advanced systems.
- A pre-tarot character is still in the start forest/camp.
- A post-tarot character starts from the actual selected card destination.
- If an action requires a system gate, prove the gate from code and state before using it.

PHASE 3: THE CODE TRACE
Trace the RunUO/ServUO C# execution path for the chosen action.
Examples:
- Creating character: CharacterCreation.EventSink_CharacterCreated -> AddBackpack -> SetStats/SetSkills -> MoveToWorld.
- Talking to gypsy: ShardGreeterEntry.OnClick -> GypsyTarotGump.
- Reading a gump: gump constructor -> visible AddHtml/AddLabel/AddTextEntry/AddButton calls -> helper text methods such as GypsySpeech/cardText -> record player-facing knowledge without pressing a response.
- Paging cards: GypsyTarotGump.pageShow -> visitLodor/visitSavage/AllowAlienChoice.
- Choosing a card: GypsyTarotGump.OnResponse -> EnterLand, but only for a card that was visible through pageShow.
- Moving into a region: Region.OnEnter / StartRegion.OnEnter.
- Routine Travel Segment: route pre-check -> Mobile.Move / MovementImpl.CheckMovement for accepted movement inputs -> Region.CanMove / OnMoveOver gates -> endpoint scan/classifier -> optional controlled follower shadowing evidence. Do not expand every accepted movement input into its own beat unless a stop rule fires.
Identify the exact friction: inaccessible card, missing discovery flag, position requirement, inventory shortage, skill check, PvP rule, random encounter gate, hunger/thirst pressure, etc.

PHASE 4: ASSIMILATION & SAVING
1. Update docs/CCWM/Confictura_Codex_World_Map.md with the mechanical truth learned through the player’s friction and discovery. Cut lore/fluff aggressively.
2. Update docs/CCWM/Simulation_State.yaml with the new full state, not just a summary. Preserve enough fields that the next run cannot invent access it has not earned.
3. If a route becomes newly available because of discovery, record the exact discovered flag and code path that made it available.
4. Do not stage or commit.

OUTPUT
Print a brief Simulation Log:
- Starting state
- beats_attempted
- beats_completed
- Action taken for each completed beat
- Travel Segment route summary if a beat compressed movement
- C# paths traced
- Access gates checked
- Ending state
- early_stop_reason
- next_pressure
- git diff --check result
