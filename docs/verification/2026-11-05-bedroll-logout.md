# Bedroll logout repair and isolated acceptance

## Result and scope

Ordinary successful bedroll use now enables instant logout through `PlayerMobile.GetLogoutDelay()`. The former workflow only placed a restorative bed and never enabled its existing instant-logout flag. The new workflow derives eligibility from the live owned bed at disconnect, without setting that flag. The isolated native Player logged out with two loaded horses and seven already-stabled pets; both extra horses were immediately sheltered and returned on reconnect with their identities and cargo intact.

This is local candidate verification, not deployment or a recovery action on a live player's account. No original account credentials were used. The earlier native failure and assisted-flag observations remain in `D:/ConficturaUOTestTools/stable-native-20261105-161124/`.

## Implementation and entry points

- `Data/Scripts/Items/Explorers/Bedroll.cs`, `OnDoubleClick(Mobile)`: existing Player-only Camping placement and ten-minute cooldown; reject deleted items and unsafe camping. Find an existing owned bed relative to the player, including when the rolled bed is inside a backpack. Explain instant logout after successful placement.
- `Data/Scripts/Items/Explorers/BedrolledOut.cs`, `IsSafeCamp(Mobile)` and `CanLogout(Mobile)`: require a live, alive owner on a non-internal map; permitted camping region; no boat, criminal flag, combatant, aggression-list entries or nearby hostile creature. Require an owned, top-level, non-deleted bed within three paces, the same map, at most 16 height units away, with line of sight and age below 100 seconds. Pooled enumeration is freed in `finally`.
- `Data/Scripts/Mobiles/Base/PlayerMobile.cs`, `GetLogoutDelay()`: include live bedroll eligibility in the existing instant-logout branch. The existing `OnDisconnected()` protects pets before other disconnected subscribers; logout remains an idempotent fallback. The legacy `BedrollLogout` property remains compatible, but ordinary use never sets it.
- `BedrolledOut.RollUp()`: ignore repeated calls after deletion, preventing duplicate returned bedrolls. Existing restoration ticks, placement skill rules and serializer payloads retain their formats.

Rolling up, deleting, expiring or containing the bed revokes eligibility. Leaving range or unsafe conditions revokes eligibility synchronously, including before the next timer tick; returning to the still-active safe camp restores it. Recent aggression remains blocked until the normal aggression lists clear. A cooldown/placement failure grants no instant logout.

No new commands, event subscriptions, script files, project includes, serialized fields or type identities were introduced. `PlayerMobile` remains version 38; `Bedroll` and `BedrolledOut` retain their original version-0 write/read maps. Existing laid-out beds roll up on deserialization.

## Verification evidence

Production verification directory: `D:/ConficturaUO-main/output/stabling-verification/4dc3b00af1f64e63aecb5f1213933513/`.

| Evidence class | Command or artifact | Observed result |
| --- | --- | --- |
| Operational build | `powershell -NoProfile -ExecutionPolicy Bypass -File Scripts/Test-Stabling.ps1`; `server-build.log` | Isolated Release/x86 operational server build passed. Script-only repair; no tracked executable replacement required. |
| Production runtime script compile | `runtime-compile.log`, `-compileonly -nocache` | Forced recursive script compilation passed. |
| Production-method components | `tests/StableRegression.cs`; `regression.log` | 124 assertions passed, including real `Bedroll.OnDoubleClick`, eligibility without a fixture flag, three-pace boundary, ownership, ghosts, criminal/combat/aggression gates, internal/contained/deleted beds, hostile creatures, height, expiration, repeat roll-up, loaded multiple followers beyond capacity, disconnect and cargo-preserving return. Existing billing/return/fault-injection tests also passed. |
| Copied-world compatibility | `world-write.log`, `world-read.log` | Adopted 863 pre-billing records, saved version 38 and independently reloaded unchanged pet identity, stable/pending ownership, nested cargo, billing balance and deadline. Original save hashes unchanged. This is separate from native gameplay. |
| Isolated startup smoke | `startup-smoke.log`, `startup-smoke/Data/Logs/Console.log` | Forced compilation and post-initialization milestone passed with a time limit; fixture exited before game listeners and disabled Orb. |
| IDE/project hygiene | `ide-build.log` | `ConficturaUO.sln` Release/Any CPU built with isolated outputs/intermediates, including `Scripts.csproj`; existing unrelated warnings remain. |
| Project/source truth | `project-truth.json` | 6,599 runtime source files and project compile entries; zero missing, unlisted or duplicate entries. URI decoding and literal paths applied. |

Native evidence directory: `D:/ConficturaUOTestTools/stable-native-20261105-170808/evidence/`. Reproduction helpers and the guarded test-only fixture remain under `D:/ConficturaUOTestTools/tools/bedroll/`. `bedroll-results.json` records **16/16 passing assertions** against retained snapshots and native screenshots. Tests used a fresh disposable Player, normal client controls and copied saves, on `127.0.0.1:54690`; eight hashed isolation overlays disable external integrations and isolate paths/listeners. All 6,599 non-overlay production scripts match the candidate snapshot.

| Native workflow | Retained evidence | Observed result |
| --- | --- | --- |
| Ordinary pack bedroll double-click | `before-bedroll.xml`, `active-bedroll.xml`, `native-bedroll-open.jpg` | Delay changed from 300 seconds to zero. `BedrollLogout` remained false. |
| Paperdoll logout and confirmation | `protected-logout.xml`, `native-logout.jpg`, `events.tsv` | Player immediately internalized; stable count 7 to 9, pending count 2. Both loaded horses sheltered before disconnected-event observation. |
| Normal reconnect | `returned-pets.xml`, `native-return.jpg` | Both original horse serials returned under the same control master; stable count 7, pending 0. Each nested bag retained 137 gold and 20 ingots, three cargo items, and loyalty 100. Snapshots prove both quantities; this run did not repeat the earlier separate visual cargo-tooltip check. |
| Grace and loaded rate | `protected-logout.xml` | Both new episodes had a 35-gold weekly rate, zero outstanding, and a UTC deadline seven days later. No upfront withdrawal. |
| Real bed lifetime | `before-expiry.xml`, `expired-bedroll.xml`, `events.tsv` | Active at age 82.5 seconds; bed naturally rolled up after 100 seconds and delay returned to 300. No clock/age injection in this native case. |
| Native walking away/back | `active-out-of-range.xml`, `walked-back.xml`, `native-out-of-range.jpg` | Four paces from the same 35-second-old bed gave delay 300; returning to three paces at age 51 seconds restored zero. |
| Native roll-up and retry | `rolled-up.xml`, `cooldown-denied.xml`, `native-roll-up.jpg`, `native-cooldown.jpg` | Double-click removed the active bed before expiry and revoked instant logout. Retry displayed the ten-minute cooldown and retained delayed logout. |
| Disconnect after cancellation | `cancelled-disconnect.xml` | Offline Player remained on the world map with delay 300; no stale instant-logout protection or new pending entries. Loaded pets retained owner and cargo. |
| Instrumented safety gate | `criminal-block.xml`, `lawful-restored.xml` | Fixture-set criminal status blocked eligibility; clearing it restored the same live camp. This is an instrumented state transition, not native combat acceptance. Combat/aggression guards were separately verified by production-method components. |

Fixture setup fills stables, creates cargo, sets Camping to 125, and resets the placement cooldown between independent cases. It never sets `BedrollLogout` true in these cases. Native Player movement, bedroll use, roll-up, logout and reconnect trigger production methods. The unrelated prior stable integrations were rerun by the component suite; this focused native run does not repeat every trainer, city, summoning or billing-timer interaction.

## Failed observations retained

The first added elevation regression failed: production line of sight accepted a bed 100 height units above the owner. The explicit height guard corrected this; the final 124-assertion suite passed. Earlier 111-assertion output under `65feda4cf3964b299e98511cb74ac0b2/` predates that guard and is not final candidate evidence. A manual expanded-fixture command also failed to compile because its relative slash path was misparsed; its subsequent old executable output is not accepted as an expanded pass.

`walked-away.xml` captured the first bed after natural expiry, not a clean range-only result. The later `active-out-of-range.xml` is the accepted range-only observation. An initial integrity helper rejected the deliberately modified candidate because it required a clean worktree; its failed JSON is retained. The corrected helper checks an explicit changed-path allowlist and exact candidate/copy hashes; all protected originals matched in both checks.

## Cleanup and rollout

`input-integrity.json` verifies all 51 original save files and nine original client settings/profile files unchanged. `cleanup.json` records zero owned client/server processes and no listener on port 54690. Native client exit was 0. Test-server shutdown used the existing fixture's `Core.Kill(false)` and process exit -1, without a final save; this is not evidence of a graceful final world save. Ordinary copied-world save/reload was verified separately above. Resident feedback remains under the isolated test root; no model training was performed.

Before deployment, take a full pre-update save backup. Deploy compatible safe-stabling code together; rollback of the underlying version-38 stable billing change still requires compatible code and its pre-update backup. This bedroll repair introduces no additional save-format change.
