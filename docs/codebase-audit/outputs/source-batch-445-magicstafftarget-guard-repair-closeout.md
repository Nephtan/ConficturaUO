# SOURCE-BATCH-445 MagicStaffTarget Guard Repair Closeout

## Summary

`SOURCE-BATCH-445` implemented `SB445-CAND-001`, a non-gated guard repair for MagicStaffTarget dispatch.

## Source Change

File changed: `Data/Scripts/Items/Wands/MagicStaffTarget.cs`

`MagicStaffTarget.OnTarget(Mobile from, object targeted)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source staff is `null`
- the source staff is `Deleted`

The guard runs before the existing `BaseMagicStaff.DoMagicStaffTarget(from, targeted)` dispatch.

## Preserved Behavior

- target range `6`
- harmful flag `false`
- `TargetFlags.None`
- `BaseMagicStaff.DoMagicStaffTarget` dispatch
- targeted object pass-through
- `BaseMagicStaff` behavior
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Wands/MagicStaffTarget.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Wands/MagicStaffTarget.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `MagicStaffTarget.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-445` source commit: `c8a6ede1`. `SOURCE-BATCH-446+` should run fresh candidate discovery after `SOURCE-BATCH-445`.
