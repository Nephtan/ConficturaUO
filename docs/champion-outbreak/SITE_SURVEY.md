# Overworld outbreak site survey - 2026-10-28

Status: **No replacement site approved.** This survey identifies usable world geography and a placement-design conflict; it does not authorize housing changes or activate an encounter.

## Findings

The original six forest sites permit actual small-house placement inside their encounter footprints. They remain retired. A failed placement at the exact center had been insufficient evidence of permanent protection.

The universal square layout is a provisional implementation constraint, not a demonstrated property of the map. It requires a 113x113 combat footprint, a 161x161 protected clearance box, three fixed brood offsets and 4,000 connected walkable tiles. These requirements can reject a substantial protected combat area simply because it has different proportions.

**The central Orc Fort has 4,041 connected walkable tiles inside its existing 74x64 permanent housing ban.** This makes it the strongest measured lead for a site-specific land encounter. It does not meet the current square footprint/buffer rule, and it has not received native-client acceptance. No reviewed catalog entry has been added.

The map supports a more useful distinction for a future family of events: large forts, smaller forts/towers, ruins, and water each need their own encounter layout. It does not follow that six places suitable for different events can all support the same three-brood Arachnid layout.

## Protected landmark shortlist

Counts below use the real server's terrain/region rules. Connectivity is cardinal adjacency with at most two elevation units between cells, matching the current placement validator. A 5x5 staging center means its whole square belongs to that connected component. These are geometry measurements, not pathfinding, encounter capacity or native gameplay acceptance.

| Landmark / center | Existing protected area | Largest connected component | 5x5 staging centers | Copied portals inside | House areas within 24 tiles |
|---|---:|---:|---:|---:|---:|
| Central Orc Fort (2316,1804) | 74x64 | 4,041 | 1,479 | 0 | 0 |
| Brigand Fort (3012,386) | 67x71 | 2,487 | 830 | 1 | 0 |
| Lich Tower (3509,2283) | 41x38 | 1,322 | 257 | 1 | 0 |
| Ratmen Fort (3402,1175) | 37x37 | 1,186 | 392 | 2 | 1 |
| Northern Orc Fort (573,488) | 29x27 | 534 | 16 | 1 | 0 |
| Swamp Ruins of the Dead (522,203) | 38x26 | 303 | 42 | 0 | 0 |

All six listed rectangles satisfy the current conservative permanent-region test: innermost static `OutDoorBadRegion`, which unconditionally rejects player housing. Other tested landmarks are less promising: Castle Exodus has only 648 tiles in its largest ground component and one 5x5 staging center; Ratmen Ruins has no 5x5 staging center. Buildings, doors, elevations, existing populations and access routes still need inspection using the copied world and client.

The central Orc Fort also has no copied portal candidates, house-area rectangles or city-stone rectangles within a 24-tile expansion of its protected rectangle. That is an occupancy observation from the copied save, **not** proof that the expansion prohibits future housing. Brigand Fort has five copied portal candidates in that expansion; Ratmen Fort has three and an adjacent house area. These should not be promoted as interchangeable safe sites.

## Wider search and its limits

`HousingCandidateSurvey.cs` samples 250,240 mainland positions on an eight-tile grid using actual `HousePlacement.Check`, with an ordinary applicant trained to 120 Camping/Tracking. Prefix sums select 1,094 boxes with no sampled legal small-house center and enough sampled passable terrain to investigate. Every one of those sampled candidate centers fails the current rigid layout's topology, staging or reservation checks. This is **not** an exhaustive proof that no suitable location exists between grid samples or with a different layout.

An earlier detailed attempt screened 500 nearby candidates before its configured cap, eliminating another 163 from discovered legal-house plots. It found 478 boxes without a legal placement for the two tested common shapes, but none passed the rigid layout. Those negative shape tests never established protection against all housing templates. The later topology-first run avoids spending exhaustive house tests on layouts already unsuitable for combat; both logs are retained.

## Design recommendation, pending agreement

Use a reusable catalog of site-specific protected combat footprints and anchors, rather than require every event type to fit one square. Investigate the central Orc Fort first for Arachnid; retain the smaller landmarks as leads for appropriately sized future encounters. Survey the permanent boundary, existing population and surrounding approaches before deciding how an internal safety margin can work. Do not silently remove the buffer or lower combat-space criteria to mark a site approved.

If the current larger wilderness layout is retained, newly reserving unclaimed land would be a separate housing-policy decision. That is outside the present specification and has not been implemented. Empty copied-save land cannot be presented as an existing permanent non-housing site.

## Sources and repeatability

- `Data/Scripts/Items/Houses/HousePlacement.cs`: `Check`, foundation/terrain/border checks and road exemptions; staff placement bypass is not player eligibility.
- `Data/Scripts/System/Regions/OutDoorBadRegion.cs`: unconditional `AllowHousing=false`.
- `Data/Scripts/System/Regions/OutDoorRegion.cs`: conditional permission depends on the applicant's Ranger Outpost region and skills; it is not treated as an unconditional ban.
- `Data/System/XML/Regions.xml`: Sosaria's 186 loaded region definitions, including exact landmark rectangles.
- `Data/Scripts/Custom/Champions/Outbreak/ArachnidOutbreakSite.cs`: `Validate`, `TryGetPoint`, `HasPermanentHousingProhibition`, `IsReserved`.
- `tools/champion-outbreak/ProtectedLandmarkSurvey.cs`: real region/terrain bootstrap, connected components and 5x5 staging measurements.
- `tools/champion-outbreak/HousingCandidateSurvey.cs`: sampled ranking, actual placement screening and topology-first elimination. Its terrain-only wrapper bypasses permanent-proof checks solely for exploratory rejection; it cannot approve a runtime site.
- `tools/champion-outbreak/RenderLandmarkAtlas.cs`: six-panel map atlas using the verified legacy SDK file-index remapping. Rendered geometry is not native-client evidence.

Private evidence root: `D:/ConficturaUOTestTools/outbreak-map-survey-20261028/`. Atlas: `protected-landmarks.png`. Logs under `occupancy-001/`: `protected-landmark-survey.log`, `adaptive-housing-survey.log`, `adaptive-housing-survey-attempt-2.log`. Matching server/runtime assemblies are from `outbreak-20261028-controller-005`; no saves or listeners are loaded by these terrain probes.

Occupancy comes from a separate hash-verified copy of the test world's **2026-10-17** save, not today's live world. Original/copied save hashes were preserved during the earlier read-only export; raw exports and player identities remain private. Filtered geometry has no owner names. No production process, save, installed editor setting or housing rule was changed.
