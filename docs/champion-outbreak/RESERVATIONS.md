# Wilderness reservation prototype

Current decision: the user explicitly approved all six footprints. Source declarations and the derived six-site catalog are now reviewed; DeliveryReady remains false. [GEOGRAPHY_APPROVAL.md](GEOGRAPHY_APPROVAL.md) and [APPROVED_WORLD_PREFLIGHT.json](APPROVED_WORLD_PREFLIGHT.json) supersede the hypothetical-only preflight as the current policy verification. The earlier snapshot/method history below remains retained. No live deployment or post-October17 occupancy is claimed.

Historical proposal checkpoint (superseded by the approval above): **proposed, inactive**. The user selected consideration of new permanent wilderness reservations on 2026-10-28. No proposal has been enabled, no player land has been restricted, and the outbreak delivery gate remains false.

## Historical selected final-preflight evidence basis

On 2026-10-29 the user explicitly selected the available October 17 offline test-world save. D:/ConficturaUOTestTools/shard/Saves contains 20 files dated 2026-10-17 and matches the native baseline byte-for-byte. The matching-source final preflight now passes in a separate copy of that chosen snapshot. This supersedes the pending request for a fresher folder; it does not claim current live occupancy or approve any reservation. Existing proposals/review flags remain inactive. A future deployment must consider changes after the chosen snapshot.

## Historical hypothetical selected-snapshot preflight - 2026-10-29

The matching-source repeat passes all six hypothetical reservations in a separate copy of the user-selected October 17 world. The probe matches core startup order Configure -> Region.Load -> World.Load, with no script-wide Initialize, listener startup, timer-thread startup or world save. All 186 declared static Sosaria regions load. Each proposed layout passes production terrain/service-region/portal clearance, with zero house-area intersections at any elevation, zero maximum-city-growth intersections, zero valid legacy-area intersections and zero portal-item intersections. Connected tile counts match the table below.

[SELECTED_WORLD_PREFLIGHT.json](SELECTED_WORLD_PREFLIGHT.json) retains the non-player aggregate. Private provenance/build/log/XML/preservation records remain at D:/ConficturaUOTestTools/outbreak-reserve-preflight-20261029-002/. The 6742 production inputs captured at that checkpoint, accepted core/script assemblies, Regions.xml and the 20 source/private save files remain unchanged. Source commit at preparation is f07b5e77; exact probe source and executable hashes are retained separately. The first private attempt fails compilation on the Rectangle2D.Contains overload before any world load; it is preserved with snapshot preservation evidence, then corrected in a fresh 002 root.

One inactive malformed legacy controller remains reported, unchanged and without guessed repair. This repeat establishes selected-snapshot clearance only. It does not approve land restrictions or establish occupancy after October 17. At that checkpoint, native ground/sign presentation and explicit geography approval remained open and all flags were false. Future deployment must handle any changes since that snapshot before promoting reservations.

Reproduction: [RunReservationPreflight.py](../../tools/champion-outbreak/RunReservationPreflight.py) prepares a fresh root, verifies current candidate source hashes, copies the chosen saves/config, compiles [LoadedReservationPreflight.cs](../../tools/champion-outbreak/LoadedReservationPreflight.cs) against matching core/scripts, bounds the read-only worker and verifies input preservation. The probe's housing-ban predicate is explicitly hypothetical; it never marks a reserve reviewed or bypasses occupancy/terrain/region checks.

## Approved geography

Each reservation is 161x161 tiles, enclosing the unchanged 113x113 combat footprint and 24-tile buffer. All three 5x5 brood staging areas and the 4,000-tile connected-space floor remain required. These are six alternate locations for one active outbreak, not six simultaneous encounters.

| Source ID | Center | Connected combat tiles |
|---|---|---:|
| central-woodland | 2514,2088 | 12,478 |
| northern-woodland | 2242,652 | 12,534 |
| eastern-woodland | 3746,876 | 12,439 |
| northwestern-woodland | 1208,928 | 11,553 |
| southeastern-woodland | 3762,2744 | 12,422 |
| western-woodland | 1122,1704 | 12,634 |

Machine-readable measurements: [reservation-proposals.csv](reservation-proposals.csv). Canonical source definitions are `WildernessEncounterReserve.cs`; all six `Reviewed` values are now true following explicit user approval, and the site catalog derives those six entries. Event activation remains disabled.

Terrain selection considered 486 local candidates, 882 expanded candidates and 289 farther northwest candidates. Copied-world filtering includes house areas, city regions, portal candidates, vendor/escort/quest-service types, and conservative legacy champion envelopes. It checks another 24 tiles beyond the proposed reservation. Static roof tiles are rejected: the first northwest lead contained map buildings despite having no player-house record, so it was replaced. Unnamed map content, traversal, player experience and today's live occupancy cannot be inferred from those filters.

## Permanent policy, independent of encounter state

The source prototype enforces reviewed reservation tiles directly in `HousePlacement.Check`, after the existing staff bypass and before ordinary region checks. It checks both the multi center and every occupied multi tile. This prevents a house crossing a reserve whose center is outside, and prevents another region overlay from silently reopening that land. It applies only on Sosaria and does not modify current staff placement policy.

Reservation definitions are static geography, not controller-owned world items. Starting, stopping, deleting or cooling down an outbreak cannot release approved housing protection. Unreviewed proposals impose no restriction.

New city founding checks the city hall's offset-adjusted center against the largest of all six configured city-growth radii. A hall outside a reservation is refused if its future territory could reach inside. Existing cities must pass a separate maximum-growth preflight before approval; they are not evicted or silently resized.

XML item spawning checks legacy `ChampionSpawn` areas after applying the full property string, including a configured radius enlargement. It retries the spawner's existing positioning policy at most ten times, then deletes the newly created unsafe controller and removes its spawn-list reference. It never falls back inside a reserve. XML spawn pools and existing champion reward rules are unchanged.

## Existing legacy rectangle defect

`ChampionSpawn.OnLocationChange` previously assigned `Rectangle2D.X` and `.Y`. Those setters move only Start, so End remained at the old coordinate and dimensions could become negative. The relocation regression reproduced this with an actual controller. Translation now constructs a new rectangle from translated Start and the original Width/Height. The serializer and deserializer, including their positional field order, are unchanged.

A matching-serializer load of the separate copied world found one **already malformed, inactive** mainland controller: serial `0x4002A01D`, location `(2230,302,0)`, area `(2225,297)+(-2220,-292)`. Its saved area was not rewritten. The fix prevents fresh relocation damage; it does not claim a migration of old malformed data. Staff must review that controller and its intended radius before reactivating it. Do not normalize all saved rectangles from guessed dimensions.

## Verification and limits

Accepted source snapshot: `D:/ConficturaUOTestTools/outbreak-20261028-reservations-004`.

- Release/x86 server build and forced runtime script compile passed.
- 234 lifecycle/serialization assertions, 59 controller assertions, 73 reservation/placement assertions, 679 existing champion reward assertions and 261 existing action assertions passed.
- Reservation tests simulate reviewed definitions only inside their process. They exercise actual `HousePlacement.Check`, a previously legal boundary-crossing plot, unchanged GM policy, complete terrain validation at all six coordinates, permanent protection after controller deletion, actual city placement preflight, fixed unsafe XML placement, safe legacy placement and post-property radius enlargement.
- Scripts.csproj IDE/project hygiene passed with the matched x86 reference and isolated outputs. Project truth has zero missing includes and zero unlisted runtime files; all frozen runtime/core input hashes match. No core source or tracked executable changed.
- Saved-world reconstruction was separately verified in 13 worlds / 26 fresh workers, recorded in [DELIVERY.md](DELIVERY.md).

Private native copy: `D:/ConficturaUOTestTools/outbreak-native-20261028-001`. Computer Use inspected the six boss-point viewports with a copied ordinary Player protected from harm. Relocation was scripted. Observations: central/north/southeast open grass; east jungle edge; northwest dry riverbank with water west; west woodland with a bare tree near the avatar. Boss spawn visibility/offset at the western point still needs review.

**No successful native translation is credited.** A numpad key entered unsent chat and was cleared; right click turned the actor, and an arrow-key press did not translate it. A read-only load confirmed the copied actor's saved Frozen/Paralyzed/CantWalk flags are false. Native input/macro configuration, traversal routes and brood-anchor inspection remain open. Visual inspection is not combat, pacing or complete site acceptance.

The native client connected only after both owned processes were launched in the host execution context. The earlier sandboxed listener could not be reached from the host. No firewall/security settings changed. ClassicUO's documented `skiploginscreen` launch option handled the local saved-credential login; no authentication dialog was automated. [Official launch arguments](https://github.com/ClassicUO/ClassicUO/wiki/Launch-Arguments).

All owned native processes were stopped; server `Core.Kill` and client process stops were forced, not graceful shutdown. Source saves, client settings, Data/Profiles and ClassicAssist/Profiles remained byte-identical to their captured hashes. Plugins were disabled only in the new copied client; no Resident/model/controller was launched.

Read-only loaded-world preflight: `D:/ConficturaUOTestTools/outbreak-reserve-loaded-20261028-001`. A private adapter reuses the CCWM exporter's matching-assembly bootstrap, invokes Configure/World.Load without script-wide Initialize or listeners, and exports only reserve findings/anonymous actor flags. All six hypothetical layouts passed full loaded-copy clearance with zero house-area intersections at any elevation, zero existing-city maximum-growth intersections and zero valid legacy-area intersections. The one malformed inactive legacy area is separately reported. Every copied save byte remained unchanged. This is a stopped private copy of the October 17 source world, not today's live world.

Retained failed observations include the private native helper's initial account-type compile failure; the unreachable sandboxed listener; the unsuccessful native movement inputs; the pre-fix legacy rectangle regression; and an initial enlargement test using the incorrect property name (`SpawnRange` corrected to `SpawnRadius`). These are not counted as successful cases.

## Source traces and next gates

- `Data/Scripts/Custom/Champions/Outbreak/WildernessEncounterReserve.cs`: `Proposals`, `ContainsHousingTile`, `Intersects` (no serialized world types).
- `Data/Scripts/Items/Houses/HousePlacement.cs`: `Check`, center/multi checks and existing staff bypass.
- `Data/Scripts/Custom/Government System/PlayerGovernmentSystem.cs`: `PlaceCityHall`, `GetMaximumCityBounds`; deed is retained and no civic objects are created on refusal.
- `Data/Scripts/Custom/XMLSpawner/BaseXmlSpawner.cs`: final `AddSpawnItem` overload, bounded post-property legacy placement guard.
- `Data/Scripts/Custom/Champions/System/CannedEvil/ChampionSpawn.cs`: `OnLocationChange`, `SpawnArea`, `SpawnRadius`, unchanged `Serialize`/`Deserialize`.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSite.cs`: permanent-policy proof and exclusion of unguarded villages/public/protected services as well as guarded towns.
- `tools/champion-outbreak/ReservationCandidateSurvey.cs`, `SelectReservationCandidates.py`, `RenderReservationAtlas.cs`: terrain, anonymous copied-world screening and mapped proposal.

Before approval: finish native traversal/anchors and western visibility, investigate the inactive malformed legacy controller, and complete the final occupancy/maximum-city-growth/portal checks against the user-selected October 17 copy. Only then may source review flags and matching site entries be promoted. Keep outbreak production activation disabled through all remaining reward, mechanic and pacing gates.

## Copied-client traversal follow-up - 2026-10-29

[SITE_TRAVERSAL.md](SITE_TRAVERSAL.md) records normal client walks through all anchors and opposite boundaries at five proposals: central226, northern224 on retry, eastern228, northwestern228 and western226 steps. Southeastern remains unresolved after two failed fixed-macro attempts; neither is counted as a completed route. Current-world approval is still absent.

Native views identified foliage overlapping the standing character near the northern/western boss positions and dense canopy at eastern brood0. Those need actual encounter/ground-warning visibility checks or clearer placements before approval. The northwestern river edge has an actual complete walking route and a visible boss position; that resolves access along this tested route, not every placement/use case in the footprint. No proposal or housing rule was activated.

The later instrumented002 copy completed southeastern's224 steps with zero deviations, zero throttle refusals and no changed movement rule. Combined verification now covers all six walking routes/1,356 exact steps across001/002, while preserving both old southeastern failures and the original failed report. Their cause did not reproduce and remains unproven. Walking access is established along the tested routes; foliage/encounter visibility and current-world approval remain open. Details and source identity checks are in [SITE_TRAVERSAL.md](SITE_TRAVERSAL.md).
