# Confictura Development History

Confictura began as a heavily customized RunUO shard built from a Ruins & Riches-style foundation, then grew into its own multi-world sandbox with custom starts, survival pressure, player-run systems, dynamic PvE, extensive magic paths, and an increasingly documented toolchain.

This chronicle is a past-facing narrative history. It is not a roadmap, and it does not promise future systems. Dates are used only where the git history, Discord announcements, or existing shard news support them.

## Approximate / Undated Early Development: The Ruins & Riches Foundation

The earliest layer is not well dated in the available repository evidence. Confictura inherited a large custom RunUO/Ruins & Riches core before the public announcement and GitHub records begin. That base already carried the broad shape that still defines the shard: a fantasy sandbox with many maps, uncommon magic systems, expanded items and artifacts, dungeon travel, specialized vendors, boats, quests, and a large body of imported custom content.

What changed over time was not merely the addition of more files. Confictura gradually became more explicit about its own identity: a shard where old UO systems support stranger starts, harsher survival, player settlement, alternate progression, and staff-run world events.

## January-February 2023: Public Identity And First Community Footing

The existing in-game news archive opens on January 5, 2023 with a welcome to "Confictura: Legends & Adventure." By February, the Discord announcement log shows the shard recovering from downtime, establishing a web presence at confictura.net, and inviting players into early group adventures such as a Dardin's Pit expedition.

This era reads like a public opening phase. The player-facing emphasis was exploration, getting people connected, organizing Discord roles, and building enough trust that a small community could report issues and join live events.

## March-May 2023: Survival, Clones, PvP Consent, And Daily Iteration

Spring 2023 is where Confictura's personality starts to separate from a stock UO experience.

Early March brought practical quality-of-life updates: voting links from in game, staff-side character transfers, house item decay relief, artifact appearance and color customization, easier movement through owned pets, and safer handling around common travel and housing friction. Offline player clones then arrived as one of the shard's signature experiments: logged-out human characters could remain in the world as visible replicas, carry their equipped identity, and train other players when their skills were high enough. Later updates made those clones more accurate, hireable, safer around loot handling, and better separated from normal creature behavior.

The same month introduced a survival and domestic thread. Farming, cooking, brewing, juicing, and winemaking entered preliminary use alongside hunger and thirst messaging. Britain Castle gained a Master Cook, and Sosaria soon received Master Cook, Gardener, and Wine Crafter vendors. This gave Confictura a stronger day-to-day economy than combat loot alone.

Combat and world danger also changed quickly. Champion spawn tests began in the Fires of Hell, then were temporarily sealed after testing showed that locations and behavior needed more thought. Research Magic damage caps, Elementalism behavior, Jester crash fixes, Druidism fixes, shinobi item crashes, wandering healer behavior, corpse decay, and dangerous spawn loops were adjusted throughout this period.

May 2023 introduced the consensual PvP system. Existing players could use `[ChoosePvP`, while new characters could speak with the Goddess of Protection. Follow-up hotfixes clarified attackability through name colors and extended PvP/PvE protections into dungeons. This made PvP a declared path rather than a hidden assumption, while still leaving room for neutral and event states.

## June-August 2023: Client Stability, Connection Reliability, And GitHub Arrival

The summer focused heavily on keeping people connected and reducing technical interruptions. A DNS failure led to a new `play.confictura.net` connection target and an updated client package. That client update refreshed ClassicUO and Razor, removed unnecessary files, and added some map/static water sources. Later Discord posts also documented mobile setup details and connection troubleshooting.

The shard was still fighting stability issues. Announcements mention freeze investigations, a sailing-related freeze fix after extended testing, outage recovery after severe weather and hardware failures, and continued emphasis that progress loss should be minimal after restarts.

On August 31, 2023, the public git history begins. That same day also has an announcement for a weekly restart and a small harvesting convenience: cotton and flax could be spun directly from pack animals. Shortly afterward, Discord added a development role and repository update channel, marking a shift from purely live-server announcement history into a GitHub-backed development era.

## September-December 2023: GitHub-Era Systems Start To Settle

The first GitHub-era months show a mix of visible quality-of-life work, bug fixing, and larger system experiments.

Auto-loot gained a container-setting feature so `[loot` could place items into a chosen bag. AoE potions, provocation, offline clone training, autostabling, shoppe resource handling, custom vendor behavior, hunger and waterskin behavior, and pet bonding information all received attention. Pandora's Gift Box and the clear-deck command arrived as staff/player-facing utility additions.

Offline clones continued to mature. Fixes covered repeated startup cloning, paperdoll inspection, stealing protection, non-human hue handling, and clone stability. By late September, the clone system was framed as a persistent part of the world rather than a one-off curiosity.

Seasonal and community touches also appeared. Provisioners received Halloween stock, players could claim an October gift, and a Town Crier Discord channel began broadcasting world activity. These changes connected the live shard more tightly to community rhythms.

November 2023 was especially important. Offline Skill Training launched through Study Books sold by skill vendors, letting characters study while logged off. Several magic lines had costs reduced, the Jesters Guild opened, royal clothing layers were corrected, and the Soul Lantern and Holy Symbol were updated to accept more coin types from fresh kills.

The player city and monster-nest direction also emerged in November. Announcements described the Player City System as newly grounded, then made City Manager access and City Hall deeds available through Sosaria and Lodoria locations. Monster Nests were introduced as destructible sources of continuous enemy pressure and rewards. The random encounter system was updated, and December tied encounters more closely to player level calculations shown in the paper doll.

Magic Pools were revamped in December 2023 with luck-influenced outcomes, limited uses, and both positive and dangerous effects. The end-of-year notes also warned about house decay and reflected on a year of fixes, safer lands, stronger spells, and broader adventures.

## January-March 2024: Invasions, Nests, PvE Refinement, And A Major Client Update

Early 2024 brought a more ambitious live-event layer. Git history shows invasion framework work in January, with refactors and updates following. On February 10, Britain was attacked by Adimarchus and his forces. The invasion concluded without escalating fully, and the town guards pushed most spawn activity outward. The announcement framed it as a test of the invasion system rather than a permanent event state.

Monster nests became more visible in February through undead graveyard activity. Some nights, graveyards could open undead nests that players had to destroy directly, including by double-clicking the nest, using area spells, or targeting its nameplate.

The PvP/PvE system received several important February fixes. PvE-tagged pets were protected from unintended player attacks, and PvE players regained the ability to attack NPC mobiles they should be able to fight. Mage behavior was also tuned: poison casting gained a cooldown and dynamic chance curve, and later MageAI changes reduced summon dispel pressure while improving summoned creature self-healing.

March 2024 delivered a major client and server update. Announcements highlighted new animals, item naming fixes, spell and skill improvements, Artist NPC gold rewards, ClassicUO/Razor/ClassicAssist updates, around 120 buff icons, and server settings for hunger/thirst, bonding, skill points, and other gameplay options. The update also fixed special quest creature spawning and several crashes or naming problems. Players were warned that a client update was required.

Late March continued the "systems becoming real" pattern: hiring a clone could pay the offline player who owned it, citizens were stopped from unexpectedly teleporting home, Begging Study Books became available through thief vendors, the AFK command gained sound feedback, housing-system groundwork was laid for more dynamic rentals, and dough became stackable for easier baking.

## Mid-Late 2024: Maintenance And Live-Server Continuity

The public announcement record is quieter after March 2024, but not inactive. Git and Discord show maintenance around lawn tools, quivers, store sales lists, animal-form ninjitsu behavior, shard greeter updates, downtime recovery, and an October server move to a new location.

This period is best understood as operational continuity. The shard stayed online, recovered from crashes and infrastructure moves, and accumulated smaller fixes without the same density of new public systems seen in spring 2023 or spring 2024.

## March-November 2025: Documentation, City Work, Housing Boundaries, And New Creature Content

The March 2025 Discord log introduced Brother Samuel at an abbey east of Britain: a daily-appearing priest tied to criminal pardon quests and divine quest hints. Around the same broad period, git shows focused stability fixes such as a Research Magic null reference, lawn tool improvements, and house remodeling confirmation.

May 2025 marks a repository milestone: the `v1.0.0` and `ConficturaServer` tags both point to a ResearchRockFlesh null-reference fix. The tag names should not be read as a traditional commercial release announcement, but they do mark a stable source-control checkpoint.

August and September 2025 brought a large development wave. Player city work improved voting and management stones, city NPC placement, vendor handling, banning controls, and accelerated testing mode. Housing boundaries went through repeated passes across classic houses, townhouses, foundations, and house regions. NPC control and clone commands were hardened.

The same period launched an extensive documentation push. System audits and wiki pages were created or expanded for government, OmniAI, invasions, random encounters, XMLSpawner, homesteads, champion spawns, boats, bulk orders, bard song magic, staff commands, and many other systems. This mattered for players indirectly: more of the shard's unusual behavior became explainable and supportable.

Creature and world content also expanded. September commits and announcements added or prepared new mobiles such as pirates, Captain Swag, Maze Minotaurs, Mana Golems, Dio-themed creatures, Cursed Adventurers, Dark Cultists, The Mad Duke, Fire King, Fire Angel, Fire Spirit, Geode, and Grumpy Earth Spirit. The September 14 announcement explicitly framed some of that content as groundwork for Dio's new areas, so its exact live availability should be treated cautiously.

Late September and October expanded Dio's content wave beyond the first announced set. Additional custom mobiles, spawn entries, map danger data, Dragon Hydra and Honor Guard spawn fixes, Garden Golem testing, map freeze work for Dio's areas, UOArchitect support files, Book of the Dead enhancements, artifact boot synchronization, and crafting UI experiments all appeared in git. Some craft-search work was later reverted, showing that not every experiment remained part of the live player surface.

November 2025 was quieter in public announcements but still player-relevant in the repository: skill handling, spell bars, and Research Magic functions received follow-up adjustments. In the chronicle, this period is best treated as post-content stabilization rather than a separate launch.

## January-June 2026: Runtime Hardening, Roaming Champions, AI Testing, Dynamic Dungeons, And Current-State Mapping

Early 2026 continued cleanup and synchronization work. The repository reverted the craft-search experiment, synced test and live shard changes, adjusted Quest Tome, Phoenix, and OrbServer behavior, rebuilt under .NET Framework 4.8, and hardened crash/restart handling. Player clone AI gained support for multiple magic systems, and OmniAI fixes corrected spell type and lifecycle issues.

Champion spawns were substantially reorganized in April 2026. The system moved out of legacy maintenance space, its content was cleaned up, and its configuration support was hardened so placed champion encounters could retain their intended behavior.

On April 22, 2026, roaming champion spawns were announced live. Instead of fixed-only altars, each facet could have a single active roaming champion altar. Once defeated, the altar would go dark for 20 to 24 hours, then return at a new random location. The announced active themes were Unholy Terror, Vermin Horde, Abyss, and Arachnid.

That same day, a new launcher was announced as live, intended to keep clients up to date for map and client updates. On May 8, another launcher-delivered update refreshed ClassicUO and fixed book/text display problems.

On April 26, the first live monster AI overhaul test began. This was explicitly not a global AI replacement. It tested target selection on selected stock hostile creatures, especially Headless Ones, Ratmen, Lizardmen, Ratman Archers, and Lizardman Archers. Ratman and Lizardman nests were seeded across Sosaria and Lodoria to give players concrete fights to observe.

On May 2, 2026, Confictura's character level and dungeon danger model changed significantly. A unified 1-100 Character Level system became live, summarizing skill power, stats, reputation, and build identity rather than treating raw totals as the whole character. Dynamic random encounters became live across dungeon, cave, and bard-dungeon regions, adding temporary ambushes on top of hand-placed spawns. The announcement was careful about scope: wilderness and towns were not part of this system, enemies were not arbitrarily stat-scaled, and the internal archetype scoring did not lock players into classes. Follow-up commits tuned encounter gates, test configuration, archer spacing, skirmisher behavior, and dungeon difficulty regions.

The May 2026 git history also shows Godfrey 2026 premium mobiles, custom mobile title fixes, and a major wave of system documentation. The wiki index grew to cover player commands, PvP consent, offline skill training, monster nests, player government, housing, champion spawns, random encounters, boats, trades, magic schools, crafting, vendors, help systems, and staff-facing support tools. That documentation push did not itself add all of those systems, but it made the current shard far more explainable to players, staff, and returning maintainers.

May and early June also introduced the Confictura Codex World Map effort. This is a simulated-player continuity and mapping project rather than an in-game release: it records observed routes, maintains current playable state, stores exported live-state snapshots, and produces client world-map marker files and derived navigation aids. It matters historically because it marks a shift from only documenting code and announcements toward reconstructing how a normal player experiences the shard world.

Live community events continued during this period as well, including a February 2026 Hide & Seek event, March event announcements, and April clue-loading for that month's live event.

## Major Themes Across The Whole History

### Gameplay Systems

Confictura's history is defined by turning a large inherited shard into an interconnected sandbox. Offline clones, offline study books, PvP consent, player cities, monster nests, invasions, champion spawns, random encounters, and character level all became ways for player choice and world pressure to matter beyond ordinary spawn-and-loot loops.

### World And Content

The world expanded through multi-map play, updated spawns, new dungeons or dungeon-region work, custom mobiles, seasonal content, live invasions, roaming champions, Dio-area groundwork, Godfrey-era mobiles, and a growing map/documentation layer.

### Economy, Crafting, And Resources

Domestic and trade systems steadily grew: farming, cooking, brewing, juicing, winecrafting, hunger/thirst pressure, bulk orders, shoppes, runic tools, harvesting changes, pack-animal resource handling, stackable dough, and player-city treasury/tax services all gave non-combat play more structure.

### Combat And Balance

Combat work ranged from spell damage tuning and Research/Elementalism adjustments to PvP/PvE enforcement, pet protection, MageAI poison cooldowns, summon handling, clone combat behavior, OmniAI work, target-selection tests, and level-aware dungeon ambushes.

### Quality Of Life And Stability

The changelog is full of practical fixes: connection and DNS recovery, client packages, ClassicUO/Razor updates, auto-loot bags, stable capacity checks, house decay relief, boat and sailing freeze fixes, corpse decay changes, better feedback when placing house addons, client launch/update support, crash guard work, and .NET Framework 4.8 rebuilds.

### Knowledge, Documentation, And World Mapping

By 2025 and 2026, Confictura's current form was no longer defined only by live systems. The shard gained a large wiki, system audit, repository README, and CCWM mapping layer that made its unusual systems easier to understand, preserve, and revisit. That work is mostly not live gameplay, but it matters to players and staff because it turns a sprawling inherited world into something navigable.

### Events And Community

The shard has repeatedly used Discord, in-game news, the Town Crier, seasonal gifts, dungeon outings, invasions, live events, and player testing calls to make development participatory. Some systems, such as champion spawns and AI target selection, were explicitly rolled out as tests where player reports shaped follow-up work.

## How This History Was Reconstructed

- The GitHub-era timeline was reconstructed from the full git history using chronological commit subjects, file stats, changed-file lists, and tag metadata.
- The pre-GitHub and live-announcement timeline was reconstructed from `discord_announcements_log.json`, whose message records cover announcements from February 15, 2023 through May 8, 2026.
- Existing `Info/News.txt` was reviewed as the older in-game news archive, including entries back to January 5, 2023.
- README, wiki, settings, scripts, and source files were used only to clarify current behavior where announcements or commit subjects were too terse.
- Small technical commits were grouped into player-facing themes. This document intentionally does not list every commit, internal file path, or implementation-only change.

## Open Questions / Uncertain History

- The exact date and contents of the original Ruins & Riches import are not recoverable from the available git history. The current repository's first git commit is August 31, 2023, while the in-game and Discord records show the shard was already live earlier in 2023.
- The announcement log begins on February 15, 2023. Existing news reaches January 5, 2023, but any earlier private development remains undated here.
- Some announcements describe test-shard work or content prepared for later areas. Where that distinction is visible, this chronicle treats it as groundwork rather than assuming immediate live availability.
- The 2025-2026 documentation and world-map tooling history is extensive. Only the parts with clear player-facing meaning are summarized here; the CCWM material is described as mapping and continuity work, not as a live gameplay feature.
