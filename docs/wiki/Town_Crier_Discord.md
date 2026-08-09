# Town Crier Discord Bridge

The Town Crier Discord bridge sends the shard's recorded Town Crier activity to a Discord text channel through an app-owned incoming webhook. It is a companion news feed: a Discord post does not mean that a nearby `TownHerald` spoke the same line at that moment.

## Discord setup

Installing the Town Crier App in the Discord server does not create an incoming webhook. Authorize the application separately with Discord's [`webhook.incoming` OAuth scope](https://docs.discord.com/developers/topics/oauth2#webhooks):

1. Add an HTTPS redirect URI to the Town Crier App in the Discord Developer Portal.
2. Open an authorization URL using the app's client ID, the exact registered redirect URI, and `scope=webhook.incoming`:

   ```text
   https://discord.com/oauth2/authorize?response_type=code&client_id=APP_CLIENT_ID&scope=webhook.incoming&redirect_uri=URL_ENCODED_REDIRECT_URI
   ```

3. Choose the shard's Town Crier text channel and approve the request.
4. Exchange the returned authorization code at `https://discord.com/api/v10/oauth2/token` as described in Discord's OAuth documentation. Perform this exchange only in a trusted local environment because it uses the app client secret.
5. Copy only `webhook.url` from the token response. Do not retain the OAuth access token, refresh token, client secret, or full token response in the repository or shard configuration.
6. In Discord's channel integrations, confirm that the resulting webhook has the intended Town Crier name and avatar.

The webhook URL contains its execution token and must be treated as a password. Delete the webhook in Discord and repeat the authorization flow if the URL is exposed.

## Shard configuration

Copy `Data/System/CFG/town-crier-discord.example.cfg` to the ignored local path `Data/System/CFG/town-crier-discord.local.cfg`, then set:

```text
Enabled=true
WebhookUrl=https://discord.com/api/webhooks/WEBHOOK_ID/WEBHOOK_TOKEN
MinimumIntervalMinutes=5
ExplorationRepeatMinutes=15
```

Only HTTPS webhook URLs on `discord.com` under `/api/webhooks/` are accepted. `MinimumIntervalMinutes` accepts values from 1 through 1440. `ExplorationRepeatMinutes` accepts values from 0 through 1440; `0` disables cross-post Exploration suppression. The URL is never included in status output or failure logs.

Restart the shard, or use `[TownCrierDiscord reload` after editing the local file.

## Staff command

`[TownCrierDiscord <status|test|reload>` requires `Administrator` access.

- `status` reports whether the bridge and webhook are configured, the configured intervals, pending and omitted counts, cooldown, sanitized success/failure state, and session delivery counters.
- `test` queues a fixed test message through the normal transport and cooldown.
- `reload` rereads the ignored local configuration without restarting the shard. Pending unsent news is discarded, while session counters, successful Exploration history, and the last successfully delivered Wanted baseline remain in memory.

## Delivery behavior

- The first eligible event after an idle period sends immediately.
- During the following cooldown, the bridge retains up to five distinct events and posts them chronologically as one digest. Exact pending duplicates are coalesced with a report count.
- Overflow keeps higher-value news first: Test; Wanted Murderers, Deaths, and Deeds; Victories; Gossip; then Exploration. The oldest event in the lowest priority is discarded, and the next digest reports how many lower-priority reports were omitted.
- The minute-based murderer register remains one line per murderer in `Info/murderers.txt`, but Discord treats it as current state. The first nonempty scan in a process reports the total and the five most-wanted characters, ranked by murder count and then display name. Later scans report only additions, removals, and changed murder counts. Name or title changes with the same stable character identity and murder count update the in-memory snapshot silently; an unchanged roster remains silent indefinitely.
- At most one unsent Wanted snapshot is retained. A newer minute scan replaces an older pending snapshot, and its changes are computed against the last successfully delivered snapshot when delivery begins. A failed delivery does not advance that baseline, so the next scan can recover the update. The first empty scan remains silent, while a delivered nonempty roster becoming empty posts one all-clear notice.
- Exact Exploration prose is suppressed across successful posts for `ExplorationRepeatMinutes`. Pending duplicates still coalesce, and different player or place prose remains distinct.
- Discord mentions are disabled, player-derived text is escaped, and each entry is safely shortened as needed so the complete post remains within Discord's 2,000-character content limit.
- HTTP work runs outside the game loop. Discord rate limits use `Retry-After`; network and server failures receive up to three retries. HTTP 401, 403, or 404 disables delivery until configuration is reloaded.
- Pending digests, counters, the successfully delivered Wanted baseline, and Exploration duplicate history exist only in memory and do not survive a restart. A nonempty Wanted register may therefore produce one fresh compact summary after each shard restart.

Killer tiles and the Moon Core now produce one Deaths report through the normal player death hook. Public players receive the environmental cause; private players receive a generic untimely-death notice. They no longer produce a second Exploration report.

The bridge reuses only entries already admitted to the Town Crier logs, so the existing `[private` visibility and redaction rules remain authoritative. Exploration journeys are recorded only when a Player-level `PlayerMobile` has a live connection. Region messages shown to connected players are unchanged, but offline world-load region rebinding no longer writes `Info/journies.txt`, supplies `TownHerald`, or enters Discord.

## Staging checklist

1. Use a temporary Discord text channel and an ignored local configuration.
2. Run `[TownCrierDiscord reload`, followed by `[TownCrierDiscord status`; confirm the displayed interval and repeat window are intentional.
3. Run `[TownCrierDiscord test` and confirm one escaped post appears without a mention notification.
4. The current test-shard cadence is one minute for compressed exercises. Keep the committed example at five minutes, and restore `MinimumIntervalMinutes=5`, reload, and confirm `status` before production use.
5. Restart the test shard with players offline. Confirm the simultaneous startup journey cluster does not appear in `Info/journies.txt` or Discord, then cross a named region while connected and public; confirm the normal player message and one journey entry/feed item still appear.
6. On a disposable player, use staff properties to exercise `Kills` from `0` to `1`, then `2`, then `0`. Expect one compact initial/addition report, one count-change report, one removal or all-clear, and silent unchanged minute scans.
7. Test a public and private player against a `KillerTile` and the Moon Core entrance. Each fatality must create one Deaths post and no duplicate Exploration post.

## Source trace

- `Data/Scripts/Custom/Integrations/Discord/TownCrierDiscord.cs`: configuration, priority queue, latest-state Wanted snapshots and deltas, Exploration suppression, digest formatting, counters, webhook transport, retry policy, and `[TownCrierDiscord` command.
- `Data/Scripts/System/Misc/Logs.cs`: `LoggingFunctions.LogEvent` queues ordinary Discord activity only after the event file update succeeds; `LogRegions` admits connected Player-level journeys; `StatusPage` submits stable serial identity, display prose, and murder count only after each murderer line is written; environmental causes are scoped into the existing death log path.
- `Data/Scripts/Items/Traps/KillerTile.cs` and `Data/Scripts/System/Regions/MoonCore.cs`: apply the existing fatal damage through the cause-aware logging helper without changing serialized data.
- `Data/Scripts/Mobiles/Civilized/TownHerald.cs`: existing NPC speech and serialization remain unchanged.
- `Data/System/CFG/town-crier-discord.example.cfg`: safe deployment template; the `.local.cfg` counterpart is ignored.

Discord references: [incoming webhooks](https://docs.discord.com/developers/platform/webhooks), [webhook resource](https://docs.discord.com/developers/resources/webhook), and [rate limits](https://docs.discord.com/developers/topics/rate-limits).
