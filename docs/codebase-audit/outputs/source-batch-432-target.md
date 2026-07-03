# SOURCE-BATCH-432 RobeOfTeleportation Guard Repair

## Target

- Candidate: `SB432-CAND-001`
- System: `Items:Magical / Artifact_RobeOfTeleportation`
- File: `Data/Scripts/Items/Magical/Artifacts/Artifact_RobeOfTeleportation.cs`
- Behavior: add a stale/null mobile and deleted source-robe guard to `Artifact_RobeOfTeleportation.OnDoubleClick(Mobile from)` before existing worn-robe check and teleport cast behavior.

## Fence Result

- POST-BATCH-Y exact-file gate hits: `0`
- Active overlay rows: `0`
- No gated approval crossed.

## Must Stay Unchanged

Wearing requirement `Parent == from`, message `You must be wearing the robe to teleport.`, `TeleportSpell(from, this).Cast()` behavior, Arty setup level/text, randomized hue, name, serialization layout/versioning, namespace/type/file layout, project/config/data files, staff/access behavior, economy/reward tuning, region/map policy, and reorganization state.

## Verification Required

Targeted source scan, exact gate/overlay scans, serializer diff scan, forbidden-surface diff scan, `Data/System/Source/Server.csproj` Debug/x86 build, `.\ConficturaServer.exe -compileonly -nocache`, `git diff --check`, and generated root artifact restoration before staging.
