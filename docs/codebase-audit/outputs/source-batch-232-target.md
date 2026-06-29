# SOURCE-BATCH-232 UnknownLiquid Identification Guard Repair

## Target

- Candidate: `SB232-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `UnknownLiquid.OnDoubleClick(Mobile from)`.
- System: `Items:Unknown / UnknownLiquid`
- File: `Data/Scripts/Items/Unknown/UnknownLiquid.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted unknown liquid items before movable, range, backpack-policy, skill, effect, reaction, or delete state reads.

## Must Stay Unchanged

- Movable rejection.
- Range 3 check.
- `IdentifyItemsOnlyInPack` policy.
- Tasting skill behavior.
- Sound/animation behavior.
- Liquid effect/reaction behavior.
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
