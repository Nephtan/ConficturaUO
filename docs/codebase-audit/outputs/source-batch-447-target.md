# SOURCE-BATCH-447 BankCheck Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-447`
- Candidate: `SB447-CAND-001`
- System: `Items:Misc / BankCheck`
- Source file: `Data/Scripts/Items/Misc/BankCheck.cs`
- Behavior: add stale/null mobile and deleted source-check guards to `BankCheck.OnSingleClick(Mobile from)` and `BankCheck.OnDoubleClick(Mobile from)` before label send or bank-box lookup.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

- hue `0xB51`
- blessed loot type
- label/property display
- worth persistence
- single-click localized label packet for valid mobiles
- bank-box-only use rule
- `Delete()` before conversion
- `60000` gold chunking
- `TryDropItem` fallback behavior
- new `BankCheck` remainder behavior
- localized messages `1042672` and `1047026`
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/changed-line-serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard BankCheck interactions`.
