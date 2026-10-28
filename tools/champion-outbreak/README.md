# Overworld survey helpers

These offline helpers reuse the locally installed Pandora Ultima map library. They are outside `Data/Scripts` and are not server scripts. Compile with the .NET Framework x86 C# compiler, referencing `System.Drawing.dll` and the installed `Ultima.dll`. Put a copy of that DLL beside the helper executable in a private test output directory; do not commit binaries.

`RenderMap.exe ASSET_DIRECTORY OUTPUT_PNG OVERLAY_CSV` renders mainland Sosaria. Coordinates passed to the SDK renderer are blocks (640 by 391), producing 5120 by 3128 terrain pixels before the overview is scaled. Overlay rows are `kind,x,y,width,height`; kinds are house, city, portal and nohousing. Keep copied-save details private.

`VerifyMapReader.cs` additionally references a matching `ConficturaServer.exe` and compiled script assembly (`Data.dll`). Pass the asset directory. It compares server and SDK land IDs/elevations at four sample coordinates and throws on disagreement. Keep both assemblies beside the executable. Run before trusting rendered overlays.

The legacy SDK's Trammel uses file index 0. Both helpers remap its three file-0 inputs to the corresponding file-1 assets in memory; asset files and installed app configuration are not edited. Rendering is not a walkability, housing, occupancy or native-client acceptance test.

Current full private survey artifacts: `D:/ConficturaUOTestTools/outbreak-map-survey-20261028/`. The source save is dated October 17, 2026, not a live occupancy feed. The private CCWM-derived exporter retains exact serializer assemblies and a hash-verified separate save copy; its extra geometry output omits player identities. No public/repository export of player save data is intended.

`HousingEligibilityProbe.cs` uses actual `HousePlacement.Check` against a standard small-house multi across the original candidate footprints. Any legal placement disqualifies a candidate. A negative result for this one shape is explicitly inconclusive and never approves a site. Reference matching core/runtime assemblies when compiling and pass asset directory plus Regions.xml path. No house is constructed.
