# SOURCE-BATCH-431 MagicObjectTarget Guard Repair

## Target

- Candidate: `SB431-CAND-001`
- System: `Magic:Misc / MagicObjectTarget`
- File: `Data/Scripts/Magic/Misc/MagicObjectTarget.cs`
- Behavior: add stale/null mobile and source magic-object guards to `MagicObjectTarget.OnTarget(Mobile from, object targeted)` before dispatching to `BaseMagicObject.DoMagicObjectTarget`.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Target range `6`, harmful flag `false`, `TargetFlags.None`, `BaseMagicObject.DoMagicObjectTarget` dispatch, targeted object pass-through, `BaseMagicObject.OnMagicObjectUse` target assignment, `BaseMagicObject.DoMagicObjectTarget` rules, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
