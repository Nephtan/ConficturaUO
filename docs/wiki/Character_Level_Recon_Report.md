# Character Level Recon Report

This page is no longer a pre-implementation reconnaissance note. The canonical character-level system is live in the compiled shard and already drives player level display, random encounter gating for players, and staff diagnostics.

## Live Entry Points

- `Server.Misc.GetPlayerInfo.GetPlayerLevel(Mobile)` now returns `CharacterLevelService.GetOverallLevel(m)`.
- `Server.Misc.GetPlayerInfo.GetPlayerDifficulty(Mobile)` still buckets the returned overall level into the existing `25`, `50`, `75`, and `95` thresholds.
- `Server.Custom.RandomEncounters.Helpers.CalculateLevelForMobile(Mobile, LevelType)` now routes player mobiles to `CharacterLevelService.GetEncounterLevel(...)`.
- Non-player mobiles still use the legacy formula through `CharacterLevelService.GetLegacyMobileLevel(...)`.
- Creature AI and creature-facing scaling continue to use `IntelligentAction.GetCreatureLevel(Mobile)`, which remains a separate 1-125 formula.

## Canonical Overall Formula

For player mobiles, the overall level is built from normalized power and then converted to a 1-100 level with:

`level = clamp(1, 100, round(1 + 99 * power))`

Where the live `power` value is:

`power = clamp01(0.60 * skillPower + 0.25 * statProgress + 0.15 * reputationProgress)`

And:

- `skillPower = 0.75 * bestAdventurePower + 0.25 * skillProgress`
- `skillProgress = Skills.Total / skillCap`
- `statProgress = (RawStr + RawDex + RawInt) / statCap`
- `reputationProgress = (min(Fame, 15000) + min(abs(Karma), 15000)) / 30000`

`bestAdventurePower` is the highest score among the adventure archetypes currently tracked by the service:

- Martial
- Archer
- Assassin
- Ninja
- Samurai
- Knight
- DeathKnight
- ArcaneMage
- Elementalist
- Necromancer
- Witch
- Druid
- HolyMan
- MysticMonk
- Jedi
- Syth
- Researcher
- Ranger
- Bard
- Thief
- Jester

The service does define extra `CreatureRace` and `Alien` archetype scores for diagnostics, but those two are not part of `bestAdventurePower`, so they do not directly raise the overall level by themselves.

## Cap Handling

The live system is cap-aware:

- `skillCap` prefers `mobile.Skills.Cap`.
- If `Skills.Cap` is not usable, player mobiles fall back to `SkillStart + SkillBoost + SkillEther`.
- `statCap` uses `mobile.StatCap`; only invalid or unset values fall back to `225`.
- Individual archetype skill credit caps each contributing skill at `100.0` by dividing by `100.0` and clamping to `0.0-1.0`.

Current cap families come from `MyServerSettings.SkillBegin(...)` and later persistence repair:

| Start family | `SkillStart` | Typical `Skills.Cap` before Ether | Notes |
| --- | ---: | ---: | --- |
| Default | 10000 | 10000 + `SkillBoost` | Normal human start. |
| Savage | 11000 | 11000 + `SkillBoost` | Savaged Empire route. |
| Fugitive | 13000 | 13000 + `SkillBoost` | Britain dungeon / wanted route. |
| Alien | 40000 | 40000 + `SkillBoost` | Shuttle crash route. |

The Titan/Ether reward path adds `5000` to `Skills.Cap`, sets `SkillEther = 5000`, and raises `StatCap` to `300`. `PlayerMobile.Deserialize(...)` reconstructs these cap families from saved characters and reassigns `Skills.Cap = SkillStart + SkillBoost + SkillEther`.

## Encounter Alias Mapping

Random encounters still consume the legacy `LevelType` enum in XML, but player level checks now map each alias to the best matching Confictura archetype group:

| XML `LevelType` | Canonical mapping |
| --- | --- |
| `Fighter` | Best of Martial, Archer, Assassin, Ninja, Samurai, Knight, DeathKnight, MysticMonk, Jedi, Syth |
| `Ranger` | Best of Ranger, Druid, Archer, Bard |
| `Mage` | Best of ArcaneMage, Elementalist, HolyMan, Researcher |
| `Necromancer` | Best of Necromancer, Witch, DeathKnight |
| `Thief` | Best of Thief, Assassin, Ninja, Jester |
| `Overall` | Canonical overall level |

This means the old claim that random encounters ignore `LevelType` is no longer true for players.

## Archetype Weighting Highlights

Most archetypes use one of two live helper patterns:

- `Weighted(primary, supports...)` = `0.70 * primary + 0.30 * average(supports)`
- `RequiredPair(first, second, supports...)` = `0.55 * min(first, second) + 0.25 * max(first, second) + 0.20 * average(supports)`

Examples from the shipped implementation:

- Martial favors the best melee skill, then Tactics, Anatomy, Parry, Focus, and Healing.
- Knight uses `Knightship`, then Swords, Tactics, Healing, Spiritualism, and positive karma.
- DeathKnight uses `Knightship`, martial support skills, and negative karma.
- ArcaneMage uses `Magery` with Alchemy, Inscribe, Meditation, MagicResist, and Psychology.
- Elementalist uses `Elementalism`, Meditation, Psychology, and Focus.
- Necromancer uses `Necromancy`, then Spiritualism, Poisoning, MagicResist, and negative karma.
- Bard uses Musicianship with Provocation, Discordance, and Peacemaking.
- Jedi and Syth require both Psychology and Swords, with Tactics, karma polarity, and identity checks.

## Start Paths And Cap Mutation Reality

The wiki used to describe many start-path effects as if they were only future design inputs. In live code they already matter:

- Human tarot starts still place players at the documented coordinates.
- The fugitive human path does not directly mutate the cap in `GypsyTarotGump.EnterLand`; it sends the player to the Britain dungeon route, where `WantedRegion.OnEnter` calls `PlayerSettings.SetWanted(...)`.
- The savage human path places the player in `Map.SavagedEmpire`; `SavageRegion.OnEnter` calls `PlayerSettings.SetSavage(...)`.
- The alien human path places the player at the shuttle crash site; `CrashRegion.OnEnter` calls `PlayerSettings.SetSpaceMan(...)`, which moves the player to Lodoria after the alien setup work.
- Creature-race tarot starts use `BaseRace.StartArea(...)` to choose one of the race-specific start buckets and directly apply fugitive state on pages 2 and 4.

## Diagnostics And Staff Tools

The live system ships with two staff commands:

- `[CharLevel]` shows your own canonical diagnostics.
- `[CharLevelTarget]` targets any mobile and reports overall level, best archetype, skill/stat caps, start-family fields, race/profession/guild metadata, encounter alias levels, and archetypes above level 10.

## Legacy Paths That Still Exist

The modernization is real, but it is not total:

- Non-player mobiles still use the old hard-capped legacy formula for random encounter calculations.
- `IntelligentAction.GetCreatureLevel(...)` remains separate and still clamps creatures to 1-125.
- Encounter `scaleUp` logic still compares the player's `Fighter` alias against the spawned mobile's legacy `Overall` level, but the current `RandomEncounters.xml` keeps `scaleUp="false"` across the shipped tables.

## Bottom Line

The canonical character-level service is implemented, compiled, and integrated. The stale parts of the old report were the claims that player level still depended on `SkillBase()`, that random encounters ignored `LevelType`, and that the unified service was only a recommendation. The current shard already uses the service for player-facing level display and player random-encounter gating, while creatures and creature AI still retain their legacy level formulas.

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0010.
- Backlog rows: RB-06674.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs (CurrentFile)
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs (CurrentFile)
- Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs (CurrentFile)
- Data/Scripts/System/Misc/Players.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=4; Event=1; Gump=5; Initialize=4.
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs:L12 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs:L14 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs:L20 Command CommandSystem.Register access=Unknown
- Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs:L30 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Misc/Players.cs:L1474 Initialize Initialize access=GlobalOrInternal
- Data/Scripts/System/Misc/Players.cs:L1476 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/Players.cs:L1485 Command CommandSystem.Register access=Unknown
- Data/Scripts/System/Misc/Players.cs:L1494 Gump SendGump access=Internal
- Data/Scripts/System/Misc/Players.cs:L1856 Gump OnResponse access=Internal
- Data/Scripts/System/Misc/Players.cs:L1862 Gump SendGump access=Internal
- Data/Scripts/System/Misc/Players.cs:L1863 Gump SendGump access=Internal
- Data/Scripts/System/Misc/Players.cs:L1885 Initialize Initialize access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- No serialized classes matched the reviewed source set in serialization-register.csv.

### Project And Runtime Coverage

- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs=Keep
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelCommands.cs=Keep
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs=Keep
- Data/Scripts/Custom/Progression/CharacterLevel/CharacterLevelService.cs=Keep
- Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs=Keep
- Data/Scripts/Custom/PvE/RandomEncounters/Helpers.cs=Keep
- Data/Scripts/System/Misc/Players.cs=Keep
- Data/Scripts/System/Misc/Players.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
