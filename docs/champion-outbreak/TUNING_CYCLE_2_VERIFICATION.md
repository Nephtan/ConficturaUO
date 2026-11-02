# Arachnid tuning cycle 2 verification

Work is local on `ChampRevamp`. Production activation remains disabled. These are different evidence classes; component checks do not establish natural-client balance or production acceptance.

## Source and save checks

- Latest exact production candidate forced runtime script compile: `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/verification/190fbf8dbf544b00b070139c2241f817/runtime-compile.log`. The isolated compile-only executable did not load production saves or listen on a game port.
- Ten component/source-method suites passed in that snapshot, evidence `component-runs/4f7023cb9cbb4954ba04a7f14e4324f2`: territory91,259; persistent infestation928; infestation440; town targeting7; controller64; outbreak rewards347; webs92; bards59; generic champion rewards679; reagent regression47. Geometry loops account for most checks; these are not independent player trials. Infestation search counts vary with generated test terrain candidates.
- These checks exercise actual ordinary AI acquisition methods, bounded settings and rejection of active edits, population/guardian placement policy, queen decision predicates and finite/source-dependent backup, frontier entitlement after claim deletion, and current serializer sentinels. They are controlled fixtures.
- An actual older version3 copied world loaded with the version4 controller and default settings, then ran explicit `RecoverInfestation`:4,266 checks. Evidence `D:/ConficturaUOTestTools/arachnid-saved-world-verification/d9d823d505924765a5716f2d77a43ae6`. Retired patch322, nine local response births, eight saved defenders, actor/inventory identities and finite resources survived. This was offline load/recovery, without global initialization, listener or resumed gameplay. It predates only a queen-health-hook type fast guard that changes no serializer.
- Explicit Visual Studio source list:6,624 includes, no missing, duplicate or unlisted runtime sources. Isolated `Scripts.csproj` Release/AnyCPU project build passed at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/ide-project/38c6b7a90b914e0081690d2e54eccfa4`. This is IDE/project evidence. It predates later method-body corrections; current runtime compilation above covers those corrections.
- Existing operational core executable SHA256: `BBFC851A97378F9490C891AAB3E110256A0DD22111B8217FD22BCF83CFA29EC8`. Core source and tracked executable are unchanged.

## Saved-build audit

An offline survey loaded1,384 actual saved players from the authorized October17 test-world snapshot. Latest private evidence: `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/surveys/af7690eb8e7849588f9cb1662052a4f8`. Names and inventories remain in private test evidence. See [capability audit](LEVEL_CAPABILITY_AUDIT.md) for limits and classifications. A survey is not a combat result.

The archer audit initially counted only four backpack bolts. Equipped-quiver inspection found another214, correcting the total to218. Offline zero nearby pets does not mean no available pets: ordinary login can return auto-stabled companions. These observations must inform native drivers before interpreting performance.

## Natural comparison checkpoint

A fresh colony began at a random ordinary-search origin2522,1240 at2026-11-02T04:08:14.6113056Z. No cells were staged and no client case ran. Real founders, ordinary AI and wall clock produced the colony. The common pre-combat ordinary `World.Save` completed at04:33:22.3195008Z, retained with20 file hashes at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/checkpoints/natural-before-combat-01`.

Generation used population100 and replacement5. It predates the final refuge-leash/retreat-commit correction and queen-only health-hook fast guard. Every comparison arm must therefore load the same latest candidate source. Applying declared initial comparison settings to copied state is a harness intervention, not live administrator retuning or proof that production activation/recovery is enabled. This young colony is not a seven-day infestation and has no established weak outskirts; separate frontier fixtures must be labeled.

Native matched comparisons, current-version resumed gameplay, additional saved-build cases and actual supply collection remain pending in this source-check checkpoint. Guard respawn, production release recovery and economic acceptance remain separate release gates.

## Native-discovered resident recovery correction

The first five-minute ordinary-clock control case exercised real queen engagement, retreat and finite guards. Reloading its later cleanup checkpoint failed on an existing version0 resident record with368 health and a current maximum of350. The positional data was intact; `ArachnidInfestationResident.Deserialize` incorrectly required health never to exceed the current maximum. The native world demonstrated that this is not a valid invariant for a live creature under changing effects. The correction accepts that health snapshot while preserving slot/kind/tier/identity/maximum validation and every serialized field/type/order. Dormant hydration still assigns through the ordinary `Mobile.Hits` setter, which clamps to the current maximum. It grants no healing and changes no global health behavior.

Failed attempts remain under `D:/ConficturaUOTestTools/arachnid-saved-world-verification/905a4fe367b947d3a668be274d0b4bf1`, `7adf2454cf054a67bc135fa8c8e65c36` and `65a59fd56ab34b2a8a763a36cd861dd9`. The offline runner now suppresses destructive interactive recovery prompts and retains the original exception. The regression reproduces the368/350 record and checks its exact following sentinel.

Corrected exact source forced runtime compilation and all ten component suites passed at `D:/ConficturaUOTestTools/infestation-tuning-20261102-001/verification/dc3c6be82eb74d3f89c838c939215630`. The actual previously rejected save then loaded and recovered:622 checks,35 patches,184 resident actors and six saved queen guards; finite expenditure survived. Evidence: `D:/ConficturaUOTestTools/arachnid-saved-world-verification/d3c6633cdd2c445fbf015f0da0e1cc43`. This checkpoint was saved after the300-second case boundary, so it includes ordinary continued world activity; its six guards must not be substituted for the three spent at the measured case boundary. Offline recovery is still distinct from globally initialized resumed gameplay.

## Source traces

Implementation and positional serializer map: [settings](TUNING_CYCLE_2_SETTINGS.md). Source-method fixtures: `tests/ArachnidTerritoryRegression.cs`, `tests/ArachnidOutbreakControllerRegression.cs` and the eight existing regression suites listed above. Offline replay fixture and runner: `tests/ArachnidSavedWorldRoundTrip.cs`, `scripts/Test-ArachnidTerritorySavedWorld.ps1`. No test fixture is under the production runtime script tree.
