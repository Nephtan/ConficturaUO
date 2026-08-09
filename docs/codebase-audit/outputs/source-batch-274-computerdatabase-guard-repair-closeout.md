# SOURCE-BATCH-274 ComputerDatabase Guard Repair Closeout

## Summary

`SOURCE-BATCH-274` implemented `SB274-CAND-001` in `Data/Scripts/Items/Technology/ComputerDatabase.cs`.

`ComputerDatabase.OnDoubleClick(Mobile from)` now returns immediately for null/deleted mobiles or deleted terminals before range checks, gump opening, or sound playback. `ComputerDatabaseGump.OnResponse(NetState state, RelayInfo info)` now returns immediately for null `NetState`, null `RelayInfo`, or null/deleted mobiles before button handling, appearance mutation, feature recording, sound playback, or gump redisplay.

## Preserved Behavior

- Terminal range `4`, `ComputerDatabaseGump` opening, and open sound `0x54D`.
- Gump layout, text, items, and button IDs `1` through `32`.
- Random skin and hair color choices and exact hue mappings.
- `Hue`, `HairHue`, `FacialHairHue`, `RecordSkinColor`, `RecordHairColor`, and `RecordBeardColor` mutation behavior.
- `RecordFeatures(true)`, gump redisplay, response sound `0x54B`, and fallback sound `0x54D`.
- Serialization layout/versioning, constructors, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map behavior, appearance policy, and reorganization state.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits: `0`.
- Exact-file unresolved active overlay rows: `0`.
- Resolved exact-file audit rows remain `Documented`, `ReviewedNoChange`, and `SafeNoChange`; this batch did not change documentation policy, serialization, source layout, appearance policy, or gated behavior.

## Verification

- Passed: candidate CSV import.
- Passed: targeted source scan confirmed the stale/null/mobile/netstate guards and preserved range check, gump open/redisplay calls, sounds, button IDs, skin/hair assignment, feature recording, and serializer methods.
- Passed: POST-BATCH-Y exact-file gate scan found `0` gate hits.
- Passed: exact-file unresolved active overlay scan found `0` rows. Resolved `Documented`, `ReviewedNoChange`, and `SafeNoChange` rows remain nonblocking because source behavior and docs policy were untouched except stale/null safety.
- Passed: serializer diff scan found no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Passed: forbidden-surface diff scan found no command, event hook, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes. The only gump-surface diff is the named stale response guard.
- Passed: changed-file scan found only `Data/Scripts/Items/Technology/ComputerDatabase.cs` as a source change, with no project/config/data changes.
- Passed: `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild.
- Passed: `.\ConficturaServer.exe -compileonly -nocache`.
- Passed: `git diff --check` with the repository's existing CRLF warning only.

## Artifact Restoration

- Restored tracked root build artifacts after verification: `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.
