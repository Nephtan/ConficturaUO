# SOURCE-BATCH-438 StaffofSnakes Guard Repair Closeout

## Summary

`SOURCE-BATCH-438` implemented `SB438-CAND-001`, a non-gated guard repair for StaffofSnakes double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs`

`Artifact_StaffofSnakes.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source staff is `Deleted`

The guard runs before cooldown calculation, held-item messages, and the existing `SummonSnakesSpell(from, this).Cast()` dispatch.

## Preserved Behavior

- staff name/hue
- artifact setup
- poison elemental damage
- spell channeling
- `SlayerName.SnakesBane`
- `HitPoisonArea`
- cooldown calculation
- held-staff requirement
- failure/wait messages
- `SummonSnakesSpell(from, this).Cast()`
- `TimeUsed` update
- `TimeUsed` serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs`: `0`
- Inactive backlog rows: `1` (`FalsePositive`)
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `Artifact_StaffofSnakes.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-438` source commit: pending. `SOURCE-BATCH-439+` should run fresh candidate discovery after `SOURCE-BATCH-438`.
