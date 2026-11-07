# Ghost moongate and reconnect verification

## Reproduction and repair

- With **Always run** and **Unless hidden** enabled in ClassicUO 1.1.0.0, die as an ordinary player, step into the saved Sosaria gate at `(6603, 1082, 2)`, and select **Lodoria - Islegem**. On `6c331c76`, the client sent a run before travel and a walk after travel with the same input. The shard set the traveler's `Hidden` flag both while opening the gate menu and at the destination. The shared gate concealment rule now leaves player ghosts unhidden, and the death and login hooks clear leftover concealment. [ClassicUO movement code](https://github.com/ClassicUO/ClassicUO/blob/1.1.0.0/src/ClassicUO.Client/Game/GameObjects/PlayerMobile.cs#L1407-L1420)
- Die as a custom-race character and log out while dead. Reopen the client and reconnect. Baseline login replaced the visible ghost body `402` with the living Orc body `11` while the character was still dead, which returned terrain to color. Login now applies the custom race body and zero-delay region vital recovery only to living characters. [ClassicUO ghost detection](https://github.com/ClassicUO/ClassicUO/blob/1.1.0.0/src/ClassicUO.Client/Game/GameObjects/Mobile.cs)

## Runtime entry points

- [GateMoon](../../Data/Scripts/Items/Doors/GateMoon.cs) opens `MoonGateGump` on player movement over the gate and calls `ApplyTravelConcealment` from the delayed menu callback and `TeleportMe`. The existing destination checks, pet concealment, living traveler concealment and staff behavior are retained. The [public](../../Data/Scripts/Items/Misc/PublicMoongate.cs), [civic destination](../../Data/Scripts/Custom/Government%20System/Gumps/PCMoongateGump.cs), [first civic toll](../../Data/Scripts/Custom/Government%20System/Gumps/PCMoongateTollGump.cs), and [second civic toll](../../Data/Scripts/Custom/Government%20System/Gumps/PCMoongateToll2Gump.cs) call the same rule after their existing eligibility checks. Those variants were source-reviewed and compiled. No saved instances were available in the copied world for native tests of those variants.
- [Broadcast](../../Data/Scripts/System/Misc/Broadcast.cs) registers `Announce.World_Login` and `Announce.OnDeath` for `EventSink.Login` and `EventSink.PlayerDeath`. The hidden-flag reset is limited to ordinary players. The recovery check still requires a zero-delay region and no poison. No save-format fields changed.

## ClassicUO results

Testing used Xeron from a copied save with a disposable Player login and copied ClassicUO profile. Fixtures prepared the starting tile, removed death-prevention equipment, invoked the real player death routine, assigned the existing Orc race, and placed a shrine. A fixture region with zero logout delay exercised complete logout without a five-minute wait. A fixture set the old hidden flag before that logout to reproduce the stale state. ClassicUO input performed movement, gate selection, logout, reconnect, and shrine use. The server trace recorded movement packets and before/after state.

| Case | Result | Trace time (UTC) |
| --- | --- | --- |
| Baseline ghost gate | Sosaria entry moved with direction `132`. The same input at Islegem moved with direction `4`, and the journal showed “You begin to move quietly.” | 15:13:12–15:13:39 |
| Baseline reconnect | The human ghost stayed gray. The Orc ghost started gray and returned to full-color terrain after a fresh client login. Server state was still dead with visible body `11`. | 15:14:52, 15:17:29 |
| Patched ghost gate | A ghost that died while hidden could run across the Sosaria-to-Islegem gate and on to Portshine. Accepted run packets `132` and `128` each moved one tile after arrival. Terrain stayed gray. | 15:25:51, 15:26:46 |
| Patched Orc ghost reconnect | Native logout moved the character to `Map.Internal`. The fresh client showed gray terrain and the ghost paperdoll. The server restored body `402` with no body override, no hidden flag, and zero hits, stamina, and mana. Run packet `128` moved one tile. | 15:29:56–15:30:25 |
| Resurrection and living gate | The shrine brought back the Orc body and world color. The living character then took the Portshine-to-Islegem gate and arrived concealed. | 15:30:32, 15:31:08 |

## Build and evidence

The Release x86 server project built successfully. Both isolated shards started with `-service -nocache` on loopback. The exact six tested scripts also passed `-compileonly -nocache` with the uninstrumented production script tree. The protected input hashes matched after the run. The isolated client and server exited and their port was closed. Gameplay changes in the copied world were kept in memory for this run.

Local evidence is under `D:/ConficturaUOTestTools/ghost-native-20261107-085809/evidence/`. See `baseline-events.tsv`, `candidate-events.tsv`, `runtime-compile-production-payload.log`, `source-and-protection-check.json`, and `shutdown.json`. The native ClassicUO transcript shows the baseline color regression and the patched reconnect and resurrection.
