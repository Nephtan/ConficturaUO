# System Name: Voting Rewards

## Overview

The Voting Rewards package under `Data/Scripts/Custom/Voting/` is a vote-launch and cooldown framework, not a complete external reward redemption system.

Players can trigger it either with the `[Vote` command or by double-clicking a placed `VoteStone`. Both paths end in the same `Server.Voting` flow:

1. Resolve a `VoteSite` profile.
2. Check whether the `VoteSite.URL` parses as a `Uri`.
3. Check an account tag cooldown keyed as `VS_LAST_VOTE_<SITE NAME>`.
4. If allowed, launch the configured vote URL in the client's browser.
5. Stamp the account tag with the current `DateTime`.

There is no HTTP callback, shard-side vote verification, reward item grant, point ledger, top-voter leaderboard, or XMLSpawner attachment in this package.

## Script Inventory

| Script | Role |
| --- | --- |
| `Data/Scripts/Custom/Voting/VoteCommand.cs` | Registers the `[Vote`, `[VoteConfig`, and `[VoteInstance` commands. |
| `Data/Scripts/Custom/Voting/VoteItem.cs` | Base `Item` that owns a `VoteSite` profile and handles vote status messaging. |
| `Data/Scripts/Custom/Voting/VoteStone.cs` | Placeable world item that inherits `VoteItem` and exposes the site details in its property list. |
| `Data/Scripts/Custom/Voting/VoteEvents.cs` | Internal vote-request event pipeline and `VoteRequestEventArgs`. |
| `Data/Scripts/Custom/Voting/VoteHelper.cs` | URL validation, cooldown checks, account-tag reads/writes, and formatted cooldown text. |
| `Data/Scripts/Custom/Voting/VoteSite.cs` | `PropertyObject` that stores the vote site name, URL, and cooldown. |
| `Data/Scripts/Custom/Voting/VoteConfig.cs` | Global default configuration loaded from and saved to `Data/VoteSystem.cfg`. |
| `Data/Scripts/Custom/Voting/VoteStatus.cs` | `Invalid`, `Success`, `TooEarly`, and `Custom` status enum. |

## Player Entry Points

### `[Vote`

`VoteCommand.Initialize()` registers `[Vote` at `AccessLevel.Player`.

When a player uses `[Vote`:

1. `Vote_OnCommand` rejects null or deleted mobiles.
2. The command calls `VoteItem.Instance.CastVote(e.Mobile)`.
3. `VoteItem.Instance` lazily creates an invisible internalized `VoteItem(0)` if no static instance is currently assigned.
4. `VoteItem.CastVote` forwards to `VoteHelper.CastVote(from, VoteSite)`.

### `VoteStone`

`VoteStone` is a `[Constructable]` world item with item ID `0xED4`.

* Default name: `Vote Stone`
* Default hue: `0`
* `Movable = false`
* Double-click behavior: inherited from `VoteItem.OnDoubleClick`

`VoteStone.GetProperties(ObjectPropertyList list)` adds a green usage message plus the configured site name and uppercased URL when the site is valid. If the URL is not valid, it shows a red "Voting is currently unavailable." line instead.

## Vote Request Flow

| Step | Code path | Observed behavior |
| --- | --- | --- |
| 1 | `VoteHelper.CastVote` | Rejects null/deleted mobiles and null sites, then raises `VoteEvents.InvokeVoteRequest(new VoteRequestEventArgs(from, site))`. |
| 2 | `VoteEvents.InternalVoteRequest` | Always runs `OnVoteRequest` before any external subscriber because the internal handler is attached in the field initializer. |
| 3 | `VoteSite.Valid` | Treats the site as valid only if `new Uri(URL)` succeeds. |
| 4 | `VoteRequestEventArgs.CanVote` | Reads cooldown state through `VoteHelper.GetLastVoteTime`. |
| 5 | `VoteItem.OnBeforeVote` | Base implementation always returns `true`; `VoteStone` does not add extra checks. |
| 6 | `VoteItem.OnVote(..., Success)` | Sends a thank-you message if `Messages` is enabled, launches the browser to `VoteSite.URL`, and writes the current time to the account tag. |
| 7 | `VoteItem.OnAfterVote` | Base implementation does nothing. `VoteStone` also does nothing. |

## Status Outcomes

| `VoteStatus` | Trigger | Base behavior |
| --- | --- | --- |
| `Success` | `VoteSite.Valid` and cooldown allows another request and `OnBeforeVote` returns `true` | Sends a thank-you message, launches the browser, and writes the cooldown tag. |
| `TooEarly` | Cooldown has not expired | Sends `Sorry, you can not vote at <site> for <formatted time>.` when `Messages` is enabled. |
| `Invalid` | URL is missing or fails `new Uri(URL)` | Sends `Sorry, voting is currently unavailable.` when `Messages` is enabled. |
| `Custom` | `OnBeforeVote` returns `false` | Base implementation sends no fallback message and grants no reward. |

## Cooldown And Tracking Model

Cooldown tracking is account-scoped, not character-scoped.

* Tag key format: `VS_LAST_VOTE_<SITE NAME IN UPPERCASE>`
* Storage location: `Account` tags via `Account.GetTag()` and `Account.SetTag()`
* Default cooldown: `VoteConfig.GlobalDefaultCoolDown`, which is `24` hours until a GM changes the config or the site profile

### First-use behavior

If the account tag is missing, `VoteHelper.GetLastVoteTime` does two different things in the same call:

1. It writes the current time into the real account tag with `SetLastVoteTime`.
2. It locally substitutes `now - voteSite.CoolDown` so the current request still counts as eligible.

That means the first request always passes the cooldown check, but the account is immediately placed on cooldown even before the player proves they completed the vote page.

### Rename behavior

Because the cooldown key is derived from `VoteSite.Name`, changing the configured site name creates a different account-tag namespace and effectively resets every player's cooldown history for that site.

## GM Configuration Surfaces

| Command | Access | Behavior |
| --- | --- | --- |
| `[Vote` | Player | Uses the static internal `VoteItem.Instance`. |
| `[VoteInstance` | GameMaster | Opens a `PropertiesGump` for the static internal `VoteItem.Instance`. |
| `[VoteConfig` | GameMaster | Opens a `PropertiesGump` for the singleton `VoteConfig.Instance`. |

### `VoteConfig`

`VoteConfig.Initialize()` hooks:

* `EventSink.ServerStarted` -> `Instance.Deserialize()`
* `EventSink.WorldSave` -> `Instance.Serialize()`

The config file is `Data/VoteSystem.cfg` and stores:

| Field | Default source |
| --- | --- |
| `DefaultName` | `VoteConfig.GlobalDefaultName` |
| `DefaultURL` | `https://www.uogateway.com/shard.php?id=1429&act=vote` |
| `DefaultCoolDown` | `TimeSpan.FromHours(24.0)` |

New `VoteSite` objects copy those defaults when constructed.

## Persistence

| Type | Persistence path | Versioned fields |
| --- | --- | --- |
| `VoteConfig` | Binary file `Data/VoteSystem.cfg` saved on world save and loaded on server start | Version `0`: default name, default URL, default cooldown |
| `VoteSite` | Serialized through its owning `VoteItem`/`VoteStone` | Version `0`: parent item, name, URL, cooldown |
| `VoteItem` | Standard RunUO item serialization | Version `0`: `Messages`, `VoteSite` |
| `VoteStone` | Standard RunUO item serialization | Version `0`: `LabelMessage` |

### Static command instance caveat

The `[Vote` command depends on the static `VoteItem.Instance` field, but no constructor or `Deserialize` path reassigns deserialized `VoteItem` objects back into that static field. After a restart, the next `[Vote` call creates a fresh invisible `VoteItem` with default settings instead of reusing any previously saved internal instance.

Placed `VoteStone` items do keep their serialized `VoteSite` and `LabelMessage`, but they are separate from the command-only static instance.

## Technical Trace

* Targeting `Voting Rewards` -> found entry point `VoteCommand.cs` -> traced `[Vote` into `VoteItem.Instance.CastVote`, then into `VoteHelper.CastVote`, then into `VoteEvents.OnVoteRequest`.
* Traced logic: code executes URL validation, account-tag cooldown lookup, `OnBeforeVote`, and then on `Success` only sends a message, opens the vote URL in the client's browser, and stamps the account tag with `DateTime.Now`.
* Traced persistence: `VoteConfig` saves global defaults to `Data/VoteSystem.cfg`; `VoteItem`, `VoteSite`, and `VoteStone` serialize normally, but the static `[Vote` instance is not rebound after deserialize.
* Traced reward path: there is no code anywhere in `Data/` subscribing to `VoteEvents.VoteRequest` to verify off-shard voting or grant items, currency, or points.

## Observed Implementation Gaps

* The package does not implement actual external vote verification or reward payout. A `Success` status means only that the site URL was valid, the cooldown allowed the request, and the client browser was launched.
* The cooldown is consumed locally when the command is used, not when the shard confirms a real vote.
* The static `VoteItem.Instance` used by `[Vote` is not restored back into the static field after deserialization, so GM-edited command-instance settings do not survive restart as the active `[Vote` target.
* If a derived `VoteItem.OnBeforeVote` returns `false`, the base `Custom` branch sends no explanatory message.

## XMLSpawner

This package does not use XMLSpawner attachments, XMLSpawner spawn definitions, XMLSpawner packet hooks, or XML quest helpers. It is entirely command/item/config driven.
