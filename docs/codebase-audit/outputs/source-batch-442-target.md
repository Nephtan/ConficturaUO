# SOURCE-BATCH-442 TowerLanternArtifact Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-442`
- Candidate: `SB442-CAND-001`
- System: `Items:Decorations / SEDecorationArtifacts / TowerLanternArtifact`
- Source file: `Data/Scripts/Items/Decorations/Artifacts/SEDecorationArtifacts.cs`
- Behavior: add a stale/null/mobile/source-lantern guard to `TowerLanternArtifact.OnDoubleClick(Mobile from)` before range check, `IsOn` toggle, and sound playback.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard TowerLanternArtifact interactions`.
