# Arachnid territory polish: measured review

Candidate source: `0019b067` on `ChampRevamp`. This is a tested prototype for guided review. `ArachnidOutbreakController.DeliveryReady` remains false; no push, deployment, or production activation occurred. The [design brief](TERRITORY_POLISH_DESIGN.md) describes intent; this page records evidence and its limits.

## What changed

The queen's location defines a 300-by-300 square, with the highest three tiers in nested inner bands. Outside it, selected cells produce sparse weak spiders, occasional stationary 60-health egg sacs, and small decorative webs. Destroying a sac suppresses its local patch without erasing independent daughters or active combatants. Recent deliberate hunting can produce warned finite responses, credited to players and pet masters. Each nest has at most nine response births, in waves of two, three, and four. Reinforcements exclude non-contributing players and their pets and have no generated loot.

The queen plans toward the interior of the largest connected occupied territory. Her own escort density cannot anchor the calculation. She follows ordinary paths, changes refuge as territory changes, and uses blocked-goal alternatives. Shared rate quotas govern background work; the background actor target no longer blocks territorial founders, queens, or structural actors. Nearby player, pet, and relevant NPC interactions retain ordinary AI.

Quiet dormant surplus is pruned before hydration when former inner territory becomes outskirts. Existing visible or engaged survivors remain real; instantaneous thinning of those survivors is not claimed. Tier changes preserve identity and inventory and never refill health, stamina, or mana. The custom-map void floor at Z=-127 is now rejected.

## Native copied-world outcomes

The main evidence is `D:/ConficturaUOTestTools/infestation-polish-20261101-004`. This uses the October 17 copied save, an unaccelerated operational core, ordinary server initialization/AI, real saved characters, normal item effects, and eight declared containment overlays. Initial legal staging and supplied routes/queen coordinates are test assistance. These are guarded client drivers, not autonomous discovery or learned gameplay. No victory was required.

| Case | Measured result |
| --- | --- |
| Novice passage | Outward and return completed at 90/90 health with zero case health loss. |
| Novice reclamation | Latest attempt dealt 14 damage to a sac while a frontier spider pressured him; he withdrew alive at 28 health. Earlier trial03 destroyed sac1389/patch322 and visibly suppressed it, then completed passage at90/90. Both outcomes are retained. |
| Intermediate resistance | Triggered a warning, recorded 82 health loss including68 from defenders, and withdrew alive at63 health. His actual kit had zero tithing and no bandages; no healing supplies were granted. |
| Sargar hunting | Native warned responses reached nine births at nest322; another nest produced five. He withdrew alive at235 health with all seven cats alive. Recorded health loss279 included15 from defenders; outgoing6373 included2563 pet damage. One route-driver stop/restart narrowed the supplied hunting area; this was not an uninterrupted autonomous run. |
| Bystander | Uninvolved Zanthus remained at262 health with zero incoming/outgoing damage. Third-tier defenders were measured within4 tiles; near-defender samples span79 seconds. Positions and health are observed; internal AI target-selection decisions were not instrumented. |
| Zanthus full macro | His unchanged original Archmage Overlord source ran for the complete ten-minute window. He reached the moving queen, dealt5566 recorded queen damage, recorded4001 health loss, and finished alive at230 health. His existing immortality jewel was consumed: a native0-to290 restoration at23:45:15UTC matches the shard's normal death-prevention code. This is not survival without that consumable. The queen survived. |
| Queen movement |493 shelter movement steps and four arrivals were recorded, including refuge changes during combat. These counters are not every movement the queen can make. |
| Separate natural origin | The normal random search selected3403,2450. Ordinary founders walked and established patches; Sargar's client observed two further establishments. No constructed patches or accelerated clock were used for this separate colony. All seven cats were alive at that observation's completion; two died afterward while the group remained idle. |

Health-loss counters include all recorded decreases, including changes from ordinary effects; they are not exclusively attributed enemy damage. Boundary health comes from the immutable per-case manifests, rather than later regeneration. Actual inventory amounts confirm one immortality jewel consumed and, during Sargar's hunting case, eight bandages and four greater cure potions used. Amount comparisons do not audit every charge field or intrinsic gear effect.

Earlier failed trials remain under `infestation-polish-20261101-001`, `-002`, and `-003`: blocked routes, ordinary wildlife deaths, invalid staging, and a wrapper observation-file sharing failure. The latter stopped Zanthus's earlier macro; that death was not accepted as a combat-build result. A later shared-read retry allowed the complete trial04 run.

Background `Start-Process` recorders in trial04 did not retain their expected evidence. Explicit execution sessions began around23:36; sampled telemetry starts at23:36:30UTC. Native health/death/case traces and boundary manifests exist from fixture initialization. The recorded peak of1429 tracked actors applies to the retained telemetry interval.

## Verification and reproducibility

- Matching operational Release/x86 core SHA-256: `BBFC851A97378F9490C891AAB3E110256A0DD22111B8217FD22BCF83CFA29EC8`. It was built in isolation from the matching core source. No core source or tracked root executable changed in this script-only batch.
- Forced production runtime script compile passed at `infestation-polish-20261101-001/verification/dc608bb16a194751b0d1125ddac171fd/compile-refresh/7e5ea9c914fb46018221c73efa539897`.
- All ten component suites passed at `component-runs/60cf58c6a52d411a98e5b5b3fc33cdb7`. This includes91235 territory/source-method checks, persistent/shared scheduling, town targeting, existing champion rewards, contribution rewards, web rescue, bard behavior, and the two merged crash regressions. Large geometry assertion counts are not independent gameplay trials.
- Explicit project/source inventory:6620 entries, no missing/unlisted/duplicate files. Isolated IDE `Scripts.csproj` Release/AnyCPU build passed at `infestation-polish-20261101-001/ide-project/8d16881a722d47ae9fce5544c34b9f40`. This is separate from runtime script compilation.
- Ordinary copied `World.Save` checkpoints passed native offline `World.Load` plus explicit recovery. The suppression/nine-birth checkpoint passed4263 checks with eight saved defenders; the completed trial04 checkpoint passed1908 with18 response births and13 saved defenders. Principal references resolve to actual loaded players; the non-contributing novice remains excluded. No global script initialization, listener, gameplay resume, or additional save was run by these tests.
- Old patch versions0/1 and current2 retain positional sentinel checks. Version2 appends `ResponseBirths`; the new reinforcement type serializes its contributing-principal list as version0 after its base serializer.
- Original20 source save files and three source profile inputs were rehashed unchanged. `main` remained `0d9d5d7d`. Native clients rewrote the serialized `profilespath` field to empty after launch; initial private profile files, unique test namespaces, launcher working directories, and protected source hashes were verified. Independent live profile-path tracing was not performed.
- Development processes were stopped only after identity checks and retained checkpoints. Closures were forced process stops, not graceful shutdowns. All artifacts remain available.

Reproduce the source-method checks with `scripts/Test-ArachnidTerritory.ps1`. Reproduce the fixture checkpoint checks with `scripts/Test-ArachnidTerritorySavedWorld.ps1`, supplying a private checkpoint manifest and matching verified build. For example:

```powershell
& .\scripts\Test-ArachnidTerritorySavedWorld.ps1 `
  -CheckpointDirectory 'D:\ConficturaUOTestTools\infestation-polish-20261101-003\checkpoints\responses-nine-01' `
  -VerifiedBuild 'D:\ConficturaUOTestTools\infestation-polish-20261101-001\verification\dc608bb16a194751b0d1125ddac171fd' `
  -RetiredPatchId 322 -MinimumResponseBirths 9 -ExpectNoviceDead
```

Fresh runner evidence is under `D:/ConficturaUOTestTools/arachnid-saved-world-verification/`: `a84b6121eba24db982d0ce373f4b282f` and `5c5262caa2f143349e37b0b7b23eb339`.

## Remaining production acceptance and design questions

Native civic-guard combat and its respawn cycle were not exercised. The seven real NPC/region method checks and observed ordinary wildlife interactions do not prove that cycle. A resumed live encounter after full script initialization was also not tested; the production delivery gate deliberately stops loaded outbreaks while disabled. These remain release checks before production activation.

Do not equate a nominal skill profile with an appropriate usable kit. The saved novice has sword skill, while his starting dagger uses Fencing. His result does not isolate egg-sac difficulty from weapon choice or the decision to ignore an attacking spider. Starter shopping/training deserves its own real-gold test; it should not be silently replaced by item grants.

The private driver also used an inadequate distance sentinel to check depleted supplies. Future demonstrations now use actual client item-cache presence and explicitly open owned containers that hold relevant supplies. Visibility/preparation evidence must precede claiming the complete usable toolkit. Earlier outcomes retain that limitation; none are promoted to autonomous learning.

Broad populations and fast area damage can change loot per hour even when each creature's loot table is unchanged. These runs did not loot corpses or measure net earnings. Measure currency/material supply, consumable costs, local reclamation value, and the effects on crafting/trade before calling the economy balanced. A universal artifact lottery is not part of this change.

The current egg-sac model is a pale mobile with a normal corpse after destruction. It is attackable and weak; final visual presentation remains reviewable. A dense group already visible during queen migration can take time to disperse or be defeated. Preserving active creatures is intentional; a claim of immediate sparse density everywhere would be too strong.

## Runtime and source traces

- `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakController.cs`: `Initialize`, Administrator command `ArachnidOutbreak <status|create|start|stop> [serial]`, `OnHealthChanged`, `Cleanup`, `Restore`, disabled `DeliveryReady`.
- `ArachnidTerritoryPolicy.cs`, `ArachnidInfestation.cs`, `ArachnidInfestationActors.cs`, and `ArachnidInfestationTerrain.cs` in the same directory: geometry, safe admission/hydration, local suppression, actual actor policy and void rejection.
- `ArachnidBroodResistance.cs`: kill attribution independent of sealed reward stages, warning/service paths, finite persistent births, reinforcement harm/enemy checks and principal serializer.
- `ArachnidQueenShelter.cs`, `QueenBroodMassPlanner.cs`, and `SpreadingEncounterScheduler.cs`: transient planning, normal AI movement, shared quotas and protected interactions.
- `ArachnidInfestationState.cs`: patch version2 and older-format reads. Existing actor version1 remains intact; queen planning is rebuilt from saved state.
- `Data/Scripts/Mobiles/Base/PlayerMobile.cs`: `OnBeforeDeath`; `Data/Scripts/Magic/Research/Spells/Death/ResearchWithstandDeath.cs`: `SeeIfJewelInBag.IHaveAJewel`, normal jewel restoration/consumption.
- `tests/ArachnidTerritoryRegression.cs`, `tests/ArachnidSavedWorldRoundTrip.cs`, `tests/ArachnidSavedBuildSurvey.cs`, and `tests/ArachnidTerritoryNativeAcceptance.cs`: distinct component, offline copied-world, private inventory, and real-client fixtures. The native fixture is outside the production runtime tree and rejects every root except its exact declared private copy; it is copied into that copy only for testing/demonstration.

The separate final introduction uses `D:/ConficturaUOTestTools/infestation-polish-20261101-005`, fresh original copied characters, loopback port57913, no automatic macro combat, and no encounter started. This is a fresh demonstration, not continuation or reversal of the measured losses in trial04. Confirm actual visible readiness before starting the narrated tour/combat. Final demonstration cleanup remains user-directed.

All four introduction clients passed an inventory-only preflight using actual item-cache presence after ordinary owned-container opens. Relevant visible stacks: novice0, intermediate4, Sargar23, Zanthus7. Sargar's two original bandage stacks total991; Zanthus's original immortality jewel is present. The novice has no qualifying consumable supplies, rather than an unobserved hidden supply stock. An initial Sargar check refused to proceed before queued container replies arrived; the retained retry passed after bounded waits. No consumables, buffs, or combat preparation were used in this preflight. All four game windows were maximized and inspected with status, inventory, paperdoll, and map at the edges; Zanthus is left foreground. `natural/evidence/FINAL_READINESS.json` and four `*-ready.private.xml` manifests record the separate paused scene.
