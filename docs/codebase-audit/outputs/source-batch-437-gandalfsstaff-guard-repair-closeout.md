# SOURCE-BATCH-437 GandalfsStaff Guard Repair Closeout

## Summary

`SOURCE-BATCH-437` implemented `SB437-CAND-001`, a non-gated guard repair for GandalfsStaff double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs`

`Artifact_GandalfsStaff.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source staff is `Deleted`

The guard runs before cooldown calculation, held-item messages, and the existing `SummonDragonSpell(from, this).Cast()` dispatch.

## Preserved Behavior

- Merlin staff name/hue
- artifact setup
- skill bonuses and attributes
- cooldown calculation
- held-staff requirement
- failure/wait messages
- `SummonDragonSpell(from, this).Cast()`
- `TimeUsed` update
- `TimeUsed` serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs`: `0`
- Inactive backlog rows: `1` (`FalsePositive`)
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `Artifact_GandalfsStaff.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-437` source commit: `e055c0f4`. `SOURCE-BATCH-438+` should run fresh candidate discovery after `SOURCE-BATCH-437`.
