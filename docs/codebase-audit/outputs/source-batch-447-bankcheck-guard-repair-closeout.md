# SOURCE-BATCH-447 BankCheck Guard Repair Closeout

## Summary

`SOURCE-BATCH-447` implemented `SB447-CAND-001`, a non-gated guard repair for BankCheck display and use entry points.

## Source Change

File changed: `Data/Scripts/Items/Misc/BankCheck.cs`

`BankCheck.OnSingleClick(Mobile from)` and `BankCheck.OnDoubleClick(Mobile from)` now return safely when:

- `from == null`
- `from.Deleted`
- the source bank check is deleted

The guards run before the existing label packet send and bank-box lookup paths dereference `from` or the source item.

## Preserved Behavior

- hue `0xB51`
- blessed loot type
- label/property display for valid mobiles
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

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Misc/BankCheck.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Misc/BankCheck.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guards and preserved deposit behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- changed-line serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `BankCheck.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-447` source commit: `3ff3cc22`. `SOURCE-BATCH-448+` should run fresh candidate discovery after `SOURCE-BATCH-447`.
