# Chat System

## Overview
The Chat System is the `Knives.Chat3` package under `Data/Scripts/System/Chat`. It replaces and extends player communication with channel commands, private messages, friends and ignores, global staff monitoring, configurable filters, spam delays, IRC bridging, multi-server chat, and three gump skins.

The system is event-driven rather than item-based. `General.Configure()` hooks `WorldLoad` and `WorldSave`, while `General.Initialize()` hooks `Speech`, `Login`, and `CharacterCreated`. `Chat3Guild.Initialize()` also replaces the ASCII and Unicode speech packet handlers so RunUO guild chat packets can be routed through the `Guild` channel.

## Core Components
| Component | Type | Purpose |
| --- | --- | --- |
| `General` | Static coordinator | Loads/saves chat data, hooks speech/login/create events, dispatches gump skins, and handles global world speech monitoring. |
| `RUOVersion` | Command adapter | Registers chat commands with `Server.Commands.CommandSystem` under the compiled `RunUO_2_RC1` define. |
| `Channel` | Base channel class | Stores members, colors, commands, history, filter/delay flags, global/regional style, IRC forwarding, and persistence. |
| `Data` | Per-Mobile and global state | Stores all player options, friends, ignores, private messages, filters, spam settings, IRC settings, logging settings, and total chat count. |
| `Message` | Private mail record | Stores sender, subject, body, message type, received time, and read state. |
| `Filter` | Text filter | Masks configured filter terms and optionally applies warning, ban, or jail penalties. |
| `TrackSpam` | Static rate limiter | Tracks timestamped spam buckets by `Mobile` and string type. |
| `GumpPlus` and chat gumps | `Gump` framework | Shared callback/text-field framework and chat UI skins/options. |
| `IrcConnection` | TCP IRC bridge | Connects to one configured IRC server/channel and relays IRC channel text. |
| `MultiConnection` | TCP shard bridge | Runs or connects to a simple multi-server chat socket. |
| `ChatJail` | Optional jail adapter | Calls Xanthos Jail only when the `Use_Xanthos` compile symbol is enabled. |

## Entry Points
| Entry point | Compiled behavior |
| --- | --- |
| World load | Loads local text, help, filter file stub, `Data`, `Channel`, `GumpInfo`, then auto-connects IRC if enabled. |
| World save | Saves `Data`, `Channel`, and `GumpInfo`, then deletes messages older than seven days for players with `SevenDays` enabled. |
| Speech | If the speaker is recording a `SendMessageGump`, speech is appended to the pending message and suppressed. Otherwise optional global speech filtering runs, then global world listeners can receive `(Global) <World>` copies when outside 10 tiles. |
| Login | Shows private-message notification when unread mail exists, reports mailbox count when `WhenFull` is false, and notifies players who have the logging-in Mobile on their friend list with `FriendAlert`. |
| Character creation | New characters auto-join every channel whose `NewChars` flag is true. |
| Guild speech packets | Message type `Guild` is redirected to the Chat3 `Guild` channel; other speech falls back to `Mobile.DoSpeech`. |
| Party public message | Party chat remains RunUO party chat, but global world listeners outside the party can receive `(Global) <World->Party>` copies. |

## Commands
No chat command file in this package declares `[Usage]` or `[Description]` attributes. The actual command set is registered through `RUOVersion.AddCommand`.

| Command | Access | Parameters | Result |
| --- | --- | --- | --- |
| `[ViewAll]`, `[Va]` | Player | None | Opens the main chat list gump on the all-player page. |
| `[Mail]`, `[Ma]` | Player | None | Opens the main chat list gump on the mail page. |
| `[Friends]`, `[Fri]` | Player | None | Opens the main chat list gump on the friends page. |
| `[HelpContents]`, `[hc]` | Player | None | Opens `HelpContentsGump`. |
| `[pm <name> [text]]`, `[msg <name> [text]]` | Player | Name fragment and optional initial body | Searches loaded chat `Data` records whose `RawName` contains the fragment and opens `SendMessageGump` for one match, a selection gump for 2-10 matches, or sends an error for no/too many matches. |
| `[All <text>]` | GameMaster | Message text | Sends a staff-colored broadcast to every connected `NetState.Mobile`. |
| `[ChatErrors]`, `[ce]` | Counselor | Optional text | Opens the chat error log with no text, or adds the supplied text to the error log. |

Channel commands are dynamic. The predefined channel command aliases are:

| Channel | Commands | Access gate | Default flags |
| --- | --- | --- | --- |
| `Public` | `[chat]`, `[c]` | Any player who is joined and not squelched/chat-banned | `NewChars = true`, filter on, delay on, hue `0x47E`. |
| `Staff` | `[staff]`, `[st]` | `AccessLevel` above `Player` | Shows staff in lists, hue `0x26`, filter on, delay on. |
| `Guild` | `[guild]`, `[g]` | Sender must have `Mobile.Guild != null` | `NewChars = true`, filter off, delay off, hue `0x44`, separate history per guild. |
| `Faction` | `[faction]`, `[f]` | Sender must be a player in a faction | `NewChars = true`, filter on, delay on, hue `0x17`. |
| `Alliance` | `[ally]`, `[a]` | Sender must have a guild | `NewChars = true`, no persisted history, sends to guildmates and allied guilds. |
| `IRC` | `[irc]`, `[i]` | IRC connection must be active | `NewChars = true`, hue `0x1FC`, forwards to configured IRC room. |
| `Multi` | `[Mult]`, `[mu]` | Multi-server connection must be active | Hue `0x1FC`, forwards to `MultiConnection`. |

## Channel Flow
Calling a channel command with no message opens the chat list gump. If the caller is already a member of that channel, it also sets `Data.CurrentChannel` to that channel.

Calling a channel command with message text performs this sequence:

1. `CanChat(m, true)` rejects disabled channels, squelched Mobiles, chat-banned Mobiles, and regional channels where `Worlds.GetRegionName(m.Map, m.Location)` is empty.
2. If the channel `Filter` flag is true, `Filter.FilterText` masks configured terms and may apply penalties.
3. `CanChat(m, false)` runs again after filtering.
4. The sender must already be in `c_Mobiles`, otherwise they receive the "must join the channel first" message.
5. If the channel `Delay` flag is true, `TrackSpam.LogSpam(m, "Chat", TimeSpan.FromSeconds(Data.ChatSpam))` must pass. Failure sends the spam warning and schedules a retry after 4 seconds.
6. The message is added to channel history and history is trimmed to 50 entries.
7. `Events.InvokeChat` fires.
8. If chat logging is enabled, the message is appended to `Data/Logs/Chat/Chat-<long date>.log`.
9. `Data.TotalChats` increments and the sender's chat Karma setter is invoked.
10. The channel broadcasts to members, staff/global listeners, and optionally IRC.

`Channel.Broadcast` sends normal channel text only to joined Mobiles who have not ignored the sender. Regional channels also require the recipient to be in the same `Region` object. Separate global listener flags exist for chat, world speech, private mail, guild, and faction traffic.

## Private Mail
Private mail is stored as `Message` records under the receiving player's `Data.Messages`.

| Rule | Compiled behavior |
| --- | --- |
| Direct self-message | Rejected by `Message.CanMessage`. |
| Staff bypass | If `from.AccessLevel > to.AccessLevel`, `CanMessage` returns true before checking bans, ignores, friends-only settings, or mailbox size. |
| Chat bans | Rejected when either sender or recipient `Data.Banned` is true, except for the staff bypass above. |
| Friends-only | Rejected if either side has `FriendsOnly` enabled and the other Mobile is not in that side's `Friends` list. |
| Ignores | Rejected if either side ignores the other. |
| Full mailbox | Rejected when recipient message count is at least `Data.MaxMsgs` and recipient `WhenFull` is false. |
| Message spam | `SendMessageGump.Send` checks `TrackSpam.LogSpam(Owner, "Message", TimeSpan.FromSeconds(Data.MsgSpam))`. |
| Message filtering | If `Data.FilterMsg` is true, subject and body are filtered without punishment. |
| Full mailbox auto-delete | If recipient `WhenFull` is true and message count exceeds `Data.MaxMsgs`, the oldest message is removed. |
| Login notification | Unread mail opens a PM notify gump on login. |
| Read receipt | Opening a message can send a read receipt to the sender when the recipient has `ReadReceipt` enabled. |
| Staff timeout | Replies to staff-originated normal messages are suppressed after 5 hours. |

`MsgType.System` broadcasts a message record to every loaded `Data` record. `MsgType.Staff` sends only to non-player access levels. `MsgType.Invite` is used for friend requests; accepting adds each player to the other's friend list.

## Moderation And Staff Controls
Most moderation controls are gump-driven through `ListGump`, `ProfileGump`, and admin option gumps.

| Control | Required access in code | Effect |
| --- | --- | --- |
| Chat ban | `GameMaster` controls are shown for player targets | Opens a ban duration gump with 30 minutes, 1 hour, 12 hours, 1 day, 1 week, 1 month, or 1 year. `Data.Ban` sets `BannedUntil`, marks `Banned`, and schedules `RemoveBan`. |
| Remove chat ban | Same ban control | Calls `Data.RemoveBan`. |
| Global access grant/revoke | `Administrator` owner and staff target below Administrator | Toggles the target's `GlobalAccess`. |
| Global monitor mode | User with `GlobalAccess` | Chooses monitor mode (`Global = true`) or listen-list mode (`Global = false`), then toggles chat/world/mail/guild/faction/IRC raw streams. |
| Global ignore/listen | User with `GlobalAccess` | Adds/removes Mobiles from `GIgnores` or `GListens`. |
| Goto/client | `GameMaster` owner and online target | Moves owner to target or opens `ClientGump` for target `NetState`. |
| System/staff broadcast | Shown to staff from the main list gump | Opens `SendMessageGump` with `MsgType.System` or `MsgType.Staff`. |
| Error log | `Counselor` command | Opens or appends to the in-memory chat error log. |

## Filter And Spam Settings
| Setting | Default | Meaning |
| --- | --- | --- |
| `Data.FilterSpeech` | `false` | Applies filter masking to ordinary world speech. |
| `Data.FilterMsg` | `false` | Applies filter masking to private-message subject/body without punishment. |
| Channel `Filter` | `true` for most channels | Applies filter masking and penalties to channel messages. |
| `Data.FilterPenalty` | `None` | Penalty after too many filter warnings: `None`, `Ban`, or `Jail`. |
| `Data.FilterWarnings` | `3` | Warnings allowed before penalty. |
| `Data.FilterBanLength` | `5` | Chat-ban length in minutes for filter penalty `Ban`. |
| `Data.ChatSpam` | `2` | Channel delay window in seconds. |
| `Data.MsgSpam` | `5` | Private-message delay window in seconds. |
| `Data.RequestSpam` | `24` | Friend-request delay window in hours. |
| `Data.AntiMacroDelay` | `60` | Seconds before an anti-macro notification penalty fires. |

Filtering lowercases filter terms in the gump and masks each matched term with the same number of `*` characters. Player filter violations increment `Data.Warnings` only when `punish` is true. When warnings exceed `Data.FilterWarnings`, warnings reset to `0`, `Events.InvokeFilterViolation` fires, and the selected penalty is applied.

## IRC And Multi-Server Chat
`IrcConnection` uses `TcpClient`, `StreamReader`, and `StreamWriter` to connect to the configured IRC server, send `USER`/`NICK`, join `Data.IrcRoom`, and relay `PRIVMSG` text. The IRC room is forced to begin with `#` by the `Data.IrcRoom` setter. IRC names from numeric `353` are stored in `Data.IrcList` and displayed in the IRC channel list. `!status` in either direction sends the server status string if the 15 second status throttle has elapsed.

`MultiConnection` can either run a master TCP socket on `Data.MultiPort` or connect as a slave to `Data.MultiServer:Data.MultiPort`. The `Multi` channel sends messages into the connection, and incoming messages are broadcast through the `Multi` channel. The master can block connected server names through `MultiGump`, which immediately closes the matching socket.

## Persistence
All chat persistence uses `General.SavePath`, currently `Saves/ChatBeta8`.

| File | Writer version | Contents |
| --- | --- | --- |
| `GlobalOptions.bin` | `2` | Multi port/server, notifications, filter terms, filter/spam settings, IRC settings, logging flags, IRC staff color, and total chat count plus one. |
| `PlayerOptions.bin` | wrapper `0`, per-player options `3` | Per-Mobile colors, skin, global monitor settings, ban state, mail settings, IRC raw, away message, signature, karma, status, and ban expiry. |
| `Friends.bin` | wrapper `0`, per-player list `1` | Friend mobile list. |
| `Ignores.bin` | wrapper `0`, per-player list `1` | Ignore mobile list. |
| `GlobalListens.bin` | wrapper `0`, per-player list `1` | Global ignore and global listen mobile lists. |
| `Pms.bin` | wrapper `0`, per-player list `1`; message `0` | Private messages. |
| `Channels.bin` | wrapper `0`, per-channel `1` | Channel type name, joined Mobiles, filter/delay/name/style/IRC/new-character/show-staff/enabled flags, colors, and command aliases. |
| `Gumps.bin` | wrapper `0`, per-gump info `0` | Forced and per-player gump backgrounds, transparency, and text color. |

Channel histories are runtime-only and are not written to `Channels.bin`. `Guild` keeps runtime history per `Mobile.Guild`; `Alliance` explicitly returns no history and no-ops its history methods.

## Events For Integrations
The package exposes four static events:

| Event | Fired from |
| --- | --- |
| `Events.Chat` | After a channel message is accepted and added to history. |
| `Events.FilterViolation` | When filter warnings exceed the configured threshold. |
| `Events.Error` | When `Errors.Report` records a chat-system error. |
| `Events.GumpCreated` | Every `GumpPlus` constructor. |

## Known Issues
- `FilterPenalty.Jail` is effectively a no-op in the current compile because `Jail.cs` has `//#define Use_Xanthos`, and `ChatJail.SendToJail` contains only `#if (Use_Xanthos)` code.
- `TrackSpam.LogSpam` does not record a timestamp when a `Mobile` already has a spam table but the requested spam `type` is new. After the first recorded type, other spam buckets such as `Message` or individual friend-request keys can remain unthrottled.
- `SendMessageGump.Send` does not re-run `Message.CanMessage` before normal message delivery. A stale send gump can deliver after the target's ignore, friends-only, ban, or mailbox state changes.
- `General.LoadHelpFile`, `LoadFilterFile`, `LoadBacksFile`, `LoadColorsFile`, and `LoadAvatarFile` are empty stubs. `[HelpContents]` has no loaded help topics unless another script populates `General.Help`, and the reload buttons do not load external files.
- `Channel.Load` tries `Activator.CreateInstance(ScriptCompiler.FindTypeByFullName(reader.ReadString()))` before checking for a missing type. If a saved channel type name no longer resolves, the fallback `new Channel()` path is not reached and channel loading falls into the outer catch.
- `MultiGump.MultiBlock` only adds to `Data.MultiBlocks` and closes the named socket; it does not toggle or remove blocks. `Data.MultiBlocks` and `Data.MultiMaster` are also not written to `GlobalOptions.bin`, so they do not persist across restarts.
- `GumpPlus.UniqueTextId` checks `c_Buttons[random]` instead of `c_Fields[random]`, so generated text-entry IDs are not checked against existing text-entry IDs.
