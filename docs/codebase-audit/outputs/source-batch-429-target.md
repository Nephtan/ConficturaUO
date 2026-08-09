# SOURCE-BATCH-429 ForgetfulGem Guard Repair

## Target

- Candidate: `SB429-CAND-001`
- System: `Magic:Base / ForgetfulGem`
- File: `Data/Scripts/Magic/Base/ForgetfulGem.cs`
- Behavior: add stale/null mobile guards to `ForgetfulGem.OnDoubleClick(Mobile from)` and `CrystalGump.OnResponse(NetState state, RelayInfo info)` before existing skill checks, sound sends, gump navigation, or skill reset behavior.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Magery/Necromancy/Elementalism skill checks, `CrystalGump` text/page navigation, sound IDs `0x5C9`, `0x4A`, and `0x65C`, skill `BaseFixedPoint` reset to `0`, location effect `0x3039`, no-skill warm-touch message, stationary crystal item setup, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
