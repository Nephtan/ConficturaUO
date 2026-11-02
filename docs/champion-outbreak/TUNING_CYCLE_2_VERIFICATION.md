# Arachnid tuning cycle 2 verification

Work is local on `ChampRevamp`. Production activation remains disabled. These are different evidence classes; component checks do not establish natural-client balance or production acceptance.

## Source and save checks

- Latest exact production candidate forced runtime script compile: `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/verification/dc3c6be82eb74d3f89c838c939215630/runtime-compile.log`. The isolated compile-only executable did not load production saves or listen on a game port.
- Ten component/source-method suites passed in that snapshot, evidence `component-runs/06b5777ca9d449258f04e7fdb6c94d86`: territory91,260; persistent infestation928; infestation448; town targeting7; controller64; outbreak rewards347; webs92; bards59; generic champion rewards679; reagent regression47. Geometry loops account for most checks; these are not independent player trials. Infestation search counts vary with generated test terrain candidates.
- These checks exercise actual ordinary AI acquisition methods, bounded settings and rejection of active edits, population/guardian placement policy, queen decision predicates and finite/source-dependent backup, frontier entitlement after claim deletion, and current serializer sentinels. They are controlled fixtures.
- An actual older version3 copied world loaded with the version4 controller and default settings, then ran explicit `RecoverInfestation`:4,266 checks. Evidence `D:/ConficturaUOTestTools/arachnid-saved-world-verification/d9d823d505924765a5716f2d77a43ae6`. Retired patch322, nine local response births, eight saved defenders, actor/inventory identities and finite resources survived. This was offline load/recovery, without global initialization, listener or resumed gameplay. It predates only a queen-health-hook type fast guard that changes no serializer.
- Explicit Visual Studio source list:6,624 includes, no missing, duplicate or unlisted runtime sources. Isolated `Scripts.csproj` Release/AnyCPU project build passed at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/ide-project/38c6b7a90b914e0081690d2e54eccfa4`. This is IDE/project evidence. It predates later method-body corrections; current runtime compilation above covers those corrections.
- The tracked root executable is unchanged from HEAD, SHA256 `8746B2340E5116AD3B980AFB0C1AEF4466D86A9D658B8DC022D74F04E7CBF31B`. Native copied worlds use the earlier isolated operational build `BBFC851A97378F9490C891AAB3E110256A0DD22111B8217FD22BCF83CFA29EC8`; the latest compile/component snapshot rebuilt `Server.csproj` as `EE3830C5AC1DB418612850AE47272D7222CE2E1F659325F575FB377258A1C2A0`. These are distinct artifacts, not byte-identical copies of the tracked executable. This cycle changes no core source or tracked executable and does not replace the root artifact.

## Saved-build audit

An offline survey loaded1,384 actual saved players from the authorized October17 test-world snapshot. Latest private evidence: `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/surveys/af7690eb8e7849588f9cb1662052a4f8`. Names and inventories remain in private test evidence. See [capability audit](LEVEL_CAPABILITY_AUDIT.md) for limits and classifications. A survey is not a combat result.

The archer audit initially counted only four backpack bolts. Equipped-quiver inspection found another214, correcting the total to218. Offline zero nearby pets does not mean no available pets: ordinary login can return auto-stabled companions. These observations must inform native drivers before interpreting performance.

## Natural comparison checkpoint

A fresh colony began at a random ordinary-search origin2522,1240 at2026-11-02T04:08:14.6113056Z. No cells were staged and no client case ran. Real founders, ordinary AI and wall clock produced the colony. The common pre-combat ordinary `World.Save` completed at04:33:22.3195008Z, retained with20 file hashes at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/checkpoints/natural-before-combat-01`.

Generation used population100 and replacement5. It predates the final refuge-leash/retreat-commit correction and queen-only health-hook fast guard. Every comparison arm must therefore load the same latest candidate source. Applying declared initial comparison settings to copied state is a harness intervention, not live administrator retuning or proof that production activation/recovery is enabled. This young colony is not a seven-day infestation and has no established weak outskirts; separate frontier fixtures must be labeled.

Native exploratory comparisons, actual claim collection and current-version globally initialized copied-world resumed combat are recorded below. Guard respawn, production release recovery and economic acceptance remain separate gates.

## Ordinary-clock native comparison

All three branches loaded the same naturally grown checkpoint and used the latest combat code. Zanthus was a vulnerable Player with his unchanged Archmage Overlord source, ordinary preparation and finite saved kit. No win was required. Each branch was observed for up to300 seconds, ending sooner on victory. Private evidence: `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/COMPARISON_RESULTS.json` and `arms/<arm>/natural/evidence/`.

| Branch | Population / replacement seconds | Measured outcome | Owned brood median / peak | Patches start / end | Incoming health loss | Queen damage credited |
| --- | --- | --- | --- | --- | --- | --- |
| control-02 |100 /5 | Both alive at300s; queen2862HP |190 /207 |29 /34 |2441 |6303 |
| density |45 /5 | Queen defeated at228.0s |93 /101 |33 /36 |1253 |9109 |
| candidate |45 /30 | Queen defeated at196.6s |73 /93 |32 /35 |458 |9100 |

These are single exploratory branches. Ordinary growth, random combat/familiar outcomes and preparation duration diverged. Damage totals can include overkill and changing maxima; incoming loss is cumulative, not final health or potion consumption. Owned brood includes charmed creatures and is not a count of hostile attackers. The control's nearby-pressure recorder began190.3s into its case, while the easier arms include quiet post-victory samples. Their whole-window nearby medians therefore cannot support a direct pressure ratio.

Actual queen shelter movements/arrivals were92/5,51/4 and80/5 respectively. She engaged, withdrew and used finite guards:3,3 and6 spent at the measured boundaries. This directly contradicts the earlier impression that she never moves. It does not prove every retreat was tactically useful, or that players understood the tells. Killing a rally source during its warning remains a native acceptance gap; component tests cover cancellation.

The density branch collected five claims containing six glands and one skull. The candidate collected six claims containing six glands, one skull and a decorative Web artifact. Collection used the native `[OutbreakClaim` UI; the artifact is not guaranteed equipment. Distributed ground gold was visible, but not all cash was picked up. No hourly economy or long-term supply claim follows from these cases.

The45/30 default is a promising reduction in attrition for this young colony. Zanthus defeating it in about three minutes does not establish seven-day, landmass-scale, group or newcomer balance. Increasing boss health in response would obscure the measured density/replacement changes and should require separate evidence.

## Additional saved builds and failed frontier cases

The frontier fixtures explicitly constructed671 connected cells on a fresh copied save; they are not naturally aged seven-day worlds. All failures and later deaths remain retained.

| Evidence | What was observed | What it proves or leaves open |
| --- | --- | --- |
| frontier novice | Died before active combat;64 of92 recorded damage came from an unrelated riding tiger | Wildlife contaminated the probe; no spider-only difficulty conclusion |
| frontier-02 novice | Reclaimed a weak cell and earned supplies, then died after the driver ended in the field | Reclamation and entitlement occurred; survival/escape failed verification |
| frontier-02 journeyman | Reclaimed a cell;32 outgoing damage, zero incoming at the recorded endpoint | Actual equipped combat worked in this narrow case; later survival is not established |
| frontier-02 archer | Reclaimed a cell;30 outgoing, zero incoming at the endpoint, finite quiver ammunition available | Native archer participation occurred; exact ammunition delta and broader pressure acceptance remain open |
| frontier-02 hybrid |2067 outgoing,63 incoming over120s; alive at325HP, tithing84 unchanged | Actual hybrid loadout fought; this basic driver does not represent optimal mage/Knightship use |
| frontier-02 bard | No verified control invocation; later died after the driver stopped | Driver failure, not a valid bard balance trial |
| frontier-03 | Additional pre-action skill API errors and the same erroneous retreat geometry | Retain as harness defects; do not retune creatures to compensate |
| frontier-04 | No combat cases started. Safe API and movement preflight passed | Safe readiness evidence only; no new frontier balance result |

The old retreat always led west, which moved an eastern entry toward the queen. The corrected fixture chooses an outward dominant axis, starts at least320 tiles from the queen and verifies increasing separation. Ordinary offline `PathFollower` movement traversed its actual opening-to-exit route in `safe-preflight-01/load-corrected2.log`. That is server movement evidence, separate from a full live-client escape.

Safe live bard evidence is `arms/frontier-04/natural/evidence/safe-api-preflight-bard.jsonl`: actual equipped Trumpet and finite293 bandages/one food item were visible; the character used his own food and bandage. Installed assistant metadata showed `UseSkill(string)` resolves the client's table; its skill15 name was literally `Discordanc`. That call produced a song cursor, accepted a harmless self target and cleared the cursor. Native pathfinding moved one safe town tile and returned. The controlled exception remained bounded in intro with caseId0, and server state confirmed the bard alive at82/82 afterward. Combat support effectiveness is still unverified. Hunger caused pre-combat town health loss and is a readiness confounder, not encounter damage.

Four frontier supply recipients, retired patch436, five response births/defenders and actual inventories survived offline replay of frontier-02's final save:1606 checks,697 patches and252 resident actors. Evidence: `D:/ConficturaUOTestTools/arachnid-saved-world-verification/1f037820924d4062bd1a25d283f863a0`. This includes the failed characters; no resurrection or state cleanup rewrote their outcomes.

## Remaining acceptance and design questions

- The copied current-version combat save now has a globally initialized native resume and a new saved-world replay, detailed below. Production activation/restore is still a separate gate because the private resume bridge deliberately preserves a disabled-production attempt.
- Interrupt an actual warned queen rally source through native attacks and observe cancellation. Do not substitute direct fixture damage, deletion or component checks.
- Verify full ordinary outward escape and meaningful bard support after the repaired driver. New harness code requires a fresh prepared driver/heartbeat before staging and labels driver errors invalid; test-only safe parking occurs after the measured endpoint and must never count as ordinary escape.
- The infestation branch of `KeepCreatureInBounds` permits ordinary pursuit beyond occupied territory. A tier0 spider's home pursuit bound is not a guarantee that every inner-tier actor stays inside the300×300 core. Clarify and test the policy for strong pursuers, queen migration and uninvolved travel before release.
- Useful early rewards cannot repair a starting weapon/skill mismatch or make a poor, unprepared novice ready for the core. Evaluate clear starter equipment/training guidance separately from artifact inflation.
- Validate civic guard recovery, production activation/restore behavior, longer natural growth, varied origins/terrain, several simultaneous players and long-term loot supply. Production remains disabled; no push or deployment occurred.

## Native current-version resume and cleanup follow-through

After the user's direct approval of routine test cleanups, frontier-04 saved and closed with20 retained files at `arms/frontier-04/checkpoints/safe-preflight-close-01`. A single prepared `arms/control-recovery` world then loaded the actual later control-02 combat checkpoint. It preserved population100/replacement5 and six spent queen guards, using the documented test-only resume bridge and ordinary clock. The resumed queen and existing brood ran ordinary AI; this is stronger evidence than offline `World.Load`, while remaining separate from production activation approval.

The saved veteran's previous fight location was unsafe for an unprepared login. The fixture logged an explicit relocation to his separately retained original precombat position,345,3614,27, without restoring health or resources. His normal original macro then prepared him. The novice's safe-intro failure sentinel exercised the actual driver exception path; the server recorded `driver-failure-held-novice.txt`, rejected a staging command with `Fresh prepared client driver required; errors prohibit staging`, and retained caseId0, original position and90HP. This proves exception containment and admission rejection in safe intro. The field stale-heartbeat/error rescue branches remain unexercised native paths.

Zanthus's actual case ran from23:07:02.7015063Z to23:08:03.0381625Z on2026-11-02: approximately60.34 seconds. Native spell/control actions caused16,082 credited damage, including33 to the queen; cumulative incoming health loss was392. The pre-parking endpoint manifest recorded him alive at258/300HP and286 mana. The queen withdrew and her spent backup remained6. No victory was required or forced. These results verify resumed combat and preserved expenditure; they are not another density comparison or a full escape test. The terminal fixture logged its measured endpoint before returning him to the original safe location. Later status regeneration must not replace the pre-parking endpoint.

Actual before/after combat inventory manifests showed no changes in carried item counts. This does not establish hidden charge use or cost-free casting. Evidence: `arms/control-recovery/natural/evidence/veteran-before-veteran-combat.private.xml`, `veteran-after-veteran-combat.private.xml`, `combat-inventory-deltas.json`, `veteran-bound-result.json`, `case-events.tsv`, `health.tsv` and `client-veteran.jsonl` under the private tuning root. Since all six queen births were already spent, this recovered encounter could not supply a new rally interruption test without resetting the very expenditure being verified; that acceptance gap remains open.

Cleanup retained another ordinary20-file save at `arms/control-recovery/checkpoints/native-recovery-close-01`, then closed only the two owned copied clients/server and signaled the recorder. The recorder ended normally; clients/server were forcibly closed after save, not gracefully shut down. The new checkpoint passed581 offline load/recovery checks with39 patches,174 resident actors, one surviving saved defender and the minimum six spent queen births still satisfied. Evidence: `D:/ConficturaUOTestTools/arachnid-saved-world-verification/fe4e68127bcd49608ed2e016609dbf7f`. One living defender does not imply only one birth was spent.

Final identity-checked process/listener inspection found no owned cycle clients, servers or recorder helpers, and neither test port57913 nor57914 listening. Original20 save hashes and three source profile hashes remain the preservation checks. The user's standing cleanup approval applies to routine owned test runs; demonstrations stay open until the user requests their cleanup.

## Native-discovered resident recovery correction

The first five-minute ordinary-clock control case exercised real queen engagement, retreat and finite guards. Reloading its later cleanup checkpoint failed on an existing version0 resident record with368 health and a current maximum of350. The positional data was intact; `ArachnidInfestationResident.Deserialize` incorrectly required health never to exceed the current maximum. The native world demonstrated that this is not a valid invariant for a live creature under changing effects. The correction accepts that health snapshot while preserving slot/kind/tier/identity/maximum validation and every serialized field/type/order. Dormant hydration still assigns through the ordinary `Mobile.Hits` setter, which clamps to the current maximum. It grants no healing and changes no global health behavior.

Failed attempts remain under `D:/ConficturaUOTestTools/arachnid-saved-world-verification/905a4fe367b947d3a668be274d0b4bf1`, `7adf2454cf054a67bc135fa8c8e65c36` and `65a59fd56ab34b2a8a763a36cd861dd9`. The offline runner now suppresses destructive interactive recovery prompts and retains the original exception. The regression reproduces the368/350 record and checks its exact following sentinel.

Corrected exact source forced runtime compilation and all ten component suites passed at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/verification/dc3c6be82eb74d3f89c838c939215630`. The actual previously rejected save then loaded and recovered:622 checks,35 patches,184 resident actors and six saved queen guards; finite expenditure survived. Evidence: `D:/ConficturaUOTestTools/arachnid-saved-world-verification/d3c6633cdd2c445fbf015f0da0e1cc43`. This checkpoint was saved after the300-second case boundary, so it includes ordinary continued world activity; its six guards must not be substituted for the three spent at the measured case boundary. Offline recovery is still distinct from globally initialized resumed gameplay.

## Source traces

Implementation and positional serializer map: [settings](TUNING_CYCLE_2_SETTINGS.md). Source-method fixtures: `tests/ArachnidTerritoryRegression.cs`, `tests/ArachnidOutbreakControllerRegression.cs` and the eight existing regression suites listed above. Offline replay fixture and runner: `tests/ArachnidSavedWorldRoundTrip.cs`, `scripts/Test-ArachnidTerritorySavedWorld.ps1`. No test fixture is under the production runtime script tree.
