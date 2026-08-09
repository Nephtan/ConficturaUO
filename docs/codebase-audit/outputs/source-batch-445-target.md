# SOURCE-BATCH-445 MagicStaffTarget Guard Repair Target

## Target

- Batch: `SOURCE-BATCH-445`
- Candidate: `SB445-CAND-001`
- System: `Items:Wands / MagicStaffTarget`
- Source file: `Data/Scripts/Items/Wands/MagicStaffTarget.cs`
- Behavior: add stale/null/mobile/source-staff guard to `MagicStaffTarget.OnTarget(Mobile from, object targeted)` before dispatching to `BaseMagicStaff.DoMagicStaffTarget(from, targeted)`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay exact-file rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

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

## Ready Goal Shape

Implement one local guard-only source edit, verify with targeted source/gate/overlay/serializer/forbidden-surface scans, build `Data/System/Source/Server.csproj` Debug/x86, run `.\ConficturaServer.exe -compileonly -nocache`, restore generated root artifacts, and commit as `fix: guard MagicStaffTarget interactions`.
