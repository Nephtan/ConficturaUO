# SOURCE-BATCH-222 UnknownWand Guard Repair

## Target

- Candidate: `SB220-CAND-003`
- Behavior: add stale/null/mobile/source-item guard to `UnknownWand.OnDoubleClick(Mobile from)`.
- System: `Items:Unknown / Identification / UnknownWand`
- File: `Data/Scripts/Items/Unknown/UnknownWand.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source unknown wands before movability, backpack, range, or identification dispatch checks.

## Must Stay Unchanged

- Movable rejection message.
- Backpack-use message.
- Range message.
- `ItemIdentification.IDItem` dispatch.
- Wand metadata, name properties, and identification labels.
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
