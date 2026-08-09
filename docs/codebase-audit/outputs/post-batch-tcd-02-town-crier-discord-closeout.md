# POST-BATCH-TCD-02 Town Crier Startup and Wanted-Feed Closeout

## Outcome

`POST-BATCH-TCD-02` removes false startup journeys and replaces the oversized full Wanted roster with compact latest-state reporting. Offline world-load region rebinding is rejected before it can write `Info/journies.txt`, supply `TownHerald`, or enter Discord. Discord now receives one initial total/top-five Wanted summary and then only additions, removals, and murder-count changes.

The source batch is locally complete. Live Discord-channel, connected region-crossing, disposable-player `Kills`, and KillerTile checks remain deployment-owner staging steps because this checkout has no local webhook configuration or connected game client. The copied `Info` files and Discord export used to identify the defects remain ignored test evidence and are not repository artifacts.

## Audit-Grounded Boundaries

- `Data/Scripts/System/Misc/Logs.cs` remains the owner of Info-file writes, player privacy admission, region prose, and the one-minute `StatusPage` register rebuild.
- `Data/Scripts/Custom/Integrations/Discord/TownCrierDiscord.cs` remains the webhook, queue, formatting, retry, counter, and administrator-command boundary.
- `TownCrierDiscord.QueueEvent(string category, string eventText)` and public `QueueWantedRoster(IList<string> notices)` remain available. The source register uses an internal structured overload with stable mobile serial identity.
- `Data/Scripts/Mobiles/Civilized/TownHerald.cs`, `Data/Scripts/Mobiles/Base/PlayerMobile.cs`, `Data/Scripts/Items/Traps/KillerTile.cs`, and `Data/Scripts/System/Regions/MoonCore.cs` are unchanged.
- No namespace, serialized type, serializer/version, world-save layout, bot/OAuth secret, database record, persistent queue, or dedupe file was added.

The canonical audit generators were not rerun. This focused CSV records the command, timer, worker callback, death hook, dependencies, privacy boundary, and serializer review without unrelated canonical-table churn.

## Implemented Behavior

- `LoggingFunctions.LogRegions` still sends normal entry/exit messages to players. Journey publication now additionally requires a `PlayerMobile`, exact `AccessLevel.Player`, and a live `NetState`.
- `StatusPage` still rewrites `Info/murderers.txt` once per minute and writes each murderer separately. Only successfully written lines add stable serial identity, current display prose, and murder count to the Discord snapshot.
- The first nonempty snapshot in a process reports the total and five highest murder counts, with display-name ordering for ties. The first empty scan is silent.
- Later snapshots report only additions, removals, and count changes. Name/title-only edits update the delivered snapshot silently, unchanged state has no 24-hour repost, and nonempty-to-empty produces the existing all-clear.
- A newer pending Wanted snapshot replaces the older one. Its delta is prepared against the last successfully delivered snapshot; retries do not advance the baseline, exhaustion permits recovery on the next scan, and reload clears pending work while retaining the successful baseline.
- Complete compact change clauses are added while they fit; an explicit additional-change count covers the remainder. Markdown escaping, mention disabling, priority overflow, digest ordering, and the 2,000-character limit remain enforced.
- Victory and Gossip suppression remain deferred pending a clean post-repair sample. The test shard may continue its ignored one-minute interval; the committed production example remains five minutes.

## Verification

- Deterministic reflection harness: passed 40 focused assertions for disconnected/connected/staff journey admission; initial empty/nonempty Wanted scans; total/top-five ranking and tie order; unchanged silence beyond 24 hours; additions, removals, count changes, title-only changes, all-clear; pending replacement; successful-baseline advancement; failed-send next-scan recovery; reload/restart behavior; complete omitted-change clauses; Markdown escaping; mention blocking; Unicode safety; and the 2,000-character limit. The temporary harness was removed after verification.
- Project/source truth: 6,598 `Scripts.csproj` includes and 6,598 runtime `.cs` sources excluding `bin`/`obj`, with zero missing targets, zero unlisted sources, and the existing Discord script include present.
- `Data/System/Source/Server.csproj` Release/x86: passed.
- `ConficturaUO.sln` Release/`Any CPU`: passed as IDE/project hygiene with existing warnings, including the known MSIL/x86 reference mismatch.
- `Data/Scripts/Scripts.csproj` Release/`AnyCPU` with project-reference rebuilding disabled: passed as standalone IDE/project hygiene with the known architecture warning.
- Forced runtime script compile: the fresh Release executable completed `-compileonly -nocache` successfully.
- Isolated real-save initialization: copied 19 save files and the runtime data tree to a temporary root, excluded the local webhook configuration, changed only the isolated listener to port 4599, and ran `-service -nocache`. Script initialization loaded, Town Crier reported the sanitized disabled state, four offline clones initialized, listeners opened, and the console-ready milestone was reached. `Info/journies.txt` remained exactly 150 lines, 11,424 bytes, SHA-256 `7979507B91A933345A1A34668FB86816F1E12CD91FAD16477D9F738BC8E84C9E` before and after startup. The process was terminated after the milestone.
- Shutdown evidence: an earlier non-PTY isolated attempt reached initialization but later hit the existing `ServerConsole.Next` null-input failure after standard input closed. The final PTY run reached the required milestone without that failure and was stopped explicitly; this is test-runner shutdown behavior, not a Town Crier source regression.
- Static compatibility checks found no changed serializer calls and no changes to `TownHerald`, `PlayerMobile`, `KillerTile`, or `MoonCore`. Existing environmental-death behavior therefore remains source-identical to POST-BATCH-TCD-01.

## Staging Follow-up

1. Restart the actual test shard with players offline and confirm the historical simultaneous journey cluster is absent from both `Info/journies.txt` and Discord.
2. Cross a named region as one connected public player and confirm the player message, one Info line, Town Crier availability, and one Discord Exploration item. Repeat while private to confirm existing privacy behavior.
3. Change one disposable player's `Kills` from `0` to `1`, then `2`, then `0`; allow one minute after each transition and confirm summary/addition, count change, removal/all-clear, and silent unchanged scans.
4. Recheck one KillerTile fatality for the single cause-aware Deaths notice.
5. Collect the next export, `Info/*.txt`, `[TownCrierDiscord status`, and shutdown/failure console lines. Use that clean sample to investigate the unmatched `Nephtan has slain an enemy` event and the August 7 one-hour timestamp discrepancy.
