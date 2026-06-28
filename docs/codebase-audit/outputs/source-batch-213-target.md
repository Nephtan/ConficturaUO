# SOURCE-BATCH-213 TowerLantern Guard Repair

## Target

- Candidate: `SB212-CAND-002`
- Behavior: add stale/null/mobile/source-item guard to `TowerLantern.OnDoubleClick(Mobile from)`.
- System: `Items:Construction / Lights / TowerLantern`
- File: `Data/Scripts/Items/Construction/Lights/TowerLantern.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or a deleted source lantern before reading range or delegating to `BaseLight`.

## Must Stay Unchanged

- Range `1` reach check.
- Localized reach message `1019045`.
- `base.OnDoubleClick(from)` delegation.
- `ISecurable` context menu behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Targeted `OnDoubleClick` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
