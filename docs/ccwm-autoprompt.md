Update docs/Confictura_Codex_World_Map.md by acting as a simulated human player grinding through the shard's mechanics. You are not writing a wiki; you are the internal monologue of a player surviving the code.

CRITICAL RULE: Simulate only what a normal player can do through visible game flow. Do not forge gump ButtonIDs, call destination helpers directly, or choose starts/cards that the current character/account state would not expose.

PHASE 0: CHARACTER CREATION BEFORE FATE
Read docs/Simulation_State.yaml.

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

If docs/Simulation_State.yaml exists, load the complete account, character, and world-facing state. Treat missing fields as unknown, not false. If an unknown field would affect access, survival, movement, combat, inventory, social reaction, or tarot availability, trace the code before acting.

Required state sections:

- schema/meta: schema_version, run_number, last_updated, source_paths_traced, unresolved_unknowns, random_branches_chosen, random_branches_unresolved.
- account: account_id/name placeholder, access level, character slot index, other known account characters, and each character's decoded discovery flags because tarot gates can check the whole account.
- lifecycle: stage, last_action, current_goal, elapsed_play_time, session/hour marker, login/create/tarot status, current open gumps, and whether the character has left the start flow.
- identity: serial placeholder, name, title/profile, player flag, access level, young flag, public MyRunUO flag, female, body, hue, hair, facial hair, race, race_id, race_home_land, creature-race start area if any.
- fate/tarot: selected card/page/name if chosen, fate family, accessible tarot pages, inaccessible tarot pages with gate reason, AllowAlienChoice result, visitLodor result, visitSavage result, gypsy/table position requirement status.
- location: map, Point3D, region name/type, start-region status, nearby known NPCs/items, land bucket, time bucket, safe/guarded/public-area flags if traced.
- perception/visual_scan: scan_center, map, radius_tiles, sources_searched, visible_mobiles, visible_items, potential_entities, scan_confidence, and unresolved_spawn_sources. Distinguish mechanically traced entities from text-only scenery.
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
- To simulate this without a live server, search the repository's spawn data (e.g., XMLSpawners, initialization scripts, or region decorators) for your current location (e.g., Map.Sosaria, near 3579,3423).
- Before any binary `map*.mul`, `staidx*.mul`, or `statics*.mul` scan, resolve the current map through `Data/Scripts/System/Misc/MapDefinitions.cs`. Record the map name, map index, map ID, and file index, then use the file index to choose the correct `mapN.mul`, `staidxN.mul`, and `staticsN.mul` files. If the binary file number does not match the current map's registered file index, stop and correct the state before taking another simulated action.
- Do not just read the `WelcomeGump` text. What actual `Mobile` or `Item` entities are standing on the tiles next to you?
- Scan only the current map. Treat the range as roughly 18 tiles around the current `Point3D`; include deterministic spawn/decorator hits inside that range, but mark XMLSpawner, randomized, region-only, or otherwise non-deterministic entries as potential instead of certain.
- Use terrain and static tiles as visual context, not just raw tile IDs. A hidden `Teleporter` may be mechanically invisible while the surrounding cave mouth, path, floor, stairs, or wilderness tiles still make a normal player believe walking that way is possible.
- Persist the scan into `Simulation_State.yaml` before choosing the next move: scan_center, map, radius_tiles, sources_searched, visible_mobiles, visible_items, potential_entities, scan_confidence, and unresolved_spawn_sources.
- If the scan finds no nearby entities, record negative evidence: which source paths were searched, whether the result is `none_found` or `inconclusive`, and what unresolved spawn source could still change live visibility.
- If there is an NPC, a chest, or a corpse within visual range, you must acknowledge it in your internal monologue before you formulate your next goal. Players are easily distracted by shiny things; act like one.

PHASE 1.75: OPEN GUMPS ARE VISIBLE UI
If any gump is currently open, or if the chosen action opens a gump, treat that gump as part of the player's screen before choosing the next action.
- Trace the visible text and controls, not just the gump type, page number, or ButtonIDs. Inspect `AddHtml`, `AddLabel`, `AddHtmlLocalized`, `AddTextEntry`, `AddButton`, and helper methods that supply text such as `GypsySpeech()` or `cardText()`.
- Record the gump's title/header when traceable, visible body text summary, visible controls, text-entry fields, close/drag/resize flags, and whether the text has been read yet.
- A newly opened information-rich gump usually makes the next chronological action "read and think over the visible text" before pressing next, OK, close, or choice buttons.
- Gump text can teach actionable player knowledge, such as where to find a nearby shelf, journal, NPC, danger, or control. This knowledge can guide the next move, but it does not grant mechanical discovery flags, unlock tarot pages, move the character, or mutate account/character state unless the code path explicitly writes that state.
- Do not skip from opening a gump to using a response unless the current state says the player has already read or intentionally ignored the visible text and the response control is visible.

PHASE 2: THE NEXT MOVE
Based purely on your current State, Location, Inventory, Last Action, persisted Phase 1.5 Visual Scan, AND any open-gump reading state from Phase 1.75, decide one chronological action.
Visible, mechanically traced interactive entities from Phase 1.5 and unread information-rich gumps from Phase 1.75 outrank long-term goals. If a tutorial NPC is standing two tiles away, your next move is likely to click them; if a readable gump just opened, your next move is likely to read and interpret it. If no mechanically traced entity or unread gump competes, continue with the table/tarot flow or the next state-legal goal.
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
Identify the exact friction: inaccessible card, missing discovery flag, position requirement, inventory shortage, skill check, PvP rule, random encounter gate, hunger/thirst pressure, etc.

PHASE 4: ASSIMILATION & SAVING
1. Update docs/Confictura_Codex_World_Map.md with the mechanical truth learned through the player’s friction and discovery. Cut lore/fluff aggressively.
2. Update docs/Simulation_State.yaml with the new full state, not just a summary. Preserve enough fields that the next run cannot invent access it has not earned.
3. If a route becomes newly available because of discovery, record the exact discovered flag and code path that made it available.
4. Do not stage or commit.

OUTPUT
Print a brief Simulation Log:
- Starting state
- Action taken
- C# paths traced
- Access gates checked
- Ending state
- git diff --check result
