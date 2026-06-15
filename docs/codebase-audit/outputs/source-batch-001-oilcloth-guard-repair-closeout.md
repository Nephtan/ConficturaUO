# SOURCE-BATCH-001 OilCloth Guard Repair Closeout

Reviewed at: 2026-06-15T16:44:08.0818812-05:00

## Summary

`SOURCE-BATCH-001` implemented the selected non-gated OilCloth guard repair in `Data/Scripts/Items/Misc/OilCloth.cs`.

Behavior changed:

- `OnDoubleClick(Mobile from)` now returns for null/deleted mobiles.
- `OnDoubleClick(Mobile from)` now treats a deleted cloth, missing backpack, or cloth outside the user's backpack as the existing backpack-use failure.
- `OnTarget(Mobile from, object obj)` now returns for null/deleted mobiles.
- `OnTarget(Mobile from, object obj)` now treats a deleted cloth, missing backpack, or cloth outside the user's backpack as the existing backpack-use failure.
- `OnTarget(Mobile from, object obj)` now treats null target objects and deleted targeted items as the existing invalid-target failure.

Preserved behavior:

- poison-charge reduction behavior
- firebomb conversion behavior
- oil cloth consumption semantics
- localized messages and targeting flow
- serialization layout and versioning
- region/map behavior
- economy/reward tuning
- staff/access behavior
- folder, namespace, type, and package layout

## Gate Result

POST-BATCH-Y preflight result: `Data/Scripts/Items/Misc/OilCloth.cs` has zero gate hits.

No gated approval was crossed. Staff/access, economy/reward tuning, region/map behavior, `HouseFoundation` serializer migration, and folder/namespace/package reorganization remain blocked pending explicit approval.

## Source Evidence

Targeted source scan:

- `OilCloth` class: `Data/Scripts/Items/Misc/OilCloth.cs`
- `OnDoubleClick(Mobile from)`: null/deleted mobile guard and deleted/missing/out-of-backpack cloth guard are present.
- `OnTarget(Mobile from, object obj)`: null/deleted mobile guard, deleted/missing/out-of-backpack cloth guard, null-target guard, and deleted targeted-item guard are present.
- Poison cleaning branch still uses the existing `BaseWeapon` logic and messages.
- Firebomb conversion branch still uses the existing `BaseBeverage` liquor conversion, backpack add, location restore, message, and `Consume()` flow.
- `Serialize(GenericWriter writer)` and `Deserialize(GenericReader reader)` remain unchanged by the source diff.

Hook/gump/command scan was not required because this batch only changed existing item interaction overrides and did not add or modify command registration, event hooks, gumps, timers, packet handlers, regions, or startup wiring.

## Verification

| Check | Result |
| --- | --- |
| Initial worktree | Clean before source edits. |
| Applicable instructions | Root `AGENTS.md` and `docs/codebase-audit/AGENTS.md` re-read; no nested `AGENTS.md` applies to `Data/Scripts/Items/Misc/OilCloth.cs`. |
| POST-BATCH-Y preflight | `oilcloth_gate_hits=0`. |
| Targeted source scan | New OilCloth guards found in `OnDoubleClick` and `OnTarget`. |
| Serializer diff scan | No `Serial`, `Serialize`, `Deserialize`, `writer.Write`, or `reader.Read` changes in the OilCloth diff. |
| `git diff --check` | Passed with expected LF-to-CRLF working-copy warnings only. |
| `Server.csproj` Debug/x86 build | Initial sandboxed attempt failed on denied access to `C:\Users\nepht\AppData\Local\Microsoft SDKs`; approved rerun passed with 0 warnings and 0 errors. |
| Runtime script compile | `.\ConficturaServer.exe -compileonly -nocache` passed with `Scripts: Compile-only verification completed successfully.` |
| Build artifacts | Tracked root artifacts `ConficturaServer.exe`, `ConficturaServer.exe.config`, and `ConficturaServer.pdb` were restored before staging. |

## Outputs

- `Data/Scripts/Items/Misc/OilCloth.cs`
- `docs/codebase-audit/outputs/source-batch-001-oilcloth-guard-repair-closeout.md`
- `docs/codebase-audit/outputs/source-batch-controller-roadmap-status.csv`
- `docs/codebase-audit/outputs/source-batch-controller-closeout.md`
- `docs/codebase-audit/outputs/source-batch-intake-register.csv`
- `docs/codebase-audit/PHASE_STATUS.md`
- `docs/codebase-audit/RUN_LOG.md`
- `docs/codebase-audit/outputs/README.md`
