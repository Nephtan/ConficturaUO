# In-Game Command List

## Scope

This page documents the player-facing command guide rendered by `Server.Engines.Help.HelpGump` page `1`.
The guide is a `Gump` text page plus button dispatch into existing player command handlers. It does not define a new `Item`, `Mobile`, packet handler, XMLSpawner attachment, save object, or serializer.

## Core Scripts

| Script | Role |
| --- | --- |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Registers the Help request event hook, builds the Help Gump navigation, renders `MyHelp()` on page `1`, and dispatches Help button replies. |
| `Data/System/Source/Commands.cs` | Provides `CommandSystem.Prefix`, command registration, case-insensitive command lookup, argument splitting, and access checks. |
| `Data/Scripts/System/Commands/Handlers.cs` | Sets the shard command prefix to `[` and registers the core `[Help` command. |
| `Data/Scripts/System/Commands/Player/*.cs` | Defines most player commands listed by the Help text, including AFK, bandaging, emotes, loot settings, bars, play styles, and skill commands. |
| `Data/Scripts/System/Skills/Weapon Abilities/*.cs` | Defines weapon ability toolbar commands such as `[sad`, `[set1` through `[set5`, and `[abilitynames`. |
| `Data/Scripts/System/Misc/Accounts.cs` | Defines `[Password <newPassword> <repeatPassword>` when the password command flag is enabled. |
| `Data/Scripts/System/Misc/MOTD.cs` | Defines `[MOTD` and opens the message-of-the-day gump. |
| `Data/Scripts/System/Misc/Statistics.cs` | Defines `[Statistics` when the statistics system is enabled. |
| `Data/Scripts/Quests/Quests.cs` | Defines `[quests` and opens the quest gump. |
| `Data/Scripts/Custom/ClearDeckCommand/ClearDeckCommand.cs` | Defines `[ClearDeck` for deleting eligible corpses from the caller's current boat. |

## Entry Points

`HelpGump.Initialize()` subscribes to `EventSink.HelpRequest`. When the client help request arrives, `EventSink_HelpRequest()` checks whether the caller already has a `HelpGump`, verifies `PageQueue.CheckAllowedToPage()`, and then either opens a duplicate-page menu or sends `new HelpGump(e.Mobile, 1)`.

The `HelpGump(Mobile from, int page)` constructor draws the left navigation and, when `page == 1`, places the complete `MyHelp()` HTML string in the main scroll area. The gump always plays sound `0x4A` when opened.

`HelpGump.OnResponse(NetState state, RelayInfo info)` uses `Mobile from = state.Mobile`, plays sound `0x4A`, closes the current Help Gump, and dispatches by `info.ButtonID`. `InvokeCommand(string c, Mobile from)` runs `CommandSystem.Handle(from, String.Format("{0}{1}", CommandSystem.Prefix, c))`, so the active command prefix is prepended before command execution.

There is no persistent object state and no `Serialize` or `Deserialize` block for this guide.

## Help Navigation

| Button ID | Label | Compiled action |
| ---: | --- | --- |
| `1` | Main | Reopens Help page `1`, which displays `MyHelp()`. |
| `2` | AFK | Invokes `[afk`, then redraws page `2`. |
| `3` | Chat | Invokes `[c`. |
| `4` | Corpse Clear | Invokes `[corpseclear`, then redraws page `4`. |
| `5` | Corpse Search | Invokes `[corpse`. |
| `6` | Emote | Invokes `[emote`. |
| `7` | Magic Toolbars | Opens Help page `7`; toolbar buttons then open editor gumps or invoke toolbar open/close commands. |
| `8` | Moongate Search | Invokes `[magicgate`. |
| `9` | MOTD | Sends the MOTD gump through `Joeku.MOTD.MOTD_Utility.SendGump()`. |
| `10` | Quests | Opens Help page `10`, which appends `MyQuests(from)` to the quest guide text. |
| `11` | Quick Bar | Opens `QuickBar`. |
| `62` | Reagent Bar | Opens `RegBar`. |
| `12` | Settings | Opens Help page `12`, a settings panel with direct toggles and info buttons. |
| `13` | Library | Opens `MyLibrary`. |
| `14` | Statistics | Opens `Server.Statistics.StatisticsGump`. |
| `15` | Stuck in World | Appears only in unsafe logout regions outside owner houses and excluded regions. It either moves an AOS house owner to the house ban location or opens `StuckMenu` if combat/criminal/frozen/region checks pass. |
| `16` | Weapon Abilities | Invokes `[sad`. |
| `17` | Wealth Bar | Opens `WealthBar`. |
| `18` | Conversations | Opens `MyChat`. |
| `19` | Version | Opens Help page `19` with `MyServerSettings.Versions()`. |

## Main Guide Content

`MyHelp()` starts by directing players toward in-world information sources:

| Source named by guide text | Compiled source of the text |
| --- | --- |
| Merchant scrolls and books | Static `MyHelp()` string only. |
| Sage tomes for skills, weapon abilities, and magic | Static `MyHelp()` string only. |
| `Guide to Adventure` book from a sage | Static `MyHelp()` string only. |
| Townsfolk conversation | Static `MyHelp()` string only. |
| Paperdoll `Info` section | Static `MyHelp()` string only. |

The guide then lists common commands, area difficulty labels, skill-title setup, reagent bar commands, magic toolbar command families, music commands, and the evil/oriental/barbaric play-style summaries.

## Common Commands

All commands below are shown by `MyHelp()`. The command registry is case-insensitive, so `[Statistics`, `[statistics`, `[ClearDeck`, and `[cleardeck` resolve through the same registered command names when access checks pass.

| Command shown | Access | Compiled behavior traced |
| --- | --- | --- |
| `[abilitynames` | Player | Toggles `PlayerMobile.CharacterWepAbNames` for labels on the weapon ability toolbar. |
| `[afk` | Player | Toggles AFK state in `AFK.m_AFK`; speech, death, and logout clear the AFK timer. |
| `[ancient` | Player | Toggles ancient magic casting between the ancient spellbook and research bag paths. |
| `[autoattack` | Player | Toggles `Mobile.NoAutoAttack`; false means auto attack is enabled. |
| `[bandother` | Player | Finds a `Bandage` in the caller's backpack and starts the bandage-other target flow. |
| `[bandself` | Player | Finds a `Bandage` in the caller's backpack and applies the bandage-to-self command path. |
| `[barbaric` | Player | Toggles the barbaric play style and related `PlayerMobile` state. |
| `[c` | Player chat channel command | Opens or routes into the public chat system's `c` command alias. |
| `[corpse` | Player | Searches up to 1000 tiles for the caller's owned non-empty corpse in the same area and creates a directional quest arrow. |
| `[cleardeck` | Player | Deletes eligible NPC corpses and empty player corpses from the boat the caller is standing on. |
| `[e` | Player | Opens or uses the mini emote command path. |
| `[emote` | Player | Opens or uses the full emote command path. |
| `[evil` | Player | Toggles the evil play style and related `PlayerMobile` state. |
| `[loot` | Player | Opens automatic loot-option setup. |
| `[magicgate` | Player | Searches up to 1000 tiles for the nearest `GateMoon`, `moongates`, or `Moongate` in the same area and creates a directional quest arrow. |
| `[motd` | Player | Opens the message-of-the-day gump. |
| `[oriental` | Player | Toggles the oriental play style and related `PlayerMobile` state. |
| `[password <newPassword> <repeatPassword>` | Player | Changes the caller account password when the account and login IP checks pass. |
| `[poisons` | Player | Toggles `PlayerMobile.ClassicPoisoning` between precision infectious-strike poison behavior and one-handed slashing/piercing weapon-hit poison behavior. |
| `[private` | Player | Toggles town-crier/local chatter privacy through `PlayerMobile.PublicMyRunUO`. |
| `[quests` | Player | Opens the quest gump. |
| `[quickbar` | Player | Opens the vertical `QuickBar` gump. |
| `[sad` | Player | Opens the weapon special attacks display through the `SAD` alias. |
| `[set1` through `[set5` | Player | Selects weapon ability slots from the open `SpecialAttackGump`; higher slots return without action if the gump lacks that many abilities. |
| `[sheathe` | Player | Toggles `PlayerMobile.CharacterSheath` when auto-sheathe command registration is enabled. |
| `[skill` | Player | Opens the skill definition gump. |
| `[skilllist` | Player | Opens the compact watched-skill list. |
| `[spellhue <number>` | Player | Parses the first argument with `GetInt32(0)` and stores it in `PlayerMobile.MagerySpellHue`. Missing arguments store `0`. |
| `[statistics` | Player by default | Opens the statistics gump when `Statistics.Config.Enabled` is true and `Config.CanSeeStats` remains `AccessLevel.Player`. |
| `[wealth` | Player | Opens the wealth tracking bar. |

## Skill Title Command

The guide text documents:

```text
[SkillName "taming"
```

The actual registered command is `[SkillName <name>]`. Its handler stores an advertised skill title choice; the text says using `clear` returns title handling to the game's automatic behavior.

## Reagent Bar Commands

| Command | Access | Compiled behavior |
| --- | --- | --- |
| `[regbar` | Player | Opens the reagent bar. |
| `[regclose` | Player | Closes the reagent bar. |

## Magic Toolbar Commands

The main command list includes the same toolbar families documented in the dedicated Magic Toolbars guide.

| Command family | Meaning |
| --- | --- |
| `[archspell1` through `[archspell4` | Open Ancient/Archmage spell bar editors. |
| `[bardsong1`, `[bardsong2` | Open Bard song bar editors. |
| `[knightspell1`, `[knightspell2` | Open Knight spell bar editors. |
| `[deathspell1`, `[deathspell2` | Open Death Knight spell bar editors. |
| `[elementspell1`, `[elementspell2` | Open Elementalist spell bar editors. |
| `[holyspell1`, `[holyspell2` | Open Priest prayer bar editors. |
| `[magespell1` through `[magespell4` | Open Magery spell bar editors. |
| `[monkspell1`, `[monkspell2` | Open Monk ability bar editors. |
| `[necrospell1`, `[necrospell2` | Open Necromancer spell bar editors. |
| `*tool*` families | Open visible spell or ability bars. |
| `*close*` families | Close visible spell or ability bars. |

## Area Difficulty Labels

The guide text lists these labels only as explanatory prose; no formula is defined in `HelpGump` for assigning them to regions.

| Label | Description shown by guide text |
| --- | --- |
| Easy | Not much of a challenge. |
| Normal | An average level of challenge. |
| Difficult | A tad more difficult. |
| Challenging | You will probably run away a lot. |
| Hard | You will probably die a lot. |
| Deadly | I dare you. |
| Epic | For Titans of Ether. |

## Music Commands

| Command | Access | Compiled behavior |
| --- | --- | --- |
| `[music` | Player | Opens the music playlist/player gump. |
| `[musical` | Player | Toggles the dungeon music preference. |

## Administrative Notes

There are no required staff or GameMaster commands for this guide. The only non-player command discovered near this feature is `[UpdateStatistics`, which is not listed by `MyHelp()` and defaults to `AccessLevel.Seer`; it is part of the statistics subsystem, not the command-list guide.

## Known Issues

| Issue | Trace |
| --- | --- |
| `Scripts.csproj` points at `System\Help\HelpGump.cs`, but the discovered source file is `System\Help\Gumps\HelpGump.cs`. The maintained project file therefore does not reference the actual Help Gump path. |
| `EventSink_HelpRequest()` dereferences `e.Mobile.NetState.Gumps` without checking `e`, `e.Mobile`, or `e.Mobile.NetState`. |
| `OnResponse()` dereferences `state.Mobile` before any `NetState` or `Mobile` guard, then many settings cases cast `Mobile` directly to `PlayerMobile`. |
| The Help text advertises `[cleardeck`, but the left navigation `Corpse Clear` button invokes `[corpseclear`; these are separate command paths with different behavior. |
| The Help text describes `[spellhue ##`, while the command metadata says `spellhue [<name>]` and the handler actually parses the first argument as an integer hue. |
