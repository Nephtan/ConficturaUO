# SOURCE-BATCH-005 PowerCrystal Target Guard Repair Closeout

Reviewed at: 2026-06-16T11:22:52.2093583-05:00

## Summary

`SOURCE-BATCH-005` implemented the `SB004-CAND-002` PowerCrystal target guard repair in `Data/Scripts/Items/Misc/PowerCrystal.cs`.

The batch added stale/null/backpack guards before PowerCrystal interaction paths dereference mobile, backpack, source crystal, or target golem-porter state. No source files outside PowerCrystal were edited.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before reading `from.Backpack`.
- `OnDoubleClick(Mobile from)` now treats deleted PowerCrystal state, missing backpacks, or crystals outside the backpack as the existing backpack-use failure.
- `PowerTarget.OnTarget(Mobile from, object targeted)` now returns for null/deleted mobiles before reading interaction state.
- `PowerTarget.OnTarget(Mobile from, object targeted)` now treats null/deleted/out-of-backpack source crystal state as the existing backpack-use failure.
- Targeted golem porter items are now checked for deleted/out-of-backpack state before charge state is read or mutated.

## Preserved Behavior

- Golem porter target eligibility.
- Charge cap of 100.
- Charge increment behavior, including the `PorterType > 0` one-charge path.
- Existing messages.
- `RevealingAction`.
- Sound `0x652`.
- `InvalidateProperties`.
- Crystal `Delete()` consumption.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/PowerCrystal.cs`: 0.
- Active overlay rows for `Data/Scripts/Items/Misc/PowerCrystal.cs`: 0.
- No gated approval was crossed.

## Verification

- Targeted source scan confirmed the new mobile, source crystal, backpack, and deleted-target guards.
- Targeted source scan confirmed existing charge cap, `PorterType` increment behavior, success effects, and crystal delete path remain present.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.

## Outputs

- `Data/Scripts/Items/Misc/PowerCrystal.cs`
- `docs/codebase-audit/outputs/source-batch-005-target.md`
- `docs/codebase-audit/outputs/source-batch-005-powercrystal-target-guard-repair-closeout.md`
