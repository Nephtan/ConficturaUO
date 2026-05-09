# Help Request System

## Overview
The Help Request System is the RunUO help-button and staff paging framework under `Server.Engines.Help`. Client help requests enter through the core packet handler, open the custom `HelpGump`, and share queue infrastructure with the older `PagePrompt`/`PagePromptGump` page submission classes.

The live queue is memory-only. It stores `PageEntry` objects in static `PageQueue` collections, updates staff reporting through `Server.Engines.Reports.PageInfo` when reporting is loaded, and uses file-backed predefined staff responses from `Data/System/CFG/pageresponse.cfg`.

## Source Files
| File | Type | Purpose |
| --- | --- | --- |
| `Data/System/Source/Network/PacketHandlers.cs` | Core packet handler | Registers packet `0x9B` and invokes `EventSink.HelpRequest`. |
| `Data/System/Source/EventSink.cs` | Core event bus | Defines `HelpRequestEventArgs`, the `HelpRequest` event, and `InvokeHelpRequest`. |
| `Data/Scripts/System/Help/Gumps/HelpGump.cs` | Gump | Handles the help-button event, displays self-service help options, duplicate-page cancellation menu, and the stuck-menu entry point. |
| `Data/Scripts/System/Help/PageQueue.cs` | Static queue | Defines `PageType`, `PageEntry`, `[Pages]`, queue state, staff notifications, status timers, and speech-log email forwarding. |
| `Data/Scripts/System/Help/PagePrompt.cs` | Prompt | Text-prompt page submission path that enqueues a `PageEntry`. |
| `Data/Scripts/System/Help/Gumps/PagePromptGump.cs` | Gump | Text-entry page submission gump that enqueues a `PageEntry`. |
| `Data/Scripts/System/Help/Gumps/PageQueueGump.cs` | Gump set | Staff queue list, staff page detail gump, predefined responses, player response indicator, and message delivery. |
| `Data/Scripts/System/Help/Gumps/PageResponseGump.cs` | Gump | Player-facing staff response dialog. |
| `Data/Scripts/System/Help/SpeechLog.cs` | Command/helper | `[SpeechLog]` command plus short-lived player speech capture. |
| `Data/Scripts/System/Help/Gumps/SpeechLogGump.cs` | Gump | Staff speech-log viewer. |
| `Data/Scripts/System/Help/StuckMenu.cs` | Gump | Safe-location teleport menu used by the help gump's "Stuck in World" option. |

## Entry Points
| Entry point | Access | Compiled behavior |
| --- | --- | --- |
| Client help packet `0x9B` | Connected client | Calls `EventSink.InvokeHelpRequest(new HelpRequestEventArgs(state.Mobile))`. |
| `HelpGump.Initialize()` | Script startup | Subscribes `EventSink_HelpRequest` to `EventSink.HelpRequest`. |
| `[Pages]` | `AccessLevel.Counselor` | Opens the current handled page, the queue list, or sends "The page queue is empty." |
| `[SpeechLog]` | `AccessLevel.GameMaster` | Targets a `PlayerMobile` and opens that player's speech log when access checks pass. |
| `PagePrompt` / `PagePromptGump` | No command registration found | Enqueues pages when invoked, but no source reference was found that opens either submission path from `HelpGump` or another command. |

## Player Help Flow
| Step | Compiled behavior |
| --- | --- |
| Help request event | `EventSink_HelpRequest` scans `e.Mobile.NetState.Gumps` and returns if a `HelpGump` is already open. |
| Paging eligibility | `PageQueue.CheckAllowedToPage` rejects `PlayerMobile` callers in `DesignContext` and players with `PagingSquelched`; non-`PlayerMobile` callers pass. |
| Existing queued page | If `PageQueue.Contains(e.Mobile)` is true, the player receives `ContainedMenu`. Choosing cancel removes only unhandled pages; handled pages remain unchanged. |
| No queued page | The player receives `new HelpGump(e.Mobile, 1)`. This gump is mostly a self-service command and settings hub. |
| Page submission | `PagePrompt.OnResponse` and `PagePromptGump.OnResponse` enqueue `new PageEntry(from, text, m_Type)`. The gump path rejects empty text; the prompt path does not. |

## Page Types
| `PageType` | Display name |
| --- | --- |
| `Bug` | `Bug` |
| `Stuck` | `Stuck` |
| `Account` | `Account` |
| `Question` | `Question` |
| `Suggestion` | `Suggestion` |
| `Other` | `Other` |
| `VerbalHarassment` | `Verbal Harassment` |
| `PhysicalHarassment` | `Physical Harassment` |

Only `VerbalHarassment` attaches a speech-log snapshot to the page.

## `PageEntry` Data
| Field | Source |
| --- | --- |
| Sender | Constructor `Mobile sender`. |
| Sent time | `DateTime.Now` at construction. |
| Message | `Utility.FixHtml(message)`. |
| Type | Constructor `PageType type`. |
| Original location | `sender.Location`. |
| Original map | `sender.Map`. |
| Speech log | Copied from `PlayerMobile.SpeechLog` only when the page type is in `SpeechLogAttachment`. |
| Staff report info | `new PageInfo(this)` added to `Reports.Reports.StaffHistory` when that history object exists. |
| Status timer | Starts immediately, ticks after 1 second and then every 2 minutes. |

The status timer sends the sender their queue position while they remain online. If the sender logs out or the entry leaves the queue, it records `[Logout]` when applicable and removes the page. If the page handler disconnects, the timer clears the handler.

## Queue Mechanics
| Operation | Compiled behavior |
| --- | --- |
| Enqueue | Adds the `PageEntry` to `m_List` and sets `m_KeyedBySender[entry.Sender] = entry`. |
| Staff notification | Every connected `NetState.Mobile` with `AccessLevel >= Counselor`, `AutoPageNotify`, and no currently handled page receives "A new page has been placed in the queue." |
| Staff availability check | A staff member counts as available only when they are at least `Counselor`, have `AutoPageNotify`, and moved within the last 10 minutes. |
| No active staff | The sender is told no staff are currently available, but the page remains queued. |
| Speech-log email | If `Email.SpeechLogPageAddresses` is configured and the entry has a speech log, `SendEmail` asynchronously sends page details and captured speech. |
| Remove | Stops the page timer, removes the entry from `m_List`, clears the sender map, and clears the handler map when a handler exists. |

## Staff Queue Gumps
| Gump | Behavior |
| --- | --- |
| `PageQueueGump` | Cleans offline senders from the queue, renders five pages per gump page, labels entries as `Unhandled` or `Handling`, and opens `PageEntryGump` for the selected entry. |
| `PageEntryGump` | Shows sent time, sender, handler, original page location, page type, message, response text box, predefined response page, and speech-log button when present. |
| `MessageSentGump` | Small player notification gump. Clicking it opens `PageResponseGump`. |
| `PageResponseGump` | Shows the staff response. Pressing OK closes it; any other response reopens `MessageSentGump`. |
| `PredefGump` | Lists, edits, orders, creates, and removes predefined responses backed by `pageresponse.cfg`. |
| `SpeechLogGump` | Shows up to 30 speech entries per page, including speaker name, account, and fixed HTML speech text. |

## Staff Page Actions
| Button | Condition | Effect |
| --- | --- | --- |
| Close | Handler is not the responder | Returns to `PageQueueGump`. |
| Go to Sender | Sender exists and is in a valid map | Moves staff to `Sender.Location` / `Sender.Map` and records `[Go Sender]`. |
| Go to Handler | Handler exists and is in a valid map | Moves staff to the handler and records `[Go Handler]`. |
| Go to Page Location | `PageMap` is not null or internal | Moves staff to the saved page location and records `[Go PageLoc]`. |
| Handle Page | Entry has no handler | Records `[Handling]` and sets `entry.Handler = state.Mobile`. |
| Delete Page | Entry has no handler | Records `[Deleting]`, removes the page, and returns to the queue. |
| Abandon Page | Responder is the handler | Records `[Abandoning]` and clears `entry.Handler`. |
| Page Handled | Responder is the handler | Records `[Handled]`, removes the page, clears handler, and returns to the queue. |
| Send Message | Response text entry exists | Records `[Response] <text>` and sends `MessageSentGump` to the page sender. |
| Predefined Response | Selected response index exists | Records `[PreDef] <title>` and sends the configured response message to the page sender. |
| View Speech Log | Entry has a speech log | Reopens `PageEntryGump` and opens `SpeechLogGump`. |

## Self-Service Help Buttons
The custom `HelpGump` is not limited to staff paging. It routes many buttons to commands and other gumps:

| Button ID | Label / comment | Result |
| --- | --- | --- |
| `1` | Main | Reopens `HelpGump` page 1. |
| `2` | AFK | Invokes `[afk]`, then reopens the AFK page. |
| `3` | Chat | Invokes `[c]`. |
| `4` | Corpse Clear | Invokes `[corpseclear]`, then reopens the corpse-clear page. |
| `5` | Corpse Search | Invokes `[corpse]`. |
| `6` | Emote | Invokes `[emote]`. |
| `7` | Magic Toolbars | Opens help page 7. |
| `8` | Moongate | Invokes `[magicgate]`. |
| `9` | MOTD | Opens `Joeku.MOTD.MOTD_Gump`. |
| `10` | Quests | Opens help page 10. |
| `11` | Quick Bar | Opens `QuickBar`. |
| `62` | Reagent Bar | Opens `RegBar`. |
| `12` | Settings | Opens help page 12. |
| `13` | Library | Opens `MyLibrary`. |
| `14` | Statistics | Opens `Server.Statistics.StatisticsGump`. |
| `15` | Stuck | Runs the stuck-menu checks and may open `StuckMenu`. |
| `16` | Weapon Abilities | Invokes `[sad]`. |
| `17` | Wealth Bar | Opens `WealthBar`. |
| `18` | Conversations | Opens `MyChat`. |
| `19` | Versions | Opens help page 19. |

## Stuck Menu
`HelpGump` only displays "Stuck in World" when the caller is outside immediate-logout regions, not in an owned house context, and not in prison, gargoyle, or safe regions. On response, the stuck action:

1. Moves the player to a house ban location when `BaseHouse.FindHouseAt(from)` returns an AoS-rules house.
2. Rejects jail regions with localized message `1041530`.
3. Otherwise requires `from.CanUseStuckMenu()`, `from.Region.CanUseStuckMenu(from)`, no combat within the last 30 seconds, not frozen, not criminal, and either AoS mode or fewer than 5 kills.
4. Opens `StuckMenu`, freezes the player, and starts a close timer.

`StuckMenu` chooses a safe destination list from `Worlds.GetMyWorld(...)`. Player-triggered use schedules a random delay from 10 to 120 seconds, records `UsedStuckMenu()`, freezes the player during the delay, teleports pets, and moves the player to the selected destination map and point.

## Speech Logs
`SpeechLog.Enabled` is hard-coded true. `PlayerMobile.OnSpeech` creates a `SpeechLog` while the player is online and records each speech event. Entries expire after 20 minutes; `MaxLength = 0` means there is no fixed entry count limit.

The `[SpeechLog]` command lets Game Masters target a `PlayerMobile`. Staff cannot inspect a target with equal or higher access unless the viewer is `Owner`. If the target has no log, the command reports that no speech log exists.

Speech logs are not RunUO world-save data. They are transient in-memory queues and are only copied into a page when a page type with `SpeechLogAttachment` is created.

## Serialization And Persistence
No class in `Data/Scripts/System/Help` overrides RunUO `Serialize` or `Deserialize`.

| State | Persistence model |
| --- | --- |
| Active page queue | Static runtime collections only; lost on server restart. |
| Page timers | Runtime `Timer` instances only. |
| Player speech logs | Runtime queues only. |
| Staff page history | Recorded through `Server.Engines.Reports.PageInfo` when `Reports.Reports.StaffHistory` exists. |
| Predefined staff responses | Plain text file `Data/System/CFG/pageresponse.cfg`, one tab-delimited title/message pair per line. |

## Known Issues
| Issue | Impact |
| --- | --- |
| `Scripts.csproj` references `System\Help\HelpGump.cs`, `PagePromptGump.cs`, `PageQueueGump.cs`, `PageResponseGump.cs`, and `SpeechLogGump.cs`, but those files are actually under `System\Help\Gumps\`. | The maintained Visual Studio/MSBuild workflow can miss these gump files or fail with missing source-file paths. |
| `PagePrompt` and `PagePromptGump` enqueue pages, but no command or `HelpGump` button was found that opens either class. | The staff queue exists, but player-facing page submission appears unreachable through the traced C# help-button path. |
| `HelpGump.EventSink_HelpRequest` assumes `e.Mobile` and `e.Mobile.NetState` are non-null before iterating `NetState.Gumps`. | A null `Mobile` or disconnected `NetState` reaching the event handler would throw. |
| `PageQueue.Enqueue` does not reject an existing sender entry before adding to `m_List`. | Any direct invocation path can create duplicate queue rows while only the newest entry is keyed by sender. |
| `PagePrompt.OnResponse` does not trim or validate empty text, unlike `PagePromptGump.OnResponse`. | Prompt-based pages can enter blank or whitespace-only messages if that prompt path is used. |
| `PredefGump` shows edit controls to `GameMaster` access, but `OnResponse` immediately returns unless the user is `Administrator`. | Game Masters can see predefined-response edit controls that do nothing. |
| `PageQueueGump` and `PageEntryGump` do not re-check access level in `OnResponse`. | The normal `[Pages]` command gate is `Counselor`, but the gump handlers rely on that entry path rather than validating staff access again. |
