# SOURCE-BATCH-006 Target

Reviewed at: 2026-06-16T11:29:34.9621783-05:00

## Target

- Batch: `SOURCE-BATCH-006`
- Candidate: `SB004-CAND-003`
- Behavior: add stale/null/backpack guards to ClockworkAssembly interaction paths.
- System: `Items:Misc / ClockworkAssembly`
- Expected file: `Data/Scripts/Items/Misc/ClockworkAssembly.cs`

## Fence Result

- POST-BATCH-Y gate hits for `Data/Scripts/Items/Misc/ClockworkAssembly.cs`: 0
- Active overlay rows for `Data/Scripts/Items/Misc/ClockworkAssembly.cs`: 0
- Gated approval crossed: No

## Must Stay Unchanged

- Tinkering threshold.
- Follower-slot requirement.
- Scalar calculation.
- Resource list and quantities.
- Consume-order result messages.
- Golem creation/control behavior.
- Assembly deletion behavior.
- Serialization layout/versioning.
- Namespace/type/file layout.
- Project files.
- XML/config/data files.
- Staff/access behavior.
- Economy/reward tuning.
- Region/map behavior.

## Ready Goal Command

```text
/goal SOURCE-BATCH-006 ClockworkAssembly Guard Repair

Implement stale/null/backpack guards in Data/Scripts/Items/Misc/ClockworkAssembly.cs for ClockworkAssembly OnDoubleClick only. Preserve tinkering threshold, follower-slot requirement, scalar calculation, resource list and quantities, consume-order result messages, golem creation/control behavior, assembly deletion behavior, serialization layout/versioning, namespace/type/file layout, project files, XML/config/data files, staff/access behavior, economy/reward tuning, and region/map behavior. Verify POST-BATCH-Y gate hits=0, active overlay rows=0, serializer diff unchanged, forbidden surfaces unchanged, Server.csproj Debug/x86 build, compile-only runtime verification, artifact restoration, and git diff --check.
```
