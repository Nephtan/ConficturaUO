# Isolated outbreak native acceptance

Use only a disposable output. This parameterizes the established client005 copied-world workflow; it is not a production launcher or general gameplay agent.

Preparation requires a verified foundation snapshot matching current core/runtime inputs, the existing isolated client005 baseline with its eight recorded isolation overlays, and installed test assets. It checks the original save/profile/settings hashes, creates a fresh loopback port, disables autosave in the guarded fixture, disables plugins in the copied client only, and generates a source-hash compatibility register for the private tree. It never starts a process. Do not print copied settings or account-name files.

```powershell
python -X utf8 tools/champion-outbreak/PrepareNativeAcceptance.py `
  --baseline D:\ConficturaUOTestTools\foundation-ingame-20261025\batch-3\client-005 `
  --candidate D:\ConficturaUOTestTools\outbreak-20261029-input-throttle-001 `
  --output D:\ConficturaUOTestTools\outbreak-native-next `
  --assets D:\ConficturaUOTestTools\shard\Data\Files
```

From that copy's `shard` directory, force runtime compilation with `ConficturaServer.exe -service -nocache -compileonly`. Before starting its world, verify the recorded port is unoccupied and all eight isolation overlays are present. Start only the copied executable, using `-service -nocache`; capture process identity and console/error logs. Never use the repository executable against these or production saves. The copied client can use its prepared test profile with `-skiploginscreen true -autologin true`; do not automate authentication dialogs. Select its exact returned window through Computer Use.

Publish requests only through the atomic helper, and wait for consumption/telemetry before another request:

```powershell
python -X utf8 tools/champion-outbreak/SendNativeRequest.py `
  --output D:\ConficturaUOTestTools\outbreak-native-next turn-self
```

| Request | Private setup/action |
| --- | --- |
| `show` | Display the native QA control gump. |
| `reset` | Disable/dissolve the private battle and recreate the stationary boss scene. |
| `warn`, `arm` | Trigger the real warning immediately or after eight seconds; natural warning/restraint durations remain unchanged. |
| `turn-self`, `turn-pet` | Real manager enrollment, stopped scheduler, directly constructed self/pet restraint and a prepared player turn. |
| `dose` | Place four real treatments within backpack bounds. |
| `status` | Dispatch the production status command with the copied Player; also available through a native QA button. |
| `queue` | Construct one explicit pending stage entitlement for claim presentation. Does not prove combat earning. |
| `claim` | Dispatch the real player claim command; also available through a native QA button. |
| `stop` | Stop the owned fixture without saving. Record it as forced termination, not graceful logout. |

The fixture requires `evidence/native-private-root.txt` to match its executable directory. Its watchdog is thirty minutes. `native-state.json` and `native-events.jsonl` report instrumented positions, AP, held state and contribution; they do not prove native pixels or input origin. Use native keyboard/mouse observations for that evidence. The native QA status/claim buttons call existing Player commands and validate the responding actor. They are not production UI additions.

Turn scenes bypass encounter discovery/placement and directly construct a restraint. The scheduler and boss/pet AI are stopped. Keep their native item/target/AP evidence separate from legal warning impact, live AI, pacing, site approval and combat rewards. Preserve the original character's equipment/stats; record any fixture-only scene changes. Opening the backpack currently costs the ordinary four-AP ItemUse allowance; prepare inventory before benchmarks and do not silently change that policy.

After testing, check the exact process paths before stopping owned processes, confirm their listeners are gone, and compare all original and private save hashes against `protected-source-inputs.json`/`prepared.json`. Preserve failed logs and identify crash exits separately from explicit stop markers. Never remove unrelated processes or profiles. Native004's request-sharing crash is retained;005 completed forced teardown;007 verifies final fixture compilation and atomic publication only.
