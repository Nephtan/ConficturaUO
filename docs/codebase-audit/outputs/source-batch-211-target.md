# SOURCE-BATCH-211 Artifact_HelmOfBrilliance Guard Repair

## Target

- Candidate: `SB210-CAND-002`
- Behavior: add stale/null/mobile/source-item guard to `Artifact_HelmOfBrilliance.OnDoubleClick(Mobile from)`.
- System: `Items:Magical / Artifacts / HelmOfBrilliance`
- File: `Data/Scripts/Items/Magical/Artifacts/Artifact_HelmOfBrilliance.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or a deleted source helm before reading `Parent`, sending the wear-message, or casting `FireballSpell`.

## Must Stay Unchanged

- Wear-message: `You must be wearing the helm to unleash a fireball.`
- Parent ownership rule.
- `new FireballSpell(from, this).Cast()` success behavior.
- Artifact metadata, name, hue, `NightSight`, `FireBonus`, and `ArtySetup`.
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
