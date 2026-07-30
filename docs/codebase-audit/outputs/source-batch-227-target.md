# SOURCE-BATCH-227 MarbleWell Guard Repair

## Target

- Candidate: `SB227-CAND-001`
- Behavior: add stale/null/mobile/source-component/parent-addon guard to `MarbleWellPiece.OnDoubleClick(Mobile from)`.
- System: `Items:Construction / Wells / MarbleWell`
- File: `Data/Scripts/Items/Construction/Wells/marblewell.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles, deleted source well components, or missing/deleted parent well addons before range, thirst, message, or parent-addon state reads.

## Must Stay Unchanged

- Range 4 check.
- Not-thirsty message.
- Randomized drink messages.
- Get-closer message.
- Thirst set-to-20 behavior.
- Addon layout and deed behavior.
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
