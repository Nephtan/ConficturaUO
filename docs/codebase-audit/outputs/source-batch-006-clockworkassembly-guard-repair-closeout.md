# SOURCE-BATCH-006 ClockworkAssembly Guard Repair Closeout

Reviewed at: 2026-06-16T11:29:34.9621783-05:00

## Summary

`SOURCE-BATCH-006` implemented the `SB004-CAND-003` ClockworkAssembly guard repair in `Data/Scripts/Items/Misc/ClockworkAssembly.cs`.

The batch added stale/null/backpack guards before ClockworkAssembly interaction paths dereference mobile, backpack, skill, follower, resource, or assembly state. No source files outside ClockworkAssembly were edited.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before reading `from.Backpack`.
- `OnDoubleClick(Mobile from)` now treats deleted ClockworkAssembly state, missing backpacks, or assemblies outside the backpack as the existing backpack-use failure.

## Preserved Behavior

- Tinkering threshold of 60.0.
- Follower-slot requirement.
- Skill scalar calculation.
- Resource list and quantities: 1 PowerCrystal, 50 IronIngots, 50 BronzeIngots, and 5 Gears.
- Consume-order result messages.
- Golem creation and `SetControlMaster(from)` behavior.
- Assembly `Delete()` success behavior.
- Golem `MoveToWorld(from.Location, from.Map)` placement.
- Sound `0x241`.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/ClockworkAssembly.cs`: 0.
- Active overlay rows for `Data/Scripts/Items/Misc/ClockworkAssembly.cs`: 0.
- No gated approval was crossed.

## Verification

- Targeted source scan confirmed the new mobile, deleted-assembly, and backpack guards.
- Targeted source scan confirmed existing tinkering threshold, follower-slot requirement, scalar branches, resource quantities, golem creation/control path, assembly delete path, move-to-world path, and sound remain present.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.

## Outputs

- `Data/Scripts/Items/Misc/ClockworkAssembly.cs`
- `docs/codebase-audit/outputs/source-batch-006-target.md`
- `docs/codebase-audit/outputs/source-batch-006-clockworkassembly-guard-repair-closeout.md`
