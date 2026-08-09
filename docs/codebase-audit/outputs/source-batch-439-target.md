# SOURCE-BATCH-439 Head Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-439`
- Candidate: `SB439-CAND-001`
- System: `Items:Misc / Bodies / Head`
- Source file: `Data/Scripts/Items/Misc/Bodies/Head.cs`
- Behavior: add a stale/null/mobile/source-head guard to `Head.OnDoubleClick(Mobile from)` before item-ID cycling.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Inactive backlog rows: `1`
- No gated approval crossed.

## Must Stay Unchanged

- `DefaultName` behavior
- `HeadType` values
- `PlayerName` and `Job` properties
- `AddNameProperties`
- item-ID cycling sequence
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard Head interactions`.
