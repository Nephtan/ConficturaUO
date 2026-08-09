# SOURCE-BATCH-438 StaffofSnakes Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-438`
- Candidate: `SB438-CAND-001`
- System: `Items:Magical / Artifacts / StaffofSnakes`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Artifact_StaffofSnakes.cs`
- Behavior: add a stale/null/mobile/source-staff guard to `Artifact_StaffofSnakes.OnDoubleClick(Mobile from)` before cooldown calculation, held-item messages, and `SummonSnakesSpell` dispatch.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Inactive backlog rows: `1` (`FalsePositive`)
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard StaffofSnakes interactions`.
