# System Name: Voting Rewards

## Overview

The Voting Rewards audit entry maps to the custom `Server.Voting` scripts under `Data/Scripts/Custom/Voting`. The compiled system is a vote-link and cooldown framework: players run `[Vote` or double-click a `VoteStone`, the shard validates a configured vote URL, launches that URL in the player's client browser, and stores the last vote-request timestamp on the player's account.

No traced C# script in `Data/Scripts/Custom/Voting` grants an automated reward, receives an external callback, parses a vote-service response, or verifies that a vote was actually completed. The only persistent player-facing state is the account tag used for cooldown enforcement.

Code-Verified: 2026-05-07

## Script Inventory

| Script | Namespace | Role |
| --- | --- | --- |
| `Data/Scripts/Custom/Voting/VoteCommand.cs` | `Server.Voting` | Registers `[Vote`, `[VoteConfig`, and `[VoteInstance`. |
| `Data/Scripts/Custom/Voting/VoteConfig.cs` | `Server.Voting` | Stores global default vote-site settings and persists them to `Data/VoteSystem.cfg`. |
| `Data/Scripts/Custom/Voting/VoteEvents.cs` | `Server.Voting` | Defines the vote request event, default request handler, and `VoteRequestEventArgs`. |
| `Data/Scripts/Custom/Voting/VoteHelper.cs` | `Server.Voting` | Validates URLs, formats cooldown text, dispatches vote requests, and reads/writes account cooldown tags. |
| `Data/Scripts/Custom/Voting/VoteItem.cs` | `Server.Voting` | Base `Item` implementation that owns one `VoteSite` and performs vote launch/cooldown behavior. |
| `Data/Scripts/Custom/Voting/VoteSite.cs` | `Server.Voting` | Property object for one vote site profile: parent item, name, URL, cooldown, and validity. |
| `Data/Scripts/Custom/Voting/VoteStatus.cs` | `Server.Voting` | Enum for `Invalid`, `Success`, `TooEarly`, and `Custom` vote request outcomes. |
| `Data/Scripts/Custom/Voting/VoteStone.cs` | `Server.Items` | Constructable world `Item` that inherits `VoteItem` and displays vote-site properties. |

`Data/Scripts/Scripts.csproj` explicitly compiles all eight Voting scripts.

## Command Surface

| Command | Access | Usage attribute | Description attribute | Behavior |
| --- | --- | --- | --- | --- |
| `[Vote` | `Player` | `Vote` | `Cast a vote for your shard.` | Guards null/deleted callers, then calls `VoteItem.Instance.CastVote(e.Mobile)`. |
| `[VoteConfig` | `GameMaster` | `VoteConfig` | `Gets the property list for the Vote System config.` | Opens a `PropertiesGump` for `VoteConfig.Instance`. |
| `[VoteInstance` | `GameMaster` | `VoteInstance` | `Gets the property list for the internal Vote Item instance.` | Opens a `PropertiesGump` for the hidden singleton `VoteItem.Instance`. |

The commands do not accept parameters. Game Masters configure the system through standard RunUO property editing rather than custom command arguments.

## Configuration Defaults

`VoteConfig` is a singleton `[PropertyObject]` exposed through `[VoteConfig`.

| Property | Access | Default source value | Meaning |
| --- | --- | --- | --- |
| `DefaultName` | `GameMaster` | Random `NameList.RandomName("UOGateway.com")` | Initial `VoteSite.Name` value for new vote profiles. |
| `DefaultURL` | `GameMaster` | `https://www.uogateway.com/shard.php?id=1429&act=vote` | Initial URL launched for vote requests. |
| `DefaultCoolDown` | `GameMaster` | `TimeSpan.FromHours(24.0)` | Initial delay before the same account may request the same vote site again. |

`VoteSite` instances copy these defaults when they are constructed. Later edits to `VoteConfig.Instance` do not automatically rewrite already-created `VoteSite` instances.

## Vote Site Profile

Each `VoteItem` owns one `VoteSite`.

| Property | Access | Stored field | Behavior |
| --- | --- | --- | --- |
| `Parent` | `GameMaster` getter | `_Parent` | Back-reference to the owning `VoteItem`. |
| `Name` | `GameMaster` | `_Name` | Used in player messages and in the account tag name. |
| `URL` | `GameMaster` | `_URL` | Launched through `Mobile.LaunchBrowser` on successful requests. |
| `CoolDown` | `GameMaster` | `_CoolDown` | Required elapsed time since the account's last stored request for this site. |
| `Valid` | `GameMaster` getter | Computed | Returns true when `new Uri(URL)` succeeds. |

Changing `Name`, `URL`, or `CoolDown` calls `VoteSite.InvalidateProperties()`, which refreshes the parent item properties if the parent still exists.

## Player Vote Flow

1. A player runs `[Vote`, or double-clicks a `VoteItem` such as `VoteStone`.
2. The entry point calls `VoteHelper.CastVote(Mobile from, VoteSite site)`.
3. `VoteHelper.CastVote` guards null/deleted mobiles and null sites, then raises `VoteEvents.InvokeVoteRequest`.
4. The internal vote request handler reads `VoteRequestEventArgs.CanVote`, which checks the account cooldown tag for this mobile and site.
5. If `VoteSite.Valid` is false, the parent receives `VoteStatus.Invalid`.
6. If the site is valid and cooldown has elapsed, `VoteItem.OnBeforeVote(from)` runs.
7. The base `VoteItem.OnBeforeVote` returns true, so stock `VoteItem` and `VoteStone` requests continue as `VoteStatus.Success`.
8. On success, `VoteItem.OnVote` optionally sends `Thank you for voting on {site}!`, calls `Mobile.LaunchBrowser(VoteSite.URL)`, and writes the account's last-vote timestamp.
9. If cooldown has not elapsed, the parent receives `VoteStatus.TooEarly` and optionally sends the remaining time message.
10. `VoteItem.OnAfterVote` runs after every status, but the base method is empty.

## Cooldown State

Cooldowns are stored on the caller's `Account`, not on `PlayerMobile`.

| Data | Source |
| --- | --- |
| Account tag prefix | `VS_LAST_VOTE_` |
| Full tag key | `VS_LAST_VOTE_` + `VoteSite.Name.ToUpper()` |
| Tag value | `DateTime.Now.ToString()` |
| Read path | `VoteHelper.GetLastVoteTime(Mobile, VoteSite, out bool canVote, out TimeSpan timeLeft)` |
| Write path | `VoteHelper.SetLastVoteTime(Mobile, VoteSite)` |
| Cooldown comparison | `lastVoteTime.Add(voteSite.CoolDown) < DateTime.Now` |

Because the tag key uses `VoteSite.Name.ToUpper()`, renaming a vote site creates a different cooldown bucket. The old account tag is not migrated or deleted by this system.

When no tag exists, `GetLastVoteTime` creates the account tag, but returns a backdated local timestamp so the request is treated as immediately eligible.

## Player Messages

`VoteItem.Messages` is a `[CommandProperty(AccessLevel.GameMaster)]` bool. When true, the base `VoteItem.OnVote` sends these status messages:

| Status | Message behavior |
| --- | --- |
| `Success` with valid site | `Thank you for voting on {VoteSite.Name}!`, then browser launch. |
| `Success` with invalid site | Hue `0x22`: `Sorry, voting is currently unavailable.` |
| `Invalid` | Hue `0x22`: `Sorry, voting is currently unavailable.` |
| `TooEarly` | Hue `0x22`: `Sorry, you can not vote at {VoteSite.Name} for {formatted time}.` |
| `Custom` | No base message. |

`VoteHelper.GetFormattedTime(TimeSpan)` only includes hour and minute components. It omits seconds and omits day totals beyond the hour component exposed by `TimeSpan.Hours`.

## Vote Stone Item

`VoteStone` is a constructable `Item` in `Server.Items` that inherits `VoteItem`.

| Constructable | Behavior |
| --- | --- |
| `VoteStone()` | Creates a stone named `Vote Stone`. |
| `VoteStone(string name)` | Creates a named vote stone with hue `0`. |
| `VoteStone(string name, int hue)` | Creates a named, hued vote stone with item ID `0xED4`. |

The constructor sets `Movable = false`. Its default `LabelMessage` is `Use: Launches your browser to cast a vote for ` plus `ServerList.ServerName`.

`GetProperties(ObjectPropertyList list)` adds a green property line when the configured `VoteSite.Valid` is true and a red unavailable line when it is false. The valid line includes:

| Display component | Source |
| --- | --- |
| Usage text | `VoteStone.LabelMessage` |
| Site name | `VoteSite.Name` |
| Site URL | `VoteSite.URL.ToUpper()` |

`VoteStone` does not add reward behavior. Its `OnBeforeVote`, `OnVote`, and `OnAfterVote` overrides delegate directly to `VoteItem`.

## Event Extension Points

The system exposes a public static `VoteEvents.VoteRequest` event. The default internal handler always runs first when `InvokeVoteRequest` is called, then any external subscribers are invoked.

| Extension point | Base behavior |
| --- | --- |
| `VoteItem.OnBeforeVote(Mobile from)` | Returns true. Returning false from a derived item changes the status to `VoteStatus.Custom`. |
| `VoteItem.OnVote(Mobile from, VoteStatus status)` | Handles messages, browser launch, and cooldown writes. |
| `VoteItem.OnAfterVote(Mobile from, VoteStatus status)` | Empty hook after status handling. |
| `VoteEvents.VoteRequest` | Optional static event for additional listeners. |

No current traced custom Voting script subscribes to `VoteEvents.VoteRequest` to grant rewards.

## Persistence

### `VoteConfig`

`VoteConfig.Initialize()` hooks:

| EventSink hook | Behavior |
| --- | --- |
| `EventSink.ServerStarted` | Calls `VoteConfig.Instance.Deserialize()`. |
| `EventSink.WorldSave` | Calls `VoteConfig.Instance.Serialize()`. |

`VoteConfig` persists to `Data/VoteSystem.cfg`, not to the normal world item stream.

| Version | Write/read order |
| --- | --- |
| `0` | `_DefaultName`, `_DefaultURL`, `_DefaultCoolDown` |

If `Data/VoteSystem.cfg` does not exist, serialization/deserialization creates it. If the file is empty, deserialization returns without changing defaults.

### `VoteItem`

`VoteItem.Serialize()` calls `base.Serialize(writer)`, writes version `0`, then writes:

1. `_Messages`
2. `VoteSite.Serialize(writer)`

`VoteItem.Deserialize()` reads version `0`, then reads `_Messages` and delegates to `VoteSite.Deserialize(reader)`.

### `VoteSite`

`VoteSite` is not an `Item`; it serializes through its owning `VoteItem`.

| Version | Write/read order |
| --- | --- |
| `0` | `_Parent` as an `Item`, `_Name`, `_URL`, `_CoolDown` |

### `VoteStone`

`VoteStone.Serialize()` calls `base.Serialize(writer)`, writes version `0`, then writes `_LabelMessage`. `Deserialize()` reads that same version and string after the base `VoteItem` payload.

## Reward Verification

The Voting scripts do not contain:

| Missing behavior | Trace result |
| --- | --- |
| Reward item creation | No `AddToBackpack`, `DropItem`, `BankBox`, gold, token, or reward-table path in `Data/Scripts/Custom/Voting`. |
| External vote callback | No packet handler, HTTP listener, file poller, or vote-service parser in the Voting scripts. |
| Completed-vote verification | `Mobile.LaunchBrowser(URL)` is treated as success; the code does not confirm the external page accepted a vote. |
| Multiple vote sites | A `VoteItem` owns one `VoteSite`; the singleton `[Vote` path uses one hidden `VoteItem.Instance`. |

Administrators should treat this as a vote-link cooldown system unless another untraced subsystem is later added to verify votes and pay rewards.

## Technical Trace

* Target acquisition -> `docs/SystemAudit.md` marked `Voting Rewards` as `Missing` and pointed to `Data/Scripts/Custom/Voting/VoteCommand.cs`.
* Entry points -> traced `VoteCommand.Initialize`, `[Vote`, `[VoteConfig`, `[VoteInstance`, `VoteItem.OnDoubleClick`, and `VoteStone`.
* Vote mechanics -> traced `VoteHelper.CastVote`, `VoteEvents.OnVoteRequest`, `VoteRequestEventArgs.CanVote`, and `VoteItem.OnVote`.
* Cooldown state -> traced `VoteHelper.GetLastVoteTime` and `SetLastVoteTime` to account tags using `VS_LAST_VOTE_` plus the upper-cased vote-site name.
* Persistence -> traced `VoteConfig` file persistence, `VoteItem` serialization, `VoteSite` serialization, and `VoteStone` serialization.
* Reward claim check -> searched the Voting directory and found no reward payout, callback verification, or external vote-tracking implementation.
