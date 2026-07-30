# SOURCE-BATCH-212 BaseLight Guard Repair Closeout

## Summary

`SOURCE-BATCH-212` implemented `SB212-CAND-001` in `Data/Scripts/Items/Construction/Lights/BaseLight.cs`.

`BaseLight.OnDoubleClick(Mobile from)` now guards stale/null/deleted interaction state before reading protected/access state, checking range, or toggling the light.

## Preserved Behavior

- Burnt-out no-op behavior, protected-player rule, range requirement, `Ignite()` / `Douse()` behavior, lit/unlit/burnt-out sounds, burnout timer behavior, worn-light clothing processing, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state were preserved.

## Gate Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Construction/Lights/BaseLight.cs`: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Verification

- Candidate CSV import: passed for `SB212-CAND-001` and `SB212-CAND-002`.
- Targeted source scan: passed; new mobile/source guard is present and existing light behavior remains present.
- Serializer diff scan: passed; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes.
- Forbidden-surface diff scan: passed; no command, event hook, gump, packet, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build: passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache`: passed.
- `git diff --check`: passed with expected LF-to-CRLF warnings only.
- Generated root build artifacts restoration: completed for `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb`.

## Result

Ready for focused `SOURCE-BATCH-212` commit.
