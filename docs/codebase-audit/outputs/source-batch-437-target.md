# SOURCE-BATCH-437 GandalfsStaff Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-437`
- Candidate: `SB437-CAND-001`
- System: `Items:Magical / Artifacts / GandalfsStaff`
- Source file: `Data/Scripts/Items/Magical/Artifacts/Artifact_GandalfsStaff.cs`
- Behavior: add a stale/null/mobile/source-staff guard to `Artifact_GandalfsStaff.OnDoubleClick(Mobile from)` before cooldown calculation, held-item messages, and `SummonDragonSpell` dispatch.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- Inactive backlog rows: `1` (`FalsePositive`)
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard GandalfsStaff interactions`.
