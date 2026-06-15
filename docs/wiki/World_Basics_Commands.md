# World Basics And Commands

## Scope

This page documents the player-facing world-basics help surfaced through `Server.Engines.Help.HelpGump` page `1`, plus the built-in `BasicsGump` opened from `MyLibrary`. It is a `Gump` and static text guide, not a persistent `Item`, `Mobile`, packet handler, XMLSpawner attachment, or serialized system.

There is no `Serialize` or `Deserialize` block for this guide.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Registers the Help request event hook, builds the Help navigation gump, renders `MyHelp()` on page `1`, and dispatches Help button replies. |
| `Data/Scripts/System/Commands/Player/Basics.cs` | Defines `BasicsGump`, a scrollable gump that renders `DynamicBook.BasicHelp()`. |
| `Data/Scripts/Items/Books/DynamicBook.cs` | Stores the long-form world-basics text in `BasicHelp()` and also assigns it to `BookGuideToAdventure` and `LoreGuidetoAdventure`. |
| `Data/Scripts/System/Commands/Player/MyLibrary.cs` | Opens built-in library help entries; button `400` opens `BasicsGump`. |
| `Data/Scripts/System/Commands/Handlers.cs` | Registers `[Help`, which lists registered commands by access level. This command does not open `HelpGump`. |
| `Data/System/Source/Commands.cs` | Provides `CommandSystem.Prefix`, case-insensitive command registration, command parsing, access checks, and handler dispatch. |
| `Data/Scripts/System/Misc/World.cs` | Provides `Worlds.GetDifficultyLevel(Point3D loc, Map map)`, the hardcoded dungeon-difficulty lookup referenced by nearby gameplay systems. |
| `Data/Scripts/Mobiles/Base/BaseCreature.cs` | Applies difficulty heat to creature stats and loot through `BeefUp()` and `BeefUpLoot()`. |

## Entry Points

| Entry point | Compiled behavior |
| --- | --- |
| Client Help request | `HelpGump.Initialize()` subscribes `EventSink.HelpRequest`. `EventSink_HelpRequest()` skips duplicate open `HelpGump` instances, checks `PageQueue.CheckAllowedToPage()`, and opens `new HelpGump(e.Mobile, 1)` when the player is not already in the page queue. |
| Help gump page `1` | The `HelpGump(Mobile from, int page)` constructor sets `HelpText = MyHelp()`. When `page == 1`, it adds that HTML text into the main scroll area and highlights the `Main` navigation button. |
| Help gump button replies | `OnResponse(NetState state, RelayInfo info)` takes `Mobile from = state.Mobile`, plays sound `0x4A`, closes the current `HelpGump`, and dispatches by `info.ButtonID`. |
| `InvokeCommand(string c, Mobile from)` | Runs `CommandSystem.Handle(from, String.Format("{0}{1}", CommandSystem.Prefix, c))`, so the current command prefix is prepended before dispatch. The default prefix is `[`. |
| `[Help` command | Registered at player access by `Handlers`. It builds and sends a text list of registered commands available to the caller by comparing `m.AccessLevel` to each command entry. |
| Library `Basics` entry | `MyLibrary.OnResponse()` button `400` closes `BasicsGump` and sends `new BasicsGump(from, 0)`. `BasicsGump` renders `DynamicBook.BasicHelp()`. |
| Guide to Adventure books | `DynamicBook.SetStaticText()` assigns `BasicHelp()` to `BookGuideToAdventure` and `LoreGuidetoAdventure`. |

## HelpGump Navigation

| Button ID | Label | Action |
| ---: | --- | --- |
| `1` | Main | Reopens page `1` and renders `MyHelp()`. |
| `2` | AFK | Invokes `[afk`, then redraws page `2`. |
| `3` | Chat | Invokes `[c`. |
| `4` | Corpse Clear | Invokes `[corpseclear`, then redraws page `4`. |
| `5` | Corpse Search | Invokes `[corpse`. |
| `6` | Emote | Invokes `[emote`. |
| `7` | Magic Toolbars | Opens page `7`, which contains spell-bar editor/open/close buttons. |
| `8` | Moongate Search | Invokes `[magicgate`. |
| `9` | MOTD | Opens the MOTD gump through `Joeku.MOTD.MOTD_Utility.SendGump()`. |
| `10` | Quests | Opens page `10`, which includes `MyQuests(from)`. |
| `11` | Quick Bar | Opens `QuickBar`. |
| `62` | Reagent Bar | Opens `RegBar`. |
| `12` | Settings | Opens the Help settings panel. |
| `13` | Library | Opens `MyLibrary`. |
| `14` | Statistics | Opens `Server.Statistics.StatisticsGump`. |
| `15` | Stuck in World | Moves an AOS house caller to the house ban location, or opens `StuckMenu` if region, combat, frozen, criminal, and kill-count checks allow it. |
| `16` | Weapon Abilities | Invokes `[sad`. |
| `17` | Wealth Bar | Opens `WealthBar`. |
| `18` | Conversations | Opens `MyChat`. |
| `19` | Version | Opens page `19` with `MyServerSettings.Versions()`. |

## Main Guide Text

`MyHelp()` is one large static HTML string. It directs players to in-world information sources before listing command shortcuts and gameplay toggles.

| Help topic | Source in code | Compiled behavior |
| --- | --- | --- |
| Merchant scrolls/books, sage tomes, Guide to Adventure, townsfolk, and paperdoll `Info` | Static prose in `MyHelp()` | Advisory text only; no lookup or validation is performed by the help gump. |
| Common commands | Static command list in `MyHelp()` | Text only; actual command availability depends on registered command handlers and access checks. |
| Area difficulty labels | Static label list in `MyHelp()` | Text only; `HelpGump` does not compute the difficulty of the caller's current region. |
| Skill title command | Static prose in `MyHelp()` | Describes `[SkillName "<skill>"]` and `"clear"` behavior; actual command logic is in `SkillName.cs`. |
| Reagent bars | Static prose in `MyHelp()` | Lists `[regbar` and `[regclose`. |
| Magic toolbars | Static prose in `MyHelp()` plus page `7` buttons | Lists editor, open, and close commands for Ancient, Bard, Knight, Death Knight, Elemental, Priest, Mage, Monk, and Necromancer bars. |
| Music | Static prose in `MyHelp()` plus settings buttons | Lists `[music` and `[musical`; page `12` can open `MusicPlaylist` or toggle `PlayerMobile.CharMusical`. |
| Evil, Oriental, and Barbaric styles | Static prose in `MyHelp()` plus settings buttons | Page `12` mutates `PlayerMobile.CharacterEvil`, `CharacterOriental`, and `CharacterBarbaric` directly. |

## Commands Listed By MyHelp

All commands below are literal entries in the main Help text. The command registry is case-insensitive.

| Command | Guide description or traced dispatch |
| --- | --- |
| `[abilitynames` | Toggles special weapon ability names near the appropriate icons. |
| `[afk` | Toggles the caller's away-from-keyboard notification. |
| `[ancient` | Toggles ancient magic between research-bag and ancient-spellbook casting paths. |
| `[autoattack` | Toggles automatic counter-attacking when attacked. |
| `[bandother` | Bandage-other shortcut. |
| `[bandself` | Bandage-self shortcut. |
| `[barbaric` | Toggles the Barbaric play style. |
| `[c` | Opens or enters the chat system. |
| `[corpse` | Searches for the caller's remains. |
| `[cleardeck` | Clears eligible corpses from the boat deck. |
| `[e` | Opens the emote mini window. |
| `[emote` | Opens the emote window. |
| `[evil` | Toggles the Evil play style. |
| `[loot` | Opens automatic loot category settings. |
| `[magicgate` | Searches for the nearest magical gate. |
| `[motd` | Opens the message of the day. |
| `[oriental` | Toggles the Oriental play style. |
| `[password` | Changes the caller's account password. |
| `[poisons` | Toggles classic poison behavior. |
| `[private` | Toggles detailed journey messages for town crier and local chatter systems. |
| `[quests` | Opens the quest overview. |
| `[quickbar` | Opens the common-actions quick bar. |
| `[sad` | Opens weapon special abilities. |
| `[set1` through `[set5` | Selects weapon ability slots. |
| `[sheathe` | Toggles automatic weapon sheathing outside combat. |
| `[skill` | Opens the skill-description gump. |
| `[skilllist` | Opens the condensed watched-skill list. |
| `[spellhue ##` | Sets `PlayerMobile.MagerySpellHue` from the first integer argument. |
| `[statistics` | Opens server statistics when the statistics subsystem allows player access. |
| `[wealth` | Opens a wealth bar for backpack and bank currency values. |

## Command Families

| Family | Commands |
| --- | --- |
| Reagent bars | `[regbar`, `[regclose` |
| Ancient spell editors | `[archspell1`, `[archspell2`, `[archspell3`, `[archspell4` |
| Bard song editors | `[bardsong1`, `[bardsong2` |
| Knight spell editors | `[knightspell1`, `[knightspell2` |
| Death Knight spell editors | `[deathspell1`, `[deathspell2` |
| Elemental spell editors | `[elementspell1`, `[elementspell2` |
| Priest prayer editors | `[holyspell1`, `[holyspell2` |
| Mage spell editors | `[magespell1`, `[magespell2`, `[magespell3`, `[magespell4` |
| Monk ability editors | `[monkspell1`, `[monkspell2` |
| Necromancer spell editors | `[necrospell1`, `[necrospell2` |
| Visible spell bars | `[archtool1` through `[archtool4`, `[bardtool1` through `[bardtool2`, `[knighttool1` through `[knighttool2`, `[deathtool1` through `[deathtool2`, `[elementtool1` through `[elementtool2`, `[holytool1` through `[holytool2`, `[magetool1` through `[magetool4`, `[monktool1` through `[monktool2`, `[necrotool1` through `[necrotool2` |
| Visible spell-bar close commands | Matching `close` families for Ancient, Bard, Knight, Death Knight, Elemental, Priest, Mage, Monk, and Necromancer bars. |
| Music | `[music`, `[musical` |

## Area Difficulty

The Help text lists seven prose labels:

| Label | Description shown in Help |
| --- | --- |
| Easy | Not much of a challenge. |
| Normal | An average level of challenge. |
| Difficult | A tad more difficult. |
| Challenging | You will probably run away a lot. |
| Hard | You will probably die a lot. |
| Deadly | I dare you. |
| Epic | For Titans of Ether. |

The actual dungeon-difficulty lookup lives outside `HelpGump` in `Worlds.GetDifficultyLevel(Point3D loc, Map map)`. It initializes `Heat` to `-5`, checks hardcoded region names by map, and returns the resulting integer. Its comment describes dungeon levels from `0` (`NEWBIE`) to `1` (`NORMAL`) up to `5` (`EPIC`), but the traced hardcoded region assignments commonly return `0`, `1`, `2`, `3`, or `4`, with some special `-1` values.

`BaseCreature` uses positive heat for creature scaling. If `Heat > 0`, non-paragon creatures are passed to `BeefUp(this, Heat)`. `BeefUp()` caps the scaling value by fame, then applies these formulas when `up > 0`:

| Field | Formula |
| --- | --- |
| HitsMaxSeed | `HitsMaxSeed + (HitsMaxSeed * (0.1 * up))` |
| RawStr | `RawStr + (RawStr * (0.1 * up))` |
| RawInt | `RawInt + (RawInt * (0.3 * up))` |
| RawDex | `RawDex + (RawDex * (0.3 * up))` |
| Each positive skill base | `skill.Base + (skill.Base * (0.3 * up))` |
| DamageMin / DamageMax | `+ up` |
| Fame | `Fame + (Fame * (0.1 * up))`, capped at `32000` |
| Karma | `Karma + (Karma * (0.1 * up))`, absolute value capped at `32000` |
| Bonus gold | Existing backpack gold amount multiplied by `(0.1 * up)`, then added as new gold. |
| Minimum taming skill | `MinTameSkill + (MinTameSkill * (0.15 * up))` when `Tamable` is true. |

`AdditionalHitPoints()` is also called with the original difficulty input. If `DungeonDifficulty > 1`, it adds `DungeonDifficulty * 10%` more maximum hit points after the other scaling pass.

On death, non-paragon creatures with `Heat > 0` call `BeefUpLoot(this, Heat)`. That method adds one fame-tiered loot pack when `up >= Utility.Random(7)`.

## BasicsGump Content

`BasicsGump` does not register a command. It opens from the built-in `MyLibrary` entry `Basics` and renders `DynamicBook.BasicHelp()` in a scrollable gump. The same `BasicHelp()` text is also assigned to `BookGuideToAdventure` and `LoreGuidetoAdventure`.

The `BasicHelp()` text covers these broad topics as static prose:

| Topic | Notes |
| --- | --- |
| Interface controls | Right-click movement, paperdoll, menu bar, backpack, options, stats, skills, and status windows. |
| Discovery sources | Message of the day, sages, skill scrolls, and in-game experimentation. |
| Chat | `[c` starts the chat system and supports character-specific offline messages. |
| Citizens | Context menus, training limits, hire options, and murder warnings. |
| Banks | Bank speech, currency exchange, and bank checks. |
| Inns and taverns | Safe logout/social areas and tavern services. |
| Practice and combat | Training dummies, war mode, melee/ranged combat, followers, spells, criminals, and murderers. |
| Equipment | Weapon requirements, damage, durability, armor slots, meditation/stealth armor impact, and mage armor. |
| Travel and magic | Magic gates, recall runes, runebooks, and spellbooks. |
| Death | Resurrection options and low-stat/low-skill exception text. |

## Administrative Notes

There are no required GameMaster commands for this guide. `[Help` is a player-access command that sends the registered-command list. The Help request gump is opened by client help requests rather than by `[Help`.

## Known Issues

| Issue | Trace |
| --- | --- |
| `Scripts.csproj` includes `System\Help\HelpGump.cs` and several other parent-folder Help gump paths, but the traced files are under `System\Help\Gumps\`. |
| `EventSink_HelpRequest()` dereferences `e.Mobile.NetState.Gumps` without guarding `e`, `e.Mobile`, or `e.Mobile.NetState`. |
| `HelpGump.OnResponse()` and `BasicsGump.OnResponse()` dereference `NetState.Mobile` before null validation. |
| Many `HelpGump` settings and display paths cast `Mobile` directly to `PlayerMobile`, so any non-player `Mobile` reaching those pages can throw. |
| The static main Help text advertises `[cleardeck`, but the left navigation `Corpse Clear` button invokes `[corpseclear`; those commands are different corpse-cleanup paths. |
| `BasicHelp()` tells players that `[status` displays the stats window, but no registered `[Status`/`[status` command was found in the C# trace. |
| The Help text lists seven difficulty labels, while `Worlds.GetDifficultyLevel()` documents a `0` to `5` difficulty range and traced assignments commonly stop at `4`; no direct mapping from all seven labels to returned integer heat values was found in this guide code. |

## Source Trace

POST-BATCH-T reviewed this page on 2026-06-14T21:09:11.0049244-05:00 against current source and audit registers.

- Canonical status: Canonical.
- Queue rows: PBN-0119.
- Backlog rows: RB-06800.
- Audit registers used: documentation-truth-table.csv, runtime-hook-map.csv, serialization-register.csv, and project-truth-register.csv.

### Source Files Reviewed

- Data/Scripts/System/Help/Gumps/HelpGump.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/Basics.cs (CurrentFile)
- Data/Scripts/Items/Books/DynamicBook.cs (CurrentFile)
- Data/Scripts/System/Commands/Player/MyLibrary.cs (CurrentFile)
- Data/Scripts/System/Commands/Handlers.cs (CurrentFile)
- Data/System/Source/Commands.cs (CurrentFile)
- Data/Scripts/System/Misc/World.cs (CurrentFile)
- Data/Scripts/Mobiles/Base/BaseCreature.cs (CurrentFile)

### Runtime Evidence

- Hook summary: Command=3; Event=2; Gump=97; Initialize=4; Movement=3; Speech=3; Timer=8.
- Data/Scripts/Items/Books/DynamicBook.cs:L77 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L122 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L181 Gump OnResponse access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L452 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L460 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L468 Gump SendGump access=Internal
- Data/Scripts/Items/Books/DynamicBook.cs:L858 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Items/Books/DynamicBook.cs:L900 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L487 Timer CustomTimerSubclass access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L914 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L950 Timer Timer.DelayCall access=GlobalOrInternal
- Data/Scripts/Mobiles/Base/BaseCreature.cs:L5879 Timer Timer.DelayCall access=GlobalOrInternal
- Additional hook rows are recorded in runtime-hook-map.csv for this source set.

### Serialization Evidence

- Serialized rows matched: 25.
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.AlchemicalElixirs version=Unknown serialize=L1523 deserialize=L1529 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.AlchemicalMixtures version=Unknown serialize=L1563 deserialize=L1569 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookBottleCity version=Unknown serialize=L1017 deserialize=L1023 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookGuideToAdventure version=Unknown serialize=L977 deserialize=L984 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookofDeadClue version=Unknown serialize=L1071 deserialize=L1077 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.BookOfPoisons version=Unknown serialize=L1603 deserialize=L1609 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookDruidicHerbalism version=Unknown serialize=L888 deserialize=L894 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookElvesandOrks version=Unknown serialize=L1151 deserialize=L1157 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookNecroticAlchemy version=Unknown serialize=L847 deserialize=L853 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookTheLostTribeofSosaria version=Unknown serialize=L1327 deserialize=L1333 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.CBookTombofDurmas version=Unknown serialize=L1111 deserialize=L1117 alignment=AlignedByCountAndKnownTypes
- Data/Scripts/Items/Books/DynamicBook.cs:Server.Items.DynamicBook version=0 serialize=L482 deserialize=L498 alignment=CountMatchNeedsTypeReview:UnknownWrites=10
- Additional serializer rows are recorded in serialization-register.csv for this source set.

### Project And Runtime Coverage

- Data/Scripts/Items/Books/DynamicBook.cs=Keep
- Data/Scripts/Items/Books/DynamicBook.cs=Keep
- Data/Scripts/Mobiles/Base/BaseCreature.cs=Keep
- Data/Scripts/Mobiles/Base/BaseCreature.cs=Keep
- Data/Scripts/System/Commands/Handlers.cs=Keep
- Data/Scripts/System/Commands/Handlers.cs=Keep
- Data/Scripts/System/Commands/Player/Basics.cs=Keep
- Data/Scripts/System/Commands/Player/Basics.cs=Keep
- Data/Scripts/System/Commands/Player/MyLibrary.cs=Keep
- Data/Scripts/System/Commands/Player/MyLibrary.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Help/Gumps/HelpGump.cs=Keep
- Data/Scripts/System/Misc/World.cs=Keep
- Data/Scripts/System/Misc/World.cs=Keep

No C# source, project files, XML/config/data files, namespaces, serializers, gameplay behavior, or migration policy were changed in POST-BATCH-T.
