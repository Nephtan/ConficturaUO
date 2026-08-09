# SOURCE-BATCH-441 TapestryOfSosaria Guard Repair Closeout

## Summary

`SOURCE-BATCH-441` implemented `SB441-CAND-001`, a non-gated guard repair for TapestryOfSosaria double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Special/TapestryOfSosaria.cs`

`TapestryOfSosaria.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source tapestry is `Deleted`

The guard runs before the existing range check, gump close, gump send, and reach failure message.

## Preserved Behavior

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

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Special/TapestryOfSosaria.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Special/TapestryOfSosaria.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `TapestryOfSosaria.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-441` source commit: `a09b0db1`. `SOURCE-BATCH-442+` should run fresh candidate discovery after `SOURCE-BATCH-441`.
