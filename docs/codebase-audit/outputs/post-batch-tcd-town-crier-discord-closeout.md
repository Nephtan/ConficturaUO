# POST-BATCH-TCD-01 Town Crier Discord Closeout

## Outcome

`POST-BATCH-TCD-01` curates the Town Crier Discord companion feed in response to the test-shard export: wanted-register rebuilds are aggregated, repeated Exploration prose is session-suppressed, higher-value news wins queue overflow, operational counters are visible to administrators, and fatal environmental hazards produce one cause-aware Deaths report instead of a self-kill plus Exploration pair.

The source batch is locally complete. Live Discord-channel and in-game public/private fatality checks remain deployment-owner staging steps because this checkout contains no local webhook configuration or secret.

## Audit-Grounded Boundaries

- `Data/Scripts/System/Misc/Logs.cs` remains the owner of event-file writes and the one-minute `StatusPage` murderer-register timer.
- `Data/Scripts/Mobiles/Base/PlayerMobile.cs` remains the owner of the `OnBeforeDeath` publication hook and was not changed.
- `Data/Scripts/Items/Traps/KillerTile.cs` remains in place with its namespace, type, `Serial` constructor, version `0`, and serializer read/write layout unchanged.
- `Data/Scripts/Mobiles/Civilized/TownHerald.cs` remains completely unchanged, including movement speech and serialization.
- `Data/Scripts/Custom/Integrations/Discord/TownCrierDiscord.cs` remains the app-owned incoming-webhook integration boundary. No bot connection, OAuth secret, world-save state, database state, or persistent dedupe file was added.

The broad canonical audit generators were not rerun. The completed audit predates this integration and a full regeneration would create unrelated evidence churn; the focused review CSV records the current hooks, dependencies, risks, and compatibility decisions for this batch.

## Behavior and Interfaces

- `TownCrierDiscord.QueueEvent(string category, string eventText)` is preserved.
- `TownCrierDiscord.QueueWantedRoster(IList<string> notices)` admits one stable roster built only from successfully written murderer lines.
- Pending duplicates coalesce with an occurrence count. Overflow keeps Test; Wanted/Deaths/Deeds; Victories; Gossip; then Exploration, and reports the omitted count.
- Successful identical wanted rosters are suppressed for 24 hours and successful identical Exploration prose for the configured `ExplorationRepeatMinutes`. Both histories are memory-only and reset on restart.
- `[TownCrierDiscord status` reports sanitized configuration, intervals, queue state, cooldown, and session counters. Reload preserves session counters and successful-delivery dedupe history while discarding pending work.
- `LoggingFunctions.ApplyFatalEnvironmentalDamage` scopes an environmental cause around the existing 10,000 self-damage and always clears it. The existing `LogDeaths` path produces the sole event.

## Compatibility Review

- World-save serialization: unchanged.
- `TownHerald` speech and entry behavior: unchanged.
- Town Crier `Info/*.txt` formats: unchanged; murderer lines remain individual file entries.
- Privacy: existing `PublicMyRunUO` admission remains authoritative. Public environmental deaths include the cause; private deaths use generic prose.
- Project/runtime inclusion: no new runtime source file. `TownCrierDiscord.cs` remains explicitly included in `Scripts.csproj`.

## Verification

- Temporary policy/transport reflection harness: passed 64 assertions covering configuration, mapping, timestamp removal, priority overflow, pending coalescing, chronological ordering, roster aggregation and all-clear transition, repeat windows, escaping, mention suppression, message length, HTTP/network classification, counters, and reload behavior. The temporary harness was removed after execution.
- Project/source truth: 6,598 `Scripts.csproj` compile includes, 6,598 runtime `.cs` sources excluding `bin`/`obj`, 0 missing targets, 0 unlisted sources, Town Crier include present.
- `Data/System/Source/Server.csproj` Release/x86: passed and produced the fresh verification executable outside the tracked root artifacts.
- `ConficturaUO.sln` Release/`Any CPU`: passed as IDE/project hygiene with existing warnings, including the known MSIL/x86 reference mismatch warning.
- Runtime script compile: the first forced compile rejected `HashSet<T>` because the runtime script compiler does not reference `System.Core`; the implementation was changed to repository-compatible dictionaries and the final `-compileonly -nocache` run passed.
- Time-boxed `-service -nocache` initialization: passed against the non-live workspace snapshot. Town Crier initialization reported the expected sanitized missing-local-config state, world initialization completed, listeners opened on the free configured test port, and the verification process was stopped immediately.
- Serializer/hook scan: `KillerTile` serializer lines and all `TownHerald`/`PlayerMobile` source remain unchanged; only `KillerTile.OnMoveOver` and `MoonCore.OnEnter` route their existing fatal damage through the cause-aware helper.
- Final whitespace/source-diff verification: passed with only the checkout's expected LF-to-CRLF working-copy warnings. The final runtime compile was rerun after the last source adjustment and passed.

## Owner Staging Checks

1. In a temporary Discord channel, set the ignored local configuration, run `[TownCrierDiscord reload`, `[TownCrierDiscord status`, and `[TownCrierDiscord test`, and confirm the five-minute production setting before rollout.
2. Exercise an identical and changed wanted roster, repeated exact Exploration prose, a five-item overflow digest, HTTP failure recovery, and a reload.
3. Test public and private players against `[add KillerTile` and the Moon Core entrance. Confirm one Deaths post, correct cause/redaction, no Exploration duplicate, and unchanged fatal effects.
4. Save and restart the test shard and confirm the version-0 `KillerTile` reloads and remains functional.
