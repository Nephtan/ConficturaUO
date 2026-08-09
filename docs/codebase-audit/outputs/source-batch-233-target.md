# SOURCE-BATCH-233 UnknownKeg Identification Guard Repair

## Target

- Candidate: `SB233-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `UnknownKeg.OnDoubleClick(Mobile from)`.
- System: `Items:Unknown / UnknownKeg`
- File: `Data/Scripts/Items/Unknown/UnknownKeg.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted unknown keg items before movable, range, backpack-policy, skill, effect, or delete state reads.

## Must Stay Unchanged

- Movable rejection.
- Range 3 and range 1 checks.
- `IdentifyItemsOnlyInPack` policy.
- Tasting skill behavior.
- Sound/animation behavior.
- Keg effect behavior.
- Source item `Delete()` behavior.
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
