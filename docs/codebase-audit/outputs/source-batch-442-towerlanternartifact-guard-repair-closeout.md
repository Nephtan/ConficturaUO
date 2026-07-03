# SOURCE-BATCH-442 TowerLanternArtifact Guard Repair Closeout

## Summary

`SOURCE-BATCH-442` implemented `SB442-CAND-001`, a non-gated guard repair for TowerLanternArtifact double-click interaction.

## Source Change

File changed: `Data/Scripts/Items/Decorations/Artifacts/SEDecorationArtifacts.cs`

`TowerLanternArtifact.OnDoubleClick(Mobile from)` now returns safely when:

- `from == null`
- `from.Deleted`
- the source lantern is `Deleted`

The guard runs before the existing range check, `IsOn` toggle, sound playback, and reach failure message.

## Preserved Behavior

- `TowerLanternArtifact` item IDs `0x24BF` and `0x24C0`
- `IsOn` property semantics
- range requirement
- sounds `0x3BE` and `0x47`
- localized reach message `1019045`
- `LightType.Circle225` constructor/deserialize behavior
- all other SE decoration artifact classes
- serialization layout/versioning
- namespace/type/file layout
- project/config/data files
- staff/access behavior
- economy/reward tuning
- region/map policy
- reorganization state

## Fence Evidence

- POST-BATCH-Y exact-file gate hits for `Data/Scripts/Items/Decorations/Artifacts/SEDecorationArtifacts.cs`: `0`
- Active overlay exact-file rows for `Data/Scripts/Items/Decorations/Artifacts/SEDecorationArtifacts.cs`: `0`
- No gated approval crossed.

## Verification

Passed:

- candidate CSV import
- targeted source scan for new guard and preserved behavior
- exact-file POST-BATCH-Y gate scan
- exact-file active overlay scan
- serializer diff scan: no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes
- forbidden-surface diff scan: only `SEDecorationArtifacts.cs` source path plus audit artifacts
- `Data/System/Source/Server.csproj` Debug/x86 build with Visual Studio MSBuild
- `.\ConficturaServer.exe -compileonly -nocache`
- `git diff --check`
- generated tracked root build artifacts restored before staging

## Commit

`SOURCE-BATCH-442` source commit: pending. `SOURCE-BATCH-443+` should run fresh candidate discovery after `SOURCE-BATCH-442`.
