# SOURCE-BATCH-441 TapestryOfSosaria Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-441`
- Candidate: `SB441-CAND-001`
- System: `Items:Special / TapestryOfSosaria`
- Source file: `Data/Scripts/Items/Special/TapestryOfSosaria.cs`
- Behavior: add a stale/null/mobile/source-tapestry guard to `TapestryOfSosaria.OnDoubleClick(Mobile from)` before range check and display gump send.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- range requirement
- localized reach message `1019045`
- `InternalGump` image `0x2C95`
- secure-level context menu behavior
- `Level` persistence
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard TapestryOfSosaria interactions`.
