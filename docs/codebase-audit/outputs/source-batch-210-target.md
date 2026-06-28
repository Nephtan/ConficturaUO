# SOURCE-BATCH-210 Artifact_HammerofThor Guard Repair

## Target

- Candidate: `SB210-CAND-001`
- Behavior: add stale/null/mobile/source-item guard to `Artifact_HammerofThor.OnDoubleClick(Mobile from)`.
- System: `Items:Magical / Artifacts / HammerOfThor`
- File: `Data/Scripts/Items/Magical/Artifacts/Artifact_HammerofThor.cs`

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Exact-file active overlay rows: `0`
- No gated approval crossed.

## Allowed Change

Return immediately for null/deleted mobiles or a deleted source hammer before reading `Parent`, sending the hold-message, or casting `LightningSpell`.

## Must Stay Unchanged

- Hold-message: `You must be holding the hammer to unleash a lightning bolt.`
- Parent ownership rule.
- `new LightningSpell(from, this).Cast()` success behavior.
- Artifact metadata, item ID, name, hue, damage attributes, and `ArtySetup`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

- Candidate CSV import.
- Targeted `OnDoubleClick` guard and behavior scan.
- Exact-file POST-BATCH-Y gate scan.
- Exact-file active overlay scan.
- Serializer diff scan with zero context.
- Forbidden-surface diff scan.
- `Data/System/Source/Server.csproj` Debug/x86 build.
- `.\ConficturaServer.exe -compileonly -nocache`.
- `git diff --check`.
- Restore generated root build artifacts before staging.
