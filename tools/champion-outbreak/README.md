# Overworld survey helpers

These offline helpers reuse the locally installed Pandora Ultima map library. They are outside `Data/Scripts` and are not server scripts. Compile with the .NET Framework x86 C# compiler, referencing `System.Drawing.dll` and the installed `Ultima.dll`. Put a copy of that DLL beside the helper executable in a private test output directory; do not commit binaries.

`RenderMap.exe ASSET_DIRECTORY OUTPUT_PNG OVERLAY_CSV` renders mainland Sosaria. Coordinates passed to the SDK renderer are blocks (640 by 391), producing 5120 by 3128 terrain pixels before the overview is scaled. Overlay rows are `kind,x,y,width,height`; kinds are house, city, portal and nohousing. Keep copied-save details private.

`VerifyMapReader.cs` additionally references a matching `ConficturaServer.exe` and compiled script assembly (`Data.dll`). Pass the asset directory. It compares server and SDK land IDs/elevations at four sample coordinates and throws on disagreement. Keep both assemblies beside the executable. Run before trusting rendered overlays.

The legacy SDK's Trammel uses file index 0. Both helpers remap its three file-0 inputs to the corresponding file-1 assets in memory; asset files and installed app configuration are not edited. Rendering is not a walkability, housing, occupancy or native-client acceptance test.

Current full private survey artifacts: `D:/ConficturaUOTestTools/outbreak-map-survey-20261028/`. The source save is dated October 17, 2026, not a live occupancy feed. The private CCWM-derived exporter retains exact serializer assemblies and a hash-verified separate save copy; its extra geometry output omits player identities. No public/repository export of player save data is intended.

`HousingEligibilityProbe.cs` uses actual `HousePlacement.Check` against a standard small-house multi across the original candidate footprints. Any legal placement disqualifies a candidate. A negative result for this one shape is explicitly inconclusive and never approves a site. Reference matching core/runtime assemblies when compiling and pass asset directory plus Regions.xml path. No house is constructed.


`HousingCandidateSurvey.cs` and `ProtectedLandmarkSurvey.cs` reference matching server/runtime assemblies, like the eligibility probe. Pass asset directory and Regions.xml. They bootstrap empty world dictionaries and checked-in regions without loading saves, calling script Initialize, opening listeners or creating houses. Their observations cannot establish live occupancy.

The candidate survey samples an eight-tile grid, then checks actual topology before detailed two-shape housing screening. It has a 500 detailed-candidate cap and never approves a site; unsampled locations and untested house templates remain unknown. The landmark survey measures connected ground and 5x5 staging inside named protected rectangles; it does not certify access routes or fit the current encounter's clearance box.

`RenderLandmarkAtlas.exe ASSET_DIRECTORY OUTPUT_PNG OVERLAY_CSV` uses the same Ultima.dll/System.Drawing references and remapping as RenderMap. Its six landmark rectangles and component counts are a fixed 2026-10-28 survey snapshot, not a live report. Regenerate and review them with the landmark probe if assets/regions change. The optional copied-save geometry remains private. Results and limitations: `docs/champion-outbreak/SITE_SURVEY.md`.


`ReservationCandidateSurvey.exe ASSET_DIRECTORY REGIONS_XML CANDIDATE_CSV` references matching core/runtime assemblies. Input rows are scouting-region,x,y without a header. It validates the full existing combat layout while hypothetically allowing permanent protection, exports service type names beside the input file, and reports static roof counts across each proposed reserve. It neither approves a site nor enables housing restrictions.

`SelectReservationCandidates.py --help` describes inputs for the private terrain logs, anonymous geometry overlay and private mobile/item exports. It rejects architecture, copied house/city/portal/service/legacy envelopes, retains one lead per scouting region and writes an anonymous report. Keep raw save exports and overlays private. Scouting IDs in that report are mapped to the canonical source IDs in docs/champion-outbreak/reservation-proposals.csv.

`RenderReservationAtlas.exe ASSET_DIRECTORY OUTPUT_PNG OVERLAY_CSV SELECTED_CSV` takes six rows of label,x,y,connected_tiles without a header, using the same Ultima.dll/System.Drawing dependencies and in-memory map remapping. The optional service geometry uses kind service. Its image is a proposed 161x161 reservation and 113x113 combat layout, not proof of activation or traversal. See docs/champion-outbreak/RESERVATIONS.md for the accepted checkpoint and limits.
