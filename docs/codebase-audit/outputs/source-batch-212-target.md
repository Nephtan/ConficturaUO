# SOURCE-BATCH-212 BaseLight Guard Repair

## Target

- Candidate: `SB212-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `BaseLight.OnDoubleClick(Mobile from)`.
- System: `Items:Construction / Lights / BaseLight`
- File: `Data/Scripts/Items/Construction/Lights/BaseLight.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or a deleted source light before reading protected/access state, checking range, or toggling the light.

## Must Stay Unchanged

- Burnt-out no-op behavior.
- Protected-player rule.
- Range requirement.
- `Ignite()` / `Douse()` behavior.
- Lit/unlit/burnt-out sounds.
- Burnout timer behavior.
- Clothing processing when a worn light changes state.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Candidate CSV import.
- Targeted `OnDoubleClick` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
