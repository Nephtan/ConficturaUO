# SOURCE-BATCH-002 Target

Reviewed at: 2026-06-15T18:45:22.3593239-05:00

## Target

Source batch: `SOURCE-BATCH-002 OilCloth Dye Scissor Guard Repair`

Behavior to change: add stale/null guards to OilCloth `Dye(Mobile from, DyeTub sender)` and `Scissor(Mobile from, Scissors scissors)` interaction paths.

System: `Items:Misc / OilCloth`

Expected file:

- `Data/Scripts/Items/Misc/OilCloth.cs`

## Must Stay Unchanged

- SOURCE-BATCH-001 `OnDoubleClick` and `OnTarget` behavior
- poison-charge reduction behavior
- firebomb conversion behavior
- oil cloth consumption semantics
- localized messages
- targeting flow
- serialization layout and versioning
- region/map behavior
- economy/reward tuning
- staff/access behavior
- folder, namespace, type, and project layout
- broader `IDyable` and `IScissorable` behavior outside OilCloth

## Fence Result

POST-BATCH-Y gate preflight result: `OilCloth.cs` has zero gate hits.

Active overlay result: no active overlay row currently targets `Data/Scripts/Items/Misc/OilCloth.cs`.

No gated approval is crossed. The target is non-gated under the POST-BATCH-AA roadmap rules.

## Ready Goal Command

```text
/goal SOURCE-BATCH-002 OilCloth Dye Scissor Guard Repair

Implement stale/null guards in OilCloth `Dye` and `Scissor` only.

Allowed source changes:
- `Dye` returns `false` when OilCloth is deleted, `from` is null/deleted, or `sender` is null/deleted.
- `Scissor` returns `false` when OilCloth is deleted, `from` is null/deleted, `scissors` is null/deleted, or `from` cannot see the OilCloth.

Preserve existing successful dye hue assignment and scissor bandage conversion.
Do not change `OnDoubleClick`, `OnTarget`, serialization, project files, layout, or gated behavior.
```

## Gated Decisions

- Staff/access: no approval; remains blocked pending explicit approval.
- Economy/reward tuning: no approval; remains blocked pending explicit approval.
- Region/map behavior: no approval; remains blocked pending explicit approval.
- `HouseFoundation` serializer migration: no approval; remains blocked pending explicit approval.
- Folder/namespace/package reorganization: no approval; remains blocked pending explicit approval.
