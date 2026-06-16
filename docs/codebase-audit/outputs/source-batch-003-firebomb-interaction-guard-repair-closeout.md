# SOURCE-BATCH-003 Firebomb Interaction Guard Repair Closeout

Reviewed at: 2026-06-15T19:36:50.2500868-05:00

## Summary

`SOURCE-BATCH-003` implemented the selected non-gated Firebomb interaction guard repair in `Data/Scripts/Items/Misc/Firebomb.cs`.

Behavior changed:

- `OnDoubleClick(Mobile from)` now returns before dereferencing interaction state when the Firebomb is deleted, `from` is null/deleted, `from.Backpack` is null, or the Firebomb is no longer in `from.Backpack`.
- `OnFirebombTarget(Mobile from, object obj)` now returns before target processing when the Firebomb is deleted, `from` is null/deleted, `from.Backpack` is null, the Firebomb is no longer in `from.Backpack`, or the Firebomb map is null/internal.

Preserved behavior:

- Valid Firebomb use still sends the existing backpack failure message, harmful-region rejection message, AOS blocked-use message, lit/already-lit localized messages, target range, moving effect, delayed reposition, internalization, explosion timing, damage values, and field behavior.
- Firebomb and FirebombField serialization remain unchanged.
- FirebombField transient no-payload behavior remains unchanged.
- Timer scheduling, timer callback behavior, region/map policy, economy/reward tuning, staff/access behavior, command access, folder/namespace/type layout, project files, and XML/config/data files remain unchanged.

## Gate Result

POST-BATCH-Y preflight result: `Data/Scripts/Items/Misc/Firebomb.cs` has zero gate hits.

Active overlay result: three active overlay rows target `Data/Scripts/Items/Misc/Firebomb.cs`, and all are serializer no-change or intentional-legacy rows:

- `RB-01034`: `SafeNoChange`; FirebombField transient no-payload serialization is approved as intentional.
- `RB-01033`: `IntentionalLegacy`; Firebomb fixed-format legacy serializer must be preserved.
- `RB-01035`: `IntentionalLegacy`; FirebombField no-payload serializer must be preserved.

No gated approval was crossed. Staff/access, command-access, economy/reward tuning, region/map behavior, `HouseFoundation` serializer migration, broader serializer/migration work, project/XML/config/data edits, and reorganization remain blocked pending explicit approval.

## Source Evidence

Targeted source scan:

- `Firebomb` class: `Data/Scripts/Items/Misc/Firebomb.cs`
- `OnDoubleClick(Mobile from)`: deleted Firebomb, null/deleted mobile, missing backpack, and out-of-backpack guards are present before `from.Region`, spell state, timer, or target setup access.
- `OnFirebombTarget(Mobile from, object obj)`: deleted Firebomb, null/deleted mobile, missing backpack, out-of-backpack, null map, and internal map guards are present before target processing, `from.RevealingAction()`, moving effect, delayed reposition, or `Internalize()`.
- `Serialize(GenericWriter writer)` and `Deserialize(GenericReader reader)` remain unchanged by the source diff.
- Existing `Timer.DelayCall` sites and timer callback methods remain unchanged by the source diff.

No command, event hook, gump, packet handler, region, startup, project, XML/config/data, or reorganization changes were made.

## Verification

| Check | Result |
| --- | --- |
| Initial worktree | Clean before source edits. |
| Applicable instructions | Root `AGENTS.md` and `docs/codebase-audit/AGENTS.md` re-read; no nested `AGENTS.md` applies to `Data/Scripts/Items/Misc/Firebomb.cs`. |
| POST-BATCH-Y preflight | `firebomb_gate_hits=0`. |
| Active overlay preflight | `active_overlay_firebomb_matches=3`; all matches are `SafeNoChange` or `IntentionalLegacy` serializer rows. |
| Targeted source scan | New Firebomb `OnDoubleClick` and `OnFirebombTarget` guards found; valid interaction paths remain present. |
| Serializer diff scan | `serializer_diff_hits=0`; no `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes in the Firebomb diff. |
| Forbidden-surface diff scan | `forbidden_surface_diff_hits=0`; no timer, hook, command, gump, packet, region, startup, project, XML/config/data, or reorganization diff hits. |
| Changed-file scope | Source change limited to `Data/Scripts/Items/Misc/Firebomb.cs`; audit changes limited to source-batch records. |
| `git diff --check` | Passed with expected LF-to-CRLF working-copy warning only. |
| `Server.csproj` Debug/x86 build | Initial sandboxed run failed on denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`; approved rerun passed with 0 warnings and 0 errors. |
| Runtime script compile | `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.` |
| Build artifacts | Tracked root artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging. |

## Outputs

- `Data/Scripts/Items/Misc/Firebomb.cs`
- `docs/codebase-audit/outputs/source-batch-003-target.md`
- `docs/codebase-audit/outputs/source-batch-003-firebomb-interaction-guard-repair-closeout.md`
- `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`
- `docs/codebase-audit/outputs/source-batch-controller-closeout.md`
- `docs/codebase-audit/outputs/source-batch-intake-register.csv`
- `docs/codebase-audit/PHASE_STATUS.md`
- `docs/codebase-audit/RUN_LOG.md`
- `docs/codebase-audit/outputs/README.md`

Commit hash: pending until the batch commit is created; final hash must be recorded by the source-batch runner closeout update.
