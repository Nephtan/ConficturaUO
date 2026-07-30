# SOURCE-BATCH-004 ArcaneGem Interaction Guard Repair Closeout

Reviewed at: 2026-06-16T11:15:37.0000000-05:00

## Summary

`SOURCE-BATCH-004` implemented the `SB004-CAND-001` ArcaneGem interaction guard repair in `Data/Scripts/Items/Misc/ArcaneGem.cs`.

The batch added stale/null/backpack guards before ArcaneGem interaction paths dereference mobile, backpack, or target item state. No source files outside ArcaneGem were edited.

## Source Changes

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles before reading `from.Backpack`.
- `OnDoubleClick(Mobile from)` now treats deleted ArcaneGem state, missing backpacks, or gems outside the backpack as the existing backpack-use failure.
- `OnTarget(Mobile from, object obj)` now returns for null/deleted mobiles before reading interaction state.
- `OnTarget(Mobile from, object obj)` now treats deleted ArcaneGem state, missing backpacks, or gems outside the backpack as the existing backpack-use failure.
- Targeted arcane items are now checked for deleted/out-of-backpack state before resource, loot-type, skill, charge, hue, or consumption behavior is evaluated.

## Preserved Behavior

- Arcane charge math and `GetChargesFor`.
- `DefaultArcaneHue`.
- Tailoring thresholds.
- Item eligibility and exceptional-item checks.
- Leather resource restrictions.
- Blessed-item rejection.
- Existing messages.
- Gem `Amount` decrement and `Delete()` semantics.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior.

## Gate Evidence

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/ArcaneGem.cs`: 0.
- Active overlay rows for `Data/Scripts/Items/Misc/ArcaneGem.cs`: 0.
- No gated approval was crossed.

## Verification

- Targeted source scan confirmed the new mobile, backpack, and deleted-target guards.
- Targeted source scan confirmed existing target assignment, charge math, and gem amount/delete paths remain present.
- Serializer diff scan found no changed `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` lines.
- Forbidden-surface diff scan found no command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes.
- Initial unqualified `msbuild` command was unavailable on PATH.
- Visual Studio MSBuild at `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe` initially failed in the sandbox with denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`.
- Escalated `Data/System/Source/Server.csproj` Debug/x86 build passed with 0 warnings and 0 errors.
- `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.`
- `git diff --check` passed with expected LF-to-CRLF working-copy warnings only.
- Generated tracked root build artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging.

## Outputs

- `Data/Scripts/Items/Misc/ArcaneGem.cs`
- `docs/codebase-audit/outputs/source-batch-004-target.md`
- `docs/codebase-audit/outputs/source-batch-004-arcanegem-interaction-guard-repair-closeout.md`
