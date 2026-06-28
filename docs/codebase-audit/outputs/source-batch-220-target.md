# SOURCE-BATCH-220 UnidentifiedArtifact Guard Repair

## Target

- Candidate: `SB220-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `UnidentifiedArtifact.OnDoubleClick(Mobile from)`.
- System: `Items:Unknown / Identification / UnidentifiedArtifact`
- File: `Data/Scripts/Items/Unknown/UnidentifiedArtifact.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted source artifacts before movability, backpack, range, or identification dispatch checks.

## Must Stay Unchanged

- Movable rejection message.
- Backpack-use message.
- Range message.
- `ItemIdentification.IDItem` dispatch.
- Name properties and identification labels.
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
