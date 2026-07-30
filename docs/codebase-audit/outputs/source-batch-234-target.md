# SOURCE-BATCH-234 UnknownScroll Identification Guard Repair

## Target

- Candidate: `SB234-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `UnknownScroll.OnDoubleClick(Mobile from)`.
- System: `Items:Unknown / UnknownScroll`
- File: `Data/Scripts/Items/Unknown/UnknownScroll.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or deleted unknown scroll items before movable, backpack-policy, range, or item-identification state reads.

## Must Stay Unchanged

- Movable rejection.
- `IdentifyItemsOnlyInPack` policy.
- Range 3 check.
- Message behavior.
- `Server.Items.ItemIdentification.IDItem(from, this, this, false)` call.
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
