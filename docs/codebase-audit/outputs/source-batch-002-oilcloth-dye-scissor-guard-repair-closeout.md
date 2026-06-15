# SOURCE-BATCH-002 OilCloth Dye Scissor Guard Repair Closeout

Reviewed at: 2026-06-15T18:45:22.3593239-05:00

## Summary

`SOURCE-BATCH-002` implemented the selected non-gated OilCloth dye/scissor guard repair in `Data/Scripts/Items/Misc/OilCloth.cs`.

Behavior changed:

- `Dye(Mobile from, DyeTub sender)` now returns `false` when the OilCloth is deleted, `from` is null/deleted, or `sender` is null/deleted.
- `Scissor(Mobile from, Scissors scissors)` now returns `false` when the OilCloth is deleted, `from` is null/deleted, `scissors` is null/deleted, or `from` cannot see the OilCloth.

Preserved behavior:

- `Dye` still assigns `Hue = sender.DyedHue` on valid input.
- `Scissor` still calls `base.ScissorHelper(from, new Bandage(), 1)` and returns `true` on valid input.
- SOURCE-BATCH-001 `OnDoubleClick` and `OnTarget` behavior remain unchanged.
- Poison-charge reduction, firebomb conversion, oil cloth consumption semantics, localized messages, targeting flow, serialization, region/map behavior, economy/reward tuning, staff/access behavior, folder/namespace/type layout, and project files remain unchanged.

## Gate Result

POST-BATCH-Y preflight result: `Data/Scripts/Items/Misc/OilCloth.cs` has zero gate hits.

Active overlay result: no active overlay row currently targets `Data/Scripts/Items/Misc/OilCloth.cs`.

No gated approval was crossed. Staff/access, economy/reward tuning, region/map behavior, `HouseFoundation` serializer migration, and folder/namespace/package reorganization remain blocked pending explicit approval.

## Source Evidence

Targeted source scan:

- `OilCloth` class: `Data/Scripts/Items/Misc/OilCloth.cs`
- `Dye(Mobile from, DyeTub sender)`: deleted OilCloth, null/deleted mobile, and null/deleted dye tub guards are present.
- `Dye(Mobile from, DyeTub sender)`: valid success path still assigns `Hue = sender.DyedHue`.
- `Scissor(Mobile from, Scissors scissors)`: deleted OilCloth, null/deleted mobile, null/deleted scissors, and cannot-see guards are present.
- `Scissor(Mobile from, Scissors scissors)`: valid success path still calls `base.ScissorHelper(from, new Bandage(), 1)` and returns `true`.
- `Serialize(GenericWriter writer)` and `Deserialize(GenericReader reader)` remain unchanged by the source diff.

No command, event hook, gump, timer, packet handler, region, startup, project, XML/config/data, or reorganization changes were made.

## Verification

| Check | Result |
| --- | --- |
| Initial worktree | Clean before source edits. |
| Applicable instructions | Root `AGENTS.md` and `docs/codebase-audit/AGENTS.md` re-read; no nested `AGENTS.md` applies to `Data/Scripts/Items/Misc/OilCloth.cs`. |
| POST-BATCH-Y preflight | `oilcloth_gate_hits=0`. |
| Active overlay preflight | `active_overlay_oilcloth_matches=0`. |
| Targeted source scan | New OilCloth `Dye` and `Scissor` guards found; success paths remain present. |
| Serializer diff scan | No `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes in the OilCloth diff. |
| Changed-file scope | Source change limited to `Data/Scripts/Items/Misc/OilCloth.cs`; audit changes limited to source-batch records. |
| `git diff --check` | Passed with expected LF-to-CRLF working-copy warnings only. |
| `Server.csproj` Debug/x86 build | Visual Studio MSBuild passed with 0 warnings and 0 errors. |
| Runtime script compile | `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.` |
| Build artifacts | Tracked root artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging. |

## Outputs

- `Data/Scripts/Items/Misc/OilCloth.cs`
- `docs/codebase-audit/outputs/source-batch-002-target.md`
- `docs/codebase-audit/outputs/source-batch-002-oilcloth-dye-scissor-guard-repair-closeout.md`
- `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`
- `docs/codebase-audit/outputs/source-batch-controller-closeout.md`
- `docs/codebase-audit/outputs/source-batch-intake-register.csv`
- `docs/codebase-audit/PHASE_STATUS.md`
- `docs/codebase-audit/RUN_LOG.md`
- `docs/codebase-audit/outputs/README.md`
