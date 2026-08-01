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
```

Only HTTPS webhook URLs on `discord.com` under `/api/webhooks/` are accepted. The interval accepts values from 1 through 1440 minutes. The URL is never included in status output or failure logs.

Restart the shard, or use `[TownCrierDiscord reload` after editing the local file.

## Staff command

`[TownCrierDiscord <status|test|reload>` requires `Administrator` access.

- `status` reports whether the bridge and webhook are configured, pending message count, cooldown, and sanitized success/failure state.
- `test` queues a fixed test message through the normal transport and cooldown.
- `reload` rereads the ignored local configuration without restarting the shard. Pending unsent news is discarded when configuration is reloaded.

## Delivery behavior

- The first eligible event after an idle period sends immediately.
- During the following cooldown, the bridge retains the five newest distinct events and posts them chronologically as one digest.
- Exact wanted-murderer notices are suppressed for 24 hours because the shard rebuilds that register every minute. A changed murder count is a distinct notice.
- Discord mentions are disabled, player-derived text is escaped, and posts are limited to Discord's 2,000-character content limit.
- HTTP work runs outside the game loop. Discord rate limits use `Retry-After`; network and server failures receive up to three retries. HTTP 401, 403, or 404 disables delivery until configuration is reloaded.
- Pending digests and duplicate tracking exist only in memory and do not survive a restart.

The bridge reuses only entries already admitted to the Town Crier logs, so the existing `[private` visibility and redaction rules remain authoritative.

## Source trace

- `Data/Scripts/Custom/Integrations/Discord/TownCrierDiscord.cs`: configuration, queue, digest, webhook transport, retry policy, and `[TownCrierDiscord` command.
- `Data/Scripts/System/Misc/Logs.cs`: `LoggingFunctions.LogEvent` calls `TownCrierDiscord.QueueEvent` only after the event file update succeeds.
- `Data/Scripts/Mobiles/Civilized/TownHerald.cs`: existing NPC speech and serialization remain unchanged.
- `Data/System/CFG/town-crier-discord.example.cfg`: safe deployment template; the `.local.cfg` counterpart is ignored.

Discord references: [incoming webhooks](https://docs.discord.com/developers/platform/webhooks), [webhook resource](https://docs.discord.com/developers/resources/webhook), and [rate limits](https://docs.discord.com/developers/topics/rate-limits).
